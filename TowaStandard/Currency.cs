/*TASK Currency*/
using System;
using System.Linq;

//                                                          //AUTHOR: Towa (IDL-Iñaki De Atela, SIM-Sergio Mercado).
//                                                          //CO-AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //DATE: 6-marzo-2019.

namespace TowaStandard
{
    //==================================================================================================================
    public struct Currency : BsysInterface, IComparable, SerializableInterface<Currency>
    {
        //                                                  //Currency is an structure which handles money information
        //                                                  //      (including 2 cents).

        //--------------------------------------------------------------------------------------------------------------
        /*CONSTANTS*/

        public static readonly Currency DummyValue = new Currency();

        public static readonly Currency Zero = new Currency(0);
        public static readonly Currency MinValue = new Currency(Int64.MinValue);
        public static readonly Currency MaxValue = new Currency(Int64.MaxValue);

        //                                                  //This range will be used to verify Literals
        private const long longDIGITS_MIN = Int64.MinValue / 100;
        private const long longDIGITS_MAX = Int64.MaxValue / 100;

        //                                                  //Valid number picture
        //                                                  //Initializer should order (ascending)
        //                                                  //Sorted array is used only to verify standard pictures.
        private static readonly String[] arrstrPICTURE_UNSORTED = {
            //                                              //Only digits
            "0", "0.00",
            //                                              //Include ,s and digits
            "#,##0", "#,##0.00",
            //                                              //Include ,'s and digits
            "#'###,##0", "#'###,##0.00"};
        private static readonly String[] arrstrPICTURE;

        //--------------------------------------------------------------------------------------------------------------
        /*INITIALIZER*/

        //--------------------------------------------------------------------------------------------------------------
        static Currency(
            )
        {
            arrstrPICTURE = new String[arrstrPICTURE_UNSORTED.Length];
            arrstrPICTURE = Std.ConcatenateArrays(arrstrPICTURE_UNSORTED);
            Std.Sort(arrstrPICTURE);
        }

        //--------------------------------------------------------------------------------------------------------------
        /*SUPPORT METHODS FOR INITIALIZER*/

        //--------------------------------------------------------------------------------------------------------------
        /*INSTANCE VARIABLES*/

        //                                                  //Currency info is hold in cents.
        //                                                  //Example: $1,000.32 ==> 100032
        public readonly long TotalCents;

        //--------------------------------------------------------------------------------------------------------------
        /*COMPUTED VARIABLES*/

