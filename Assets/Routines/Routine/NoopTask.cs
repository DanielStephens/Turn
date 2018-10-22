
public class NoopTask : Task
{

    public bool Pause()
    {
        return true;
    }

    public bool Resume()
    {
        return true;
    }

    public bool Cancel()
    {
        return true;
    }

}