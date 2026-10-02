# CCTO Coordination

This file is the shared coordination surface between separate ChatGPT chats/workstreams working on CCTO.

Use this instead of asking the user to manually relay messages between chats.

## Working rule

At the start of CCTO work:

1. Read this file.
2. Check whether there are open items owned by the current workstream.
3. Perform the relevant work without asking the user to relay the request again.
4. Update this file with the result, status, and commit/reference when the item is resolved or materially changed.

For durable specifications and decided values, update the appropriate design/framework document as well. This file is for coordination and handoff, not the final source of truth for design.

## Workstreams

- **Balance/XML**
  - crop-specific `minGrowthTemperature`
  - fixed `coldDeathTemperature`
  - Vanilla / Medieval Overhaul balance XML
  - AMJ crop values and compatibility XML

- **Code/framework**
  - C# framework and Harmony patches
  - dormancy behavior
  - Info Card
  - validation
  - automated tests
  - build/test tooling

## Status vocabulary

- **OPEN** — needs work
- **IN PROGRESS** — currently being investigated/implemented
- **BLOCKED** — waiting on a specific prerequisite
- **DONE** — completed and reflected in the proper source-of-truth document/code

## Current coordination items

### CODE-001 — Fixed cold-death boundary

**Requested by:** Balance/XML  
**Owner:** Code/framework  
**Status:** IN PROGRESS

Confirm that a configured fixed death threshold uses a strict below-threshold rule:

- exactly at the configured threshold: survive;
- below the configured threshold: die.

Current implementation uses:

`temperature < coldDeathTemperature`

An E2E boundary scenario exists. The first runtime attempt exposed a test-harness temperature synchronization issue rather than a CCTO logic failure. The harness was fixed so the plant's actual `AmbientTemperature` is synchronized before forcing the cold check.

**Next action:** rerun the Pickle suite and require the boundary scenario to pass.

### CODE-002 — Recovery from cold dormancy

**Requested by:** Balance/XML  
**Owner:** Code/framework  
**Status:** OPEN

Confirm intended recovery behavior after a dormant plant warms above `minGrowthTemperature`.

Current implementation refreshes RimWorld's private `madeLeaflessTick` while the plant is cold. Once temperature rises, CCTO stops refreshing it. Vanilla `LeaflessNow` remains true until 60,000 ticks have elapsed since the last refresh.

Potential issue:

- the plant can resume temperature-based growth eligibility before the visible leafless state clears;
- visual/state recovery can therefore lag warming by up to about one in-game day.

**Decision required:** decide whether CCTO should:
- preserve vanilla delayed leafless recovery; or
- explicitly clear cold dormancy when the plant warms.

Do not turn either behavior into a regression contract until this is decided.

### CODE-003 — Indoor cold behavior / `CheckMakeLeafless` postfix

**Requested by:** Balance/XML  
**Owner:** Code/framework  
**Status:** OPEN

Confirm whether CCTO should intentionally bypass Vanilla's:

`room.UsesOutdoorTemperature`

gate for configured plants.

Current implementation's postfix does bypass that gate, but uses the plant's actual:

`plant.AmbientTemperature`

rather than outdoor temperature directly.

Therefore current behavior is:

- heated indoor greenhouse, cold outdoors -> plant survives if its room/ambient temperature is safe;
- genuinely cold indoor room -> configured plant can enter dormancy or die even though the room is not using outdoor temperature.

This appears consistent with CCTO's cold-tolerance model, but it needs an explicit design decision and an E2E regression test before beta.

**Next action:** confirm intended semantics, then add indoor safe/indoor cold E2E coverage.

## Completed handoffs

None yet.
