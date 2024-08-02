using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 3f;

    private Vector3 targetPosition;
    private Animator animator;
    private bool movingToB = true;

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = pointB.position;
        animator = GetComponent<Animator>();
        animator.SetBool("isWalking", true);
    }

    // Update is called once per frame
    void Update()
    {
        MoveTowardsTarget();
        UpdateTargetPosition();
    }

    void MoveTowardsTarget()
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        transform.LookAt(targetPosition);
    }

    void UpdateTargetPosition()
    {
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            if (movingToB)
            {
                targetPosition = pointA.position;
                movingToB = false;
            }
            else
            {
                targetPosition = pointB.position;
                movingToB = true;
            }
        }
    }
}
