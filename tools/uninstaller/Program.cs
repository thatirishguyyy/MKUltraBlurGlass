// SPDX-License-Identifier: LGPL-3.0-or-later
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Security.Principal;
using System.Windows.Forms;
using System.Reflection;
[assembly: AssemblyTitle("MKUltraBlurGlass Uninstaller")]
[assembly: AssemblyDescription("Uninstaller for MKUltraBlurGlass and legacy DWMBlurGlass")]
[assembly: AssemblyVersion("0.1.0.0")]

namespace DwmUninstaller {
    internal static class Program {
        [STAThread] public static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new UninstallForm());
        }
    }
    internal sealed class UninstallForm : Form {
        private Target target;
        private bool readFailed;
        private readonly TextBox location = new TextBox();
        private readonly TextBox status = new TextBox();
        private readonly Label taskStatus = new Label();
        private readonly CheckBox deleteFiles = new BlueCheckBox();
        private readonly Button uninstall = new BlueButton(), browse = new BlueButton(), admin = new BlueButton();
        private bool IsAdmin {
            get { return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator); }
        }
        internal UninstallForm() {
            Text = "Uninstall MKUltraBlurGlass";
            ClientSize = new Size(640, 522);
            AutoScaleMode = AutoScaleMode.Dpi;
            Font = new Font("Segoe UI", 9.5f);
            BackColor = Color.White;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false; MinimizeBox = false; StartPosition = FormStartPosition.CenterScreen;
            AddLabel("Remove MKUltraBlurGlass", 24, 22, 592, 34, new Font("Segoe UI", 20, FontStyle.Bold));
            AddLabel("Stops the selected copy and removes its startup task.\r\nYour settings and data stay in place.", 24, 70, 592, 44, Font);
            AddLabel("Program location", 24, 122, 592, 22, Font);
            location.SetBounds(24, 148, 480, 28); location.ReadOnly = true; Controls.Add(location);
            browse.Text = "Choose folder"; browse.SetBounds(516, 145, 100, 32);
            browse.Click += delegate { ChooseFolder(); }; Controls.Add(browse);
            taskStatus.SetBounds(24, 186, 592, 38); Controls.Add(taskStatus);
            deleteFiles.Text = "Also delete the four application binaries";
            deleteFiles.SetBounds(24, 232, 592, 26); deleteFiles.Checked = false; Controls.Add(deleteFiles);
            AddLabel("Keeps settings, symbols, language files, and all other data.", 45, 262, 570, 32, Font);
            uninstall.Text = "Uninstall"; uninstall.SetBounds(24, 308, 130, 36);
            uninstall.Click += delegate { Uninstall(); }; Controls.Add(uninstall);
            admin.Text = "Run as administrator"; admin.SetBounds(166, 308, 185, 36);
            admin.Click += delegate {
                try { Process.Start(new ProcessStartInfo(Application.ExecutablePath) { UseShellExecute = true, Verb = "runas" }); Close(); }
                catch (Win32Exception error) { status.Text = error.NativeErrorCode == 1223 ? "Administrator approval cancelled. Nothing was changed." : error.Message; }
            }; Controls.Add(admin);
            var refresh = new BlueButton { Text = "Refresh" }; refresh.SetBounds(363, 308, 100, 36);
            refresh.Click += delegate { Detect(); }; Controls.Add(refresh);
            var close = new BlueButton { Text = "Close", DialogResult = DialogResult.Cancel }; close.SetBounds(516, 308, 100, 36);
            close.Click += delegate { Close(); }; Controls.Add(close); CancelButton = close;
            status.SetBounds(24, 362, 592, 100); status.Multiline = true; status.ReadOnly = true;
            status.ScrollBars = ScrollBars.Vertical; status.BackColor = SystemColors.Control; Controls.Add(status);
            var sound = new BlueCheckBox { Text="Menu sound",Left=24,Top=478,Width=180,Height=25,Checked=MenuAudio.Enabled };
            sound.CheckedChanged += delegate { MenuAudio.SetEnabled(sound.Checked);MenuAudio.Play(); };Controls.Add(sound);
            BlueTheme.Apply(this);
            Shown += delegate { Detect(); };
        }
        private void AddLabel(string text, int x, int y, int width, int height, Font font) {
            var label = new Label { Text = text, Font = font }; label.SetBounds(x, y, width, height); Controls.Add(label);
        }
        private void Detect() {
            readFailed = false;
            deleteFiles.Checked = false;
            try {
                Target registered = new WindowsBackend().Read();
                if (registered != null) target = registered;
                else if (target == null || target.Registered || !File.Exists(target.Exe)) {
                    string local = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MKUltraBlurGlass.exe");
                    target = File.Exists(local) ? new Target { Exe = local } : null;
                }
                taskStatus.Text = registered != null ? "Startup task found: " + registered.TaskName
                    : "No startup task found. Choose the program folder if you want to clean up an extracted copy.";
                status.Text = IsAdmin ? "Ready. Review the detected location before uninstalling."
                    : "Inspection only. Use Run as administrator to enable uninstalling.";
            } catch (Exception error) {
                readFailed = true; target = null;
                taskStatus.Text = "The startup task could not be inspected.";
                status.Text = error.Message + "\r\nNothing was changed. Try running as administrator.";
            }
            UpdateButtons();
        }
        private void UpdateButtons() {
            location.Text = target == null ? "" : target.Exe;
            admin.Visible = !IsAdmin;
            uninstall.Enabled = IsAdmin && !readFailed && target != null;
            browse.Enabled = !readFailed && (target == null || !target.Registered);
            deleteFiles.Enabled = target != null && File.Exists(target.Exe);
        }
        private void ChooseFolder() {
            using (var dialog = new FolderBrowserDialog { Description = "Select the folder containing MKUltraBlurGlass.exe (or legacy DWMBlurGlass.exe)" }) {
                if (dialog.ShowDialog(this) != DialogResult.OK) return;
                try {
                    FilePolicy.CheckFolder(dialog.SelectedPath);
                    string exe = Path.Combine(dialog.SelectedPath, "MKUltraBlurGlass.exe");
                    if (!File.Exists(exe)) exe = Path.Combine(dialog.SelectedPath, "DWMBlurGlass.exe");
                    if (!File.Exists(exe) || !File.Exists(Path.Combine(dialog.SelectedPath, "data", "defaultres.dmres")))
                        throw new InvalidOperationException("Choose a supported program folder containing its executable and data resources.");
                    target = new Target { Exe = FilePolicy.Normalize(exe) }; deleteFiles.Checked = false; UpdateButtons();
                } catch (Exception error) { status.Text = error.Message; }
            }
        }
        private void Uninstall() {
            if (target == null || !IsAdmin) return;
            string message = "Stop the selected copy and remove its startup task?\r\n\r\n" + target.Exe
                + (deleteFiles.Checked ? "\r\n\r\nThe four application binaries will also be deleted." : "\r\n\r\nApplication files will be kept.")
                + "\r\nYour data folder and settings will be kept.";
            if (!BlueTheme.Confirm(this, message)) return;
            Enabled = false; Cursor = Cursors.WaitCursor;
            try {
                status.Text = Core.Run(target, deleteFiles.Checked, new WindowsBackend());
                taskStatus.Text = "No startup task remains for the selected copy.";
                target = null;
            } catch (Exception error) { status.Text = error.Message; }
            finally { Enabled = true; Cursor = Cursors.Default; UpdateButtons(); }
        }
    }
}
