
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;
using System.Xml.Linq;

namespace PacientService.Entities
{
    public class Person
    {
        [Key]
        [Display(Name = "IDALL")]
        public int IDALL { get; set; }

        public Guid? PersonLink { get; set; } 

        [Display(Name = "Фамилия")]
        public string FamilyPerson { get; set; } = string.Empty;

        [Display(Name = "Имя")]
        public string NamePerson { get; set; } = string.Empty;

        [Display(Name = "Отчество")]
        public string FathersPerson { get; set; } = string.Empty;

        [Display(Name = "Дата рождения")]
        public DateTime? birthDayPerson { get; set; }

        [Display(Name = "Дата создания")]
        public DateTime DataCreatePerson { get; set; } = DateTime.Now;

        [Display(Name = "Дата изменения")]
        public DateTime DateChangePerson { get; set; } = DateTime.Now;

        [Display(Name = "ID в промеде")]
        public int IdPromedPerson { get; set; } = 0;

        [Display(Name = "ID пол")]
        public int Sex_idPerson { get; set; } = 0;

        [Display(Name = "Пол")]
        public string Sex_Person { get; set; } = string.Empty;


        [Display(Name = "СНИЛС")]
        public string SnilsPerson { get; set; } = string.Empty;

        [Display(Name = "Телефон")]
        public string PhonePerson { get; set; } = string.Empty;

        [Display(Name = "ID Соц.Статус")]
        public int SocStatus_id_Person { get; set; } = 0;

        [Display(Name = "Соц. Статус")]
        public string SocStatus_Person { get; set; } = string.Empty;

        [Display(Name = "ИНН")]
        public string Inn_Person { get; set; } = string.Empty;
    }
}
