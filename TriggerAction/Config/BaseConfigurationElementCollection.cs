/*
 * https://gist.github.com/danielgreen/5772822
 * Code to create a custom configuration section with child collections
 */
using System.Collections.Generic;
using System.Configuration;

namespace TriggerAction.Config
{
    public abstract class BaseConfigurationElementCollection<TKey, TElement> 
        : ConfigurationElementCollection, IEnumerable<TElement> 
        where TElement : ConfigurationElement, new()
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new TElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return GetElementKey((TElement)element);
        }

        protected abstract TKey GetElementKey(TElement element);

        IEnumerator<TElement> IEnumerable<TElement>.GetEnumerator()
        {
            foreach (TElement element in this)
            {
                yield return element;
            }
        }

        public virtual void Add(TElement configurationElement)
        {
            BaseAdd(configurationElement);
        }

        public virtual void Remove(TKey key)
        {
            BaseRemove(key);
        }

        protected virtual TElement Get(TKey key)
        {
            return (TElement)BaseGet(key);
        }

        protected virtual TElement Get(int index)
        {
            return (TElement)BaseGet(index);
        }

        public TElement this[TKey key]
        {
            get { return Get(key); }
        }

        public TElement this[int index]
        {
            get { return Get(index); }
        }
    }
}
