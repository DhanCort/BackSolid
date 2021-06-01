/*TASK RP.JDF*/
using System;
//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia Aguazul).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierre).
//                                                          //DATE: January 6, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class AttrrestempjsonAttributeResourceTemplateJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public String strTemplateName { get; set; }
        public String strResourceName { get; set; }
        public String strXJDFDescriptiveName { get; set; }
        public String strXJDFMediaColorName { get; set; }
        public String strXJDFMediaType { get; set; }
        public String strXJDFStockType { get; set; }
        public String strXJDFWeight { get; set; }
        public String strXJDFWidth { get; set; }
        public String strXJDFLength { get; set; }
        public String strXJDFDimensionUnit { get; set; }
        public bool boolPriceTemplate { get; set; }
        public String strPricePerThousand { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public AttrrestempjsonAttributeResourceTemplateJson(
            String strTemplateName_I,
            String strResourceName_I,
            String strXJDFDescriptiveName_I,
            String strXJDFMediaColorName_i, 
            String strXJDFMediaType_I,
            String strXJDFStockType_I,
            String strXJDFWeight_I, 
            String strXJDFWidth_I,
            String strXJDFLength_I,
            String strXJDFDimensionUnit_I,
            bool boolPriceTemplate_I,
            String strPricePerThousand_I
            )
        {
            this.strTemplateName = strTemplateName_I;
            this.strResourceName = strResourceName_I;
            this.strXJDFDescriptiveName = strXJDFDescriptiveName_I;
            this.strXJDFMediaColorName = strXJDFMediaColorName_i;
            this.strXJDFMediaType = strXJDFMediaType_I;
            this.strXJDFStockType = strXJDFStockType_I;
            this.strXJDFWeight = strXJDFWeight_I;
            this.strXJDFWidth = strXJDFWidth_I;
            this.strXJDFLength = strXJDFLength_I;
            this.strXJDFDimensionUnit = strXJDFDimensionUnit_I;
            this.boolPriceTemplate = boolPriceTemplate_I;
            this.strPricePerThousand = strPricePerThousand_I;
        }
    }

    //==================================================================================================================
}
/*END-TASK*/
