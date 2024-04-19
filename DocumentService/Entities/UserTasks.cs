using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DocumentService.Entities
{
    [Index(nameof(DocLink))]
    public class UserTasks
    {
        [Key]
        [Display(Name = "IDALL")]
        public int IDALL { get; set; }

        [Display(Name = "Срочность")]
        public String PriorityTask { get; set; } = string.Empty;

        [Display(Name = "Дата постановки")]
        public DateTime DataTask { get; set; } = DateTime.Now;

        [Display(Name = "Ответственный")]
        public String FioExec { get; set; } = string.Empty;

        [Display(Name = "Срок")]
        public DateTime DataTaskPlan { get; set; } = DateTime.Now;

        [Display(Name = "Выполнена")]
        public bool successfully { get; set; } = false;

        [Display(Name = "Выполнил")]
        public String? FioFinish { get; set; } = string.Empty;

        [Display(Name = "Дата выполнения")]
        public DateTime? DataFinish { get; set; }

        [Display(Name = "Результат")]
        public String? TextFinish { get; set; } = string.Empty;
        public Guid DocLink { get; set; }
        [Display(Name = "Задача")]
        public String TextTask { get; set; } = string.Empty;

        [Display(Name = "Автор / создатель")]
        public String ownertask { get; set; } = string.Empty;
        public String? Fiokey { get; set; } = string.Empty;
        public String? Fio { get; set; } = string.Empty;
        public Int64? IdFio { get; set; } = 0;
        [Display(Name = "E-mail")]
        public string? email { get; set; } = string.Empty;

        [Display(Name = "Использовать Mail")]
        public bool? usemail { get; set; } = false;

        [Display(Name = "Телеграм (Имя пользователя)")]
        public string? telegram { get; set; } = string.Empty;

        [Display(Name = "Использовать Телеграм")]
        public bool? usetelegram { get; set; } = false;
        public string? errors { get; set; } = string.Empty;


    }
}
