using com.cyborgAssets.inspectorButtonPro;
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
            group.OnCompleteRotation += (angleChange, writeToHistory) => PieceCompleteRotationHandler(group, angleChange, writeToHistory);
    }

    private void PieceCompleteRotationHandler(PiecesGroup group, float angleChange, bool writeToHistory)
    {
        if (writeToHistory)
            _autoConstructor.AddActionToHistory(group, angleChange);

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

    public void Assemble()
    {
        _autoConstructor.AutoAssemblePieces(null);
    }

    public void Mix()
    {
        _autoConstructor.MixPieces(null);
    }
}
