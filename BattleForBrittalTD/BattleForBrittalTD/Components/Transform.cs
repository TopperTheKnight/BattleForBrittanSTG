using Microsoft.Xna.Framework;

namespace BattleForBrittalTD
{
    public class Transform : Component
    {
        #region Feilds

        private Vector2 position;

        #endregion

        #region Constructor

        public Transform(GameObject gameObject, Vector2 position) : base(gameObject)
        {
            this.position = position;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Returns the position based on map origin, meaning it can be outside the screen.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                if (GameObject.GetComponent("Player") == null) return Map.Instance.MapOrigin + position;
                return position;
            }
            set
            {
                if (GameObject.GetComponent("Player") == null) position = value;
                else position = value;
            }
        }

        #endregion
    }
}