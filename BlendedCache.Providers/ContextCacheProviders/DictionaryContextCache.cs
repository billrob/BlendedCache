using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache.Providers
{
	/// <summary>
	/// Provides a dictionary based context based cache.  Very similar to how HttpContext.Items collection works.
	/// It keeps a private field of dictionary so be aware of how the instances of this class are shared.  If using 
	/// IoC, you want to ensure this is created new for each httpcontext or thread based IoC.
	/// </summary>
	public class DictionaryContextCache : IContextCache, IDisposable
	{
		private Dictionary<string, object> __items = new Dictionary<string, object>();
		private Dictionary<string, object> _items
		{
			get
			{
				if (__items == null)
					__items = new Dictionary<string, object>();

				return _items;
			}
		}

		//todo:0 pull over final docs.
		void IContextCache.Set<T>(string key, T value)
		{
			_items[key] = value;
		}

		T IContextCache.Get<T>(string key)
		{
			object obj;
			return _items.TryGetValue(key, out obj) ? (T)obj : default(T);
		}

		void IContextCache.Clear()
		{
			Dispose();
		}

		void IContextCache.Remove(string key)
		{
			_items.Remove(key);
		}

		public void Dispose() //public for ease.
		{
			__items.Clear();
			__items = null;
		}
	}
}
