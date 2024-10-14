using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Common.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private Stopwatch stopwatch;
        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                stopwatch = Stopwatch.StartNew();
                string reqTime = DateTime.Now.ToString("HH:mm:ss.ttt");
                string reqBody = await GetRequest(context.Request);
                var originalBodyStream = context.Response.Body;
                using (var responseBody = new MemoryStream())
                {
                    //...and use that for the temporary response body
                    context.Response.Body = responseBody;

                    //Continue down the Middleware pipeline, eventually returning to this class
                    await _next(context);

                    //Format the response from the server
                    string resBody = await GetResponse(context.Response);

                    //TODO: Save log to chosen datastore

                    //Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
                    await responseBody.CopyToAsync(originalBodyStream);
                    stopwatch.Stop();
                    string resTime = DateTime.Now.ToString("HH:mm:ss.ttt");
                    LogModel.LogModel log = new LogModel.LogModel(reqTime, reqBody, resBody, resTime, stopwatch.ElapsedMilliseconds.ToString("#,##0 Ms"), context);
                    LogModel.LogModel.LogUser(log);
                }
            }
            catch (Exception ex)
            {

                await _next(context);
            }
        }

        private async Task<string> GetRequest(HttpRequest request)
        {
            var body = request.Body;


            //We now need to read the request stream.  First, we create a new byte[] with the same length as the request stream...
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            //...Then we copy the entire request stream into the new buffer.
            await request.Body.ReadAsync(buffer, 0, buffer.Length);

            //We convert the byte[] into a string using UTF8 encoding...
            var bodyAsText = Encoding.UTF8.GetString(buffer);

            //..and finally, assign the read body back to the request body, which is allowed because of EnableRewind()
            request.Body = body;

            return $"{request.Scheme} {request.Host}{request.Path} {request.QueryString} {bodyAsText}";
        }

        private async Task<string> GetResponse(HttpResponse response)
        {
            //We need to read the response stream from the beginning...
            response.Body.Seek(0, SeekOrigin.Begin);

            //...and copy it into a string
            string text = await new StreamReader(response.Body).ReadToEndAsync();

            //We need to reset the reader for the response so that the client can read it.
            response.Body.Seek(0, SeekOrigin.Begin);

            //Return the string for the response, including the status code (e.g. 200, 404, 401, etc.)
            return $"{response.StatusCode}: {text}";
        }
    }
}
