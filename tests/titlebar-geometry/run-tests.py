from pathlib import Path
import re,subprocess,argparse
parser=argparse.ArgumentParser();parser.add_argument('repository',type=Path)
repo=parser.parse_args().repository.resolve()
root=repo/"Build/tests/titlebar-geometry";root.mkdir(parents=True,exist_ok=True)
(root/".gitignore").write_text("*\n",encoding="utf-8")
cpp=repo/'DWMBlurGlassExt/Common/DWMStruct.cpp'
h=repo/'DWMBlurGlassExt/Common/DWMStruct.h'
s=cpp.read_text(encoding='utf-8-sig')
start=s.index('CRgnGeometryProxy* CTopLevelWindow::GetTitlebarGeometry() const')
body_start=s.index('{',start);depth=1;end=body_start+1
while depth:
 depth+=(s[end]=='{')-(s[end]=='}');end+=1
function=s[start:end]
decl=re.search(r'CRgnGeometryProxy\*\s+GetTitlebarGeometry\(\) const;',h.read_text(encoding='utf-8-sig')).group()
pre="""#include <array>
#include <excpt.h>
#include <cstdio>
#include <type_traits>
struct CRgnGeometryProxy { int tag; };
struct CVisual { std::array<void*, 80> slots{}; };
namespace os { unsigned long buildNumber; }
struct CTopLevelWindow { std::array<void*,80> slots{}; DECL };
"""
post="""
static_assert(std::is_same_v<decltype(CTopLevelWindow{}.GetTitlebarGeometry()),CRgnGeometryProxy*>);
int main() {
 int checks=0;
 // Expected layout fixtures at supported build boundaries.
 struct Fixture { unsigned long build; int windowSlot; int visualSlot; };
 const Fixture fixtures[] = {
  {18362,69,-1},{19040,69,-1},{19041,70,-1},{21999,70,-1},
  {22000,72,-1},{22620,72,-1},{22621,39,39},{26019,39,39},
  {26020,34,33},{26099,34,33},{26100,34,33},{26200,34,33},{30000,34,33}
 };
 for (const auto& fixture : fixtures) {
  os::buildNumber=fixture.build;
  CTopLevelWindow window;
  CVisual visual;
  CRgnGeometryProxy wanted{123},replacement{456};
  void** slot=&window.slots[fixture.windowSlot];
  if(fixture.visualSlot>=0) {
   // Missing visual must return null without dereferencing it.
   if(window.GetTitlebarGeometry()!=nullptr) return 10;
   ++checks;
   *slot=&visual;
   slot=&visual.slots[fixture.visualSlot];
  }
  // Null and non-null values must propagate without changing lookup offsets.
  if(window.GetTitlebarGeometry()!=nullptr) return 11;
  ++checks;
  *slot=&wanted;
  auto returned=window.GetTitlebarGeometry();
  if(returned!=&wanted) return 12;
  ++checks;
  *slot=&replacement;
  if(returned!=&wanted || window.GetTitlebarGeometry()!=&replacement) return 13;
  ++checks;
 }
 std::printf("PASS: %d checks across 13 Windows build boundaries\\n",checks);
}
"""
(root/'fixed.cpp').write_text(pre.replace('DECL',decl)+function+post,encoding='utf-8')
# Negative control: catch the original getter's access violation in an isolated test process.
original=(repo/'tests/baseline/DWMStruct.cpp').read_text(encoding='utf-8-sig')
a=original.index('CRgnGeometryProxy* const& CTopLevelWindow::GetTitlebarGeometry() const');b=original.index('\n\tbool CTopLevelWindow::HasNonClientBackground',a)
(root/'original.cpp').write_text(pre.replace('DECL','CRgnGeometryProxy* const& GetTitlebarGeometry() const;')+original[a:b]+'\nint check_original_missing_visual(unsigned long build) {\n os::buildNumber=build;\n CTopLevelWindow window;\n __try {\n  CRgnGeometryProxy* volatile result=window.GetTitlebarGeometry();\n  return result==nullptr?1:2;\n } __except(_exception_code()==0xc0000005?1:0) { return 0; }\n}\nint main() {\n for(auto build:{22621UL,26019UL,26020UL,26200UL})\n  if(check_original_missing_visual(build)!=0) return 10;\n std::puts("PASS: caught original null dereference in 4 simulated layouts");\n}\n',encoding='utf-8')
base=r"""<Project DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
<ItemGroup Label="ProjectConfigurations"><ProjectConfiguration Include="Release|x64"><Configuration>Release</Configuration><Platform>x64</Platform></ProjectConfiguration></ItemGroup>
<PropertyGroup Label="Globals"><WindowsTargetPlatformVersion>10.0</WindowsTargetPlatformVersion></PropertyGroup>
<Import Project="$(VCTargetsPath)\Microsoft.Cpp.Default.props"/>
<PropertyGroup Label="Configuration"><ConfigurationType>Application</ConfigurationType><PlatformToolset>v143</PlatformToolset></PropertyGroup>
<Import Project="$(VCTargetsPath)\Microsoft.Cpp.props"/>
<PropertyGroup><OutDir>$(MSBuildProjectDirectory)\NAME\</OutDir><IntDir>$(MSBuildProjectDirectory)\NAME\obj\</IntDir></PropertyGroup>
<ItemDefinitionGroup><ClCompile><WarningLevel>Level4</WarningLevel><TreatSpecificWarningsAsErrors>4172</TreatSpecificWarningsAsErrors><DebugInformationFormat>OldStyle</DebugInformationFormat><RuntimeLibrary>MultiThreaded</RuntimeLibrary><LanguageStandard>stdcpp20</LanguageStandard><Optimization>OPT</Optimization></ClCompile><Link><SubSystem>Console</SubSystem></Link></ItemDefinitionGroup>
<ItemGroup><ClCompile Include="SOURCE.cpp"/></ItemGroup>
<Import Project="$(VCTargetsPath)\Microsoft.Cpp.targets"/>
</Project>"""
msbuild=Path(r'C:/Program Files/Microsoft Visual Studio/2022/Community/MSBuild/Current/Bin/amd64/MSBuild.exe')
for name,source,opt in [('original','original','Disabled'),('fixed-debug','fixed','Disabled'),('fixed-release','fixed','MaxSpeed')]:
 project=root/(name+'.vcxproj');project.write_text(base.replace('NAME',name).replace('SOURCE',source).replace('OPT',opt),encoding='utf-8')
 r=subprocess.run([str(msbuild),str(project),'/p:Configuration=Release','/p:Platform=x64','/p:PreferredToolArchitecture=x64','/nr:false','/nologo','/v:minimal'],cwd=root,stdout=subprocess.PIPE,stderr=subprocess.STDOUT,text=True,errors='replace')
 (root/(name+'.log')).write_text(r.stdout,encoding='utf-8')
 assert r.returncode==0,r.stdout
 output=subprocess.check_output([str(root/name/(name+'.exe'))],text=True)
 print(name+': '+output.strip(),flush=True)
