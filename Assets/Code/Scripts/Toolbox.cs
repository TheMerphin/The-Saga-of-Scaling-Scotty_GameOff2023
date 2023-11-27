using System.Collections.Generic;
using UnityEngine;

public static class Toolbox
{
    public static DiagonalDirection GetDiagonalDirection(Vector2 vector)
    {
        if (vector.x >= 0 && vector.y > 0)
            return DiagonalDirection.UpRight;
        else if (vector.x < 0 && vector.y >= 0)
            return DiagonalDirection.UpLeft;
        else if (vector.x <= 0 && vector.y < 0)
            return DiagonalDirection.DownLeft;
        else // if (vector.x > 0 && vector.y <= 0)
            return DiagonalDirection.DownRight;
    }

    public enum DiagonalDirection
    {
        UpRight,
        UpLeft,
        DownRight, // Default state
        DownLeft
    }

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

    public enum ScaleLevel
    {
        Small = -1,
        Normal = 0, // Default state
        Big = 1
    }

    public static PlayerScalingInfo GetScaleStructByScaleLevel(ScaleLevel scaleLevel)
    {
        switch (scaleLevel)
        {
            case ScaleLevel.Small:
                return new PlayerScalingInfo(ScaleLevel.Small, 0.5f, 1.1f, 1.1f, 0.8f, 1.4f);
            case ScaleLevel.Big:
                return new PlayerScalingInfo(ScaleLevel.Big, 1.75f, 0.8f, 0.8f, 1.2f, 0.6f);
            default: // ScaleLevel.Normal
                return new PlayerScalingInfo(ScaleLevel.Normal, 1f, 1f, 1f, 1f, 1f);
        }
    }
}
