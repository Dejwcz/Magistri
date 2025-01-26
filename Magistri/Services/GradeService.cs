
using Magistri.DTO;
using Magistri.Models;
using Magistri.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Magistri.Services {
    public class GradeService {
        private ApplicationDbContext _dbContext;

        public GradeService(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        internal async Task<GradesDropdownsViewModel> GetDropdownsData() {
            var gradesDropdownData = new GradesDropdownsViewModel() {
                Students = await _dbContext.Students.OrderBy(x => x.LastName).ToListAsync(),
                Subjects = await _dbContext.Subjects.OrderBy(x => x.Name).ToListAsync(),
            };
            return gradesDropdownData;
        }
        public async Task CreateAsync(GradeDto gradeDto) {
            Grade gradeToInsert = await DtoToModelAsync(gradeDto);
            await _dbContext.AddAsync(gradeToInsert);
            await _dbContext.SaveChangesAsync();
        }

        private async Task<Grade> DtoToModelAsync(GradeDto gradeDto) {
            return new Grade {
                Id = gradeDto.Id,
                Date = DateTime.Now,
                Mark = gradeDto.Mark,
                Student = await _dbContext.Students.FirstOrDefaultAsync(x => x.Id == gradeDto.StudentId),
                Subject = await _dbContext.Subjects.FirstOrDefaultAsync(x => x.Id == gradeDto.SubjectId),
                Topic = gradeDto.Topic,
            };
        }
        public async Task<IEnumerable<GradesViewModel>> GetAllGradesAsync() {
            var grades = await _dbContext.Grades.Include(gr => gr.Student).Include(gr => gr.Subject).ToListAsync();
            List<GradesViewModel> gradesViewModels = new List<GradesViewModel>();
            foreach (var grade in grades) {
                gradesViewModels.Add(new GradesViewModel {
                    Id = grade.Id,
                    Date = grade.Date,
                    Mark = grade.Mark,
                    StudentName = grade.Student.FullName,
                    SubjectName = grade.Subject.Name,
                    Topic = grade.Topic,
                });
            }
            return gradesViewModels;
        }

        internal async Task<GradeDto> GetByIdAsync(int id) {
            var grade = await _dbContext.Grades.Include(gr => gr.Student).Include(gr => gr.Subject).FirstOrDefaultAsync(x => x.Id == id);
            if (grade == null) {
                return null;
            }
            return ModelToDto(grade);
        }

        private GradeDto ModelToDto(Grade grade) {
            return new GradeDto {
                Id = grade.Id,
                Date = grade.Date,
                Mark = grade.Mark,
                StudentId = grade.Student.Id,
                SubjectId = grade.Subject.Id,
                Topic = grade.Topic,
            };
        }

        internal async Task UpdateAsync(int id, GradeDto gradeDto) {
            var grade = await DtoToModelAsync(gradeDto);
            _dbContext.Grades.Update(grade);
            await _dbContext.SaveChangesAsync();
        }

        internal async Task DeleteAsync(int id) {
            var grade = await _dbContext.Grades.FirstOrDefaultAsync(x => x.Id == id);
            _dbContext.Remove(grade);
            await _dbContext.SaveChangesAsync();
        }
    }
}
