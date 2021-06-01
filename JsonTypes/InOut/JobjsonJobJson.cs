/*TASK RP.JDF*/
using System;
using System.Collections.Generic;
//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: November 21, 2019. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class JobjsonJobJson : TjsonTJson, IComparable
    {
        public int? intnOrderId { get; set; }
        public int intJobId { get; set; }
        public String strJobNumber { get; set; }
        public String strJobTicket { get; set; }
        public int? intnProductKey { get; set; }
        public String strProductName { get; set; }
        public String strProductCategory { get; set; }
        public int? intnQuantity { get; set; }
        public String dateLastUpdate { get; set; }
        public List<AttrjsonAttributeJson> darrattrjson { get; set; }
        public double numProgress { get; set; }
        public int intPkProduct { get; set; }
        public int intPkWorkflow { get; set; }
        public String strStartDateTime { get; set; }
        public String strEndDateTime { get; set; }
        public String strCustomerName { get; set; }
        public String strCompany { get; set; }
        public String strBranch { get; set; }
        public String strAddressLine1 { get; set; }
        public String strAddressLine2 { get; set; }
        public String strCity { get; set; }
        public String strState { get; set; }
        public String strPostalCode { get; set; }
        public String strCountry { get; set; }
        public String strEmail { get; set; }
        public String strPhone { get; set; }
        public String strCellPhone { get; set; }
        public String strCustomerRep { get; set; }
        public String strSalesRep { get; set; }
        public String strDelivery { get; set; }
        public bool? boolnOdyssey2Pricing { get; set; }
        public int? intnReorderedFromJobID { get; set; }
        public double? numnWisnetPrice { get; set; }

        //                                                  //The following attribute will be filled in 
        //                                                  //      the CalculateJob() controller.
        public double numMinCost { get; set; }
        public double numMinPrice { get; set; }
        public double numMaxCost { get; set; }
        public double numMaxPrice { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public int CompareTo(
            Object obj_I
            )
        {
            //                                              //This job number
            string[] strThisJobNumber = (this.strJobNumber).Split('-');
            int intThisOrderNumber = Int32.Parse(strThisJobNumber[0].TrimEnd());
            int intThisJobNumber = Int32.Parse(strThisJobNumber[1].TrimStart());

            //                                              //Job number to compare
            JobjsonJobJson jobjson = (JobjsonJobJson)obj_I;
            string[] strNewJobNumber = (jobjson.strJobNumber).Split('-');
            int intNewOrderNumber = Int32.Parse(strNewJobNumber[0].TrimEnd());
            int intNewJobNumber = Int32.Parse(strNewJobNumber[1].TrimStart());

            int intSortValue;
            //                                              //SORT BY ORDER
            //                                              //Compare the orders
            int intOrderValue = intThisOrderNumber.CompareTo(intNewOrderNumber);
            if (
                //                                          //This order number equals new order number
                intOrderValue == 0
                ) 
            {
                intSortValue = 0;
                //                                          //SORT BY JOB
                //                                          //Compare the jobs
                int intJobValue = intThisJobNumber.CompareTo(intNewJobNumber);
                if (
                    //                                      //This job number equals new job number
                    intJobValue == 0
                    )
                {
                }
                else if (
                    //                                      //This job number is less than new job number
                    intJobValue < 0
                    )
                {
                    //                                      //For ascending order
                    intSortValue = -1;
                }
                else
                //                                          //This job number is greater than new job number
                {
                    //                                      //For ascending order
                    intSortValue = 1;
                }
            }
            else if (
                //                                          //This order number is less than new order number
                intOrderValue < 0
                )
            {
                //                                          //For descending order
                intSortValue = 1;
            }
            else
            //                                              //This order number is greater than new order number
            {
                //                                          //For descending order
                intSortValue = -1;
            }

            return intSortValue;
        }
    }

    //==================================================================================================================
}
/*END-TASK*/
