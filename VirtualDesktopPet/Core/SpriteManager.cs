using System.Drawing;
using VirtualDesktopPet.Models;

namespace VirtualDesktopPet.Core
{
    public class SpriteManager
    {
        private PetSpriteSet sprites;

        private int frameIndex = 0;
        private int frameCounter = 0;
        private int frameSpeed = 10;

        public SpriteManager(PetSpriteSet spriteSet)
        {
            sprites = spriteSet;
        }

        public Image GetCurrentFrame(PetState state, Direction direction)
        {
            frameCounter++;

            if (frameCounter >= frameSpeed)
            {
                frameIndex++;
                frameCounter = 0;
            }

            Image[] frames;

            switch (state)
            {
                case PetState.Walking:

                    switch (direction)
                    {
                        case Direction.Left:
                            frames = sprites.WalkLeft;
                            break;

                        case Direction.Right:
                            frames = sprites.WalkRight;
                            break;

                        case Direction.Up:
                            frames = sprites.WalkUp;
                            break;

                        case Direction.Down:
                            frames = sprites.WalkDown;
                            break;

                        default:
                            frames = sprites.WalkDown;
                            break;
                    }

                    break;

                case PetState.Sitting:
                    frames = sprites.Sit;
                    break;

                case PetState.Sleeping:
                    frames = sprites.Sleep;
                    break;

                default:
                    frames = sprites.Sit;
                    break;
            }

            if (frames == null || frames.Length == 0)
                return null;

            return frames[frameIndex % frames.Length];
        }
    }
}