﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Butthesda
{



    public class VibrationEvents
    {
        public event EventHandler Notification_Message;
        public event EventHandler Warning_Message;
        public event EventHandler Error_Message;
        public event EventHandler Debug_Message;

        public event EventHandler<StringArg> SexLab_Animation_Changed;

        string Game_Path;
        public VibrationEvents(string Game_Path)
        {
            this.Game_Path = Game_Path;
        }

        public void Init()
		{
            Init_SexLabAnimations();
            Init_Events();
        }



        private List<Actor_Data> events = new List<Actor_Data>();


        private void Init_Events()
        {
            string other_dir = Game_Path + @"\FunScripts";
            string[] mod_dirs = Directory.GetDirectories(other_dir);
            foreach (string mod_dir in mod_dirs)
            {
				if (Path.GetFileName(mod_dir).ToLower() == "sexlab")
				{
                    continue;
				}

                string[] event_dirs = Directory.GetDirectories(mod_dir);
                foreach (string event_dir in event_dirs)
                {
                    string name = Path.GetFileName(event_dir).ToLower();
                    events.Add(new Actor_Data(name, event_dir));
                }
            }

            int funscript_count = 0;
            foreach (Actor_Data p_d in events)
            {
                if (p_d == null) continue;
                foreach (BodyPart_Data b_d in p_d.bodyparts)
                {
                    if (b_d == null) continue;
                    foreach (EventType_Data e_d in b_d.eventTypes)
                    {
                        funscript_count++;
                    }
                }
            }
            Notification_Message?.Invoke(this, new StringArg(String.Format("Registered {0} lose events, with a total of {1} funscripts", events.Count, funscript_count)));
        }




        private List<Animation_Data> SexLab_Animations = new List<Animation_Data>();
        private Actor_Data SexLab_Orgasm_Event = new Actor_Data();
        private void Init_SexLabAnimations()
        {
            string sexlab_dir = Game_Path + @"\FunScripts\SexLab";
            string[] mod_dirs = Directory.GetDirectories(sexlab_dir);

            foreach (string mod_dir in mod_dirs)
            {
                if (mod_dir.ToLower() == "orgasm")
                {
                    SexLab_Orgasm_Event = new Actor_Data("orgasm", mod_dir);
                    continue;
                }

                string[] animation_dirs = Directory.GetDirectories(mod_dir);
                foreach (string animation_dir in animation_dirs)
                {
                    string name = Path.GetFileName(animation_dir).ToLower();
                    SexLab_Animations.Add(new Animation_Data(name, animation_dir));
                }
            }


            int funscript_count = 0;

            foreach(Animation_Data sl_a in SexLab_Animations)
			{
				foreach (Stage_Data s_d in sl_a.stages)
				{
                    if (s_d == null) continue;
                    foreach (Actor_Data p_d in s_d.positions)
					{
                        if (p_d == null) continue;
                        foreach (BodyPart_Data b_d in p_d.bodyparts)
						{
                            if (b_d == null) continue;
							foreach (EventType_Data e_d in b_d.eventTypes)
							{
                                funscript_count++;
                            }
						}
					}
				}
			}

            Notification_Message?.Invoke(this,new StringArg(String.Format("Registered {0} SexLab animations, with a total of {1} funscripts", SexLab_Animations.Count, funscript_count)));
        }




        private List<Running_Event> PlayEvent(Actor_Data event_data, bool synced_by_animation)
        {
            // Some events are meant for multiple body parts. If a physical device is mapped to more than one of these body parts AND 
            // a single event triggers multiple body parts in one call of this method, only the first effect is played on a device.
            uint devicesReceivedEventAlready = 0;  // used as bitmask for Device.devices
            uint currentDeviceIndex;

            // These loops can create 0, 1 or more events on a device, e.g. if one device is registered for multiple body parts.
            // We return a list of all created events so we don't loose the reference to an event object,
            // because we need to be able to call the End() method to prematurely stop an event.

            List<Running_Event> runningEvents = new List<Running_Event>();
            foreach (BodyPart_Data bodypart in event_data.bodyparts)
            {
                if (bodypart == null) { continue; }
                Device.BodyPart bodyPart_id = bodypart.bodyPart;  // Head, Body, Breast, ...

                foreach (EventType_Data eventType in bodypart.eventTypes)
                {
                    if (eventType == null) { continue; }
                    Device.EventType eventType_id = eventType.eventType;  // Shock, Damage, Penetrate, Vibrate, Equip (with footsteps==Penetrate)

                    currentDeviceIndex = 1;
                    foreach (Device device in Device.devices)  // physical device
                    {
                        if (device.HasType(bodyPart_id, eventType_id) && (devicesReceivedEventAlready & currentDeviceIndex) == 0) // and device did not already receive an event in this method's call
                        {
                            // if (event_data.name.StartsWith("dd vibrator"))  // Debug
                            // {
                            //     Notification_Message?.Invoke(this, new StringArg(String.Format(
                            //         "Creating event {0} at part {1} with type {2}", event_data.name, bodyPart_id.ToString(), eventType_id.ToString())
                            //     ));
                            // }
                            runningEvents.Add(device.AddEvent(event_data.name, eventType.actions, synced_by_animation));
                            devicesReceivedEventAlready = devicesReceivedEventAlready | currentDeviceIndex;
                        }
                        currentDeviceIndex = currentDeviceIndex << 1;
                    }
                }
            }
            return runningEvents;
        }

        private List<Running_Event> PlayEvent(Actor_Data event_data)
        {
            return PlayEvent(event_data, false);
        }

        public List<Running_Event> PlayEvent(string name)
        {
            name = name.ToLower();
            foreach (Actor_Data event_data in events)
            {
                if (event_data.name == name)
                {
                    List<Running_Event> runningEvents = PlayEvent(event_data);
                    Notification_Message?.Invoke(this, new StringArg(String.Format("Playing event: {0} ({1})", name, runningEvents.Count)));
                    return runningEvents;
                }
            }
            Warning_Message?.Invoke(this, new StringArg("Count not find: " + name));
            return new List<Running_Event>();
        }


        private Animation_Data Sexlab_Playing_Animation = new Animation_Data();
        private List<Running_Event> sexLab_running_Event = new List<Running_Event>();

		public int Sexlab_Position { get; private set; } = 0;
        public int Sexlab_Stage { get; private set; } = 0;
        public string Sexlab_Name { get; private set; } = "";
        private List<Running_Event> sexLab_running_Event_orgasm = new List<Running_Event>();

        public bool SexLab_StartAnimation(string name, int stage, int position, bool usingStrappon)
        {
            SexLab_StopAnimation();

            name = name.ToLower();
            Sexlab_Name = name;
            foreach (Animation_Data animation in SexLab_Animations)
            {
                if (animation.name == name)
                {
                    

                    Sexlab_Playing_Animation = animation;
                    Sexlab_Stage = stage;
                    Sexlab_Position = position;

                    

                    //lets not do this here because the animation is not started yet
                    //it starts playing when stage updates
                    //UpdateSexLabEvent();

                    return true;
                }
            }
            Warning_Message?.Invoke(this, new StringArg("Can't find SexLab animation: " + name));
            return false;
        }

        public void SexLab_StopAnimation()
        {
            sexLab_running_Event.ForEach(runningEvent => runningEvent.End());  // end old events
            sexLab_running_Event.Clear();
            Sexlab_Playing_Animation = new Animation_Data();
        }

        public void SexLab_SetStage(int stage)
        {
            Sexlab_Stage = stage;
            SexLab_Update_Event();
        }

        public void SexLab_SetPosition(int pos)
        {
            Sexlab_Position = pos;
            SexLab_Update_Event();
        }

        //return the current playing sexlab event
        public void SexLab_Update_Event()
        {
            SexLab_Animation_Changed?.Invoke(this, new StringArg(String.Format("{0} S-{1}, P-{2}", Sexlab_Name, Sexlab_Stage,Sexlab_Position)));
            sexLab_running_Event.ForEach(runningEvent => runningEvent.End());
            Stage_Data stage_data = Sexlab_Playing_Animation.stages[Sexlab_Stage];
            if (stage_data != null)
            {
                Actor_Data position_data = stage_data.positions[Sexlab_Position];
                sexLab_running_Event = PlayEvent(position_data,true);
            }
        }

        public void SexLab_Start_Orgasm()
        {
            sexLab_running_Event_orgasm = PlayEvent(SexLab_Orgasm_Event);
        }

        public void SexLab_Stop_Orgasm()
        {
            sexLab_running_Event_orgasm.ForEach(runningEvent => runningEvent.End());
            sexLab_running_Event_orgasm.Clear();
        }
    }


    class Animation_Data
    {
        public string name;
        public Stage_Data[] stages = new Stage_Data[50];
        public Animation_Data(string name, string dir)
        {
            this.name = name;
            String[] stage_dirs = Directory.GetDirectories(dir);
            foreach (string stage_dir in stage_dirs)
            {
                int index = Int32.Parse(Path.GetFileName(stage_dir).Substring(1));
                stages[index - 1] = new Stage_Data(stage_dir);
            }

        }
        public Animation_Data()
        {
            name = "none";
        }
    }

    class Stage_Data
    {
        public Actor_Data[] positions = new Actor_Data[10];
        public Stage_Data(String stage_dir)
        {
            String[] position_dirs = Directory.GetDirectories(stage_dir);

            foreach (String position_dir in position_dirs)
            {
                int index = Int32.Parse(Path.GetFileName(position_dir).Substring(1));
                positions[index - 1] = new Actor_Data(position_dir);
            }
        }

    }

    class Actor_Data
    {
        public string name = "";
        public BodyPart_Data[] bodyparts = new BodyPart_Data[Enum.GetNames(typeof(Device.BodyPart)).Length];

        public Actor_Data(string name, string position_dir)
        {
            this.name = name;
            string[] bodyPart_dirs = Directory.GetDirectories(position_dir);

            foreach (string bodyPart_dir in bodyPart_dirs)
            {
                string s_eventType = Path.GetFileName(bodyPart_dir);

                Device.BodyPart bodyPart;
                try
                {
                    bodyPart = (Device.BodyPart)Enum.Parse(typeof(Device.BodyPart), s_eventType, true);
                }
                catch
                {
                    continue;
                }

                int index = (int)bodyPart;
                bodyparts[index] = new BodyPart_Data(bodyPart_dir, bodyPart);
            }
        }

        public Actor_Data(string position_dir) : this("", position_dir)
        {
        }

        public Actor_Data()
        {
        }

    }

    class BodyPart_Data
    {
        public EventType_Data[] eventTypes = new EventType_Data[Enum.GetNames(typeof(Device.EventType)).Length];
        public Device.BodyPart bodyPart;
        public BodyPart_Data(string bodyPart_dir, Device.BodyPart bodyPart)
        {
            this.bodyPart = bodyPart;

            string[] eventType_dirs = Directory.GetFiles(bodyPart_dir);
            foreach (string eventType_dir in eventType_dirs)
            {
                string s_eventType = Path.GetFileName(eventType_dir).ToLower();
                bool is_funscript = false;
                bool is_estim = false;
                if (s_eventType.EndsWith(".funscript"))
				{
                    is_funscript = true;
                    s_eventType = s_eventType.Remove(s_eventType.Length - ".funscript".Length);
				}
				else if(s_eventType.EndsWith(".mp3"))
				{
                    is_estim = true;
                    s_eventType = s_eventType.Remove(s_eventType.Length - ".mp3".Length);
				}
				else
				{
                    continue;
				}
                

                Device.EventType eventType;
                try
                {
                    eventType = (Device.EventType)Enum.Parse(typeof(Device.EventType), s_eventType, true);
                }
                catch
                {
                    continue;
                }

                int index = (int)eventType;

                if(eventTypes[index] == null)
				{
                    eventTypes[index] = new EventType_Data(eventType);
                }

				if (is_estim)
				{
                    eventTypes[index].Add_Estim(eventType_dir);
                }
                else if (is_funscript)
				{
                    eventTypes[index].Add_Funscript(eventType_dir);
                }
                
                

            }
        }

    }

    class EventType_Data
    {
        public List<FunScriptAction> actions;
        public Device.EventType eventType;
        public string estim_file = "";

        public EventType_Data(Device.EventType eventType)
        {
            this.eventType = eventType;
        }

        public void Add_Funscript(string file)
		{
            actions = FunScriptLoader.Load(file).ToList();
        }

        public void Add_Estim(string file)
		{
            estim_file = file;
        }

    }
}
