using UnityEngine;

public class SphereRotation : MonoBehaviour
{
    [SerializeField] private Pointer _pointer;

    private bool _isPointingInVoid = false;


    private void Awake()
    {
        _pointer.OnPointerHold += Rotate;
        _pointer.OnPointerDown += PointerDownHandler;
    }

    private void Rotate(Vector2 pointerSpeed)
    {
        if (!_isPointingInVoid)
            return;

        transform.RotateAround(Camera.main.transform.up, -pointerSpeed.x);
        transform.RotateAround(Camera.main.transform.right, pointerSpeed.y);
    }

    private void PointerDownHandler(Vector2 pointerPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(pointerPos.x, pointerPos.y, 0f));

        if (Physics.Raycast(ray))
            _isPointingInVoid = false;
        else
            _isPointingInVoid = true;
    }
}
