using UnityEngine;

public class SquadIcon : MonoBehaviour
{
    public Transform target; // The unit or squad center to follow
    public Vector3 offset = new Vector3(0, 2.5f, 0); // Height above unit
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Follow position
        transform.position = target.position + offset;

        // Face the camera
        if (cam != null)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
        }
    }
}
