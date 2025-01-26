using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Magistri.DTO {
    public class SubjectDto {
        public int Id { get; set; }
        [StringLength(10, ErrorMessage = "Nejde",MinimumLength =3)]
        [DisplayName("Subject name: ")]
        public string Name { get; set; }
    }
}
