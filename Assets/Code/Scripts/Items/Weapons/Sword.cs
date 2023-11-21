using System.Linq;
using UnityEngine;
using static Toolbox;

public class Sword : Weapon
{
    public Vector2 attackBoxSize = new Vector2(1.2f, 1.2f);
    public Vector2 attackOffset = new Vector2(0.6f, 0.6f);
    private float playerTransformFactor = 1f;

    ParticleSystem attackParticles;

    protected override void Awake()
    {
        base.Awake();
        attackParticles = GetComponentInChildren<ParticleSystem>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + (Vector3)((attackOffset * 0.5f) * new Vector2(1f, 1f)), attackBoxSize * 0.5f);
        Gizmos.DrawWireCube(transform.position + (Vector3)((attackOffset * 0.5f) * new Vector2(-1f, 1f)), attackBoxSize * 0.5f);
        Gizmos.DrawWireCube(transform.position + (Vector3)((attackOffset * 0.5f) * new Vector2(1f, -1f)), attackBoxSize * 0.5f);
        Gizmos.DrawWireCube(transform.position + (Vector3)((attackOffset * 0.5f) * new Vector2(-1f, -1f)), attackBoxSize * 0.5f);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + (Vector3)((attackOffset) * new Vector2(1f, 1f)), attackBoxSize);
        Gizmos.DrawWireCube(transform.position + (Vector3)((attackOffset) * new Vector2(-1f, 1f)), attackBoxSize);
        Gizmos.DrawWireCube(transform.position + (Vector3)((attackOffset) * new Vector2(1f, -1f)), attackBoxSize);
        Gizmos.DrawWireCube(transform.position + (Vector3)((attackOffset) * new Vector2(-1f, -1f)), attackBoxSize);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + (Vector3)((attackOffset * 1.75f) * new Vector2(1f, 1f)), attackBoxSize * 1.75f);
        Gizmos.DrawWireCube(transform.position + (Vector3)((attackOffset * 1.75f) * new Vector2(-1f, 1f)), attackBoxSize * 1.75f);
        Gizmos.DrawWireCube(transform.position + (Vector3)((attackOffset * 1.75f) * new Vector2(1f, -1f)), attackBoxSize * 1.75f);
        Gizmos.DrawWireCube(transform.position + (Vector3)((attackOffset * 1.75f) * new Vector2(-1f, -1f)), attackBoxSize * 1.75f);
    }

    public override void Attack()
    {
        var attackDirection = player.GetPlayerFacingDirection();
        var attackPos = (Vector2)player.transform.position;

        Vector2 particlesOffset = Vector2.zero;
        Quaternion particlesRotation = Quaternion.identity;

        Vector2 _attackOffset;
        switch (attackDirection)
        {
            case DiagonalDirection.UpRight:
                _attackOffset = attackOffset * new Vector2(1f, 1f) * playerTransformFactor;
                particlesOffset = new Vector2(0.25f, 0.15f);
                particlesRotation = Quaternion.Euler(0, 0, 10f);
                break;
            case DiagonalDirection.UpLeft:
                _attackOffset = attackOffset * new Vector2(-1f, 1f) * playerTransformFactor;
                particlesOffset = new Vector2(-0.25f, 0.15f);
                particlesRotation = Quaternion.Euler(0, 0, 70f);
                break;
            case DiagonalDirection.DownLeft:
                _attackOffset = attackOffset * new Vector2(-1f, -1f) * playerTransformFactor;
                particlesOffset = new Vector2(-0.25f, 0.45f);
                particlesRotation = Quaternion.Euler(0, 0, 190f);
                break;
            default: // DiagonalDirection.DownRight
                _attackOffset = attackOffset * new Vector2(1f, -1f) * playerTransformFactor;
                particlesOffset = new Vector2(0.25f, 0.45f);
                particlesRotation = Quaternion.Euler(0, 0, 250f);
                break;
        }

        var boxCast = Physics2D.BoxCastAll(attackPos + _attackOffset, attackBoxSize * playerTransformFactor, 0f, Vector2.zero, 0f, LayerMask.GetMask("Enemy"));
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

    public override void OnPlayerScaleChange(PlayerScalingInfo updatedScalingLevelInfo)
    {
        base.OnPlayerScaleChange(updatedScalingLevelInfo);

        playerTransformFactor = updatedScalingLevelInfo.TransformScale;
        attackParticles.transform.localScale = Vector3.one * playerTransformFactor;
    }
}
