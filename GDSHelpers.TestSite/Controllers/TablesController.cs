using System.Collections.Generic;
using GDSHelpers.TestSite.Models.Table;
using Microsoft.AspNetCore.Mvc;

namespace GDSHelpers.TestSite.Controllers
{
    public class TablesController : Controller
    {
        public IActionResult Index()
        {
            var people = new List<Person> {
                new Person { Age = 30, FirstName = "Matt", LastName = "Mitchell", EmailAddress = "na" },
                new Person { Age = 31, FirstName = "Ant", LastName = "Carbutt", EmailAddress = "anthony.carbutt@cqc.org.uk" },
                new Person { Age = 32, FirstName = "Paul", LastName = "Senior", EmailAddress = "na" }
            };

            var vm = new TableVM
            {
                TeamMembers = people
            };

            return View(vm);
        }
    }
}