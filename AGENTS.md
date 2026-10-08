# CCTO Agent Instructions

## Start here

1. Read this file and `main:Docs/Coordination.md`; locate the latest relevant owner/status/evidence, including later corrections. Historical entries are not current approval.
2. Read Project [AGENTS.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/AGENTS.md) and [Docs/SharedRules.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/SharedRules.md): apply its stop conditions, then open only the task-relevant canonical procedures.
3. Read the local specification and affected source/tests below. Shared rules are owned by Project; this file owns only local scope and routing. Missing access or conflicting authority blocks the dependent action, not unrelated safe work.

New features cannot enter implementation before the Project [existing-Mod audit gate](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/ExistingModAudit.md#implementation-entry-gate) covers VE and non-VE alternatives and records why independent implementation is needed. Existing approved behavior is not redesigned by this rule audit.

## Local task routes and stops

- CCTO remains a generic cold-tolerance framework and owns its own Vanilla/MO temperature support, not AMJ-specific plant design or historical presentation.
- AMJ historical-description policy applies only to explicitly requested AMJ-facing prose. Do not rewrite descriptions merely because a plant is supported.
- CCTO keeps its own preview identity; the AMJ cover pipeline applies only to an explicitly requested AMJ-series cover. AMJ era/branding rules do not impose an AMJ world configuration on standalone CCTO.
- Release: `Docs/ReleaseChecklist.md`; public copy: `Docs/WorkshopDescription.md` and `Docs/2GamePresentation.md`; developer-only tools: `Docs/DevelopmentTools.md`.
- `Tests/validate_workshop_description.py`, `Tests/validate_workshop_payload.py`, and `Tests/validate_add_changenote.py` check publication source/metadata/payload contracts. They do not prove actual upload or Add Changenote hook execution.

## Framework-consumer data ownership

AMJC and AMJE own their custom plant values, balance/design tables (including archived candidate ranges), Def mappings, and optional CCTO compatibility XML. Do not add those plants to CCTO's data or supported/planned content sets. CCTO provides the reusable API; changes to consumer-owned plant data belong in the owning repository.
