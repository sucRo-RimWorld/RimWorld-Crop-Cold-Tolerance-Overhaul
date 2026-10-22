#!/usr/bin/env python3
"""Independent, *offline* CCTO numerical candidate review, not a biology PASS.

Deliberately separate schema/math invariants from biological/ecological
acceptance. The failed V1 numbers remain reproducible but NOT deployable.
No RimWorld process, XML mutation or external access.
"""
import argparse
import csv
import json
import math
from pathlib import Path
import sys

ROOT = Path(__file__).resolve().parents[1]
TABLE = ROOT / "Docs/ColdDamageBalanceV1.csv"
REVIEW = ROOT / "Docs/ColdDamageBalanceReview.json"
PATCHES = [
    ROOT / "Patches/Vanilla_ColdTolerance.xml",
    ROOT / "Patches/MedievalOverhaul_ColdTolerance.xml",
]


def load():
    with TABLE.open(encoding="utf-8", newline="") as src:
        rows = list(csv.DictReader(line for line in src if not line.startswith("#")))
    with REVIEW.open(encoding="utf-8") as src:
        review = json.load(src)
    if review.get("balance_table") != "Docs/ColdDamageBalanceV1.csv":
        raise ValueError("Numerical review does not name the active table")
    ids = [r["defName"] for r in rows]
    if len(rows) != 78 or len(set(ids)) != 78:
        raise ValueError("Balance table must have exactly 78 unique plant Defs")
    if sum(r["current_mode"] == "death" for r in rows) != 48:
        raise ValueError("Current 48 death Defs have changed")
    if sum(r["current_mode"] == "dormancy" for r in rows) != 30:
        raise ValueError("Current 30 dormant Defs have changed")
    for r in rows:
        if r["current_mode"] == "dormancy":
            if any(r[k] for k in ("damage_onset_C",
                   "median_lethal_degree_game_hours",
                   "individual_dose_uniform_halfwidth",
                   "warm_recovery_damage_fraction_per_game_hour")):
                raise ValueError("Dormant-only Def received cold injury: " + r["defName"])
            continue
        vals = [float(r[k]) for k in (
            "damage_onset_C",
            "median_lethal_degree_game_hours",
            "individual_dose_uniform_halfwidth",
            "warm_recovery_damage_fraction_per_game_hour",
            "legacy_instant_death_C")]
        A, D, w, recovery, old = vals
        if not all(map(math.isfinite, vals)) or D <= 0 or not 0 <= w < 1 or recovery < 0:
            raise ValueError("Malformed candidate numbers: " + r["defName"])
        if A <= old:
            raise ValueError("V1 proposed reference anchor is not below onset: " + r["defName"])
    return {r["defName"]: r for r in rows}, review


def uniform_survival(row, temperature, hours):
    """Idealized cohort, uninterrupted exposure, 0 injury, no sensor gaps."""
    A = float(row["damage_onset_C"])
    D = float(row["median_lethal_degree_game_hours"])
    w = float(row["individual_dose_uniform_halfwidth"])
    normalized = max(0, A - temperature) * hours / D
    if w == 0:
        return 1.0 if normalized < 1 else 0.0
    return max(0.0, min(1.0, (1 + w - normalized) / (2 * w)))


def daily_survival(row, daily, days, population=2001):
    """Idealized piecewise-constant temperature schedule, not live game weather."""
    A = float(row["damage_onset_C"])
    D = float(row["median_lethal_degree_game_hours"])
    w = float(row["individual_dose_uniform_halfwidth"])
    recovery = float(row["warm_recovery_damage_fraction_per_game_hour"])
    survivors = 0
    for j in range(population):
        fatal = D * (1 + w * (2 * (j + .5) / population - 1))
        damage = 0.0
        for _ in range(days):
            for temperature, hours in daily:
                if temperature < A:
                    damage += (A - temperature) * hours / fatal
                else:
                    damage = max(0.0, damage - recovery * hours)
                if damage >= 1:
                    break
            if damage >= 1:
                break
        else:
            survivors += 1
    return round(survivors / population, 4)


