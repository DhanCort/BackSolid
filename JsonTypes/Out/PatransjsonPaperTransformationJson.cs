/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (CCC - Cesar Cigarroa).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: September 23, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class PatransjsonPaperTransformationJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPkPatrans { get; set; }
        public double numWidth { get; set; }
        public double? numnHeight { get; set; }
        public double numCutWidth { get; set; }
        public double numCutHeight { get; set; }
        public double? numnMarginTop { get; set; }
        public double? numnMarginBottom { get; set; }
        public double? numnMarginLeft { get; set; }
        public double? numnMarginRight { get; set; }
        public double? numnVerticalGap { get; set; }
        public double? numnHorizontalGap { get; set; }
        public String strUnit { get; set; }
        public bool boolInputIsChangeable { get; set; }
        public RowcutdatajsonRowCutDataJson[] arrrow { get; set; }
        public bool boolIsReversed { get; set; }
        public bool boolIsOptimized { get; set; }
        public bool boolIsPostSize { get; set; }
        public bool boolCut { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public PatransjsonPaperTransformationJson(
            int intPkPatrans_I,
            double numWidth_I,
            double? numnHeight_I,
            double numCutWidth_I,
            double numCutHeight,
            double? numnMarginTop,
            double? numnMarginBottom,
            double? numnMarginLeft,
            double? numnMarginRight,
            double? numnVerticalGap,
            double? numnHorizontalGap,
            String strUnit_I,
            bool boolInputIsChangeable_I,
            RowcutdatajsonRowCutDataJson[] arrrow_I,
            bool boolIsReversed_I,
            bool boolIsOptimized_I,
            bool boolIsPostSize_I,
            bool boolCut_I
            )
        {
            this.intPkPatrans = intPkPatrans_I;
            this.numWidth = numWidth_I;
            this.numnHeight = numnHeight_I;
            this.numCutWidth = numCutWidth_I;
            this.numCutHeight = numCutHeight;
            this.numnMarginTop = numnMarginTop;
            this.numnMarginBottom = numnMarginBottom;
            this.numnMarginLeft = numnMarginLeft;
            this.numnMarginRight = numnMarginRight;
            this.numnVerticalGap = numnVerticalGap;
            this.numnHorizontalGap = numnHorizontalGap;
            this.strUnit = strUnit_I;
            this.boolInputIsChangeable = boolInputIsChangeable_I;
            this.arrrow = arrrow_I;
            this.boolIsReversed = boolIsReversed_I;
            this.boolIsOptimized = boolIsOptimized_I;
            this.boolIsPostSize = boolIsPostSize_I;
            this.boolCut = boolCut_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
