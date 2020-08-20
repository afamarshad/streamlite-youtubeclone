using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace youtubeclone.Models
{
    public class CommentModel
    {
        public int CommentId { get; set; }
        public string Id { get; set; }
        public string CommentDesc { get; set; }
        public string datetime { get; set; }
        public string username { get; set; }
        public string videoid { get; set; }
        public int totalVideoComments { get; set; }
    }
}