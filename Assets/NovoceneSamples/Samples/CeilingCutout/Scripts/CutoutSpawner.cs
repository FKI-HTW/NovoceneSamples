using UnityEngine;

namespace AURORA.NovoceneSamples.CeilingCutout
{
    public class CutoutSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject cutoutPrefab;
        
        public void SpawnCutout()
        {
            if (Physics.Raycast(transform.position, Vector3.up, out var hit, float.PositiveInfinity, LayerMask.GetMask("Wall")))
                Instantiate(cutoutPrefab, hit.point, Quaternion.identity, transform);
        }
    }
}
