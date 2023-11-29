using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class ScreenBase : MonoBehaviour
{
    [SerializeField] private List<MoveFromScreenAnimation> _elementsAnimation = new List<MoveFromScreenAnimation>();

    private Sequence _hideSequence;

    [SerializeField] private float _disableOnHideDelay = 2f;

    private int _finishedShowElementAnimationsCount = 0;
    private int _finishedHideElementAnimationsCount = 0;

    private Action _onCompleteShow;
    private Action _onCompleteHide;


    public void Show()
    {
        Show(null);
    }    

    public void Hide()
    {
        Hide(null);
    }

    public virtual void Hide(Action onComplete = null)
    {
        if (!gameObject.activeSelf)
        {
            onComplete?.Invoke();
            return;
        }

        _finishedHideElementAnimationsCount = 0;
        _onCompleteHide = onComplete;

        if (_hideSequence != null)
            _hideSequence.Kill();

        foreach (MoveFromScreenAnimation element in _elementsAnimation)
        {
            element.MoveFromScreen(() => IncreaseFinishedHideElementAnimationsCount());
        }

        _hideSequence = DOTween.Sequence().AppendCallback(() =>
        {
            gameObject.SetActive(false);
        }).SetDelay(_disableOnHideDelay);
    }

    public virtual void Show(Action onComplete = null)
    {
        if (gameObject.activeSelf)
        {
            onComplete?.Invoke();
            return;
        }

        _finishedShowElementAnimationsCount = 0;
        _onCompleteShow = onComplete;

        if (_hideSequence != null)
            _hideSequence.Kill();

        gameObject.SetActive(true);

        foreach (MoveFromScreenAnimation element in _elementsAnimation)
        {
            element.MoveToScreen(() => IncreaseFinishedShowElementAnimationsCount());
        }
    }

    private void IncreaseFinishedShowElementAnimationsCount()
    {
        _finishedShowElementAnimationsCount++;

        if (_finishedShowElementAnimationsCount == _elementsAnimation.Count)
        {
            _onCompleteShow?.Invoke();
            _finishedShowElementAnimationsCount = 0;
            _onCompleteShow = null;
        }
    }

    private void IncreaseFinishedHideElementAnimationsCount()
    {
        _finishedHideElementAnimationsCount++;

        if (_finishedHideElementAnimationsCount == _elementsAnimation.Count)
        {
            _onCompleteHide?.Invoke();
            _finishedHideElementAnimationsCount = 0;
            _onCompleteHide = null;
        }
    }
}
