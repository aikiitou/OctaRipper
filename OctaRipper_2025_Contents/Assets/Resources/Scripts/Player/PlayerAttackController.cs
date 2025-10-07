using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackController
{
    InputSystem_Actions isInput;
    Animator aAnimator;
    Rigidbody rb;

    public PlayerAttackController(Animator _animator,Rigidbody _rb)
    {
        this.aAnimator = _animator;
        this.rb = _rb;
    }
    
    public void WeakAttack()
    {
        aAnimator.SetBool("bIsAttack", true);
    }
}
