using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using static Toolbox;
using static UnityEditor.Progress;

public class Sword : Weapon
{
    ParticleSystem attackParticles;

    void Awake()
    {
        attackParticles = GetComponentInChildren<ParticleSystem>();
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
        Gizmos.DrawWireCube(transform.position + new Vector3(0.6f, -0.6f, 0f), new Vector2(1.2f, 1.2f));
        Gizmos.DrawWireCube(transform.position + new Vector3(0.6f, 0.6f, 0f), new Vector2(1.2f, 1.2f));
        Gizmos.DrawWireCube(transform.position + new Vector3(-0.6f, 0.6f, 0f), new Vector2(1.2f, 1.2f));
        Gizmos.DrawWireCube(transform.position + new Vector3(-0.6f, -0.6f, 0f), new Vector2(1.2f, 1.2f));
    }

    public override void Attack()
    {
        var attackDirection = player.GetPlayerFacingDirection();
        var attackPos = (Vector2)player.transform.position;

        var attackBoxSize = new Vector2(1.2f, 1.2f);
        var attackOffset = new Vector2(0.6f, 0.6f);
        Vector2 particlesOffset = Vector2.zero;
        Quaternion particlesRotation = Quaternion.identity;

        switch (attackDirection)
        {
            case DiagonalDirection.UpRight:
                attackOffset = attackOffset * new Vector2(1f, 1f);
                particlesOffset = new Vector2(0.25f, -0.1f);
                particlesRotation = Quaternion.Euler(0, 0, 10f);
                break;
            case DiagonalDirection.UpLeft:
                attackOffset = attackOffset * new Vector2(-1f, 1f);
                particlesOffset = new Vector2(-0.25f, -0.1f);
                particlesRotation = Quaternion.Euler(0, 0, 70f);
                break;
            case DiagonalDirection.DownLeft:
                attackOffset = attackOffset * new Vector2(-1f, -1f);
                particlesOffset = new Vector2(-0.25f, 0.2f);
                particlesRotation = Quaternion.Euler(0, 0, 190f);
                break;
            default: // DiagonalDirection.DownRight
                attackOffset = attackOffset * new Vector2(1f, -1f);
                particlesOffset = new Vector2(0.25f, 0.2f);
                particlesRotation = Quaternion.Euler(0, 0, 250f);
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

        attackParticles.transform.localPosition = particlesOffset;
        attackParticles.transform.localRotation = particlesRotation;
        attackParticles.Play();
    }
}
