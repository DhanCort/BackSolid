/*TASK T2languagePictureTuple*/
using System;
//                                                          //To allow Towa's definition of some types (TimeZoneX, etc.)
//                                                          //      Any reference to System.Globalization type should be
//                                                          //      codes (Ex. System.Globalization.CultureInfo).

//                                                          //AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: 13-Mayo-2011.

namespace TowaStandard
{
    //==================================================================================================================
    public class T2languagePictureTuple : BtupleAbstract
    {
        //                                                  //Map special character description.

        //--------------------------------------------------------------------------------------------------------------
        public /*KEY*/ DateTextEnum datetext; 
        public String strPicture; 

        //--------------------------------------------------------------------------------------------------------------
        public override String ToLogShort()
        {
            return "<" + Test.ToLog(this.datetext) + ", " + this.strPicture.ToString() + ">";
        }

        //--------------------------------------------------------------------------------------------------------------
        public override String ToLogFull()
        {
            return "<" + Test.ToLog(this.datetext, "strLanguage") + ", " + "culture(" + this.strPicture.ToString() +
                   ")>";
        }

        //--------------------------------------------------------------------------------------------------------------
        public T2languagePictureTuple(DateTextEnum datetext_I, String strPicture_I)
        {
            this.datetext = datetext_I;
            this.strPicture = strPicture_I;
        }

        //--------------------------------------------------------------------------------------------------------------
        public override int CompareTo(Object objArgument_I)
        {
            int intCompareTo;
            /*CASE*/
            if (
                objArgument_I is T2languagePictureTuple
                )
            {
                intCompareTo = this.datetext.CompareTo(((T2languagePictureTuple)objArgument_I).datetext);
            }
            else if (
                objArgument_I is DateTextEnum
                )
            {
                intCompareTo = this.datetext.CompareTo(((String)objArgument_I));
            }
            else
            {
                Test.Abort(
                    Test.ToLog(objArgument_I.GetType(), "objArgument_I.type") +
                        " is not a compatible CompareTo argument, the options are: T2languagePictureTuple & DateTextEnum",
                    Test.ToLog(this.GetType(), "this.type"));

                intCompareTo = -1;
            }
            /*CASE*/

            return intCompareTo;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
