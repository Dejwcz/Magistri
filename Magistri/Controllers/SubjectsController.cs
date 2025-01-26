using Magistri.DTO;
using Magistri.Models;
using Magistri.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Magistri.Controllers {
    [Authorize]
    public class SubjectsController : Controller {
        private SubjectService _subjectService;

        public SubjectsController(SubjectService subjectService) {
            _subjectService = subjectService;
        }

        [HttpGet]
        public IActionResult Index() {
            var allSubjects = _subjectService.GetAllSubjects();
            return View(allSubjects);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create() {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(SubjectDto subjectDto) {
            await _subjectService.CreateSubjectAsync(subjectDto);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditAsync(int id) {
            var subjectToEdit = await _subjectService.GetByIdAsync(id);
            if (subjectToEdit == null) {
                return View("NotFound");
            }
            return View(subjectToEdit);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditAsync(int id, SubjectDto editedSubject) {
            await _subjectService.EditStudentAsync(id, editedSubject);
            if (editedSubject == null) return RedirectToAction("NotFound");
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id) {
            var subjectToDelete = await _subjectService.GetByIdAsync(id);
            if (subjectToDelete == null) {
                return View("NotFound");
            }
            await _subjectService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
