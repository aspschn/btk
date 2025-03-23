namespace Btk.Events;

using Btk.Drawing;
using Btk.Swingby;

public enum EventType
{
    PointerEnter,
    PointerLeave,
    PointerMove,
    PointerPress,
    PointerRelease,
    PointerClick,
    PointerDoubleClick,
    PointerScroll,
    Move,
    Resize,
    StateChange,
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
            bool propagation = Swingby.sb_event_propagation(_sbEvent);
            return propagation;
        }
        set
        {
            Swingby.sb_event_set_propagation(_sbEvent, value);
        }
    }

    internal void SetSwingbyEvent(IntPtr sbEvent)
    {
        this._sbEvent = sbEvent;
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
