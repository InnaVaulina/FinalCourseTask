using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace TTClassLibrary.Support
{
    public static class JwtHelpers
    {
        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            if (string.IsNullOrWhiteSpace(jwt))
                return Array.Empty<Claim>();

            var parts = jwt.Split('.');
            if (parts.Length < 2) return Array.Empty<Claim>();

            string payload = parts[1];
            payload = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=')
                             .Replace('-', '+').Replace('_', '/');

            var bytes = Convert.FromBase64String(payload);
            var json = Encoding.UTF8.GetString(bytes);
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var claims = new List<Claim>();

            // name
            if (root.TryGetProperty("unique_name", out var nameProp) ||
                root.TryGetProperty("name", out nameProp) ||
                root.TryGetProperty("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", out nameProp))
            {
                if (nameProp.ValueKind == JsonValueKind.String)
                    claims.Add(new Claim(ClaimTypes.Name, nameProp.GetString() ?? string.Empty));
            }

            // name identifier
            if (root.TryGetProperty("nameid", out var idProp) ||
                root.TryGetProperty("sub", out idProp) ||
                root.TryGetProperty("id", out idProp) ||
                root.TryGetProperty("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", out idProp))
            {
                if (idProp.ValueKind == JsonValueKind.String)
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, idProp.GetString() ?? string.Empty));
            }

            // roles
            if (root.TryGetProperty("role", out var roleProp) ||
                root.TryGetProperty("roles", out roleProp) ||
                root.TryGetProperty("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", out roleProp))
            {
                if (roleProp.ValueKind == JsonValueKind.Array)
                {
                    foreach (var r in roleProp.EnumerateArray())
                        if (r.ValueKind == JsonValueKind.String)
                            claims.Add(new Claim(ClaimTypes.Role, r.GetString() ?? string.Empty));
                }
                else if (roleProp.ValueKind == JsonValueKind.String)
                {
                    claims.Add(new Claim(ClaimTypes.Role, roleProp.GetString() ?? string.Empty));
                }
            }

            // дополнительные утверждения
            foreach (var prop in root.EnumerateObject())
            {
                if (prop.NameEquals("unique_name") || prop.NameEquals("name") ||
                    prop.NameEquals("nameid") || prop.NameEquals("sub") ||
                    prop.NameEquals("id") || prop.NameEquals("role") || prop.NameEquals("roles"))
                    continue;

                if (prop.Value.ValueKind == JsonValueKind.String)
                    claims.Add(new Claim(prop.Name, prop.Value.GetString() ?? string.Empty));
                else
                    claims.Add(new Claim(prop.Name, prop.Value.GetRawText()));
            }

            return claims;
        }
    }
}

