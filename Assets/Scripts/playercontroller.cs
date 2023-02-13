using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class playercontroller : MonoBehaviour
{
    //varible for rigidbody2d 
    private Rigidbody2D rb;

    //Values jumping left right
    [SerializeField] private float jump = 10f;
    [SerializeField] private int left = -3;
    [SerializeField] private int right = +3;
    [SerializeField] private float time = 0.4f;

    //varible for animator
    private Animator anim;
    
    //Diffrent States
    //stop = 0 
    //run = 1
    //jumping =2

    private enum State {stop, run, jump, fall}

    private State state = State.stop;
    //varible for collider
    private Collider2D col;
    [SerializeField] private LayerMask ground;

    public int food = 0;
   [SerializeField] private Text count_text;
    public GameObject level_complete;
    [SerializeField] private AudioSource jumpingsound;
    [SerializeField] private AudioSource diesound;
    [SerializeField] private AudioSource collectsound;
    [SerializeField] private AudioSource backroundsound;
    public GameObject StartMessage;

    private void Start()
    {
        Elements();
        level_complete.SetActive(false);
        backroundsound.Play();
        
    }
    private void Update()
    {
        Animations();
        anim.SetInteger("state", (int)state);
        Mechanics();

        //if (rb.velocity == Vector2.right)
        //{
        //    StartMessage.SetActive(false);
        //}

    }
    private void Mechanics()
    {
        float direction = Input.GetAxis("Horizontal");
        if (direction < 0)
        {
            rb.velocity = new Vector2(left, rb.velocity.y); //to the left is  -
            transform.localScale = new Vector2(-1, 1);
            //anim.SetTrigger("run_start");
            anim.SetBool("running", true);

        }
        else if (direction > 0)
        {
            rb.velocity = new Vector2(right, rb.velocity.y); //to the right is +
            transform.localScale = new Vector2(1, 1);
            anim.SetBool("running", true);
            //anim.SetTrigger("run_start");
        }
        // else{ 
        //     //anim.SetBool("running", false);
        //     // anim.ResetTrigger("run_start");
        // }
        //and is &&
        if (Input.GetButtonDown("Jump") && col.IsTouchingLayers(ground))
        {
            jumpingsound.Play();
            rb.velocity = new Vector2(rb.velocity.x, jump);
            //rb.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
            //anim.SetBool("jump", true);
            state = State.jump;
        }
    }
    private void Animations()
    {
        if (state == State.jump)
        {
            if (rb.velocity.y < 0.1f)
            {
                state = State.fall;
            }
        }
        else if (state == State.fall)
        {

            if (col.IsTouchingLayers(ground))
            {
                state = State.stop;
            }
        }
        else if (Mathf.Abs(rb.velocity.x) > 1f)
        {
            state = State.run;
        }
        //if not running states = stop/idle
        else
        {
            state = State.stop;
        }
    }
    private void Elements()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    //For if player hits a trap.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Getting the spike collision tags
        if (collision.gameObject.CompareTag("spike"))
        {
            Death();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "cherry")
        {
            Destroy(collision.gameObject);
            collectsound.Play();
            food += 1;
            count_text.text = food.ToString();
        }

        if(collision.tag == "house" && food == 3)
        {
            level_complete.SetActive(true);
            rb.bodyType = RigidbodyType2D.Static;
            SceneManager.LoadScene("StartScreen");
        } 

    }

    private void Death()
    {
        //Trigger animation death
        anim.SetTrigger("death");
        //Makes the rb static in this case rb is player.
        rb.bodyType = RigidbodyType2D.Static;
        diesound.Play();
        //Runs after x amount seconds.
        Invoke("restart", time);
    }

    //Restart Level when dead 
    private void restart()
    {
        //Using UnityEngine.Scenemangment; namespace.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Message()
    {
        //if 3 food collected level = complete.
        if (food == 3)
        {
            //if player has collected all food
            //add a message to player that he must take the food to the house.
            
        }
    }
}

