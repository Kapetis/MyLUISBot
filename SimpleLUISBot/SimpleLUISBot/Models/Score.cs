using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SimpleLUISBot.Models
{
    public class Score
    {
        public Score(string user, int point, DateTime dateTime)
        {
            User = user;
            Point = point;
            DateTime = dateTime;
        }

        public string User { get; set; }
        public int Point { get; set; }
        public DateTime DateTime { get; set; }

        public override string ToString()
        {
            StringBuilder sb =new StringBuilder();
            sb.Append($"User:{User}, Score:{Point}, Date: {DateTime.ToLocalTime().ToShortDateString()}");
            return sb.ToString();
        }
    }
}