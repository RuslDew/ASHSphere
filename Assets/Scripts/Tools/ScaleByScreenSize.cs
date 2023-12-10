using UnityEngine;

public class ScaleByScreenSize : MonoBehaviour
{
    [SerializeField] private Vector2 _originalScreenSize = new Vector2(1920f, 1080f);

    [SerializeField] private Vector3 _originalScale = Vector3.one;

    [SerializeField] private float _multiplier = 1f;

    public float ScaleMultiplier { get; private set; }


    private void UpdateScale()
    {
        ScaleMultiplier = GetScaleMultiplier();

        transform.localScale = _originalScale * ScaleMultiplier;
    }

    private float GetScaleMultiplier()
    {
        Vector2 currentScreenSize = new Vector2(Screen.width, Screen.height);

        float currentRatio = currentScreenSize.x / currentScreenSize.y;
        float originalRatio = _originalScreenSize.x / _originalScreenSize.y;

        float scale = currentRatio / originalRatio;
        float scaleCorrection = _multiplier * (1f - scale);
        float correctedScale = scale + scaleCorrection;

        return correctedScale;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        UpdateScale();
    }

    private void OnValidate()
    {
        UpdateScale();
    }
#endif

    private void Update()
    {
        UpdateScale();
    }
}
