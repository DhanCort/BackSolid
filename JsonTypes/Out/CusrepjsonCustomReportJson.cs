/*TASK RP.XJDF*/
using Odyssey2Backend.Utilities;
using System;
using System.Text.Json;
using TowaStandard;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: October 27, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //==================================================================================================================  
    public class CusrepjsonCustomReportJson
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPk { get; set; }
        public String strName { get; set; }
        public JsonElement filter { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //--------------------------------------------------------------------------------------------------------------
        public CusrepjsonCustomReportJson(
            int intPk_I,
            String strName_I,
            JsonElement filter_I
            )
        {
            this.intPk = intPk_I;
            this.strName = strName_I;
            this.filter = filter_I;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
