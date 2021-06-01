/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: March 17, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //==================================================================================================================  
    public class UnitinhejsonUnitInheritanceJson
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.
        public String strValue { get; set; }
        public bool? boolnIsInherited { get; set; }
        public bool? boolnIsChangeable { get; set; } 
        public bool? boolnIsDecimal { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //--------------------------------------------------------------------------------------------------------------
        public UnitinhejsonUnitInheritanceJson(
            String strValue_I,
            bool? boolnIsInherited_I,
            bool? boolnIsChangeable_I,
            bool? boolnIsDecimal_I
            )
        {
            this.strValue = strValue_I;
            this.boolnIsInherited = boolnIsInherited_I;
            this.boolnIsChangeable = boolnIsChangeable_I;
            this.boolnIsDecimal = boolnIsDecimal_I;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
