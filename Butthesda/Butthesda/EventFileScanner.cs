using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Media;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static Butthesda.Program;

namespace Butthesda
{


    class EventFileScanner
    {

        public event EventHandler Notification_Message;
        public event EventHandler Warning_Message;
        public event EventHandler Error_Message;
        public event EventHandler Debug_Message;

        public event EventHandler Arousal_Updated;
        public event EventHandler Save_Loaded;
        public event EventHandler DD_Device_Update;

        public bool inMenu = false;
        public int arousal = 50;
        public DD_Device_Type[] dd_devices = new DD_Device_Type[Enum.GetNames(typeof(DD_Device_Location)).Length];
        public enum DD_Device_Location
        {
            Vaginal = 0,
            Anal,
            VaginalPiecing,
            NipplePiercing
        }
        public enum DD_Device_Type
        {
            none = 0,
            pump,
            magic,
            normal
        }

        public EventFileScanner(Memory_Scanner memory_scanner)
        {
            memory_scanner.AnimationEvent += MemoryScanner_AnimationEvent;

        }

        private void MemoryScanner_AnimationEvent(object sender, EventArgs e)
        {
            StringArg e2 = (StringArg)e;
            switch (e2.String.ToLower())
            {
                case "footright":
                case "footleft":
                case "jumpup":
                case "jumpdown":
                    PlaySound();
                    break;
            }

        }

        private void PlaySound()
        {
            using (var soundPlayer = new SoundPlayer(@"c:\Windows\Media\Windows Information Bar.wav"))
            {
                soundPlayer.Play(); // can also use soundPlayer.PlaySync()
            }
        }


        private Running_Event dd_horny_event = new Running_Event();


