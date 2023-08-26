using System;

namespace DemoRecorder.Patching
{
    [AttributeUsage(AttributeTargets.Class)]
    sealed class PatchAttribute : HG.Reflection.SearchableAttribute
    {
    }
}
