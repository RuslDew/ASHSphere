using System;
using UnityEngine;
using DG.Tweening;

public class Pointer : MonoBehaviour
{
    public event Action<Vector2> OnPointerDown;
    public event Action<Vector2> OnPointerUp;
    public event Action<Vector2> OnPointerHold;

    private Sequence _pointerHoldSequence;
    private Vector2 _prevPointerPos;

    [SerializeField] private float _sensetivity = 1f;

    public Vector2 CurrentSpeed { get; private set; } = Vector2.zero;


    private void Update()
    {
        if (IsPointerDown())
            PointerDownHandler();

        if (IsPointerUp())
            PointerUpHandler();
    }

    private bool IsPointerDown()
    {
        return Input.GetMouseButtonDown(0);
    }
    private bool IsPointerUp()
    {
        return Input.GetMouseButtonUp(0);
    }
    
    private void PointerDownHandler()
    {
        OnPointerDown?.Invoke(GetCurrentPointerPos());

        StartPointerHold();
    }

    private void PointerUpHandler()
    {
        StopPointerHold();

        OnPointerUp?.Invoke(GetCurrentPointerPos());
    }

    private void StartPointerHold()
    {
        StopPointerHold();

        SetPrevPointerPos(GetCurrentPointerPos());

        _pointerHoldSequence = DOTween.Sequence().AppendCallback(() =>
        {
            Vector2 prevPos = GetPrevPointerPos();
            Vector2 currentPos = GetCurrentPointerPos();
            Vector2 speed = currentPos - prevPos;

            Vector2 currentSpeed = speed * GetSensetivity();
            SetCurrentSpeed(currentSpeed);
            OnPointerHold?.Invoke(currentSpeed);

            SetPrevPointerPos(currentPos);
        }).SetLoops(-1).SetDelay(0.01f);
    }

    private Vector2 GetPrevPointerPos()
    {
        return _prevPointerPos;
    }

    public Vector2 GetCurrentPointerPos()
    {
        return Input.mousePosition;
    }

    private void SetPrevPointerPos(Vector2 pos)
    {
        _prevPointerPos = pos;
    }

    private float GetSensetivity()
    {
        return _sensetivity;
    }

    private void StopPointerHold()
    {
        if (_pointerHoldSequence != null)
            _pointerHoldSequence.Kill();

        CurrentSpeed = Vector2.zero;
    }

    private void SetCurrentSpeed(Vector2 speed)
    {
        CurrentSpeed = speed;
    }

    public void Enable(bool enable)
    {
        this.enabled = enable;

        StopPointerHold();
    }
}
