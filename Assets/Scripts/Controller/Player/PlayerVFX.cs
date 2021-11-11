using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFX : MonoBehaviour
{
    [Header("Particles")]
    [SerializeField] ParticleSystem _dustEffect;
    [SerializeField] ParticleSystem _takeItemEffect;
    [SerializeField] ParticleSystem _recoverEffect;
    [SerializeField] ParticleSystem _dandelionEffect;
    [SerializeField] ParticleSystem _dashEffect;

    public void PlayVFX(string clip)
    {
        switch (clip)
        {
            case Definition.VFX_DUST:
                _dustEffect.Play();
                break;
            case Definition.VFX_TAKE_ITEM:
                _takeItemEffect.Play();
                break;
            case Definition.VFX_RECOVER:
                _recoverEffect.Play();
                break;
            case Definition.VFX_DANDELION:
                _dandelionEffect.Play();
                break;
            case Definition.VFX_DASH:
                _dashEffect.Play();
                break;
        }
    }

    public void StopVFX(string clip)
    {
        switch (clip)
        {
            case Definition.VFX_DUST:
                _dustEffect.Stop();
                break;
            case Definition.VFX_TAKE_ITEM:
                _takeItemEffect.Stop();
                break;
            case Definition.VFX_RECOVER:
                _recoverEffect.Stop();
                break;
            case Definition.VFX_DANDELION:
                _dandelionEffect.Stop();
                break;
            case Definition.VFX_DASH:
                _dashEffect.Stop();
                break;
        }
    }

    [System.Obsolete]
    public void ChangeDashEffectColor(Color targetColor)
    {
        _dashEffect.startColor = targetColor;
    }
}
