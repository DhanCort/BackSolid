using Odyssey2Backend.Customer;
using Odyssey2Backend.DB_Odyssey2;
using Odyssey2Backend.PrintShop.sOlid_Cal;
using Odyssey2Backend.Utilities;
using Odyssey2Backend.XJDF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TowaStandard;

namespace Odyssey2Backend.PrintShop.sOlid_cal_base
{
    public class AddBaseCal : IAddCal
    {
        public void AddCalculation(
            ICalculation cal,
            PsPrintShop ps_I,
            ProProcess pro_I,
            ResResource res_I,
            ProdtypProductType prodtyp_I,
            ref int intStatus_IO,
            ref string strUserMessage_IO,
            ref string strDevMessage_IO)
        {
            //                                              //Establish the connection.
            Odyssey2Context context = new Odyssey2Context();

            int intPkAccount = cal.intnPkAccount != null ? (int)cal.intnPkAccount :
                ps_I.intGetExpensePkAccount(context);
            bool boolAccountValid = AccAccounting.boolIsExpenseValid(intPkAccount, context);

            //                                  //Assign Value eleetI or eleeleI.
            int? intnPkElementElementTypeI = cal.boolnIsEleetI == true ? cal.intnPkEleetOrEleeleI : null;
            int? intnPkElementElementI = cal.boolnIsEleetI == true ? null : cal.intnPkEleetOrEleeleI;

            bool boolIsValid = false;
            intStatus_IO = 404;
            strUserMessage_IO = "";
            strDevMessage_IO = "The strBy and the product do not match for a valid calculation.";
            /*CASE*/
            if (
                //                                          //ByProduct.
                cal.strByX == CalCalculation.strByProduct
                )
            {
                intStatus_IO = 405;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "For a ByProduct base calculation you need: A product and strBy = ByProduct. " +
                    "Process, resource type, resource, ascendants and value must be null.";
                if (
                    (prodtyp_I != null) && (pro_I == null) && (res_I == null) &&
                    (cal.strAscendantElements == null) && (cal.strValue == null)
                    )
                {
                    intStatus_IO = 422;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "Account not valid.";
                    if (
                        boolAccountValid
                        )
                    {
                        boolIsValid = true;
                    }
                }
            }
            else if (
                //                                          //ByProcess.
                (cal.strByX == CalCalculation.strByProcess) &&
                (prodtyp_I != null || (bool)cal.boolnWorkflowIsBase)
                )
            {
                intStatus_IO = 407;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "For a ByProcess base calculation you need: A product, a process and strBy = " +
                    "ByProcess. Resource type, resource, ascendants and value must be null.";
                if (
                    (pro_I != null) && (res_I == null) &&
                    (cal.strAscendantElements == null) && (cal.strValue == null)
                    )
                {
                    intStatus_IO = 423;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "Account not valid.";
                    if (
                        boolAccountValid
                        )
                    {
                        boolIsValid = true;
                    }
                }
            }
            else if (
                //                                          //ByProcess default.
                (cal.strByX == CalCalculation.strByProcess) &&
                (prodtyp_I == null)
                )
            {
                intStatus_IO = 421;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "For a ByProcess default per quantity calculation you need: A process and strBy = " +
                    "ByProcess. Product, resource type, resource, ascendants and value must be null.";
                if (
                    (res_I == null) && (cal.strAscendantElements == null) && (cal.strValue == null)
                    )
                {
                    intStatus_IO = 423;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "Account not valid.";
                    if (
                        boolAccountValid
                        )
                    {
                        boolIsValid = true;
                    }
                }
            }

            /*END-CASE*/

            if (
                boolIsValid
                )
            {
                intStatus_IO = 408;
                strUserMessage_IO = "The cost should be greater than zero.";
                strDevMessage_IO = "";
                if (
                    //                                      //The cost is greater than zero.
                    cal.numnCost > 0
                    )
                {
                    intStatus_IO = 409;
                    strUserMessage_IO = "The condition to apply is invalid. Please verify it.";
                    strDevMessage_IO = "";
                    if (
                       Tools.boolValidConditionList(cal.gpcondCondition)
                       )
                    {
                        PiwentityProcessInWorkflowEntityDB piwentity =
                                context.ProcessInWorkflow.FirstOrDefault(piw =>
                                piw.intPk == cal.intnPkProcessInWorkflow);

                        int? intnPkWorkflow = null;
                        int? intnProcessInWorkflowId = null;
                        if (
                            piwentity != null
                            )
                        {
                            intnPkWorkflow = piwentity.intPkWorkflow;
                            intnProcessInWorkflowId = piwentity.intProcessInWorkflowId;
                        }

                        //                      //Add to db.
                        CalentityCalculationEntityDB calentity = new CalentityCalculationEntityDB
                        {
                            intnPkProduct = cal.intnPkProduct,
                            intnPkProcess = cal.intnPkProcess,
                            strCalculationType = CalCalculation.strBase,
                            strDescription = cal.strDescription,
                            boolIsEnable = cal.boolIsEnable,
                            numnCost = cal.numnCost,
                            strByX = cal.strByX,
                            intnPkWorkflow = intnPkWorkflow,
                            intnProcessInWorkflowId = intnProcessInWorkflowId,
                            intnPkElementElementType = intnPkElementElementTypeI,
                            intnPkElementElement = intnPkElementElementI,
                            strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                            strStartTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                            intnPkAccount = intPkAccount
                        };

                        context.Calculation.Add(calentity);
                        context.SaveChanges();

                        Tools.subAddCondition(calentity.intPk, null, null, null, cal.gpcondCondition, context);

                        intStatus_IO = 200;
                        strUserMessage_IO = "Success.";
                        strDevMessage_IO = "";
                    }
                }
            }
        }
    }
}
