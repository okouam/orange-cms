using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace OrangeCMS.Application.Tests
{
    class Helpers
    {
        public static MultipartFormDataContent CreateMultipartFormDataContent(string filename)
        {
            var content = new MultipartFormDataContent();
            var fileContent = new ByteArrayContent(GetFileBytes(filename));
            var contentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = Guid.NewGuid().ToString()
            };
            fileContent.Headers.ContentDisposition = contentDisposition;
            content.Add(fileContent);
            return content;
        }

        private static byte[] GetFileBytes(string filename)
        {
            var path = Path.Combine(Environment.CurrentDirectory, filename);
            return File.ReadAllBytes(path);
        }
    }
}
