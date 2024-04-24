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

    internal Event(IntPtr sbEvent)
    {
        _sbEvent = sbEvent;
        _eventType = 0; // TODO: Get from Swingby event object.
    }

    public EventType EventType
    {
        get => this._eventType;
    }

    public bool Propagation
    {
        get
        {
            bool propagation = Foundation.ft_event_propagation(_sbEvent);
            return propagation;
        }
        set
        {
            Foundation.ft_event_set_propagation(_sbEvent, value);
        }
    }

    internal void SetFoundationEvent(IntPtr ftEvent)
    {
        this._sbEvent = ftEvent;
    }

    private IntPtr _sbEvent;
    private EventType _eventType;
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
