/*TASK RP.XJDF*/
using System;
using System.Collections.Generic;

//                                                          //AUTHOR: Towa (CLGA - Cesar garcia).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: July 3, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class BdgestjsonBudgetEstimationJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.
        public int intEstimationId { get; set; }
        public String strName { get; set; }
        public int intPkWorkflow { get; set; }
        public String strNameWorkflow { get; set; }
        public bool boolHasOption { get; set; }
        public int intOrderId { get; set; }
        public int intJobId { get; set; }
        public String strJobTicket { get; set; }
        public int intProductKey { get; set; }
        public String strProductCategory { get; set; }       
        public int? intnQuantity { get; set; }
        public String dateLastUpdate { get; set; }
        public String strBaseDate { get; set; }
        public String strBaseTime { get; set; }
        public String strDeliveryDate { get; set; }
        public String strDeliveryTime { get; set; }
        public String strDueDate { get; set; }
        public String strDueTime { get; set; }
        public List<AttrjsonAttributeJson> arrattr { get; set; }
        public int intPkProduct { get; set; }
        public String strProductName { get; set; }
        public List<CostbycaljsonCostByCalculationJson> arrcal { get; set; }
        public List<ProcbdgjsonProcessBudgetJson> arrpro { get; set; }
        public double? numnJobEstimateCost { get; set; }
        public double? numnJobEstimatePrice { get; set; }
        public bool boolIsConfirmable { get; set; }
        public String strJobNumber { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public BdgestjsonBudgetEstimationJson(
            int intEstimationId_I,
            String strName_I,
            int intPkWorkflow_I,
            String strNameWorkflow_I,
            bool boolHasOption_I,
            int intOrderId_I,
            int intJobId_I,
            String strJobTicket_I,
            int intProductKey_I,
            String strProductCategory_I,
            int? intnQuantity_I,
            String dateLastUpdate_I,
            String strBaseDate_I,
            String strBaseTime_I,
            String strDeliveryDate_I,
            String strDeliveryTime_I,
            String strDueDate_I,
            String strDueTime_I,
            List<AttrjsonAttributeJson> arrattr_I,
            int intPkProduct_I,
            String strProductName_I,
            List<CostbycaljsonCostByCalculationJson> arrcostbycaljson_I,
            List<ProcbdgjsonProcessBudgetJson> arrprocbdgjson_I,
            double? numnJobEstimateCost_I,
            double? numnJobEstimatePrice_I,
            bool boolIsConfirmable_I,
            String strJobNumber_I
            )
        {
            this.intEstimationId = intEstimationId_I;
            this.strName = strName_I;
            this.intPkWorkflow = intPkWorkflow_I;
            this.strNameWorkflow = strNameWorkflow_I;
            this.boolHasOption = boolHasOption_I;
            this.intOrderId = intOrderId_I;
            this.intJobId = intJobId_I;
            this.strJobTicket = strJobTicket_I;
            this.intProductKey = intProductKey_I;
            this.strProductCategory = strProductCategory_I;
            this.intnQuantity = intnQuantity_I;
            this.dateLastUpdate = dateLastUpdate_I;
            this.strBaseDate = strBaseDate_I;
            this.strBaseTime = strBaseTime_I;
            this.strDeliveryDate = strDeliveryDate_I;
            this.strDeliveryTime = strDeliveryTime_I;
            this.strDueDate = strDueDate_I;
            this.strDueTime = strDueTime_I;
            this.arrattr = arrattr_I;
            this.intPkProduct = intPkProduct_I;
            this.strProductName = strProductName_I;
            this.arrcal = arrcostbycaljson_I;
            this.arrpro = arrprocbdgjson_I;
            this.numnJobEstimateCost = numnJobEstimateCost_I;
            this.numnJobEstimatePrice = numnJobEstimatePrice_I;
            this.boolIsConfirmable = boolIsConfirmable_I;
            this.strJobNumber = strJobNumber_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
