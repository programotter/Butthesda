using System.Diagnostics;
using System.Security.Principal;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;

namespace Butthesda
{
    public sealed class Game
    {
        static List<Game> game_list = new List<Game>();
        public static readonly Game Skyrim = new Game("TESV");
        public static readonly Game SkyrimSe = new Game("SkyrimSe");
        public static readonly Game SkyrimVr = new Game("SkyrimVR");
        public static readonly Game Fallout4 = new Game("Fallout 4");

        private Game(string value)
        {
            Executable_Name = value;
            Game.game_list.Add(this);
        }



        static public List<Game> List()
        {
            return game_list;
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

        public bool IsGameRunningAsAdmin()
        {
            return ProcessHelper.IsProcessOwnerAdmin(this.Executable_Name);
        }

        public string Executable_Name { get; private set; }

        public string Install_Dir()
		{
            string keyName = "";

            if (this == Game.Skyrim)
            {
                keyName = @"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Bethesda Softworks\Skyrim";

            }
            else if (this == Game.SkyrimSe)
            {
                keyName = @"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Bethesda Softworks\Skyrim Special Edition";
            }
            else if (this == Game.SkyrimVr)
            {
                keyName = @"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Bethesda Softworks\Skyrim VR";
            }
            else if (this == Game.Fallout4)
            {
                keyName = @"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Bethesda Softworks\Fallout4";
            }

            return (string)Registry.GetValue(keyName, "Installed Path", "");
        }


    }
}


public class ProcessHelper
{
    [DllImport("advapi32.dll", SetLastError = true)]
    private static extern bool OpenProcessToken(IntPtr ProcessHandle, UInt32 DesiredAccess, out IntPtr TokenHandle);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool CloseHandle(IntPtr hObject);

    private const int STANDARD_RIGHTS_REQUIRED = 0xF0000;
    private const int TOKEN_ASSIGN_PRIMARY = 0x1;
    private const int TOKEN_DUPLICATE = 0x2;
    private const int TOKEN_IMPERSONATE = 0x4;
    private const int TOKEN_QUERY = 0x8;
    private const int TOKEN_QUERY_SOURCE = 0x10;
    private const int TOKEN_ADJUST_GROUPS = 0x40;
    private const int TOKEN_ADJUST_PRIVILEGES = 0x20;
    private const int TOKEN_ADJUST_SESSIONID = 0x100;
    private const int TOKEN_ADJUST_DEFAULT = 0x80;
    private const int TOKEN_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED | TOKEN_ASSIGN_PRIMARY | TOKEN_DUPLICATE | TOKEN_IMPERSONATE | TOKEN_QUERY | TOKEN_QUERY_SOURCE | TOKEN_ADJUST_PRIVILEGES | TOKEN_ADJUST_GROUPS | TOKEN_ADJUST_SESSIONID | TOKEN_ADJUST_DEFAULT);

    public static bool IsProcessOwnerAdmin(string processName)
    {
        Process proc = Process.GetProcessesByName(processName)[0];

        IntPtr ph = IntPtr.Zero;

        
        try
        {
            OpenProcessToken(proc.Handle, TOKEN_ALL_ACCESS, out ph);
            WindowsIdentity iden = new WindowsIdentity(ph);
            bool result = false;

            foreach (IdentityReference role in iden.Groups)
            {
                if (role.IsValidTargetType(typeof(SecurityIdentifier)))
                {
                    SecurityIdentifier sid = role as SecurityIdentifier;

                    if (sid.IsWellKnown(WellKnownSidType.AccountAdministratorSid) || sid.IsWellKnown(WellKnownSidType.BuiltinAdministratorsSid))
                    {
                        result = true;
                        break;
                    }
                }
            }

            CloseHandle(ph);

            return result;
        }
		catch {
            return true;
        };

    }
}
