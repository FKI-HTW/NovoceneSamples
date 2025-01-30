using Meta.XR.MRUtilityKit;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace AURORA.NovoceneSamples.SceneAnchoring
{
    [RequireComponent(typeof(SpatialAnchorManager))]
    public class AnchorObjectPlacer : MonoBehaviour
    {
        [SerializeField] private List<AnchorID> anchorObjects = new();
        [SerializeField] private Transform rayTransform;
        [SerializeField] private float minScaleFactor = 0.7f;
        [SerializeField] private float maxScaleFactor = 1.2f;

        private SpatialAnchorManager _spatialAnchorManager;
        private AnchorID _selectedObject;
        private LabelFilter _selectedObjectLabelFilter;

        private readonly List<MRUKRoom> _rooms = new();

        private void Awake()
        {
            _spatialAnchorManager = GetComponent<SpatialAnchorManager>();
            StartCoroutine(BindAnchorsCoroutine());
        }

        private void Update()
        {
            ReadUserInput();
        }

        public void AddLoadedRoom(MRUKRoom room)
        {
            _rooms.Add(room);
        }

        public void RemoveLoadedRoom(MRUKRoom room)
        {
            _rooms.Remove(room);
        }

        public void SetSelectedAnchor(AnchorID anchorID)
        {
            _selectedObject = anchorID;
            _selectedObjectLabelFilter = new(anchorID.allowedLabels);
            if (anchorID.gameObject.TryGetComponent(out OVRSpatialAnchor anchor))
            {
                _spatialAnchorManager.UnsaveAnchor(anchor);
                Destroy(anchor);
            }
        }

        private IEnumerator BindAnchorsCoroutine()
        {
            bool anchorsLoaded = false;

            _spatialAnchorManager.LoadAnchors((_, anchorData, pose) =>
            {
                AnchorID sceneObject = anchorObjects.FirstOrDefault(obj => obj.anchorName == anchorData.Name);
                if (sceneObject != null)
                {
                    var trf =  sceneObject.transform;
                    trf.SetPositionAndRotation(pose.position, pose.rotation);
                    trf.localScale = Vector3.one * anchorData.Scale;
                }
                anchorsLoaded = true;
            });

            // Wait until markers are bound
            yield return new WaitUntil(() => anchorsLoaded);
        }

        private void ReadUserInput()
        {
            Vector3 rayOrigin = rayTransform.position;
            Vector3 rayDirection = rayTransform.forward;
            
            // Select object
            if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
            {
                TrySelectObject(rayOrigin, rayDirection);
            }
            
            // Deselect object
            if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
            {
                _selectedObject = null;
                _selectedObjectLabelFilter = default;
            }
            
            if (!_selectedObject) 
                return;
            
            // Rotate object
            Vector2 rightStickInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
            if (rightStickInput.magnitude > 0.1f)
            {
                const float rotationSpeed = 100f;
                float currentRotationY = _selectedObject.transform.eulerAngles.y;
                currentRotationY += rightStickInput.x * rotationSpeed * Time.deltaTime;
                _selectedObject.transform.rotation = Quaternion.Euler(0, currentRotationY, 0);
            }

            // Scale object
            Vector2 leftStickInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);
            if (leftStickInput.magnitude > 0.1f)
            {
                const float scaleSpeed = 1.0f;
                float scaleChange = leftStickInput.x * scaleSpeed * Time.deltaTime;
                float newScaleFactor = Mathf.Clamp(_selectedObject.transform.localScale.x + scaleChange, minScaleFactor, maxScaleFactor);
                _selectedObject.transform.localScale = newScaleFactor * Vector3.one;
            }

            // Position raycast against all loaded rooms
            foreach (var room in _rooms)
            {
                if (room.Raycast(new(rayOrigin, rayDirection), Mathf.Infinity, _selectedObjectLabelFilter, out var hit))
                {
                    _selectedObject.transform.position = hit.point;
                }
            }

            // Save anchor and deselect
            if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
            {
                SaveAnchor();
            }
            
            // Remove all anchors
            if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch))
            {
                foreach (AnchorID item in anchorObjects)
                {
                    if (!item.gameObject.TryGetComponent(out OVRSpatialAnchor anchor)) continue;
                    _spatialAnchorManager.UnsaveAnchor(anchor);             
                    Destroy(anchor);
                }
            }
        }
        
        private void TrySelectObject(Vector3 rayOrigin, Vector3 rayDirection)
        {
            RaycastHit[] hits = Physics.RaycastAll(rayOrigin, rayDirection, Mathf.Infinity);
            Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            foreach (RaycastHit hit in hits)
            {
                if (!hit.transform.TryGetComponent(out AnchorID anchorObject)) continue;
                SetSelectedAnchor(anchorObject);
                return;
            }
        }

        private async void SaveAnchor()
        {
            if (!_selectedObject.gameObject.TryGetComponent(out OVRSpatialAnchor anchor))
                anchor = _selectedObject.gameObject.AddComponent<OVRSpatialAnchor>();
            
            await Task.Delay(100);
            await _spatialAnchorManager.SaveAnchor(anchor, _selectedObject.anchorName, _selectedObject.transform.lossyScale.x);
            _selectedObject = null;
            _selectedObjectLabelFilter = default;
        }
    }
}
