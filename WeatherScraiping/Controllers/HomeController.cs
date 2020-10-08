using System;
using System.Diagnostics;
using System.Net;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeatherScraiping.Models;

namespace WeatherScraiping.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                //各サイトのURL
                var tenkijp     = @"https://tenki.jp/";
                var yahoo       = @"https://weather.yahoo.co.jp/weather/";
                var weathernews = @"https://weathernews.jp/s/forecast/";
                var kisyotyo    = @"https://www.jma.go.jp/jp/yoho/";

                //htmlソースの取得
                WebClient wc = new WebClient();

                var tenkijpHtml     = wc.DownloadString(tenkijp);
                var yahooHtml       = wc.DownloadString(yahoo);
                var weathernewsHtml = wc.DownloadString(weathernews);

                var parser = new HtmlParser();
                var tenkijpParsed     = parser.ParseDocument(tenkijpHtml);
                var yahooParsed       = parser.ParseDocument(yahooHtml);
                var weathernewsParsed = parser.ParseDocument(weathernewsHtml);

                var tenkijpElement     = tenkijpParsed.QuerySelector("#forecast-map-wrap");
                var yahooElement       = yahooParsed.QuerySelector("#forecastMap");
                var weathernewsElement = weathernewsParsed.QuerySelector(".map");

                ViewData["tenkiJp"]       = tenkijpElement.ToHtml();
                ViewData["yahooJp"]       = yahooElement.ToHtml();
                ViewData["weathernewsJp"] = weathernewsElement.ToHtml();

            }
            catch (Exception)
            {

                throw;
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

        }
    }        
}
