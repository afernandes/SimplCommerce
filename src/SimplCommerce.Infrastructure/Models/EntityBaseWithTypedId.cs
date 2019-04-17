using System;
using System.Collections.Generic;
using System.Reflection;

namespace SimplCommerce.Infrastructure.Models
{
    /// <summary>
    /// Basic implementation of IEntityBase interface.
    /// An entity can inherit this class of directly implement to IEntityBase interface.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity</typeparam>
    [Serializable]
    public abstract class EntityBase<TPrimaryKey> : ValidatableObject, IEntity<TPrimaryKey>
    {
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        public virtual TPrimaryKey Id { get; set; }

        /// <summary>
        /// Checks if this entity is transient (it has not an Id).
        /// </summary>
        /// <returns>True, if this entity is transient</returns>
        public virtual bool IsTransient()
        {
            if (EqualityComparer<TPrimaryKey>.Default.Equals(Id, default(TPrimaryKey)))
            {
                return true;
            }

            //Workaround for EF Core since it sets int/long to min value when attaching to dbcontext
            if (typeof(TPrimaryKey) == typeof(int))
            {
                return Convert.ToInt32(Id) <= 0;
            }

            if (typeof(TPrimaryKey) == typeof(long))
            {
                return Convert.ToInt64(Id) <= 0;
            }

            return false;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is EntityBase<TPrimaryKey>))
            {
                return false;
            }

            //Same instances must be considered as equal
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            //Transient objects are not considered as equal
            var other = (EntityBase<TPrimaryKey>)obj;
            if (IsTransient() && other.IsTransient())
            {
                return false;
            }

            //Must have a IS-A relation of types or must be same type
            var typeOfThis = GetType();
            var typeOfOther = other.GetType();
            if (!typeOfThis.GetTypeInfo().IsAssignableFrom(typeOfOther) && !typeOfOther.GetTypeInfo().IsAssignableFrom(typeOfThis))
            {
                return false;
            }            

            return Id.Equals(other.Id);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            if (Id == null)
            {
                return 0;
            }

            return Id.GetHashCode();
        }

        /// <inheritdoc/>
        public static bool operator ==(EntityBase<TPrimaryKey> left, EntityBase<TPrimaryKey> right)
        {
            if (Equals(left, null))
            {
                return Equals(right, null);
            }

            return left.Equals(right);
        }

        /// <inheritdoc/>
        public static bool operator !=(EntityBase<TPrimaryKey> left, EntityBase<TPrimaryKey> right)
        {
            return !(left == right);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"[{GetType().Name} {Id}]";
        }
    }
}
