namespace Btk.Gui.Widgets;

using Btk.Drawing;
using Btk.Events;
using Btk.Input;
using Btk.Gui.Widgets;

public class PushButton : Widget
{
    private string _text;
    private Widget _outer;
    private Widget _inner;
    private Label _label;

    private readonly Color _outerColor = new Color(62, 62, 62, 255);
    private readonly Color _innerColor = new Color(94, 94, 94, 255);
    private readonly Color _innerPressedColor = new Color(46, 46, 46, 255);

    public PushButton(Widget parent) : this("", parent)
    {
    }

    public PushButton(string text, Widget parent) : base(parent)
    {
        base.Width = 100.0f;
        base.Height = 30.0f;
        _outer = new Widget(this);
        _outer.Color = _outerColor;
        _outer.Anchors.Fill = this;
        _inner = new Widget(_outer);
        _inner.Color = _innerColor;
        _inner.Width = 90.0f;
        _inner.Height = 20.0f;
        _inner.Anchors.CenterIn = _outer;
        _label = new Label(text, _inner);
        _text = text;
    }

    protected override void PointerPressEvent(PointerEvent evt)
    {
        _inner.Color = _innerPressedColor;

        base.PointerPressEvent(evt);
    }

    protected override void PointerReleaseEvent(PointerEvent evt)
    {
        _inner.Color = _innerColor;

        base.PointerReleaseEvent(evt);
    }

    protected override void PointerClickEvent(PointerEvent evt)
    {
        if (evt.Button == PointerButton.Left)
        {
            base.PointerClickEvent(evt);
        }
    }

    public string Text
    {
        get => _text;
        set
        {
            _text = value;
            _label.Text = value;
        }
    }
}
