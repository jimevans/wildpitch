// -----------------------------------------------------------------------
// <copyright file="UpperCaseUTF8Encoding.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace System.Text
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UpperCaseUTF8Encoding : UTF8Encoding
    {
        public override string WebName
        {
            get
            {
                return base.WebName.ToUpperInvariant();
            }
        }

        public static UpperCaseUTF8Encoding UpperCaseUTF8
        {
            get
            {
                return new UpperCaseUTF8Encoding();
            }
        }
    }
}
