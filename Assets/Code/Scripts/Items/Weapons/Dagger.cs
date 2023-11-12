using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using static Toolbox;
using static UnityEditor.Progress;

public class Dagger : Weapon
{
    void Awake()
    {
        OnAwake();
    }

    void Start()
    {
        OnStart();
    }

    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + new Vector3(0.5f, -0.5f, 0f), new Vector2(1.0f, 1.0f));
        Gizmos.DrawWireCube(transform.position + new Vector3(0.5f, 0.5f, 0f), new Vector2(1.0f, 1.0f));
        Gizmos.DrawWireCube(transform.position + new Vector3(-0.5f, 0.5f, 0f), new Vector2(1.0f, 1.0f));
        Gizmos.DrawWireCube(transform.position + new Vector3(-0.5f, -0.5f, 0f), new Vector2(1.0f, 1.0f));
    }

    public override void Attack()
    {
        var attackDirection = player.GetPlayerFacingDirection();
        var attackPos = (Vector2)player.transform.position;

        var attackBoxSize = new Vector2(1.0f, 1.0f);
        var attackOffset = new Vector2(0.5f, 0.5f);

        switch (attackDirection)
        {
            case DiagonalDirection.UpRight:
                attackOffset = attackOffset * new Vector2(1f, 1f);
                break;
            case DiagonalDirection.UpLeft:
                attackOffset = attackOffset * new Vector2(-1f, 1f);
                break;
            case DiagonalDirection.DownLeft:
                attackOffset = attackOffset * new Vector2(-1f, -1f);
                break;
            default: // DiagonalDirection.DownRight
                attackOffset = attackOffset * new Vector2(1f, -1f);
                break;
        }

        var boxCast = Physics2D.BoxCastAll(attackPos + attackOffset, attackBoxSize, 0f, Vector2.zero, 0f, LayerMask.GetMask("Enemy"));
        boxCast.ToList().ForEach(hit => {
            var enemyController = hit.transform.GetComponent<Transform>(); // Transform durch EnemyController swappen

            if (enemyController != null)
            {
                // TODO enemyController.Damage(this.damage);
                print("Hit: " + enemyController.name);
            }
        });

        audioManager.Play(AttackSound.name);
    }
}
