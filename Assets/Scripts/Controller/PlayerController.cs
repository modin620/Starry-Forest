using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("[ Core ]")]
    [SerializeField] GameManager gm;
    [SerializeField] CapsuleCollider2D _defaultCollider;
    [SerializeField] CapsuleCollider2D _slidingCollider;

    [Header("[ Status ]")]
    public float Hp;
    [SerializeField] float _jumpPower;
    [SerializeField, Range(0, 1)] float _doubleJumpPower;

    [Header("[ Audio ]")]
    [SerializeField] AudioClip _walkClip;
    [SerializeField] AudioClip _jumpClip;
    [SerializeField] AudioClip _slidingClip;
    [SerializeField] AudioClip _itemClip;
    [SerializeField] AudioClip _thronClip;
    [SerializeField] AudioSource audioSourceFirst;
    [SerializeField] AudioSource audioSourceSecond;


    [Header("[ Effect ]")]
    [SerializeField] ParticleSystem _dustEffect;
    [SerializeField] ParticleSystem _itemEffect;

    Rigidbody2D rigidbody2D;
    Animator animator;
    SpriteRenderer spriteRenderer;

    bool _ground;
    bool _onJump;
    bool _doSliding;
    bool _onDoubleJump;
    bool _onInvincibility;

    const float GRAVITY_VALUE = 1.5f;
    const float DOWNHILL_VALUE = 0.1f;

    private void Start()
    {
        SetComponents(); // allocate component in the variable
    }

    private void SetComponents()
    {
        if (gm == null)
            gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Walk();
        Jump();
        Sliding();
    }

    private void Walk()
    {
        if (_ground)
        {
            animator.SetBool("doJump", false);

            if (!audioSourceFirst.isPlaying && !_doSliding)
                PlayAudio("walk");
        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (_onDoubleJump)
                return;

            if (_doSliding)
                return;

            rigidbody2D.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
            _ground = false;
            _onJump = true;
            animator.SetBool("doJump", true);
            PlayAudio("jump");

            if (Input.GetButton("Jump"))
                rigidbody2D.gravityScale = DOWNHILL_VALUE;
            else
                rigidbody2D.gravityScale = GRAVITY_VALUE;
        }
    }

    private void Sliding()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (!_ground)
                return;

            if (!_doSliding)
            {
                PlayAudio("sliding");
                var effectController = _dustEffect.main;
                effectController.startSize = 0.8f;
            }

            _doSliding = true;
            animator.SetBool("doSliding", true);
            _defaultCollider.enabled = false;
            _slidingCollider.enabled = true;
        }
        else
        {
            _doSliding = false;
            animator.SetBool("doSliding", false);

            var effectController = _dustEffect.main;
            effectController.startSize = 0.0f;

            if (!_defaultCollider.isActiveAndEnabled)
                _defaultCollider.enabled = true;

            if (_slidingCollider.isActiveAndEnabled)
                _slidingCollider.enabled = false;
        }
    }

    private void ExitDoubleJump()
    {
        _onDoubleJump = true;
    }

    private void PlayAudio(string clipName)
    {

        switch (clipName)
        {
            case "walk":
                audioSourceFirst.Stop();
                audioSourceFirst.clip = _walkClip; 
                audioSourceFirst.Play(); 
                break;
            case "jump":
                audioSourceFirst.Stop();
                audioSourceFirst.clip = _jumpClip; 
                audioSourceFirst.Play(); 
                break;
            case "sliding":
                audioSourceFirst.Stop();
                audioSourceFirst.clip = _slidingClip; 
                audioSourceFirst.Play(); 
                break;
            case "item":
                audioSourceSecond.Stop();
                audioSourceSecond.clip = _itemClip;
                audioSourceSecond.Play(); 
                break;
            case "thorn":
                audioSourceSecond.Stop();
                audioSourceSecond.clip = _thronClip; 
                audioSourceSecond.Play(); 
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            rigidbody2D.gravityScale = GRAVITY_VALUE;
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

        yield return new WaitForSeconds(1.0f);
        spriteRenderer.color = new Color(1, 1, 1, 1f);
        _onInvincibility = false;
    }


    public void TakeItem()
    {
        PlayAudio("item");
        _itemEffect.Play();
    }
}