using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace MetaDataSearchResults
{
    public class iTunesTvSeasonDetailsSearchProvider : SearchProvider
    {
        private const string TvEpisodeSearchString = "http://itunes.apple.com/lookup?id={0}&entity=tvEpisode&attribute=tvSeasonTerm";

        private int seasonId = 0;

        public iTunesTvSeasonDetailsSearchProvider(int seasonId)
        {
            this.seasonId = seasonId;
        }

        public override SearchResults Search()
        {
            WebClient client = new WebClient();
            string queryString = string.Format(TvEpisodeSearchString, seasonId);
            string episodeQuery = client.DownloadString(queryString);
            SearchResults results = JsonConvert.DeserializeObject<SearchResults>(episodeQuery);
            results.Items.Sort((first, second) => { return first.TrackNumber - second.TrackNumber; });
            return results;
        }
    }
}
