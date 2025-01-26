using UnityEngine;
using Random = UnityEngine.Random;

namespace AURORA.NovoceneSamples.UnderwaterScene
{
    [RequireComponent(typeof(Rigidbody))]
    public class BuoyancySimulation : MonoBehaviour
    {
        public static float TargetHeight;
        
        [SerializeField] private float buoyancyForce = 10f;
        [SerializeField] private float dampingFactor = 1.5f;
        [SerializeField] private float maxHeight = 3f;

        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }
        private void FixedUpdate()
        {
            var height = Mathf.Clamp(TargetHeight, -1, maxHeight);
            var displacement = Mathf.Max(height - transform.position.y, 0);
            var buoyantForce = new Vector3(0, displacement * buoyancyForce, 0);
            var damping = -_rb.velocity * dampingFactor;

            var randomForceVertical = Random.Range(-0.2f, 0.2f);
            var randomForceHorizontal = Random.Range(-0.2f, 0.2f);
            var randomForce = new Vector3(randomForceHorizontal, randomForceVertical, randomForceHorizontal);
            _rb.AddForce(buoyantForce + damping + randomForce, ForceMode.Force);
            _rb.AddTorque(randomForce * 0.1f, ForceMode.Force);
        }
    }
}
