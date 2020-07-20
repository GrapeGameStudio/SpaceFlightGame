using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderLimit : MonoBehaviour
{
    //进入边界
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    //处于边界
    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }

    //出边界
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameManager._instance.isVictory = true;
        }
    }
}
