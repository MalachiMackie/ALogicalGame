using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class CoroutineWithData<T>
    {
        public Coroutine Coroutine { get; private set;  }

        public T Result;

        private readonly IEnumerator _target;

        public CoroutineWithData(MonoBehaviour owner, IEnumerator target)
        {
            _target = target;
            Coroutine = owner.StartCoroutine(Run());
        }

        private IEnumerator Run()
        {
            while(_target.MoveNext())
            {
                if(_target.Current is T result)
                {
                    Result = result;
                }
                yield return Result;
            }
        }
    }
}
