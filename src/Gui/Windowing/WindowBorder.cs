namespace Blusher.Gui.Windowing;

using Blusher.Drawing;
using Blusher.Gui;

public class WindowBorder : View, IWindowDecoration
{
    public WindowBorder(Window window, View parent) : base(parent, new Rect(0F, 0F, 0F, 0F))
    {
        _thickness = 1;
        _window = window;

        this.Color = new Color(100, 100, 100, 255);
    }

    public uint Thickness
    {
        get => _thickness;
    }

    private uint _thickness;
    private Window _window;
}
