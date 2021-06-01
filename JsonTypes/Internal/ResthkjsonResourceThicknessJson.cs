/*TASK RP.JDF*/
using System;

//                                                          //AUTHOR: Towa (JLBD - Luis Basurto).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: March 25, 2021. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class ResthkjsonResourceThicknessJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.


        public int? intnPkEleet { get; set; }
        public int? intnPkEleele { get; set; }
        public int? intnPkCalculation { get; set; }
        public int intPkProcessInWorkflow { get; set; }
        public int intPkResource { get; set; }
        public int intPkTypeResource { get; set; }
        public bool boolIsMedia { get; set; }
        public bool boolIsComponent { get; set; }
        public double? numnThickness { get; set; }
        public String strThicknessUnit { get; set; }
        public double? numnWidth { get; set; }
        public String strWidthUnit { get; set; }
        public double? numnLength { get; set; }
        public String strLengthUnit { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public ResthkjsonResourceThicknessJson(
            int? intnPkEleet_I,
            int? intnPkEleele_I,
            int? intnPkCalculation_I,
            int intPkProcessInWorkflow_I,
            int intPkResource_I,
            bool boolIsMedia_I,
            bool boolIsComponent_I,
            double? numnThickness_I,
            String strThicknessUnit_I,
            double? numnWidth_I,
            String strWidthUnit_I,
            double? numnLength_I,
            String strLengthUnit_I
            )
        {
            this.intnPkEleet = intnPkEleet_I;
            this.intnPkEleele = intnPkEleele_I;
            this.intnPkCalculation = intnPkCalculation_I;
            this.intPkProcessInWorkflow = intPkProcessInWorkflow_I;
            this.intPkResource = intPkResource_I;
            this.boolIsComponent = boolIsComponent_I;
            this.boolIsMedia = boolIsMedia_I;
            this.numnThickness = numnThickness_I;
            this.strThicknessUnit = strThicknessUnit_I;
            this.numnWidth = numnWidth_I;
            this.strWidthUnit = strWidthUnit_I;
            this.numnLength = numnLength_I;
            this.strLengthUnit = strLengthUnit_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
