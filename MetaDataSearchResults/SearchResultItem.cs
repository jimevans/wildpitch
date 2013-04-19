using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace MetaDataSearchResults
{
    [JsonObject]
    public class SearchResultItem
    {
        [JsonProperty("wrapperType")]
        public string WrapperType { get; set; }
        
        [JsonProperty("collectionType")]
        public string CollectionType { get; set; }

        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("artistId")]
        public int ArtistId { get; set; }
        
        [JsonProperty("collectionId")]
        public int CollectionId { get; set; }
        
        [JsonProperty("trackId")]
        public int TrackId { get; set; }

        [JsonProperty("artistName")]
        public string ArtistName { get; set; }

        [JsonProperty("copyright")]
        public string Copyright { get; set; }

        [JsonProperty("collectionName")]
        public string CollectionName { get; set; }

        [JsonProperty("trackName")]
        public string TrackName { get; set; }

        [JsonProperty("artworkUrl100")]
        public string ArtworkUrl { get; set; }
        
        [JsonProperty("releaseDate")]
        public DateTime ReleaseDate { get; set; }

        [JsonProperty("trackExplicitness")]
        public string TrackExplicitness { get; set; }

        [JsonProperty("discCount")]
        public int DiscCount { get; set; }
        
        [JsonProperty("discNumber")]
        public int DiscNumber { get; set; }
        
        [JsonProperty("trackCount")]
        public int TrackCount { get; set; }
        
        [JsonProperty("trackNumber")]
        public int TrackNumber { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("primaryGenreName")]
        public string PrimaryGenreName { get; set; }

        [JsonProperty("contentAdvisoryRating")]
        public string ContentAdvisoryRating { get; set; }

        [JsonProperty("shortDescription")]
        public string ShortDescription { get; set; }
        
        [JsonProperty("longDescription")]
        public string LongDescription { get; set; }
        
        public override string ToString()
        {
            return string.Format("Id: {0}, Number: {1}, Title: {2}", this.TrackId, this.TrackNumber, this.TrackName);
        }
    }
}
