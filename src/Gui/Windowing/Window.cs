namespace Blusher.Gui.Windowing;

using System.Runtime.InteropServices;

using Blusher.Drawing;
using Blusher.Gui;
using Blusher.Events;
using Blusher.Swingby;

public class Window
{
    public Window()
    {
        this._ftDesktopSurface = Swingby.sb_desktop_surface_new(
            Swingby.SB_DESKTOP_SURFACE_ROLE_TOPLEVEL);

        // Set the root view color as transparent.
        IntPtr ftSurface = Swingby.sb_desktop_surface_surface(this._ftDesktopSurface);
        IntPtr ftView = Swingby.sb_surface_root_view(ftSurface);
        sb_color_t color = sb_color_t.FromColor(new Color(0, 0, 0, 0));

        IntPtr ftColor = color.AllocCPtr();
        Swingby.sb_view_set_color(ftView, ftColor);
        Marshal.FreeHGlobal(ftColor);

        // Create a root view. Blusher's root view is different from Foundation's root view.
        this._rootView = new View(ftView, new Rect(0.0F, 0.0F, 100.0F, 100.0F));
        this._rootView.Color = new Color(0, 0, 0, 0);

        // Add decoration.
        // - Add shadow.
        this._shadow = new WindowShadow(this);
        // - Add resize.
        this._resize = new WindowResize(_shadow);
        // - Add border.
        this._border = new WindowBorder(this, _resize);
        // - Add title bar.
        this._titleBar = new TitleBar(this, _border);

        // Set surface size.
        this.SurfaceSize = this.CalculateSurfaceSize();
        if (this.HasDecoration) {
            this.UpdateDecoration();
        }

        // Add event listeners.
        this.AddResizeEventListener();
    }

    public void Show()
    {
        Swingby.sb_desktop_surface_show(this._ftDesktopSurface);

        // Set WM geometry. This SHOULD done after Show.
        var wmGeometry = this.CalculateWindowGeometry();
        var ftWmGeometry = sb_rect_t.FromRect(wmGeometry);
        var ftWmGeometryPtr = ftWmGeometry.AllocCPtr();

        Swingby.sb_desktop_surface_set_wm_geometry(_ftDesktopSurface, ftWmGeometryPtr);

        Marshal.FreeHGlobal(ftWmGeometryPtr);
    }

    public View RootView
    {
        get => this._rootView;
    }

    public Size Size
    {
        get => this._geometry.Size;
        set
        {
            this._geometry.Size = value;

            // TODO: Set the root view size.

            // TODO: Set surface size and wm_geometry of desktop surface.
            this.SurfaceSize = this.CalculateSurfaceSize();
            if (this.HasDecoration) {
                this.UpdateDecoration();
            }

            var wmGeometry = this.CalculateWindowGeometry();
            var ftWmGeometry = sb_rect_t.FromRect(wmGeometry);
            var ftWmGeometryPtr = ftWmGeometry.AllocCPtr();

            Swingby.sb_desktop_surface_set_wm_geometry(_ftDesktopSurface, ftWmGeometryPtr);

            Marshal.FreeHGlobal(ftWmGeometryPtr);
        }
    }

    public bool HasDecoration
    {
        get => true;
    }

    public void StartMove()
    {
        Swingby.sb_desktop_surface_toplevel_move(_ftDesktopSurface);
    }

    public void StartResize(ResizeEdge resizeEdge)
    {
        int ftEdge = resizeEdge switch
        {
            ResizeEdge.None => Swingby.SB_DESKTOP_SURFACE_TOPLEVEL_RESIZE_EDGE_NONE,
            ResizeEdge.Top => Swingby.SB_DESKTOP_SURFACE_TOPLEVEL_RESIZE_EDGE_TOP,
            ResizeEdge.Bottom => Swingby.SB_DESKTOP_SURFACE_TOPLEVEL_RESIZE_EDGE_BOTTOM,
            ResizeEdge.Left => Swingby.SB_DESKTOP_SURFACE_TOPLEVEL_RESIZE_EDGE_LEFT,
            ResizeEdge.TopLeft => Swingby.SB_DESKTOP_SURFACE_TOPLEVEL_RESIZE_EDGE_TOP_LEFT,
            ResizeEdge.BottomLeft => Swingby.SB_DESKTOP_SURFACE_TOPLEVEL_RESIZE_EDGE_BOTTOM_LEFT,
            ResizeEdge.Right => Swingby.SB_DESKTOP_SURFACE_TOPLEVEL_RESIZE_EDGE_RIGHT,
            ResizeEdge.TopRight => Swingby.SB_DESKTOP_SURFACE_TOPLEVEL_RESIZE_EDGE_TOP_RIGHT,
            ResizeEdge.BottomRight => Swingby.SB_DESKTOP_SURFACE_TOPLEVEL_RESIZE_EDGE_BOTTOM_RIGHT,
            _ => 0
        };

        Swingby.sb_desktop_surface_toplevel_resize(_ftDesktopSurface, ftEdge);
    }

