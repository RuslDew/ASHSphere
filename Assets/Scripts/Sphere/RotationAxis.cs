using UnityEngine;
using System;
using System.Collections.Generic;
using DG.Tweening;

public class RotationAxis : MonoBehaviour
{
    public Vector3 Axis => GetCorrectAxis();

    private Tweener _setRotationTweener;

    [SerializeField] private Transform _staticPart;
    [SerializeField] private Transform _rotatingPart;

    [SerializeField] private bool _debug = false;

    public float CurrentZAngle => GetCurrentZAngle();

    public Transform ObjectsContainer => _rotatingPart;


    private Vector3 GetCorrectAxis()
    {
        return _staticPart.forward;
    }

    public void SetRotation(List<Transform> rotatingObjects, float zAngle, Action onComplete, float snapSpeed = 30f, Ease snapEase = Ease.OutBounce, bool speedBased = true)
    {
        if (_setRotationTweener != null)
            _setRotationTweener.Kill();

        _setRotationTweener = DOTween.To(() => CurrentZAngle, (newAngle) => SetRotation(rotatingObjects, newAngle), zAngle, snapSpeed)
            .SetSpeedBased(speedBased)
            .SetEase(snapEase)
            .OnComplete(() => onComplete?.Invoke());
    }

    private void SetRotation(List<Transform> rotatingObjects, float zAngle)
    {
        float diff = zAngle - CurrentZAngle;

        _rotatingPart.Rotate(Axis, diff, Space.World);

        foreach (Transform rotatigObject in rotatingObjects)
            rotatigObject.Rotate(Axis, diff, Space.World);
    }

    public void Rotate(List<Transform> rotatingObjects, float angle)
    {
        float axisSum = Axis.x + Axis.y + Axis.z;
        float sign = -axisSum.GetSign();

        _rotatingPart.Rotate(Axis, angle * sign, Space.World);

        foreach (Transform rotatigObject in rotatingObjects)
            rotatigObject.Rotate(Axis, angle * sign, Space.World);
    }

    private float GetCurrentZAngle()
    {
        Vector3 originVector = _staticPart.up;
        Vector3 currentVector = _rotatingPart.up;

        float signedAngle = Vector3.SignedAngle(originVector, currentVector, Axis);
        float angle360 = signedAngle < 0f ? signedAngle + 360f : signedAngle;

        return angle360;
    }

    private void OnDrawGizmos()
    {
        if (!_debug)
            return;

        Vector3 originVector = _staticPart.up;
        Vector3 currentVector = _rotatingPart.up;

        Gizmos.color = Color.white;

        Gizmos.DrawLine(Vector3.zero, originVector * 100f);
        Gizmos.DrawLine(Vector3.zero, currentVector * 100f);
    }
}
