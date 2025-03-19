namespace Btk.Gui.Windowing;

using Btk.Drawing;
using Btk.Gui.Rendering;
using Btk.Events;

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
    private ResizeEdge _topLeftEdge;
    private ResizeEdge _topEdge;
    private ResizeEdge _topRightEdge;
    private ResizeEdge _rightEdge;
    private ResizeEdge _bottomRightEdge;
    private ResizeEdge _bottomEdge;
    private ResizeEdge _bottomLeftEdge;
    private ResizeEdge _leftEdge;

    public WindowResize(Window window, View parent) : base(parent, new Rect(0.0F, 0.0F, 10.0F, 10.0F))
    {
        _thickness = 10;
        _window = window;
        _topLeftEdge = new ResizeEdge(SurfaceResizeEdge.TopLeft, this, window);
        _topLeftEdge.CursorShape = CursorShape.NwseResize;
        _topEdge = new ResizeEdge(SurfaceResizeEdge.Top, this, window);
        _topEdge.CursorShape = CursorShape.NSResize;
        _topRightEdge = new ResizeEdge(SurfaceResizeEdge.TopRight, this, window);
        _topRightEdge.CursorShape = CursorShape.NeswResize;
        _rightEdge = new ResizeEdge(SurfaceResizeEdge.Right, this, window);
        _rightEdge.CursorShape = CursorShape.EWResize;
        _bottomEdge = new ResizeEdge(SurfaceResizeEdge.Bottom, this, window);
        _bottomEdge.CursorShape = CursorShape.NSResize;
        _bottomRightEdge = new ResizeEdge(SurfaceResizeEdge.BottomRight, this, window);
        _bottomRightEdge.CursorShape = CursorShape.NwseResize;
        _bottomLeftEdge = new ResizeEdge(SurfaceResizeEdge.BottomLeft, this, window);
        _bottomLeftEdge.CursorShape = CursorShape.NeswResize;
        _leftEdge = new ResizeEdge(SurfaceResizeEdge.Left, this, window);
        _leftEdge.CursorShape = CursorShape.EWResize;

        this.Color = new Color(0, 0, 0, 0);
    }

    public void UpdateResizeEdges()
    {
        _topLeftEdge.Geometry = new Rect(
            0.0f,
            0.0f,
            15.0f,
            15.0f);
        _topEdge.Geometry = new Rect(
            15.0f,
            0.0f,
            Geometry.Width - (15.0f * 2),
            15.0f);
        _topRightEdge.Geometry = new Rect(
            Geometry.Width - 15.0f,
            0.0f,
            15.0f,
            15.0f);
        _rightEdge.Geometry = new Rect(
            Geometry.Width - 15.0f,
            15.0f,
            15.0f,
            Geometry.Height - (15.0f * 2));
        _bottomRightEdge.Geometry = new Rect(
            Geometry.Width - 15.0f,
            Geometry.Height - 15.0f,
            15.0f,
            15.0f);
        _bottomEdge.Geometry = new Rect(
            15.0f,
            Geometry.Height - 15.0f,
            Geometry.Width - (15.0f * 2),
            15.0f);
        _bottomLeftEdge.Geometry = new Rect(
            0.0f,
            Geometry.Height - 15.0f,
            15.0f,
            15.0f);
        _leftEdge.Geometry = new Rect(
            0.0f,
            15.0f,
            15.0f,
            Geometry.Height - (15.0f * 2));
    }

    public uint Thickness
    {
        get => this._thickness;
    }
}
