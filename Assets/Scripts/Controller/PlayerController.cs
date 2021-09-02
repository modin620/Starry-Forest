using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("[ Core ]")]
    [SerializeField] CapsuleCollider2D _defaultCollider;
    [SerializeField] CapsuleCollider2D _slidingCollider;

    [Header("[ Status ]")]
    public float Hp;
    float _maxHp;
    [SerializeField] float _jumpPower;

    [Header("[ Audio ]")]
    [SerializeField] AudioClip _walkClip;
    [SerializeField] AudioClip _jumpClip;
    [SerializeField] AudioClip _slidingClip;
    [SerializeField] AudioClip _itemClip;
    [SerializeField] AudioClip _thronClip;
    [SerializeField] AudioClip _recoverClip;
    [SerializeField] AudioSource audioSourceFirst;
    [SerializeField] AudioSource audioSourceSecond;

    [Header("[ Effect ]")]
    [SerializeField] ParticleSystem _dustEffect;
    [SerializeField] ParticleSystem _itemEffect;
    [SerializeField] ParticleSystem _recoverEffect;

    Rigidbody2D rigidbody2D;
    Animator animator;
    SpriteRenderer spriteRenderer;

    bool _ground;
    bool _onJump;
    bool _onDownhill;
    bool _onDownhillAudio;
    bool _doSliding;
    bool _onInvincibility;

    const float GRAVITY_VALUE = 1.5f;
    const float DOWNHILL_VALUE = 0.05f;

    private void Start()
    {
        _maxHp = Hp;

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
        Sliding();
        Dead();
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
        RaycastHit2D hit;
        Vector2 rayVector = new Vector2 (transform.position.x - 0.5f, transform.position.y);
        hit = Physics2D.Raycast(rayVector, Vector2.down, 2.0f, LayerMask.GetMask("Platform"));

        if (Input.GetButton("Jump"))
        {
            if (_onDownhill  && !_ground)
                rigidbody2D.gravityScale = DOWNHILL_VALUE;
        }

        if (Input.GetButtonUp("Jump"))
            rigidbody2D.gravityScale = GRAVITY_VALUE;

        if (hit.collider != null)
        {
            if (hit.collider.tag == "Platform")
            {
                if (Input.GetButtonDown("Jump"))
                {
                    if (_doSliding || _onJump)
                        return;

                    _ground = false;
                    _onJump = true;
                    Invoke("OnDownhill", 0.5f);

                    rigidbody2D.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
                    animator.SetBool("doJump", true);
                    PlayAudio("jump");
                }
            }
        }
    }

    private void OnDownhill()
    {
        _onDownhill = true;
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
            case "recover":
                audioSourceSecond.Stop();
                audioSourceSecond.clip = _recoverClip;
                audioSourceSecond.Play();
                break;
            case "thorn":
                audioSourceSecond.Stop();
                audioSourceSecond.clip = _thronClip; 
                audioSourceSecond.Play(); 
                break;
        }
    }

    private void Dead()
    {
        if (Hp > 0)
            return;

        GameManager.instance.GameOver();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            rigidbody2D.gravityScale = GRAVITY_VALUE;
            _ground = true;
            _onJump = false;
            _onDownhill = false;
        }
    }

    public void Damaged(float damage, string clipName)
    {
        if (_onInvincibility)
            return;

        StartCoroutine(OnInvincibility());

        Hp -= damage;
        GameManager.instance.PlayBloodEffect();
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

    public void Recover(float recoverValue)
    {
        Hp += Mathf.Clamp(recoverValue, 0, _maxHp);
        _recoverEffect.Play();
        PlayAudio("recover");
    }
}