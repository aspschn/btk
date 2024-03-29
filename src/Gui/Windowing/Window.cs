namespace Blusher.Gui.Windowing;

using System.Runtime.InteropServices;

using Blusher.Foundation;
using Blusher.Drawing;
using Blusher.Gui;
using Blusher.Events;

public class Window
{
    public Window()
    {
        this._ftDesktopSurface = Foundation.ft_desktop_surface_new(
            Foundation.FT_DESKTOP_SURFACE_ROLE_TOPLEVEL);

        // Set the root view color as transparent.
        IntPtr ftSurface = Foundation.ft_desktop_surface_surface(this._ftDesktopSurface);
        IntPtr ftView = Foundation.ft_surface_root_view(ftSurface);
        ft_color_t color = ft_color_t.FromColor(new Color(0, 0, 0, 0));

        IntPtr ftColor = color.AllocCPtr();
        Foundation.ft_view_set_color(ftView, ftColor);
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
        this._border = new WindowBorder(_resize);
        // - Add title bar.
        this._titleBar = new TitleBar(_border);

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
        Foundation.ft_desktop_surface_show(this._ftDesktopSurface);

        // Set WM geometry. This SHOULD done after Show.
        var wmGeometry = this.CalculateWindowGeometry();
        var ftWmGeometry = ft_rect_t.FromRect(wmGeometry);
        var ftWmGeometryPtr = ftWmGeometry.AllocCPtr();

        Foundation.ft_desktop_surface_set_wm_geometry(_ftDesktopSurface, ftWmGeometryPtr);

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

            // TODO: Set surface size and wm_geometry of desktop surface.
            this.SurfaceSize = this.CalculateSurfaceSize();
            if (this.HasDecoration) {
                this.UpdateDecoration();
            }

            var wmGeometry = this.CalculateWindowGeometry();
            var ftWmGeometry = ft_rect_t.FromRect(wmGeometry);
            var ftWmGeometryPtr = ftWmGeometry.AllocCPtr();

            Foundation.ft_desktop_surface_set_wm_geometry(_ftDesktopSurface, ftWmGeometryPtr);

            Marshal.FreeHGlobal(ftWmGeometryPtr);
        }
    }

    public bool HasDecoration
    {
        get => true;
    }

    private Size SurfaceSize
    {
        set
        {
            IntPtr ftSurface = Foundation.ft_desktop_surface_surface(this._ftDesktopSurface);
            var ftSize = ft_size_t.FromSize(value);
            var ftSizePtr = ftSize.AllocCPtr();

            Foundation.ft_surface_set_size(ftSurface, ftSizePtr);

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
        var eventListener = new Foundation.EventListener(this.CallResizeEvent);
        Foundation.ft_desktop_surface_add_event_listener(this._ftDesktopSurface, Foundation.FT_EVENT_TYPE_RESIZE, eventListener);
    }

    private void CallResizeEvent(IntPtr ftEvent)
    {
        IntPtr oldSize = Foundation.ft_event_resize_size(ftEvent);
        var oldWidth = Foundation.ft_size_width(oldSize);
        var oldHeight = Foundation.ft_size_height(oldSize);
        IntPtr size = Foundation.ft_event_resize_size(ftEvent);
        var width = Foundation.ft_size_width(size);
        var height = Foundation.ft_size_height(size);

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
