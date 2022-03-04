using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMaleZombie : MonoBehaviour
{
    public Vector3 targetPosition;
    public float mySpeed;
    public GameObject attackColider;
    public int enemyLife;

    protected Animator myAnim;
    protected Vector3 originPosition , turnPoint;

    protected bool isFirstTimeIdle , isAfterBattleCheck , isAlive;

    protected GameObject myPlayer;
    protected BoxCollider2D myColider;
    protected SpriteRenderer mySr;
    [SerializeField]
    protected AudioClip[] myAudioClip;
    protected AudioSource myAudioSource;


    protected virtual void Awake()
    {
        myAnim = GetComponent<Animator>();
        myColider = GetComponent<BoxCollider2D>();
        mySr = GetComponent<SpriteRenderer>();
        //玩家物件
        myPlayer = GameObject.Find("Player");
        originPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        isFirstTimeIdle = true;
        isAfterBattleCheck = false;
        isAlive = true;
        myAudioSource = GetComponent<AudioSource>();


    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveAndAttack();
    }

    protected virtual void MoveAndAttack()
    {
        if (isAlive)
        {
            //判斷敵人與玩家距離,做出攻擊,並不會重複多做一次攻擊
            if (Vector3.Distance(myPlayer.transform.position, transform.position) < 1.2f)
            {
                //敵人會依據玩家方向轉向攻擊
                if (myPlayer.transform.position.x <= transform.position.x)
                {
                    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                }
                else
                {
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }


                if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("MaleZombieattack") || myAnim.GetCurrentAnimatorStateInfo(0).IsName("MaleZombieattackwait"))
                {
                    return;
                }

                //myAudioSource.PlayOneShot(myAudioClip[1]);
                myAnim.SetTrigger("attack");
                isAfterBattleCheck = true;
                return;
            }
            else
            {
                if (isAfterBattleCheck)
                {
                    //敵人轉向攻擊完,會轉回去
                    if (turnPoint == targetPosition)
                    {
                        StartCoroutine(TurnRight(false));
                    }
                    else if (turnPoint == originPosition)
                    {
                        StartCoroutine(TurnRight(true));
                    }
                    isAfterBattleCheck = false;
                }
            }

            //走到定點敵人會休息,並不讓敵人多重複休息一次
            if (transform.position.x == targetPosition.x)
            {
                myAnim.SetTrigger("idle");
                turnPoint = originPosition;
                StartCoroutine(TurnRight(true));
                isFirstTimeIdle = false;
            }
            else if (transform.position.x == originPosition.x)
            {

                if (isFirstTimeIdle == false)
                {
                    myAnim.SetTrigger("idle");
                }

                turnPoint = targetPosition;
                StartCoroutine(TurnRight(false));
            }

            //敵人走路
            if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("MaleZombiewalk"))
            {
                transform.position = Vector3.MoveTowards(transform.position, turnPoint, mySpeed * Time.deltaTime);
            }
        }
    }


    protected IEnumerator TurnRight(bool turnLeft) 
    {
        //敵人會休息完後,才進行轉向
        yield return new WaitForSeconds(2.0f);
        if (turnLeft == true)
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else 
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
        
    }


    public void SetAttackColiderOn()
    {
        attackColider.SetActive(true);
    }

    public void SetAttackColiderOff()
    {
        attackColider.SetActive(false);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        //敵人受傷觸發,並且死亡後不會轉向也關閉colider
        if (collision.tag == "PlayerAttack")
        {
            myAudioSource.PlayOneShot(myAudioClip[0]);
            enemyLife--;
            if (enemyLife >= 1)
            {
                myAnim.SetTrigger("hurt");
            }
            else if (enemyLife < 1)
            {
                isAlive = false;
                myColider.enabled = false;
                myAnim.SetTrigger("dead");
                StartCoroutine("AfterDead");
            }
        }
    }

    IEnumerator AfterDead()
    {
        //敵人死亡後會變淡,並消失
        yield return new WaitForSeconds(1.0f);
        //顏色的變更可使用這行程式碼即可
        mySr.material.color = new Color(1.0f , 1.0f , 1.0f , 0.5f);

        yield return new WaitForSeconds(1.0f);
        mySr.material.color = new Color(1.0f, 1.0f, 1.0f, 0.2f);

        yield return new WaitForSeconds(1.0f);
        Destroy(this.gameObject);
    }
}
