using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using DanceApp1.Models;
using DanceApp1.Models.ViewModels;
using System.Web.Script.Serialization;
using DanceApp1.Migrations;

namespace DanceApp1.Controllers
{
    public class GroupController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static GroupController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44306/api/");
        }

        /// <summary>
        /// To communicate with our group data api to retrieve a list of groups
        /// </summary>
        /// <returns>A list of group's names</returns>
        /// <example>GET: Group/List</example>
        public ActionResult List()
        {
            // curl https://localhost:44306/api/GroupData/ListGroups

            string url = "GroupData/ListGroups";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<GroupDto> groups = response.Content.ReadAsAsync<IEnumerable<GroupDto>>().Result;

            //Debug.WriteLine("Number of groups received: ");
            //Debug.WriteLine(groups.Count());

            return View(groups);
        }

        /// <summary>
        /// To communicate with our group data api to retrieve details about one group
        /// </summary>
        /// <param name="id">ID of the groupd</param>
        /// <returns>Group's name, information about group dance style and a group bio</returns>
        /// <example>GET: Group/Details/5</example>
        public ActionResult Details(int id)
        {
            // curl https://localhost:44306/api/GroupData/FindGroup/{id}

            DetailsGroup ViewModel = new DetailsGroup();

            string url = "GroupData/FindGroup/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            GroupDto SelectedGroup = response.Content.ReadAsAsync<GroupDto>().Result;

            //Debug.WriteLine("Group received: ");
            //Debug.WriteLine(selectedgroup.groupName);

            // Showcase info about dancers related to this group
            ViewModel.SelectedGroup = SelectedGroup;


            // Send a request to gather info about dancers related to a particular group ID
            url = "DancerData/ListDancersForGroup/" + id; 
            response = client. GetAsync(url).Result;
            IEnumerable<DancerDto> RelatedDancers = response.Content.ReadAsAsync<IEnumerable<DancerDto>>().Result;

            url = "ShowcaseData/ListShowcasesForGroup/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<ShowcaseDto> ShowcaseAttended = response.Content.ReadAsAsync<IEnumerable<ShowcaseDto>>().Result;
            ViewModel.ShowcaseAttended = ShowcaseAttended;

            ViewModel.RelatedDancers = RelatedDancers;


            return View(ViewModel);
        }
        public ActionResult Error()
        {
            return View();
        }

        /// <summary>
        /// To show a view that has the required information to create a new group
        /// </summary>
        /// <returns>Returns a view that allows me to create a new group</returns>
        /// <example>GET: Group/New</example> 
        public ActionResult New()
        {
            return View();
        }

        /// <summary>
        /// Add a new dance group into our system using the API
        /// </summary>
        /// <param name="group">A new group object created</param>
        /// <returns>Information about the new group</returns>
        /// <example>POST: Group/Create </example>
        [HttpPost]
        public ActionResult Create(Group group)
        {
            Debug.WriteLine("the jsonpayload is: ");

            // curl -d @group.json -H "Content-type:application/json"  "https://localhost:44306/api/GroupData/AddGroup"
            string url = "GroupData/AddGroup";

            string jsonpayload = jss.Serialize(group);

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
        /// Routes to a dynamically generated 'Edit Group' page. Gathers information from the database
        /// </summary>
        /// <param name="id">ID of the group</param>
        /// <returns>A dynamic 'Edit Group' webpage which provides the current information of the group and asks the user for new info as a form</returns>
        /// <example>GET: Group/Edit/5</example>
        public ActionResult Edit(int id)
        {
            string url = "GroupData/FindGroup/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;
            GroupDto selectegroup = response.Content.ReadAsAsync<GroupDto>().Result;

            return View(selectegroup);
        }


        /// <summary>
        /// Receives a POST request containing info about existing group in the system with new values
        /// Conveys this info to the API. Redirects to the 'Group Details page of our updated group.
        /// </summary>
        /// <param name="id">ID of the group</param>
        /// <param name="group">Information of the group</param>
        /// <returns>If the update is successful, you will be directed back to the List page, otherwise, the Error page</returns>
        /// <example>POST: Group/Update/5</example>
        [HttpPost]
        public ActionResult Update(int id, Group group)
        {
            string url = "GroupData/UpdateGroup/" + id;
            string jsonpayload = jss.Serialize(group);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            Debug.WriteLine(content);

            Debug.WriteLine(response.IsSuccessStatusCode);

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
        /// To confirm if the user wants to delete this dancer
        /// </summary>
        /// <param name="id">ID of group</param>
        /// <returns>The detail page of the selected dancer</returns>
        /// <example>GET: Group/DeleteConfirm/5</example>
        public ActionResult DeleteConfirm(int id)
        {
            string url = "GroupData/FindGroup/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            GroupDto selectegroup = response.Content.ReadAsAsync<GroupDto>().Result;

            return View(selectegroup);
        }


        /// <summary>
        /// To show a POST request to our database to remove a selected group
        /// </summary>
        /// <param name="id">ID of group</param>
        /// <returns>If the deletion is successful, you will be re-directed to List page, otherwise, Error page</returns>
        /// <example>POST: Group/Delete/5</example>
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "GroupData/DeleteGroup/" + id;
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
