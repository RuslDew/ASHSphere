using UnityEngine;
using UnityEngine.UI;
using System;

public class MenuScreen : ScreenBase
{
    [SerializeField] private VerticalLayoutGroup _layout;


    public override void Show(Action onComplete = null)
    {
        if (_layout != null)
            _layout.enabled = false;

        base.Show(() =>
        {
            onComplete?.Invoke();

            if (_layout != null)
                _layout.enabled = true;
        });
    }

    public override void Hide(Action onComplete = null)
    {
        if (_layout != null)
            _layout.enabled = false;

        base.Hide(() => onComplete?.Invoke());
    }
}
