using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Container
{
    public class UnityManager
    {
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool SendMessageCallback(IntPtr hWnd, uint Msg, UIntPtr wParam,
            IntPtr lParam, SendMessageDelegate lpCallBack, UIntPtr dwData);

        internal delegate int WindowEnumProc(IntPtr hwnd, IntPtr lparam);
        [DllImport("user32.dll")]
        internal static extern bool EnumChildWindows(IntPtr hwnd, WindowEnumProc func, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        private const int WM_ACTIVATE = 0x0006;
        private readonly IntPtr WA_ACTIVE = new IntPtr(1);
        private readonly IntPtr WA_INACTIVE = new IntPtr(0);

        delegate void SendMessageDelegate(IntPtr hWnd, uint uMsg, UIntPtr dwData, IntPtr lResult);

        List<UnityInstance> unityInstances;

        public UnityManager()
        {
            unityInstances = new List<UnityInstance>();
        }

        private void ActivateUnityWindow(UnityInstance ui)
        {
            SendMessage(ui.handle, WM_ACTIVATE, WA_ACTIVE, IntPtr.Zero);
        }

        private void DeactivateUnityWindow(UnityInstance ui)
        {
            SendMessage(ui.handle, WM_ACTIVATE, WA_INACTIVE, IntPtr.Zero);
        }

        public int EnumerateStoredWindows()
        {
            foreach (UnityInstance ui in unityInstances)
            {
                ActivateUnityWindow(ui);
            }
            return 0;
        }



        public UnityInstance RunUnityBuild(String path, System.Windows.Forms.Panel desiredPanel )
        {
            IntPtr unityHWND = IntPtr.Zero;
            Process processToUse;
            processToUse = new Process();
            try
            {
                processToUse.StartInfo.FileName = path + "\\test.exe";
                processToUse.StartInfo.Arguments = "-parentHWND " + desiredPanel.Handle.ToInt32() + " " + Environment.CommandLine;
                processToUse.StartInfo.UseShellExecute = true;
                processToUse.StartInfo.CreateNoWindow = true;

                processToUse.Start();

                processToUse.WaitForInputIdle();
                // Doesn't work for some reason ?!
                unityHWND = processToUse.MainWindowHandle;
                //EnumChildWindows(desiredPanel.Handle, WindowEnum, IntPtr.Zero);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message + ".\nCheck that test.exe was built to root location for container");
            }

            UnityInstance tracker;
            tracker.process = processToUse;
            tracker.handle = unityHWND;

            unityInstances.Add(tracker);

            return tracker;
        }

        public void KillUnityInstances()
        {
            foreach(UnityInstance ui in unityInstances)
            {
                ui.process.CloseMainWindow();

                Thread.Sleep(1000);
                while(ui.process.HasExited == false)
                {
                    ui.process.Kill();
                }
            }
        }

        public struct UnityInstance
        {
            public Process process;
            public IntPtr handle;
        }
    }
}
