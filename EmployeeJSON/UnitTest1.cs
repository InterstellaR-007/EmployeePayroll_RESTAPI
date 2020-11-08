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
    /// 
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

        RestClient client;

        /// <summary>
        /// Setups this instance.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            client = new RestClient("http://localhost:3000");
        }

        /// <summary>Called when [list return employee list].</summary>
        [TestMethod]
        public void OncallingList_ReturnEmployeeList()
        {
            IRestResponse response = getAllEmployees();

            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            List<Employee> dataresponse = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            Assert.AreEqual(4, dataresponse.Count);

        }

        [TestMethod]
        public void GivenEmployee_usingPOST_AddMultipleEmployeeObject()
        {
            IRestResponse response1 = addEmployee("add1","200");
            IRestResponse response2 = addEmployee("add2", "300");
            IRestResponse response3 = addEmployee("add3", "400");

            Assert.AreEqual(response1.StatusCode, System.Net.HttpStatusCode.Created);

            List<Employee> dataresponse = new List<Employee>();

            dataresponse.Add(JsonConvert.DeserializeObject<Employee>(response1.Content));
            dataresponse.Add(JsonConvert.DeserializeObject<Employee>(response2.Content));
            dataresponse.Add(JsonConvert.DeserializeObject<Employee>(response3.Content));


            Assert.AreEqual(3,dataresponse.Count);
            


        }

        [TestMethod]
        public void GivenEmployee_usingPUT_UpdateEMployeeSalary()
        {
            IRestResponse response1 = UpdateSalary(3, "updated_name", "12345");

            

            Assert.AreEqual(response1.StatusCode, System.Net.HttpStatusCode.OK);

            List<Employee> dataresponse = new List<Employee>();

            dataresponse.Add(JsonConvert.DeserializeObject<Employee>(response1.Content));
            


            Assert.AreEqual("updated_name",dataresponse.First().name );
            Assert.AreEqual("12345", dataresponse.First().salary);



        }


        /// <summary>
        /// Givens the employee using delete deletes employee.
        /// </summary>
        [TestMethod]
        public void GivenEmployee_usingDELETE_DeletesEmployee()
        {

            RestRequest restRequest = new RestRequest("/employees/4", Method.DELETE);

            IRestResponse response = client.Execute(restRequest);

            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);


        }


        /// <summary>Gets all employees.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        private IRestResponse getAllEmployees()
        {
            RestRequest request = new RestRequest("/employees", Method.GET);
            IRestResponse response = client.Execute(request);
            return response;

        }

        private IRestResponse addEmployee(string name, string salary)
        {
            RestRequest request = new RestRequest("/employees", Method.POST);
            JObject jObjectBody = new JObject();
            jObjectBody.Add("name", name);
            jObjectBody.Add("salary",salary);
            request.AddParameter("application/json", jObjectBody, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            return response;

        }

        private IRestResponse UpdateSalary(int id, string name,string salary)
        {
            RestRequest request = new RestRequest(string.Format("/employees/"+id), Method.PUT);
            JObject jObjectBody = new JObject();
            jObjectBody.Add("name", name);
            jObjectBody.Add("salary", salary);
            request.AddParameter("application/json", jObjectBody, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            return response;

        }
    }
}
