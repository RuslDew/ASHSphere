using UnityEngine;
using System;

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
}
