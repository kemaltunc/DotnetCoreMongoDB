using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace WallPaperApp.Controllers
{
    public class BaseController : ControllerBase
    {
        private string FindUser() => User.FindFirst(ClaimTypes.Name).Value;

        // public string userId => FindUser ();
        public ObjectId userId = new ObjectId("5f410efe1a46ef27cbbe668c");
    }
}