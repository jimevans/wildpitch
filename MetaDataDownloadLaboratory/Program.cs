using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing;
using Knuckleball;
using System.Xml;
using MetaDataSearchResults;
using Newtonsoft.Json;

namespace iTunesMetaDataDownloader
{
    class Program
    {
        enum FooValues : byte
        {
            Zero = 0,
            One = 1,
            Two = 2
        }

        static void Main(string[] args)
        {
            GetMetadata();
            GetGenreInfo();
            return;
        }

        private static void GetGenreInfo()
        {
            string queryString = "http://itunes.apple.com/WebObjects/MZStoreServices.woa/ws/genres";
            WebClient client = new WebClient();
            string genreQuery = client.DownloadString(queryString);
            object results = JsonConvert.DeserializeObject(genreQuery);
        }

        private static void GetMetadata()
        {
            //Console.Write("Enter the TV Show title: ");
            //string showName = Console.ReadLine().Replace(' ', '+');
            //Console.Write("Enter the season number: ");
            //string season = Console.ReadLine();
            //iTunesTvSeasonDetailsSearchProvider provider = new iTunesTvSeasonDetailsSearchProvider(showName, int.Parse(season));
            //SearchResults epResults = provider.Search();
            //foreach (SearchResultItem episode in epResults.Items)
            //{
            //    if (episode.WrapperType == "track")
            //    {
            //        Console.WriteLine(episode.ToString());
            //    }
            //}
            //Console.ReadLine();
        }
    }
}
