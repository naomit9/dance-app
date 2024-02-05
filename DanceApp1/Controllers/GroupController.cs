using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using DanceApp1.Models;
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
            client.BaseAddress = new Uri("https://localhost:44306/api/GroupData/");
        }

        /// <summary>
        /// To communicate with our group data api to retrieve a list of groups
        /// </summary>
        /// <returns>A list of group's names</returns>
        /// <example>GET: Group/List</example>
        public ActionResult List()
        {
            // curl https://localhost:44306/api/GroupData/ListGroups

            string url = "ListGroups";
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

            string url = "FindGroup/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            GroupDto selectedgroup = response.Content.ReadAsAsync<GroupDto>().Result;
            
            //Debug.WriteLine("Group received: ");
            //Debug.WriteLine(selectedgroup.groupName);

            return View(selectedgroup);
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
            string url = "AddGroup";

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
        // GET: Group/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "FindGroup/" + id;

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
            string url = "UpdateGroup/" + id;
            string jsonpayload = jss.Serialize(group);

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
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Group/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "FindGroup/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            GroupDto selectegroup = response.Content.ReadAsAsync<GroupDto>().Result;

            return View(selectegroup);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // POST: Group/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "DeleteGroup/" + id;
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
