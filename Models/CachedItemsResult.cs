using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CachingTokenAndAbsoluteExpiration.Models
{
    public class CachedItemsResult
    {
        public CachedItem Relative { get; set; }
        public CachedItem Absolute { get; set; }
    }
}
