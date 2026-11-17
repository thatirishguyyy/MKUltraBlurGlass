# MKUltraBlurGlass

[English](README.md) · [Deutsch](README_DE.md) · [Español](README_ES.md) · [Français](README_FR.md) · [Italiano](README_IT.md) · [Português (Brasil)](README_PTBR.md) · [Türkçe](README_TR.md) · [Tiếng Việt](README_VI.md) · [简体中文](README_ZH.md)

这是基于 DWMBlurGlass 2.3.1 的个人维护分支，用于为 Windows 窗口标题栏和边框添加效果，由 **thatirishguyyy** 维护。

**原项目由 Maplespe 创建。** 感谢 Maplespe、ALTaleX531，以及所有贡献者和翻译者。请参阅[致谢](CREDITS.md)和[原项目](https://github.com/Maplespe/DWMBlurGlass)。

## 版本与下载

**0.1.1** 修复了较新 Windows 11 版本上的窗口深色模式识别问题。浅色和深色窗口现在能分别使用各自的活动、非活动窗口颜色设置。本分支还包含几何对象与颜色上下文修复、蓝色界面、可在 About 页面关闭的菜单点击音效，以及英文卸载工具。

只需下载一个文件：[MKUltraBlurGlass-0.1.1.zip](https://github.com/thatirishguyyy/MKUltraBlurGlass/releases/download/v0.1.1/MKUltraBlurGlass-0.1.1.zip)，其中已包含程序和 `MKUltraBlurGlassUninstall.exe` 卸载工具。源码压缩包供开发者使用，普通用户无需下载。所有下载见[发布页面](https://github.com/thatirishguyyy/MKUltraBlurGlass/releases)。

## 安装与更新

1. 切换到另一份程序前，先卸载旧副本的宿主并关闭其界面。不要同时启用原版和本分支的宿主。
2. 将完整 ZIP 解压到用户配置文件目录之外，例如 `C:\ProgramData\MKUltraBlurGlass`。程序、DLL 和 `data` 文件夹必须放在一起。
3. 以管理员身份运行 `MKUltraBlurGlass.exe`。在 **Symbol** 页面下载与当前 Windows 匹配的符号文件，再在 **General** 页面点击 **Install**。仅显示“Installed”并不能证明符号有效或模糊效果已经生效。Windows 更新后可能需要重新下载符号。
4. **Light Mode colors** 和 **Dark Mode colors** 根据每个窗口自身的浅色、深色模式应用。修改后点击 **Save** 保存。

## 卸载

打开 `MKUltraBlurGlassUninstall.exe`，核对识别到的程序路径，获取管理员权限，然后点击 **Uninstall**。默认保留程序文件、设置和符号文件。可选清理仅删除所选副本的四个程序二进制文件，保留数据文件夹。卸载工具也能识别原版 DWMBlurGlass 的启动任务。

## 测试与已知限制

已在 Windows 11 25H2 **26200.9974** 上测试。0.1.0 已验证安装、卸载、重新安装和重启后自动启动。0.1.1 已确认更新成功，以及浅色、深色窗口颜色可独立设置；尚未针对本次补丁重新进行重启测试。其他系统版本及效果方法尚未得到完整验证。开始菜单和任务栏不在本程序的作用范围内。

取色器可能需要移动主调色板上的选择点后才会应用选定颜色。准确复现步骤尚未确认，详见[已知问题](KNOWN-ISSUES.md)。

## 源码、文档与许可证

维护分支为 `mkultra/main`。合并上游改动前必须先审查并测试，不要直接同步。保留历史解决方案和项目名称；输出文件为 `MKUltraBlurGlass.exe` 和 `MKUltraBlurGlassExt.dll`。

详细维护文档目前使用英文：[构建](docs/BUILDING.md)、[维护](docs/MAINTAINING.md)、[故障排查](docs/TROUBLESHOOTING.md)、[卸载工具](tools/uninstaller/README.md)和[发布说明](RELEASE-NOTES.md)。如需翻译界面，请修改 `Languagefiles/` 中的 XML 字段值，并保留键名、占位符及作者信息。

本项目采用 GNU LGPL 第 3 版或更新版本，见 [COPYING.LESSER](COPYING.LESSER) 和 [LICENSE.txt](LICENSE.txt)。保留原作者及第三方的版权和许可声明。依赖项及作者列于 [CREDITS.md](CREDITS.md)。
