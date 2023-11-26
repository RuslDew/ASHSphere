using UnityEngine;
using DG.Tweening;
using System;

public class PieceColorAnimation : MonoBehaviour
{
    [SerializeField] private MeshRenderer _renderer;

    private MaterialPropertyBlock _propertyBlock = null;

    private Sequence _blinkingSequence;

    private bool _isHighlited = false;

    private Color _defaultColor = Color.white;

    private Tweener _setColorTweener;

    private float _lastFrequency;


    private void Awake()
    {
        if (_propertyBlock == null)
        {
            _propertyBlock = new MaterialPropertyBlock();

            _renderer.GetPropertyBlock(_propertyBlock);
        }
    }

    public void StartBlinkingAnimation(Color color, float frequency)
    {
        if (_blinkingSequence != null)
            _blinkingSequence.Kill();

        _lastFrequency = frequency;

        _blinkingSequence = DOTween.Sequence().AppendCallback(() =>
        {
            _isHighlited = !_isHighlited;

            Color currentColor = _isHighlited ? color : _defaultColor;

            SetColor(currentColor, frequency, () =>
            {
                if (_blinkingSequence != null)
                    _blinkingSequence.Restart();
            });
        }).SetAutoKill(false);
    }

    public void StopBlinkingAnimation()
    {
        if (_blinkingSequence != null)
            _blinkingSequence.Kill();

        _isHighlited = false;

        SetColor(_defaultColor, _lastFrequency, null);
    }

    private void SetColor(Color color, float duration, Action onComplete)
    {
        if (_setColorTweener != null)
            _setColorTweener.Kill();

        _setColorTweener = DOTween.To(() => _propertyBlock.GetColor("_Color"), (newColor) =>
        {
            _propertyBlock.SetColor("_Color", newColor);
            _renderer.SetPropertyBlock(_propertyBlock);

        }, color, duration).OnComplete(() => onComplete?.Invoke());      
    }
}
