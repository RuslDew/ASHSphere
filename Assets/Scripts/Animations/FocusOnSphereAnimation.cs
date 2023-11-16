using System;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class FocusOnSphereAnimation : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _menuCamera;
    [SerializeField] private CinemachineVirtualCamera _gameCamera;

    [Space]

    [SerializeField] private RotationAnimation _sphereRotationAnim;

    [Space]

    [SerializeField] private Transform _background;
    [SerializeField] private float _backgroundMenuScale;
    [SerializeField] private float _backgroundGameScale;
    [SerializeField] private float _backgroundAnimDuration;
    private Tweener _backgroundScaleTweener;


    public void Focus(Action onComplete = null)
    {
        _menuCamera.enabled = false;
        _gameCamera.enabled = true;

        _sphereRotationAnim.Enable(false);

        SetBackgroundScale(_backgroundGameScale, onComplete);
    }

    public void Unfocus(Action onComplete = null)
    {
        _menuCamera.enabled = true;
        _gameCamera.enabled = false;

        _sphereRotationAnim.Enable(true);

        SetBackgroundScale(_backgroundMenuScale, onComplete);
    }

    private void SetBackgroundScale(float scale, Action onComplete = null)
    {
        Vector3 localScale = Vector3.one * scale;

        if (_backgroundScaleTweener != null)
            _backgroundScaleTweener.Kill();

        _backgroundScaleTweener = _background.DOScale(localScale, _backgroundAnimDuration).OnComplete(() => onComplete?.Invoke());
    }
}
