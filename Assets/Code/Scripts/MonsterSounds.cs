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
   public AudioClip WolfStep;
   public AudioClip WolfHurt;
   public AudioClip WolfAttack;

    private AudioSource MonsterAudioSource;
    public void setAudioSource(AudioSource audioSource)
    {
        MonsterAudioSource =audioSource;
    }

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


}
