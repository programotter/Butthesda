using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Buttplug.Client;
using Buttplug.Core.Logging;
using Buttplug.Core.Messages;
using ScriptPlayer.Shared;
using ScriptPlayer.Shared.Scripts;

namespace Butthesda
{
    public delegate void Notify();

    public class Device
    {
        public static List<Device> devices = new List<Device>();
        private static readonly SemaphoreSlim _clientLock = new SemaphoreSlim(1);

        public String name;
        public ButtplugClientDevice device;
        public ButtplugClient client;
        public bool active;

        private byte maxPosition = 95;
        private byte minPosition = 5;
        private byte minSpeed = 20;
        private byte maxSpeed = 95;
        private double speedMultiplier = 1;
        private bool autoHomingEnabled = false;
        private bool invertPosition = false;

        private Thread thread;

        public enum BodyPart
        {
            Head = 0,
            Body,
            Breast,
            Belly,
            Feet,
            Mouth,
            Vaginal,
            Clit,
            Anal
        }

        public enum EventType
        {
            Shock = 0,
            Damage,
            Penetrate,
            Vibrate,
            Equip
        }

        private List<Running_Event> running_events;

        public Device(String name, ButtplugClient client, ButtplugClientDevice device)
        {
            this.name = name;
            this.device = device;
            this.client = client;
            this.active = false;
            this.running_events = new List<Running_Event>();

            thread = new Thread(UpdateLoop)
            {
                IsBackground = true
            };
            thread.Start();

            //VibrationEvents.Animation_Timer_Reset_Event += On_Animation_Timer_Reset;


        }



        private void On_Animation_Timer_Reset(object sender, EventArgs e)
        {
            foreach(Running_Event running_event in running_events)
            {
                running_event.Reset();
            }
            ForceUpdate();            
        }


        public event Notify EventAdded;
        public event Notify EventRemoved;
        public event Notify EventsCleared;

        public Running_Event AddEvent(List<FunScriptAction> actions,bool synced_by_animation)
        {
            Running_Event new_event = new Running_Event(actions, synced_by_animation);
            EventAdded?.Invoke();
            lock (running_events)
            {
                running_events.Add(new_event);
            }
            ForceUpdate();
            return new_event;
        }

        public static void Stop_All()
        {
            
            foreach (Device device in devices)
            {
                lock (device.running_events)
                {
                    device.EventsCleared?.Invoke();
                    device.running_events.Clear();
                    device.ForceUpdate();
                }
            }
        }


        DateTime nextUpdate = new DateTime(0);
        private async void UpdateLoop()
        {
            while (true)
            {
                Thread.Sleep(10);
                //if (!gameRunning) continue;
                //check if there are running events
                lock (running_events)
                {                   
                    if(running_events.Count == 0)
                    {
                        continue;
                    }
                }


                DateTime timeNow = DateTime.Now;
                if (timeNow >= nextUpdate)
                {

                    List<int> positions = new List<int>();

                    lock (running_events)
                    {
                        //update all events and find next update time
                        foreach (Running_Event running_event in running_events)
                        {
                            running_event.Update(timeNow);
                        }

                        //remove events that are done
                        for(int i = running_events.Count - 1; i>=0; i--)
                        {
                            if (running_events[i].ended)
                            {
                                running_events.RemoveAt(i);
                                EventRemoved?.Invoke();
                            }
                        }

                        //find earliest time at wich point we need to do a update again
                        foreach (Running_Event running_event in running_events)
                        {
                            if (nextUpdate < running_event.nextTime)
                            {
                                nextUpdate = running_event.nextTime;
                            }
                        }

                        //calculate what position of all events at next point (update)
                        foreach (Running_Event running_event in running_events)
                        {
                            positions.Add(running_event.GetPosition(nextUpdate));
                        }
                    }

                    if (positions.Count == 0)
                    {
                        //No events are plaing on this device so we can set it back to position 0
                        await Set(0, 1000);
                        continue;
                    }


                    //avarage the positions to get final position
                    int position = 0;
                    foreach (int p in positions)
                    {
                        position += p;
                    }
                    position /= positions.Count;

                    //calculate duration to next point
                    int duration = (int)(nextUpdate - timeNow).TotalMilliseconds;

                    await Set(position, duration);
                }
            }
        }


