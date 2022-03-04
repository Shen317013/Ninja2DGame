using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float mySpeed;
    public float jumpForce;
    public GameObject attackColider , kunaiPrefab;

    float kunaiDistance;
    [HideInInspector] public int playerLife;
    [HideInInspector] public int playerKunai;

    Canvas myCanvas;

    [HideInInspector]
    public Animator myAnim;
    Rigidbody2D myRigi;
    SpriteRenderer mySr;

    [HideInInspector]
    public bool isJumpPressed , canJump , isAttack , isHurt , canBeHurt;

    public AudioClip[] myAudioClip;
    AudioSource myAudioSource;

    InputAction playerMove , playerJump , playerAttack , playerThrow;

    // Start is called before the first frame update

    private void Awake()
    {
        playerMove = GetComponent<PlayerInput>().currentActionMap["Move"];
        playerJump = GetComponent<PlayerInput>().currentActionMap["Jump"];
        playerAttack = GetComponent<PlayerInput>().currentActionMap["Attack"];
        playerThrow = GetComponent<PlayerInput>().currentActionMap["Kunai"];
        
        myCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        //GetComponent放在Awake
        myAnim = GetComponent<Animator>();
        myRigi = GetComponent<Rigidbody2D>();
        mySr = GetComponent<SpriteRenderer>();
        myAudioSource = GetComponent<AudioSource>();

        isJumpPressed = false;
        canJump = true;
        isAttack = false;
        isHurt = false;
        canBeHurt = true;

        playerLife = PlayerPrefs.GetInt("PlayerLife");
        playerKunai = PlayerPrefs.GetInt("PlayerKunai");
    }

    private void Update()
    {
        //跳躍
        if (playerJump.triggered && canJump == true && isHurt == false)
        {
            isJumpPressed = true;
            canJump = false;
        }

        //攻擊
        if (playerAttack.triggered && isHurt == false)
        {
            myAnim.SetTrigger("Attack");
            isAttack = true;
            canJump = false;
        }

        //飛鏢
        if (playerThrow.triggered && isHurt == false && !myAnim.GetCurrentAnimatorStateInfo(0).IsName("AttackThrow") && isAttack == false)
        {
            if (playerKunai > 0)
            {
                playerKunai--;
                PlayerPrefs.SetInt("PlayerKunai", playerKunai);
                myCanvas.KunaiUpdate();
                myAnim.SetTrigger("AttackThrow");
                isAttack = true;
                canJump = false;
            }
        }
    }

    private void FixedUpdate()
    {

        //上下左右移動,有關Rigidbody的程式必須放在fixedupdate裡面
        //float a = Input.GetAxisRaw("Horizontal");
        float a = playerMove.ReadValue<Vector2>().x;

        if(isAttack == true || isHurt == true)
        {
            a = 0;
        }

        if (a > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (a < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        myAnim.SetFloat("Run", Mathf.Abs(a));

        if (isJumpPressed)
        {
            myRigi.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isJumpPressed = false;

            myAnim.SetBool("Jump", true);
        }

        if (isHurt == false)
        {
            myRigi.velocity = new Vector2(a * mySpeed, myRigi.velocity.y);
        }

    }

    public void PlaySwordEffect()
    {
        myAudioSource.PlayOneShot(myAudioClip[3]);
    }

    public void PlayKunaiEffect()
    {
        myAudioSource.PlayOneShot(myAudioClip[2]);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "BoundBottom")
        {
            myAudioSource.PlayOneShot(myAudioClip[4]);
            playerLife = 0;
            PlayerPrefs.SetInt("PlayerLife" , playerLife);
            myCanvas.LifeUpdate();
            PlayerPrefs.SetInt("PlayerLife", 5);

            isHurt = true;
            isAttack = true;
            myRigi.velocity = new Vector2(0f, 0f);
            myAnim.SetBool("Dead", true);
            FadeInOut.instance.ScenceFadeInOut("LevelChange");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && isHurt == false && canBeHurt == true)
        {
            playerLife--;
            PlayerPrefs.SetInt("PlayerLife", playerLife);
            myCanvas.LifeUpdate();


            if (playerLife >= 1)
            {
                //玩家受傷
                myAudioSource.PlayOneShot(myAudioClip[0]);
                isHurt = true;
                canBeHurt = false;
                mySr.color = new Color(mySr.color.r, mySr.color.g, mySr.color.b, 0.5f);
                myAnim.SetBool("Hurt", true);

                if (transform.localScale.x == 1.0f)
                {
                    myRigi.velocity = new Vector2(-2.5f, 10.0f);
                }
                else if (transform.localScale.x == -1.0f)
                {
                    myRigi.velocity = new Vector2(2.5f, 10.0f);
                }

                StartCoroutine("SetIsHurtFalse");
            }

            else if (playerLife < 1)
            {
                //玩家死亡
                myAudioSource.PlayOneShot(myAudioClip[4]);
                isHurt = true;
                isAttack = true;
                myRigi.velocity = new Vector2(0f, 0f);
                myAnim.SetBool("Dead", true);
                PlayerPrefs.SetInt("PlayerLife", 5);
                FadeInOut.instance.ScenceFadeInOut("LevelChange");
            }
        }

        if (collision.tag == "Item")
        {
            myAudioSource.PlayOneShot(myAudioClip[1]);
        }
    }


    IEnumerator SetIsHurtFalse()
    {
        yield return new WaitForSeconds(1.0f);
        isHurt = false;
        myAnim.SetBool("Hurt", false);

        yield return new WaitForSeconds(1.0f);
        canBeHurt = true;
        mySr.color = new Color(mySr.color.r, mySr.color.g, mySr.color.b, 1.0f);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && isHurt == false && canBeHurt == true)
        {
            playerLife--;
            PlayerPrefs.SetInt("PlayerLife", playerLife);
            myCanvas.LifeUpdate();

            if (playerLife >= 1)
            {
                myAudioSource.PlayOneShot(myAudioClip[0]);
                isHurt = true;
                canBeHurt = false;
                mySr.color = new Color(mySr.color.r, mySr.color.g, mySr.color.b, 0.5f);
                myAnim.SetBool("Hurt", true);

                if (transform.localScale.x == 1.0f)
                {
                    myRigi.velocity = new Vector2(-2.5f, 10.0f);
                }
                else if (transform.localScale.x == -1.0f)
                {
                    myRigi.velocity = new Vector2(2.5f, 10.0f);
                }

                StartCoroutine("SetIsHurtFalse");
            }

            else if (playerLife < 1)
            {
                myAudioSource.PlayOneShot(myAudioClip[4]);
                isHurt = true;
                isAttack = true;
                myRigi.velocity = new Vector2(0f, 0f);
                myAnim.SetBool("Dead", true);
                PlayerPrefs.SetInt("PlayerLife" , 5);
                FadeInOut.instance.ScenceFadeInOut("LevelChange");
            }
        }
    }


    //要在第一個受傷的Frame呼叫這個涵式
    public void SetIsAttackFalse()
    {
        isAttack = false;
        canJump = true;
        myAnim.ResetTrigger("Attack");
        myAnim.ResetTrigger("AttackThrow");
    }

    public void ForIsHurtSetting()
    {
        isAttack = false;
        myAnim.ResetTrigger("Attack");
        myAnim.ResetTrigger("AttackThrow");
        attackColider.SetActive(false);
    }

    public void SetAttackColiderOn()
    {
        attackColider.SetActive(true);
    }

    public void SetAttackColiderOff()
    {
        attackColider.SetActive(false);
    }

    public void KunaiInstatiate()
    {
        if (transform.localScale.x == 1)
        {
            kunaiDistance = 1.0f;
        }
        else if (transform.localScale.x == -1.0f)
        {
            kunaiDistance = -1.0f;
        }

        Vector3 temp = new Vector3(transform.position.x + kunaiDistance, transform.position.y, transform.position.z);
        Instantiate(kunaiPrefab, temp, Quaternion.identity);
    }

}
