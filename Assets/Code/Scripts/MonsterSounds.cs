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

    //Minotaur Sounds
    public AudioClip MinotaurStep;
    public AudioClip MinotaurHurt;
    public AudioClip MinotaurAttack;
    public AudioClip MinotaurDeath;

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
        MonsterAudioSource.PlayOneShot(WolfDeath);
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
        MonsterAudioSource.pitch = 1f;
        MonsterAudioSource.PlayOneShot(SlimeStep);
    }

    public void playSlimeAttack()
    {
        MonsterAudioSource.pitch = 1f;
        MonsterAudioSource.PlayOneShot(SlimeAttack);
    }

    public void playSlimeHurt()
    {
        MonsterAudioSource.pitch = 2f;
        MonsterAudioSource.PlayOneShot(SlimeDeath);

    }

    public void playSlimeDeath()
    {
        MonsterAudioSource.pitch = 1f;
        MonsterAudioSource.PlayOneShot(SlimeDeath);
    }

    public void hitPLayer()
    {
        EnemyController enemyController = GetComponentInParent<EnemyController>();
        enemyController.hitPLayer();
    }

    //Minotaur
    public void playMinoStep()
    {
        MonsterAudioSource.PlayOneShot(MinotaurStep);
    }

    public void playMinoAttack()
    {
        MonsterAudioSource.PlayOneShot(MinotaurAttack);
    }

    public void playMinoHurt()
    {
        MonsterAudioSource.PlayOneShot(MinotaurHurt);
    }

    public void playMinoDeath()
    {
        MonsterAudioSource.PlayOneShot(MinotaurDeath);
    }


}
