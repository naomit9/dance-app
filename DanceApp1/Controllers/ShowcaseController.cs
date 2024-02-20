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
using DanceApp1.Models.ViewModels;

namespace DanceApp1.Controllers
{
    public class ShowcaseController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static ShowcaseController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44306/api/");
        }

        /// <summary>
        /// To communicate with our group data api to retrieve a list of showcases
        /// </summary>
        /// <returns>A list of showcase's names</returns>
        /// <example>GET: Showcase/List</example>
        public ActionResult List()
        {
            // curl https://localhost:44306/api/ShowcaseData/ListShowcases

            string url = "ShowcaseData/ListShowcases";
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
            DetailsShowcase ViewModel = new DetailsShowcase();

            // curl https://localhost:44306/api/ShowcaseData/FindShowcase/{id}

            string url = "ShowcaseData/FindShowcase/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            ShowcaseDto SelectedShowcase = response.Content.ReadAsAsync<ShowcaseDto>().Result;

            Debug.WriteLine("Showcase received:");
            Debug.WriteLine(SelectedShowcase.showcaseName);

            ViewModel.SelectedShowcase = SelectedShowcase;

            // Show all dance groups that participated in a showcase
            url = "GroupData/ListGroupsForShowcase/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<GroupDto> ShowcaseGroups = response.Content.ReadAsAsync<IEnumerable<GroupDto>>().Result;

            ViewModel.ShowcaseGroups = ShowcaseGroups;

            return View(ViewModel);
        }
        public ActionResult Error()
        {
            return View();
        }

        /// <summary>
        /// To show a view that has the required information to create a new showcase
        /// </summary>
        /// <returns>Returns a view that allows me to create a new showcase</returns>
        /// <example>GET: Showcase/New</example>
        public ActionResult New()
        {
            return View();
        }


        /// <summary>
        /// Add a new showcase event into our system using the API
        /// </summary>
        /// <param name="showcase">A new showcase object</param>
        /// <returns>Info about the new showcase event</returns>
        /// <example>POST: Showcase/Create</example>
        [HttpPost]
        public ActionResult Create(Showcase showcase)
        {
            string url = "ShowcaseData/AddShowcase";

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


        /// <summary>
        /// Routes to a dynamically generated 'Edit Showcase' page. Gathers information from the database
        /// </summary>
        /// <param name="id">ID of showcase</param>
        /// <returns>A dynamic 'Edit Showcase' webpage which provides the current information of the showcase and asks the user for new info as a form</returns>
        /// <example>GET: Showcase/Edit/5</example>
        public ActionResult Edit(int id)
        {
            string url = "ShowcaseData/FindShowcase/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;
            ShowcaseDto selectedshowcase = response.Content.ReadAsAsync<ShowcaseDto>().Result;

            return View(selectedshowcase);
        }


        /// <summary>
        /// Receives a POST request containing info about existing showcase in the system with new values
        /// Conveys this info to the API. Redirects to the 'Showcase Details' page of our updated showcase.
        /// </summary>
        /// <param name="id">ID of showcase</param>
        /// <param name="showcase">Updated showcase info</param>
        /// <returns>If the update is successful, you will be directed back to the List page, otherwise, the Error page</returns>
        /// <example>POST: Showcase/Update/5</example>
        [HttpPost]
        public ActionResult Update(int id, Showcase showcase)
        {
            string url = "ShowcaseData/UpdateShowcase/" + id;
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


        /// <summary>
        /// To confirm if the user wants to delete this showcase
        /// </summary>
        /// <param name="id">ID of showcase</param>
        /// <returns>The detail page of the selected  showcase</returns>
        /// <example>GET: Showcase/DeleteConfirm/5</example>
        public ActionResult DeleteConfirm(int id)
        {
            string url = "ShowcaseData/FindShowcase/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ShowcaseDto selectedshowcase = response.Content.ReadAsAsync<ShowcaseDto>().Result;

            return View(selectedshowcase);
        }


        /// <summary>
        /// To show a POST request to our database to remove a selected showcase
        /// </summary>
        /// <param name="id">ID of showcase</param>
        /// <returns>If the deletion is successful, you will be re-directed to List page, otherwise, Error page</returns>
        /// <example>POST: Showcase/Delete/5</example>
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "ShowcaseData/DeleteShowcase/" + id;
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
