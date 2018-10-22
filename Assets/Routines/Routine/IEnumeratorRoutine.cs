using UnityEngine;
using System.Collections;
using System;

public class IEnumeratorRoutine<OUTPUT, PROGRESS> : Routine<OUTPUT, PROGRESS> {

    private Func<IEnumerator> enumerator;

    public IEnumeratorRoutine(Func<IEnumerator> enumerator)
    {
        this.enumerator = enumerator;
    }

    public Future<OUTPUT, PROGRESS> Start()
    {
        StructWrapper<bool> paused = new StructWrapper<bool>(false);
        StructWrapper<bool> canceled = new StructWrapper<bool>(false);

        SimpleTask t = new SimpleTask(paused, canceled);
        Promise<OUTPUT, PROGRESS> promise = new Promise<OUTPUT, PROGRESS>(t);

        MonoBehaviour mb = MonoBehaviourHolder.MonoBehaviour();
        mb.StartCoroutine(instrument(enumerator.Invoke(), promise, paused, canceled));

        return promise as Future<OUTPUT, PROGRESS>;
    }

    private IEnumerator instrument(IEnumerator inner, Promise<OUTPUT, PROGRESS> promise, StructWrapper<bool> paused, StructWrapper<bool> canceled)
    {
        while (!canceled && inner.MoveNext())
        {
            object o = inner.Current;
            if(o == null)
            {
                yield return null;
            }
            else
            {
                System.Type t = o.GetType();
                if(t == typeof(OutputHolder<OUTPUT>))
                {
                    promise.Complete((o as OutputHolder<OUTPUT>).Get());
                }
                else if(t == typeof(ProgressHolder<PROGRESS>))
                {
                    promise.Progress((o as ProgressHolder<PROGRESS>).Get());
                }
                else if (t == typeof(IEnumeratorHolder))
                {
                    IEnumerator e = instrument((o as IEnumeratorHolder).Get(), promise, paused, canceled);
                    while (e.MoveNext())
                    {
                        yield return e.Current;
                    }
                }
                else
                {
                    yield return o;
                }
            }

            if (canceled)
            {
                yield break;
            }

            while (paused)
            {
                yield return null;
            }
        }
    }

}
