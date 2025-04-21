using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;

    private void Start()
    {

    }

    private void Update()
    {
        // if target is not null
        if (target != null)
        {
            // set camera position to target position
            transform.position = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
        }
    }
}
