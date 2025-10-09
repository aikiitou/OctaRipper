using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;

public class PlayerAttackController
{
    Animator aAnimator;
    Rigidbody rb;

    const float FREEZE_TIME = 1.0f;

    private bool isWeakButton = false;
    private bool isStrongButton = false;
    private bool isAnimataion = false;
    private int nButtonOnFrame = 0;
    private float fCurrentDamage = 0f;

    GameObject gRightChargeEffect;
    GameObject gLeftChargeEffect;
    GameObject[] gAttackColliders;
    PlayerAttackColliderController[] cPlayerAttackColliderControllers;
    public PlayerAttackController(Rigidbody _rb, Animator _animator, GameObject[] attackColliders, GameObject _rightChargeEffect, GameObject _leftChargeEffect)  
    {
        this.aAnimator = _animator;
        this.rb = _rb;
        this.gAttackColliders = attackColliders;
        this.gRightChargeEffect = _rightChargeEffect;
        this.gLeftChargeEffect = _leftChargeEffect;
        cPlayerAttackColliderControllers = new PlayerAttackColliderController[gAttackColliders.Length];
        for(int i= 0;i < cPlayerAttackColliderControllers.Length; i++)
        {
            cPlayerAttackColliderControllers[i] = gAttackColliders[i].GetComponent<PlayerAttackColliderController>();
        }
    }
    public void UpdataAttack(int _fpsDiff)
    {
        if (isWeakButton == true)
        {
            nButtonOnFrame += _fpsDiff;
            aAnimator.SetInteger("nButtonOnFrame", nButtonOnFrame);
            if(nButtonOnFrame >= 20)
            {
                if(gRightChargeEffect.activeInHierarchy == false)
                {
                    OnRightChargeEffect();
                }
            }
        }
        if (isStrongButton == true)
        {
            nButtonOnFrame += _fpsDiff;
            aAnimator.SetInteger("nButtonOnFrame", nButtonOnFrame);
            if (nButtonOnFrame >= 25)
            {
                if (gRightChargeEffect.activeInHierarchy == false || gLeftChargeEffect.activeInHierarchy == false)
                {
                    OnBothChargeEffect();
                }
            }   
        }
    }
    public void OnWeakAttackButton()
    {
        if(isAnimataion == false && isStrongButton == false)
        {
            isWeakButton = true;
            aAnimator.SetBool("bIsButton", true);
            aAnimator.SetTrigger("weakAttackTrigger");

            nButtonOnFrame = 0;
            aAnimator.SetInteger("nButtonOnFrame", 0);
        }
        else if(isAnimataion == true)
        {
            aAnimator.SetTrigger("weakComboTrigger");
        }
    }
    public void ReleaseWeakAttackButton()
    {
        isWeakButton = false;

        aAnimator.SetBool("bIsButton", false);
    }
    public void OnStrongAttackButton()
    {
        if(isWeakButton == false && isAnimataion == false)
        {
            isStrongButton = true;
            aAnimator.SetBool("bIsButton", true);
            aAnimator.SetTrigger("strongAttackTrigger");

            nButtonOnFrame = 0;
            aAnimator.SetInteger("nButtonOnFrame", 0);
        }
        else if (isAnimataion == true)
        {
            aAnimator.SetTrigger("strongComboTrigger");
        }
    }
    public void ReleaseStrongAttackButton()
    {
        isStrongButton = false;

        aAnimator.SetBool("bIsButton", false);
    }
    public void AnimationStr()
    {
        isAnimataion = true;
    }
    public void AnimationEnd()
    {
        isAnimataion = false;
        aAnimator.ResetTrigger("weakAttackTrigger");
        aAnimator.ResetTrigger("strongAttackTrigger");
    }
    public void SetDamage(float _damage)
    {
        fCurrentDamage = _damage;
    }
    public void OnAttackCollider(float _knockBack)
    {
        foreach (GameObject obj in gAttackColliders)
        { 
            obj.SetActive(true);
        }
        foreach (PlayerAttackColliderController colliderController in cPlayerAttackColliderControllers)
        {
            colliderController.SetAttackInfo(fCurrentDamage, FREEZE_TIME, _knockBack);
        }
    }
    public void DisAttackCollider()
    {
        foreach (GameObject obj in gAttackColliders)
        {
            obj.SetActive(false);
        }
        foreach (PlayerAttackColliderController colliderController in cPlayerAttackColliderControllers)
        {
            colliderController.ResetAttackInfo();
        }
    }
    public void OnRightChargeEffect()
    {
        gRightChargeEffect.SetActive(true);
    }
    public void DisRightChargeEffect()
    {
        gRightChargeEffect.SetActive(false);
    }
    public void OnBothChargeEffect()
    {
        gRightChargeEffect.SetActive(true);
        gLeftChargeEffect.SetActive(true);
    }
    public void DisBothChargeEffect()
    {
        gRightChargeEffect.SetActive(false);
        gLeftChargeEffect.SetActive(false);
    }
}
