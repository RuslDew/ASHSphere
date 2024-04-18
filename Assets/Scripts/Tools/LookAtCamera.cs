using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform MainCamera;


    private void Start()
    {
        MainCamera = Camera.main.transform;
    }

    private void FixedUpdate()
    {
        transform.LookAt(MainCamera);
    }
}
