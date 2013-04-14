using BlendedCache;
using BlendedCache.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BasicMvcApplication.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Diag(int? id)
		{
			id = id ?? 42;
			var cacheKey = "HomeController.Index.SampleData." + id;
			var cache = GetCache();
			var sb = new StringBuilder();

			var firstMetric = BlendedCacheMetricsStore.GetCachedItemMetrics<SampleData,string>(cacheKey) ?? new Metrics();

			var data = (SampleData)null;
			if (cache.Get<SampleData>(cacheKey) == null)
			{
				sb.Append("<p>data did not exist</p>");
				data = DataBase.GetSampleData(id.Value);
				cache.Set(cacheKey, data);
			}
			else
			{
				sb.Append("<p>The data did exist.</p>");
			}

			var secondMetric = BlendedCacheMetricsStore.GetCachedItemMetrics<SampleData, string>(cacheKey);

			data = cache.Get<SampleData>(cacheKey);
			sb.Append("<p>Date Created: " + data.DateCreated.ToString() + "</p>");

			cache.Get<SampleData>(cacheKey);
			var thirdMetric = BlendedCacheMetricsStore.GetCachedItemMetrics<SampleData, string>(cacheKey);

			sb.Append("<p>Refresh the page to see it in action and when it expires.</p>");

			sb.Append("<p>Metrics of first, second, and third call</p>");
			sb.Append((new List<Metrics>() { firstMetric, secondMetric, thirdMetric }).ToHtmlString());
			sb.Append("<p>On app start, should be 0,1,1 for long term, </p>");
			sb.Append("<p>Volatile should hits and lookups should increase by 1 with 5 second miss.</p>");
			sb.Append("<p>LongTerm should hits and lookups should increase by 1 with 10 second miss. (once long term is completed)</p>");

			return Content(sb.ToString());
		}

		public ActionResult Index(int? id)
		{
			id = id ?? 42;

			var cache = GetCache();
			var cacheKey = "SampleData." + id;

			var data = cache.Get<SampleData>(cacheKey);

			if(data == null)
			{
				ViewData["CacheMiss"] = new object();
				data = DataBase.GetSampleData(id.Value);
				cache.Set(cacheKey, data);
			}

			return View(data);
		}

		public object SampleGetByCacheKey(int id)
		{
			var configuration = new BlendedCacheConfiguration();
			configuration.DefaultCacheKeyConverter = DefaultCacheKeyConverter.MyLookupKeyIsTheCacheKey;

			var cache = new BlendedCache.BlendedCache(new HttpContextCache(), new RuntimeMemoryCachingVolatileCache(), NullLongTermCache.NullInstance, configuration);

			var cacheKey = "SD." + id;

			var dataByInt = cache.Get<SampleData>(cacheKey);

			var dataByString = cache.Get<SampleData>(cacheKey);

			//complex really doesn't transfer, but it would be some string concat.
			//var dataByComplex = cache.Get<SampleData, SampleComplexKey>(new SampleComplexKey());

			return dataByInt;
		}

		public object SampleGetByPrimary(int? id)
		{
			var cache = new BlendedCache.BlendedCache(new HttpContextCache(), new RuntimeMemoryCachingVolatileCache(), NullLongTermCache.NullInstance, new BlendedCacheConfiguration());

			id = id ?? 42;

			var dataByInt = cache.Get<SampleData>(id.Value);

			var dataByString = cache.Get<SampleData>(id.Value.ToString());

			var dataByComplex = cache.Get<SampleData, SampleComplexKey>(new SampleComplexKey());

			return dataByInt;
		}

		public ActionResult SampleGetByMulti()
		{
			var cache = new BlendedCache.BlendedCache(new HttpContextCache(), new RuntimeMemoryCachingVolatileCache(), NullLongTermCache.NullInstance, new BlendedCacheConfiguration());

			var ids = new List<int>() { 1, 2, 5 };

			//notice being set one at a time.
			ids.ForEach(x => cache.Set(x, DataBase.GetSampleData(x)));
			
			//pulled in one trip.
			var list = cache.Get<SampleData>(ids);




			return Content(list.Count.ToString());
		}

		private class SampleComplexKey
		{
			public int Id { get; set; }
			public Guid Guid { get; set; }
		}

		private static ILongTermCache _dictionaryLongTermCache = new DictionaryLongTermCache();
		private static IBlendedCache GetCache()
		{
			var contextCache = new HttpContextCache();
			var volatileCache = new RuntimeMemoryCachingVolatileCache();
			var longTermCache = new DictionaryLongTermCache();
			var configuration = new BlendedCacheConfiguration()
				{
					DefaultCacheTimeout = new DefaultCacheTimeout()
					{
						VolatileTimeoutInSeconds = 5,
						LongTermTimeoutInSeconds = 10,
					}
				}; // this could be driven by the web.config

			return new BlendedCache.BlendedCache(contextCache, volatileCache, _dictionaryLongTermCache, configuration);
		}
	}

	internal static class HomeControllerExtensions
	{
		public static string ToHtmlString(this List<Metrics> metrics)
		{
			var sb = new StringBuilder();


			sb.Append("<table><tr>");
			sb.Append("<th>CacheKey</th>");
			sb.Append("<th>VolatileCacheHits</th>");
			sb.Append("<th>VolatileCacheLookUps</th>");
			sb.Append("<th>VolatileCacheMisses</th>");
			sb.Append("<th>LongTermCacheHits</th>");
			sb.Append("<th>LongTermCacheLookUps</th>");
			sb.Append("<th>LongTermCacheMisses</th>");
			sb.Append("<th>DateCreated</th>");
			sb.Append("<th>FirstLoaded</th>");
			sb.Append("<th>LastLoaded</th>");
			sb.Append("<th>TimesFlushed</th>");
			sb.Append("<th>TimeInBackgroundLoad</th>");
			sb.Append("<th>TimeInLoad</th>");
			sb.Append("<th>TimesBackgroundLoadFailed</th>");
			sb.Append("<th>TimesBackgroundLoaded</th>");
			sb.Append("<th>TimesLoaded</th>");
			
			sb.Append("</tr>");

			foreach (var metric in metrics)
			{

				sb.Append("<tr>");
				sb.AppendFormat("<td>{0}</td>", metric.LookupKey);
				sb.AppendFormat("<td>{0}</td>", metric.VolatileCacheHits);
				sb.AppendFormat("<td>{0}</td>", metric.VolatileCacheLookUps);
				sb.AppendFormat("<td>{0}</td>", metric.VolatileCacheMisses);
				sb.AppendFormat("<td>{0}</td>", metric.LongTermCacheHits);
				sb.AppendFormat("<td>{0}</td>", metric.LongTermCacheLookUps);
				sb.AppendFormat("<td>{0}</td>", metric.LongTermCacheMisses);
				sb.AppendFormat("<td>{0}</td>", metric.DateCreated);
				sb.AppendFormat("<td>{0}</td>", metric.FirstLoaded);
				sb.AppendFormat("<td>{0}</td>", metric.LastLoaded);
				sb.AppendFormat("<td>{0}</td>", metric.TimesFlushed);
				sb.AppendFormat("<td>{0}</td>", metric.TimeInBackgroundLoad);
				sb.AppendFormat("<td>{0}</td>", metric.TimeInLoad);
				sb.AppendFormat("<td>{0}</td>", metric.TimesBackgroundLoadFailed);
				sb.AppendFormat("<td>{0}</td>", metric.TimesBackgroundLoaded);
				sb.AppendFormat("<td>{0}</td>", metric.TimesLoaded);
				sb.Append("</tr>");

			}

			sb.Append("</table>");

			return sb.ToString();
		}
	}
}