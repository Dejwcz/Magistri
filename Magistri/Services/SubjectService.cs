using Magistri.DTO;
using Magistri.Models;
using Microsoft.EntityFrameworkCore;

namespace Magistri.Services {
    public class SubjectService {
        private ApplicationDbContext _dbContext;
        public SubjectService(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        internal IEnumerable<SubjectDto> GetAllSubjects() {
            var allSubjects = _dbContext.Subjects;
            var subjectDtos = new List<SubjectDto>();
            foreach (var subject in allSubjects) {
                subjectDtos.Add(ModelToDto(subject));
            }
            return subjectDtos;
        }

        private static SubjectDto ModelToDto(Subject subject) {
            if (subject == null) return null;
            return new SubjectDto {
                Id = subject.Id,
                Name = subject.Name,
            };
        }

        public async Task CreateSubjectAsync(SubjectDto subjectDto) {
            await _dbContext.Subjects.AddAsync(DtoToModel(subjectDto));
            await _dbContext.SaveChangesAsync();
        }

        private Subject DtoToModel(SubjectDto subjectDto) {
            return new Subject {
                Id = subjectDto.Id,
                Name = subjectDto.Name,
            };
        }

        internal async Task<SubjectDto> GetByIdAsync(int id) {
            var subject  = await _dbContext.Subjects.FirstOrDefaultAsync(x => x.Id == id);
            return ModelToDto(subject);
        }

        internal async Task EditStudentAsync(int id, SubjectDto editedSubject) {
            _dbContext.Subjects.Update(DtoToModel(editedSubject));
            await _dbContext.SaveChangesAsync();
        }

        internal async Task DeleteAsync(int id) {
            var subjectToDelete = await _dbContext.Subjects.FirstOrDefaultAsync(x =>x.Id == id);
            _dbContext.Remove(subjectToDelete);
            await _dbContext.SaveChangesAsync();
        }
    }
}
