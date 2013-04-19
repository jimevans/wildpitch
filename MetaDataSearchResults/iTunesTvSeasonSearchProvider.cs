using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace MetaDataSearchResults
{
    public class iTunesTvSeasonSearchProvider : SearchProvider
    {
        private const string TvSeasonDetailsSearchString = "http://itunes.apple.com/search?term={0}+season+{1}&media=tvShow&entity=tvSeason&attribute=tvSeasonTerm";
        private const string TvEpisodeSearchString = "http://itunes.apple.com/lookup?id={0}&entity=tvEpisode&attribute=tvSeasonTerm";

        private string showName = string.Empty;
        private int season = 0;

        public iTunesTvSeasonSearchProvider(string showName, int season)
        {
            if (string.IsNullOrEmpty(showName))
            {
                throw new ArgumentException("Show name must not be null", "showName");
            }

            if (season <= 0)
            {
                throw new ArgumentException("Season number must be greater than zero", "season");
            }

            this.showName = this.SanitizeCriteria(showName);
            this.season = season;
        }

        public override SearchResults Search()
        {
            string queryString = string.Format(TvSeasonDetailsSearchString, showName, season);
            WebClient client = new WebClient();
            string seasonQuery = client.DownloadString(queryString);
            SearchResults results = JsonConvert.DeserializeObject<SearchResults>(seasonQuery);
            int seasonId = results.Items[0].CollectionId;
            iTunesTvSeasonDetailsSearchProvider childProvider = new iTunesTvSeasonDetailsSearchProvider(seasonId);
            return childProvider.Search();
        }
    }
}
