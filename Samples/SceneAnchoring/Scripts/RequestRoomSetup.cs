using Meta.XR.MRUtilityKit;
using UnityEngine;

namespace AURORA.NovoceneSamples.SceneAnchoring
{
    public class RequestRoomSetup : MonoBehaviour
    {
        [SerializeField] private MRUK mruk;

        /// <summary>
        /// Triggers the Room Setup workflow, even if a room setup is already in place.
        /// </summary>
        public async void InitiateRoomSetup()
        {
            if (!mruk)
            {
                Debug.LogError("Scene manager not set, cannot initiate room setup!");
                return;
            }

            _ = await OVRScene.RequestSpaceSetup();
        }
    }
}
