/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: February 24, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class TyportemjsonTypeOrTemplateJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public PejsonPathElementJson[] arrpePathElement { get; set; }
        public Attrjson10AttributeJson10[] arrattr { get; set; }
        public ResortemjsonResourceOrTemplateJsonsourceJson[] arrtem { get; set; }
        public ResortemjsonResourceOrTemplateJsonsourceJson[] arrres { get; set; }
        public bool boolIsPhysical { get; set; }
        public bool boolIDeviceToolOrCustom { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public TyportemjsonTypeOrTemplateJson(
            PejsonPathElementJson[] arrpePathElement_I,
            Attrjson10AttributeJson10[] arrattr_I,
            ResortemjsonResourceOrTemplateJsonsourceJson[] arrtem_I,
            ResortemjsonResourceOrTemplateJsonsourceJson[] arrres_I,
            bool boolIsPhysical_I,
            bool boolIsDeviceToolOrCustom_I
            )
        {
            this.arrpePathElement = arrpePathElement_I;
            this.arrattr = arrattr_I;
            this.arrtem = arrtem_I;
            this.arrres = arrres_I;
            this.boolIsPhysical = boolIsPhysical_I;
            this.boolIDeviceToolOrCustom = boolIsDeviceToolOrCustom_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
