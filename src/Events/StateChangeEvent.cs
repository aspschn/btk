namespace Btk.Events;

using Btk.Gui.Rendering;

public class StateChangeEvent : Event
{
    public StateChangeEvent(ToplevelState state, bool value) : base(Events.EventType.StateChange)
    {
        State = state;
        Value = value;
    }

    public ToplevelState State { get; set; }

    public bool Value { get; set; }
}
