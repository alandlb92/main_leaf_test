using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CorotineUtils
{
    public static void WaitFixedUpdateAndExecute(MonoBehaviour monoBehaviour, Action execute)
    {
        monoBehaviour.StartCoroutine(RunWaitFixedUpdateAndExecute(execute));
    }

    public static Coroutine WaiSecondsAndExecute(MonoBehaviour monoBehaviour, float time,Action execute)
    {
        return monoBehaviour.StartCoroutine(RunWaiSecondsAndExecute(time, execute));
    }

    public static void WaitEndOfFrameAndExecute(MonoBehaviour monoBehaviour, Action execute)
    {
        monoBehaviour.StartCoroutine(RunWaitEndOfFrameAndExecute(execute));
    }

    private static IEnumerator RunWaiSecondsAndExecute(float time, Action execute)
    {
        yield return new WaitForSeconds(time);
        execute?.Invoke();
    }

    private static IEnumerator RunWaitFixedUpdateAndExecute(Action execute)
    {
        yield return new WaitForFixedUpdate();
        execute?.Invoke();
    }

    private static IEnumerator RunWaitEndOfFrameAndExecute(Action execute)
    {
        yield return new WaitForEndOfFrame();
        execute?.Invoke();
    }
}