        //                                                  //$123.45 should return 45.
        //                                                  //-$123.45 should return -45.
        public int Cents
        {
            get
            {
                return (int)(TotalCents % 100);
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public String ToLogShort()
        {
            return Test.ToLog(this);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public String ToLogFull()
        {
            return Test.ToLog(this);
        }

        //--------------------------------------------------------------------------------------------------------------
        /*OBJECT CONSTRUCTORS*/

        //--------------------------------------------------------------------------------------------------------------
        private Currency(
            //                                              //this.*[O], assing values. 

            long longTotalCents_I
            )
        {
            this.TotalCents = longTotalCents_I;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static Currency z_TowaPRIVATE_subConstructCurrency(
            //                                              //THIS METHOS WAS INCLUDED JUST TO MAKE CLEAR THIS IS A
            //                                              //      TOOL TO IMPLEMENTE TowaStandard (NOT INTENDED FOR
            //                                              //      DEVELOPER USE).
            //                                              //This method is called when "TryParseToCurrency" needs to
            //                                              //      construct an Currency.

            long longPrimaryKey_I
            )
        {
            return new Currency(longPrimaryKey_I);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static Currency Literal(
            //                                              //At most 13 digits and 2 decimals.
            //                                              //This parameter shuld always be a "literal" (never a
            //                                              //      variable, but this condition could not be verify. 
            double num_I
            )
        {
            if (
                !num_I.IsBetween(-9999999999999.99, 9999999999999.99)
                )
                Test.Abort(Test.ToLog(num_I, "num_I") + 
                    " should be in the range -9999999999999.99 to 9999999999999.99 (13 digits and 2 decimals)");
            if (
                num_I != num_I.Round(2)
                )
                Test.Abort(Test.ToLog(num_I, "num_I") + " should include at most cents (2 decimals)");

            long longTotalCents = (long)Math.Round(num_I * 100);
            return new Currency(longTotalCents);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static Currency Literal(
            //                                              //Currency (no cents)
            int int_I
            )
        {
            return new Currency(int_I * 100);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static Currency Literal(
            //                                              //Currency (no cents)
            long long_I
            )
        {
            if (
                !long_I.IsBetween(Currency.longDIGITS_MIN, Currency.longDIGITS_MAX)
                )
                Test.Abort(Test.ToLog(long_I, "long_I") + " should be in the range " + Currency.longDIGITS_MIN + 
                    " to " + Currency.longDIGITS_MAX);

            return new Currency(long_I * 100);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static Currency Literal(
            //                                              //Currency (with cents)
            long longDigits_I,
            int intCc_I
            )
        {
            if (
                intCc_I == 0
                )
                Test.Abort(Test.ToLog(intCc_I, "intCc_I") + " should use method with one parameter.");
            if (
                longDigits_I.IsBetween(-9999999999999, 9999999999999)
                )
                Test.Abort(Test.ToLog(longDigits_I, "longDigits_I") +
                    " should use method with one parameter, including a decimal point");
            if (
                !longDigits_I.IsBetween(Currency.longDIGITS_MIN, Currency.longDIGITS_MAX)
                )
                Test.Abort(Test.ToLog(longDigits_I, "longDigits_I") + " should be in the range " +
                    Currency.longDIGITS_MIN + " to " + Currency.longDIGITS_MAX);

            /*CASE*/
            if (
                longDigits_I == Currency.longDIGITS_MIN
                )
            {
                if (
                    //                                          //Cents should always be positive (sign is taken from
                    //                                          //      integer part)
                    !intCc_I.IsBetween(0, 8)
                    )
                    Test.Abort("For " + Test.ToLog(longDigits_I, "longDigits_I") + ", " +
                        Test.ToLog(intCc_I, "intCc_I") + " should be in the range 0 to 8");
            }
            else if (
                longDigits_I == Currency.longDIGITS_MAX
                )
            {
                if (
                    //                                          //Cents should always be positive (sign is taken from
                    //                                          //      integer part)
                    !intCc_I.IsBetween(0, 7)
                    )
                    Test.Abort("For " + Test.ToLog(longDigits_I, "longDigits_I") + ", " +
                        Test.ToLog(intCc_I, "intCc_I") + " should be in the range 0 to 7");
            }
            else
            {
                if (
                    //                                          //Cents should always be positive (sign is taken from
                    //                                          //      integer part)
                    !intCc_I.IsBetween(0, 99)
                    )
                    Test.Abort(Test.ToLog(intCc_I, "intCc_I") + " should be in the range 0 to 99");
            }
            /*END-CASE*/

            //                                                  //Take cents sign from integer part
            long longTotalCents = longDigits_I * 100 + Math.Sign(longDigits_I) * intCc_I;

            return new Currency(longTotalCents);
        }

        //--------------------------------------------------------------------------------------------------------------
        /*ACCESS METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public Currency Round(
            )
        {
            //                                                  //Need to extract 1 digit + cents, to apply banker's
            //                                                  //      rounding
            int intTotalCentsInOneDigitPlusCents = (int)(this.TotalCents % 1000);

            //                                                  //Round (banker's) 1 digit + cents
            double numOneDigitRounded = Math.Round((double)intTotalCentsInOneDigitPlusCents / 100.0);
            int intTotalCentsInOneDigitPlusCentsRounded = ((int)numOneDigitRounded) * 100;

            long longTotalCentsRounded =  this.TotalCents - intTotalCentsInOneDigitPlusCents +
                intTotalCentsInOneDigitPlusCentsRounded;

            //                                                  //OTRA OPCIÓN, sin convertir a double

            //                                                  //Need to extract the last 0 to 199 (+ or -)
            //                                                  //      rounding
            int intTotalCents000To199 = Math.Sign(this.TotalCents) * (int)(this.TotalCents % 200);

            //                                                  //Banker's rounding (get rid of cents).
            //                                                  //0-50 ==> 0, 51-149 ==> 100, 150-199 ==> 200
            int intTotalCents000To199Positive = Math.Sign(this.TotalCents) * (int)(this.TotalCents % 200);
            int intTotalCents000To199PositiveRounded = (intTotalCents000To199Positive <= 50) ? 0 :
                (intTotalCents000To199Positive < 150) ? 100 : 200;
            int intTotalCents000To199Rounded = Math.Sign(this.TotalCents) * intTotalCents000To199PositiveRounded;

            long longTotalCentsRoundedX = this.TotalCents - intTotalCents000To199 + intTotalCents000To199Rounded;

            long longZero = longTotalCentsRounded - longTotalCentsRoundedX;
            Test.Log("VERIFICAR ROUND " + Test.ToLog(longTotalCentsRounded, "longTotalCentsRounded") + ", " +
                Test.ToLog(longTotalCentsRounded, "longTotalCentsRounded") + ", " + Test.ToLog(longZero, "longZero"));

            return new Currency(longTotalCentsRounded);
        }   

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public Currency Ceiling(
            )
        {
            long longCents = (int)(this.TotalCents % 100);
            long longTotalCentsCeiling = (Math.Sign(this.TotalCents) < 0) ? this.TotalCents - longCents :
                this.TotalCents - longCents + 100;

            return new Currency(longTotalCentsCeiling);
        }
        
        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public Currency Floor(
            )
        {
            long longCents = (int)(this.TotalCents % 100);
            long longTotalCentsFloor = (Math.Sign(this.TotalCents) < 0) ? this.TotalCents - longCents + 100 :
                this.TotalCents - longCents;

            return new Currency(longTotalCentsFloor);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public Currency Truncate(
            )
        {
            long longCents = (int)(this.TotalCents % 100);
            long longTotalCentsTruncate = this.TotalCents - longCents;

            return new Currency(longTotalCentsTruncate);
        }

        //-------------------------------------------------------------------------------------------------------------
        public static Currency operator *(

            //                                              //time, new Currency adding seconds

            Currency curr_I,
            int intSeconds_I
            )
        {
            return new Currency(curr_I.TotalCents * intSeconds_I);
        }

        //-------------------------------------------------------------------------------------------------------------
        public static Currency operator *(

            //                                              //time, new Currency adding seconds

            Currency curr_I,
            long intSeconds_I
            )
        {
            return new Currency(curr_I.TotalCents * intSeconds_I);
        }

        //-------------------------------------------------------------------------------------------------------------
        public static Currency operator *(

            //                                              //time, new Currency adding seconds

            Currency curr_I,
            double intSeconds_I
            )
        {
            long longTotalCents = (long)Math.Round(intSeconds_I * 100);
            return new Currency(curr_I.TotalCents * longTotalCents);
        }

        //-------------------------------------------------------------------------------------------------------------
        public static Currency operator *(

            //                                              //time, new Currency adding seconds

            Currency curr_I,
            Currency intSeconds_I
            )
        {
            return new Currency(curr_I.TotalCents * intSeconds_I.TotalCents);
        }

        //-------------------------------------------------------------------------------------------------------------
        public static Currency operator /(

            //                                              //time, new Currency adding seconds

            Currency curr_I,
            int intSeconds_I
            )
        {
            return new Currency(curr_I.TotalCents / intSeconds_I);
        }

        //-------------------------------------------------------------------------------------------------------------
        public static Currency operator /(

            //                                              //time, new Currency adding seconds

            Currency curr_I,
            long intSeconds_I
            )
        {
            return new Currency(curr_I.TotalCents / intSeconds_I);
        }

        //-------------------------------------------------------------------------------------------------------------
        public static Currency operator /(

            //                                              //time, new Currency adding seconds

            Currency curr_I,
            double intSeconds_I
            )
        {
            long longTotalCents = (long)Math.Round(intSeconds_I * 100);
            return new Currency(curr_I.TotalCents / longTotalCents);
        }

        //-------------------------------------------------------------------------------------------------------------
        public static Currency operator /(

            //                                              //time, new Currency adding seconds

            Currency curr_I,
            Currency intSeconds_I
            )
        {
            return new Currency(curr_I.TotalCents / intSeconds_I.TotalCents);
        }

        //-------------------------------------------------------------------------------------------------------------
        public static Currency operator +(

            //                                              //time, new Currency adding seconds

            Currency curr_I,
            Currency intSeconds_I
            )
        {
            return new Currency(curr_I.TotalCents + intSeconds_I.TotalCents);
        }

        //-------------------------------------------------------------------------------------------------------------
        public static Currency operator -(

            //                                              //time, new Currency subtracting seconds

            Currency curr_I,
            Currency intSeconds_I
            )
        {
            return new Currency(curr_I.TotalCents - intSeconds_I.TotalCents);
        }

        //-------------------------------------------------------------------------------------------------------------
        public static bool operator <(
            Currency currLeft_I,
            Currency currRight_I
            )
        {
            return (
                currLeft_I.TotalCents < currRight_I.TotalCents
                );
        }

        //-------------------------------------------------------------------------------------------------------------
        public static bool operator <=(
            Currency currLeft_I,
            Currency currRight_I
            )
        {
            return (
                currLeft_I.TotalCents <= currRight_I.TotalCents
                );
        }

        //-------------------------------------------------------------------------------------------------------------
        public static bool operator >(
            Currency currLeft_I,
            Currency currRight_I
            )
        {
            return (
                currLeft_I.TotalCents > currRight_I.TotalCents
                );
        }

        //-------------------------------------------------------------------------------------------------------------
        public static bool operator >=(
            Currency currLeft_I,
            Currency currRight_I
            )
        {
            return (
                currLeft_I.TotalCents >= currRight_I.TotalCents
                );
        }

        //-------------------------------------------------------------------------------------------------------------
        public bool IsBetween(
            Currency currStart_I,
            Currency currEnd_I
            )
        {
            return this.TotalCents.IsBetween(currStart_I.TotalCents, currEnd_I.TotalCents);
        }

        //-------------------------------------------------------------------------------------------------------------
        public static bool operator ==(
            Currency currLeft_I,
            Currency currRight_I
            )
        {
            return (
                currLeft_I.TotalCents == currRight_I.TotalCents
                );
        }

        //-------------------------------------------------------------------------------------------------------------
        public static bool operator !=(
            Currency currLeft_I,
            Currency currRight_I
            )
        {
            return (
                currLeft_I.TotalCents != currRight_I.TotalCents
                );
        }

        //-------------------------------------------------------------------------------------------------------------
        public int Sign(
            )
        {
            return (
                Math.Sign(this.TotalCents)
                );
        }

        //-------------------------------------------------------------------------------------------------------------
        public Currency Abs(
            )
        {
            return (
                new Currency(Math.Abs(this.TotalCents))
                );
        }

        //--------------------------------------------------------------------------------------------------------------
        public String ToText(
            //                                              //str, integer to text.
            )
        {
            return this.ToText("#,##0.00");
        }

        //--------------------------------------------------------------------------------------------------------------
        public String ToText(
            //                                              //str, integer to text.

            String strPicture_I
            )
        {
            Test.AbortIfNull(strPicture_I, "strPicture_I");
            if (
                !strPicture_I.IsInSortedSet(Currency.arrstrPICTURE)
                )
                Test.Abort(Test.ToLog(strPicture_I, "strPicture_I") + " is not a valid picture",
                    Test.ToLog(Currency.arrstrPICTURE_UNSORTED, "CURRENCY_PICTURES"));

            //                                              //Gets quantity before cents
            long intTotalWhole = this.TotalCents / 100;

            //                                              //Formats depending on the picture
            String strToText = (strPicture_I.Contains('.')) ?
                intTotalWhole.ToString(strPicture_I.Substring(0, strPicture_I.IndexOf('.'))) + "." +
                    Math.Abs(this.Cents) :
                intTotalWhole.ToString(strPicture_I);

            return strToText;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool IsParsableToCurrency(
            //                                              //Verify if it is parsable to Currency.

            /*this*/ String str_I
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

            /*this*/ String str_I
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

            return new Currency(longTotalCents);
        }

        //--------------------------------------------------------------------------------------------------------------
        public int CompareTo(
            //                                              //Required for Sort, BinarySearch and CompareTo.

            //                                              //this[I], object key info.

            //                                              //syspath or str
            Object obj_L
            )
        {
            if (!(
                obj_L is Currency
                ))
                Test.Abort(Test.ToLog(obj_L.GetType(), "obj_L.GetType") + " should be Currency");

            int intTotalSecondsToCompare = 0;

            return (this.TotalCents < intTotalSecondsToCompare) ? -1 :
                (this.TotalCents > intTotalSecondsToCompare) ? 1 : 0;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //NEXT 2 METHODS ARE TO AVOID COMPILE WARNINGS.

        //--------------------------------------------------------------------------------------------------------------
        public override bool Equals(
            Object obj_L
            )
        {
            if (!(
                obj_L is Currency
                ))
                Test.Abort(Test.ToLog(obj_L.GetType(), "obj_L.GetType") + " should be Currency");

            Currency timeRight = (Currency)obj_L;

            return (
                this == timeRight
                );
        }

        //--------------------------------------------------------------------------------------------------------------
        public override int GetHashCode()
        {
            return this.GetHashCode();
        }

        //--------------------------------------------------------------------------------------------------------------
        public byte[] Serialize(
            //                                              //Returns a serialized version of the object.

            )
        {
            Int64 intTotalCents = this.TotalCents;
            return BitConverter.GetBytes(intTotalCents);
        }

        //--------------------------------------------------------------------------------------------------------------
        public void Deserialize(
            //                                              //Returns a deserialized object.

            //                                              //The object to deserialize.
            out Currency Currency_O,
            //                                              //The serialized bytes.
            ref byte[] Bytes_IO
            )
        {
            byte[] arrbyteCurrency = new byte[8];
            Array.Copy(Bytes_IO, 0, arrbyteCurrency, 0, 8);
            Int64 intTotalCents = BitConverter.ToInt64(arrbyteCurrency, 0);

            Bytes_IO = Bytes_IO.Skip(8).ToArray();

            Currency_O = new Currency(intTotalCents);
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
