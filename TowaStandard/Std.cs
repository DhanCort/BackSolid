/*TASK Std Support for OO Coding Standard*/
using System;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Reflection;

//                                                          //To allow Towa's definition of some types (TimeZoneX, etc.)
//                                                          //      Any reference to System.Globalization type should be
//                                                          //      codes (Ex. System.Globalization.DateTimeStyles).

//                                                          //AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: March 18, 2017.

namespace TowaStandard
{
    //                                                      //Tools to support standard coding C# OO technology.
    //                                                      //Some standards are meet just with coding translation from
    //                                                      //      one OO instance to the other.
    //                                                      //Other standards requires "different" content, this tools
    //                                                      //      are for this purpose.

    //==================================================================================================================
    public static class Std
    {
        /*TASK Std.Generic Generic methods*/
        //==============================================================================================================
        //                                                  //LITERAL

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //DECLARATION AND ASSIGNMENT

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONVERSIONS

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONVERSION TO TEXT

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //RELATIONAL OPERATORS*/

        //--------------------------------------------------------------------------------------------------------------
        public static bool IsBetween<XC>(
            //                                              //bool, true if is in set.

            this XC xc_I,
            XC xcStartInclusive_I,
            XC xcEndInclusive_I
            )
            where XC : IComparable
        {
            return (
                //                                          //Is between
                (xc_I.CompareTo(xcStartInclusive_I) >= 0) && (xc_I.CompareTo(xcEndInclusive_I) <= 0)
                );
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //EQUALITY OPERATORS

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //********************************OTHER OPERATORS

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //TERNARY CONDITIONAL OPERATOR

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //MINIMUM AND MAXIMUM

        //--------------------------------------------------------------------------------------------------------------
        public static XT MinOf<XT>(
            //                                              //Get minimum value.
            //                                              //T, MIN value.

            params XT[] arrxt_I
            )
            where XT : IComparable
        {
            if (
                arrxt_I == null
                )
                Test.Abort(Test.ToLog(arrxt_I, "arrxt_I") + " can not be null");
            if (
                arrxt_I is String[]
                )
                Test.Abort(Test.ToLog(arrxt_I, "arrxt_I") + " can not be String[], " +
                    "use nongeneric method MinOf(arrstr), to get here MinOf<String>(arrstr) was coded");
            if (
                arrxt_I.Length == 0
                )
                Test.Abort(Test.ToLog(arrxt_I, "arrxt_I") + " should have at least 1 item");

            XT tMinOf = arrxt_I[0];

            for (int intI = 1; intI < arrxt_I.Length; intI = intI + 1)
            {
                if (
                    //                                      //a < b (remember, null is less than other object)
                    (arrxt_I[intI] == null) ? true : (arrxt_I[intI].CompareTo(tMinOf) < 0)
                    )
                {
                    tMinOf = arrxt_I[intI];
                }
            }

            return tMinOf;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static XT MaxOf<XT>(
            //                                              //Get maximum value.
            //                                              //T, MAX value.

            params XT[] arrxt_I
            )
            where XT : IComparable
        {
            if (
                arrxt_I == null
                )
                Test.Abort(Test.ToLog(arrxt_I, "arrxt_I") + " can not be null");
            if (
                arrxt_I is String[]
                )
                Test.Abort(Test.ToLog(arrxt_I, "arrxt_I") + " can not be String[], " +
                    "use nongeneric method MaxOf(arrstr), to get here MaxOf<String>(arrstr) was coded");
            if (
                arrxt_I.Length == 0
                )
                Test.Abort(Test.ToLog(arrxt_I, "arrxt_I") + " should have at least 1 item");

            XT tMaxOf = arrxt_I[0];

            for (int intI = 1; intI < arrxt_I.Length; intI = intI + 1)
            {
                if (
                    //                                      //a > b (remember, null is less than other object)
                    (arrxt_I[intI] == null) ? false : (arrxt_I[intI].CompareTo(tMaxOf) > 0)
                    )
                {
                    tMaxOf = arrxt_I[intI];
                }
            }

            return tMaxOf;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //MATHEMATICAL FUNCTIONS

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Std.Integer*/
        //==============================================================================================================
        //                                                  //LITERAL

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //DECLARATION AND ASSIGNMENT

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONVERSIONS

        //--------------------------------------------------------------------------------------------------------------
        public static bool IsInt(
            //                                              //Verify if it within the range of integer.
            //                                              //bool, true = it is

            //                                              //Ej. "123"
            this long long_I
            )
        {
            return (
                (long_I >= Int32.MinValue) && (long_I <= Int32.MaxValue)
                );
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONVERSION TO TEXT

        //--------------------------------------------------------------------------------------------------------------
        /*CONSTANTS*/

        //                                                  //Valid integer (and long integer) picture
        //                                                  //Initializer should order (ascending).
        //                                                  //Sorted array is used only to verify standard pictures.
        public static readonly String[] INTEGER_PICTURES = { "0", "#,##0" };
        private static readonly String[] arrstrINT_PICTURE_SORTED;

        //--------------------------------------------------------------------------------------------------------------
        /*METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public static String ToText(
            //                                              //str, integer to text.

            this int int_I
            )
        {
            return Std.ToText(int_I, "#,##0");
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String ToText(
            //                                              //str, integer to text.

            this int int_I,
            String strPicture_I
            )
        {
            Test.AbortIfNull(strPicture_I, "strPicture_I");
            if (
                !strPicture_I.IsInSortedSet(Std.arrstrINT_PICTURE_SORTED)
                )
                Test.Abort(Test.ToLog(strPicture_I, "strPicture_I") + " is not a valid picture",
                    Test.ToLog(Std.INTEGER_PICTURES, "Std.INTEGER_PICTURES"));

            return int_I.ToString(strPicture_I);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String ToTextBlankWhenZero(
            //                                              //str, integer to text.

            this int int_I,
            String strPicture_I
            )
        {
            return (int_I == 0) ? "" : Std.ToText(int_I, strPicture_I);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool IsParsableToInt(
            //                                              //Verify if it is parsable to integer.
            //                                              //bool, true = it is

            //                                              //Ej. "123"
            this String str_I
            )
        {
            Test.AbortIfNull(str_I, "str:I");

            //                                              //Remove space on both sides and commas (,).
            //                                              //Ex. 1,234,567 => 1234567
            String strClean = str_I.Trim(' ').Replace(",", "");

            int intX;
            bool boolIsParsableToInt = Int32.TryParse(strClean, out intX);

            return boolIsParsableToInt;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static int ParseToInt(
            //                                              //Text to integer
            //                                              //int, value

            //                                              //Ej. "123"
            this String str_I
            )
        {
            if (
                str_I == null
                )
                Test.Abort(Test.ToLog(str_I, "str_I") + " can not be null");

            //                                              //Remove space on both sides and commas (,).
            //                                              //Ex. 1,234,567 => 1234567
            String strClean = str_I.Trim(' ').Replace(",", "");

            int intParseToInt;
            bool boolOk = Int32.TryParse(strClean, out intParseToInt);

            if (!(
                boolOk
                ))
                Test.Abort(Test.ToLog(str_I, "str_I") + " can not be parsed to int",
                    Test.ToLog(strClean, "strClean"), Test.ToLog(boolOk, "boolOk"));

            return intParseToInt;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String ToText(
            //                                              //str, long integer to text.

            this long long_I
            )
        {
            return Std.ToText(long_I, "#,##0");
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String ToText(
            //                                              //str, long integer to text.

            this long long_I,
            String strPicture_I
            )
        {
            Test.AbortIfNull(strPicture_I, "strPicture_I");
            if (
                !strPicture_I.IsInSortedSet(Std.arrstrINT_PICTURE_SORTED)
                )
                Test.Abort(Test.ToLog(strPicture_I, "strPicture_I") + " is not a valid picture",
                    Test.ToLog(Std.INTEGER_PICTURES, "Std.INTEGER_PICTURES"));

            return long_I.ToString(strPicture_I);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String ToTextBlankWhenZero(
            //                                              //str, long integer to text.

            this long long_I,
            String strPicture_I
            )
        {
            return (long_I == 0) ? "" : Std.ToText(long_I, strPicture_I);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool IsParsableToLong(
            //                                              //Verify if it is parsable to long.
            //                                              //bool, true = it is

            //                                              //Ej. "123"
            this String str_I
            )
        {
            if (
                str_I == null
                )
                Test.Abort(Test.ToLog(str_I, "str_I") + " can not be null");

            //                                              //Remove space on both sides and commas (,).
            //                                              //Ex. 1,234,567 => 1234567
            String strClean = str_I.Trim(' ').Replace(",", "");

            long longX;
            bool boolIsParsableToLong = Int64.TryParse(strClean, out longX);

            return boolIsParsableToLong;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static long ParseToLong(
            //                                              //Text to long.
            //                                              //long, value

            //                                              //Ej. "123.456"
            this String str_I
            )
        {
            if (
                str_I == null
                )
                Test.Abort(Test.ToLog(str_I, "str_I") + " can not be null");

            //                                              //Remove space on both sides and commas (,).
            //                                              //Ex. 1,234,567 => 1234567
            String strClean = str_I.Trim(' ').Replace(",", "");

            long longParseToLong;
            bool boolOk = Int64.TryParse(strClean, out longParseToLong);

            if (!(
                boolOk
                ))
                Test.Abort(Test.ToLog(str_I, "str_I") + " can not be parsed to long",
                    Test.ToLog(strClean, "strClean"), Test.ToLog(boolOk, "boolOk"));

            return longParseToLong;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //RELATIONAL OPERATORS*/

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //EQUALITY OPERATORS

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //********************************OTHER OPERATORS

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //TERNARY CONDITIONAL OPERATOR

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //MINIMUM AND MAXIMUM

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //MATHEMATICAL FUNCTIONS

        //--------------------------------------------------------------------------------------------------------------
        public static int Pow(
            //                                              //int, int_I^intPower_I.

            this int int_I,
            int intPower_I
            )
        {
            if (
                intPower_I < 0
                )
                Test.Abort(Test.ToLog(int_I, "int_I") + ", " + Test.ToLog(intPower_I, "intPower_I") +
                    " power should be 0 or positive");

            int intPow = 1;
            for (int intI = 1; intI <= intPower_I; intI = intI + 1)
            {
                intPow = intPow * int_I;
            }

            return intPow;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static long Pow(
            //                                              //long, long_I^intPower_I.

            this long long_I,
            int intPower_I
            )
        {
            if (
                intPower_I < 0
                )
                Test.Abort(Test.ToLog(long_I, "long_I") + ", " + Test.ToLog(intPower_I, "intPower_I") +
                    " power should be 0 or positive");

            long longPow = 1;
            for (int intI = 1; intI <= intPower_I; intI = intI + 1)
            {
                longPow = longPow * long_I;
            }

            return longPow;
        }

        //--------------------------------------------------------------------------------------------------------------
        /*SUPPORT METHODS FOR INITIALIZER*/

        //--------------------------------------------------------------------------------------------------------------
        private static void subVerifyConstantsInteger(
            )
        {
            Test.AbortIfNullOrEmpty(Std.INTEGER_PICTURES, "Std.INTEGER_PICTURES");

            //                                              //Verify it works
            for (int intI = 0; intI < Std.INTEGER_PICTURES.Length; intI = intI + 1)
            {
                try
                {
                    //                                      //int.strText(str) can not be used, initialization should be
                    //                                      //      finished before.
                    String str = 123456789.ToString(Std.INTEGER_PICTURES[intI]);
                }
                catch (Exception sysexceptError)
                {
                    Test.Abort(
                        Test.ToLog(Std.INTEGER_PICTURES[intI], "Std.INTEGER_PICTURES[" + intI + "]") +
                            "can not be a standard picture, it does not work",
                        Test.ToLog(Std.INTEGER_PICTURES, "Std.INTEGER_PICTURES"),
                        Test.ToLog(sysexceptError, "sysexceptError"));
                }

                if (
                    Std.INTEGER_PICTURES[intI].Contains(';')
                    )
                    Test.Abort(
                        Test.ToLog(Std.INTEGER_PICTURES[intI], "Std.INTEGER_PICTURES[" + intI + "]") +
                            "can not contains ';' (picture separator), should include just ONE picture",
                        Test.ToLog(Std.INTEGER_PICTURES, "Std.INTEGER_PICTURES"));
            }

            Test.AbortIfDuplicate(Std.arrstrINT_PICTURE_SORTED, "Std.arrstrINT_PICTURE_SORTED");
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Std.Number*/
        //==============================================================================================================
        //                                                  //LITERALS

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //DECLARATION AND ASSIGNMENT

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ROUND

        //--------------------------------------------------------------------------------------------------------------
        /*CONSTANTS*/

        //                                                  //Factor to be usesed in numRound (form 0 to 15)
        private static double[] arrnum10S_FACTOR = { 1, 10, 100, 1000, 1.0E+4, 1.0E+5, 1.0E+6, 1.0E+7, 1.0E+8, 1.0E+9,
            1.0E+10, 1.0E+11, 1.0E+12, 1.0E+13, 1.0E+14, 1.0E+15 };

        //--------------------------------------------------------------------------------------------------------------
        /*METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public static double Round(
            //                                              //Rounds similar to Excel, but with Bankers Rounding
            //                                              //num, number rounded.

            this double num_I,
            //                                              //Decimals to round (can be negative)
            int intDigits_I
            )
        {
            int intMAX_DIGITS_TO_ROUND = Std.arrnum10S_FACTOR.Length - 1;
            if (!(
                (intDigits_I >= -intMAX_DIGITS_TO_ROUND) && (intDigits_I <= intMAX_DIGITS_TO_ROUND)
                ))
                Test.Abort(Test.ToLog(intDigits_I, "intDigits_I") +
                    " should be in the range " + -intMAX_DIGITS_TO_ROUND + " up to " + intMAX_DIGITS_TO_ROUND);

            double numRound;
            if (
                intDigits_I >= 0
                )
            {
                //                                          //Use .net Math.Round(num, int), int = 0 up to 15.
                numRound = Math.Round(num_I, intDigits_I);
            }
            else
            {
                //                                          //.Net do not accept int negative. Need to move point left.
                double numFactor = Std.arrnum10S_FACTOR[Math.Abs(intDigits_I)];
                numRound = Math.Round(num_I / numFactor) * numFactor;
            }

            return numRound;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONVERSIONS

        //--------------------------------------------------------------------------------------------------------------
        public static bool IsInt(
            //                                              //Verify if it does not have decimal part and is within the
            //                                              //      range of integer.
            //                                              //bool, true = it is

            //                                              //Ej. "123.o"
            this double num_I
            )
        {
            return (
                //                                          //Has no decimal
                (num_I == Math.Truncate(num_I)) &&
                //                                          //In range
                (num_I >= (double)Int32.MinValue) && (num_I <= (double)Int32.MaxValue)
                );
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool IsLong(
            //                                              //Verify if it does not have decimal part and is within the
            //                                              //      range -999,999,999,999,999 and 999,999,999,999,999
            //                                              //Values outside this range can not be converted to long
            //                                              //      integer without loosing some digits.
            //                                              //bool, true = it is

            //                                              //Ej. "123.o"
            this double num_I
            )
        {
            return (
                //                                          //Has no decimal
                (num_I == Math.Truncate(num_I)) &&
                //                                          //In range
                (num_I >= -999999999999999.0) && (num_I <= 999999999999999.0)
                );
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONVERSION TO TEXT

        //--------------------------------------------------------------------------------------------------------------
        /*CONSTANTS*/

        //                                                  //Valid number picture
        //                                                  //Initializer should order (ascending)
        //                                                  //Sorted array is used only to verify standard pictures.
        public static readonly String[] NUMBER_PICTURES = {
            //                                              //Only digits (1 to 7 decimals)
            "0.0", "0.00", "0.000", "0.0000", "0.00000", "0.000000", "0.0000000", 
            //                                              //Include ,'s and digits (1 to 7 decimals)
            "#,##0.0", "#,##0.00", "#,##0.000", "#,##0.0000", "#,##0.00000", "#,##0.000000", "#,##0.0000000", 
            //                                              //Mantissa picture, 1 to 14 decimals
            "0.0E+0", "0.00E+0", "0.000E+0", "0.0000E+0", "0.00000E+0", "0.000000E+0", "0.0000000E+0", "0.00000000E+0",
            "0.000000000E+0", "0.0000000000E+0", "0.00000000000E+0", "0.000000000000E+0", "0.0000000000000E+0",
            "0.00000000000000E+0",
            //                                              //Percentage (1 to 5 decimal, equivalente to up to 7 decimal
            //                                              //      in number)
            "0.0%", "0.00%", "0.000%", "0.0000%", "0.00000%" };
        private static readonly String[] arrstrNUM_PICTURE_SORTED;

        //                                                  //Java display 16 digits but C# just 15.
        //                                                  //Should be in the range 10-15.
        private const int intNUM_MAXIMUM_DIGITS = 15;

        //--------------------------------------------------------------------------------------------------------------
        /*METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public static String ToText(
            //                                              //Text to 15 significant digits.
            //                                              //str, number.

            this double num_I
            )
        {
            return Std.ToText(num_I, Std.intNUM_MAXIMUM_DIGITS);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String ToText(
            //                                              //Text to 2 up to 15 significant digits.
            //                                              //Examples (x = 6):
            //                                              //1,234.56 or -1,234.56
            //                                              //0.000123456 or -0.000123456.
            //                                              //1.23456E+18 or -1.23456E+18.
            //                                              //1.23456E-6 or -1.23456E-6.
            //                                              //str, number.

            this double num_I,
            //                                              //Significant digist to display (2 to max))
            int intDigits_I
            )
        {
            if (!(
                (intDigits_I >= 2) && (intDigits_I <= Std.intNUM_MAXIMUM_DIGITS)
                ))
            {
                Test.Abort(Test.ToLog(intDigits_I, "intDigits_I") + " should be in the range 2-" +
                    Std.intNUM_MAXIMUM_DIGITS);
            }

            //                                              //Get x digits mantissa picture.
            //                                              //Number will be rounded to x meaningful digits
            String strPicture = "0." + "".PadRight(intDigits_I - 1, '0') + "E+0";
            String ToText = Std.ToText(num_I, strPicture);

            //                                              //May not have 'E'
            int intE = ToText.IndexOf('E');

            if (
                intE >= 0
                )
            {
                //                                          //Get exponent.
                String strExponent = ToText.Substring(intE + 1);
                int intExponent = Std.ParseToInt(strExponent);

                if (
                    //                                      //Decimal point can be move up to 4 position left and _____
                    //                                      //      (x - 2) rigth.
                    //                                      //At least 2 digits (0.0) should remain
                    (intExponent >= -4) && (intExponent <= (intDigits_I - 2))
                    )
                {
                    //                                      //Separate Sign and All Digits
                    String strSign;
                    String strAllDigits;
                    if (
                        ToText[0] == '-'
                        )
                    {
                        strSign = "-";
                        strAllDigits = ToText[1] + ToText.Substring(3, intE - 3);
                    }
                    else
                    {
                        strSign = "";
                        strAllDigits = ToText[0] + ToText.Substring(2, intE - 2);
                    }

                    //                                      //Move point left or right (remove 0s).
                    //                                      //At least 2 digits (0.0) should remain
                    ToText = (intExponent < 0) ?
                        "0." + "".PadRight(-intExponent - 1, '0') + strAllDigits.TrimEnd('0') :
                        strAllDigits.Substring(0, 1 + intExponent) + '.' + strAllDigits[1 + intExponent] +
                            strAllDigits.Substring(2 + intExponent).TrimEnd('0');

                    ToText = strSign + ToText;
                }
            }

            return ToText;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String ToText(
            //                                              //Text to picture.
            //                                              //str, number.

            this double num_I,
            String strPicture_I
            )
        {
            Test.AbortIfNullOrEmpty(strPicture_I, "strPicture_I");
            if (
                !strPicture_I.IsInSortedSet(Std.arrstrNUM_PICTURE_SORTED)
                )
                Test.Abort(Test.ToLog(strPicture_I, "strPicture_I") + " is not a valid picture",
                    Test.ToLog(Std.NUMBER_PICTURES, "Std.NUM_PICTURES"));

            String ToText;
            /*CASE*/
            if (
                (num_I == Double.PositiveInfinity) || (num_I == Double.NegativeInfinity) || Std.IsNaN(num_I)
                )
            {
                ToText = (num_I == Double.PositiveInfinity) ? "∞" : (num_I == Double.NegativeInfinity) ? "-∞" : "NaN";
            }
            else if (
                //                                          //Mantissa picture
                strPicture_I.Contains('E')
                )
            {
                ToText = Std.strTextWithE(num_I, strPicture_I);
            }
            else
            {
                //                                          //Normal picture
                ToText = Std.strTextWithoutE(num_I, strPicture_I);
            }
            /*END-CASE*/

            return ToText;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String ToTextBlankWhenZero(
            //                                              //Text to picture.
            //                                              //str, number.

            this double num_I,
            //                                              //Could be normal or mantissa with Ed.., E+d.. or E-d..
            String strPicture_I
            )
        {

            return (
                num_I == 0.0
                ) ? "" : Std.ToText(num_I, strPicture_I);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static String strTextWithE(
            //                                              //str, number.

            double num_I,
            String strPicture_I
            )
        {
            String strTextWithE;
            if (
                //                                      //From 0 up to Max ....E-310
                Math.Abs(num_I) <= 202402253307310.0 * Double.Epsilon
                )
            {
                //                                      //It is a very small number, significat digists should be
                //                                      //      reduced (2 to 15 digits)
                strTextWithE = Std.strTextVerySmall(num_I);
            }
            else
            {
                strTextWithE = num_I.ToString(strPicture_I);
            }

            return strTextWithE;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static String strTextVerySmall(
            //                                              //E-323 and 324 display 1
            //                                              //E-322 displays 2 decimals.
            //                                              //.....
            //                                              //E-312 displays 12 decimals.
            //                                              //E-311 displays 13 decimals.
            //                                              //E-310 displays 14 decimals.
            //                                              //str, Reformated number.

            //                                              //x digits mantissa format. (E-310 to E-324)
            double num_I
            )
        {
            double numAbs = Math.Abs(num_I);
            String strSign = (num_I < 0.0) ? "-" : "";

            String strTextVerySmall;
            if (
                numAbs <= Double.Epsilon * 2.0
                )
            {
                //                                          //Rounding is special
                strTextVerySmall = strSign + ((numAbs == Double.Epsilon) ? "4.9" : "9.8") + "E-324";
            }
            else
            {
                //                                          //Move point left 324 positions, round and get text for
                //                                          //      digits 
                double numNoDecimals = numAbs * 1.0E+300 * 1.0E+24;
                long longNoDecimals = (long)Math.Round(numNoDecimals);
                String strDigits = "" + longNoDecimals;

                //                                          //Form Text, 15==>1.5E-323, 123==>1.23E-322, ....,
                //                                          //      12345678901234==>1.2345678901234E-311,
                //                                          //      123456789012345==>1.23456789012345E-310,
                strTextVerySmall = strSign + strDigits[0] + '.' + strDigits.Substring(1) + "E-" +
                    (325 - strDigits.Length);
            }

            return strTextVerySmall;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static String strTextWithoutE(
            //                                              //str, number.

            double num_I,
            String strPicture_I
            )
        {
            String strTextWithoutE;
            if (
                //                                          //Requires more than 15 integer digits
                Math.Abs(num_I) >= 1.0E+15
                )
            {
                //                                          //Use mantissa with 14 decimals
                strTextWithoutE = num_I.ToString("0.00000000000000E+0");
            }
            else
            {
                strTextWithoutE = num_I.ToString(strPicture_I);
            }

            return strTextWithoutE;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool IsParsableToNum(
            //                                              //Verify has the form:
            //                                              //[+-]d*[.d*][E[+-]d+], at least 1 digit before 'E', intger
            //                                              //      part may include ,'s, shoul remove before verify.
            //                                              //NOTE: (Glg 24Jul2018) in the near future, this method
            //                                              //      should be simplify using "regex" (regex "neutral"),
            //                                              //      we will execute a efficiency test comparing this
            //                                              //      implementation an regex implementation.
            //                                              //bool, true if ok

            this String str_I
            )
        {
            Test.AbortIfNull(str_I, "str_I");

            //                                              //Remove space on both sides.
            String strClean = str_I.Trim(' ');

            bool boolIsParsableToNum;
            if (
                //                                          //Non digits valid texts to produce a num
                (strClean == "Infinity") || (strClean == "+Infinity") || (strClean == "-Infinity") ||
                (strClean == "∞") || (strClean == "+∞") || (strClean == "-∞") || (strClean == "NaN")
                )
            {
                boolIsParsableToNum = true;
            }
            else
            {
                //                                          //Will mark to false
                boolIsParsableToNum = true;

                //                                          //Verify exponent and extract mantissa
                String strMantissa;
                int intE = strClean.IndexOf('E');
                if (
                    intE >= 0
                    )
                {
                    //                                      //Mantissa + Exponent.
                    String strExponent = strClean.Substring(intE + 1);
                    boolIsParsableToNum = Std.boolIsOnlyDigits(strExponent, true);
                    strMantissa = strClean.Substring(0, intE);
                }
                else
                {
                    //                                      //Only Mantissa
                    strMantissa = strClean;
                }

                //                                          //Only if ok, continue with mantissa
                if (
                    boolIsParsableToNum
                    )
                {
                    int intPoint = strMantissa.IndexOf('.');

                    //                                      //Need to include at least 1 digit before or after the
                    //                                      //      point or, if no point, al least 1 digit at the end.

                    if (
                        intPoint >= 0
                        )
                    {
                        boolIsParsableToNum = (
                            //                              //Digit before.
                            ((intPoint - 1) >= 0) && Std.IsDigit(strMantissa[intPoint - 1]) ||
                            //                              //Digit after
                            ((intPoint + 1) < strMantissa.Length) && Std.IsDigit(strMantissa[intPoint + 1])
                            );
                    }
                    else
                    {
                        boolIsParsableToNum = (
                            (strMantissa.Length > 0) && Std.IsDigit(strMantissa[strMantissa.Length - 1])
                            );
                    }

                    //                                  //Verify decimal part
                    if (
                        boolIsParsableToNum && (intPoint >= 0)
                        )
                    {
                        String strDecimals = strMantissa.Substring(intPoint + 1);
                        boolIsParsableToNum = Std.boolIsOnlyDigits(strDecimals, false);
                    }

                    //                                  //Verify integer part
                    if (
                        boolIsParsableToNum
                        )
                    {
                        String strInteger = (intPoint < 0) ? strMantissa : strMantissa.Substring(0, intPoint);

                        //                              //Exclude sign
                        if (
                            (strInteger.Length > 0) &&
                            ((strInteger[0] == '+') || (strInteger[0] == '-'))
                            )
                        {
                            strInteger = strInteger.Substring(1);
                        }

                        //                                  //Integer part (no sign) can have the form: "" (empty), 1
                        //                                  //      digit, digit + optional(digits and ,'s) + digits.
                        boolIsParsableToNum = (strInteger == "") ? true :
                            (strInteger.Length == 1) ? Std.IsDigit(strInteger[0]) :
                            (
                                //                              //Start and End with digit
                                Std.IsDigit(strInteger[0]) && Std.IsDigit(strInteger[strInteger.Length - 1]) &&
                                //                              //In the middle, only digits and ,'s
                                Std.boolIsOnlyDigits(
                                    strInteger.Substring(1, strInteger.Length - 2).Replace(",", ""), false)
                            );
                    }
                }
            }
            return boolIsParsableToNum;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static double ParseToNum(
            //                                              //Text to number.
            //                                              //num, value

            //                                              //Ej. "123.456"
            this String str_I
            )
        {
            Test.AbortIfNull(str_I, "str_I");
            if (
                //                                          //Can not parse
                !Std.IsParsableToNum(str_I)
                )
                Test.Abort(Test.ToLog(str_I, "str_I") + " can not parse to number");

            //                                              //Remove space on both sides
            String strClean = str_I.Trim(' ');

            double numParseToNum;
            /*CASE*/
            if (
                (strClean == "Infinity") || (strClean == "+Infinity") || (strClean == "∞") || (strClean == "+∞")
                )
            {
                numParseToNum = Double.PositiveInfinity;
            }
            else if (
                (strClean == "-Infinity") || (strClean == "-∞")
                )
            {
                numParseToNum = Double.NegativeInfinity;
            }
            else if (
                (strClean == "NaN")
                )
            {
                numParseToNum = Double.NaN;
            }
            else
            {
                int intDecimalPoint = strClean.IndexOf('.');

                //                                              //Remove commas (,) from integer part.
                //                                              //Ex. 1,234,567.123,45 => 1234567.123,45
                strClean = (intDecimalPoint < 0) ? strClean.Replace(",", "") :
                    strClean.Substring(0, intDecimalPoint).Replace(",", "") +
                        strClean.Substring(intDecimalPoint);

                bool boolOk = Double.TryParse(strClean, out numParseToNum);

                //                                          //If not parsable, verify if is very big (∞ or -∞) or very
                //                                          //      small (zero).
                if (
                    //                                      //Not parsable
                    !boolOk
                    )
                {
                    //                                      //It is very big (∞ or -∞) or very small (zero).
                    //                                      //(FOR SURE IT IS)
                    numParseToNum = Std.numParseVeryBigOrVerySmall(strClean);
                }
            }
            /*END-CASE*/

            return numParseToNum;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static bool boolIsOnlyDigits(
            //                                              //+ or - sign is optional.
            //                                              bool, true if Ok

            String str_I,
            bool boolOptionalSign_I
            )
        {
            //                                              //Advance sign
            int intI = ((str_I.Length >= 1) && ((str_I[0] == '+') || (str_I[0] == '-'))) ? 1 : 0;
            /**WHILE*/
            while (
                (intI < str_I.Length) && Std.IsDigit(str_I[intI])
                )
            {
                intI = intI + 1;
            }

            return (
                //                                          //Only digits
                intI >= str_I.Length
                );
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static double numParseVeryBigOrVerySmall(
            //                                              //Text to num, (only ∞ or -∞, 0.0).
            //                                              //∞ or -∞, could be for many integer digits o large 
            //                                              //      exponent, digits + exponent >= +308.
            //                                              //0.0, could be for many zero significants decimals
            //                                              //      0.00.......00ddd or small exponent, zero 
            //                                              //      significants decimal + exponent <= -324.
            //                                              //IT SHOULD BE ONE OF THIS OPTIONS.
            //                                              //num, ∞, -∞ or 0.0 (NaN if could not parse)

            //                                              //This text is "clean", it has the form:
            //                                              //[+-]d*[.d*][E[+-]d+], at least 1 digit before or after
            //                                              //      decimal point.
            String strClean_I
            )
        {
            //                                              //The strategy will be:
            //                                              //a. Separate integers, decimal and exponente.
            //                                              //b. Compute exponent equivanten to integers (Ej 123.
            //                                              //      means +2).
            //                                              //c. Compute exponent equivalent to decimals (Ej. 0.00012
            //                                              //      means -4).

            //                                              //Get exponent and mantissa
            int intE = strClean_I.IndexOf('E');
            long longExponent;
            String strMantissa;
            if (
                intE >= 0
                )
            {
                //                                          //E+ddddd, could be very "long" (Ex. E-123456789023456)
                longExponent = Std.ParseToLong(strClean_I.Substring(intE + 1));
                strMantissa = strClean_I.Substring(0, intE);
            }
            else
            {
                longExponent = 0;
                strMantissa = strClean_I;
            }

            //                                              //Get sign
            bool boolIsPositive;
            if (
                (strMantissa[0] == '+') || (strMantissa[0] == '-')
                )
            {
                boolIsPositive = (
                    strMantissa[0] == '+'
                    );
                strMantissa = strMantissa.Substring(1);
            }
            else
            {
                //                                          //No sign, is positive
                boolIsPositive = true;
            }

            //                                              //Compute mantissa exponent, 1234.ddd (is +), 0.0001234
            //                                              //      (will be negative)
            strMantissa = strMantissa.TrimStart('0');
            int intPoint = strMantissa.IndexOf('.');

            int intMantissaExponent;
            if (
                //                                          //Only decimal parte (.0001234), point in position 0
                intPoint == 0
                )
            {
                //                                          //Compute 0's after point
                String strDecimals = strMantissa.Substring(1);
                String strDecimalsRemovingLeadingZeros = strDecimals.TrimStart('0');

                //                                          //Ej. 0.00012 means -4
                intMantissaExponent = -(1 + strDecimals.Length - strDecimalsRemovingLeadingZeros.Length);
            }
            else
            {
                //                                          //Only integer part (1234), no point, or (1234.567)
                String strInteger = (intPoint < 0) ? strMantissa : strMantissa.Substring(0, intPoint);

                //                                          //Ej 1234 means +3
                intMantissaExponent = strInteger.Length - 1;
            }

            long longTotalExponent = intMantissaExponent + longExponent;

            if (
                (longTotalExponent > -324) && (longTotalExponent < 308)
                )
                Test.Abort("SOMETHING IS WRONG!!!, " + Test.ToLog(longTotalExponent, "longTotalExponent") +
                    " should not be in the range -323 to 307");

            return (longTotalExponent < 0) ? 0.0 : (boolIsPositive) ? Double.PositiveInfinity : Double.NegativeInfinity;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //RELATIONAL OPERATORS*/

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //EQUALITY OPERATORS

        //--------------------------------------------------------------------------------------------------------------
        public static bool IsNaN(
            //                                              //Verify if it is NaN (NOT_A_NUMBER).
            //                                              //By IEEE 754 standard definition.
            //                                              //NOT_A_NUMBER == num is FALSE for all values including
            //                                              //      NOT_A_NUMBER.
            //                                              //A number has 2 posibibilities:
            //                                              //1. Beetwen NEGATIVE_INFINITY and POSITIVE_INFINITY.
            //                                              //2. NOT_A_NUMBER.
            //                                              //bool, true (NaN).

            //                                              //Ej. "123.456" o NaN
            this double num_I
            )
        {
            return (!(
                (num_I >= Double.NegativeInfinity) || (num_I <= Double.PositiveInfinity)
                ));
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //********************************OTHER OPERATORS

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //TERNARY CONDITIONAL OPERATOR

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //MINIMUM AND MAXIMUM

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //MATHEMATICAL FUNCTIONS

        //--------------------------------------------------------------------------------------------------------------
        public static double Pow(
            //                                              //Math.Pow(1.0, ∞) ==> NaN.
            //                                              //Math.Pow(1.0, -∞) ==> NaN.
            //                                              //But it do not work.

            this double num_I,
            double numExponent_I
            )
        {
            double numPow;

            if (
                (num_I == 1.0) &&
                ((numExponent_I == Double.NegativeInfinity) || (numExponent_I == Double.PositiveInfinity))
                )
            {
                numPow = Double.NaN;
            }
            else
            {
                numPow = Math.Pow(num_I, numExponent_I);
            }

            return numPow;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //TRIGONOMETRICAL FUCNTIONS

        //                                                  //***** ESPERAR A VER LA PRUEBA COMPARATIVA PARA DECIDIR

        //--------------------------------------------------------------------------------------------------------------
        /*CONSTANTS*/

        /*
        private const int intROUND_TRIGONOMETRICAL_FUCNTIONS = 6;
        */

        /*
        //--------------------------------------------------------------------------------------------------------------
        private static double numSin(
            double num_I
            )
        {
            return Round(Math.Sin(num_I), intROUND_TRIGONOMETRICAL_FUCNTIONS);
        }

        //--------------------------------------------------------------------------------------------------------------
        private static double numCos(
            double num_I
            )
        {
            return Round(Math.Cos(num_I), intROUND_TRIGONOMETRICAL_FUCNTIONS);
        }

        //--------------------------------------------------------------------------------------------------------------
        private static double numTan(
            double num_I
            )
        {
            return Round(Math.Tan(num_I), intROUND_TRIGONOMETRICAL_FUCNTIONS);
        }

        //--------------------------------------------------------------------------------------------------------------
        private static double numAsin(
            double num_I
            )
        {
            return Round(Math.Asin(num_I), intROUND_TRIGONOMETRICAL_FUCNTIONS);
        }

        //--------------------------------------------------------------------------------------------------------------
        private static double numAcos(
            double num_I
            )
        {
            return Round(Math.Acos(num_I), intROUND_TRIGONOMETRICAL_FUCNTIONS);
        }

        //--------------------------------------------------------------------------------------------------------------
        private static double numAtan(
            double num_I
            )
        {
            return Round(Math.Atan(num_I), intROUND_TRIGONOMETRICAL_FUCNTIONS);
        }
        */

        //--------------------------------------------------------------------------------------------------------------
        /*SUPPORT METHODS FOR INITIALIZER*/

        //--------------------------------------------------------------------------------------------------------------
        private static void subVerifyConstantsNumber(
            )
        {
            Std.subVerifyConstantsNumberRound();
            Std.subVerifyConstantsNumberConversionToText();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subVerifyConstantsNumberRound(
            )
        {
            Test.AbortIfNullOrEmpty(Std.arrnum10S_FACTOR, "Std.arrnum10S_FACTOR");

            for (int intF = 1; intF < Std.arrnum10S_FACTOR.Length; intF = intF + 1)
            {
                //                                      //Verify it is 10 times the previous
                if (
                    Std.arrnum10S_FACTOR[intF] != (10.0 * Std.arrnum10S_FACTOR[intF - 1])
                    )
                    Test.Abort(
                        Test.ToLog(Std.arrnum10S_FACTOR[intF], "Std.arrnum10S_FACTOR[" + intF + "]") +
                            " should be 10 times " +
                            Test.ToLog(Std.arrnum10S_FACTOR[intF - 1], "Std.arrnum10S_FACTOR[" + (intF - 1) + "]"),
                        Test.ToLog(Std.arrnum10S_FACTOR, "Std.arrnum10S_FACTOR"));
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subVerifyConstantsNumberConversionToText(
            )
        {
            Test.AbortIfNullOrEmpty(Std.NUMBER_PICTURES, "Std.NUM_PICTURES");

            //                                              //Verify it works
            for (int intI = 0; intI < Std.NUMBER_PICTURES.Length; intI = intI + 1)
            {
                try
                {
                    //                                      //num.strText(str) can not be used, initialization should be
                    //                                      //      finished before.
                    String str = 12345.6789.ToString(Std.NUMBER_PICTURES[intI]);
                }
                catch (Exception sysexceptError)
                {
                    Test.Abort(
                        Test.ToLog(Std.NUMBER_PICTURES[intI], "Std.NUM_PICTURES[" + intI + "]") +
                            "can not be a standard picture, it does not work",
                        Test.ToLog(Std.NUMBER_PICTURES, "Std.NUM_PICTURES"),
                        Test.ToLog(sysexceptError, "sysexceptError"));
                }

                if (
                    Std.NUMBER_PICTURES[intI].Contains(';')
                    )
                    Test.Abort(
                        Test.ToLog(Std.NUMBER_PICTURES[intI], "Std.NUM_PICTURES[" + intI + "]") +
                            " can not contains ';' (picture separator), should include just ONE picture",
                        Test.ToLog(Std.NUMBER_PICTURES, "Std.NUM_PICTURES"));
            }

            Test.AbortIfDuplicate(Std.arrstrNUM_PICTURE_SORTED, "Std.arrstrNUM_PICTURE_SORTED");

            //                                              //To avoid warning
            int intNUM_MAXIMUM_DIGITS = Std.intNUM_MAXIMUM_DIGITS;

            //                                              //Verify limit
            if (!(
                (intNUM_MAXIMUM_DIGITS >= 10) && (intNUM_MAXIMUM_DIGITS <= 15)
                ))
                Test.Abort(
                    Test.ToLog(Std.intNUM_MAXIMUM_DIGITS, "Std.intNUM_MAXIMUM_DIGITS") +
                        " should be in the range 10-15");
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Std.Character*/
        //==============================================================================================================
        //                                                  //LITERAL

        //--------------------------------------------------------------------------------------------------------------
        /*CONSTANTS*/

        //                                                  //Se incluyen todos los caracteres del mismo tamaño que A
        //                                                  //      (SizeF en font consolas 10 points).
        //                                                  //Para efectos de entendimiento y documentación se agrupan
        //                                                  //      por UccUnicodeCategory (ascencente), también los
        //                                                  //      caracteres están en orden ascendente.
        //                                                  //Nótese que blank no está en este conjunto dado que su
        //                                                  //      SizeF es diferente.
        //                                                  //ES PROBABLE QUE EN OTRO FONT (diferente de consolas) ESTOS
        //                                                  //      CARACTERES SE MUESTREN DISTINTO.
        //                                                  //DADO QUE EL ESTÁNDAR ES USAR consolas PARA VISUALIZAR EL
        //                                                  //      CÓDIGO TANTO EN PANTALLA COMO EN IMPRESORA SE
        //                                                  //      UTILIZO ESTE FONT.
        //                                                  //TODAS ESTAS CONSTANTES PRETENDEN SER UNA AYUDA PARA
        //                                                  //      CODIFICAR Y PROBAR EL CÓDIGO.
        //                                                  //NÓTESE QUE:
        //                                                  //1. Cualquier caracter de x0000-xFFFF puede ser utilizado.
        //                                                  //2. Solo los caracteres en este conjunto y el blank
        //                                                  //      pueden ser visualizado en pantalla o en texto
        //                                                  //3. Solo loc caracteres en este conjunto, el blank y los de
        //                                                  //      escape pueden ser escritos en archivos de texto.
        //                                                  //4. Los métodos LogTo hacen lo necesario para poner mostrar
        //                                                  //      todos los caracteres x0000-xFFFF en font consolas.
        public static readonly T2uccCategoryAndCharsTuple[] arrt2uccCHAR_USEFUL_IN_TEXT = {
            new T2uccCategoryAndCharsTuple(UccUnicodeCategoryEnum.UPPERCASE_LETTER,
                "ABCDEFGHIJKLMNOPQRSTUVWXYZÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖØÙÚÛÜÝÞĀĂĄĆĈĊČĎĐĒĔĖĘĚĜĞĠĢĤĦĨĪĬĮİĲĴĶĹĻĽĿŁŃŅŇŊŌŎŐŒŔŖŘ" +
                "ŚŜŞŠŢŤŦŨŪŬŮŰŲŴŶŸŹŻŽƁƂƄƆƇƉƊƋƎƏƐƑƓƔƖƗƘƜƝƟƠƢƤƦƧƩƬƮƯƱƲƳƵƷƸƼǄǇǊǍǏǑǓǕǗǙǛǞǠǢǤǦǨǪǬǮǱǴǶǷǸǺǼǾȀȂȄȆȈȊȌȎȐȒȔȖȘȚȜȞȠ" +
                "ȢȤȦȨȪȬȮȰȲȺȻȽȾɁɃɄɅɆɈɊɌɎΆΈΉΊΌΎΏΑΒΓΔΕΖΗΘΙΚΛΜΝΞΟΠΡΣΤΥΦΧΨΩΪΫϒϓϔϘϚϜϞϠϢϤϦϨϪϬϮϴϷϹϺϽϾϿЀЁЂЃЄЅІЇЈЉЊЋЌЍЎЏАБВГДЕЖ" +
                "ЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯѠѢѤѦѨѪѬѮѰѲѴѶѸѺѼѾҀҊҌҎҐҒҔҖҘҚҜҞҠҢҤҦҨҪҬҮҰҲҴҶҸҺҼҾӀӁӃӅӇӉӋӍӐӒӔӖӘӚӜӞӠӢӤӦӨӪӬӮӰӲӴӶӸӺӼ" +
                "ӾԀԂԄԆԈԊԌԎԐԒḀḂḄḆḈḊḌḎḐḒḔḖḘḚḜḞḠḢḤḦḨḪḬḮḰḲḴḶḸḺḼḾṀṂṄṆṈṊṌṎṐṒṔṖṘṚṜṞṠṢṤṦṨṪṬṮṰṲṴṶṸṺṼṾẀẂẄẆẈẊẌẎẐẒẔẠẢẤẦẨẪẬẮẰẲẴẶẸẺ" +
                "ẼẾỀỂỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪỬỮỰỲỴỶỸἈἉἊἋἌἍἎἏἘἙἚἛἜἝἨἩἪἫἬἭἮἯἸἹἺἻἼἽἾἿὈὉὊὋὌὍὙὛὝὟὨὩὪὫὬὭὮὯᾸᾹᾺΆῈΈῊΉῘῙῚΊῨῩῪΎῬῸΌῺΏ" +
                "ΩℲↃ"),
            new T2uccCategoryAndCharsTuple(UccUnicodeCategoryEnum.LOWERCASE_LETTER,
                "abcdefghijklmnopqrstuvwxyzªµºßàáâãäåæçèéêëìíîïðñòóôõöøùúûüýþÿāăąćĉċčďđēĕėęěĝğġģĥħĩīĭįıĳĵķĸĺļľŀłńņňŉŋ" +
                "ōŏőœŕŗřśŝşšţťŧũūŭůűųŵŷźżžſƀƃƅƈƌƍƒƕƙƚƛƞơƣƥƨƪƫƭưƴƶƹƺƽƾƿǆǉǌǎǐǒǔǖǘǚǜǝǟǡǣǥǧǩǫǭǯǰǳǵǹǻǽǿȁȃȅȇȉȋȍȏȑȓȕȗșțȝȟȡȣȥ" +
                "ȧȩȫȭȯȱȳȴȵȶȷȸȹȼȿɀɂɇɉɋɍɏɐɑɒɓɔɕɖɗɘəɚɛɜɝɞɟɠɡɢɣɤɥɦɧɨɩɪɫɬɭɮɯɰɱɲɳɴɵɶɷɸɹɺɻɼɽɾɿʀʁʂʃʄʅʆʇʈʉʊʋʌʍʎʏʐʑʒʓʕʖʗʘʙʚʛʜʝʞ" +
                "ʟʠʡʢʣʤʥʦʧʨʩʪʫʬʭʮʯͻͼͽΐάέήίΰαβγδεζηθικλμνξοπρςστυφχψωϊϋόύώϐϑϕϖϗϙϛϝϟϡϣϥϧϩϫϭϯϰϱϲϳϵϸϻϼабвгдежзийклмнопрст" +
                "уфхцчшщъыьэюяѐёђѓєѕіїјљњћќѝўџѡѣѥѧѩѫѭѯѱѳѵѷѹѻѽѿҁҋҍҏґғҕҗҙқҝҟҡңҥҧҩҫҭүұҳҵҷҹһҽҿӂӄӆӈӊӌӎӏӑӓӕӗәӛӝӟӡӣӥӧөӫӭӯӱӳӵ" +
                "ӷӹӻӽӿԁԃԅԇԉԋԍԏԑԓḁḃḅḇḉḋḍḏḑḓḕḗḙḛḝḟḡḣḥḧḩḫḭḯḱḳḵḷḹḻḽḿṁṃṅṇṉṋṍṏṑṓṕṗṙṛṝṟṡṣṥṧṩṫṭṯṱṳṵṷṹṻṽṿẁẃẅẇẉẋẍẏẑẓẕẖẗẘẙẚẛạảấầ" +
                "ẩẫậắằẳẵặẹẻẽếềểễệỉịọỏốồổỗộớờởỡợụủứừửữựỳỵỷỹἀἁἂἃἄἅἆἇἐἑἒἓἔἕἠἡἢἣἤἥἦἧἰἱἲἳἴἵἶἷὀὁὂὃὄὅὐὑὒὓὔὕὖὗὠὡὢὣὤὥὦὧὰάὲέὴήὶ" +
                "ίὸόὺύὼώᾀᾁᾂᾃᾄᾅᾆᾇᾐᾑᾒᾓᾔᾕᾖᾗᾠᾡᾢᾣᾤᾥᾦᾧᾰᾱᾲᾳᾴᾶᾷιῂῃῄῆῇῐῑῒΐῖῗῠῡῢΰῤῥῦῧῲῳῴῶῷℓⅎↄﬁﬂ"),
            new T2uccCategoryAndCharsTuple(UccUnicodeCategoryEnum.TITLECASE_LETTER,
                //                                          //Las siguiente ᾼῌῼ parecen iguales, sin embargo NO LO SON
                //                                          //      las primeras 8 de cada grupo tienen antes arriba un
                //                                          //      pequeño acento que las hace diferentes.
                //                                          //NO SE PORQUE AQUÍ NO SE VE, sin embargo al separarlas como
                //                                          //      char para forma sus t3fake aparecieron los acentos.
                "ǅǈǋǲᾈᾉᾊᾋᾌᾍᾎᾏᾘᾙᾚᾛᾜᾝᾞᾟᾨᾩᾪᾫᾬᾭᾮᾯᾼῌῼ"),
            new T2uccCategoryAndCharsTuple(UccUnicodeCategoryEnum.MODIFIER_LETTER,
                "ʰʱʲʳʴʵʶʷʸʹʺʻʼʽʾʿˀˁˆˇˈˉˊˋˌˍˎˏːˑˠˡˢˣˤˬˮʹͺⁱⁿₐₑₒₓₔ"),
            new T2uccCategoryAndCharsTuple(UccUnicodeCategoryEnum.OTHER_LETTER, "ƻǀǁǂǃʔ"),
            new T2uccCategoryAndCharsTuple(UccUnicodeCategoryEnum.NON_SPACING_MARK,
                //                                          //Se eliminarnos 7 caracteres que parecen tener el mismo
                //                                          //      tamaño que A, sin embargo se muestra hacía arriba.
                ""),
            new T2uccCategoryAndCharsTuple(UccUnicodeCategoryEnum.SPACING_COMBINING_MARK, ""),
            new T2uccCategoryAndCharsTuple(UccUnicodeCategoryEnum.ENCLOSING_MARK, ""),
            new T2uccCategoryAndCharsTuple(UccUnicodeCategoryEnum.DECIMAL_DIGIT_NUMBER, "0123456789"),
            new T2uccCategoryAndCharsTuple(UccUnicodeCategoryEnum.LETTER_NUMBER, ""),
            new T2uccCategoryAndCharsTuple(UccUnicodeCategoryEnum.OTHER_NUMBER,
                "²³¹¼½¾⁰⁴⁵⁶⁷⁸⁹₀₁₂₃₄₅₆₇₈₉⅓⅔⅕⅖⅗⅘⅙⅚⅛⅜⅝⅞①②③④⑤⑥⑦⑧⑨⑩⑪⑫⑬⑭⑮⑯⑰⑱⑲⑳⓫⓬⓭⓮⓯⓰⓱⓲⓳⓴⓿❶❷❸❹❺❻❼❽❾❿"),
            new T2uccCategoryAndCharsTuple(UccUnicodeCategoryEnum.SPACE_SEPARATOR, "             "),
            new T2uccCategoryAndCharsTuple(UccUnicodeCategoryEnum.LINE_SEPARATOR, ""),
            new T2uccCategoryAndCharsTuple(UccUnicodeCategoryEnum.PARAGRAPH_SEPARATOR, ""),
            new T2uccCategoryAndCharsTuple(UccUnicodeCategoryEnum.CONTROL, ""),
            new T2uccCategoryAndCharsTuple(UccUnicodeCategoryEnum.FORMAT, ""),
            new T2uccCategoryAndCharsTuple(UccUnicodeCategoryEnum.SURROGATE, ""),
            new T2uccCategoryAndCharsTuple(UccUnicodeCategoryEnum.PRIVATE_USE, ""),
            new T2uccCategoryAndCharsTuple(UccUnicodeCategoryEnum.CONNECTOR_PUNCTUATION, "_"),
            new T2uccCategoryAndCharsTuple(UccUnicodeCategoryEnum.DASH_PUNCTUATION, "-‐‒–—―"),
            new T2uccCategoryAndCharsTuple(UccUnicodeCategoryEnum.OPEN_PUNCTUATION, "([{‚„⁽₍"),
            new T2uccCategoryAndCharsTuple(UccUnicodeCategoryEnum.CLOSE_PUNCTUATION, ")]}⁾₎"),
            new T2uccCategoryAndCharsTuple(UccUnicodeCategoryEnum.INITIAL_QUOTE_PUNCTUATION, "«‘‛“‟‹"),
            new T2uccCategoryAndCharsTuple(UccUnicodeCategoryEnum.FINAL_QUOTE_PUNCTUATION, "»’”›"),
            new T2uccCategoryAndCharsTuple(UccUnicodeCategoryEnum.OTHER_PUNCTUATION,
                "!\"#%&'*,./:;?@\\¡·¿;·‖‗†‡•…‰′″‴‼‽‾⁃⁞"),
            new T2uccCategoryAndCharsTuple(UccUnicodeCategoryEnum.MATH_SYMBOL,
                "+<=>|~¬±×÷϶⁄⁺⁻⁼₊₋₌←↑→↓↔∂∆∏∑−∕∙√∞∟∩∫≈≠≡≤≥⌠⌡"),
            new T2uccCategoryAndCharsTuple(UccUnicodeCategoryEnum.CURRENCY_SYMBOL, "$¢£¤¥₠₡₢₣₤₥₦₧₨₩₫€₭₮₯₰₱₲₳₴₵₸₹₺"),
            new T2uccCategoryAndCharsTuple(UccUnicodeCategoryEnum.MODIFIER_SYMBOL,
                "^`¨¯´¸˂˃˄˅˒˓˔˕˖˗˘˙˚˛˜˝˟˪˫˭˯˰˱˲˳˴˵˶˷˸˹˺˻˼˽˾˿͵΄΅᾽᾿῀῁῍῎῏῝῞῟῭΅`´῾"),
            new T2uccCategoryAndCharsTuple(UccUnicodeCategoryEnum.OTHER_SYMBOL,
                "¦§©®°¶҂℅№℗™℮⅍↕↨⌂⌐─│┌┐└┘├┤┬┴┼═║╒╓╔╕╖╗╘╙╚╛╜╝╞╟╠╡╢╣╤╥╦╧╨╩╪╫╬▀▄█▌▐■□▪▫▬▲▴▸►▼▾◂◄◊○◌●◘◙◦☺☻☼♀♂♠♣♥♦♪♫✶"),
            new T2uccCategoryAndCharsTuple(UccUnicodeCategoryEnum.OTHER_NOT_ASSIGNED, "₼₽₾"),
            };

        //                                                  //Conjunto de caracteres escapados no visibles.
        //                                                  //Estos caracteres no deben existir en USEFUL y el
        //                                                  //      initializer los debe ordenar.
        public static readonly T2charDescriptionTuple[] arrt2charESCAPE =
        {
            new T2charDescriptionTuple('\0', @"\0 Zero"),
            new T2charDescriptionTuple('\b', @"\b Backspace"),
            new T2charDescriptionTuple('\f', @"\f Formfeed"),
            new T2charDescriptionTuple('\n', @"\n New Line"),
            new T2charDescriptionTuple('\r', @"\r Carriage Return"),
            new T2charDescriptionTuple('\t', @"\t Horizontal Tab"),
        };

        private static readonly String strCHAR_USEFUL_IN_TEXT;

        public static readonly char[] CHARS_USEFUL_IN_TEXT;

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //DECLARATION AND ASSIGNMENT

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONVERSIONS
        //--------------------------------------------------------------------------------------------------------------
        /*SHARED METHODS*/
        //
        //--------------------------------------------------------------------------------------------------------------
        public static bool IsChar(
            //                                              //Checks wether an int can be converted to a char

            this int int_I
            )
        {
            return 0 <= int_I && int_I <= 65535;
        }
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONVERSION TO HEXADECIMAL TEXT

        //--------------------------------------------------------------------------------------------------------------
        public static String ToHexText(
            //                                              //Convert character to hexadecimal.

            //                                              //str, 4 hexadecimal (Ej. 0129).

            this char char_I
            )
        {
            int intChar = (int)char_I;
            String ToHexText = String.Format("{0:X4}", intChar);

            return ToHexText;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //RELATIONAL OPERATORS*/

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //EQUALITY OPERATORS

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //En .net existen las funciones Char.IsDigits,
        //                                                  //      Char.IsLetter y Char.IsLetterOrDigit, estas
        //                                                  //      reconocen como válidos los dígitos y letras en
        //                                                  //      TODOS los lenguajes implementados en .net.
        //                                                  //310 digits y 47,672 letters. By the time is changing

        //                                                  //Aquí se implementas funciones para reconoder dígito (solo
        //                                                  //      0-9), letras (solo A-Z y a-z).

        //--------------------------------------------------------------------------------------------------------------
        public static bool IsDigit(
            //                                              //bool, true if 0-9.
            this char char_I
            )
        {
            return (
                (char_I >= '0') && (char_I <= '9')
                );
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool IsLetter(
            //                                              //bool, true if A-Z o a-z.

            this char char_I
            )
        {
            return (
                (char_I >= 'A') && (char_I <= 'Z') ||
                (char_I >= 'a') && (char_I <= 'z')
                );
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool IsDigitOrLetter(
            //                                              //bool, true if 0-9, A-Z o a-z.

            this char char_I
            )
        {
            return (
                (char_I >= '0') && (char_I <= '9') ||
                (char_I >= 'A') && (char_I <= 'Z') ||
                (char_I >= 'a') && (char_I <= 'z')
                );
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool IsLetterUpper(
            //                                              //bool, true if A-Z.

            this char char_I
            )
        {
            return (
                (char_I >= 'A') && (char_I <= 'Z')
                );
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool IsLetterLower(
            //                                              //bool, true if a-z.

            this char char_I
            )
        {
            return (
                (char_I >= 'a') && (char_I <= 'z')
                );
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool IsDigitOrLetterUpper(
            //                                              //bool, true if 0-9 o A-Z.

            this char char_I
            )
        {
            return (
                (char_I >= '0') && (char_I <= '9') ||
                (char_I >= 'A') && (char_I <= 'Z')
                );
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool IsDigitOrLetterLower(
            //                                              //bool, true if 0-9 o a-z.

            this char char_I
            )
        {
            return (
                (char_I >= '0') && (char_I <= '9') ||
                (char_I >= 'a') && (char_I <= 'z')
                );
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //********************************OTHER OPERATORS

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //TERNARY CONDITIONAL OPERATOR

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //MINIMUM AND MAXIMUM

        //--------------------------------------------------------------------------------------------------------------
        /*SUPPORT METHODS FOR INITIALIZER*/

        //--------------------------------------------------------------------------------------------------------------
        private static void subVerifyConstantsCharacter(
            //                                              //Método de apoyo llamado en constructor estático.
            //                                              //Prepara las constantes para poder utilizarlas.
            //                                              //2. arrt2uccCHAR_USEFUL_IN_TEXT:
            //                                              //2a. Este ordenada por ucc, sin duplicados.
            //                                              //2b. Dentro de cada ucc, este ordenada por la secuencia del
            //                                              //      caracter, sin duplicados
            //                                              //2c. Todos sean <= al caracter xD7FF.
            //                                              //2d. En forma global, no haya caracteres duplicados.

            //                                              //3. arrt3fakecharFAKE:
            //                                              //3a. ordenar.
            //                                              //3b. no duplicados
            //                                              //3c. charFAKE debe estar en USEFUL.
            //                                              //3d. charHEX y charFAKE debe ser el mismo.
            //                                              //3e. strDESCRIPTION "..... ?(u????)", el x???? debe ser la
            //                                              //      correspondiente al caracter ?.
            //                                              //4. arrt2charNONPRINTABLE.
            //                                              //4a. ordenar.
            //                                              //4b. no duplicados
            //                                              //4c. debe estar en USEFUL.
            //                                              //4d. tener descripción.
            //                                              //5. arrt2charESCAPE.
            //                                              //5a. ordenar.
            //                                              //5b. no duplicados
            //                                              //5c. NO debe estar en USEFUL.
            //                                              //5d. tener descripción.
            )
        {
            Std.subPrepareArrt2uccCharUsefulInText();

            Std.subPrepareArrt2charEscape();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static void subPrepareArrt2uccCharUsefulInText(
            //                                              //2. arrt2uccCHAR_USEFUL_IN_TEXT:
            //                                              //2a. Este ordenada por ucc, sin duplicados.
            //                                              //2b. Dentro de cada ucc, este ordenada por la secuencia del
            //                                              //      caracter, sin duplicados
            //                                              //2c. Todos sean <= al caracter xD7FF.
            //                                              //2d. En forma global, no haya caracteres duplicados.
            )
        {
            //                                              //To easy code
            T2uccCategoryAndCharsTuple[] arrt2ucc = Std.arrt2uccCHAR_USEFUL_IN_TEXT;

            //                                              //Verifica secuencia de cada ucc
            for (int intUcc = 1; intUcc < arrt2ucc.Length; intUcc = intUcc + 1)
            {
                if (
                    //                                      //No estan en orden ascendente
                    arrt2ucc[intUcc - 1].ucc >= arrt2ucc[intUcc].ucc
                    )
                    Test.Abort(
                        Test.ToLog(arrt2ucc[intUcc].ucc, "arrt2ucc[" + intUcc + "].ucc") +
                            " should be in ascending order",
                        Test.ToLog(arrt2ucc, "arrt2ucc"));
            }

            //                                              //Verifica chars en cada ucc
            for (int intUcc = 0; intUcc < arrt2ucc.Length; intUcc = intUcc + 1)
            {
                String strChars = arrt2ucc[intUcc].strChars;

                //                                          //Verify ascending sequence
                for (int intC = 1; intC < strChars.Length; intC = intC + 1)
                {
                    if (
                        //                                  //No estan en orden ascendente
                        strChars[intC - 1] >= strChars[intC]
                        )
                        Test.Abort(
                            Test.ToLog(strChars[intC], "arrt2ucc[" + intUcc + "].strChars[" + intC + "]") +
                                " should be in ascending order",
                            Test.ToLog(strChars, "arrt2ucc[" + intUcc + "].strChars"));
                }
            }

            Test.AbortIfDuplicate(Std.CHARS_USEFUL_IN_TEXT, "Std.CHARS_USEFUL_IN_TEXT");
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static void subPrepareArrt2charEscape(
            //                                              //5. arrt2charESCAPE.
            //                                              //5a. ordenar.
            //                                              //5b. no duplicados
            //                                              //5c. NO debe estar en USEFUL.
            //                                              //5d. tener descripción.
            )
        {
            //                                              //Verifica no duplicados
            Test.AbortIfDuplicate(Std.arrt2charESCAPE, "Std.arrt2charESCAPE");

            //                                              //Verifica chars en tupla
            for (int intT2 = 0; intT2 < Std.arrt2charESCAPE.Length; intT2 = intT2 + 1)
            {
                if (
                    Std.arrt2charESCAPE[intT2].charX.IsInSortedSet(Std.CHARS_USEFUL_IN_TEXT)
                    )
                    Test.Abort(
                        Test.ToLog(Std.arrt2charESCAPE[intT2].charX, "Std.arrt2charESCAPE[" + intT2 + "].charX") +
                            " exists in USEFUL_IN_TEXT",
                        Test.ToLog(Std.arrt2charESCAPE[intT2], "Std.arrt2charESCAPE[" + intT2 + "]"),
                        Test.ToLog(Std.arrt2charESCAPE, "Std.arrt2charESCAPE"));

                Test.AbortIfItemIsInSortedSet(Std.arrt2charESCAPE[intT2].charX,
                    "Std.arrt2charESCAPE[" + intT2 + "].charX", Std.CHARS_USEFUL_IN_TEXT, "(Std.arrcharUSEFUL_IN_TEXT");

                Test.AbortIfNullOrEmpty(Std.arrt2charESCAPE[intT2].strDESCRIPTION,
                    "Std.arrt2charESCAPE[intT2].strDESCRIPTION");
            }
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Std.Boolean*/
        //==============================================================================================================
        //                                                  //LITERALS

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //DECLARATION AND ASSIGNMENT

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONVERSION TO TEXT

        //--------------------------------------------------------------------------------------------------------------
        public static String ToText(
            //                                              //str, boolean to text ("true" or "false").

            this bool bool_I
            )
        {
            return (bool_I) ? "true" : "false";
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool IsParsableToBool(
            //                                              //Tries to parse a String to determine if its a bool
            //                                              //      primitive.
            //                                              //bool, true it represente  a boolean

            this String str_I
            )
        {
            String strClean = str_I.Trim(' ').ToLower();

            return (
                (strClean == "true") || (strClean == "false")
                );
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool ParseToBool(
            //                                              //Parses a valid String to determine what boolean it is.
            //                                              //bool, true if represent "true", false it "false"

            //                                              //String to parse
            this String str_I
            )
        {
            String strClean = str_I.Trim(' ').ToLower();

            if (!(
                //                                          //Is parsable
                (strClean == "true") || (strClean == "false")
                ))
                Test.Abort(Test.ToLog(str_I, "str_I") + " is not parsable to boolean");

            return (
                strClean == "true"
                );
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //UNARY OPERATOR*/

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //EQUALITY OPERATORS

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //********************************OTHER OPERATORS

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Std.Currency*/
        //==============================================================================================================
        //                                                  //LITERAL

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //DECLARATION AND ASSIGNMENT

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONVERSIONS

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONVERSION TO TEXT

        //--------------------------------------------------------------------------------------------------------------
        public static Currency TotalCentsToCurr(

            this long longTotalCents_I
            )
        {
            return Currency.Literal(longTotalCents_I / 100, (int)(longTotalCents_I % 100));
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool IsCurr(

            this double num_I
            )
        {

            return (
                //                                          //In range
                (num_I.IsBetween(-9999999999999.99, 9999999999999.99)) &&
                //                                          //should have at most 2 decimals
                !(num_I != num_I.Round(2))
                );
        }

        //--------------------------------------------------------------------------------------------------------------
        public static Currency ToCurr(

            this double num_I
            )
        {
            return Currency.Literal(num_I);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool IsCurr(
            //                                              //Without cents
            this long long_I
            )
        {
            long longMin = Int64.MinValue / 100;
            long longMax = Int64.MaxValue / 100;

            return (
                //                                          //In range
                (long_I.IsBetween(longMin, longMax))
                );
        }

        //--------------------------------------------------------------------------------------------------------------
        public static Currency ToCurr(

            this long long_I
            )
        {
            return Currency.Literal(long_I);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static Currency ToCurr(

            this int int_I
            )
        {
            return (Currency.Literal(int_I));
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool IsParsableToCurrency(
            //                                              //Verify if it is parsable to Currency.

            this String str_I
            )
        {
            Test.AbortIfNullOrEmpty(str_I, "str_I");

            //                                              //Could have the form ddddd or ddddd.cc
            String[] arrstrParts = str_I.Split('.');
            String strCents = (arrstrParts.Length == 2) ? arrstrParts[1] : "00";
            String strTotalCents = arrstrParts[0] + strCents;

            return (
                (arrstrParts.Length <= 2) &&
                (strCents.Length == 2) &&
                strTotalCents.IsParsableToLong()
                );
        }

        //--------------------------------------------------------------------------------------------------------------
        public static Currency ParseToCurrency(
            //                                              //curr, string to currency

            this String str_I
            )
        {
            Test.AbortIfNullOrEmpty(str_I, "str_I");

            if (
                Currency.IsParsableToCurrency(str_I)
                )
                Test.Abort(Test.ToLog(str_I, "str_I") + " could not be parsed to currency");

            //                                              //Have the form ddddd or ddddd.cc
            int intPoint = str_I.IndexOf('.');
            String strTotalCents = (intPoint < 0) ? str_I + "00" :
                str_I.Substring(0, intPoint) + str_I.Substring(intPoint + 1);

            long longTotalCents = strTotalCents.ParseToLong();

            return Currency.z_TowaPRIVATE_subConstructCurrency(longTotalCents);
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //UNARY OPERATOR*/

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //EQUALITY OPERATORS

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //********************************OTHER OPERATORS

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Std.String*/
        //==============================================================================================================
        //                                                  //LITERALS

        public const String strMIN = "\x0000";

        public const String strMAX = "\xFFFF";

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //RELATIONAL OPERATORS*/

        //--------------------------------------------------------------------------------------------------------------
        public static int CompareToIgnoreCase(
            //                                              //int, -1, 0 or 1.

            this String Left_I,
            String Right_I
            )
        {
            Test.AbortIfNull(Left_I, "Left_I");
            Test.AbortIfNull(Right_I, "Right_I");

            return String.CompareOrdinal(Left_I.ToLower(), Right_I.ToLower());
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //MINIMUM AND MAXIMUM

        //--------------------------------------------------------------------------------------------------------------
        public static String MinOf(
            //                                              //Get minimum value (bitwise).
            //                                              //To get bitwise, C# requires String.CompareOrdinal.
            //                                              //str, MIN value ("A" es antes que "A ").

            params String[] arrstr_I
            )
        {
            if (
                arrstr_I == null
                )
                Test.Abort(Test.ToLog(arrstr_I, "arrstr_I") + " can not be null");
            if (
                arrstr_I.Length == 0
                )
                Test.Abort(Test.ToLog(arrstr_I, "arrstr_I") + " should have at least 1 item");

            String strMinOf = arrstr_I[0];

            for (int intI = 1; intI < arrstr_I.Length; intI = intI + 1)
            {
                if (
                    //                                      //a < b
                    String.CompareOrdinal(arrstr_I[intI], strMinOf) < 0
                    )
                {
                    strMinOf = arrstr_I[intI];
                }
            }

            return strMinOf;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String MaxOf(
            //                                              //Get maximum value (bitwise).
            //                                              //To get bitwise, C# requires String.CompareOrdinal.
            //                                              //str, MAX value.

            params String[] arrstr_I
            )
        {
            if (
                arrstr_I == null
                )
                Test.Abort(Test.ToLog(arrstr_I, "arrstr_I") + " can not be null");
            if (
                arrstr_I.Length == 0
                )
                Test.Abort(Test.ToLog(arrstr_I, "arrstr_I") + " should have at least 1 item");

            String strMaxOf = arrstr_I[0];

            for (int intI = 1; intI < arrstr_I.Length; intI = intI + 1)
            {
                if (
                    //                                      //a > b
                    String.CompareOrdinal(arrstr_I[intI], strMaxOf) > 0
                    )
                {
                    strMaxOf = arrstr_I[intI];
                }
            }

            return strMaxOf;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static int SearchWord(
            //                                              //Busca una "palabra" en un String, una "palabra" es un 
            //                                              //      conjunto de caracteres (diferentes de espacios) 
            //                                              //      DELIMITED por el inicio o fin del String o por uno
            //                                              //      o varios espacios. Ej. en el String "__ABC___ZYZ" se
            //                                              //      tiene las palabras "ABC" y XYZ" (ojo en el String se
            //                                              //      uso _ como substituto de espacio).

            //                                              //int, posición (base 0) donde encuentra la palabra, -1 si
            //                                              //      no encontró.

            //                                              //String sobre el cual se buscará la palabra.
            this String str_I,
            //                                              //Palabra que se buscará. (Deben ser caracteres continuos 
            //                                              //      sin espacios).
            String strWord_I
            )
        {
            //                                              //Inicializa resultado a NO ENCONTRO
            int intSearchWord = -1;

            int intIni = 0;

            /*LOOP*/
            while (true)
            {
                //                                          //Se cicla para buscar el incial de las siguiente palabra en
                //                                          //      String, termina cuando:
                /*UNTIL-DO*/
                while (!(
                    //                                      //Llego al fin del String.
                    (intIni >= str_I.Length) ||
                    //                                      //Encuentra el inicio de una palabra
                    (str_I[intIni] != ' ')
                    ))
                {
                    intIni = intIni + 1;
                }

                /*EXIT-IF*/
                if (
                    //                                      //En el ciclo anterior encontro la palabra
                    (intSearchWord >= 0) ||
                    //                                      //LLego al fin del String.
                    (intIni >= str_I.Length)
                    )
                    break;

                //                                          //Se cicla para buscar el fin de la palabra que inicia en
                //                                          //      intIni.
                int intFin = intIni;
                /*UNTIL-DO*/
                while (!(
                    //                                      //Llego al fin del String
                    (intFin >= str_I.Length) ||
                    //                                      //Encontró el fin de la palabra
                    (str_I[intFin] == ' ')
                    ))
                {
                    intFin = intFin + 1;
                }

                if (
                    str_I.Substring(intIni, intFin - intIni) == strWord_I
                    )
                {
                    //                                      //Pasa la posición de la palabra 
                    intSearchWord = intIni;
                }

                intIni = intFin + 1;
            }
            /*END-LOOP*/

            return intSearchWord;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String TrimExcel(
            //                                              //Hace un Trim similar al lo que hace Excel, esto es, 
            //                                              //      elimina los espacios al principio y al final y solo 
            //                                              //      deja un espacio entre las palabras que contenga, una
            //                                              //      palabra es un conjunto de caracteres contiguos
            //                                              //      diferentes a espacio.

            //                                              //str, ya sin espacios en exceso (Trim EXCEL).

            //                                              //String para hacer el Trim EXCEL
            this String str_I
            )
        {
            //                                              //Se cicla para buscar el inicio de la primera palabra, sale
            //                                              //      cuando
            int intIni = 0;
            /*UNTIL-DO*/
            while (!(
                //                                          //Llego al fin del String
                (intIni >= str_I.Length) ||
                //                                          //Encuentra caracter diferente de espacio
                (str_I[intIni] != ' ')
                ))
            {
                intIni = intIni + 1;
            }

            String strTrimExcel = "";

            //                                              //Se cicla para procesar cada palabra
            /*LOOP*/
            while (true)
            {
                //                                          //Extrae la siguiente palabra del String
                String strWord;
                Std.subWord(str_I, ref intIni, out strWord);

                //                                          //Concatena la palabra
                strTrimExcel = strTrimExcel + strWord;

                /*EXIT-IF*/
                if (
                    //                                      //sale cuando llega al fin del String
                    intIni >= str_I.Length
                    )
                    break;

                strTrimExcel = strTrimExcel + " ";
            }
            /*END-LOOP*/

            return strTrimExcel;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subWord(                        //Procesa una palabra del String.

            //                                              //String que contiene las palabras.
            String str_I,
            //                                              //Posición donde inicia la palabra que se procesará, regresa
            //                                              //      la posición del inicio de la siguiente palabra, o la
            //                                              //      posición inmediata al fin del String.
            ref int intI_IO,
            //                                              //Palabra procesada.
            out String strWord_O
            )
        {
            //                                              //Se cicla buscando el fin de la palabra, sale cuando;
            int intFin = intI_IO;
            /*UNTIL-DO*/
            while (!(
                //                                          //Llega al fin del String
                (intFin >= str_I.Length) ||
                //                                          //Encontró un espacio (fin de palabra)
                (str_I[intFin] == ' ')
                ))
            {
                intFin = intFin + 1;
            }

            strWord_O = str_I.Substring(intI_IO, intFin - intI_IO);

            //                                              //Se cicla buscando el inicio de la siguiente palabra, hasta
            intI_IO = intFin;
            /*UNTIL-DO*/
            while (!(
                //                                          //Llega al fin del String
                (intI_IO >= str_I.Length) ||
                //                                          //Encontró el inicio de la siguiente palabra
                (str_I[intI_IO] != ' ')
                ))
            {
                intI_IO = intI_IO + 1;
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String Center(
            //                                              //Centra el texto y lo edita.
            //                                              //Si el filler es impar, el relleno del lado derecho será un
            //                                              //      caracter de relleno un caracter mayor al de la
            //                                              //      izquierda.

            //                                              //str, texto centrado conforme a los parámetros, si excede
            //                                              //      la longitud deseada se trunca y se deja como último
            //                                              //      caracter '…'.

            //                                              //Texto que debe ser alineado.
            this String strText_I,
            //                                              //Longitud del texto nuevo que se debe producir.
            int intLength_I,
            //                                              //Caracter para relleno a la izquierda.
            char charLeft_I,
            //                                              //Caracter para relleno a la derecha.
            char charRight_I
            )
        {
            //                                              //Para formar el nuevo String.
            String strCenter = strText_I;

            //                                              //Si excede el tamaño deseado lo trunca del lado derecho.
            if (
                //                                          //Excede el tamaño
                strText_I.Length > intLength_I
                )
            {
                //                                          //Corta la parte excedente.
                strCenter = strCenter.Substring(0, intLength_I - 1) + '…';
            }

            //                                              //Calcula la cantidad de caracteres de inicio y fin.
            int intFiller = intLength_I - strCenter.Length;

            //                                              //Si el valor en impar lo redondea hacia abajo.
            int intLeft = intFiller / 2;
            int intRigth = intFiller - intLeft;

            //                                              //Genera el texto con los inicio y fin y el texto alineado.
            //                                              //Nótese que es indistinto usar PadLeft o PadRight
            strCenter = "".PadLeft(intLeft, charLeft_I) + strCenter + "".PadLeft(intRigth, charRight_I);

            return strCenter;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String PluralS(
            //                                              //Text "s" for plural.
            //                                              //str, "s" or "".

            this int int_I
            )
        {
            return (int_I == 1) ? "" : "s";
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String PluralEs(
            //                                              //Text "es" for plural.
            //                                              //str, "es" or "".

            this int int_I
            )
        {
            return (int_I == 1) ? "" : "es";
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool Contains(

            this String str_I,
            char charToSearch_I
            )
        {
            Test.AbortIfNull(str_I, "str_I");
            Test.AbortIfNull(charToSearch_I, "charToSearch_I");

            int intI = 0;
            /*UNTIL-DO*/
            while (!(
                (intI >= str_I.Length) ||
                (str_I[intI] == charToSearch_I)
              ))
            {
                intI = intI + 1;
            }

            return (
                    intI < str_I.Length
                ) ? true : false;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool ContainsOrdinal(

            this String str_I,
            String strToSearch_I
            )
        {
            Test.AbortIfNull(str_I, "str_I");
            Test.AbortIfNull(strToSearch_I, "charToSearch_I");
            
            //                                              //En NetCore si se puedo codificar esto.
            return str_I.Contains(strToSearch_I/*, StringComparison.Ordinal*/);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool StartsWithOrdinal(

            this String str_I,
            String strToSearch_I
            )
        {
            Test.AbortIfNull(str_I, "str_I");
            Test.AbortIfNull(strToSearch_I, "charToSearch_I");

            return str_I.StartsWith(strToSearch_I, StringComparison.Ordinal);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool EndsWithOrdinal(

            this String str_I,
            String strToSearch_I
            )
        {
            Test.AbortIfNull(str_I, "str_I");
            Test.AbortIfNull(strToSearch_I, "charToSearch_I");

            return str_I.EndsWith(strToSearch_I, StringComparison.Ordinal);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static int IndexOfOrdinal(

            this String str_I,
            String strToSearch_I
            )
        {
            Test.AbortIfNull(str_I, "str_I");
            Test.AbortIfNull(strToSearch_I, "charToSearch_I");

            return str_I.IndexOf(strToSearch_I, StringComparison.Ordinal);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static int IndexOfOrdinal(

            this String str_I,
            String strToSearch_I,
            int from_I
            )
        {
            Test.AbortIfNull(str_I, "str_I");
            Test.AbortIfNull(strToSearch_I, "charToSearch_I");

            return str_I.IndexOf(strToSearch_I, from_I, StringComparison.Ordinal);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static int LastIndexOfOrdinal(

            this String str_I,
            String strToSearch_I
            )
        {
            Test.AbortIfNull(str_I, "str_I");
            Test.AbortIfNull(strToSearch_I, "charToSearch_I");

            return str_I.LastIndexOf(strToSearch_I, StringComparison.Ordinal);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static int LastIndexOfOrdinal(

            this String str_I,
            String strToSearch_I,
            int from_I
            )
        {
            Test.AbortIfNull(str_I, "str_I");
            Test.AbortIfNull(strToSearch_I, "charToSearch_I");

            return str_I.LastIndexOf(strToSearch_I, from_I, StringComparison.Ordinal);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String[] Split(

            //                                              //arrstr, parts.

            this String str_I,
            //                                              //One or more separators.
            params String[] strSeparator_I
            )
        {
            return str_I.Split(strSeparator_I, StringSplitOptions.None);
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Std.Type*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //LITERALS

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //DECLARATION AND ASSIGNMENT

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //EQUALITY OPERATORS

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS

        //--------------------------------------------------------------------------------------------------------------
        public static String Name(
            //                                              //Neutralize type name.
            //                                              //str, standard name.

            this Type type_I
            )
        {
            String strName;

            /*CASE*/
            if (
                type_I.IsArray
                )
            {
                int intRank = type_I.GetArrayRank();
                String strRank = (intRank == 1) ? "[]" : (intRank == 2) ? "[,]" : "[,,]";

                //                                          //Ex. Object[,,]
                strName = type_I.GetElementType().Name() + strRank;
            }
            else if (
                type_I.IsGenericType
                )
            {
                //                                          //Name without generic paramenter count ("List`1" ==> "List")
                strName = type_I.Name;
                int intApostrophe = strName.LastIndexOf('`');
                strName = strName.Substring(0, intApostrophe);

                //                                          //Convert some names to standard generic name
                strName = (strName == "List") ? "DynamicArray" : strName;

                strName = strName + "<>";
            }
            else
            {
                strName = type_I.Name;

                //                                          //Convert some names to standard name
                strName = (strName == "Int32") ? "int" :
                    (strName == "Int64") ? "long" :
                    (strName == "Double") ? "number" :
                    (strName == "Char") ? "char" :
                    (strName == "Boolean") ? "bool" :
                    (strName == "RuntimeType") ? "Type" :
                    (strName == "TimeZoneX") ? "TimeZone" :
                    (strName == "PathX") ? "Path" :
                    (strName == "DirectoryInfo") ? "Directory" :
                    (strName == "FileInfo") ? "File" :
                    (strName == "StreamReader") ? "TextReader" :
                    (strName == "StreamWriter") ? "TextWriter" :
                    strName;
            }
            /*END-CASE*/

            return strName;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String ApplicationName(
            //                                              //Only for user defined type.
            //                                              //str, Application Name (Ex. Softwara Automation)

            //                                              //corresponding to a btuple or bclass
            this Type type_I
            )
        {
            if (!(
                typeof(BobjAbstract).IsAssignableFrom(type_I)
                ))
                Test.Abort(type_I.Name() + " should be a user defined type",
                    Test.ToLog(type_I, "type_I"), Test.ToLog(typeof(BobjAbstract), "typeof(BobjBaseObjectAbstract)"));

            String strFullName = type_I.FullName;
            int intFirstDot = strFullName.IndexOf('.');

            return strFullName.Substring(0, intFirstDot);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String RelevantPartName(
            //                                              //Only for user defined type.
            //                                              //str, Relevant Part Name (Ex. Cod)

            //                                              //corresponding to a btuple or bclass
            this Type type_I
            )
        {
            if (!(
                typeof(BobjAbstract).IsAssignableFrom(type_I)
                ))
                Test.Abort(type_I.Name() + " should be a user defined type",
                    Test.ToLog(type_I, "type_I"),
                    Test.ToLog(typeof(BobjAbstract), "typeof(BobjBaseObjectAbstract)"));

            String strFullName = type_I.FullName;
            int intFirstDot = strFullName.IndexOf('.');
            int intSecondDot = strFullName.IndexOf('.', intFirstDot + 1);

            return strFullName.Substring(intFirstDot + 1, intSecondDot - (intFirstDot + 1));
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Std.Regex*/
        //==============================================================================================================
        //                                                  //LITERALS

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //DECLARATION AND ASSIGNMENT

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONVERSION TO TEXT

        //--------------------------------------------------------------------------------------------------------------
        /*CONSTANTS*/

        //--------------------------------------------------------------------------------------------------------------
        /*METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public static Regex ParseToRegex(

            //                                              //Form: days.date or date
            this String string_I
            )
        {
            Test.AbortIfNullOrEmpty(string_I, "string_I");

            if (!(
                //                                          //Anchor character OK
                (string_I[0] == '^') && (string_I[string_I.Length - 1] == '$')
                ))
                Test.Abort(Test.ToLog(string_I, "string_I") +
                    " anchor character is missing, form should be: ^{pattern}$");

            Regex regexX;
            try
            {
                regexX = new Regex(string_I, RegexOptions.Compiled);
            }
            catch (Exception sysexcepRegex)
            {
                Test.Abort("\"new Regex(string_I, RegexOptions.Compiled);\" fail",
                    Test.ToLog(string_I, "string_I"), Test.ToLog(sysexcepRegex, "sysexcepRegex"));

                regexX = null;
            }

            return regexX;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //UNARY OPERATOR*/

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //EQUALITY OPERATORS

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //********************************OTHER OPERATORS

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Std.Date*/
        //==============================================================================================================
        //                                                  //LITERALS

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //DECLARATION AND ASSIGNMENT

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONVERSION TO TEXT

        //--------------------------------------------------------------------------------------------------------------
        /*CONSTANTS*/

        //--------------------------------------------------------------------------------------------------------------
        /*METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public static bool IsParsableToDate(
            //                                              //Verify if it is parsable to Date.

            this String str_I
            )
        {
            Test.AbortIfNullOrEmpty(str_I, "str_I");

            bool boolIsParsableToDate;
            DateTime dt;
            Std.subTryParseDt(out boolIsParsableToDate, out dt, str_I);

            return boolIsParsableToDate;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static Date ParseToDate(

            //                                              //Form: days.date or date
            this String str_I
            )
        {
            Test.AbortIfNull(str_I, "str_I");

            bool boolIsParsableToDate;
            DateTime dtParsed;
            Std.subTryParseDt(out boolIsParsableToDate, out dtParsed, str_I);

            if (
                !boolIsParsableToDate
                )
                Test.Abort(Test.ToLog(str_I, "str_I") + " could not be parsed to date",
                    Test.ToLog(Language.ENGLISH, "arrlanguages_L"),
                    Test.ToLog(boolIsParsableToDate, "boolIsParsableToDate"),
                    "dtParsed(" + dtParsed.ToString("yyyy-MM-ddTHH:mm:ss.fffZZZ") + ")");

            return new Date((int)(dtParsed.Ticks / TimeSpan.TicksPerDay));
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subTryParseDt(
            //                                              //Verify if it is parsable to Date.
            out bool boolIsParsable_O,
            out DateTime dtParsed_O,
            String str_I
            )
        {
            //                                              //Clean a verify pattern
            String strClean = str_I.Trim(' ');

            int intI = 0;

            do
            {
                boolIsParsable_O = DateTime.TryParseExact(strClean, Date.PICTURES[intI],
                    Language.ENGLISH.culture, System.Globalization.DateTimeStyles.None, out dtParsed_O);

                intI = intI + 1;
            }
            /*REPEAT-WHILE*/
            while (
                (intI < Date.PICTURES.Length) &&
                (boolIsParsable_O == false)
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //UNARY OPERATOR*/

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //EQUALITY OPERATORS

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //********************************OTHER OPERATORS

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Std.Time*/
        //==============================================================================================================
        //                                                  //LITERALS

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //DECLARATION AND ASSIGNMENT

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONVERSION TO TEXT

        //--------------------------------------------------------------------------------------------------------------
        /*CONSTANTS*/

        //                                                  //Valid time picture
        //                                                  //Initializer should order (ascending)
        //                                                  //Sorted array is used only to verify standard pictures.
        private static readonly String[] arrstrTIME_PICTURES = {
            //                                              //24-hour clock
            "HH:mm:ss", "HH:mm", "HHmmss", "HHmm",
            //                                              //12-hour clock (space and am/pm)
            "hh:mm:ss tt", "hh:mm tt", "h:mm:ss tt", "h:mm tt",
            //                                              //12-hour clock (no space)
            "hh:mm:sstt", "hh:mmtt", "h:mm:sstt", "h:mmtt",
            //                                              //12-hour clock (a/m or A/P).
            //                                              //In some lenguages the character should be the second.
            "hh:mm:ss t", "hh:mm t", "h:mm:ss t", "h:mm t",
            "hh:mm:sst", "hh:mmt", "h:mm:sst", "h:mmt",
            //                                              //In some other languages there is not AM/PM, in these
            //                                              //      languages 24-hour clock "HH:mm:ss" or "HH:mm" should
            //                                              //      be used instead
            };

        private static readonly String[] arrstrTIME_PICTURE_SORTED;

        //                                                  //(opcional days.)horas:minutos(opcional :segundos)
        //                                                  //      (opcional [ ]am/pm)
        private static readonly Regex regexTIME_PATTERN =
            new Regex(@"^(\d{1,4}.)?\d{1,2}(:\d{1,2}){1,2}(\s?[a-zA-Z]{1,2})?$");

        //--------------------------------------------------------------------------------------------------------------
        /*METHODS*/

        //-------------------------------------------------------------------------------------------------------------
        public static String ToText(
            this Time Time_I
            )
        {
            return Time_I.ToText("HH:mm:ss");
        }

        //-------------------------------------------------------------------------------------------------------------
        public static String ToText(
            this Time Time_I,
            String Picture_I
            )
        {
            return Time_I.ToText(Picture_I, Language.ENGLISH);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String ToText(
            //                                              //Convert to the specify picture.
            //                                              //Options:
            //                                              //00:00:00-23:59:59 use the specify picture.
            //                                              //Negative or Aditional Days use ToString() removing seconds
            //                                              //      of not present in picture.

            this Time Time_I,
            String Picture_I,
            Language language_I
            )
        {
            Test.AbortIfNull(Picture_I, "Pictures_I");
            Test.AbortIfNull(language_I, "language_I");

            if (
                !Picture_I.IsInSortedSet(Std.arrstrTIME_PICTURE_SORTED)
                )
                Test.Abort(Test.ToLog(Picture_I, "Picture_I") + " is not a valid picture",
                    Test.ToLog(Std.arrstrTIME_PICTURES, "Std.TIME_PICTURES"));

            return Time_I.ToString(Picture_I, language_I.culture);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool IsParsableToTime(
            this String str_I
            )
        {
            return Std.IsParsableToTime(str_I, Language.ENGLISH);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static bool IsParsableToTime(
            //                                              //Verify if it is parsable to Time.

            //                                              //Form: days.time or time
            this String str_I,
            params Language[] arrlanguages_L
            )
        {
            Test.AbortIfNull(str_I, "str_I");
            Test.AbortIfNullOrEmpty(arrlanguages_L, "arrlanguages_L");

            //                                              //Clean a verify pattern
            String strClean = str_I.Trim(' ');

            //                                              //Get negative hours
            String strNegativeHours = null;

            int intOpenParenthesis = str_I.IndexOf('(');
            int intCloseParenthesis = str_I.IndexOf(')');
            if (
                intOpenParenthesis >= 0 &&
                intCloseParenthesis > intOpenParenthesis
                )
            {
                strNegativeHours = strClean.Substring(intOpenParenthesis + 1,
                                       intCloseParenthesis - intOpenParenthesis - 1);

                //                                          //Negative hours are temporarily substitute by 00: because
                //                                          //      is what regex accepts.
                strClean = strClean.Substring(0, intOpenParenthesis) + "00" +
                           strClean.Substring(intCloseParenthesis + 1);
            }

            bool boolIsParsableToTime = regexTIME_PATTERN.IsMatch(strClean);

            //                                              //Days part should be ok.
            //                                              //Verify time part
            if (
                boolIsParsableToTime
                )
            {
                //                                          //Get time part
                int intPoint = strClean.IndexOf('.');
                String strTimePart = (intPoint < 0) ? strClean : strClean.Substring(intPoint + 1);

                //                                          //Get additional days
                String strAdditionalDays = (intPoint < 0) ? null : strClean.Substring(0, intPoint);

                int intL = 0;
                /*REPEAT-UNTIL*/
                do
                {
                    DateTime zdt;
                    boolIsParsableToTime = DateTime.TryParse(strTimePart, arrlanguages_L[intL].culture,
                        System.Globalization.DateTimeStyles.NoCurrentDateDefault, out zdt);

                    //                                      //To avoid warning
                    zdt = default(DateTime);

                    intL = intL + 1;
                }
                while (!(
                    (intL >= arrlanguages_L.Length) ||
                    //                                      //Previous language parse ok
                    boolIsParsableToTime
                    ));

                if (
                    (strAdditionalDays != null) &&
                    strAdditionalDays.IsParsableToInt() &&
                    //                                      //Additional days should be in the range 0,9
                    !strAdditionalDays.ParseToInt().IsBetween(0, 9)
                    )
                    boolIsParsableToTime = false;

                if (
                    strNegativeHours != null &&
                    strNegativeHours.IsParsableToInt() &&
                    //                                      //Negative hours must be in the range -9,0
                    !strNegativeHours.ParseToInt().IsBetween(-9, -1)
                    )
                    boolIsParsableToTime = false;

                if (
                    //                                      //Time String cannot have both additional days and
                    //                                      //      negative hours
                    strNegativeHours != null && strAdditionalDays != null
                )
                    boolIsParsableToTime = false;

            }

            return boolIsParsableToTime;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static Time ParseToTime(

            this String str_I
            )
        {
            return Std.ParseToTime(str_I, Language.ENGLISH);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static Time ParseToTime(

            //                                              //Form: days.time or time
            this String str_I,
            params Language[] arrlanguages_L
            )
        {
            Test.AbortIfNull(str_I, "str_I");
            Test.AbortIfNullOrEmpty(arrlanguages_L, "arrlanguages_L");

            String strClean = str_I.Trim(' ');
            int intPoint = str_I.IndexOf('.');

            int intAdditionalDays = (intPoint < 0) ? 0 : strClean.Substring(0, intPoint).ParseToInt();
            String strTimePart = (intPoint < 0) ? str_I : strClean.Substring(intPoint + 1);

            //                                              //Get negative hours
            int intOpenParenthesis = str_I.IndexOf('(');
            int intCloseParenthesis = str_I.IndexOf(')');

            int intNegativeHours = 0;
            if (
                (intOpenParenthesis >= 0) &&
                (intCloseParenthesis > intOpenParenthesis)
                )
            {
                intNegativeHours = strClean.Substring(intOpenParenthesis + 1,
                    intCloseParenthesis - intOpenParenthesis - 1).ParseToInt();

                //                                          //Negative hours are temporarily substitute to 00 to be 
                //                                          //      correctly parsed by c# datetime
                strTimePart = "00" + strTimePart.Substring(intCloseParenthesis + 1);
            }

            //                                              //Parse time part
            DateTime zdt;
            int intL = 0;
            /*LOOP*/
            while (true)
            {
                bool boolIsParsable = DateTime.TryParse(strTimePart, arrlanguages_L[intL].culture,
                    System.Globalization.DateTimeStyles.NoCurrentDateDefault, out zdt);

                /*EXIT-IF*/
                if (
                    (intL >= arrlanguages_L.Length) || boolIsParsable
                    )
                    break;

                intL = intL + 1;
            }
            if (
                //                                          //Could not parse
                (intL >= arrlanguages_L.Length)
                )
                Test.Abort(Test.ToLog(strTimePart, "strTimePart") + " can not be parsed",
                    Test.ToLog(str_I, "str_I"), Test.ToLog(arrlanguages_L, "arrlanguages_L"));

            return
                new Time(((intAdditionalDays * 24 + zdt.Hour + intNegativeHours) * 60 + zdt.Minute) * 60 + zdt.Second);
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //UNARY OPERATOR*/

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //EQUALITY OPERATORS

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //********************************OTHER OPERATORS

        //--------------------------------------------------------------------------------------------------------------
        /*SUPPORT METHODS FOR INITIALIZER*/

        //--------------------------------------------------------------------------------------------------------------
        private static void subVerifyConstantsTime(
            )
        {
            Test.AbortIfNullOrEmpty(Std.arrstrTIME_PICTURES, "Std.TIME_PICTURES");

            //                                              //Variables requires to test pictures
            DateTime dtTEST = new DateTime(2018, 11, 14, 5, 41, 28);
            Char[] arrcharUSEFUL_IN_TIME_PICTURE = "HhmstT :".ToCharArray();
            Std.Sort(arrcharUSEFUL_IN_TIME_PICTURE);

            for (int intI = 0; intI < Std.arrstrTIME_PICTURES.Length; intI = intI + 1)
            {
                //                                          //Verify it works
                try
                {
                    String str = dtTEST.ToString(Std.arrstrTIME_PICTURES[intI]);
                }
                catch (Exception sysexceptError)
                {
                    Test.Abort(
                        Test.ToLog(Std.arrstrTIME_PICTURES[intI], "Std.TIME_PICTURES[" + intI + "]") +
                            " can not be a standard picture, it does not work",
                        Test.ToLog(Std.arrstrTIME_PICTURES, "Std.TIME_PICTURES"),
                            "dtTEST(" + dtTEST.ToString("yyyy-MM-dd HH:mm:ss"),
                        Test.ToLog(sysexceptError, "sysexceptError"));
                }

                Test.AbortIfOneOrMoreCharactersAreNotInSortedSet(Std.arrstrTIME_PICTURES[intI],
                    "Std.TIME_PICTURES[" + intI + "]", arrcharUSEFUL_IN_TIME_PICTURE, "arrcharUSEFUL_IN_TIME_PICTURE");
            }

            Test.AbortIfDuplicate(Std.arrstrTIME_PICTURE_SORTED, "Std.arrstrTIME_PICTURE_SORTED");
        }

        //--------------------------------------------------------------------------------------------------------------

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Std.ZonedTime*/
        //==============================================================================================================
        //                                                  //LITERALS

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //DECLARATION AND ASSIGNMENT

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONVERSION TO TEXT

        //--------------------------------------------------------------------------------------------------------------
        /*CONSTANTS*/

        //                                                  //Array that contains all the time zones display name
        private static readonly String[] arrstrTIME_ZONE_DISPLAY_NAME;

        //--------------------------------------------------------------------------------------------------------------
        /*METHODS*/

        //-------------------------------------------------------------------------------------------------------------
        public static String ToText(
            this ZonedTime ztime_I
            )
        {
            return ztime_I.ToText(ZonedTimeTextEnum.TIME_ZONE);
        }

        //-------------------------------------------------------------------------------------------------------------
        public static String ToText(
            this ZonedTime ztime_I,
            ZonedTimeTextEnum ztimetext_I
            )
        {
            String strZZZ = (ztimetext_I == ZonedTimeTextEnum.UTC) ? "" : "ZZZ";

            return ztime_I.ToString("yyyy-MM-ddTHH:mm:ss.fff" + strZZZ);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool IsParsableToZtime(
            this String str_I
            )
        {
            Test.AbortIfNull(str_I, "str_I");

            //                                              //Clean a verify pattern
            String strClean = str_I.Trim(' ');

            //                                              //Get the indexes that separate the elements of the String
            //                                              //      Remember the format: "yyyy-MM-ddTHH:mm:ss.fff(zzz)"
            int intT = strClean.IndexOf('T');
            int intPoint = strClean.IndexOf('.');
            int intOpenBracket = strClean.IndexOf('[');
            int intCloseBracket = strClean.IndexOf(']');

            bool boolIsParsable = false;

            String strDatePart = null;
            //                                              //Work with date part
            if (
                intT > 0
                )
            {
                strDatePart = strClean.Substring(0, intT);
                boolIsParsable = strDatePart.IsParsableToDate();
            }

            String strTimePart = null;
            //                                              //Work with time part
            if (
                boolIsParsable && intPoint > intT
                )
            {
                strTimePart = strClean.Substring(intT + 1, intPoint - (intT + 1));
                boolIsParsable = (
                    strTimePart.IsParsableToTime() &&
                    strTimePart.ParseToTime().IsBetween(new Time(0), new Time(23, 59, 59))
                    );
            }
            else
            {
                boolIsParsable = false;
            }

            String strMsPart = null;
            //                                              //Work with milliseconds
            if (
                boolIsParsable && intOpenBracket > intPoint
                )
            {
                strMsPart = strClean.Substring(intPoint + 1, intOpenBracket - (intPoint + 1));
                boolIsParsable = strMsPart.IsParsableToInt() && strMsPart.ParseToInt().IsBetween(0, 999);
            }
            else
            {
                boolIsParsable = false;
            }

            String strTzPart = null;
            //                                              //Work with Time Zone
            if (
                boolIsParsable && intCloseBracket > intOpenBracket
                )
            {
                strTzPart = strClean.Substring(intOpenBracket + 1, intCloseBracket - (intOpenBracket + 1));

                //                                          //Search if it is one of Towa's available Time Zones
                boolIsParsable = strTzPart.BinarySearch(Std.arrstrTIME_ZONE_DISPLAY_NAME) >= 0;
            }
            else
            {
                boolIsParsable = false;
            }

            if (
                boolIsParsable
                )
            {
                //                                           //Verify date and time are valid for current Time Zone
            }

            return boolIsParsable;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static ZonedTime ParseToZtime(

            this String str_I
            )
        {
            Test.AbortIfNull(str_I, "str_I");
            if (
                !Std.IsParsableToZtime(str_I)
            )
                Test.Abort(Test.ToLog(str_I, "str_I") + " can not parse to zonedTime");

            //                                              //Clean a verify pattern
            String strClean = str_I.Trim(' ');

            //                                              //Get the indexes that separate the elements of the String
            //                                              //      Remember the format: "yyyy-MM-ddTHH:mm:ss.fff(zzz)"
            int intT = strClean.IndexOf('T');
            int intPoint = strClean.IndexOf('.');
            int intOpenBracket = strClean.IndexOf('[');
            int intCloseBracket = strClean.IndexOf(']');

            //bool boolIsParsable = false;


            String strDatePart = strClean.Substring(0, intT);
            String strTimePart = strClean.Substring(intT + 1, intPoint - (intT + 1));
            String strMsPart = strClean.Substring(intPoint + 1, intOpenBracket - (intPoint + 1));
            String strTzPart = strClean.Substring(intOpenBracket + 1, intCloseBracket - (intOpenBracket + 1));

            return new ZonedTime(Std.ParseToDate(strDatePart), Std.ParseToTime(strTimePart), Std.ParseToInt(strMsPart),
                TimeZoneX.arrtimezoneSORTED[strTzPart.BinarySearch(Std.arrstrTIME_ZONE_DISPLAY_NAME)]);
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //UNARY OPERATOR*/

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //EQUALITY OPERATORS

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //********************************OTHER OPERATORS

        //--------------------------------------------------------------------------------------------------------------
        /*SUPPORT METHODS FOR INITIALIZER*/

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Std.DateTime*/
        //==============================================================================================================
        /*CONSTANTS*/

        public static readonly DateTime dateMIN = new DateTime(0001, 1, 1);
        public static readonly DateTime dateMAX = new DateTime(9999, 12, 31);
        public static readonly DateTime dtMIN = new DateTime(0001, 1, 1, 0, 0, 0, 0);
        //                                                  //(26Mar2018, GLG) La documentación de .net dice que pude
        //                                                  //      almacenar hasta 59.9999999 segs, sin embargo, si
        //                                                  //      son 59.9995 (se redondea a 60.000 y aborta por
        //                                                  //      fuera de rango. Se puede iniciar com MaxValue que lo
        //                                                  //      inicia con 59.9999999 pero no sirve dado que la
        //                                                  //      de AddSeconds será solo a milisegundos.
        //          
        public static readonly DateTime dtMAX = new DateTime(9999, 12, 31, 23, 59, 59, 999);

        //--------------------------------------------------------------------------------------------------------------
        /*METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public static DateTime Now(
            //                                              //Genera un dtNow, si estamos en comparable Test, lo genera
            //                                              //      masked
            //                                              //dt, Now
            )
        {
            return (Test.z_TowaPRIVATE_boolIsComparableLog())
                ? Test.z_TowaPRIVATE_GetNowForComparableTest() : DateTime.Now;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool boolIsDate(
            //                                              //Valida sea una fecha.

            //                                              //bool, true si es fecha.

            //                                              //Fecha a validar.
            DateTime date_I
            )
        {
            return (
                //                                          //Con excepción de la fecha todo esta en ceros.
                (date_I.Hour == 0) && (date_I.Minute == 0) && (date_I.Second == 0) && (date_I.Millisecond == 0)
                );
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Std.Array*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //LITERALS

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //DECLARATION AND ASSIGNMENT

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTION

        //--------------------------------------------------------------------------------------------------------------
        public static TItem[] ConcatenateArrays<TItem>(

            //                                              //1 or many arrays
            params TItem[][] ArrayOfArrayOfItems_I
            )
        {
            //                                              //Compute concatenated length
            int intLength = 0;
            for (int intI = 0; intI < ArrayOfArrayOfItems_I.Length; intI = intI + 1)
            {
                intLength = intLength + ArrayOfArrayOfItems_I[intI].Length;
            }

            TItem[] arritemConcatenated = new TItem[intLength];

            int intPos = 0;
            foreach (TItem[] arritem in ArrayOfArrayOfItems_I)
            {
                //                                          //Copy and advance position to insert
                Array.Copy(arritem, 0, arritemConcatenated, intPos, arritem.Length);
                intPos = intPos + arritem.Length;
            }

            return arritemConcatenated;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONVERSIONS

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //EQUALITY OPERATORS

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS

        //--------------------------------------------------------------------------------------------------------------
        public static int BinarySearch<TSearch, TItem>(

            //                                              //Argument to search, should be able to compare to XC
            this TSearch searchArgument_I,
            //                                              //Should be in ascending sequence
            TItem[] arrayOfItem_I
            )
            where TSearch : IComparable where TItem : IComparable
        {
            Test.AbortIfNull(searchArgument_I, "searchArgument_I");
            Test.AbortIfNull(arrayOfItem_I, "arrayOfItem_I");

            return Array.BinarySearch(arrayOfItem_I, searchArgument_I, StringComparer.Ordinal);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static int SequentialSearch(

            //                                              //Argument to search, should be able to compare to XC
            this String strArgument_I,
            //                                              //Do not need to be ins ascending sequence
            String[] arrstr_I
            )
        {
            Test.AbortIfNull(strArgument_I, "xcaArgument_I");
            Test.AbortIfNull(arrstr_I, "arrxc_I");

            int intI = 0;
            /*UNTIL-DO*/
            while (!(
                (intI >= arrstr_I.Length) ||
                (arrstr_I[intI] == strArgument_I)
              ))
            {
                intI = intI + 1;
            }

            //                                              //-1 if not found
            return (intI < arrstr_I.Length) ? intI : -1;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static int SequentialSearch<TSearch, TItem>(
            //                                              //This method will process PRIMITIVES, any other object will
            //                                              //      will use the following process

            //                                              //Argument to search, should be able to compare to XC
            this TSearch searchArgument_I,
            //                                              //Do not need to be ins ascending sequence
            TItem[] arrayOfItems_I
            )
        {
            Test.AbortIfNull(searchArgument_I, "searchArgument_I");
            Test.AbortIfNull(arrayOfItems_I, "arrayOfItems_I");

            int intI = 0;
            /*UNTIL-DO*/
            while (!(
                (intI >= arrayOfItems_I.Length) ||
                //                                          //item can be null, but can never equals argument
                (arrayOfItems_I[intI] != null) && arrayOfItems_I[intI].Equals(searchArgument_I)
              ))
            {
                intI = intI + 1;
            }

            //                                              //-1 if not found
            return (
                intI < arrayOfItems_I.Length
                ) ? intI : -1;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static int SequentialSearch(

            //                                              //Argument to search, should be able to compare to XC
            this Object xaArgument_I,
            //                                              //Do not need to be ins ascending sequence
            Object[] arrxb_I
            )
        {
            Test.AbortIfNull(xaArgument_I, "xaArgument_I");
            Test.AbortIfNull(arrxb_I, "arrxb_I");

            int intI = 0;
            /*UNTIL-DO*/
            while (!(
                (intI >= arrxb_I.Length) ||
                //                                          //is the same object
                arrxb_I[intI] == xaArgument_I
              ))
            {
                intI = intI + 1;
            }

            //                                              //-1 if not found
            return (intI < arrxb_I.Length) ? intI : -1;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool IsInSet(
            //                                              //bool, true if is in set.

            this String strArgument_I,
            params String[] arrstrSet_I
            )
        {
            Test.AbortIfNull(strArgument_I, "strArgument_I");
            Test.AbortIfNull(arrstrSet_I, "arrstrSet_I");

            return (
                //                                          //Is in set
                strArgument_I.SequentialSearch(arrstrSet_I) >= 0
                );
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static bool IsInSet<XA, XB>(
            //                                              //This method will process PRIMITIVES, any other object will
            //                                              //      will use the following process

            //                                              //bool, true if is in set.

            this XA xaArgument_I,
            params XB[] arrxbSet_I
            )
        {
            Test.AbortIfNull(xaArgument_I, "xaArgument_I");
            Test.AbortIfNull(arrxbSet_I, "arrxbSet_I");

            return (
                //                                          //Is in set
                xaArgument_I.SequentialSearch(arrxbSet_I) >= 0
                );
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static bool IsInSet(
            //                                              //bool, true if is in set.

            this Object objArgument_I,
            params Object[] arrobjSet_I
            )
        {
            Test.AbortIfNull(objArgument_I, "objArgument_I");
            Test.AbortIfNull(arrobjSet_I, "arrobjSet_I");

            return (
                //                                          //Is in set
                objArgument_I.SequentialSearch(arrobjSet_I) >= 0
                );
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool IsInSortedSet<XCA, XC>(
            //                                              //bool, true if is in set.

            this XCA xca_I,
            XC[] arrxcSortedSet_I
            )
            where XCA : IComparable where XC : IComparable
        {
            Test.AbortIfNull(xca_I, "xca_I");
            Test.AbortIfNull(arrxcSortedSet_I, "arrxcSortedSet_I");

            return (
                //                                          //Is in set
                xca_I.BinarySearch(arrxcSortedSet_I) >= 0
                );
        }

        //--------------------------------------------------------------------------------------------------------------
        public static TItem[] Copy<TItem>(

            this TItem[] arrayOfItem_I
            )
        {
            Test.AbortIfNull(arrayOfItem_I, "arrayOfItem_I");

            return arrayOfItem_I.Copy(0, arrayOfItem_I.Length);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static TItem[] Copy<TItem>(

            this TItem[] arrayOfItem_I,
            int index_I
            )
        {
            Test.AbortIfNull(arrayOfItem_I, "arrayOfItem_I");

            return arrayOfItem_I.Copy(index_I, arrayOfItem_I.Length - index_I);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static TItem[] Copy<TItem>(

            this TItem[] arrayOfItem_I,
            int index_I,
            //                                              //If index + size > array length, last items will be
            //                                              //      unassigned
            int size_I
            )
        {
            Test.AbortIfNull(arrayOfItem_I, "arrayOfItem_I");

            TItem[] arritemCopy = new TItem[size_I];

            //                                              //Compute size to copy
            int intSizeToCopy = ((index_I + size_I) > arrayOfItem_I.Length) ? arrayOfItem_I.Length - index_I : size_I;

            Array.Copy(arrayOfItem_I, index_I, arritemCopy, 0, intSizeToCopy);

            return arritemCopy;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //MANIPULATION

        //--------------------------------------------------------------------------------------------------------------
        public static void Sort<XC>(

            XC[] arrxc_I
            )
            where XC : IComparable
        {
            Test.AbortIfNull(arrxc_I, "arrxc_I");

            Array.Sort(arrxc_I, StringComparer.Ordinal);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void Sort<XC, XT>(

            XC[] arrxc_I,
            XT[] arrxt_I
            )
            where XC : IComparable
        {
            Test.AbortIfNull(arrxc_I, "arrxc_I");
            Test.AbortIfNull(arrxt_I, "arrxt_I");
            if (
                arrxc_I.Length != arrxt_I.Length
                )
                Test.Abort(Test.ToLog(arrxc_I.Length, "arrxc_I.Length") + ", " +
                    Test.ToLog(arrxt_I.Length, "arrxt_I.Length") + " both arrays should be the same size");

            Array.Sort(arrxc_I, arrxt_I, StringComparer.Ordinal);
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Std.LinkedList*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //DECLARATION AND ASSIGNMENT

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTION

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONVERSION

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS

        //--------------------------------------------------------------------------------------------------------------
        public static LinkedListNode<XT> NodeAt<XT>(
           //                                              //Returns the node at the specified position in the 
           //                                              //      linked list. 

           //                                              //The linkedlist where the node will be retrieved
           this LinkedList<XT> lnklxt_I,
            //                                              //The index position
            int intIndex_I
            )
        {
            Test.AbortIfNull(lnklxt_I, "lnklxt_I");
            if (
                lnklxt_I.Count == 0
                )
                Test.Abort(Test.ToLog(lnklxt_I, "lnklxt_I") + " should have at least one node");
            if (!(
                intIndex_I.IsBetween(0, lnklxt_I.Count - 1)
                ))
                Test.Abort(Test.ToLog(intIndex_I, "intIndex_I") + " should be in the range 0-" + (lnklxt_I.Count - 1));

            LinkedListNode<XT> nodextNodeAt;

            if (
                intIndex_I < (lnklxt_I.Count / 2)
                )
            {
                nodextNodeAt = lnklxt_I.First;
                for (int intI = 0; intI < intIndex_I; intI = intI + 1)
                {
                    nodextNodeAt = nodextNodeAt.Next;
                }
            }
            else
            {
                nodextNodeAt = lnklxt_I.Last;
                for (int intI = lnklxt_I.Count - 1; intI > intIndex_I; intI = intI - 1)
                {
                    nodextNodeAt = nodextNodeAt.Previous;
                }
            }

            return nodextNodeAt;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static LinkedListNode<XT> FindNext<XT>(
            //                                              //Returns the linked list node of the next ocurrence of
            //                                              //      obj, after nodext_I

            //                                              //The node where the search starts
            this LinkedListNode<XT> nodext_I,
            //                                              //The object to search
            Object obj
            )
        {
            Test.AbortIfNull(nodext_I, "nodext_I");
            Test.AbortIfNull(nodext_I.List, "nodext_I.List");

            LinkedListNode<XT> nodetCurrentNode = nodext_I;
            /*UNTIL-DO*/
            while (!(
                //                                          //Reaches the end of the linked list
                (nodetCurrentNode == null) ||
                //                                          //Both obj and node value are null
                (obj == null && nodetCurrentNode.Value == null) ||
                //                                          //Both obj and node value contain the same object
                (obj != null && obj.Equals(nodetCurrentNode.Value))
                ))
            {
                nodetCurrentNode = nodetCurrentNode.Next;
            }

            return nodetCurrentNode;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static LinkedListNode<XT> FindPrevious<XT>(
            //                                              //Returns the linked list node of the previous ocurrence of
            //                                              //      obj, before nodext_I

            //                                              //The node where the search starts
            this LinkedListNode<XT> nodext_I,
            //                                              //The object to search
            Object obj
            )
        {
            Test.AbortIfNull(nodext_I, "nodext_I");
            Test.AbortIfNull(nodext_I.List, "nodext_I.List");

            LinkedListNode<XT> nodetCurrentNode = nodext_I;
            /*UNTIL-DO*/
            while (!(
                //                                          //Reaches the end of the linked list
                (nodetCurrentNode == null) ||
                //                                          //Both obj and node value are null
                (obj == null && nodetCurrentNode.Value == null) ||
                //                                          //Both obj and node value contain the same object
                (obj != null && obj.Equals(nodetCurrentNode.Value))
                ))
            {
                nodetCurrentNode = nodetCurrentNode.Previous;
            }

            return nodetCurrentNode;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //MANIPULATION

        //--------------------------------------------------------------------------------------------------------------
        public static void AddAfter<XT>(
            //                                              //Creates a node with value xtToAdd_I and adds it
            //                                              //      inmediatily after nodext_I in the linked list

            //                                              //Previous node of the new node that will be added
            this LinkedListNode<XT> nodext_I,
            //                                              //Value to be added
            XT xtToAdd_I
            )
        {
            Test.AbortIfNull(nodext_I, "nodext_I");
            Test.AbortIfNull(nodext_I.List, "nodext_I.List");

            nodext_I.List.AddAfter(nodext_I, xtToAdd_I);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void AddBefore<XT>(
            //                                              //Creates a node with value xtToAdd_I and adds it 
            //                                              //     inmediatily before nodext_I in the linked list

            //                                              //Next node of the new node that will be added
            this LinkedListNode<XT> nodext_I,
            //                                              //Value to be added
            XT xtToAdd_I
            )
        {
            Test.AbortIfNull(nodext_I, "nodext_I");
            Test.AbortIfNull(nodext_I.List, "nodext_I.List");

            nodext_I.List.AddBefore(nodext_I, xtToAdd_I);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void AddFirst<XT>(
            //                                              //Appends at the beginning of lnklxt_I all the elements from
            //                                              //      collxtToAdd_I

            //                                              //Linked list where the collection will be appended
            this LinkedList<XT> lnklxt_I,
            //                                              //The collection which contains all the elements that will
            //                                              //      be appended
            IEnumerable<XT> collxtToAdd_I
            )
        {
            Test.AbortIfNull(lnklxt_I, "lnklxt_I");
            Test.AbortIfNull(collxtToAdd_I, "collxtToAdd_I");

            foreach (XT xtItem in collxtToAdd_I.Reverse())
            {
                lnklxt_I.AddFirst(xtItem);
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void AddLast<XT>(
            //                                              //Appends at the end of lnklxt_I all the elements from
            //                                              //      collxtToAdd_I

            //                                              //Linked list where the collection will be appended
            this LinkedList<XT> lnklxt_I,
            //                                              //The collection which contains all the elements that will
            //                                              //      be appended
            IEnumerable<XT> collxtToAdd_I
            )
        {
            Test.AbortIfNull(lnklxt_I, "lnklxt_I");
            Test.AbortIfNull(collxtToAdd_I, "collxtToAdd_I");

            foreach (XT xtItem in collxtToAdd_I)
            {
                lnklxt_I.AddLast(xtItem);
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void AddAfter<XT>(
            //                                              //Adds every element of collxtToAdd_I to the linked list 
            //                                              //      inmediatily after nodext_I.

            //                                              //Previous node where the collection will be appended
            this LinkedListNode<XT> nodext_I,
            //                                              //The collection which contains all the elements that will
            //                                              //      be appended
            IEnumerable<XT> collxtToAdd_I
            )
        {
            Test.AbortIfNull(nodext_I, "nodext_I");
            Test.AbortIfNull(nodext_I.List, "nodext_I.List");
            Test.AbortIfNull(collxtToAdd_I, "collxtToAdd_I");

            foreach (XT xtItem in collxtToAdd_I.Reverse())
            {
                nodext_I.List.AddAfter(nodext_I, xtItem);
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void AddBefore<XT>(
            //                                              //Adds every element of collxtToAdd_I to the linked list 
            //                                              //      inmediatily before nodext_I.

            //                                              //Next node where the collection will be appended
            this LinkedListNode<XT> nodext_I,
            //                                              //The collection which contains all the elements that will
            //                                              //      be appended
            IEnumerable<XT> collxtToAdd_I
            )
        {
            Test.AbortIfNull(nodext_I, "nodext_I");
            Test.AbortIfNull(nodext_I.List, "nodext_I.List");
            Test.AbortIfNull(collxtToAdd_I, "collxtToAdd_I");

            foreach (XT xtItem in collxtToAdd_I)
            {
                nodext_I.List.AddBefore(nodext_I, xtItem);
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void Remove<XT>(
            //                                              //Removes nodext_I from the linked list

            //                                              //Node to be removed
            this LinkedListNode<XT> nodext_I
            )
        {
            Test.AbortIfNull(nodext_I, "nodext_I");
            Test.AbortIfNull(nodext_I.List, "nodext_I.List");

            nodext_I.List.Remove(nodext_I);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void RemoveRange<XT>(
            //                                              //Removes all nodes between nodextStart_I and nodetEnd_I
            //                                              //      inclusive

            //                                              //Linked list where the nodes will be removed
            this LinkedList<XT> lnklxt_I,
            //                                              //Node where the remove operation will start
            LinkedListNode<XT> nodextStart_I,
            //                                              //Node where the remove operation will end
            LinkedListNode<XT> nodextEnd_I
            )
        {
            Test.AbortIfNull(lnklxt_I, "lnklxt_I");
            Test.AbortIfNull(nodextStart_I, "nodextStart_I");
            Test.AbortIfNull(nodextEnd_I, "nodextEnd_I");
            if (
                nodextStart_I.List != lnklxt_I
                )
                Test.Abort(Test.ToLog(nodextStart_I, "nodextStart_I") + " should be member of " +
                    Test.ToLog(lnklxt_I, "lnklxt_I"));
            if (
                nodextEnd_I.List != lnklxt_I
                )
                Test.Abort(Test.ToLog(nodextEnd_I, "nodextEnd_I") + " should be member of " +
                    Test.ToLog(lnklxt_I, "lnklxt_I"));

            LinkedListNode<XT> nodetToRemove = nodextStart_I;

            /*WHILE*/
            while (
                nodetToRemove != nodextEnd_I
                )
            {
                nodetToRemove = nodetToRemove.Next;

                if (
                    //                                      //Verify that nodetToRemove has not reached the end of the
                    //                                      //      list because this would mean that nodextStart_I was 
                    //                                      //      not located before nodetEnd_I
                    nodetToRemove == null
                    )
                    Test.Abort(Test.ToLog(nodextStart_I, "nodextStart_I") + " should be located before " +
                                  Test.ToLog(nodextEnd_I, "nodextEnd_I"));

                Std.Remove(nodetToRemove.Previous);
            }

            Std.Remove(nodetToRemove);
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Std.XXXXX*/
        //==============================================================================================================
        //                                                  //LITERAL

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //DECLARATION, CONSTRUCTION AND ASSIGNMENT

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONVERSIONS

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //RELATIONAL OPERATORS*/

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //EQUALITY OPERATORS

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //********************************OTHER OPERATORS

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //TERNARY CONDITIONAL OPERATOR

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //MINIMUM AND MAXIMUM

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Std.DbItems*/
        //==============================================================================================================

        //                                                  //Static methods to support data verification

        //--------------------------------------------------------------------------------------------------------------
        public static bool IsTextValid(
            //                                              //1. All chars are valid.
            //                                              //2. Separators are use as separators
            //                                              //bool, true if valid

            this String Text_I,
            char[] ValidCharacters_I,
            char Separator_I
            )
        {
            Test.AbortIfNullOrEmpty(Text_I, "Text_I");
            Test.AbortIfNullOrEmpty(ValidCharacters_I, "ValidCharacters_I");

            //                                              //To verify chars, for EFFICIENCY, separator are replaced
            //                                              //      for any other valid char.
            String strTextClean = Text_I.Replace(Separator_I, ValidCharacters_I[0]);

            int intI = 0;
            /*UNTIL-DO*/
            while (!(
                (intI >= Text_I.Length) ||
                !Text_I[intI].IsInSortedSet(ValidCharacters_I)
                ))
            {
                intI = intI + 1;
            }

            return (
                //                                          //All chars were valid
                (intI >= Text_I.Length) &&
                //                                          //Do not starts/ends with separator
                (Text_I[0] != Separator_I) && (Text_I[Text_I.Length - 1] != Separator_I) &&
                //                                          //No 2 separators
                !Text_I.ContainsOrdinal("" + Separator_I + Separator_I)
                );
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static bool IsTextValid(

            this String Text_I
            )
        {
            return Text_I.IsTextValid(Std.CHARS_USEFUL_IN_TEXT, '\uFFFF');
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static bool IsTextValid(

            this String Text_I,
            char[] ValidCharacters_I
            )
        {
            return Text_I.IsTextValid(ValidCharacters_I, '\uFFFF');
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Std.Serialization*/
        //==============================================================================================================

        //                                                  //Static methods to support serialization

        //--------------------------------------------------------------------------------------------------------------
        public static byte[] Serialize(
            //                                              //arrbyte, {4 bytes for length} + {UTF8 encoding of string}
            this String string_I
            )
        {
            byte[] arrbyte = Encoding.UTF8.GetBytes(string_I);
            byte[] arrbyteTotalLength = BitConverter.GetBytes(arrbyte.Length + 4);

            return Std.ConcatenateArrays(arrbyteTotalLength, arrbyte);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static byte[] Serialize(
            //                                              //arrbyte, method serialized
            this MethodInfo method_I
            )
        {
            //                                              //To completely serialize a "method" will need
            //                                              //      typeDeclaring, we can get this type
            //                                              //      (method_I.ReflectType()), but this was hard to
            //                                              //      serialize/deserialize.
            //                                              //typeof(TTarget) should be provided as a paramenter to
            //                                              //      DeserializeMethodInfo.

            //                                              //One element of info (method name) is required to
            //                                              //      Deserialize.
            //                                              //Also typeDeclaring is required but it should be provided
            //                                              //      as a parameter to Deserialize method.
            String strTypeDeclaring = method_I.ReflectedType.Name;
            String strMethodName = method_I.Name;

            return Std.ConcatenateArrays(strTypeDeclaring.Serialize(), strMethodName.Serialize());
        }

        //--------------------------------------------------------------------------------------------------------------
        public static byte[] Serialize<TObject>(
            //                                              //Get a serialized version of an array of object.

            this TObject[] ArrayOfObject_I
            )
            where TObject : SerializableInterface<TObject>
        {
            List<byte> darrbyteSerialized = new List<byte>();

            foreach (TObject objectX in ArrayOfObject_I)
            {
                byte[] arrbyteDperiod = objectX.Serialize();
                darrbyteSerialized.AddRange(arrbyteDperiod);
            }

            //                                              //Insert 4 bytes of final count + 4 (includes count)
            byte[] arrbyteCount = BitConverter.GetBytes(darrbyteSerialized.Count + 4);
            darrbyteSerialized.InsertRange(0, arrbyteCount);

            return darrbyteSerialized.ToArray();
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void DeserializeString(
            //                                              //Returns a deserialized string.

            //                                              //String deserialized.
            out String string_O,
            //                                              //{4 for length} + {string} + {other}, returns {other}
            ref byte[] Bytes_IO
            )
        {
            byte[] arrbyteToDeserialize;
            Std.SeparateToDeserializeVariableSize(out arrbyteToDeserialize, ref Bytes_IO);

            string_O = Encoding.UTF8.GetString(arrbyteToDeserialize);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void DeserializeMethodInfo(
            //                                              //Returns a deserialized method.

            //                                              //Method deserialized.
            out MethodInfo method_O,
            //                                              //{4 for length} + {string<type name>} + {4 for length} +
            //                                              //      {string<method name>} + {other}, returns {other}
            ref byte[] Bytes_IO
            )
        {
            //                                              //Desarialization contain strDeclaringType & strMethodName
            String strDeclaringType;
            Std.DeserializeString(out strDeclaringType, ref Bytes_IO);
            String strMethodName;
            Std.DeserializeString(out strMethodName, ref Bytes_IO);

            /*THROWS AN EXCEPTION*/
            Type typeDeclaring = Type.ReflectionOnlyGetType(strDeclaringType, true, false);
            method_O = typeDeclaring.GetMethod(strMethodName);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void DeserializeArray<TObject>(
            //                                              //Returns a deserialized object.

            //                                              //Array deserialized.
            out TObject[] ArrayOfObjects_O,
            //                                              //The serialized bytes.
            ref byte[] Bytes_IO,
            TObject Object_DUMMY_I
            )
            where TObject : SerializableInterface<TObject>
        {
            byte[] arrbyteToDeserialize;
            Std.SeparateToDeserializeVariableSize(out arrbyteToDeserialize, ref Bytes_IO);

            List<TObject> darrobject = new List<TObject>();

            /*WHILE-DO*/
            while (
                //                                          //More periods to deserialize
                arrbyteToDeserialize.Length > 0
                )
            {
                TObject objectX;
                Object_DUMMY_I.Deserialize(out objectX, ref arrbyteToDeserialize);

                darrobject.Add(objectX);
            }

            ArrayOfObjects_O = darrobject.ToArray();
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void SeparateToDeserializeFixSize(
            //                                              //Returns a deserialized object.

            //                                              //Bytes to deserialize.
            out byte[] BytesToDeserialize_O,
            //                                              //Return rest.
            ref byte[] Bytes_IO,

            //                                              //int 4, long 8, char 1, etc.
            int Size_I
            )
        {
            if (
                Bytes_IO.Length < Size_I
                )
                Test.Abort(Test.ToLog(Bytes_IO, "Bytes_IO") + " should be at least " + Size_I + " bytes long");

            //                                              //Separate in 2 arrays (deserialize & return)

            //                                              //Fix part to deserialize
            BytesToDeserialize_O = new byte[Size_I];
            Array.Copy(Bytes_IO, 0, BytesToDeserialize_O, 0, Size_I);

            //                                              //Bytes left, to return
            byte[] arrbyteToReturn = new byte[Bytes_IO.Length - Size_I];
            Array.Copy(Bytes_IO, Size_I, arrbyteToReturn, 0, arrbyteToReturn.Length);
            Bytes_IO = arrbyteToReturn;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void SeparateToDeserializeVariableSize(
            //                                              //Returns a deserialized object.

            //                                              //Bytes to deserialize (excluding 4 bytes of count).
            //                                              //This could be one object (String) or an array of objects
            out byte[] BytesToDeserialize_O,
            //                                              //Return rest.
            ref byte[] Bytes_IO
            )
        {
            //                                              //Extract count (total bytes to deserialize)
            byte[] arrbyteCount = new byte[4];
            Array.Copy(Bytes_IO, 0, arrbyteCount, 0, 4);
            int intCount = BitConverter.ToInt32(arrbyteCount, 0);

            //                                              //Separate in 2 arrays (deserialize & return)

            //                                              //Variable size part to deserialize
            BytesToDeserialize_O = new byte[intCount - 4];
            Array.Copy(Bytes_IO, 4, BytesToDeserialize_O, 0, BytesToDeserialize_O.Length);

            //                                              //Bytes left, to return
            byte[] arrbyteToReturn = new byte[Bytes_IO.Length - intCount];
            Array.Copy(Bytes_IO, intCount, arrbyteToReturn, 0, arrbyteToReturn.Length);
            Bytes_IO = arrbyteToReturn;
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Std.Constants*/
        //==============================================================================================================
        //                                                  //***** SECCIÓN PENDIENTE DE ORDENAR

        //--------------------------------------------------------------------------------------------------------------
        /*CONTANTS*/

        public const String OO_INST = "C#";

        //                                                  //Ordenando TODOS los caracteres en forma de String con
        //                                                  //      Std.Sort(arrstr) se determinaron los siguientes
        //                                                  //      Valores.
        //                                                  //¿¿Será igual en Java y en Swift??

        //==============================================================================================================
        /*END-TASK*/

        //--------------------------------------------------------------------------------------------------------------
        /*INITIALIZER*/

        //--------------------------------------------------------------------------------------------------------------
        static Std(
            )
        {
            //                                              //PREPARE CONSTANTS (Verify later).
            //                                              //WARNING: intentionaly I use Array.Sort intead of Std.Sort
            //                                              //      to avoid the use of Test before completing Std
            //                                              //      initialization process.

            Std.arrstrINT_PICTURE_SORTED = new String[Std.INTEGER_PICTURES.Length];
            Array.Copy(Std.INTEGER_PICTURES, 0, Std.arrstrINT_PICTURE_SORTED, 0, Std.INTEGER_PICTURES.Length);
            Array.Sort(Std.arrstrINT_PICTURE_SORTED, StringComparer.Ordinal);

            Std.arrstrNUM_PICTURE_SORTED = new String[Std.NUMBER_PICTURES.Length];
            Array.Copy(Std.NUMBER_PICTURES, 0, Std.arrstrNUM_PICTURE_SORTED, 0, Std.NUMBER_PICTURES.Length);
            Array.Sort(Std.arrstrNUM_PICTURE_SORTED, StringComparer.Ordinal);

            Std.strCHAR_USEFUL_IN_TEXT = " ";
            foreach (T2uccCategoryAndCharsTuple t2ucc in Std.arrt2uccCHAR_USEFUL_IN_TEXT)
            {
                Std.strCHAR_USEFUL_IN_TEXT = Std.strCHAR_USEFUL_IN_TEXT + t2ucc.strChars;
            }
            Std.CHARS_USEFUL_IN_TEXT = Std.strCHAR_USEFUL_IN_TEXT.ToCharArray();
            Array.Sort(Std.CHARS_USEFUL_IN_TEXT);
            Array.Sort(Std.arrt2charESCAPE);

            Std.arrstrTIME_PICTURE_SORTED = new String[Std.arrstrTIME_PICTURES.Length];
            Array.Copy(Std.arrstrTIME_PICTURES, 0, Std.arrstrTIME_PICTURE_SORTED, 0, Std.arrstrTIME_PICTURES.Length);
            Array.Sort(Std.arrstrTIME_PICTURE_SORTED, StringComparer.Ordinal);

            /*
            Std.arrstrTIME_ZONE_DISPLAY_NAME = new String[TimeZoneX.arrtimezoneSORTED.Length];
            for (int intI = 0; intI < TimeZoneX.arrtimezoneSORTED.Length; intI = intI + 1)
            {
                Std.arrstrTIME_ZONE_DISPLAY_NAME[intI] = TimeZoneX.arrtimezoneSORTED[intI].DisplayName;
            }
            */

            //                                              //VERIFY CONSTANTS.
            Std.subVerifyConstantsCharacter();
            Std.subVerifyConstantsInteger();
            Std.subVerifyConstantsNumber();
            Std.subVerifyConstantsTime();
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
