using blbrserver.Patches;
using HarmonyLib;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;
using System;

namespace blbrserver
{
    public class BlbrserverModSystem : ModSystem
    {
        // harmony patcher instance
        private Harmony patcher;

        // Called on server and client
        // Useful for registering block/entity classes on both sides
        public override void Start(ICoreAPI api)
        {
            //Mod.Logger.Notification("Hello from template mod: " + api.Side);

            // double instance check
            if (!Harmony.HasAnyPatches(Mod.Info.ModID))
            {
                patcher = new Harmony(Mod.Info.ModID);
                patcher.PatchCategory(Mod.Info.ModID);

                //Mod.Logger.Notification("Attempting BlBrServer Harmony Patches");

                var targetMod = "steepandbrew";
                if (api.ModLoader.IsModEnabled(targetMod))
                {
                    try
                    {
                        var targetType = AccessTools.TypeByName("SteepAndBrew.BlockEntitySteepingJar");
                        var targetMethod = AccessTools.Method(targetType, "ExtractHerbCode");

                        patcher.Patch(targetMethod, postfix: new HarmonyMethod(typeof(BlbrTeaFix), nameof(BlbrTeaFix.AddHerbsToExtractHerbCode)));
                        //Mod.Logger.Notification($"{targetMod} patched by BlBrServer");

                    }
                    catch(Exception)
                    {
                        Mod.Logger.Notification($"{targetMod} patch failed by BlBrServer");
                    }

                    try
                    {
                        var targetType = AccessTools.TypeByName("SteepAndBrew.ItemSlotKettleInput");
                        var targetMethod = AccessTools.Method(targetType, "CanHold");

                        patcher.Patch(targetMethod, postfix: new HarmonyMethod(typeof(BlbrTeaFix), nameof(BlbrTeaFix.AddHerbsToCanHold)));
                       // Mod.Logger.Notification($"{targetMod} patched by BlBrServer");

                    }
                    catch (Exception)
                    {
                        Mod.Logger.Notification($"{targetMod} patch failed by BlBrServer");
                    }
                }
            }
        }

        public override void StartServerSide(ICoreServerAPI api)
        {
            //Mod.Logger.Notification("Hello from template mod server side: " + Lang.Get("blbrserver:hello"));
        }

        public override void StartClientSide(ICoreClientAPI api)
        {
            // Mod.Logger.Notification("Hello from template mod client side: " + Lang.Get("blbrserver:hello"));
        }

        public override void Dispose()
        {
            patcher?.UnpatchAll(Mod.Info.ModID);

        }
    }
}
