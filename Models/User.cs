using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Grigorova_Server.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string UserMail { get; set; } = string.Empty;

        [Display(Name = "User Birthday")]
        [DataType(DataType.Date)]
        public DateTime UserBirthday { get; set; }

        private string userRole = "Читатель";
        [Required]
        public string UserRole
        {
            get => userRole;
            set
            {
                if (value != "Читатель" && value != "Администратор")
                    throw new ArgumentException("Роль должна быть *Читатель* или *Администратор*");
                userRole = value;
            }
        }
    }
}
