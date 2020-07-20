using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedStar : MonoBehaviour
{
    public float rotationRate;
    
    void Update()
    {
        this.transform.Rotate(Vector3.forward * rotationRate * Time.deltaTime);                         //自转
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SpaceCraft._instance.isDie = true;
                
         }
    }
}
