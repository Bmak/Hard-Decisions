
using System;
using System.Collections;
using UnityEngine;

public static class Extensions
{
    public static void DelayedMethod(this MonoBehaviour obj, float delay, Action callBack)
    {
        obj.StartCoroutine(DelayMethod(delay, callBack));
    }

    private static IEnumerator DelayMethod(float delay, Action callBack)
    {
        yield return new WaitForSeconds(delay);

        callBack();

        yield return null;
    }

}
