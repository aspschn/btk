namespace Blusher.Gui;

using Blusher.Swingby;

/// <summary>
/// A global application object.
/// </summary>
public class Application
{
    public Application(in string[] args)
    {
        int argc = args.Length;

        this._sbApplication = Swingby.sb_application_new(argc, args);
    }

    public int Exec()
    {
        return Swingby.sb_application_exec(this._sbApplication);
    }

    private IntPtr _sbApplication;
}
