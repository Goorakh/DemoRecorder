using RoR2.Networking;

namespace DemoRecorder.Patching.PatchInfos
{
    [Patch]
    sealed class ForceServerHostInSingleplayerPatch : PatchInfo
    {
        protected override void apply()
        {
            On.RoR2.Networking.NetworkManagerSystem.StartSinglePlayer += NetworkManagerSystem_StartSinglePlayer;
        }

        protected override void undo()
        {
            On.RoR2.Networking.NetworkManagerSystem.StartSinglePlayer -= NetworkManagerSystem_StartSinglePlayer;
        }

        static void NetworkManagerSystem_StartSinglePlayer(On.RoR2.Networking.NetworkManagerSystem.orig_StartSinglePlayer orig, NetworkManagerSystem self)
        {
            orig(self);

            self.desiredHost = new HostDescription(new HostDescription.HostingParameters
            {
                listen = true,
                maxPlayers = 1
            });
        }
    }
}
