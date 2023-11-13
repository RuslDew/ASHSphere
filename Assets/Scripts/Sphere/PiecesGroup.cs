using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PiecesGroup : MonoBehaviour
{
    [SerializeField] private RotationAxis _axis;
    [SerializeField] private List<GroupingRay> _rays = new List<GroupingRay>();
    public List<SpherePiece> Pieces { get; private set; } = new List<SpherePiece>();

    [SerializeField] private Transform _defaultPiecesParent;

    public event Action<float, bool> OnCompleteRotation;

    private Sequence _rotateSequence;

    public Vector3 Axis => _axis.Axis;
    public float CurrentZAngle => _axis.CurrentZAngle;

    private float _prevZAngle;


    private void Awake()
    {
        UpdatePieces();

        SetPrevZAngle(CurrentZAngle);
    }

    public void UpdatePieces()
    {
        Pieces.Clear();
        
        foreach (GroupingRay ray in _rays)
        {
            SpherePiece piece = ray.GetTargetPiece();

            if (piece != null)
            {
                Pieces.Add(piece);
            }
        }
    }

    public void StartRotate(Pointer pointer)
    {
        if (_rotateSequence != null)
            _rotateSequence.Kill();

        _rotateSequence = DOTween.Sequence().AppendCallback(() =>
        {
            float rotationSpeed = pointer.CurrentSpeed.x + pointer.CurrentSpeed.y;

            _axis.Rotate(Pieces.Select(piece => piece.transform).ToList(), rotationSpeed);
        }).SetLoops(-1);
    }

    public void StopRotate(Action onCompleteRotation, float snapSpeed, Ease snapEase)
    {
        if (_rotateSequence != null)
            _rotateSequence.Kill();

        float currentZAngle = _axis.CurrentZAngle;
        float singleRotationAngle = 72f;
        float rotationPeriods = Mathf.Round(currentZAngle / singleRotationAngle);
        float snappingZAngle = singleRotationAngle * rotationPeriods;

        SetRotation(snappingZAngle, onCompleteRotation, snapSpeed, snapEase);
    }

    public void SetRotation(float zAngle, Action onComplete, float snapSpeed = 30f, Ease snapEase = Ease.OutElastic, bool reparentPieces = false, bool speedBased = true, bool writeToHistory = true)
    {
        List<Transform> rotatingObjects = Pieces.Select(piece => piece.transform).ToList();

        _axis.SetRotation(rotatingObjects, zAngle, () =>
        {
            StartCoroutine(WaitForFixedUpdateRoutine(() =>
            {
                OnCompleteRotation?.Invoke(_prevZAngle, writeToHistory);

                SetPrevZAngle(zAngle);

                onComplete?.Invoke();
            }));
        }, snapSpeed, snapEase, speedBased);
    }

    private void SetPrevZAngle(float angle)
    {
        _prevZAngle = angle;
    }

    private IEnumerator WaitForFixedUpdateRoutine(Action onComplete)
    {
        yield return new WaitForFixedUpdate();

        onComplete?.Invoke();
    }
}
