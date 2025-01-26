using Magistri.DTO;
using Magistri.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Magistri.Controllers {
    [Authorize]
    public class GradesController : Controller {
        private GradeService _gradeService;

        public GradesController(GradeService gradeService) {
            _gradeService = gradeService;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync() {
            var gradesVievModel = await _gradeService.GetAllGradesAsync();
            return View(gradesVievModel);
        }

        [HttpGet]
        public async Task<IActionResult> CreateAsync() {
            await FillSelects();
            return View();
        }

        private async Task FillSelects() {
            var dropDownData = await _gradeService.GetDropdownsData();
            ViewBag.Students = new SelectList(dropDownData.Students, "Id", "FullName");
            ViewBag.Subjects = new SelectList(dropDownData.Subjects, "Id", "Name");
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(GradeDto newGrade) {
            await _gradeService.CreateAsync(newGrade);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(int id) {
            var gradeToEdit = await _gradeService.GetByIdAsync(id);
            await FillSelects();
            if (gradeToEdit == null) {
                return View("NotFound");
            }
            return View(gradeToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(int id, GradeDto gradeDto) {
            await _gradeService.UpdateAsync(id, gradeDto);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsync(int id) {
            await _gradeService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
