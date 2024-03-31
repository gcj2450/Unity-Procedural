// This script is placed in public domain. The author takes no responsibility for any possible harm.
using UnityEngine;
public class SinusCurveModifier : MonoBehaviour
{
    public float scale = 10.0f;
    public float speed = 1.0f;
    private Vector3[] baseHeight;

    void Update()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        if (baseHeight == null)
            baseHeight = mesh.vertices;

        var vertices = new Vector3[baseHeight.Length];
        for (var i = 0; i < vertices.Length; i++)
        {
            var vertex = baseHeight[i];
            vertex.y += Mathf.Sin(Time.time * speed + baseHeight[i].x + baseHeight[i].y + baseHeight[i].z) * scale;
            vertices[i] = vertex;
        }
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }
}