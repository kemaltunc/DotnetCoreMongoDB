using System;

namespace WallPaperApp.Entity.Base
{
    public interface IEntity
    {
        public string Id { get; }
        DateTime CreatedAt { get; set; }
    }
    
}