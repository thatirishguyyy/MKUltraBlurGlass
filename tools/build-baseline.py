"""Build DWMBlurGlass 2.3.1r in a NEW directory. Requires Python 3, Git and VS2022 C++."""
import argparse
import hashlib
import json
import os
from pathlib import Path
import subprocess
import urllib.request
import xml.etree.ElementTree as ET
import zipfile

SOURCE_COMMIT = '828f09503193aaadf2a00edcbcbadbf21b39e057'
MIAO_COMMIT = '3537e787417463b72eba556be01e89070781096b'
PACKAGES = [
    ('Microsoft.Windows.ImplementationLibrary', '1.0.231216.1', '5557b5c82d00db3bd539aa886058475be552ee09b538cef87cbcaff440baf5e8'),
    ('Microsoft.Windows.ImplementationLibrary', '1.0.240122.1', '2762643e7865c42ac60d809fe951a813ae10960141e56bb74ad28843f2d6b5c1'),
    ('VC-LTL', '5.0.9', '99e6a8d442c18ffecc80b3c65b7f783ff1f163c2d08fc95201652fece88ef62f'),
    ('YY.NuGet.Import.Helper', '1.0.0.4', '111920568af7648d8dbcea5b39f9d73971752d68ca80f817dd304e9d65fe9e4a'),
]


def run(args, cwd=None):
    subprocess.run([str(x) for x in args], cwd=cwd, check=True)


def checkout(url, commit, path):
    run(['git', 'clone', '--no-checkout', url, path])
    run(['git', '-C', path, 'checkout', '--detach', commit])


def build_settings(project):
    # Use an isolated build copy. These settings affect build/debug plumbing,
    # not application source. /Z7 avoids the compiler PDB service stall observed here.
    ns = '{http://schemas.microsoft.com/developer/msbuild/2003}'
    ET.register_namespace('', ns[1:-1])
    tree = ET.parse(project)
    group = ET.Element(ns + 'ItemDefinitionGroup')
    compile_settings = ET.SubElement(group, ns + 'ClCompile')
    ET.SubElement(compile_settings, ns + 'MultiProcessorCompilation').text = 'false'
    ET.SubElement(compile_settings, ns + 'DebugInformationFormat').text = 'OldStyle'
    ET.SubElement(compile_settings, ns + 'AdditionalOptions').text = '/utf-8 %(AdditionalOptions)'
    root = tree.getroot()
    index = next(i for i, node in enumerate(root)
                 if node.tag == ns + 'Import' and node.get('Project') == r'$(VCTargetsPath)\Microsoft.Cpp.targets')
    root.insert(index, group)
    tree.write(project, encoding='utf-8', xml_declaration=True)


def build(msbuild, project, solution_root, log, miao=None):
    args = [msbuild, project, '/m:1', '/nr:false', '/nologo', '/v:quiet',
            '/p:Configuration=Release', '/p:Platform=x64',
            '/p:PreferredToolArchitecture=x64',
            '/p:SolutionDir=' + str(solution_root) + '\\',
            '/fl', '/flp:logfile=' + str(log) + ';verbosity=normal']
    if miao:
        args.append('/p:MUI_LITE_LIBRARY=' + str(miao))
    print('Building', project.name, flush=True)
    run(args, cwd=solution_root)


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('directory', type=Path, help='New, non-existing build directory')
    args = parser.parse_args()
    work = args.directory.resolve()
    if work.exists():
        parser.error('Choose a new directory; existing directories are never overwritten.')
    vswhere = Path(os.environ.get('ProgramFiles(x86)', r'C:\Program Files (x86)')) / 'Microsoft Visual Studio/Installer/vswhere.exe'
    vs = subprocess.check_output([str(vswhere), '-latest', '-version', '[17.0,18.0)',
                                 '-products', '*', '-requires', 'Microsoft.VisualStudio.Component.VC.Tools.x86.x64',
                                 '-property', 'installationPath'], text=True).strip()
    if not vs:
        raise RuntimeError('Visual Studio 2022 C++ tools were not found.')
    msbuild = Path(vs) / 'MSBuild/Current/Bin/amd64/MSBuild.exe'
    work.mkdir(parents=True)
    source = work / 'DWMBlurGlass'
    miao = work / 'MiaoUILite'
    checkout('https://github.com/Maplespe/DWMBlurGlass.git', SOURCE_COMMIT, source)
    checkout('https://github.com/Maplespe/MiaoUILite.git', MIAO_COMMIT, miao)
    for name, version, expected in PACKAGES:
        dest = source / 'packages' / (name + '.' + version)
        dest.mkdir(parents=True)
        archive = dest / (name + '.' + version + '.nupkg')
        url = f'https://api.nuget.org/v3-flatcontainer/{name.lower()}/{version}/{name.lower()}.{version}.nupkg'
        print('Downloading', name, version, flush=True)
        with urllib.request.urlopen(url, timeout=60) as response:
            archive.write_bytes(response.read())
        if hashlib.sha256(archive.read_bytes()).hexdigest() != expected:
            raise RuntimeError('Package hash mismatch: ' + archive.name)
        with zipfile.ZipFile(archive) as z:
            for entry in z.infolist():
                if not (dest / entry.filename).resolve().is_relative_to(dest.resolve()):
                    raise RuntimeError('Unsafe archive path')
            z.extractall(dest)
    miao_project = miao / 'MiaoUI/MiaoUI.vcxproj'
    projects = [source / 'DWMBlurGlassExt/DWMBlurGlassExt.vcxproj',
                source / 'DWMBlurGlass/DWMBlurGlass.vcxproj']
    for project in [miao_project, *projects]:
        build_settings(project)
    build(msbuild, miao_project, miao, work / 'miao-build.log')
    for project in projects:
        build(msbuild, project, source, work / (project.stem + '-build.log'), miao / 'MiaoUI')
    binaries = source / 'Build/x64/Release'
    manifest = {'source_commit': SOURCE_COMMIT, 'miao_commit': MIAO_COMMIT,
                'msbuild': str(msbuild), 'runtime_tested': False,
                'sha256': {name: hashlib.sha256((binaries / name).read_bytes()).hexdigest()
                           for name in ['DWMBlurGlass.exe', 'DWMBlurGlassExt.dll']}}
    (work / 'build-manifest.json').write_text(json.dumps(manifest, indent=2), encoding='utf-8')
    print('Build complete:', binaries)
    print('Not installed or runtime-tested. Runtime packaging is a separate step.')


if __name__ == '__main__':
    main()
