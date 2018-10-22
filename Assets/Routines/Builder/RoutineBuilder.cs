using System.Collections;
using System;

public static class RoutineBuilder {

    public static Routine<OUTPUT, PROGRESS> Build<OUTPUT, PROGRESS>(Func<RoutineOps<OUTPUT, PROGRESS>, IEnumerator> builder)
    {
        RoutineOps<OUTPUT, PROGRESS> ops = new RoutineOps<OUTPUT, PROGRESS>();
        return new IEnumeratorRoutine<OUTPUT, PROGRESS>(() => builder.Invoke(ops));
    }
	
}
