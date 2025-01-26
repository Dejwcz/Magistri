using Magistri.DTO;
using Magistri.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Magistri.Controllers {
    [Authorize]
    public class StudentsController : Controller {
        private StudentService _studentService;

        public StudentsController(StudentService studentService) {
            _studentService = studentService;
        }

        [HttpGet]
        public IActionResult Index() {
            var allStudents = _studentService.GetAllStudents();
            return View(allStudents);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create() {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(StudentDto studentDto) {
            await _studentService.CreateStudentAsync(studentDto);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin, Teacher")]
        [HttpGet]
        public async Task<IActionResult> EditAsync(int id) {
            var studentToEdit = await _studentService.GetByIdAsync(id);
            if (studentToEdit == null) {
                return View("NotFound");
            }
            return View(studentToEdit);
        }

        [Authorize(Roles = "Admin, Teacher")]
        [HttpPost]
        public async Task<IActionResult> EditAsync(int id, StudentDto editedStudent) {
            await _studentService.EditStudentAsync(id, editedStudent);
            if (editedStudent == null) return RedirectToAction("NotFound");
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id) {
            var studentToDelete = await _studentService.GetByIdAsync(id);
            if (studentToDelete == null) {
                return View("NotFound");
            }
            await _studentService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
