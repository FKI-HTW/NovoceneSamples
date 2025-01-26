using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using static OVRSpatialAnchor;

namespace AURORA.NovoceneSamples.SceneAnchoring
{
    public class SpatialAnchorManager : MonoBehaviour
    {
        private OVRSpatialAnchor _anchorPrefab;
        private const string AMOUNT_UUIDS_PLAYER_PREF = "numUuids";

        public async Task SaveAnchor(OVRSpatialAnchor anchor, string name = "", float scale = 1)
        {
            if ((await anchor.SaveAnchorAsync()).Success)
            {
                SaveUuidToPlayerPrefs(anchor.Uuid, name == "" ? anchor.gameObject.name : name, scale);
            }
            else
                Debug.LogError("Failed to save Anchor!");
        }
        
        private void SaveUuidToPlayerPrefs(Guid uuid, string objectName, float scale)
        {
            if (!PlayerPrefs.HasKey(AMOUNT_UUIDS_PLAYER_PREF))
                PlayerPrefs.SetInt(AMOUNT_UUIDS_PLAYER_PREF, 0);

            int playerNumUuids = PlayerPrefs.GetInt(AMOUNT_UUIDS_PLAYER_PREF);
            PlayerPrefs.SetString("uuid" + playerNumUuids, uuid.ToString());
            PlayerPrefs.SetString("objectName" + playerNumUuids, objectName);
            PlayerPrefs.SetFloat("objectScale" + playerNumUuids, scale);
            PlayerPrefs.SetInt(AMOUNT_UUIDS_PLAYER_PREF, ++playerNumUuids);
            PlayerPrefs.Save();
        }
        
        private void SaveAllAnchors()
        {
            OVRSpatialAnchor[] foundAnchors = FindObjectsOfType<OVRSpatialAnchor>();

            foreach (OVRSpatialAnchor item in foundAnchors)
            {
                _ = SaveAnchor(item);
            }
        }
        
        public async void UnsaveAnchor(OVRSpatialAnchor anchor)
        {
            await anchor.EraseAnchorAsync();
        }
        
        public void UnsaveAllAnchors()
        {
            OVRSpatialAnchor[] foundAnchors = FindObjectsOfType<OVRSpatialAnchor>();

            foreach (OVRSpatialAnchor anchor in foundAnchors)
            {
                UnsaveAnchor(anchor);
                Destroy(anchor);
            }

            ClearAllUuidsFromPlayerPrefs();
        }
        
        private void ClearAllUuidsFromPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }

        public async void LoadAnchors(Action<UnboundAnchor, AnchorData, Pose> onAnchorLoaded)
        {
            if (!PlayerPrefs.HasKey(AMOUNT_UUIDS_PLAYER_PREF))
                PlayerPrefs.SetInt(AMOUNT_UUIDS_PLAYER_PREF, 0);

            int playerUuidCount = PlayerPrefs.GetInt(AMOUNT_UUIDS_PLAYER_PREF);
            Dictionary<Guid, AnchorData> uuidToObjectMap = new(); // Uuid to name

            if (playerUuidCount == 0)
                return;

            for (int i = 0; i < playerUuidCount; ++i)
            {
                Guid uuid = new Guid(PlayerPrefs.GetString("uuid" + i));
                string objectName = PlayerPrefs.GetString("objectName" + i);
                float objectScale = PlayerPrefs.GetFloat("objectScale" + i);
                uuidToObjectMap[uuid] = new AnchorData(objectName, objectScale);
            }

            List<UnboundAnchor> unboundAnchors = new();
            var result = await LoadUnboundAnchorsAsync(uuidToObjectMap.Keys.ToArray(), unboundAnchors);

            if (!result.Success)
            {
                Debug.LogError($"Error while loading anchor: {result.Status}");
                return;
            }

            foreach (UnboundAnchor unboundAnchor in unboundAnchors)
            {
                unboundAnchor.LocalizeAsync().ContinueWith(success =>
                {
                    if (!success) return;
                    if (unboundAnchor.TryGetPose(out Pose pose))
                    {
                        onAnchorLoaded?.Invoke(unboundAnchor, uuidToObjectMap[unboundAnchor.Uuid], pose);
                    }
                });
            }
        }
    }

    public class AnchorData
    {
        public string Name { get; set; }
        public float Scale { get; set; }

        public AnchorData(string name, float scale)
        {
            Name = name;
            Scale = scale;
        }
    }
}