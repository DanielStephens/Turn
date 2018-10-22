
public interface State<OUTPUT, PROGRESS> {

    PROGRESS Progress();

    OUTPUT Get();

    bool IsDone();

}
