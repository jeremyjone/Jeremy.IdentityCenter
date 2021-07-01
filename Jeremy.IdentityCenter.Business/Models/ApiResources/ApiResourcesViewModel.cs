using System.Collections.Generic;

namespace Jeremy.IdentityCenter.Business.Models.ApiResources
{
    public class ApiResourcesViewModel
    {
        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public List<ApiResourceViewModel> Resources { get; set; } = new List<ApiResourceViewModel>();
    }
}