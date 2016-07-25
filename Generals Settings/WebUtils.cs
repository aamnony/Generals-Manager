using System.Collections.Generic;
using System.Net;
using System.Web.Script.Serialization;

namespace Generals_Manager
{
    internal class WebUtils
    {
        private const string JSON_URL = "http://jsonblob.com/api/jsonBlob/56bde9b0e4b01190df4ea9a0";

        /// <summary>
        /// Downloads a 2 dimensional string array. First row contains current maps, second row contains bad maps.
        /// </summary>
        public static string[][] DownloadAll()
        {
            using (WebClient client = new WebClient())
            {
                return DecodeJson(client.DownloadString(JSON_URL));
            }
        }

        public static string[] DownloadInGameMaps()
        {
            return DownloadAll()[0];
        }

        public static void UploadAll(List<string> badList, List<string> ingameList)
        {
            using (WebClient client = new WebClient())
            {
                client.UploadString(JSON_URL, "PUT", EncodeJson(badList, ingameList));
            }
        }

        private static string[][] DecodeJson(string json)
        {
            return new JavaScriptSerializer().Deserialize<string[][]>(json);
        }

        private static string EncodeJson(List<string> badList, List<string> ingameList)
        {
            return new JavaScriptSerializer().Serialize(new string[][]
                {
                    ingameList.ToArray(),
                    badList.ToArray()
                });
        }
    }
}