# Development Tools and Local Agent Workflow

This document records the development helpers used around CCTO/AMJ and when to use local Work/Codex.

The goal is to avoid leaving every development mod enabled, avoid unnecessary agent quota consumption, and make tool selection repeatable across chats.

## 1. RimWorld development helpers

### Confirmed installed / already present in the user's environment

| Tool | Status | Main use | Enablement policy |
|---|---|---|---|
| RimDoctor | Installed | startup/log diagnosis, Harmony/patch diagnostics | Enable when diagnosing startup/load/runtime problems; can remain off otherwise |
| Prepatcher | Installed | startup-time assembly patching required by mods that depend on it | Keep enabled when required by the active mod list; not a CCTO test tool by itself |
| HugsLib | Installed | shared library/controller and diagnostic logging for mods that depend on it | Keep enabled when required by active mods |
| Gagarin | Installed | startup/cache handling and load-time support | Keep according to the normal mod setup; use its cache functions when startup/Def cache issues are suspected |

### Development/test helpers to enable only for the task that needs them

These are useful for CCTO/AMJ, but should not be treated as normal-play requirements.

| Tool | Installation status | Best use |
|---|---|---|
| RimTest Redux | Verify locally before use | C# logic, XML/Def assumptions, Harmony regression tests, repeatable automated checks |
| Pickle | Verify locally before use | in-game/E2E tests: map state, temperature changes, ticks, death, dormancy, recovery |
| Things Explorer | Verify locally before use | inspect loaded Defs and resolved values |
| XML Patch Helper | Verify locally before use | inspect XPath/patch results and confirm the final Def state |
| Better Stacktraces | Verify locally before use | improve exception stack traces during debugging |
| Quickstarts | Verify locally before use | rapidly enter a reproducible test state |
| YADA | Verify locally before use | developer/publishing support where relevant |
| RimLogging | Verify locally before use | logging instrumentation when ordinary logs are insufficient |
| XML Extensions | Verify locally before use | only when a patch actually needs its extra XML operations or another dependency requires it |

Do not assume the tools in the second table are installed merely because they were evaluated. Confirm them in the local mod list before relying on them.

## 2. Enablement rule

Development helpers should normally be **off** and enabled for the specific task.

Examples:

- C# regression test -> enable RimTest Redux.
- Temperature/death/dormancy scenario -> enable Pickle.
- Resolved Def/XPath check -> enable Things Explorer and/or XML Patch Helper.
- Exception investigation -> enable Better Stacktraces and RimDoctor.
- Normal gameplay compatibility check -> disable development-only helpers unless another active mod requires them.

This reduces startup overhead, avoids adding extra compatibility variables, and makes it easier to tell whether a problem belongs to CCTO/AMJ or to a development tool.

## 3. Default agent escalation order

Use the least expensive workflow that can actually complete the task:

1. **Normal Chat / GPT-5.6 Sol** for specification, reasoning, research, review, and small self-contained code/XML.
2. **Local Work/Codex with Sol** when direct access to the local repository, installed mods, DLLs, build scripts, logs, or repeated file edits is materially useful.
3. **Local Work/Codex with Astra** only for tasks whose complexity or uncertainty justifies the limited Astra allowance.

Do not use Local Work/Codex just because a task involves code. If the work can be completed accurately from pasted files, GitHub, or a small isolated snippet, normal Chat should be preferred.

## 4. Local Work/Codex: Sol is normally sufficient

Use **Sol** for local tasks where the target files and desired behavior are already reasonably understood.

Typical CCTO/AMJ examples:

- inspect known Vanilla/MO XML Def files;
- inspect a known local mod folder for exact Def names or inheritance;
- create or update XML patches across a small or medium known file set;
- implement ordinary C# classes, DefModExtensions, Harmony Prefix/Postfix patches, settings, translations, or small utilities;
- make targeted edits across several known source files;
- run `build.bat`, compile, inspect compiler output, patch errors, and rerun;
- inspect Player.log or RimDoctor output when the failing subsystem is already localized;
- add or update RimTest Redux tests after the expected behavior is defined;
- add straightforward Pickle scenarios after the test setup is understood;
- update README/Description/Framework docs and English/Japanese translations;
- compare CCTO XML against already-decided balance values;
- perform repository housekeeping, file moves, naming cleanup, and small refactors;
- check one or a few compatibility mods whose relevant files are known.

For CCTO specifically, the current framework implementation, ordinary compile-fix loop, XML API work, and most automated-test authoring should remain in **Sol** unless a genuinely difficult failure appears.

## 5. Local Work/Codex: use Astra for high-complexity work

Reserve **Astra** for tasks where broad codebase reasoning, difficult debugging, or a large unknown search space is the bottleneck.

Typical cases:

- trace behavior across many installed mods when the conflicting mod or patch is not known;
- audit a large codebase or many mod folders to discover where a behavior is actually implemented;
- analyze a DLL/decompiled implementation when source is unavailable and the relevant execution path is not known;
- diagnose a persistent Harmony conflict involving several patches and unclear ordering;
- write or repair a complex Transpiler/IL patch;
- investigate save-compatibility or migration problems that span Def changes, serialized data, and runtime code;
- perform a major multi-file architecture refactor with many invariants and compatibility constraints;
- debug an intermittent or state-dependent failure that survives focused Sol attempts;
- reconcile several frameworks/mod APIs whose interactions are poorly documented;
- repeatedly run build/test/log cycles where the failure keeps moving between subsystems and cannot be localized.

Astra should not be selected merely because a task touches many files. If the files and transformation are straightforward, Sol is still the default.

## 6. Escalation rule from Sol to Astra

Escalate to Astra when at least one of these is true:

- the root cause remains unclear after one or two focused Sol investigation passes;
- the task requires tracing an unfamiliar execution path across many assemblies/mods;
- a Harmony/IL problem cannot be solved safely with ordinary Prefix/Postfix techniques;
- the requested change is a large architectural rewrite rather than a targeted implementation;
- repeated automated tests expose interacting failures that cannot be localized.

Before escalating, provide Astra with a narrow handoff: current task, relevant files, known facts, latest error/test output, and decisions that must not be changed.

Do not give Astra the entire project history unless it is necessary.

## 7. Work vs Codex

Prefer **Codex** when the main job is repository work:

- edit source/XML;
- run build scripts;
- run tests;
- inspect diffs;
- fix compiler/runtime errors.

Prefer **local Work** when the task is broader than one repository and needs to inspect or correlate several local resources, such as:

- multiple installed RimWorld mod directories;
- local DLLs plus XML plus logs;
- reference documentation and project notes alongside source files.

For mixed tasks, use the tool that needs the fewest unnecessary files and the smallest context.

## 8. CCTO test workflow

Recommended sequence:

1. Compile the framework.
2. Run RimTest Redux regression tests for extension parsing and Harmony logic.
3. Run Pickle E2E tests for actual temperature/tick behavior.
4. Inspect final Def values with Things Explorer/XML Patch Helper when XML balance data is added.
5. Run a normal-game smoke test with development-only helpers disabled.
6. Only use Astra if failures remain difficult to localize after the above steps.

Balance values remain owned by the separate balance/XML workflow; code-side tools must not silently redefine decided values.
