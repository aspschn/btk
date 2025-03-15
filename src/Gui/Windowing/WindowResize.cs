namespace Btk.Gui.Windowing;

using Btk.Drawing;
using Btk.Gui.Rendering;

internal class ResizeEdge : View
{
    private SurfaceResizeEdge _edge;
    private Window _window;
    private bool _pressed = false;

    public ResizeEdge(SurfaceResizeEdge edge, View parent, Window window) : base(parent, new Rect(0.0f, 0.0f, 15.0f, 15.0f))
    {
        _edge = edge;
        _window = window;
    }

    protected override void PointerPressEvent(PointerEvent evt)
    {
        _pressed = true;

        base.PointerPressEvent(evt);
    }

    protected override void PointerMoveEvent(PointerEvent evt)
    {
        if (_pressed) {
            _window.StartResize(_edge);
            _pressed = false;
        }

        evt.Propagation = false;

        base.PointerMoveEvent(evt);
    }
}

public class WindowResize : View, IWindowDecoration
{
    private uint _thickness;
    private Window _window;
    private ResizeEdge _bottomRightEdge;

    public WindowResize(Window window, View parent) : base(parent, new Rect(0.0F, 0.0F, 10.0F, 10.0F))
    {
        _thickness = 5;
        _window = window;
        _bottomRightEdge = new ResizeEdge(SurfaceResizeEdge.BottomRight, this, window);

        this.Color = new Color(0, 255, 0, 100);
    }

    public void UpdateResizeEdges()
    {
        _bottomRightEdge.Geometry = new Rect(
            Geometry.Width - 15.0f,
            Geometry.Height - 15.0f,
            15.0f,
            15.0f);
    }

    public uint Thickness
    {
        get => this._thickness;
    }
}
