using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using SimpleLUISBot.Models;

namespace SimpleLUISBot.Dialogs
{
    [LuisModel("your app id", "your key")]
    [Serializable]
    public class LUISDialog : LuisDialog<object>
    {
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {

            var reply = context.MakeMessage();
            reply.Text = "Sorry, I didn't get it";
            //Make you cortana speak
            reply.Speak = "Sorry, I can't understand";
            await context.PostAsync(reply);
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("HighScores")]
        public async Task GetHighScore(IDialogContext context, LuisResult result)
        {
            bool isIntentMatch = false;
            foreach (var objIntent in result.Intents)
            {
                if (objIntent.Intent == "HighScores" && objIntent.Score >= .98f)
                {
                    isIntentMatch = true;
                }
            }

            if (!isIntentMatch)
            {
                var reply = context.MakeMessage();
                reply.Type = "message";
                reply.Text = "I don't understand";
                reply.Speak = "I don't understand";
                await context.PostAsync(reply);
                context.Wait(this.MessageReceived);
                return;
            }

            // Determine the days in the past
            // to search for High Scores
            int intDays = -1;

            #region PeriodOfTime

            EntityRecommendation PeriodOfTime;
            if (result.TryFindEntity("PeriodOfTime", out PeriodOfTime))
            {
                switch (PeriodOfTime.Entity)
                {
                    case "month":
                        intDays = -30;
                        break;
                    case "day":
                        intDays = -1;
                        break;
                    case "week":
                        intDays = -7;
                        break;
                    default:
                        intDays = -1;
                        break;
                }
            }

            #endregion

            #region Days

            EntityRecommendation Days;
            if (result.TryFindEntity("Days", out Days))
            {
                // Set Days
                int intTempDays;
                if (int.TryParse(Days.Entity, out intTempDays))
                {
                    // A Number was passed
                    intDays = (Convert.ToInt32(intTempDays) * (-1));
                }
                else
                {
                    // A number was not passed
                    // Call ParseEnglish Method
                    // From: http://stackoverflow.com/questions/11278081/convert-words-string-to-int
                    intTempDays = ParseEnglish(Days.Entity);
                    intDays = (Convert.ToInt32(intTempDays) * (-1));
                }

                // 30 days maximum
                if (intDays > 30)
                {
                    intDays = 30;
                }
            }

            #endregion

            await ShowHighScores(context, intDays);
            context.Wait(this.MessageReceived);
        }
        private async Task ShowHighScores(IDialogContext context, int paramDays)
        {
            // Get Yesterday
            var ParamYesterday = DateTime.Now.AddDays(paramDays);
            ScoreService scoreService=new ScoreService(new ScoreRepository());
            var scoreRecord = scoreService.GetHighScore(ParamYesterday);
            // Create a reply message
            var resultMessage = context.MakeMessage();
            resultMessage.Type = "message";
            resultMessage.Text = scoreRecord;
            resultMessage.Speak = scoreRecord;

            // Send Message
            await context.PostAsync(resultMessage);
        }

        public string GuessNumber;
        [LuisIntent("PlayGame")]
        public async Task PlayGame(IDialogContext context, LuisResult result)
        {
           
            var options=new List<string>() {"1111", "221", "321"};
            Random random = new Random();
            int idx = random.Next(0, options.Count);
            GuessNumber = options[idx % options.Count];
            PromptDialog.Choice<string>(context, OnOptionSelected, options, "Guess a number");
        }
        private async Task OnOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            string optionSelected = await result;
            if (optionSelected == GuessNumber)
            {
                var reply = context.MakeMessage();
                reply.Text = "congratulation ! you win the game";
                reply.Speak = "congratulation ! you win the game";
                context.PostAsync(reply);
            }
            else
            {
                var reply = context.MakeMessage();
                reply.Text = "Sorry ! you lose the game";
                reply.Speak = "Sorry ! you lose the game";
                context.PostAsync(reply);
            }

            context.Wait(this.MessageReceived);
        }


        [LuisIntent("Greeting")]
        public async Task Welcome(IDialogContext context, LuisResult result)
        {

            var reply = context.MakeMessage();
            reply.Text = "Hello";
            //Make you cortana speak
            reply.Speak = "Hi, there!";
            await context.PostAsync(reply);
            context.Wait(this.MessageReceived);
        }
        


        #region static int ParseEnglish(string number)

        // From: http://stackoverflow.com/questions/11278081/convert-words-string-to-int
        static int ParseEnglish(string number)
        {
            string[] words = number.ToLower().Split(new char[] {' ', '-', ','}, StringSplitOptions.RemoveEmptyEntries);
            string[] ones = {"one", "two", "three", "four", "five", "six", "seven", "eight", "nine"};
            string[] teens =
                {"eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen"};
            string[] tens = {"ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety"};
            Dictionary<string, int> modifiers = new Dictionary<string, int>()
            {
                {"billion", 1000000000},
                {"million", 1000000},
                {"thousand", 1000},
                {"hundred", 100}
            };

            if (number == "eleventy billion")
                return int.MaxValue; // 110,000,000,000 is out of range for an int!

            int result = 0;
            int currentResult = 0;
            int lastModifier = 1;

            foreach (string word in words)
            {
                if (modifiers.ContainsKey(word))
                {
                    lastModifier *= modifiers[word];
                }
                else
                {
                    int n;

                    if (lastModifier > 1)
                    {
                        result += currentResult * lastModifier;
                        lastModifier = 1;
                        currentResult = 0;
                    }

                    if ((n = Array.IndexOf(ones, word) + 1) > 0)
                    {
                        currentResult += n;
                    }
                    else if ((n = Array.IndexOf(teens, word) + 1) > 0)
                    {
                        currentResult += n + 10;
                    }
                    else if ((n = Array.IndexOf(tens, word) + 1) > 0)
                    {
                        currentResult += n * 10;
                    }
                    else if (word != "and")
                    {
                        throw new ApplicationException("Unrecognized word: " + word);
                    }
                }
            }

            return result + currentResult * lastModifier;
        }

        #endregion
    }
}