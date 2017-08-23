using Attendance_Management_System.Data.Repositories;
using Attendance_Management_System.Helpers;
using Attendance_Management_System.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Attendance_Management_System.Controllers
{
    [Authorize]
    public class LatenessController : Controller
    {
        private readonly LatenessRepository _latenessRepo; 

        public LatenessController()
        {
            _latenessRepo = new LatenessRepository(ConnectionString.DB);
        }

        // GET: Lateness
        public ActionResult Index()
        {
            var vm = new LatenessVM
            {
                Latenesses = _latenessRepo.GetLatenesses()
            };

            return View(vm);
        }
    }
}