using UnityEngine;

public class AxisDrawer : MonoBehaviour
{
    [SerializeField] private Color _color;
    [SerializeField] private float _size = 10f;


    private void OnDrawGizmos()
    {
        Gizmos.color = _color;

        Vector3 from = transform.position - (transform.forward * _size);
        Vector3 to = transform.position + (transform.forward * _size);

        Gizmos.DrawLine(from, to);
        Gizmos.DrawSphere(transform.position, 0.1f);
    }

    public void SetColor(Color color)
    {
        _color = color;
    }
}
