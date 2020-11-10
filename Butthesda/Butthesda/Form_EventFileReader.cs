using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Security.RightsManagement;
using System.Threading;
using System.Windows.Forms;
using static Butthesda.Program;

namespace Butthesda
{
    public partial class Form_EventFileReader : Form
    {
        Thread Thread_eventFileScanner;
        public bool RequestRestart = false;
        private readonly Memory_Scanner memory_scanner;
        private readonly VibrationEvents vibrationEvents;
        private readonly EventFileScanner eventFileScanner;
        private readonly string Game_Path;

        public Form_EventFileReader(string Game_Name, string Game_Path)
        {
            this.Game_Path = Game_Path;

            InitializeComponent();

            //load presets
            checkBox_ShowWarnings.Checked = Properties.Settings.Default.Show_warnings;
            checkBox_ShowErrors.Checked = Properties.Settings.Default.Show_errors;
            checkBox_ShowNotifications.Checked = Properties.Settings.Default.Show_notifications;
            checkBox_ShowDebug.Checked = Properties.Settings.Default.Show_debug_info;

            vibrationEvents = new VibrationEvents(Game_Path);
            memory_scanner = new Memory_Scanner(Game_Name);
            eventFileScanner = new EventFileScanner(Game_Path, memory_scanner, vibrationEvents);
        }

