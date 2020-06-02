using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject ToFollow { get; set; }
    private readonly float cameraMoveSpeed = 5f;

    // Update is called once per frame
    void FixedUpdate() {

        if (ToFollow == null) {
            return;
        }

        Vector3 toFollowPosition = ToFollow.transform.position;
        toFollowPosition.z = transform.position.z;

        Vector3 cameraMoveDir = (toFollowPosition - transform.position).normalized;
        float distance = Vector3.Distance(toFollowPosition, transform.position);

        if (distance > 0) {
            Vector3 newCameraPosition = transform.position + cameraMoveDir * distance * cameraMoveSpeed * Time.fixedDeltaTime;

            float distanceAfterMoving = Vector3.Distance(newCameraPosition, toFollowPosition);
            if (distanceAfterMoving > distance) {
                // we overshot the target
                newCameraPosition = toFollowPosition;
            }

            transform.position = newCameraPosition;
        }

    }
}
