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
    public AudioClip TrollStep;
    public AudioClip TrollHurt;
    public AudioClip TrollAttack;

    private AudioSource MonsterAudioSource;
    public void setAudioSource(AudioSource audioSource)
    {
        MonsterAudioSource =audioSource;
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
}
