using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DocumentService.Entities
{
    [Index(nameof(DocLink))]
    public class DocAnaliz
    {
        [Key]
        [Display(Name = "IDALL")]
        public int IDALL { get; set; }
        public Guid DocLink { get; set; }
        [Display(Name = "Ном. Документа")]
        public String NomDoc { get; set; } = string.Empty;
        [Display(Name = "Дата Документа")]
        public DateTime Datadoc { get; set; } = DateTime.Now.Date;
        [Display(Name = "Ф.И.О. пациента")]
        public String Fio { get; set; } = string.Empty;
        public String Fiokey { get; set; } = string.Empty;
        public int IdFio { get; set; } = 0;
        [Display(Name = "Ф.И.О. врача")]
        public String FioDoctor { get; set; } = string.Empty;
        public String FioDoctorkey { get; set; } = string.Empty;
        public int IdDoctor { get; set; } = 0;
        [Display(Name = "Дата пробы")]
        public DateTime Databiomaterial { get; set; } = DateTime.Now.Date;
    }
}
