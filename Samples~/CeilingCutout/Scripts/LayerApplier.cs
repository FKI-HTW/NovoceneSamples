using Meta.XR.MRUtilityKit;
using UnityEngine;

namespace AURORA.NovoceneSamples.CeilingCutout
{
    public class LayerApplier : MonoBehaviour
    {
        public void ApplyMRUKLayers(MRUKRoom room)
        {
            foreach (var anchor in room.Anchors)
            {
                ApplyLayer(anchor.gameObject, anchor.Label
                    is MRUKAnchor.SceneLabels.FLOOR
                    or MRUKAnchor.SceneLabels.CEILING
                    or MRUKAnchor.SceneLabels.WALL_FACE
                        ? LayerMask.GetMask("Wall")
                        : LayerMask.GetMask("Furniture"));
            }
        }

        private static void ApplyLayer(GameObject obj, LayerMask layer)
        {
            obj.layer = Mathf.RoundToInt(Mathf.Log(layer.value, 2));
            foreach (Transform child in obj.transform)
                ApplyLayer(child.gameObject, layer);
        }
    }
}
