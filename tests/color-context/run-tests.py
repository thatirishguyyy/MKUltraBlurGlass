"""Exercise the actual color hooks and visual-validation hook with isolated API stubs."""
import argparse
from pathlib import Path
import re
import subprocess

parser = argparse.ArgumentParser(description=__doc__)
parser.add_argument('repository', type=Path)
repo = parser.parse_args().repository.resolve()
root = repo / 'Build/tests/color-context'
root.mkdir(parents=True, exist_ok=True)
(root / '.gitignore').write_text('*\n', encoding='utf-8')
path = 'DWMBlurGlassExt/Section/CustomBackdropEffect.cpp'
fixed = (repo / path).read_text(encoding='utf-8-sig')
original = (repo / 'tests/baseline/CustomBackdropEffect.cpp').read_text(encoding='utf-8-sig')
template = (Path(__file__).parent / 'harness.cpp.in').read_text(encoding='utf-8')


def functions(source):
    result = []
    for signature in [
        'HRESULT CTopLevelWindow_ValidateVisual(',
        'DWORD64 CGlassColorizationParameters_AdjustWindowColorization_Win10(',
        'DWORD64 CGlassColorizationParameters_AdjustWindowColorization_Win11('
    ]:
        start = source.index(signature)
        end = source.index('{', start) + 1
        depth = 1
        while depth:
            depth += (source[end] == '{') - (source[end] == '}')
            end += 1
        result.append(source[start:end])
    return '\n\n'.join(result)


project_template = r'''<Project DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
<ItemGroup Label="ProjectConfigurations"><ProjectConfiguration Include="Release|x64"><Configuration>Release</Configuration><Platform>x64</Platform></ProjectConfiguration></ItemGroup>
<PropertyGroup Label="Globals"><WindowsTargetPlatformVersion>10.0</WindowsTargetPlatformVersion></PropertyGroup>
<Import Project="$(VCTargetsPath)\Microsoft.Cpp.Default.props"/>
<PropertyGroup Label="Configuration"><ConfigurationType>Application</ConfigurationType><PlatformToolset>v143</PlatformToolset></PropertyGroup>
<Import Project="$(VCTargetsPath)\Microsoft.Cpp.props"/>
<PropertyGroup><OutDir>$(MSBuildProjectDirectory)\NAME\</OutDir><IntDir>$(MSBuildProjectDirectory)\NAME\obj\</IntDir></PropertyGroup>
<ItemDefinitionGroup><ClCompile><WarningLevel>Level4</WarningLevel><DebugInformationFormat>OldStyle</DebugInformationFormat><RuntimeLibrary>MultiThreaded</RuntimeLibrary><LanguageStandard>stdcpp20</LanguageStandard><Optimization>OPT</Optimization></ClCompile><Link><SubSystem>Console</SubSystem></Link></ItemDefinitionGroup>
<ItemGroup><ClCompile Include="NAME.cpp"/></ItemGroup>
<Import Project="$(VCTargetsPath)\Microsoft.Cpp.targets"/>
</Project>'''
msbuild = Path(r'C:/Program Files/Microsoft Visual Studio/2022/Community/MSBuild/Current/Bin/amd64/MSBuild.exe')
for name, source, optimization in [
    ('original', original, 'Disabled'),
    ('fixed-debug', fixed, 'Disabled'),
    ('fixed-release', fixed, 'MaxSpeed')
]:
    (root / (name + '.cpp')).write_text(template.replace('// INSERT PRODUCTION FUNCTIONS', functions(source)), encoding='utf-8')
    project = root / (name + '.vcxproj')
    project.write_text(project_template.replace('NAME', name).replace('OPT', optimization), encoding='utf-8')
    result = subprocess.run([
        str(msbuild), str(project), '/p:Configuration=Release', '/p:Platform=x64',
        '/p:PreferredToolArchitecture=x64', '/nr:false', '/nologo', '/v:minimal'
    ], cwd=root, stdout=subprocess.PIPE, stderr=subprocess.STDOUT, text=True, errors='replace')
    (root / (name + '-build.log')).write_text(result.stdout, encoding='utf-8')
    assert result.returncode == 0, result.stdout
    result = subprocess.run([str(root / name / (name + '.exe'))], capture_output=True, text=True)
    (root / (name + '.log')).write_text(result.stdout + result.stderr, encoding='utf-8')
    if name == 'original':
        assert result.returncode == 1, result.stdout
        for description in ['unscoped original color preserved', 'early return clears stale context', 'completed validation clears context']:
            assert 'FAIL: ' + description in result.stdout, result.stdout
        print('Original release: reproduced unscoped color override and stale context failures', flush=True)
    else:
        assert result.returncode == 0 and re.search(r'\d+ checks, 0 failures', result.stdout), result.stdout
        print(name + ': ' + result.stdout.strip(), flush=True)
