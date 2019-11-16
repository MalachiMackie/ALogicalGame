using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public class CoroutineWithData<T>
    {
        /// <summary>
        /// Gets or sets the Coroutine
        /// </summary>
        public Coroutine Coroutine { get; private set; }

        /// <summary>
        /// The result of the Coroutine
        /// </summary>
        public T Result;

        /// <summary>
        /// The target <see cref="IEnumerator"/>
        /// </summary>
        private readonly IEnumerator _target;

        /// <summary>
        /// Initializes a new instance of <see cref="CoroutineWithData{T}"/>
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="target"></param>
        public CoroutineWithData(MonoBehaviour owner, IEnumerator target)
        {
            _target = target;
            Coroutine = owner.StartCoroutine(Run());
        }

        /// <summary>
        /// Run the Coroutine
        /// </summary>
        /// <returns></returns>
        private IEnumerator Run()
        {
            while (_target.MoveNext())
            {
                if (_target.Current is T result)
                {
                    Result = result;
                }
                yield return Result;
            }
        }
    }
}
