using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("[ Core ]")]
    [SerializeField] private GameManager gm;

    [Header("[ Audio ]")]
    [SerializeField] private AudioClip _walkClip;
    [SerializeField] private AudioClip _jumpClip;
    [SerializeField] private AudioClip _itemClip;
    [SerializeField] private AudioClip _thronClip;
    [SerializeField] private AudioSource audioSourceFirst;
    [SerializeField] private AudioSource audioSourceSecond;

    [Header("[ Status ]")]
    public float Hp;
    [SerializeField] private float _jumpPower;
    [SerializeField, Range(0, 1)] private float _doubleJumpPower;

    [Header("[ Effect ]")]
    [SerializeField] private ParticleSystem _dustEffect;
    [SerializeField] private ParticleSystem _itemEffect;

    private Rigidbody2D rigidbody2D;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool _ground;
    private bool _onJump;
    private bool _onDoubleJump;
    private bool _onInvincibility;

    private void Start()
    {
        SetComponents(); // allocate component in the variable
    }

    private void SetComponents()
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
                PlayAudio("walk");
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
            PlayAudio("jump");
        }
    }

    private void ExitDoubleJump()
    {
        _onDoubleJump = true;
    }

    private void PlayAudio(string clipName)
    {
        audioSourceFirst.Stop();

        switch (clipName)
        {
            case "walk": audioSourceFirst.clip = _walkClip; audioSourceFirst.Play(); break;
            case "jump": audioSourceFirst.clip = _jumpClip; audioSourceFirst.Play(); break;
            case "item": audioSourceSecond.clip = _itemClip; audioSourceSecond.Play(); break;
            case "thorn": audioSourceSecond.clip = _thronClip; audioSourceSecond.Play(); break;
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

    public void Damaged(float damage, string clipName)
    {
        if (_onInvincibility)
            return;

        StartCoroutine(OnInvincibility());

        Hp -= damage;
        gm.PlayBloodEffect();
        PlayAudio(clipName);
    }

    IEnumerator OnInvincibility()
    {
        _onInvincibility = true;
        spriteRenderer.color = new Color(1, 1, 1, 0.8f);

        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = new Color(1, 1, 1, 1f);
        _onInvincibility = false;
    }


    public void TakeItem()
    {
        PlayAudio("item");
        _itemEffect.Play();
    }
}