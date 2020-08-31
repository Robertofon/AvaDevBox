using System;
using System.Collections.Generic;
using System.Text;
using Avalonia.Controls.Primitives;

namespace AvaDevBox.Controls
{
    /// <summary>
    /// Represents a bar that can show up or disappear and show a message.
    /// It would usually be docked to the top or bottom using a dock panel with
    /// the actual content.
    /// 
    /// </summary>
    public class NotificationBar : HeaderedContentControl
    {
        public bool IsOpen { get; set; }
    }
}
