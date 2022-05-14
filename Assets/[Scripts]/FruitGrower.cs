using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitGrower : MonoBehaviour
{
    [Header("Gameplay Values")]
    public float offset;
    [Range(1f, 1.04f)]
    public float growRate;
    public float growDelay; 

    [Header("References")]
    public GameObject fruitPrefab;
    
    private GameObject newFruit; 
    private bool isGrowingFruit = false;

    private void FixedUpdate()
    {
        if (gameObject.transform.childCount == 0 && !isGrowingFruit)    // Make a new fruit if has no fruit
        {
            Invoke("GrowFruit", growDelay);
            isGrowingFruit = true;
        }
        if (newFruit)   // Make new fruit grow to max size. 
        {
            if (newFruit.transform.localScale.x < 1)
            {
                newFruit.transform.localScale *= growRate;
            }
        }
    }

    public void GrowFruit()
    {
        newFruit = Instantiate(fruitPrefab, new Vector3(transform.position.x, transform.position.y - offset, 0), Quaternion.identity, gameObject.transform);
        newFruit.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        isGrowingFruit = false;
    }
}
