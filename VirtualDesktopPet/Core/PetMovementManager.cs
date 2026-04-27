using System;
using VirtualDesktopPet.Models;

namespace VirtualDesktopPet.Core
{
    public class PetMovementManager
    {
        private Random random = new Random();

        public Direction CurrentDirection { get; private set; }
        public int SpeedVariation { get; private set; }

        public PetMovementManager()
        {
            CurrentDirection = GetRandomDirection();
        }

        public void Update()
        {
            // occasionally change direction
            if (random.Next(1, 101) <= 2)
            {
                CurrentDirection = GetRandomDirection();
            }

            SpeedVariation = random.Next(-1, 2);
        }

        private Direction GetRandomDirection()
        {
            Array values = Enum.GetValues(typeof(Direction));
            return (Direction)values.GetValue(random.Next(values.Length));
        }

        public int GetFinalSpeed(int baseSpeed)
        {
            return Math.Max(1, baseSpeed + SpeedVariation);
        }

        public void SetDirection(Direction direction)
        {
            CurrentDirection = direction;
        }
    }
}