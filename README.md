# MKUltraBlurGlass

Windows title-bar effects, based on DWMBlurGlass 2.3.1. This personal maintenance fork adds focused fixes, English documentation, a nice interface, menu-click sounds, and a small uninstaller.

**Created from the original work of Maplespe.** Rendering research and contributions by ALTaleX531 and other upstream contributors made this fork possible. See [credits](CREDITS.md) and the [original project](https://github.com/Maplespe/DWMBlurGlass).

## Status

Version 0.1.1. Tested on Windows 11 25H2 build 26200.9974: matching symbol download, visible window blur, automatic startup after reboot, menu sound and its toggle, About layout, and install/uninstall/reinstall. Default uninstall preserved all 10 recorded program, settings and symbol files unchanged. These results apply to this configuration; other Windows builds and rendering methods have not been validated.

Only one download is needed: `MKUltraBlurGlass-0.1.1.zip` includes both the application and `MKUltraBlurGlassUninstall.exe`. The source archive is optional for developers.

Download the runtime ZIP from [Releases](https://github.com/thatirishguyyy/MKUltraBlurGlass/releases). The maintained branch is `mkultra/main`; `master` preserves the later upstream snapshot.

Version 0.1.1 fixes inherited dark-mode detection on current Windows 11 builds. Confirmed that explicit Light and Dark windows now independently follow their matching active/inactive color settings. See [known issues](KNOWN-ISSUES.md) for the separately reported color-picker interaction.

## Changes

- Return border geometry by value instead of returning a dangling reference.
- Handle absent title-bar geometry without dereferencing null.
- Backport upstream's stale window-color context fix, commit 74945a1.
- Use the MKUltraBlurGlass executable, DLL, startup-task and notification names consistently.
- Match the owner's Strife blue palette, with an embedded Final Fantasy menu click and a Menu sound toggle.
- Add a matching English uninstaller that keeps settings by default and recognizes both MKUltraBlurGlass and legacy DWMBlurGlass tasks.
- Repair the Swedish language XML declaration and closing tags.

## Run

Extract the complete release folder outside your user-profile directory. Launch `MKUltraBlurGlass.exe` as administrator. Open **Symbol** and download matching Windows symbol files before enabling effects. An Installed status alone does not prove symbols are valid or blur is active. The About page contains the Menu sound toggle and upstream credits.

Do not enable this fork and the original DWMBlurGlass host at the same time; they modify the same Windows compositor. The uninstaller keeps settings and data by default. Optional cleanup removes only the selected copy's executable, extension, and symbol helper DLLs.

## Source and license

The retained Visual Studio solution/project names reflect the upstream source layout. Their output files are `MKUltraBlurGlass.exe` and `MKUltraBlurGlassExt.dll`.

See [building](docs/BUILDING.md), [maintenance](docs/MAINTAINING.md), [troubleshooting](docs/TROUBLESHOOTING.md), [uninstaller](tools/uninstaller/README.md), and [credits](CREDITS.md). The existing translated READMEs are retained as historical upstream translations.

This project remains under the GNU Lesser General Public License v3 or later. See COPYING.LESSER and LICENSE.txt, and retained third-party notices.
