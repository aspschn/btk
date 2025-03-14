namespace Btk.Gui.Windowing;

using Btk.Drawing;
using Btk.Gui;

public class TitleBar : View, IWindowDecoration
{
    private View _closeButton;

    public TitleBar(Window window, View parent) : base(parent, new Rect(0F, 0F, 10F, 10F))
    {
        _thickness = 30;
        _window = window;
        Pressed = false;
        Color = new Color(128, 128, 128, 255);

        // Close button test.
        _closeButton = new View(this, new Rect(5.0f, 5.0f, 20.0f, 20.0f));
        _closeButton.Color = new Color(255, 0, 0, 255);
        _closeButton.OnPointerPress += (v, evt) =>
        {
            _window.Close();
            evt.Propagation = false;
        };
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
            Pressed = false;
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
