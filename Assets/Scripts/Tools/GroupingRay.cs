using UnityEngine;

public class GroupingRay : MonoBehaviour
{
    public SpherePiece TargetPiece { get; private set; }

    public void UpdateTargetPiece()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        SpherePiece piece = null;

        if (Physics.Raycast(ray, out hit))
        {
            piece = hit.collider.gameObject.GetComponent<SpherePiece>();
        }
        else
        {
            Debug.LogError("Target not found");
        }

        TargetPiece = piece;
    }

    public void SetColor(Color color)
    {
        gameObject.GetComponent<AxisDrawer>().SetColor(color);
    }
}
