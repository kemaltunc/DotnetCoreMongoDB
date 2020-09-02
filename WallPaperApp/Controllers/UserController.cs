using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using WallPaperApp.Dto.Post;
using WallPaperApp.Dto.User;
using WallPaperApp.Repository.Account;
using WallPaperApp.Repository.Like;
using WallPaperApp.Repository.Post;

namespace WallPaperApp.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ILikeRepository _likeRepository;

        private readonly IPostRepository _postRepository;

        private readonly IMapper _mapper;

        public UserController(IAccountRepository accountRepository, IMapper mapper, IPostRepository postRepository,
            ILikeRepository likeRepository)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
            _postRepository = postRepository;
            _likeRepository = likeRepository;
        }

        [Authorize]
        [AllowAnonymous]
        [HttpGet("get/{id}")]
        public async Task<ActionResult> Get(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            

            var data = await _accountRepository.GetAsync(x => x.Id == id);

            var response = _mapper.Map<UserResponse>(data);

            return Ok(response);
        }

        [Authorize]
        [AllowAnonymous]
        [HttpGet("get/sharePosts/{id}")]
        public async Task<ActionResult> SharePosts(string id, int page)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var data = await _postRepository.GetAllWithUserId(page, id);

            var response = _mapper.Map<List<PostResponse>>(data);

            return Ok(response);
        }

        [Authorize]
        [AllowAnonymous]
        [HttpGet("get/likedPost/{id}")]
        public async Task<ActionResult> LikedPost(string id, int page)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var data = await _postRepository.LikedPost(page, new ObjectId(id));

            foreach (var p in data)
            {
                var oId = new ObjectId(p.Id);

                var likes = await _likeRepository.GetAll(x => x.postId == oId);
                p.LikeCount = likes.Count;

                var check = likes.Find(x => x.userId == userId);
                p.isLike = check != null;
            }

            return Ok(data);
        }
    }
}