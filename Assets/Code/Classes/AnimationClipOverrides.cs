using System.Collections.Generic;
using UnityEngine;

/**
 * Class used to override/swap animation clips of a running animator.
 */
public class AnimationClipOverrides : List<KeyValuePair<AnimationClip, AnimationClip>>
{
    public AnimationClipOverrides(int capacity) : base(capacity) { }

    public AnimationClip this[string name]
    {
        get { return this.Find(x => AnimationClipNameEquals(x.Key.name, name)).Value; }
        set
        {
            int index = this.FindIndex(x => AnimationClipNameEquals(x.Key.name, name));
            if (index != -1)
                this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
        }
    }

    /**
     * Two clip names are equals either if they have the same Identifier at the last token
     * separated by underscores (e.g. "XXXX_AttackBL") or otherwise they fully match.
     */
    private bool AnimationClipNameEquals(string clipName1, string clipName2)
    {
        var stringTokens = clipName2.Split("_");

        if (stringTokens.Length > 0)
        {
            var directionalActionIdentifier = stringTokens[stringTokens.Length - 1];
            return clipName1.Contains(directionalActionIdentifier);
        }
        else
        {
            return clipName1.Equals(clipName2);
        }
    }
}
