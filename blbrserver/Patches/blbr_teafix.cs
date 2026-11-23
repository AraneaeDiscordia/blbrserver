using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Text;
using HarmonyLib;
using Vintagestory.API.Common;

namespace blbrserver.Patches
{
    [HarmonyPatchCategory("blbrserver")]
    internal static class BlbrTeaFix
    {
        public static void AddHerbsToExtractHerbCode(string fullCode, ref string __result)
        {
            // bounce if the result already returned something other than null
            if (__result != null) return;

            // split the code into useful chunks
            int num = fullCode.IndexOf(':');
            string[] array = ((num >= 0) ? fullCode.Substring(num + 1) : fullCode).Split('-');
            // if the expected array chunk contains herb, return it as the expected value
            if (array.Length >= 2 && array[0] == "herb") __result = array[1];
        }

        public static void AddHerbsToCanHold(ItemSlot sourceSlot, ref bool __result)
        {
            // bounce if the result is already true
            if (__result == true) return;

            // convert code to string
            var code = sourceSlot?.Itemstack?.Collectible?.Code?.ToString();

            // bounce if the code is null
            if (code == null) return;
            
            // return result as true if code contains :herb- and -dried
            if (code.Contains(":herb-") && code.Contains("-dried")) __result = true;

        }
    }
}
