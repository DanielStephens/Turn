
public class OutputHolder<OUTPUT> {

    private OUTPUT output;

    public OutputHolder(OUTPUT output)
    {
        this.output = output;
    }

    public OUTPUT Get()
    {
        return output;
    }

}
