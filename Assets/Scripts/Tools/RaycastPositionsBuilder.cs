using com.cyborgAssets.inspectorButtonPro;
using System.Collections.Generic;
using UnityEngine;

public class RaycastPositionsBuilder : MonoBehaviour
{
    [SerializeField] private List<MeshRenderer> _pieces = new List<MeshRenderer>();
    [SerializeField] private Transform _center;

    private List<Transform> _points = new List<Transform>();

    [SerializeField] private float _distFromCenter;


    [ProButton]
    public void Build()
    {
        Clear();

        Vector3 centerPos = _center.position;

        foreach (MeshRenderer piece in _pieces)
        {
            Vector3 pieceCenter = piece.gameObject.GetComponent<MeshFilter>().sharedMesh.bounds.center;
            Vector3 normalVector = (pieceCenter - centerPos).normalized;

            Transform newPoint = new GameObject().transform;
            newPoint.position = centerPos + (normalVector * _distFromCenter);

            Vector3 pointLookDirection = piece.transform.position - newPoint.position;

            newPoint.forward = pointLookDirection;

            newPoint.gameObject.AddComponent<AxisDrawer>().SetColor(Color.white);

            _points.Add(newPoint);
        }
    }

    [ProButton]
    public void Clear()
    {
        foreach (Transform point in _points)
        {
            DestroyImmediate(point.gameObject);
        }

        _points.Clear();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        foreach (Transform point in _points)
        {
            Gizmos.DrawSphere(point.position, 0.1f);
        }
    }
}
