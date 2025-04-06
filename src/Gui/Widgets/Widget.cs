namespace Btk.Gui.Widgets;

using Btk.Events;
using Btk.Drawing;

public class Widget : View
{
    public Widget(Widget parent) : base(parent, new Rect(0.0f, 0.0f, 10.0f, 10.0f))
    {
        base.Clip = true;
    }

    public Widget(View parent) : base(parent, new Rect(0.0f, 0.0f, 10.0f, 10.0f))
    {
        base.Clip = true;
    }

    public float Width
    {
        get => base.Geometry.Width;
        set => base.Geometry = new Rect(base.Geometry.X, base.Geometry.Y, value, base.Geometry.Height);
    }

    public float Height
    {
        get => base.Geometry.Height;
        set => base.Geometry = new Rect(base.Geometry.X, base.Geometry.Y, base.Geometry.Width, value);
    }
}
