using LongRoad.Core.Scriptables.Abstractions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LongRoad.Core.Scriptables
{
    [CreateAssetMenu(fileName = "Character", menuName = "Entities/Character")]
    public class Person : LongRoadScriptable 
    {
        public Sprite Sprite;

        public Specailty Specailty;

        public List<Trait> Traits = new();

        public int DefaultHeal = 100;
        public int DefaultHunger = 100;
        public int DefaultMood = 100;

        public bool HasTrait(string tag)
        {
            return Traits.Any(x => x.Tag == tag);
        }
        public bool HasTrait(Trait trait)
        {
            return HasTrait(trait.Tag);
        }
    }
}
