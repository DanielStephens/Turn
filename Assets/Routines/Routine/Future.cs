
public interface Future<OUTPUT, PROGRESS> : Task, State<OUTPUT, PROGRESS>, CompletionStage<OUTPUT, PROGRESS> {

}
