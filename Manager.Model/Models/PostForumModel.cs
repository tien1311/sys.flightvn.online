using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Models
{
    public class PostForumModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CreatedOn { get; set; }
        public string UserCreated { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public bool IsDeleted { get; set; }
        public string IsActive { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public List<TagModel> ListTags { get; set; }
    }
    public class TagModel
    {
        public int IdTag { get; set; }
        public string Name { get; set; }
    }
}
