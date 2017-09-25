using System;
using System.Data.Entity.Infrastructure;

namespace Provider.EntityFramework.CustomServices
{
    internal class DbModelCacheKey : IDbModelCacheKey
    {
        #region fields

        private readonly Type ContextType;
        private readonly string ProviderName;
        private readonly Type ProviderType;
        private readonly string CustomKey;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DbModelCacheKey" /> class.
        /// </summary>
        /// <param name="contextType">Type of the context.</param>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="providerType">Type of the provider.</param>
        /// <param name="customKey">The custom key.</param>
        public DbModelCacheKey(Type contextType, string providerName, Type providerType, string customKey)
        {
            this.ContextType = contextType;
            this.CustomKey = customKey;
            this.ProviderName = providerName;
            this.ProviderType = providerType;
        }

        #endregion

        /// <summary>
        ///     Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        /// <remarks>Modified from decompiled DefaultModelCacheKey in System.Data.Entity.Internal</remarks>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (object.ReferenceEquals(this, obj))
                return true;

            var compareTo = obj as DbModelCacheKey;
            if (compareTo == null)
                return false;

            return this.Equals(compareTo);
        }


        /// <summary>
        ///     Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        /// <remarks>Taken from decompiled DefaultModelCacheKey in System.Data.Entity.Internal</remarks>
        public override int GetHashCode()
        {
            throw new NotImplementedException();
            //return
            //    typeof(DBContext<>).GetHashCode() * 397
            //    ^ this.ProviderName.GetHashCode()
            //    ^ this.ProviderType.GetHashCode()
            //    ^ (!string.IsNullOrWhiteSpace(this.CustomKey) ? this.CustomKey.GetHashCode() : 0);
        }

        /// <summary>
        ///     Equalses the specified other.
        /// </summary>
        /// <param name="compareTo">The other.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool Equals(DbModelCacheKey compareTo)
        {
            throw new NotImplementedException();
            //// both this DbModelCacheKey and the compareTo DbModelCacheKey must be generic for this method to work
            //if (!this.ContextType.IsGenericType || !compareTo.ContextType.IsGenericType)
            //    return false;

            //var thisIsAssignable = this.ContextType.IsAssignableToGenericType(typeof(DBContext<>));

            //var compareToIsAssignable = compareTo.ContextType.IsAssignableToGenericType(typeof(DBContext<>));

            ////Both this and compereTo must be derived from DBContext<>
            //if (!thisIsAssignable || !compareToIsAssignable)
            //    return false;

            ////Return true if this ProviderName, ProviderType and Custom key match compareTo ProviderName, ProviderType and Custom key respectively
            //if
            //    (
            //    string.Equals(this.ProviderName, compareTo.ProviderName) &&
            //    object.Equals(this.ProviderType, compareTo.ProviderType)
            //    )
            //    return string.Equals(this.CustomKey, compareTo.CustomKey);

            //return false;
        }
    }
}