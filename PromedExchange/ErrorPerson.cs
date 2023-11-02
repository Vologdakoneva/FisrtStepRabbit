using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace PromedExchange
{
    public class ErrorPerson
    {
        [Key]
        [Display(Name = "IDALL")]
        public int IDALL { get; set; }

        public Guid? PersonLink { get; set; }

        [Display(Name = "Источник ошибки")]
        public string? ErrorSource { get; set; } = string.Empty;

        [Display(Name = "Описание ошибки")]
        public string? ErrorText { get; set; } = string.Empty;

        [Display(Name = "Дата ошибки")]
        public DateTime DataError { get; set; } = DateTime.Now;


    }
}
