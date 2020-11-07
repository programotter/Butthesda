using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Butthesda
{
    public sealed class Games
    {
        public static readonly Games Skyrim = new Games("TESV");
        public static readonly Games SkyrimSe = new Games("SkyrimSe");
        public static readonly Games Fallout4 = new Games("Fallout 4");

        private Games(string value)
        {
            Executable_Name = value;
        }

        public static string[] List()
        {
            return new string[] { Skyrim.Executable_Name, SkyrimSe.Executable_Name, Fallout4.Executable_Name};
        }

        public bool Running()
        {
            Process[] pname = Process.GetProcessesByName(Executable_Name);
            return pname.Length != 0;
        }

        public static bool Running(string Executable_Name)
        {
            Process[] pname = Process.GetProcessesByName(Executable_Name);
            return pname.Length != 0;
        }

        public string Executable_Name { get; private set; }
    }
}
