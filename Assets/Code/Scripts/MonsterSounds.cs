using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSounds : MonoBehaviour
{
    //Skeleton Sounds
    public AudioClip SkeletonHurt;
    public AudioClip SkeletonDeath;
    public AudioClip SkeletonAttack;
    public AudioClip SkeletonStep;
    public AudioClip SkeletonAssemble;
    public AudioClip SkeletonDisassemble;
    //Wolf Sounds
    public AudioClip WolfStep;
    public AudioClip WolfHurt;
    public AudioClip WolfAttack;
    public AudioClip WolfDeath;
    //Troll Sounds
    public AudioClip TrollStep;
    public AudioClip TrollHurt;
    public AudioClip TrollAttack;
    public AudioClip TrollDeath;

    //Slime Sounds
    public AudioClip SlimeStep;
    public AudioClip SlimeHurt;
    public AudioClip SlimeAttack;
    public AudioClip SlimeDeath;

    private AudioSource MonsterAudioSource;
    public void setAudioSource(AudioSource audioSource)
    {
        MonsterAudioSource = audioSource;
    }


    public void destroyParentMonster()
    {
        Destroy(this.transform.parent.gameObject);
    }

    //skeleton

    public void playSkeletonHurt()
    {
        MonsterAudioSource.PlayOneShot(SkeletonHurt);
    }

    public void playSkeletonDeath()
    {
        MonsterAudioSource.PlayOneShot(SkeletonDeath);
    }

    public void playSkeletonAttack()
    {
        MonsterAudioSource.PlayOneShot(SkeletonAttack);
    }
    public void playSkeletonStep()
    {
        MonsterAudioSource.PlayOneShot(SkeletonStep);
    }

    public void playSkeletonDisassemble()
    {
        MonsterAudioSource.PlayOneShot(SkeletonDisassemble);
    }

    public void playSkeletonAssemble()
    {
        MonsterAudioSource.PlayOneShot(SkeletonAssemble);
    }

    //wolf
    public void playWolfStep()
    {
        MonsterAudioSource.PlayOneShot(WolfStep);
    }

    public void playWolfAttack()
    {
        MonsterAudioSource.PlayOneShot(WolfAttack);
    }

    public void playWolfHurt()
    {
        MonsterAudioSource.PlayOneShot(WolfHurt);
    }
    public void playWolfDeath()
    {
        MonsterAudioSource.PlayOneShot(WolfHurt);
    }
    //Troll
    public void playTrollStep()
    {
        MonsterAudioSource.PlayOneShot(TrollStep);
    }

    public void playTrollAttack()
    {
        MonsterAudioSource.PlayOneShot(TrollAttack);
    }

    public void playTrollHurt()
    {
        MonsterAudioSource.PlayOneShot(TrollHurt);
    }

    public void playTrollDeath()
    {
        MonsterAudioSource.PlayOneShot(TrollDeath);
    }

    //Slime
    public void playSlimeStep()
    {
        MonsterAudioSource.PlayOneShot(SlimeStep);
    }

    public void playSlimeAttack()
    {
        MonsterAudioSource.PlayOneShot(SlimeAttack);
    }

    public void playSlimeHurt()
    {
        MonsterAudioSource.PlayOneShot(SlimeHurt);
    }

    public void playSlimeDeath()
    {
        MonsterAudioSource.PlayOneShot(SlimeDeath);
    }

    public void hitPLayer()
    {
        EnemyController enemyController = GetComponentInParent<EnemyController>();
        enemyController.hitPLayer();
    }
}
