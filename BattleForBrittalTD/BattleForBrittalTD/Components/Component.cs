namespace BattleForBrittalTD
{
    public class Component
    {
        #region Feilds

        #endregion

        #region Properties

        public GameObject GameObject { get; }

        #endregion

        #region Constructor

        public Component(GameObject gameObject)
        {
            GameObject = gameObject;
        }

        public Component()
        {
        }

        #endregion
    }
}