using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScreenBase : MonoBehaviour
{
    [SerializeField] private List<MoveFromScreenAnimation> _elementsAnimation = new List<MoveFromScreenAnimation>();

    private Sequence _hideSequence;

    [SerializeField] private float _disableOnHideDelay = 2f;


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

    public virtual void Show()
    {
        if (_hideSequence != null)
            _hideSequence.Kill();

        gameObject.SetActive(true);

        foreach (MoveFromScreenAnimation element in _elementsAnimation)
        {
            element.MoveToScreen();
        }
    }
}
