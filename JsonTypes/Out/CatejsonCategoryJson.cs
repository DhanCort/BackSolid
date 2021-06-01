/*TASK RP.JDF*/
using Newtonsoft.Json;
using System;

//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: November 21, 2019. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class CatejsonCategoryJson : TjsonTJson
    {
        public String StrCategory { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public CatejsonCategoryJson(
            String strCategory_I
            )
        {
            this.StrCategory = strCategory_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
