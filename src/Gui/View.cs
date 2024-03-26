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
        var ftGeometry = ft_rect_t.FromRect(geometry);
        var ftGeometryCPtr = ftGeometry.AllocCPtr();

        this._ftView = Foundation.ft_view_new(ftParent, ftGeometryCPtr);

        Marshal.FreeHGlobal(ftGeometryCPtr);

        // Initialize.
        this._parent = parent;
        this._geometry = geometry;
        this._color = new Color(255, 255, 255, 255);

        // Parenting.
        parent._children.Add(this);

        // Add event listeners.
        this.AddEventListeners();
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
        var ftGeometry = ft_rect_t.FromRect(geometry);
        var ftGeometryCPtr = ftGeometry.AllocCPtr();

        this._ftView = Foundation.ft_view_new(rootView, ftGeometryCPtr);

        Marshal.FreeHGlobal(ftGeometryCPtr);

        // Initialize.
        this._parent = null;
        this._geometry = geometry;
        this._color = new Color(255, 255, 255, 255);

        // Add event listeners.
        this.AddEventListeners();
    }

    public Rect Geometry
    {
        get => this._geometry;
        set
        {
            this._geometry = value;

            var ftGeometry = ft_rect_t.FromRect(value);
            var ftGeometryPtr = ftGeometry.AllocCPtr();

            Foundation.ft_view_set_geometry(this._ftView, ftGeometryPtr);

            Marshal.FreeHGlobal(ftGeometryPtr);
        }
    }

    public Color Color
    {
        get => this._color;
        set
        {
            this._color = value;

            var ftColor = ft_color_t.FromColor(value);
            var ftColorPtr = ftColor.AllocCPtr();

            Foundation.ft_view_set_color(this._ftView, ftColorPtr);

            Marshal.FreeHGlobal(ftColorPtr);
        }
    }

    protected virtual void PointerEnterEvent(PointerEvent evt)
    {
        Console.WriteLine("View PointerEnterEvent");
    }

    protected virtual void PointerLeaveEvent(PointerEvent evt)
    {
        Console.WriteLine("View PointerLeaveEvent");
    }

    private void AddEventListeners()
    {
        IntPtr ftView = this._ftView;

        var enterEventListener = new Foundation.EventListener(this.CallPointerEnterEvent);
        Foundation.ft_view_add_event_listener(ftView, Foundation.FT_EVENT_TYPE_POINTER_ENTER, enterEventListener);

        var leaveEventListener = new Foundation.EventListener(this.CallPointerLeaveEvent);
        Foundation.ft_view_add_event_listener(ftView, Foundation.FT_EVENT_TYPE_POINTER_LEAVE, leaveEventListener);
    }

    private void CallPointerEnterEvent(IntPtr ftEvent)
    {
        // TODO: fill info.

        var evt = new PointerEvent(EventType.PointerEnter);
        this.PointerEnterEvent(evt);
    }

    private void CallPointerLeaveEvent(IntPtr ftEvent)
    {
        // TODO: fill info

        var evt = new PointerEvent(EventType.PointerLeave);
        this.PointerLeaveEvent(evt);
    }

    private IntPtr _ftView;
    private View? _parent;
    private List<View> _children = [];
    private Rect _geometry;
    private Color _color;
}
