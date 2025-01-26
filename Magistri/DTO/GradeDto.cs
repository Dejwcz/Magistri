using Magistri.Models;
using System.ComponentModel.DataAnnotations;

namespace Magistri.DTO {
    public class GradeDto {
        public int Id { get; set; }
        [Display(Name = "Student")]
        public int StudentId { get; set; }
        [Display(Name = "Subject")]
        public int SubjectId { get; set; }
        public string Topic { get; set; }
        [Display(Name = "Grade")]
        public int Mark { get; set; }
        public DateTime Date { get; set; }
    }
}
