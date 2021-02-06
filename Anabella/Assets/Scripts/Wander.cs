using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Wander : MonoBehaviour
{

    public float pursuitSpeed;
    public float wanderSpeed;

    float currentSpeed;

    public float directionChangeInterval;

    public bool followPlayer;

    Coroutine moveCoroutine;

    Rigidbody2D rb2D;
    Animator animator;
    Transform targetTransform = null;

    Vector3 endPosition;
    public GameObject player;

    float currentAngle = 0;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        currentSpeed = wanderSpeed;
        StartCoroutine(WanderRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator WanderRoutine()
    {
        while(true)
        {
            ChooseEndpoint();

            moveCoroutine = StartCoroutine(Move(rb2D, currentSpeed));

            yield return new WaitForSeconds(directionChangeInterval);
            
        }
    }

    private void ChooseEndpoint()
    {
        currentAngle += UnityEngine.Random.Range(0,360);
        currentAngle = Mathf.Repeat(currentAngle, 360);

        endPosition = Vector3FromAngle(currentAngle);
    }

    private Vector3 Vector3FromAngle(float inputAngleDegrees)
    {
        inputAngleDegrees  = inputAngleDegrees * Mathf.Deg2Rad;

        return new Vector3(Mathf.Cos(inputAngleDegrees), Mathf.Sin(inputAngleDegrees),0);
    }

    private IEnumerator Move(Rigidbody2D rigidBodyToMove, float speed)
    {
        float remainingDistance = (transform.position - endPosition).sqrMagnitude;

        while (remainingDistance > float.Epsilon)
        {
                if(targetTransform != null)
                {
                    endPosition = targetTransform.position;
                }
                if(rigidBodyToMove != null)
                {
                    animator.SetInteger("AnimationState", 0);

                    Vector3 newPosition = Vector3.MoveTowards(rigidBodyToMove.position, endPosition, speed * Time.deltaTime);

                    rb2D.MovePosition(newPosition);

                    remainingDistance = (transform.position - endPosition).sqrMagnitude;
                }
                yield return new WaitForFixedUpdate();
        }
                    animator.SetInteger("AnimationState", 1);

    }

    //used with the circle collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && followPlayer)
        {
            currentSpeed = pursuitSpeed;

            targetTransform = collision.gameObject.transform;
        }
        moveCoroutine = StartCoroutine(Move(rb2D, currentSpeed));

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && followPlayer)
        {
            currentSpeed = wanderSpeed;

            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }

            targetTransform = null;
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            currentSpeed = wanderSpeed;

            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }

            targetTransform = null;
        }
        
    }
    
}