    private Size SurfaceSize
    {
        set
        {
            IntPtr ftSurface = Swingby.sb_desktop_surface_surface(this._ftDesktopSurface);
            var ftSize = sb_size_t.FromSize(value);
            var ftSizePtr = ftSize.AllocCPtr();

            Swingby.sb_surface_set_size(ftSurface, ftSizePtr);

            Marshal.FreeHGlobal(ftSizePtr);
        }
    }

    /// <summary>
    /// Call this method after add the decoration.
    /// </summary>
    /// <returns></returns>
    private Size CalculateSurfaceSize()
    {
        var windowSize = this.Size;
        if (this._shadow != null) {
            windowSize.Width += (this._shadow.Thickness * 2);
            windowSize.Height += (this._shadow.Thickness * 2);
        }

        return windowSize;
    }

    private Rect CalculateWindowGeometry()
    {
        var surfaceSize = this.CalculateSurfaceSize();

        if (!this.HasDecoration) {
            Rect geo = new Rect(0F, 0F, surfaceSize.Width, surfaceSize.Height);
            return geo;
        } else {
            Rect geo = new Rect(
                _shadow!.Thickness,
                _shadow!.Thickness,
                surfaceSize.Width - (_shadow!.Thickness * 2),
                surfaceSize.Height - (_shadow!.Thickness * 2));
            return geo;
        }
    }

    private void UpdateDecoration()
    {
        var surfaceSize = this.CalculateSurfaceSize();
        if (this._shadow != null) {
            this._shadow.Geometry = new Rect(0F, 0F, surfaceSize.Width, surfaceSize.Height);
        }

        if (this._shadow != null && this._resize != null) {
            var shadowSize = _shadow.Geometry.Size;
            this._resize.Geometry = new Rect(
                _shadow.Thickness - _resize.Thickness,
                _shadow.Thickness - _resize.Thickness,
                shadowSize.Width - (_shadow.Thickness * 2) + (_resize.Thickness * 2),
                shadowSize.Height - (_shadow.Thickness * 2) + (_resize.Thickness * 2)
            );
        }

        if (_border != null) {
            var resizeSize = _resize!.Geometry.Size;
            _border.Geometry = new Rect(
                _resize!.Thickness - _border.Thickness,
                _resize!.Thickness - _border.Thickness,
                resizeSize.Width - (_resize.Thickness * 2) + (_border.Thickness * 2),
                resizeSize.Height - (_resize.Thickness * 2) + (_border.Thickness * 2)
            );
        }

        if (_titleBar != null) {
            var borderSize = _border!.Geometry.Size;
            _titleBar.Geometry = new Rect(
                _border!.Thickness,
                _border!.Thickness,
                borderSize.Width - (_border!.Thickness * 2),
                _titleBar.Thickness
            );
        }
    }

    protected virtual void ResizeEvent(ResizeEvent evt)
    {
        Console.WriteLine("Window resize event.");
        Console.WriteLine(evt.Size.Width);
        Console.WriteLine(evt.Size.Height);
        this.Size = evt.Size;
    }

    private void AddResizeEventListener()
    {
        var eventListener = new Swingby.EventListener(this.CallResizeEvent);
        Swingby.sb_desktop_surface_add_event_listener(this._ftDesktopSurface, Swingby.SB_EVENT_TYPE_RESIZE, eventListener);
    }

    private void CallResizeEvent(IntPtr ftEvent)
    {
        IntPtr oldSize = Swingby.sb_event_resize_size(ftEvent);
        var oldWidth = Swingby.sb_size_width(oldSize);
        var oldHeight = Swingby.sb_size_height(oldSize);
        IntPtr size = Swingby.sb_event_resize_size(ftEvent);
        var width = Swingby.sb_size_width(size);
        var height = Swingby.sb_size_height(size);

        ResizeEvent evt = new ResizeEvent(new Size(oldWidth, oldHeight), new Size(width, height));
        this.ResizeEvent(evt);
    }

    private IntPtr _ftDesktopSurface;
    private WindowShadow? _shadow;
    private WindowResize? _resize;
    private WindowBorder? _border;
    private TitleBar? _titleBar;
    private View _rootView;
    private Rect _geometry = new Rect(0.0F, 0.0F, 200.0F, 200.0F);
}
