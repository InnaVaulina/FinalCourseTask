using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using TTClassLibrary.Support;




namespace TTB.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private TokenProcessing _tokenProcessing;
        private AuthenticationState auth;

        public CustomAuthStateProvider(TokenProcessing tokenProcessing)
        {
            _tokenProcessing = tokenProcessing;
            _tokenProcessing.TokenChanged += AuthenticationStateChanging;
            auth = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(auth);
        }

        private void AuthenticationStateChanging(string? token)
        {
            if(token != null)
            {
                var identity = new ClaimsIdentity(JwtHelpers.ParseClaimsFromJwt(token), "jwt");
                auth = new AuthenticationState(new ClaimsPrincipal(identity));
            }
            else
            {
                auth = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
            NotifyAuthenticationStateChanged(Task.FromResult(auth));
        }

    }

    
}
