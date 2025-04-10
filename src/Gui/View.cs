namespace Btk.Gui;

using System.Runtime.InteropServices;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

using Btk.Drawing;
using Btk.Drawing.Imaging;
using Btk.Drawing.Effects;
using Btk.Events;
using Btk.Input;
using Btk.Gui.Rendering;
using Btk.Gui.Layouts;
using Btk.Swingby;

public enum ViewFillType
{
    SingleColor,
    Image,
}

public class View
{
    private IntPtr _sbView;
    private View? _parent;
    private Surface _surface;
    private List<View> _children = [];
    private Rect _geometry;
    private Color _color;
    private Image? _image = null;
    private ViewFillType _fillType = ViewFillType.SingleColor;
    private bool _clip;
    private ViewRadius _radius;
    //=================
    // Anchor Lines
    //=================
    private AnchorLine _topAnchor;
    private AnchorLine _leftAnchor;
    private AnchorLine _bottomAnchor;
    private AnchorLine _rightAnchor;
    private AnchorLayout _anchors;
    //==================
    // Event Listeners
    //==================
    private Swingby.EventListener? _enterEventListener;
    private Swingby.EventListener? _leaveEventListener;
    private Swingby.EventListener? _pointerMoveEventListener;
    private Swingby.EventListener? _pressEventListener;
    private Swingby.EventListener? _releaseEventListener;
    private Swingby.EventListener? _clickEventListener;
    private Swingby.EventListener? _moveEventListener;
    private Swingby.EventListener? _resizeEventListener;

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
        this._surface = parent!._surface;
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