def independent_cases(rows):
    wheat = rows["DankPyon_Plant_Wheat"]
    rice = rows["Plant_Rice"]
    corn = rows["Plant_Corn"]
    pine = rows["Plant_TreePine"]
    # Measured W13 values are for named genotypes and specific conditioning.
    # They are comparison anchors only, NEVER generic wheat release tolerances.
    return [
        {"id": "W13_UNACCLIMATED_M39",
         "observed_24h_survival": .35,
         "observed_source": "W13, M39 22C control, -3C x24h",
         "v1_predicted": round(uniform_survival(wheat, -3, 24), 4),
         "condition_mismatch": True},
        {"id": "W13_ACCLIMATED_M39",
         "observed_24h_survival": .65,
         "observed_source": "W13, M39 4C x7d prior, -5C x24h",
         "v1_predicted": round(uniform_survival(wheat, -5, 24), 4),
         "condition_mismatch": True},
        {"id": "R02_RICE_CONDITIONED",
         "observed_survival_range": [0.88, 1.00],
         "observed_source": "R02 rice 12C x5d preconditioning, 4C x10 days",
         "v1_predicted": round(uniform_survival(rice, 4, 240), 4),
         "condition_mismatch": True},
        {"id": "CORN_ONSET_SENSITIVITY",
         "T_minus_1_9_median_hours": round(
             float(corn["median_lethal_degree_game_hours"]) /
             (float(corn["damage_onset_C"]) - (-1.9)), 2),
         "T_minus_2_0_median_hours": round(
             float(corn["median_lethal_degree_game_hours"]) /
             (float(corn["damage_onset_C"]) - (-2.0)), 2)},
        {"id": "WHEAT_REPEATED_NIGHTS",
         "assumed_schedule": "-5C x8h, +1C x16h per day, five days",
         "v1_survival_day5": daily_survival(wheat, [(-5, 8), (1, 16)], 5)},
        {"id": "EVERGREEN_SUBZERO_RECOVERY",
         "temperature_C": -30,
         "onset_C": float(pine["damage_onset_C"]),
         "recovery_per_hour": float(pine["warm_recovery_damage_fraction_per_game_hour"]),
         "physiological_repair_supported": False},
    ]


def main():
    arg = argparse.ArgumentParser(description=__doc__)
    arg.add_argument("--require-xml-ready", action="store_true",
                     help="Fail until independently reviewed and all blockers closed")
    opts = arg.parse_args()
    rows, review = load()
    cases = independent_cases(rows)
    status = review.get("status")
    blockers = [x["id"] for x in review.get("reasons", [])
                if x.get("state") != "resolved"]
    if status not in ("rejected_for_xml", "under_review", "accepted_for_xml"):
        raise ValueError("Unrecognized design review state")
    enabled_xml = [str(p.relative_to(ROOT)) for p in PATCHES
                   if p.is_file() and ("<coldDamageStartTemperature>" in
                                      p.read_text(encoding="utf-8"))]
    ready = status == "accepted_for_xml" and not blockers
    print(json.dumps({"arithmetic_and_schema": "CONSISTENT_ONLY_NOT_APPROVED",
         "table_row_count": len(rows),
         "review_status": status,
         "unresolved_blockers": blockers,
         "xml_activated": enabled_xml,
         "independent_scenarios": cases,
         "ready_for_xml": ready}, ensure_ascii=False, indent=2))
    if enabled_xml and not ready:
        print("ERROR: unreviewed cold-injury XML activation is forbidden", file=sys.stderr)
        return 2
    if opts.require_xml_ready and not ready:
        print("BLOCKED: arithmetic agreement is not gameplay validation", file=sys.stderr)
        return 2
    return 0


if __name__ == "__main__":
    sys.exit(main())
