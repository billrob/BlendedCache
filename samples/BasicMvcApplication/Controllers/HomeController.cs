﻿using BlendedCache;
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
		public ActionResult Index()
		{
			var cacheKey = "HomeController.Index.SampleData";
			var cache = GetCache();
			var sb = new StringBuilder();

			var firstMetric = BlendedCacheMetricsStore.GetCacheMetrics(cacheKey) ?? new Metrics();

			if (cache.Get<SampleData>(cacheKey) == null)
			{
				sb.Append("<p>data did not exist</p>");
				cache.Set(cacheKey, new SampleData("Sylvester Martin"));
			}
			else
			{
				sb.Append("<p>The data did exist.</p>");
			}

			var secondMetric = BlendedCacheMetricsStore.GetCacheMetrics(cacheKey);

			var data = cache.Get<SampleData>(cacheKey);
			sb.Append("<p>Date Created: " + data.DateCreated.ToString() + "</p>");

			cache.Get<SampleData>(cacheKey);
			var thirdMetric = BlendedCacheMetricsStore.GetCacheMetrics(cacheKey);

			sb.Append("<p>Refresh the page to see it in action and when it expires.</p>");

			sb.Append("<p>Metrics of first, second, and third call</p>");
			sb.Append((new List<Metrics>() { firstMetric, secondMetric, thirdMetric }).ToHtmlString());
			sb.Append("<p>On app start, should be 0,1,1 for long term, </p>");
			sb.Append("<p>Volatile should hits and lookups should increase by 1 with an occasional miss.</p>");


			return Content(sb.ToString());
		}

		private static IBlendedCache GetCache()
		{
			var contextCache = new HttpContextCache();
			var volatileCache = new RuntimeMemoryCachingVolatileCache();
			var configuration = new BlendedCacheConfiguration(); // this should be driven by the web.config

			return new BlendedCache.BlendedCache(contextCache, volatileCache, NullLongTermCache.NullInstance, configuration);
		}

		private class SampleData
		{
			public SampleData(string name)
			{
				Name = name;
				DateCreated = DateTime.UtcNow;

			}
			public string Name { get; private set; }
			public DateTime DateCreated { get; private set; }
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
				sb.AppendFormat("<td>{0}</td>", metric.CacheKey);
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