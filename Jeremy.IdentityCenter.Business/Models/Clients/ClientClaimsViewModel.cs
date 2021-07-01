using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Jeremy.IdentityCenter.Business.Models.Clients
{
    public class ClientClaimsViewModel
    {
        public int ClientClaimId { get; set; }

        public int ClientId { get; set; }

        public string ClientName { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Value { get; set; }

        public List<ClientClaimViewModel> ClientClaims { get; set; } = new List<ClientClaimViewModel>();

        public int TotalCount { get; set; }

        public int PageSize { get; set; }

    }
}