/*TASK RP.JSON*/
using Odyssey2Backend.Utilities;
using System;
using TowaStandard;

//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia Aguazul).
//                                                          //CO-AUTHOR: Towa (LGF -Liliana Gutierrez).
//                                                          //DATE: March 31, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class EqvjsoninEquivalenceJsonInternal
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public double numEqvTop { get; set; }
        public String strEqvUnitTop { get; set; }
        public double numEqvBottom { get; set; }
        public String strEqvUnitBottom { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public EqvjsoninEquivalenceJsonInternal(
            
            double numEqvTop_I,
            String strEqvUnitTop_I,
            double numEqvBottom_I,
            String strEqvUnitBottom_I
            )
        {
            this.numEqvTop = numEqvTop_I;
            this.strEqvUnitTop = strEqvUnitTop_I;
            this.numEqvBottom = numEqvBottom_I;
            this.strEqvUnitBottom = strEqvUnitBottom_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
