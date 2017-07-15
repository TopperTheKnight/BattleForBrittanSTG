using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace BattleForBrittalTD
{
    public class Animator : Component, IUpdateable
    {
        #region Constructor

        public Animator(GameObject gameObject, float animationSpeed) : base(gameObject)
        {
            Animations = new Dictionary<string, Animation>();
            this.animationSpeed = animationSpeed;
            spriteRenderer = (SpriteRenderer)gameObject.GetComponent("SpriteRenderer");
        }

        #endregion

        #region fields

        private readonly SpriteRenderer spriteRenderer;
        private float timeElapsed;
        private float animationSpeed;
        private Rectangle[] rectangles;

        #endregion

        #region Propeties

        public string AnimationName { get; set; }

        public int Index { get; private set; }

        public Dictionary<string, Animation> Animations { get; }

        #endregion

        #region Methods

        public void CreateAnimation(string name, Animation animation)
        {
            Animations.Add(name, animation);
        }

        public void PlayAnimation(string animationName)
        {
            if (AnimationName != animationName)
            {
                AnimationName = animationName;
                //Sets the rectangles
                rectangles = Animations[animationName].Rectangles;
                //Resets the rectangle
                spriteRenderer.Rectangle = rectangles[0];
                //Sets the offset
                spriteRenderer.Offset = Animations[animationName].Offset;
                //Sets the animation name
                AnimationName = animationName;
                //Sets the fps
                animationSpeed = Animations[animationName].AnimationSpeed;
                //Resets the animation
                timeElapsed = 0;

                Index = 0;
            }
        }

        public void Update()
        {
            timeElapsed += GameWorld.Instance.Deltatime;
            Index = (int)(timeElapsed * animationSpeed);
            if (AnimationName != null)
            {
                if (Index > rectangles.Length - 1)
                {
                    GameObject.OnAnimationDone(AnimationName);
                    timeElapsed = 0;
                    Index = 0;
                }
                spriteRenderer.Rectangle = rectangles[Index];
            }
        }

        #endregion
    }
}