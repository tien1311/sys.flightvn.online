using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Model.Models.LandingPage
{
    public class RewardYear
    {
        public int Id { get; set; }
        public string Year { get; set; }
        public List<Reward> Rewards { get; set; }
    }
}
