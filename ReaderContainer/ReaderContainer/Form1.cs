using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Windows.Forms.VisualStyles;

namespace ReaderContainer
{
    public partial class Form1 : Form
    {
        [DllImport("User32.dll")]
        static extern bool MoveWindow(IntPtr handle, int x, int y, int width, int height, bool redraw);

        internal delegate int WindowEnumProc(IntPtr hwnd, IntPtr lparam);
        [DllImport("user32.dll")]
        internal static extern bool EnumChildWindows(IntPtr hwnd, WindowEnumProc func, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        private Process process;
        private IntPtr unityHWND = IntPtr.Zero;

        private const int WM_ACTIVATE = 0x0006;
        private readonly IntPtr WA_ACTIVE = new IntPtr(1);
        private readonly IntPtr WA_INACTIVE = new IntPtr(0);


        ReaderSetting readerSetting = new ReaderSetting();
        //string file = "App"+ "\\ReaderSetting.xml";
        string file = "ReaderSetting.xml";
        public void Setting()
        {
            this.Size = new Size(readerSetting.Width, readerSetting.Height);
            if (readerSetting.Borderless)
                this.FormBorderStyle = FormBorderStyle.None;
            else
                this.FormBorderStyle = FormBorderStyle.Sizable;

            this.Location = new Point(readerSetting.Left, readerSetting.Top);
        }
        public Form1()
        {
            readerSetting = ReaderSetting.Load(file);

            InitializeComponent();

            Setting();

            
            Console.WriteLine(readerSetting.Width);

            try
            {
                process = new Process();
                process.StartInfo.FileName = "App"+"\\ScreenReader.exe";
                process.StartInfo.Arguments = "-parentHWND " + panel1.Handle.ToInt32() + " " + Environment.CommandLine;
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.CreateNoWindow = true;

                process.Start();

                process.WaitForInputIdle();
                EnumChildWindows(panel1.Handle, WindowEnum, IntPtr.Zero);

                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ".\nCheck if ScreenReader.exe is placed next to ScreenReader.exe.");
            }
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
        }
        private int WindowEnum(IntPtr hwnd, IntPtr lparam)
        {
            unityHWND = hwnd;
            ActivateUnityWindow();
            return 0;
        }
        private void ActivateUnityWindow()
        {
            SendMessage(unityHWND, WM_ACTIVATE, WA_ACTIVE, IntPtr.Zero);
        }

        private void DeactivateUnityWindow()
        {
            SendMessage(unityHWND, WM_ACTIVATE, WA_INACTIVE, IntPtr.Zero);
        }
  

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                process.CloseMainWindow();
                
                Thread.Sleep(1000);
                while (process.HasExited == false)
                    process.Kill();
            }
            catch (Exception)
            {
                
            }
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            //ActivateUnityWindow();
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            //DeactivateUnityWindow();
        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            MoveWindow(unityHWND, 0, 0, panel1.Width, panel1.Height, true);
            //ActivateUnityWindow();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            panel1.Size = this.Size;
        }
        void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S)
            {
                readerSetting.Height = this.Height;
                readerSetting.Width = this.Width;
                readerSetting.Top = this.Top;
                readerSetting.Left = this.Left;

                readerSetting.Save(file);
                try
                {
                    string soundpath = Application.StartupPath + "/Sounds/Complete.wav";
                    //MessageBox.Show(soundpath);
                    System.Media.SoundPlayer player = new System.Media.SoundPlayer(soundpath);
                    player.Play();
                }catch(Exception ex)
                {

                }

                //MessageBox.Show("Save Settings");
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.B)
            {
                readerSetting.Borderless = !readerSetting.Borderless;
                if (readerSetting.Borderless)
                    this.FormBorderStyle = FormBorderStyle.None;
                else
                    this.FormBorderStyle = FormBorderStyle.Sizable;

                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                Application.Exit();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.R)
            {
                readerSetting = ReaderSetting.Load(file);
                Setting();
                e.Handled = true;
            }
        }
    }
}
