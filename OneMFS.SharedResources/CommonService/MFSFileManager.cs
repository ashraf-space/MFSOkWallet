using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace OneMFS.SharedResources.CommonService
{
    public class MFSFileManager
    {
        public HttpResponseMessage ConvertByteToFileStream(byte[] bytes, HttpResponseMessage httpResponseMessage,string fileNameAndExt)
        {
            var dataStream = new MemoryStream(bytes);            
            httpResponseMessage.Content = new StreamContent(dataStream);
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = fileNameAndExt;
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            return httpResponseMessage;
        }
    }
}
