using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace youtubeclone.Models
{
    public class SearchFileModel
    {
        public int video_id { get; set; }
        public string Id { get; set; }
        public string video_title { get; set; }
        public string video_desc { get; set; }
        public string publish_datetime { get; set; }
        public int video_views { get; set; }
        public int video_likes { get; set; }
        public int video_dislikes { get; set; }
        public string content_type { get; set; }
        public string video_path { get; set; }
        public int total_comments { get; set; }
        public int category_id { get; set; }
        public int playlist_id { get; set; }
        public int channel_id { get; set; }
    }
}