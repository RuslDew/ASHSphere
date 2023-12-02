using UnityEngine;

public class RendererTextureOffsetAnimation : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Vector2 _animationSpeed;
    [SerializeField] private Vector2 _tilling = new Vector2(1f, 1f);

    private MaterialPropertyBlock _propertyBlock = null;

    private string _tillingPropertyName;

    private Vector4 _texture_ST;


    private void Awake()
    {
        if (_propertyBlock == null)
        {
            _propertyBlock = new MaterialPropertyBlock();

            _renderer.GetPropertyBlock(_propertyBlock);

            _texture_ST = _propertyBlock.GetVector(_tillingPropertyName);
            _texture_ST.x = _tilling.x;
            _texture_ST.y = _tilling.y;
        }

        _tillingPropertyName = $"_MainTex_ST";
    }

    private void FixedUpdate()
    {
        _texture_ST.z += _animationSpeed.x;
        _texture_ST.w += _animationSpeed.y;

        _propertyBlock.SetVector(_tillingPropertyName, _texture_ST);
        _renderer.SetPropertyBlock(_propertyBlock);
    }
}