        public void Run()
        {
            Thread.Sleep(100);
            string path = Form_Main.game_path + @"buttplugio\link.txt";
            return;
            const Int32 BufferSize = 128;
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                bool loading_game = true;
                bool skipping_old = true;
                while (true)
                {
                    String line = streamReader.ReadLine();
                    if (line == null)
                    {
                        skipping_old = false; //we skipped all old lines
                        Thread.Sleep(10);
                    }
                    else
                    {
                        line = line.ToLower().Replace("\'", "\"");

                        //Console.WriteLine(line);
                        if (!line.StartsWith("{")) { continue; };

                        JObject json;
                        try { json = JObject.Parse(line); }
                        catch
                        {
                            Warning_Message?.Invoke(this, new StringArg("Count not format line: " + line));
                            continue;
                        };

                        string mod_property = (string)json.Property("mod");
                        string event_property = (string)json.Property("event");
                        string type_property = (string)json.Property("type");


                        //this code runs mainly purpose is to skip old lines of code in the event file
                        if (loading_game || skipping_old)
                        {
                            bool allowed = false;
                            if(mod_property == "game")
                            {
                                if(event_property == "loading save" || event_property == "loading save done")
                                {
                                    allowed = true;
                                }
                                
                            }

                            if (mod_property == "dd")
                            {
                                if(event_property == "(de)equiped")
                                {
                                    allowed = true;
                                }
                            }

                            if (mod_property =="sla")  
                            {
                                allowed = true;
                            }

                            if (!allowed)
                            {
                                continue;
                            }

                            
                        }

                        switch (mod_property)
                        {
                            case "game":
                                switch (event_property)
                                {
                                    case "loading save":
                                        loading_game = true;

                                        string mods_found = "";

                                        if ((bool)json.Property("sla_running"))
                                        {
                                            mods_found += "- SexLab Arousal\n";
                                        }
                                        if ((bool)json.Property("dd_running"))
                                        {
                                            mods_found += "- Deviouse Devices\n";
                                        }
                                        if ((bool)json.Property("mme_running"))
                                        {
                                            mods_found += "- Milk Mod Economy\n";
                                        }
                                        if ((bool)json.Property("bf_running"))
                                        {
                                            mods_found += "- Being Female\n";
                                        }
                                        if ((bool)json.Property("sgo_running"))
                                        {
                                            mods_found += "- Soul Gem Oven\n";
                                        }

                                        Save_Loaded?.Invoke(this, new StringArg(mods_found));
                                        break;
                                    case "loading save done":
                                        loading_game = false;
                                        Notification_Message?.Invoke(this, new StringArg("Save Game Loaded!"));
                                        break;
                                    case "menu opened"://TODO might need to re add this if the animation timer based check doest work good enough
                                        //form_EventFileReader.GamePaused(true);
                                        inMenu = true;
                                        break;
                                    case "menu closed":
                                        //form_EventFileReader.GamePaused(false);
                                        inMenu = false;
                                        break;
                                    case "animation"://not used anymore, but we need to re implement this part in the new framework
                                        string name_property = (string)json.Property("name");
                                        switch (name_property)
                                        {
                                            case "footleft":
                                            case "footright":
                                            case "FootSprintLeft":
                                            case "FootSprintRight":
                                            case "tailmtidle":
                                                if (dd_devices[(int)DD_Device_Location.Anal] != DD_Device_Type.none)
                                                {
                                                    //VibrationEvents.Play_Event("dd device footstep anal");
                                                }
                                                if (dd_devices[(int)DD_Device_Location.Vaginal] != DD_Device_Type.none)
                                                {
                                                    //VibrationEvents.Play_Event("dd device footstep vaginal");
                                                }

                                                break;
                                            case "jumpup":
                                            case "jumplandend":

                                            case "idlechairsitting":
                                            case "idlechairgetup":

                                            case "soundplay.npchorsemount":
                                            case "soundplay.npchorsedismount":
                                            case "footback":
                                            case "footfront":
                                            case "jumpbegin":
                                            case "horselocomotion":
                                            
                                            case "idlestop":
                                                
                                                break;
                                        }
                                        break;
                                }
                                break;
                            case "sla":
                                arousal = (int)json.Property("arousal");
                                Arousal_Updated?.Invoke(this, new IntArg(arousal));
                                break;
                            case "dd":

                                switch (event_property)
                                {
                                    case "(de)equiped":

                                        string[] devices_names = new string[Enum.GetNames(typeof(DD_Device_Location)).Length];

                                        //check which device was (de)equiped
                                        for (int i = 0; i < dd_devices.Length; i++)
                                        {
                                            DD_Device_Type dd_device_current_type = dd_devices[i];

                                            string s_location = Enum.GetNames(typeof(DD_Device_Location))[i].ToLower();
                                            string s_location_type = (string)json.Property(s_location);
                                            string[] s_location_types = Array.ConvertAll(Enum.GetNames(typeof(DD_Device_Type)), d => d.ToLower());
                                            DD_Device_Type dd_device_new_type = (DD_Device_Type)Array.IndexOf(s_location_types, s_location_type);

                                            //check if this was the device that changed
                                            //And we dont run any event when the game was loading
                                            if (!(loading_game || skipping_old) && dd_device_current_type != dd_device_new_type)
                                            {
                                                //check if device was added or removed
                                                if (dd_device_new_type != DD_Device_Type.none)
                                                {//device was removed
                                                    Notification_Message?.Invoke(this, new StringArg("Deviouse Device equiped: " + s_location + " " + s_location_type));
                                                    //VibrationEvents.Play_Event("dd device equiped " + s_location);
                                                }
                                                else
                                                {//device was added
                                                    Notification_Message?.Invoke(this, new StringArg("Deviouse Device de-equiped: " + s_location + " " + s_location_type));
                                                    //VibrationEvents.Play_Event("dd device de-equiped " + s_location);
                                                }
                                                //break;
                                            }
                                            dd_devices[i] = dd_device_new_type;

                                            devices_names[i] = s_location_type;


                                        }
                                        DD_Device_Update?.Invoke(this, new StringArgs(devices_names));
                                        


                                        break;
                                    case "vibrate effect start":
                                        Notification_Message?.Invoke(this, new StringArg("Deviouse Device vibrate " + (float)json.Property("arg")));
                                        break;
                                    case "vibrate effect stop":
                                        Notification_Message?.Invoke(this, new StringArg("Deviouse Device vibrate stop " + (float)json.Property("arg")));
                                        break;
                                    case "orgasm":
                                        Notification_Message?.Invoke(this, new StringArg("Deviouse Device orgasm " + (float)json.Property("arg")));
                                        break;
                                    case "edged":
                                        Notification_Message?.Invoke(this, new StringArg("Deviouse Device edged " + (float)json.Property("arg")));
                                        break;
                                    case "device event":
                                        string type = (string)json.Property("type");
                                        Notification_Message?.Invoke(this, new StringArg("Deviouse Device event: " + type));

                                        switch (type)
                                        {
                                            case "trip over":

                                                break;
                                            case "drip":

                                                break;
                                            case "stamina drain":

                                                break;
                                            case "blindfold mystery":

                                                break;
                                            case "restraints+armor":

                                                break;
                                            case "posture collar":

                                                break;
                                            case "wet padding":

                                                break;
                                            case "blindold trip":

                                                break;
                                            case "nipple piercings":

                                                break;
                                            case "tight corset":

                                                break;
                                            case "plug moan":

                                                break;
                                            case "trip and fall":

                                                break;
                                            case "bump pumps":

                                                break;
                                            case "struggle":

                                                break;
                                            case "belted empty":

                                                break;
                                            case "mounted":

                                                break;
                                            case "tight gloves":

                                                break;
                                            case "bra chafing":

                                                break;
                                            case "periodic shock":

                                                break;
                                            case "arm cuff fumble":

                                                break;
                                            case "draugr plug vibration":

                                                break;
                                            case "restrictive collar":

                                                break;
                                            case "mana drain":

                                                break;
                                            case "vibration":

                                                break;
                                            case "harness":

                                                break;
                                            case "horny":

                                                break;
                                            case "chaos plug":

                                                break;
                                            case "belt chafing":

                                                break;
                                            case "health drain":

                                                break;
                                            case "organicvibrationeffect":

                                                break;
                                            default:
                                                Warning_Message?.Invoke(this, new StringArg("Deviouse Device event unknown type: " + type));
                                                break;
                                        }

                                        break;

                                }
                                break;
                            case "sexlab":

                                string name = (string)json.Property("name");
                                

                                switch (event_property)
                                {
                                    case "animation started":
                                    case "animation changed":
                                        {
                                            int stage = (int)json.Property("stage")-1;
                                            int position = (int)json.Property("pos");
                                            bool usingStrapon = (bool)json.Property("usingstrappon");
                                            Warning_Message?.Invoke(this, new StringArg("SexLab " + event_property + " : \"" + name + "\" stage:" + stage + " position: " + position + " using strapon: " + usingStrapon));
                                            //VibrationEvents.SexLab_StartAnimation(name, stage, position, usingStrapon);
                                            if (event_property == "animation changed")
                                            {
                                                //VibrationEvents.SexLab_Update_Event();
                                            }
                                        }
                                        break;
                                    case "animation ended":
                                        Warning_Message?.Invoke(this, new StringArg("SexLab animation stopped"));
                                        //VibrationEvents.SexLab_StopAnimation();
                                        break;
                                    case "stage started":
                                        {
                                            int stage = (int)json.Property("stage")-1;
                                            Warning_Message?.Invoke(this, new StringArg("SexLab stage started: " + stage));
                                            //VibrationEvents.SexLab_SetStage(stage);
                                        }
                                        break;
                                    case "position changed":
                                        {
                                            int position = (int)json.Property("pos");
                                            Warning_Message?.Invoke(this, new StringArg("SexLab position changed: " + position));
                                            //VibrationEvents.SexLab_SetPosition(position);
                                        }
                                        break;
                                    case "orgasm started":
                                        Warning_Message?.Invoke(this, new StringArg("SexLab orgasm"));
                                        //VibrationEvents.SexLab_Start_Orgasm();
                                        break;
                                    case "orgasm ended":
                                        Warning_Message?.Invoke(this, new StringArg("SexLab orgasm ended"));
                                        //VibrationEvents.SexLab_Stop_Orgasm();
                                        break;
                                }
                                break;
                            case "mme":
                                int mpas = (int)json.Property("mpas");
                                int milkingType = (int)json.Property("milkingtype");
                                switch (event_property)
                                {
                                    case "StartMilkingMachine":

                                        break;
                                    case "StopMilkingMachine":

                                        break;
                                    case "FeedingStage":

                                        break;
                                    case "MilkingStage":

                                        break;
                                    case "FuckMachineStage":

                                        break;
                                }
                                break;
                            
          
                        }

                        //VibrationEvents.animations;

                    }
                }
            }
        }       
    }
}
