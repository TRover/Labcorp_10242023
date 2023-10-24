using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication_LabCorp_10242023
{
    public abstract class Employee
    {
        // a constant for the number of workdays in a work year
        public const int WorkYear = 260;

        private float vacationDays;

        // float public property for vacation days that is not writable externally
        public float VacationDays
        {
            get { return vacationDays; }
            protected set { vacationDays = value; }
        }

        // a default constructor that sets the vacation days to 0
        public Employee()
        {
            VacationDays = 0;
        }

        public abstract void Work(int workdays);

        public void TakeVacation(float days)
        {
            // Check if the days used are positive and less than or equal to the available vacation days
            if (days > 0 && days <= VacationDays)
            {
                VacationDays -= days;
            }
            else
            {
                //throw an exception for invalid input
                throw new ArgumentException("Invalid number of vacation days");
            }
        }
    }

    public class HourlyEmployee : Employee
    {
        // Override the Work method to update the vacation days based on the hourly rate
        public override void Work(int days)
        {
            // Check if the days worked are positive and less than or equal to the work year
            if (days > 0 && days <= WorkYear)
            {
                // Add 10 vacation days for every WorkYear workdays
                VacationDays += (float)days / WorkYear * 10;
            }
            else
            {
                // Throw an exception if the input is invalid
                throw new ArgumentException("Invalid number of workdays");
            }
        }
    }
    public class SalariedEmployee : Employee
    {
        // Override the Work method to update the vacation days based on the salaried rate
        public override void Work(int days)
        {
            // Check if the days worked are positive and less than or equal to the work year
            if (days > 0 && days <= WorkYear)
            {
                // Add 15 vacation days for every WorkYear workdays
                VacationDays += (float)days / WorkYear * 15;
            }
            else
            {
                // Throw an exception if the input is invalid
                throw new ArgumentException("Invalid number of workdays");
            }
        }

    }
    public class Manager : SalariedEmployee
    {
        // Override the Work method to update the vacation days based on the manager rate
        public override void Work(int days)
        {
            // Check if the days worked are positive and less than or equal to the work year
            if (days > 0 && days <= WorkYear)
            {
                // Add 30 vacation days for every 260 workdays
                VacationDays += (float)days / WorkYear * 30;
            }
            else
            {
                // Throw an exception if the input is invalid
                throw new ArgumentException("Invalid number of workdays");
            }
        }
    }
    public partial class WebForm1 : System.Web.UI.Page
    { // Define a list of employees to store 10 instances of each type of employee on startup
        private List<Employee> employees;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write("Default workdays in a work year :" + Employee.WorkYear.ToString() + "<br/>");

            HourlyEmployee hourlyEmployee = new HourlyEmployee();
            Response.Write("Default Hourly Employee Vacation Days :" + hourlyEmployee.VacationDays.ToString() + "<br/>");
            SalariedEmployee salariedEmployee = new SalariedEmployee();
            Response.Write("Default Salaried Employee Vacation Days :" + salariedEmployee.VacationDays.ToString() + "<br/> ");
            Manager manager = new Manager();
            Response.Write("Default Manager Vacation Days :" + manager.VacationDays.ToString() + "<br/> ");

            if (!IsPostBack)
            {
                // Initialize the list of employees with random data
                employees = new List<Employee>();
                Random random = new Random();
                for (int i = 0; i < 10; i++)
                {

                    employees.Add(new HourlyEmployee());
                    employees.Add(new SalariedEmployee());
                    employees.Add(new Manager());
                    foreach (var employee in employees)
                    {
                        employee.Work(random.Next(0, Employee.WorkYear + 1));
                        employee.TakeVacation((float)random.NextDouble() * employee.VacationDays);
                    }
                }

                // Save the list of employees in session state
                Session["employees"] = employees;

                // Bind the list of employees to a grid view control to show their status
                GridView1.DataSource = employees;
                GridView1.DataBind();
            }
            else
            {
                // Retrieve the list of employees from session state
                employees = (List<Employee>)Session["employees"];
            }

        }
    }
}