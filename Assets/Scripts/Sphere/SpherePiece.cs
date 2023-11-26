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


    public void StartBlinking(Color color, float frequency)
    {
        _animation.StartBlinkingAnimation(color, frequency);
    }

    public void StopBlinking()
    {
        _animation.StopBlinkingAnimation();
    }
}
