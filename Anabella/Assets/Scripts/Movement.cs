using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    public float movementSpeed = 3.0f;

    Vector2 movement = new Vector2();

    Rigidbody2D rb2D;

    Animator animator;

    string animationState = "AnimationState";
    private int souls = 0;
    public Text soulsText;
    public Text levelComplete;
    public Text playerLivesText;
    public Text youWin;
    public Text buttonPrompt;
    public Text gameOver;
    public Text gameOver2;
    public int playerLives;
    public int soulGoal;
    public Button closeButton;
    int hit = 0;



    enum CharStates
    {
        idle = 0,
        rightwalk= 1,
        leftwalk = 2
    }


    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        soulsText.text = "Souls: " + souls;
        playerLivesText.text = "Lives: " + playerLives;

    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
        if(hit == playerLives)
        {
            gameOver.text = "Game Over";
            gameOver2.text = "Press the R key to Restart Game";
            Time.timeScale = 0;
            if (Input.GetKeyDown(KeyCode.R))
            {
                Time.timeScale = 1;
                Application.LoadLevel("LevelOne");
            }
            // Destroy(col.gameObject);
        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        if(souls == soulGoal)
        {
            levelComplete.text = "Level Complete";
            youWin.text = "You Win!";
            buttonPrompt.text = "Press escape or click the button to continue";
            closeButton.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void closeGame()
    {
        Application.Quit();
    }


    private void FixedUpdate()
    {
        MoveCharacter();
        /*if(souls == soulGoal){
            levelComplete.text = "Level Complete";
        }*/

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


    private void MoveCharacter()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement.Normalize();

        rb2D.velocity = movement * movementSpeed;

    }

        void OnCollisionEnter2D (Collision2D col)
     {
        if(col.gameObject.tag == "Enemy")
        {
            hit += 1;
            playerLivesText.text = "Lives: " + (playerLives-hit);
        }

       if(col.gameObject.tag == "Coin")
       {
        // Destroy(col.gameObject);
        col.gameObject.SetActive(false);
        Vector2 randomPositionOnScreen = Camera.main.ViewportToWorldPoint(new Vector2(Random.value, Random.value));
        col.gameObject.transform.position = randomPositionOnScreen;
        col.gameObject.SetActive(true);


        // Instantiate(col.gameObject, randomPositionOnScreen, Quaternion.identity);
        souls = souls + 1;
        soulsText.text = "Souls: " + souls + "/" + soulGoal;

       }
     }

}