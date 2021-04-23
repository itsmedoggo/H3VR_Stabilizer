using HarmonyLib;
using Deli.Setup;
using FistVR;
using UnityEngine;

namespace Stabilizer
{
    public class Plugin : DeliBehaviour
    {
        public readonly string GUID;
        public readonly string STABILIZER_GUID;
        public static Plugin Instance { get; private set; }

        public Plugin()
        {
            GUID = Info.Guid;
            STABILIZER_GUID = GUID + ".stabilizer";
            Instance = this;
            Harmony st = new Harmony(STABILIZER_GUID);
            st.PatchAll();
        }
    }

    [HarmonyPatch(typeof(FVRFireArm), "IsTwoHandStabilized")]
    internal class Patch
    {
        [HarmonyPrefix]
        static bool IsTwoHandStabilized_Patch(FVRFireArm __instance, ref bool __result)
        {
            __result = false;
            if (__instance.m_hand != null && __instance.m_hand.OtherHand != null)
            {
                float num = Vector3.Distance(__instance.m_hand.PalmTransform.position, __instance.m_hand.OtherHand.PalmTransform.position);
                if (num < 0.15f)
                {
                    __result = true;
                }
            }
            return false;
        }
    }
}
