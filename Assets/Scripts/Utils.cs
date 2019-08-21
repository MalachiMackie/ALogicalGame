using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public static class Utils
    {
        public static IEnumerator DoAfterSeconds(float seconds, Action action)
        {
            yield return new WaitForSeconds(seconds);
            action.Invoke();
        }
    }
}
