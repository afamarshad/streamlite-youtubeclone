using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace youtubeclone.Models
{
    public class PlaylistModel
    {
        public int playlist_id { get; set; }
        public string id { get; set; }
        public string playlist_name { get; set; }
        public string playlist_desc { get; set; }
        public int Channel_id { get; set; }
    }
}