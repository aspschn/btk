namespace Blusher.Gui.Windowing;

using Blusher.Drawing;

public class WindowResize : View
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
