
using ScriptPlayer.Shared;
using ScriptPlayer.Shared.Scripts;
using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Microsoft.Win32;
using System.Diagnostics;
using WindowsInput;
using WindowsInput.Native;
using AOB_Scanner;

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

}
