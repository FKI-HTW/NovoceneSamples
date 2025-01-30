using System.Collections;
using System.Threading;
using UnityEngine;

namespace AURORA.NovoceneSamples.UnderwaterScene
{
    public class WaterVolume : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private new Camera camera;
        [SerializeField] private new Light light;
        [SerializeField] private OVRPassthroughLayer passthroughLayer;
        
        [Header("Settings")]
        [SerializeField] private float raiseDuration;
        [SerializeField] private float waveHeightOffset;
        [SerializeField] private float bottomHeight;
        [SerializeField] private float topHeight;
        
        [SerializeField, ColorUsage(true, true)] private Color underWaterPassthrough = Vector4.one;
        [SerializeField, ColorUsage(true, true)] private Color underWaterLightColour;
        [SerializeField] private float colorTransitionEnd;

        private void Start()
        {
            BuoyancySimulation.TargetHeight = transform.position.y + waveHeightOffset;
            
            // DEBUG : remove this if the water shouldn't raise immediately
            RaiseWater();
        }

        public void RaiseWater() => RaiseWater(default);
        public void RaiseWater(CancellationToken skipToken)
        {
            StartCoroutine(ChangeWaterHeight(topHeight, skipToken));
        }

        public void LowerWater() => LowerWater(default);
        public void LowerWater(CancellationToken skipToken)
        {
            StartCoroutine(ChangeWaterHeight(bottomHeight, skipToken));
        }

        private IEnumerator ChangeWaterHeight(float targetHeight, CancellationToken skipToken)
        {
            var animationTime = 0f;
            var trf = transform;
            var startPosition = trf.position;
            var targetPosition = new Vector3(0, targetHeight, 0);
            
            while (animationTime < raiseDuration && !skipToken.IsCancellationRequested)
            {
                yield return null;
                animationTime += Time.deltaTime;
                var timePassed = Mathf.Clamp01(animationTime / raiseDuration);
                var newPos = Vector3.Lerp(startPosition, targetPosition, timePassed);
                trf.position = newPos;
                
                var offsetWave = newPos.y + waveHeightOffset;
                var camY = camera.transform.position.y;
                var distance = offsetWave - camY;
                DistanceToWaterSurface = distance;
                BuoyancySimulation.TargetHeight = offsetWave;
            }
            
            trf.position = targetPosition;
            DistanceToWaterSurface = targetPosition.y + waveHeightOffset;
            BuoyancySimulation.TargetHeight = targetPosition.y + waveHeightOffset;
        }
        
        private Color _lightColor = Color.white;
        private Color _passthroughColor = Color.white;

        private float DistanceToWaterSurface
        {
            set
            {
                var t = Mathf.Clamp(value, 0, colorTransitionEnd) / colorTransitionEnd;
                
                _lightColor = Color.Lerp(Color.white, underWaterLightColour, t);
                _passthroughColor = Color.Lerp(Color.white, underWaterPassthrough, t);
                
                light.color = _lightColor;
                passthroughLayer.colorScale = _passthroughColor;
                passthroughLayer.overridePerLayerColorScaleAndOffset = true;
            }
        }
    }
}
