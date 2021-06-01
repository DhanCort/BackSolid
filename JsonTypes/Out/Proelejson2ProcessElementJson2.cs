/*TASK RP.JDF*/
using System;

//                                                          //AUTHOR: Towa (CCC - Cesar Cigarroa).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: April 1, 2020.  

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class Proelejson2ProcessElementJson2
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPk { get; set; }
        public String strElementName { get; set; }
        public Restyportemjson1ResourceTypeOrTemplateJson1[] arrrestyportemInput { get; set; }
        public Restyportemjson1ResourceTypeOrTemplateJson1[] arrrestyportemOutput { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------

        public Proelejson2ProcessElementJson2(
            int intPk_I,
            String strElementName_I,
            Restyportemjson1ResourceTypeOrTemplateJson1[] arrrestyportemInput_I,
            Restyportemjson1ResourceTypeOrTemplateJson1[] arrrestyportemOutput_I
            )
        {
            this.intPk = intPk_I;
            this.strElementName = strElementName_I;
            this.arrrestyportemInput = arrrestyportemInput_I;
            this.arrrestyportemOutput = arrrestyportemOutput_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
