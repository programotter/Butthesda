using System;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Windows.Forms;

namespace Butthesda
{
    static class Program
    {

        [STAThread]
        static void Main()
        {
            start_time = DateTime.Now;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new Form_Main());
        }

        public static DateTime start_time;


        public static bool IsRunningAsAdmin()
		{
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public static void RestartAsAdmin()
		{
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = Path.Combine
            (Path.GetDirectoryName(Application.ExecutablePath), "Butthesda.exe"); // replace with your filename
            startInfo.Arguments =
            string.Empty; // if you need to pass any command line arguments to your stub, enter them here
            startInfo.UseShellExecute = true;
            startInfo.Verb = "runas";

            Process.Start(startInfo);

            if (Application.MessageLoop)
            {
                Application.Exit();
            }
            else
            {
                Environment.Exit(1);
            }
        }
    }





    public class StringArg : EventArgs
    {
        public string String { get; private set; }
        public StringArg(string message)
        {
            String = message;
        }
    }

    public class StringArgs : EventArgs
    {
        public string[] StringArray { get; private set; }
        public StringArgs(string[] message)
        {
            StringArray = message;
        }
    }

    public class BoolArg : EventArgs
    {
        public bool Bool { get; private set; }
        public BoolArg(bool message)
        {
            Bool = message;
        }
    }

    public class IntArg : EventArgs
    {
        public int Int { get; private set; }
        public IntArg(int message)
        {
            Int = message;
        }
    }

    public class FloatArg : EventArgs
    {
        public float Float { get; private set; }
        public FloatArg(float message)
        {
            Float = message;
        }
    }

}