using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public static class Utils
    {
        /// <summary>
        /// Do an action after an amount of seconds
        /// </summary>
        /// <param name="seconds"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IEnumerator DoAfterSeconds(float seconds, Action action)
        {
            yield return new WaitForSeconds(seconds);
            action.Invoke();
        }
    }
}
