using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Weapon : Item
{
    [Header("Weapon Settings")]
    [SerializeField]
    private WeaponType weaponType;
    public WeaponType WeaponType { get { return weaponType; } set { weaponType = value; } }

    [SerializeField]
    private int damage;
    public int Damage { get { return damage; } set { damage = value; } }

    [SerializeField]
    private float attackSpeedMultiplier = 1f;
    public float AttackSpeedMultiplier { get { return attackSpeedMultiplier; } set { attackSpeedMultiplier = value; } }

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

    protected override void OnAwake()
    {
        base.OnAwake();
    }

    protected override void OnStart()
    {
        base.OnStart();

        Array.ForEach(gameObject.GetComponentsInChildren<TextMeshProUGUI>(), text => {
            if (text.name.Equals("#AUTOFILL_WeaponType")) { text.text = WeaponType.ToString(); }
            if (text.name.Equals("#AUTOFILL_Damage")) { text.text = Damage.ToString(); }
            if (text.name.Equals("#AUTOFILL_AttackSpeed")) { text.text = AttackSpeedMultiplier > 0.8 ? AttackSpeedMultiplier > 1.2 ? "Fast" : "Medium" : "Slow"; }
        });

        audioManager.AddSound(attackSound);
    }

    public abstract void Attack();
}

public enum WeaponType
{
    Melee,
    Range,
    Special
}
