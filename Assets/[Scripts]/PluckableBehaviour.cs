using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PluckableBehaviour : MonoBehaviour
{
    [Header("Gameplay Values")]
    public float pullDistance = 2f;

    [Header("References")]
    public AudioClip pluckSound;
    public AudioClip grabSound;

    private Vector3 mOffset;
    private bool isBeingGrabbed; 
    private GameObject parentObject;
    private Vector3 targetPosition;
    private Vector3 originalPosition;

    void Start()
    {
        parentObject = transform.parent.gameObject;
        originalPosition = transform.position;
    }

    private void OnMouseDown()
    {
        isBeingGrabbed = true;
        gameObject.GetComponent<AudioSource>().PlayOneShot(grabSound);
    }

    private void OnMouseUp()
    {
        isBeingGrabbed = false;
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = 0;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private void OnMouseDrag()
    {
        targetPosition = GetMouseWorldPos() + mOffset;
    }


    private void FixedUpdate()
    {
        MoveSelf();
    }

    private void MoveSelf()
    {
        if (isBeingGrabbed) // Get tugged by mouse if being grabbed. 
        {
            Vector2 direction = GetMouseWorldPos() - transform.position;

            if (direction.magnitude > 0.3)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
                parentObject.transform.rotation = Quaternion.Slerp(parentObject.transform.rotation, rotation, 0.1f);

                gameObject.transform.position = originalPosition + (GetMouseWorldPos() - parentObject.transform.position).normalized;

                if (direction.magnitude > pullDistance) // Get pulled off branch if exceeding required pull distance. 
                {
                    Pluck();
                }
            }
        }
        else  // Return to default if not being grabbed
        {
            parentObject.transform.rotation = Quaternion.Lerp(parentObject.transform.rotation, Quaternion.identity, 0.1f);
            transform.position = Vector3.Lerp(transform.position, originalPosition, 0.2f);
        }
    }

    private void Pluck()
    {
        gameObject.GetComponent<AudioSource>().PlayOneShot(pluckSound);
        gameObject.GetComponent<FruitBehaviour>().enabled = true;
        gameObject.transform.parent = null;
        parentObject.transform.rotation = Quaternion.identity;
        gameObject.GetComponent<PluckableBehaviour>().enabled = false;
    }
}
