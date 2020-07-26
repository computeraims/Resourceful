using Resourceful.Utils;
using SDG.Framework.Modules;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Resourceful
{
    public class Main : MonoBehaviour, IModuleNexus
    {
        private static GameObject ResourcefulObject;

        public static Main Instance;

        public static Config Config;

        public void initialize()
        {
            Instance = this;
            Console.WriteLine("Resourceful by Corbyn loaded");

            Patcher patch = new Patcher();
            Patcher.DoPatching();

            ResourcefulObject = new GameObject("Resourceful");
            DontDestroyOnLoad(ResourcefulObject);

            string callingPath = $"{Path.GetDirectoryName(Assembly.GetCallingAssembly().Location)}";

            DirectoryInfo configDirectory = Directory.CreateDirectory($"{callingPath}/config");

            ConfigHelper.EnsureConfig($"{callingPath}/config/resourceful.json");

            Config = ConfigHelper.ReadConfig($"{callingPath}/config/resourceful.json");

            ResourcefulObject.AddComponent<ResourcefulManager>();
        }

        public void shutdown()
        {
            Instance = null;
        }
    }
}
