// SPDX-License-Identifier: LGPL-3.0-or-later
using System;
using System.Drawing;
using System.IO;
using System.Media;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DwmUninstaller {
    internal static class MenuAudio {
        private static SoundPlayer player;
        private static Stream stream;
        private static bool enabled = GetPrivateProfileInt("interface", "menuSound", 1, PreferencePath) != 0;
        private static string PreferencePath {
            get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MKUltraBlurGlass", "ui.ini"); }
        }
        internal static bool Enabled { get { return enabled; } }
        internal static void SetEnabled(bool value) {
            enabled = value;
            if (!value && player != null) player.Stop();
            try {
                Directory.CreateDirectory(Path.GetDirectoryName(PreferencePath));
                WritePrivateProfileString("interface", "menuSound", value ? "1" : "0", PreferencePath);
            } catch (IOException) { } catch (UnauthorizedAccessException) { }
        }
        internal static void Play() {
            if (!enabled) return;
            try {
                if (player == null) {
                    stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MKUltraBlurGlass.MenuClick.wav");
                    if (stream == null) return;
                    player = new SoundPlayer(stream); player.Load();
                }
                player.Play();
            } catch (Exception) { /* Optional audio never blocks an action. */ }
        }
        [DllImport("kernel32.dll", CharSet=CharSet.Unicode)] private static extern int GetPrivateProfileInt(string section, string key, int value, string path);
        [DllImport("kernel32.dll", CharSet=CharSet.Unicode)] private static extern bool WritePrivateProfileString(string section, string key, string value, string path);
    }
    internal static class BlueTheme {
        internal static readonly Color Background = Color.FromArgb(6,21,78), Panel = Color.FromArgb(16,47,144),
            Input = Color.FromArgb(7,29,99), Button = Color.FromArgb(22,72,189), Hover = Color.FromArgb(61,117,237),
            Pressed = Color.FromArgb(23,60,158), Border = Color.FromArgb(199,215,255), Text = Color.FromArgb(243,246,255),
            Muted = Color.FromArgb(142,167,210);
        internal static void Apply(Form form) {
            form.BackColor = Background;
            form.Paint += delegate(object sender, PaintEventArgs e) {
                using (var brush = new SolidBrush(Panel)) e.Graphics.FillRectangle(brush, 12,12,form.ClientSize.Width-24,form.ClientSize.Height-24);
                using (var pen = new Pen(Border)) e.Graphics.DrawRectangle(pen,12,12,form.ClientSize.Width-25,form.ClientSize.Height-25);
            };
            StyleControls(form);
            form.Shown += delegate {
                uint caption = (uint)(Background.R | Background.G<<8 | Background.B<<16), text = 0xffffff;
                DwmSetWindowAttribute(form.Handle,35,ref caption,4);
                DwmSetWindowAttribute(form.Handle,36,ref text,4);
            };
        }
        private static void StyleControls(Control parent) {
            foreach (Control control in parent.Controls) {
                control.ForeColor = Text;
                if (control is TextBox) { control.BackColor = Input; ((TextBox)control).BorderStyle = BorderStyle.FixedSingle; }
                else if (!(control is Button)) control.BackColor = Panel;
                if (control.HasChildren) StyleControls(control);
            }
        }
        internal static bool Confirm(Form owner, string message) {
            using (var dialog = new Form { Text="Confirm MKUltraBlurGlass uninstall", ClientSize=new Size(570,330),
                FormBorderStyle=FormBorderStyle.FixedDialog, StartPosition=FormStartPosition.CenterParent,
                MaximizeBox=false, MinimizeBox=false, Font=new Font("Segoe UI",10) }) {
                var label = new Label { Text=message,Left=28,Top=28,Width=514,Height=220 }; dialog.Controls.Add(label);
                var yes = new BlueButton { Text="Uninstall",Left=28,Top=265,Width=140,Height=36,DialogResult=DialogResult.Yes };
                var no = new BlueButton { Text="Cancel",Left=402,Top=265,Width=140,Height=36,DialogResult=DialogResult.No };
                dialog.Controls.Add(yes);dialog.Controls.Add(no);dialog.CancelButton=no;dialog.AcceptButton=no;
                Apply(dialog);dialog.Shown+=delegate { no.Focus(); };
                return dialog.ShowDialog(owner)==DialogResult.Yes;
            }
        }
        [DllImport("dwmapi.dll")] private static extern int DwmSetWindowAttribute(IntPtr window,int attribute,ref uint value,int size);
    }
    internal sealed class BlueButton : Button {
        private bool hover, pressed;
        internal BlueButton() {
            SetStyle(ControlStyles.UserPaint|ControlStyles.AllPaintingInWmPaint|ControlStyles.OptimizedDoubleBuffer,true);
            FlatStyle=FlatStyle.Flat;UseVisualStyleBackColor=false;
        }
        protected override void OnPaint(PaintEventArgs e) {
            e.Graphics.Clear(!Enabled ? BlueTheme.Input : pressed ? BlueTheme.Pressed : hover ? BlueTheme.Hover : BlueTheme.Button);
            ControlPaint.DrawBorder(e.Graphics,ClientRectangle,BlueTheme.Border,ButtonBorderStyle.Solid);
            TextRenderer.DrawText(e.Graphics,Text,Font,ClientRectangle,Enabled ? Color.White : BlueTheme.Muted,
                TextFormatFlags.HorizontalCenter|TextFormatFlags.VerticalCenter|TextFormatFlags.EndEllipsis);
            if (Focused && ShowFocusCues) ControlPaint.DrawFocusRectangle(e.Graphics,Rectangle.Inflate(ClientRectangle,-5,-5),Color.White,BlueTheme.Button);
        }
        protected override void OnMouseEnter(EventArgs e) { hover=true;Invalidate();base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { hover=false;pressed=false;Invalidate();base.OnMouseLeave(e); }
        protected override void OnMouseDown(MouseEventArgs e) { pressed=true;Invalidate();base.OnMouseDown(e); }
        protected override void OnMouseUp(MouseEventArgs e) { pressed=false;Invalidate();base.OnMouseUp(e); }
        protected override void OnEnabledChanged(EventArgs e) { Invalidate();base.OnEnabledChanged(e); }
        protected override void OnClick(EventArgs e) { if(Enabled)MenuAudio.Play();base.OnClick(e); }
    }
    internal sealed class BlueCheckBox : CheckBox {
        internal BlueCheckBox() { SetStyle(ControlStyles.UserPaint|ControlStyles.AllPaintingInWmPaint|ControlStyles.OptimizedDoubleBuffer,true); }
        protected override void OnPaint(PaintEventArgs e) {
            e.Graphics.Clear(BlueTheme.Panel);
            var box = new Rectangle(1,(Height-14)/2,14,14);
            using(var brush=new SolidBrush(BlueTheme.Input)) e.Graphics.FillRectangle(brush,box);
            using(var pen=new Pen(Enabled ? BlueTheme.Border : BlueTheme.Muted)) e.Graphics.DrawRectangle(pen,box);
            if(Checked) using(var pen=new Pen(Enabled ? Color.White : BlueTheme.Muted,2))
                e.Graphics.DrawLines(pen,new[]{new Point(box.Left+3,box.Top+7),new Point(box.Left+6,box.Top+10),new Point(box.Left+12,box.Top+3)});
            var text=new Rectangle(23,0,Width-23,Height);
            TextRenderer.DrawText(e.Graphics,Text,Font,text,Enabled ? BlueTheme.Text : BlueTheme.Muted,TextFormatFlags.Left|TextFormatFlags.VerticalCenter);
            if(Focused && ShowFocusCues) ControlPaint.DrawFocusRectangle(e.Graphics,text,Color.White,BlueTheme.Panel);
        }
        protected override void OnCheckedChanged(EventArgs e) { base.OnCheckedChanged(e);Invalidate(); }
        protected override void OnEnabledChanged(EventArgs e) { base.OnEnabledChanged(e);Invalidate(); }
    }
}
