using UnityEngine;

public class ExtrudedMeshTrail : MonoBehaviour
{
    public float time = 2.0f;
    public bool autoCalculateOrientation = true;
    public float minDistance = 0.1f;
    public bool invertFaces = false;

    private Mesh srcMesh;
    private MeshExtrusion.Edge[] precomputedEdges;

    private class ExtrudedTrailSection
    {
        public Vector3 point;
        public Matrix4x4 matrix;
        public float time;
    }

    private System.Collections.Generic.List<ExtrudedTrailSection> sections = new System.Collections.Generic.List<ExtrudedTrailSection>();

    private void Start()
    {
        srcMesh = GetComponent<MeshFilter>().sharedMesh;
        precomputedEdges = MeshExtrusion.BuildManifoldEdges(srcMesh);
    }

    private void LateUpdate()
    {
        Vector3 position = transform.position;
        float now = Time.time;

        // Remove old sections
        while (sections.Count > 0 && now > sections[sections.Count - 1].time + time)
        {
            sections.RemoveAt(sections.Count - 1);
        }

        // Add a new trail section to the beginning of the list
        if (sections.Count == 0 || (sections[0].point - position).sqrMagnitude > minDistance * minDistance)
        {
            ExtrudedTrailSection section = new ExtrudedTrailSection();
            section.point = position;
            section.matrix = transform.localToWorldMatrix;
            section.time = now;
            sections.Insert(0, section);
        }

        // We need at least 2 sections to create the line
        if (sections.Count < 2)
            return;

        Matrix4x4 worldToLocal = transform.worldToLocalMatrix;
        Matrix4x4[] finalSections = new Matrix4x4[sections.Count];
        Quaternion previousRotation = Quaternion.identity;
        for (int i = 0; i < sections.Count; i++)
        {
            if (autoCalculateOrientation)
            {
                if (i == 0)
                {
                    Vector3 direction = sections[0].point - sections[1].point;
                    Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
                    previousRotation = rotation;
                    finalSections[i] = worldToLocal * Matrix4x4.TRS(position, rotation, Vector3.one);
                }
                else if (i != sections.Count - 1)
                {
                    Vector3 direction = sections[i].point - sections[i + 1].point;
                    Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);

                    if (Quaternion.Angle(previousRotation, rotation) > 20)
                        rotation = Quaternion.Slerp(previousRotation, rotation, 0.5f);

                    previousRotation = rotation;
                    finalSections[i] = worldToLocal * Matrix4x4.TRS(sections[i].point, rotation, Vector3.one);
                }
                else
                {
                    finalSections[i] = finalSections[i - 1];
                }
            }
            else
            {
                if (i == 0)
                {
                    finalSections[i] = Matrix4x4.identity;
                }
                else
                {
                    finalSections[i] = worldToLocal * sections[i].matrix;
                }
            }
        }

        // Rebuild the extrusion mesh
        MeshExtrusion.ExtrudeMesh(srcMesh, GetComponent<MeshFilter>().mesh, finalSections, precomputedEdges, invertFaces);
    }
}