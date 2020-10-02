using System;
using System.Collections.Generic;
using System.Text;
using Avalonia.Controls.Shapes;
using Avalonia.Media;

namespace AvaDevBox.Controls.Shapes
{
    /// <summary>
    /// Shall become a seven (or eight) segment shape where Input is one
    /// byte. Each bit is one segment and the segments show up in the
    /// activated color and their shape is implemented in here.
    /// </summary>
    class SevenSegmentShape : Shape
    {
        protected override Geometry CreateDefiningGeometry()
        {
            throw new NotImplementedException();
        }
    }
}
