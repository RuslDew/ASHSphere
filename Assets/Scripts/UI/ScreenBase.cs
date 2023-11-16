using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class ScreenBase : MonoBehaviour
{
    [SerializeField] private List<MoveFromScreenAnimation> _elementsAnimation = new List<MoveFromScreenAnimation>();

    private Sequence _hideSequence;

    [SerializeField] private float _disableOnHideDelay = 2f;

    private int _finishedElementAnimationsCount = 0;

    private Action _onCompleteShow;


    public virtual void Hide()
    {
        if (_hideSequence != null)
            _hideSequence.Kill();

        foreach (MoveFromScreenAnimation element in _elementsAnimation)
        {
            element.MoveFromScreen();
        }

        _hideSequence = DOTween.Sequence().AppendCallback(() =>
        {
            gameObject.SetActive(false);
        }).SetDelay(_disableOnHideDelay);
    }

    public virtual void Show(Action onComplete = null)
    {
        _finishedElementAnimationsCount = 0;
        _onCompleteShow = onComplete;

        if (_hideSequence != null)
            _hideSequence.Kill();

        gameObject.SetActive(true);

        foreach (MoveFromScreenAnimation element in _elementsAnimation)
        {
            element.MoveToScreen(() => IncreaseFinishedElementAnimationsCount());
        }
    }

    private void IncreaseFinishedElementAnimationsCount()
    {
        _finishedElementAnimationsCount++;

        if (_finishedElementAnimationsCount == _elementsAnimation.Count)
        {
            _onCompleteShow?.Invoke();
            _finishedElementAnimationsCount = 0;
            _onCompleteShow = null;
        }
    }
}
