/*TASK RP.JSON*/
using System;

//                                                          //AUTHOR: Towa (JLBD - Luis Basurto).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: October 27, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class RepftrjsonReportFilterJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int? intPk { get; set; }
        public String strName { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public RepftrjsonReportFilterJson(
            int intPk_I,
            String strName_I
            )
        {
            this.intPk = intPk_I;
            this.strName = strName_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //=================================================================================================================  
    public class RepjsonReportsFiltersJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public RepftrjsonReportFilterJson[] arrrepPrintshop { get; set; }
        public RepftrjsonReportFilterJson[] arrrepReadyToUse { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public RepjsonReportsFiltersJson(
            RepftrjsonReportFilterJson[] arrrepPrintshop_I,
            RepftrjsonReportFilterJson[] arrrepReadyToUse_I
            )
        {
            this.arrrepPrintshop = arrrepPrintshop_I;
            this.arrrepReadyToUse = arrrepReadyToUse_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
