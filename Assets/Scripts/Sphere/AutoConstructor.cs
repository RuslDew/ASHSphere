using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

[Serializable]
public class AutoConstructor
{
    private List<PiecesGroup> _groups = new List<PiecesGroup>();
    private List<HistoryAction> _actionsHistory = new List<HistoryAction>();

    [SerializeField] private float _mixDuration = 0.5f;
    [SerializeField] private float _assmebleDuration = 0.5f;


    public void Init(List<PiecesGroup> avaibleGroups)
    {
        _groups.Clear();
        _groups.AddRange(avaibleGroups);
    }

    public void MixPieces()
    {
        SetRandomAngleForGroup(0);
    }

    public void AutoAssemblePieces()
    {
        if (_actionsHistory.Count == 0)
            return;

        RedoHistoryAction(_actionsHistory.Count - 1);
    }

    private void RedoHistoryAction(int actionIndex)
    {
        if (actionIndex < 0 || actionIndex >= _actionsHistory.Count)
            return;

        HistoryAction redoAction = _actionsHistory[actionIndex];
        PiecesGroup redoGroup = redoAction.RotatedGroup;
        float previousAngle = redoAction.PreviousAngle;

        Debug.LogError(previousAngle);

        redoGroup.SetRotation(previousAngle, () =>
        {
            RedoHistoryAction(actionIndex - 1);
        }, _assmebleDuration, snapEase: DG.Tweening.Ease.OutElastic, reparentPieces: true, speedBased: false, writeToHistory: false);
    }

    private void SetRandomAngleForGroup(int groupIndex)
    {
        if (groupIndex < 0 || groupIndex >= _groups.Count)
            return;

        PiecesGroup group = _groups[groupIndex];

        float singleAngleStep = 72f;
        int maxPositionsCount = 5;
        int randomPosition = UnityEngine.Random.Range(0, maxPositionsCount);
        float randomAngle = (float)randomPosition * singleAngleStep;

        group.SetRotation(randomAngle, () =>
        {
            SetRandomAngleForGroup(groupIndex + 1);
        }, _mixDuration, snapEase: DG.Tweening.Ease.OutElastic, reparentPieces: true, speedBased: false);
    }

    public void AddActionToHistory(PiecesGroup group, float previousAngle)
    {
        _actionsHistory.Add(new HistoryAction(group, previousAngle));
    }
}