        private void ForceUpdate()
        {
            //set time to zero so the update loop is force to do a update
            nextUpdate = new DateTime(0);
        }


        private int currentPos = 0;
        public async Task Set(int newpos, int duration)
        {
            if (client == null) return;

            try
            {
                await _clientLock.WaitAsync();
                ButtplugDeviceMessage message = null;

                int deltaPos = Math.Abs(newpos - currentPos);
                if (deltaPos != 0) {
                    currentPos = newpos;

                    if (device.AllowedMessages.ContainsKey(typeof(FleshlightLaunchFW12Cmd)))
                    {
                        Console.WriteLine((float)deltaPos / (float)duration + " " + newpos + " ");
                        uint speed = (uint)Math.Floor(25000 * Math.Pow((duration * 90) / deltaPos, -1.05));
                        speed = Math.Max(Math.Min(speed,90),5);
                        Console.WriteLine("Device.Set \"" +this.name+"\" speed: "+ speed + " position: "+ newpos);
                        message = new FleshlightLaunchFW12Cmd(device.Index, Math.Max(Math.Min(speed, 99), 1), (uint)Math.Max(Math.Min(newpos,99),0) );
                    }



                    if (message != null)
                    {
                        await device.SendMessageAsync(message);
                    }

                    //ButtplugMessage response = await _client.SendDeviceMessage(device, message);
                    //await CheckResponse(response);
                }
            }
            finally
            {
                _clientLock.Release();
            }
        }


        public void SetType(BodyPart bodyPart, EventType eventType, bool set)
        {
            this.type[(int)bodyPart, (int)eventType] = set;
        }

        public bool HasType(BodyPart bodyPart, EventType eventType)
        {
            return this.type[(int)bodyPart, (int)eventType];
        }

        private bool[,] type = new bool[Enum.GetNames(typeof(BodyPart)).Length, Enum.GetNames(typeof(EventType)).Length];

        public override string ToString()
        {
            return name;
        }
    }

    public class Running_Event
    {
        List<FunScriptAction> actions;
        DateTime timeStarted;

        int nextPosition;
        public DateTime nextTime;

        int prevPosition;
        DateTime prevTime;

        int current_step ;
        public bool ended;

        bool synced_by_animation;

        public void End()
        {
            ended = true;
        }

        public Running_Event() : this(new List<FunScriptAction>(),false) { }

        public Running_Event(List<FunScriptAction> actions, bool synced_by_animation)
        {
            this.synced_by_animation = synced_by_animation;
            this.actions = actions;
            this.ended = false;
            Reset();
        }


        public void Reset()
        {
            timeStarted = DateTime.Now;
            prevTime = timeStarted;
            nextTime = timeStarted;
            nextPosition = 0;
            prevPosition = 0;
            current_step = 0;
        }


        public void Update(DateTime date)
        {

            //We dont need to update if time didnt pass
            if (date <= nextTime)
            {
                return;
            }

            //no more steps lets mark it for removal
            if (current_step >= actions.Count)
            {
                if (!synced_by_animation)//uf its synced we dont remove it because the animation might run again.
                {
                    ended = true;      
                }
                return;
            }

            FunScriptAction action = actions[current_step];
            current_step++;

            prevPosition = nextPosition;
            prevTime = nextTime;

            nextPosition = action.Position;
            nextTime = timeStarted + action.TimeStamp;

            while (nextTime < date)
            {
                this.Update(date);
            }
        }

        public int GetPosition(DateTime date)
        {

            if (ended)
            {
                return 0;
            }

            if(date > nextTime)
            {
                return nextPosition;
            }else if (date < prevTime)
            {
                return prevPosition;
            }
            else
            {
                //map position between old and new position based on current time
                int position = (int)((float)(date - prevTime).TotalMilliseconds / (float)(nextTime - prevTime).TotalMilliseconds * (nextPosition - prevPosition)) + prevPosition;
                return Math.Max(Math.Min(position, 99), 0);
            }


        }
    }
}
