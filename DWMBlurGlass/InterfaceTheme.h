// SPDX-License-Identifier: LGPL-3.0-or-later
#pragma once
#include <windows.h>
#include <shlobj.h>
#include <mmsystem.h>
#include <filesystem>
#include <string>
#include "resource.h"
#pragma comment(lib, "winmm.lib")

namespace MDWMBlurGlass::InterfaceTheme
{
    inline std::wstring PreferencePath()
    {
        PWSTR folder = nullptr;
        if (FAILED(SHGetKnownFolderPath(FOLDERID_LocalAppData, 0, nullptr, &folder)))
            return {};
        std::filesystem::path path(folder);
        CoTaskMemFree(folder);
        return (path / L"MKUltraBlurGlass" / L"ui.ini").wstring();
    }
    inline bool& SoundEnabled()
    {
        static bool enabled = [] {
            const auto path = PreferencePath();
            return path.empty() || GetPrivateProfileIntW(L"interface", L"menuSound", 1, path.c_str()) != 0;
        }();
        return enabled;
    }
    inline void SetSoundEnabled(bool enabled)
    {
        SoundEnabled() = enabled;
        if (!enabled) PlaySoundW(nullptr, nullptr, 0);
        const auto path = PreferencePath();
        if (path.empty()) return;
        std::error_code error;
        std::filesystem::create_directories(std::filesystem::path(path).parent_path(), error);
        if (!error) WritePrivateProfileStringW(L"interface", L"menuSound", enabled ? L"1" : L"0", path.c_str());
    }
    inline void PlayClick()
    {
        if (SoundEnabled())
            PlaySoundW(MAKEINTRESOURCEW(IDR_MENU_CLICK), GetModuleHandleW(nullptr), SND_RESOURCE | SND_ASYNC | SND_NODEFAULT);
    }
}
