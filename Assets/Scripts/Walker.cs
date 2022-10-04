using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : Entity
{
    private float speed = 3.5f;
    private Vector3 dir;
    private SpriteRenderer sprite;
    public Transform groundCheck;

    public AudioSource audioSource;
    public AudioClip hit;

    private void Start()
    {
        dir = transform.right;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position + groundCheck.up * .1f + groundCheck.right * dir.x * .7f, .1f);

        if (colliders.Length > 0) dir *= -1f;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir * speed, Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Hero.Instance.gameObject)
        {
            Hero.Instance.GetDamage();
            audioSource.PlayOneShot(hit);
        }
    }
}
