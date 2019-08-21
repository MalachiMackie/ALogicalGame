using System;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class LogicOutput : MonoBehaviour
    {
        public abstract bool Output { get; protected set; }

        public abstract void SetOutput();

        public abstract event EventHandler<bool> OutputUpdated;

        protected virtual void Start()
        {
            
        }

        protected virtual void Update()
        {

        }
    }
}
