using System;

public interface CompletionStage<OUTPUT, PROGRESS> {

	CompletionStage<R, PROGRESS> Then<R>(Func<OUTPUT, R> then);

	CompletionStage<OUTPUT, PROGRESS> Then(Action<OUTPUT> then);

    bool OnProgression(Action<PROGRESS> progression);

}
