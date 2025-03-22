namespace Btk.Gui.Rendering;

using System.Runtime.InteropServices;

using Btk.Drawing;
using Btk.Swingby;

public enum SurfaceResizeEdge
{
    None,
    Top,
    Bottom,
    Left,
    TopLeft,
    BottomLeft,
    Right,
    TopRight,
    BottomRight,
}

public class Surface
{
    public Surface(SurfaceRole role, Surface? parent = null)
    {
        Role = role;
        _parent = parent;
        _inputGeometry = null;

        int sbRole = 0;
        sbRole = role == SurfaceRole.Toplevel ? Swingby.SB_DESKTOP_SURFACE_ROLE_TOPLEVEL : Swingby.SB_DESKTOP_SURFACE_ROLE_POPUP;
        _sbDesktopSurface = Swingby.sb_desktop_surface_new(sbRole);
    }

    public SurfaceRole Role { get; set; }

    public Color RootViewColor
    {
        get
        {
            IntPtr sbSurface = Swingby.sb_desktop_surface_surface(_sbDesktopSurface);
            IntPtr sbRootView = Swingby.sb_surface_root_view(sbSurface);
            IntPtr sbColor = Swingby.sb_view_color(sbRootView);

            // TODO: Implementation.
            return new Color(0, 0, 0, 0);
        }
        set
        {
            IntPtr sbSurface = Swingby.sb_desktop_surface_surface(_sbDesktopSurface);
            IntPtr sbRootView = Swingby.sb_surface_root_view(sbSurface);

            var sbColor = sb_color_t.FromColor(value);
            var sbColorPtr = sbColor.AllocCPtr();
            Swingby.sb_view_set_color(sbRootView, sbColorPtr);
            Marshal.FreeHGlobal(sbColorPtr);
        }
    }

    public Size Size
    {
        get
        {
            IntPtr sbSurface = Swingby.sb_desktop_surface_surface(_sbDesktopSurface);
            IntPtr sbSize = Swingby.sb_surface_size(sbSurface);

            float width = Swingby.sb_size_width(sbSize);
            float height = Swingby.sb_size_height(sbSize);

            return new Size(width, height);
        }
        set
        {
            IntPtr sbSurface = Swingby.sb_desktop_surface_surface(_sbDesktopSurface);

            var sbSize = sb_size_t.FromSize(value);
            var sbSizePtr = sbSize.AllocCPtr();
            Swingby.sb_surface_set_size(sbSurface, sbSizePtr);
            Marshal.FreeHGlobal(sbSizePtr);
        }
    }

    public Rect WMGeometry
    {
        get
        {
            // TODO: Implementation.
            return new Rect(0F, 0F, 0F, 0F);
        }
        set
        {
            var sbRect = sb_rect_t.FromRect(value);
            var sbRectPtr = sbRect.AllocCPtr();
            Swingby.sb_desktop_surface_set_wm_geometry(_sbDesktopSurface, sbRectPtr);
            Marshal.FreeHGlobal(sbRectPtr);
        }
    }

    public Rect? InputGeometry
    {
        get => _inputGeometry;
        set
        {
            // Input geometry cannot set to null.
            if (value == null)
            {
                return;
            }
            var sbSurface = Swingby.sb_desktop_surface_surface(_sbDesktopSurface);
            _inputGeometry = value;

            var sbRect = sb_rect_t.FromRect(value.Value);
            var sbRectPtr = sbRect.AllocCPtr();
            Swingby.sb_surface_set_input_geometry(sbSurface, sbRectPtr);
            Marshal.FreeHGlobal(sbRectPtr);
        }
    }

    public void Show()
    {
        Swingby.sb_desktop_surface_show(_sbDesktopSurface);
    }

    public void Hide()
    {
        Swingby.sb_desktop_surface_hide(_sbDesktopSurface);
    }

    public void Close()
    {
        if (Role == SurfaceRole.Toplevel)
        {
            Swingby.sb_desktop_surface_toplevel_close(_sbDesktopSurface);
        }
    }

    public void SetMinimized()
    {
        if (Role == SurfaceRole.Toplevel)
        {
            Swingby.sb_desktop_surface_toplevel_set_minimized(_sbDesktopSurface);
        }
    }

    public void Resize(SurfaceResizeEdge edge)
    {
        int sbEdge = edge switch
        {
            SurfaceResizeEdge.TopLeft => Swingby.SB_DESKTOP_SURFACE_TOPLEVEL_RESIZE_EDGE_TOP_LEFT,
            SurfaceResizeEdge.Top => Swingby.SB_DESKTOP_SURFACE_TOPLEVEL_RESIZE_EDGE_TOP,
            SurfaceResizeEdge.TopRight => Swingby.SB_DESKTOP_SURFACE_TOPLEVEL_RESIZE_EDGE_TOP_RIGHT,
            SurfaceResizeEdge.Right => Swingby.SB_DESKTOP_SURFACE_TOPLEVEL_RESIZE_EDGE_RIGHT,
            SurfaceResizeEdge.BottomRight => Swingby.SB_DESKTOP_SURFACE_TOPLEVEL_RESIZE_EDGE_BOTTOM_RIGHT,
            SurfaceResizeEdge.Bottom => Swingby.SB_DESKTOP_SURFACE_TOPLEVEL_RESIZE_EDGE_BOTTOM,
            SurfaceResizeEdge.BottomLeft => Swingby.SB_DESKTOP_SURFACE_TOPLEVEL_RESIZE_EDGE_BOTTOM_LEFT,
            SurfaceResizeEdge.Left => Swingby.SB_DESKTOP_SURFACE_TOPLEVEL_RESIZE_EDGE_LEFT,
            _ => Swingby.SB_DESKTOP_SURFACE_TOPLEVEL_RESIZE_EDGE_NONE
        };
        Swingby.sb_desktop_surface_toplevel_resize(_sbDesktopSurface, sbEdge);
    }

    protected IntPtr _sbDesktopSurface;
    private Surface? _parent;
    private Rect? _inputGeometry;
}
