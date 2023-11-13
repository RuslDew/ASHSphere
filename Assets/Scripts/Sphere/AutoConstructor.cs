using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class AutoConstructor
{
    private List<PiecesGroup> _groups = new List<PiecesGroup>();
    private List<HistoryAction> _actionsHistory = new List<HistoryAction>();

    [SerializeField] private float _mixDuration = 0.5f;


    public void Init(List<PiecesGroup> avaibleGroups)
    {
        _groups.Clear();
        _groups.AddRange(avaibleGroups);
    }

    public void MixPieces()
    {
        SetRandomAngleForGroup(0);
    }

    private void SetRandomAngleForGroup(int groupIndex)
    {
        if (groupIndex < 0 || groupIndex > _groups.Count)
            return;

        PiecesGroup group = _groups[groupIndex];

        float singleAngleStep = 72f;
        int maxPositionsCount = 5;
        int randomPosition = UnityEngine.Random.Range(0, maxPositionsCount);
        float randomAngle = (float)randomPosition * singleAngleStep;

        group.SetRotation(randomAngle, () =>
        {
            SetRandomAngleForGroup(groupIndex + 1);
        }, _mixDuration, snapEase: DG.Tweening.Ease.Linear, reparentPieces: true, speedBased: false);
    }

    public void AddActionToHistory(PiecesGroup group, float oldAngle, float newAngle)
    {
        _actionsHistory.Add(new HistoryAction(group, oldAngle, newAngle));
    }
}
