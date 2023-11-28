using System.Linq;
using UnityEngine;
using static Toolbox;

public class Dagger : Weapon
{
    public Vector2 attackBoxSize = new Vector2(1.0f, 1.0f);
    public Vector2 attackOffset = new Vector2(0.5f, 0.5f);
    private float playerTransformFactor = 1f;

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

        Vector2 _attackOffset;
        switch (attackDirection)
        {
            case DiagonalDirection.UpRight:
                _attackOffset = attackOffset * new Vector2(1f, 1f) * playerTransformFactor;
                break;
            case DiagonalDirection.UpLeft:
                _attackOffset = attackOffset * new Vector2(-1f, 1f) * playerTransformFactor;
                break;
            case DiagonalDirection.DownLeft:
                _attackOffset = attackOffset * new Vector2(-1f, -1f) * playerTransformFactor;
                break;
            default: // DiagonalDirection.DownRight
                _attackOffset = attackOffset * new Vector2(1f, -1f) * playerTransformFactor;
                break;
        }

        var boxCast = Physics2D.BoxCastAll(attackPos + _attackOffset, attackBoxSize * playerTransformFactor, 0f, Vector2.zero, 0f, LayerMask.GetMask("Enemy"));
        boxCast.ToList().ForEach(hit => {
            EnemyController enemyController = hit.transform.GetComponent<EnemyController>(); // Transform durch EnemyController swappen

            if (enemyController != null)
            {
                //print("Hit: " + enemyController.name + " with " + this.Damage);
                enemyController.getAttacked(this.Damage);
            }
        });

        audioManager.Play(AttackSound.name);
    }

    public override void OnPlayerScaleChange(PlayerScalingInfo updatedScalingLevelInfo)
    {
        base.OnPlayerScaleChange(updatedScalingLevelInfo);

        playerTransformFactor = updatedScalingLevelInfo.TransformScale;
    }
}
