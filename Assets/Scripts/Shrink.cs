using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Shrink : MonoBehaviour
{
    public float speed = 2;
    private bool isHitPlayer;
    private CircleCollider2D shrinkCollider;
    void Start()
    {
        shrinkCollider = this.GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isHitPlayer)
        {
            ShrinkSpeed(speed);
        }
    }

    private void ShrinkSpeed(float speed)
    {
        this.transform.localScale += new Vector3(speed * Time.deltaTime, speed * Time.deltaTime, 0);
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SpaceCraft._instance.isDie = true;

        }
    }
}
