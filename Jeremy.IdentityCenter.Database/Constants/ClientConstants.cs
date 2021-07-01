using Jeremy.IdentityCenter.Database.Models;
using System.Collections.Generic;

namespace Jeremy.IdentityCenter.Database.Constants
{
    public static class ClientConstants
    {
        public static List<string> SecretTypes =>
            new List<string>
            {
                "SharedSecret",
                "X509Thumbprint",
                "X509Name",
                "X509CertificateBase64"
            };

        public static List<string> StandardClaims =>
            //http://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
            new List<string>
            {
                "name",
                "given_name",
                "family_name",
                "middle_name",
                "nickname",
                "preferred_username",
                "profile",
                "picture",
                "website",
                "gender",
                "birthdate",
                "zoneinfo",
                "locale",
                "address",
                "updated_at"
            };

        public static List<string> GrantTypes =>
            new List<string>
            {
                "implicit",
                "client_credentials",
                "authorization_code",
                "hybrid",
                "password",
                "urn:ietf:params:oauth:grant-type:device_code",
                "delegation"
            };

        public static List<string> SigningAlgorithms =>
            new List<string>
            {
                "RS256",
                "RS384",
                "RS512",
                "PS256",
                "PS384",
                "PS512",
                "ES256",
                "ES384",
                "ES512"
            };

        public static List<SelectItem> ProtocolTypes =>
            new List<SelectItem> { new SelectItem("oidc", "OpenID Connect") };

    }
}
