﻿using MasterServerToolkit.MasterServer.Web;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using UnityEngine;
using WebSocketSharp.Net;
using WebSocketSharp.Server;

namespace MasterServerToolkit.MasterServer
{
    [Serializable]
    public class MasterServerInfo
    {
        public string Name;
        public int ModulesQty;
    }

    public class MstInfoHttpController : HttpController
    {
        Dictionary<string, string> info;

        public override void Initialize(HttpServerModule server)
        {
            base.Initialize(server);
            server.RegisterHttpRequestHandler("info", OnGetMstInfoJsonHttpRequestHandler);

            info = new Dictionary<string, string>
            {
                { "Device Id", SystemInfo.deviceUniqueIdentifier },
                { "Device Model", SystemInfo.deviceModel },
                { "Device Name", SystemInfo.deviceName },
                { "Graphics Device Name", SystemInfo.graphicsDeviceName },
                { "Graphics Device Version", SystemInfo.graphicsDeviceVersion },
                { "...", "etc." },
            };
        }

        private void OnGetMstInfoJsonHttpRequestHandler(HttpRequestEventArgs eventArgs)
        {
            var response = eventArgs.Response;

            HtmlDocument html = new HtmlDocument();
            html.Title = "MST Info";
            html.AddMeta(new KeyValuePair<string, string>("charset", "utf-8"));
            html.AddMeta(new KeyValuePair<string, string>("name", "viewport"), new KeyValuePair<string, string>("content", "width=device-width, initial-scale=1"));

            html.Links.Add(new HtmlLinkElement()
            {
                Href = "https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta2/dist/css/bootstrap.min.css",
                Rel = "stylesheet",
                Integrity = "sha384-BmbxuPwQa2lc/FVzBcNJ7UAyJxM6wuqIj61tLrc4wSX0szH/Ev+nYRRuWlolflfl",
                Crossorigin = "anonymous"
            });

            html.Scripts.Add(new HtmlScriptElement()
            {
                Src = "https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta2/dist/js/bootstrap.bundle.min.js",
                Integrity = "sha384-b5kHyXgcpbZJO/tY9Ul7kGkf1S0CWuKcCD38l8YkeH8z8QjE0GmW1gYU5S9FOnJ0",
                Crossorigin = "anonymous"
            });

            html.Body.AddClass("body");

            var container = html.CreateElement("div");
            container.AddClass("container-fluid pt-5");
            html.Body.AppendChild(container);

            var h1 = html.CreateElement("h1");
            h1.InnerText = $"{Mst.Name} {Mst.Version}";
            container.AppendChild(h1);

            var row = html.CreateElement("div");
            row.AddClass("row");
            container.AppendChild(row);

            var col1 = html.CreateElement("div");
            col1.AddClass("col-md-4");
            row.AppendChild(col1);

            var col2 = html.CreateElement("div");
            col2.AddClass("col-md-8");
            row.AppendChild(col2);

            GenerateMachineInfo(html, col1);

            GenerateListOfModules(html, col2);

            byte[] contents = Encoding.UTF8.GetBytes(html.ToString());

            response.ContentType = "text/html";
            response.ContentEncoding = Encoding.UTF8;
            response.ContentLength64 = contents.LongLength;
            response.Close(contents, true);
        }

        private void GenerateMachineInfo(HtmlDocument html, XmlElement parent)
        {
            var h2 = html.CreateElement("h2");
            h2.InnerText = "System Info";
            parent.AppendChild(h2);

            var table = html.CreateElement("table");
            table.AddClass("table table-bordered table-striped table-hover table-sm mb-5");
            parent.AppendChild(table);

            var thead = html.CreateElement("thead");
            table.AppendChild(thead);

            var tbody = html.CreateElement("tbody");
            table.AppendChild(tbody);

            var thr = html.CreateElement("tr");
            thead.AppendChild(thr);

            var th1 = html.CreateElement("th");
            th1.InnerText = "#";
            th1.SetAttribute("scope", "col");
            th1.SetAttribute("width", "200");
            thr.AppendChild(th1);

            var thr2 = html.CreateElement("th");
            thr2.InnerText = "Name";
            thr.AppendChild(thr2);

            foreach (var property in info)
            {
                var tr = html.CreateElement("tr");
                tbody.AppendChild(tr);

                var th = html.CreateElement("th");
                th.InnerText = property.Key;
                th.SetAttribute("scope", "row");
                tr.AppendChild(th);

                var th2 = html.CreateElement("td");
                th2.InnerText = property.Value;
                tr.AppendChild(th2);
            }
        }

        private void GenerateListOfModules(HtmlDocument html, XmlElement parent)
        {
            var h2 = html.CreateElement("h2");
            h2.InnerText = "Modules";
            parent.AppendChild(h2);

            var table = html.CreateElement("table");
            table.AddClass("table table-bordered table-striped table-hover table-sm");
            parent.AppendChild(table);

            var thead = html.CreateElement("thead");
            table.AppendChild(thead);

            var tbody = html.CreateElement("tbody");
            table.AppendChild(tbody);

            var thr = html.CreateElement("tr");
            thead.AppendChild(thr);

            var th1 = html.CreateElement("th");
            th1.InnerText = "#";
            th1.SetAttribute("scope", "col");
            th1.SetAttribute("width", "50");
            thr.AppendChild(th1);

            var th2 = html.CreateElement("th");
            th2.InnerText = "Name";
            thr.AppendChild(th2);

            var th3 = html.CreateElement("th");
            th3.InnerText = "Info";
            thr.AppendChild(th3);

            int index = 1;

            foreach (var module in MasterServer.GetInitializedModules())
            {
                var tr = html.CreateElement("tr");
                tbody.AppendChild(tr);

                var th = html.CreateElement("th");
                th.InnerText = index.ToString();
                th.SetAttribute("scope", "row");
                tr.AppendChild(th);

                var td = html.CreateElement("td");
                td.InnerText = module.GetType().Name;
                tr.AppendChild(td);

                var td1 = html.CreateElement("td");
                tr.AppendChild(td1);

                var ul = html.CreateElement("ul");
                td1.AppendChild(ul);

                string[] infos = module.Info().ToReadableString("\n", ": ").Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                foreach(string info in infos)
                {
                    var li = html.CreateElement("li");
                    li.InnerText = info;
                    ul.AppendChild(li);
                }

                index++;
            }
        }
    }
}