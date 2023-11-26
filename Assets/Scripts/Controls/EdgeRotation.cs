using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

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

    private SpherePiece _prevPiece;
    private List<PiecesGroup> _prevPieceTriedGroups = new List<PiecesGroup>();


    private void Awake()
    {
        _directionSelector.OnSelectDirection += SelectDirectionHandler;
        _pointer.OnPointerUp += (pointerPos) => StopRotation();
    }

    private void SelectDirectionHandler(Vector3 pointerDirection, Vector3 pointerDirectionPerpendicular)
    {
        if (_isCurrentlyRotating)
            return;

        Vector2 pointerPos = _pointer.GetCurrentPointerPos();
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(pointerPos.x, pointerPos.y, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            SpherePiece piece = hit.collider.gameObject.GetComponent<SpherePiece>();

            if (piece != null)
            {
                List<PiecesGroup> pieceGroups = _groupsController.GetGroupsThatContainingPiece(piece);

                if (_currentRotatingGroup != null)
                {
                    if (_prevPiece == piece && _prevPieceTriedGroups.Count < pieceGroups.Count - 1)
                        _prevPieceTriedGroups.Add(_currentRotatingGroup);
                    else
                        _prevPieceTriedGroups.Clear();
                }

                _prevPiece = piece;

                _currentRotatingGroup = GetCorrectGroup(pieceGroups, _prevPieceTriedGroups, pointerDirection, pointerDirectionPerpendicular);

                if (_currentRotatingGroup != null)
                {
                    _currentRotatingGroup.StartRotate(_pointer);

                    SetCurrentlyRotating(true);
                }
            }
        }
    }

    private PiecesGroup GetCorrectGroup(List<PiecesGroup> avaibleGroups, List<PiecesGroup> alreadyTriedGroups, Vector3 pointerDirection, Vector3 pointerDirectionPerpendicular)
    {
        if (avaibleGroups.Count == 0)
            return null;

        Comparison<PiecesGroup> comparison = (a, b) =>
        {
            float angleBetweenAxisAndDirA = Vector3.Angle(pointerDirection, a.Axis);
            float angleBetweenAxisAndPerpendicularA = Vector3.Angle(pointerDirectionPerpendicular, a.Axis);

            float angleBetweenAxisAndDirB = Vector3.Angle(pointerDirection, b.Axis);
            float angleBetweenAxisAndPerpendicularB = Vector3.Angle(pointerDirectionPerpendicular, b.Axis);

            float diffTo90ForPointerDirA = 90f - angleBetweenAxisAndDirA; 
            float diffTo90ForDirPerpendicularA = 90f - angleBetweenAxisAndPerpendicularA; 
            
            float diffTo90ForPointerDirB = 90f - angleBetweenAxisAndDirB; 
            float diffTo90ForDirPerpendicularB = 90f - angleBetweenAxisAndPerpendicularB; 

            if (diffTo90ForPointerDirA < diffTo90ForPointerDirB && diffTo90ForDirPerpendicularA < diffTo90ForDirPerpendicularB)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        };

        avaibleGroups.Sort(comparison);

        List<PiecesGroup> notTriedGroups = avaibleGroups.Where(group => !alreadyTriedGroups.Contains(group)).ToList();

        if (notTriedGroups != null && notTriedGroups.Count > 0)
        {
            PiecesGroup correctGroup = notTriedGroups[0];
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
