using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;
using HarmonyLib;
using System.Runtime.CompilerServices;

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
            }
        }

        public override void StartServerSide(ICoreServerAPI api)
        {
            //Mod.Logger.Notification("Hello from template mod server side: " + Lang.Get("blbrserver:hello"));
        }

        public override void StartClientSide(ICoreClientAPI api)
        {
            // Mod.Logger.Notification("Hello from template mod client side: " + Lang.Get("blbrserver:hello"));

            patcher?.UnpatchAll(Mod.Info.ModID);
        }
    }
}
