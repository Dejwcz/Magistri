
using Magistri.DTO;
using Magistri.Models;
using Microsoft.EntityFrameworkCore;

namespace Magistri.Services {
    public class StudentService {
        private ApplicationDbContext _dbContext;
        public StudentService(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        internal IEnumerable<StudentDto> GetAllStudents() {
            var allStudents = _dbContext.Students;
            var studentDtos = new List<StudentDto>();
            foreach (var student in allStudents) {
                studentDtos.Add(ModelToDto(student));
            }
            return studentDtos;
        }

        private static StudentDto ModelToDto(Student student) {
            if (student == null) return null;
            return new StudentDto {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                DateOfBirth = student.DateOfBirth,
            };
        }

        public async Task CreateStudentAsync(StudentDto studentDto) {
            await _dbContext.Students.AddAsync(DtoToModel(studentDto));
            await _dbContext.SaveChangesAsync();
        }

        private Student DtoToModel(StudentDto studentDto) {
            return new Student() {
                Id = studentDto.Id,
                FirstName = studentDto.FirstName,
                LastName = studentDto.LastName,
                DateOfBirth = studentDto.DateOfBirth,
            };
        }

        internal async Task<StudentDto> GetByIdAsync(int id) {
            var student = await _dbContext.Students.FirstOrDefaultAsync(x => x.Id == id);
            return ModelToDto(student);
        }

        internal async Task EditStudentAsync(int id, StudentDto editedStudent) {
            _dbContext.Update(DtoToModel(editedStudent));
            await _dbContext.SaveChangesAsync();
        }

        internal async Task DeleteAsync(int id) {
            var studentToDelete =  _dbContext.Students.FirstOrDefault(x => x.Id == id);
            _dbContext.Students.Remove(studentToDelete);
            await _dbContext.SaveChangesAsync();
        }
    }
}
