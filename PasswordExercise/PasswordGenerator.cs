using PasswordExercise;
using PasswordExercise.Models;
using System;
using System.Text;

namespace PasswordExcercise
{
    public class PasswordGenerator : IPasswordGenerator
    {
        delegate string GetCharDelegate(int length);
        static Random rnd = new Random();

        public string GeneratePassword(PasswordRequirements requirements)
        {
            //recieves any validation errors
            string error;

            if (RequirementsAreValid(requirements, out error))
            {
                int passwordLength = requirements.MaxLength;
                StringBuilder passwordBuilder = new StringBuilder();

                //get requested numeric characters
                if (requirements.MinNumericChars > 0)
                {
                    passwordBuilder.Append(GetNumericChars(requirements.MinNumericChars));
                    passwordLength -= requirements.MinNumericChars;
                }

                //get requested special characters
                if (requirements.MinSpecialChars > 0)
                {
                    passwordBuilder.Append(GetSpecialChars(requirements.MinSpecialChars));
                    passwordLength -= requirements.MinSpecialChars;
                }

                //get requested alpha characters in uppercase
                if (requirements.MinUpperAlphaChars > 0)
                {
                    passwordBuilder.Append(GetUpperChars(requirements.MinUpperAlphaChars));
                    passwordLength -= requirements.MinUpperAlphaChars;
                }

                //get requested alpha characters in lowercase
                if (requirements.MinLowerAlphaChars > 0)
                {
                    passwordBuilder.Append(GetLowerChars(requirements.MinLowerAlphaChars));
                    passwordLength -= requirements.MinLowerAlphaChars;
                }

                //get random remaining characters
                if (passwordLength > 0)
                {
                    while (passwordLength > 0)
                    {
                        GetCharDelegate[] getCharDelegate = { GetLowerChars, GetUpperChars, GetSpecialChars, GetNumericChars};
                        
                        passwordBuilder.Append(getCharDelegate[rnd.Next(getCharDelegate.Length)](1));
                        passwordLength--;
                    }
                }
                return passwordBuilder.ToString().Shuffle();
            }

            throw new Exception(string.IsNullOrEmpty(error) ? "Something went wrong!!" : error);
        }

        bool RequirementsAreValid(PasswordRequirements requirements, out string reason)
        {
            reason = "";

            //check requirements object is not null
            if (requirements is null)
            {
                reason = "Password requirements are null.";
                return false;
            }

            //check valid min length
            if (requirements.MinLength > requirements.MaxLength)
            {
                reason = "MinLength cannot be greater than MaxLength.";
                return false;
            }

            //check other parameters are not exceeding max length
            if ((requirements.MinLowerAlphaChars +
                 requirements.MinNumericChars +
                 requirements.MinSpecialChars +
                 requirements.MinUpperAlphaChars)
                 > requirements.MaxLength)
            {
                reason = "Combination of MinLowerAlphaChars, MinNumericChars, MinSpecialChars and MinUpperAlphaChars cannot be greater than MaxLength.";
                return false;
            }

            return true;
        }

        string GetLowerChars(int length)
        {
            const string LOWER = "abcdefghijklmnopqrstuvwxyz";
            string result = GenerateRandomString(LOWER, length);
            return result;
        }

        string GetUpperChars(int length)
        {
            const string UPPER = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string result = GenerateRandomString(UPPER, length);
            return result;
        }

        string GetNumericChars(int length)
        {
            const string NUMBER = "1234567890";
            string result = GenerateRandomString(NUMBER, length);
            return result;
        }

        string GetSpecialChars(int length)
        {
            const string SPECIAL = "!@#$%^&*";
            string result = GenerateRandomString(SPECIAL, length);
            return result;
        }

        string GenerateRandomString(string inputString, int length)
        {
            StringBuilder result = new StringBuilder();

            while (length > 0)
            {
                result.Append(inputString[rnd.Next(inputString.Length)]);
                length--;
            }
            return result.ToString();
        }
    }
}
