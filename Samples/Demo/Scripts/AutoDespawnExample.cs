using UnityEngine;
using OccaSoftware.Bop.Runtime;

namespace OccaSoftware.Bop.Demo
{
    public class AutoDespawnExample : MonoBehaviour
    {
        float timer;
        readonly float timeToDespawn = 5f;
        float r;

        private void OnEnable()
        {
            timer = 0f;
            r = Random.Range(0f, 1f);
            Debug.Log("Spawned: " + gameObject.name);
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= timeToDespawn)
            {
                if (r > 0.5f)
                    Destroy(gameObject);
                else
                    GetComponent<Instance>().Despawn();
            }
        }

        private void OnDisable()
        {
            Debug.Log("Despawned: " + gameObject.name);
        }

        private void OnDestroy()
        {
            Debug.Log("Destroyed / Removed From Pool: " + gameObject.name);
        }
    }
}
