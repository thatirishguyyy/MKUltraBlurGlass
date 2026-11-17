// SPDX-License-Identifier: LGPL-3.0-or-later
using System;
using System.Collections.Generic;
using System.IO;

namespace DwmUninstaller {
    internal sealed class Target {
        internal string Exe;
        internal string TaskName;
        internal bool Registered;
        internal bool Enabled;
    }
    internal interface IBackend {
        void Verify(Target target);
        void DisableStartup(Target target);
        void StopProgram(Target target);
        void RemoveStartup(Target target);
        void RestoreStartup(Target target);
        string DeleteFiles(Target target);
    }
    internal static class Core {
        internal static string Run(Target target, bool deleteFiles, IBackend backend) {
            backend.Verify(target);
            bool removed = false;
            try {
                backend.DisableStartup(target);
                backend.StopProgram(target);
                backend.RemoveStartup(target);
                removed = true;
                string cleanup = deleteFiles ? backend.DeleteFiles(target) : "Application files were kept.";
                return (target.Registered ? "Startup task removed." : "No startup task was registered.")
                    + " Settings and data were kept.\r\n" + cleanup
                    + "\r\nSign out or restart Windows if any visual effects remain.";
            } catch (Exception error) {
                if (!removed) {
                    try { backend.RestoreStartup(target); }
                    catch (Exception restoreError) {
                        throw new InvalidOperationException("Uninstall stopped: " + error.Message
                            + "\r\nStartup could not be restored: " + restoreError.Message);
                    }
                    throw new InvalidOperationException("Uninstall stopped; the startup setting was restored. " + error.Message);
                }
                throw new InvalidOperationException("Startup was removed, but file cleanup failed. Settings and data were kept. " + error.Message);
            }
        }
    }
    internal static class FilePolicy {
        // No recursive deletion, self-deletion, data files, settings, or symbols.
        internal static readonly string[] Names = {
            "DWMBlurGlassExt.dll", "dbghelp.dll", "symsrv.dll", "DWMBlurGlass.exe"
        };
        internal static string[] NamesFor(string exe) {
            string name = Path.GetFileName(exe);
            if (String.Equals(name, "MKUltraBlurGlass.exe", StringComparison.OrdinalIgnoreCase))
                return new[] { "MKUltraBlurGlassExt.dll", "dbghelp.dll", "symsrv.dll", "MKUltraBlurGlass.exe" };
            if (String.Equals(name, "DWMBlurGlass.exe", StringComparison.OrdinalIgnoreCase)) return Names;
            throw new InvalidOperationException("This is not a supported program executable path.");
        }
        internal static string Normalize(string path) {
            if (String.IsNullOrWhiteSpace(path)) throw new InvalidOperationException("The program path is empty.");
            path = Environment.ExpandEnvironmentVariables(path.Trim());
            if (path.StartsWith("\"") && path.EndsWith("\"")) path = path.Substring(1, path.Length - 2);
            string root = Path.GetPathRoot(path);
            if (String.IsNullOrEmpty(root) || (!path.StartsWith("\\\\") && root.Length < 3))
                throw new InvalidOperationException("The program path must be absolute.");
            string full = Path.GetFullPath(path);
            return full.Length == Path.GetPathRoot(full).Length ? full : full.TrimEnd(Path.DirectorySeparatorChar);
        }
        internal static bool Same(string a, string b) {
            return String.Equals(Normalize(a), Normalize(b), StringComparison.OrdinalIgnoreCase);
        }
        internal static void CheckFolder(string folder) {
            folder = Normalize(folder);
            if (folder.StartsWith("\\\\")) throw new InvalidOperationException("Network-folder cleanup is not supported.");
            string[] protectedFolders = {
                Path.GetPathRoot(folder + "\\"),
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)
            };
            foreach (string path in protectedFolders)
                if (!String.IsNullOrEmpty(path) && Same(folder, path))
                    throw new InvalidOperationException("Choose the application's own folder, not a shared root folder.");
            string windows = Normalize(Environment.GetFolderPath(Environment.SpecialFolder.Windows));
            if (Same(folder, windows) || folder.StartsWith(windows + "\\", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Windows system folders cannot be cleaned up.");
            for (DirectoryInfo item = new DirectoryInfo(folder); item != null; item = item.Parent)
                if ((item.Attributes & FileAttributes.ReparsePoint) != 0)
                    throw new InvalidOperationException("Cleanup through junctions or symbolic links is not supported.");
        }
        internal static string DeleteProgramFiles(string exe) {
            exe = Normalize(exe);
            string[] selectedNames = NamesFor(exe);
            string folder = Path.GetDirectoryName(exe);
            CheckFolder(folder);
            if (!File.Exists(exe) || !File.Exists(Path.Combine(folder, "data", "defaultres.dmres")))
                throw new InvalidOperationException("The selected folder could not be verified as a supported program folder.");
            foreach (string name in selectedNames) {
                string file = Path.Combine(folder, name);
                if (File.Exists(file) && (File.GetAttributes(file) & FileAttributes.ReparsePoint) != 0)
                    throw new InvalidOperationException("A program file is a symbolic link; cleanup was cancelled.");
            }
            var failures = new List<string>();
            foreach (string name in selectedNames) {
                // Keep the EXE for a later retry if another binary is locked.
                if (String.Equals(name, Path.GetFileName(exe), StringComparison.OrdinalIgnoreCase) && failures.Count != 0) break;
                try { File.Delete(Path.Combine(folder, name)); }
                catch (IOException error) { failures.Add(name + ": " + error.Message); }
                catch (UnauthorizedAccessException error) { failures.Add(name + ": " + error.Message); }
            }
            return failures.Count == 0 ? "The four application binaries were removed."
                : "Some binaries were kept. Close DWMBlurGlass and try again:\r\n" + String.Join("\r\n", failures);
        }
    }
}
