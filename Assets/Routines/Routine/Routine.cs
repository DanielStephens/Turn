
public interface Routine<OUTPUT, PROGRESS> {

    Future<OUTPUT, PROGRESS> Start();

}
