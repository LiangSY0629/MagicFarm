using System.Collections;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UIElements;

public class CowMovement : MonoBehaviour
{
    public SpriteLibraryAsset[] spriteLibrarys;
    public SpriteLibrary spriteLibrary;
    public Animator anim;
    public Rigidbody2D rb;

    [Header("设置动画持续时间和移动时间")]

    public float speed = 1;
    public Vector2Int minDistace, maxDistance;
    public float animTime = 10f;


    float currentAnimTime;
    float distance;
    int idle;
    bool walk = false;
    bool switchIdle = false;
    Vector2 finalPosition;

    private void Start()
    {
        StatsManager.Instance.maxAnimalTypes = spriteLibrarys.Length;

        spriteLibrary = GetComponent<SpriteLibrary>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        currentAnimTime = animTime;

    }

    private void Update()
    {

        if (TimeController.Instance.isNight)
        {
            return;
        }

        //spriteLibrary.spriteLibraryAsset = spriteLibrarys[StatsManager.Instance.animalSpriteLibrary];
        if (currentAnimTime > 0)
        {
            currentAnimTime -= Time.deltaTime;

            if (walk)
            {
                //距离目的地十分接近，直接调用函数重新生成目的地；
                distance = Vector2.Distance(transform.position, finalPosition);
                if (distance < 0.1f)
                {
                    ToMovement();
                }
            }

        }
        else
        {
            if (walk)
            {
                distance = Vector2.Distance(transform.position, finalPosition);
                if (distance > 0.1f)
                {
                    return;
                }
            }

            currentAnimTime = animTime;

            //当前正在休息动画，转成休息结束动画；
            if (idle == 3)
            {
                SetAwake();

                return;
            }
            else
            {

                walk = false;
                rb.velocity = Vector2.zero;
                switchIdle = !switchIdle;
                anim.SetBool("Switch", switchIdle);

            }

            //可切换时进行切换操作；
            if (switchIdle)
            {
                
                idle = Random.Range(0, 3);
                anim.SetFloat("Idle", idle);

                if (idle == 1)
                {
                    ToMovement();
                    walk = true;
                }

            }
            

        }

    }

    public void LieStay()
    {
        idle = 3;
        anim.Play("RangeAnim", 0, 0);
        anim.SetFloat("Idle", idle);
    }

    public void LieEnd()
    {
        switchIdle = false;
        anim.SetBool("Switch", false);
        currentAnimTime = animTime;
    }


    public void RangeDistance()
    {
        //随机目的地；
        finalPosition.x = Random.Range(minDistace.x, maxDistance.x);
        finalPosition.y = Random.Range(minDistace.y, maxDistance.y);
    }

    public void ToMovement()
    {
        //先随机目的地，如果过近就重新生成；
        RangeDistance();
        distance = Vector2.Distance(finalPosition, transform.position);

        while (distance < 1.5f)
        {
            RangeDistance();
            distance = Vector2.Distance(finalPosition, transform.position);
        }

        //计算到达目的地的向量值，赋值
        Vector2 movement = (finalPosition - (Vector2)(transform.position)).normalized;

        if (movement.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        rb.velocity = movement * speed;
    }

    /// <summary>
    /// 将动物切换到对应的品种；
    /// </summary>
    /// <param name="type"></param>
    public void SetType(int type)
    {
        spriteLibrary.spriteLibraryAsset = spriteLibrarys[type];
    }

    /// <summary>
    ///OKR老铁，也是吃上饭了；
    /// </summary>
    public void SetEat(float eatTime)
    {
        switchIdle = true;
        anim.SetBool("Switch", true);
        anim.Play("RangeAnim", 0 , 0);
        anim.SetFloat("Idle", 0);
        currentAnimTime = eatTime;
        walk = false;
        rb.velocity = Vector2.zero;
    }

    /// <summary>
    /// 看我雷霆产出雷霆大奶；
    /// </summary>
    public void SetProduct()
    {
        currentAnimTime = animTime;
        anim.SetBool("Switch", true);
        anim.Play("RangeAnim", 0, 0);
        anim.SetFloat("Idle", 5);
        walk = false;
        rb.velocity = Vector2.zero;

    }

    public void SetSleep()
    {
        anim.SetBool("Switch", true);
        anim.Play("RangeAnim", 0, 0);
        anim.SetFloat("Idle", 2);
        walk = false;
        rb.velocity = Vector2.zero;

    }


    public void SetAwake()
    {
        idle = 4;
        anim.Play("RangeAnim", 0, 0);
        anim.SetFloat("Idle", idle);
        walk = false;
    }

}
