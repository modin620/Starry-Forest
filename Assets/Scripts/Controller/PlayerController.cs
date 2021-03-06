using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Status")]
    public int _hp;
    public int _maxHp;
    bool _onInvincibility;

    SpriteRenderer spriteRenderer;
    AudioManager audioManager;
    PlayerAnimation playerAnim;
    PlayerVFX playerVFX;


    void Awake()
    {
        AllocateComponent();
    }

    void AllocateComponent()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnim = GetComponent<PlayerAnimation>();
        playerVFX = GetComponent<PlayerVFX>();
    }

    void Start()
    {
        audioManager = GameManager.instance.AudioManagerInstance;

        SetHp();
    }

    void Update()
    {
        Dead();
    }

    void SetHp()
    {
        _hp = GameManager.instance.LoadHp();
        _maxHp = GameManager.instance.LoadMaxHp();

        GameManager.instance.UIManagerInstance.heartInstance.CheckHeart();
    }

    void Dead()
    {
        if (_hp > 0)
            return;

        GameManager.instance.StageManagerInstance.GameOver();
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
        playerVFX.PlayVFX(Definition.VFX_TAKE_ITEM);
    }

    public void Recover(int recoverValue)
    {
        if (_hp < _maxHp)
            _hp += recoverValue;

        GameManager.instance.UIManagerInstance.heartInstance.CheckHeart();

        audioManager.PlaySFX(Definition.RECOVER_CLIP);
        playerVFX.PlayVFX(Definition.VFX_RECOVER);
    }
}