// This script is placed in public domain. The author takes no responsibility for any possible harm.

// Moves the object along as far as range units randomly but in a smooth way.
// This script requires the Noise.cs script.
using UnityEngine;
public class SmoothRandomPosition : MonoBehaviour
{
    public float speed = 1.0f;
    public Vector3 range = new Vector3(1.0f, 1.0f, 1.0f);

    private Perlin noise = new Perlin();
    private Vector3 position;

    void Start()
    {
        position = transform.position;
    }

    void Update()
    {
        transform.position = position + Vector3.Scale(SmoothRandom.GetVector3(speed), range);
    }
}