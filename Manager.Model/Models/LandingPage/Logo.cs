using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Manager.Model.Models.LandingPage
{
    public class Logo
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }

    }
}
