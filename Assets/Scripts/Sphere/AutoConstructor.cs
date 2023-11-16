using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class AutoConstructor
{
    private List<PiecesGroup> _groups = new List<PiecesGroup>();
    private List<HistoryAction> _actionsHistory = new List<HistoryAction>();

    [SerializeField] private float _mixDuration = 0.5f;
    [SerializeField] private float _assembleDuration = 0.5f;


    public void Init(List<PiecesGroup> avaibleGroups)
    {
        _groups.Clear();
        _groups.AddRange(avaibleGroups);
    }

    public void AutoAssemblePieces(Action onComplete)
    {
        if (_actionsHistory.Count == 0)
        {
            onComplete?.Invoke();
            return;
        }

        RedoHistoryAction(_actionsHistory.Count - 1, () =>
        {
            _actionsHistory.Clear();
            onComplete?.Invoke();
        });
    }

    private void RedoHistoryAction(int actionIndex, Action onRedoAllActions)
    {
        if (actionIndex < 0 || actionIndex >= _actionsHistory.Count)
        {
            onRedoAllActions?.Invoke();
            return;
        }

        HistoryAction redoAction = _actionsHistory[actionIndex];
        PiecesGroup redoGroup = redoAction.RotatedGroup;
        float angleChange = redoAction.AngleChange;
        float angle = redoGroup.CurrentZAngle - angleChange;

        redoGroup.SetRotation(angle, () =>
        {
            RedoHistoryAction(actionIndex - 1, onRedoAllActions);
        }, _assembleDuration, snapEase: DG.Tweening.Ease.OutElastic, reparentPieces: true, speedBased: false, writeToHistory: false);
    }

    public void MixPieces(Action onComplete)
    {
        SetRandomAngleForGroup(0, onComplete);
    }

    private void SetRandomAngleForGroup(int groupIndex, Action onSetAllGroups)
    {
        if (groupIndex < 0 || groupIndex >= _groups.Count)
        {
            onSetAllGroups?.Invoke();
            return;
        }

        PiecesGroup group = _groups[groupIndex];

        float singleAngleStep = 72f;
        int maxPositionsCount = 5;
        int randomPosition = UnityEngine.Random.Range(0, maxPositionsCount);
        float randomAngle = (float)randomPosition * singleAngleStep;

        group.SetRotation(randomAngle, () =>
        {
            SetRandomAngleForGroup(groupIndex + 1, onSetAllGroups);
        }, _mixDuration, snapEase: DG.Tweening.Ease.OutElastic, reparentPieces: true, speedBased: false);
    }

    public void AddActionToHistory(PiecesGroup group, float angleChange)
    {
        if (angleChange == 0)
            return;

        if (_actionsHistory.Count > 1)
        {
            HistoryAction previousAction = _actionsHistory[_actionsHistory.Count - 1];

            bool isSameGroupActions = previousAction.RotatedGroup == group;

            if (isSameGroupActions)
            {
                float previousActionAngleChange = previousAction.AngleChange;
                bool isRedoAction = previousActionAngleChange == -angleChange;

                if (isRedoAction)
                {
                    _actionsHistory.Remove(previousAction);

                    return;
                }

                bool isPartiallyRedoAction = previousActionAngleChange.GetSign() != angleChange.GetSign() && Mathf.Abs(angleChange) < Mathf.Abs(previousActionAngleChange);

                if (isPartiallyRedoAction)
                {
                    _actionsHistory.Remove(previousAction);

                    _actionsHistory.Add(new HistoryAction(group, previousActionAngleChange + angleChange));

                    return;
                }
            }
        }

        _actionsHistory.Add(new HistoryAction(group, angleChange));
    }
}
