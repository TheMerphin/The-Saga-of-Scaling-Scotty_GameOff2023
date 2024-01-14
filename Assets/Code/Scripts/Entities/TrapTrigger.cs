using System;
using UnityEngine;

public class TrapTrigger : MonoBehaviour
{

    [SerializeField]
    private bool isDamageable = false;
    public bool IsDamageable { get { return isDamageable; } set { isDamageable = value; } }

    void Update()
    {
        TriggerTrap();
    }

    public void TriggerTrap()
    {
        var trapCast = Physics2D.CircleCastAll(transform.position, 0.01f, Vector2.zero, 0f, LayerMask.GetMask("Trap"));
        Array.ForEach(trapCast, trap =>
        {
            var trapController = trap.collider.GetComponent<Trap>();
            if (trapController != null) trapController.TriggerTrap(gameObject, isDamageable);
        });
    }
}
