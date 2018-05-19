using UnityEngine;

[RequireComponent (typeof (MeshFilter))]
public class MeshDeformer : MonoBehaviour {
    Mesh deformingMesh;
    MeshCollider meshCollider;

    float dampeningConstant = 2;
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

    public void AddDeformingForce (Vector3 point, float force) {
        for (int i = 0; i < displacedVertices.Length; i++) {
            AddForceToVertex (i, point, force);
        }
    }

    void Update () {
        for (int i = 0; i < displacedVertices.Length; i++) {
            UpdateVertex (i);
            dampen (i);
        }
        deformingMesh.vertices = displacedVertices;
        deformingMesh.RecalculateNormals ();
        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = deformingMesh; // do not do this
    }

    void dampen (int i) {
        var displacement = displacedVertices[i] - originalVertices[i];
        if (vertexVelocities[i].magnitude > 0) {
            vertexVelocities[i] -= displacement * dampeningConstant * Time.deltaTime;
        }
    }

    void UpdateVertex (int i) {
        Vector3 velocity = vertexVelocities[i];
        displacedVertices[i] += velocity * Time.deltaTime;
    }

    void AddForceToVertex (int i, Vector3 point, float force) {
        Vector3 distance = displacedVertices[i] - point;
        var attenuatedForce = force / (distance.sqrMagnitude + 1);
        var vertexVelocity = attenuatedForce * Time.deltaTime;
        vertexVelocities[i] += distance.normalized * vertexVelocity;
    }

    void OnCollisionEnter (Collision other) {
        foreach (var contact in other.contacts) {
            var point = contact.point;
            var force = other.gameObject.GetComponent<Rigidbody> ().mass * 10; // the hevier the object the more force it would exert s
            AddDeformingForce (point, force);
        }
    }
}