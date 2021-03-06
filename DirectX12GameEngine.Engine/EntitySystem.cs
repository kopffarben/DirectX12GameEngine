﻿using System;
using System.Collections.Generic;
using DirectX12GameEngine.Games;

namespace DirectX12GameEngine.Engine
{
    public abstract class EntitySystem : IDisposable
    {
        public EntitySystem(Type mainType, params Type[] requiredAdditionalTypes)
        {
            MainType = mainType;
            RequiredTypes = requiredAdditionalTypes;
        }

        public EntityManager? EntityManager { get; internal set; }

        public Type MainType { get; }

        public Type[] RequiredTypes { get; }

        public int Order { get; protected set; }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(GameTime gameTime)
        {
        }

        public virtual void Dispose()
        {
        }

        protected internal abstract void ProcessEntityComponent(EntityComponent component, Entity entity, bool forceRemove);

        internal bool Accept(Type type)
        {
            return MainType.IsAssignableFrom(type);
        }
    }

    public abstract class EntitySystem<TComponent> : EntitySystem where TComponent : EntityComponent
    {
        public EntitySystem(params Type[] requiredAdditionalTypes)
            : base(typeof(TComponent), requiredAdditionalTypes)
        {
        }

        protected HashSet<TComponent> Components { get; } = new HashSet<TComponent>();

        protected internal override void ProcessEntityComponent(EntityComponent component, Entity entity, bool forceRemove)
        {
            if (!(component is TComponent entityComponent)) throw new ArgumentException("The entity component must be assignable to TComponent", nameof(component));

            bool entityMatch = !forceRemove && EntityMatch(entity);
            bool entityAdded = Components.Contains(entityComponent);

            if (entityMatch && !entityAdded)
            {
                Components.Add(entityComponent);
                OnEntityComponentAdded(entityComponent);
            }
            else if (!entityMatch && entityAdded)
            {
                Components.Remove(entityComponent);
                OnEntityComponentRemoved(entityComponent);
            }
        }

        protected virtual void OnEntityComponentAdded(TComponent component)
        {
        }

        protected virtual void OnEntityComponentRemoved(TComponent component)
        {
        }

        private bool EntityMatch(Entity entity)
        {
            if (RequiredTypes.Length == 0) return true;

            List<Type> remainingRequiredTypes = new List<Type>(RequiredTypes);

            foreach (EntityComponent component in entity.Components)
            {
                remainingRequiredTypes.RemoveAll(t => t.IsAssignableFrom(component.GetType()));

                if (remainingRequiredTypes.Count == 0) return true;
            }

            return false;
        }
    }
}
