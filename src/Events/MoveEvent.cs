namespace Btk.Events;

using Btk.Drawing;

public class MoveEvent : Event
{
    public MoveEvent(Point oldPosition, Point position) : base(EventType.Move)
    {
        OldPosition = oldPosition;
        Position = position;
    }

    public Point OldPosition
    {
        get;
        init;
    }

    public Point Position
    {
        get;
        init;
    }
}
