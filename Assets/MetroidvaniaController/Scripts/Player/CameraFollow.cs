using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float FollowSpeed = 2f;
    public Transform Target;

    public BoxCollider2D TargetRoomBounds;

    public bool LockHorizontal;
    public bool LockVertical;

    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    private Transform camTransform;

    // How long the object should shake for.
    public float shakeDuration = 0f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.1f;
    public float decreaseFactor = 1.0f;
    public bool verticalMovementAllowed = true;
    public bool horizontalMovementAllowed = true;

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

    private void SetCameraPosition()
    {
        Vector3 targetPosition = Target.position;
        targetPosition.z = -10;
        Vector3 newPosition = Vector3.Slerp(transform.position, targetPosition, FollowSpeed * Time.deltaTime);
        float cameraHalfHeight = Camera.main.orthographicSize;
        float cameraHalfWidth = Camera.main.aspect * cameraHalfHeight;

        float cameraHorizontalMin = -cameraHalfWidth;
        float cameraHorizontalMax = cameraHalfWidth;
        float cameraVerticalMin = -cameraHalfHeight;
        float cameraVerticalMax = cameraHalfHeight;
        if (TargetRoomBounds != null)
        {
            float boundHalfHeight = TargetRoomBounds.bounds.extents.y;
            float boundHalfWidth = TargetRoomBounds.bounds.extents.x;

            float boundHorizontalMin = -boundHalfWidth;
            float boundHorizontalMax = boundHalfWidth;
            float boundVerticalMin = -boundHalfHeight;
            float boundVerticalMax = boundHalfHeight;

            if (horizontalMovementAllowed)
            {
                if (newPosition.x + cameraHorizontalMax > TargetRoomBounds.transform.position.x + boundHorizontalMax)
                {
                    newPosition.x = TargetRoomBounds.transform.position.x + boundHorizontalMax - cameraHorizontalMax;
                }
                if (newPosition.x + cameraHorizontalMin < TargetRoomBounds.transform.position.x + boundHorizontalMin)
                {
                    newPosition.x = TargetRoomBounds.transform.position.x + boundHorizontalMin - cameraHorizontalMin;
                }
            }
            else
            {
                newPosition.x = transform.position.x;
            }

            if (verticalMovementAllowed)
            {
                if (newPosition.y + cameraVerticalMax > TargetRoomBounds.transform.position.y + boundVerticalMax)
                {
                    newPosition.y = TargetRoomBounds.transform.position.y + boundVerticalMax - cameraVerticalMax;
                }

                if (newPosition.y + cameraVerticalMin < TargetRoomBounds.transform.position.y + boundVerticalMin)
                {
                    newPosition.y = TargetRoomBounds.transform.position.y + boundVerticalMin - cameraVerticalMin;
                }
            }
            else
            {
                newPosition.y = transform.position.y;
            }
        }

        transform.position = newPosition;
    }

    public void ShakeCamera()
    {
        originalPos = camTransform.localPosition;
        shakeDuration = 0.2f;
    }
}
