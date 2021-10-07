using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Core")]
    [SerializeField] CapsuleCollider2D _defaultCollider;
    [SerializeField] BoxCollider2D _slidingCollider;

    [Header("Status")]
    public int _hp;
    public int _maxHp;
    [SerializeField] float _jumpPower;
    [SerializeField] float _flyTime;
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
    [SerializeField] AudioClip _dandelionClip;
    [SerializeField] AudioSource audioSourceFirst;
    [SerializeField] AudioSource audioSourceSecond;

    [Header("Effect")]
    [SerializeField] ParticleSystem _dustEffect;
    [SerializeField] ParticleSystem _itemEffect;
    [SerializeField] ParticleSystem _recoverEffect;
    [SerializeField] ParticleSystem _dandelionEffect;
    [SerializeField] SpriteRenderer _dandelionBuds;

    Rigidbody2D rigidbody2D;
    Animator animator;
    SpriteRenderer spriteRenderer;

    float _flyCurrentTime;

    bool _ground;
    bool _onJump;
    bool _onDownhill;
    bool _doSliding;
    bool _onFly;
    bool _onInvincibility;
    bool _onRest;
    bool _onDoubleJump;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        SetHp();
    }

    void SetHp()
    {
        _hp = GameManager.instance.LoadHp();
        _maxHp = GameManager.instance.LoadMaxHp();

        GameManager.instance.UIManagerInstance.heartInstance.CheckHeart();
    }

    private void Update()
    {
        if (GameManager.instance.StageManagerInstance.end)
        {
            Rest();
        }
        else
        {
            Walk();
            Jump();
            Sliding();
            Fly();
            Dead();
        }
    }

    private void Walk()
    {
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
        if (!_onFly)
        {
            if (_flyCurrentTime != 0) 
                _flyCurrentTime = 0;

            animator.SetBool("doFly", false);
            if (_dandelionEffect.isPlaying)
                _dandelionEffect.Stop();

            return;
        }

        animator.SetBool("doFly", true);
        if (!_dandelionEffect.isPlaying)
            _dandelionEffect.Play();

        _flyCurrentTime += Time.deltaTime;
        _dandelionBuds.color = new Color(1, 1, 1, 1 - _flyCurrentTime / _flyTime);

        if (_flyCurrentTime >= _flyTime)
        {
            _onFly = false;
            rigidbody2D.gravityScale = FLY_DOWN_VALUE;
            return;
        }

        if (Input.GetButton("Jump"))
        {
            rigidbody2D.gravityScale = FLY_UP_VALUE;
        }
        else
            rigidbody2D.gravityScale = FLY_DOWN_VALUE;
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
            case "dandelion":
                audioSourceSecond.Stop();
                audioSourceSecond.clip = _dandelionClip;
                audioSourceSecond.Play();
                break;
        }
    }

    private void Dead()
    {
        if (_hp > 0)
            return;

        GameManager.instance.StageManagerInstance.GameOver();
    }

    private void Rest()
    {
        if (!_onRest)
        {
            _onRest = true;
            audioSourceFirst.Stop();
            audioSourceSecond.Stop();

            animator.SetBool("doStanding", true);
            animator.SetBool("doJump", false);
            animator.SetBool("doSliding", false);
        }
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
            PlayAudio("dandelion");
            Destroy(collision.gameObject);
        }
    }

    public void Damaged(int damage, string clipName)
    {
        if (_onInvincibility)
            return;

        StartCoroutine(OnInvincibility());

        _hp -= damage;
        GameManager.instance.UIManagerInstance.BloodInstance.PlayBlood();
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

    public void Recover(int recoverValue)
    {
        if (_hp < _maxHp)
            _hp += recoverValue;

        GameManager.instance.UIManagerInstance.heartInstance.CheckHeart();

        _recoverEffect.Play();
        PlayAudio("recover");
    }
}