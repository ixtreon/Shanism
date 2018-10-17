using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IconPacker
{
    class GameIconsClient
    {
        readonly HttpClient client;

        public string DownloadUrl { get; }

        public GameIconsClient(string background = "transparent", string foreground = "ffffff")
        {
            client = new HttpClient();
            DownloadUrl = $@"https://game-icons.net/archives/png/zip/{foreground}/{background}/game-icons.net.png.zip";
        }

        public async Task<GameIconsInfo> GetRemoteFileInfo()
        {
            var resp = await client.GetAsync(DownloadUrl, HttpCompletionOption.ResponseHeadersRead);
            if (!resp.IsSuccessStatusCode)
                throw new HttpRequestException($"Status Code: {resp.StatusCode}, Reason: {resp.ReasonPhrase}");

            var sContentLength = GetHeader(resp, "Content-Length");
            if(!long.TryParse(sContentLength, out var contentLength))
                throw new HttpRequestException($"The content-length header - {sContentLength} - was unparseable.");

            var sLastModified = GetHeader(resp, "Last-Modified");
            if(!DateTime.TryParse(sLastModified, out var lastModified))
                throw new HttpRequestException($"The last-modified header - {sLastModified} - was unparseable.");

            return new GameIconsInfo(contentLength, lastModified);
        }

        public async Task<Stream> GetIconsAsync() 
            => await client.GetStreamAsync(DownloadUrl);

        static string GetHeader(HttpResponseMessage msg, string key)
        {
            if (!msg.Content.Headers.TryGetValues(key, out var values))
                throw new HttpRequestException($"No '{key}' header was present in the response.");

            return values.First();
        }

    }

    readonly struct GameIconsInfo
    {
        public readonly long ArchiveSize;

        public readonly DateTime LastModified;

        public GameIconsInfo(long archiveSize, DateTime lastModified)
        {
            ArchiveSize = archiveSize;
            LastModified = lastModified;
        }
    }
}
