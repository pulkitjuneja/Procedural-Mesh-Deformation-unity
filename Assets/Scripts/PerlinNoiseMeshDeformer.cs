using UnityEngine;


[RequireComponent (typeof (MeshFilter))]
public class PerlinNoiseMeshDeformer : MonoBehaviour {
    Mesh deformingMesh;
    MeshCollider meshCollider;

    Vector3[] originalVertices, displacedVertices, vertexVelocities;

    void Start () {
        deformingMesh = GetComponent<MeshFilter> ().mesh;
        originalVertices = deformingMesh.vertices;
        meshCollider = GetComponent<MeshCollider> ();
        displacedVertices = new Vector3[originalVertices.Length];
        for (int i = 0; i < originalVertices.Length; i++) {
            displacedVertices[i] = originalVertices[i];
        }
        vertexVelocities = new Vector3[originalVertices.Length];
    }

    void Update () {
        for(int i = 0 ; i < displacedVertices.Length; i ++) {
            displacedVertices[i].z = Mathf.PerlinNoise(i,i + Time.timeSinceLevelLoad)*5;
        }
         deformingMesh.vertices = displacedVertices;
        deformingMesh.RecalculateNormals ();
        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = deformingMesh; // do not do this
    }

}