using UnityEngine;
using System.Collections;
using System;

public static class RotationUtils {

    public static Routine<Transform, Void> RotateAroundBySpeed(Transform transform, Func<float, Vector3> point, Func<float, Vector3> axis, Func<float, float> degreesPerSecond, float time)
    {
        return RoutineBuilder.Build<Transform, Void>(ops => rotateAround(ops, transform, point, axis, degreesPerSecond, time));
    }

    public static Routine<Transform, Void> RotateAroundBySpeed(Transform transform, Func<float, Vector3> point, Func<float, Vector3> axis, Func<float, float> degreesPerSecond)
    {
        return RoutineBuilder.Build<Transform, Void>(ops => rotateAroundForever(ops, transform, point, axis, degreesPerSecond));
    }

    public static Routine<Transform, Void> RotateAround(Transform transform, Func<float, Vector3> point, Func<float, Vector3> axis, float degrees, float time)
    {
        float degreesPerSecond = degrees / time;
        return RoutineBuilder.Build<Transform, Void>(ops => rotateAround(ops, transform, point, axis, t => degreesPerSecond, time));
    }

    private static IEnumerator rotateAround(RoutineOps<Transform, Void> ops, Transform transform, Func<float, Vector3> point, Func<float, Vector3> axis, Func<float, float> degreesPerSecond, float time)
    {
        float passed = 0;
        float dt = 0;
        float normalisedPass = 0;

        while (passed < time)
        {
            dt = passed + Time.deltaTime > time ? time - passed : Time.deltaTime;
            normalisedPass = passed / time;
            transform.RotateAround(point.Invoke(normalisedPass), axis.Invoke(normalisedPass), dt * degreesPerSecond.Invoke(normalisedPass));
            passed += dt;
            yield return ops.WaitForNextFrame();
        }

        yield return ops.Output(transform);
    }

    private static IEnumerator rotateAroundForever(RoutineOps<Transform, Void> ops, Transform transform, Func<float, Vector3> point, Func<float, Vector3> axis, Func<float, float> degreesPerSecond)
    {
        float passed = 0;
        float dt = 0;

        while (true)
        {
            dt = Time.deltaTime;
            transform.RotateAround(point.Invoke(passed), axis.Invoke(passed), dt * degreesPerSecond.Invoke(passed));
            passed += dt;
            yield return ops.WaitForNextFrame();
        }
    }

}
