using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeRestSharpMain
{
    public class MainUI
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Employee Management Program based on REST Sharp API \n");
            

            bool exit_Program = false;
            API_Service service = new API_Service();
            string Field_Title = String.Format("{0,-15}{1,-15}{2,-15}\n", "ID", "Name", "Salary");

            while (exit_Program != true)
            {
                string emp_Name;
                string emp_Salary;
                int emp_ID;
                IRestResponse response;

                Console.WriteLine("Choose among the following operations :");
                Console.WriteLine("1: Add a new Employee ");
                Console.WriteLine("2: Update an existing Employee Details ");
                Console.WriteLine("3: Delete an existing Employee ");
                Console.WriteLine("4: Display all the current Employees ");
                Console.WriteLine("5: Exit programs \n ");

                int input_Option = int.Parse(Console.ReadLine());
                Console.WriteLine();

                switch (input_Option)
                {
                    case 1:
                        Console.WriteLine("Enter the Employee Name :");
                         emp_Name = Console.ReadLine();
                        Console.WriteLine("Enter the Salary");
                         emp_Salary = Console.ReadLine();

                        response = service.addEmployee(emp_Name, emp_Salary);
                        if (response.StatusCode.Equals(System.Net.HttpStatusCode.Created))
                            Console.WriteLine("Employee Added Successfully !\n");
                        else
                            Console.WriteLine("Employee Not Added\n");

                        break;

                    case 2:
                        Console.WriteLine("Enter the existing Employee ID:");
                        emp_ID = int.Parse(Console.ReadLine());
                        Console.WriteLine("Enter the Updated Employee Name :");
                        emp_Name = Console.ReadLine();
                        Console.WriteLine("Enter the Updated Salary");
                        emp_Salary = Console.ReadLine();

                        response = service.UpdateNameAndSalary(emp_ID, emp_Name, emp_Salary);

                        if (response.StatusCode.Equals(System.Net.HttpStatusCode.OK))
                            Console.WriteLine("Employee Updated successfully !\n");
                        else
                            Console.WriteLine("Employee Not Updated\n");

                        break;

                    case 3:
                        Console.WriteLine("Enter the existing Employee ID to be deleted :\n");
                        emp_ID = int.Parse(Console.ReadLine());

                        response = service.deleteEmployee(emp_ID);

                        if (response.StatusCode.Equals(System.Net.HttpStatusCode.OK))
                            Console.WriteLine("Employee Deleted successfully !\n");
                        else
                            Console.WriteLine("Employee Not Deleted\n");
                        break;

                    case 4:

                        response = service.getAllEmployees();

                        List<EmployeeObject> dataresponse = JsonConvert.DeserializeObject<List<EmployeeObject>>(response.Content);
                        Console.WriteLine(Field_Title);

                        foreach(var employee in dataresponse)
                        {
                            string tabular_Output = String.Format("{0,-15}{1,-15}{2,-15}", employee.id, employee.name,employee.salary);
                            Console.WriteLine(tabular_Output);
                        }

                        Console.WriteLine();

                        break;

                    case 5:

                        exit_Program = true;
                        break;



                }
            }
        }
    }
}
