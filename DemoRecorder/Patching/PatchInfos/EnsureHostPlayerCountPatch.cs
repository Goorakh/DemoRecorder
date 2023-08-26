using HarmonyLib;
using MonoMod.RuntimeDetour;
using RoR2.Networking;
using System.Reflection;

namespace DemoRecorder.Patching.PatchInfos
{
    [Patch]
    sealed class EnsureHostPlayerCountPatch : PatchInfo
    {
        delegate void orig_set_desiredHost(NetworkManagerSystem self, HostDescription value);

        static readonly MethodInfo _desiredHostSetter = AccessTools.DeclaredPropertySetter(typeof(NetworkManagerSystem), nameof(NetworkManagerSystem.desiredHost));

        static EnsureHostPlayerCountPatch()
        {
            if (_desiredHostSetter == null)
            {
                Log.Error("Could not find NetworkManagerSystem.set_desiredHost");
            }
        }

        public override bool ShouldBeActive => base.ShouldBeActive && _desiredHostSetter != null;

        Hook _hook;

        protected override void apply()
        {
            _hook = new Hook(_desiredHostSetter, (orig_set_desiredHost orig, NetworkManagerSystem self, HostDescription value) =>
            {
                if (value.hostType == HostDescription.HostType.Self)
                {
                    // Add extra player for fake client listener
                    value.hostingParameters.listen = true;
                    value.hostingParameters.maxPlayers++;
                }

                orig(self, value);
            });
        }

        protected override void undo()
        {
            _hook?.Undo();
        }
    }
}
