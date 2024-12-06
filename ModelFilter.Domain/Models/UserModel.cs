using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelFilter.Domain.Models
{
    [Table("Users")]
    public class UserModel : BaseModel
    {
        [StringLength(maximumLength: 50, MinimumLength = 2, ErrorMessage = "Message in 2 or 50 length")]
        public string UserName { get; set; }
        [StringLength(maximumLength: 100, ErrorMessage = "The max length is 100")]
        public string? Name { get; set; }
        [StringLength(maximumLength: 250, MinimumLength = 2, ErrorMessage = "Message in 2 or 50 length")]
        public string Password { get; set; }
    }
}
