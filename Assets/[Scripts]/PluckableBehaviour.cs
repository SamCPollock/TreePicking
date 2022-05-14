using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PluckableBehaviour : MonoBehaviour
{
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
        if (isBeingGrabbed)
        {
            Vector2 direction = GetMouseWorldPos() - transform.position;

            if (direction.magnitude > 0.3)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
                //parentObject.transform.rotation = rotation;
                parentObject.transform.rotation = Quaternion.Slerp(parentObject.transform.rotation, rotation, 0.1f);

                //gameObject.transform.position = Vector3.Lerp(parentObject.transform.position, GetMouseWorldPos - parentObject.transform.position);
                gameObject.transform.position = originalPosition + (GetMouseWorldPos() - parentObject.transform.position).normalized;

                if (direction.magnitude > 2)
                {
                    Pluck();
                }
            }
        }
        else
        {
            parentObject.transform.rotation = Quaternion.Lerp(parentObject.transform.rotation, Quaternion.identity, 0.1f);
            transform.position = Vector3.Lerp(transform.position, originalPosition, 0.2f);
        }
    }

    private void Pluck()
    {
        //Destroy(gameObject);
        gameObject.GetComponent<FruitBehaviour>().enabled = true;
        gameObject.transform.parent = null;
        parentObject.transform.rotation = Quaternion.identity;
        gameObject.GetComponent<PluckableBehaviour>().enabled = false;
    }
    //      else if (isOnBranch)
    //        {
    //            if (isBeingDragged)
    //            {
    //                //parentObject.transform.localEulerAngles =  new Vector3(0, 0, Vector3.Angle(parentObject.transform.position, targetPosition));
    //                //parentObject.transform.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(parentObject.transform.position.x - targetPosition.x, parentObject.transform.position.y - targetPosition.y) * Mathf.Rad2Deg);
    //                //parentObject.transform.localEulerAngles = Vector3.RotateTowards()
    //            }
    //        }


    //       else if (isOnBranch)
    //{
    //    parentObject = gameObject.transform.parent.gameObject;
    //    isBeingDragged = true;
    //}
}
