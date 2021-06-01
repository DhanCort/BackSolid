/*TASK RP.JDF*/
using System;
using System.Collections.Generic;
//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: January 19, 2021. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class EstimjsonEstimateJson : IComparable
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intJobId { get; set; }
        public String strJobTicket { get; set; }
        public String strProductName { get; set; }
        public String dateLastUpdate { get; set; }
        public String strCustomerName { get; set; }
        public int intPkProduct { get; set; }
        public int? intnPkWorkflow { get; set; }
        public String strEstimateNumber { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public EstimjsonEstimateJson(
            int intJobId_I,
            String strJobTicket_I,
            String strProductName_I,
            String dateLastUpdate_I,
            String strCustomerName_I,
            int intPkProduct_I,
            int? intnPkWorkflow_I,
            String strEstimateNumber_I
            )
        {
            this.intJobId = intJobId_I;
            this.strJobTicket = strJobTicket_I;
            this.strProductName = strProductName_I;
            this.dateLastUpdate = dateLastUpdate_I;
            this.intPkProduct = intPkProduct_I;
            this.intnPkWorkflow = intnPkWorkflow_I;
            this.strCustomerName = strCustomerName_I;
            this.strEstimateNumber = strEstimateNumber_I;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public int CompareTo(
            Object obj_I
            )
        {
            //                                              //This Estimatenumber

            int intThisEstimateNumber = Int32.Parse(this.strEstimateNumber.TrimStart());

            EstimjsonEstimateJson estimjson = (EstimjsonEstimateJson)obj_I;
            int intNewEstimateNumber = Int32.Parse(estimjson.strEstimateNumber.TrimStart());

            int intSortValue;
            //                                              //SORT EstimateNumber
            //                                              //Compare the EstimateNumber
            int intEstimateNumber = intThisEstimateNumber.CompareTo(intNewEstimateNumber);
            if (
                //                                          //This EstimateNumber equals new EstimateNumber
                intEstimateNumber == 0
                )
            {
                intSortValue = 0;
            }
            else if (
                //                                          //This EstimateNumber is less than new EstimateNumber
                intEstimateNumber < 0
                )
            {
                //                                          //For descending EstimateNumber
                intSortValue = 1;
            }
            else
            //                                              //This EstimateNumber is greater than new EstimateNumber
            {
                //                                          //For descending EstimateNumber
                intSortValue = -1;
            }

            return intSortValue;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
