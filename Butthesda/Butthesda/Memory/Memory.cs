using System;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Butthesda
{
    enum AllocationType
    {
        Commit = 0x1000,
        Reserve = 0x2000,
        Decommit = 0x4000,
        Release = 0x8000,
        Reset = 0x80000,
        Physical = 0x400000,
        TopDown = 0x100000,
        WriteWatch = 0x200000,
        LargePages = 0x20000000,
        MEM_TOP_DOWN = 0x00100000
}

    enum MemoryProtection
    {
        Execute = 0x10,
        ExecuteRead = 0x20,
        ExecuteReadWrite = 0x40,
        ExecuteWriteCopy = 0x80,
        NoAccess = 0x01,
        ReadOnly = 0x02,
        ReadWrite = 0x04,
        WriteCopy = 0x08,
        GuardModifierflag = 0x100,
        NoCacheModifierflag = 0x200,
        WriteCombineModifierflag = 0x400
    }

    class MemoryApi
    {
        public const int PROCESS_VM_READ = (0x0010);
        public const int PROCESS_VM_WRITE = (0x0020);
        public const int PROCESS_VM_OPERATION = (0x0008);
        public const int PAGE_READWRITE = 0x0004;
        public const int PAGE_READONLY =  0x02;
        public const int MEM_FREE = 0x10000;
        

        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_BASIC_INFORMATION
        {
            public IntPtr BaseAddress;
            public IntPtr AllocationBase;
            public uint AllocationProtect;
            public IntPtr RegionSize;
            public uint State;
            public uint Protect;
            public uint Type;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_BASIC_INFORMATION64
        {
            public ulong BaseAddress;
            public ulong AllocationBase;
            public int AllocationProtect;
            public int __alignment1;
            public ulong RegionSize;
            public int State;
            public int Protect;
            public int Type;
            public int __alignment2;
        }

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwAccess, bool inherit, int pid);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr handle);

        [DllImport("kernel32.dll")]
        public static extern bool IsWow64Process([In] IntPtr process, [Out] out bool wow64Process);


        [DllImport("kernel32.dll")]
        public static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);
        [DllImport("kernel32.dll")]
        public static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION64 lpBuffer, uint dwLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] lpBuffer, int dwSize, out IntPtr lpNumberOfBytesRead);



        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] lpBuffer, int dwSize, out IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, int dwSize, AllocationType flAllocationType, MemoryProtection flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, int dwSize, int flNewProtect, out int lpflOldProtect);


    }

    public class Memory
    {
        public IntPtr ProcessHandle;
        public string ProcessName;
        public int ProcessID;
        public IntPtr BaseModule;
        public Process process;
        public bool is64Bit;
        public bool attached;

        // Destruktor
        ~Memory() { if (ProcessHandle != IntPtr.Zero) MemoryApi.CloseHandle(ProcessHandle); }

        // Get Process for work
        public Memory(string _ProcessName)
        {
            Process[] Processes = Process.GetProcessesByName(_ProcessName);

            if (Processes.Length > 0)
            {
                process = Processes[0];
                BaseModule = process.MainModule.BaseAddress;
                ProcessID = process.Id;
                ProcessName = _ProcessName;

                is64Bit = _ProcessName != "TESV";


                ProcessHandle = MemoryApi.OpenProcess(MemoryApi.PROCESS_VM_READ | MemoryApi.PROCESS_VM_WRITE | MemoryApi.PROCESS_VM_OPERATION, false, ProcessID);

            }
        }

        // If Process attacked
        public bool IsOpen()
        {
            return ProcessName != string.Empty;
        }

        public void Hook(IntPtr toHook, IntPtr ourFunc, int len, bool _64bit = true)
        {
            
            int oldProtect;
            MemoryApi.VirtualProtectEx(ProcessHandle, toHook, len, MemoryApi.PAGE_READWRITE, out oldProtect);

            var emptyBytes = new byte[len];
            for (int i = 0; i < len; i++)
            {
                emptyBytes[i] = 0x90;
            }
            WriteBytes(toHook, emptyBytes);

            

            byte[] byte_jump_delta = BitConverter.GetBytes((ulong)ourFunc - (ulong)toHook - (ulong)5);
            byte[] managedArray = new byte[5];
            managedArray[0] = 0xE9;
            for (int i = 0; i < 4; i++)
            {
                managedArray[i + 1] = byte_jump_delta[i];
            }
        
            

            WriteBytes(toHook, managedArray);
            

            MemoryApi.VirtualProtectEx(ProcessHandle, toHook, len, oldProtect, out oldProtect);

        }
        public IntPtr AllocateMemory(int size, IntPtr nearThisAddr)
        {
            MemoryApi.MEMORY_BASIC_INFORMATION mbi;

            while (MemoryApi.VirtualQueryEx(ProcessHandle, nearThisAddr, out mbi, (uint)Marshal.SizeOf(typeof(MemoryApi.MEMORY_BASIC_INFORMATION))) != 0)
            {
                Console.WriteLine(mbi.BaseAddress.ToString("X"));
                if (mbi.State == MemoryApi.MEM_FREE)
                {
                    IntPtr addr = MemoryApi.VirtualAllocEx(ProcessHandle, mbi.BaseAddress, size, AllocationType.Commit | AllocationType.Reserve, MemoryProtection.ExecuteReadWrite);
                    if ((ulong)addr != 0) return addr;
                }
                nearThisAddr = (IntPtr)((ulong)nearThisAddr- (ulong)0x10000);
                
            }

            return new IntPtr();


        }

        public IntPtr AllocateMemory(int size)
        {
            return MemoryApi.VirtualAllocEx(ProcessHandle, new IntPtr(), size, AllocationType.Commit | AllocationType.Reserve, MemoryProtection.ExecuteReadWrite);
        }

        public byte[] ReadBytes(IntPtr _lpBaseAddress, int size)
        {
            byte[] Buffer = new byte[size];
            IntPtr ByteRead;

            //int oldProtect;
            //MemoryApi.VirtualProtectEx(ProcessHandle, _lpBaseAddress, size, MemoryApi.PAGE_READONLY, out oldProtect);

            MemoryApi.ReadProcessMemory(ProcessHandle, _lpBaseAddress, Buffer, size, out ByteRead);

            //int oldProtect2;
            //MemoryApi.VirtualProtectEx(ProcessHandle, _lpBaseAddress, size, oldProtect,out oldProtect2);

            return Buffer;
        }

        public byte ReadByte(IntPtr _lpBaseAddress)
        {
            byte[] Buffer = ReadBytes(_lpBaseAddress, 4);
            return (byte)BitConverter.ToChar(Buffer, 0);
        }


        //readPointer
        public IntPtr ReadPointer(IntPtr _lpBaseAddress)
        {
            if (is64Bit)
            {
                return (IntPtr)ReadInt64(_lpBaseAddress);
            }
            else
            {
                return (IntPtr)ReadInt32(_lpBaseAddress);
            }
        }

        public IntPtr ReadPointer(IntPtr _lpBaseAddress, int[] offsets)
        {
            int last_offset = offsets[offsets.Length-1];
            Array.Resize<int>(ref offsets, offsets.Length - 1);
            foreach (int offset in offsets)
            {
                _lpBaseAddress = ReadPointer(_lpBaseAddress + offset);
            }
            _lpBaseAddress += last_offset;

            return _lpBaseAddress;
        }

        public IntPtr ReadPointer(int[] offsets)
        {
            return ReadPointer(BaseModule, offsets);
        }

        // Read Int32
        public Int32 ReadInt32(IntPtr _lpBaseAddress)
        {
            byte[] Buffer = ReadBytes(_lpBaseAddress,4);
            return BitConverter.ToInt32(Buffer, 0);
        }

        public Int32 ReadInt32(int[] offsets)
        {
            return ReadInt32(BaseModule, offsets);
        }

        public Int32 ReadInt32(IntPtr _lpBaseAddress, int[] offsets)
        {
            return ReadInt32(ReadPointer(_lpBaseAddress, offsets));
        }


        // Read Int64
        public Int64 ReadInt64(IntPtr _lpBaseAddress)
        {
            byte[] Buffer = ReadBytes(_lpBaseAddress, 8);
            return BitConverter.ToInt64(Buffer, 0);
        }

        public Int64 ReadInt64(IntPtr _lpBaseAddress, int[] offsets )
        {
            return ReadInt64(ReadPointer(_lpBaseAddress, offsets));
        }

        public Int64 ReadInt64(int[] offsets)
        {
            return ReadInt64(BaseModule,offsets);
        }


        // Read float
        public float ReadFloat(IntPtr _lpBaseAddress)
        {
            byte[] Buffer = ReadBytes(_lpBaseAddress, sizeof(float));
            return BitConverter.ToSingle(Buffer, 0);
        }
        public float ReadFloat(IntPtr _lpBaseAddress, int[] offsets)
        {
            return ReadFloat(ReadPointer(_lpBaseAddress, offsets));
        }

        public float ReadFloat(int[] offsets)
        {
            return ReadFloat(BaseModule,offsets);
        }


        // Read String
        public string ReadString(IntPtr _lpBaseAddress, int _Size)
        {
            byte[] Buffer = ReadBytes(_lpBaseAddress, _Size);

            int length = 0;
            foreach (byte b in Buffer)
            {
                if (b == 0) break;
                length++;
            }
            return Encoding.UTF8.GetString(Buffer, 0, length);
        }




        public void WriteBytes(IntPtr MemoryAddress, byte[] Buffer)
        {
            int oldProtect;
            MemoryApi.VirtualProtectEx(ProcessHandle, (IntPtr)MemoryAddress, Buffer.Length, MemoryApi.PAGE_READWRITE, out oldProtect);

            IntPtr ptrBytesWritten;
            MemoryApi.WriteProcessMemory(ProcessHandle, MemoryAddress, Buffer, Buffer.Length, out ptrBytesWritten);

            int oldProtect2;
            MemoryApi.VirtualProtectEx(ProcessHandle, (IntPtr)MemoryAddress, Buffer.Length, oldProtect, out oldProtect2);
        }

        public void WriteInt32(IntPtr _lpBaseAddress, int _Value)
        {
            byte[] Buffer = BitConverter.GetBytes(_Value);
            WriteBytes(_lpBaseAddress, Buffer);
        }
    }

}