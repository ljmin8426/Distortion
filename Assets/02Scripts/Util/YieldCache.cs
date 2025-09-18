using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal static class YieldCache
{
    public static readonly WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
    public static readonly WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    public static readonly Dictionary<float, WaitForSeconds> waitForSeconds = new Dictionary<float, WaitForSeconds>();

    public static WaitForSeconds WaitForSeconds(float seconds)
    {
        WaitForSeconds wfs;
        if (!waitForSeconds.TryGetValue(seconds, out wfs))
        {
            waitForSeconds.Add(seconds, wfs = new WaitForSeconds(seconds));
        }
        return wfs;
    }
}
