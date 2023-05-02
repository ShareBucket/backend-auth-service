using Grpc.Core;
using ShareBucket.AuthenticationService.Authorization;
using ShareBucket.DataAccessLayer.Data;
using ShareBucket.AuthenticationService.GrpServices;
using Microsoft.EntityFrameworkCore;

namespace ShareBucket.AuthenticationService.GrpcServices
{
    public class JwtValidatorGrpcService : TokenService.TokenServiceBase
    {
        private IJwtUtils _jwtUtils;

        public JwtValidatorGrpcService(
            IJwtUtils jwtUtils)
        {
            _jwtUtils = jwtUtils;
        }
        public override Task<TokenValidResponse> TokenValidationRequest(TokenParam request, ServerCallContext context)
        {
            var userId = _jwtUtils.ValidateToken(request.Token);
            var response = new TokenValidResponse();
            if (userId != null)
            {
                response.IsValid = true;
                response.UserId = userId.Value;
            }
            else
            {
                response.IsValid = false;
                response.UserId = -1;
            }
            return Task.FromResult(response);
        }

        public override Task<TokenResponse> TokenGenerationRequest(IdUserParam request, ServerCallContext context)
        {
            var Token = _jwtUtils.GenerateToken(request.Id);
            var response = new TokenResponse();
            response.Token = Token;
            return Task.FromResult(response);
        }
    }
}
