using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Jeremy.IdentityCenter.Business.Models.ApiResources
{
    public class ApiResourcePropertiesViewModel
    {
        public int ApiResourcePropertyId { get; set; }

        public int ApiResourceId { get; set; }

        public string ApiResourceName { get; set; }

        [Required]
        public string Key { get; set; }

        [Required]
        public string Value { get; set; }

        public List<ApiResourcePropertyViewModel> ApiResourceProperties { get; set; } = new List<ApiResourcePropertyViewModel>();

        public int TotalCount { get; set; }

        public int PageSize { get; set; }
    }
}
