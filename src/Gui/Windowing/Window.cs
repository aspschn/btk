namespace Blusher.Gui.Windowing;

using System.Runtime.InteropServices;

using Blusher.Drawing;
using Blusher.Gui;
using Blusher.Gui.Rendering;
using Blusher.Events;
using Blusher.Swingby;

public class Window : Surface
{
    public Window() : base(SurfaceRole.Toplevel)
    {
        // Set the root view color as transparent.
        base.RootViewColor = new Color(0, 0, 0, 0);

        IntPtr sbSurface = Swingby.sb_desktop_surface_surface(this._sbDesktopSurface);
        IntPtr sbRootView = Swingby.sb_surface_root_view(sbSurface);

        // Add decoration.
        // - Add shadow.
        this._shadow = new WindowShadow(this, sbRootView);

        // Create a body view.
        // THIS ORDER IS IMPORTANT ELSE IT CRASH. REASON WHY I DON'T KNOW.
        _body = new View(sbRootView, new Rect(0.0f, 0.0f, 200.0f, 200.0f));
        _body.Color = new Color(255, 255, 255, 255);

        // - Add resize.
        this._resize = new WindowResize(_shadow);
        // - Add border.
        this._border = new WindowBorder(this, _resize);
        // - Add title bar.
        this._titleBar = new TitleBar(this, _border);

        // Set surface size.
        this.SurfaceSize = this.CalculateSurfaceSize();
        _body.Geometry = CalculateBodyGeometry();
        if (this.HasDecoration) {
            this.UpdateDecoration();
        }

        // Add event listeners.
        this.AddResizeEventListener();
    }

    public void Show()
    {
        base.Show();

        // Set WM geometry. This SHOULD be done after Show.
        base.WMGeometry = CalculateWindowGeometry();
    }

    /// <summary>
    /// Size of the window frame. The frame contain it's border and the title bar.
    /// </summary>
    public Size FrameSize
    {
        get
        {
            // TODO: Implementation.
            return new Size(0f, 0f);
        }
    }

    /// <summary>
    /// Size of the body area.
    /// </summary>
    public Size Size
    {
        get => this._body.Geometry.Size;
        set
        {
            Console.WriteLine("Size setter");
            var prevGeo = _body.Geometry;
            _body.Geometry = new Rect(prevGeo.X, prevGeo.Y, value.Width, value.Height);

            // TODO: Set the root view size.

            // TODO: Set surface size and wm_geometry of desktop surface.
            this.SurfaceSize = this.CalculateSurfaceSize();
            if (this.HasDecoration) {
                this.UpdateDecoration();
            }

            // Update WM geometry.
            // TODO.
        }
    }

    public bool HasDecoration
    {
        get => true;
    }

    public View Body => _body;

    public void StartMove()
    {
        Swingby.sb_desktop_surface_toplevel_move(base._sbDesktopSurface);
    }

    public void StartResize(ResizeEdge resizeEdge)
    {
        int sbEdge = resizeEdge switch
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

        Swingby.sb_desktop_surface_toplevel_resize(base._sbDesktopSurface, sbEdge);
    }

    /// <summary>
    /// Entire surface size. Include decorations such as shadow.
    /// </summary>
    private Size SurfaceSize
    {
        get => base.Size;
        set
        {
            base.Size = value;
        }
    }

    /// <summary>
    /// Call this method after add the decoration.
    /// </summary>
    /// <returns></returns>
    private Size CalculateSurfaceSize()
    {
        var surfaceSize = Size;
        if (this._shadow != null) {
            surfaceSize.Width += (this._shadow.Thickness * 2);
            surfaceSize.Height += (this._shadow.Thickness * 2) + (_titleBar.Thickness);
        }

        return surfaceSize;
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

    /// <summary>
    /// Calculate the absolute geometry of the body view.
    /// </summary>
    /// <returns></returns>
    private Rect CalculateBodyGeometry()
    {
        var width = _body.Geometry.Width;
        var height = _body.Geometry.Height;
        return new Rect(50.0f, 50.0f, width, height);
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
        var eventListener = new Swingby.EventListener(CallResizeEvent);
        Swingby.sb_desktop_surface_add_event_listener(base._sbDesktopSurface, Swingby.SB_EVENT_TYPE_RESIZE, eventListener);
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

    private WindowShadow? _shadow;
    private WindowResize? _resize;
    private WindowBorder? _border;
    private TitleBar? _titleBar;
    private View _body;
    private Rect _geometry = new Rect(0.0F, 0.0F, 200.0F, 200.0F);
}
