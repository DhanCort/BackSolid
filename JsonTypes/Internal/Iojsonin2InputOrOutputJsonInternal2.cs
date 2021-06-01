/*TASK RP.DB_Odyssey2 */
using System;

//                                                          //AUTHOR: Towa (JLBD-Luis Basurto).
//                                                          //CO-AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //DATE: November 26, 2020.

namespace Odyssey2Backend.JsonTypes
{ 

    //==================================================================================================================
    public class Iojsonin2InputOrOutputJsonInternal2
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPk { get; set; }
        public int intPkWorkflow { get; set; }
        public int? intnPkElementElementType { get; set; }
        public int? intnPkElementElement { get; set; }
        public int? intnPkResource { get; set; }
        public String strLink { get; set; }
        public int? intnGroupResourceId { get; set; }
        public bool? boolnIsFinalProduct { get; set; }
        public int? intnProcessInWorkflowId { get; set; }
        public bool? boolnSize { get; set; }
        public bool? boolnThickness { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //--------------------------------------------------------------------------------------------------------------
        public Iojsonin2InputOrOutputJsonInternal2(
            int intPk_I,
            int intPkWorkflow_I,
            int? intnPkElementElementType_I,
            int? intnPkElementElement_I,
            int? intnPkResource_I,
            String strLink_I,
            int? intnGroupResourceId_I,
            bool? boolnIsFinalProduct_I,
            int? intnProcessInWorkflowId_I,
            bool? boolnSize_I,
            bool? boolnThickness_I
            )
        {
            this.intPk = intPk_I;
            this.intPkWorkflow = intPkWorkflow_I;
            this.intnPkElementElementType = intnPkElementElementType_I;
            this.intnPkElementElement = intnPkElementElement_I;
            this.intnPkResource = intnPkResource_I;
            this.strLink = strLink_I;
            this.intnGroupResourceId = intnGroupResourceId_I;
            this.boolnIsFinalProduct = boolnIsFinalProduct_I;
            this.intnProcessInWorkflowId = intnProcessInWorkflowId_I;
            this.boolnSize = boolnSize_I;
            this.boolnThickness = boolnThickness_I;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
