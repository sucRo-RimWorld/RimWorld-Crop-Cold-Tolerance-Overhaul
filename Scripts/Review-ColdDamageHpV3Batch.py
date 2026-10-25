"""Generate an idealized V3 exposure matrix; never edits candidates or game files.

Run from any directory with Python 3. No dependencies, game launch or network.
Fractions intentionally exclude integer HP rounding and DamageInfo processing.
"""
import csv
import hashlib
import math
from collections import Counter
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
source = ROOT / 'Docs/ColdDamageHpV3Candidates.csv'
rows = list(csv.DictReader(x for x in source.read_text(encoding='utf-8').splitlines()
                           if not x.startswith('#')))
if len(rows) != 78 or len({r['defName'] for r in rows}) != 78:
    raise ValueError('Expected 78 unique supported Defs')
hp = [r for r in rows if r['mode'] == 'hp_damage']
if len(hp) != 46 or sum(r['mode'] == 'dormancy' for r in rows) != 32:
    raise ValueError('Final V3 design is 46 HP + 32 dormancy (two Healroots moved)')
for row in rows:
    if row['mode'] == 'hp_damage':
        a, d, w = (float(row[k]) for k in (
            'hp_injury_onset_C', 'full_hp_degree_game_hours', 'lethal_dose_variation'))
        if not all(math.isfinite(x) for x in (a, d, w)) or d <= 0 or w != 0:
            raise ValueError('Final HP profiles require finite A, positive D, w=0: ' + row['defName'])
        if row['review_status'] != 'FINALIZED_DESIGN_VALUES':
            raise ValueError('Unfinalized HP profile: ' + row['defName'])
    elif any(row[k] for k in ('hp_injury_onset_C', 'full_hp_degree_game_hours', 'lethal_dose_variation')):
        raise ValueError('Dormancy profile must not carry HP parameters: ' + row['defName'])
out = ROOT / 'Docs/Research/ColdDamageV3BatchExposure.csv'
fields = ['defName', 'review_status', 'A_C', 'D_C_game_hours', 'temperature_C',
          'episode_hours', 'episodes', 'billable_hours', 'w',
          'normalized_HP_remaining', 'conditional_yield_factor', 'field_yield_factor']
with out.open('w', encoding='utf-8', newline='') as stream:
    writer = csv.DictWriter(stream, fieldnames=fields)
    writer.writeheader()
    for row in hp:
        a, d = float(row['hp_injury_onset_C']), float(row['full_hp_degree_game_hours'])
        for temperature in (5, 1, 0, -3, -5, -10, -20, -30, -40, -45):
            for hours, count in ((8, 1), (8, 3), (24, 1)):
                # Explicit aligned sampling assumption: first reading at 0,
                # then 2000, 4000,... strictly before the warm endpoint.
                # 8h -> 7.2h; 24h -> 23.2h. Every warm-separated episode
                # omits its first cold interval; fractional injury persists.
                billed = (hours - .8) * count
                fraction = max(0., 1 - max(0., a - temperature) * billed / d)
                alive = fraction > 0
                conditional = .5 + .5 * fraction if alive else ''
                writer.writerow(dict(zip(fields, (
                    row['defName'], row['review_status'], a, d, temperature,
                    hours, count, round(billed, 4), 0, round(fraction, 6),
                    round(conditional, 6) if alive else '',
                    round(conditional, 6) if alive else 0))))
# Normalize checkout line endings so Windows CRLF and repository LF agree.
print('Source SHA256 (UTF-8/LF):', hashlib.sha256(source.read_text(encoding='utf-8').encode('utf-8')).hexdigest())
print('Classification:', dict(Counter(r['review_status'] for r in rows)))
print('Wrote', len(hp) * 30, 'idealized rows:', out)
