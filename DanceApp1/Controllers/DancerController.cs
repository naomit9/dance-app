using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using DanceApp1.Models;
using DanceApp1.Models.ViewModels;
using System.Web.Script.Serialization;
using DanceApp1.Migrations;

namespace DanceApp1.Controllers
{
    public class DancerController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static DancerController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44306/api/");
        }

        /// <summary>
        /// To communicate with our group data api to retrieve a list of dancers
        /// </summary>
        /// <returns>A list of dancers's names</returns>
        /// <example>GET: Dancer/List</example>
        public ActionResult List()
        {
            // curl https://localhost:44306/api/DancerData/ListDancers
           
            // Establish URL Endpoint
            string url = "DancerData/ListDancers";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<DancerDto> dancers = response.Content.ReadAsAsync<IEnumerable<DancerDto>>().Result;

            //Debug.WriteLine("Number of dancers received: ");
            //Debug.WriteLine(dancers.Count());

            return View(dancers);
        }

        /// <summary>
        /// To communicate with our group data api to retrieve details about one dancer
        /// </summary>
        /// <param name="id">ID of the dancerId</param>
        /// <returns>Dancer's first name, last name, their dance style and a dancer bio</returns>
        /// <example>GET: Dancer/Details/5</example>
        public ActionResult Details(int id)
        {
            // curl https://localhost:44306/api/DancerData/FindDancer/{id}

            // Establish URL Endpoint
            string url = "DancerData/FindDancer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            DancerDto selecteddancer = response.Content.ReadAsAsync<DancerDto>().Result;

            //Debug.WriteLine("Dancer received: ");
            //Debug.WriteLine(selecteddancer.firstName);

            return View(selecteddancer);
        }
        public ActionResult Error()
        {
            return View();
        }

        /// <summary>
        /// To show a view that has the required information to create a new dancer
        /// </summary>
        /// <returns>Returns a view that allows me to create a new dancer</returns>
        /// <example>GET: Dancer/New </example> 
        public ActionResult New()
        {
            // Get informationa about all dancers in the system
            // GET api/dancerdata/listdancers

            string url = "GroupData/ListGroups";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<GroupDto> GroupOptions = response.Content.ReadAsAsync<IEnumerable<GroupDto>>().Result;

            return View(GroupOptions);
        }

        /// <summary>
        ///  Add a new dancer into our system using the API
        /// </summary>
        /// <param name="dancer">A new dancer object created</param>
        /// <returns>Information about the new dancer</returns>
        /// <example>POST: Dancer/Create </example>
        [HttpPost]
        public ActionResult Create(Dancer dancer)
        {
            Debug.WriteLine("the jsonpayload is: ");
            //Debug.WriteLine(dancer.firstName);

            // curl -d @dancer.json -H "Content-type:application/json"  "https://localhost:44306/api/DancerData/AddDancer"
            string url = "DancerData/AddDancer";           
            
            string jsonpayload = jss.Serialize(dancer);

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
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Dancer/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateDancer ViewModel = new UpdateDancer();

            // The existing animal information
            string url = "DancerData/FindDancer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DancerDto selecteddancer = response.Content.ReadAsAsync<DancerDto>().Result;

            ViewModel.selecteddancer = selecteddancer;

            // To include all groups to choose from when updating this dancer profile

            // The existing group information
            url = "GroupData/ListGroups/";
            response = client.GetAsync(url).Result;
            IEnumerable<GroupDto> GroupOptions = response.Content.ReadAsAsync<IEnumerable<GroupDto>>
                ().Result;

            ViewModel.GroupOptions = GroupOptions;

            return View(ViewModel);
        }


        /// <summary>
        /// Update an existing dancer in our system using the API
        /// </summary>
        /// <param name="id">ID of the dancer</param>
        /// <param name="dancer">Information of the dancer</param>
        /// <returns>If the update is successful, you will be directed back to the List page, otherwise, the Error page</returns>
        /// <example>POST: Dancer/Update/5</example>
        [HttpPost]
        public ActionResult Update(int id, Dancer dancer)
        {
            try
            {
                Debug.WriteLine("new dancer info is: ");
                Debug.WriteLine(dancer.firstName);
                Debug.WriteLine(dancer.lastName);

                string url = "DancerData/UpdateDancer/" + id;
                string jsonpayload = jss.Serialize(dancer);

                Debug.WriteLine(jsonpayload);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                HttpResponseMessage response = client.PostAsync(url, content).Result;

                Debug.WriteLine(content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Details/" + id);
                }
                else
                {
                    return RedirectToAction("Error");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception occurred: " + ex.Message);
                return RedirectToAction("Error");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Dancer/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "DancerData/FindDancer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DancerDto selecteddancer = response.Content.ReadAsAsync<DancerDto>().Result;

            return View(selecteddancer);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // POST: Dancer/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "DancerData/DeleteDancer/" + id;
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
