using UnityEngine;

public class MinimapFollower : MonoBehaviour
{
    public Transform playerTransform;
    
    void Update()
    {
        this.transform.position = new Vector3(playerTransform.position.x, 500f, this.transform.position.z);
        
        this.transform.LookAt(playerTransform);
    }
}
