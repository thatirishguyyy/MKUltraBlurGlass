# MKUltraBlurGlass Uninstaller 0.1.0

A small English Windows utility for MKUltraBlurGlass and the legacy DWMBlurGlass 2.3.1 host/task layout, with a blue interface and embedded menu-click sound. It is separate from the application's GUI and does not require MiaoUI or the beta's .NET 9 runtime. It uses the .NET Framework already available on this build machine.

## Use

1. Open `MKUltraBlurGlassUninstall.exe`.
2. Review the detected program path. If no startup task is found, choose the extracted program folder.
3. Use **Run as administrator** if needed, then review the location again.
4. Choose **Uninstall** and confirm. By default all application files and settings are kept.
5. Optionally select **Also delete the four application binaries**. This removes only the selected product's EXE and extension DLL, plus `dbghelp.dll` and `symsrv.dll` from the verified program folder. The complete `data` folder and unrelated files remain.

The utility recognizes the root task `MKUltraBlurGlass_Extend` targeting `MKUltraBlurGlass.exe runhost`, or legacy `DWMBlurGlass_Extend` targeting `DWMBlurGlass.exe runhost`, each with exactly one executable action. The MKUltraBlurGlass task is preferred if both are present, and operations remain scoped to the selected task and executable. An unexpected task layout is rejected. A registered task's location takes precedence; folder selection is disabled while that task is detected. Task state/path is rechecked before changes. Unknown task access errors are reported rather than interpreted as an absent task.

The startup task is disabled while the selected copy is stopped. Matching host windows receive the same shutdown/unload message as the original GUI; only the corresponding DWMBlurGlass host process is then terminated. The task is removed after stopping completes. The utility never terminates `dwm.exe`. If stopping fails, startup is restored when possible and file deletion is skipped. Signing out or restarting may be needed if effects remain.

File cleanup rejects shared root directories, Windows system folders, network paths, and junction/symlink traversal. It never deletes folders recursively or deletes the uninstaller itself. Locked binaries are reported; the EXE is kept when another binary is locked so cleanup can be retried.

## Build and checks

```powershell
.\tools\uninstaller\build.ps1 -OutputDirectory C:\src\DWMBlurGlass-uninstaller-build
```

The build uses the 64-bit .NET Framework C# compiler installed with Windows. The separate test executable uses a fake uninstall backend plus temporary file fixtures. Fifty-six checks passed: operation order, default preservation, failure rollback, path rejection, allowlisted deletion, retention of settings/unrelated files, locked files, and retry behavior. No real startup task or installed DWMBlurGlass process was changed by those checks.

The window and read-only task detection were inspected successfully on November 17, 2026. The utility detected the existing 2.3.1 startup task and executable path. File deletion was unchecked and Uninstall was disabled without administrator elevation. The preview was closed without uninstalling. The default uninstall path was tested successfully against the live DWMBlurGlass-named test copy: its task and processes were removed while files/settings were kept. The rebranded MKUltraBlurGlass task also passed a live uninstall/reinstall cycle; all 10 recorded program, settings and symbol files were preserved unchanged by default uninstall. The optional file-deletion path has been tested with temporary fixtures, not a live installation.

## Protocol sources

- Legacy application shutdown protocol: `Common/Common.h`, `DWMBlurGlass/MHostHelper.cpp`, and `DWMBlurGlass/Helper/Helper.cpp` at the 2.3.1 release source.
- Task Scheduler interfaces: https://learn.microsoft.com/en-us/windows/win32/taskschd/taskfolder-gettask
- Executable action type: https://learn.microsoft.com/en-us/windows/win32/api/taskschd/ne-taskschd-task_action_type

The source is supplied under LGPL-3.0-or-later; see the source package's license texts.

The user confirmed the embedded Final Fantasy click plays correctly. Menu sound can be toggled in the footer; this preference is shared with the main GUI under the current user's MKUltraBlurGlass UI preferences.
