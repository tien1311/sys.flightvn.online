using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Model.Models.LandingPage
{
    public class Slider
    {
        public int Id { get; set; }
        public string Image {  get; set; }
        public int Position { get; set; }
        public string Header { get; set; }
        public string Title { get; set; }
        public bool IsActived { get; set; }
    }
}
