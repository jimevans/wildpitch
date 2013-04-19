// -----------------------------------------------------------------------
// <copyright file="FileMetaDataInfo.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MetaDataEditor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
using MetaDataSearchResults;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class FileMetaDataInfo
    {
        private string localFileName = string.Empty;
        private bool updateFile = false;
        private SearchResultItem metadata;

        public FileMetaDataInfo(SearchResultItem item)
        {
            this.metadata = item;
        }

        public string LocalFileName
        {
            get { return localFileName; }
            set { localFileName = value; }
        }

        public bool UpdateFile
        {
            get { return updateFile; }
            set { updateFile = value; }
        }

        public SearchResultItem Metadata
        {
            get { return metadata; }
        }
    }
}
