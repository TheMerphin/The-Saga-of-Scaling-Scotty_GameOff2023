using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static Toolbox;

public abstract class Weapon : Item
{
    [Header("Weapon Settings")]
    [SerializeField]
    private WeaponType weaponType;
    public WeaponType WeaponType { get { return weaponType; } set { weaponType = value; } }

    [SerializeField]
    private float damage;
    private float _damage;
    public float Damage { get { return _damage; } set { _damage = value; } }

    [SerializeField]
    private float attackSpeedMultiplier = 1f;
    private float _attackSpeedMultiplier;
    public float AttackSpeedMultiplier { get { return _attackSpeedMultiplier; } set { _attackSpeedMultiplier = value; } }

    [SerializeField]
    private Sound attackSound;
    public Sound AttackSound { get { return attackSound; } set { attackSound = value; } }

    /**
     * Stores the 4-directional attack animations in an array. The indices are assigned as follows:
     * 0 -> TR
     * 1 -> BR
     * 2 -> BL
     * 3 -> TL
     */
    [SerializeField]
    private AnimationClip[] attackAnimations;
    public AnimationClip[] AttackAnimations { get { return attackAnimations; } set { attackAnimations = value; } }

    protected override void Awake()
    {
        base.Awake();

        _attackSpeedMultiplier = attackSpeedMultiplier;
        _damage = damage;
    }

    protected override void Start()
    {
        base.Start();

        Array.ForEach(gameObject.GetComponentsInChildren<TextMeshProUGUI>(), text => {
            if (text.name.Equals("#AUTOFILL_WeaponType")) { text.text = WeaponType.ToString(); }
            if (text.name.Equals("#AUTOFILL_Damage")) { text.text = Damage.ToString(); }
            if (text.name.Equals("#AUTOFILL_AttackSpeed")) { text.text = AttackSpeedMultiplier > 0.8 ? AttackSpeedMultiplier > 1.2 ? "Fast" : "Medium" : "Slow"; }
        });

        audioManager.AddSound(attackSound);
    }

    public override void OnPlayerScaleChange(PlayerScalingInfo updatedScalingLevelInfo)
    {
        base.OnPlayerScaleChange(updatedScalingLevelInfo);
        _attackSpeedMultiplier = attackSpeedMultiplier * updatedScalingLevelInfo.AttackSpeedModifier;
        _damage = damage * updatedScalingLevelInfo.AttackDamageModifier;
    }

    public abstract void Attack();
}

public enum WeaponType
{
    Melee,
    Range,
    Special
}
