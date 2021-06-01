/*TASK RP.JSON*/
using System;
using System.Collections.Generic;
using TowaStandard;

//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia Aguazul).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: January 20, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //==================================================================================================================  
    public class ToorderjsoninToOrderJsonInternal
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intJobId { get; set; }
        public int intContactId { get; set; }
        public int? intnPrintshopId { get; set; }
        public int? intnQty1 { get; set; }
        public double? numnPrice1 { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //--------------------------------------------------------------------------------------------------------------
        public ToorderjsoninToOrderJsonInternal(
            int intJobId_I,
            int intContactId_I,
            int? intnPrintshopId_I,
            int? intnQty1_I,
            double? numnPrice1_I
            )
        {
            this.intJobId = intJobId_I;
            this.intContactId = intContactId_I;
            this.intnPrintshopId = intnPrintshopId_I;
            this.numnPrice1 = numnPrice1_I;
            this.intnQty1 = intnQty1_I;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
