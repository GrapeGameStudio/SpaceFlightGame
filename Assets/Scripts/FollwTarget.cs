using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollwTarget : MonoBehaviour
{

    public Transform targetObj;
    private Vector3 offset;
    [Range(1, 10)]             
    public float smooth = 3;            
    [Range(1,150)]
    public float borderRight = 10f;
    [Range(-1, -150)]
    public float borderLeft = -10f;
    [Range(1, 150)]
    public float borderUp = 10f;
    [Range(-1, -150)]
    public float borderDown = -10f;
    public bool isFollowTarget  = true;
    void Start()
    {
        offset = transform.position - targetObj.position;
    }
    void Update()
    {
        BorderCheck();
        if (isFollowTarget)transform.position = Vector3.Lerp(transform.position, targetObj.position + offset, smooth * Time.deltaTime);
    }
    private void BorderCheck()
    {
        float posX = this.transform.position.x;
        float posY = this.transform.position.y;
        if (posX >= borderRight||posX <=borderLeft || posY >= borderUp || posY <= borderDown) isFollowTarget = false;
    }
}
