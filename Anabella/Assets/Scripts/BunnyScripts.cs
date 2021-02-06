using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyScripts : MonoBehaviour
{  
  Rigidbody2D rb2D;
  Transform soul;
  Animator animator;

  // Use this for initialization
  void Start () {
    rb2D = this.GetComponent<Rigidbody2D>();
    animator = GetComponent<Animator>();
    soul = GameObject.FindGameObjectWithTag("Coin").GetComponent<Transform>();
    Vector2 randomPositionOnScreen = Camera.main.ViewportToWorldPoint(new Vector2(Random.value, Random.value));
    transform.position = randomPositionOnScreen;

  }

    Vector2 movement = new Vector2();
    string animationState = "AnimationState";

    enum CharStates
    {
        idle = 0,
        rightwalk= 1,
        leftwalk = 2
    }

  private void UpdateState()
  {
      if (movement.x > 0)
          animator.SetInteger(animationState, (int)CharStates.leftwalk);

      else if (movement.x < 0)
          animator.SetInteger(animationState, (int)CharStates.rightwalk);

      else
          animator.SetInteger(animationState, (int)CharStates.idle);
  }

    // Update is called once per frame
    void Update () 
    {
      UpdateState();

      if(Vector2.Distance(soul.position, transform.position) < 4)
      {
        transform.position = Vector3.MoveTowards(transform.position, soul.position, Time.deltaTime);
      }
    }

  void OnCollisionEnter2D (Collision2D col)
    {
       if(col.gameObject.tag == "Coin")
       {
        // Destroy(col.gameObject);
        col.gameObject.SetActive(false);
        Vector2 randomPositionOnScreen = Camera.main.ViewportToWorldPoint(new Vector2(Random.value, Random.value));
        col.gameObject.transform.position = randomPositionOnScreen;
        col.gameObject.SetActive(true);
        randomPositionOnScreen = Camera.main.ViewportToWorldPoint(new Vector2(Random.value, Random.value));

        Instantiate(rb2D, randomPositionOnScreen, Quaternion.identity);
        
       }
    }
}

