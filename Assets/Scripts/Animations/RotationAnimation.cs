using UnityEngine;

public class RotationAnimation : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _rotationSpeed;

    private bool _isEnabled = true;


    private void FixedUpdate()
    {
        if (_isEnabled)
            _target.rotation = Quaternion.Euler(_target.rotation.eulerAngles + _rotationSpeed);
    }

    public void Enable(bool enable)
    {
        _isEnabled = enable;
    }
}
