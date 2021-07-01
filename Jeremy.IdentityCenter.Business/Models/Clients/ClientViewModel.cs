using IdentityServer4.Models;
using Jeremy.IdentityCenter.Database.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Jeremy.IdentityCenter.Business.Models.Clients
{
    public class ClientViewModel
    {
        #region 基本信息

        public int Id { get; set; }
        public bool NonEditable { get; set; }
        public ClientType ClientType { get; set; }
        [Display(Name = "启用")] public bool Enabled { get; set; } = true;
        [Required] [Display(Name = "客户端ID")] public string ClientId { get; set; }
        [Required] [Display(Name = "客户端名称")] public string ClientName { get; set; }
        [Display(Name = "客户端描述")] public string Description { get; set; } = "";
        [Display(Name = "协议类型")] public string ProtocolType { get; set; } = "oidc";
        public List<SelectItemViewModel> ProtocolTypes { get; set; }
        [Display(Name = "需要客户端秘钥")] public bool RequireClientSecret { get; set; } = true;
        [Display(Name = "客户端秘钥")] public List<ClientSecretViewModel> ClientSecrets { get; set; } = new List<ClientSecretViewModel>();
        [Display(Name = "需要请求对象")] public bool RequireRequestObject { get; set; }
        [Display(Name = "需要 Pkce")] public bool RequirePkce { get; set; } = true;
        [Display(Name = "需要纯文本 Pkce")] public bool AllowPlainTextPkce { get; set; }
        [Display(Name = "允许离线访问")] public bool AllowOfflineAccess { get; set; }
        [Display(Name = "允许通过浏览器访问令牌")] public bool AllowAccessTokensViaBrowser { get; set; }
        [Display(Name = "回调地址")] public List<string> RedirectUris { get; set; } = new List<string>();
        public string RedirectUrisItems { get; set; }
        [Display(Name = "允许的作用域")] public List<string> AllowedScopes { get; set; } = new List<string>();
        public string AllowedScopesItems { get; set; }
        [Display(Name = "允许的授权类型")] public List<string> AllowedGrantTypes { get; set; } = new List<string>();
        public string AllowedGrantTypesItems { get; set; }
        [Display(Name = "属性")] public List<ClientPropertyViewModel> Properties { get; set; } = new List<ClientPropertyViewModel>();
        [Display(Name = "创建时间")] public DateTime Created { get; set; } = DateTime.UtcNow;
        [Display(Name = "更新时间")] public DateTime? Updated { get; set; }
        [Display(Name = "最后一次访问时间")] public DateTime? LastAccessed { get; set; }

        #endregion



        #region 认证

        [Display(Name = "前端通道注销地址")] public string FrontChannelLogoutUri { get; set; }
        [Display(Name = "需要前端通道注销会话")] public bool FrontChannelLogoutSessionRequired { get; set; } = true;
        [Display(Name = "后端通道注销地址")] public string BackChannelLogoutUri { get; set; }
        [Display(Name = "需要后端通道注销会话")] public bool BackChannelLogoutSessionRequired { get; set; } = true;
        [Display(Name = "启用本地登录")] public bool EnableLocalLogin { get; set; } = true;
        [Display(Name = "注销回调地址")] public List<string> PostLogoutRedirectUris { get; set; } = new List<string>();
        public string PostLogoutRedirectUrisItems { get; set; }
        [Display(Name = "身份提供程序限制")] public List<string> IdentityProviderRestrictions { get; set; } = new List<string>();
        public string IdentityProviderRestrictionsItems { get; set; }
        [Display(Name = "用户 SSO 生命周期")] public int? UserSsoLifetime { get; set; }

        #endregion




        #region 令牌

        [Display(Name = "身份令牌的生命周期")] public int IdentityTokenLifetime { get; set; } = 300;
        [Display(Name = "允许的身份令牌的签入算法")] public List<string> AllowedIdentityTokenSigningAlgorithms { get; set; } = new List<string>();
        public string AllowedIdentityTokenSigningAlgorithmsItems { get; set; }
        [Display(Name = "始终在身份令牌中包含用户声明")] public bool AlwaysIncludeUserClaimsInIdToken { get; set; }
        [Display(Name = "访问令牌的生命周期")] public int AccessTokenLifetime { get; set; } = 3600;
        [Display(Name = "访问令牌类型")] public int AccessTokenType { get; set; } = (int)0; // AccessTokenType.Jwt;
        public List<SelectItemViewModel> AccessTokenTypes { get; set; }
        [Display(Name = "授权码的生命周期")] public int AuthorizationCodeLifetime { get; set; } = 300;
        [Display(Name = "令牌的绝对刷新生命周期")] public int AbsoluteRefreshTokenLifetime { get; set; } = 2592000;
        [Display(Name = "令牌的滑动刷新生命周期")] public int SlidingRefreshTokenLifetime { get; set; } = 1296000;
        [Display(Name = "刷新时更新访问令牌的声明")] public bool UpdateAccessTokenClaimsOnRefresh { get; set; }
        [Display(Name = "刷新令牌时 Token 的使用方式")] public int RefreshTokenUsage { get; set; } = (int)TokenUsage.OneTimeOnly;
        public List<SelectItemViewModel> RefreshTokenUsages { get; set; }
        [Display(Name = "刷新令牌的过期方式")] public int RefreshTokenExpiration { get; set; } = (int)TokenExpiration.Absolute;
        public List<SelectItemViewModel> RefreshTokenExpirations { get; set; }
        [Display(Name = "允许的跨域源")] public List<string> AllowedCorsOrigins { get; set; } = new List<string>();
        public string AllowedCorsOriginsItems { get; set; }
        [Display(Name = "包含 Jwt 标识")] public bool IncludeJwtId { get; set; }
        [Display(Name = "配对主体盐值")] public string PairWiseSubjectSalt { get; set; }
        [Display(Name = "客户端声明前缀")] public string ClientClaimsPrefix { get; set; } = "client_";
        [Display(Name = "始终发送客户端声明")] public bool AlwaysSendClientClaims { get; set; }
        [Display(Name = "客户端声明")] public List<ClientClaimViewModel> Claims { get; set; } = new List<ClientClaimViewModel>();

        #endregion




        #region 同意屏幕

        [Display(Name = "同意的生命周期")] public int? ConsentLifetime { get; set; } = null;
        [Display(Name = "需要同意")] public bool RequireConsent { get; set; } = true;
        [Display(Name = "允许记住同意")] public bool AllowRememberConsent { get; set; } = true;
        [Display(Name = "客户端地址")] public string ClientUri { get; set; }
        [Display(Name = "徽标地址")] public string LogoUri { get; set; }

        #endregion



        #region 设备

        [Display(Name = "用户码类型")] public string UserCodeType { get; set; }
        [Display(Name = "设备码生命周期")] public int DeviceCodeLifetime { get; set; } = 300;

        #endregion

    }
}
