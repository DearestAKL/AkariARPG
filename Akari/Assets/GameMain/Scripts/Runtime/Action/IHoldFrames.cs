using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akari
{
    /// <summary>
    /// IHoldFrames
    /// </summary>
    public interface IHoldFrames
    {
        int GetBeginFrame();

        void SetBeginFrame(int frameIndex);

        int GetEndFrame();

        void SetEndFrame(int frameIndex);

        bool EnableBeginEnd();

        bool EnableLoop();
    }

    [Serializable]
    public abstract class HoldFrames : IHoldFrames
    {
        #region IHoldFrames

        [EnableToggle()]
        public bool enableBeginEnd = true;

        [EnableToggleItem(nameof(enableBeginEnd))]
        public int beginFrame;

        [EnableToggleItem(nameof(enableBeginEnd))]
        public int endFrame;

        /// <summary>
        /// enableBeginEnd 为 true 时，才有效
        /// </summary>
        [EnableToggleItem(nameof(enableBeginEnd))]
        public bool enableLoop = true;

        public bool EnableLoop() => enableLoop;

        public bool EnableBeginEnd() => enableBeginEnd;

        public int GetBeginFrame() => beginFrame;

        public void SetBeginFrame(int frameIndex) => beginFrame = frameIndex;

        public int GetEndFrame() => endFrame;

        public void SetEndFrame(int frameIndex) => endFrame = frameIndex;

        #endregion IHoldFrames
    }
}
