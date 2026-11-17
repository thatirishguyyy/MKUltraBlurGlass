# Return border geometry by value

`CTopLevelWindow::GetBorderGeometry()` previously returned `CRgnGeometryProxy* const&` while its result came from a local pointer variable named `geometry`. The reference became invalid when the function returned. Copying through that reference in a caller is undefined behavior, even though the pointed-to geometry object may still exist.

The declaration and definition now return `CRgnGeometryProxy*` by value. The existing offsets, Windows build thresholds, null checks, and ownership are unchanged. Existing callers already copy the returned pointer, so no caller changes are needed.

This fix does not change `GetTitlebarGeometry()`, adopt a newer geometry layout, or include other pending upstream changes. It does not establish the cause of any reported user crash.

## Focused regression checks

From the repository root, with Python 3 and VS2022 Community C++ tools installed:

```powershell
python tests/border-geometry/run-tests.py .
```

The harness extracts the actual getter body and declaration from the current source. It checks the pointer return type and exercises explicit simulated layout fixtures at 13 build-number boundaries, including null geometry and missing visual cases. It does not load or inject into DWM. The original implementation from release commit `828f09503193aaadf2a00edcbcbadbf21b39e057` is a negative control: it must fail when MSVC warning C4172 is treated as an error.

The corrected getter passes 46 checks in both unoptimized and optimized builds. These fixtures check lookup and return behavior; they do not validate real DWM layouts on every Windows build. Full extension build results are recorded separately. Runtime validation on the owner's Windows installation remains pending.

## Full build result

The x64 extension rebuilt successfully. C4172 is absent. An initial LNK1103 debug-information error was resolved by recompiling the named object and clearing generated linker state; the successful retry had zero errors. The unrelated Notifier.cpp numeric-conversion warning from the full compile remains outside this fix. Exported DLL names match the installed release, and the GUI binary still matches the local baseline. No desktop runtime test or installation was performed.
