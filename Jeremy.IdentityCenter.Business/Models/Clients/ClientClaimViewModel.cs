using System.ComponentModel.DataAnnotations;

namespace Jeremy.IdentityCenter.Business.Models.Clients
{
    public class ClientClaimViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Value { get; set; }
    }
}