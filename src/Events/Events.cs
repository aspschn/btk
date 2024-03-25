namespace Blusher.Events;

using Blusher.Drawing;
using Blusher.Foundation;

public enum EventType
{
    PointerEnter,
    PointerLeave,
    PointerMove,
    PointerPress,
    PointerRelease,
    PointerClick,
    Move,
    Resize,
}

public class Event
{
    public Event(EventType eventType)
    {
        this._eventType = eventType;
    }

    public EventType EventType
    {
        get => this._eventType;
    }

    public bool Propagation
    {
        get => this._propagation;
        set
        {
            this._propagation = value;
        }
    }

    internal void SetFoundationEvent(IntPtr ftEvent)
    {
        this._ftEvent = ftEvent;
    }

    private IntPtr _ftEvent;
    private EventType _eventType;
    private bool _propagation = true;
}

public class ResizeEvent : Event
{
    public ResizeEvent(Size oldSize, Size size) : base(EventType.Resize)
    {
        this._oldSize = oldSize;
        this._size = size;
    }

    public Size OldSize
    {
        get => this._oldSize;
    }

    public Size Size
    {
        get => this._size;
    }

    private Size _oldSize;
    private Size _size;
}
