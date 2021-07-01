using Jeremy.IdentityCenter.Database.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Jeremy.IdentityCenter.Business.Models.ApiResources
{
    public class ApiResourceSecretsViewModel
    {
        public int ApiResourceId { get; set; }

        public int ApiSecretId { get; set; }

        public string ApiResourceName { get; set; }

        [Required]
        public string Type { get; set; } = "SharedSecret";

        public List<SelectItemViewModel> TypeList { get; set; } = new List<SelectItemViewModel>();
        public string Description { get; set; }

        [Required]
        public string Value { get; set; }

        public string HashType { get; set; }

        public HashType HashTypeEnum => Enum.TryParse(HashType, true, out HashType result) ? result : Database.Enums.HashType.Sha256;

        public List<SelectItemViewModel> HashTypes { get; set; } = new List<SelectItemViewModel>();

        public DateTime? Expiration { get; set; }

        public int TotalCount { get; set; }

        public int PageSize { get; set; }

        public List<ApiResourceSecretViewModel> ApiSecrets { get; set; } = new List<ApiResourceSecretViewModel>();
    }
}
