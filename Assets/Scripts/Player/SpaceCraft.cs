using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class SpaceCraft : MonoBehaviour
{
    public static SpaceCraft _instance;

    private float fuel = 20;    //燃料
    public float speed;         //移动速度
    //public float dspeed;      //减速
    public bool isDie;          //飞船被摧毁
    public bool isPull;         //是否被牵引
    public bool isIgnition;     //飞船是否点火
    public bool isStatic;


    //Time
    private float timer;
    public float maxChargeTime = 3;   //最大按压时间长度
    
    //UI相关
    public Slider fuelSlider;             //燃料条
    public Text fuelValue;                //燃料值

    public Transform fiexedStar;
    public Transform headSpaceCraft;
    private Rigidbody2D spaceCarftRigidbody;

    public Vector3 flyDirection;   //飞船飞行方向
    
    private Vector2 v1, v2;

    public float Fuel { get => fuel; set => fuel = value; }

    private void Awake()
    {
        _instance = this;
    }
    public void Start()
    {
        spaceCarftRigidbody = this.GetComponent<Rigidbody2D>();
        flyDirection = headSpaceCraft.position - this.transform.position;
    }

    public void Update()
    {
        v1 = transform.position;
        //飞船   
        
        
        if (Input.GetMouseButton(0) && timer <= maxChargeTime)
        {
            timer += Time.deltaTime;    
        }
        if (Input.GetMouseButtonUp(0))
        {
            if(isPull)CostFuel(1f);
            isIgnition = true;
            print("Timer:" + timer);
        }
        if (Vector2.Distance(fiexedStar.position, transform.position) < 3f)
        {
            isDie = true;
            print("撞上太阳，游戏结束");
        }
        //ControlMoveMent();
        CheckFule();
        CheckIsDie();
        ShowUI();
    }

    private void FixedUpdate()
    {
        flyDirection = headSpaceCraft.position - this.transform.position;
        if (isIgnition)
        {
            Ignition(isPull, speed * timer, flyDirection);
            timer = 0;
            StopCoroutine(CountDrag(2f));
            StartCoroutine(CountDrag(2f));
        }

        if (isStatic && !isPull)
        {
            SpaceCarftDecelerate();
        }
        else if(isStatic||isPull)
        {
           // StopAllCoroutines();
            Vector2 dir = fiexedStar.position - transform.position;
            StopCoroutine(Rotate(dir));
        }

    }



    //IEnumerator CoutTime(float timer)
    //{
    //    yield return new WaitForSeconds(timer);
    //    isIgnition = false;
    //}

    IEnumerator CountDrag(float drag)
    {
        spaceCarftRigidbody.drag = 1;
        while (spaceCarftRigidbody.drag < drag)
        {
            spaceCarftRigidbody.drag += Time.deltaTime;
            yield return null;
        }
        isIgnition = false;
        yield return new WaitForSeconds(3f);
        isStatic = true;
    }

    private void ShowUI()         //显示燃料UI
    {
        fuelSlider.value = Fuel;
        fuelValue.text = Fuel.ToString() + "/20";
    }


    private void SpaceCarftDecelerate()  //飞船减速 -> 行星方向飞行
    {
        Vector2 dir = fiexedStar.position - transform.position;
        StopCoroutine(Rotate(dir));
        StartCoroutine(Rotate(dir));
        spaceCarftRigidbody.AddForce(dir * 0.09f, ForceMode2D.Force);   
    }


    IEnumerator Rotate(Vector2 dir)
    {
        float z;
        if ((dir).x > 0)
        {
            z = -Vector3.Angle(Vector3.up, dir);
            float angle = 0;
            while (angle <= z)
            {
                this.transform.eulerAngles = new Vector3(0, 0, angle);
                angle -= 0.3f;
                yield return null;
            }
        }
        else
        {
            z = Vector3.Angle(Vector3.up, dir);
            float angle = 0;
            while (angle >= z)
            {
                this.transform.eulerAngles = new Vector3(0, 0, angle);
                angle += 0.1f;
                yield return null;
            }
        }
        
        
        
    }
    

    //操控飞船
    private void ControlMoveMent()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        transform.Translate(new Vector2(horizontal * speed * Time.deltaTime, vertical * speed * Time.deltaTime));
    }
   
    private void Ignition(bool isPull,float speed,Vector2 direction)   //控制飞船点火
    {
        if (!isPull) return;      
        spaceCarftRigidbody.AddForce(direction.normalized * speed, ForceMode2D.Impulse);    
    }

    public void CostFuel(float cost)     //消耗燃料
    {
        this.Fuel = Mathf.Clamp(Fuel,1,20)-cost;
    }

    private void CheckFule()         //燃料为空,速度为负方向
    {
        if(Fuel <= 0)
        {
            SpaceCarftDecelerate();
        }
    }
    private void OnBecameVisible()
    {
        
    }
    private void OnBecameInvisible()
    {      
        isDie = true;
        print("飞离摄像机视野,游戏结束");
    }

    public void CheckIsDie()            //检查是否死亡
    {
        if (isDie)
        {
            print("Game Over");
        }
    }

   
}
