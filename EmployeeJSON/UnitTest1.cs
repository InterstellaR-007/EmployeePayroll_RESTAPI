using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace EmployeeJSON
{
    /// <summary>
    /// NUnit Test Class to check CURD Operation over JSon Server using Rest Sharp API
    /// Note: Start the json server from command prompt first
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int id { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string name { get; set; }
        /// <summary>
        /// Gets or sets the salary.
        /// </summary>
        /// <value>
        /// The salary.
        /// </value>
        public string salary { get; set; }

    }


    [TestClass]
    public class UnitTest1
    {
        //Declare the RestClient
        RestClient client;

        /// <summary>
        /// Setups this instance.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            //instantiating the rest client with json server url
            client = new RestClient("http://localhost:3000");
        }

        /// <summary>
        /// Test Method to check Read Operation on JSON Server using RestSharp API 
        /// </summary>
        [TestMethod]
        public void OncallingList_ReturnEmployeeList()
        {
            IRestResponse response = getAllEmployees();

            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            List<Employee> dataresponse = JsonConvert.DeserializeObject<List<Employee>>(response.Content);

            //Assert might fail due to updation/addition/deleting test methods which is intended
            Assert.AreEqual(4, dataresponse.Count);

        }


        /// <summary>
        /// Test Method to check Addition operation of multiple Employee Object using Rest Sharp API
        /// </summary>
        [TestMethod]
        public void GivenEmployee_usingPOST_AddMultipleEmployeeObject()
        {
            IRestResponse response1 = addEmployee("newEmp1","200");
            IRestResponse response2 = addEmployee("newEmp2", "300");
            IRestResponse response3 = addEmployee("newEmp3", "400");

            Assert.AreEqual(response1.StatusCode, System.Net.HttpStatusCode.Created);

            List<Employee> dataresponse = new List<Employee>();

            dataresponse.Add(JsonConvert.DeserializeObject<Employee>(response1.Content));
            dataresponse.Add(JsonConvert.DeserializeObject<Employee>(response2.Content));
            dataresponse.Add(JsonConvert.DeserializeObject<Employee>(response3.Content));

            //Checking Added accounts count is 3
            Assert.AreEqual(3,dataresponse.Count);


        }

        /// <summary>
        /// Test Method to check the Updation of Employee Object in Json File using REST Sharp API.
        /// </summary>
        [TestMethod]
        public void GivenEmployee_usingPUT_UpdateEmployeeSalary()
        {
            //Calling Update Salary request to update existing object field using XPUT
            IRestResponse response1 = UpdateSalary(3, "updated_name", "12345");

            // Comparing the status code of updation
            Assert.AreEqual(response1.StatusCode, System.Net.HttpStatusCode.OK);

            List<Employee> dataresponse = new List<Employee>();

            //Returning and desearlizing the updated employee object
            dataresponse.Add(JsonConvert.DeserializeObject<Employee>(response1.Content));
            

            //Comapring the required updated field values
            Assert.AreEqual("updated_name",dataresponse.First().name );
            Assert.AreEqual("12345", dataresponse.First().salary);



        }


        /// <summary>
        /// Test Method to check Deletion operation of employee in JSON file using RestSharp API
        /// </summary>
        [TestMethod]
        public void GivenEmployee_usingDELETE_DeletesEmployee()
        {
            //Creating the Rest Request of type DELETE http method call 
            RestRequest restRequest = new RestRequest("/employees/9", Method.DELETE);

            //Executing the request
            IRestResponse response = client.Execute(restRequest);

            //Comparing the result of status code 
            // Deletion might fail if that object with id already deleted return <Not Found> status code
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);


        }


        /// <summary> 
        /// Gets all employees objects from JSON File RestSharp API.
        /// </summary>
        /// <returns>
        ///   <br />
        /// </returns>
        private IRestResponse getAllEmployees()
        {
            //Creating the Rest Request of type GET http method call 
            RestRequest request = new RestRequest("/employees", Method.GET);

            //Execute the request
            IRestResponse response = client.Execute(request);
            return response;

        }



        /// <summary>
        /// Adds the employee object to json File using RestSharp API.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="salary">The salary.</param>
        /// <returns></returns>
        private IRestResponse addEmployee(string name, string salary)
        {
            //Creating the Rest Request of type POST http method call 
            RestRequest request = new RestRequest("/employees", Method.POST);

            //Creating json Object body to store fields
            JObject jObjectBody = new JObject();
            jObjectBody.Add("name", name);
            jObjectBody.Add("salary",salary);

            //Adding Object body as paramter to existing response
            request.AddParameter("application/json", jObjectBody, ParameterType.RequestBody);

            //Execute response
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
        private IRestResponse UpdateSalary(int id, string name,string salary)
        {
            //Creating the Rest Request of type PUT http method call 
            RestRequest request = new RestRequest(string.Format("/employees/"+id), Method.PUT);
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
    }
}
