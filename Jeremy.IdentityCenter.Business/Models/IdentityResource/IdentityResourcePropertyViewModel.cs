using System.ComponentModel.DataAnnotations;

namespace Jeremy.IdentityCenter.Business.Models.IdentityResource
{
    public class IdentityResourcePropertyViewModel
    {
        public int Id { get; set; }

        [Display(Name = "键")]
        public string Key { get; set; }

        [Display(Name = "值")]
        public string Value { get; set; }
    }
}