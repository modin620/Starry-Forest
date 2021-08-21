using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool OnDialog;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpPower;
    [SerializeField] private AudioClip _walkClip;
    [SerializeField] private AudioClip _jumpClip;
    [SerializeField] private ParticleSystem _dustEffect;

    private Rigidbody2D rigidbody2D;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    private bool _ground;
    private bool _onJump;
    private float _dashValue = 1.0f;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        Jump();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");

        if (OnDialog)
        {
            animator.SetBool("onWalk", false);
            audioSource.Stop();
            horizontal = 0.0f;
            var main = _dustEffect.main;
            main.startSize = 0.0f;
            return;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (_dashValue != 1.25f)
            {
                _dashValue = 1.25f;
                var main = _dustEffect.main;
                main.startSize = 0.6f;
            }
        }
        else
        {
            if (_dashValue != 1.0f)
            {
                _dashValue = 1.0f;
                var main = _dustEffect.main;
                main.startSize = 0.0f;
            }
        }

        Vector2 moveVec = new Vector2(horizontal * _moveSpeed * _dashValue * Time.deltaTime, 0);
        transform.Translate(moveVec);

        if (Input.GetButton("Horizontal"))
        {
            if (!audioSource.isPlaying && _ground)
                AudioPlay("walk");

            animator.SetBool("onWalk", true);
            spriteRenderer.flipX = (horizontal < 0) ? true : false;
        }
        else
        {
            animator.SetBool("onWalk", false);
            if (_ground && !_onJump)
                audioSource.Stop();
        }
    }

    private void Jump()
    {
        if (OnDialog)
            return;

        if (Input.GetButtonDown("Jump"))
        {
            if (!_ground || _onJump)
                return;

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
            if (collision.contacts[0].normal.y > 0.7f)
            {
                _ground = true;
                _onJump = false;
            }
        }
        else
        {
            _ground = false;
        }
    }
}