        // Add event listeners.
        this.AddEventListeners();
    }

    /// <summary>
    /// Construct a `View` with the root view C ptr.
    /// </summary>
    /// <param name="rootView">
    /// A valid C ptr to the surface's root view.
    /// </param>
    /// <param name="surface"></param>
    /// <param name="geometry"></param>
    internal View(IntPtr rootView, Surface surface, Rect geometry)
    {
        var ftGeometry = sb_rect_t.FromRect(geometry);
        var ftGeometryCPtr = ftGeometry.AllocCPtr();

        this._sbView = Swingby.sb_view_new(rootView, ftGeometryCPtr);

        Marshal.FreeHGlobal(ftGeometryCPtr);

        // Initialize.
        this._parent = null;
        _surface = surface;
        this._geometry = geometry;
        this._color = new Color(255, 255, 255, 255);

        // Anchors.
        _anchors = new AnchorLayout(this);

        _topAnchor = new AnchorLine(this, Anchor.Top);
        _leftAnchor = new AnchorLine(this, Anchor.Left);
        _bottomAnchor = new AnchorLine(this, Anchor.Bottom);
        _rightAnchor = new AnchorLine(this, Anchor.Right);

        // Add event listeners.
        this.AddEventListeners();
    }

    /// <summary>
    /// This must be set when constructed with C raw pointer root view.
    /// </summary>
    public Surface Surface
    {
        get => _surface;
        internal set
        {
            _surface = value;
        }
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

    public Image? Image
    {
        get => _image;
        set
        {
            this._image = value;
            if (value != null)
            {
                Swingby.sb_view_set_image(_sbView, value._sbImage);
            }
            else
            {
                Swingby.sb_view_set_image(_sbView, IntPtr.Zero);
            }
        }
    }

    public ViewFillType FillType
    {
        get => _fillType;
        set
        {
            _fillType = value;
            switch (value)
            {
                case ViewFillType.SingleColor:
                    Swingby.sb_view_set_fill_type(_sbView, Swingby.SB_VIEW_FILL_TYPE_SINGLE_COLOR);
                    break;
                case ViewFillType.Image:
                    Swingby.sb_view_set_fill_type(_sbView, Swingby.SB_VIEW_FILL_TYPE_IMAGE);
                    break;
            }
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

    public bool Clip
    {
        get => this._clip;
        set
        {
            this._clip = value;
            Swingby.sb_view_set_clip(this._sbView, value);
        }
    }

    public AnchorLine TopAnchor => _topAnchor;

    public AnchorLine BottomAnchor => _bottomAnchor;

    public AnchorLine LeftAnchor => _leftAnchor;

    public AnchorLine RightAnchor => _rightAnchor;

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
        _topAnchor.OnAnchorMove(this, evt);
        _bottomAnchor.OnAnchorMove(this, evt);
        _leftAnchor.OnAnchorMove(this, evt);
        _rightAnchor.OnAnchorMove(this, evt);
        // Anchor.Fill
        foreach (View view in FillViews)
        {
            view.Geometry = new Rect(0.0f, 0.0f, Geometry.Width, Geometry.Height);
        }
        // Anchor.CenterIn
        foreach (View view in CenterInViews)
        {
            if (!FillViews.Contains(view))
            {
                view.Geometry = new Rect(
                    Geometry.X + (Geometry.Width - view.Geometry.Width) / 2.0f,
                    Geometry.Y + (Geometry.Height - view.Geometry.Height) / 2.0f,
                    view.Geometry.Width,
                    view.Geometry.Height);
            }
        }
        OnMove?.Invoke(this, evt);
    }

    protected virtual void ResizeEvent(ResizeEvent evt)
    {
        _topAnchor.OnAnchorResize(this, evt);
        _bottomAnchor.OnAnchorResize(this, evt);
        _leftAnchor.OnAnchorResize(this, evt);
        _rightAnchor.OnAnchorResize(this, evt);
        // Anchor.Fill
        foreach (View view in FillViews)
        {
            view.Geometry = new Rect(0.0f, 0.0f, Geometry.Width, Geometry.Height);
        }
        foreach (View view in CenterInViews)
        {
            if (!FillViews.Contains(view))
            {
                view.Geometry = new Rect(
                    Geometry.X + (Geometry.Width - view.Geometry.Width) / 2.0f,
                    Geometry.Y + (Geometry.Height - view.Geometry.Height) / 2.0f,
                    view.Geometry.Width,
                    view.Geometry.Height);
            }
        }
        OnResize?.Invoke(this, evt);
    }

    private void AddEventListeners()
    {
        IntPtr sbView = this._sbView;

        _enterEventListener = new Swingby.EventListener(this.CallPointerEnterEvent);
        Swingby.sb_view_add_event_listener(sbView, Swingby.SB_EVENT_TYPE_POINTER_ENTER, _enterEventListener);

        _leaveEventListener = new Swingby.EventListener(this.CallPointerLeaveEvent);
        Swingby.sb_view_add_event_listener(sbView, Swingby.SB_EVENT_TYPE_POINTER_LEAVE, _leaveEventListener);

        _pointerMoveEventListener = new Swingby.EventListener(this.CallPointerMoveEvent);
        Swingby.sb_view_add_event_listener(sbView, Swingby.SB_EVENT_TYPE_POINTER_MOVE, _pointerMoveEventListener);

        _pressEventListener = new Swingby.EventListener(this.CallPointerPressEvent);
        Swingby.sb_view_add_event_listener(sbView, Swingby.SB_EVENT_TYPE_POINTER_PRESS, _pressEventListener);

        _releaseEventListener = new Swingby.EventListener(this.CallPointerReleaseEvent);
        Swingby.sb_view_add_event_listener(sbView, Swingby.SB_EVENT_TYPE_POINTER_RELEASE, _releaseEventListener);

        _clickEventListener = new Swingby.EventListener(CallPointerClickEvent);
        Swingby.sb_view_add_event_listener(sbView, Swingby.SB_EVENT_TYPE_POINTER_CLICK, _clickEventListener);

        _moveEventListener = new Swingby.EventListener(CallMoveEvent);
        Swingby.sb_view_add_event_listener(sbView, Swingby.SB_EVENT_TYPE_MOVE, _moveEventListener);

        _resizeEventListener = new Swingby.EventListener(CallResizeEvent);
        Swingby.sb_view_add_event_listener(sbView, Swingby.SB_EVENT_TYPE_RESIZE, _resizeEventListener);
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

        var sbButton = Swingby.sb_event_pointer_button(sbEvent);

        var button = sbButton switch
        {
            Swingby.SB_POINTER_BUTTON_LEFT => PointerButton.Left,
            Swingby.SB_POINTER_BUTTON_RIGHT => PointerButton.Right,
            Swingby.SB_POINTER_BUTTON_MIDDLE => PointerButton.Middle,
            _ => PointerButton.None
        };

        evt.Button = button;

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
    // Anchor Fill or CenterIn
    //=============================
    internal List<View> FillViews = [];

    internal List<View> CenterInViews = [];
}
