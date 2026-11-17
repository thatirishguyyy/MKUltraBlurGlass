# Dark-mode detection on current Windows builds

Reported symptom: changing Light active/inactive colors affects windows even with Windows in dark mode; Dark colors have no visible effect.

The inherited CWindowData::IsUsingDarkMode reads byte 677 for all builds from 22621 onward. Upstream 2.3.2b3 (commit 94e85eb) identifies byte 741, mask 4, for the newer 24H2/25H2 layout. This fix adopts that field for build 26200+ and 26100 revisions 7171+, leaving older layout boundaries unchanged. The 24H2 revision gate is scoped to build 26100 so high revision numbers on older Windows branches do not select the new field. UBR is read once from the 64-bit CurrentVersion registry key with DWORD validation; read failure yields zero, while 25H2 selection depends on build alone.

Only dark-mode detection is changed. Other beta layout changes are not included. Per-window light/dark semantics are retained; this does not force all windows to use the global Windows preference.

Run: python tests/dark-mode/run-tests.py .

The fixture extracts the actual production method and checks all 256 byte values at 17 build/revision boundaries, both with zeroed and poisoned obsolete offsets. The original fails 1,280 of 8,704 checks; the fixed method passes all checks in both unoptimized and optimized builds. This proves layout selection against the documented fixtures, not the live DWM layout. The owner confirmed on 26200.9974 that Dark-tab colors now affect dark windows, then verified simultaneous explicit LIGHT and DARK test windows each follow their matching active/inactive colors independently. The test app's DwmSetWindowAttribute and DwmGetWindowAttribute calls returned success and read back dark=0 and dark=1 respectively. Reboot persistence has not been repeated specifically for this patch.

Source: https://github.com/Maplespe/DWMBlurGlass/blob/2.3.2b3/DWMBlurGlassExt/Common/DWMStruct.cpp
