/*TASK Database Database All-in-Memory*/
using System;

//                                                          //AUTHOR: Towa (LGCR-Leoncio Chiunty).
//                                                          //CO-AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //DATE: October 22, 2019.

namespace TowaStandard
{
    //==================================================================================================================
    public sealed class JsonPersonName : JsonAbstract
    {
        //--------------------------------------------------------------------------------------------------------------
        /*INSTANCE VARIABLES*/

        public String given;
        public String family;

        //--------------------------------------------------------------------------------------------------------------
        public override String ToLogShort()
        {
            return "{" + Test.ToLog(this.given) + ", " + Test.ToLog(this.family) + "}";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public override String ToLogFull()
        {
            return "{" + Test.ToLog(this.given, "given") + ", " + Test.ToLog(this.family, "family") + "}";
        }

        //--------------------------------------------------------------------------------------------------------------
        /*OBJECT CONSTRUCTORS*/

        //--------------------------------------------------------------------------------------------------------------
        public JsonPersonName(

            PersonName pname_I
            )
            : base()
        {
            this.given = pname_I.given;
            this.family = pname_I.family;
        }

        //--------------------------------------------------------------------------------------------------------------
        public JsonPersonName(

            String given_I,
            String family_I
            )
            : base()
        {
            this.given = given_I;
            this.family = family_I;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
