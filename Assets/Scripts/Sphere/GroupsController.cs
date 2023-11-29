using System;
using System.Collections.Generic;
using UnityEngine;

public class GroupsController : MonoBehaviour
{
    [SerializeField] private List<PiecesGroup> _groups = new List<PiecesGroup>();

    [SerializeField] private AutoConstructor _autoConstructor = new AutoConstructor();

    [SerializeField] private List<Pointer> _sphereControlPointers = new List<Pointer>();

    [SerializeField] private RotateAnglePanel _rotateAngleVisual;


    private void Awake()
    {
        _autoConstructor.Init(_groups);

        foreach (PiecesGroup group in _groups)
        {
            group.OnCompleteRotation += (angleChange, writeToHistory) => PieceCompleteRotationHandler(group, angleChange, writeToHistory);
            group.OnRotate += (angleChange) => _rotateAngleVisual.Show(angleChange);
        }
    }

    private void PieceCompleteRotationHandler(PiecesGroup group, float angleChange, bool writeToHistory)
    {
        if (writeToHistory)
            _autoConstructor.AddActionToHistory(group, angleChange);

        UpdateGroups();

        _rotateAngleVisual.Hide();
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

    public void Assemble(Action onComplete = null)
    {
        _autoConstructor.AutoAssemblePieces(onComplete);
    }

    public void Mix(Action onComplete)
    {
        _autoConstructor.MixPieces(onComplete);
    }

    public void LoadHistory(string history, Action onComplete)
    {
        List<HistoryAction> historyActions = LoadActionsFromString(history);

        _autoConstructor.LoadHistory(historyActions, onComplete);
    }

    public void AllowActions(bool allow)
    {
        EnablePointers(allow);
    }

    private void EnablePointers(bool enable)
    {
        foreach (Pointer pointer in _sphereControlPointers)
        {
            pointer.Enable(enable);
        }
    }

    public string GetCurrentActionsHistory()
    {
        return _autoConstructor.GetCurrentActionsHistory();
    }

    private List<HistoryAction> LoadActionsFromString(string history)
    {
        return _autoConstructor.LoadActionsFromString(history);
    }
}
