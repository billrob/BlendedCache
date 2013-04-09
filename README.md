BlendedCache
============

Blended Cache Framework for .net

Basic Principles:

* 3 layers of caching:
  * Context (Thread based)
  * Volatile (in memory)
  * LongTerm (out of process)
* Wraps up interaction of 3 layers, back filling.  eg found in longTerm, put in context, volatile
* Provides flush mode support for forcing refreshes at various layers.
* Allows type load configuration to auto-load items.
* LongTerm supports Refresh and Absolute expiration time.  Also background refresh support with TypeLoadConfig

Sample Context and Volatile for a website.

var contextCache = new HttpContextCache(); // IContextCache<br/>
var volatileCache = new RuntimeMemoryCachingVolatileCache();<br/>
var configuration = new BlendedCacheConfiguration(); <br/>

//fancy ioc stuff here.<br/>
return new BlendedCache(contextCache, volatileCache, NullLongTermCache.NullInstance, configuration);<br/>


Building Source
============

NuGet auto-download is required to be configured to supply any third-parties.

1. Tools -> Options -> Package Manager
2. Check the box that says "Allow NuGet to download missing packages during build"

