using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Mvc;
using youtubeclone.Models;
using Microsoft.AspNet.Identity;
using System.Data;
using System.Threading.Tasks;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Enums;
using System.Linq;

namespace youtubeclone.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            var files = new List<FileModel>();
            var categories = new List<CategoryModel>();
            GetFiles(files);
            GetCategories(categories);
            TempData["Category"] = CategoryList();
            return View(Tuple.Create(categories, files));
        }

        public JsonResult getVideoTitle()
        {
            List<FileModel> videotitle = new List<FileModel>();
            VideoTitles(videotitle);
            TempData["Category"] = CategoryList();
            return Json(videotitle.OrderBy(x => x.video_title).ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult getChannelTitle()
        {
            TempData["Category"] = CategoryList();
            List<ChannelModel> channelTitle = new List<ChannelModel>();
            ChannelTitles(channelTitle);
            return Json(channelTitle.OrderBy(x => x.channel_name).ToList(), JsonRequestBehavior.AllowGet);
        }

        private List<FileModel> VideoTitles(List<FileModel> file)
        {
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT Video_title FROM video";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                file.Add(new FileModel
                                {
                                    video_title = sdr["Video_title"].ToString()
                                });
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return file;
        }

        private List<ChannelModel> ChannelTitles(List<ChannelModel> channel)
        {
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT Channel_name FROM channel";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                channel.Add(new ChannelModel
                                {
                                    channel_name = sdr["Channel_name"].ToString()
                                });
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return channel;
        }

        private List<SearchFileModel> VideoDetailsForSearch(List<SearchFileModel> file ,string  tags)
        {
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT * FROM video where Video_title LIKE '%"+tags+"%'";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        //cmd.Parameters.AddWithValue("@tags", tags);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                file.Add(new SearchFileModel
                                {
                                    video_id = Convert.ToInt32(sdr["Video_id"]),
                                    video_title = sdr["Video_title"].ToString(),
                                    video_views = Convert.ToInt32(sdr["Video_views"]),
                                    category_id = Convert.ToInt32(sdr["Category_id"]),
                                    content_type = sdr["Content_type"].ToString(),
                                    video_path = sdr["Video_path"].ToString(),
                                    video_desc = sdr["Video_desc"].ToString(),
                                    Id = sdr["Id"].ToString(),
                                    publish_datetime = sdr["Publish_datetime"].ToString(),
                                    video_likes = Convert.ToInt32(sdr["Video_likes"]),
                                    video_dislikes = Convert.ToInt32(sdr["Video_dislikes"]),
                                    total_comments = Convert.ToInt32(sdr["Total_comments"]),
                                    playlist_id = Convert.ToInt32(sdr["Playlist_id"]),
                                    channel_id = Convert.ToInt32(sdr["Channel_id"])
                                });
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return file;
        }

        private static List<CategoryModel> GetCategories(List<CategoryModel> category)
        {
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT * FROM category where Category_id!=110 AND Category_id!=111";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                category.Add(new CategoryModel
                                {
                                    CategoryId = Convert.ToInt32(sdr["Category_id"]),
                                    CategoryName = sdr["Category_name"].ToString()
                                });
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return category;
        }

        private static List<FileModel> GetFiles(List<FileModel> files)
        {
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT * FROM video ORDER BY Video_id DESC";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                files.Add(new FileModel
                                {

                                    video_id = Convert.ToInt32(sdr["Video_id"]),
                                    video_title = sdr["Video_title"].ToString(),
                                    video_views = Convert.ToInt32(sdr["Video_views"]),
                                    category_id = Convert.ToInt32(sdr["Category_id"]),
                                    content_type = sdr["Content_type"].ToString(),
                                    video_path = sdr["Video_path"].ToString(),
                                    video_desc = sdr["Video_desc"].ToString(),
                                    Id = sdr["Id"].ToString(),
                                    publish_datetime = sdr["Publish_datetime"].ToString(),
                                    video_likes = Convert.ToInt32(sdr["Video_likes"]),
                                    video_dislikes = Convert.ToInt32(sdr["Video_dislikes"]),
                                    total_comments = Convert.ToInt32(sdr["Total_comments"]),
                                    playlist_id = Convert.ToInt32(sdr["Playlist_id"]),
                                    channel_id = Convert.ToInt32(sdr["Channel_id"])
                                });
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return files;
        }

        public ActionResult About()
        {
            try
            {
                TempData["Category"] = CategoryList();
                ViewBag.Message = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod" +
                "tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam," +
                "quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat" +
                "nulla pariatur.Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia" +
                "deserunt mollit anim id est laborum.";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return View("Index", "Home");
            }


            return View();
        }

        public ActionResult Contact()
        {
            try
            {
                TempData["Category"] = CategoryList();
                ViewBag.Message = "Your contact page.";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return View("Index", "Home");
            }

            return View();
        }

        public ActionResult TermsAndConditions()
        {
            try
            {
                TempData["Category"] = CategoryList();
                ViewBag.Message = "Terms And Conditions";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return View("Index", "Home");
            }


            return View();
        }

        [HttpGet]
        public ActionResult Category(int CategoryID)
        {
            try
            {
                TempData["Category"] = CategoryList();
                var files = new List<FileModel>();
                GetFilesWithCategory(files, CategoryID);
                var channelofvideoscategory = new List<ChannelModel>();
                GetChannelofVideo(channelofvideoscategory);
                return View(Tuple.Create(files, channelofvideoscategory));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return View("Index", "Home");
            }

        }

        [Authorize]
        public static List<ChannelModel> GetChannelofVideo(List<ChannelModel> channel)
        {
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT * FROM channel";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                channel.Add(new ChannelModel
                                {
                                    channel_id = Convert.ToInt32(sdr["Channel_id"]),
                                    channel_name = sdr["Channel_name"].ToString(),
                                    channel_desc = sdr["Channel_desc"].ToString(),
                                    channel_profile_pic = sdr["Channel_profilepic"].ToString(),
                                    channel_cover_pic = sdr["Channel_coverpic"].ToString(),
                                    email = sdr["Email"].ToString(),
                                    id = sdr["Id"].ToString(),
                                    Subscribers = Convert.ToInt32(sdr["Subscribers"]),
                                    total_videos = Convert.ToInt32(sdr["Total_videos"]),
                                });
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return channel;
        }

        public static string getuseridofvideothroughcategory(int categoryid)
        {
            string userid = "";
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT Id FROM video where Category_id=@Category_id";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@Category_id", categoryid);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                userid = sdr["Id"].ToString();

                            }
                        }
                        sqlConnection.Close();
                    }
                }

            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }

            return userid;
        }

        private static List<FileModel> GetFilesWithCategory(List<FileModel> files, int categoryid)
        {
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT * FROM video where Category_id=@Category_id";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@Category_id", categoryid);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                files.Add(new FileModel
                                {

                                    video_id = Convert.ToInt32(sdr["Video_id"]),
                                    video_title = sdr["Video_title"].ToString(),
                                    video_views = Convert.ToInt32(sdr["Video_views"]),
                                    category_id = Convert.ToInt32(sdr["Category_id"]),
                                    content_type = sdr["Content_type"].ToString(),
                                    video_path = sdr["Video_path"].ToString(),
                                    video_desc = sdr["Video_desc"].ToString(),
                                    Id = sdr["Id"].ToString(),
                                    publish_datetime = sdr["Publish_datetime"].ToString(),
                                    video_likes = Convert.ToInt32(sdr["Video_likes"]),
                                    video_dislikes = Convert.ToInt32(sdr["Video_dislikes"]),
                                    total_comments = Convert.ToInt32(sdr["Total_comments"]),
                                    playlist_id = Convert.ToInt32(sdr["Playlist_id"]),
                                    channel_id = Convert.ToInt32(sdr["Channel_id"])
                                });
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return files;
        }

        [HttpGet]
        public ActionResult Search(string tags)
        {
            TempData["Category"] = CategoryList();
            var userID = User.Identity.GetUserId();
            
            if (tags != null)
            {
                
                List<SearchFileModel> videotitle = new List<SearchFileModel>();
                List<ChannelModel> channel = new List<ChannelModel>();
                List<ChannelModel> channelaftersearch = new List<ChannelModel>();
                List<ChannelModel> subscribedchannel = new List<ChannelModel>();
                try
                {
                    VideoDetailsForSearch(videotitle, tags);
                    GetChannelForSearchPage(channel);
                    GetChannelDeatilsForSearchPage(channelaftersearch, tags, userID);
                    if (userID != null)
                    {
                        GetSubscribedChannelDetails(userID, subscribedchannel);
                    }
                    return View(Tuple.Create(videotitle, channel, channelaftersearch, subscribedchannel));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        //For Your Channel View
        private static int Playlistofuser(string userID)
        {
            int playlistid = 0;
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                using (SqlConnection con = new SqlConnection(mainConnection))
                {
                    string query = "SELECT * FROM playlist where Id=@userID";
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@userID", userID);
                        cmd.Connection = con;
                        con.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                playlistid = Convert.ToInt32(sdr["Playlist_id"]);
                            }
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }

            return playlistid;
        }

        //For Your Channel View
        private static List<PlaylistModel> Playlistofusercomplete(List<PlaylistModel> playlist, string userID)
        {
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                using (SqlConnection con = new SqlConnection(mainConnection))
                {
                    string query = "SELECT * FROM playlist where Id=@userID";
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@userID", userID);
                        cmd.Connection = con;
                        con.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                playlist.Add(new PlaylistModel
                                {
                                    playlist_id = Convert.ToInt32(sdr["Playlist_id"]),
                                    playlist_name = sdr["Playlist_name"].ToString(),
                                    playlist_desc = sdr["Playlist_desc"].ToString(),
                                    id = sdr["Id"].ToString()
                                });
                            }
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }

            return playlist;
        }

        //For Your channel page
        private static int channelofuser(string userID)
        {
            int channel_id = 0;
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                using (SqlConnection con = new SqlConnection(mainConnection))
                {
                    string query = "SELECT Channel_id FROM channel where Id=@userID";
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@userID", userID);
                        cmd.Connection = con;
                        con.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {

                                channel_id = Convert.ToInt32(sdr["Channel_id"]);
                            }
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }

            return channel_id;
        }

        [Authorize]
        public static List<ChannelModel> GetSubscribedChannelDetails(string userid, List<ChannelModel> subchannel)
        {
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT * FROM channel where Channel_id IN (Select Channel_id from channelsubscriptions where Id=@userid)";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@userid", userid);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                subchannel.Add(new ChannelModel
                                {
                                    channel_id = Convert.ToInt32(sdr["Channel_id"]),
                                    channel_name = sdr["Channel_name"].ToString(),
                                    channel_desc = sdr["Channel_desc"].ToString(),
                                    channel_profile_pic = sdr["Channel_profilepic"].ToString(),
                                    channel_cover_pic = sdr["Channel_coverpic"].ToString(),
                                    email = sdr["Email"].ToString(),
                                    id = sdr["Id"].ToString(),
                                    Subscribers = Convert.ToInt32(sdr["Subscribers"]),
                                    total_videos = Convert.ToInt32(sdr["Total_videos"]),
                                });
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return subchannel;
        }

        [Authorize]
        public ActionResult YourChannel()
        {
            try
            {
                var userID = User.Identity.GetUserId();
                TempData["subscribersofcurrentchannel"] = GetChannelSubscribers(userID);
                TempData["currentchannelviews"] = GetChannelViews(userID);
                TempData["Category"] = CategoryList();
                TempData["Playlist"] = PlaylistList(userID);
                var files = new List<FileModel>();
                var playlist = new List<PlaylistModel>();
                var channel = new List<ChannelModel>();
                var channelsub = new List<ChannelModel>();
                GetSubscribedChannelDetails(userID, channelsub);

                GetChannel(userID, channel);
                GetFilesPlaylistAndUser(files, userID);
                Playlistofusercomplete(playlist, userID);
                int count = channelofuser(userID);
                if (count > 0)
                {
                    ViewBag.channelcount = count;
                }
                else
                {
                    ViewBag.channelcount = 0;
                }
                return View(Tuple.Create(files, playlist, channel, channelsub));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return View("Index", "Home");
            }
        }

        //For Your channel page
        [Authorize]
        public static List<ChannelModel> GetChannel(String userID, List<ChannelModel> channel)
        {
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT * FROM channel where Id=@userID";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("userID", userID);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                channel.Add(new ChannelModel
                                {
                                    channel_id = Convert.ToInt32(sdr["Channel_id"]),
                                    channel_name = sdr["Channel_name"].ToString(),
                                    channel_desc = sdr["Channel_desc"].ToString(),
                                    channel_profile_pic = sdr["Channel_profilepic"].ToString(),
                                    channel_cover_pic = sdr["Channel_coverpic"].ToString(),
                                    email = sdr["Email"].ToString(),
                                    id = sdr["Id"].ToString(),
                                    Subscribers = Convert.ToInt32(sdr["Subscribers"]),
                                    total_videos = Convert.ToInt32(sdr["Total_videos"]),
                                });
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return channel;
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddChannel(string Channelname, HttpPostedFileBase profilepic, HttpPostedFileBase coverpic, string Channeldesc)
        {
            try
            {
                var userID = User.Identity.GetUserId();
                var useremail = User.Identity.GetUserName();
                string profile = Path.GetFileName(profilepic.FileName);
                string cover = Path.GetFileName(coverpic.FileName);

                profilepic.SaveAs(Server.MapPath("/Content/profilepics/" + profile));
                coverpic.SaveAs(Server.MapPath("/Content/coverphotos/" + cover));
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(mainConnection);
                SqlCommand sqlcomm = new SqlCommand("insertchannel", sqlcon);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcon.Open();
                sqlcomm.Parameters.AddWithValue("@Channel_name", Channelname);
                sqlcomm.Parameters.AddWithValue("@Id", userID);
                sqlcomm.Parameters.AddWithValue("@Email", useremail);
                sqlcomm.Parameters.AddWithValue("@Channel_profilepic", "/Content/profilepics/" + profile);
                sqlcomm.Parameters.AddWithValue("@Channel_coverpic", "/Content/coverphotos/" + cover);
                sqlcomm.Parameters.AddWithValue("@Channel_desc", Channeldesc);
                sqlcomm.Parameters.AddWithValue("@Subscribers", 0);
                sqlcomm.Parameters.AddWithValue("@Total_videos", 0);
                sqlcomm.Parameters.AddWithValue("@Channel_views", 0);
                sqlcomm.ExecuteNonQuery();
                sqlcon.Close();
                TempData["ChannelSuccess"] = "Channel Successfully Created!";
            }
            catch (Exception ex)
            {
                TempData["ChannelError"] = "Error Creating Channel!";
                Console.WriteLine(ex.Message);
            }

            return RedirectToAction("YourChannel", "Home");
        }

        //For Your Channel View
        private static List<FileModel> GetFilesPlaylistAndUser(List<FileModel> files, string userID)
        {
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT * FROM video where Id=@userid";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@userid", userID);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                files.Add(new FileModel
                                {

                                    video_id = Convert.ToInt32(sdr["Video_id"]),
                                    video_title = sdr["Video_title"].ToString(),
                                    video_views = Convert.ToInt32(sdr["Video_views"]),
                                    category_id = Convert.ToInt32(sdr["Category_id"]),
                                    content_type = sdr["Content_type"].ToString(),
                                    video_path = sdr["Video_path"].ToString(),
                                    video_desc = sdr["Video_desc"].ToString(),
                                    Id = sdr["Id"].ToString(),
                                    publish_datetime = sdr["Publish_datetime"].ToString(),
                                    video_likes = Convert.ToInt32(sdr["Video_likes"]),
                                    video_dislikes = Convert.ToInt32(sdr["Video_dislikes"]),
                                    total_comments = Convert.ToInt32(sdr["Total_comments"]),
                                    playlist_id = Convert.ToInt32(sdr["Playlist_id"]),
                                    channel_id = Convert.ToInt32(sdr["Channel_id"])
                                });
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return files;
        }

        public static List<FileModel> GetVideos(String userID, List<FileModel> files)
        {
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT * FROM video where Channel_id IN (Select Channel_id from channel where id=@userID)";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@userID", userID);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                files.Add(new FileModel
                                {
                                    video_id = Convert.ToInt32(sdr["Video_id"]),
                                    video_title = sdr["Video_title"].ToString(),
                                    video_views = Convert.ToInt32(sdr["Video_views"]),
                                    category_id = Convert.ToInt32(sdr["Category_id"]),
                                    content_type = sdr["Content_type"].ToString(),
                                    video_path = sdr["Video_path"].ToString(),
                                    video_desc = sdr["Video_desc"].ToString(),
                                    Id = sdr["Id"].ToString(),
                                    publish_datetime = sdr["Publish_datetime"].ToString(),
                                    video_likes = Convert.ToInt32(sdr["Video_likes"]),
                                    video_dislikes = Convert.ToInt32(sdr["Video_dislikes"]),
                                    total_comments = Convert.ToInt32(sdr["Total_comments"]),
                                    playlist_id = Convert.ToInt32(sdr["Playlist_id"]),
                                    channel_id = Convert.ToInt32(sdr["Channel_id"])
                                });
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return files;
        }

        //For Your Channel View
        private static int channelofregistereduserexists(string userID)
        {
            int channelid = 0;
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                using (SqlConnection con = new SqlConnection(mainConnection))
                {
                    string query = "SELECT * FROM channel where Id=@userID";
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@userID", userID);
                        cmd.Connection = con;
                        con.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                channelid = Convert.ToInt32(sdr["Channel_id"]);
                            }
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return channelid;
        }

        [Authorize]
        public ActionResult UploadVideo()
        {
            try
            {
                TempData["Category"] = CategoryList();
                var userID = User.Identity.GetUserId();
                TempData["Playlist"] = PlaylistList(userID);
                int count = channelofregistereduserexists(userID);
                if (count > 0)
                {
                    ViewBag.channelcount = count;
                }
                else
                {
                    ViewBag.channelcount = 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return View("Index", "Home");
            }
            return View();
        }

        private static List<SelectListItem> CategoryList()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                using (SqlConnection con = new SqlConnection(mainConnection))
                {
                    string query = " SELECT Category_id, Category_name FROM category where Category_id!=110 AND Category_id!=111";
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        con.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                items.Add(new SelectListItem
                                {
                                    Text = sdr["Category_name"].ToString(),
                                    Value = sdr["Category_id"].ToString()
                                });
                            }
                        }
                        con.Close();
                    }
                }
            }

            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return items;
        }

        private static List<SelectListItem> PlaylistList(string userID)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                using (SqlConnection con = new SqlConnection(mainConnection))
                {
                    string query = "SELECT * FROM playlist where Id=@userID";
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@userID", userID);
                        cmd.Connection = con;
                        con.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                items.Add(new SelectListItem
                                {
                                    Text = sdr["Playlist_name"].ToString(),
                                    Value = sdr["Playlist_id"].ToString()
                                });
                            }
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }

            return items;
        }

        const int BYTES_TO_READ = sizeof(Int64);

        static bool FilesAreEqual(FileInfo first, FileInfo second)
        {

            if (first.Length != second.Length)
                return false;

            if (string.Equals(first.FullName, second.FullName, StringComparison.OrdinalIgnoreCase))
                return true;

            int iterations = (int)Math.Ceiling((double)first.Length / BYTES_TO_READ);

            using (FileStream fs1 = first.OpenRead())
            using (FileStream fs2 = second.OpenRead())
            {
                byte[] one = new byte[BYTES_TO_READ];
                byte[] two = new byte[BYTES_TO_READ];

                for (int i = 0; i < iterations; i++)
                {
                    fs1.Read(one, 0, BYTES_TO_READ);
                    fs2.Read(two, 0, BYTES_TO_READ);

                    if (BitConverter.ToInt64(one, 0) != BitConverter.ToInt64(two, 0))
                        return false;
                }
            }

            return true;
        }

        [Authorize]
        public bool UploadedVideoMatchExistingDBVideo(FileInfo video1)
        {
            bool count = false;
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT Video_path FROM video";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                FileInfo video2 = new FileInfo(Server.MapPath("~"+sdr["Video_path"].ToString()));
                                if(FilesAreEqual(video1, video2) == true)
                                {
                                    count = true;
                                    break;
                                }
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }

            return count;
        }

        [Authorize]
        public bool videoNameAlreadyExists(string videoname)
        {
            bool count = false;
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT Video_title FROM video where Video_title=@Videotitle";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@Videotitle",videoname);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                count = true;
                                break;
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return count;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> UploadVideo2(HttpPostedFileBase videofile, int CategoryID, string Description, int PlaylistID=0)
        {
            try
            {
                var userID = User.Identity.GetUserId();

                int videoviews = 0;
                int videolikes = 0;
                int videodislikes = 0;
                int totalcomments = 0;
                int categoryid = CategoryID;
                int playlistid = PlaylistID;
                int channelid = channelofuser(userID);
                bool success = false;
                string error = "";
                if (videofile != null && videofile.ContentLength>0)
                {
                    string filename = Path.GetFileName(videofile.FileName);
                    string videoname = Path.GetFileNameWithoutExtension(videofile.FileName);
                    string fileext = Path.GetExtension(filename);
                    
                    if (fileext==".mp4" || fileext == ".avi" || fileext == ".mov" || fileext == ".flv" || fileext == ".mpeg"|| fileext==".wmv")
                    {
                        //To run on Microsoft edge
                        videofile.SaveAs(Server.MapPath("/Content/video/" + filename));
                        //To run on Google Chrome
                        //using (var fileStream =
                        //        new FileStream(Path.Combine(ConfigurationManager.AppSettings["folderpath"].ToString() + "video\\", videofile.FileName), FileMode.Create, FileAccess.Write))
                        //{
                        //    await videofile.InputStream.CopyToAsync(fileStream);
                        //}
                        //var convert = new NReco.VideoConverter.FFMpegConverter();
                        //convert.ConvertMedia(ConfigurationManager.AppSettings["folderpath"].ToString() + @"video\" + filename, ConfigurationManager.AppSettings["folderpath"].ToString() + "video\\" + videoname + ".mp4", NReco.VideoConverter.Format.mp4);

                        FileInfo file1 = new FileInfo(ConfigurationManager.AppSettings["folderpath"].ToString() + @"converted\" + videoname + ".mp4");

                        if (videoNameAlreadyExists(videoname) == false)
                        {
                            if (await ConvertVideo(videoname,fileext) == true)
                            {
                                if (UploadedVideoMatchExistingDBVideo(file1) == false)
                                {
                                    string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                                    SqlConnection sqlcon = new SqlConnection(mainConnection);
                                    SqlCommand sqlcomm = new SqlCommand("insertvideo", sqlcon);
                                    sqlcomm.CommandType = CommandType.StoredProcedure;
                                    sqlcon.Open();
                                    sqlcomm.Parameters.AddWithValue("@Video_title", videoname);
                                    sqlcomm.Parameters.AddWithValue("@Id", userID);
                                    sqlcomm.Parameters.AddWithValue("@Video_desc", Description);
                                    sqlcomm.Parameters.AddWithValue("@Publish_datetime", DateTime.Today.ToString());
                                    sqlcomm.Parameters.AddWithValue("@Video_views", videoviews);
                                    sqlcomm.Parameters.AddWithValue("@Video_likes", videolikes);
                                    sqlcomm.Parameters.AddWithValue("@Video_dislikes", videodislikes);
                                    sqlcomm.Parameters.AddWithValue("@Video_path", "/Content/converted/" + videoname + ".mp4");
                                    sqlcomm.Parameters.AddWithValue("@Content_type", ".mp4");
                                    sqlcomm.Parameters.AddWithValue("@Total_comments", totalcomments);
                                    sqlcomm.Parameters.AddWithValue("@Category_id", categoryid);
                                    sqlcomm.Parameters.AddWithValue("@Playlist_id", playlistid);
                                    sqlcomm.Parameters.AddWithValue("@Channel_id", channelid);

                                    sqlcomm.ExecuteNonQuery();
                                    sqlcon.Close();

                                    success = true;
                                }
                                else
                                {
                                    error = "File Already Exists!";
                                    System.IO.File.Delete(ConfigurationManager.AppSettings["folderpath"].ToString() + "video\\" + filename);
                                    System.IO.File.Delete(ConfigurationManager.AppSettings["folderpath"].ToString() + "converted\\" + videoname + ".mp4");

                                }
                            }
                            else
                            {
                                error = "Error Converting Video!";
                                System.IO.File.Delete(ConfigurationManager.AppSettings["folderpath"].ToString() + "video\\" + filename);

                            }
                        }
                        else
                        {
                            error = "Video Name Already Exists!";
                            System.IO.File.Delete(ConfigurationManager.AppSettings["folderpath"].ToString() + "video\\" + filename);

                        }

                        
                    }
                    else
                    {
                       error = "Invalid file format, only .avi, .mov, .flv, .mpeg, .wmv are allowed!";
                    }

                    System.IO.File.Delete(ConfigurationManager.AppSettings["folderpath"].ToString() + "video\\" + filename);
                }

                
                if (success == true)
                {
                    TempData["Success"] = "Successfully Uploaded!";
                }
                else if (error != null)
                {

                    TempData["Error"] = error.ToString();
                }


            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error Uploading!";
                Console.WriteLine(ex.Message);
            }

            return RedirectToAction("UploadVideo", "Home");
        }

        public bool playlistexists(string playlistname)
        {
            bool count = false;
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT Playlist_name FROM playlist where Playlist_name=@Playlist_name";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@Playlist_name", playlistname);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                count = true;
                                break;
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return count;
        }

        //For Your Channel
        [Authorize]
        [HttpPost]
        public ActionResult AddPlaylist(string Playlistname, string Playlistdesc, int channelid)
        {
            try
            {
                if (playlistexists(Playlistname) == false)
                {
                    var userID = User.Identity.GetUserId();
                    string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                    SqlConnection sqlcon = new SqlConnection(mainConnection);
                    SqlCommand sqlcomm = new SqlCommand("insertplaylist", sqlcon);
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcon.Open();
                    sqlcomm.Parameters.AddWithValue("@Id", userID);
                    sqlcomm.Parameters.AddWithValue("@Playlist_name", Playlistname);
                    sqlcomm.Parameters.AddWithValue("@Playlist_desc", Playlistdesc);
                    sqlcomm.Parameters.AddWithValue("@Channel_id", channelid);
                    sqlcomm.ExecuteNonQuery();
                    sqlcon.Close();
                    TempData["playlistcreated"] = "success";
                }
                else
                {
                    TempData["playlistcreated"] = "error";
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["playlistcreated"] = "error";
            }

            return RedirectToAction("YourChannel", "Home");
        }

        [Authorize]
        public bool WatchHistoryExists(string userID, int VideoID)
        {
            bool count = false;
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT * FROM watchlist where Video_id=@Video_id and Id=@userID";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@Video_id", VideoID);
                        cmd.Parameters.AddWithValue("@userID", userID);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                count = true;
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }

            return count;
        }

        public int GetChannelofVideoFromVideoid(int videoid)
        {
            int channel_id = 0;
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT Channel_id FROM video where Video_id=@videoid";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@videoid", videoid);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                channel_id = Convert.ToInt32(sdr["Channel_id"]);
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return channel_id;
        }

        public int CheckVideoCategoryInapp(int VideoId)
        {
            int catid = 0;
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT Category_id FROM video where Video_id=@Video_id";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@Video_id", VideoId);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                catid = Convert.ToInt32(sdr["Category_id"]);
                                
                            }
                        }
                        sqlConnection.Close();
                    }
                }
                
            }
            catch (Exception e) {
               Console.WriteLine( e.Message);
            }
            
            return catid;
        }

        [HttpGet]
        public ActionResult Video(int VideoID)
        {
            try
            {
                TempData["ReportCategories"] = GetReportCategories();
                if (CheckVideoCategoryInapp(VideoID) != 110)
                {
                    var userID = User.Identity.GetUserId();
                    if (userID != null)
                    {
                        TempData["user"] = userID;

                        if (WatchHistoryExists(userID, VideoID) != true)
                        {
                            UpdateViews(VideoID);
                            AddWatchHistory(VideoID, userID);
                        }
                        if (userChannelSubscriptionExists(userID, GetChannelofVideoFromVideoid(VideoID)) == true)
                        {
                            TempData["subscription"] = "UnSubscribe";
                        }

                    }

                    TempData["Category"] = CategoryList();
                    List<FileModel> files = new List<FileModel>();
                    getSingleVideo(files, VideoID);
                    var channel = new List<ChannelModel>();
                    GetChannel(getuseridofvideo(VideoID), channel);
                    List<CommentModel> comments = new List<CommentModel>();
                    GetComments(VideoID, comments);
                    List<FileModel> selectedCategoryVideo = new List<FileModel>();
                    GetSelectedCategoryVideo(VideoID, selectedCategoryVideo);

                    return View(Tuple.Create(files, channel, comments, selectedCategoryVideo));
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return View("Index", "Home");
            }
        }

        [Authorize]
        public ActionResult AddWatchHistory(int VideoID, string userID)
        {
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(mainConnection);
                SqlCommand sqlcomm = new SqlCommand("addwatchhistory", sqlcon);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcon.Open();
                sqlcomm.Parameters.AddWithValue("@Id", userID);
                sqlcomm.Parameters.AddWithValue("@Video_id", VideoID);
                sqlcomm.ExecuteNonQuery();
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //TempData["commenterror"] = "Error Adding Comment!";
            }

            return RedirectToAction("Video", "Home", new { @VideoID = VideoID });
        }

        public int getViewsofVideo(int VideoID)
        {
            int views = 0;
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "Select Video_views from video where Video_id=@Video_id";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@Video_id", VideoID);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                views = Convert.ToInt32(sdr["Video_views"]);
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return views;
        }

        public void UpdateViews(int VideoID)
        {
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(mainConnection);
                SqlCommand sqlcomm = new SqlCommand("updateViews", sqlcon);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcon.Open();
                sqlcomm.Parameters.AddWithValue("@Video_views", getViewsofVideo(VideoID) + 1);
                sqlcomm.Parameters.AddWithValue("@Video_id", VideoID);
                sqlcomm.ExecuteNonQuery();
                sqlcon.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public static List<FileModel> GetSelectedCategoryVideo(int VideoId, List<FileModel> file)
        {
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT * FROM video where Category_id IN (Select Category_id from video where Video_id=@Video_id)";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@Video_id", VideoId);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                file.Add(new FileModel
                                {
                                    video_id = Convert.ToInt32(sdr["Video_id"]),
                                    video_title = sdr["Video_title"].ToString(),
                                    video_views = Convert.ToInt32(sdr["Video_views"]),
                                    category_id = Convert.ToInt32(sdr["Category_id"]),
                                    content_type = sdr["Content_type"].ToString(),
                                    video_path = sdr["Video_path"].ToString(),
                                    video_desc = sdr["Video_desc"].ToString(),
                                    Id = sdr["Id"].ToString(),
                                    publish_datetime = sdr["Publish_datetime"].ToString(),
                                    video_likes = Convert.ToInt32(sdr["Video_likes"]),
                                    video_dislikes = Convert.ToInt32(sdr["Video_dislikes"]),
                                    total_comments = Convert.ToInt32(sdr["Total_comments"]),
                                    playlist_id = Convert.ToInt32(sdr["Playlist_id"]),
                                    channel_id = Convert.ToInt32(sdr["Channel_id"])
                                });
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return file;
        }

        public static List<CommentModel> GetComments(int VideoId, List<CommentModel> comments)
        {
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT * FROM comments where Video_id=@Video_id";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@Video_id", VideoId);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                comments.Add(new CommentModel
                                {
                                    CommentId = Convert.ToInt32(sdr["Comment_id"]),
                                    CommentDesc = sdr["Comment_desc"].ToString(),
                                    Id = sdr["Id"].ToString(),
                                    datetime = sdr["Comment_datetime"].ToString(),
                                    username = sdr["Username"].ToString(),
                                    totalVideoComments = getTotalVideoComments(VideoId)
                                });
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return comments;
        }

        public static int getTotalVideoComments(int videoid)
        {
            int total = 0;
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT * FROM comments where Video_id=@Video_id";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@Video_id", videoid);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                total = total + 1;

                            }
                        }
                        sqlConnection.Close();
                    }
                }

            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return total;
        }

        public static string getuseridofvideo(int videoid)
        {
            string userid = "";
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT Id FROM video where Video_id=@Video_id";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@Video_id", videoid);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                userid = sdr["Id"].ToString();

                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return userid;
        }

        //For Single Video
        public List<FileModel> getSingleVideo(List<FileModel> files, int videoid)
        {
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT * FROM video where Video_id=@Video_id";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@Video_id", videoid);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                files.Add(new FileModel
                                {

                                    video_id = Convert.ToInt32(sdr["Video_id"]),
                                    video_title = sdr["Video_title"].ToString(),
                                    video_views = Convert.ToInt32(sdr["Video_views"]),
                                    category_id = Convert.ToInt32(sdr["Category_id"]),
                                    content_type = sdr["Content_type"].ToString(),
                                    video_path = sdr["Video_path"].ToString(),
                                    video_desc = sdr["Video_desc"].ToString(),
                                    Id = sdr["Id"].ToString(),
                                    publish_datetime = sdr["Publish_datetime"].ToString(),
                                    video_likes = Convert.ToInt32(sdr["Video_likes"]),
                                    video_dislikes = Convert.ToInt32(sdr["Video_dislikes"]),
                                    total_comments = Convert.ToInt32(sdr["Total_comments"]),
                                    playlist_id = Convert.ToInt32(sdr["Playlist_id"])

                                });
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return files;
        }

        private int GetVideoIDFromPlaylist(int playlistid)
        {
            int video_id = 0;
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT Video_id FROM video where Playlist_id=@Playlist_id";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@Playlist_id", playlistid);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                video_id = Convert.ToInt32(sdr["Video_id"]);
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return video_id;
        }

        private int ChannelidfromPlaylist(int playlistid)
        {
            int channelid = 0;
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT Channel_id FROM playlist where Playlist_id=@Playlist_id";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@Playlist_id", playlistid);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                channelid = Convert.ToInt32(sdr["Channel_id"]);
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return channelid;
        }

        [HttpGet]
        public ActionResult PlaylistVideos(int PlaylistID ,int VideoID)
        {
            TempData["ReportCategories"] = GetReportCategories();
            try
            {
                if (CheckVideoCategoryInapp(VideoID) != 110)
                {
                    TempData["Category"] = CategoryList();
                    List<FileModel> files = new List<FileModel>();
                    getSingleVideo(files, VideoID);
                    var channel = new List<ChannelModel>();
                    GetChannel(getuseridofvideo(VideoID), channel);
                    List<CommentModel> comments = new List<CommentModel>();
                    GetComments(VideoID, comments);
                    List<FileModel> file = new List<FileModel>();
                    GetAllPlaylistVideos(PlaylistID, file);
                    var userID = User.Identity.GetUserId();
                    if (userID != null)
                    {
                        if (userChannelSubscriptionExists(userID, GetChannelofVideoFromVideoid(VideoID)) == true)
                        {
                            TempData["subscription"] = "UnSubscribe";
                        }
                    }

                    return View(Tuple.Create(files, channel, comments, file));
                }
                else
                {
                    return RedirectToAction("Index","Home");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return View("Index", "Home");
            }
        }

        //For PlaylistVideos page
        private static List<FileModel> GetAllPlaylistVideos(int playlistid, List<FileModel> files)
        {
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT * FROM video where Playlist_id=@Playlist_id";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@Playlist_id", playlistid);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                files.Add(new FileModel
                                {

                                    video_id = Convert.ToInt32(sdr["Video_id"]),
                                    video_title = sdr["Video_title"].ToString(),
                                    video_views = Convert.ToInt32(sdr["Video_views"]),
                                    category_id = Convert.ToInt32(sdr["Category_id"]),
                                    content_type = sdr["Content_type"].ToString(),
                                    video_path = sdr["Video_path"].ToString(),
                                    video_desc = sdr["Video_desc"].ToString(),
                                    Id = sdr["Id"].ToString(),
                                    publish_datetime = sdr["Publish_datetime"].ToString(),
                                    video_likes = Convert.ToInt32(sdr["Video_likes"]),
                                    video_dislikes = Convert.ToInt32(sdr["Video_dislikes"]),
                                    total_comments = Convert.ToInt32(sdr["Total_comments"]),
                                    playlist_id = Convert.ToInt32(sdr["Playlist_id"])
                                });
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return files;
        }

        //For Playlist Videos page
        public static List<ChannelModel> GetChannelfromPlaylist(List<ChannelModel> channel, string userID)
        {
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT * FROM channel where Id=@userid";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@userid", userID);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                channel.Add(new ChannelModel
                                {
                                    channel_id = Convert.ToInt32(sdr["Channel_id"]),
                                    channel_name = sdr["Channel_name"].ToString(),
                                    channel_desc = sdr["Channel_desc"].ToString(),
                                    channel_profile_pic = sdr["Channel_profilepic"].ToString(),
                                    channel_cover_pic = sdr["Channel_coverpic"].ToString(),
                                    email = sdr["Email"].ToString(),
                                    id = sdr["Id"].ToString(),
                                    Subscribers = Convert.ToInt32(sdr["Subscribers"]),
                                });
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return channel;
        }

        [Authorize]
        public static List<ChannelModel> GetChannelFromChannelid(int channelid, List<ChannelModel> channel)
        {
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT * FROM channel where Channel_id=@channelid";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("channelid", channelid);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                channel.Add(new ChannelModel
                                {
                                    channel_id = Convert.ToInt32(sdr["Channel_id"]),
                                    channel_name = sdr["Channel_name"].ToString(),
                                    channel_desc = sdr["Channel_desc"].ToString(),
                                    channel_profile_pic = sdr["Channel_profilepic"].ToString(),
                                    channel_cover_pic = sdr["Channel_coverpic"].ToString(),
                                    email = sdr["Email"].ToString(),
                                    id = sdr["Id"].ToString(),
                                    Subscribers = Convert.ToInt32(sdr["Subscribers"]),
                                    total_videos = Convert.ToInt32(sdr["Total_videos"]),
                                });
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return channel;
        }

        private static List<FileModel> GetFilesofChannel(List<FileModel> files, int channelid)
        {
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT * FROM video where Channel_id=@channelid";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@channelid", channelid);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                files.Add(new FileModel
                                {

                                    video_id = Convert.ToInt32(sdr["Video_id"]),
                                    video_title = sdr["Video_title"].ToString(),
                                    video_views = Convert.ToInt32(sdr["Video_views"]),
                                    category_id = Convert.ToInt32(sdr["Category_id"]),
                                    content_type = sdr["Content_type"].ToString(),
                                    video_path = sdr["Video_path"].ToString(),
                                    video_desc = sdr["Video_desc"].ToString(),
                                    Id = sdr["Id"].ToString(),
                                    publish_datetime = sdr["Publish_datetime"].ToString(),
                                    video_likes = Convert.ToInt32(sdr["Video_likes"]),
                                    video_dislikes = Convert.ToInt32(sdr["Video_dislikes"]),
                                    total_comments = Convert.ToInt32(sdr["Total_comments"]),
                                    playlist_id = Convert.ToInt32(sdr["Playlist_id"]),
                                    channel_id = Convert.ToInt32(sdr["Channel_id"])
                                });
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e) {

                Console.WriteLine(e.Message);
            }
            return files;
        }

        private static List<PlaylistModel> PlaylistFromChannel(List<PlaylistModel> playlist, int channelid)
        {
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                using (SqlConnection con = new SqlConnection(mainConnection))
                {
                    string query = "SELECT * FROM playlist where Channel_id=@channelid";
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@channelid", channelid);
                        cmd.Connection = con;
                        con.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                playlist.Add(new PlaylistModel
                                {
                                    playlist_id = Convert.ToInt32(sdr["Playlist_id"]),
                                    playlist_name = sdr["Playlist_name"].ToString(),
                                    playlist_desc = sdr["Playlist_desc"].ToString(),
                                    id = sdr["Id"].ToString()
                                });
                            }
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception e) {

                Console.WriteLine(e.Message);
            }

            return playlist;
        }

        [Authorize]
        public bool userChannelSubscriptionExists(string userID, int channelid)
        {
            bool count = false;
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT * FROM channelsubscriptions where Channel_id=@channelid and Id=@userID";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@channelid", channelid);
                        cmd.Parameters.AddWithValue("@userID", userID);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                count = true;
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return count;
        }

        public string userfromchannel(int channelid)
        {
            string userid = "";
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT Id FROM channel where Channel_id=@channelid";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@channelid", channelid);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                userid = sdr["Id"].ToString();
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return userid;
        }

        public int getViewsofChannel(int channelid)
        {
            int views = 0;
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "Select Channel_views from channel where Channel_id=@Channel_id";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@Channel_id", channelid);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                views = Convert.ToInt32(sdr["Channel_views"]);
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return views;
        }

        [HttpPost]
        [Authorize]
        public ActionResult updatechannelviews(int channelid)
        {
            var userID = User.Identity.GetUserId();
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(mainConnection);
                SqlCommand sqlcomm = new SqlCommand("updatechannelviews", sqlcon);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcon.Open();
                sqlcomm.Parameters.AddWithValue("@Channel_id", channelid);
                sqlcomm.Parameters.AddWithValue("@Channel_views", getViewsofChannel(channelid) + 1);
                sqlcomm.Parameters.AddWithValue("@Id", userID);
                sqlcomm.ExecuteNonQuery();
                sqlcon.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return RedirectToAction("Channel", "Home", new { @ChannelID = channelid });
        }

        [Authorize]
        public bool ChannelViewExists(string userID, int ChannelID)
        {
            bool count = false;
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT * FROM channel_views where Channel_id=@Channel_id and Id=@userID";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@Channel_id", ChannelID);
                        cmd.Parameters.AddWithValue("@userID", userID);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                count = true;
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }

            return count;
        }

        public ActionResult Channel(int ChannelID)
        {
            try
            {
                var userID = User.Identity.GetUserId();
                if (ChannelViewExists(userID,ChannelID) == false)
                {
                    updatechannelviews(ChannelID);
                }
                
                TempData["Category"] = CategoryList();
                
                if (userID != null)
                {
                    if (userChannelSubscriptionExists(userID, ChannelID) == true)
                    {
                        TempData["subscription"] = "UnSubscribe";
                    }
                }

                var files = new List<FileModel>();
                var playlist = new List<PlaylistModel>();
                var channel = new List<ChannelModel>();
                GetChannelFromChannelid(ChannelID, channel);
                GetFilesofChannel(files, ChannelID);
                PlaylistFromChannel(playlist, ChannelID);
                var channelsub = new List<ChannelModel>();
                GetSubscribedChannelDetails(userfromchannel(ChannelID), channelsub);

                return View(Tuple.Create(files, channel, playlist, channelsub));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return View("Index", "Home");
            }

        }

        [Authorize]
        public ActionResult AddComment(int VideoID, string Comment)
        {
            try
            {
                var userID = User.Identity.GetUserId();
                var username = User.Identity.GetUserName();
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(mainConnection);
                SqlCommand sqlcomm = new SqlCommand("insertcomment", sqlcon);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcon.Open();
                sqlcomm.Parameters.AddWithValue("@Id", userID);
                sqlcomm.Parameters.AddWithValue("@Comment_desc", Comment);
                sqlcomm.Parameters.AddWithValue("@Username", username);
                sqlcomm.Parameters.AddWithValue("@Video_id", VideoID);
                sqlcomm.Parameters.AddWithValue("@Comment_datetime", DateTime.Today.ToString());
                sqlcomm.ExecuteNonQuery();
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //TempData["commenterror"] = "Error Adding Comment!";
            }

            return RedirectToAction("Video", "Home", new { @VideoID = VideoID });
        }

        [Authorize]
        public ActionResult AddVideoCommentInPlaylist(int VideoID, string Comment, int PlaylistID)
        {
            try
            {
                var userID = User.Identity.GetUserId();
                var username = User.Identity.GetUserName();
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(mainConnection);
                SqlCommand sqlcomm = new SqlCommand("insertcomment", sqlcon);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcon.Open();
                sqlcomm.Parameters.AddWithValue("@Id", userID);
                sqlcomm.Parameters.AddWithValue("@Comment_desc", Comment);
                sqlcomm.Parameters.AddWithValue("@Username", username);
                sqlcomm.Parameters.AddWithValue("@Video_id", VideoID);
                sqlcomm.Parameters.AddWithValue("@Comment_datetime", DateTime.Today.ToString());
                sqlcomm.ExecuteNonQuery();
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //TempData["commenterror"] = "Error Adding Comment!";
            }

            return RedirectToAction("PlaylistVideos", "Home", new { @PlaylistID = PlaylistID, @VideoID = VideoID});
        }

        [Authorize]
        public bool userVideoLikeExists(string userID, int VideoID)
        {
            bool count = false;
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT * FROM videoLikes where Video_id=@Video_id and Id=@userID";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@Video_id", VideoID);
                        cmd.Parameters.AddWithValue("@userID", userID);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                count = true;
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return count;
        }

        [Authorize]
        public ActionResult AddLikeForPlaylist(int VideoID, int videoLike, int PlaylistID)
        {
            try
            {
                var userID = User.Identity.GetUserId();
                if (userVideoDisLikeExists(userID, VideoID) == false)
                {
                    if (userVideoLikeExists(userID, VideoID) == false)
                    {
                        string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                        SqlConnection sqlcon = new SqlConnection(mainConnection);
                        SqlCommand sqlcomm = new SqlCommand("addlike", sqlcon);
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcon.Open();
                        sqlcomm.Parameters.AddWithValue("@Video_likes", videoLike + 1);
                        sqlcomm.Parameters.AddWithValue("@Video_id", VideoID);
                        sqlcomm.Parameters.AddWithValue("@Id", userID);
                        sqlcomm.ExecuteNonQuery();
                        sqlcon.Close();
                    }
                    else
                    {
                        if (videoLike > 0)
                        {
                            videoLike = videoLike - 1;
                        }
                        else if (videoLike == 0)
                        {
                            videoLike = 0;
                        }
                        string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                        SqlConnection sqlcon = new SqlConnection(mainConnection);
                        SqlCommand sqlcomm = new SqlCommand("removelike", sqlcon);
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcon.Open();
                        sqlcomm.Parameters.AddWithValue("@Video_likes", videoLike);
                        sqlcomm.Parameters.AddWithValue("@Video_id", VideoID);
                        sqlcomm.Parameters.AddWithValue("@Id", userID);
                        sqlcomm.ExecuteNonQuery();
                        sqlcon.Close();
                    }
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

            return RedirectToAction("PlaylistVideos", "Home", new { @PlaylistID = PlaylistID });
        }

        [Authorize]
        public ActionResult AddLike(int VideoID, int videoLike)
        {
            try
            {
                var userID = User.Identity.GetUserId();
                if (userVideoDisLikeExists(userID, VideoID) == false)
                {
                    if (userVideoLikeExists(userID, VideoID) == false)
                    {
                        string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                        SqlConnection sqlcon = new SqlConnection(mainConnection);
                        SqlCommand sqlcomm = new SqlCommand("addlike", sqlcon);
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcon.Open();
                        sqlcomm.Parameters.AddWithValue("@Video_likes", videoLike + 1);
                        sqlcomm.Parameters.AddWithValue("@Video_id", VideoID);
                        sqlcomm.Parameters.AddWithValue("@Id", userID);
                        sqlcomm.ExecuteNonQuery();
                        sqlcon.Close();
                    }
                    else
                    {
                        if (videoLike > 0)
                        {
                            videoLike = videoLike - 1;
                        }
                        else if (videoLike == 0)
                        {
                            videoLike = 0;
                        }
                        string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                        SqlConnection sqlcon = new SqlConnection(mainConnection);
                        SqlCommand sqlcomm = new SqlCommand("removelike", sqlcon);
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcon.Open();
                        sqlcomm.Parameters.AddWithValue("@Video_likes", videoLike);
                        sqlcomm.Parameters.AddWithValue("@Video_id", VideoID);
                        sqlcomm.Parameters.AddWithValue("@Id", userID);
                        sqlcomm.ExecuteNonQuery();
                        sqlcon.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return RedirectToAction("Video", "Home", new { @VideoID = VideoID });
        }

        [Authorize]
        public ActionResult AddVideoLikePlaylist(int VideoID, int videoLike, int PlaylistID)
        {
            try
            {
                var userID = User.Identity.GetUserId();
                if (userVideoDisLikeExists(userID, VideoID) == false)
                {
                    if (userVideoLikeExists(userID, VideoID) == false)
                    {
                        string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                        SqlConnection sqlcon = new SqlConnection(mainConnection);
                        SqlCommand sqlcomm = new SqlCommand("addlike", sqlcon);
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcon.Open();
                        sqlcomm.Parameters.AddWithValue("@Video_likes", videoLike + 1);
                        sqlcomm.Parameters.AddWithValue("@Video_id", VideoID);
                        sqlcomm.Parameters.AddWithValue("@Id", userID);
                        sqlcomm.ExecuteNonQuery();
                        sqlcon.Close();
                    }
                    else
                    {
                        if (videoLike > 0)
                        {
                            videoLike = videoLike - 1;
                        }
                        else if (videoLike == 0)
                        {
                            videoLike = 0;
                        }
                        string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                        SqlConnection sqlcon = new SqlConnection(mainConnection);
                        SqlCommand sqlcomm = new SqlCommand("removelike", sqlcon);
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcon.Open();
                        sqlcomm.Parameters.AddWithValue("@Video_likes", videoLike);
                        sqlcomm.Parameters.AddWithValue("@Video_id", VideoID);
                        sqlcomm.Parameters.AddWithValue("@Id", userID);
                        sqlcomm.ExecuteNonQuery();
                        sqlcon.Close();
                    }
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

            return RedirectToAction("PlaylistVideos", "Home", new { @PlaylistID = PlaylistID });
        }

        [Authorize]
        public bool userVideoDisLikeExists(string userID, int VideoID)
        {
            bool count = false;
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT * FROM videoDislike where Video_id=@Video_id and Id=@userID";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@Video_id", VideoID);
                        cmd.Parameters.AddWithValue("@userID", userID);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                count = true;
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return count;
        }

        [Authorize]
        public ActionResult AddDislike(int VideoID, int videoDislike)
        {
            try
            {
                var userID = User.Identity.GetUserId();
                if (userVideoLikeExists(userID, VideoID) == false)
                {
                    if (userVideoDisLikeExists(userID, VideoID) == false)
                    {
                        string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                        SqlConnection sqlcon = new SqlConnection(mainConnection);
                        SqlCommand sqlcomm = new SqlCommand("addDislike", sqlcon);
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcon.Open();
                        sqlcomm.Parameters.AddWithValue("@Video_dislikes", videoDislike + 1);
                        sqlcomm.Parameters.AddWithValue("@Video_id", VideoID);
                        sqlcomm.Parameters.AddWithValue("@Id", userID);
                        sqlcomm.ExecuteNonQuery();
                        sqlcon.Close();
                    }
                    else
                    {
                        if (videoDislike > 0)
                        {
                            videoDislike = videoDislike - 1;
                        }
                        else if (videoDislike == 0)
                        {
                            videoDislike = 0;
                        }
                        string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                        SqlConnection sqlcon = new SqlConnection(mainConnection);
                        SqlCommand sqlcomm = new SqlCommand("removedislike", sqlcon);
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcon.Open();
                        sqlcomm.Parameters.AddWithValue("@Video_dislikes", videoDislike);
                        sqlcomm.Parameters.AddWithValue("@Video_id", VideoID);
                        sqlcomm.Parameters.AddWithValue("@Id", userID);
                        sqlcomm.ExecuteNonQuery();
                        sqlcon.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //TempData["commenterror"] = "Error Adding Comment!";
            }

            return RedirectToAction("Video", "Home", new { @VideoID = VideoID });
        }

        [Authorize]
        public ActionResult AddVideoDislikePlaylist(int VideoID, int videoDislike, int PlaylistID)
        {
            try
            {
                var userID = User.Identity.GetUserId();
                if (userVideoLikeExists(userID, VideoID) == false)
                {
                    if (userVideoDisLikeExists(userID, VideoID) == false)
                    {
                        string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                        SqlConnection sqlcon = new SqlConnection(mainConnection);
                        SqlCommand sqlcomm = new SqlCommand("addDislike", sqlcon);
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcon.Open();
                        sqlcomm.Parameters.AddWithValue("@Video_dislikes", videoDislike + 1);
                        sqlcomm.Parameters.AddWithValue("@Video_id", VideoID);
                        sqlcomm.Parameters.AddWithValue("@Id", userID);
                        sqlcomm.ExecuteNonQuery();
                        sqlcon.Close();
                    }
                    else
                    {
                        if (videoDislike > 0)
                        {
                            videoDislike = videoDislike - 1;
                        }
                        else if (videoDislike == 0)
                        {
                            videoDislike = 0;
                        }
                        string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                        SqlConnection sqlcon = new SqlConnection(mainConnection);
                        SqlCommand sqlcomm = new SqlCommand("removedislike", sqlcon);
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcon.Open();
                        sqlcomm.Parameters.AddWithValue("@Video_dislikes", videoDislike);
                        sqlcomm.Parameters.AddWithValue("@Video_id", VideoID);
                        sqlcomm.Parameters.AddWithValue("@Id", userID);
                        sqlcomm.ExecuteNonQuery();
                        sqlcon.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //TempData["commenterror"] = "Error Adding Comment!";
            }

            return RedirectToAction("PlaylistVideos", "Home", new { @PlaylistID = PlaylistID });
        }

        [Authorize]
        public ActionResult WatchList()
        {
            try
            {
                TempData["Category"] = CategoryList();
                var userID = User.Identity.GetUserId();
                List<FileModel> files = new List<FileModel>();
                GetWatchList(userID, files);
                return View(Tuple.Create(files));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return View("Index", "Home");
            }
        }

        public static List<FileModel> GetWatchList(String userID, List<FileModel> files)
        {
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT * FROM video where Video_id IN (Select Video_id from watchlist where Id=@userID)";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@userID", userID);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                files.Add(new FileModel
                                {
                                    video_id = Convert.ToInt32(sdr["Video_id"]),
                                    video_title = sdr["Video_title"].ToString(),
                                    video_views = Convert.ToInt32(sdr["Video_views"]),
                                    category_id = Convert.ToInt32(sdr["Category_id"]),
                                    content_type = sdr["Content_type"].ToString(),
                                    video_path = sdr["Video_path"].ToString(),
                                    video_desc = sdr["Video_desc"].ToString(),
                                    Id = sdr["Id"].ToString(),
                                    publish_datetime = sdr["Publish_datetime"].ToString(),
                                    video_likes = Convert.ToInt32(sdr["Video_likes"]),
                                    video_dislikes = Convert.ToInt32(sdr["Video_dislikes"]),
                                    total_comments = Convert.ToInt32(sdr["Total_comments"]),
                                    playlist_id = Convert.ToInt32(sdr["Playlist_id"]),
                                    channel_id = Convert.ToInt32(sdr["Channel_id"])
                                });
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return files;
        }

        [Authorize]
        public ActionResult ClearWatchlist()
        {
            try
            {
                var userID = User.Identity.GetUserId();
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(mainConnection);
                SqlCommand sqlcomm = new SqlCommand("clearwatchlist", sqlcon);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcon.Open();
                sqlcomm.Parameters.AddWithValue("@Id", userID);
                sqlcomm.ExecuteNonQuery();
                sqlcon.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return RedirectToAction("WatchList", "Home");
        }

        [Authorize]
        public ActionResult ChangeChannelName(int Channelid, string Channelname)
        {
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(mainConnection);
                SqlCommand sqlcomm = new SqlCommand("ChangeChannelname", sqlcon);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcon.Open();
                sqlcomm.Parameters.AddWithValue("@Channel_name", Channelname);
                sqlcomm.Parameters.AddWithValue("@Channel_id", Channelid);
                sqlcomm.ExecuteNonQuery();
                sqlcon.Close();
                TempData["channeldetailsupdated"] = "success";

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["channeldetailsupdated"] = "error";
            }

            return RedirectToAction("YourChannel", "Home");
        }

        public string GetVideoName(int videoid)
        {
            string video_title = "";
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT Video_title FROM video where Video_id=@Videoid";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@Videoid", videoid);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                video_title = sdr["Video_title"].ToString();
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return video_title;
        }

        [Authorize]
        public ActionResult DeleteVideo(int id, int channelid)
        {
            try
            {
                string videoname = GetVideoName(id);
                System.IO.File.Delete("E:\\BS(CS)\\FYP\\youtubecloneapp\\youtubecloneapp\\youtubeclone\\Content\\converted\\" + videoname + ".mp4");
                System.IO.File.Delete("E:\\BS(CS)\\FYP\\youtubecloneapp\\youtubecloneapp\\youtubeclone\\Content\\converted\\Converted " + videoname + ".mp4");

                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(mainConnection);
                SqlCommand sqlcomm = new SqlCommand("DeleteVideo", sqlcon);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcon.Open();
                sqlcomm.Parameters.AddWithValue("@Video_id", id);
                sqlcomm.Parameters.AddWithValue("@Channel_id", channelid);
                sqlcomm.ExecuteNonQuery();
                sqlcon.Close();
                TempData["videodeleted"] = "success";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["videodeleted"] = "error";
            }

            return RedirectToAction("YourChannel", "Home");
        }

        [Authorize]
        public ActionResult AddSubscription(int Channelid)
        {
            var userID = User.Identity.GetUserId();
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(mainConnection);
                SqlCommand sqlcomm = new SqlCommand("addchannelsubscription", sqlcon);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcon.Open();
                sqlcomm.Parameters.AddWithValue("@Id", userID);
                sqlcomm.Parameters.AddWithValue("@Channel_id", Channelid);
                sqlcomm.ExecuteNonQuery();
                sqlcon.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return RedirectToAction("Channel", "Home", new { @ChannelID = Channelid });
        }

        [Authorize]
        public ActionResult RemoveSubscription(int Channelid)
        {
            var userID = User.Identity.GetUserId();
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(mainConnection);
                SqlCommand sqlcomm = new SqlCommand("removechannelsubscription", sqlcon);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcon.Open();
                sqlcomm.Parameters.AddWithValue("@Id", userID);
                sqlcomm.Parameters.AddWithValue("@Channel_id", Channelid);
                sqlcomm.ExecuteNonQuery();
                sqlcon.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return RedirectToAction("Channel", "Home", new { @ChannelID = Channelid });
        }

        [Authorize]
        public ActionResult AddSubscriptionVideoRedirect(int Channelid, int Videoid)
        {
            var userID = User.Identity.GetUserId();
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(mainConnection);
                SqlCommand sqlcomm = new SqlCommand("addchannelsubscription", sqlcon);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcon.Open();
                sqlcomm.Parameters.AddWithValue("@Id", userID);
                sqlcomm.Parameters.AddWithValue("@Channel_id", Channelid);
                sqlcomm.ExecuteNonQuery();
                sqlcon.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return RedirectToAction("Video", "Home", new { @VideoID = Videoid });
        }

        [Authorize]
        public ActionResult RemoveSubscriptionVideoRedirect(int Channelid, int Videoid)
        {
            var userID = User.Identity.GetUserId();
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(mainConnection);
                SqlCommand sqlcomm = new SqlCommand("removechannelsubscription", sqlcon);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcon.Open();
                sqlcomm.Parameters.AddWithValue("@Id", userID);
                sqlcomm.Parameters.AddWithValue("@Channel_id", Channelid);
                sqlcomm.ExecuteNonQuery();
                sqlcon.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return RedirectToAction("Video", "Home", new { @VideoID = Videoid });
        }

        [Authorize]
        public ActionResult RemoveSubscriptionYourChannelRedirect(int Channelid)
        {
            var userID = User.Identity.GetUserId();
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(mainConnection);
                SqlCommand sqlcomm = new SqlCommand("removechannelsubscription", sqlcon);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcon.Open();
                sqlcomm.Parameters.AddWithValue("@Id", userID);
                sqlcomm.Parameters.AddWithValue("@Channel_id", Channelid);
                sqlcomm.ExecuteNonQuery();
                sqlcon.Close();
                TempData["removesubscriptionfromyourchannel"] = "success";

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["removesubscriptionfromyourchannel"] = "error";
            }

            return RedirectToAction("YourChannel", "Home");
        }

        [Authorize]
        public ActionResult ChangeCoverPic(int Channelid, HttpPostedFileBase NewCover, HttpPostedFileBase CoverPic)
        {
            try
            {
                string filename = Path.GetFileName(NewCover.FileName);
                string fileext = Path.GetExtension(filename);
                NewCover.SaveAs(Server.MapPath("/Content/coverphotos/" + filename));
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(mainConnection);
                SqlCommand sqlcomm = new SqlCommand("changeCoverPic", sqlcon);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcon.Open();
                sqlcomm.Parameters.AddWithValue("@Channel_id", Channelid);
                sqlcomm.Parameters.AddWithValue("@Channel_coverpic", "/Content/coverphotos/" + filename);
                sqlcomm.ExecuteNonQuery();
                sqlcon.Close();
                TempData["channeldetailsupdated"] = "success";

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["channeldetailsupdated"] = "error";
            }

            return RedirectToAction("YourChannel", "Home");
        }

        [Authorize]
        public ActionResult ChangeProfilePic(int Channelid, HttpPostedFileBase NewProfile, HttpPostedFileBase ProfilePic)
        {
            try
            {
                string filename = Path.GetFileName(NewProfile.FileName);
                string fileext = Path.GetExtension(filename);
                NewProfile.SaveAs(Server.MapPath("/Content/profilepics/" + filename));
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(mainConnection);
                SqlCommand sqlcomm = new SqlCommand("changeProfilePic", sqlcon);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcon.Open();
                sqlcomm.Parameters.AddWithValue("@Channel_id", Channelid);
                sqlcomm.Parameters.AddWithValue("@Channel_profilepic", "/Content/profilepics/" + filename);
                sqlcomm.ExecuteNonQuery();
                sqlcon.Close();
                TempData["channeldetailsupdated"] = "success";

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["channeldetailsupdated"] = "error";
            }

            return RedirectToAction("YourChannel", "Home");
        }

        [Authorize]
        public ActionResult ChangeChannelDesc(int Channelid, string Channeldesc)
        {
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(mainConnection);
                SqlCommand sqlcomm = new SqlCommand("changeChannelDesc", sqlcon);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcon.Open();
                sqlcomm.Parameters.AddWithValue("@Channel_id", Channelid);
                sqlcomm.Parameters.AddWithValue("@Channel_desc", Channeldesc);
                sqlcomm.ExecuteNonQuery();
                sqlcon.Close();
                TempData["channeldetailsupdated"] = "success";

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["channeldetailsupdated"] = "error";
            }

            return RedirectToAction("YourChannel", "Home");
        }

        [Authorize]
        public ActionResult ChangeChannelEmail(int Channelid, string ChannelEmail)
        {
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(mainConnection);
                SqlCommand sqlcomm = new SqlCommand("changeChannelEmailInstaFB", sqlcon);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcon.Open();
                sqlcomm.Parameters.AddWithValue("@Channel_id", Channelid);
                sqlcomm.Parameters.AddWithValue("@Email", ChannelEmail);
                sqlcomm.ExecuteNonQuery();
                sqlcon.Close();
                TempData["channeldetailsupdated"] = "success";

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["channeldetailsupdated"] = "error";
            }

            return RedirectToAction("YourChannel", "Home");
        }

        [Authorize]
        public ActionResult DeletePlaylist(int PlaylistID)
        {
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(mainConnection);
                SqlCommand sqlcomm = new SqlCommand("deletePlaylist", sqlcon);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcon.Open();
                sqlcomm.Parameters.AddWithValue("@Playlist_id", PlaylistID);
                sqlcomm.ExecuteNonQuery();
                sqlcon.Close();
                TempData["playlistdeleted"] = "success";

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["playlistdeleted"] = "error";
            }

            return RedirectToAction("YourChannel", "Home");
        }

        [Authorize]
        public ActionResult DeleteVideoFromWatchlist(int id)
        {
            try
            {
                var userID = User.Identity.GetUserId();
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(mainConnection);
                SqlCommand sqlcomm = new SqlCommand("DeleteWatchlistVideo", sqlcon);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcon.Open();
                sqlcomm.Parameters.AddWithValue("@Video_id", id);
                sqlcomm.Parameters.AddWithValue("@Id", userID);
                sqlcomm.ExecuteNonQuery();
                sqlcon.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return RedirectToAction("WatchList", "Home");
        }

        public async Task<bool> ConvertVideo(string filename, string ext)
        {
            try
            {
                var input = Server.MapPath("/Content/video/" + filename+ext);
                var output = Server.MapPath("/Content/converted/" +filename+".mp4");

                FFmpeg.ExecutablesPath = Server.MapPath("/Content/ffmpeg/");

                var info = await MediaInfo.Get(input);

                var videostream = info.VideoStreams.First()
                    .SetCodec(VideoCodec.Hevc)
                    .SetSize(VideoSize.Hd480);
                var audioStream = info.AudioStreams.First().SetCodec(AudioCodec.Aac);

                await Conversion.New()
                    .AddStream(videostream)
                    .AddStream(audioStream)
                    .SetOutput(output)
                    .SetPreset(ConversionPreset.UltraFast)
                    .Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return true;
        }

        [Authorize]
        [HttpPost]
        public ActionResult updatevideo(int VideoID, string videotitle, string videodesc)
        {
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(mainConnection);
                SqlCommand sqlcomm = new SqlCommand("changevideodetails", sqlcon);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcon.Open();
                sqlcomm.Parameters.AddWithValue("@Video_id", VideoID);
                sqlcomm.Parameters.AddWithValue("@Video_title", videotitle);
                sqlcomm.Parameters.AddWithValue("@Video_desc", videodesc);
                sqlcomm.Parameters.AddWithValue("@Publish_datetime", DateTime.Today.ToString());
                sqlcomm.ExecuteNonQuery();
                sqlcon.Close();
                TempData["videoupdated"] = "success";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["videoupdated"] = "error";
            }

            return RedirectToAction("YourChannel", "Home");
        }

        [Authorize]
        [HttpPost]
        public ActionResult UpdatePlaylist(int PlaylistID, string playlisttitle, string playlistdesc)
        {
            try
            {
                var userID = User.Identity.GetUserId();
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(mainConnection);
                SqlCommand sqlcomm = new SqlCommand("updateplaylist", sqlcon);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcon.Open();
                sqlcomm.Parameters.AddWithValue("@Playlist_id", PlaylistID);
                sqlcomm.Parameters.AddWithValue("@Playlist_name", playlisttitle);
                sqlcomm.Parameters.AddWithValue("@Playlist_desc", playlistdesc);
                sqlcomm.ExecuteNonQuery();
                sqlcon.Close();
                TempData["playlistupdated"] = "success";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["playlistupdated"] = "error";
            }

            return RedirectToAction("YourChannel", "Home");
        }

        [Authorize]
        public ActionResult RemoveComment(int CommentID, int VideoID)
        {
            var userID = User.Identity.GetUserId();
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(mainConnection);
                SqlCommand sqlcomm = new SqlCommand("removecomment", sqlcon);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcon.Open();
                sqlcomm.Parameters.AddWithValue("@Comment_id", CommentID);
                sqlcomm.ExecuteNonQuery();
                sqlcon.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return RedirectToAction("Video", "Home", new { @VideoID = VideoID });
        }

        [Authorize]
        public ActionResult RemovePlaylistVideoComment(int CommentID, int VideoID, int PlaylistID)
        {
            var userID = User.Identity.GetUserId();
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(mainConnection);
                SqlCommand sqlcomm = new SqlCommand("removecomment", sqlcon);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcon.Open();
                sqlcomm.Parameters.AddWithValue("@Comment_id", CommentID);
                sqlcomm.ExecuteNonQuery();
                sqlcon.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return RedirectToAction("PlaylistVideos", "Home", new { @VideoID = VideoID,@PlaylistID=PlaylistID });
        }

        [HttpPost]
        [Authorize]
        public ActionResult updatecommentforplaylist(int commentid, int videoid,int playlistid, string CommentDesc)
        {
            var userID = User.Identity.GetUserId();
            try
            {
                
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(mainConnection);
                SqlCommand sqlcomm = new SqlCommand("UpdateComment", sqlcon);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcon.Open();
                sqlcomm.Parameters.AddWithValue("@Comment_id", commentid);
                sqlcomm.Parameters.AddWithValue("@Comment_desc", CommentDesc);
                sqlcomm.ExecuteNonQuery();
                sqlcon.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return RedirectToAction("PlaylistVideos", "Home", new { @VideoID = videoid,@PlaylistID = playlistid });
        }

        [HttpPost]
        [Authorize]
        public ActionResult updatecomment(int commentid, int videoid,string CommentDesc)
        {
            var userID = User.Identity.GetUserId();
            try
            {

                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(mainConnection);
                SqlCommand sqlcomm = new SqlCommand("UpdateComment", sqlcon);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcon.Open();
                sqlcomm.Parameters.AddWithValue("@Comment_id", commentid);
                sqlcomm.Parameters.AddWithValue("@Comment_desc", CommentDesc);
                sqlcomm.ExecuteNonQuery();
                sqlcon.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return RedirectToAction("Video", "Home", new { @VideoID = videoid });
        }

        [Authorize]
        public ActionResult UserGuide()
        {
            TempData["Category"] = CategoryList();
            return View();
        }

        [Authorize]
        public ActionResult reportvideo(int VideoID,string report)
        {
            var userID = User.Identity.GetUserId();
            var catid = CheckVideoCategoryInapp(VideoID);
            var reportedcategoryid = GetReportCategoriesID(report);
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(mainConnection);
                SqlCommand sqlcomm = new SqlCommand("addvideoreport", sqlcon);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcon.Open();
                sqlcomm.Parameters.AddWithValue("@Video_id", VideoID);
                sqlcomm.Parameters.AddWithValue("@Category_id", catid);
                sqlcomm.Parameters.AddWithValue("@ReportedCategory_id", reportedcategoryid);
                sqlcomm.ExecuteNonQuery();
                sqlcon.Close();
                TempData["videoreport"] = "success";

            }
            catch (Exception ex)
            {
                TempData["videoreport"] = "error";
                Console.WriteLine(ex.Message);
            }

            return RedirectToAction("Video", "Home", new { @VideoID = VideoID });
        }

        public static List<ChannelModel> GetChannelForSearchPage(List<ChannelModel> channel)
        {
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT * FROM channel";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                channel.Add(new ChannelModel
                                {
                                    channel_id = Convert.ToInt32(sdr["Channel_id"]),
                                    channel_name = sdr["Channel_name"].ToString(),
                                    channel_desc = sdr["Channel_desc"].ToString(),
                                    channel_profile_pic = sdr["Channel_profilepic"].ToString(),
                                    channel_cover_pic = sdr["Channel_coverpic"].ToString(),
                                    email = sdr["Email"].ToString(),
                                    id = sdr["Id"].ToString(),
                                    Subscribers = Convert.ToInt32(sdr["Subscribers"]),
                                });
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return channel;
        }

        public static List<ChannelModel> GetChannelDeatilsForSearchPage(List<ChannelModel> channel, string tag, string userid)
        {
            try
            {
                if(userid == null) { userid = ""; }
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT * FROM channel where Channel_name LIKE '%" + tag + "%' and Id!=@userid";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@userid", userid);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                channel.Add(new ChannelModel
                                {
                                    channel_id = Convert.ToInt32(sdr["Channel_id"]),
                                    channel_name = sdr["Channel_name"].ToString(),
                                    channel_desc = sdr["Channel_desc"].ToString(),
                                    channel_profile_pic = sdr["Channel_profilepic"].ToString(),
                                    channel_cover_pic = sdr["Channel_coverpic"].ToString(),
                                    email = sdr["Email"].ToString(),
                                    id = sdr["Id"].ToString(),
                                    Subscribers = Convert.ToInt32(sdr["Subscribers"]),
                                    total_videos = Convert.ToInt32(sdr["Total_videos"]),
                                });
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return channel;
        }

        [Authorize]
        public ActionResult WatchLater()
        {
            try
            {
                TempData["Category"] = CategoryList();
                var userID = User.Identity.GetUserId();
                List<FileModel> files = new List<FileModel>();
                GetWatchLater(userID, files);
                return View(Tuple.Create(files));
               
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return View("Index", "Home");
            }
        }

        [Authorize]
        public ActionResult AddWatchLater(int VideoID)
        {
            try
            {
                
                var userID = User.Identity.GetUserId();
                if (WatchLaterVideoExists(userID, VideoID) != true)
                {
                    string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                    SqlConnection sqlcon = new SqlConnection(mainConnection);
                    SqlCommand sqlcomm = new SqlCommand("addwatchlater", sqlcon);
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcon.Open();
                    sqlcomm.Parameters.AddWithValue("@Id", userID);
                    sqlcomm.Parameters.AddWithValue("@Video_id", VideoID);
                    sqlcomm.ExecuteNonQuery();
                    sqlcon.Close();
                }
               
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //TempData["commenterror"] = "Error Adding Comment!";
            }

            return RedirectToAction("WatchLater", "Home");
        }

        public static List<FileModel> GetWatchLater(String userID, List<FileModel> files)
        {
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT * FROM video where Video_id IN (Select Video_id from watchlater where Id=@userID)";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@userID", userID);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                files.Add(new FileModel
                                {
                                    video_id = Convert.ToInt32(sdr["Video_id"]),
                                    video_title = sdr["Video_title"].ToString(),
                                    video_views = Convert.ToInt32(sdr["Video_views"]),
                                    category_id = Convert.ToInt32(sdr["Category_id"]),
                                    content_type = sdr["Content_type"].ToString(),
                                    video_path = sdr["Video_path"].ToString(),
                                    video_desc = sdr["Video_desc"].ToString(),
                                    Id = sdr["Id"].ToString(),
                                    publish_datetime = sdr["Publish_datetime"].ToString(),
                                    video_likes = Convert.ToInt32(sdr["Video_likes"]),
                                    video_dislikes = Convert.ToInt32(sdr["Video_dislikes"]),
                                    total_comments = Convert.ToInt32(sdr["Total_comments"]),
                                    playlist_id = Convert.ToInt32(sdr["Playlist_id"]),
                                    channel_id = Convert.ToInt32(sdr["Channel_id"])
                                });
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return files;
        }

        [Authorize]
        public ActionResult ClearWatchlater()
        {
            try
            {
                var userID = User.Identity.GetUserId();
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(mainConnection);
                SqlCommand sqlcomm = new SqlCommand("clearwatchlater", sqlcon);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcon.Open();
                sqlcomm.Parameters.AddWithValue("@Id", userID);
                sqlcomm.ExecuteNonQuery();
                sqlcon.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return RedirectToAction("WatchLater", "Home");
        }

        [Authorize]
        public ActionResult DeleteVideoFromWatchlater(int id)
        {
            try
            {
                var userID = User.Identity.GetUserId();
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(mainConnection);
                SqlCommand sqlcomm = new SqlCommand("DeleteWatchlaterVideo", sqlcon);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcon.Open();
                sqlcomm.Parameters.AddWithValue("@Video_id", id);
                sqlcomm.Parameters.AddWithValue("@Id", userID);
                sqlcomm.ExecuteNonQuery();
                sqlcon.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return RedirectToAction("WatchLater", "Home");
        }

        [Authorize]
        public bool WatchLaterVideoExists(string userID, int VideoID)
        {
            bool count = false;
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT * FROM watchlater where Video_id=@Video_id and Id=@userID";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@Video_id", VideoID);
                        cmd.Parameters.AddWithValue("@userID", userID);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                count = true;
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }

            return count;
        }

        [Authorize]
        public ActionResult LikedVideos()
        {
            try
            {
                TempData["Category"] = CategoryList();
                var userID = User.Identity.GetUserId();
                List<FileModel> files = new List<FileModel>();
                GetLikedVideos(userID, files);
                return View(Tuple.Create(files));

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return View("Index", "Home");
            }
        }

        [Authorize]
        public ActionResult DeleteLikedVideos(int id,int videolike)
        {
            try
            {
                var userID = User.Identity.GetUserId();
                if (videolike > 0)
                {
                    videolike = videolike - 1;
                }
                else if (videolike == 0)
                {
                    videolike = 0;
                }
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(mainConnection);
                SqlCommand sqlcomm = new SqlCommand("removelike", sqlcon);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcon.Open();
                sqlcomm.Parameters.AddWithValue("@Video_likes", videolike);
                sqlcomm.Parameters.AddWithValue("@Video_id", id);
                sqlcomm.Parameters.AddWithValue("@Id", userID);
                sqlcomm.ExecuteNonQuery();
                sqlcon.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return RedirectToAction("LikedVideos", "Home");
        }

        public int GetLikedVideosnumber(string userid)
        {
            int video_likes = 0;
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT * FROM video where Video_id IN (Select Video_id from videolikes where Id=@userID)";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@userID", userid);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                video_likes = Convert.ToInt32(sdr["Video_likes"]);
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return video_likes;
        }

        public static List<FileModel> GetLikedVideos(string userid, List<FileModel> files)
        {
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT * FROM video where Video_id IN (Select Video_id from videolikes where Id=@userID)";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@userID", userid);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                files.Add(new FileModel
                                {
                                    video_id = Convert.ToInt32(sdr["Video_id"]),
                                    video_title = sdr["Video_title"].ToString(),
                                    video_views = Convert.ToInt32(sdr["Video_views"]),
                                    category_id = Convert.ToInt32(sdr["Category_id"]),
                                    content_type = sdr["Content_type"].ToString(),
                                    video_path = sdr["Video_path"].ToString(),
                                    video_desc = sdr["Video_desc"].ToString(),
                                    Id = sdr["Id"].ToString(),
                                    publish_datetime = sdr["Publish_datetime"].ToString(),
                                    video_likes = Convert.ToInt32(sdr["Video_likes"]),
                                    video_dislikes = Convert.ToInt32(sdr["Video_dislikes"]),
                                    total_comments = Convert.ToInt32(sdr["Total_comments"]),
                                    playlist_id = Convert.ToInt32(sdr["Playlist_id"]),
                                    channel_id = Convert.ToInt32(sdr["Channel_id"])
                                });
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e) {

                Console.WriteLine(e.Message);
            }
            return files;
        }

        private static List<string> GetReportCategories()
        {
            List<string> ReportCategoryName = new List<string>();
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT ReportCategory_name FROM report_category";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                ReportCategoryName.Add(sdr["ReportCategory_name"].ToString());
                                
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return ReportCategoryName;
        }

        private static int GetReportCategoriesID(string catname)
        {
            int ReportCategoryID = 0;
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT ReportCategory_id FROM report_category where ReportCategory_name=@catname";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = sqlConnection;
                        cmd.Parameters.AddWithValue("@catname", catname);
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                ReportCategoryID=Convert.ToInt32(sdr["ReportCategory_id"]);

                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return ReportCategoryID;
        }

        private static int GetChannelSubscribers (string UID)
        {
            int subscribers = 0;
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT Subscribers FROM channel where Id=@Id";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = sqlConnection;
                        cmd.Parameters.AddWithValue("@Id", UID);
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                subscribers = Convert.ToInt32(sdr["Subscribers"]);

                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return subscribers;
        }

        private static int GetChannelViews(string UID)
        {
            int views = 0;
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT Channel_views FROM channel where Id=@Id";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = sqlConnection;
                        cmd.Parameters.AddWithValue("@Id", UID);
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                views = Convert.ToInt32(sdr["Channel_views"]);

                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return views;
        }

        private static List<PlaylistModel> GetPlaylist(List<PlaylistModel> playlist,int playlistid)
        {
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT * FROM playlist where Playlist_id=@Playlist_id";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@Playlist_id", playlistid);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                playlist.Add(new PlaylistModel
                                {

                                    playlist_id = Convert.ToInt32(sdr["Playlist_id"]),
                                    id = sdr["Id"].ToString(),
                                    playlist_name = sdr["Playlist_name"].ToString(),
                                    playlist_desc = sdr["Playlist_desc"].ToString(),
                                    Channel_id = Convert.ToInt32(sdr["Channel_id"])
                                });
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return playlist;
        }

        private string GetUserIdFromPlaylist(int playlistid)
        {
            string id = "";
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT Id FROM playlist where Playlist_id=@Playlist_id";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@Playlist_id", playlistid);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {

                                id = sdr["Id"].ToString();
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return id;
        }

        private string GetUserName(string userid)
        {
            string name = "";
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT UserName FROM AspNetUsers where Id=@id";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@id", userid);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {

                                name = sdr["UserName"].ToString();
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return name;
        }

        [HttpGet]
        public ActionResult PlaylistPage(int PlaylistID)
        {
            TempData["Category"] = CategoryList();
            var userID = GetUserIdFromPlaylist(PlaylistID);
            var username = GetUserName(userID);
            TempData["user"] = username;
            TempData["playlistid"] = PlaylistID;
            try
            {
                List<FileModel> file = new List<FileModel>();
                List<PlaylistModel> playlist = new List<PlaylistModel>();
                List<ChannelModel> channel = new List<ChannelModel>();
                GetChannel(userID, channel);
                GetAllPlaylistVideos(PlaylistID,file);
                GetPlaylist(playlist,PlaylistID);
                return View(Tuple.Create(file,playlist,channel));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return RedirectToAction("Index", "Home");

            }
            
        }

        [Authorize]
        public ActionResult DeleteVideofromPlaylist(int id)
        {
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(mainConnection);
                SqlCommand sqlcomm = new SqlCommand("deletevideofromPlaylist", sqlcon);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcon.Open();
                sqlcomm.Parameters.AddWithValue("@Video_id", id);
                sqlcomm.ExecuteNonQuery();
                sqlcon.Close();
                TempData["removevideofromplaylist"] = "success";

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["removevideofromplaylist"] = "error";
            }

            return RedirectToAction("YourChannel", "Home");
        }

        [Authorize]
        public ActionResult addvideoinplaylist(int videoid, int PlaylistID)
        {
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(mainConnection);
                SqlCommand sqlcomm = new SqlCommand("insertvideotoplaylist", sqlcon);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcon.Open();
                sqlcomm.Parameters.AddWithValue("@Video_id", videoid);
                sqlcomm.Parameters.AddWithValue("@Playlist_id", PlaylistID);
                sqlcomm.ExecuteNonQuery();
                sqlcon.Close();
                TempData["addvideotoplaylist"] = "success";

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["addvideotoplaylist"] = "error";
            }

            return RedirectToAction("YourChannel", "Home");
        }


        [Authorize]
        public ActionResult UpdateVideoDetails(int VideoID)
        {
            try
            {
                var userID = User.Identity.GetUserId();
                TempData["Category"] = CategoryList();
                var files = new List<FileModel>();
                getSingleVideo(files,VideoID);
                return View(Tuple.Create(files));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return View("Index", "Home");
            }
        }

        [Authorize]
        public ActionResult UpdatePlaylistDetails(int playlistID)
        {
            try
            {
                var userID = User.Identity.GetUserId();
                TempData["Category"] = CategoryList();
                var playlist = new List<PlaylistModel>();
                GetPlaylist(playlist,playlistID);
                return View(Tuple.Create(playlist));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return View("Index", "Home");
            }
        }

        public static List<CommentModel> GetSingleCommentDetails(int commentid, List<CommentModel> comments)
        {
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT * FROM comments where Comment_id=@commentid";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@commentid", commentid);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                comments.Add(new CommentModel
                                {
                                    CommentId = Convert.ToInt32(sdr["Comment_id"]),
                                    CommentDesc = sdr["Comment_desc"].ToString(),
                                    Id = sdr["Id"].ToString(),
                                    datetime = sdr["Comment_datetime"].ToString(),
                                    username = sdr["Username"].ToString()
                                });
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return comments;
        }

        [Authorize]
        public ActionResult UpdateCommentPlaylist(int commentid, int VideoID, int playlistid)
        {
            try
            {
                var userID = User.Identity.GetUserId();
                TempData["Category"] = CategoryList();
                var comment = new List<CommentModel>();
                GetSingleCommentDetails(commentid,comment);
                TempData["videoid"] = VideoID;
                TempData["playlistid"] = playlistid;
                return View(Tuple.Create(comment));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return View("Index", "Home");
            }
        }

        [Authorize]
        public ActionResult UpdateVideoComment(int commentid, int VideoID)
        {
            try
            {
                var userID = User.Identity.GetUserId();
                TempData["Category"] = CategoryList();
                var comment = new List<CommentModel>();
                GetSingleCommentDetails(commentid, comment);
                TempData["videoid"] = VideoID;
                return View(Tuple.Create(comment));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return View("Index", "Home");
            }
        }

        [Authorize]
        public ActionResult AddVideoToPlaylist(int id, int channelid)
        {
            try
            {
                var userID = User.Identity.GetUserId();
                TempData["Category"] = CategoryList();
                TempData["videoid"] = id;
                var playlist = new List<PlaylistModel>();
                GetAllPlaylistofUsers(playlist,channelid);
                return View(Tuple.Create(playlist));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return View("Index", "Home");
            }
        }

        private static List<PlaylistModel> GetAllPlaylistofUsers(List<PlaylistModel> playlist, int channelid)
        {
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var query = "SELECT * FROM playlist where Channel_id=@channelid";
                using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = sqlConnection;
                        cmd.Parameters.AddWithValue("@channelid",channelid);
                        sqlConnection.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                playlist.Add(new PlaylistModel
                                {

                                    playlist_id = Convert.ToInt32(sdr["Playlist_id"]),
                                    id = sdr["Id"].ToString(),
                                    playlist_name = sdr["Playlist_name"].ToString(),
                                    playlist_desc = sdr["Playlist_desc"].ToString(),
                                    Channel_id = Convert.ToInt32(sdr["Channel_id"])
                                });
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return playlist;
        }
    }

}