using UnityEngine;
using System.Collections;

public class RoutineOps<OUTPUT, PROGRESS> {

    public WaitForSeconds WaitForSeconds(int seconds)
    {
        return new WaitForSeconds(seconds);
    }

    public YieldInstruction WaitForNextFrame()
    {
        return null;
    }
    public WaitForFixedUpdate WaitForFixedUpdate()
    {
        return new WaitForFixedUpdate();
    }

    public IEnumeratorHolder WaitForSubRoutine(IEnumerator enumerator)
    {
        return new IEnumeratorHolder(enumerator);
    }

    public ProgressHolder<PROGRESS> Progress(PROGRESS progress)
    {
        return new ProgressHolder<PROGRESS>(progress);
    }

    public OutputHolder<OUTPUT> Output(OUTPUT output)
    {
        return new OutputHolder<OUTPUT>(output);
    }

}
