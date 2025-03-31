namespace Btk.Gui.Widgets;

using Btk.Drawing;

public class ProgressBar : Widget
{
    private float _value = 0.0f;
    private View _border;
    private View _progress;

    public ProgressBar(Widget parent) : base(parent)
    {
        base.Geometry = new Rect(0.0f, 0.0f, 100.0f, 30.0f);
        base.Color = new Color(0, 0, 0, 0);

        _border = new View(this, new Rect(0.0f, 0.0f, 90.0f, 10.0f));
        _border.Color = new Color(128, 128, 128, 255);
        _border.Radius = new ViewRadius(15.0f, 15.0f, 15.0f, 15.0f);
        _border.Anchors.CenterIn = this;
        _border.Clip = true;
        _progress = new View(_border, new Rect(0.0f, 1.0f, 0.0f, 8.0f));
        _progress.Color = new Color(0, 0, 255, 255);
    }

    public float Value
    {
        get => _value;
        set
        {
            _value = value;
            float prog = value * (_border.Geometry.Width + 2.0f);
            _progress.Geometry = new Rect(_progress.Geometry.X, _progress.Geometry.Y, prog, _progress.Geometry.Height);
        }
    }
}
