using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using DG.Tweening;

public class EdgeRotation : MonoBehaviour
{
    [SerializeField] private EdgeRotationDirectionSelector _directionSelector;
    [SerializeField] private Pointer _pointer;
    [SerializeField] private GroupsController _groupsController;

    [Space]

    [SerializeField] private float _rotationSnapSpeed;
    [SerializeField] private DG.Tweening.Ease _rotationSnapEase;

    private PiecesGroup _currentRotatingGroup;

    private bool _isCurrentlyRotating = false;
    private bool _isStoppingRotation = false;

    [Space]

    [SerializeField] private float _clearTriedGroupsDelay = 2f;
    private Sequence _clearTriedGroupsSequence;


    private void Awake()
    {
        _directionSelector.OnSelectDirection += SelectDirectionHandler;
        _pointer.OnPointerUp += (pointerPos) => StopRotation();
    }

    private void SelectDirectionHandler(Vector3 pointerDirection, Vector3 pointerDirectionPerpendicular)
    {
        if (_isCurrentlyRotating || _isStoppingRotation)
            return;

        Vector2 pointerPos = _pointer.GetCurrentPointerPos();
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(pointerPos.x, pointerPos.y, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            SpherePiece piece = hit.collider.gameObject.GetComponent<SpherePiece>();

            if (piece != null)
            {
                if (_clearTriedGroupsSequence != null)
                    _clearTriedGroupsSequence.Kill();

                List<PiecesGroup> pieceGroups = _groupsController.GetGroupsThatContainingPiece(piece);

                _currentRotatingGroup = GetCorrectGroup(pieceGroups, pointerDirection, pointerDirectionPerpendicular);

                if (_currentRotatingGroup != null)
                {
                    _currentRotatingGroup.StartRotate(_pointer);

                    SetCurrentlyRotating(true);
                }
            }
        }
    }

    private PiecesGroup GetCorrectGroup(List<PiecesGroup> avaibleGroups, Vector3 pointerDirection, Vector3 pointerDirectionPerpendicular)
    {
        if (avaibleGroups.Count == 0)
            return null;

        Comparison<PiecesGroup> comparison = (a, b) =>
        {
            float aScalarToDir = a.Axis.ScalarProduct(pointerDirection);
            float bScalarToDir = b.Axis.ScalarProduct(pointerDirection);

            float aScalarToDirPerp = a.Axis.ScalarProduct(pointerDirectionPerpendicular);
            float bScalarToDirPerp = b.Axis.ScalarProduct(pointerDirectionPerpendicular);

            float aScalar = aScalarToDir + aScalarToDirPerp;
            float bScalar = bScalarToDir + bScalarToDirPerp;

            if (bScalar < aScalar)
            {
                return 1;
            }
            else if (bScalar > aScalar)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        };

        avaibleGroups.Sort(comparison);

        if (avaibleGroups != null && avaibleGroups.Count > 0)
        {
            PiecesGroup correctGroup = avaibleGroups[0];
            return correctGroup;
        }

        return null;
    }

    private void StopRotation()
    {
        if (_isCurrentlyRotating && !_isStoppingRotation)
        {
            _isStoppingRotation = true;

            _currentRotatingGroup.StopRotate(() =>
            {
                SetCurrentlyRotating(false);
                CompleteStoppingRotation();
            }, _rotationSnapSpeed, _rotationSnapEase);
        }
    }

    private void CompleteStoppingRotation()
    {
        _isStoppingRotation = false;
    }

    private void SetCurrentlyRotating(bool isRotating)
    {
        _isCurrentlyRotating = isRotating;
    }
}
