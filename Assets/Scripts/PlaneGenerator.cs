using UnityEngine;

[RequireComponent (typeof (MeshFilter), typeof (MeshRenderer))]
public class PlaneGenerator : MonoBehaviour {
    public int xSize, ySize; // grid size
    private Vector3[] vertices;

    private Mesh mesh;

    private void Awake () {
        Generate ();
        GenerateTriangles ();
        mesh.RecalculateNormals ();
        AddColliders ();
    }

    private void Generate () {
        vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        Vector2[] uv = new Vector2[vertices.Length];
        for (int i = 0, y = 0; y <= ySize; y++) {
            for (int x = 0; x <= xSize; x++, i++) {
                vertices[i] = new Vector3 (x, y);
                uv[i] = new Vector2 ((float) x / xSize, (float) y / ySize);
            }
        }
        GetComponent<MeshFilter> ().mesh = mesh = new Mesh ();
        mesh.name = "Procedural Grid";
        mesh.vertices = vertices;
        mesh.uv = uv;
    }

    void AddColliders () {
        gameObject.AddComponent<MeshCollider> ();
    }

    void GenerateTriangles () {
        int[] triangles = new int[xSize * ySize * 6];
        for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++) {
            for (int x = 0; x < xSize; x++, ti += 6, vi++) {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                triangles[ti + 5] = vi + xSize + 2;

            }
        }
        mesh.triangles = triangles;
    }

    // private void OnDrawGizmos () {
    //     if (vertices == null)
    //         return;
    //     Gizmos.color = Color.black;
    //     for (int i = 0; i < vertices.Length; i++) {
    //         var movedPoint = transform.TransformPoint (vertices[i]);
    //         Gizmos.DrawSphere (movedPoint, 0.1f);
    //         Gizmos.DrawRay(vertices[i], mesh.normals[i]);
    //     }
    // }
}