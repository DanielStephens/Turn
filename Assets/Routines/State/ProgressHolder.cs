
public class ProgressHolder<PROGRESS> {

    private PROGRESS progress;

    public ProgressHolder(PROGRESS progress)
    {
        this.progress = progress;
    }

    public PROGRESS Get()
    {
        return progress;
    }

}
