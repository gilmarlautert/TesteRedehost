using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using ProjetoRedehost.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ProjetoRedehost.Auth
{ 
 public class JwtFactory : IJwtFactory
 {
        private readonly JwtIssuerOptions _jwtOptions;

        public JwtFactory(IOptions<JwtIssuerOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtOptions);
        }

        public async Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity)
        {
            var claims = new[]
         {
                 new Claim(JwtRegisteredClaimNames.Sub, userName),
                 new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                 new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                 identity.FindFirst(Helpers.Constants.Strings.JwtClaimIdentifiers.Rol),
                 identity.FindFirst(Helpers.Constants.Strings.JwtClaimIdentifiers.Id)
             };

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        public ClaimsIdentity GenerateClaimsIdentity(string userName,string id)
        {
            return new ClaimsIdentity(new GenericIdentity(userName, "Token"), new[]
            {
                new Claim(Helpers.Constants.Strings.JwtClaimIdentifiers.Id, id),
                new Claim(Helpers.Constants.Strings.JwtClaimIdentifiers.Rol, Helpers.Constants.Strings.JwtClaims.ApiAccess)
            });
        }

        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() -
                               new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                              .TotalSeconds);

        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
            }
        }
    }
     public class JwtIssuerOptions
  {
    /// <summary>
    /// "iss" (Issuer) Claim
    /// </summary>
    /// <remarks>The "iss" (issuer) claim identifies the principal that issued the
    ///   JWT.  The processing of this claim is generally application specific.
    ///   The "iss" value is a case-sensitive string containing a StringOrURI
    ///   value.  Use of this claim is OPTIONAL.</remarks>
    public string Issuer { get; set; }

    /// <summary>
    /// "sub" (Subject) Claim
    /// </summary>
    /// <remarks> The "sub" (subject) claim identifies the principal that is the
    ///   subject of the JWT.  The claims in a JWT are normally statements
    ///   about the subject.  The subject value MUST either be scoped to be
    ///   locally unique in the context of the issuer or be globally unique.
    ///   The processing of this claim is generally application specific.  The
    ///   "sub" value is a case-sensitive string containing a StringOrURI
    ///   value.  Use of this claim is OPTIONAL.</remarks>
    public string Subject { get; set; }

    /// <summary>
    /// "aud" (Audience) Claim
    /// </summary>
    /// <remarks>The "aud" (audience) claim identifies the recipients that the JWT is
    ///   intended for.  Each principal intended to process the JWT MUST
    ///   identify itself with a value in the audience claim.  If the principal
    ///   processing the claim does not identify itself with a value in the
    ///   "aud" claim when this claim is present, then the JWT MUST be
    ///   rejected.  In the general case, the "aud" value is an array of case-
    ///   sensitive strings, each containing a StringOrURI value.  In the
    ///   special case when the JWT has one audience, the "aud" value MAY be a
    ///   single case-sensitive string containing a StringOrURI value.  The
    ///   interpretation of audience values is generally application specific.
    ///   Use of this claim is OPTIONAL.</remarks>
    public string Audience { get; set; }

    /// <summary>
    /// "nbf" (Not Before) Claim (default is UTC NOW)
    /// </summary>
    /// <remarks>The "nbf" (not before) claim identifies the time before which the JWT
    ///   MUST NOT be accepted for processing.  The processing of the "nbf"
    ///   claim requires that the current date/time MUST be after or equal to
    ///   the not-before date/time listed in the "nbf" claim.  Implementers MAY
    ///   provide for some small leeway, usually no more than a few minutes, to
    ///   account for clock skew.  Its value MUST be a number containing a
    ///   NumericDate value.  Use of this claim is OPTIONAL.</remarks>
    public DateTime NotBefore => DateTime.UtcNow;

    /// <summary>
    /// "iat" (Issued At) Claim (default is UTC NOW)
    /// </summary>
    /// <remarks>The "iat" (issued at) claim identifies the time at which the JWT was
    ///   issued.  This claim can be used to determine the age of the JWT.  Its
    ///   value MUST be a number containing a NumericDate value.  Use of this
    ///   claim is OPTIONAL.</remarks>
    public DateTime IssuedAt => DateTime.UtcNow;

    /// <summary>
    /// Set the timespan the token will be valid for (default is 5 min/300 seconds)
    /// </summary>
    public TimeSpan ValidFor { get; set; } = TimeSpan.FromMinutes(5);

    /// <summary>
    /// "exp" (Expiration Time) Claim (returns IssuedAt + ValidFor)
    /// </summary>
    /// <remarks>The "exp" (expiration time) claim identifies the expiration time on
    ///   or after which the JWT MUST NOT be accepted for processing.  The
    ///   processing of the "exp" claim requires that the current date/time
    ///   MUST be before the expiration date/time listed in the "exp" claim.
    ///   Implementers MAY provide for some small leeway, usually no more than
    ///   a few minutes, to account for clock skew.  Its value MUST be a number
    ///   containing a NumericDate value.  Use of this claim is OPTIONAL.</remarks>
    public DateTime Expiration => IssuedAt.Add(ValidFor);

    /// <summary>
    /// "jti" (JWT ID) Claim (default ID is a GUID)
    /// </summary>
    /// <remarks>The "jti" (JWT ID) claim provides a unique identifier for the JWT.
    ///   The identifier value MUST be assigned in a manner that ensures that
    ///   there is a negligible probability that the same value will be
    ///   accidentally assigned to a different data object; if the application
    ///   uses multiple issuers, collisions MUST be prevented among values
    ///   produced by different issuers as well.  The "jti" claim can be used
    ///   to prevent the JWT from being replayed.  The "jti" value is a case-
    ///   sensitive string.  Use of this claim is OPTIONAL.</remarks>
    public Func<Task<string>> JtiGenerator =>
      () => Task.FromResult(Guid.NewGuid().ToString());

    /// <summary>
    /// The signing key to use when generating tokens.
    /// </summary>
    public SigningCredentials SigningCredentials { get; set; }
  }
}