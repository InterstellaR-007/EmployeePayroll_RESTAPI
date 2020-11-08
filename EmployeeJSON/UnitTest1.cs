using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;
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
        public void GivenEmployee_usingPOST_AddEmployeeObject()
        {
            IRestResponse response = addEmployee();

            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.Created);
            Employee dataresponse = JsonConvert.DeserializeObject<Employee>(response.Content);

            Assert.AreEqual("yohoo",dataresponse.name);
            Assert.AreEqual("10000", dataresponse.salary);


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

        private IRestResponse addEmployee()
        {
            RestRequest request = new RestRequest("/employees", Method.POST);
            JObject jObjectBody = new JObject();
            jObjectBody.Add("name", "yohoo");
            jObjectBody.Add("salary","10000");
            request.AddParameter("application/json", jObjectBody, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            return response;

        }
    }
}
