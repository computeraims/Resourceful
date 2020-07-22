using HarmonyLib;

namespace Resourceful.Utils
{
    public class Patcher
    {
        public static void DoPatching()
        {
            var harmony = new Harmony("com.example.patch");

            harmony.PatchAll();
        }
    }
}
