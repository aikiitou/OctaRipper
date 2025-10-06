using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    InputSystem_Actions isInput;
    Animator aAnimator;
    Rigidbody rb;

    //プレイヤームーブコントローラー用変数
    [Header("プレイヤーの移動のスピード")]
    [SerializeField]
    private float fSpeed;
    [Header("プレイヤーの移動時の傾くスピード")]
    [SerializeField]
    private float fAngleSpeed;
    [Header("プレイヤーの回避時の移動距離")]
    [SerializeField]
    private float fDodgeDistance;
    [Header("回避のクールタイム時間")]
    [SerializeField]
    private float fDodgeCoolTimeValue;
    [Header("アニメーションの速度")]
    [SerializeField]
    private float fAnimSpeed;

    [Header("最大体力")]
    [SerializeField]
    private float fMaxLif;

    PlayerMoveController moveController;
    LifeController cLifeController;

    List<Material> materials = new List<Material>();
    private void OnEnable()
    {
        aAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        moveController = new PlayerMoveController(rb, aAnimator, fSpeed, fAnimSpeed, fDodgeDistance, fDodgeCoolTimeValue, fAnimSpeed);
        isInput = new InputSystem_Actions();
        isInput.Enable();
        //インプットシステムに関数の追加
        isInput.Player.Move.performed += Move;
        isInput.Player.Move.canceled += Stop;
        isInput.Player.Dodge.started += Dodge;
        isInput.Player.Look.performed += Look;
    }
    private void OnDisable()
    {
        isInput.Disable();
        //インプットシステムに関数の解除
        isInput.Player.Move.performed -= Move;
        isInput.Player.Move.canceled -= Stop;
        isInput.Player.Dodge.started -= Dodge;
        isInput.Player.Look.performed -= Look;
    }
    private void Start()
    {
        cLifeController = GetComponent<LifeController>();

        cLifeController.SetLifePoint(fMaxLif);
        cLifeController.SetInvincible(false);

        Renderer[] renderers = transform.GetComponentsInChildren<MeshRenderer>();
        foreach (Renderer ren in renderers)
        {
            foreach (Material mat in ren.materials)
            {
                materials.Add(mat);
            }
        }

        foreach (Material mat in materials)
        {
            mat.SetFloat("_Rate", 1);
        }

    }

    private void FixedUpdate()
    {
        moveController.FixedUpdateMove(transform);
    }

    private void Move(InputAction.CallbackContext _context)
    {
        Vector2 input = _context.ReadValue<Vector2>();
        moveController.Move(input);   
    }
    private void Stop(InputAction.CallbackContext _context)
    {
        moveController.Stop(transform);
    }
    private void Dodge(InputAction.CallbackContext _context)
    {
        StartCoroutine(moveController.Dodge(transform, materials.ToArray()));
    }
    private void Look(InputAction.CallbackContext _context)
    {
        moveController.Look();
    }

    public void Damage(float _damageValue, Vector3 _knockback)
    {
        cLifeController.ChangeLifePoint(_damageValue);
    }
}