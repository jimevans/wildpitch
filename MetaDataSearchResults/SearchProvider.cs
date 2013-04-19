// -----------------------------------------------------------------------
// <copyright file="SearchProvider.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MetaDataSearchResults
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class SearchProvider
    {
        public abstract SearchResults Search();

        protected string SanitizeCriteria(string criteria)
        {
            return criteria.Replace(' ', '+');
        }
    }
}
