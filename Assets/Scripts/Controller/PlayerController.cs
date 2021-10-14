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
    AudioManager audioManager;

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
    bool _onDoubleJump;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        audioManager = GameManager.instance.AudioManagerInstance;

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
            animator.SetBool("doStanding", false);
            animator.SetBool("doJump", false);
            animator.SetBool("doDoubleJump", false);

            audioManager.PlayWalkChannel();
        }
        else
        {
            audioManager.PauseWalkCahnnel();
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
                    audioManager.PlaySFX(Definition.DOWNHILL_CLIP);
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
                    audioManager.PlaySFX(Definition.JUMP_CLIP);
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
                audioManager.PlaySFX(Definition.SLIDING_CLIP);
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

    private void Dead()
    {
        if (_hp > 0)
            return;

        GameManager.instance.StageManagerInstance.GameOver();
    }

    private void Rest()
    {
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
            audioManager.PlaySFX(Definition.DANDELION_CLIP);
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
        audioManager.PlaySFX(clipName);
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
        audioManager.PlaySFX(Definition.ITEM_CLIP);
        _itemEffect.Play();
    }

    public void Recover(int recoverValue)
    {
        if (_hp < _maxHp)
            _hp += recoverValue;

        GameManager.instance.UIManagerInstance.heartInstance.CheckHeart();

        _recoverEffect.Play();
        audioManager.PlaySFX(Definition.RECOVER_CLIP);
    }
}