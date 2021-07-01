using AutoMapper;
using IdentityServer4.EntityFramework.Entities;
using Jeremy.IdentityCenter.Business.Models;
using Jeremy.IdentityCenter.Business.Models.Clients;
using Jeremy.IdentityCenter.Database.Common.Models;
using Jeremy.IdentityCenter.Database.Models;
using System.Collections.Generic;

namespace Jeremy.IdentityCenter.Business.Mappers.Extension
{
    public static class ClientExtension
    {
        internal static IMapper Mapper { get; }

        static ClientExtension()
        {
            Mapper = new MapperConfiguration(config => config.AddProfile<ClientProfile>()).CreateMapper();
        }

        public static ClientViewModel ToViewModel(this Client client)
        {
            return Mapper.Map<ClientViewModel>(client);
        }

        public static Client ToEntity(this ClientViewModel client)
        {
            return Mapper.Map<Client>(client);
        }

        public static ClientsViewModel ToViewModel(this PageList<Client> clients)
        {
            return Mapper.Map<ClientsViewModel>(clients);
        }

        public static List<SelectItemViewModel> ToViewModel(this List<SelectItem> selectItems)
        {
            return Mapper.Map<List<SelectItemViewModel>>(selectItems);
        }

        public static ClientSecretsViewModel ToViewModel(this PageList<ClientSecret> secrets)
        {
            return Mapper.Map<ClientSecretsViewModel>(secrets);
        }

        public static ClientSecretsViewModel ToViewModel(this ClientSecret secret)
        {
            return Mapper.Map<ClientSecretsViewModel>(secret);
        }

        public static ClientSecret ToEntity(this ClientSecretsViewModel secrets)
        {
            return Mapper.Map<ClientSecret>(secrets);
        }

        public static ClientPropertiesViewModel ToViewModel(this PageList<ClientProperty> properties)
        {
            return Mapper.Map<ClientPropertiesViewModel>(properties);
        }

        public static ClientPropertiesViewModel ToViewModel(this ClientProperty property)
        {
            return Mapper.Map<ClientPropertiesViewModel>(property);
        }

        public static ClientProperty ToEntity(this ClientPropertiesViewModel model)
        {
            return Mapper.Map<ClientProperty>(model);
        }

        public static ClientClaimsViewModel ToViewModel(this PageList<ClientClaim> clientClaims)
        {
            return Mapper.Map<ClientClaimsViewModel>(clientClaims);
        }

        public static ClientClaimsViewModel ToViewModel(this ClientClaim clientClaim)
        {
            return Mapper.Map<ClientClaimsViewModel>(clientClaim);
        }

        public static ClientClaim ToEntity(this ClientClaimsViewModel clientClaim)
        {
            return Mapper.Map<ClientClaim>(clientClaim);
        }
    }
}
