/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: March 17, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //==================================================================================================================  
    public class WfjjsonWorkflowJobJson
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public String strJobId { get; set; }
        public String strJobNumber { get; set; }
        public String strJobName { get; set; }
        public String strProductName { get; set; }
        public int intJobQuantity { get; set; }
        public int intPkProduct { get; set; }
        public double numCostByProduct { get; set; }
        public Piwjson1ProcessInWorkflowJson1[] arrpro { get; set; }
        public double numJobPrice { get; set; }
        public double numJobCost { get; set; }
        public double numJobFinalCost { get; set; }
        public double numJobProfit { get; set; }
        public double numJobFinalProfit { get; set; }
        public bool boolIsReady { get; set; }
        public bool boolAllResourcesAreAvailable { get; set; }
        public String strDeliveryDate { get; set; }
        public String strStage { get; set; }
        public String strDueDate { get; set; }
        public String strDueTime { get; set; }
        public bool boolIsDueDateReachable { get; set; }
        public bool boolInvoiced { get; set; }
        public double? numnWisnetPrice { get; set; }
        public String strPriceMessage { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //--------------------------------------------------------------------------------------------------------------
        public WfjjsonWorkflowJobJson(
            String strJobId_I,
            String strJobNumber_I,
            String strJobName_I,
            String strProductName_I,
            int intJobQuantity_I,
            int intPkProduct_I,
            double numCostByProduc_I,
            Piwjson1ProcessInWorkflowJson1[] arrpro_I,
            double numJobPrice_I,
            double numJobCost_I,
            double numJobFinalCost_I,
            double numJobProfit_I,
            double numJobFinalProfit_I,
            bool boolIsReady_I,
            bool boolAllResourcesAreAvailable_I,
            String strDeliveryDate_I,
            String strStage_I,
            String strDueDate_I,
            String strDueTime_I, 
            bool boolIsDueDateReachable_I,
            bool boolInvoiced_I,
            double? numnWisnetPrice_I,
            String strPriceMessage_I
            )
        {
            this.strJobId = strJobId_I;
            this.strJobNumber = strJobNumber_I;
            this.strJobName = strJobName_I;
            this.strProductName = strProductName_I;
            this.intJobQuantity = intJobQuantity_I;
            this.intPkProduct = intPkProduct_I;
            this.numCostByProduct = numCostByProduc_I;
            this.arrpro = arrpro_I;
            this.numJobPrice = numJobPrice_I;
            this.numJobCost = numJobCost_I;
            this.numJobFinalCost = numJobFinalCost_I;
            this.numJobProfit = numJobProfit_I;
            this.numJobFinalProfit = numJobFinalProfit_I;
            this.boolIsReady = boolIsReady_I;
            this.boolAllResourcesAreAvailable = boolAllResourcesAreAvailable_I;
            this.strDeliveryDate = strDeliveryDate_I;
            this.strStage = strStage_I;
            this.strDueDate = strDueDate_I;
            this.strDueTime = strDueTime_I;
            this.boolIsDueDateReachable = boolIsDueDateReachable_I;
            this.boolInvoiced = boolInvoiced_I;
            this.numnWisnetPrice = numnWisnetPrice_I;
            this.strPriceMessage = strPriceMessage_I;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
