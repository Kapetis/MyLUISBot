using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SimpleLUISBot;
using SimpleLUISBot.Models;

namespace UnitTestProject2
{
    [TestClass]
    public class UnitTest1
    {
  

        [TestMethod]
        public void TestGetHighScoreWithDayAndPassNagiveThree()
        {
            Mock<IRepository<Score>> scoreMock = new Mock<IRepository<Score>>();
            scoreMock.Setup(m => m.GetAll())
                .Returns(new List<Score>()
                {
                    new Score("Andy", 7, DateTime.Now),
                    new Score("Andy", 8, DateTime.Now),
                    new Score("Bob", 9, DateTime.Now),
                    new Score("Calvin", 10, DateTime.Now.AddDays(-7))}
                );

            ScoreService scoreService = new ScoreService(scoreMock.Object);

            Assert.IsTrue(scoreService.GetHighScore(DateTime.Now.AddDays(-3)).Contains("Bob"));
          
        }

        [TestMethod]
        public void TestGetHighScoreWithDayAndPassNagiveSeven()
        {
            Mock<IRepository<Score>> scoreMock = new Mock<IRepository<Score>>();
            scoreMock.Setup(m => m.GetAll())
                .Returns(new List<Score>()
                    {
                        new Score("Andy", 7, DateTime.Now),
                        new Score("Andy", 8, DateTime.Now),
                        new Score("Bob", 9, DateTime.Now),
                        new Score("Calvin", 10, DateTime.Now.AddDays(-7))}
                );

            ScoreService scoreService = new ScoreService(scoreMock.Object);

            Assert.IsTrue(scoreService.GetHighScore(DateTime.Now.AddDays(-8)).Contains("Calvin"));

        }
    }

    
}
