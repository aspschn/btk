namespace Blusher.Gui.Rendering;

using System.Runtime.InteropServices;

using Blusher.Drawing;
using Blusher.Swingby;

public class Surface
{
    public Surface(SurfaceRole role, Surface? parent = null)
    {
        Role = role;
        _parent = parent;

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

    public void Show()
    {
        Swingby.sb_desktop_surface_show(_sbDesktopSurface);
    }

    public void Hide()
    {
        Swingby.sb_desktop_surface_hide(_sbDesktopSurface);
    }

    protected IntPtr _sbDesktopSurface;
    private Surface? _parent;
}
