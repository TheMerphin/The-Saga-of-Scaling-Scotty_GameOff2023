using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * Enables to store and manage multiple status values, such as health, mana, etc.
 * The entity stats themselves further offer functionalities to update and modify their values.
 */
public class StatManager : MonoBehaviour
{
    public readonly List<EntityStat> EntityStats = new();

    public void AddStats(params EntityStat[] entityStats)
    {
        EntityStats.AddRange(entityStats);
    }

    public bool RemoveStats(params string[] statIdentifiers)
    {
        return EntityStats.RemoveAll(x => statIdentifiers.Contains(x.DisplayName)) > 0;
    }

    public int GetIntValue(string statIdentifier)
    {
        var entityStat = EntityStats.Find(x => x.DisplayName.Equals(statIdentifier));

        if (entityStat is EntityStatFloat)
        {
            return (int) ((EntityStatFloat)entityStat).Value;
        }
        else
        {
            return ((EntityStatInt)entityStat).Value;
        }
    }

    public float GetFloatValue(string statIdentifier)
    {
        var entityStat = EntityStats.Find(x => x.DisplayName.Equals(statIdentifier));

        if (entityStat is EntityStatFloat)
        {
            return (entityStat as EntityStatFloat).Value;
        }
        else
        {
            return (entityStat as EntityStatInt).Value;
        }
    }

    public Range<int> GetIntValueRange(string statIdentifier)
    {
        var entityStat = EntityStats.Find(x => x.DisplayName.Equals(statIdentifier));

        if (entityStat is EntityStatFloat)
        {
            var floatRange = (entityStat as EntityStatFloat).ValueRange;
            return RangeInt.of((int)floatRange.Min, (int)floatRange.Max);
        }
        else
        {
            return (entityStat as EntityStatInt).ValueRange;
        }
    }

    public Range<float> GetFloatValueRange(string statIdentifier)
    {
        var entityStat = EntityStats.Find(x => x.DisplayName.Equals(statIdentifier));

        if (entityStat is EntityStatFloat)
        {
            return (entityStat as EntityStatFloat).ValueRange;
        }
        else
        {
            var intRange = (entityStat as EntityStatInt).ValueRange;
            return RangeFloat.of(intRange.Min, intRange.Max);
        }
    }

    public bool UpdateStatValue(string statIdentifier, float additive)
    {
        var entityStat = EntityStats.Find(x => x.DisplayName.Equals(statIdentifier));

        if (entityStat != null)
        {
            if (entityStat is EntityStatFloat)
            {
                (entityStat as EntityStatFloat).Update(additive);
            }
            else
            {
                (entityStat as EntityStatInt).Update((int)additive);
            }
            
            return true;
        }

        return false;
    }

    public bool SetStatValue(string statIdentifier, float value)
    {
        var entityStat = EntityStats.Find(x => x.DisplayName.Equals(statIdentifier));

        if (entityStat != null)
        {
            if (entityStat is EntityStatFloat)
            {
                (entityStat as EntityStatFloat).Value = value;
            }
            else
            {
                (entityStat as EntityStatInt).Value = (int) value;
            }

            return true;
        }

        return false;
    }

    public bool AddStatModifier(string statIdentifier, StatModifier modifier)
    {
        var entityStat = EntityStats.Find(x => x.DisplayName.Equals(statIdentifier));

        if (entityStat != null)
        {
            entityStat.AddModifier(modifier);
            return true;
        }

        return false;
    }

    public bool RemoveStatModifier(string statIdentifier, string modifierIdentifier)
    {
        var entityStat = EntityStats.Find(x => x.DisplayName.Equals(statIdentifier));

        if (entityStat != null)
        {
            return entityStat.RemoveModifier(modifierIdentifier);
        }

        return false;
    }
}
