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

    [SerializeField] private bool _keepX = false;
    [SerializeField] private bool _keepY = false;


    private void CheckToKeepPositions()
    {
        if (_keepX)
        {
            _defaultPos.x = _rect.anchoredPosition.x;
            _hiddenPos.x = _rect.anchoredPosition.x;
        }

        if (_keepY)
        {
            _defaultPos.y = _rect.anchoredPosition.y;
            _hiddenPos.y = _rect.anchoredPosition.y;
        }
    }

    public void MoveToScreen()
    {
        CheckToKeepPositions();

        if (_animTweener != null)
            _animTweener.Kill();

        _rect.anchoredPosition = _hiddenPos;

        _animTweener = _rect.DOAnchorPos(_defaultPos, _animDuration).SetDelay(_startShowAnimDelay).SetEase(_showEase);
    }

    public void MoveFromScreen()
    {
        CheckToKeepPositions();

        if (_animTweener != null)
            _animTweener.Kill();

        _rect.anchoredPosition = _defaultPos;

        _animTweener = _rect.DOAnchorPos(_hiddenPos, _animDuration).SetDelay(_startHideAnimDelay).SetEase(_hideEase);
    }
}
