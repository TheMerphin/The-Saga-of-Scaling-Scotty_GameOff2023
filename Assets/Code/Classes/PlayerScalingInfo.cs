using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Toolbox;

public struct PlayerScalingInfo
{
    public ScaleLevel ScaleLevel;
    public float TransformScale;
    public float MovementSpeedModifier;
    public float AttackSpeedModifier;
    public float AttackDamageModifier;
    public float MaxHealthModifier;

    public PlayerScalingInfo(ScaleLevel scaleLevel, float playerScale, float movementSpeedModifier, float attackSpeedModifier, float attackDamageModifier, float maxHealthModifier)
    {
        this.ScaleLevel = scaleLevel;
        this.TransformScale = playerScale;
        this.MovementSpeedModifier = movementSpeedModifier;
        this.AttackSpeedModifier = attackSpeedModifier;
        this.AttackDamageModifier = attackDamageModifier;
        this.MaxHealthModifier = maxHealthModifier;
    }
}
