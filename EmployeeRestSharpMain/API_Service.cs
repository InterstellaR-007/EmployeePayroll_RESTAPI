using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeRestSharpMain
{
    public class API_Service
    {
        
        RestClient client = new RestClient("http://localhost:3000");

        /// <summary> 
        /// Gets all employees objects from JSON File RestSharp API.
        /// </summary>
        /// <returns>
        ///   <br />
        /// </returns>
        public IRestResponse getAllEmployees()
        {
            //Creating the Rest Request of type GET http method call 
            RestRequest request = new RestRequest("/employees", Method.GET);

            //Execute the request
            IRestResponse response = client.Execute(request);
            return response;

        }



        /// <summary>
        /// Updates the Employee object field on JSON file using RestSharp API
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="salary">The salary.</param>
        /// <returns></returns>
        public IRestResponse UpdateNameAndSalary(int id, string name, string salary)
        {
            //Creating the Rest Request of type PUT http method call 
            RestRequest request = new RestRequest(string.Format("/employees/" + id), Method.PUT);
            // Creating json Object body to store updated fields
            JObject jObjectBody = new JObject();

            jObjectBody.Add("name", name);
            jObjectBody.Add("salary", salary);

            //Adding Object body as paramter to existing response
            request.AddParameter("application/json", jObjectBody, ParameterType.RequestBody);

            //Execute response
            IRestResponse response = client.Execute(request);
            return response;

        }



        /// <summary>
        /// Adds the employee object to json File using RestSharp API.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="salary">The salary.</param>
        /// <returns></returns>
        public IRestResponse addEmployee(string name, string salary)
        {
            //Creating the Rest Request of type POST http method call 
            RestRequest request = new RestRequest("/employees", Method.POST);

            //Creating json Object body to store fields
            JObject jObjectBody = new JObject();
            jObjectBody.Add("name", name);
            jObjectBody.Add("salary", salary);

            //Adding Object body as paramter to existing response
            request.AddParameter("application/json", jObjectBody, ParameterType.RequestBody);

            //Execute response
            IRestResponse response = client.Execute(request);
            return response;

        }


        /// <summary>
        /// Deletes the employee Object from JSON file using DELETE Request.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public IRestResponse deleteEmployee(int id)
        {

            //Creating the Rest Request of type DELETE http method call 
            RestRequest restRequest = new RestRequest("/employees/"+id, Method.DELETE);
            //Executing the request
            IRestResponse response = client.Execute(restRequest);

            return response;
        }
    }
}
