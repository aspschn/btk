namespace Blusher.Events;

using Blusher.Drawing;

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

    private EventType _eventType;
    private bool _propagation = true;
}

public class ResizeEvent : Event
{
    ResizeEvent(Size oldSize, Size size) : base(EventType.Resize)
    {
        //
    }
}
