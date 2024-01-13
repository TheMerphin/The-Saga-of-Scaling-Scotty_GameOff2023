using System.Collections;
using UnityEngine;

public class SpikeTrap : Trap 
{
    public override void TriggerTrap(GameObject triggeringObject, bool isDamageable)
    {
        if (!Active) return;
        Active = false;

        StartCoroutine(ActivateDelay(triggeringObject));

        triggerAreaCollider.enabled = false;
    }

    private IEnumerator ActivateDelay(GameObject triggeringObject)
    {
        yield return new WaitForSeconds(0.23f);
        audioManager.Play(TrapSound.name);
        animator.SetTrigger(animatorTriggerTrapId);
        var boxCast = Physics2D.BoxCast(transform.position, Vector2.one, 0f, Vector2.zero, 0f, LayerMask.GetMask("Player"));

        if (boxCast.collider != null)
        {
            var trapTrigger = boxCast.collider.GetComponent<TrapTrigger>();
            if (trapTrigger != null && trapTrigger.IsDamageable)
            {
                ApplyDamage();
            }
        }

        GetComponent<Trap>().enabled = false;
    }
}
