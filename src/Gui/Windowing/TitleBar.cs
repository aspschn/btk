namespace Blusher.Gui.Windowing;

using Blusher.Drawing;
using Blusher.Gui;

public class TitleBar : View, IWindowDecoration
{
    public TitleBar(Window window, View parent) : base(parent, new Rect(0F, 0F, 10F, 10F))
    {
        _thickness = 30;
        _window = window;
        this.Pressed = false;
    }

    public uint Thickness
    {
        get => _thickness;
    }

    private bool Pressed { get; set; }

    protected override void PointerMoveEvent(PointerEvent evt)
    {
        if (Pressed) {
            _window.StartMove();
        }

        evt.Propagation = false;

        base.PointerMoveEvent(evt);
    }

    protected override void PointerPressEvent(PointerEvent evt)
    {
        Console.WriteLine(">>> Title bar pressed.");
        Pressed = true;

        base.PointerPressEvent(evt);
    }

    protected override void PointerReleaseEvent(PointerEvent evt)
    {
        Pressed = false;

        base.PointerReleaseEvent(evt);
    }

    private uint _thickness;
    private Window _window;
}
