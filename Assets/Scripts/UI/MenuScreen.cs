using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class MenuScreen : MonoBehaviour
{
    [SerializeField] private Animation _buttonsAnimation;
    [SerializeField] private string _buttonsShowAnimationName;
    [SerializeField] private string _buttonsHideAnimationName;

    [Space]

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


    public void Hide()
    {
        _buttonsAnimation.Play(_buttonsHideAnimationName);

        _menuCamera.enabled = false;
        _gameCamera.enabled = true;

        _sphereRotationAnim.Enable(false);

        SetBackgroundScale(_backgroundGameScale);
    }

    private void SetBackgroundScale(float scale)
    {
        Vector3 localScale = Vector3.one * scale;

        if (_backgroundScaleTweener != null)
            _backgroundScaleTweener.Kill();

        _backgroundScaleTweener = _background.DOScale(localScale, _backgroundAnimDuration);
    }
}
