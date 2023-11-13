using System.Collections.Generic;
using UnityEngine;

public class GroupsController : MonoBehaviour
{
    [SerializeField] private List<PiecesGroup> _groups = new List<PiecesGroup>();

    [SerializeField] private AutoConstructor _autoConstructor = new AutoConstructor();


    private void Awake()
    {
        _autoConstructor.Init(_groups);

        foreach (PiecesGroup group in _groups)
            group.OnCompleteRotation += (oldAngle, newAngle) => PieceCompleteRotationHandler(group, oldAngle, newAngle);
    }

    private void Start()
    {
        _autoConstructor.MixPieces();
    }

    private void PieceCompleteRotationHandler(PiecesGroup group, float oldAngle, float newAngle)
    {
        _autoConstructor.AddActionToHistory(group, oldAngle, newAngle);

        UpdateGroups();
    }

    private void UpdateGroups()
    {
        foreach (PiecesGroup group in _groups)
            group.UpdatePieces();
    }

    public List<PiecesGroup> GetGroupsThatContainingPiece(SpherePiece piece)
    {
        List<PiecesGroup> containingGroups = new List<PiecesGroup>();

        foreach (PiecesGroup group in _groups)
        {
            foreach (SpherePiece groupPiece in group.Pieces)
            {
                if (piece == groupPiece && !containingGroups.Contains(group))
                    containingGroups.Add(group);
            }
        }

        return containingGroups;
    }
}
