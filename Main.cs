using Resourceful.Utils;
using SDG.Framework.Modules;
using System;
using UnityEngine;

namespace Resourceful
{
    public class Main : MonoBehaviour, IModuleNexus
    {
        private static GameObject ResourcefulObject;

        public static Main Instance;

        public void initialize()
        {
            Instance = this;
            Console.WriteLine("Resourceful by Corbyn loaded");

            Patcher patch = new Patcher();
            Patcher.DoPatching();

            ResourcefulObject = new GameObject("Resourceful");
            DontDestroyOnLoad(ResourcefulObject);

            ResourcefulObject.AddComponent<ResourcefulManager>();
        }

        public void shutdown()
        {
            Instance = null;
        }
    }
}
