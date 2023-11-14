using System;
using UnityEngine;
using UnityEngine.UI;

public class AnimatedButton : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _defaultSprite;
    [SerializeField] private Sprite _clickSprite;

    public event Action OnClick;


    public void ClickHandler()
    {
        OnClick?.Invoke();
    }

    public void PointerDownHandler()
    {
        _image.sprite = _clickSprite;
    }

    public void PointerUpHandler()
    {
        _image.sprite = _defaultSprite;
    }
}
