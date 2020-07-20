using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [Range(0,10)]
    public float speed = 0;
    [Range(-10, 0)]
    public float fuspeed = 0;
    [Range(1,100000)]
    public float mass;
    public Transform head;
    public bool flag;

    private Rigidbody2D rigidbodySpace;
    private Vector2 dir;

    void Start()
    {
        rigidbodySpace = this.GetComponent<Rigidbody2D>();
        dir = head.position - this.transform.position;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigidbodySpace.mass = mass;

        transform.Translate(dir.normalized * Time.deltaTime*speed);
        

    }


}