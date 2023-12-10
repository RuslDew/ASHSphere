using UnityEngine;

public static class Extansions
{
    public static float ScalarProduct(this Vector3 vector, Vector3 otherVector)
    {
        return Mathf.Abs((vector.x * otherVector.x) + (vector.y * otherVector.y) + (vector.z * otherVector.z));
    }

    public static float GetSign(this float number)
    {
        return number / Mathf.Abs(number);
    }
}
