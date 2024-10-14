using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IO;

namespace Manager.Common.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
      
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
        private Stopwatch stopwatch;

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }

        public async Task Invoke(HttpContext context)
        {
            stopwatch = Stopwatch.StartNew();
            string reqTime = DateTime.Now.ToString("HH:mm:ss.ttt");
            var originalBodyStream = context.Response.Body;
            string reqBody = await LogRequest(context);
            string resBody = await LogResponse(context);
            stopwatch.Stop();
            string resTime = DateTime.Now.ToString("HH:mm:ss.ttt");
            LogModel.LogModel log = new LogModel.LogModel(reqTime, reqBody, resBody, resTime, stopwatch.ElapsedMilliseconds.ToString("#,##0 Ms"),context);
            LogModel.LogModel.LogUser(log);
            


        }

        private async Task<String> LogRequest(HttpContext context)
        {
            context.Request.EnableBuffering();

            var requestStream = _recyclableMemoryStreamManager.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);
      
            context.Request.Body.Position = 0;
            return $"{context.Request.Scheme} {context.Request.Host}{context.Request.Path} {context.Request.QueryString} {ReadStreamInChunks(requestStream)}";
           
        }

        private async Task<String> LogResponse(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;

           var responseBody = _recyclableMemoryStreamManager.GetStream();
            context.Response.Body = responseBody;

            await _next(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

          
            await responseBody.CopyToAsync(originalBodyStream);
            return $"{context.Request.Scheme} {context.Request.Host}{context.Request.Path} {context.Request.QueryString} {text}";

        }

        private static string ReadStreamInChunks(Stream stream)
        {
            const int readChunkBufferLength = 4096;

            stream.Seek(0, SeekOrigin.Begin);

           var textWriter = new StringWriter();
            var reader = new StreamReader(stream);

            var readChunk = new char[readChunkBufferLength];
            int readChunkLength;

            do
            {
                readChunkLength = reader.ReadBlock(readChunk, 0, readChunkBufferLength);
                textWriter.Write(readChunk, 0, readChunkLength);
            } while (readChunkLength > 0);

            return textWriter.ToString();
        }
    }
}