using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OperationStacked.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int CurrentLiftDay { get; set; } = 0;
        public int CurrentLiftWeek { get; set; } = 0;
    }
}
