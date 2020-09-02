using System.Diagnostics;
using AutoMapper;
using WallPaperApp.Dto.Account;
using WallPaperApp.Dto.Post;
using WallPaperApp.Dto.User;
using WallPaperApp.Entity;
using WallPaperApp.Entity.Account;
using WallPaperApp.Entity.Comment;
using WallPaperApp.Entity.Like;
using WallPaperApp.Entity.Post;

namespace WallPaperApp.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterRequest, AccountEntity>();
            CreateMap<AccountEntity, AccountResponse>();
            CreateMap<AccountEntity, UserResponse>();


            CreateMap<PostRequest, PostEntity>();
            CreateMap<PostEntity, PostResponse>();


            CreateMap<CommentRequest, CommentEntity>();


            CreateMap<PostWithUser, PostResponse>().ForMember(
                dest => dest.ownerUser, opt => opt.MapFrom(src => src.ownerUser[0])
            );
            
        }
    }
}