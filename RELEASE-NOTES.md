# MKUltraBlurGlass 0.1.0

A personal maintenance fork of DWMBlurGlass 2.3.1, created by Maplespe, with rendering contributions by ALTaleX531 and other upstream contributors. Maintained by thatirishguyyy.

## Changes

- Fix a dangling border-geometry reference and guard missing title-bar geometry.
- Backport upstream commit 74945a1 to clear stale window-color context.
- Add a blue interface matching the owner's Strife palette, embedded menu-click audio, and a persistent sound toggle.
- Rename the executable, extension, startup task and notifications to MKUltraBlurGlass.
- Add an English uninstaller supporting this fork and the legacy DWMBlurGlass layout. Settings and files are preserved by default.
- Provide English maintainer documentation and preserve original author, translator and license credits.

## Setup

Extract the complete runtime ZIP outside your user-profile directory, for example C:\ProgramData\MKUltraBlurGlass. Run MKUltraBlurGlass.exe as administrator. On Symbol, download matching Windows symbols, then use Install on General. Windows updates can require new symbols. Do not enable the original and fork hosts simultaneously. Blur applies to supported window title bars and borders; Start and taskbar effects are outside this program's scope.

To remove the host, run MKUltraBlurGlassUninstall.exe, review the detected path, elevate, then Uninstall. Leave file deletion unchecked to keep all settings and files.

## Validation and limits

The owner confirmed working blur after downloading symbols and automatic restoration after reboot on Windows 11 25H2 build 26200.9974. Menu sound, its toggle and About were confirmed visually/audibly. The renamed task was detected at the correct path; uninstall and reinstall passed. All 10 recorded program/config/symbol files were unchanged after default uninstall.

Regression fixtures pass 320 checks in both unoptimized and optimized modes. The uninstaller passes 56 fake-backend/temporary-file checks. Optional deletion has fixture coverage; it was not exercised on a live installation. This is a release for the tested configuration, not a claim of compatibility with every Windows build or blur method. Existing compiler conversion warnings remain. The final version-label build uses the same rendering code as the tested build.

## Origins and source

Base: DWMBlurGlass 2.3.1 commit 828f09503193aaadf2a00edcbcbadbf21b39e057.
MiaoUI Lite: 3537e787417463b72eba556be01e89070781096b.
Backport: 74945a1656944c5a99864e5000b2e2323d75ffdc.

See CREDITS.md, COPYING.LESSER and LICENSE.txt. The release includes corresponding application, uninstaller and MiaoUI source. The supplied MP3 and decoded WAV are included with source; runtime audio is embedded.
