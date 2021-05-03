using System;

namespace WichtelGenerator.Core.Exeptions
{
    public class LotteryFailedExeption : Exception
    {
        public LotteryFailedExeption() : base("An error has occurred during the lottery")
        {
        }

        public LotteryFailedExeption(string message) : base(message)
        {
        }
    }
}