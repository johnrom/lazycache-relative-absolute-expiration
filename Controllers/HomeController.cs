using System;
using Microsoft.AspNetCore.Mvc;
using CachingTokenAndAbsoluteExpiration.Models;
using Microsoft.Extensions.Caching.Memory;
using LazyCache;

namespace CachingTokenAndAbsoluteExpiration.Controllers
{

    public class HomeController : Controller
    {
        const string CACHED_ITEM_RELATIVE_KEY = "cachedItemRelative";
        const string CACHED_ITEM_ABSOLUTE_KEY = "cachedItemAbsolute";

        private readonly IAppCache _cache;

        public HomeController(
            IAppCache cache
        ) {
            _cache = cache;
        }

        public IActionResult Index()
        {
            // Adding an expiration token allows us to trigger expiration, 
            // to ensure that expiration itself is still working.
            var relative = _cache.GetOrAdd(CACHED_ITEM_RELATIVE_KEY, (entry) => {
                // expire immediately
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(1);

                return new CachedItem
                {
                    Created = DateTime.Now,
                };
            });

            var absolute = _cache.GetOrAdd(CACHED_ITEM_ABSOLUTE_KEY, (entry) => {
                // expire immediately
                entry.AbsoluteExpiration = DateTime.UtcNow + TimeSpan.FromSeconds(1);

                return new CachedItem
                {
                    Created = DateTime.Now,
                };
            });

            return View("Index", new CachedItemsResult
            {
                Relative = relative,
                Absolute = absolute,
            });
        }

        public IActionResult Refresh()
        {
            _cache.Remove(CACHED_ITEM_RELATIVE_KEY);
            _cache.Remove(CACHED_ITEM_ABSOLUTE_KEY);

            return RedirectToAction("Index");
        }
    }
}
