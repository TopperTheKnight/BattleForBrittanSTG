using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BattleForBrittalTD
{
    public class GameObject : Component
    {
        #region Constructor

        public GameObject(Vector2 posistion)
        {
            components = new List<Component>();
            componentsToRemove = new List<Component>();
            Transform = new Transform(this, posistion);
        }

        #endregion

        #region Properties

        public Transform Transform { get; }

        #endregion

        #region Fields

        private readonly List<Component> components;
        private bool isLoaded;
        private readonly List<Component> componentsToRemove;

        #endregion

        #region Methods

        public void LoadContent(ContentManager content)
        {
            if (!isLoaded)
            {
                foreach (var component in components)
                    if (component is ILoadable)
                        (component as ILoadable).LoadContent(content);
                isLoaded = true;
            }
        }

        public void Update()
        {
            foreach (var component in components)
                if (component is IUpdateable)
                    (component as IUpdateable).Update();
            if (componentsToRemove.Count > 0)
                components.Remove(componentsToRemove[0]);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var component in components)
                if (component is IDrawable)
                    (component as IDrawable).Draw(spriteBatch);
        }

        public Component GetComponent(string component)
        {
            for (var i = 0; i < components.Count; i++)
                if (components[i].GetType().Name == component)
                    return components[i];
            return null;
        }

        public void AddComponent(Component component)
        {
            components.Add(component);
        }

        public void RemoveComponent(string component)
        {
            componentsToRemove.Add(GetComponent(component));
        }

        public void OnAnimationDone(string animationName)
        {
            foreach (var component in components)
                if (component is IAnimateable) //Checks if any components are IAnimateable
                    (component as IAnimateable).OnAnimationDone(animationName);
        }

        public void OnCollisionStay(Collider other)
        {
            foreach (var component in components)
                if (component is ICollider)
                    (component as ICollider).OnCollisionStay(other);
        }

        public void OnCollisionEnter(Collider other)
        {
            foreach (var component in components)
                if (component is ICollider)
                    (component as ICollider).OnCollisionEnter(other);
        }

        public void OnCollisionExit(Collider other)
        {
            foreach (var component in components)
                if (component is ICollider)
                    (component as ICollider).OnCollisionExit(other);
        }

        #endregion
    }
}