using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitBehaviour : MonoBehaviour
{
 

    [Header("Gameplay Values")]
    public float maxSpeed;
    public float dragSpeed;
    public float throwForce;
    [Range(0, 1)]
    public float bounciness;
    [Range(0.98f, 1f)]
    public float airDecelerationRate;

    [Header("References")]
    public AudioClip grabSound;
    public AudioClip squishSound;
    public AudioClip biteSound;
    public AudioClip spitSound;

    public SpriteRenderer mySprite;
    public Sprite bittenSprite;

    private Vector3 targetPosition;
    private Vector3 myVel;
    private float xVel;
    private float yVel;
    private bool isOnGround = false;
    private int bitesTaken = 0;
    private AudioSource audioSource;
    private Vector3 mOffset;
    private bool isBeingDragged = false;

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void OnMouseDown()
    {
        mOffset = gameObject.transform.position - GetMouseWorldPos();
        xVel = 0;
        yVel = 0;
        isBeingDragged = true;
        gameObject.GetComponent<AudioSource>().PlayOneShot(grabSound);
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
        MoveSelf();
        CheckBoundaries();
    }

    private void MoveSelf()
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

    }

    private void CheckBoundaries()
    {
        // Ground check
        if (transform.position.y <= PhysicsManager.floorLevel)
        {
            transform.position = new Vector3(transform.position.x, PhysicsManager.floorLevel, 0);
            if (yVel < -maxSpeed * 0.2)
            {
                yVel = -yVel * bounciness;
                audioSource.PlayOneShot(squishSound);
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
                audioSource.PlayOneShot(squishSound);

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
                audioSource.PlayOneShot(squishSound);

            }
        }
    }

    public void EnterBasket()
    {
        Invoke("GetEaten", 0.2f);
        mySprite.enabled = false;

        xVel = 0;
        yVel = 0;
    }

    public void GetEaten()
    {
        bitesTaken++;
        audioSource.PlayOneShot(biteSound);

        if (bitesTaken == 1)
        {
            mySprite.sprite = bittenSprite;
            Invoke("SpitOut", 1f);
        }
        if (bitesTaken == 2)
        {
            Invoke("FinishApple", 0.9f);
        }
    }

    public void SpitOut()
    {
        mySprite.enabled = true;
        audioSource.pitch += 0.2f;
        audioSource.PlayOneShot(spitSound);
        xVel = 0.15f;
        yVel = 0.15f;
    }

    public void FinishApple()
    {
        Destroy(gameObject);
    }
}
