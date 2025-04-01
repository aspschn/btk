namespace Btk.Events;

using Btk.Drawing;
using Btk.Input;

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

    public PointerButton Button { get; set; } = PointerButton.None;

    private Point _position;
}