        public void Init()
		{


            vibrationEvents.Notification_Message += Notivication_Message;
            vibrationEvents.Warning_Message += Warning_Message;
            vibrationEvents.Error_Message += Error_Message;
            vibrationEvents.Debug_Message += Debug_Message;


            memory_scanner.Notification_Message += Notivication_Message;
            memory_scanner.Warning_Message += Warning_Message;
            memory_scanner.Error_Message += Error_Message;
            memory_scanner.Debug_Message += Debug_Message;
            memory_scanner.AnimationEvent += Memory_Scanner_AnimationEvent;
            memory_scanner.AnimationTimeResetted += Memory_Scanner_AnimationTimeResetted;
            memory_scanner.AnimationTimeUpdated += Memory_Scanner_AnimationTimeUpdated;
            memory_scanner.GamePaused += Memory_Scanner_GamePaused;
            memory_scanner.GameResumed += Memory_Scanner_GameResumed;
            memory_scanner.GameOpened += Memory_Scanner_GameOpened;
            memory_scanner.GameClosed += Memory_Scanner_GameClosed;


            eventFileScanner.Notification_Message += Notivication_Message;
            eventFileScanner.Error_Message += Error_Message;
            eventFileScanner.Debug_Message += Debug_Message;
            eventFileScanner.Warning_Message += Warning_Message;
            eventFileScanner.Save_Loaded += EventFileScanner_SaveLoaded;
            eventFileScanner.Arousal_Updated += EventFileScanner_ArousalUpdated;
            eventFileScanner.DD_Device_Update += EventFileScanner_DD_DeviceUpdate;




            foreach (Device d in Device.devices)
            {
                d.EventAdded += Event_Device_Added;
                d.EventRemoved += Event_Device_removed;
                d.EventsCleared += Event_Device_Cleared;
                d.SetMemoryEvents(memory_scanner);
            }


            Thread_eventFileScanner = new Thread(new ThreadStart(eventFileScanner.Run))
            {
                IsBackground = true
            };
            Thread_eventFileScanner.Start();


            //We need to start it after the constructor is done, because we need the event handlers setup first
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                vibrationEvents.Init();
                bool success = memory_scanner.Init();

                Thread.Sleep(5000);//we dont want to restart to fast after eachother
                if (!success)
                {
                    Warning_Message(this, new StringArg("Program will to inject in 5s"));
                    Request_Restart();
                }


            }).Start();
        }

        private void Request_Restart()
        {
            Thread_eventFileScanner.Abort();
            memory_scanner.Close();

            RequestRestart = true;
            Invoke((MethodInvoker)(() => this.Close()));
        }



        private void Memory_Scanner_GameClosed(object sender, EventArgs e)
        {
            File.WriteAllText(Game_Path + @"\FunScripts\link.txt", string.Empty);
            Invoke((MethodInvoker)(() => game_running = false));
            Update_Game_State();
        }

        
        private void Memory_Scanner_GameOpened(object sender, EventArgs e)
		{
            Request_Restart();
        }
        

        private void Notivication_Message(object sender, EventArgs e)
		{
            if (!checkBox_ShowNotifications.Checked) return;
            StringArg e2 = (StringArg)e;
            WriteTo_EventViewer(sender.GetType().Name+" - " + e2.String, Color.Black);
        }


        private void Error_Message(object sender, EventArgs e)
        {
            if (!checkBox_ShowErrors.Checked) return;
            StringArg e2 = (StringArg)e;            
            WriteTo_EventViewer(sender.GetType().Name + " - " + e2.String, Color.Red);
        }


        private void Debug_Message(object sender, EventArgs e)
        {
            if (!checkBox_ShowDebug.Checked) return;
            StringArg e2 = (StringArg)e;
            WriteTo_EventViewer(sender.GetType().Name + " - " + e2.String, Color.Gray);
        }


        private void Warning_Message(object sender, EventArgs e)
        {
            if (!checkBox_ShowWarnings.Checked) return;
            StringArg e2 = (StringArg)e;
            WriteTo_EventViewer(sender.GetType().Name + " - " + e2.String, Color.Orange);
        }



        private void EventFileScanner_DD_DeviceUpdate(object sender, EventArgs e)
        {
            StringArgs e2 = (StringArgs)e;
            Change_Devices(e2.StringArray);
        }

        private void EventFileScanner_ArousalUpdated(object sender, EventArgs e)
        {
            IntArg e2 = (IntArg)e;
            Update_Arousal(e2.Int);

        }
        private void EventFileScanner_SaveLoaded(object sender, EventArgs e)
        {
            StringArg e2 = (StringArg)e;

            Update_Arousal(0);
            Clear_EventViewer();

            Invoke((MethodInvoker)(() => text_detected_mods.Text = e2.String));
        }

        private void Memory_Scanner_GamePaused(object sender, EventArgs e)
        {
            GamePaused(true);
        }
        private void Memory_Scanner_GameResumed(object sender, EventArgs e)
        {
            GamePaused(false);
        }

        private void Memory_Scanner_AnimationTimeUpdated(object sender, EventArgs e)
        {
            FloatArg e2 = (FloatArg)e;

            Invoke((MethodInvoker)(() =>
            {
                label_animation_time.Text = e2.Float.ToString("0.00");
            }));
        }

        private void Memory_Scanner_AnimationTimeResetted(object sender, EventArgs e)
        {
            //WriteTo_EventViewer("- Animation Timer Update!");
        }
        


        private void Memory_Scanner_AnimationEvent(object sender, EventArgs e)
        {
            StringArg e2 = (StringArg)e;
            //WriteTo_EventViewer("Animation: "+e2.String);
        }



        private int running_events = 0;

        private void Event_Device_removed()
        {
            running_events--;
            //Update_Running_Events();
        }

        private void Event_Device_Added()
        {
            running_events++;
            //Update_Running_Events();
        }
        private void Event_Device_Cleared()
        {
            running_events = 0;
            //Update_Running_Events();
        }

        private void Update_Running_Events()
        {
            Invoke((MethodInvoker)(() =>
            {
                //label_running_events.Text = "Running events: " + running_events;
            }));
        }



        internal void WriteTo_EventViewer(string text, Color color)
        {

            Invoke((MethodInvoker)(() => {
                int start = event_viewer.Text.Length;
                event_viewer.AppendText(text + "\n");
                int finished = event_viewer.Text.Length;

                event_viewer.Select(start, finished); //Select text within 0 and 8
                event_viewer.SelectionColor = color;
            }));
        }

        internal void WriteTo_EventViewer(string text)
        {
            WriteTo_EventViewer(text, Color.Black);
        }

        internal void Clear_EventViewer()
        {
            //Invoke((MethodInvoker)(() => event_viewer.Clear()));
        }


        internal void Update_Arousal(int arousal)
        {
            if (arousal > 100) { arousal = 100; }
            if (arousal < 0) { arousal = 0; }
            Invoke((MethodInvoker)(() => progressBar_arousal.Value = arousal));
        }

        internal void Change_Devices(string[] devices)
        {
            Invoke((MethodInvoker)(() => {
                label_vaginal.Text = devices[0];
                label_anal.Text = devices[1];
                label_vaginal_piercing.Text = devices[2];
                label_nipple_piercing.Text = devices[3];

            }));
        }


        bool game_running = true;
        bool game_paused = false;


        internal void GamePaused(bool paused)
        {
            Invoke((MethodInvoker)(() => game_paused = paused));
            Update_Game_State();
        }

        internal void Update_Running_Events(List<Running_Event> list)
        {
            
            Invoke((MethodInvoker)(() => label_running_events.Text = "running events: " + list.Count));
            
        }


        private void Update_Game_State()
        {
            string text = label_game_state.Text;
            if (game_running)
            {
                if (game_paused)
                {
                    text = "Paused";
                }
                else
                {
                    text = "Running";
                }
            }
            else
            {
                text = "Not running";
            }

            Invoke((MethodInvoker)(() => label_game_state.Text = text));
        }

        private void Form_EventFileReader_FormClosing(object sender, FormClosingEventArgs e)
        {
            memory_scanner.Close();
            Thread_eventFileScanner.Abort();
            Device.Stop_All();
        }



        private void Event_viewer_TextChanged(object sender, EventArgs e)
        {
            event_viewer.SelectionStart = event_viewer.Text.Length;
            // scroll it automatically
            event_viewer.ScrollToCaret();
        }







        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

		private void CheckBox_ShowDebug_CheckedChanged(object sender, EventArgs e)
		{
            Properties.Settings.Default.Show_debug_info = checkBox_ShowDebug.Checked;
            Properties.Settings.Default.Save();
        }

		private void CheckBox_ShowNotification_CheckedChanged(object sender, EventArgs e)
		{
            Properties.Settings.Default.Show_notifications = checkBox_ShowNotifications.Checked;
            Properties.Settings.Default.Save();
        }

		private void CheckBox_ShowErrors_CheckedChanged(object sender, EventArgs e)
		{
            Properties.Settings.Default.Show_errors = checkBox_ShowErrors.Checked;
            Properties.Settings.Default.Save();
        }

		private void CheckBox_ShowWarnings_CheckedChanged(object sender, EventArgs e)
		{
            Properties.Settings.Default.Show_warnings = checkBox_ShowWarnings.Checked;
            Properties.Settings.Default.Save();
        }

		private void button1_Click(object sender, EventArgs e)
		{
            vibrationEvents.Init();
		}
	}
}
