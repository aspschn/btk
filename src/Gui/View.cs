namespace Blusher.Gui;

using System.Runtime.InteropServices;

using Blusher.Foundation;
using Blusher.Drawing;
using Blusher.Events;

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
