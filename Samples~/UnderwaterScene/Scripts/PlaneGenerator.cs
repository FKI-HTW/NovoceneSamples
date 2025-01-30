using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AURORA.NovoceneSamples.UnderwaterScene
{
#if UNITY_EDITOR
    [ExecuteInEditMode]
#endif
    public class PlaneGenerator : MonoBehaviour
    {
        [Header("Mesh")]
        [SerializeField] private MeshFilter meshFilter;
        [SerializeField] private Vector2 size;
        [SerializeField] private int resolution;
        [SerializeField] private string path = "Assets/Models/Water/CustomPlane.asset";
        
        public void GenerateMesh()
        {
            if (meshFilter == null)
                meshFilter = GetComponent<MeshFilter>();
            
            var mesh = new Mesh { name = "Custom Plane Mesh" };

            var numVerticesX = resolution + 1;
            var numVerticesZ = resolution + 1;
            var numVertices = numVerticesX * numVerticesZ;

            var vertices = new Vector3[numVertices];
            var uv = new Vector2[numVertices];
            var triangles = new int[6 * resolution * resolution];

            var stepX = size.x / resolution;
            var stepZ = size.y / resolution;

            for (int z = 0, i = 0; z < numVerticesZ; z++)
            {
                for (var x = 0; x < numVerticesX; x++, i++)
                {
                    vertices[i] = new(x * stepX - size.x * 0.5f, 0, z * stepZ - size.y * 0.5f);
                    uv[i] = new((float)x / resolution, (float)z / resolution);
                }
            }

            int triangleIndex = 0;
            for (int z = 0, i = 0; z < resolution; z++, i++)
            {
                for (int x = 0; x < resolution; x++, i++)
                {
                    triangles[triangleIndex++] = i;
                    triangles[triangleIndex++] = i + numVerticesX;
                    triangles[triangleIndex++] = i + 1;

                    triangles[triangleIndex++] = i + 1;
                    triangles[triangleIndex++] = i + numVerticesX;
                    triangles[triangleIndex++] = i + numVerticesX + 1;
                }
            }

            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            
            meshFilter.mesh = mesh;
            
#if UNITY_EDITOR
            AssetDatabase.CreateAsset(mesh, path);
            AssetDatabase.SaveAssets();
#endif
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(PlaneGenerator))]
    public class PlaneGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var planeGenerator = (PlaneGenerator)target;
            if (GUILayout.Button("Generate Plane"))
                planeGenerator.GenerateMesh();
        }
    }
#endif
}
