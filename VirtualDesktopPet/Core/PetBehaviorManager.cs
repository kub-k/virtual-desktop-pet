using System;
using VirtualDesktopPet.Models;

namespace VirtualDesktopPet.Core
{
    public class PetBehaviorManager
    {
        private Random random = new Random();

        private int walkChance;
        private int sitChance;
        private int sleepChance;

        public PetBehaviorManager(int walk, int sit, int sleep)
        {
            walkChance = walk;
            sitChance = sit;
            sleepChance = sleep;
        }

        public PetState GetNextState()
        {
            int roll = random.Next(1, 101);

            if (roll <= walkChance)
                return PetState.Walking;

            if (roll <= walkChance + sitChance)
                return PetState.Sitting;

            return PetState.Sleeping;
        }
    }
}