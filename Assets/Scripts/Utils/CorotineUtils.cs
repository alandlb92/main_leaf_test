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
    public static void WaiSecondsAndExecute(MonoBehaviour monoBehaviour, float time,Action execute)
    {
        monoBehaviour.StartCoroutine(RunWaiSecondsAndExecute(time, execute));
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
}
