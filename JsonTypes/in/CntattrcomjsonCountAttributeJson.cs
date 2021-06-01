/*TASK RP.JDF*/
using System;
//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia Aguazul).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierre).
//                                                          //DATE: January 6, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class CntattrcomjsonCountAttributeJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public String strXJDFMediaType { get; set; }
        public String strXJDFStockType { get; set; }
        public String strXJDFWeight { get; set; }
        public String strXJDFWidth { get; set; }
        public String strXJDFLength { get; set; }
        public String strXJDFDimensionUnit { get; set; }
        public int intCount { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public CntattrcomjsonCountAttributeJson(
            //                                              //Receive attribute from template.
            //                                              //    These attrbute are properties that is common 
            //                                              //    with several resources.
            //                                              //These attribute are considered how a Unit.
            //                                              //Var intCount is used for count Units. 

            String strXJDFMediaType_I,
            String strXJDFStockType_I,
            String strXJDFWeight_I,
            String strXJDFWidth_I,
            String strXJDFLength_I,
            String strXJDFDimensionUnit_I,
            int intCount_I
            )
        {
            this.strXJDFMediaType = strXJDFMediaType_I;
            this.strXJDFStockType = strXJDFStockType_I;
            this.strXJDFWeight = strXJDFWeight_I;
            this.strXJDFWidth = strXJDFWidth_I;
            this.strXJDFLength = strXJDFLength_I;
            this.strXJDFDimensionUnit = strXJDFDimensionUnit_I;
            this.intCount = intCount_I;
        }
    }

    //==================================================================================================================
}
/*END-TASK*/
