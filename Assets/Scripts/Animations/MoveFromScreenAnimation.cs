using UnityEngine;
using DG.Tweening;

public class MoveFromScreenAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform _rect;
    [SerializeField] private Vector3 _defaultPos;
    [SerializeField] private Vector3 _hiddenPos;
    [SerializeField] private float _startShowAnimDelay;
    [SerializeField] private float _startHideAnimDelay;
    [SerializeField] private float _animDuration;
    [SerializeField] private Ease _showEase;
    [SerializeField] private Ease _hideEase;

    private Tweener _animTweener;


    public void MoveToScreen()
    {
        if (_animTweener != null)
            _animTweener.Kill();

        _rect.anchoredPosition = _hiddenPos;

        _animTweener = _rect.DOAnchorPos(_defaultPos, _animDuration).SetDelay(_startShowAnimDelay).SetEase(_showEase);
    }

    public void MoveFromScreen()
    {
        if (_animTweener != null)
            _animTweener.Kill();

        _rect.anchoredPosition = _defaultPos;

        _animTweener = _rect.DOAnchorPos(_hiddenPos, _animDuration).SetDelay(_startHideAnimDelay).SetEase(_hideEase);
    }
}
