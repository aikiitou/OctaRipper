using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackController
{
    InputSystem_Actions isInput;
    Animator aAnimator;
    Rigidbody rb;

    private bool isWeakButton = false;
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
        isWeakButton = true;
        aAnimator.SetBool("bIsButton", true);
        aAnimator.SetTrigger("WeakAttackTrigger");
    }
    public void ReleaseWeakAttackButton()
    {
        isWeakButton = false;

        nButtonOnFrame = 0;

        aAnimator.SetBool("bIsButton", false);
        aAnimator.SetInteger("nButtonOnFrame", nButtonOnFrame);
    }
}
