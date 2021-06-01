/*TASK RP.XJDF*/
using System;
using System.Collections.Generic;

//                                                          //AUTHOR: Towa (DTC-Daniel Texon).
//                                                          //CO-AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //DATE: September 15, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class JobticjsonJobTicketJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.
        public int intJobId { get; set; }
        public String strJobNumber { get; set; }
        public String strJobTicket { get; set; }
        public int intProductKey { get; set; }
        public String strProductName { get; set; }
        public String strProductCategory { get; set; }
        public String strJobStatus { get; set; }
        public String strDeliveryDate { get; set; }
        public String strDeliveryTime { get; set; }
        public String strDueDate { get; set; }
        public String strDueTime { get; set; }
        public String strStartDate { get; set; }
        public String strStartTime { get; set; }
        public String strEndDate { get; set; }
        public String strEndTime { get; set; }
        public int? intnQuantity { get; set; }
        public List<AttrjsonAttributeJson> arrattr { get; set; }
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
        public String strWorkflowName { get; set; }
        public List<CostbycaljsonCostByCalculationJson> arrcal { get; set; }
        public List<ProcjobjsonProcessJobJson> arrpro { get; set; }
        public double? numnJobCost { get; set; }
        public double? numnJobPrice { get; set; }
        public String strWisnetNote { get; set; }
        public String strOdyssey2Note { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public JobticjsonJobTicketJson(
            int intjobId_I,
            String strJobNumber_I,
            String strJobTicket_I,
            int intProductKey_I,
            String strProductName_I,
            String strProductCategory_I,
            String strJobStatus_I,
            String strDeliveryDate_I,
            String strDeliveryTime_I,
            String strDueDate_I,
            String strDueTime_I,
            String strStartDate_I,
            String strStartTime_I,
            String strEndDate_I,
            String strEndTime_I,
            int? intnQuantity_I,
            List<AttrjsonAttributeJson> arrattr_I,
            String strCustomerName_I,
            String strCompany_I,
            String strBranch_I,
            String strAddressLine1_I,
            String strAddressLine2_I,
            String strCity_I,
            String strState_I,
            String strPostalCode_I,
            String strCountry_I,
            String strEmail_I,
            String strPhone_I,
            String strCellPhone_I,
            String strCustomerRep_I,
            String strSalesRep_I,
            String strDelivery_I,
            String strWorkflowName_I,
            List<CostbycaljsonCostByCalculationJson> arrcal_I,
            List<ProcjobjsonProcessJobJson> arrpro_I,
            double? numnJobCost_I,
            double? numnJobPrice_I,
            String strWisnetNote_I,
            String strOdyssey2Note_I
            )
        {
            this.intJobId = intjobId_I;
            this.strJobNumber = strJobNumber_I;
            this.strJobTicket = strJobTicket_I;
            this.intProductKey = intProductKey_I;
            this.strProductName = strProductName_I;
            this.strProductCategory = strProductCategory_I;
            this.strJobStatus = strJobStatus_I;
            this.strDeliveryDate = strDeliveryDate_I;
            this.strDeliveryTime = strDeliveryTime_I;
            this.strDueDate = strDueDate_I;
            this.strDueTime = strDueTime_I;
            this.strStartDate = strStartDate_I;
            this.strStartTime = strStartTime_I;
            this.strEndDate = strEndDate_I;
            this.strEndTime = strEndTime_I;
            this.intnQuantity = intnQuantity_I;
            this.arrattr = arrattr_I;
            this.strCustomerName = strCustomerName_I;
            this.strCompany = strCompany_I;
            this.strBranch = strBranch_I;
            this.strAddressLine1 = strAddressLine1_I;
            this.strAddressLine2 = strAddressLine2_I;
            this.strCity = strCity_I;
            this.strState = strState_I;
            this.strPostalCode = strPostalCode_I;
            this.strCountry = strCountry_I;
            this.strEmail = strEmail_I;
            this.strPhone = strPhone_I;
            this.strCellPhone = strCellPhone_I;
            this.strCustomerRep = strCustomerRep_I;
            this.strSalesRep = strSalesRep_I;
            this.strDelivery = strDelivery_I;
            this.strWorkflowName = strWorkflowName_I;
            this.arrcal = arrcal_I;
            this.arrpro = arrpro_I;
            this.numnJobCost = numnJobCost_I;
            this.numnJobPrice = numnJobPrice_I;
            this.strWisnetNote = strWisnetNote_I;
            this.strOdyssey2Note = strOdyssey2Note_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
