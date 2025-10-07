using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackController
{
    InputSystem_Actions isInput;
    Animator aAnimator;
    Rigidbody rb;

    private bool isWeakButton = false;
    private bool isStrongButton = false;
    private bool isAnimataion = false;
    private int nButtonOnFrame = 0;
    public PlayerAttackController(Rigidbody _rb , Animator _animator)
    {
        this.aAnimator = _animator;
        this.rb = _rb;
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
}
