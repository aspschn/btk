namespace Btk.Gui;

using System.Runtime.InteropServices;

using Btk.Drawing;
using Btk.Events;
using Btk.Swingby;

public class View
{
    private IntPtr _sbView;
    private View? _parent;
    private List<View> _children = [];
    private Rect _geometry;
    private Color _color;
    private ViewRadius _radius;

    public event EventHandler<PointerEvent>? OnPointerPress = null;
    public event EventHandler<PointerEvent>? OnPointerRelease = null;
    public event EventHandler<PointerEvent>? OnPointerClick = null;

    public View(View parent, Rect geometry)
    {
        // C functions.
        var ftParent = parent._sbView;
        var ftGeometry = sb_rect_t.FromRect(geometry);
        var ftGeometryCPtr = ftGeometry.AllocCPtr();

        this._sbView = Swingby.sb_view_new(ftParent, ftGeometryCPtr);

        Marshal.FreeHGlobal(ftGeometryCPtr);

        // Initialize.
        this._parent = parent;
        this._geometry = geometry;
        this._color = new Color(255, 255, 255, 255);
        _radius = new ViewRadius(0.0f, 0.0f, 0.0f, 0.0f);

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
        var ftGeometry = sb_rect_t.FromRect(geometry);
        var ftGeometryCPtr = ftGeometry.AllocCPtr();

        this._sbView = Swingby.sb_view_new(rootView, ftGeometryCPtr);

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

            var ftGeometry = sb_rect_t.FromRect(value);
            var ftGeometryPtr = ftGeometry.AllocCPtr();

            Swingby.sb_view_set_geometry(this._sbView, ftGeometryPtr);

            Marshal.FreeHGlobal(ftGeometryPtr);
        }
    }

    public Color Color
    {
        get => this._color;
        set
        {
            this._color = value;

            var sbColor = sb_color_t.FromColor(value);
            var sbColorPtr = sbColor.AllocCPtr();

            Swingby.sb_view_set_color(this._sbView, sbColorPtr);
            Marshal.FreeHGlobal(sbColorPtr);
        }
    }

    public ViewRadius Radius
    {
        get => _radius;
        set
        {
            _radius = value;

            var sbViewRaduis = sb_view_radius_t.FromViewRadius(_radius);
            var sbViewRadiusPtr = sbViewRaduis.AllocCPtr();

            Swingby.sb_view_set_radius(_sbView, sbViewRadiusPtr);
            Marshal.FreeHGlobal(sbViewRadiusPtr);
        }
    }

    public CursorShape CursorShape
    {
        get
        {
            int sbCursorShape = Swingby.sb_view_cursor_shape(this._sbView);
            return Swingby.ToCursorShape(sbCursorShape);
        }
        set => Swingby.sb_view_set_cursor_shape(_sbView, Swingby.FromCursorShape(value));
    }

    protected virtual void PointerEnterEvent(PointerEvent evt)
    {
        Console.WriteLine("View PointerEnterEvent");
    }

    protected virtual void PointerLeaveEvent(PointerEvent evt)
    {
        Console.WriteLine("View PointerLeaveEvent");
    }

    protected virtual void PointerMoveEvent(PointerEvent evt)
    {
        //
    }

    protected virtual void PointerPressEvent(PointerEvent evt)
    {
        OnPointerPress?.Invoke(this, evt);
    }

    protected virtual void PointerReleaseEvent(PointerEvent evt)
    {
        OnPointerRelease?.Invoke(this, evt);
    }

    protected virtual void PointerClickEvent(PointerEvent evt)
    {
        OnPointerClick?.Invoke(this, evt);
    }

    private void AddEventListeners()
    {
        IntPtr ftView = this._sbView;

        var enterEventListener = new Swingby.EventListener(this.CallPointerEnterEvent);
        Swingby.sb_view_add_event_listener(ftView, Swingby.SB_EVENT_TYPE_POINTER_ENTER, enterEventListener);

        var leaveEventListener = new Swingby.EventListener(this.CallPointerLeaveEvent);
        Swingby.sb_view_add_event_listener(ftView, Swingby.SB_EVENT_TYPE_POINTER_LEAVE, leaveEventListener);

        var moveEventListener = new Swingby.EventListener(this.CallPointerMoveEvent);
        Swingby.sb_view_add_event_listener(ftView, Swingby.SB_EVENT_TYPE_POINTER_MOVE, moveEventListener);

        var pressEventListener = new Swingby.EventListener(this.CallPointerPressEvent);
        Swingby.sb_view_add_event_listener(ftView, Swingby.SB_EVENT_TYPE_POINTER_PRESS, pressEventListener);

        var releaseEventListener = new Swingby.EventListener(this.CallPointerReleaseEvent);
        Swingby.sb_view_add_event_listener(ftView, Swingby.SB_EVENT_TYPE_POINTER_RELEASE, releaseEventListener);

        var clickEventListener = new Swingby.EventListener(CallPointerClickEvent);
        Swingby.sb_view_add_event_listener(ftView, Swingby.SB_EVENT_TYPE_POINTER_CLICK, clickEventListener);
    }

    private void CallPointerEnterEvent(IntPtr ftEvent)
    {
        // TODO: fill info.

        var evt = new PointerEvent(EventType.PointerEnter);
        evt.SetSwingbyEvent(ftEvent);

        this.PointerEnterEvent(evt);
    }

    private void CallPointerLeaveEvent(IntPtr ftEvent)
    {
        // TODO: fill info

        var evt = new PointerEvent(EventType.PointerLeave);
        evt.SetSwingbyEvent(ftEvent);

        this.PointerLeaveEvent(evt);
    }

    private void CallPointerMoveEvent(IntPtr ftEvent)
    {
        // TODO: fill info

        var evt = new PointerEvent(EventType.PointerMove);
        evt.SetSwingbyEvent(ftEvent);

        this.PointerMoveEvent(evt);
    }

    private void CallPointerPressEvent(IntPtr ftEvent)
    {
        // TODO: fill info

        var evt = new PointerEvent(EventType.PointerPress);
        evt.SetSwingbyEvent(ftEvent);

        this.PointerPressEvent(evt);
    }

    private void CallPointerReleaseEvent(IntPtr ftEvent)
    {
        // TODO: fill info

        var evt = new PointerEvent(EventType.PointerRelease);
        evt.SetSwingbyEvent(ftEvent);

        this.PointerReleaseEvent(evt);
    }

    private void CallPointerClickEvent(IntPtr sbEvent)
    {
        var evt = new PointerEvent(EventType.PointerClick);
        evt.SetSwingbyEvent(sbEvent);

        this.PointerClickEvent(evt);
    }
}
