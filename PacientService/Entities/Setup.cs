using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace PacientService.Entities
{
    public class Setups
    {
        [Key]
        [Display(Name = "IDALL")]
        public int IDALL { get; set; }
        public string Namenastr { get; set; } = string.Empty;

        [Display(Name = "Наименование")]
        public string NamenRus { get; set; } = string.Empty;

        [Display(Name = "Значение")]
        public string ValueString { get; set; } = string.Empty;

    }
}
