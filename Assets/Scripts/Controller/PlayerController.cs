using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _jumpPower;
    [SerializeField, Range(0, 1)] private float _doubleJumpPower;
    [SerializeField] private AudioClip _walkClip;
    [SerializeField] private AudioClip _jumpClip;
    [SerializeField] private AudioClip _itemClip;
    [SerializeField] private ParticleSystem _dustEffect;
    [SerializeField] private AudioSource audioSourceFirst;
    [SerializeField] private AudioSource audioSourceSecond;
    [SerializeField] private ParticleSystem _itemEffect;
    [SerializeField] private GameManager gm;

    public float Hp;

    private Rigidbody2D rigidbody2D;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool _ground;
    private bool _onJump;
    private bool _onDoubleJump;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
            animator.SetBool("doJump", false);

            if (!audioSourceFirst.isPlaying)
                AudioPlay("walk");
        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (_onDoubleJump)
                return;

            if (_onJump)
            {
                rigidbody2D.AddForce(Vector2.up * _jumpPower * _doubleJumpPower, ForceMode2D.Impulse);
                _onDoubleJump = true;
            }
            else
            {
                rigidbody2D.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
                Invoke("ExitDoubleJump", 1.0f);
            }

            _ground = false;
            _onJump = true;
            animator.SetBool("doJump", true);
            AudioPlay("jump");
        }
    }

    private void ExitDoubleJump()
    {
        _onDoubleJump = true;
    }

    private void AudioPlay(string clipName)
    {
        audioSourceFirst.Stop();

        switch (clipName)
        {
            case "walk": audioSourceFirst.clip = _walkClip; audioSourceFirst.Play(); break;
            case "jump": audioSourceFirst.clip = _jumpClip; audioSourceFirst.Play(); break;
            case "item": audioSourceSecond.clip = _itemClip; audioSourceSecond.Play(); break;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            _ground = true;
            _onJump = false;
            CancelInvoke("ExitDoubleJump");
            _onDoubleJump = false;
        }
    }

    public void Damaged(float damage)
    {
        Hp -= damage;
        gm.PlayBloodEffect();
    }

    public void TakeItem()
    {
        AudioPlay("item");
        _itemEffect.Play();
    }
}