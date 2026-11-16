using LongRoad.Core.Scriptables;
using System.Collections;

namespace LongRoad.Core.PersonAI.Abstractions
{
    public abstract class PersonAIBase
    {
        public PersonEntity Person { get; private set; }

        protected PersonAIConfig Config => Person.Prototype.AIConfig;

        protected PersonAIBase(PersonEntity currentPerson)
        {
            Person = currentPerson;
        }

        public abstract IEnumerator Think(GameData gameData);
    }
}
