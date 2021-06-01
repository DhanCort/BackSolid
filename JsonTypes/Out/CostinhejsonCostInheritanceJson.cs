/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: March 17, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //==================================================================================================================  
    public class CostinhejsonCostInheritanceJson
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public double? numnCost { get; set; }
        public double? numnQuantity { get; set; }
        public double? numnMin { get; set; }
        public double? numnBlock { get; set; }
        public bool? boolnIsInherited { get; set; }
        public bool? boolnIsChangeable { get; set; }
        public int? intnPkAccount { get; set; }
        public String strAccountName { get; set; }
        public double? numnHourlyRate { get; set; }
        public bool? boolnArea { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //--------------------------------------------------------------------------------------------------------------
        public CostinhejsonCostInheritanceJson(
            double? numnCost_I,
            double? numnQuantity_I,
            double? numnMin_I,
            double? numnBlock_I,
            bool? boolnIsInherited_I,
            bool? boolnIsChangeable_I,
            int? intnPkAccount_I,
            String strAccountName_I,
            double? numnHourlyRate_I,
            bool? boolnArea_I
            )
        {
            this.numnCost = numnCost_I;
            this.numnQuantity = numnQuantity_I;
            this.numnMin = numnMin_I;
            this.numnBlock = numnBlock_I;
            this.boolnIsInherited = boolnIsInherited_I;
            this.boolnIsChangeable = boolnIsChangeable_I;
            this.intnPkAccount = intnPkAccount_I;
            this.strAccountName = strAccountName_I;
            this.numnHourlyRate = numnHourlyRate_I;
            this.boolnArea = boolnArea_I;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
