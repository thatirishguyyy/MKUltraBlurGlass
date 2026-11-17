# Safely handle missing title-bar geometry

`CTopLevelWindow::GetTitlebarGeometry()` initialized its geometry-slot pointer to null. On Windows build paths using a legacy background visual, a missing visual left that pointer null. The function nevertheless returned `*geometry` by reference, allowing an invalid memory access when the caller copied the result.

The declaration and definition now return `CRgnGeometryProxy*` by value, and the return expression checks the geometry-slot pointer before dereferencing it. Missing visuals now produce a null result. The existing geometry offsets and Windows build thresholds remain unchanged, including the title-bar threshold at build 26020. No caller changes or pointer-ownership changes are required.

This addresses a concrete invalid-access path. It does not establish why any particular installed application crashed, or validate these internal layouts on all Windows releases.

## Verification

Run the focused regression checks from this repository using Python and VS2022 Community C++ tools:

```powershell
python tests/titlebar-geometry/run-tests.py .
python tests/border-geometry/run-tests.py .
```

The title-bar harness extracts the actual getter declaration and implementation. The original release implementation reproduces a null access in four simulated layouts, captured through structured exception handling in the isolated harness. The fixed implementation passes 46 checks across 13 explicit build-number fixtures in both unoptimized and optimized builds. Null geometry slots, absent legacy visuals, expected pointer values, and copied values are covered. No DWM process is loaded or modified by these tests.

The earlier border-geometry fix remains included. `titlebar-geometry-fix.patch` applies after that fix; `geometry-fixes.patch` contains both source fixes and applies to the original 2.3.1 release source. Build-setting changes are separate from these source patches.

Runtime testing on the owner's desktop remains pending. No installed files, settings, symbols, or scheduled task are changed by this work.

## Full build result

A clean x64 extension rebuild completed successfully with zero errors. The existing Notifier.cpp C4244 conversion warning remains unchanged. The border dangling-reference warning is absent. DLL exports match the installed release; the GUI binary matches the local baseline. Installed binaries were verified unchanged by SHA-256. Both the combined patch and the separate patches reproduce the corrected source exactly from the pinned release.
