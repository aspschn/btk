namespace Blusher.Gui.Windowing;

using Blusher.Drawing;
using Blusher.Gui;

public class TitleBar : View, IWindowDecoration
{
    public TitleBar(View parent) : base(parent, new Rect(0F, 0F, 10F, 10F))
    {
        _thickness = 30;
    }

    public uint Thickness
    {
        get => _thickness;
    }

    protected override void PointerPressEvent(PointerEvent evt)
    {
        Console.WriteLine("Title bar pressed.");
        base.PointerPressEvent(evt);
    }

    private uint _thickness;
}
