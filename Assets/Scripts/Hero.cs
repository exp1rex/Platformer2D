using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hero : Entity
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private int lives = 5;
    [SerializeField] public int coins = 0;
    [SerializeField] private List<GameObject> coinsLength;
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private Transform groundCheck;
    private bool isGrounded = false;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private AudioSource audioSource;
    public AudioClip jumpSound;

    public List<GameObject> texts;
    public GameObject youWinText;

    bool gravity = true;

    public static Hero Instance { get; set; }
    private States State
    {
        get { return (States)anim.GetInteger("state"); }
        set { anim.SetInteger("state", (int)value); }
    }

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isGrounded) State = States.idle; 

        if (Input.GetButton("Horizontal"))
            Run();
        if (isGrounded && Input.GetButtonDown("Jump"))
            Jump();
        /*if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (gravity == true)
            {
                Physics2D.gravity = new Vector2();
                rb.gravityScale = 1;
                gravity = false;
            }
            else if (gravity == false)
            {
                rb.gravityScale = 5;
                gravity = true;
            }
        }*/
        if (rb.velocity.y < 0)
        {
            State = States.fall;
        }

        // Debug.Log(isGrounded);
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    // Движение игрока
    private void Run()
    {
        if (isGrounded) State = States.run;

        Vector3 dir = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);
        spriteRenderer.flipX = dir.x < 0.0f;
    }

    // Прыжок
    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        audioSource.PlayOneShot(jumpSound);
    }

    // Проверка земли под игроком
    private void CheckGround()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(groundCheck.position, 0.3f);
        isGrounded = collider.Length > 1;

        if (!isGrounded) State = States.jump;
    }

    // Получение урона
    public override void GetDamage()
    {
        lives -= 1;
        Debug.Log(lives);
        if (lives < 1)
        {
            transform.gameObject.SetActive(false);
            for (int i = 0; i < texts.Count; i++)
            {
                texts[i].SetActive(true);
            }
        }
    }

    public void AddCoin()
    {
        coins++;


        if (coins == coinsLength.Count)
        {
            youWinText.SetActive(true);
        }
    }
}

public enum States
{
    idle,
    run,
    jump,
    fall
}
