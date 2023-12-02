using UnityEngine;

[SerializeField]
public enum PieceColor
{
    red,
    blue,
    green,
    cyan,
    pink,
    brown,
    gray,
    purple,
    violet,
    orange,
    dark_green,
    yellow
}

public class SpherePiece : MonoBehaviour
{
    [SerializeField] private PieceColor _color;
    public PieceColor Color => _color;

    [SerializeField] private PieceColorAnimation _animation;

    [SerializeField] private MeshRenderer _renderer;

    public Vector3 Center => _renderer.bounds.center;

    public bool IsBlinking { get; private set; } = false;


    public void StartBlinking(Color color, float frequency)
    {
        _animation.StartBlinkingAnimation(color, frequency);

        IsBlinking = true;
    }

    public void StopBlinking()
    {
        _animation.StopBlinkingAnimation();

        IsBlinking = false;
    }
}
