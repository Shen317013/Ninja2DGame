using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airplatform : MonoBehaviour
{
    public Vector3 turnPoint;
    Vector3 targetPosition , originPosition;
    public float moveSpeed;

    private void Awake()
    {
        originPosition = transform.position;
    }


    // Update is called once per frame
    void Update()
    {
        if (transform.position == originPosition)
        {
            targetPosition = turnPoint;
        }
        else if (transform.position == turnPoint)
        {
            targetPosition = originPosition;
        }


        transform.position = Vector3.MoveTowards(transform.position , targetPosition , moveSpeed * Time.deltaTime);
    }
}
