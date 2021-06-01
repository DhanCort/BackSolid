/*TASK T2CharDescriptionTuple*/
using System;

//                                                          //AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: 13-Mayo-2011.
//                                                          //PURPOSE:
//                                                          //Implementación de funciones o subrutinas de uso compartido
//                                                          //      en todos los sistemas.

namespace TowaStandard
{
    //==================================================================================================================
    public class T2charDescriptionTuple : BtupleAbstract
    {
        //                                                  //Map special character description.

        //--------------------------------------------------------------------------------------------------------------
        public /*KEY*/ char charX;
        public String strDESCRIPTION;

        //--------------------------------------------------------------------------------------------------------------
        public override String ToLogShort()
        {
            return "<" + Test.ToLog(this.charX) + ", " + Test.ToLog(this.strDESCRIPTION) + ">";
        }

        //--------------------------------------------------------------------------------------------------------------
        public override String ToLogFull()
        {
            return "<" + Test.ToLog(this.charX, "charX") + ", " + Test.ToLog(this.strDESCRIPTION, "strDESCRIPTION") + ">";
        }

        //--------------------------------------------------------------------------------------------------------------
        public T2charDescriptionTuple(char charCHAR_I, String strDESCRIPTION_I) : base()
        {
            this.charX = charCHAR_I;
            this.strDESCRIPTION = strDESCRIPTION_I;
        }

        //--------------------------------------------------------------------------------------------------------------
        public override int CompareTo(Object objArgument_I)
        {
            int intCompareTo;
            /*CASE*/
            if (
                objArgument_I is T2charDescriptionTuple
                )
            {
                intCompareTo = this.charX.CompareTo(((T2charDescriptionTuple)objArgument_I).charX);
            }
            else if (
                objArgument_I is char
                )
            {
                intCompareTo = this.charX.CompareTo(((char)objArgument_I));
            }
            else
            {
                Test.Abort(
                    Test.ToLog(objArgument_I.GetType(), "objArgument_I.type") +
                        " is not a compatible CompareTo argument, the options are: t2char & char",
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
