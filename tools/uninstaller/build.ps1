param([Parameter(Mandatory=$true)][string]$OutputDirectory)
$ErrorActionPreference = 'Stop'
$outputPath = [System.IO.Path]::GetFullPath($OutputDirectory)
New-Item -ItemType Directory -Force -Path $outputPath | Out-Null
$compilerPath = Join-Path $env:WINDIR 'Microsoft.NET\Framework64\v4.0.30319\csc.exe'
$sourceFiles = @('Core.cs','WindowsBackend.cs','Program.cs','Theme.cs') | ForEach-Object { Join-Path $PSScriptRoot $_ }
$testSourceFiles = @($sourceFiles) + @(Join-Path $PSScriptRoot 'Tests.cs')
$soundResource = Join-Path $PSScriptRoot '..\..\DWMBlurGlass\assets\menu-click.wav'
$references = @("/resource:$soundResource,MKUltraBlurGlass.MenuClick.wav",'/r:System.Windows.Forms.dll','/r:System.Drawing.dll','/r:Microsoft.CSharp.dll','/r:System.Core.dll')
& $compilerPath /nologo /platform:x64 /optimize+ /target:winexe /main:DwmUninstaller.Program "/win32manifest:$PSScriptRoot\app.manifest" "/out:$outputPath\MKUltraBlurGlassUninstall.exe" @references @sourceFiles
if ($LASTEXITCODE -ne 0) { throw 'Uninstaller compilation failed.' }
& $compilerPath /nologo /platform:x64 /optimize+ /target:exe /main:DwmUninstaller.Tests "/out:$outputPath\UninstallerTests.exe" @references @testSourceFiles
if ($LASTEXITCODE -ne 0) { throw 'Test compilation failed.' }
& (Join-Path $outputPath 'UninstallerTests.exe') (Join-Path $outputPath ('test-fixtures-' + [guid]::NewGuid().ToString('N')))
if ($LASTEXITCODE -ne 0) { throw 'Uninstaller tests failed.' }
