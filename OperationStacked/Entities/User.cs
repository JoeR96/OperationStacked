using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OperationStacked.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(255)]
        public Guid CognitoUserId { get; set; }
        public int CurrentWeek { get; set; } = 1;
        public int CurrentDay { get; set; } = 1;
        public int WorkoutDaysInWeek { get; set; }
        public int WorkoutWeeks { get; set; }
        public ICollection<UserRole> UserRoles { get; set; } = new Collection<UserRole>();

        public User() { }
    }
}
