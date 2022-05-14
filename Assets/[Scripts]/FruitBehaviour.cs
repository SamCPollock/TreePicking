using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitBehaviour : MonoBehaviour
{
    //**** Implementations of draggable object//****
    private Vector3 mOffset;
    private bool isBeingDragged = false;

    public Vector3 gravity;
    public Vector3 myVel;
    public float xVel;
    public float yVel;
    public float maxSpeed;
    public float dragSpeed;
    public float throwForce;
    public float bounciness;
    public float airDecelerationRate; 

    public float floorLevel;
    public float screenWidth;

    private Vector3 targetPosition;

    private bool isOnGround = false;


    private void OnMouseDown()
    {
            mOffset = gameObject.transform.position - GetMouseWorldPos();
            xVel = 0;
            yVel = 0;
            isBeingDragged = true;

    }

    private void OnMouseUp()
    {
            isBeingDragged = false;
            xVel = (targetPosition.x - transform.position.x) * throwForce;
            yVel = (targetPosition.y - transform.position.y) * throwForce;
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
            myVel = new Vector3(xVel, yVel, 0);
            if (transform.position.y == PhysicsManager.floorLevel)
            {
                isOnGround = true;
            }
            else
            {
                isOnGround = false;
            }

            if (!isBeingDragged)
            {
                if (yVel > -maxSpeed)
                {
                    //yVel -= 0.001f;
                    yVel += PhysicsManager.gravity.y;
                }
                transform.position += myVel;
            }
            else if (isBeingDragged)
            {
                if (transform.position != targetPosition)
                {
                    transform.position = Vector3.Lerp(transform.position, targetPosition, dragSpeed);
                }
            }

            // Reduce xVel according to airDrag
            if (xVel > 0.001f || xVel < -0.001f)
            {
                if (isOnGround)
                {
                    xVel = xVel * 0.9f;

                }
                else
                {
                    xVel = xVel * airDecelerationRate;
                }
            }
            else
            {
                xVel = 0;
            }

            // Ground check
            if (transform.position.y <= PhysicsManager.floorLevel)
            {
                transform.position = new Vector3(transform.position.x, floorLevel, 0);
                if (yVel < -maxSpeed * 0.2)
                {
                    yVel = -yVel * bounciness;
                }
                else
                {
                    yVel = 0;
                }
            }

            // Walls checks
            {
                if (transform.position.x >= PhysicsManager.screenWidth)
                {
                    transform.position = new Vector3(PhysicsManager.screenWidth, transform.position.y, 0);
                    if (xVel > maxSpeed * 0.2)
                    {
                        xVel = -xVel * bounciness;
                    }
                    else
                    {
                        xVel = 0;
                    }
                }
                else if (transform.position.x <= -PhysicsManager.screenWidth)
                {
                    transform.position = new Vector3(-PhysicsManager.screenWidth, transform.position.y, 0);
                    if (xVel < -maxSpeed * 0.2)
                    {
                        xVel = -xVel * bounciness;
                    }
                    else
                    {
                        xVel = 0;
                    }
                }
            }
    }
}
