namespace Blusher.Gui;

using Blusher.Foundation;

/// <summary>
/// A global application object.
/// </summary>
public class Application
{
    public Application(in string[] args)
    {
        int argc = args.Length;

        this._ftApplication = Foundation.ft_application_new(argc, args);
    }

    public int Exec()
    {
        return Foundation.ft_application_exec(this._ftApplication);
    }

    private IntPtr _ftApplication;
}
