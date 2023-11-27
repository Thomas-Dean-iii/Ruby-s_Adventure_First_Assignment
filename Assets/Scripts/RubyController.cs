using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class RubyController : MonoBehaviour
{
    // Movement
    public float speed = 3.0f;

    // Health
    public int maxHealth = 5;
    public float timeInvincible = 2.0f;
    public int health { get { return currentHealth; } }
    int currentHealth;
    public ParticleSystem hitParticle;
    public Transform respawnPosition;
    
    // Projectile
    public GameObject projectilePrefab;

    // Audio
    public AudioClip throwSound;
    public AudioClip hitSound;

    // Invincibility
    bool isInvincible;
    float invincibleTimer;

    // Movement
    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    // Scoreing
    public TextMeshProUGUI scoreText;
    public int score;

    // GameOver and Win Screen
    public GameObject gameOverText;
    public GameObject YouWinText;
    public bool gameOver = false;

    // Animation
    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    // Audio
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        // Movement
        rigidbody2d = GetComponent<Rigidbody2D>();
        
        // Animation
        animator = GetComponent<Animator>();

        // Health
        invincibleTimer = -1.0f;
        currentHealth = maxHealth;

        // Audio
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Movement
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }


        // Animation
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        // Health
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        // Projectile
        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }

        // Dialogue
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NPC character = hit.collider.GetComponent<NPC>();
                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
        }

        if (score == 3)
        {
            YouWinText.SetActive(true);
            
           // gameOverText.text = "You Won! Press R to Restart!";
        }

        if (currentHealth == 0)
        {
            gameOverText.SetActive(true);
            speed = 0f;
           // gameOverText.text = "You lost! Press R to Restart!";
        }


        if (Input.GetKey(KeyCode.R))

        {

            if (gameOver == true)

            {

                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // this loads the currently active scene

            }
             
        }
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;

            animator.SetTrigger("Hit");
            audioSource.PlayOneShot(hitSound);

            Instantiate(hitParticle, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        if (currentHealth == 0)
            gameOver = true;
        // gameOverText.text = "You lost! Press R to Restart!";
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }

    public void ChangeScore(int amount)
    {
        score += 1;
        scoreText.text = "Fixed Robots: " + score.ToString(); 
        
    }





    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");

        audioSource.PlayOneShot(throwSound);
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
