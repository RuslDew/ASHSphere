using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PiecesGroup : MonoBehaviour
{
    [SerializeField] private RotationAxis _axis;
    [SerializeField] private List<GroupingRay> _rays = new List<GroupingRay>();
    public List<SpherePiece> Pieces { get; private set; } = new List<SpherePiece>();

    [SerializeField] private Transform _defaultPiecesParent;

    public event Action<float, float> OnCompleteRotation;

    private Sequence _rotateSequence;

    public Vector3 Axis => _axis.Axis;


    private void Awake()
    {
        UpdatePieces();
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
        SetParentForPieces(_axis.ObjectsContainer);

        if (_rotateSequence != null)
            _rotateSequence.Kill();

        _rotateSequence = DOTween.Sequence().AppendCallback(() =>
        {
            float rotationSpeed = pointer.CurrentSpeed.x + pointer.CurrentSpeed.y;

            _axis.Rotate(rotationSpeed);
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

    public void SetRotation(float zAngle, Action onComplete, float snapSpeed = 30f, Ease snapEase = Ease.OutElastic, bool reparentPieces = false, bool speedBased = true)
    {
        if (reparentPieces)
            SetParentForPieces(_axis.ObjectsContainer);

        float oldAngle = _axis.CurrentZAngle;

        _axis.SetRotation(zAngle, () =>
        {
            SetParentForPieces(_defaultPiecesParent);

            float newAngle = _axis.CurrentZAngle;

            OnCompleteRotation?.Invoke(oldAngle, newAngle);

            onComplete?.Invoke();
        }, snapSpeed, snapEase, speedBased);
    }

    private void SetParentForPieces(Transform parent)
    {
        foreach (SpherePiece piece in Pieces)
            piece.transform.SetParent(parent, true);
    }
}
