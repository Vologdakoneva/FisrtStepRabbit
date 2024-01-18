using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DocumentService.Entities
{
    public class Docsummary
    {
        public class DocError
        {
            public string ErrorSource { get; set; } = string.Empty;
            public string ErrorText { get; set; } = string.Empty;
        }
        public DocError[] Errors { get; set; } = new DocError[0];

        public DocItems[] Items { get; set; } = new DocItems[0];
        public DocAnaliz docAnaliz { get; set; }
    }

    public class DocItems
    {
        [Display(Name = "Успешно")]
        public bool successfully { get; set; } = false;
        public Guid DocLink { get; set; }
        [Display(Name = "Наименование")]
        public String AnalizText { get; set; } = string.Empty;
        [Display(Name = "Норма")]
        public String norma { get; set; } = string.Empty;
        [Display(Name = "Результат")]
        public String result { get; set; } = string.Empty;
        [Display(Name = "код УЕТ")]
        public String uet { get; set; } = string.Empty;
        [Display(Name = "код")]
        public String kod { get; set; } = string.Empty;

    }


    [Index(nameof(DocLink))]
    public class DocAnaliz
    {
        [Key]
        [Display(Name = "IDALL")]
        public int IDALL { get; set; }
        
        [Display(Name = "Успешно")]
        public bool successfully { get; set; } = false; 
        public Guid DocLink { get; set; }
        [Display(Name = "Ном. Документа")]
        public String NomDoc { get; set; } = string.Empty;
        [Display(Name = "Дата Документа")]
        public DateTime Datadoc { get; set; } = DateTime.Now.Date;
        [Display(Name = "Ф.И.О. пациента")]
        public String Fio { get; set; } = string.Empty;
        public String Fiokey { get; set; } = string.Empty;
        public Int64? IdFio { get; set; } = 0;
        [Display(Name = "Ф.И.О. врача")]
        public String FioDoctor { get; set; } = string.Empty;
        public String FioDoctorkey { get; set; } = string.Empty;
        public Int64? IdDoctor { get; set; } = 0;
        [Display(Name = "Дата пробы")]
        public DateTime Databiomaterial { get; set; } = DateTime.Now.Date;
        [Display(Name = "Анализы")]
        public String Items { get; set; } = string.Empty;
        [Display(Name = "Ошибки")]
        public String Errors { get; set; } = string.Empty;
        [Display(Name = "Рекомендации")]
        public String Recomedation { get; set; } = string.Empty;
        [Display(Name = "Анализ")]
        public String? AnalizHead { get; set; } = string.Empty;
        [Display(Name = "Анализ Код УЕТ")]
        public String? UetHead { get; set; } = string.Empty;
        [Display(Name = "Дата мзменения")]
        public DateTime DataChange { get; set; } = DateTime.Now;
    }
}
