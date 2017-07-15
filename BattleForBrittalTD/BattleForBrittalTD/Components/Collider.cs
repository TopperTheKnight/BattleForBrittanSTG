using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BattleForBrittalTD
{
    public class Collider : Component, IDrawable, ILoadable
    {
        private readonly Animator animator;

        /// <summary>
        ///     Contains all the colliders that this collider is colliding with
        /// </summary>
        protected HashSet<Collider> otherColliders = new HashSet<Collider>();

        /// <summary>
        ///     Dictionary that contains pixels for all animations
        /// </summary>
        private readonly Lazy<Dictionary<string, Color[][]>> pixels;

        /// <summary>
        ///     A reference to the colliders texture
        /// </summary>
        private Texture2D texture;

        public Collider(GameObject gameObject) : base(gameObject)
        {
            pixels = new Lazy<Dictionary<string, Color[][]>>(() => CachePixels());

            DoCollisionChecks = true;

            UsePixelCollision = false;

            spriteRenderer = (SpriteRenderer)gameObject.GetComponent("SpriteRenderer");

            animator = (Animator)gameObject.GetComponent("Animator");

            GameWorld.Instance.StandardCollidersToAdd.Add(this);
        }

        /// <summary>
        ///     A reference to the boxcolliders spriterenderer
        /// </summary>
        public SpriteRenderer spriteRenderer { get; }

        /// <summary>
        ///     Indicates if we need to use pixel collision or not
        /// </summary>
        public bool UsePixelCollision { get; set; }

        /// <summary>
        ///     Indicates if this collider needs to check for collisions
        /// </summary>
        public bool DoCollisionChecks { get; set; }

        private Color[] CurrentPixels => pixels.Value[animator.AnimationName][animator.Index];

        /// <summary>
        ///     The colliders collisionbox
        /// </summary>
        public Rectangle CollisionBox => new Rectangle
        (
            (int)(GameObject.Transform.Position.X + spriteRenderer.Offset.X),
            (int)(GameObject.Transform.Position.Y + spriteRenderer.Offset.Y),
            spriteRenderer.Rectangle.Width,
            spriteRenderer.Rectangle.Height
        );

        public void Draw(SpriteBatch spriteBatch)
        {
#if DEBUG
            var topLine = new Rectangle(CollisionBox.X, CollisionBox.Y, CollisionBox.Width, 1);
            var bottomLine = new Rectangle(CollisionBox.X, CollisionBox.Y + CollisionBox.Height, CollisionBox.Width, 1);
            var rightLine = new Rectangle(CollisionBox.X + CollisionBox.Width, CollisionBox.Y, 1, CollisionBox.Height);
            var leftLine = new Rectangle(CollisionBox.X, CollisionBox.Y, 1, CollisionBox.Height);

            spriteBatch.Draw(texture, topLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(texture, bottomLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(texture, rightLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(texture, leftLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
#endif
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("CollisionTexture");

            CachePixels();
        }

        public void Update()
        {
            CheckCollision();
        }

        private Dictionary<string, Color[][]> CachePixels()
        {
            var tmpPixels = new Dictionary<string, Color[][]>();
            if (animator != null)
                foreach (var pair in animator.Animations)
                {
                    var animation = pair.Value;

                    var colors = new Color[animation.Frames][];

                    for (var i = 0; i < animation.Frames; i++)
                    {
                        colors[i] = new Color[animation.Rectangles[i].Width * animation.Rectangles[i].Height];

                        spriteRenderer.Sprite.GetData(0, animation.Rectangles[i], colors[i], 0,
                            animation.Rectangles[i].Width * animation.Rectangles[i].Height);
                    }

                    tmpPixels.Add(pair.Key, colors);
                }

            return tmpPixels;
        }

        protected virtual void CheckCollision()
        {
            if (DoCollisionChecks)
            {
                var remvoveCol = false;
                Collider toRemove = null;
                foreach (var other in otherColliders)
                    if (other != this)
                        if (!GameWorld.Instance.StandardColliders.Contains(other))
                        {
                            remvoveCol = true;
                            toRemove = other;
                        }
                if (remvoveCol)
                {
                    otherColliders.Remove(toRemove);
                    GameObject.OnCollisionExit(toRemove);
                }
                foreach (var other in GameWorld.Instance.StandardColliders)
                    if (other != null)
                        if (other != this)
                            if (CollisionBox.Intersects(other.CollisionBox) &&
                                (UsePixelCollision && CheckPixelCollision(other) || !UsePixelCollision))
                            {
                                if (!otherColliders.Contains(other))
                                {
                                    otherColliders.Add(other);
                                    GameObject.OnCollisionEnter(other);
                                }

                                GameObject.OnCollisionStay(other);
                            }
                            else if (otherColliders.Contains(other) && !UsePixelCollision ||
                                     CollisionBox.Intersects(other.CollisionBox) && UsePixelCollision &&
                                     !CheckPixelCollision(other))
                            {
                                otherColliders.Remove(other);
                                GameObject.OnCollisionExit(other);
                            }
            }
        }

        protected bool CheckPixelCollision(Collider other)
        {
            // Find the bounds of the rectangle intersection
            var top = Math.Max(CollisionBox.Top, other.CollisionBox.Top);
            var bottom = Math.Min(CollisionBox.Bottom, other.CollisionBox.Bottom);
            var left = Math.Max(CollisionBox.Left, other.CollisionBox.Left);
            var right = Math.Min(CollisionBox.Right, other.CollisionBox.Right);

            for (var y = top; y < bottom; y++)
                for (var x = left; x < right; x++)
                {
                    var firstIndex = x - CollisionBox.Left + (y - CollisionBox.Top) * CollisionBox.Width;
                    var secondIndex = x - other.CollisionBox.Left + (y - other.CollisionBox.Top) * other.CollisionBox.Width;

                    //Get the color of both pixels at this point 
                    var colorA = CurrentPixels[firstIndex];
                    var colorB = other.CurrentPixels[secondIndex];

                    // If both pixels are not completely transparent
                    if (colorA.A != 0 && colorB.A != 0)
                        return true;
                }

            return false;
        }
    }
}