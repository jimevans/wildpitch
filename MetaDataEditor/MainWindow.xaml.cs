using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Knuckleball;
using MetaDataSearchResults;

namespace MetaDataEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string artworkFile = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            try
            {
                string searchTerm = searchTermTextBox.Text;
                int seasonNumber = 0;
                if (!string.IsNullOrEmpty(seasonTextBox.Text))
                {
                    seasonNumber = int.Parse(seasonTextBox.Text);
                }

                if (this.tvSeasonsRadioButton.IsChecked.HasValue && this.tvSeasonsRadioButton.IsChecked.Value)
                {
                    if (seasonNumber <= 0)
                    {
                        iTunesTvShowSearchProvider seasonProvider = new iTunesTvShowSearchProvider(searchTerm);
                        SearchResults seasonList = seasonProvider.Search();
                        this.seasonsDataGrid.ItemsSource = seasonList.Items;
                        this.resultsCanvas.Visibility = System.Windows.Visibility.Hidden;
                        this.seasonListCanvas.Visibility = System.Windows.Visibility.Visible;
                    }
                    else
                    {
                        iTunesTvSeasonSearchProvider provider = new iTunesTvSeasonSearchProvider(searchTerm, seasonNumber);
                        this.UpdateMetaData(provider);
                    }
                }
                else
                {
                    iTunesMovieSearchProvider movieProvider = new iTunesMovieSearchProvider(searchTerm);
                    SearchResults movieList = movieProvider.Search();
                }
            }
            finally
            {
                this.Cursor = null;
            }
        }

        private void UpdateMetaData(SearchProvider provider)
        {
            this.resultsCanvas.Visibility = System.Windows.Visibility.Visible;
            this.seasonListCanvas.Visibility = System.Windows.Visibility.Hidden;
            iTunesSeasonMetaData seasonData = new iTunesSeasonMetaData(provider);
            this.artworkFile = seasonData.ArtworkFile;
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = File.OpenRead(this.artworkFile);
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.EndInit();
            this.artworkImage.Source = image;
            image.StreamSource.Close();

            this.albumTextBox.Text = seasonData.SeasonName;
            this.showTextBox.Text = seasonData.ShowName;
            this.seasonNumberTextBox.Text = seasonData.SeasonNumber.ToString();
            this.episodeListDataGrid.ItemsSource = seasonData.EpisodeList;
        }

        private void seasonsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            try
            {
                DataGrid grid = sender as DataGrid;
                if (grid != null && grid.SelectedItems.Count > 0)
                {
                    SearchResultItem x = grid.SelectedItems[0] as SearchResultItem;
                    iTunesTvSeasonDetailsSearchProvider provider = new iTunesTvSeasonDetailsSearchProvider(x.CollectionId);
                    this.UpdateMetaData(provider);
                }
            }
            finally
            {
                this.Cursor = null;
            }
        }

        private void selectFolderButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.ShowDialog();
            if (!string.IsNullOrEmpty(dialog.FileName))
            {
                List<string> foundFiles = new List<string>();
                Regex fileNameRegex = new Regex(@"(\d+)");
                string directory = System.IO.Path.GetDirectoryName(dialog.FileName);
                string[] files = Directory.GetFiles(directory);
                foreach (string file in files)
                {
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(file);
                    Match match = fileNameRegex.Match(fileName);
                    if (match.Success)
                    {
                        int numberCandidate = int.Parse(match.Groups[1].Value);
                        foreach (object item in this.episodeListDataGrid.Items)
                        {
                            FileMetaDataInfo infoItem = item as FileMetaDataInfo;
                            if (infoItem.Metadata.TrackNumber == numberCandidate)
                            {
                                infoItem.LocalFileName = file;
                                infoItem.UpdateFile = true;
                            }
                        }
                    }
                }

                this.episodeListDataGrid.Items.Refresh();
            }
        }

        private void goButton_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            try
            {
                foreach (object item in this.episodeListDataGrid.Items)
                {
                    FileMetaDataInfo infoItem = item as FileMetaDataInfo;
                    if (!string.IsNullOrEmpty(infoItem.LocalFileName) && infoItem.UpdateFile)
                    {
                        MP4File currentFile = MP4File.Open(infoItem.LocalFileName);
                        currentFile.Tags.Album = this.albumTextBox.Text;
                        currentFile.Tags.AlbumArtist = this.showTextBox.Text;
                        currentFile.Tags.Artist = this.showTextBox.Text;
                        currentFile.Tags.TVShow = this.showTextBox.Text;
                        currentFile.Tags.SeasonNumber = int.Parse(this.seasonNumberTextBox.Text);
                        currentFile.Tags.EpisodeNumber = infoItem.Metadata.TrackNumber;
                        currentFile.Tags.Title = infoItem.Metadata.TrackName;
                        currentFile.Tags.TrackNumber = Convert.ToInt16(infoItem.Metadata.TrackNumber);
                        currentFile.Tags.TotalTracks = Convert.ToInt16(infoItem.Metadata.TrackCount);
                        currentFile.Tags.DiscNumber = Convert.ToInt16(infoItem.Metadata.DiscNumber);
                        currentFile.Tags.TotalDiscs = Convert.ToInt16(infoItem.Metadata.DiscCount);
                        currentFile.Tags.Description = infoItem.Metadata.ShortDescription;
                        currentFile.Tags.LongDescription = infoItem.Metadata.LongDescription;
                        currentFile.Tags.EpisodeId = string.Format("S{0}E{1}", this.seasonNumberTextBox.Text, infoItem.Metadata.TrackNumber);

                        System.Drawing.Image artwork = System.Drawing.Image.FromFile(this.artworkFile);
                        currentFile.Tags.Artwork = artwork;

                        if (this.albumTextBox.Text.StartsWith("a ", StringComparison.InvariantCultureIgnoreCase) ||
                            this.albumTextBox.Text.StartsWith("an ", StringComparison.InvariantCultureIgnoreCase) ||
                            this.albumTextBox.Text.StartsWith("the ", StringComparison.InvariantCultureIgnoreCase))
                        {
                            string sortAlbumValue = this.albumTextBox.Text.Substring(this.albumTextBox.Text.IndexOf(' ') + 1);
                            currentFile.Tags.SortAlbum = sortAlbumValue;
                        }

                        if (this.showTextBox.Text.StartsWith("a ", StringComparison.InvariantCultureIgnoreCase) ||
                            this.showTextBox.Text.StartsWith("an ", StringComparison.InvariantCultureIgnoreCase) ||
                            this.showTextBox.Text.StartsWith("the ", StringComparison.InvariantCultureIgnoreCase))
                        {
                            string sortShowValue = this.showTextBox.Text.Substring(this.showTextBox.Text.IndexOf(' ') + 1);
                            currentFile.Tags.SortTVShow = sortShowValue;
                            currentFile.Tags.SortArtist = sortShowValue;
                            currentFile.Tags.SortAlbumArtist = sortShowValue;
                        }


                        if (infoItem.Metadata.TrackName.StartsWith("a ", StringComparison.InvariantCultureIgnoreCase) ||
                            infoItem.Metadata.TrackName.StartsWith("an ", StringComparison.InvariantCultureIgnoreCase) ||
                            infoItem.Metadata.TrackName.StartsWith("the ", StringComparison.InvariantCultureIgnoreCase))
                        {
                            string sortNameValue = infoItem.Metadata.TrackName.Substring(infoItem.Metadata.TrackName.IndexOf(' ') + 1);
                            currentFile.Tags.SortName = sortNameValue;
                        }

                        currentFile.Tags.ContentId = infoItem.Metadata.TrackId;
                        currentFile.Tags.ReleaseDate = infoItem.Metadata.ReleaseDate.ToString("yyyy-MM-dd");
                        currentFile.Tags.MediaType = MediaKind.TVShow;

                        currentFile.Save();

                        infoItem.UpdateFile = false;
                    }
                }

                this.episodeListDataGrid.Items.Refresh();
            }
            finally
            {
                this.Cursor = null;
            }
        }
    }
}
