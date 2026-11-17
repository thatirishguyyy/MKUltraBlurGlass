// SPDX-License-Identifier: LGPL-3.0-or-later
using System;
using System.Collections.Generic;
using System.IO;

namespace DwmUninstaller {
    internal sealed class FakeBackend : IBackend {
        internal readonly List<string> Calls = new List<string>();
        internal string FailAt;
        private void Call(string name) { Calls.Add(name); if (FailAt == name) throw new IOException("simulated " + name); }
        public void Verify(Target t) { Call("verify"); }
        public void DisableStartup(Target t) { Call("disable"); }
        public void StopProgram(Target t) { Call("stop"); }
        public void RemoveStartup(Target t) { Call("remove"); }
        public void RestoreStartup(Target t) { Call("restore"); }
        public string DeleteFiles(Target t) { Call("delete"); return "deleted"; }
    }
    internal static class Tests {
        private static int checks;
        private static void Check(bool value, string name) {
            ++checks; if (!value) throw new Exception("FAIL: " + name);
        }
        private static bool Reject(Action action) {
            try { action(); return false; } catch { return true; }
        }
        private static string Fixture(string root, string name) {
            string folder = Path.Combine(root, name);
            Directory.CreateDirectory(Path.Combine(folder, "data"));
            File.WriteAllText(Path.Combine(folder, "data", "defaultres.dmres"), "fixture");
            File.WriteAllText(Path.Combine(folder, "data", "config.ini"), "settings to preserve");
            File.WriteAllText(Path.Combine(folder, "unrelated.txt"), "unrelated file");
            foreach (string file in FilePolicy.Names) File.WriteAllText(Path.Combine(folder, file), "fixture");
            return folder;
        }
        public static int Main(string[] args) {
            try {
                string root = Path.GetFullPath(args[0]);
                Directory.CreateDirectory(root);
                var target = new Target { Exe = @"C:\DWMBlurGlass\DWMBlurGlass.exe", Registered = true, Enabled = true };
                var backend = new FakeBackend();
                Core.Run(target, false, backend);
                Check(String.Join(",", backend.Calls) == "verify,disable,stop,remove", "default never deletes files");
                backend = new FakeBackend(); Core.Run(target, true, backend);
                Check(String.Join(",", backend.Calls) == "verify,disable,stop,remove,delete", "stop before removing task and files");
                foreach (string fail in new[] { "verify", "disable", "stop", "remove" }) {
                    backend = new FakeBackend { FailAt = fail };
                    Check(Reject(delegate { Core.Run(target, true, backend); }), "failure is reported: " + fail);
                    Check(!backend.Calls.Contains("delete"), "no deletion after failure: " + fail);
                    Check(fail == "verify" ? !backend.Calls.Contains("restore") : backend.Calls.Contains("restore"), "rollback after mutation only: " + fail);
                }
                backend = new FakeBackend { FailAt = "delete" };
                Check(Reject(delegate { Core.Run(target, true, backend); }), "cleanup failure is reported");
                Check(!backend.Calls.Contains("restore"), "removed task is not recreated after cleanup failure");
                Check(FilePolicy.Same(@"C:\Example\..\DWMBlurGlass\DWMBlurGlass.exe", @"c:\dwmblurglass\DWMBlurGlass.exe"), "path normalization");
                Check(FilePolicy.Same(@"C:\", @"c:\"), "root normalization is stable");
                foreach (string path in new[] { "", "DWMBlurGlass.exe", @"C:relative.exe" })
                    Check(Reject(delegate { FilePolicy.Normalize(path); }), "relative or empty paths rejected");
                foreach (string path in new[] {
                    Path.GetPathRoot(root), Environment.GetFolderPath(Environment.SpecialFolder.Windows),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "System32"),
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"\\server\share\folder"
                }) Check(Reject(delegate { FilePolicy.CheckFolder(path); }), "protected folder rejected: " + path);
                string good = Fixture(root, "cleanup");
                FilePolicy.DeleteProgramFiles(Path.Combine(good, "DWMBlurGlass.exe"));
                foreach (string file in FilePolicy.Names) Check(!File.Exists(Path.Combine(good, file)), "allowlisted file removed: " + file);
                Check(File.ReadAllText(Path.Combine(good, "data", "config.ini")) == "settings to preserve", "settings preserved");
                Check(File.Exists(Path.Combine(good, "data", "defaultres.dmres")), "resources preserved");
                Check(File.Exists(Path.Combine(good, "unrelated.txt")), "unrelated file preserved");
                string locked = Fixture(root, "locked");
                using (var stream = new FileStream(Path.Combine(locked, "DWMBlurGlassExt.dll"), FileMode.Open, FileAccess.ReadWrite, FileShare.None)) {
                    string result = FilePolicy.DeleteProgramFiles(Path.Combine(locked, "DWMBlurGlass.exe"));
                    Check(result.Contains("Some binaries were kept"), "locked binary is reported");
                    Check(File.Exists(Path.Combine(locked, "DWMBlurGlass.exe")), "EXE retained for retry");
                    Check(File.Exists(Path.Combine(locked, "data", "config.ini")), "locked cleanup preserves settings");
                }
                FilePolicy.DeleteProgramFiles(Path.Combine(locked, "DWMBlurGlass.exe"));
                Check(!File.Exists(Path.Combine(locked, "DWMBlurGlass.exe")), "retry succeeds after lock released");
                string invalid = Fixture(root, "invalid");
                File.Delete(Path.Combine(invalid, "data", "defaultres.dmres"));
                Check(Reject(delegate { FilePolicy.DeleteProgramFiles(Path.Combine(invalid, "DWMBlurGlass.exe")); }), "missing marker rejected");
                Check(File.Exists(Path.Combine(invalid, "DWMBlurGlassExt.dll")), "invalid folder left untouched");
                Check(Reject(delegate { FilePolicy.DeleteProgramFiles(Path.Combine(invalid, "unrelated.txt")); }), "wrong executable rejected");
                Check(WindowsBackend.ExpectedExe(WindowsBackend.TaskName) == "MKUltraBlurGlass.exe", "new task identity");
                Check(WindowsBackend.ExpectedExe(WindowsBackend.LegacyTaskName) == "DWMBlurGlass.exe", "legacy task identity");
                Check(Reject(delegate { WindowsBackend.ExpectedExe("UnrelatedTask"); }), "unrelated tasks rejected");
                string renamed = Fixture(root, "renamed");
                File.Move(Path.Combine(renamed, "DWMBlurGlass.exe"), Path.Combine(renamed, "MKUltraBlurGlass.exe"));
                File.Move(Path.Combine(renamed, "DWMBlurGlassExt.dll"), Path.Combine(renamed, "MKUltraBlurGlassExt.dll"));
                FilePolicy.DeleteProgramFiles(Path.Combine(renamed, "MKUltraBlurGlass.exe"));
                foreach (string file in FilePolicy.NamesFor("MKUltraBlurGlass.exe"))
                    Check(!File.Exists(Path.Combine(renamed, file)), "renamed binary removed: " + file);
                Check(File.Exists(Path.Combine(renamed, "data", "config.ini")), "renamed cleanup preserves settings");
                using (var audio = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("MKUltraBlurGlass.MenuClick.wav")) {
                    Check(audio != null, "click sound embedded");
                    byte[] header = new byte[12]; audio.Read(header,0,12);
                    Check(System.Text.Encoding.ASCII.GetString(header,0,4) == "RIFF" && System.Text.Encoding.ASCII.GetString(header,8,4) == "WAVE", "embedded sound is WAV");
                }
                Check(WindowsBackend.LookupTask(delegate { throw new FileNotFoundException(); }) == null, "missing task managed exception");
                Check(WindowsBackend.LookupTask(delegate { throw new System.Runtime.InteropServices.COMException("missing",unchecked((int)0x80070002)); }) == null, "missing task COM exception");
                Check(Reject(delegate { WindowsBackend.LookupTask(delegate { throw new UnauthorizedAccessException(); }); }), "task permission error is not absence");
                Check(Reject(delegate { WindowsBackend.LookupTask(delegate { throw new System.Runtime.InteropServices.COMException("denied",unchecked((int)0x80070005)); }); }), "COM permission error is not absence");
                object marker = new object(); Check(Object.ReferenceEquals(WindowsBackend.LookupTask(delegate { return marker; }),marker), "existing task returned unchanged");
                Console.WriteLine("PASS: " + checks + " checks. No real task or installed process was changed.");
                return 0;
            } catch (Exception error) { Console.Error.WriteLine(error); return 1; }
        }
    }
}
