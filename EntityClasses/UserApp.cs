using System.ComponentModel.DataAnnotations;

namespace MobileApp.Entities
{
    public class UserApp
    {
        [Key]
        public string phone { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public string Family { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Fathers { get; set; } = string.Empty;
        public string snils { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public DateTime birthDayPerson { get; set; }
        public int? Sex_idPerson { get; set; } = 0;
        public string? Sex_Person { get; set; } = string.Empty;

        public bool isWorker { get; set; } = false;


    }
}
