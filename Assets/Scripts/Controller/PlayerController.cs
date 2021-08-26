using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _hp;
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

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        Walk();
        Jump();
    }

    private void Walk()
    {
        if (_ground)
        {
            if (!audioSource.isPlaying)
                AudioPlay("walk");
        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && _ground)
        {
            _ground = false;
            animator.SetTrigger("onJump");
            AudioPlay("jump");
            rigidbody2D.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
        }

        if (Input.GetButtonDown("Jump"))
        {
            _onJump = true;

        }
    }

    private void AudioPlay(string clipName)
    {
        audioSource.Stop();

        switch (clipName)
        {
            case "walk": audioSource.clip = _walkClip; audioSource.pitch = 1.5f; audioSource.volume = 0.6f; break;
            case "jump": audioSource.clip = _jumpClip; audioSource.pitch = 1.0f; audioSource.volume = 0.8f; break;
        }

        audioSource.Play();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            _ground = true;
        }
    }

    public void Damaged(float damage)
    {
        _hp -= damage;
    }
}