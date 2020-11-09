using ScriptPlayer.Shared.Scripts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Butthesda.Program;

namespace Butthesda
{



    public class VibrationEvents
    {
        public event EventHandler Notification_Message;
        public event EventHandler Warning_Message;
        public event EventHandler Error_Message;
        public event EventHandler Debug_Message;

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


        public Running_Event Play_Event(string name)
        {
            name = name.ToLower();
            foreach (Actor_Data event_data in events)
            {
                if (event_data.name == name)
                {
                    Notification_Message?.Invoke(this, new StringArg("Playing event: " + name));
                    return PlayEvent(event_data);
                }
            }
            Warning_Message?.Invoke(this, new StringArg("Count not find: " + name));
            return new Running_Event();
        }

        private Running_Event PlayEvent(Actor_Data event_data,bool synced_by_animation)
        {
            Running_Event running_Event = new Running_Event();
            foreach (BodyPart_Data bodypart in event_data.bodyparts)
            {
                if (bodypart == null) { continue; };
                Device.BodyPart bodyPart_id = bodypart.bodyPart;

                foreach (EventType_Data eventType in bodypart.eventTypes)
                {
                    if (eventType == null) { continue; };
                    Device.EventType eventType_id = eventType.eventType;

                    foreach (Device device in Device.devices)
                    {
                        if (device.HasType(bodyPart_id, eventType_id))
                        {
                            running_Event = device.AddEvent(event_data.name, eventType.actions, synced_by_animation);
                        }
                    }
                }

            }
            return running_Event;
        }
        private Running_Event PlayEvent(Actor_Data event_data) {
            return PlayEvent(event_data, false);
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





        private Animation_Data Sexlab_Playing_Animation = new Animation_Data();
        private Running_Event sexLab_running_Event = new Running_Event();
        private int Sexlab_Position = 0;
        private int Sexlab_Stage = 0;

        private Running_Event sexLab_running_Event_orgasm = new Running_Event();

        public void SexLab_StartAnimation(string name, int stage, int position, bool usingStrappon)
        {
            SexLab_StopAnimation();

            name = name.ToLower();

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

                    return;
                }
            }
            Warning_Message?.Invoke(this, new StringArg("Can't find SexLab animation: " + name));
        }

        public void SexLab_StopAnimation()
        {
            sexLab_running_Event.End();//end old event
            sexLab_running_Event = new Running_Event();
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
            sexLab_running_Event.End();
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
            sexLab_running_Event_orgasm.End();
            sexLab_running_Event_orgasm = new Running_Event();
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
                string s_eventType = Path.GetFileName(eventType_dir);
                s_eventType = s_eventType.Remove(s_eventType.Length - ".funscript".Length).ToLower();

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

                eventTypes[index] = new EventType_Data(eventType_dir, eventType);
            }
        }

    }

    class EventType_Data
    {
        public List<FunScriptAction> actions;
        public Device.EventType eventType;
        public EventType_Data(string eventType_dir, Device.EventType eventType)
        {
            this.eventType = eventType;

            FunScriptLoader loader = new FunScriptLoader();
            actions = loader.Load(eventType_dir).Cast<FunScriptAction>().ToList(); ;

        }
    }
}
