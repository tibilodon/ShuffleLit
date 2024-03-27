using ShuffleLit.Interfaces;
using System.Text.RegularExpressions;
using System.Web;

namespace ShuffleLit.Services
{
    public class LinkUrlFormatService : ILinkUrlFormatService
    {
        private readonly string template = "https://img.youtube.com/vi/";
        public string FormatYoutubeUrl(string url)
        {
            if (url.StartsWith(template))
            {
                return url;
            }
            if (string.IsNullOrEmpty(url))
            {
                throw new Exception("no link provided");
            }
            return Execute(url);
        }

        //  template
        private string UrlTemplate(string id)
        {
            //  for more options see: https://developers.google.com/youtube/v3/getting-started#Sample_Partial_Requests
            string quality = "/hqdefault.jpg";
            return template + id + quality;
        }

        private string Execute(string url)

        {
            //    regex pattern
            var videoIdRegex = new Regex(@"(?:https?:\/\/)?(?:www\.)?(?:youtube\.com\/(?:[^\/\n\s]+\/\S+\/|(?:v|e(?:mbed)?)\/|\S*?[?&]v=)|youtu\.be\/)([a-zA-Z0-9_-]{11})", RegexOptions.Compiled);
            var match = videoIdRegex.Match(url);
            //  on success
            if (match.Success)
            {
                //  return templated value
                return UrlTemplate(match.Groups[1].Value);
            }
            //  attempt to extract the video ID from the query string if the URL is a long format YouTube URL with additional parameters
            //  validate url
            Uri uriResult;
            bool isValidUri = Uri.TryCreate(url, UriKind.Absolute, out uriResult);

            if (isValidUri)
            {
                var uri = new Uri(url);
                var queryString = HttpUtility.ParseQueryString(uri.Query);
                var videoId = queryString.Get("v");
                if (!string.IsNullOrEmpty(videoId))
                {
                    return UrlTemplate(videoId);
                }
            }
            //  otherwise return placeholder image
            return url;
        }
    }
}
