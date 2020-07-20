using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Planets : MonoBehaviour
{
    public Transform planetTrans;
    public float gravitationalRadius;       //引力半径
    public float drawingSpeed;              //牵引速率
    public float rotationRate;              //自转速度
    public float planetSpeedRevolution;     //公转公转速度
    public float spaceCraftSpeedRevolution; //飞船公转速度
    public float distance = 2f;

    public bool isShowDrawRange;            //是否在场景中显示引力覆盖范围
    public bool isOtherRevolution;          //其他物体围绕是否行星公转
    public bool isRevolution;               //自身是否公转

    public bool isFollowTarget;             //是否跟随目标
    //private Vector3 offset;               //偏移量

    public CircleCollider2D planetCollider1;
    public CircleCollider2D planetCollider2;

    public Transform fixedStar;             //公转中心
    private Transform spaceCraftTrans;      //星球引力范围捕获对象

    private Vector3 tp;                    //切点

    public Transform gameObj;              //测试区
    public Transform posintObj;
    private void Start()
    {
        planetCollider1 = this.GetComponent<CircleCollider2D>();
        planetCollider1.radius = gravitationalRadius;     
    }
    public void FixedUpdate()
    {
        this.transform.Rotate(Vector3.forward * rotationRate * Time.deltaTime);                         //自转
        Revolution(fixedStar, planetTrans, isRevolution, planetSpeedRevolution);                        //公转
        if (spaceCraftTrans != null) {                                                                  //飞船相关
            Revolution(planetTrans, spaceCraftTrans, isOtherRevolution, spaceCraftSpeedRevolution);     //飞船公转   

            if (planetCollider2.IsTouching(spaceCraftTrans.GetComponent<BoxCollider2D>()))
            {
                SpaceCraft._instance.isDie = true;
                print("撞上小行星，游戏结束");
            }

            Vector3 dir = new Vector3();
            if (SpaceCraft._instance.isPull)                                                            //调整偏移方向
            {      
                tp = PointOnTangent(planetTrans, spaceCraftTrans, 90);
                posintObj.position = tp;
                dir = (tp - spaceCraftTrans.position).normalized;
                if (!SpaceCraft._instance.isIgnition)
                {
                    float z;
                    if ((dir).x > 0)
                    {
                        z = -Vector3.Angle(Vector3.up, dir);
                    }
                    else
                    {
                        z = Vector3.Angle(Vector3.up, dir);
                    }
                    spaceCraftTrans.eulerAngles = Vector3.Lerp(spaceCraftTrans.eulerAngles, new Vector3(0, 0, (z)),1f);                
                }
                if (SpaceCraft._instance.isIgnition)                    //如果飞船点火
                {                   
                    SpaceCraft._instance.flyDirection = dir;
                }
                //Debug.DrawLine(planetTrans.position, spaceCraftTrans.position, Color.green);
                //Debug.DrawLine(spaceCraftTrans.position, tp, Color.yellow);
            }

            
           
        }       
    }
    private void OnTriggerEnter2D(Collider2D collision)   
    {
        if (collision == null) return;   
        if(collision.gameObject.tag == "Player")
        {
            spaceCraftTrans = collision.transform;
            //offset = planetTrans.position - collision.transform.position;
            isFollowTarget = true;
            SetParent(spaceCraftTrans, planetTrans);
            isOtherRevolution = true;         
            SpaceCraft._instance.isPull = true;
            SpaceCraft._instance.isStatic = false;
            StartCoroutine(MoveToTarget(collision.transform,planetTrans));
            //print(collision.name);
        }
        else
        {
            return;
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == null) return;
        if (collision.gameObject.tag == "Player")
        {
            StopAllCoroutines();
           // StopCoroutine(MoveToTarget(collision.transform,planetTrans));
            print("线程结束");
            SetParent(spaceCraftTrans, null);
            isFollowTarget = false;
            isOtherRevolution = false;          
            SpaceCraft._instance.isPull = false;
        }
        else
        {
            return;
        }
            
    }

    IEnumerator MoveToTarget(Transform target,Transform planetCenter)  //Target移动到目标位置
    {
        while (Vector2.Distance(planetCenter.position, target.position) > 0.05)
        {
            print(planetCenter.name);
            drawingSpeed = gravitationalRadius / Vector2.Distance(planetCenter.position, target.position);   //
            target.position = Vector2.Lerp(target.position, planetCenter.position, drawingSpeed *0.008f* Time.deltaTime);
            
            yield return null;
        }
        
    }
    public void Revolution(Transform revolutionCenter,Transform target,bool isRevolution,float speed)   //公转
    {
        if(isRevolution)target.RotateAround(revolutionCenter.position, Vector3.forward, rotationRate * Time.deltaTime*speed);    //公转
    }
    private void  SetParent(Transform child, Transform parent)
    {
         child.SetParent(parent);
    }
    private void OnDrawGizmos()  //在场景绘制引力覆盖范围
    {
        if (isShowDrawRange)
        {
            Gizmos.DrawWireSphere(transform.position, gravitationalRadius * this.transform.localScale.x);
        }     
    }
    private Vector2 PointOnTangent(Transform center,Transform spaceCraftTrans,float rotateAngle) //求切线上的点
    {       
        float angle = AngleBetweenVector(center.position, spaceCraftTrans.position);      
        float radius = Vector2.Distance(spaceCraftTrans.position, center.position);  //半径
        Vector2 point = PointOnCrile(center.position, radius, angle);
        Vector2 tangentPoint = PointOnCrile(point, 2f, angle + rotateAngle);
        return tangentPoint;
    }
    private float AngleBetweenVector(Vector3 center, Vector3 spaceCraftTrans)  //求两向量之间的夹角
    {
        float angle = 0;
        Vector3 fixedPoint = new Vector3(Mathf.Abs(center.x) + 1f, center.y);
        Vector2 dirA = center - fixedPoint;
        Vector2 dirB = center - spaceCraftTrans;
        float dot = Vector2.Dot(dirA.normalized, dirB.normalized);
        if (spaceCraftTrans.y < center.y)
        {
            angle = -Mathf.Acos(dot) * Mathf.Rad2Deg;
        }
        else
        {
            angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
        }
        return angle;
    }
    private Vector2 PointOnCrile(Vector2 position,float radius,float radian)  //求圆上的一点
    {
        Vector2 vec =  new Vector2();
        vec.x = position.x + radius * Mathf.Cos(Mathf.Deg2Rad*radian);
        vec.y = position.y + radius * Mathf.Sin(Mathf.Deg2Rad*radian);
        return vec;
    }


}
