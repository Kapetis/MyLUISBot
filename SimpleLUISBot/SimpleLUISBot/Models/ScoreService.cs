using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleLUISBot.Models
{
    public class ScoreService
    {
        private IRepository<Score> _scoRepository;

        public ScoreService(IRepository<Score> scoRepository)
        {
            this._scoRepository = scoRepository;
        }


        public string GetHighScore()
        {
            return _scoRepository.GetAll().OrderByDescending(x => x.Point).FirstOrDefault()?.ToString();
        }

        public string GetHighScore(DateTime periodOfTime)
        {
            return _scoRepository.GetAll().Where(t => t.DateTime >= periodOfTime).OrderByDescending(x => x.Point).FirstOrDefault()?.ToString();
        }
    }
}