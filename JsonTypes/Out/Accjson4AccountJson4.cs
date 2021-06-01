/*TASK RP.JSON*/
using System;

//                                                          //AUTHOR: Towa (JLBD - Luis Basurto).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: December 21, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class Accjson4AccountJson4
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        //-------------------------------------------------------------------------------------------------------------

        public int intPk { get; set; }
        public String strNumber { get; set; }
        public String strName { get; set; }
        public int intPkType { get; set; }


        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public Accjson4AccountJson4(
            int intPk_I,
            String strNumber_I,
            String strName_I,
            int intPkType_I
            )
        {
            this.intPk = intPk_I;
            this.strNumber = strNumber_I;
            this.strName = strName_I;
            this.intPkType = intPkType_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
