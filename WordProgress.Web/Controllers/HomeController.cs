using System;
using Microsoft.AspNet.Mvc;
using WordProgress.Domain.Commands;
using WordProgress.ReadModels;
using WordProgress.Web.ViewModels.Home;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WordProgress.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEdumentDispatcher _edumentDispatcher;
        private readonly IWriterReader _writerReader;

        public HomeController(IEdumentDispatcher edumentDispatcher, IWriterReader writerReader)
        {
            _edumentDispatcher = edumentDispatcher;
            _writerReader = writerReader;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var model = new HomeIndexModel
            {
                WriterUserName = _writerReader.GetWriterDetails("").UserName
            };

            return View(model);
        }

        //[HttpPost]
        public IActionResult SetUserName()
        {
            _edumentDispatcher.Dispatcher.SendCommand(new RegisterWriter
            {
                Id = Guid.NewGuid(),
                UserName = "New Dugong!",
                Name = "Dugong Charlie"
            });

            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
