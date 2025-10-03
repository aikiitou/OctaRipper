using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [Header("ç≈ëÂëÃóÕ")]
    [SerializeField]
    private float fMaxLif;

    PlayerMoveController moveController;
    LifeController cLifeController;

    private void Start()
    {
        cLifeController = GetComponent<LifeController>();
        moveController = GetComponent<PlayerMoveController>();

        cLifeController.SetLifePoint(fMaxLif);
        cLifeController.SetInvincible(false);
    }

    public void Damage(float _damageValue, Vector3 _knockback)
    {
        cLifeController.ChangeLifePoint(_damageValue);
    }
}