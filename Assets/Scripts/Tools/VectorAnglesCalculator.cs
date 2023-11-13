using UnityEngine;

public class VectorAnglesCalculator
{
    public float GetAngleBetweenVectors(Vector3 a, Vector3 b)
    {
        b = b - a;
        float scalarProduct = (a.x * b.x) + (a.y * b.y) + (a.z * b.z);
        float cameraModule = Mathf.Sqrt((a.x * a.x) + (a.y * a.y) + (a.z * a.z));
        float axisModule = Mathf.Sqrt((b.x * b.x) + (b.y * b.y) + (b.z * b.z));
        float cos = scalarProduct / (cameraModule * axisModule);

        float angle = Mathf.Acos(cos) * Mathf.Rad2Deg;

        return angle;
    }
}
