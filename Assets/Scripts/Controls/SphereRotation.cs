using UnityEngine;
using System.Collections;
using DG.Tweening;

public class SphereRotation : MonoBehaviour
{
    [SerializeField] private Pointer _pointer;

    private bool _isPointingInVoid = false;

    [Space]

    [SerializeField] private float FixedAngleRotationDuration = 1f;
    [SerializeField] private float ResetRotationDuration = 1f;

    private bool IsRotationAnimationPlaying = false;

    private Coroutine FixedRotationRoutine;

    private Tween ResetRotationTween;

    private const float FixedRotationSingleStepDuration = 0.01f;


    private void Awake()
    {
        _pointer.OnPointerHold += Rotate;
        _pointer.OnPointerDown += PointerDownHandler;
    }

    private void Rotate(Vector2 pointerSpeed)
    {
        if (IsRotationAnimationPlaying || !_isPointingInVoid)
            return;

        transform.Rotate(Camera.main.transform.up, -pointerSpeed.x, Space.World);
        transform.Rotate(Camera.main.transform.right, pointerSpeed.y, Space.World);
    }

    public void ResetRotation()
    {
        if (IsRotationAnimationPlaying)
            return;

        if (ResetRotationTween.IsActive())
            ResetRotationTween.Kill();

        IsRotationAnimationPlaying = true;

        ResetRotationTween = transform.DORotate(Vector3.zero, ResetRotationDuration).SetEase(Ease.Linear).OnComplete(CompleteResetRotationHandler);
    }

    private void CompleteResetRotationHandler()
    {
        IsRotationAnimationPlaying = false;
    }

    public void RotateFixedAngle(Vector3 axis, float angle)
    {
        if (IsRotationAnimationPlaying)
            return;

        IsRotationAnimationPlaying = true;

        int loopsCount = Mathf.RoundToInt(FixedAngleRotationDuration / FixedRotationSingleStepDuration);
        float angleStep = angle / (float)loopsCount;

        if (FixedRotationRoutine != null)
            StopCoroutine(FixedRotationRoutine);

        FixedRotationRoutine = StartCoroutine(LoopRotation(loopsCount, angleStep, axis));
    }

    private IEnumerator LoopRotation(int loops, float angleStep, Vector3 axis)
    {
        for (int i = 0; i < loops; i++)
        {
            transform.Rotate(axis, angleStep, Space.World);

            yield return new WaitForSeconds(FixedRotationSingleStepDuration);
        }

        IsRotationAnimationPlaying = false;
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
