/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: March 17, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //==================================================================================================================  
    public class AvainhejsonAvailabilityInheritanceJson
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.
        public bool? boolnIsCalendar { get; set; }
        public bool? boolnIsInherited { get; set; }
        public bool? boolnIsChangeable { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //--------------------------------------------------------------------------------------------------------------
        public AvainhejsonAvailabilityInheritanceJson(
            bool? boolnIsCalendar_I,
            bool? boolIsInherited_I,
            bool? boolnIsChangeable_I
            )
        {
            this.boolnIsCalendar = boolnIsCalendar_I;
            this.boolnIsInherited = boolIsInherited_I;
            this.boolnIsChangeable = boolnIsChangeable_I;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
