using Jeremy.IdentityCenter.Database.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Jeremy.IdentityCenter.Business.Models.Clients
{
    public class ClientSecretsViewModel
    {
        public int ClientId { get; set; }

        public string ClientName { get; set; }

        public int ClientSecretId { get; set; }

        [Required]
        public string Type { get; set; } = "SharedSecret";

        public List<SelectItemViewModel> TypeList { get; set; } = new List<SelectItemViewModel>();

        public string Description { get; set; }

        [Required]
        public string Value { get; set; }

        public string HashType { get; set; }

        public HashType HashTypeEnum => Enum.TryParse(HashType, true, out HashType result)
            ? result
            : Database.Enums.HashType.Sha256;

        public List<SelectItemViewModel> HashTypes { get; set; }

        public DateTime? Expiration { get; set; }

        public int TotalCount { get; set; }

        public int PageSize { get; set; }

        public List<ClientSecretViewModel> ClientSecrets { get; set; } = new List<ClientSecretViewModel>();
    }
}