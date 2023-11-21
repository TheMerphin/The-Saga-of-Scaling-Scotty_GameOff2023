using System.Collections;
using UnityEngine;

public class SpikeTrap : Trap 
{
    public override void TriggerTrap(PlayerController player)
    {
        if (!Active) return;
        Active = false;

        StartCoroutine(ActivateDelay(player));

        triggerAreaCollider.enabled = false;
    }

    private IEnumerator ActivateDelay(PlayerController player)
    {
        yield return new WaitForSeconds(0.21f);
        audioManager.Play(TrapSound.name);
        animator.SetTrigger(animatorTriggerTrapId);
        var boxCast = Physics2D.BoxCast(transform.position, Vector2.one, 0f, Vector2.zero, 0f, LayerMask.GetMask("Player"));

        if (boxCast.collider != null && boxCast.collider.GetComponent<PlayerController>() != null)
        {
            //player.GetDamage();
            print(TrapName + " was activated. Applying " + Damage + " Damage to " + player.name);
        }

        GetComponent<Trap>().enabled = false;
    }
}
