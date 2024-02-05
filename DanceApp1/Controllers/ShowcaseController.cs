using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using DanceApp1.Models;
using System.Web.Script.Serialization;
using System.Runtime.CompilerServices;

namespace DanceApp1.Controllers
{
    public class ShowcaseController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static ShowcaseController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44306/api/ShowcaseData/");
        }

        /// <summary>
        /// To communicate with our group data api to retrieve a list of showcases
        /// </summary>
        /// <returns>A list of showcase's names</returns>
        /// <example>GET: Showcase/List</example>
        public ActionResult List()
        {
            // curl https://localhost:44306/api/ShowcaseData/ListShowcases

            string url = "ListShowcases";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<ShowcaseDto> showcases = response.Content.ReadAsAsync<IEnumerable<ShowcaseDto>>().Result;

            //Debug.WriteLine("Number of showcases received:");
            //Debug.WriteLine(showcases.Count());

            return View(showcases);
        }

        /// <summary>
        /// To communicate with our group data api to retrieve details about one showcase
        /// </summary>
        /// <param name="id">ID of the showcaseId</param>
        /// <returns>Name of the showcase, information about date and location of the showcase</returns>
        /// <example>GET: Showcase/Details/5</example>
        public ActionResult Details(int id)
        {
            // curl https://localhost:44306/api/ShowcaseData/FindShowcase/{id}

            string url = "FindShowcase/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            ShowcaseDto selectedshowcase = response.Content.ReadAsAsync<ShowcaseDto>().Result;

            //Debug.WriteLine("Showcase received:");
            //Debug.WriteLine(selectedshowcase.showcaseName);

            return View(selectedshowcase);
        }
        public ActionResult Error()
        {
            return View();
        }

        // GET: Showcase/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Showcase/Create
        [HttpPost]
        public ActionResult Create(Showcase showcase)
        {
            string url = "AddShowcase";

            string jsonpayload = jss.Serialize(showcase);
            
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Showcase/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "FindShowcase/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;
            ShowcaseDto selectedshowcase = response.Content.ReadAsAsync<ShowcaseDto>().Result;

            return View(selectedshowcase);
        }

        // POST: Showcase/Update/5
        [HttpPost]
        public ActionResult Update(int id, Showcase showcase)
        {
            string url = "UpdateShowcase/" + id;
            string jsonpayload = jss.Serialize(showcase);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            Debug.WriteLine(content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Showcase/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "FindShowcase/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ShowcaseDto selectedshowcase = response.Content.ReadAsAsync<ShowcaseDto>().Result;

            return View(selectedshowcase);
        }

        // POST: Showcase/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "DeleteShowcase/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            Debug.WriteLine(content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
