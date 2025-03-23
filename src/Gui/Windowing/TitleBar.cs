namespace Btk.Gui.Windowing;

using Btk.Drawing;
using Btk.Gui;
using Btk.Events;

public enum TitleBarButtonAction
{
    Close,
    Minimize,
    Maximize,
}

public class TitleBarButton : View
{
    private TitleBarButtonAction _action;
    private Color _closeColor = new Color(255, 0, 0, 255);
    private Color _closeHoverColor = new Color(255, 100, 100, 255);
    private Color _minimizeColor = new Color(255, 255, 0, 255);
    private Color _minimizeHoverColor = new Color(255, 255, 100, 255);
    private Color _maximizeColor = new Color(0, 255, 0, 125);
    private Color _maximizeHoverColor = new Color(100, 255, 100, 255);

    public TitleBarButton(TitleBarButtonAction action, View parent) : base(parent, new Rect(0.0f, 0.0f, 20.0f, 20.0f))
    {
        _action = action;

        base.Color = action switch
        {
            TitleBarButtonAction.Close => _closeColor,
            TitleBarButtonAction.Minimize => _minimizeColor,
            TitleBarButtonAction.Maximize => _maximizeColor,
            _ => base.Color
        };

        base.Radius = new ViewRadius(25.0f, 25.0f, 25.0f, 25.0f);
    }

    protected override void PointerEnterEvent(PointerEvent evt)
    {
        base.Color = _action switch
        {
            TitleBarButtonAction.Close => _closeHoverColor,
            TitleBarButtonAction.Minimize => _minimizeHoverColor,
            TitleBarButtonAction.Maximize => _maximizeHoverColor,
            _ => base.Color
        };
        base.PointerEnterEvent(evt);
    }

    protected override void PointerLeaveEvent(PointerEvent evt)
    {
        base.Color = _action switch
        {
            TitleBarButtonAction.Close => _closeColor,
            TitleBarButtonAction.Minimize => _minimizeColor,
            TitleBarButtonAction.Maximize => _maximizeColor,
            _ => base.Color
        };
        base.PointerLeaveEvent(evt);
    }

    protected override void PointerPressEvent(PointerEvent evt)
    {
        evt.Propagation = false;

        base.PointerPressEvent(evt);
    }
}

public class TitleBar : View, IWindowDecoration
{
    private TitleBarButton _closeButton;
    private TitleBarButton _minimizeButton;
    private TitleBarButton _maximizeButton;
    private bool _activated;
    private readonly Color _activeColor = new Color(128, 128, 128, 255);
    private readonly Color _inactiveColor = new Color(192, 192, 192, 255);

    public TitleBar(Window window, View parent) : base(parent, new Rect(0F, 0F, 10F, 10F))
    {
        _thickness = 30;
        _window = window;
        Pressed = false;
        Color = _activeColor;

        // Close button.
        _closeButton = new TitleBarButton(TitleBarButtonAction.Close, this);
        _closeButton.Geometry = new Rect(5.0f, 5.0f, _closeButton.Geometry.Width, _closeButton.Geometry.Height);
        _closeButton.OnPointerClick += (v, evt) =>
        {
            _window.Close();
            evt.Propagation = false;
        };
        // Minimize button.
        _minimizeButton = new TitleBarButton(TitleBarButtonAction.Minimize, this);
        _minimizeButton.Anchors.Left = _closeButton.RightAnchor;
        _minimizeButton.Anchors.LeftMargin = 5.0f;
        _minimizeButton.Anchors.Top = _closeButton.TopAnchor;
        _minimizeButton.OnPointerClick += (v, evt) =>
        {
            _window.Minimize();
            evt.Propagation = false;
        };
        // Maximize/Restore button.
        _maximizeButton = new TitleBarButton(TitleBarButtonAction.Maximize, this);
        _maximizeButton.Anchors.LeftMargin = 5.0f;
        _maximizeButton.Anchors.Left = _minimizeButton.RightAnchor;
        _maximizeButton.Anchors.Top = _closeButton.TopAnchor;
    }

    public uint Thickness
    {
        get => _thickness;
    }

    private bool Pressed { get; set; }

    public bool Activated
    {
        get => _activated;
        set
        {
            _activated = value;
            if (value)
            {
                Color = _activeColor;
            }
            else
            {
                Color = _inactiveColor;
            }
        }
    }

    protected override void PointerMoveEvent(PointerEvent evt)
    {
        if (Pressed) {
            _window.StartMove();
            Pressed = false;
        }

        evt.Propagation = false;

        base.PointerMoveEvent(evt);
    }

    protected override void PointerPressEvent(PointerEvent evt)
    {
        Console.WriteLine(">>> Title bar pressed.");
        Pressed = true;

        base.PointerPressEvent(evt);
    }

    protected override void PointerReleaseEvent(PointerEvent evt)
    {
        Pressed = false;

        base.PointerReleaseEvent(evt);
    }

    private uint _thickness;
    private Window _window;
}
