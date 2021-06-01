/*TASK RP.JSON*/
using System;

//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia Aguazul).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: January 20, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //==================================================================================================================  
    public class SndestjsoninSendEstimateJsonInternal
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intJobId { get; set; }
        public int intContactId { get; set; }
        public double? numnPrice1 { get; set; }
        public double? numnPrice2 { get; set; }
        public double? numnPrice3 { get; set; }
        public int? intnQty1 { get; set; }
        public int? intnQty2 { get; set; }
        public int? intnQty3 { get; set; }
        public String Notes { get; set; }
        public String Terms { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //--------------------------------------------------------------------------------------------------------------
        public SndestjsoninSendEstimateJsonInternal(
            int intJobId_I,
            int intContactId_I,
            double? numnPrice1_I,
            double? numnPrice2_I,
            double? numnPrice3_I,
            int? intnQty1_I,
            int? intnQty2_I,
            int? intnQty3_I, 
            String strNotes_I,
            String strTerms_I
            )
        {
            this.intJobId = intJobId_I;
            this.intContactId = intContactId_I;
            this.numnPrice1 = numnPrice1_I;
            this.numnPrice2 = numnPrice2_I;
            this.numnPrice3 = numnPrice3_I;
            this.intnQty1 = intnQty1_I;
            this.intnQty2 = intnQty2_I;
            this.intnQty3 = intnQty3_I;
            this.Notes = strNotes_I;
            this.Terms = strTerms_I;
        }

        //--------------------------------------------------------------------------------------------------------------
        public SndestjsoninSendEstimateJsonInternal(
            )
        {
            
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
