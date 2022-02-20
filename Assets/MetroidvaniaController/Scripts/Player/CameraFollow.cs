using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float FollowSpeed = 2f;
    public Transform Target;

    public BoxCollider2D TargetRoomBounds;

    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    private Transform camTransform;

    // How long the object should shake for.
    public float shakeDuration = 0f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.1f;
    public float decreaseFactor = 1.0f;

    Vector3 originalPos;

    void Awake()
    {
        Cursor.visible = false;
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    private void Update()
    {
        SetCameraPosition();

        if (shakeDuration > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
    }

    private void SetCameraPosition() {
        Vector3 targetPosition = Target.position;
        targetPosition.z = -10;
        Vector3 newPosition = Vector3.Slerp(transform.position, targetPosition, FollowSpeed * Time.deltaTime);
        float cameraHalfHeight = Camera.main.orthographicSize;
        float cameraHalfWidth = Camera.main.aspect * cameraHalfHeight;

        float cameraHorizontalMin = 0;
        float cameraHorizontalMax =  cameraHalfWidth * 2;
        float cameraVerticalMin = 0;
        float cameraVerticalMax = cameraHalfHeight * 2;

        float boundHalfHeight = TargetRoomBounds.bounds.extents.y;
        float boundHalfWidth =  TargetRoomBounds.bounds.extents.x;

        float boundHorizontalMin = -boundHalfWidth;
        float boundHorizontalMax =  boundHalfWidth;
        float boundVerticalMin = -boundHalfHeight;
        float boundVerticalMax = boundHalfHeight; 

        if(newPosition.x + cameraHorizontalMax > TargetRoomBounds.transform.position.x + boundHorizontalMax) {
            newPosition.x = TargetRoomBounds.transform.position.x + boundHorizontalMax - cameraHorizontalMax;
        }
        if(newPosition.x + cameraHorizontalMin < TargetRoomBounds.transform.position.x + boundHorizontalMin) {
            newPosition.x = TargetRoomBounds.transform.position.x + boundHorizontalMin - cameraHorizontalMin;
        }

        transform.position = newPosition;
        //if(newPosition.y + Camera.main.orthographicSize.)
    }

    public void ShakeCamera()
    {
        originalPos = camTransform.localPosition;
        shakeDuration = 0.2f;
    }
}
