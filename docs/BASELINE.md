# Building the working 2.3.1 baseline

Run this on Windows with Python 3, Git, Visual Studio 2022 C++ tools, and a Windows SDK installed:

```powershell
python tools/build-baseline.py C:\src\DWMBlurGlass-baseline-build
```

Choose a new directory. The script refuses to overwrite an existing directory. Downloads require internet access.

The script checks out DWMBlurGlass release commit `828f09503193aaadf2a00edcbcbadbf21b39e057` and MiaoUI Lite commit `3537e787417463b72eba556be01e89070781096b`. It downloads the four exact NuGet packages referenced by the projects, verifies their recorded SHA-256 hashes, and extracts them into the build directory. This avoids reading or modifying your user NuGet configuration.

Build copies use 64-bit MSBuild and compiler tools, serial compilation, embedded compiler debug information (`/Z7`), and UTF-8 source/execution encoding. These settings address stalls and encoding warnings encountered on this machine. The application's C++ source is unchanged; the two later upstream fixes and the WPF beta are excluded.

Build logs and `build-manifest.json` are saved in the chosen directory. Compiled files are under `DWMBlurGlass\Build\x64\Release`.

A successful build establishes compilation with the recorded dependencies. It does not establish byte-identical output to the original release or confirm runtime compatibility. The script does not install or launch the application, copy settings, or modify scheduled tasks. Runtime assets and packaging must be prepared separately before testing.
