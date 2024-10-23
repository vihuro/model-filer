using System.ComponentModel.DataAnnotations;

namespace ModelFilter.Domain.Models
{
    public class UserModel : BaseModel
    {
        [StringLength(maximumLength: 50, MinimumLength = 2, ErrorMessage = "Message in 2 or 50 length")]
        public string UserName { get; set; }
        [StringLength(maximumLength: 50, MinimumLength = 2, ErrorMessage = "Message in 2 or 50 length")]
        public string Password { get; set; }
    }
}
