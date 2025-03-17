namespace Btk.Gui;

using System.Runtime.InteropServices;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

using Btk.Drawing;
using Btk.Drawing.Effects;
using Btk.Events;
using Btk.Gui.Layouts;
using Btk.Swingby;

public class View
{
    private IntPtr _sbView;
    private View? _parent;
    private List<View> _children = [];
    private Rect _geometry;
    private Color _color;
    private ViewRadius _radius;
    private AnchorLine _topAnchor;
    private AnchorLine _leftAnchor;
    private AnchorLine _bottomAnchor;
    private AnchorLine _rightAnchor;
    private AnchorLayout _anchors;

    public event EventHandler<PointerEvent>? OnPointerPress = null;
    public event EventHandler<PointerEvent>? OnPointerRelease = null;
    public event EventHandler<PointerEvent>? OnPointerClick = null;
    public event EventHandler<MoveEvent>? OnMove = null;
    public event EventHandler<ResizeEvent>? OnResize = null;

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

        // Add `Filters` property added action.
        Filters.CollectionChanged += OnFilterAdded;

        // Anchors.
        _anchors = new AnchorLayout(this);

        _topAnchor = new AnchorLine(this, Anchor.Top);
        _leftAnchor = new AnchorLine(this, Anchor.Left);
        _bottomAnchor = new AnchorLine(this, Anchor.Bottom);
        _rightAnchor = new AnchorLine(this, Anchor.Right);

        OnMove += CalculateTopAnchor;
        OnMove += CalculateBottomAnchor;
        OnResize += CalculateBottomAnchor;

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

        // Anchors.
        _anchors = new AnchorLayout(this);

        _topAnchor = new AnchorLine(this, Anchor.Top);
        _leftAnchor = new AnchorLine(this, Anchor.Left);
        _bottomAnchor = new AnchorLine(this, Anchor.Bottom);
        _rightAnchor = new AnchorLine(this, Anchor.Right);

        OnMove += CalculateTopAnchor;
        OnMove += CalculateBottomAnchor;
        OnResize += CalculateBottomAnchor;

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

    public AnchorLine TopAnchor => _topAnchor;

    public AnchorLine BottomAnchor => _bottomAnchor;

    public AnchorLayout Anchors
    {
        get
        {
            return _anchors;
        }
    }

    public ObservableCollection<IFilter> Filters { get; set; } = [];

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

    protected virtual void MoveEvent(MoveEvent evt)
    {
        OnMove?.Invoke(this, evt);
    }

    protected virtual void ResizeEvent(ResizeEvent evt)
    {
        OnResize?.Invoke(this, evt);
    }

    private void AddEventListeners()
    {
        IntPtr sbView = this._sbView;

        var enterEventListener = new Swingby.EventListener(this.CallPointerEnterEvent);
        Swingby.sb_view_add_event_listener(sbView, Swingby.SB_EVENT_TYPE_POINTER_ENTER, enterEventListener);

        var leaveEventListener = new Swingby.EventListener(this.CallPointerLeaveEvent);
        Swingby.sb_view_add_event_listener(sbView, Swingby.SB_EVENT_TYPE_POINTER_LEAVE, leaveEventListener);

        var pointerMoveEventListener = new Swingby.EventListener(this.CallPointerMoveEvent);
        Swingby.sb_view_add_event_listener(sbView, Swingby.SB_EVENT_TYPE_POINTER_MOVE, pointerMoveEventListener);

        var pressEventListener = new Swingby.EventListener(this.CallPointerPressEvent);
        Swingby.sb_view_add_event_listener(sbView, Swingby.SB_EVENT_TYPE_POINTER_PRESS, pressEventListener);

        var releaseEventListener = new Swingby.EventListener(this.CallPointerReleaseEvent);
        Swingby.sb_view_add_event_listener(sbView, Swingby.SB_EVENT_TYPE_POINTER_RELEASE, releaseEventListener);

        var clickEventListener = new Swingby.EventListener(CallPointerClickEvent);
        Swingby.sb_view_add_event_listener(sbView, Swingby.SB_EVENT_TYPE_POINTER_CLICK, clickEventListener);

        var moveEventListener = new Swingby.EventListener(CallMoveEvent);
        Swingby.sb_view_add_event_listener(sbView, Swingby.SB_EVENT_TYPE_MOVE, moveEventListener);

        var resizeEventListener = new Swingby.EventListener(CallResizeEvent);
        Swingby.sb_view_add_event_listener(sbView, Swingby.SB_EVENT_TYPE_RESIZE, resizeEventListener);
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

    private void CallMoveEvent(IntPtr sbEvent)
    {
        var sbOldPos = Swingby.sb_event_move_old_position(sbEvent);
        var sbPos = Swingby.sb_event_move_position(sbEvent);

        float oldX = Swingby.sb_point_x(sbOldPos);
        float oldY = Swingby.sb_point_y(sbOldPos);
        float x = Swingby.sb_point_x(sbPos);
        float y = Swingby.sb_point_y(sbPos);

        Point oldPos = new Point(oldX, oldY);
        Point pos = new Point(x, y);

        MoveEvent(new MoveEvent(oldPos, pos));
    }

    private void CallResizeEvent(IntPtr sbEvent)
    {
        var sbOldSize = Swingby.sb_event_resize_old_size(sbEvent);
        var sbSize = Swingby.sb_event_resize_size(sbEvent);

        float oldWidth = Swingby.sb_size_width(sbOldSize);
        float oldHeight = Swingby.sb_size_height(sbOldSize);
        float width = Swingby.sb_size_width(sbSize);
        float height = Swingby.sb_size_height(sbSize);

        Size oldSize = new Size(oldWidth, oldHeight);
        Size size = new Size(width, height);

        ResizeEvent(new ResizeEvent(oldSize, size));
    }

    private void OnFilterAdded(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            if (e.NewItems == null)
            {
                return;
            }

            var lastItem = (IFilter)e.NewItems[^1]!;
            var sbFilter = lastItem.GetCPtr();
            Swingby.sb_view_add_filter(_sbView, sbFilter);
        }
    }

    //=============================
    // Anchor Layout Calculations
    //=============================
    private void CalculateTopAnchor(object? sender, Event evt)
    {
        foreach (View subscribed in TopAnchor.SubscribedViews)
        {
            subscribed.Geometry = new Rect(subscribed.Geometry.X, Geometry.Y,
                subscribed.Geometry.Width, subscribed.Geometry.Height);
        }
    }

    private void CalculateBottomAnchor(object? sender, Event evt)
    {
        foreach (View subscribed in BottomAnchor.SubscribedViews)
        {
            float y = Geometry.Height - subscribed.Geometry.Height;
            subscribed.Geometry = new Rect(subscribed.Geometry.X, y,
                subscribed.Geometry.Width, subscribed.Geometry.Height);
        }
    }
}
