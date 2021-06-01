/*TASK T2uccCategoryAndCharsTuple*/
using System;

//                                                          //AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: 16-Febrero-2016.
//                                                          //PURPOSE:
//                                                          //Implementación de funciones o subrutinas de uso compartido
//                                                          //      en todos los sistemas.

namespace TowaStandard
{
    //==================================================================================================================
    public class T2uccCategoryAndCharsTuple : BtupleAbstract
    {
        //                                                  //Map Unicode Category to characters.

        //--------------------------------------------------------------------------------------------------------------
        public /*KEY*/ UccUnicodeCategoryEnum ucc;
        public String strChars;

        //--------------------------------------------------------------------------------------------------------------
        public override String ToLogShort()
        {
            return "<" + Test.ToLog(this.ucc) + ", " + Test.ToLog(this.strChars) + ">";
        }

        //--------------------------------------------------------------------------------------------------------------
        public override String ToLogFull()
        {
            return "<" + Test.ToLog(this.ucc, "ucc") + ", " + Test.ToLog(this.strChars, "strChars") + ">";
        }

        //--------------------------------------------------------------------------------------------------------------
        public T2uccCategoryAndCharsTuple(UccUnicodeCategoryEnum ucc_I, String strChars_I)
            : base()
        {
            this.ucc = ucc_I;
            this.strChars = strChars_I;
        }

        //--------------------------------------------------------------------------------------------------------------
        public override int CompareTo(Object objArgument_I)
        {
            int intCompareTo;
            /*CASE*/
            if (
                objArgument_I is T2uccCategoryAndCharsTuple
                )
            {
                intCompareTo = this.ucc.CompareTo(((T2uccCategoryAndCharsTuple)objArgument_I).ucc);
            }
            else if (
                objArgument_I is UccUnicodeCategoryEnum
                )
            {
                intCompareTo = this.ucc.CompareTo(((UccUnicodeCategoryEnum)objArgument_I));
            }
            else
            {
                Test.Abort(
                    Test.ToLog(objArgument_I.GetType(), "objArgument_I.type") +
                        " is not a compatible CompareTo argument, the options are: t2ucc & ucc",
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
