/*TASK T3fakecharFakeCharacterTuple*/
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
    public class T3fakecharTuple : BtupleAbstract
    {
        //                                                  //Map fake character to description.
        //                                                  //A "fake character" is a character thats looks like "other"
        //                                                  //      but its code is diferente.
        //                                                  //Ej. there exist several character that on screen and
        //                                                  //      printer looks IDENTICAL to A but they ARE NOT the
        //                                                  //      same "A" that you get when you key "A" on your
        //                                                  //      keyboard.
        //                                                  //THERE ARE MANY "FAKE" CHARACTERS.

        //--------------------------------------------------------------------------------------------------------------
        public /*KEY*/ char charFAKE;
        //                                                  //This should be equal to charFake, but should be "enter" in
        //                                                  //      \x???? format.
        //                                                  //The intention is to "see" the hexadecimal code of
        //                                                  //      the character.
        public char charHEX;
        //                                                  //Form: "Au????) Fake"
        public String strDESCRIPTION;

        //--------------------------------------------------------------------------------------------------------------
        public override String ToLogShort()
        {
            return "<" + Test.ToLog(this.charFAKE) + ", " + Test.ToLog(this.charHEX) + ", " +
                Test.ToLog(this.strDESCRIPTION) + ">";
        }

        //--------------------------------------------------------------------------------------------------------------
        public override String ToLogFull()
        {
            return "<" + Test.ToLog(this.charFAKE, "charFAKE") + ", " + Test.ToLog(this.charHEX, "charHEX") + ", " +
                Test.ToLog(this.strDESCRIPTION, "strDESCRIPTION") + ">";
        }

        //--------------------------------------------------------------------------------------------------------------
        public T3fakecharTuple(char charFAKE_I, char charHEX_I, String strDESCRIPTION_I)
            : base()
        {
            this.charFAKE = charFAKE_I;
            this.charHEX = charHEX_I;
            this.strDESCRIPTION = strDESCRIPTION_I;
        }

        //--------------------------------------------------------------------------------------------------------------
        public override int CompareTo(Object objArgument_I)
        {
            int intCompareTo;
            /*CASE*/
            if (
                objArgument_I is T3fakecharTuple
                )
            {
                intCompareTo = this.charFAKE.CompareTo(((T3fakecharTuple)objArgument_I).charFAKE);
            }
            else if (
                objArgument_I is char
                )
            {
                intCompareTo = this.charFAKE.CompareTo((char)objArgument_I);
            }
            else
            {
                Test.Abort(
                    Test.ToLog(objArgument_I.GetType(), "objArgument_I.type") +
                        " is not a compatible CompareTo argument, the options are: t3fakechar & char",
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
