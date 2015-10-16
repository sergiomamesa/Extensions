using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    public class TokenHelper
    {
        //public const string LETTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        //public const string DIGITS = "0123456789";

        //private bool IsOnlyLetters = false;
        //private bool IsOnlyDigits = false;

        private const string CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private int DefaultLength = 20;

        public TokenHelper()
        {

        }

        public string Generate()
        {
            var random = new Random();
            var result = String.Concat(Enumerable.Repeat(CHARS, DefaultLength).Select(s => s[random.Next(s.Length)]));

            return result;
        }
    }
}
