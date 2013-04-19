// -----------------------------------------------------------------------
// <copyright file="iTunesSeasonDescription.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MetaDataSearchResults;
using System.Drawing;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;

namespace MetaDataEditor
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class iTunesSeasonMetaData
    {
        private List<FileMetaDataInfo> episodeList = new List<FileMetaDataInfo>();
        private string seasonName;
        private string showName;
        private int seasonNumber = 1;
        private string artworkFile = string.Empty;

        public iTunesSeasonMetaData(SearchProvider provider)
        {
            string directory = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            this.artworkFile = System.IO.Path.Combine(directory, "artwork.jpg");
            this.PerformSearch(provider);
        }

        public string SeasonName
        {
            get { return seasonName; }
        }

        public string ShowName
        {
            get { return showName; }
        }

        public int SeasonNumber
        {
            get { return seasonNumber; }
        }

        public string ArtworkFile
        {
            get { return this.artworkFile; }
        }

        public List<FileMetaDataInfo> EpisodeList
        {
            get { return episodeList; }
        }

        public void PerformSearch(SearchProvider provider)
        {
            SearchResults epResults = provider.Search();
            List<SearchResultItem> collectionItems = epResults.Items.FindAll((item) => { return item.WrapperType == "collection"; });
            foreach (SearchResultItem item in epResults.Items.Except(collectionItems))
            {
                this.episodeList.Add(new FileMetaDataInfo(item));
            }

            if (collectionItems.Count > 0)
            {
                SearchResultItem seasonItem = collectionItems[0];
                this.seasonName = seasonItem.CollectionName;
                this.showName = seasonItem.ArtistName;
                Regex seasonNumberMatcher = new Regex(@".*(\d).*");
                if (seasonNumberMatcher.IsMatch(seasonName))
                {
                    this.seasonNumber = int.Parse(seasonNumberMatcher.Matches(seasonName)[0].Groups[1].Value);
                }

                // Look for the 600x600 pixel artwork. This is a hack, and
                // may not work properly forever.
                this.DownloadArtwork(seasonItem.ArtworkUrl.Replace("100x100", "600x600"));
            }
        }

        private bool DownloadArtwork(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            // Check that the remote file was found. The ContentType
            // check is performed since a request for a non-existent
            // image file might be redirected to a 404-page, which would
            // yield the StatusCode "OK", even though the image was not
            // found.
            if ((response.StatusCode == HttpStatusCode.OK ||
                response.StatusCode == HttpStatusCode.Moved ||
                response.StatusCode == HttpStatusCode.Redirect) &&
                response.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
            {

                // if the remote file was found, download oit
                using (Stream inputStream = response.GetResponseStream())
                {
                    using (Stream outputStream = File.OpenWrite(this.artworkFile))
                    {
                        byte[] buffer = new byte[4096];
                        int bytesRead;
                        do
                        {
                            bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                            outputStream.Write(buffer, 0, bytesRead);
                        } while (bytesRead != 0);
                    }
                }

                return true;
            }

            return false;
        }
    }
}
