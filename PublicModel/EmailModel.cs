using System.ComponentModel.DataAnnotations;

namespace PublicModel
{
    public class EmailModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Nhập email")]
        [RegularExpression(@"^[\w+][\w\.\-]+@[\w\-]+(\.\w{2,4})+$", ErrorMessage = "eg: support@gmail.com")]
        public string Email { get; set; }
    }
}
