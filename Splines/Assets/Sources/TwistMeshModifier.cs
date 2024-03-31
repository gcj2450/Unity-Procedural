// This script is placed in public domain. The author takes no responsibility for any possible harm.
using UnityEngine;

public class TwistMeshModifier : MonoBehaviour
{
    // twist a mesh by this amount
    float twist = 1.0f;

    float inputSensitivity = 1.5f;

    private Vector3[] baseVertices;
    private Vector3[] baseNormals;

    void Update()
    {
        twist += Input.GetAxis("Horizontal") * inputSensitivity * Time.deltaTime;

        Mesh mesh = GetComponent<MeshFilter>().mesh;

        if (baseVertices == null)
            baseVertices = mesh.vertices;
        if (baseNormals == null)
            baseNormals = mesh.normals;

        var vertices = new Vector3[baseVertices.Length];
        var normals = new Vector3[baseVertices.Length];

        for (var i = 0; i < vertices.Length; i++)
        {
            vertices[i] = DoTwist(baseVertices[i], baseVertices[i].y * twist);
            normals[i] = DoTwist(baseNormals[i], baseVertices[i].y * twist);
        }

        mesh.vertices = vertices;
        mesh.normals = vertices;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    Vector3 DoTwist(Vector3 pos, float t)
    {
        var st = Mathf.Sin(t);
        var ct = Mathf.Cos(t);
        Vector3 new_pos = Vector3.zero;

        new_pos.x = pos.x * ct - pos.z * st;
        new_pos.z = pos.x * st + pos.z * ct;
        new_pos.y = pos.y;

        return new_pos;
    }

}