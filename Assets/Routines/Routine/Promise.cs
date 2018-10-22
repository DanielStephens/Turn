using System;
using System.Collections.Generic;

public class Promise<OUTPUT, PROGRESS> : Future<OUTPUT, PROGRESS> {

    private readonly object syncLock = new object();

    private List<Action<PROGRESS>> onProgressActions = new List<Action<PROGRESS>>();
    private List<Action<OUTPUT>> onCompletionActions = new List<Action<OUTPUT>>();
    private List<Action<Exception>> onExceptionActions = new List<Action<Exception>>();

    private Task task;
    private PROGRESS progress;
    private OUTPUT output;
    private Exception exception;

    private bool progression = false;
    private bool successful = false;
    private bool finished = false;

    public Promise(Task task)
    {
        this.task = task;
    }

    public bool Pause()
    {
        return task.Pause();
    }

    public bool Resume()
    {
        return task.Resume();
    }

    public bool Cancel()
    {
        lock (syncLock)
        {
            task.Cancel();
            return CompleteExceptionally(null); // TODO cancel exception
        }
    }

    public PROGRESS Progress()
    {
        return progress;
    }

    public OUTPUT Get()
    {
        if (finished)
        {
            if (successful)
            {
                return output;
            }

            throw exception;
        }
        throw new InvalidOperationException("Get cannot be called on an incomplete Future.");
    }

    public bool IsDone()
    {
        return finished;
    }

    public bool Progress(PROGRESS progress)
    {
        lock (syncLock)
        {
            if (finished)
            {
                return false;
            }

            this.progression = true;
            this.progress = progress;
            foreach(Action<PROGRESS> a in onProgressActions)
            {
                a.Invoke(progress);
            }

            return true;
        }
    }

    public bool Complete(OUTPUT output)
    {
        lock (syncLock)
        {
            if (finished)
            {
                return false;
            }

            successful = true;
            finished = true;
            this.output = output;
            foreach (Action<OUTPUT> a in onCompletionActions)
            {
                a.Invoke(output);
            }

            return true;
        }
    }

    public bool CompleteExceptionally(Exception exception)
    {
        lock (syncLock)
        {
            if (finished)
            {
                return false;
            }

            successful = false;
            finished = true;
            this.exception = exception;
            foreach (Action<Exception> a in onExceptionActions)
            {
                a.Invoke(exception);
            }

            return true;
        }
    }

    public CompletionStage<R, PROGRESS> Then<R>(Func<OUTPUT, R> then)
    {
        Promise<R, PROGRESS> p = new Promise<R, PROGRESS>(new NoopTask());
        lock (syncLock)
        {
            if (finished)
            {
                if (successful)
                {
                    p.Complete(then.Invoke(output));
                }
                else
                {
                    p.CompleteExceptionally(exception);
                }
            }
            else
            {
                onCompletionActions.Add(o => p.Complete(then.Invoke(o)));
                onExceptionActions.Add(e => p.CompleteExceptionally(e));
            }
        }
        return p as CompletionStage<R, PROGRESS>;
    }

	public CompletionStage<OUTPUT, PROGRESS> Then(Action<OUTPUT> then){
        return Then<OUTPUT>(o =>
        {
            then.Invoke(o);
            return o;
        });
	}

    public bool OnProgression(Action<PROGRESS> progression)
    {
        lock (syncLock)
        {
            if (finished)
            {
                return false;
            }

            if (this.progression)
            {
                progression.Invoke(progress);
            }

            onProgressActions.Add(progression);

            return true;
        }
    }

}
