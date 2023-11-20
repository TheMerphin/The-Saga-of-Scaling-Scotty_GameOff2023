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

    private AudioSource MonsterAudioSource;
    public void setAudioSource(AudioSource audioSource)
    {
        MonsterAudioSource =audioSource;
    }

    //public void changeVolume(float volume)
    //{
    //    MonsterAudioSource.volume=volume;
    //}

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
    
}
