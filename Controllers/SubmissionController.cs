using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcMongoExample.Models;
using MvcMongoExample.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MvcMongoExample.Controllers
{
    //[Authorize]
    public class SubmissionController : Controller
    {
        private readonly SubmissionService _subSvc;
        private readonly ILogger _logger;

        public SubmissionController(ILogger<SubmissionController> logger, SubmissionService submissionService)
        {
            _logger = logger;
            _subSvc = submissionService;
        }

        [AllowAnonymous]
        public ActionResult<IList<Submission>> Index() => View(_subSvc.Read());

        [HttpGet]
        public ActionResult Create() => View();

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult<Submission> Create(Submission submission)
        {
            _logger.LogInformation("Start to create an idea ");
            submission.Created = submission.LastUpdated = DateTime.Now;
            //submission.UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            submission.UserId = "13";//DEL
            //submission.UserName = User.Identity.Name;
            submission.UserName = "Said";
            if (ModelState.IsValid)
            {
                _subSvc.Create(submission);
            }
            _logger.LogInformation("Finish to create an idea ");
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult<Submission> Edit(string id) =>
            View(_subSvc.Find(id));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Submission submission)
        {
            _logger.LogInformation("Start to edit an idea ");
            submission.LastUpdated = DateTime.Now;
            submission.Created = submission.Created.ToLocalTime();
            if (ModelState.IsValid)
            {
                //if (User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value != submission.UserId)
                //{
                //    return Unauthorized();
                //} 
                _subSvc.Update(submission);
                return RedirectToAction("Index");
            }
            return View(submission);
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            _subSvc.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
