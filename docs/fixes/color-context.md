# Backport upstream window-color context fix

This backports Maplespe's commit `74945a1656944c5a99864e5000b2e2323d75ffdc` from March 13, 2025. The changed source matches upstream after normalizing checkout line endings.

- Upstream commit: https://github.com/Maplespe/DWMBlurGlass/commit/74945a1656944c5a99864e5000b2e2323d75ffdc
- Associated crash report: https://github.com/Maplespe/DWMBlurGlass/issues/498

## Behavior

The original 2.3.1 code left `g_windowData` pointing to the last validated window's data after validation finished. It also applied custom colorization when no window context was present, defaulting to light-mode colors.

The backport clears that temporary data pointer at validation entry and on normal completion. Both Windows 10 and Windows 11 color hooks still call the original Windows function first, then preserve its result if no window-data context exists. When valid context exists, configured active/inactive, light/dark, accent-color, and existing alpha behavior are preserved.

This is the upstream fix without a UI rewrite, geometry-offset changes, or other feature additions. It does not assert exception-safety or thread-safety beyond what this upstream patch implements.

## Verification

Run from the source checkout with Python and VS2022 Community C++ tools installed:

```powershell
python tests/color-context/run-tests.py .
```

The harness extracts the actual three changed production functions and substitutes isolated Windows/DWM collaborators. The original release reproduces unscoped color overrides and stale context on early exits and completion. The corrected code passes 228 checks in both unoptimized and optimized builds. Checks cover original function calls and arguments, return values, configured colors, alpha behavior, allowed/disallowed windows, absent/invisible/minimized window data, successful/failed validation, backdrop updates, and accent-policy restoration.

The extension rebuilt successfully. It remains x64, exported names match the installed release, and the GUI binary matches the local baseline. The previous geometry fixes remain included. Installed files were verified unchanged by SHA-256. No live desktop runtime test has been performed.

## Patches

`color-context-fix.patch` is the standalone upstream backport. `maintenance-fixes.patch` includes this backport plus both previous geometry fixes and applies to the original 2.3.1 release source at `828f09503193aaadf2a00edcbcbadbf21b39e057`. Build-setting changes are separate from these source patches. Tests and notes are supplied alongside the patches.
