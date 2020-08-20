using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace youtubeclone.Models
{
    public class ChannelModel
    {
        public int channel_id { get; set; }
        public string channel_name { get; set; }
        public string id { get; set; }
        public string email { get; set; }
        public string channel_profile_pic { get; set; }
        public string channel_cover_pic { get; set; }
        public string channel_desc { get; set; }
        public int Subscribers { get; set; }
        public int total_videos { get; set; }
    }
}