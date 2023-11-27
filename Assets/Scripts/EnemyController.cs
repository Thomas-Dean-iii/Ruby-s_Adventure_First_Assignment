using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Movement
    public float speed;
    public bool vertical;
    public float changeTime = 3.0f;

    public ParticleSystem smokeEffect;
    public ParticleSystem FixedEffect;

    Rigidbody2D rigidbody2D;
    float timer;
    int direction = 1;
    bool broken = true;

    // Animation
    Animator animator;

    // Sounds
    AudioSource audioSource;
    public AudioClip FixedSound;
    public AudioClip EnemyHit;

    // Scoreing
    public GameObject scoreText;
    public GameObject score;

    private RubyController rubyController; // this line of code creates a variable called "rubyController" to store information about the RubyController script!


    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        GameObject rubyControllerObject = GameObject.FindWithTag("RubyController"); //this line of code finds the RubyController script by looking for a "RubyController" tag on Ruby

        if (rubyControllerObject != null)

        {

            rubyController = rubyControllerObject.GetComponent<RubyController>(); //and this line of code finds the rubyController and then stores it in a variable

            print("Found the RubyConroller Script!");

        }

        if (rubyController == null)

        {

            print("Cannot find GameController Script!");

        }
    }

    void Update()
    {
        //remember ! inverse the test, so if broken is true !broken will be false and return won’t be executed.
        if (!broken)
        {
            return;
        }

        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }

    void FixedUpdate()
    {
        //remember ! inverse the test, so if broken is true !broken will be false and return won’t be executed.
 
        Vector2 position = rigidbody2D.position;

        if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed * direction;
            animator.SetFloat("Move x", 0);
            animator.SetFloat("Move y", direction);
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed * direction;
            animator.SetFloat("Move x", direction);
            animator.SetFloat("Move y", 0);
        }

        rigidbody2D.MovePosition(position);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (!broken)
        {
            return;
        }

        RubyController player = other.gameObject.GetComponent<RubyController>();

        if (player != null)
        {
            player.ChangeHealth(-1);
            audioSource.PlayOneShot(EnemyHit);
        }
    }

    //Public because we want to call it from elsewhere like the projectile script
    public void Fix()
    {
        animator.SetTrigger("fixed");
        broken = false;
        smokeEffect.Stop();
        rigidbody2D.simulated = false;
        Instantiate(FixedEffect, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        audioSource.PlayOneShot(FixedSound);



        if (rubyController != null)
        {
            rubyController.ChangeScore(1);
        }
    }



}
