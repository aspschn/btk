namespace Btk.Gui.Windowing;

using Btk.Drawing;

public class WindowResize : View, IWindowDecoration
{
    public WindowResize(View parent) : base(parent, new Rect(0.0F, 0.0F, 10.0F, 10.0F))
    {
        this._thickness = 5;

        this.Color = new Color(0, 255, 0, 100);
    }

    public uint Thickness
    {
        get => this._thickness;
    }

    private uint _thickness;
}
