using System;
using System.Collections.Generic;

namespace Manager.Model.Models
{
    public class PostsAdsModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string CategoryName { get; set; }
        public int CategoryID { get; set; }
        public List<CategoriesPostsAds> List_CategoriesPostsAds { get; set; }
    }
    public class CategoriesPostsAds
    {
        public int ID { get; set; }
        public string CategoryName { get; set; }
    }
}
