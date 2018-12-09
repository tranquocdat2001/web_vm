using System.ComponentModel.DataAnnotations;

namespace PublicModel
{
    public class CallMeModel
    {
        //[Required(ErrorMessageResourceType = typeof(Webone.Resource.Resource), ErrorMessageResourceName = "Required_Phone")]
        //[RegularExpression(@"0[1-9][0-9]{8,12}", ErrorMessageResourceType = typeof(Webone.Resource.Resource), ErrorMessageResourceName = "InvalidFormatPhone")]
        [Required(ErrorMessage = "Nhập số điện thoại")]
        [RegularExpression(@"0[1-9][0-9]{8,12}", ErrorMessage = "Số điện thoại không đúng định dạng.")]
        public string Phone { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
    }
}
