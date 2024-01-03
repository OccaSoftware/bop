using UnityEngine;

namespace OccaSoftware.Bop.Runtime
{
    public class Instance : MonoBehaviour
    {
        private Pooler origin;

        public Pooler GetPoolerOrigin()
        {
            return origin;
        }

        private int index = -1;

        public int GetIndex()
        {
            return index;
        }

        internal void Setup(Pooler origin, int index)
        {
            this.origin = origin;
            this.index = index;
        }

        public void Despawn()
        {
            origin.ReturnToPool(this);
        }
    }
}
