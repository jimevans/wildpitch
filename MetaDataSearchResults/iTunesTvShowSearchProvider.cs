using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace MetaDataSearchResults
{
    public class iTunesTvShowSearchProvider : SearchProvider
    {
        private const string TvShowSearchString = "http://itunes.apple.com/search?term={0}&media=tvShow&entity=tvSeason&attribute=tvSeasonTerm";

        private string showName = string.Empty;

        public iTunesTvShowSearchProvider(string showName)
        {
            this.showName = this.SanitizeCriteria(showName);
        }

        public override SearchResults Search()
        {
            string queryString = string.Format(TvShowSearchString, showName);
            WebClient client = new WebClient();
            string seasonQuery = client.DownloadString(queryString);
            SearchResults results = JsonConvert.DeserializeObject<SearchResults>(seasonQuery);
            results.Items.Sort((first, second) => { return string.Compare(first.CollectionName, second.CollectionName); });
            return results;
        }
    }
}
