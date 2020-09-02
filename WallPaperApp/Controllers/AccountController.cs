using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using JwtAuthDemo.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WallPaperApp.Dto;
using WallPaperApp.Dto.Account;
using WallPaperApp.Entity;
using WallPaperApp.Infrastructure;
using WallPaperApp.Entity.Account;
using WallPaperApp.Repository.Account;
using WallPaperApp.Utility;

namespace WallPaperApp.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AccountController : BaseController
    {
        private readonly IJwtAuthManager _jwtAuthManager;
        private readonly Security _security;

        private readonly IAccountRepository _accountRepository;


        private readonly IMapper _mapper;

        public AccountController(IJwtAuthManager jwtAuthManager, Security security,
            IAccountRepository accountRepository, IMapper mapper)
        {
            _jwtAuthManager = jwtAuthManager;
            _security = security;
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var data = await _accountRepository.GetAsync(x => x.email == request.email);

            if (data != null)
            {
                return StatusCode(HttpConstants.NotExtended,
                    new ErrorModel {message = "Bu mail adresi daha önce alınmış"});
            }

            var lockPassword = _security.Encrypt(request.password);
            request.password = lockPassword;


            var entity = _mapper.Map<AccountEntity>(request);

            var user = await _accountRepository.AddAsync(entity);

            var jwtResult = JwtAuthResult(entity.Id, entity.email);


            var response = _mapper.Map<AccountResponse>(user);
            response.accessToken = jwtResult.AccessToken;
            response.refreshToken = jwtResult.RefreshToken.TokenString;

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var data = await _accountRepository.GetAsync(x => x.email == request.email);


            if (data == null || request.password != _security.Decrypt(data.password))
            {
                return StatusCode(HttpConstants.NotExtended,
                    new ErrorModel {message = "Kullanıcı adı veya şifre hatalı"});
            }

            var jwtResult = JwtAuthResult(data.Id, data.email);

            var response = _mapper.Map<AccountResponse>(data);
            response.accessToken = jwtResult.AccessToken;
            response.refreshToken = jwtResult.RefreshToken.TokenString;

            return Ok(response);
        }

        private JwtAuthResult JwtAuthResult(string userId, string email)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, userId)
            };

            var jwtResult = _jwtAuthManager.GenerateTokens(email, claims, DateTime.Now);

            return jwtResult;
        }
    }
}