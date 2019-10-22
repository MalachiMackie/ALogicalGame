using UnityEngine;

namespace Assets.Scripts
{
    public class Face : MonoBehaviour
    {
        protected Renderer Renderer;

        protected bool LookedAt;

        public bool CanLookAt = true;

        [SerializeField]
        private Material _lookingAtMat;

        [SerializeField]
        protected Material _invisibleMat;

        protected virtual void Awake()
        {
            Renderer = GetComponent<Renderer>();
            gameObject.tag = "Face";

            StopLookingAt();
        }

        public void StartLookingAt()
        {
            if (!LookedAt)
            {
                LookedAt = true;

                var mats = Renderer.materials;
                mats[mats.Length - 1] = _lookingAtMat;
                Renderer.materials = mats;
            }
        }

        public void StopLookingAt()
        {
            LookedAt = false;

            if(Renderer != null)
            {
                var mats = Renderer.materials;
                mats[mats.Length - 1] = _invisibleMat;
                Renderer.materials = mats;
            }
        }
    }
}
