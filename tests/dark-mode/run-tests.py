from pathlib import Path
import re,subprocess,argparse
parser=argparse.ArgumentParser();parser.add_argument('repository',type=Path)
repo=parser.parse_args().repository.resolve()
root=repo/'Build/tests/dark-mode';root.mkdir(parents=True,exist_ok=True)
(root/'.gitignore').write_text('*\n',encoding='utf-8')
def extract(s):
 start=s.index('bool CWindowData::IsUsingDarkMode() const');a=s.index('{',start);end=a+1;depth=1
 while depth:
  depth+=(s[end]=='{')-(s[end]=='}');end+=1
 return s[start:end]
pre='#include <array>\n#include <cstdio>\nusing BYTE=unsigned char;\nnamespace os { unsigned long buildNumber,ubrNumber; }\nstruct CWindowData { std::array<BYTE,800> data{}; bool IsUsingDarkMode() const; };\n'
post=r"""
int main(){
 struct Fixture {unsigned long build,ubr; int offset,mask;};
 const Fixture fixtures[]={
 {18362,0,-1,0},{19040,9999,-1,0},{19041,0,613,8},{21999,9999,613,8},
 {22000,0,669,4},{22620,9999,669,4},{22621,9999,677,4},
 {22631,9999,677,4},{26020,9999,677,4},{26099,9999,677,4},
 {26100,0,677,4},{26100,7170,677,4},{26100,7171,741,4},
 {26100,9999,741,4},{26200,0,741,4},{26200,7019,741,4},
 {26200,9974,741,4}};
 int checks=0,failures=0;
 for(auto f:fixtures){os::buildNumber=f.build;os::ubrNumber=f.ubr;
  CWindowData w;
  for(int value=0;value<256;++value){
   w.data.fill(0);
   if(f.offset>=0)w.data[f.offset]=static_cast<BYTE>(value);
   bool expected=f.offset>=0 && (value&f.mask)!=0;
   if(w.IsUsingDarkMode()!=expected)++failures;++checks;
   // Poison every obsolete offset: only the selected field may decide the mode.
   for(int off:{613,669,677,741})if(off!=f.offset)w.data[off]=255;
   if(w.IsUsingDarkMode()!=expected)++failures;++checks;
  }
 }
 std::printf("%d checks, %d failures\n",checks,failures);return failures?1:0;
}
"""
original=extract((repo/'tests/baseline/DWMStruct.cpp').read_text(encoding='utf-8-sig'))
fixed=extract((repo/'DWMBlurGlassExt/Common/DWMStruct.cpp').read_text(encoding='utf-8-sig'))
for name,body in [('original',original),('fixed',fixed)]: (root/(name+'.cpp')).write_text(pre+body+post,encoding='utf-8')
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
 (root/(name+'.log')).write_text(r.stdout,encoding='utf-8');assert r.returncode==0,r.stdout
 r=subprocess.run([str(root/name/(name+'.exe'))],text=True,capture_output=True)
 print(name+': '+r.stdout.strip(),flush=True)
 assert (r.returncode!=0 if source=='original' else r.returncode==0),r.stdout
