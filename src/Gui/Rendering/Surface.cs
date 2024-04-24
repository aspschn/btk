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

    public void Show()
    {
        Swingby.sb_desktop_surface_show(_sbDesktopSurface);
    }

    public void Hide()
    {
        Swingby.sb_desktop_surface_hide(_sbDesktopSurface);
    }

    private IntPtr _sbDesktopSurface;
    private Surface? _parent;
}
