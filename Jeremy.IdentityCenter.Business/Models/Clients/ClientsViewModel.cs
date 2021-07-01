using System.Collections.Generic;

namespace Jeremy.IdentityCenter.Business.Models.Clients
{
    public class ClientsViewModel
    {
        public List<ClientViewModel> Clients { get; set; } = new List<ClientViewModel>();

        public int TotalCount { get; set; }

        public int PageSize { get; set; }
    }
}