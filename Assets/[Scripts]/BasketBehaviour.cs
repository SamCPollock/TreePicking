using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketBehaviour : MonoBehaviour
{
    [Header("References")]
    public GameObject bunnyRef;
    public AudioClip helloClip;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision) // Let fruit know they are in the basket when they enter the trigger
    {
        FruitBehaviour enteredFruit = collision.gameObject.GetComponent<FruitBehaviour>();
        if (enteredFruit)
        {
            enteredFruit.EnterBasket();
            bunnyRef.GetComponent<Animator>().SetTrigger("EatTrigger");
        }
    }

    private void OnMouseDown()  // Say hello when clicked on
    {
        audioSource.Stop();
        bunnyRef.GetComponent<Animator>().SetTrigger("ClickedTrigger");
        audioSource.PlayOneShot(helloClip);
    }
}
