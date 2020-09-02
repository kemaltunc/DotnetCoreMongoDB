using System.Net;

namespace WallPaperApp.Utility
{
    public static class HttpConstants
    {
        public static readonly int NotExtended = (int) HttpStatusCode.NotExtended;
    }

    public static class Messages
    {
        public static readonly string DefaultMessage = "Bir sorun meydana geldi";
    }

    public static class StaticFiles
    {
        public static readonly string path = "/wallpapers";
    }
}