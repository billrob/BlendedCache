using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// class used to alleviate the complexity for key/cachekey/data item management.
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TData"></typeparam>
	internal sealed class KeyedItemLookup<TKey, TData>
	{
		public TKey LookupKey;
		public string CacheKey;
		public CachedItemMetrics Metrics;
		public TData CachedItem;

		private bool Equals(KeyedItemLookup<TKey, TData> other)
		{
			return EqualityComparer<TKey>.Default.Equals(LookupKey, other.LookupKey);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj is KeyedItemLookup<TKey, TData> && Equals((KeyedItemLookup<TKey, TData>)obj);
		}

		public override int GetHashCode()
		{
			return EqualityComparer<TKey>.Default.GetHashCode(LookupKey);
		}
	}

	/// <summary>
	/// Class containing a list of keys to lookup.
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TData"></typeparam>
	internal class KeyedItemLookupHashSet<TKey, TData> : HashSet<KeyedItemLookup<TKey, TData>>
	{
		/// <summary>
		/// Will get the items remaining to be found.  Basically selects all with the CachedItem null
		/// </summary>
		/// <returns></returns>
		public IEnumerable<KeyedItemLookup<TKey, TData>> GetRemainingList()
		{
			return this.Where(x => x.CachedItem == null);
		}
	}

	/// <summary>
	/// Class containing a list of keys to lookup.
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TData"></typeparam>
	internal class KeyedItemLookupList<TKey, TData> : List<KeyedItemLookup<TKey, TData>>
	{
		/// <summary>
		/// Will get the items remaining to be found.  Basically selects all with the CachedItem null
		/// </summary>
		/// <returns></returns>
		public IEnumerable<KeyedItemLookup<TKey, TData>> GetRemainingList()
		{
			return this.Where(x => x.CachedItem == null);
		}
	}
}
