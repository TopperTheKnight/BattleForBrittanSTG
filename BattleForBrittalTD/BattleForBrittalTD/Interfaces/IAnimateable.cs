namespace BattleForBrittalTD
{
    internal interface IAnimateable
    {
        #region Methods

        void CreateAnimations();
        void OnAnimationDone(string animationName);

        #endregion
    }
}