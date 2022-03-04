using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFemaleZombie : EnemyMaleZombie
{
    public float RunSpeed;
    bool isBattleMode;

    protected override void Awake()
    {
        base.Awake();
        isBattleMode = true;
    }

    protected override void MoveAndAttack()
    {
        if (isAlive)
        {
            if (isBattleMode == true)
            {
                //判斷敵人與玩家距離,做出攻擊,並不會重複多做一次攻擊
                if (Vector3.Distance(myPlayer.transform.position, transform.position) < 4.0f)
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

                    //讓女性殭屍不會因為玩家y軸的移動而上下移動跟隨
                    Vector3 newTarget = new Vector3(myPlayer.transform.position.x, transform.position.y, transform.position.z);

                    //讓女性殭屍不會在追蹤玩家途中播放idle
                    if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("MaleZombiewalk"))

                        //判斷玩家距離是否足夠,足夠觸發衝撞
                        transform.position = Vector3.MoveTowards(transform.position, newTarget, RunSpeed * Time.deltaTime);

                    isAfterBattleCheck = true;
                    return;
                }
                else
                {
                    if (isAfterBattleCheck)
                    {
                        //解決女性殭屍在玩家超出範圍時,倒退走
                        if (transform.position.x > turnPoint.x || transform.position.x < turnPoint.x)
                        {
                            if (transform.position.x > turnPoint.x)
                            {
                                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                            }

                            else if (transform.position.x < turnPoint.x)
                            {
                                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                            }

                            else
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
                            }
                        }
                        isAfterBattleCheck = false;
                    }
                }
            }
            else 
            {
                if (transform.position.x > turnPoint.x)
                {
                    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                }
                else
                {
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }

                if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("MaleZombiewalk"))
                {
                    transform.position = Vector3.MoveTowards(transform.position, turnPoint, mySpeed * Time.deltaTime);
                }
                
                if (transform.position == turnPoint)
                {
                    isBattleMode = true;
                }

                return;
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

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.tag == "StopPoint")
        {
            isBattleMode = false;
        }

        if (collision.tag == "PlayAttack")
        {
            isBattleMode = true;
        }
    }
}
