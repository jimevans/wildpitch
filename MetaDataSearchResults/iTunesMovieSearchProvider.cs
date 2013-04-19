using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace MetaDataSearchResults
{
    public class iTunesMovieSearchProvider : SearchProvider
    {
        private const string TvShowSeasonSearchString = "http://itunes.apple.com/search?term={0}&media=movie&entity=movie&attribute=movieTerm";

        private string movieName = string.Empty;

        public iTunesMovieSearchProvider(string movieName)
        {
            this.movieName = this.SanitizeCriteria(movieName);
        }

        public override SearchResults Search()
        {
            string queryString = string.Format(TvShowSeasonSearchString, movieName);
            WebClient client = new WebClient();
            string movieQuery = client.DownloadString(queryString);
            SearchResults results = JsonConvert.DeserializeObject<SearchResults>(movieQuery);
            return results;
        }
    }
}
