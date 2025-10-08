using UnityEngine;

public class PlayerAttackController
{
    Animator aAnimator;
    Rigidbody rb;

    private bool isWeakButton = false;
    private bool isStrongButton = false;
    private bool isAnimataion = false;
    private int nButtonOnFrame = 0;
    private float fCurrentDamage = 0f;

    GameObject[] gAttackColliders;
    PlayerAttackColliderController[] cPlayerAttackColliderControllers;
    public PlayerAttackController(Rigidbody _rb , Animator _animator, GameObject[] attackColliders)  
    {
        this.aAnimator = _animator;
        this.rb = _rb;
        this.gAttackColliders = attackColliders;
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
        }
        if (isStrongButton == true)
        {
            nButtonOnFrame += _fpsDiff;
            aAnimator.SetInteger("nButtonOnFrame", nButtonOnFrame);
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
            colliderController.SetAttackInfo(fCurrentDamage, 1.0f, _knockBack);
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
}
