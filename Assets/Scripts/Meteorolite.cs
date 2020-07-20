using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteorolite : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            SpaceCraft._instance.CostFuel(1f);
        }
    }
}
