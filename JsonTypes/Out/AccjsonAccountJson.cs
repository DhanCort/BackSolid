/*TASK RP.JSON*/
using System;

//                                                          //AUTHOR: Towa (CCC - Cesar Cigarroa).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: October 30, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class AccjsonAccountJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPk { get; set; }
        public String strNumber { get; set; }
        public String strName { get; set; }
        public int intPkType { get; set; }
        public String strTypeName { get; set; }
        public bool boolEnabled { get; set; }
        public bool boolIsGeneric { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public AccjsonAccountJson(
            int intPk_I,
            String strNumber_I,
            String strName_I,
            int intPkType_I,
            String strType_Name_I, 
            bool boolEnabled_I, 
            bool boolIsGeneric_I
            )
        {
            this.intPk = intPk_I;
            this.strNumber = strNumber_I;
            this.strName = strName_I;
            this.intPkType = intPkType_I;
            this.strTypeName = strType_Name_I;
            this.boolEnabled = boolEnabled_I;
            this.boolIsGeneric = boolIsGeneric_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
