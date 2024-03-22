namespace Blusher.Gui;

using System.Runtime.InteropServices;

using Blusher.Foundation;
using Blusher.Drawing;

/// <summary>
/// A global application object.
/// </summary>
public class Application
{
    public Application(in string[] args)
    {
        int argc = args.Length;

        this._ftApplication = Foundation.ft_application_new(argc, args);
    }

    public int Exec()
    {
        return Foundation.ft_application_exec(this._ftApplication);
    }

    private IntPtr _ftApplication;
}

public class View
{
    public View(View parent, Rect geometry)
    {
        // C functions.
        var ftParent = parent._ftView;
        var ftGeometry = Foundation.ft_rect_t.FromRect(geometry);
        var ftGeometryCPtr = ftGeometry.AllocCPtr();

        this._ftView = Foundation.ft_view_new(ftParent, ftGeometryCPtr);

        Marshal.FreeHGlobal(ftGeometryCPtr);

        // Initialize.
        this._parent = parent;
        this._geometry = geometry;
        this._color = new Color(255, 255, 255, 255);
    }

    /// <summary>
    /// Construct a `View` with the root view C ptr.
    /// </summary>
    /// <param name="rootView">
    /// A valid C ptr to the surface's root view.
    /// </param>
    /// <param name="geometry"></param>
    internal View(IntPtr rootView, Rect geometry)
    {
        var ftGeometry = Foundation.ft_rect_t.FromRect(geometry);
        var ftGeometryCPtr = ftGeometry.AllocCPtr();

        this._ftView = Foundation.ft_view_new(rootView, ftGeometryCPtr);

        Marshal.FreeHGlobal(ftGeometryCPtr);

        // Initialize.
        this._parent = null;
        this._geometry = geometry;
        this._color = new Color(255, 255, 255, 255);
    }

    public Rect Geometry
    {
        get => this._geometry;
    }

    public Color Color
    {
        get => this._color;
        set
        {
            this._color = value;

            var ftColor = Foundation.ft_color_t.FromColor(value);
            var ftColorPtr = ftColor.AllocCPtr();

            Foundation.ft_view_set_color(this._ftView, ftColorPtr);

            Marshal.FreeHGlobal(ftColorPtr);
        }
    }

    private IntPtr _ftView;
    private View? _parent;
    private Rect _geometry;
    private Color _color;
}

public class Window
{
    public Window()
    {
        this._ftDesktopSurface = Foundation.ft_desktop_surface_new(
            Foundation.ft_desktop_surface_role.FT_DESKTOP_SURFACE_ROLE_TOPLEVEL);

        // Set the root view color as transparent.
        IntPtr ftSurface = Foundation.ft_desktop_surface_surface(this._ftDesktopSurface);
        IntPtr ftView = Foundation.ft_surface_root_view(ftSurface);
        Foundation.ft_color_t color;
        color.r = 0;
        color.g = 0;
        color.b = 0;
        color.a = 0;

        IntPtr ftColor = color.AllocCPtr();
        Foundation.ft_view_set_color(ftView, ftColor);
        Marshal.FreeHGlobal(ftColor);

        // Create a shadow.
        this._shadow = new View(ftView, new Rect(0.0F, 0.0F, 100.0F, 100.0F));
        this._shadow.Color = new Color(100, 100, 100, 80);
    }

    public void Show()
    {
        Foundation.ft_desktop_surface_show(this._ftDesktopSurface);
    }

    private IntPtr _ftDesktopSurface;
    View _shadow;
}
