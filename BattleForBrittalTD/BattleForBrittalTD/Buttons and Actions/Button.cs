using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace BattleForBrittalTD
{
    class Button : IUpdateable, IDrawable
    {
        #region Fields
        private Vector2 position;
        private Texture2D sprite;
        private MouseState mouseState;
        private string name;
        private string spritePath;
        private IButtonAction myAction;
        private bool clickAble = false;
        private Rectangle rectangle;

        #endregion

        #region Props
        public string Name => name;
        public bool ClickAble { get => clickAble; set { clickAble = value; } }
        #endregion

        public Button(int posX, int posY, string name, string spritePath, IButtonAction action)
        {
            this.position.X = posX;
            this.position.Y = posY;
            this.name = name;
            this.myAction = action;
            this.spritePath = spritePath;
        }

        public void Update()
        {
            if (clickAble) {
                mouseState = Mouse.GetState();
                if (ButtonPressed())
                {
                    myAction.Execute();
                }
            }
        }

        private bool ButtonPressed()
        {
            if (mouseState.X <= position.X + sprite.Width &&
                mouseState.X >= position.X &&
                mouseState.Y <= position.Y + sprite.Height &&
                mouseState.Y >= position.Y &&
                mouseState.LeftButton == ButtonState.Pressed
                )
                return true;
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, rectangle, Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.9f);
        }
        public void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>(spritePath);
            rectangle = new Rectangle(0, 0, sprite.Width, sprite.Height);
        }
    }
}
