namespace Kernel.Configuration
{
	using System;
	using System.Configuration;

    /// <summary>
    /// Base class for ConfigurationElementCollection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AbstractConfigurationElementCollection<T> : ConfigurationElementCollection
        where T : AbstractConfigurationElement, new()
    {
        /// <summary>
        /// Gets the type of the <see cref="T:System.Configuration.ConfigurationElementCollection"/>.
        /// </summary>
        /// <value></value>
        /// <returns>The <see cref="T:System.Configuration.ConfigurationElementCollectionType"/> of this collection.</returns>
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        /// <summary>
        /// Gets or sets at the specified index.
        /// </summary>
        /// <value></value>
        public T this[int index]
        {
            get
            {
                return (T)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null) BaseRemoveAt(index);
                BaseAdd(index, value);
            }
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <value></value>
        public new T this[string key]
        {
            get
            {
                return (T)base.BaseGet(key);
            }
        }

        /// <summary>
        /// Adds the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        public void Add(T element)
        {
            BaseAdd(element);
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            BaseClear();
        }

        /// <summary>
        /// When overridden in a derived class, creates a new <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </summary>
        /// <returns>
        /// A new <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new T();
        }

        /// <summary>
        /// Gets the element key for a specified configuration element when overridden in a derived class.
        /// </summary>
        /// <param name="element">The <see cref="T:System.Configuration.ConfigurationElement"/> to return the key for.</param>
        /// <returns>
        /// An <see cref="T:System.Object"/> that acts as the key for the specified <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((T)element).Name;
        }

        /// <summary>
        /// Determines whether [contains] [the specified name].
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        /// 	<c>true</c> if [contains] [the specified name]; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(string name)
        {
            return this[name] != null;
        }

        /// <summary>
        /// Removes the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        public void Remove(T element)
        {
            BaseRemove(element.Name);
        }

        /// <summary>
        /// Removes the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        public void Remove(string name)
        {
            BaseRemove(name);
        }

        /// <summary>
        /// Removes at.
        /// </summary>
        /// <param name="index">The index.</param>
        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public T GetItem(string name)
        {
            return GetItem(name, true);
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="throwExceptionOnNull">if set to <c>true</c> [throw exception on null].</param>
        /// <returns></returns>
        public T GetItem(string name, bool throwExceptionOnNull)
        {
            if (this[name] == null && throwExceptionOnNull)
            {
                throw new ConfigurationEntityNotFoundException(name, this.ElementName);
            }

            return this[name];
        }
    }
}