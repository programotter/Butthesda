using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Butthesda
{


	public class Memory_Scanner
    {
        public event EventHandler Notification_Message;
        public event EventHandler Error_Message;
        public event EventHandler Debug_Message;
        public event EventHandler Warning_Message;
        
        public event EventHandler<StringArg> AnimationEvent;
        public event EventHandler AnimationTimeResetted;
        public event EventHandler AnimationTimeUpdated;

        public event EventHandler GamePaused;
        public event EventHandler GameResumed;

        public event EventHandler GameOpened;
        public event EventHandler GameClosed;

        readonly Memory memory;
        readonly string Game_Name;

        private int[] timer_offsets;
        IntPtr ptr_data;

        private Thread Thread_Check_Events;
        private Thread Thread_Check_Timer;
        private Thread Thread_Check_Game_Running;

        public Memory_Scanner(string game)
        {
            Game_Name = game;
            memory = new Memory(Game_Name);
        }

        public bool Init()
		{
            if (Game_Name == Game.Skyrim.Executable_Name)
            {
                timer_offsets = new int[] { 0xF10588, 0x88, 0x4, 0x100, 0x10, 0x98, 0x58, 0x0, 0x44 };
                ptr_data = Inject_TESV();
            }
            else if (Game_Name == Game.SkyrimSe.Executable_Name)
            {
                timer_offsets = new int[] { 0x01EC47C8, 0xD0, 0x8, 0x1B0, 0x20, 0x118, 0x98, 0x0, 0x44 };
                ptr_data = Inject_SkyrimSE();
            }

            if (ptr_data == IntPtr.Zero)
            {
                Warning_Message?.Invoke(this, new StringArg("No pointer was found!"));
                return false;//return failure
            }

            Debug_Message?.Invoke(this, new StringArg(String.Format("Found data address: 0x{0:X}",ptr_data.ToInt64())));
            
            IntPtr test1 = memory.ReadPointer(timer_offsets);
            Debug_Message?.Invoke(this, new StringArg(String.Format("Animation timer address is currenty: 0x{0:X}", test1.ToInt64())));

            float test2 = memory.ReadFloat(test1);
            Debug_Message?.Invoke(this, new StringArg("animation timer is:"+ test2));

            Thread_Check_Events = new Thread(Check_Events) { IsBackground = true };
            Thread_Check_Events.Start();

            Thread_Check_Timer = new Thread(Check_Timer) { IsBackground = true };
            Thread_Check_Timer.Start();

            Thread_Check_Game_Running = new Thread(Check_Game_Running) { IsBackground = true };
            Thread_Check_Game_Running.Start();
            return true;//return succes
        }


        public void Close()
        {
            if(Thread_Check_Events!= null) try { Thread_Check_Events.Abort(); } catch { };
            if (Thread_Check_Timer != null) try { Thread_Check_Timer.Abort(); } catch { };
            if (Thread_Check_Game_Running != null) try { Thread_Check_Game_Running.Abort(); } catch { };
        }



        void Check_Game_Running()
        {
            Thread.CurrentThread.Name = "Memory_Thread_Check_Game_Running";
            bool was_running = true;
            while (true)
            {
                Thread.Sleep(1000);
                Process[] pname = Process.GetProcessesByName(Game_Name);
                if ((pname.Length != 0) != (was_running))
                {
                    was_running = !was_running;

                    if (was_running)
                    {
                        Notification_Message?.Invoke(this, new StringArg("Game was opened"));
                        GameOpened?.Invoke(this, new EventArgs());
                    }
                    else
                    {
                        Notification_Message?.Invoke(this, new StringArg("Game was closed"));
                        GameClosed?.Invoke(this, new EventArgs());
                    }
                }
            }
        }


        private class AnimationItem
        {
            public string name;
            public int amount;
            public AnimationItem(string name,int amount)
            {
                this.name = name;
                this.amount = amount;
            }
        }


        private void Check_Events()
        {
            Thread.CurrentThread.Name = "Memory_Thread_Check_Events";
            List<AnimationItem> AnimationList = new List<AnimationItem>();
            bool First_Check = true;
            while (true)
            {
                IntPtr ptr = ptr_data;
                Thread.Sleep(50);

                //check if events have played
                while (true)
                {
                    IntPtr temp_debugging = ptr;
                    IntPtr name_address = memory.ReadPointer(ptr);
                    if (name_address == IntPtr.Zero) break;
                    
                    string name = memory.ReadString(name_address, 30);
                    int amount = memory.ReadInt32(ptr + 0x8);

                    bool found = false;
                    foreach (AnimationItem a in AnimationList)
                    {
                        if (a.name == name)
                        {
                            found = true;
                            if (a.amount != amount)
                            {
                                a.amount = amount;//update to new amount
                                if (!First_Check)
                                {
                                    Notification_Message?.Invoke(this, new StringArg(String.Format("Animation playing name: {0}",name)));
                                    Debug_Message?.Invoke(this, new StringArg(String.Format("Animation playing counter: {0}, address: 0x{1:X}, nameAddress: 0x{2:X}", amount, temp_debugging.ToInt64(), name_address.ToInt64())));
                                    AnimationEvent?.Invoke(this, new StringArg(name));
                                }
                            }
                                    
                            break;
                        }
                    }
                    if (!found)
                    {
                        AnimationList.Add(new AnimationItem(name, amount));
                        if (!First_Check)
                        {
                            Notification_Message?.Invoke(this, new StringArg(String.Format("Animation playing name: {0}", name)));
                            Debug_Message?.Invoke(this, new StringArg(String.Format("Animation playing counter: {0}, address: 0x{1:X}, nameAddress: 0x{2:X}", amount, temp_debugging.ToInt64(), name_address.ToInt64())));
                            AnimationEvent?.Invoke(this, new StringArg(name));
                        }
                    }
                            

                    ptr += 0x10;

                }
                
                First_Check = false;

            }
        }

        private void Check_Timer()
        {
            Thread.CurrentThread.Name = "Memory_Thread_Check_Timer";
            float old_timer = 0;

            DateTime old_time = DateTime.Now;
            bool gamePaused = false;
            while (true)
            {
                Thread.Sleep(10);

                //get animation timer
                float timer = memory.ReadFloat(timer_offsets);
                DateTime time = DateTime.Now;

                if (timer < old_timer)
                {
                    AnimationTimeResetted?.Invoke(this, EventArgs.Empty);
                }
                if (timer != old_timer)
                {
                    AnimationTimeUpdated?.Invoke(this, new FloatArg(timer));
                }
                if (timer == old_timer)
                {
                    if (!gamePaused)
                    {
                        if (time > old_time + new TimeSpan(0, 0, 0, 0, 150))
                        {
                            gamePaused = true;
                            Notification_Message?.Invoke(this, new StringArg("Game paused"));
                            GamePaused?.Invoke(this, EventArgs.Empty);
                        }
                    }
                }
                else
                {
                    if (gamePaused)
                    {
                        gamePaused = false;
                        Notification_Message?.Invoke(this, new StringArg("Game resumed"));
                        GameResumed?.Invoke(this, EventArgs.Empty);
                    }
                    old_time = time;
                }

                old_timer = timer;
            }
        }


        private IntPtr Inject_TESV()
        {
            int len = 5;
            int data_offset = 0x100;
            byte[] bytes_program = { 0x8B, 0xF0, 0x50, 0xB8, 0xA6, 0x00, 0xF1, 0x01, 0x83, 0xC0, 0x50, 0x89, 0x08, 0x8B, 0xC8, 0x83, 0xC1, 0x08, 0x58, 0x89, 0x01, 0x83, 0xC1, 0x08, 0x89, 0x11, 0x83, 0xC1, 0x08, 0x58, 0x89, 0x01, 0x83, 0xC1, 0x08, 0x58, 0x89, 0x01, 0x58, 0x8B, 0xD0, 0x50, 0x8B, 0x01, 0x50, 0x83, 0xE9, 0x08, 0x8B, 0x01, 0x50, 0x83, 0xE9, 0x08, 0xFF, 0x31, 0x83, 0xE9, 0x08, 0xFF, 0x31, 0x83, 0xE9, 0x08, 0xFF, 0x31, 0xB8, 0x00, 0x00, 0x40, 0x00, 0x05, 0x3C, 0x06, 0xF1, 0x00, 0x8B, 0x00, 0x83, 0xC0, 0x74, 0x8B, 0x00, 0x83, 0xC0, 0x04, 0x8B, 0x00, 0x05, 0x00, 0x01, 0x00, 0x00, 0x8B, 0x00, 0x83, 0xC0, 0x10, 0x8B, 0x00, 0x83, 0xC0, 0x38, 0x39, 0xC2, 0x0F, 0x85, 0x2C, 0x00, 0x00, 0x00, 0xB8, 0xA6, 0x00, 0xF1, 0x01, 0x05, 0xF0, 0x00, 0x00, 0x00, 0x83, 0xC0, 0x10, 0x39, 0x38, 0x0F, 0x84, 0x0D, 0x00, 0x00, 0x00, 0x83, 0x38, 0x00, 0x0F, 0x84, 0x02, 0x00, 0x00, 0x00, 0xEB, 0xEA, 0x89, 0x38, 0x83, 0xC0, 0x08, 0x8B, 0x18, 0x83, 0xC3, 0x01, 0x89, 0x18, 0x5A, 0x58, 0x58, 0x83, 0xFE, 0x04, 0xE9, 0x00, 0x00, 0x00, 0x00 };

            AOB_Scanner.AOB_Scanner aob_scanner = new AOB_Scanner.AOB_Scanner(memory.process, memory.ProcessHandle, "8B 44 24 04 81 EC 08 01 00 00 53 56 57 8B 38 8B C7 32 DB 8D 50 01 8A 08 40 84 C9 75 F9 2B C2");
            aob_scanner.setModule(memory.process.MainModule);

            IntPtr ptr_inject = (IntPtr)aob_scanner.FindPattern();
            if (ptr_inject == IntPtr.Zero)
            {
                Error_Message?.Invoke(this, new StringArg("Could not inject, did the game load past the main menu?"));
                return IntPtr.Zero;
            }

            ptr_inject += 0x1f;

            //check if we already injected code
            byte b = memory.ReadByte(ptr_inject);
            if (b == 0xE9)
            {
                Notification_Message?.Invoke(this, new StringArg("Skipping injection (already injected)"));
                return (IntPtr)((long)memory.ReadInt32(ptr_inject + 0x1) + (long)ptr_inject) + 5 + bytes_program.Length + data_offset;
            }


            IntPtr ptr_functon = memory.AllocateMemory(10000);

            IntPtr ptr_data = ptr_functon + 0xA6;

            byte[] bytes = new byte[10000];

            //write program
            Array.Copy(bytes_program, bytes, bytes_program.Length);

            //some parts in the program are static addresses that need to be overwriten
            byte[] bytes_ptr_data = BitConverter.GetBytes((ulong)ptr_data);
            for (int i = 0; i < 4; i++)
            {
                bytes[4 + i] = bytes_ptr_data[i];
            }
            for (int i = 0; i < 4; i++)
            {
                bytes[112 + i] = bytes_ptr_data[i];
            }

            byte[] bytes_ptr_return = BitConverter.GetBytes((ulong)ptr_inject - (ulong)bytes_program.Length + (ulong)len - (ulong)ptr_functon);
            for (int i = 0; i < 4; i++)
            {
                bytes[162 + i] = bytes_ptr_return[i];
            }

            memory.WriteBytes(ptr_functon, bytes);

            memory.Hook(ptr_inject, ptr_functon, len);
            return ptr_data + 0x100;
        }


        private IntPtr Inject_SkyrimSE()
        {

            int len = 6;
            int data_offset = 0x100;
            byte[] bytes_program = { 0x48, 0x8B, 0xF2, 0x48, 0x8B, 0x39, 0x51, 0x50, 0x48, 0xB8, 0x00, 0x00, 0xEC, 0x4E, 0xF7, 0x7F, 0x00, 0x00, 0x48, 0x05, 0xD8, 0xF7, 0xEF, 0x02, 0x48, 0x8B, 0x00, 0x48, 0x05, 0xF0, 0x00, 0x00, 0x00, 0x48, 0x8B, 0x00, 0x48, 0x83, 0xC0, 0x08, 0x48, 0x8B, 0x00, 0x48, 0x05, 0xA8, 0x01, 0x00, 0x00, 0x48, 0x8B, 0x00, 0x48, 0x05, 0x90, 0x00, 0x00, 0x00, 0x48, 0x8B, 0x00, 0x48, 0x83, 0xC0, 0x68, 0x4C, 0x39, 0xF0, 0x75, 0x3D, 0x90, 0x90, 0x90, 0x90, 0x48, 0xB8, 0x93, 0x00, 0xEB, 0x4E, 0xF7, 0x7F, 0x00, 0x00, 0x48, 0x05, 0xF0, 0x00, 0x00, 0x00, 0x48, 0x83, 0xC0, 0x10, 0x48, 0x39, 0x38, 0x74, 0x12, 0x90, 0x90, 0x90, 0x90, 0x83, 0x38, 0x00, 0x74, 0x06, 0x90, 0x90, 0x90, 0x90, 0xEB, 0xE8, 0x48, 0x89, 0x38, 0x48, 0x83, 0xC0, 0x08, 0x48, 0x8B, 0x08, 0x48, 0x83, 0xC1, 0x01, 0x48, 0x89, 0x08, 0x58, 0x59, 0xE9, 0x28, 0x1C, 0x1B, 0x00, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 };

            AOB_Scanner.AOB_Scanner aob_scanner = new AOB_Scanner.AOB_Scanner(memory.process, memory.ProcessHandle, "48 8B C4 57 48 81 EC 40 01 00 00 48 C7 44 24 20 FE FF FF FF 48 89 58 10 48 89 70 18");
            aob_scanner.setModule(memory.process.MainModule);

            IntPtr ptr_inject = (IntPtr)aob_scanner.FindPattern();
            if (ptr_inject == IntPtr.Zero)
            {
                Notification_Message?.Invoke(this, new StringArg("Could not inject, did the game load past the main menu?"));
                return IntPtr.Zero;
            }

            ptr_inject += 0x1C;

            //check if we already injected code
            byte b = memory.ReadByte(ptr_inject);
            if (b == 0xE9)
            {
                Notification_Message?.Invoke(this, new StringArg("Skipping injection (already injected)"));
                return (IntPtr)((long)memory.ReadInt32(ptr_inject + 0x1) + (long)ptr_inject) + 5 + bytes_program.Length + data_offset;
            }

            IntPtr ptr_functon = memory.AllocateMemory(10000, ptr_inject);
            IntPtr ptr_data = ptr_functon + bytes_program.Length;

            byte[] bytes = new byte[10000];

            //write program
            Array.Copy(bytes_program, bytes, bytes_program.Length);

            //some parts in the program are static addresses that need to be overwriten
            byte[] bytes_ptr_baseAddress = BitConverter.GetBytes((ulong)memory.process.MainModule.BaseAddress);
            for (int i = 0; i < 8; i++)
            {
                bytes[0x0A + i] = bytes_ptr_baseAddress[i];
            }

            byte[] bytes_ptr_data = BitConverter.GetBytes((ulong)ptr_data);
            for (int i = 0; i < 8; i++)
            {
                bytes[0x4C + i] = bytes_ptr_data[i];
            }

            byte[] bytes_ptr_return = BitConverter.GetBytes((ulong)ptr_inject + (ulong)len - (ulong)ptr_functon - (ulong)0x8A);
            for (int i = 0; i < 4; i++)
            {
                bytes[0x86 + i] = bytes_ptr_return[i];
            }

            memory.WriteBytes(ptr_functon, bytes);

            memory.Hook(ptr_inject, ptr_functon, len, true);
            return ptr_data + data_offset;
        }
    }

}
