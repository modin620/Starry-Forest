using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Core")]
    [SerializeField] CapsuleCollider2D _defaultCollider;
    [SerializeField] BoxCollider2D _slidingCollider;

    [Header("Status")]
    public float Hp;
    float _maxHp;
    [SerializeField] float _jumpPower;
    [SerializeField] float FLY_UP_VALUE;
    [SerializeField] float FLY_DOWN_VALUE;
    [SerializeField] float GRAVITY_VALUE;
    [SerializeField] float DOWNHILL_VALUE;

    [Header("Audio")]
    [SerializeField] AudioClip _walkClip;
    [SerializeField] AudioClip _jumpClip;
    [SerializeField] AudioClip _slidingClip;
    [SerializeField] AudioClip _itemClip;
    [SerializeField] AudioClip _thronClip;
    [SerializeField] AudioClip _recoverClip;
    [SerializeField] AudioClip _doubleJumpClip;
    [SerializeField] AudioSource audioSourceFirst;
    [SerializeField] AudioSource audioSourceSecond;

    [Header("Effect")]
    [SerializeField] ParticleSystem _dustEffect;
    [SerializeField] ParticleSystem _itemEffect;
    [SerializeField] ParticleSystem _recoverEffect;

    Rigidbody2D rigidbody2D;
    Animator animator;
    SpriteRenderer spriteRenderer;

    bool _ground;
    bool _onJump;
    bool _onDownhill;
    bool _doSliding;
    bool _onFly;
    bool _onInvincibility;
    bool _stopOnceAudio;
    bool _onDoubleJump;

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
        Rest();
        Walk();
        Jump();
        Sliding();
        Fly();
        Dead();
    }

    private void Walk()
    {
        if (FloorManager.stop)
        {
            if (!_stopOnceAudio)
            {
                _stopOnceAudio = true;
                audioSourceFirst.Stop();
                audioSourceSecond.Stop();
            }
            return;
        }

        if (_ground)
        {
            animator.SetBool("doJump", false);
            animator.SetBool("doDoubleJump", false);

            if (!audioSourceFirst.isPlaying && !_doSliding)
                PlayAudio("walk");
        }
    }

    private void Jump()
    {
        if (FloorManager.stop)
            return;

        if (_onFly)
            return;

        RaycastHit2D hit;
        Vector2 rayVector = new Vector2 (transform.position.x - 0.5f, transform.position.y);
        hit = Physics2D.Raycast(rayVector, Vector2.down, 2.0f, LayerMask.GetMask("Platform"));

        if (Input.GetButton("Jump"))
        {
            if (_onDownhill  && !_ground)
            {
                if (!_onDoubleJump)
                {
                    Vector2 doubleJumpVec = new Vector2(rigidbody2D.velocity.x, 0.0f);
                    rigidbody2D.velocity = doubleJumpVec;
                    rigidbody2D.AddForce(Vector2.up * _jumpPower / 4, ForceMode2D.Impulse);
                    PlayAudio("doubleJump");
                    _onDoubleJump = true;
                }
                rigidbody2D.gravityScale = DOWNHILL_VALUE;
                animator.SetBool("doDoubleJump", true);
            }
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
        if (FloorManager.stop)
            return;

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

    private void Fly()
    {
        if (FloorManager.stop)
            return;

        if (!_onFly)
            return;

        if (Input.GetButton("Jump"))
        {
            // Need Animation, Audio
            rigidbody2D.gravityScale = FLY_UP_VALUE;
        }
        else
        {
            rigidbody2D.gravityScale = FLY_DOWN_VALUE;
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
            case "doubleJump":
                audioSourceFirst.Stop();
                audioSourceFirst.clip = _doubleJumpClip;
                audioSourceFirst.Play();
                break;
        }
    }

    private void Dead()
    {
        if (FloorManager.stop)
            return;

        if (Hp > 0)
            return;

        GameManager.instance.GameOver();
    }

    private void Rest()
    {
        if (!FloorManager.stop)
            return;

        animator.SetBool("doStanding", true);
        animator.SetBool("doJump", false);
        animator.SetBool("doSliding", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            rigidbody2D.gravityScale = GRAVITY_VALUE;
            _ground = true;
            _onJump = false;
            _onDownhill = false;
            _onDoubleJump = false;
            _onFly = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Fly")
        {
            _onFly = true;
            Destroy(collision.gameObject);
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
        if (Hp < _maxHp)
            Hp += recoverValue;

        _recoverEffect.Play();
        PlayAudio("recover");
    }
}