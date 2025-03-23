namespace Btk.Gui.Windowing;

using Btk.Drawing;
using Btk.Gui;

public class WindowBorder : View, IWindowDecoration
{
    private uint _thickness;
    private Window _window;
    private View _topEdge;
    private View _rightEdge;
    private View _bottomEdge;
    private View _leftEdge;

    public WindowBorder(Window window, View parent) : base(parent, new Rect(0F, 0F, 0F, 0F))
    {
        _thickness = 1;
        _window = window;

        this.Color = new Color(0, 0, 0, 0);

        _topEdge = new View(this, new Rect(0.0f, 0.0f, Geometry.Width, _thickness));
        _rightEdge = new View(this, new Rect(Geometry.Width - _thickness, 0.0f, _thickness, Geometry.Height));
        _bottomEdge = new View(this, new Rect(0.0f, Geometry.Height - _thickness, Geometry.Width, _thickness));
        _leftEdge = new View(this, new Rect(0.0f, 0.0f, _thickness, Geometry.Height));

        _topEdge.Color = new Color(100, 100, 100, 255);
        _rightEdge.Color = new Color(100, 100, 100, 255);
        _bottomEdge.Color = new Color(100, 100, 100, 255);
        _leftEdge.Color = new Color(100, 100, 100, 255);
    }

    public uint Thickness
    {
        get => _thickness;
    }

    public bool Activated { get; set; }

    public void UpdateBorderEdges()
    {
        _topEdge.Geometry = new Rect(0.0f, 0.0f, Geometry.Width, _thickness);
        _rightEdge.Geometry = new Rect(Geometry.Width - _thickness, 0.0f, _thickness, Geometry.Height);
        _bottomEdge.Geometry = new Rect(0.0f, Geometry.Height - _thickness, Geometry.Width, _thickness);
        _leftEdge.Geometry = new Rect(0.0f, 0.0f, _thickness, Geometry.Height);
    }
}
