using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resourceful.Utils
{
    public class Config
    {
        public ushort[] items { get; set; }
    }

    public class ConfigHelper
    {
        public static void EnsureConfig(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("No config.json");

                JArray array = new JArray();
                array.Add(67); // Metal Scrap
                array.Add(37); // Birch Log
                array.Add(39); // Maple Log
                array.Add(41); // Pine Log
                array.Add(38); // Birch Stick
                array.Add(40); // Maple Stick
                array.Add(42); // Pine Stick

                JObject resourcefulConfig = new JObject();
                resourcefulConfig["items"] = array;

                // write JSON directly to a file
                using (StreamWriter file = File.CreateText(path))
                using (JsonTextWriter writer = new JsonTextWriter(file))
                {
                    resourcefulConfig.WriteTo(writer);
                    Console.WriteLine("Creating Resourceful config");
                }
            }
        }

        public static Config ReadConfig(string path)
        {
            using (StreamReader file = File.OpenText(path))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                return JsonConvert.DeserializeObject<Config>(JToken.ReadFrom(reader).ToString());
            }
        }
    }
}
