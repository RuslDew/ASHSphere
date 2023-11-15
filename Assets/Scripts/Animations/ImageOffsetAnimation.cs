using UnityEngine;
using UnityEngine.UI;

public class ImageOffsetAnimation : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Vector2 _animationSpeed;

    private Material _material;


    private void Awake()
    {
        _material = _image.material;
    }

    private void FixedUpdate()
    {
        _material.mainTextureOffset += _animationSpeed;
    }
}
