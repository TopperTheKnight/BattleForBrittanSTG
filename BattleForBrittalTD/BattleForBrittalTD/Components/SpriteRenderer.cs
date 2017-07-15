using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BattleForBrittalTD
{
    public class SpriteRenderer : Component, ILoadable, IDrawable
    {
        #region Constructor

        public SpriteRenderer(GameObject gameObject, string spriteName, float layerDepth, Vector2 scale, Color color,
            float rotation, SpriteEffects effect) : base(gameObject)
        {
            this.spriteName = spriteName;
            this.layerDepth = layerDepth;
            Scale = scale;
            overlayColor = color;
            this.rotation = rotation;
            Effects = effect;
        }

        #endregion

        #region Fields

        private readonly string spriteName;
        private readonly float layerDepth;
        private Color overlayColor;
        private readonly float rotation;
        private Vector2 origin;

        #endregion

        #region Properties

        public Rectangle Rectangle { get; set; }

        public Texture2D Sprite { get; set; }

        public Vector2 Offset { get; set; }

        public Vector2 Scale { get; }

        public Color OverlayColor
        {
            set => overlayColor = value;
        }

        public SpriteEffects Effects { get; set; }

        #endregion

        #region Methods

        public void LoadContent(ContentManager content)
        {
            Sprite = content.Load<Texture2D>(spriteName);
            if (GameObject.GetComponent("Animator") == null)
                Rectangle = new Rectangle(0, 0 + (int)Offset.Y, Sprite.Width, Sprite.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, GameObject.Transform.Position, Rectangle, overlayColor, rotation, origin, Scale,
                Effects, layerDepth);
        }

        #endregion
    }
}