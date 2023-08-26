using BepInEx;
using DemoRecorder.Patching;
using R2API.Utils;
using RoR2;
using System;
using System.Diagnostics;

namespace DemoRecorder
{
    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync, VersionStrictness.DifferentModVersionsAreOk)]
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class Main : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "Gorakh";
        public const string PluginName = "DemoRecorder";
        public const string PluginVersion = "1.0.0";

        void Awake()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            Log.Init(Logger);

            PatchManager.ApplyAll();

            Run.onRunStartGlobal += _ =>
            {
                RecorderManager.StartRecording($"{System.IO.Path.GetDirectoryName(Info.Location)}/{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.dem");
            };

            Run.onRunDestroyGlobal += _ =>
            {
                RecorderManager.StopRecording();
            };

            stopwatch.Stop();
            Log.Info_NoCallerPrefix($"Initialized in {stopwatch.Elapsed.TotalSeconds:F2} seconds");
        }

        void OnDestroy()
        {
            PatchManager.UndoAll();
        }
    }
}
