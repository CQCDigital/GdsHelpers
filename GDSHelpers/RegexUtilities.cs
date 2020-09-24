using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace GDSHelpers
{
    public static class RegexUtilities
    {
        public const string Name = "^([ \u00c0-\u01ffa-zA-Z' .-])+$";
        public const string FreeText = @"^[a-zA-Z0-9 --/=':]?[^<>@#?$£%;^*|\\~}{}\]\[]*$";
        public const string PhoneNumber = @"^ *(((\(|\[) *)?((\+ *(4 *){2})|(4 *){2}|((0 *){2}(4 *){2})|(0 *))((\)|\]) *)?((\)|\]) *)?((\(|\[) *)?(((\d *){2}((\)|\]) *)?(\d *){8})|((\d *){3}((\)|\]) *)?(\d *){7})|((\d *){4}((\)|\]) *)?(\d *){5,6})|((\d *){5}((\)|\]) *)?(\d *){4,5}))(((\(|\[) *)?( *Ext *)?(x|\#|\.)? *(\d *){1,6}((\)|\]) *)?)?)? *$";
        #region Phone number regex explanation
        /*CONSTRUCTION
        // Part 1: START: any # of spaces = "^ *("
	        ^ *(                                    //preceded only by spaces

            // Part 2: int'l code: +44|0044|0| = "((\(|\[) *)?((\+ *(4 *){2})|(4 *){2}|((0 *){2}(4 *){2})|(0 *))((\)|\]) *)?"
            ((\(|\[) *)?							//open 0-1 brackets
            (
                (\+ *(4 *){2})						//matches "+44"
	           |(4 *){2}							//matches "44"
	           |((0 *){2}(4 *){2})					//matches "0044"
	           |(0 *)								//matches "0"
	        )
	        ((\)|\]) *)?							//close 0-1 brackets

        // Part 3: number, allowing brackets = "((\)|\]) *)?((\(|\[) *)?(((\d *){2}((\)|\]) *)?(\d *){8})|((\d *){3}((\)|\]) *)?(\d *){7})|((\d *){4}((\)|\]) *)?(\d *){5,6})|((\d *){5}((\)|\]) *)?(\d *){4,5}))"
	        ((\)|\]) *)?((\(|\[) *)?				//close and/or open 0-1 brackets

            (                                       //match 2-5 digit area code, optional bracket
                                                    //then rest of 10-11 digit number
                                                    //i.e. '[area] B) [number]'
                                                    //(\d *){9,10} matches n-m, with any # of spaces

                ((\d*){2}((\)|\]) *)?(\d*){8})	    //2,8
	           |((\d*){3}((\)|\]) *)?(\d*){7})	    //3,7
	           |((\d*){4}((\)|\]) *)?(\d*){5,6})	//4,5 or 4,6
	           |((\d*){5}((\)|\]) *)?(\d*){4,5})	//5,4 or 5,5
	        )

        // Part 4: Extension = "(((\(|\[) *)?( *Ext *)?(x|\#|\.)? *(\d *){1,6}((\)|\]) *)?)?"
        	(                                       //Extension group
                ((\(|\[) *)?						//this may be in brackets        
                ( * Ext *)?(x|\#|\.)? *				//Identifier for extension (0-1 of these)
        		(\d*){1,6}							//1-6 digits, with any spaces
        		((\)|\]) *)?						//close brackets
        	)?										//0-1 instances of extension
        
        // Part 5: end = ")? *$"
        	)? *$									//0-1 instances of phone number followed only by spaces

        //COMBINED (with bracketed groups):
        ^ *(((\(|\[) *)?((\+ *(4 *){2})|(4 *){2}|((0 *){2}(4 *){2})|(0 *))((\)|\]) *)?((\)|\]) *)?((\(|\[) *)?(((\d*){2}((\)|\]) *)?(\d*){8})|((\d*){3}((\)|\]) *)?(\d*)        {7})|((\d*){4}((\)|\]) *)?(\d*){5,6})|((\d*){5}((\)|\]) *)?(\d*){4,5}))(((\(|\[) *)?( * Ext *)?(x|\#|\.)? *(\d *){1,6}((\)|\]) *)?)?)? *$


        //TESTING: these numbers should all work
        //Basic phone number:
            0113 294 8974
        
        //Checking Int'l codes:
            0 113 294 8974
            +44 113 294 8974
            0044 113 294 8974
            44 113 294 8974
        
        //Checking Brackets
            (0113) 294 8974
            (0) 113 294 8974
            (0) (113) 294 8974
        
        //Checking Extensions
            0113 294 8974 Ext. 123
            0113 294 8974 Ext x 123
            0113 294 8974 x123
            0113 294 8974 #123
            0113 294 8974.123
        
        //Checking number lengths
            0 12 12345678
            0 123 1234567
            0 1234 123456
            0 1234 12345
            0 12345 12345
            0 12334 1234

        */
        #endregion
        public const string Password = "(?=.\\d)(?=.[a-z])(?=.*[A-Z]).{8,}";
        public const string Email =
            @"^(?("")("".+?(?<!\\)""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&’'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9A-Za-z][-0-9A-Za-z]*[0-9A-Za-z]*\.)+[a-zA-Z0-9][\-a-zA-Z0-9]{0,22}[a-zA-Z0-9]))$";
        public static bool IsValidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            try
            {
                return Regex.IsMatch(name, Name, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            try
            {
                return Regex.IsMatch(phoneNumber, PhoneNumber, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        public static bool IsValidFreeText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;

            try
            {
                return Regex.IsMatch(text, FreeText, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                    RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,Email, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        public static bool IsValidCustomText(string expression, string text)
        {
            if (string.IsNullOrWhiteSpace(expression) || string.IsNullOrWhiteSpace(text))
                return false;

            try
            {
                return Regex.IsMatch(text, expression, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}
