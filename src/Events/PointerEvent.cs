using Btk.Drawing;
using Btk.Events;

public class PointerEvent : Event
{
    public PointerEvent(EventType eventType) : base(eventType)
    {
        //
    }

    public float X
    {
        get => this._position.X;
        internal set
        {
            this._position.X = value;
        }
    }

    public float Y
    {
        get => this._position.Y;
        internal set
        {
            this._position.Y = value;
        }
    }

    private Point _position;
}
