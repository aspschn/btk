namespace Btk.Gui.Widgets;

using Btk.Drawing;

public class Widget : View
{
    public Widget(Widget parent) : base(parent, new Rect(0.0f, 0.0f, 10.0f, 10.0f))
    {
        //
    }

    public Widget(View parent) : base(parent, new Rect(0.0f, 0.0f, 10.0f, 10.0f))
    {
        //
    }
}
