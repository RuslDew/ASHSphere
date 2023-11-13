using UnityEngine;

public class GroupingRay : MonoBehaviour
{
    public SpherePiece GetTargetPiece()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        SpherePiece piece = null;

        if (Physics.Raycast(ray, out hit))
        {
            piece = hit.collider.gameObject.GetComponent<SpherePiece>();
        }

        return piece;
    }
}
