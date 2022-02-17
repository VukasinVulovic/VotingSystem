using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.IO;
using System.Security;
using Newtonsoft.Json;

namespace App
{
    class HTTPServer
    {
        private int port;
        private int bufferSize; //kb
        private HttpListener listener;
        private Thread listeningThread;
        private bool isListening;
        private string rootDirectory;
        private Dictionary<string, Dictionary<string, Action<HttpListenerRequest, HttpListenerResponse>>> pathCbs;

        public int Port { get => port; set => port = value; }
        public bool IsListening { get => isListening; }
        public string RootDirectory { get => rootDirectory; set => rootDirectory = value; }
        public int BufferSize { get => bufferSize; set => bufferSize = value; }

        public HTTPServer()
        {
            port = 80;
            bufferSize = 4;
            rootDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
            listener = new HttpListener();
        }

        public void On(string path, string method, Action<HttpListenerRequest, HttpListenerResponse> cb)
        {
            if (pathCbs == null)
                pathCbs = new Dictionary<string, Dictionary<string, Action<HttpListenerRequest, HttpListenerResponse>>>();

            if (!pathCbs.ContainsKey(method))
                pathCbs[method] = new Dictionary<string, Action<HttpListenerRequest, HttpListenerResponse>>();

            pathCbs[method][path] = cb;
        }

        public static Dictionary<string, string> ParseJSON(string body) 
        {
            try
            {
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(body);
            } catch
            {
                return null;
            }

        }

        public static string StringifyJSON(object json)
        {
            try
            {
                return JsonConvert.SerializeObject(json);
            }
            catch
            {
                return "";
            }
        }

        public void Start()
        {
            listener.Prefixes.Clear();
            listener.Prefixes.Add($"http://127.0.0.1:{port}/"); //set protocol and port

            listener.Start();
            isListening = true;

            listeningThread = new Thread(() =>
            {
                while (isListening) //forever loop
                {
                    HttpListenerContext ctx = listener.GetContext();
                    handleRequest(ctx); //will execute after a client connects
                }
            });

            listeningThread.IsBackground = true;
            listeningThread.Start();
        }

        public void Stop()
        {
            isListening = false;

            if (listeningThread != null)
                listeningThread.Abort();

            GC.SuppressFinalize(this);
        }

        private void handleRequest(HttpListenerContext ctx)
        {
            Uri url = ctx.Request.Url;
            string method = ctx.Request.HttpMethod.ToUpper();
            string path = url.LocalPath;

            if (method == "GET")
            {
                string itemPath = rootDirectory + "\\" + path.Replace("..", "");

                if ((File.Exists(itemPath) || Directory.Exists(itemPath)) && File.GetAttributes(itemPath) == FileAttributes.Directory) //if it's a directory, try index.html
                    itemPath += "\\index.html";


                if (File.Exists(itemPath))
                    sendFile(itemPath, ctx);
                else if (pathCbs != null && pathCbs.ContainsKey(method) && pathCbs[method].ContainsKey(path))
                    pathCbs[method][path](ctx.Request, ctx.Response);
                else
                {
                    ctx.Response.StatusCode = 404;
                    ctx.Response.Abort();
                }
            }
            else if (method == "POST")
            {
                if (pathCbs != null && pathCbs.ContainsKey(method) && pathCbs[method].ContainsKey(path))
                    pathCbs[method][path](ctx.Request, ctx.Response);
                else
                {
                    ctx.Response.StatusCode = 404;
                    ctx.Response.Abort();
                }
            }
            else
            {
                ctx.Response.StatusCode = 405;
                ctx.Response.Abort();
                return;
            }
        }

        private void sendFile(string filePath, HttpListenerContext ctx)
        {
            FileStream file = File.OpenRead(filePath);
            byte[] buff = new byte[1024 * bufferSize]; //4kB

            ctx.Response.AddHeader("Content-Type", MimeType.FromPath(filePath));

            int read = 0;
            while ((read = file.Read(buff, 0, buff.Length)) != 0)
                ctx.Response.OutputStream.Write(buff, 0, read);

            file.Close();
            ctx.Response.OutputStream.Close();
            file.Dispose();
        }

        public void Dispose()
        {
            Stop();
            listener.Close();
            isListening = false;
        }

        ~HTTPServer()
        {
            this.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
