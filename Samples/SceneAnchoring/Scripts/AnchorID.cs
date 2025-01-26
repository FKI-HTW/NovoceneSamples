using UnityEngine;
using static Meta.XR.MRUtilityKit.MRUKAnchor;

namespace AURORA.NovoceneSamples.SceneAnchoring
{
    public class AnchorID : MonoBehaviour
    {
        public string anchorName;
        public SceneLabels allowedLabels = SceneLabels.FLOOR | SceneLabels.WALL_FACE;

        private void Reset()
        {
            anchorName = gameObject.name;
        }
    }
}