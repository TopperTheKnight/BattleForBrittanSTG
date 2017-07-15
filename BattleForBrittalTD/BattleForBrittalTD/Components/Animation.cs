using Microsoft.Xna.Framework;

namespace BattleForBrittalTD
{
    public class Animation
    {
        #region Constructor

        public Animation(int frames, int yPos, int xStartFrame, int width, int height, float animationSpeed,
            Vector2 offset)
        {
            Frames = frames;
            Rectangles = new Rectangle[frames];
            Offset = offset;
            AnimationSpeed = animationSpeed;
            for (var i = 0; i < frames; i++)
                Rectangles[i] = new Rectangle((i + xStartFrame) * width, yPos, width, height);
        }

        #endregion

        #region Fields

        #endregion

        #region Properties

        public Vector2 Offset { get; }

        public float AnimationSpeed { get; }

        public Rectangle[] Rectangles { get; }

        public int Frames { get; }

        #endregion
    }
}