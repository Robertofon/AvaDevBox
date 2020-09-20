using Avalonia.Controls.Primitives;
using Avalonia.Layout;

namespace AvaDevBox.Controls
{
    /// <summary>
    /// Control like a volume control knob. Round shape towards the user.
    /// The user finds an indicator set in an angle corresponding to the
    /// range the programmer has set. Tuning to the right value works by
    /// selecting the correct angle. It supports a dead angle down to 0 degrees.
    /// And the dead area can be set by orientation values.
    /// A Header can be set and hidden.
    /// </summary>
    public class VolumeKnobControl : HeaderedContentControl
    {
        private bool IsHeaderShown;
        

    }
}