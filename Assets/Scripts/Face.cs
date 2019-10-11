using UnityEngine;

namespace Assets.Scripts
{
    public class Face : MonoBehaviour
    {
        protected MeshRenderer MeshRenderer;

        protected bool LookedAt;

        [SerializeField]
        protected Material LookingAtMat;

        [SerializeField]
        protected Material NotLookingAtMat;

        public bool CanLookAt = true;

        protected virtual void Start()
        {
            MeshRenderer = GetComponent<MeshRenderer>();
            gameObject.tag = "Face";
        }

        public void StartLookingAt()
        {
            if (!LookedAt)
            {
                LookedAt = true;
                MeshRenderer.material = LookingAtMat;
            }
        }

        public void StopLookingAt()
        {
            LookedAt = false;
            MeshRenderer.material = NotLookingAtMat;
        }
    }
}
