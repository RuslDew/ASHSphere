using System;
using UnityEngine;

public class EdgeRotationDirectionSelector : MonoBehaviour
{
    [SerializeField] private Pointer _pointer;

    private Vector3 _selectedRotationDirection;

    [SerializeField] private  int _requiredSamplesCount = 5;
    private int _samplesCount = 0;
    private Vector3 _sampledSpeedsSum = Vector3.zero;

    private bool _isDirectionSelected = false;

    public event Action<Vector3, Vector3> OnSelectDirection;

    private bool _clickWasOnSphere = false;


    private void Awake()
    {
        _pointer.OnPointerDown += CheckIfClickedOnSphere;
        _pointer.OnPointerHold += SamplePointerSpeed;
        _pointer.OnPointerUp += (pointerPos) => ResetRotationDirectionSelection();
    }

    private void ResetRotationDirectionSelection()
    {
        _isDirectionSelected = false;
        _sampledSpeedsSum = Vector3.zero;
        _samplesCount = 0;
    }

    private void SamplePointerSpeed(Vector2 pointerSpeed)
    {
        if (_isDirectionSelected)
            return;

        if (pointerSpeed != Vector2.zero)
        {
            Vector2 normalSpeed = pointerSpeed.normalized;
            Vector3 localNormalSpeedVector = new Vector3(normalSpeed.x, normalSpeed.y, 0f);
            Vector3 globalNormalSpeedVector = Camera.main.transform.TransformVector(localNormalSpeedVector);

            _samplesCount++;
            _sampledSpeedsSum += globalNormalSpeedVector;

            if (_samplesCount >= _requiredSamplesCount)
            {
                SelectRotationDirection();
            }
        }
    }

    private void SelectRotationDirection()
    {
        if (!_clickWasOnSphere)
            return;

        _selectedRotationDirection = _sampledSpeedsSum / (float)_samplesCount;
        _isDirectionSelected = true;

        Vector3 perpendicularToDirection = _selectedRotationDirection.GetPerpendicular(Camera.main.transform.forward);

        OnSelectDirection?.Invoke(_selectedRotationDirection, perpendicularToDirection);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 from = Camera.main.transform.position;
        Vector3 to = from + (_selectedRotationDirection * 100f);
        Gizmos.DrawLine(from, to);
        Gizmos.DrawLine(from, to.GetPerpendicular(Camera.main.transform.forward));
    }
#endif

    private void CheckIfClickedOnSphere(Vector2 pointerPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(pointerPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
            _clickWasOnSphere = hit.collider.gameObject.GetComponent<SpherePiece>() != null;
        else
            _clickWasOnSphere = false;
    }
}
