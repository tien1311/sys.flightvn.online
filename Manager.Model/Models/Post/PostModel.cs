using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Model.Models.Post
{
    public class PostModel
    {
        public int RowNum { get; set; }
        public int subject_id { get; set; } 
        public string subject_name { get; set; }
        public string subject_author { get; set; }
        public string subject_header { get; set; }
        public bool isMail { get; set; }
        public int subject_isshow { get; set; }
        public int subject_isnew {  get; set; }
        public int subject_ishot { get; set; }
        public int subject_com { get; set; }
        public int subject_seq { get; set; }
        public string subject_date { get; set; }
        public int section_id { get; set; }
        public string section_name { get; set; }
        public string MailPNG { get; set; }
        public string hoticon { get; set; }
        public string subject_picnote { get; set; }
        public string subject_picture { get; set; }
        public string subject_content { get; set; }
    }

    public class Section
    {
        public int section_id { get; set; }
        public string section_name { get; set; }
        public int parent_id { get; set; }
    }
}
