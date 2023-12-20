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
    public event Action<float> OnRotate;

    private Sequence _rotateSequence;

    public Vector3 Axis => _axis.Axis;
    public float CurrentZAngle => _axis.CurrentZAngle;

    [Space]

    [SerializeField] private int _groupId;
    public int GroupId => _groupId;

    [Space]

    [SerializeField] private Color _highlightPiecesColor = Color.red;
    [SerializeField] private float _blinkingFrequence = 1f;

    [SerializeField] private float _offsetForHint = 0f;
    public float OffsetForHint => _offsetForHint;

    private List<SpherePiece> _highlightedPieces = new List<SpherePiece>();

    [SerializeField] private float _minAngleStep = 72f;
    public float MinAngleStep => _minAngleStep;


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
        if (_rotateSequence != null)
            _rotateSequence.Kill();

        EnableHighlight();

        _rotateSequence = DOTween.Sequence().AppendCallback(() =>
        {
            float rotationSpeed = pointer.CurrentSpeed.x + pointer.CurrentSpeed.y;

            _axis.Rotate(Pieces.Select(piece => piece.transform).ToList(), rotationSpeed);

            float angleChange = CurrentZAngle;

            OnRotate?.Invoke(angleChange);
        }).SetLoops(-1);
    }

    public void StopRotate(Action onCompleteRotation, float snapSpeed, Ease snapEase)
    {
        if (_rotateSequence != null)
            _rotateSequence.Kill();

        DisableHighlight();

        float rotationPeriods = Mathf.Round(CurrentZAngle / _minAngleStep);
        float snappingZAngle = _minAngleStep * rotationPeriods;

        SetRotation(snappingZAngle, onCompleteRotation, snapSpeed, snapEase);
    }

    public void EnableHighlight()
    {
        _highlightedPieces.AddRange(Pieces.Where(piece => !piece.IsBlinking));

        foreach (SpherePiece piece in _highlightedPieces)
            piece.StartBlinking(_highlightPiecesColor, _blinkingFrequence);
    }

    public void DisableHighlight()
    {
        foreach (SpherePiece piece in _highlightedPieces)
            piece.StopBlinking();

        _highlightedPieces.Clear();
    }


    public void SetRotation(float zAngle, Action onComplete, float snapSpeed = 30f, Ease snapEase = Ease.OutElastic, bool reparentPieces = false, bool speedBased = true, bool writeToHistory = true)
    {
        List<Transform> rotatingObjects = Pieces.Select(piece => piece.transform).ToList();

        _axis.SetRotation(rotatingObjects, zAngle, (angleChange) =>
        {
            StartCoroutine(WaitForFixedUpdateRoutine(() =>
            {
                OnCompleteRotation?.Invoke(angleChange, writeToHistory);

                onComplete?.Invoke();
            }));
        }, snapSpeed, snapEase, speedBased);
    }

    private IEnumerator WaitForFixedUpdateRoutine(Action onComplete)
    {
        yield return new WaitForFixedUpdate();

        onComplete?.Invoke();
    }

    public Vector3 GetActualCenterPosition()
    {
        Vector3 positionsSum = Vector3.zero;

        foreach (SpherePiece piece in Pieces)
        {
            positionsSum += piece.Center;
        }

        Vector3 averagePosition = positionsSum / Pieces.Count;

        return averagePosition;
    }
}
