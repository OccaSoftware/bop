using System.Collections.Generic;
using UnityEngine;

namespace OccaSoftware.Bop.Runtime
{
    public class Pooler : MonoBehaviour
    {
        [SerializeField]
        private GameObject objectToPool;

        [SerializeField, Min(1)]
        private int initialCount = 10;

        [SerializeField]
        private Vector3 storagePosition = Vector3.zero;

        public List<ObjectInstance> Pool { get; } = new();

        private GameObject cachedObjectReference;

        private void Awake()
        {
            objectToPool.SetActive(false);
            cachedObjectReference = objectToPool;
            CreateNewPool(initialCount);
        }

        /// <summary>
        /// Gets a fresh object from the pool. If none is available, a new one is dynamically created.
        /// </summary>
        private int GetNextIndex()
        {
            for (int a = 0; a < Pool.Count; a++)
            {
                if (!Pool[a].IsActive() && Pool[a].GetObject() != null)
                {
                    return a;
                }
            }

            return IncreasePoolSize(1);
        }

        public GameObject GetFromPool()
        {
            int a = GetNextIndex();
            return Pool[a].ActivateAndGetInstance();
        }

        public GameObject GetFromPool(Vector3 position, Quaternion rotation)
        {
            int a = GetNextIndex();
            return Pool[a].ActivateAndGetInstance(position, rotation);
        }

        public GameObject GetFromPool(Transform parent)
        {
            int a = GetNextIndex();
            return Pool[a].ActivateAndGetInstance(parent);
        }

        public GameObject GetFromPool(Vector3 position, Quaternion rotation, Transform parent)
        {
            int a = GetNextIndex();
            return Pool[a].ActivateAndGetInstance(position, rotation, parent);
        }

        internal void ReturnToPool(Instance instance)
        {
            Pool[instance.GetIndex()].SetInactive(storagePosition);
        }

        private ObjectInstance CreateObjectInstance(int i)
        {
            if (objectToPool != cachedObjectReference)
            {
                Debug.LogError(
                    "The object registered to this pooler has been changed since initialization. It is recommended that you avoid changing pooler objects during runtime."
                );
            }

            return new ObjectInstance(this, i);
        }

        private void DestroyObjectInstance(int i)
        {
            Destroy(Pool[i].GetObject());
        }

        /// <summary>
        /// Increases the pool size and returns the index of the first new instance.
        /// </summary>
        /// <param name="count">Number of additional items to add to the pool.</param>
        public int IncreasePoolSize(int count)
        {
            int initialPoolSize = Pool.Count;
            for (int a = 0; a < count; a++)
            {
                ObjectInstance newInstance = CreateObjectInstance(initialPoolSize + a);
                Pool.Add(newInstance);
            }

            return initialPoolSize;
        }

        /// <summary>
        /// Disposes of any existing pool, then sets up a fresh pool.
        /// </summary>
        /// <param name="count">Number of items to set in the pool.</param>
        public void CreateNewPool(int count)
        {
            DisposePool();

            for (int a = 0; a < count; a++)
            {
                ObjectInstance newInstance = CreateObjectInstance(a);
                Pool.Add(newInstance);
            }
        }

        /// <summary>
        /// Destroys all extant instanced objects, then clears the pool.
        /// </summary>
        public void DisposePool()
        {
            for (int a = 0; a < Pool.Count; a++)
            {
                DestroyObjectInstance(a);
            }

            Pool.Clear();
        }

        private void OnDestroy()
        {
            DisposePool();
        }

        public PoolStatistics GetPoolStats()
        {
            return new PoolStatistics(this);
        }

        public class ObjectInstance
        {
            private readonly GameObject instancedObject;
            private bool isActive;
            private readonly Instance instance;

            internal ObjectInstance(Pooler origin, int index)
            {
                instancedObject = Instantiate(origin.objectToPool);
                instancedObject.name = origin.objectToPool.name + "-" + origin.name + "-" + index;
                instance = instancedObject.AddComponent<Instance>();
                instance.Setup(origin, index);
                SetInactive(origin.storagePosition);
            }

            internal void SetActive()
            {
                SetState(true);
            }

            internal void SetInactive(Vector3 storagePosition)
            {
                SetState(false);
                instancedObject.transform.SetPositionAndRotation(
                    storagePosition,
                    Quaternion.identity
                );
                instancedObject.transform.SetParent(null);
            }

            private void SetState(bool state)
            {
                isActive = state;
                instancedObject.SetActive(state);
            }

            public bool IsActive()
            {
                return isActive;
            }

            internal GameObject ActivateAndGetInstance()
            {
                SetActive();
                return instancedObject;
            }

            internal GameObject ActivateAndGetInstance(Vector3 position, Quaternion rotation)
            {
                SetActive();
                instancedObject.transform.SetPositionAndRotation(position, rotation);
                return instancedObject;
            }

            internal GameObject ActivateAndGetInstance(Transform parent)
            {
                SetActive();
                instancedObject.transform.SetParent(parent);
                return instancedObject;
            }

            internal GameObject ActivateAndGetInstance(
                Vector3 position,
                Quaternion rotation,
                Transform parent
            )
            {
                SetActive();
                instancedObject.transform.SetPositionAndRotation(position, rotation);
                instancedObject.transform.SetParent(parent);
                return instancedObject;
            }

            public GameObject GetObject()
            {
                return instancedObject;
            }

            public Instance GetInstance()
            {
                return instance;
            }
        }
    }
}
