using DynamicRouting.Kentico.Wildcards.BaseClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Dynamic_Routings
{
    class Program
    {
        static void Main(string[] args)
        {
            GetUrlMatch("/wills-and-probate/help-and-advice/wills", "-1");
        }

        private static MatchedUrl GetUrlMatch(string relativeUrl, string cultureCode, DataTable possibleUrls = null)
        {
            var PossibleUrlPatterns = new DataTable();
            PossibleUrlPatterns.Columns.Add("possibleUrl", typeof(string));
            PossibleUrlPatterns.Columns.Add("UrlSlugNodeId", typeof(int));
            PossibleUrlPatterns.Columns.Add("ClassName", typeof(string));
            DataRow row1 = PossibleUrlPatterns.NewRow();
            DataRow row2 = PossibleUrlPatterns.NewRow();
            row1["possibleUrl"] = "/wills-and-probate/help-and-advice/{item1}/{item2?}";
            row1["UrlSlugNodeId"] = 443;
            row1["ClassName"] = "TheLawSuperStore.HelpAndAdviceHub";
            row2["possibleUrl"] = "/wills-and-probate/help-and-advice/{item1}";
            row2["UrlSlugNodeId"] = 443;
            row2["ClassName"] = "TheLawSuperStore.HelpAndAdviceHub";
            PossibleUrlPatterns.Rows.Add(row1);
            PossibleUrlPatterns.Rows.Add(row2);
            var urlMatchup = new UrlByWildcardCollection();
            if (PossibleUrlPatterns.Rows.Count > 0)
            {
                foreach (DataRow dr in PossibleUrlPatterns.Rows)
                {
                    int documentID = dr.Field<int>("UrlSlugNodeId");
                    string urlPattern = dr.Field<string>("possibleUrl");
                    string className = dr.Field<string>("ClassName");
                    urlMatchup.AddUrl(urlPattern, new KenticoData(documentID, className));
                }

                var matchedUrl = urlMatchup.FindBaseUrl(relativeUrl);
                if (matchedUrl.UrlBreakdown == null || !matchedUrl.HasMatch)
                {
                    return null;
                }
                return matchedUrl;
            }
            return null;
        }
    }
}
