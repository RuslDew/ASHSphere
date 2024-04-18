using UnityEngine;
using UnityEngine.UI;

public class FixedAngleRotationButton : MonoBehaviour
{
    [SerializeField] private SphereRotation SphereRotationControl;
    [SerializeField] private Button ClickButton;

    [Space]

    [SerializeField] private Vector3 RotationAxis;
    [SerializeField] private float Angle;


    private void Start()
    {
        ClickButton.onClick.AddListener(() => SphereRotationControl.RotateFixedAngle(RotationAxis, Angle));
    }
}
