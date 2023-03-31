using UnityEngine;

public class LookAtCameraAlways : MonoBehaviour
{

    void FixedUpdate()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}
