using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using static Butthesda.Program;

namespace Butthesda
{
    public partial class Form_EventFileReader : Form
    {
        Thread Thread_eventFileScanner;

        private Memory_Scanner memory_scanner;
        private VibrationEvents vibrationEvents;
        public Form_EventFileReader(String GameName)
        {
            InitializeComponent();
            vibrationEvents = new VibrationEvents();

            memory_scanner = new Memory_Scanner(GameName);
            memory_scanner.AnimationEvent += Memory_Scanner_AnimationEvent;
            memory_scanner.AnimationTimeResetted += Memory_Scanner_AnimationTimeResetted;
            memory_scanner.AnimationTimeUpdated += Memory_Scanner_AnimationTimeUpdated;
            memory_scanner.GamePaused += Memory_Scanner_GamePaused;
            memory_scanner.GameResumed += Memory_Scanner_GameResumed;


            EventFileScanner eventFileScanner = new EventFileScanner(memory_scanner);
            eventFileScanner.Save_Loaded += EventFileScanner_SaveLoaded;
            eventFileScanner.Arousal_Updated += EventFileScanner_ArousalUpdated;
            eventFileScanner.DD_Device_Update += EventFileScanner_DD_DeviceUpdate;

            Thread_eventFileScanner = new Thread(new ThreadStart(eventFileScanner.Run))
            {
                IsBackground = true
            };
            Thread_eventFileScanner.Start();

            
            foreach (Device d in Device.devices)
            {
                d.EventAdded += Event_Device_Added;
                d.EventRemoved += Event_Device_removed;
                d.EventsCleared += Event_Device_Cleared;

            }

            vibrationEvents.Warning_Message += Event_Warning;


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
            WriteTo_EventViewer("Loaded Save Game");
        }

        private void Memory_Scanner_GamePaused(object sender, EventArgs e)
        {
            WriteTo_EventViewer("- Game Paused");
            GamePaused(true);
        }
        private void Memory_Scanner_GameResumed(object sender, EventArgs e)
        {
            WriteTo_EventViewer("- Game Resumed!");
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
            WriteTo_EventViewer("Animation: "+e2.String);
        }



        private void Event_Warning(object sender, EventArgs e)
        {
            StringArg e2 = (StringArg)e;
            WriteTo_EventViewer(e2.String);
        }

        private int running_events = 0;

        private void Event_Device_removed()
        {
            running_events--;
            Update_Running_Events();
        }

        private void Event_Device_Added()
        {
            running_events++;
            Update_Running_Events();
        }
        private void Event_Device_Cleared()
        {
            running_events = 0;
            Update_Running_Events();
        }

        private void Update_Running_Events()
        {
            Invoke((MethodInvoker)(() =>
            {
                //label_running_events.Text = "Running events: " + running_events;
            }));
        }

        internal void WriteTo_EventViewer(string text)
        {
            Invoke((MethodInvoker)(() => event_viewer.AppendText(text + "\n")));
        }

        internal void Clear_EventViewer()
        {
            Invoke((MethodInvoker)(() => event_viewer.Clear()));
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


        bool game_running = false;
        bool game_paused = false;

        internal void GameRunning(bool running)
        {
            Invoke((MethodInvoker)(() => game_running = running));
            Update_Game_State();
        }

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
            record_video = false;
            event_viewer.SelectionStart = event_viewer.Text.Length;
            // scroll it automatically
            event_viewer.ScrollToCaret();
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void Form_EventFileReader_Load(object sender, EventArgs e)
        {

        }

        private void Label8_Click(object sender, EventArgs e)
        {

        }


        public static bool record_video = false;

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {

            record_video = checkBox1.Checked;
        }

        private void Label9_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click_1(object sender, EventArgs e)
        {

        }
    }
}
