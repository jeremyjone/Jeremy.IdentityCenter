using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Jeremy.IdentityCenter.Business.Models.IdentityResource
{
    public class IdentityResourcePropertiesViewModel
    {
        public int IdentityResourcePropertyId { get; set; }

        public int IdentityResourceId { get; set; }

        public string IdentityResourceName { get; set; }

        [Required]
        public string Key { get; set; }

        [Required]
        public string Value { get; set; }

        public List<IdentityResourcePropertyViewModel> IdentityResourceProperties { get; set; } = new List<IdentityResourcePropertyViewModel>();

        public int TotalCount { get; set; }

        public int PageSize { get; set; }
    }
}