using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace MetaDataSearchResults
{
    [JsonObject]
    public class SearchResults
    {
        [JsonProperty("resultCount")]
        public int Count { get; set; }
        [JsonProperty("results")]
        public List<SearchResultItem> Items { get; set; }
    }
}
