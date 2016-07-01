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

namespace Container
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
        //these handles are assigned to programatically, i should fix this at some point
        private UnityManager unityManager;
        private IntPtr unityHWND = IntPtr.Zero;

        private const int WM_ACTIVATE = 0x0006;
        private readonly IntPtr WA_ACTIVE = new IntPtr(1);
        private readonly IntPtr WA_INACTIVE = new IntPtr(0);

        public Form1()
        {
            InitializeComponent();
        }

        private void ActivateUnityWindow()
        {
            SendMessage(unityHWND, WM_ACTIVATE, WA_ACTIVE, IntPtr.Zero);
        }

        private void DeactivateUnityWindow()
        {
            SendMessage(unityHWND, WM_ACTIVATE, WA_INACTIVE, IntPtr.Zero);
        }

        //private int WindowEnum(IntPtr hwnd, IntPtr lparam)
        //{
        //    unityHWND = hwnd;
        //    ActivateUnityWindow();
        //    return 0;
        //}

        private void panel1_Resize(object sender, EventArgs e)
        {
            MoveWindow(unityHWND, 0, 0, panel1.Width, panel1.Height, true);
            ActivateUnityWindow();
        }

        // Close Unity application
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                process.CloseMainWindow();

                Thread.Sleep(1000);
                while (process.HasExited == false)
                    process.Kill();

                unityManager.KillUnityInstances();
            }
            catch (Exception)
            {

            }
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            ActivateUnityWindow();
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            DeactivateUnityWindow();
        }

        private void DeployTestProject(String path)
        {
            try
            {
                process = new Process();
                process.StartInfo.FileName = "C:\\Program Files\\Unity\\Editor\\Unity.exe";
                process.StartInfo.Arguments = "-quit -projectPath " + path + " -importPackage " + Application.StartupPath + "\\SpiderPackage.unitypackage" + " " + Environment.CommandLine;
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.CreateNoWindow = true;

                process.Start();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ".\nProject deployed incorrectly, check folder is a unity project.");
            }
        }

        private void BuildUnityProject(String path)
        {
            try
            {
                process = new Process();
                process.StartInfo.FileName = "C:\\Program Files\\Unity\\Editor\\Unity.exe";
                process.StartInfo.Arguments = "-quit -projectPath " + path + " -buildWindowsPlayer" + " " + path + "\\test.exe" + " -executeMethod ProgramEntryPoint.SetSpiderSceneUp" + " -importPackage "+ Application.StartupPath + "\\SpiderPackage.unitypackage" + " " + Environment.CommandLine;
                process.StartInfo.UseShellExecute = true;

                process.Start();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ".\nCheck if Container.exe is placed next to Child.exe.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBox1.Text = folderBrowserDialog1.SelectedPath;
            DeployTestProject(folderBrowserDialog1.SelectedPath);
            BuildUnityProject(folderBrowserDialog1.SelectedPath);

            unityManager = new UnityManager();

            unityManager.RunUnityBuild(folderBrowserDialog1.SelectedPath, panel2);
            unityManager.RunUnityBuild(folderBrowserDialog1.SelectedPath, panel3);
            unityManager.RunUnityBuild(folderBrowserDialog1.SelectedPath, panel4);
            unityManager.RunUnityBuild(folderBrowserDialog1.SelectedPath, panel5);
        }
    }
}
