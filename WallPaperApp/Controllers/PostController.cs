using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using WallPaperApp.Dto;
using WallPaperApp.Dto.Account;
using WallPaperApp.Dto.Post;
using WallPaperApp.Dto.User;
using WallPaperApp.Infrastructure;
using WallPaperApp.Entity;
using WallPaperApp.Entity.Account;
using WallPaperApp.Entity.Comment;
using WallPaperApp.Entity.Like;
using WallPaperApp.Entity.Post;
using WallPaperApp.Repository.Account;
using WallPaperApp.Repository.Comment;
using WallPaperApp.Repository.Like;
using WallPaperApp.Repository.Post;
using WallPaperApp.Utility;

namespace WallPaperApp.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class PostController : BaseController
    {
        private readonly IPostRepository _postRepository;

        private readonly IAccountRepository _accountRepository;

        private readonly ILikeRepository _likeRepository;

        private readonly ICommentRepository _commentRepository;

        private readonly IMapper _mapper;

        public PostController(IPostRepository postRepository, IMapper mapper, IAccountRepository accountRepository,
            ILikeRepository likeRepository, ICommentRepository commentRepository)
        {
            _postRepository = postRepository;
            _mapper = mapper;
            _accountRepository = accountRepository;
            _likeRepository = likeRepository;
            _commentRepository = commentRepository;
        }

        [Authorize]
        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<ActionResult> Create([FromBody] PostRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }


            var entity = _mapper.Map<PostEntity>(request);
            entity.senderId = (ObjectId) userId;

            var data = await _postRepository.AddAsync(entity);


            return data == null
                ? StatusCode(HttpConstants.NotExtended, new ErrorModel {message = Messages.DefaultMessage})
                : Ok(new ResultModel {result = true});
        }

        [Authorize]
        [AllowAnonymous]
        [HttpGet("getAll/{page}")]
        public async Task<ActionResult> GetAll(int page)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var data = await _postRepository.GetAll(page);

            var response = _mapper.Map<List<PostResponse>>(data);

            foreach (var p in response)
            {
                var oId = new ObjectId(p.Id);
                var likes = await _likeRepository.GetAll(x => x.postId == oId);
                p.LikeCount = likes.Count;

                p.CommentCount = (int) await _commentRepository.GetCount(x => x.postId == p.Id);
                
                var check = likes.Find(x => x.userId == userId);
                p.isLike = check != null;
            }

            return Ok(response);
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

            var data = await _postRepository.Get(id);

            var response = _mapper.Map<PostResponse>(data);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("like")]
        public async Task<ActionResult> Like([FromBody] LikeRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var oPostId = new ObjectId(request.postId);

            var check = await _likeRepository.GetAsync(x =>
                x.userId == userId && x.postId == oPostId);


            if (check != null)
            {
                return Ok(new ResultModel {result = true});
            }

            var likeModel = new LikeEntity
            {
                postId = oPostId,
                userId = userId
            };

            await _likeRepository.AddAsync(likeModel);

            return Ok(new ResultModel {result = true});
        }

        [AllowAnonymous]
        [HttpPost("unLike")]
        public async Task<ActionResult> UnLike([FromBody] LikeRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var oPostId = new ObjectId(request.postId);

            var data = await _likeRepository.GetAsync(x =>
                x.userId == userId && x.postId == oPostId);


            if (data == null)
            {
                return Ok(new ResultModel {result = true});
            }

            await _likeRepository.DeleteAsync(data);

            return Ok(new ResultModel {result = true});
        }

        [AllowAnonymous]
        [HttpGet("like/userList/{postId}")]
        public async Task<ActionResult> LikeUserList(string postId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var oPostId = new ObjectId(postId);

            var users = await _likeRepository.GetAll(x => x.postId == oPostId);


            var response = _mapper.Map<List<UserResponse>>(users);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("comments/get/{postId}")]
        public async Task<ActionResult> GetComment(string postId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var comments = await _commentRepository.Get(postId);


            return Ok(comments);
        }

        [AllowAnonymous]
        [HttpPost("comments/add")]
        public async Task<ActionResult> AddComment([FromBody] CommentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }


            var commentEntity = _mapper.Map<CommentEntity>(request);
            commentEntity.senderId = userId;

            await _commentRepository.AddAsync(commentEntity);


            return Ok(new ResultModel {result = true});
        }
    }
}