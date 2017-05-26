using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SimpleLUISBot.Models
{
    public class ScoreRepository:IRepository<Score>
    {

        public  IEnumerable<Score> GetAll()
        {
            var scores = new List<Score>();
            scores.Add(new Score("Andy", 7, DateTime.Now));
            scores.Add(new Score("Andy", 20, DateTime.Now.AddDays(-7)));
            scores.Add(new Score("Andy", 13, DateTime.Now));
            scores.Add(new Score("Bob", 6, DateTime.Now));
            scores.Add(new Score("Bob", 19, DateTime.Now));
            scores.Add(new Score("Calvin", 35, DateTime.Now.AddDays(-30)));
            return scores;
        }
    }
}