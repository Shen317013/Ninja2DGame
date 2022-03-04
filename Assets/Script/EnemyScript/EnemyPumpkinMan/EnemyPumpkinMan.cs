using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPumpkinMan : MonoBehaviour
{
    bool isAlive, isIdle, jumpAttack , isJumpUp , slideAttack , isHurt , canBeHurt;

    public int life;
    public float attackDistance, jumpHeight, jumpUpSpeed, jumpDownSpeed, slideSpeed, fallDownSpeed;

    GameObject player;
    Animator myAnim;
    Vector3 slideTargetPosition;
    BoxCollider2D myColider;
    SpriteRenderer mySr;
    AudioSource myAudioSource;

    // Start is called before the first frame update
    private void Awake()
    {
        player = GameObject.Find("Player");
        myAnim = GetComponent<Animator>();
        myColider = GetComponent<BoxCollider2D>();
        mySr = GetComponent<SpriteRenderer>();
        myAudioSource = GetComponent<AudioSource>();

        isAlive = true;
        isIdle = true;
        jumpAttack = false;
        isJumpUp = true;
        slideAttack = false;
        isHurt = false;
        canBeHurt = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {


            if (isIdle)
            {
                LookAtPlayer();
                if (Vector3.Distance(player.transform.position, transform.position) <= attackDistance)
                {
                    //slideattack
                    isIdle = false;
                    StartCoroutine("IdleToSlideAttack");
                }
                else
                {
                    //jumpattack
                    isIdle = false;
                    StartCoroutine("IdleToJumpAttack");
                }
            }

            else if (jumpAttack)
            {
                LookAtPlayer();
                if (isJumpUp)
                {
                    Vector3 myTarget = new Vector3(player.transform.position.x, jumpHeight, transform.position.z);
                    transform.position = Vector3.MoveTowards(transform.position, myTarget, jumpUpSpeed * Time.deltaTime);
                    myAnim.SetBool("jumpup", true);
                }
                else
                {
                    myAnim.SetBool("jumpup", false);
                    myAnim.SetBool("jumpdown", true);

                    Vector3 myTarget = new Vector3(transform.position.x, -2.85f, transform.position.z);
                    transform.position = Vector3.MoveTowards(transform.position, myTarget, jumpDownSpeed * Time.deltaTime);
                }

                if (transform.position.y == jumpHeight)
                {
                    isJumpUp = false;
                }
                else if (transform.position.y == -2.85f)
                {
                    jumpAttack = false;
                    StartCoroutine("JumpDownToIdle");
                }

            }

            else if (slideAttack)
            {
                myAnim.SetBool("slide", true);
                transform.position = Vector3.MoveTowards(transform.position, slideTargetPosition, slideSpeed * Time.deltaTime);

                if (transform.position == slideTargetPosition)
                {
                    //調整滑行的colider
                    myColider.offset = new Vector2(-0.1709125f, -0.1012828f);
                    myColider.size = new Vector2(0.9728805f, 2.130099f);
                    myAnim.SetBool("slide", false);
                    slideAttack = false;
                    isIdle = true;
                }
            }

            else if (isHurt)
            {
                //不會因為在空中被攻擊而發生bug(即使空中受傷依然會落下)
                Vector3 myTargetPosition = new Vector3(transform.position.x, -2.85f, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, myTargetPosition, fallDownSpeed * Time.deltaTime);
            }

        }
        else
        {
            //不會因為停在半空中死亡
            Vector3 myTargetPosition = new Vector3(transform.position.x, -2.85f, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, myTargetPosition, fallDownSpeed * Time.deltaTime);
        }

    }

    void LookAtPlayer()
    {
        //解決轉向問題
        if (player.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }

        else
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
    }

    IEnumerator IdleToSlideAttack()
    {
        yield return new WaitForSeconds(1.0f);
        myColider.offset = new Vector2(-0.1709125f , -0.490155f);
        myColider.size = new Vector2(0.9728805f , 1.33514f);
        slideTargetPosition = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
        LookAtPlayer();
        slideAttack = true;
    }

    IEnumerator IdleToJumpAttack()
    {
        yield return new WaitForSeconds(1.0f);
        jumpAttack = true;
    }

    IEnumerator JumpDownToIdle()
    {
        yield return new WaitForSeconds(0.5f);
        isIdle = true;
        isJumpUp = true;
        myAnim.SetBool("jumpup", false);
        myAnim.SetBool("jumpdown", false);
    }

    IEnumerator SetAnimHurtToFalse()
    {
        //受傷完後會立即起身,並且會3秒半透明無敵
        yield return new WaitForSeconds(0.5f);
        myAnim.SetBool("hurt", false);
        myAnim.SetBool("jumpup", false);
        myAnim.SetBool("jumpdown", false);
        myAnim.SetBool("slide", false);
        mySr.material.color = new Color(1.0f, 1.0f , 1.0f , 0.5f);
        isHurt = false;
        isIdle = true;

        yield return new WaitForSeconds(2.0f);
        canBeHurt = true;
        mySr.material.color = new Color(1.0f, 1.0f, 1.0f , 1.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag == "PlayerAttack") 
        {
            if (canBeHurt)
            {
                myAudioSource.PlayOneShot(myAudioSource.clip);
                life--;
                if (life >= 1)
                {
                    //受傷
                    isIdle = false;
                    jumpAttack = false;
                    slideAttack = false;
                    isHurt = true;
                    StopCoroutine("JumpDownToIdle");
                    StopCoroutine("IdleToJumpAttack");
                    StopCoroutine("IdleToSlideAttack");
                    myAnim.SetBool("hurt", true);
                    StartCoroutine("SetAnimHurtToFalse");
                }
                else
                {
                    //死亡
                    isAlive = false;
                    myColider.enabled = false;
                    StopAllCoroutines();
                    myAnim.SetBool("dead", true);
                    Time.timeScale = 0.5f;
                    StartCoroutine("AfterDead");
                }

                canBeHurt = false;
            }
            

        }
    }

    IEnumerator AfterDead()
    {
        yield return new WaitForSecondsRealtime(3f);
        FadeInOut.instance.ScenceFadeInOut("LevelChange");
    }
}
