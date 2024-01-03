using UnityEngine;
using OccaSoftware.Bop.Runtime;

namespace OccaSoftware.Bop.Demo
{
    public class FetchFromPoolExample : MonoBehaviour
    {
        [SerializeField]
        public Pooler pooler;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GetFromPoolExample();
            }

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                DisposePoolExample();
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                CreateNewPoolExample();
            }
        }

        public void GetFromPoolExample()
        {
            pooler.GetFromPool();
        }

        public void DisposePoolExample()
        {
            pooler.DisposePool();
        }

        public void CreateNewPoolExample()
        {
            pooler.CreateNewPool(5);
        }

        public int GetPoolSize()
        {
            return pooler.GetPoolStats().PoolSize;
        }
    }
}
