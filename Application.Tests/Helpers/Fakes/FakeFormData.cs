using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace CodeKinden.OrangeCMS.Application.Tests.Helpers.Fakes
{
    class FakeFormData
    {
        public static MultipartFormDataContent CreateMultipartFormDataContent(string filename)
        {
            var content = new MultipartFormDataContent();
            var fileContent = new ByteArrayContent(File.ReadAllBytes(filename));
            var contentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = Guid.NewGuid().ToString()
            };
            fileContent.Headers.ContentDisposition = contentDisposition;
            content.Add(fileContent);
            return content;
        }
    }
}
