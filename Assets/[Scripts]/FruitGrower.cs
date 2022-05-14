using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitGrower : MonoBehaviour
{
    public float offset;
    public float growRate;
    public float growDelay; 
    public GameObject fruitPrefab;
    private bool isGrowingFruit = false;
    public GameObject newFruit; 


    private void FixedUpdate()
    {
        if (gameObject.transform.childCount == 0 && !isGrowingFruit)
        {
            Invoke("GrowFruit", growDelay);
            isGrowingFruit = true;
        }
        if (newFruit)
        {
            if (newFruit.transform.localScale.x < 1)
            {
                // newFruit.transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(10, 10, 10), Time.deltaTime);
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
