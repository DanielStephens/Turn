public class SimpleTask : Task {

    private readonly object syncLock = new object();

    private StructWrapper<bool> pause;
    private StructWrapper<bool> cancel;

    public SimpleTask(StructWrapper<bool> pause, StructWrapper<bool> cancel)
    {
        this.pause = pause;
        this.cancel = cancel;
    }

    public bool Pause()
    {
        lock (syncLock)
        {
            if (pause)
            {
                return false;
            }
            else
            {
                pause.Value = true;
                return true;
            }
        }
    }

    public bool Resume()
    {
        lock (syncLock)
        {
            if (!pause)
            {
                return false;
            }
            else
            {
                pause.Value = false;
                return true;
            }
        }
    }

    public bool Cancel()
    {
        lock (syncLock)
        {
            if (cancel)
            {
                return false;
            }
            else
            {
                cancel.Value = true;
                return true;
            }
        }
    }

}
