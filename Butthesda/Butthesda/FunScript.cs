/*
By: FredTungsten
    Source: https://github.com/FredTungsten/ScriptPlayer
Edited by: mr.private
    moved all required code for reading funcscript in a single file
*/

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Butthesda
{

    public class FunScriptAction
    {

        [JsonIgnore]
        public TimeSpan TimeStamp;

        [JsonIgnore]
        public bool OriginalAction { get; set; }

        [JsonProperty(PropertyName = "pos")]
        public byte Position { get; set; }


        [JsonProperty(PropertyName = "at")]
        public long TimeStampWrapper
        {
            get => TimeStamp.Ticks / TimeSpan.TicksPerMillisecond;
            set => TimeStamp = TimeSpan.FromTicks(value * TimeSpan.TicksPerMillisecond);
        }

        public FunScriptAction Duplicate()
        {
            return new FunScriptAction
            {
                Position = Position,
                TimeStamp = TimeStamp,
                OriginalAction = OriginalAction
            };
        }
    }


    public static class FunScriptLoader
	{
        public static List<FunScriptAction> Load(string filename)
        {
            //wait for file to load
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    using (File.OpenRead(filename)) { }
                }
                catch (IOException)
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(50));
                }
            }

            using (FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
            using (StreamReader reader = new StreamReader(stream, new UTF8Encoding(false)))
            {
                string content = reader.ReadToEnd();
                var file = JsonConvert.DeserializeObject<FunScriptFile>(content);

                if (file.Inverted)
                    file.Actions.ForEach(a => a.Position = (byte)(99 - a.Position));

                var actions = file.Actions.Cast<FunScriptAction>().Where(a => a.TimeStamp >= TimeSpan.Zero).ToList();
                return actions;
            }
        }


        public class StringToVersionConverter : JsonConverter
        {
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                Version version = (Version)value;
                writer.WriteValue($"{version.Major}.{version.Minor}");
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                Version version = Version.Parse((string)reader.Value);
                return version;
            }

            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(Version);
            }
        }


        public class FunScriptFile
        {
            [JsonProperty(PropertyName = "version")]
            [JsonConverter(typeof(StringToVersionConverter))]
            public Version Version { get; set; }

            [JsonProperty(PropertyName = "inverted")]
            public bool Inverted { get; set; }

            [JsonProperty(PropertyName = "range")]
            public int Range { get; set; }

            [JsonProperty(PropertyName = "actions")]
            public List<FunScriptAction> Actions { get; set; }

            public FunScriptFile()
            {
                Inverted = false;
                Version = new Version(1, 0);
                Range = 90;
                Actions = new List<FunScriptAction>();
            }

            public void Save(string filename)
            {
                string content = JsonConvert.SerializeObject(this);

                File.WriteAllText(filename, content, new UTF8Encoding(false));
            }
        }
    }
}
