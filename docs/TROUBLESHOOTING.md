# Troubleshooting MKUltraBlurGlass

The fork executable is MKUltraBlurGlass.exe and its startup task is MKUltraBlurGlass_Extend. The legacy names below help locate an original installation. Do not run both hosts together.

## Finding an existing installation

DWMBlurGlass runs where its ZIP was extracted. Upstream suggests a non-user folder such as `C:\Program Files`. Also check folders named `C:\DWMBlurGlass*`; some archives contain a `Release` subfolder with the executable.

If it is running, use Task Manager to find DWMBlurGlass and select **Open file location**. Its scheduled task is named `DWMBlurGlass_Extend`; the task action can point to the executable even when the GUI is closed.

Read-only PowerShell checks:

```powershell
Get-Process -Name '*DWMBlur*' -ErrorAction SilentlyContinue | Select-Object Name, Path
Get-ScheduledTask -TaskName DWMBlurGlass_Extend -ErrorAction SilentlyContinue | Select-Object -ExpandProperty Actions
```

An empty result does not prove the files were deleted. Search the folders where you normally extract downloads.

## Missing symbols or effects after Windows Update

Open the GUI, go to **Symbols**, and choose **Download**. Upstream notes that symbols may need downloading again after a Windows update. Check network access if downloads fail. Record your exact Windows build using `winver` when reporting problems.

## GUI cannot initialize

Keep the executable, DLLs, and `data` directory together. Confirm `data/defaultres.dmres` and `data/lang/en-US.xml` exist. The program reports that user-directory locations are unsuitable for DWM access; use a non-user directory as described in the installation instructions.

## Before changing versions

Record your current version and settings, and keep a copy of the working extracted folder. Use the current GUI's **Uninstall** action before removing its files. Test new binaries with their matching DLLs and assets. Avoid replacing individual files across versions.

For a bug report include the Windows build, application version or commit, blur method, reproduction steps, other theme/customization software, and any available crash information. Review logs and dumps for private information before sharing them.
