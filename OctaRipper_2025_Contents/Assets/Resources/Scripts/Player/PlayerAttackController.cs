using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackController
{
    InputSystem_Actions isInput;
    Animator aAnimator;
    Rigidbody rb;

    private bool isWeakButton = false;
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
    }
    public void OnWeakAttackButton()
    {
        if(isAnimataion == false)
        {
            isWeakButton = true;
            aAnimator.SetBool("bIsButton", true);
            aAnimator.SetTrigger("weakAttackTrigger");

            aAnimator.SetInteger("nButtonOnFrame", 0);
        }
        else
        {
            aAnimator.SetTrigger("comboTrigger");
        }
    }
    public void ReleaseWeakAttackButton()
    {
        isWeakButton = false;
        nButtonOnFrame = 0;

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
    }
}
