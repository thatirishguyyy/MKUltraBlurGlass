// SPDX-License-Identifier: LGPL-3.0-or-later
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace DwmUninstaller {
    internal sealed class WindowsBackend : IBackend {
        internal const string TaskName = "MKUltraBlurGlass_Extend";
        internal const string LegacyTaskName = "DWMBlurGlass_Extend";
        internal static string ExpectedExe(string name) {
            if (name == TaskName) return "MKUltraBlurGlass.exe";
            if (name == LegacyTaskName) return "DWMBlurGlass.exe";
            throw new InvalidOperationException("Unsupported startup task name.");
        }
        private dynamic service, folder, task;
        internal static object LookupTask(Func<object> lookup) {
            try { return lookup(); }
            catch (FileNotFoundException) { return null; }
            catch (COMException error) {
                if (unchecked((uint)error.ErrorCode) == 0x80070002) return null;
                throw;
            }
        }
        internal Target Read() {
            return ReadTask(TaskName) ?? ReadTask(LegacyTaskName);
        }
        private Target ReadTask(string name) {
            string expectedExe = ExpectedExe(name);
            service = Activator.CreateInstance(Type.GetTypeFromProgID("Schedule.Service", true));
            service.Connect();
            folder = service.GetFolder("\\");
            task = LookupTask(delegate { return (object)folder.GetTask(name); });
            if (task == null) return null;
            dynamic actions = task.Definition.Actions;
            if ((int)actions.Count != 1 || (int)actions.Item(1).Type != 0)
                throw new InvalidOperationException("The startup task has an unrecognized action. It will not be changed.");
            string exe = FilePolicy.Normalize((string)actions.Item(1).Path);
            string args = ((string)actions.Item(1).Arguments ?? "").Trim();
            if (!String.Equals(Path.GetFileName(exe), expectedExe, StringComparison.OrdinalIgnoreCase)
                || !String.Equals(args, "runhost", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("The startup task is not a recognized host task. It will not be changed.");
            return new Target { Exe = exe, TaskName = name, Registered = true, Enabled = (bool)task.Enabled };
        }
        public void Verify(Target target) {
            Target current = target.Registered ? ReadTask(target.TaskName) : Read();
            if (target.Registered != (current != null)
                || (current != null && (!FilePolicy.Same(current.Exe, target.Exe) || current.Enabled != target.Enabled)))
                throw new InvalidOperationException("The startup task changed. Refresh before uninstalling.");
            if (!target.Registered) {
                FilePolicy.CheckFolder(Path.GetDirectoryName(target.Exe));
                if (!File.Exists(target.Exe)) throw new InvalidOperationException("The selected executable no longer exists.");
            }
        }
        private bool StillMatches(Target target) {
            Target current = target.Registered ? ReadTask(target.TaskName) : Read();
            if (current == null) return false;
            if (!FilePolicy.Same(current.Exe, target.Exe))
                throw new InvalidOperationException("The startup task now points elsewhere. It will not be changed.");
            return true;
        }
        public void DisableStartup(Target target) {
            if (target.Registered && StillMatches(target)) task.Enabled = false;
        }
        public void RemoveStartup(Target target) {
            if (target.Registered && StillMatches(target)) { folder.DeleteTask(target.TaskName, 0); task = null; }
            else if (!target.Registered && Read() != null)
                throw new InvalidOperationException("A startup task appeared. Refresh before continuing.");
        }
        public void RestoreStartup(Target target) {
            if (target.Registered && StillMatches(target)) task.Enabled = target.Enabled;
        }
        public string DeleteFiles(Target target) {
            Target other = Read();
            if (other != null && FilePolicy.Same(Path.GetDirectoryName(other.Exe), Path.GetDirectoryName(target.Exe)))
                throw new InvalidOperationException("A supported startup task still uses this folder. Files were kept.");
            return FilePolicy.DeleteProgramFiles(target.Exe);
        }
        public void StopProgram(Target target) {
            foreach (Process process in Process.GetProcessesByName(Path.GetFileNameWithoutExtension(target.Exe))) {
                using (process) {
                    IntPtr handle = OpenProcess(0x00101001, false, process.Id); // query, wait, terminate
                    if (handle == IntPtr.Zero)
                        throw new Win32Exception(Marshal.GetLastWin32Error(), "A supported program process could not be inspected. Run as administrator.");
                    try {
                        var path = new StringBuilder(32768); int length = path.Capacity;
                        if (!QueryFullProcessImageName(handle, 0, path, ref length))
                            throw new Win32Exception(Marshal.GetLastWin32Error());
                        if (!FilePolicy.Same(path.ToString(), target.Exe)) continue;
                        IntPtr host = IntPtr.Zero, found = IntPtr.Zero;
                        while ((host = FindWindowEx(new IntPtr(-3), host, (String.Equals(Path.GetFileName(target.Exe), "MKUltraBlurGlass.exe", StringComparison.OrdinalIgnoreCase) ? "MKUltraBlurGlassHostNotify" : "MDWMBlurGlassHostNotify"), null)) != IntPtr.Zero) {
                            uint pid; GetWindowThreadProcessId(host, out pid);
                            if (pid == process.Id) { found = host; break; }
                        }
                        if (found != IntPtr.Zero) {
                            UIntPtr result;
                            // Same shutdown/unload message used by the 2.3.1 GUI.
                            if (SendMessageTimeout(found, 0x8000 + 20, new UIntPtr(1), IntPtr.Zero, 3, 5000, out result) == IntPtr.Zero)
                                throw new InvalidOperationException("The host did not finish its shutdown request. Restart Windows and try again.");
                            if (!TerminateProcess(handle, 0) && WaitForSingleObject(handle, 0) != 0)
                                throw new Win32Exception(Marshal.GetLastWin32Error());
                        } else if (!process.CloseMainWindow() && WaitForSingleObject(handle, 0) != 0) {
                            throw new InvalidOperationException("A matching program process has no shutdown window. Close it and try again.");
                        }
                        if (WaitForSingleObject(handle, 3000) != 0)
                            throw new InvalidOperationException("The selected program is still running. Close its window and try again.");
                    } finally { CloseHandle(handle); }
                }
            }
        }
        [DllImport("kernel32.dll", SetLastError=true)] private static extern IntPtr OpenProcess(uint access, bool inherit, int id);
        [DllImport("kernel32.dll", CharSet=CharSet.Unicode, SetLastError=true)] private static extern bool QueryFullProcessImageName(IntPtr process, int flags, StringBuilder name, ref int length);
        [DllImport("kernel32.dll")] private static extern bool CloseHandle(IntPtr handle);
        [DllImport("kernel32.dll", SetLastError=true)] private static extern bool TerminateProcess(IntPtr process, uint code);
        [DllImport("kernel32.dll")] private static extern uint WaitForSingleObject(IntPtr handle, uint time);
        [DllImport("user32.dll", CharSet=CharSet.Unicode)] private static extern IntPtr FindWindowEx(IntPtr parent, IntPtr after, string name, string title);
        [DllImport("user32.dll")] private static extern uint GetWindowThreadProcessId(IntPtr window, out uint id);
        [DllImport("user32.dll", CharSet=CharSet.Unicode, SetLastError=true)] private static extern IntPtr SendMessageTimeout(IntPtr window, uint message, UIntPtr wParam, IntPtr lParam, uint flags, uint timeout, out UIntPtr result);
    }
}
