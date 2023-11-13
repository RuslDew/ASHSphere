using UnityEngine;

public static class Extansions
{
    public static Vector3 GetPerpendicular(this Vector3 original, Vector3 nonCollinearVector)
    {
        Vector3 a = original;
        Vector3 b = nonCollinearVector;

        Vector3 vectorProduct = new Vector3(a.z * b.y - a.y * b.z, a.x * b.z - a.z - b.x, a.y * b.x - a.x * b.y);

        return vectorProduct;
    }

    public static float GetSign(this float number)
    {
        return number / Mathf.Abs(number);
    }
}
