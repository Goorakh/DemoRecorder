using System;
using System.Collections.Generic;
using System.Text;

namespace DemoRecorder.Patching
{
    abstract class PatchInfo
    {
        public virtual string Identifier => GetType().Name;

        public virtual bool ShouldBeActive => true;

        public bool IsActive { get; private set; }

        public void TryApply()
        {
            if (IsActive || !ShouldBeActive)
                return;

            try
            {
                apply();
            }
            catch (Exception e)
            {
                Log.Error_NoCallerPrefix($"Failed to apply patch {Identifier}: {e}");
            }
        }

        protected abstract void apply();

        public void TryUndo()
        {
            if (!IsActive)
                return;

            try
            {
                undo();
            }
            catch (Exception e)
            {
                Log.Error_NoCallerPrefix($"Failed to undo patch {Identifier}: {e}");
            }
        }

        protected abstract void undo();
    }
}
