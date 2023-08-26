using System;
using System.Linq;

namespace DemoRecorder.Patching
{
    static class PatchManager
    {
        static readonly PatchInfo[] _patchInfos = PatchAttribute.GetInstances<PatchAttribute>()
                                                                .Select(s => s.target as Type)
                                                                .Where(t => t != null)
                                                                .Select(t => Activator.CreateInstance(t) as PatchInfo)
                                                                .ToArray();

        public static void ApplyAll()
        {
            foreach (PatchInfo patch in _patchInfos)
            {
                patch.TryApply();
            }
        }

        public static void UndoAll()
        {
            foreach (PatchInfo patch in _patchInfos)
            {
                patch.TryUndo();
            }
        }
    }
}
