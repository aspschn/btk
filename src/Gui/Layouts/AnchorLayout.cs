namespace Btk.Gui.Layouts;

using Btk.Drawing;
using Btk.Events;

public enum Anchor
{
    Top,
    Bottom,
    Left,
    Right,
    HorizontalCenter,
    VerticalCenter,
    Fill,
    CenterIn,
}

public class AnchorLine
{
    public AnchorLine(View view, Anchor anchor)
    {
        View = view;
        Anchor = anchor;
    }

    internal void OnAnchorMove(View view, MoveEvent evt)
    {
        foreach (AnchorLayout layout in Subscribed.Keys)
        {
            switch (Subscribed[layout])
            {
                case Anchor.Top:
                    layout.OnTopAnchorMove(Anchor);
                    break;
                case Anchor.Bottom:
                    layout.OnBottomAnchorMove(Anchor);
                    break;
            }
        }
    }

    internal void OnAnchorResize(View view, ResizeEvent evt)
    {
        foreach (AnchorLayout layout in Subscribed.Keys)
        {
            switch (Subscribed[layout])
            {
                case Anchor.Top:
                    layout.OnTopAnchorResize(Anchor);
                    break;
                case Anchor.Bottom:
                    layout.OnBottomAnchorResize(Anchor);
                    break;
            }
        }
    }

    public View View { get; private set; }

    public Anchor Anchor { get; private set; }

    internal Dictionary<AnchorLayout, Anchor> Subscribed { get; } = [];
}

public class AnchorLayout
{
    public AnchorLayout(View view)
    {
        View = view;
    }

    public View View { get; private set; }

    public AnchorLine? Top
    {
        set
        {
            if (value != null)
            {
                TopAnchorView = value.View;
                value.Subscribed.Add(this, Anchor.Top);
            }
            else
            {
                TopAnchorView = null;
            }
        }
    }

    public AnchorLine? Bottom
    {
        set
        {
            if (value != null)
            {
                BottomAnchorView = value.View;
                Console.WriteLine("Anchor.Bottom added. " + this.GetHashCode());
                Console.WriteLine(" |- Target: " + value.GetHashCode());
                value.Subscribed.Add(this, Anchor.Bottom);
            }
            else
            {
                BottomAnchorView = null;
            }
        }
    }

    public View? Fill
    {
        set
        {
            if (value != null)
            {
                FillAnchorView = value;
                value.FillViews.Add(View);
            }
        }
    }

    internal View? TopAnchorView { get; set; } = null;
    internal View? BottomAnchorView { get; set; } = null;
    internal View? LeftAnchorView { get; set; } = null;
    internal View? RightAnchorView { get; set; } = null;
    internal View? FillAnchorView { get; set; } = null;

    //=====================
    // Anchor View Moved
    //=====================

    /// <summary>
    /// Top anchor view moved.
    /// </summary>
    internal void OnTopAnchorMove(Anchor destAnchor)
    {
        if (TopAnchorView == null)
        {
            return;
        }
        Rect anchorGeo = TopAnchorView.Geometry;
        if (destAnchor == Anchor.Top)
        {
            View.Geometry = new Rect(View.Geometry.X,
                anchorGeo.Y,
                View.Geometry.Width,
                View.Geometry.Height);
        } else if (destAnchor == Anchor.Bottom)
        {
            View.Geometry = new Rect(View.Geometry.X,
                anchorGeo.Y + anchorGeo.Height,
                View.Geometry.Width,
                View.Geometry.Height);
        }
    }

    /// <summary>
    /// Bottom anchor view moved.
    /// </summary>
    internal void OnBottomAnchorMove(Anchor destAnchor)
    {
        if (BottomAnchorView == null)
        {
            return;
        }
        Rect anchorGeo = BottomAnchorView.Geometry;
        if (destAnchor == Anchor.Top)
        {
            View.Geometry = new Rect(View.Geometry.X,
                anchorGeo.Y - View.Geometry.Height,
                View.Geometry.Width,
                View.Geometry.Height);
        } else if (destAnchor == Anchor.Bottom)
        {
            View.Geometry = new Rect(View.Geometry.X,
                anchorGeo.Y + anchorGeo.Height - View.Geometry.Height,
                View.Geometry.Width,
                View.Geometry.Height);
        }
    }

    //=======================
    // Anchor View Resized
    //=======================
    internal void OnTopAnchorResize(Anchor destAnchor)
    {
        if (TopAnchorView == null)
        {
            return;
        }
        Rect anchorGeo = TopAnchorView.Geometry;
        if (destAnchor == Anchor.Bottom)
        {
            View.Geometry = new Rect(View.Geometry.X,
                anchorGeo.Height,
                View.Geometry.Width,
                View.Geometry.Height);
        } else if (destAnchor == Anchor.Top)
        {
            // TODO.
        }
    }

    internal void OnBottomAnchorResize(Anchor destAnchor)
    {
        if (BottomAnchorView == null)
        {
            return;
        }
        Rect anchorGeo = BottomAnchorView.Geometry;
        if (destAnchor == Anchor.Bottom)
        {
            View.Geometry = new Rect(View.Geometry.X,
                anchorGeo.Y + anchorGeo.Height - View.Geometry.Height,
                View.Geometry.Width,
                View.Geometry.Height);
        } else if (destAnchor == Anchor.Top)
        {
            //
        }
    }
}
