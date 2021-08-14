using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpPower;
    [SerializeField] private AudioClip _walkClip;
    [SerializeField] private AudioClip _jumpClip;

    private Rigidbody2D rigidbody2D;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    private bool _ground;
    private bool _onJump;
    private float _dashValue;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetKey(KeyCode.LeftShift))
            _dashValue = 1.25f;
        else
            _dashValue = 1.0f;

        if (horizontal != 0)
        {
            if (!audioSource.isPlaying && _ground)
            {
                AudioPlay("walk");
            }

            animator.SetBool("onWalk", true);
            spriteRenderer.flipX = (horizontal < 0) ? true : false;
        }
        else
        {
            animator.SetBool("onWalk", false);

            if (_ground)
                audioSource.Stop();
        }

        Vector2 moveVec = new Vector2(horizontal * _moveSpeed * _dashValue * Time.deltaTime, 0);
        transform.Translate(moveVec);
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && _ground && !_onJump)
        {
            _onJump = true;
            animator.SetTrigger("onJump");
            audioSource.Stop();
            AudioPlay("jump");
            rigidbody2D.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
        }
    }

    private void AudioPlay(string clipName)
    {
        switch (clipName)
        {
            case "walk": audioSource.clip = _walkClip; audioSource.pitch = 1.5f; audioSource.volume = 0.9f; break;
            case "dash": audioSource.clip = _walkClip; audioSource.pitch = 1.7f; audioSource.volume = 1.0f; break;
            case "jump": audioSource.clip = _jumpClip; audioSource.pitch = 1.2f; audioSource.volume = 0.8f; break;
        }

        audioSource.Play();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            _ground = true;
            _onJump = false;
        }
        else
        {
            _ground = false;
        }
    }
}