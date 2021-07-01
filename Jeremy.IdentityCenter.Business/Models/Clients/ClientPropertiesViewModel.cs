using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Jeremy.IdentityCenter.Business.Models.Clients
{
    public class ClientPropertiesViewModel
    {
        public int ClientPropertyId { get; set; }

        public int ClientId { get; set; }

        public string ClientName { get; set; }

        [Required]
        [Display(Name = "键")]
        public string Key { get; set; }

        [Required]
        [Display(Name = "值")]
        public string Value { get; set; }

        public List<ClientPropertyViewModel> ClientProperties { get; set; } = new List<ClientPropertyViewModel>();

        public int TotalCount { get; set; }

        public int PageSize { get; set; }
    }
}