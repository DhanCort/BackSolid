/*TASK RP. ACCOUNTING*/
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Configuration;
using Odyssey2Backend.DB_Odyssey2;
using Odyssey2Backend.Job;
using Odyssey2Backend.JsonTypes;
using Odyssey2Backend.PrintShop;
using Odyssey2Backend.Utilities;
using Odyssey2Backend.XJDF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TowaStandard;

//                                                          //AUTHOR: Towa (CCC - Cesar Cigarroa).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: October 30, 2020.

namespace Odyssey2Backend.Customer
{
    //==================================================================================================================
    public static class AccAccounting
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTANTS.

        //                                                  //Acconunt Types.
        public const String strAccountTypeExpense = "Expense";
        public const String strAccountTypeRevenue = "Revenue";
        public const String strAccountTypeLiability = "Liability";
        public const String strAccountTypeAsset = "Asset";
        public const String strAccountTypeBank = "Bank";

        //                                                  //Default Accounts.
        public const String strCashInBackAccountNumber = "1000";
        public const String strCashInBackAccountName = "Cash in Bank";

        public const String strAccountsReceivableNumber = "1100";
        public const String strAccountsReceivableName = "Accounts Receivable";

        public const String strAccountsPayableNumber = "2000";
        public const String strAccountsPayableName = "Accounts Payable";

        public const String strSalesTaxPayableNumber = "2100";
        public const String strSalesTaxPayableName = "Sales Tax Payable";

        public const String strGeneralSalesNumber = "4000";
        public const String strGeneralSalesName = "General Sales";

        public const String strDigitalSalesNumber = "4005";
        public const String strDigitalSalesName = "Digital Sales";

        public const String strOffsetPrintingSalesNumber = "4010";
        public const String strOffsetPrintingSalesName = "Offset Printing Sales";

        public const String strFreightNumber = "4500";
        public const String strFreightName = "Freight";

        public const String strPaperNumber = "5000";
        public const String strPaperName = "Paper";

        public const String strEnvelopesNumber = "5010";
        public const String strEnvelopesName = "Envelopes";

        public const String strPlatesNumber = "5020";
        public const String strPlatesName = "Plates";

        public const String strInkNumber = "5030";
        public const String strInkName = "Ink";

        public const String strUndepositedFundsNumber = "SYS01";
        public const String strUndepositedFundsName = "Undeposited Funds";

        public const String strUncategorizedRevenueNumber = "SYS02";
        public const String strUncategorizedRevenueName = "Uncategorized Revenue";

        public const String strUncategorizedExpenseNumber = "SYS03";
        public const String strUncategorizedExpenseName = "Uncategorized Expense";

        //                                                  //Payment methods.
        public const String strPaymentMethodCash = "Cash";
        public const String strPaymentMethodCreditCard = "Credit card";
        public const String strPaymentMethodCheck = "Check";
        //                                                  //CustomerBalanceStatus
        public const String strBalanceStatusOpen = "Open";
        public const String strBalanceStatusAll = "All";

        //                                                  //Types Statements.
        public const String strStatementTypeOpen = "Open";
        public const String strStatementTypeTransaction = "Transaction";
        public const String strStatementTypeForward = "Forward";

        /*
        public const String strGenericExpenseAccountNumber = "0";
        public const String strGenericRevenueAccountNumber = "1";
        public const String strLiabilityAccountNumber = "2";
        public const String strAssetAccountNumber = "3";
        */

        public static List<int> darrintOrderInvoicing_S = new List<int>();     

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.


        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTOR.

        //--------------------------------------------------------------------------------------------------------------


        //--------------------------------------------------------------------------------------------------------------
        //                                                  //TRANSFORMATION METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public static void subAddAccount(
            //                                              //Add a printshop's account.

            String strNumber_I,
            String strName_I,
            int intPkType_I,
            bool boolIsGeneric_I,
            PsPrintShop ps_I,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            if (
                AccAccounting.boolIsValidAccountNumberAndName(strNumber_I, strName_I, null, ps_I, context_M,
                ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO)
                )
            {
                //                                          //Find account type.
                AcctypentityAccountTypeEntityDB acctypentity = context_M.AccountType.FirstOrDefault(acctype =>
                    acctype.intPk == intPkType_I);

                intStatus_IO = 401;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Account type not found.";
                if (
                    acctypentity != null
                    )
                {
                    //                                          //Create account.
                    AccentityAccountEntityDB accentityNew = new AccentityAccountEntityDB
                    {
                        strNumber = strNumber_I,
                        strName = strName_I,
                        boolAvailable = true,
                        intPkPrintshop = ps_I.intPk,
                        intPkAccountType = acctypentity.intPk,
                        boolGeneric = boolIsGeneric_I,
                        numBalance = 0.0
                    };
                    context_M.Account.Add(accentityNew);
                    context_M.SaveChanges();

                    intStatus_IO = 200;
                    strUserMessage_IO = "";
                    strDevMessage_IO = "";
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static bool boolIsValidAccountNumberAndName(
            //                                              //Verify if account's number or name is already in use in 
            //                                              //      a prinshop.
            //                                              //Verify is number and name are valid.

            String strNumber_I,
            String strName_I,
            int? intnPkAccountToExclude_I,
            PsPrintShop ps_I,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            bool boolIsValidAccountNameAndDescription = false;

            intStatus_IO = 401;
            strUserMessage_IO = "Number cannot be empty.";
            strDevMessage_IO = "";
            if (
                strNumber_I.Length > 0
                )
            {
                intStatus_IO = 402;
                strUserMessage_IO = "Name cannot be empty.";
                strDevMessage_IO = "";
                if (
                    strName_I.Length > 0
                    )
                {
                    //                                      //Find printshop's accounts with same number.
                    AccentityAccountEntityDB accentityRepeatedNumber = context_M.Account.FirstOrDefault(account =>
                         account.intPkPrintshop == ps_I.intPk &&
                         account.strNumber == strNumber_I &&
                         account.intPk != intnPkAccountToExclude_I);
                    bool boolIsThereADuplicatedNumber = accentityRepeatedNumber != null ? true : false;

                    if (
                        boolIsThereADuplicatedNumber
                        )
                    {
                        intStatus_IO = 403;
                        strUserMessage_IO = "Number already in use.";
                        strDevMessage_IO = "";
                    }
                    else
                    {
                        //                                  //Find printshop's accounts with same name.
                        AccentityAccountEntityDB accentityRepeatedName = context_M.Account.FirstOrDefault(account =>
                             account.intPkPrintshop == ps_I.intPk &&
                             account.strName == strName_I &&
                             account.intPk != intnPkAccountToExclude_I);
                        bool boolIsThereADuplicatedName = accentityRepeatedName != null ? true : false;

                        if (
                            boolIsThereADuplicatedName
                            )
                        {
                            intStatus_IO = 404;
                            strUserMessage_IO = "Name already in use.";
                            strDevMessage_IO = "";
                        }
                        else
                        {
                            boolIsValidAccountNameAndDescription = true;
                        }
                    }
                }
            }

            return boolIsValidAccountNameAndDescription;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static InvjsonInvoiceJson invjsonAddInvoice(
            //                                              //Add a printshop's account.

            int intOrderId_I,
            List<int> darrintJobsIds,
            PsPrintShop ps_I,
            IConfiguration configuration_I,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Invoice.
            InvjsonInvoiceJson invjsonInvoice = new InvjsonInvoiceJson();

            //                                              //The following two if's, were added (LGF & JLBD) to prevent
            //                                              //      an Invoice to be added many times to the database
            //                                              //      when the printer click Generate Invoice many times.
            //                                              //We are proposing the _S as an static public semaphore array. 
            //                                              //If an Invoice is in that array means that is in process of
            //                                              //      being added.
            //                                              //The Invoice is remove from the array when the transaction
            //                                              //      finish in the service.
            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Invoice is being added.";
            if (
               !darrintOrderInvoicing_S.Contains(intOrderId_I)
                )
            {
                darrintOrderInvoicing_S.Add(intOrderId_I);
                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Invoice arleady added to the database.";
                if (
                   context_M.Invoice.FirstOrDefault(I => I.intOrderNumber == intOrderId_I) == null
                    )
                {
                    //                                      //Verify that jobs received are for the same order.
                    //                                      //Get data from Wisnet.
                    JobidsjsonJobIdsJson jobidsjson = new JobidsjsonJobIdsJson(darrintJobsIds.ToArray());
                    String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                        GetSection("Odyssey2Settings")["urlWisnetApi"];

                    Task<List<PsorderjsonPrintshopOrderJson>> Task_darrcatjsonFromWisnet =
                        HttpTools<PsorderjsonPrintshopOrderJson>.GetListAsyncPrintshopOrders(strUrlWisnet +
                        "/PrintShopData/PrintshopOrdersAndJobs", jobidsjson);
                    Task_darrcatjsonFromWisnet.Wait();

                    intStatus_IO = 403;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "order not valid, not completed or wisnet connection lost.";
                    if (
                        //                                  //The order exists in Wisnet.
                        Task_darrcatjsonFromWisnet.Result != null
                        )
                    {
                        //                                  //Get info from Wisnet.
                        Task<InvjsonInvoiceJson> Task_darrcatjsonFromWisnetInv =
                        HttpTools<InvjsonInvoiceJson>.GetOneAsyncToEndPoint(strUrlWisnet +
                        "/PrintShopData/Order/" + ps_I.strPrintshopId + "/" + intOrderId_I);
                        Task_darrcatjsonFromWisnetInv.Wait();

                        intStatus_IO = 404;
                        strUserMessage_IO = "Something is wrong.";
                        strDevMessage_IO = "Wisnet conection lost.";
                        if (
                            Task_darrcatjsonFromWisnetInv.Result != null
                            )
                        {
                            invjsonInvoice = Task_darrcatjsonFromWisnetInv.Result;

                            //                              //Find order's contact id.
                            Task<OrdercontjsonOrderContactJson> Task_ordercontjsonFromWisnet =
                            HttpTools<OrdercontjsonOrderContactJson>.GetOneAsyncToEndPoint(strUrlWisnet +
                                "/PrintShopData/OrderContactId/" + intOrderId_I);
                            Task_ordercontjsonFromWisnet.Wait();

                            if (
                                Task_ordercontjsonFromWisnet != null &&
                                Task_ordercontjsonFromWisnet.Result.intnContactId != null
                                )
                            {
                                //                          //Get order number.
                                JobjsonentityJobJsonEntityDB jobjsonentity = context_M.JobJson.FirstOrDefault(job =>
                                job.strPrintshopId == ps_I.strPrintshopId &&
                                job.intOrderId == invjsonInvoice.intOrderId);
                                invjsonInvoice.intnOrderNumber = invjsonInvoice.intOrderId == 0 ? (int?)null :
                                    (int)jobjsonentity.intnOrderNumber;

                                //                          //Contact id.
                                int intContactId = (int)Task_ordercontjsonFromWisnet.Result.intnContactId;

                                //                          //Create fake invoice.
                                InvoInvoiceEntityDB invoentityFake = new InvoInvoiceEntityDB
                                {
                                    intOrderNumber = intOrderId_I,
                                    strInvoiceJson = "",
                                    intPkPrintshop = ps_I.intPk,
                                    intContactId = intContactId,
                                    strDate = Date.Now(ZonedTimeTools.timezone).ToString()
                                };

                                context_M.Invoice.Add(invoentityFake);
                                context_M.SaveChanges();

                                //                          //If shipping method from api comes empty, add an specific
                                //                          //      string.
                                if (
                                    invjsonInvoice.strShippingMethod.Length == 0
                                    )
                                {
                                    if (
                                        invjsonInvoice.boolIsShipped
                                        )
                                    {
                                        invjsonInvoice.strShippingMethod = "Deliver to shipping address";
                                    }
                                    else
                                    {
                                        invjsonInvoice.strShippingMethod = "Pick up at our location";
                                    }
                                }

                                //                          //To easy code
                                String strZipCodeToUse = invjsonInvoice.boolIsShipped ? invjsonInvoice.strShippedToZip :
                                        invjsonInvoice.strPrintshopZipCode;

                                List<InvjobinfojsonInvoiceJobInformationJson> darrinvjobinfojson =
                                new List<InvjobinfojsonInvoiceJobInformationJson>();
                                double numSubtotal = 0.0;

                                //                          //Get info for each job.
                                for (int intI = 0; intI < darrintJobsIds.Count; intI++)
                                {
                                    //                      //Get job's price.
                                    List<PriceentityPriceEntityDB> darrpriceentity = context_M.Price.Where(price =>
                                        price.intJobId == darrintJobsIds[intI]).ToList();
                                    PriceentityPriceEntityDB priceentity = darrpriceentity.Last();
                                    double numJobPrice = priceentity != null ? ((double)priceentity.numnPrice).Round(2) : 0.00;

                                    numSubtotal = (numSubtotal + numJobPrice).Round(2);

                                    //                      //Get name and Quantity.
                                    JobjsonJobJson jobjson = new JobjsonJobJson();
                                    JobJob.boolIsValidJobId(darrintJobsIds[intI], ps_I.strPrintshopId, configuration_I,
                                        out jobjson, ref strUserMessage_IO, ref strDevMessage_IO);

                                    String strJobName = jobjson.strJobTicket;
                                    int intJobQuantity = (int)jobjson.intnQuantity;

                                    //                      //Get product account.
                                    EtentityElementTypeEntityDB etentity = context_M.ElementType.FirstOrDefault(et =>
                                        et.intPrintshopPk == ps_I.intPk &&
                                        et.strCustomTypeId == jobjson.strProductName &&
                                        et.strCategory == jobjson.strProductCategory &&
                                        et.boolDeleted == false &&
                                        et.intWebsiteProductKey == jobjson.intnProductKey);

                                    //                      //Get the product account.
                                    AccentityAccountEntityDB accentityProduct = null;
                                    if (
                                        (etentity != null) && (etentity.intnPkAccount != null)
                                        )
                                    {
                                        accentityProduct = context_M.Account.FirstOrDefault(acc =>
                                        acc.intPk == (int)etentity.intnPkAccount);
                                    }

                                    //                      //Take product account if it is set and available,
                                    //                      //      otherwise takes generic printshops' revenue 
                                    //                      //      account.   
                                    int intPkProdOrGentAccount = (accentityProduct != null && accentityProduct.boolAvailable) ?
                                        (int)etentity.intnPkAccount :
                                        (from accentityGeneric in context_M.Account
                                         join acctypentity in context_M.AccountType
                                         on accentityGeneric.intPkAccountType equals acctypentity.intPk
                                         where accentityGeneric.intPkPrintshop == ps_I.intPk &&
                                         accentityGeneric.boolAvailable == true &&
                                         acctypentity.strType == AccAccounting.strAccountTypeRevenue &&
                                         accentityGeneric.boolGeneric == true
                                         select accentityGeneric).FirstOrDefault().intPk;

                                    //                      //Add movement to accountMovement table.
                                    AccmoventityAccountMovementEntityDB accmoventity = new AccmoventityAccountMovementEntityDB
                                    {
                                        strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                                        strStartTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                                        strConcept = "Invoice " + intOrderId_I,
                                        numnIncrease = numJobPrice,
                                        intnJobId = darrintJobsIds[intI],
                                        intPkAccount = intPkProdOrGentAccount,
                                        intnPkInvoice = invoentityFake.intPk
                                    };
                                    context_M.AccountMovement.Add(accmoventity);
                                    context_M.SaveChanges();

                                    //                      //Generate an increase amount to the account's balance
                                    AccAccounting.subUpdateAccountBalance(intPkProdOrGentAccount, null, numJobPrice,
                                        context_M);

                                    //                      //Get strAccount.
                                    AccentityAccountEntityDB accentityName = context_M.Account.FirstOrDefault(acc =>
                                        acc.intPk == intPkProdOrGentAccount);
                                    String strAccount = accentityName.strNumber + "-" + accentityName.strName;

                                    //                      //Get strJobNumber
                                    String strJobNumber = JobJob.strGetJobNumber(intOrderId_I, darrintJobsIds[intI],
                                        ps_I.strPrintshopId, context_M);

                                    //                      //Create json.
                                    InvjobinfojsonInvoiceJobInformationJson invjobinfojson =
                                        new InvjobinfojsonInvoiceJobInformationJson(darrintJobsIds[intI], strJobNumber,
                                        strJobName, intJobQuantity, numJobPrice, intPkProdOrGentAccount, accmoventity.intPk,
                                        strAccount, false);
                                    darrinvjobinfojson.Add(invjobinfojson);

                                    //                      //Update table Job.
                                    JobentityJobEntityDB jobentity = context_M.Job.FirstOrDefault(job =>
                                        job.intJobID == darrintJobsIds[intI] && job.intPkPrintshop == ps_I.intPk);
                                    jobentity.boolInvoiced = true;
                                    context_M.Update(jobentity);
                                }

                                //                          //Calculate taxes.
                                double numTaxes = 0.0;
                                double numTaxPercentage = 0.0;
                                if (
                                    strZipCodeToUse != null
                                    )
                                {
                                    TaxentityTaxesEntityDB taxentity = context_M.Taxes.FirstOrDefault(tax =>
                                        tax.strZipCode == strZipCodeToUse);
                                    numTaxes = taxentity != null ? numSubtotal * taxentity.numTaxValue : 0.0;
                                    numTaxPercentage = taxentity != null ? (taxentity.numTaxValue * 100) : 0.0;
                                }

                                if (
                                    numTaxes >= 0
                                    )
                                {
                                    //                      //Create AccountMovement for liability account.
                                    //                      //Takes liability account.
                                    int intPkLiabilityAccount = (from accentity in context_M.Account
                                                                 join acctypentity in context_M.AccountType
                                                                 on accentity.intPkAccountType equals acctypentity.intPk
                                                                 where accentity.intPkPrintshop == ps_I.intPk &&
                                                                 accentity.boolAvailable &&
                                                                 acctypentity.strType == "Liability" &&
                                                                 accentity.boolGeneric == true &&
                                    //                      //Since there is not only one generic liability account.
                                                                 accentity.strNumber == AccAccounting.strSalesTaxPayableNumber
                                                                 select accentity).FirstOrDefault().intPk;

                                    //                      //Add movement to accountMovement table.
                                    AccmoventityAccountMovementEntityDB accmoventityLia =
                                        new AccmoventityAccountMovementEntityDB
                                        {
                                            strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                                            strStartTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                                            strConcept = "Invoice " + intOrderId_I,
                                            numnIncrease = numTaxes,
                                            intPkAccount = intPkLiabilityAccount,
                                            intnPkInvoice = invoentityFake.intPk
                                        };
                                    context_M.AccountMovement.Add(accmoventityLia);

                                    //                      //Generate an increase amount to the account's balance
                                    AccAccounting.subUpdateAccountBalance(intPkLiabilityAccount, null, numTaxes,
                                        context_M);
                                }

                                //                          //Create invoice.
                                invjsonInvoice.darrinvjobinfojson = darrinvjobinfojson;
                                invjsonInvoice.numSubtotalTotal = numSubtotal;
                                invjsonInvoice.numTaxes = numTaxes;
                                invjsonInvoice.numTotal = numSubtotal + numTaxes;
                                invjsonInvoice.numTaxPercentage = numTaxPercentage;

                                //                          //Create movement in the Account receivable.

                                //                          //Find receivable account.
                                int intPkReceivableAccount = (from accentity in context_M.Account
                                                              join acctypentity in context_M.AccountType
                                                              on accentity.intPkAccountType equals acctypentity.intPk
                                                              where accentity.intPkPrintshop == ps_I.intPk &&
                                                              accentity.boolAvailable &&
                                                              acctypentity.strType == "Asset" &&
                                                              accentity.boolGeneric == true &&
                                                              accentity.strNumber == AccAccounting.strAccountsReceivableNumber
                                                              select accentity).FirstOrDefault().intPk;

                                //                          //Add movement to accountMovement table.
                                AccmoventityAccountMovementEntityDB accmoventityReceivable =
                                    new AccmoventityAccountMovementEntityDB
                                    {
                                        strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                                        strStartTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                                        strConcept = "Invoice " + intOrderId_I,
                                        numnIncrease = numSubtotal + numTaxes,
                                        intPkAccount = intPkReceivableAccount,
                                        intnPkInvoice = invoentityFake.intPk
                                    };
                                context_M.AccountMovement.Add(accmoventityReceivable);

                                double numnTotal = numSubtotal + numTaxes;
                                //                          //Generate an increase amount to the account's balance
                                AccAccounting.subUpdateAccountBalance(intPkReceivableAccount, null, numnTotal,
                                    context_M);

                                if (
                                    invjsonInvoice.strOrderDate.Length > 0
                                    )
                                {
                                    //                      //Delete time stamp from invoice date.

                                    DateTime datetimeInvoiceDate = DateTime.Parse(invjsonInvoice.strOrderDate);
                                    String strOrderDate = datetimeInvoiceDate.Year.ToString() + "/" +
                                    datetimeInvoiceDate.Month.ToString() + "/" + datetimeInvoiceDate.Day.ToString();
                                    invjsonInvoice.strOrderDate = strOrderDate;
                                }

                                //                          //Storage invoice.
                                String strInvoice = JsonSerializer.Serialize(invjsonInvoice);

                                //                          //Set fake invoce as valid invoice.
                                InvoInvoiceEntityDB inventityToUpdate = context_M.Invoice.FirstOrDefault(inv =>
                                    inv.intPk == invoentityFake.intPk);
                                inventityToUpdate.strInvoiceJson = strInvoice;
                                inventityToUpdate.numOpenBalance = numSubtotal + numTaxes;
                                inventityToUpdate.numAmount = numSubtotal + numTaxes;
                                context_M.Invoice.Update(inventityToUpdate);
                                context_M.SaveChanges();

                                if (
                                    //                      //This invoice has a openBalance Zero
                                    inventityToUpdate.numOpenBalance == 0
                                    )
                                {
                                    //                      //This order is check how paid beacuse the order
                                    //                      //  has price of Zero.
                                    Task<String> Task_PostSendToWisnetInvoicesAlreadyPaid =
                                    HttpTools<TjsonTJson>.PostUpdateInvoiceToPaidStage(strUrlWisnet +
                                        "/PrintshopData/SetJobAsPaid/", ps_I.strPrintshopId, 
                                        inventityToUpdate.intOrderNumber);
                                    Task_PostSendToWisnetInvoicesAlreadyPaid.Wait();

                                    if (
                                        //                                  //The Order was updated in Wisnet.
                                        Task_PostSendToWisnetInvoicesAlreadyPaid.Result.Contains("200")
                                        )
                                    {
                                        inventityToUpdate.boolnOnWisnet = true;
                                        context_M.Invoice.Update(inventityToUpdate);
                                    }
                                    else
                                    {
                                        inventityToUpdate.boolnOnWisnet = false;
                                        context_M.Invoice.Update(inventityToUpdate);
                                    }
                                }

                                context_M.SaveChanges();

                                intStatus_IO = 200;
                                strUserMessage_IO = "success";
                                strDevMessage_IO = "";
                            }
                        }
                    }
                }
            }
            return invjsonInvoice;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subEditInvoice(
            //                                              //Edit invoice.

            int intPkInvoice_I,
            int intContactId_I,
            List<InvjobinfojsonInvoiceJobInformationJson> darrinvjobinfojson_I,
            PsPrintShop ps_I,
            String strEditedInvoice_I,
            String strBilledTo_I,
            int intOrderId_I,
            String strZipCodeToUse_I,
            IConfiguration configuration_I,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Validate invoice.
            InvoInvoiceEntityDB inventity = context_M.Invoice.FirstOrDefault(inv => inv.intPk == intPkInvoice_I);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Invalid PkInvoice.";
            if (
                inventity != null
                )
            {
                intStatus_IO = 402;
                strUserMessage_IO = "ZipCode not valid.";
                strDevMessage_IO = "ZipCode not valid.";
                if (
                    AccAccounting.boolIsValidZipCode(strZipCodeToUse_I, context_M, ref intStatus_IO,
                    ref strUserMessage_IO, ref strDevMessage_IO)
                    )
                {

                    //                                          //Validate there is a billed to specified.
                    intStatus_IO = 402;
                    strUserMessage_IO = "Billed to cannot be empty.";
                    strDevMessage_IO = "Billed to can not be empty..";
                    if (
                        strBilledTo_I.Length > 0
                        )
                    {
                        //                                      //Verify if the values of quantity and price for each
                        //                                      //      concept are not less than 0, and verify if
                        //                                      //      account still valid.
                        double numSubTotal = 0.0;

                        bool boolPriceOrQuantityIsValid = darrinvjobinfojson_I.Count > 0;
                        int intI = 0;
                        /*WHILE-DO*/
                        while (
                            boolPriceOrQuantityIsValid &&
                            intI < darrinvjobinfojson_I.Count
                            )
                        {
                            //                                  //Current item.
                            InvjobinfojsonInvoiceJobInformationJson invjobinfo = darrinvjobinfojson_I[intI];

                            intStatus_IO = 403;
                            strUserMessage_IO = "Job Quantity and price can not be less than 0.";
                            strDevMessage_IO = "Invalid PkInvoice.";
                            if (
                                invjobinfo.intQuantity >= 0 &&
                                invjobinfo.numPrice >= 0
                                )
                            {
                                //                              //To store total price.
                                numSubTotal = (numSubTotal + invjobinfo.numPrice).Round(2);

                                if (
                                    //                          //It is a job.
                                    invjobinfo.intnJobId != null
                                    )
                                {
                                    intStatus_IO = 404;
                                    strUserMessage_IO = "Something is wrong.";
                                    strDevMessage_IO = "A concept for a job has to have an account and a movement.";
                                    boolPriceOrQuantityIsValid = false;
                                    if (
                                        invjobinfo.intnPkAccount != null &&
                                        invjobinfo.intnPkAccountMov != null
                                        )
                                    {
                                        //                      //Get the movement for the job.
                                        AccmoventityAccountMovementEntityDB accmoventity =
                                            context_M.AccountMovement.FirstOrDefault(acc =>
                                            acc.intPk == invjobinfo.intnPkAccountMov);

                                        intStatus_IO = 405;
                                        strUserMessage_IO = "Something is wrong.";
                                        strDevMessage_IO = "Invalid PkAccountMovement or you can not add a different " +
                                            "account to a movement created before.";
                                        if (
                                            accmoventity != null &&
                                            accmoventity.intnJobId == invjobinfo.intnJobId &&
                                            accmoventity.intPkAccount == invjobinfo.intnPkAccount
                                            )
                                        {
                                            //                  //Update price table if its a job's price.
                                            //                  //Get workflow.
                                            int intPkWorkflow = context_M.Job.FirstOrDefault(job =>
                                                job.intJobID == invjobinfo.intnJobId &&
                                                job.intPkPrintshop == ps_I.intPk).intPkWorkflow;

                                            PriceentityPriceEntityDB priceentity = new PriceentityPriceEntityDB
                                            {
                                                numnPrice = (invjobinfo.numPrice).Round(2),
                                                intJobId = (int)invjobinfo.intnJobId,
                                                strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                                                strStartTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                                                strDescription = "From Invoice",
                                                intContactId = intContactId_I,
                                                boolIsReset = false,
                                                intnPkWorkflow = intPkWorkflow,
                                                intnPkEstimate = null
                                            };
                                            context_M.Price.Add(priceentity);


                                            //                  //Calculate increase or decrease amount that will affect
                                            //                  //      account's balance.
                                            if (
                                                //              //Decrease account's amount.
                                                invjobinfo.numPrice < accmoventity.numnIncrease
                                                )
                                            {
                                                double numDifference =
                                                    (int)accmoventity.numnIncrease - invjobinfo.numPrice;
                                                //              //Generate an decrease amount to the account's balance
                                                AccAccounting.subUpdateAccountBalance(accmoventity.intPkAccount,
                                                    numDifference, null, context_M);
                                            }
                                            else
                                            {
                                                //              //Increase account's ammount.

                                                double numDifference =
                                                    invjobinfo.numPrice - (int)accmoventity.numnIncrease;
                                                //              //Generate an increase amount to the account's balance
                                                AccAccounting.subUpdateAccountBalance(accmoventity.intPkAccount, null,
                                                    numDifference, context_M);
                                            }

                                            //                  //Update account increase amount.
                                            accmoventity.numnIncrease = invjobinfo.numPrice;
                                            //                  //Update movement for a job.
                                            context_M.AccountMovement.Update(accmoventity);

                                            boolPriceOrQuantityIsValid = true;
                                        }
                                    }
                                }
                                else
                                {
                                    //                          //Its a concept nor for a job.
                                    if (
                                        //                      //The concept has already a movement.(update).
                                        invjobinfo.intnPkAccountMov != null
                                        )
                                    {
                                        //                      //Verify account was not changed.
                                        AccmoventityAccountMovementEntityDB accmoventity =
                                            context_M.AccountMovement.FirstOrDefault(acc =>
                                            acc.intPk == invjobinfo.intnPkAccountMov);
                                        if (
                                            accmoventity != null &&
                                            accmoventity.intnJobId == null &&
                                            accmoventity.intPkAccount == invjobinfo.intnPkAccount
                                            )
                                        {
                                            //                  //Calculate increase or decrease amount that will affect
                                            //                  //      account's balance.
                                            if (
                                                //              //Decrease account's amount.
                                                invjobinfo.numPrice < accmoventity.numnIncrease
                                                )
                                            {
                                                double numDifference =
                                                    (int)accmoventity.numnIncrease - invjobinfo.numPrice;
                                                //              //Generate an decrease amount to the account's balance
                                                AccAccounting.subUpdateAccountBalance(accmoventity.intPkAccount,
                                                    numDifference, null, context_M);
                                            }
                                            else
                                            {
                                                //              //Increase account's ammount.

                                                double numDifference =
                                                    invjobinfo.numPrice - (int)accmoventity.numnIncrease;
                                                //              //Generate an increase amount to the account's balance
                                                AccAccounting.subUpdateAccountBalance(accmoventity.intPkAccount, null,
                                                    numDifference, context_M);
                                            }

                                            //                  //Update movement's amount.
                                            accmoventity.numnIncrease = invjobinfo.numPrice.Round(2);
                                            context_M.AccountMovement.Update(accmoventity);
                                        }
                                        else
                                        {
                                            boolPriceOrQuantityIsValid = false;

                                            intStatus_IO = 406;
                                            strUserMessage_IO = "Something is wrong.";
                                            strDevMessage_IO = "Invalid PkAccountMovement or you can not add a different" +
                                            " account to a movement created before.";
                                        }
                                    }
                                    else
                                    {
                                        //                      //It´s a new concept, we have to verify if, has an account,
                                        //                      //      than this account belongs to revenue account's 
                                        //                      //      type.

                                        if (
                                            invjobinfo.intnPkAccount != null
                                            )
                                        {
                                            //                  //Get the account.
                                            AccentityAccountEntityDB accentity = context_M.Account.FirstOrDefault(acc =>
                                                acc.intPk == invjobinfo.intnPkAccount);

                                            intStatus_IO = 407;
                                            strUserMessage_IO = "Something is wrong.";
                                            strDevMessage_IO = "PkAccount not valid or account is not available";
                                            boolPriceOrQuantityIsValid = false;
                                            if (
                                                //              //Account exists and is available.
                                                accentity != null &&
                                                accentity.boolAvailable == true
                                                )
                                            {
                                                //              //Validate type.
                                                AcctypentityAccountTypeEntityDB acctypentity =
                                                    context_M.AccountType.FirstOrDefault(acctyp =>
                                                    acctyp.intPk == accentity.intPkAccountType);

                                                intStatus_IO = 408;
                                                strUserMessage_IO = "Something is wrong.";
                                                strDevMessage_IO = "Account is not revenue type.";
                                                if (
                                                    //          //Account has to be revenue accounttype.
                                                    acctypentity.strType == AccAccounting.strAccountTypeRevenue
                                                    )
                                                {
                                                    boolPriceOrQuantityIsValid = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                boolPriceOrQuantityIsValid = false;
                            }

                            intI++;
                        }

                        if (
                            boolPriceOrQuantityIsValid
                            )
                        {
                            //                                  //Delete movements for concepts that were deleted from
                            //                                  //      invoice.

                            //                                  //Get movements for not a job concept.
                            List<AccmoventityAccountMovementEntityDB> darraccmoventity =
                                    context_M.AccountMovement.Where(acc =>
                                    acc.intnPkInvoice == intPkInvoice_I && acc.intnJobId == null).ToList();

                            //                                  //From invoice edition keep only concepts not for a job.
                            List<InvjobinfojsonInvoiceJobInformationJson> darrinvjobinfoNotForAJob =
                                    darrinvjobinfojson_I.Where(concept => concept.intnJobId == null).ToList();

                            //                                  //If both list' lengh are the same, so nothing to do.
                            if (
                                !(darraccmoventity.Count == darrinvjobinfoNotForAJob.Count)
                                )
                            {
                                //                              //Delete movements from concepts tha were deleted from 
                                //                              //      invoice.
                                for (int intJ = 0; intJ < darraccmoventity.Count; intJ++)
                                {
                                    if (
                                        !(darrinvjobinfoNotForAJob.Exists(inv =>
                                            inv.intnPkAccountMov == darraccmoventity[intJ].intPk))
                                        )
                                    {
                                        context_M.AccountMovement.Remove(darraccmoventity[intJ]);
                                    }
                                }
                                context_M.SaveChanges();
                            }

                            //                                  //To store taxes quantity.
                            double numTaxes = 0.0;

                            //                                  //Get tax value to calculate concept's taxes.
                            TaxentityTaxesEntityDB taxentity = context_M.Taxes.FirstOrDefault(tax =>
                                tax.strZipCode == strZipCodeToUse_I);

                            //                                  //Create movement for other concepts.
                            for (int intJ = 0; intJ < darrinvjobinfojson_I.Count; intJ++)
                            {
                                //                              //Current item.
                                InvjobinfojsonInvoiceJobInformationJson invjobinfo = darrinvjobinfojson_I[intJ];

                                //                              //Calculate taxes. either concept for a job or other 
                                //                              //      concept.
                                if (
                                    //                          //The concept is not taxes free.
                                    !invjobinfo.boolIsExempt &&
                                    //                          //It´s a zipcode.
                                    taxentity != null
                                    )
                                {
                                    //                          //Get tax for the concept' price.
                                    double numConceptTax = invjobinfo.numPrice * taxentity.numTaxValue;
                                    numTaxes = numTaxes + numConceptTax;
                                }

                                if (
                                    //                          //Only new concepts (not for a job).
                                    invjobinfo.intnJobId == null &&
                                    invjobinfo.intnPkAccountMov == null
                                    )
                                {
                                    //                          //If we have account, take it, otherwise takes printhsop
                                    //                          //      generic revenue account.
                                    int intPkAccount = invjobinfo.intnPkAccount != null ? (int)invjobinfo.intnPkAccount :
                                        (from accentity in context_M.Account
                                         join acctypentity in context_M.AccountType
                                         on accentity.intPkAccountType equals acctypentity.intPk
                                         where accentity.intPkPrintshop == ps_I.intPk &&
                                         accentity.boolAvailable == true &&
                                         acctypentity.strType == AccAccounting.strAccountTypeRevenue &&
                                         accentity.boolGeneric == true
                                         select accentity).FirstOrDefault().intPk;

                                    //                          //Create the movement for the concept.
                                    AccmoventityAccountMovementEntityDB accmoventity =
                                        new AccmoventityAccountMovementEntityDB
                                        {
                                            strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                                            strStartTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                                            strConcept = "Invoice " + intOrderId_I,
                                            numnIncrease = (invjobinfo.numPrice).Round(2),
                                            intnJobId = null,
                                            //boolIsCost = false,
                                            intPkAccount = intPkAccount,
                                            intnPkInvoice = intPkInvoice_I
                                        };
                                    context_M.AccountMovement.Add(accmoventity);
                                    //                          //This saveChanges is necessary because we need the 
                                    //                          //      accountMovement pk.
                                    context_M.SaveChanges();

                                    //                          //Generate an increase amount to the account's balance
                                    AccAccounting.subUpdateAccountBalance(intPkAccount, null, invjobinfo.numPrice,
                                        context_M);

                                    //                          //Get account to get number and name.
                                    AccentityAccountEntityDB accentityNumAndName = context_M.Account.FirstOrDefault(acc =>
                                        acc.intPk == intPkAccount);

                                    //                          //Set pkaccount, pkAccountMovement and strAccount for the 
                                    //                          //      new concepts added.
                                    invjobinfo.intnPkAccountMov = accmoventity.intPk;
                                    invjobinfo.intnPkAccount = intPkAccount;
                                    invjobinfo.strAccount = accentityNumAndName.strNumber + "-" +
                                        accentityNumAndName.strName;
                                }
                            }

                            //                                  //Update AccountMovement for liability account.
                            //                                  //Takes liability account.
                            int intPkLiabilityAccount = (from accentity in context_M.Account
                                                         join acctypentity in context_M.AccountType
                                                         on accentity.intPkAccountType equals acctypentity.intPk
                                                         where accentity.intPkPrintshop == ps_I.intPk &&
                                                         accentity.boolAvailable &&
                                                         acctypentity.strType == "Liability" &&
                                                         accentity.boolGeneric == true &&
                                                         //     //Since there are more than one generic liability account.
                                                         accentity.strNumber == AccAccounting.strSalesTaxPayableNumber
                                                         select accentity).FirstOrDefault().intPk;

                            AccmoventityAccountMovementEntityDB accmoventityToUpdate =
                                context_M.AccountMovement.FirstOrDefault(accmov =>
                                accmov.intnPkInvoice == intPkInvoice_I &&
                                accmov.intPkAccount == intPkLiabilityAccount);

                            if (
                                accmoventityToUpdate != null
                                )
                            {
                                //                              //Calculate increase or decrease amount that will affect
                                //                              //      account's balance.
                                if (
                                    //                          //Decrease account's amount.
                                    numTaxes < accmoventityToUpdate.numnIncrease
                                    )
                                {
                                    double numDifference =
                                        (int)accmoventityToUpdate.numnIncrease - numTaxes;
                                    //                          //Generate an decrease amount to the account's balance
                                    AccAccounting.subUpdateAccountBalance(intPkLiabilityAccount, numDifference,
                                        null, context_M);
                                }
                                else
                                {
                                    //                          //Increase account's ammount.

                                    double numDifference =
                                        numTaxes - (int)accmoventityToUpdate.numnIncrease;
                                    //                          //Generate an increase amount to the account's balance
                                    AccAccounting.subUpdateAccountBalance(intPkLiabilityAccount, null,
                                        numDifference, context_M);
                                }

                                accmoventityToUpdate.numnIncrease = numTaxes;
                                context_M.Update(accmoventityToUpdate);
                            }

                            //                              //Update taxes percentage.
                            double numTaxPercentage = taxentity != null ? (taxentity.numTaxValue * 100) : 0.0;

                            //                                  //Update total and darrinvjobinfojson.
                            InvjsonInvoiceJson invjson =
                                JsonSerializer.Deserialize<InvjsonInvoiceJson>(strEditedInvoice_I);
                            invjson.numSubtotalTotal = numSubTotal;
                            invjson.numTaxes = numTaxes;
                            invjson.numTaxPercentage = numTaxPercentage;
                            invjson.numTotal = numSubTotal + numTaxes;
                            invjson.darrinvjobinfojson = darrinvjobinfojson_I;

                            //                                  //Update receivable account movement.

                            //                                  //Find receivable account.
                            int intPkReceivableAccount = (from accentity in context_M.Account
                                                          join acctypentity in context_M.AccountType
                                                          on accentity.intPkAccountType equals acctypentity.intPk
                                                          where accentity.intPkPrintshop == ps_I.intPk &&
                                                          accentity.boolAvailable &&
                                                          acctypentity.strType == "Asset" &&
                                                          accentity.boolGeneric == true &&
                                                          accentity.strNumber == AccAccounting.strAccountsReceivableNumber
                                                          select accentity).FirstOrDefault().intPk;

                            AccmoventityAccountMovementEntityDB accmoventityReceivableToUpdate =
                                context_M.AccountMovement.FirstOrDefault(accmov =>
                                accmov.intnPkInvoice == intPkInvoice_I &&
                                accmov.intPkAccount == intPkReceivableAccount);

                            if (
                                accmoventityReceivableToUpdate != null
                                )
                            {
                                double numTotal = numSubTotal + numTaxes;

                                //                              //Calculate increase or decrease amount that will affect
                                //                              //      account's balance.
                                if (
                                    //                          //Decrease account's amount.
                                    numTotal < accmoventityReceivableToUpdate.numnIncrease
                                    )
                                {
                                    double numDifference =
                                        (int)accmoventityReceivableToUpdate.numnIncrease - numTotal;
                                    //                          //Generate an decrease amount to the account's balance
                                    AccAccounting.subUpdateAccountBalance(intPkReceivableAccount,
                                        numDifference, null, context_M);
                                }
                                else
                                {
                                    //                          //Increase account's ammount.

                                    double numDifference =
                                        numTotal - (int)accmoventityReceivableToUpdate.numnIncrease;
                                    //                          //Generate an increase amount to the account's balance
                                    AccAccounting.subUpdateAccountBalance(intPkReceivableAccount, null,
                                        numDifference, context_M);
                                }

                                accmoventityReceivableToUpdate.numnIncrease = numTotal;
                                context_M.Update(accmoventityReceivableToUpdate);
                            }

                            String strEditedInvoice = JsonSerializer.Serialize(invjson);

                            //                                  //Calculate new open balance considering the new 
                            //                                  //      grand total.

                            double numNewGrandTotal = numSubTotal + numTaxes;
                            double numCurrentAmount = inventity.numAmount;

                            if (
                                numNewGrandTotal > numCurrentAmount
                                )
                            {
                                //                              //Add the difference to the open balance.
                                inventity.numOpenBalance = inventity.numOpenBalance +
                                    (numNewGrandTotal - numCurrentAmount);
                            }
                            else
                            {
                                //                              //Substract the difference to the open balance.
                                inventity.numOpenBalance = inventity.numOpenBalance -
                                    (numCurrentAmount - numNewGrandTotal);
                            }

                            //                                  //Update customer's balance.
                            AccAccounting.subUpdateCustomerBalance(numNewGrandTotal, inventity, context_M);

                            //                                  //Update invoice
                            inventity.strInvoiceJson = strEditedInvoice;
                            inventity.numAmount = numNewGrandTotal;
                            context_M.Invoice.Update(inventity);
                            context_M.SaveChanges();

                            intStatus_IO = 200;
                            strUserMessage_IO = "Success.";
                            strDevMessage_IO = "";
                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public static void subUpdateCustomerBalance(
            //                                              //Verify if invoice needs to be unbalanced and if so,
            //                                              //      reduce customer's balance.

            double numNewGrandTotal_I,
            InvoInvoiceEntityDB inventity_M,
            Odyssey2Context context_M
            )
        {
            if (
                //                                          //User changed something that made the price change.
                numNewGrandTotal_I != inventity_M.numAmount &&
                //                                          //Invoice has been balanced
                inventity_M.boolBalanced
                )
            {
                //                                          //Find customer balance.
                CustbalentityCustomerBalanceEntityDB custbalentity =
                    context_M.CustomerBalance.FirstOrDefault(custbal =>
                    custbal.intContactId == inventity_M.intContactId &&
                    custbal.intPkPrintshop == inventity_M.intPkPrintshop);

                if (
                    custbalentity != null
                    )
                {
                    //                                      //Current invoice it now not balanced.
                    inventity_M.boolBalanced = false;

                    //                                      //Reduce invoice's amount to the customer balance.
                    custbalentity.numBalance = custbalentity.numBalance + inventity_M.numAmount;

                    context_M.SaveChanges();
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static bool boolIsValidZipCode(
            //                                              //Validate a zipCode.
            //                                              //True if valid.

            String strZipCode_I,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            bool boolIsValidZipCode = false;
            if (
                strZipCode_I.Length > 0
                )
            {
                String strZipCode = strZipCode_I.TrimExcel();

                TaxentityTaxesEntityDB taxentity = context_M.Taxes.FirstOrDefault(tax => tax.strZipCode == strZipCode);

                if (
                    taxentity != null
                    )
                {
                    boolIsValidZipCode = true;

                    intStatus_IO = 200;
                    strUserMessage_IO = "";
                    strDevMessage_IO = "";
                }
                else
                {
                    boolIsValidZipCode = false;

                    intStatus_IO = 402;
                    strUserMessage_IO = "ZipCode not valid.";
                    strDevMessage_IO = "ZipCode not valid.";
                }
            }

            return boolIsValidZipCode;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subEnableDisable(
            //                                              //Set and account available/unavailable.
            //                                              //If an account becomes unavailable then remove account 
            //                                              //      from calculations and costs and set the generic
            //                                              //      expense account.

            int intPkAccount_I,
            bool boolEnabled_I,
            PsPrintShop ps_I,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Find account.
            AccentityAccountEntityDB accentity = context_M.Account.FirstOrDefault(acc => acc.intPk == intPkAccount_I &&
                acc.intPkPrintshop == ps_I.intPk &&
                //                                          //Account must not be a generic one.
                acc.boolGeneric == false);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Account not found or it is a generic account.";
            if (
                //                                          //Account exists.
                accentity != null
                )
            {
                if (
                    //                                      //If an account becomes unavailable, then change current
                    //                                      //      account to the generic expense one.
                    boolEnabled_I == false
                    )
                {
                    //                                      //Update relationships from account.
                    AccAccounting.subUpdateRelationshipsFromAccount(accentity, ps_I, context_M, ref intStatus_IO,
                        ref strUserMessage_IO, ref strDevMessage_IO);

                    if (
                        //                                  //Everything went Ok inside the update submethod.
                        intStatus_IO == 200
                        )
                    {
                        //                                  //Set value to unavailable.
                        accentity.boolAvailable = boolEnabled_I;
                        context_M.SaveChanges();
                    }
                }
                else
                {
                    //                                      //Set value to available.
                    accentity.boolAvailable = boolEnabled_I;
                    context_M.SaveChanges();

                    intStatus_IO = 200;
                    strUserMessage_IO = "";
                    strDevMessage_IO = "";
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subUpdateRelationshipsFromAccount(
            //                                              //Remove current assigned account and set the expense 
            //                                              //      generic one.

            //                                              //Current account 
            AccentityAccountEntityDB accentity_I,
            PsPrintShop ps_I,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Find expense account type.
            AcctypentityAccountTypeEntityDB acctypentity = context_M.AccountType.FirstOrDefault(acctype =>
                acctype.strType == AccAccounting.strAccountTypeExpense);

            intStatus_IO = 400;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Expense account type not found.";
            if (
                acctypentity != null
                )
            {
                //                                          //Find printshop's generic expense account.
                AccentityAccountEntityDB accentityGeneric = context_M.Account.FirstOrDefault(acc => acc.boolGeneric &&
                    acc.intPkAccountType == acctypentity.intPk && acc.intPkPrintshop == ps_I.intPk);

                intStatus_IO = 401;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Expense account not found.";
                if (
                    accentityGeneric != null
                    )
                {
                    //                                      //Find costs using current account.
                    List<CostentityCostEntityDB> darrcostentity = context_M.Cost.Where(cost =>
                        cost.intPkAccount == accentity_I.intPk).ToList();

                    //                                      //Update account.
                    foreach (CostentityCostEntityDB costentity in darrcostentity)
                    {
                        costentity.intPkAccount = accentityGeneric.intPk;
                        context_M.Cost.Update(costentity);
                    }

                    //                                      //Find calculations using current account.
                    List<CalentityCalculationEntityDB> darrcalentity = context_M.Calculation.Where(cal =>
                        cal.intnPkAccount == accentity_I.intPk).ToList();

                    //                                      //Update account.
                    foreach (CalentityCalculationEntityDB calentity in darrcalentity)
                    {
                        calentity.intnPkAccount = accentityGeneric.intPk;
                        context_M.Calculation.Update(calentity);
                    }

                    intStatus_IO = 200;
                    strUserMessage_IO = "";
                    strDevMessage_IO = "";
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subAddAccountTypes(
            //                                              //Add default account types to DB.
            )
        {
            //                                              //Create a connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Expense account.
            AcctypentityAccountTypeEntityDB acctypentityExpense = new AcctypentityAccountTypeEntityDB
            {
                strType = "Expense"
            };
            context.Add(acctypentityExpense);
            context.SaveChanges();

            //                                              //Revenue account.
            AcctypentityAccountTypeEntityDB acctypentityRevenue = new AcctypentityAccountTypeEntityDB
            {
                strType = "Revenue"
            };
            context.Add(acctypentityRevenue);
            context.SaveChanges();

            //                                              //Liability account.
            AcctypentityAccountTypeEntityDB acctypentityLiability = new AcctypentityAccountTypeEntityDB
            {
                strType = "Liability"
            };
            context.Add(acctypentityLiability);
            context.SaveChanges();

            //                                              //Asset account.
            AcctypentityAccountTypeEntityDB acctypentityAsset = new AcctypentityAccountTypeEntityDB
            {
                strType = "Asset"
            };
            context.Add(acctypentityAsset);
            context.SaveChanges();

            //                                              //Bank account.
            AcctypentityAccountTypeEntityDB acctypentityBank = new AcctypentityAccountTypeEntityDB
            {
                strType = "Bank"
            };
            context.Add(acctypentityBank);
            context.SaveChanges();
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subAddPaymentMethods(
            //                                              //Add default payment methods to DB.
            )
        {
            //                                              //Create a connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Cash method.
            PymtmtentityPaymentMethodEntityDB pymtmtentityCash = new PymtmtentityPaymentMethodEntityDB
            {
                strName = "Cash"
            };
            context.Add(pymtmtentityCash);
            context.SaveChanges();

            //                                              //Credit card method.
            PymtmtentityPaymentMethodEntityDB pymtmtentityCreditCard = new PymtmtentityPaymentMethodEntityDB
            {
                strName = "Credit card"
            };
            context.Add(pymtmtentityCreditCard);
            context.SaveChanges();

            //                                              //Check method.
            PymtmtentityPaymentMethodEntityDB pymtmtentityCheck = new PymtmtentityPaymentMethodEntityDB
            {
                strName = "Check"
            };
            context.Add(pymtmtentityCheck);
            context.SaveChanges();
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subSetAccountToProduct(
            //                                              //Set an account to a product.

            int intPkProduct_I,
            int intPkAccount_I,
            PsPrintShop ps_I,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //To easy code.
            EtentityElementTypeEntityDB etentity = context_M.ElementType.FirstOrDefault(et =>
                et.intPk == intPkProduct_I);

            bool boolIsValidProduct = false;
            if (
                etentity != null
                )
            {
                if (
                    etentity.intPrintshopPk == ps_I.intPk &&
                    etentity.strResOrPro == ProdtypProductType.strProduct &&
                    etentity.boolDeleted == false
                    )
                {
                    boolIsValidProduct = true;
                }
            }

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Product not valid.";
            if (
                //                                          //product is not valid.
                boolIsValidProduct
                )
            {
                //                                          //Verify Account.
                //                                              //Find printshop's accounts revenue and available.
                AccentityAccountEntityDB accentity = context_M.Account.FirstOrDefault(acc =>
                    acc.intPk == intPkAccount_I);

                //                                          //To easy code.
                bool boolIsValidAccount = false;
                if (
                    accentity != null
                    )
                {
                    //                                      //Verify AccountType
                    AcctypentityAccountTypeEntityDB acctypentity = context_M.AccountType.FirstOrDefault(acctyp =>
                        acctyp.intPk == accentity.intPkAccountType);

                    if (
                        accentity.intPkPrintshop == ps_I.intPk &&
                        accentity.boolAvailable &&
                        acctypentity.strType == AccAccounting.strAccountTypeRevenue
                        )
                    {
                        boolIsValidAccount = true;
                    }
                }

                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Account not valid.";
                if (
                    //                                      //Valid account
                    boolIsValidAccount
                    )
                {
                    //                                      //Set account to product. 
                    etentity.intnPkAccount = (int)accentity.intPk;
                    context_M.ElementType.Update(etentity);
                    context_M.SaveChanges();

                    intStatus_IO = 200;
                    strUserMessage_IO = "";
                    strDevMessage_IO = "";
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subAddPayment(
            //                                              //Add a customer's payment.

            int intContactId_I,
            String strDate_I,
            int? intnPkPaymentMethod_I,
            String strReference_I,
            int? intnPkAccount_I,
            double numAmountReceived_I,
            PsPrintShop ps_I,
            Odyssey2Context context_M,
            List<int> darrintPkInvoice_I,
            List<Crjson2CreditJson2> darrcrjson2_I,
            out List<InvoInvoiceEntityDB> darrinvoentity_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //This list will be use to verify if an Invoice is already 
            //                                              //      paid.
            //                                              //If true, notify to wisnet to update job paid status.
            darrinvoentity_O = new List<InvoInvoiceEntityDB>();

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Customer not valid.";
            if (
                CusCustomer.boolCustomerIsFromPrintshop(ps_I.strPrintshopId, intContactId_I)
                )
            {
                intStatus_IO = 402;
                if (
                    ZonedTimeTools.boolDateIsNotInTheFuture(strDate_I, ref strUserMessage_IO, ref strDevMessage_IO)
                    )
                {
                    ZonedTime ztime = ZonedTimeTools.NewZonedTime(strDate_I.ParseToDate(),
                        Time.Now(ZonedTimeTools.timezone));

                    //                                      //Find receivable account.
                    int intPkReceivableAccount = (from accentityRec in context_M.Account
                                                  join acctypentity in context_M.AccountType on
                                                  accentityRec.intPkAccountType equals acctypentity.intPk
                                                  where accentityRec.intPkPrintshop == ps_I.intPk &&
                                                  accentityRec.boolAvailable == true &&
                                                  acctypentity.strType == AccAccounting.strAccountTypeAsset &&
                                                  accentityRec.boolGeneric == true &&
                                                  accentityRec.strNumber == AccAccounting.strAccountsReceivableNumber
                                                  select accentityRec).FirstOrDefault().intPk;

                    /*CASE*/
                    if (
                        //                                  //Apply amount received
                        numAmountReceived_I > 0
                        )
                    {
                        PymtmtentityPaymentMethodEntityDB pymtmtentity;
                        AccentityAccountEntityDB accentity;
                        bool boolAllPaymentDataAreValid;
                        AccAccounting.subValidatePaymentData(ps_I.intPk, intnPkPaymentMethod_I, intnPkAccount_I, 
                            context_M, out boolAllPaymentDataAreValid, out pymtmtentity, out accentity,
                            ref strUserMessage_IO, ref strDevMessage_IO);

                        intStatus_IO = 403;
                        if (
                            boolAllPaymentDataAreValid
                            )
                        {
                            if (
                                //                          //Invoices are being paid
                                darrintPkInvoice_I.Count > 0
                                )
                            {
                                bool boolAllInvoicesAreValid;
                                List<InvoInvoiceEntityDB> darrinvoentity;
                                AccAccounting.subAllPkInvoicesAreValid(intContactId_I, ps_I.intPk, context_M,
                                    darrintPkInvoice_I, out boolAllInvoicesAreValid, out darrinvoentity,
                                    ref strUserMessage_IO, ref strDevMessage_IO);

                                intStatus_IO = 405;
                                if (
                                    //                      //All invoices are valid
                                    boolAllInvoicesAreValid
                                    )
                                {
                                    if (
                                        //                  //Credits are being used to pay the invoices
                                        darrcrjson2_I.Count > 0
                                        )
                                    {
                                        //                  //Apply the payment with the amount received and customer
                                        //                  //      credits
                                        AccAccounting.subApplyingAmountReceivedAndCredits(intContactId_I,
                                            ps_I.intPk, intPkReceivableAccount, numAmountReceived_I, strReference_I,
                                            pymtmtentity, accentity, ztime, context_M, darrinvoentity,
                                            darrcrjson2_I, ref intStatus_IO, ref strUserMessage_IO,
                                            ref strDevMessage_IO);
                                    }
                                    else
                                    //                      //Credits are not being used to pay the invoices
                                    {
                                        //                  //Apply the payment only with the amount received
                                        AccAccounting.subApplyingOnlyAmountReceived(intContactId_I, ps_I.intPk,
                                            intPkReceivableAccount, numAmountReceived_I, strReference_I,
                                            pymtmtentity, accentity, ztime, context_M, darrinvoentity,
                                            ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);
                                    }
                                }
                                //                          //To Verify if the invoice is completed paid and notify 
                                //                          //      to wisnet.
                                darrinvoentity_O = darrinvoentity;
                            }
                            else
                            //                              //Invoices are not being paid
                            {
                                PaymtPaymentEntityDB paymtentity;
                                //                          //Create a new payment
                                AccAccounting.subCreatePaymentAndMovements(intContactId_I, ps_I.intPk,
                                    strReference_I, numAmountReceived_I, pymtmtentity, accentity, ztime, context_M,
                                    out paymtentity);

                                intStatus_IO = 200;
                                strUserMessage_IO = "";
                                strDevMessage_IO = "";
                            }
                        }
                    }
                    else if (
                        //                                  //Apply credits
                        numAmountReceived_I == 0
                        )
                    {
                        intStatus_IO = 407;
                        strUserMessage_IO = "At least one invoice must be selected.";
                        strDevMessage_IO = "";
                        if (
                            //                              //Invoices are being paid
                            darrintPkInvoice_I.Count > 0
                            )
                        {
                            bool boolAllInvoicesAreValid;
                            List<InvoInvoiceEntityDB> darrinvoentity;
                            AccAccounting.subAllPkInvoicesAreValid(intContactId_I, ps_I.intPk, context_M,
                                darrintPkInvoice_I, out boolAllInvoicesAreValid, out darrinvoentity,
                                ref strUserMessage_IO, ref strDevMessage_IO);

                            intStatus_IO = 408;
                            if (
                                //                          //All invoices are valid
                                boolAllInvoicesAreValid
                                )
                            {
                                intStatus_IO = 409;
                                strUserMessage_IO = "At least one credit must be selected when the amount received" +
                                    " is zero.";
                                strDevMessage_IO = "";
                                if (
                                    //                      //Credits are being used to pay the invoices
                                    darrcrjson2_I.Count > 0
                                    )
                                {
                                    //                      //Apply the payment only with customer credits
                                    AccAccounting.subApplyingOnlyCredits(intContactId_I, ps_I.intPk,
                                        intPkReceivableAccount, numAmountReceived_I, ztime, context_M, darrinvoentity,
                                        darrcrjson2_I, ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);
                                }
                            }

                            //                              //To Verify if the invoice is completed paid and notify 
                            //                              //      to wisnet.
                            darrinvoentity_O = darrinvoentity;
                        }
                        else
                        //                                  //No invoices were selected
                        {
                            intStatus_IO = 412;
                            strUserMessage_IO = "An amount must be received or select at least one invoice and at least" +
                                " one credit.";
                            strDevMessage_IO = "";

                        }
                    }
                    else if (
                        numAmountReceived_I < 0
                        )
                    {
                        intStatus_IO = 411;
                        strUserMessage_IO = "Amount should be equal or greather than zero.";
                        strDevMessage_IO = "";
                    }
                    /*END-CASE*/
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subCreatePaymentAndMovements(
            //                                              //Create a new payment, also a bank account movement or
            //                                              //      a Undeposited funds movement

            int intContactId_I,
            int intPkPrintshop_I,
            String strReference_I,
            double numAmountReceived_I,
            PymtmtentityPaymentMethodEntityDB pymtmtentity_I,
            AccentityAccountEntityDB accentity_I,
            ZonedTime ztime_I,
            Odyssey2Context context_M,
            out PaymtPaymentEntityDB paymtentity_O
            )
        {
            //                                              //Create payment with amount received.
            PaymtPaymentEntityDB paymtentityNew = new PaymtPaymentEntityDB
            {
                intContactId = intContactId_I,
                strDate = ztime_I.Date.ToString(),
                numAmount = numAmountReceived_I,
                numOpenBalance = numAmountReceived_I,
                strReference = strReference_I,
                boolBalanced = false,
                intPkPrintshop = intPkPrintshop_I,
                intPkPaymentMethod = pymtmtentity_I.intPk,
            };

            context_M.Payment.Add(paymtentityNew);
            context_M.SaveChanges();

            int? intnPkBankDeposit = null;
            if (
                accentity_I.strName == AccAccounting.strUndepositedFundsName &&
                accentity_I.strNumber == AccAccounting.strUndepositedFundsNumber
                )
            {
                //                                          //Add movement to Undeposited Funds account.
                AccmoventityAccountMovementEntityDB accmoventity = new AccmoventityAccountMovementEntityDB
                {
                    strStartDate = ztime_I.Date.ToString(),
                    strStartTime = ztime_I.Time.ToString(),
                    strConcept = "Payment",
                    numnIncrease = numAmountReceived_I,
                    intPkAccount = accentity_I.intPk,
                    intnPkPayment = paymtentityNew.intPk
                };
                context_M.AccountMovement.Add(accmoventity);

                //                                          //Increase Undeposited Funds account balance
                accentity_I.numBalance = (accentity_I.numBalance + numAmountReceived_I);
            }
            else
            {
                AcctypentityAccountTypeEntityDB acctypentity = context_M.AccountType.FirstOrDefault(
                acctyp => acctyp.intPk == accentity_I.intPkAccountType);
                if (
                    acctypentity.strType == AccAccounting.strAccountTypeBank
                    )
                {
                    //                                      //Create bank deposit.
                    BkdptentityBankDepositEntityDB bkdptentityNew = new BkdptentityBankDepositEntityDB
                    {
                        strDate = ztime_I.Date.ToString(),
                        numAmount = numAmountReceived_I,
                        intPkBankAccount = accentity_I.intPk
                    };
                    context_M.BankDeposit.Add(bkdptentityNew);

                    context_M.SaveChanges();

                    intnPkBankDeposit = bkdptentityNew.intPk;

                    //                                      //Add movement to bank account.
                    AccmoventityAccountMovementEntityDB accmoventity = new AccmoventityAccountMovementEntityDB
                    {
                        strStartDate = ztime_I.Date.ToString(),
                        strStartTime = ztime_I.Time.ToString(),
                        strConcept = "Bank Deposit",
                        numnIncrease = numAmountReceived_I,
                        intPkAccount = accentity_I.intPk,
                        intnPkBankDeposit = intnPkBankDeposit
                    };
                    context_M.AccountMovement.Add(accmoventity);

                    //                                          //Increase bank account balance
                    accentity_I.numBalance = (accentity_I.numBalance + numAmountReceived_I);
                }
                else
                {
                    throw new Exception("Account not valid to make payments.");
                }
            }

            //                                              //Update bank deposit pk from the new payment
            paymtentityNew.intnPkBankDeposit = intnPkBankDeposit;
            paymtentity_O = paymtentityNew;

            context_M.SaveChanges();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subApplyingOnlyAmountReceived(
            //                                              //Apply the payment only with the amount received

            int intContactId_I,
            int intPkPrintshop_I,
            int intPkReceivableAccount_I,
            double numAmountReceived_I,
            String strReference_I,
            PymtmtentityPaymentMethodEntityDB pymtmtentity_I,
            AccentityAccountEntityDB accentity_I,
            ZonedTime ztime_I,
            Odyssey2Context context_M,
            List<InvoInvoiceEntityDB> darrinvoentity_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            PaymtPaymentEntityDB paymtentityNew;
            //                                              //Create a new payment
            AccAccounting.subCreatePaymentAndMovements(intContactId_I, intPkPrintshop_I, strReference_I, 
                numAmountReceived_I, pymtmtentity_I, accentity_I, ztime_I, context_M, out paymtentityNew);

            darrinvoentity_M = darrinvoentity_M.OrderBy(invo => invo.intPk).ToList();

            double numOpenAmountReceived = numAmountReceived_I;
            int intPkInvoice = 0;
            double numOpenBalanceInvoice = 0.00;
            //                                              //Index counter for invoices
            int intI = 0;
            //                                              //Apply new payment to the invoices
            /*WHILE-DO*/
            while (
                intI < darrinvoentity_M.Count && numOpenAmountReceived > 0
                )
            {
                intPkInvoice = darrinvoentity_M[intI].intPk;
                numOpenBalanceInvoice = darrinvoentity_M[intI].numOpenBalance;

                //                                          //Apply the amount received to the invoice current
                AccAccounting.subApplyingAmountReceived(intPkInvoice, paymtentityNew.intPk, ztime_I, context_M,
                    ref numOpenBalanceInvoice, ref numOpenAmountReceived);

                //                                          //Update invoice open balance
                darrinvoentity_M[intI].numOpenBalance = numOpenBalanceInvoice;
                intI++;
            }

            //                                              //Update open balance of the new payment
            paymtentityNew.numOpenBalance = numOpenAmountReceived;
            context_M.Payment.Update(paymtentityNew);

            //                                              //Amount to decrease from the Account Receivable
            double numAmountToDecrease = numAmountReceived_I - numOpenAmountReceived;

            //                                              //Create movement in the Account Receivable.
            //                                              //Add movement to accountMovement table.
            AccAccounting.subCreateDecreaseMovInRecAccount(intPkReceivableAccount_I, paymtentityNew.intPk, null,
                numAmountToDecrease, context_M, ztime_I);

            context_M.SaveChanges();

            intStatus_IO = 200;
            strUserMessage_IO = "";
            strDevMessage_IO = "";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subApplyingOnlyCredits(
            //                                              //Apply the payment only with customer credits

            int intContactId_I,
            int intPkPrintshop_I,
            int intPkReceivableAccount_I,
            double numAmountReceived_I,
            ZonedTime ztime_I,
            Odyssey2Context context_M,
            List<InvoInvoiceEntityDB> darrinvoentity_M,
            List<Crjson2CreditJson2> darrcrjson2_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            bool boolAllCreditsAreValid;
            List<CrmentityCreditMemoEntityDB> darrcrmentity;
            List<PaymtPaymentEntityDB> darrpaymtentity;
            AccAccounting.subAllCreditsAreValid(intContactId_I, intPkPrintshop_I, context_M, darrcrjson2_I,
                out boolAllCreditsAreValid, out darrcrmentity, out darrpaymtentity, ref strUserMessage_IO,
                ref strDevMessage_IO);

            intStatus_IO = 410;
            if (
                //                                          //All credits are valid
                boolAllCreditsAreValid
                )
            {
                bool boolIsThereBalance = true;
                double numOpenAmountReceived = numAmountReceived_I;
                darrinvoentity_M = darrinvoentity_M.OrderBy(invo => invo.intPk).ToList();

                //                                          //Dictionary to store pk and initial open balance of 
                //                                          //      payments
                Dictionary<int, double> dicOpenBalancePayment = new Dictionary<int, double>();

                //                                          //Dictionary to store pk and initial open balance of 
                //                                          //      credit memos
                Dictionary<int, double> dicOpenBalanceCreditMemo = new Dictionary<int, double>();

                int intPkInvoice = 0;
                int intPkPayment = 0;
                int intPkCreditMemo = 0;
                double numOpenBalanceInvoice = 0.00;
                double numOpenBalancePayment = 0.00;
                double numOpenBalanceCreditMemo = 0.00;

                bool boolUseTheNextPayment;
                bool boolUseTheNextCreditMemo;

                //                                          //Index counter for invoices
                int intI = 0;
                //                                          //Index counter for payments
                int intJ = 0;
                //                                          //Index counter for credit memos
                int intK = 0;
                //                                          //Apply credits to the invoices
                /*WHILE-DO*/
                while (
                    intI < darrinvoentity_M.Count
                    )
                {
                    intPkInvoice = darrinvoentity_M[intI].intPk;
                    numOpenBalanceInvoice = darrinvoentity_M[intI].numOpenBalance;

                    //                                      //Apply payments to the invoice
                    /*WHILE-DO*/
                    while (
                        intJ < darrpaymtentity.Count && numOpenBalanceInvoice > 0
                        )
                    {
                        intPkPayment = darrpaymtentity[intJ].intPk;
                        numOpenBalancePayment = darrpaymtentity[intJ].numOpenBalance;

                        if (
                            //                              //Payment is not in the dictionary yet
                            !dicOpenBalancePayment.ContainsKey(intPkPayment)
                            )
                        {
                            //                              //Add current payment to the dictionary
                            dicOpenBalancePayment.Add(intPkPayment, numOpenBalancePayment);
                        }

                        //                                  //Apply the current payment to the current invoice
                        AccAccounting.subApplyingPayment(intPkInvoice, intPkPayment, intPkReceivableAccount_I, intI,
                            (darrinvoentity_M.Count - 1), ztime_I, context_M, dicOpenBalancePayment,
                            out boolUseTheNextPayment, ref numOpenBalanceInvoice, ref numOpenBalancePayment);

                        //                                  //Update payment open balance
                        darrpaymtentity[intJ].numOpenBalance = numOpenBalancePayment;

                        if (
                            boolUseTheNextPayment
                            )
                        {
                            intJ++;
                        }
                    }

                    //                                      //Apply credit memos to the invoice
                    /*WHILE-DO*/
                    while (
                        intK < darrcrmentity.Count && numOpenBalanceInvoice > 0
                        )
                    {
                        intPkCreditMemo = darrcrmentity[intK].intPk;
                        numOpenBalanceCreditMemo = darrcrmentity[intK].numOpenBalance;

                        if (
                            //                              //Credit memo is not in the dictionary yet
                            !dicOpenBalanceCreditMemo.ContainsKey(intPkCreditMemo)
                            )
                        {
                            //                              //Add current credit memo to the dictionary
                            dicOpenBalanceCreditMemo.Add(intPkCreditMemo, numOpenBalanceCreditMemo);
                        }

                        //                                  //Apply the current payment to the current invoice
                        AccAccounting.subApplyingCreditMemo(intPkInvoice, intPkCreditMemo, intPkReceivableAccount_I,
                            intI, (darrinvoentity_M.Count - 1), ztime_I, context_M, dicOpenBalanceCreditMemo,
                            out boolUseTheNextCreditMemo, ref numOpenBalanceInvoice, ref numOpenBalanceCreditMemo);

                        //                                  //Update credit memo open balance
                        darrcrmentity[intK].numOpenBalance = numOpenBalanceCreditMemo;

                        if (
                            boolUseTheNextCreditMemo
                            )
                        {
                            intK++;
                        }
                    }

                    //                                      //Update invoice open balance
                    darrinvoentity_M[intI].numOpenBalance = numOpenBalanceInvoice;
                    intI++;
                }

                context_M.SaveChanges();

                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "";
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subApplyingAmountReceivedAndCredits(
            //                                              //Apply the payment with the amount received and customer
            //                                              //      credits

            int intContactId_I,
            int intPkPrintshop_I,
            int intPkReceivableAccount_I,
            double numAmountReceived_I,
            String strReference_I,
            PymtmtentityPaymentMethodEntityDB pymtmtentity_I,
            AccentityAccountEntityDB accentity_I,
            ZonedTime ztime_I,
            Odyssey2Context context_M,
            List<InvoInvoiceEntityDB> darrinvoentity_M,
            List<Crjson2CreditJson2> darrcrjson2_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            bool boolAllCreditsAreValid;
            List<CrmentityCreditMemoEntityDB> darrcrmentity;
            List<PaymtPaymentEntityDB> darrpaymtentity;
            AccAccounting.subAllCreditsAreValid(intContactId_I, intPkPrintshop_I, context_M, darrcrjson2_I, 
                out boolAllCreditsAreValid, out darrcrmentity,  out darrpaymtentity, ref strUserMessage_IO, 
                ref strDevMessage_IO);

            intStatus_IO = 406;
            if (
                //                                          //All credits are valid
                boolAllCreditsAreValid
                )
            {
                PaymtPaymentEntityDB paymtentityNew;
                //                                          //Create a new payment
                AccAccounting.subCreatePaymentAndMovements(intContactId_I, intPkPrintshop_I, strReference_I,
                    numAmountReceived_I, pymtmtentity_I, accentity_I, ztime_I, context_M, out paymtentityNew);

                bool boolIsThereBalance = true;
                double numOpenAmountReceived = numAmountReceived_I;
                darrinvoentity_M = darrinvoentity_M.OrderBy(invo => invo.intPk).ToList();

                //                                          //Dictionary to store pk and initial open balance of payments
                Dictionary<int, double> dicOpenBalancePayment = new Dictionary<int, double>();

                //                                          //Dictionary to store pk and initial open balance of creditMemo
                Dictionary<int, double> dicOpenBalanceCreditMemo = new Dictionary<int, double>();

                int intPkInvoice = 0;
                int intPkPayment = 0;
                int intPkCreditMemo = 0;
                double numOpenBalanceInvoice = 0.00;
                double numOpenBalanceCreditMemo = 0.00;
                double numOpenBalancePayment = 0.00;

                bool boolUseTheNextPayment;
                bool boolUseTheNextCreditMemo;

                //                                          //Index counter for invoices
                int intI = 0;
                //                                          //Index counter for payments
                int intJ = 0;
                //                                          //Index counter for credit memos
                int intK = 0;
                //                                          //Apply payments to the invoice
                /*WHILE-DO*/
                while (
                    intI < darrinvoentity_M.Count
                    )
                {
                    intPkInvoice = darrinvoentity_M[intI].intPk;
                    numOpenBalanceInvoice = darrinvoentity_M[intI].numOpenBalance;

                    //                                      //Apply the amount received to the invoice current
                    AccAccounting.subApplyingAmountReceived(intPkInvoice, paymtentityNew.intPk, ztime_I, context_M, 
                        ref numOpenBalanceInvoice, ref numOpenAmountReceived);

                    //                                      //Apply payments to the invoice
                    /*WHILE-DO*/
                    while (
                        intJ < darrpaymtentity.Count && numOpenBalanceInvoice > 0
                        )
                    {
                        intPkPayment = darrpaymtentity[intJ].intPk;
                        numOpenBalancePayment = darrpaymtentity[intJ].numOpenBalance;

                        if (
                            //                              //Payment is not in the dictionary yet
                            !dicOpenBalancePayment.ContainsKey(intPkPayment)
                            )
                        {
                            //                              //Add current payment to the dictionary
                            dicOpenBalancePayment.Add(intPkPayment, numOpenBalancePayment);
                        }

                        //                                  //Apply the current payment to the current invoice
                        AccAccounting.subApplyingPayment(intPkInvoice, intPkPayment, intPkReceivableAccount_I, intI,
                            (darrinvoentity_M.Count - 1), ztime_I, context_M, dicOpenBalancePayment, 
                            out boolUseTheNextPayment, ref numOpenBalanceInvoice, ref numOpenBalancePayment);

                        //                                  //Update payment open balance
                        darrpaymtentity[intJ].numOpenBalance = numOpenBalancePayment;

                        if (
                            boolUseTheNextPayment
                            )
                        {
                            intJ++;
                        }
                    }

                    //                                      //Apply credit memos to the invoice
                    /*WHILE-DO*/
                    while (
                        intK < darrcrmentity.Count && numOpenBalanceInvoice > 0
                        )
                    {
                        intPkCreditMemo = darrcrmentity[intK].intPk;
                        numOpenBalanceCreditMemo = darrcrmentity[intK].numOpenBalance;

                        if (
                            //                              //Credit memo is not in the dictionary yet
                            !dicOpenBalanceCreditMemo.ContainsKey(intPkCreditMemo)
                            )
                        {
                            //                              //Add current credit memo to the dictionary
                            dicOpenBalanceCreditMemo.Add(intPkCreditMemo, numOpenBalanceCreditMemo);
                        }

                        //                                  //Apply the current payment to the current invoice
                        AccAccounting.subApplyingCreditMemo(intPkInvoice, intPkCreditMemo, intPkReceivableAccount_I,
                            intI, (darrinvoentity_M.Count - 1), ztime_I, context_M, dicOpenBalanceCreditMemo,
                            out boolUseTheNextCreditMemo, ref numOpenBalanceInvoice, ref numOpenBalanceCreditMemo);

                        //                                  //Update credit memo open balance
                        darrcrmentity[intK].numOpenBalance = numOpenBalanceCreditMemo;

                        if (
                            boolUseTheNextCreditMemo
                            )
                        {
                            intK++;
                        }
                    }

                    //                                      //Update invoice open balance
                    darrinvoentity_M[intI].numOpenBalance = numOpenBalanceInvoice;
                    intI++;
                }

                //                                          //Update open balance of the new payment
                paymtentityNew.numOpenBalance = numOpenAmountReceived;
                context_M.Payment.Update(paymtentityNew);

                //                                          //Amount to decrease from the Account Receivable
                double numAmountToDecrease = numAmountReceived_I - numOpenAmountReceived;

                //                                          //Create movement in the Account receivable.
                //                                          //Add movement to accountMovement table.
                AccAccounting.subCreateDecreaseMovInRecAccount(intPkReceivableAccount_I, paymtentityNew.intPk, null, 
                    numAmountToDecrease, context_M, ztime_I);

                context_M.SaveChanges();

                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "";
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subApplyingAmountReceived(
            //                                              //Apply the amount received to the invoice current

            int intPkInvoice_I,
            int intPkNewPayment,
            ZonedTime ztime_I,
            Odyssey2Context context_M,
            ref double numOpenBalanceInvoice_IO,
            ref double numOpenAmountReceived_IO
            )
        {
            if (
                numOpenBalanceInvoice_IO <= numOpenAmountReceived_IO
                )
            {
                numOpenAmountReceived_IO = (numOpenAmountReceived_IO - numOpenBalanceInvoice_IO);

                //                                          //The outstanding balance of the current invoice has already
                //                                          //      been paid
                numOpenBalanceInvoice_IO = 0;

                //                                          //Create a record in AppliedPayment table for current 
                //                                          //      invoice with the new payment
                AccAccounting.subCreateAppliedPayment(intPkInvoice_I, intPkNewPayment, null, context_M, ztime_I);
            }
            else if (
                numOpenBalanceInvoice_IO > numOpenAmountReceived_IO && numOpenAmountReceived_IO > 0
                )
            {
                numOpenBalanceInvoice_IO = (numOpenBalanceInvoice_IO - numOpenAmountReceived_IO);

                numOpenAmountReceived_IO = 0;

                //                                          //Create a record in AppliedPayment table for current 
                //                                          //      invoice  with the new payment
                AccAccounting.subCreateAppliedPayment(intPkInvoice_I, intPkNewPayment, null, context_M, ztime_I);
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subApplyingPayment(
            //                                              //Apply the current payment to the current invoice

            int intPkInvoice_I,
            int intPkPayment_I,
            int intPkReceivableAccount_I,
            int intCurrentInvoiceIndex_I,
            int intLastInvoiceIndex_I,
            ZonedTime ztime_I,
            Odyssey2Context context_M,
            Dictionary<int, double> dicOpenBalancePayment_I,
            out bool boolUseTheNextPayment_O,
            ref double numOpenBalanceInvoice_IO,
            ref double numOpenBalancePayment_IO
            )
        {
            boolUseTheNextPayment_O = true;

            if (
                numOpenBalanceInvoice_IO <= numOpenBalancePayment_IO
                )
            {
                numOpenBalancePayment_IO = (numOpenBalancePayment_IO - numOpenBalanceInvoice_IO);

                //                                          //The outstanding balance of the current invoice has already
                //                                          //      been paid
                numOpenBalanceInvoice_IO = 0;

                //                                          //Create a record in AppliedPayment table for current 
                //                                          //      invoice with the current payment
                AccAccounting.subCreateAppliedPayment(intPkInvoice_I, intPkPayment_I, null, context_M, ztime_I);

                if (
                    //                                      //It is the last invoice
                    intCurrentInvoiceIndex_I == intLastInvoiceIndex_I
                    )
                {
                    //                                      //Find open balance initial from the payment
                    double numOpenBalancePaymentInitial = dicOpenBalancePayment_I.FirstOrDefault(open =>
                        open.Key == intPkPayment_I).Value;

                    //                                      //Amount to decrease from the Account Receivable
                    double numAmountToDecrease = numOpenBalancePaymentInitial - numOpenBalancePayment_IO;

                    //                                      //Create movement in the Account receivable.
                    AccAccounting.subCreateDecreaseMovInRecAccount(intPkReceivableAccount_I, intPkPayment_I, null, 
                        numAmountToDecrease, context_M, ztime_I);
                }

                //                                          //Keep using current payment
                boolUseTheNextPayment_O = false;
            }
            else if (
                numOpenBalanceInvoice_IO > numOpenBalancePayment_IO && numOpenBalancePayment_IO > 0
                )
            {
                numOpenBalanceInvoice_IO = (numOpenBalanceInvoice_IO - numOpenBalancePayment_IO);

                //                                          //All open balance of the current payment has already been
                //                                          //      applied
                numOpenBalancePayment_IO = 0;

                //                                          //Create a record in AppliedPayment table for current 
                //                                          //      invoice with the current payment
                AccAccounting.subCreateAppliedPayment(intPkInvoice_I, intPkPayment_I, null, context_M, ztime_I);

                //                                          //Find open balance initial from the payment
                double numOpenBalancePaymentInitial = dicOpenBalancePayment_I.FirstOrDefault(open =>
                    open.Key == intPkPayment_I).Value;

                //                                          //Amount to decrease from the Account Receivable
                double numAmountToDecrease = numOpenBalancePaymentInitial;

                //                                          //Create movement in the Account receivable.
                AccAccounting.subCreateDecreaseMovInRecAccount(intPkReceivableAccount_I, intPkPayment_I, null, 
                    numAmountToDecrease, context_M, ztime_I);

                //                                          //Use the next payment
                boolUseTheNextPayment_O = true;
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subApplyingCreditMemo(
            //                                              //Apply the current credit memo to the current invoice
            
            int intPkInvoice_I,
            int intPkCreditMemo_I,
            int intPkReceivableAccount_I,
            int intCurrentInvoiceIndex_I,
            int intLastInvoiceIndex_I,
            ZonedTime ztime_I,
            Odyssey2Context context_M,
            Dictionary<int, double> dicOpenBalanceCreditMemo_I,
            out bool boolUseTheNextCreditMemo_O,
            ref double numOpenBalanceInvoice_IO,
            ref double numOpenBalanceCreditMemo_IO
            )
        {
            boolUseTheNextCreditMemo_O = true; 

            if (
                numOpenBalanceInvoice_IO <= numOpenBalanceCreditMemo_IO
                )
            {
                numOpenBalanceCreditMemo_IO = (numOpenBalanceCreditMemo_IO - numOpenBalanceInvoice_IO);

                //                                          //The outstanding balance of the current invoice has already
                //                                          //      been paid
                numOpenBalanceInvoice_IO = 0;

                //                                          //Create a record in AppliedPayment table for current 
                //                                          //      invoice with the current credit memo
                AccAccounting.subCreateAppliedPayment(intPkInvoice_I, null, intPkCreditMemo_I, context_M, ztime_I);

                if (
                    //                                      //It is the last invoice
                    intCurrentInvoiceIndex_I == intLastInvoiceIndex_I
                    )
                {
                    //                                      //Find open balance initial from the credit memo
                    double numOpenBalanceCreditMemoInitial = dicOpenBalanceCreditMemo_I.FirstOrDefault(open =>
                        open.Key == intPkCreditMemo_I).Value;

                    //                                      //Amount to decrease from the Account Receivable
                    double numAmountToDecrease = numOpenBalanceCreditMemoInitial - numOpenBalanceCreditMemo_IO;

                    //                                      //Create movement in the Account receivable.
                    AccAccounting.subCreateDecreaseMovInRecAccount(intPkReceivableAccount_I, null, intPkCreditMemo_I,
                        numAmountToDecrease, context_M, ztime_I);
                }

                //                                          //Keep using current memo
                boolUseTheNextCreditMemo_O = false;
            }
            else if (
                numOpenBalanceInvoice_IO > numOpenBalanceCreditMemo_IO && numOpenBalanceCreditMemo_IO > 0
                )
            {
                numOpenBalanceInvoice_IO = (numOpenBalanceInvoice_IO - numOpenBalanceCreditMemo_IO);

                //                                          //All open balance of the current credit memo has already 
                //                                          //      been applied
                numOpenBalanceCreditMemo_IO = 0;

                //                                          //Create a record in AppliedPayment table for current 
                //                                          //      invoice with the current credit memo
                AccAccounting.subCreateAppliedPayment(intPkInvoice_I, null, intPkCreditMemo_I, context_M, ztime_I);

                //                                          //Find open balance initial from the credit memo
                double numOpenBalanceCreditMemoInitial = dicOpenBalanceCreditMemo_I.FirstOrDefault(open =>
                    open.Key == intPkCreditMemo_I).Value;

                //                                          //Amount to decrease from the Account Receivable
                double numAmountToDecrease = numOpenBalanceCreditMemoInitial;

                //                                          //Create movement in the Account receivable.
                AccAccounting.subCreateDecreaseMovInRecAccount(intPkReceivableAccount_I, null, intPkCreditMemo_I, 
                    numAmountToDecrease, context_M, ztime_I);

                //                                          //Use the next credit memo
                boolUseTheNextCreditMemo_O = true;
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subValidatePaymentData(
            //                                              //Check if the payment data are valid

            int intPkPrintShop_I,
            int? intnPkPaymentMethod_I,
            int? intnPkAccount_I,
            Odyssey2Context context_M,
            out bool boolAllPaymentDataAreValid_O,
            out PymtmtentityPaymentMethodEntityDB pymtmtentity_O,
            out AccentityAccountEntityDB accentity_O,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            pymtmtentity_O = null;
            accentity_O = null;

            boolAllPaymentDataAreValid_O = false;

            strUserMessage_IO = "A payment method must be selected.";
            strDevMessage_IO = "";
            if (
                intnPkPaymentMethod_I != null
                )
            {
                pymtmtentity_O = context_M.PaymentMethod.FirstOrDefault(pymtmtm =>
                pymtmtm.intPk == intnPkPaymentMethod_I);

                strUserMessage_IO = "Payment method not valid.";
                strDevMessage_IO = "";
                if (
                     pymtmtentity_O != null
                    )
                {
                    strUserMessage_IO = "A account must be selected.";
                    strDevMessage_IO = "";
                    if (
                        intnPkAccount_I != null
                        )
                    {
                        accentity_O = (from accentity in context_M.Account
                                       join acctypentity in context_M.AccountType on
                                       accentity.intPkAccountType equals acctypentity.intPk
                                       where accentity.intPkPrintshop == intPkPrintShop_I &&
                                       accentity.boolAvailable == true &&
                                       accentity.intPk == intnPkAccount_I &&
                                       (acctypentity.strType == AccAccounting.strAccountTypeAsset ||
                                       acctypentity.strType == AccAccounting.strAccountTypeBank)
                                       select accentity).FirstOrDefault();

                        strUserMessage_IO = "Account not valid.";
                        strDevMessage_IO = "";
                        if (
                            accentity_O != null
                            )
                        {
                            boolAllPaymentDataAreValid_O = true;
                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subAllPkInvoicesAreValid(
            //                                              //Check if all pk invoice are valid
            int intContactId_I,
            int intPkPrintShop_I,
            Odyssey2Context context_M,
            List<int> darrintPkInvoice_I,
            out bool boolAllPkInvoicesAreValid_O,
            out List<InvoInvoiceEntityDB> darrinvoentity_O,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            darrinvoentity_O = new List<InvoInvoiceEntityDB>();

            boolAllPkInvoicesAreValid_O = true;

            bool boolNoRepeatedPkInvoices = true;
            String strMsg = null;
            //                                              //Check for repeating pks
            foreach (var pkgroup in darrintPkInvoice_I.GroupBy(pk => pk).Where(
                pk => pk.Count() != 1))
            {
                strMsg = string.Format("Pk invoice '{0}' is repeated {1} times.", pkgroup.Key,
                    pkgroup.Count());
                boolNoRepeatedPkInvoices = false;
                break;
            }

            if (
                //                                          //No pk invoice is repeated
                boolNoRepeatedPkInvoices
                )
            {
                darrintPkInvoice_I.Sort();

                bool boolAreAllPksValid = true;

                int intI = 0;
                //                                          //Check if all pk are valid
                /*WHILE-DO*/
                while (
                    intI < darrintPkInvoice_I.Count && boolAreAllPksValid
                    )
                {
                    InvoInvoiceEntityDB invoentity = context_M.Invoice.FirstOrDefault(invo =>
                        invo.intPk == darrintPkInvoice_I[intI] && invo.intPkPrintshop == intPkPrintShop_I &&
                        invo.intContactId == intContactId_I && invo.numOpenBalance > 0);

                    if (
                        invoentity != null
                        )
                    {
                        darrinvoentity_O.Add(invoentity);
                    }
                    else
                    {
                        boolAreAllPksValid = false;
                    }

                    intI = intI + 1;
                }

                if (
                    //                                      //Is there any invalid pk
                    !boolAreAllPksValid
                    )
                {
                    boolAllPkInvoicesAreValid_O = false;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "Is there any invalid invoice.";
                }
            }
            else
            {
                boolAllPkInvoicesAreValid_O = false;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = strMsg;
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subAllCreditsAreValid(
            //                                              //Check if all credits are valid 

            int intContactId_I,
            int intPkPrintShop_I,
            Odyssey2Context context_M,
            List<Crjson2CreditJson2> darrcrjson2Credits_I,
            out bool boolAllCreditsAreValid_O,
            out List<CrmentityCreditMemoEntityDB> darrcrmentity_O,
            out List<PaymtPaymentEntityDB> darrpaymtentity_O,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            boolAllCreditsAreValid_O = true;

            darrcrmentity_O = new List<CrmentityCreditMemoEntityDB>();
            darrpaymtentity_O = new List<PaymtPaymentEntityDB>();

            List<Crjson2CreditJson2> crjson2CreditMemos = darrcrjson2Credits_I.Where(cr => 
                cr.boolIsCreditMemo == true).ToList();

            List<Crjson2CreditJson2> crjson2Payments = darrcrjson2Credits_I.Where(cr =>
                cr.boolIsCreditMemo == false).ToList();

            bool boolNoRepeatedCredits = true;
            String strMsg = null;
            //                                              //Check for repeating pk credit memos
            foreach (var pkgroup in crjson2CreditMemos.GroupBy(pk => pk.intPkCredit).Where(
                pk => pk.Count() != 1))
            {
                strMsg = string.Format("Pk credit memo '{0}' is repeated {1} times.", pkgroup.Key,
                    pkgroup.Count());
                boolNoRepeatedCredits = false;
                break;
            }

            //                                              //Check for repeating pk payments
            foreach (var pkgroup in crjson2Payments.GroupBy(pk => pk.intPkCredit).Where(
                pk => pk.Count() != 1))
            {
                strMsg = string.Format("Pk payment '{0}' is repeated {1} times.", pkgroup.Key,
                    pkgroup.Count());
                boolNoRepeatedCredits = false;
                break;
            }

            if (
                //                                          //No credit is repeated
                boolNoRepeatedCredits
                )
            {
                bool boolAreAllPksValid = true;

                int intI = 0;
                //                                          //Check if all pk credit memo are valid
                /*WHILE-DO*/
                while (
                    intI < crjson2CreditMemos.Count && boolAreAllPksValid
                    )
                {
                    CrmentityCreditMemoEntityDB crmentity = context_M.CreditMemo.FirstOrDefault(crm =>
                        crm.intPk == crjson2CreditMemos[intI].intPkCredit && crm.intPkPrintshop == intPkPrintShop_I &&
                        crm.intContactId == intContactId_I && crm.numOpenBalance > 0);

                    if (
                        crmentity != null
                        )
                    {
                        darrcrmentity_O.Add(crmentity);
                    }
                    else
                    {
                        boolAreAllPksValid = false;
                    }

                    intI = intI + 1;
                }

                darrcrmentity_O.Sort();

                int intJ= 0;
                //                                          //Check if all pk credit memo are valid
                /*WHILE-DO*/
                while (
                    intJ < crjson2Payments.Count && boolAreAllPksValid
                    )
                {
                    PaymtPaymentEntityDB paymtentity = context_M.Payment.FirstOrDefault(paymt =>
                        paymt.intPk == crjson2Payments[intJ].intPkCredit && paymt.intPkPrintshop == intPkPrintShop_I &&
                        paymt.intContactId == intContactId_I && paymt.numOpenBalance > 0);

                    if (
                        paymtentity != null
                        )
                    {
                        darrpaymtentity_O.Add(paymtentity);
                    }
                    else
                    {
                        boolAreAllPksValid = false;
                    }

                    intJ = intJ + 1;
                }

                darrpaymtentity_O.Sort();

                if (
                    //                                      //Is there any invalid pk
                    !boolAreAllPksValid
                    )
                {
                    boolAllCreditsAreValid_O = false;

                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "Is there any invalid credit.";
                }
            }
            else
            {
                boolAllCreditsAreValid_O = false;

                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = strMsg;
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subCreateAppliedPayment(
            //                                              //Create an applied payment record in the database

            int intPkInvoice_I,
            int? intnPkPayment_I,
            int? intnPkCreditMemo_I,
            Odyssey2Context context_M,
            ZonedTime ztime_I
            )
        {
            //                                              //Create applied payment.
            AplpayentityApliedPaymentsEntityDB aplpayentityNew = new AplpayentityApliedPaymentsEntityDB
            {
                strDate = ztime_I.Date.ToString(),
                strTime = ztime_I.Time.ToString(),
                intPkInvoice = intPkInvoice_I,
                intnPkPayment = intnPkPayment_I,
                intnPkCreditMemo = intnPkCreditMemo_I

            };
            context_M.ApliedPayment.Add(aplpayentityNew);
            context_M.SaveChanges();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subCreateDecreaseMovInRecAccount(
            //                                              //Create a decrease movement in receivable account

            int intPkReceivableAccount_I,
            int? intnPkPayment_I,
            int? intnPkCreditMemo_I,
            double numAmountToDecrease_I,
            Odyssey2Context context_M,
            ZonedTime ztime_I
            )
        {
            //                                              //Find the Account receivable.
            AccentityAccountEntityDB accentityReceivable = context_M.Account.FirstOrDefault(acc =>
                acc.intPk == intPkReceivableAccount_I);

            //                                              //Decrease Account receivable account balance
            accentityReceivable.numBalance = (accentityReceivable.numBalance - numAmountToDecrease_I);

            //                                              //Create movement in the Account receivable.
            AccmoventityAccountMovementEntityDB accmoventityReceivable = new AccmoventityAccountMovementEntityDB
                {
                    strStartDate = ztime_I.Date.ToString(),
                    strStartTime = ztime_I.Time.ToString(),
                    strConcept = "Customer payment",
                    numnDecrease = numAmountToDecrease_I,
                    intPkAccount = accentityReceivable.intPk,
                    intnPkPayment = intnPkPayment_I,
                    intnPkCreditMemo = intnPkCreditMemo_I
                };

            context_M.AccountMovement.Add(accmoventityReceivable);

            context_M.SaveChanges();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static List<int> subSendToWisnetInvoicesAlreadyPaid(
            //                                              //Send to Wisnet those invoices already paid in order
            //                                              //      to update paid status.

            PsPrintShop ps_I,
            List<InvoInvoiceEntityDB> darrinvoentity_I,
            Odyssey2Context context_M
            )
        {
            //                                              //Orders id already completed.
            List<int> darrintOrdersId = darrinvoentity_I.Count > 0 ? new List<int>() : null;
            //                                              //Verify each invoice and send to Wisnet those already
            //                                              //      paid.

            foreach(InvoInvoiceEntityDB inventity in darrinvoentity_I)
            {
                if (
                    //                                      //Invoice is paid.
                    inventity.numOpenBalance == 0 
                    )
                {
                    darrintOrdersId.Add(inventity.intOrderNumber);

                    //                                      //Send info to wisnet.
                    String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                        GetSection("Odyssey2Settings")["urlWisnetApi"];

                    Task<String> Task_PostSendToWisnetInvoicesAlreadyPaid =
                    HttpTools<TjsonTJson>.PostUpdateInvoiceToPaidStage(strUrlWisnet +
                        "/PrintshopData/SetJobAsPaid/", ps_I.strPrintshopId, inventity.intOrderNumber);
                    Task_PostSendToWisnetInvoicesAlreadyPaid.Wait();

                    if (
                        //                                  //The Order was updated in Wisnet.
                        Task_PostSendToWisnetInvoicesAlreadyPaid.Result.Contains("200")
                        )
                    {
                        inventity.boolnOnWisnet = true;
                        context_M.Invoice.Update(inventity);
                    }
                    else
                    {
                        inventity.boolnOnWisnet = false;
                        context_M.Invoice.Update(inventity);
                    }
                }
            }
            context_M.SaveChanges();

            return darrintOrdersId;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subAddCreditMemo(
            //                                              //Add credit memo to a customer.

            int intContactId_I,
            String strCustomerFullName_I,
            String strDate_I,
            String strBilledTo_I,
            String strDescription_I,
            int intPkRevenueAccount_I,
            double numAmount_M,
            bool boolIsExempt_I,
            PsPrintShop psPrintshop_I,
            out CredmemjsonCreditMemoJson credmemjson_O,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Json to return.
            credmemjson_O = null;

            if (
                AccAccounting.boolIsCreditMemoDataCorrect(intContactId_I, strCustomerFullName_I, strDate_I, 
                strBilledTo_I, strDescription_I, numAmount_M, psPrintshop_I, context_M, ref intStatus_IO, 
                ref strUserMessage_IO, ref strDevMessage_IO)
                )
            {
                //                                          //Create fake customer's memo.
                CrmentityCreditMemoEntityDB crmentity = new CrmentityCreditMemoEntityDB
                {
                    intContactId = intContactId_I,
                    strCreditMemoNumber = "",
                    numOriginalAmount = numAmount_M,
                    numOpenBalance = numAmount_M,
                    strMemo = "",
                    strDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                    boolBalanced = false,
                    intPkPrintshop = psPrintshop_I.intPk
                };
                context_M.CreditMemo.Add(crmentity);
                context_M.SaveChanges();

                //                                          //Credit memo number is the Pk number;
                String strCreditMemoNumber = crmentity.intPk.ToString();
                crmentity.strCreditMemoNumber = strCreditMemoNumber;

                //                                          //Create memo json that will live inside the memo.
                String strMemo = AccAccounting.strMemoJson(strCustomerFullName_I, strCreditMemoNumber,
                    strDate_I, strBilledTo_I, strDescription_I, numAmount_M);
                //                                          //Assign memo.
                crmentity.strMemo = strMemo;
                context_M.Update(crmentity);
                context_M.SaveChanges();

                //                                          //Create one movement for the general tax payable account
                //                                          //      and put the PkMemo just created.
                double numAmountWithTax;
                AccAccounting.subAddTaxAccountMovementForMemo(numAmount_M, crmentity,
                    psPrintshop_I, boolIsExempt_I, context_M, out numAmountWithTax, ref intStatus_IO,
                    ref strUserMessage_IO, ref strDevMessage_IO);

                if (
                    intStatus_IO == 200
                    )
                {
                    //                                      //Create one movement for the general receivable account
                    //                                      //      and put the PkMemo just created.
                    AccAccounting.subAddReceivableAccountMovementForMemo(numAmountWithTax, crmentity,
                        psPrintshop_I, context_M, ref intStatus_IO, ref strUserMessage_IO,
                        ref strDevMessage_IO);

                    if (
                        intStatus_IO == 200
                        )
                    {
                        //                                  //Create one movement for the revenue account selected
                        //                                  //      and put the PkMemo just created.
                        AccAccounting.subAddRevenueAccountMovementForMemo(intPkRevenueAccount_I,
                            numAmount_M, crmentity, psPrintshop_I, context_M, ref intStatus_IO,
                            ref strUserMessage_IO, ref strDevMessage_IO);

                        //                                  //This is the new amount considering taxes.
                        numAmount_M = numAmountWithTax;
                        //                                  //Update credit amounts with the new value that is
                        //                                  //      considering taxes.
                        crmentity.numOpenBalance = numAmount_M;
                        crmentity.numOriginalAmount = numAmount_M;

                        context_M.SaveChanges();
                    }
                }
                
                if (
                    intStatus_IO == 200
                    )
                {
                    //                                      //Create json that will be used to generate PDF.
                    credmemjson_O = AccAccounting.credmemjsonGenerateCreditMemoJson(strCustomerFullName_I,
                        strCreditMemoNumber, strDate_I, strBilledTo_I, strDescription_I, numAmount_M, psPrintshop_I);
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public static bool boolIsCreditMemoDataCorrect(
            int intContactId_I,
            String strCustomerFullName_I,
            String strDate_I,
            String strBilledTo_I,
            String strDescription_I,
            double numAmount_I,
            PsPrintShop psPrintshop_I,
            Odyssey2Context context_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            bool boolIsCreditMemoDataCorrect = false;

            intStatus_IO = 402;
            strUserMessage_IO = "Amount must be greater than zero.";
            strDevMessage_IO = "";
            if (
                numAmount_I > 0
                )
            {
                intStatus_IO = 403;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Invalid customer name.";
                if (
                    strCustomerFullName_I != null &&
                    strCustomerFullName_I.Length > 0
                    )
                {
                    intStatus_IO = 404;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "Invalid date.";
                    if (
                        strDate_I != null &&
                        strDate_I.IsParsableToDate()
                        )
                    {
                        Date dateNow = Date.Now(ZonedTimeTools.timezone);
                        Date dateDate_I = strDate_I.ParseToDate();

                        intStatus_IO = 405;
                        strUserMessage_IO = "Date cannot be in the future.";
                        strDevMessage_IO = "Date cannot be in the future.";
                        if (
                            dateDate_I <= dateNow
                            )
                        {
                            intStatus_IO = 406;
                            strUserMessage_IO = "Write a description.";
                            strDevMessage_IO = "";
                            if (
                                strDescription_I != null &&
                                strDescription_I.Length > 0
                                )
                            {
                                intStatus_IO = 407;
                                strUserMessage_IO = "Write a Billed To address.";
                                strDevMessage_IO = "";
                                if (
                                    strBilledTo_I != null &&
                                    strBilledTo_I.Length > 0
                                    )
                                {
                                    CusjsonCustomerJson cusjson;
                                    //                      //Find customer.
                                    CusCustomer.subGetOneCustomerFromPrintshop(psPrintshop_I, intContactId_I,
                                        out cusjson, ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);

                                    if (
                                        cusjson != null
                                        )
                                    {
                                        boolIsCreditMemoDataCorrect = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return boolIsCreditMemoDataCorrect;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public static String strMemoJson(
            String strCustomerFullName_I,
            String strCreditMemoNumber_I,
            String strDate_I,
            String strBilledTo_I,
            String strDescription_I, 
            double numAmount_I
            )
        {
            //                                              //Create json.
            MemojsonMemoJson memojson = new MemojsonMemoJson(strCustomerFullName_I, strCreditMemoNumber_I,
                strDate_I, strBilledTo_I, strDescription_I, numAmount_I);
            String strMemoJson = JsonSerializer.Serialize(memojson);

            return strMemoJson;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public static void subAddReceivableAccountMovementForMemo(
            //                                              //Add an account movement to the receivable account.

            double numAmount_I,
            CrmentityCreditMemoEntityDB crmentity_I,
            PsPrintShop psPrintshop_I,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Find printshop's receivable account.
            int intPkReceivableAccount = (from accentity in context_M.Account
                                          join acctypentity in context_M.AccountType
                                          on accentity.intPkAccountType equals acctypentity.intPk
                                          where accentity.intPkPrintshop == psPrintshop_I.intPk &&
                                          accentity.boolAvailable &&
                                          acctypentity.strType == "Asset" &&
                                          accentity.boolGeneric == true &&
                                          accentity.strNumber == AccAccounting.strAccountsReceivableNumber
                                          select accentity).FirstOrDefault().intPk;

            //                                              //Create movement.

            //                                              //Decrease receivable account's money.
            AccmoventityAccountMovementEntityDB accmoventityNew = new AccmoventityAccountMovementEntityDB
            {
                strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                strStartTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                strConcept = "Credit memo receivable",
                //                                          //It is a decrease movement.
                numnDecrease = numAmount_I,
                intPkAccount = intPkReceivableAccount,
                intnPkCreditMemo = crmentity_I.intPk
            };
            context_M.AccountMovement.Add(accmoventityNew);

            //                                              //Generate an decrease amount to the account's balance
            AccAccounting.subUpdateAccountBalance(intPkReceivableAccount, numAmount_I, null,
                context_M);

            intStatus_IO = 200;
            strUserMessage_IO = "";
            strDevMessage_IO = "";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public static void subAddTaxAccountMovementForMemo(
            //                                              //Add an account movement to the Tax payable account.

            double numAmount_I,
            CrmentityCreditMemoEntityDB crmentity_I,
            PsPrintShop psPrintshop_I,
            bool boolIsExempt_I,
            Odyssey2Context context_M,
            out double numAmountWithTax_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //If user does not want taxes, return original amount.
            numAmountWithTax_O = numAmount_I;

            if (
                //                                          //User wants to consider taxes.
                !boolIsExempt_I
                )
            {
                //                                          //Calculate tax rate using printshop's zip.
                double numTax = AccAccounting.numTaxUsingPrintshopZip(psPrintshop_I, numAmount_I, context_M, 
                    ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);

                if (
                    //                                      //Everything went okay on wisnet.
                    intStatus_IO == 200
                    )
                {
                    //                                      //Calculate new amount.
                    numAmountWithTax_O = numAmountWithTax_O + numTax;

                    //                                      //Create AccountMovement for liability account.
                    //                                      //Takes liability account.
                    int intPkLiabilityAccount = (from accentity in context_M.Account
                                                 join acctypentity in context_M.AccountType
                                                 on accentity.intPkAccountType equals acctypentity.intPk
                                                 where accentity.intPkPrintshop == psPrintshop_I.intPk &&
                                                 accentity.boolAvailable &&
                                                 acctypentity.strType == "Liability" &&
                                                 accentity.boolGeneric == true &&
                                                 accentity.strNumber == AccAccounting.strSalesTaxPayableNumber
                                                 select accentity).FirstOrDefault().intPk;

                    //                                      //Create movement.

                    //                                      //Decrease tax account's money.
                    AccmoventityAccountMovementEntityDB accmoventityNew = new AccmoventityAccountMovementEntityDB
                    {
                        strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                        strStartTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                        strConcept = "Credit memo tax",
                        //                                  //It is a decrease movement.
                        numnDecrease = numTax,
                        intPkAccount = intPkLiabilityAccount,
                        intnPkCreditMemo = crmentity_I.intPk
                    };
                    context_M.AccountMovement.Add(accmoventityNew);

                    //                                      //Generate an decrease amount to the account's balance
                    AccAccounting.subUpdateAccountBalance(intPkLiabilityAccount, numTax, null, context_M);

                    intStatus_IO = 200;
                    strUserMessage_IO = "";
                    strDevMessage_IO = "";
                }
            }
            else
            {
                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "";
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public static double numTaxUsingPrintshopZip(
            //                                              //Calculate tax rate using printshop's zip code.

            PsPrintShop psPrintshop_I,
            double numAmount_I,
            Odyssey2Context context_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Amount to return.
            double numTaxUsingPrintshopZip = 0.0;

            //                                              //Find zip code on wisnet.

            String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                GetSection("Odyssey2Settings")["urlWisnetApi"];
            //                                              //Get info from wisnet.
            Task<PrintzipjsonPrintshopZipJson> Task_printzipjsonFromWisnet =
            HttpTools<PrintzipjsonPrintshopZipJson>.GetOneAsyncToEndPoint(strUrlWisnet +
                "/PrintShopData/PrintshopZipCode/" + psPrintshop_I.strPrintshopId);
            Task_printzipjsonFromWisnet.Wait();

            intStatus_IO = 400;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "There is a problem with wisnet.";
            if (
                Task_printzipjsonFromWisnet.Result != null
                )
            {
                //                                          //Find tax rate.
                TaxentityTaxesEntityDB taxentity = context_I.Taxes.FirstOrDefault(tax =>
                    tax.strZipCode == Task_printzipjsonFromWisnet.Result.strZipCode);
                //                                          //Calculate taxes.
                numTaxUsingPrintshopZip = taxentity != null ? numAmount_I * taxentity.numTaxValue : 0.0;

                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "";
            }

            return numTaxUsingPrintshopZip;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public static void subAddRevenueAccountMovementForMemo(
            int intPkRevenueAccount_I,
            double numAmount_I,
            CrmentityCreditMemoEntityDB crmentity_I,
            PsPrintShop psPrintshop_I,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Find account.
            AccentityAccountEntityDB accentity = context_M.Account.FirstOrDefault(acc =>
                acc.intPk == intPkRevenueAccount_I && acc.intPkPrintshop == psPrintshop_I.intPk &&
                acc.boolAvailable);

            intStatus_IO = 408;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Revenue account not found.";
            if (
                accentity != null
                )
            {
                //                                          //Find account type.
                AcctypentityAccountTypeEntityDB acctypentity = context_M.AccountType.FirstOrDefault(acctype =>
                    acctype.intPk == accentity.intPkAccountType);

                intStatus_IO = 409;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Account is not revenue type.";
                if (
                    acctypentity != null &&
                    acctypentity.strType == AccAccounting.strAccountTypeRevenue
                    )
                {
                    //                                      //Create movement.
                    //                                      //Decrease revenue account's money.
                    AccmoventityAccountMovementEntityDB accmoventityNew =
                        new AccmoventityAccountMovementEntityDB
                        {
                            strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                            strStartTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                            strConcept = "Credit memo revenue",
                            //                              //It is a decrease movement.
                            numnDecrease = numAmount_I,
                            intPkAccount = accentity.intPk,
                            intnPkCreditMemo = crmentity_I.intPk
                        };
                    context_M.AccountMovement.Add(accmoventityNew);

                    //                                      //Generate an decrease amount to the account's balance
                    AccAccounting.subUpdateAccountBalance(accentity.intPk, numAmount_I, null, context_M);

                    intStatus_IO = 200;
                    strUserMessage_IO = "";
                    strDevMessage_IO = "";
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public static CredmemjsonCreditMemoJson credmemjsonGenerateCreditMemoJson(
            String strCustomerFullName_I,
            String strCreditMemoNumber_I,
            String strDate_I,
            String strBilledTo_I,
            String strDescription_I, 
            double numAmount_I, 
            PsPrintShop psPrintshop_I
            )
        {
            //                                              //Find printshop's logo's url.

            String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                GetSection("Odyssey2Settings")["urlWisnetApi"];
            //                                              //Get info from wisnet.
            Task<PrintlogurljsonPrintshopLogoUrlJson> Task_printzipjsonFromWisnet =
            HttpTools<PrintlogurljsonPrintshopLogoUrlJson>.GetOneAsyncToEndPoint(strUrlWisnet +
                "/PrintShopData/PrintshopLogoUrl/" + psPrintshop_I.strPrintshopId);
            Task_printzipjsonFromWisnet.Wait();

            String strLogoUrl = "";
            if (
                Task_printzipjsonFromWisnet.Result != null
                )
            {
                strLogoUrl = Task_printzipjsonFromWisnet.Result.strLogoUrl;
            }

            return new CredmemjsonCreditMemoJson(strCustomerFullName_I,strCreditMemoNumber_I, strDate_I, strBilledTo_I, 
                strLogoUrl, strDescription_I, numAmount_I.Round(2));
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public static List<AccjsonAccountJson> darraccjsonGetAllAccounts(
            //                                              //Find all printshop's accounts.

            PsPrintShop psPrintshop_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //List to return.
            List<AccjsonAccountJson> darraccjson = new List<AccjsonAccountJson>();

            //                                              //Create the connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Find printshop's accounts.
            List<AccentityAccountEntityDB> darraccentity = context.Account.Where(account =>
                account.intPkPrintshop == psPrintshop_I.intPk).ToList();

            foreach (AccentityAccountEntityDB accentity in darraccentity)
            {
                //                                          //Find account type.
                AcctypentityAccountTypeEntityDB acctypentity = context.AccountType.FirstOrDefault(acctype =>
                    acctype.intPk == accentity.intPkAccountType);

                //                                          //Build json.
                AccjsonAccountJson accjson = new AccjsonAccountJson(accentity.intPk, 
                    (
                    //                                      //Cuentas SYS.
                    accentity.strNumber == strUndepositedFundsNumber ||
                    accentity.strNumber == strUncategorizedRevenueNumber ||
                    accentity.strNumber == strUncategorizedExpenseNumber ? "" : accentity.strNumber
                    ),
                    accentity.strName, accentity.intPkAccountType, acctypentity.strType, 
                    accentity.boolAvailable, accentity.boolGeneric);

                //                                          //Add to returning list.
                darraccjson.Add(accjson);
            }

            darraccjson = darraccjson.OrderBy(acc => acc.strNumber).ToList();

            intStatus_IO = 200;
            strUserMessage_IO = "";
            strDevMessage_IO = "";

            return darraccjson;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetAllAccountsExpenseAvailable(
            //                                              //Find all printshop's accounts expense available.

            PsPrintShop psPrintshop_I,
            out List<Accjson3AccountJson3> darraccexpjson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            darraccexpjson_O = new List<Accjson3AccountJson3>();

            //                                              //Create the connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Find printshop's accounts expense and available.
            List<AccentityAccountEntityDB> darraccentity = 
                (from accentity in context.Account
                join acctypentity in context.AccountType
                on accentity.intPkAccountType equals acctypentity.intPk
                where accentity.intPkPrintshop == psPrintshop_I.intPk &&
                accentity.boolAvailable == true &&
                acctypentity.strType == AccAccounting.strAccountTypeExpense
                select accentity).ToList();

            String strAccountNumber;
            foreach (AccentityAccountEntityDB accentity in darraccentity)
            {
                strAccountNumber = accentity.strNumber == AccAccounting.strUncategorizedExpenseNumber
                    ? "" : "(" + accentity.strNumber + ") ";
                //                                          //Build json.
                Accjson3AccountJson3 accjson = new Accjson3AccountJson3(accentity.intPk,
                     strAccountNumber + accentity.strName);
                //                                          //Add to returning list.
                darraccexpjson_O.Add(accjson);
            }

            darraccexpjson_O = darraccexpjson_O.OrderBy(accexp => accexp.strName).ToList();

            intStatus_IO = 200;
            strUserMessage_IO = "";
            strDevMessage_IO = "";
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetAllAccountsRevenueAvailable(
            //                                              //Find all printshop's accounts revenue available.

            PsPrintShop psPrintshop_I,
            out List<Accjson3AccountJson3> darraccrevjson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            darraccrevjson_O = new List<Accjson3AccountJson3>();

            //                                              //Create the connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Find printshop's accounts revenue and available.
            List<AccentityAccountEntityDB> darraccentity =
                (from accentity in context.Account
                 join acctypentity in context.AccountType
                 on accentity.intPkAccountType equals acctypentity.intPk
                 where accentity.intPkPrintshop == psPrintshop_I.intPk &&
                 accentity.boolAvailable == true &&
                 acctypentity.strType == AccAccounting.strAccountTypeRevenue
                 select accentity).ToList();

            String strAccountNumber;
            foreach (AccentityAccountEntityDB accentity in darraccentity)
            {
                strAccountNumber = accentity.strNumber == AccAccounting.strUncategorizedRevenueNumber
                    ? "" : "(" + accentity.strNumber + ") ";
                //                                          //Build json.
                Accjson3AccountJson3 accjson = new Accjson3AccountJson3(accentity.intPk,
                    strAccountNumber + accentity.strName);
                //                                          //Add to returning list.
                darraccrevjson_O.Add(accjson);
            }

            darraccrevjson_O = darraccrevjson_O.OrderBy(accexp => accexp.strName).ToList();

            intStatus_IO = 200;
            strUserMessage_IO = "";
            strDevMessage_IO = "";
        }

        //--------------------------------------------------------------------------------------------------------------
        public static List<AcctypjsonAccountTypeJson> darracctypjsonGetAccountTypes(
            //                                              //Find all account types.

            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //List to return.
            List<AcctypjsonAccountTypeJson> darracctypjson = new List<AcctypjsonAccountTypeJson>();

            //                                              //Create the connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Find account types.
            List<AcctypentityAccountTypeEntityDB> darracctypentity = context.AccountType.ToList();

            foreach (AcctypentityAccountTypeEntityDB acctypentity in darracctypentity)
            {
                //                                          //Build json.
                AcctypjsonAccountTypeJson acctypejson = new AcctypjsonAccountTypeJson(acctypentity.intPk,
                    acctypentity.strType);
                //                                          //Add to returning list.
                darracctypjson.Add(acctypejson);
            }

            darracctypjson = darracctypjson.OrderBy(acctyp => acctyp.strName).ToList();

            intStatus_IO = 200;
            strUserMessage_IO = "";
            strDevMessage_IO = "";

            return darracctypjson;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static List<PsorderjsonPrintshopOrderJson> darrpsorderjsonGetPrintshopOrders(
            //                                              //Get printshop's completed orders.

            PsPrintShop ps_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Create the connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //List of completed orders to send back.
            List<PsorderjsonPrintshopOrderJson> darrpsorderjson = new List<PsorderjsonPrintshopOrderJson>();

            //                                              //Get list of ids for completed jobs.
            List<int> darrintJobsIdsCompleted = (from jobentity in context.Job
                                                 where jobentity.intPkPrintshop == ps_I.intPk &&
                                                 jobentity.intStage == JobJob.intCompletedStage
                                                 select jobentity.intJobID).ToList();

            //                                              //Get data from Wisnet.
            String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                GetSection("Odyssey2Settings")["urlWisnetApi"];

            JobidsjsonJobIdsJson jobidsjson = new JobidsjsonJobIdsJson(darrintJobsIdsCompleted.ToArray());

            Task<List<PsorderjsonPrintshopOrderJson>> Task_darrcatjsonFromWisnet =
                HttpTools<PsorderjsonPrintshopOrderJson>.GetListAsyncPrintshopOrders(strUrlWisnet +
                "/PrintShopData/PrintshopOrdersAndJobs", jobidsjson);
            Task_darrcatjsonFromWisnet.Wait();

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Wisnet connection lost or there are not completed orders.";
            if (
//                                          //There is access to the service of Wisnet.
                Task_darrcatjsonFromWisnet.Result != null
                )
            {
                darrpsorderjson = Task_darrcatjsonFromWisnet.Result;

                //                                         //Verify if there is already an invoice for the order.
                for (int intI = 0; intI < darrpsorderjson.Count; intI++)
                {

                    InvoInvoiceEntityDB inventity = context.Invoice.FirstOrDefault(inv =>
                        inv.intOrderNumber == darrpsorderjson[intI].intOrderId);
                    if (
                        inventity != null
                        )
                    {
                        darrpsorderjson[intI].intnPkInvoice = inventity.intPk;
                    }

                    //                                      //Get order number.
                    JobjsonentityJobJsonEntityDB jobjsonentity = context.JobJson.FirstOrDefault(job =>
                        job.strPrintshopId == ps_I.strPrintshopId &&
                        job.intOrderId == darrpsorderjson[intI].intOrderId);
                    darrpsorderjson[intI].intnOrderNumber = darrpsorderjson[intI].intOrderId == 0 ? (int?)null :
                        (int)jobjsonentity.intnOrderNumber;

                    //                                      //To get strJobNumber for each job.
                    List<JobinfojsonJobInformationJson> darrjobinfojson = 
                    AccAccounting.subAddstrJobNumberToEachJob(darrpsorderjson[intI].intOrderId,
                        darrpsorderjson[intI].darrjobsinfo);

                    darrpsorderjson[intI].darrjobsinfo = darrjobinfojson;

                }

                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "";
            }

            return darrpsorderjson;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static List<JobinfojsonJobInformationJson> subAddstrJobNumberToEachJob(
            //                                              //Add strJobNumber to each job for an order.

            int intOrderId_I,
            List<JobinfojsonJobInformationJson> darrjobinfojson_I
            )
        {
            //                                              //Establish the connection.
            Odyssey2Context context = new Odyssey2Context();

            List<JobinfojsonJobInformationJson> darrjobinfojson = new List<JobinfojsonJobInformationJson>();

            foreach(JobinfojsonJobInformationJson jobinfojson in darrjobinfojson_I)
            {
                //                                          //Get strJobNumber.
                JobjsonentityJobJsonEntityDB jobjsonentity = context.JobJson.FirstOrDefault(job =>
                    job.intJobID == jobinfojson.intJobId);

                //String strJobNumber = JobJob.strGetJobNumber(intOrderId_I, jobinfojson.intJobId);
                jobinfojson.strJobNumber = jobjsonentity.intnJobNumber == null ? "" :
                    jobjsonentity.intnJobNumber.ToString();
                darrjobinfojson.Add(jobinfojson);
            }

            return darrjobinfojson;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool boolIsExpenseValid(
            int intPkAccount_I,
            Odyssey2Context context_M
            )
        {
            bool boolIsExpenseValid = false;
            //                                              //Account is not null.
            AccentityAccountEntityDB accentity = context_M.Account.FirstOrDefault(accentity =>
                accentity.intPk == intPkAccount_I);

            if (
                accentity != null
                )
            {
                AcctypentityAccountTypeEntityDB acctypentity = context_M.AccountType.FirstOrDefault(acctypentity =>
                    acctypentity.intPk == accentity.intPkAccountType);

                if (
                    acctypentity.strType == AccAccounting.strAccountTypeExpense &&
                    accentity.boolAvailable
                    )
                {
                    boolIsExpenseValid = true;
                }
            }

            return boolIsExpenseValid;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String strGetInvoice(
            //                                              //Find invoice's Json on DB. 

            int intPkInvoice_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            String strInvoiceJson = "";

            //                                              //Establish the connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Find invoice.
            InvoInvoiceEntityDB invoentity = context.Invoice.FirstOrDefault(invo =>
                invo.intPk == intPkInvoice_I);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Invoice not found.";
            if (
                invoentity != null
                )
            {
                strInvoiceJson = invoentity.strInvoiceJson;

                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "";
            }

            return strInvoiceJson;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetJobMovements(
            //                                              //Find all job's movements.

            int intJobId_I,
            String strPrintshopId_I,
            IConfiguration configuration_I,
            out JobmovsjsonJobMovementsJson jobmovsjson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            jobmovsjson_O = null;

            //                                              //Create the connection.
            Odyssey2Context context = new Odyssey2Context();

            intStatus_IO = 401;
            JobjsonJobJson jobjsonJob;
            if (
                JobJob.boolIsValidJobId(intJobId_I, strPrintshopId_I, configuration_I, out jobjsonJob,
                ref strUserMessage_IO, ref strDevMessage_IO)
                )
            {
                int intPkAccTypeExpense = context.AccountType.FirstOrDefault(acctyp =>
                    acctyp.strType == AccAccounting.strAccountTypeExpense).intPk;

                int intPkAccTypeRevenue = context.AccountType.FirstOrDefault(acctyp =>
                    acctyp.strType == AccAccounting.strAccountTypeRevenue).intPk;

                //                                          //Find job's expense and revenue movements.
                List<AccmovjsoninAccountMovementJsonInternal> darraccmoventityExpenseAndRevenueMovements =
                    (from accmoventity in context.AccountMovement
                     join accentity in context.Account on accmoventity.intPkAccount equals accentity.intPk
                     join acctypentity in context.AccountType on accentity.intPkAccountType equals acctypentity.intPk
                     //                                     //Get only movements of account type expense and revenue.
                     where (acctypentity.intPk == intPkAccTypeExpense || acctypentity.intPk == intPkAccTypeRevenue) &&
                     //                                     //Movements only this Job passed.
                     accmoventity.intnJobId == intJobId_I
                     select new AccmovjsoninAccountMovementJsonInternal 
                     (accmoventity.strStartDate, accmoventity.strStartTime, accentity.strNumber, accentity.strName,
                     accmoventity.numnIncrease, accmoventity.numnDecrease, acctypentity.strType, 
                     accmoventity.strConcept, accmoventity.intnPkInvoice
                     )).ToList();

                darraccmoventityExpenseAndRevenueMovements.Sort();

                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);

                //                                          //List that will save all job's movements.
                List<AccmovjsonAccountMovementJson> darraccmovExpenseMovements =
                    new List<AccmovjsonAccountMovementJson>();

                double numBalance = 0.0;
                double? numnAmountRevenue = null;
                double? numnAmountExpense = null;

                foreach (AccmovjsoninAccountMovementJsonInternal accmovjsonin in 
                    darraccmoventityExpenseAndRevenueMovements)
                {
                    //                                      //Convert base date to printshop date
                    ZonedTime ztimeStart = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                        accmovjsonin.strStartDate.ParseToDate(), accmovjsonin.strStartTime.ParseToTime(),
                        ps.strTimeZone);

                    String strMovementDate = ztimeStart.Date + " " + ztimeStart.Time;

                    String strNumber = "";
                    if (
                        //                                  //Account revenue.
                        accmovjsonin.strTransacctionType == AccAccounting.strAccountTypeRevenue
                        )
                    {
                        InvoInvoiceEntityDB invoetity = context.Invoice.FirstOrDefault(
                            invo => invo.intPk == (int)accmovjsonin.intnPkInvoice);
                        if (
                            //                              //Exist the invoice.
                            invoetity != null
                            )
                        {
                            strNumber = "" + invoetity.intOrderNumber;
                        }

                        if (
                            //                              //Movement Revenue increase.
                            accmovjsonin.numnIncrease != null
                            )
                        {
                            numBalance = numBalance + (double)accmovjsonin.numnIncrease;
                        }
                        else
                        {
                            //                              //Movement Revenue decrease.
                            numBalance = numBalance - (double)accmovjsonin.numnIncrease;
                        }

                        numnAmountRevenue = accmovjsonin.numnIncrease != null ?
                            ((double)accmovjsonin.numnIncrease).Round(2) : ((double)accmovjsonin.numnDecrease).Round(2);
                    }
                    else
                    {
                        //                                  //It is an account expense.
                        if (
                            //                              //Movement expense increase.
                            accmovjsonin.numnIncrease != null
                            )
                        {
                            numBalance = numBalance - (double)accmovjsonin.numnIncrease;
                        }
                        else
                        {
                            //                              //Movement expense decrease.
                            numBalance = numBalance + (double)accmovjsonin.numnIncrease;
                        }

                        numnAmountExpense = accmovjsonin.numnDecrease != null ?
                            ((double)accmovjsonin.numnDecrease).Round(2) : ((double)accmovjsonin.numnIncrease).Round(2);
                    }

                    //                                      //Build movement json.
                    AccmovjsonAccountMovementJson accmovsjson = new AccmovjsonAccountMovementJson(strMovementDate,
                        accmovjsonin.strAccountNumber, accmovjsonin.strAccountName, strNumber,
                        numnAmountRevenue, numnAmountExpense, accmovjsonin.strTransacctionType, accmovjsonin.strMemo,
                        numBalance);

                    darraccmovExpenseMovements.Add(accmovsjson);

                    //                                      //Reset the values
                    numnAmountRevenue = null;
                    numnAmountExpense = null;
                }

                jobmovsjson_O = new JobmovsjsonJobMovementsJson(darraccmovExpenseMovements.ToArray(),
                    numBalance.Round(2));

                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "";
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetAccountsWithMovementsInAPeriod(
            //                                              //Find all accounts that had movements in a specific 
            //                                              //      period of time.

            String strStartDate_I,
            String strStartTime_I,
            String strEndDate_I,
            String strEndTime_I,
            PsPrintShop ps_I,
            out List<Accjson2AccountJson2> darraccjson2_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Json to return.
            darraccjson2_O = new List<Accjson2AccountJson2>();

            intStatus_IO = 401;
            ZonedTime ztimeStart;
            ZonedTime ztimeEnd;

            bool boolIsValidStartDateTimeAndEndDateTimeAccount =
                ZonedTimeTools.boolIsValidStartDateTimeAndEndDateTimeAccount(strStartDate_I, strStartTime_I,
                strEndDate_I, strEndTime_I, out ztimeStart, out ztimeEnd, ref strUserMessage_IO, ref strDevMessage_IO);

            if (
                boolIsValidStartDateTimeAndEndDateTimeAccount
                )
            {
                //                                          //Create the connection.
                Odyssey2Context context = new Odyssey2Context();

                //                                          //Find printshop's movements.
                List<AccmoventityAccountMovementEntityDB> darraccmoveentity =
                    (from accmovemententity in context.AccountMovement
                     join accentity in context.Account
                     on accmovemententity.intPkAccount equals accentity.intPk
                     where accentity.intPkPrintshop == ps_I.intPk
                     select accmovemententity).ToList();

                //                                          //List that will contain all accounts found inside the
                //                                          //      period.
                List<Accjson2AccountJson2> darraccjson2 = new List<Accjson2AccountJson2>();

                foreach (AccmoventityAccountMovementEntityDB accmoventity in darraccmoveentity)
                {
                    //                                      //Create movement ztime.
                    ZonedTime ztimeMovementStart = ZonedTimeTools.NewZonedTime(
                        accmoventity.strStartDate.ParseToDate(), accmoventity.strStartTime.ParseToTime());

                    if (
                        //                                  //Movement's date must be inside period's range.
                        ztimeMovementStart >= ztimeStart &&
                        ztimeMovementStart <= ztimeEnd
                        )
                    {
                        //                                  //To get number and name of the movement's account.
                        //                                  //Find movement's account.
                        AccentityAccountEntityDB accentityForMove = context.Account.FirstOrDefault(acc =>
                            acc.intPk == accmoventity.intPkAccount);

                        //                                  //Get movement's account type.
                        AcctypentityAccountTypeEntityDB acctypentityToGetType =
                            context.AccountType.FirstOrDefault(acctyp =>
                            acctyp.intPk == accentityForMove.intPkAccountType);

                        Accjson2AccountJson2 accjson2;
                        //                                  //LILIANA: ARREGLO TEMPORAL MIENTRAS SE HACE LA HISTORIA
                        //                                  //      DETAILS. PUES TRUENA CUANDO INCREASE ES NULO.
                        if (
                            accmoventity.numnIncrease != null
                            )
                        {
                            //                                  //Create object.
                            accjson2 = new Accjson2AccountJson2(accmoventity.intPkAccount,
                                accentityForMove.strNumber, accentityForMove.strName,
                                ((double)accmoventity.numnIncrease).Round(2), acctypentityToGetType.strType);
                        }
                        else
                        {
                            //                                  //Create object.
                            accjson2 = new Accjson2AccountJson2(accmoventity.intPkAccount,
                                accentityForMove.strNumber, accentityForMove.strName,
                                ((double)accmoventity.numnDecrease).Round(2), acctypentityToGetType.strType);
                        }
                        //                                  //Verify if account already exists.
                        Accjson2AccountJson2 accjsonToCheck = darraccjson2_O.FirstOrDefault(accjson2 =>
                            accjson2.strNumber == accentityForMove.strNumber &&
                            accjson2.strName == accentityForMove.strName);

                        if (
                            //                              //Only add if account not exists.
                            accjsonToCheck != null
                            )
                        {
                            //                                  //LILIANA: ARREGLO TEMPORAL MIENTRAS SE HACE LA HISTORIA
                            //                                  //      DETAILS. PUES TRUENA CUANDO INCREASE ES NULO.
                            if (
                                accmoventity.numnIncrease != null
                                )
                            {
                                //                              //Just add amout
                                accjsonToCheck.numAmount = (accjsonToCheck.numAmount +
                                    (double)accmoventity.numnIncrease).Round(2);
                            }
                            else
                            {
                                //                              //Just add amout
                                accjsonToCheck.numAmount = (accjsonToCheck.numAmount +
                                    (double)accmoventity.numnDecrease).Round(2);
                            }
                        }
                        else
                        {
                            darraccjson2_O.Add(accjson2);
                        }
                    }
                }

                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "";
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static List<InvojsonInvoiceJson> darrinvojsonGetInvoices(
            //                                              //Get all printshop's invoices.

            int intContactId_I,
            PsPrintShop ps_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //List to return.
            List<InvojsonInvoiceJson> darrinvojson = new List<InvojsonInvoiceJson>();

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Customer not valid.";
            if (
                CusCustomer.boolCustomerIsFromPrintshop(ps_I.strPrintshopId, intContactId_I)
                )
            {
                //                                          //Establish connection.
                Odyssey2Context context = new Odyssey2Context();

                //                                          //Find printshop's invoices.
                List<InvoInvoiceEntityDB> darrinvoentity = context.Invoice.Where(invo =>
                    invo.intPkPrintshop == ps_I.intPk && invo.intContactId == intContactId_I).ToList();

                foreach (InvoInvoiceEntityDB invoentity in darrinvoentity)
                {
                    //                                      //Create json object.
                    InvojsonInvoiceJson invojson = new InvojsonInvoiceJson(invoentity.intPk, invoentity.intOrderNumber);
                    darrinvojson.Add(invojson);
                }

                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "";
            }

            return darrinvojson;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static AccdetjsonAccountDetailJson AccdetjsonGetAccountMovements(
            //                                              //Find movements for an specific account in a specific 
            //                                              //      period of time.

            String strStartDate_I,
            String strStartTime_I,
            String strEndDate_I,
            String strEndTime_I,
            PsPrintShop ps_I,
            int intPkAccount_I,
            IConfiguration configuration_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            AccdetjsonAccountDetailJson accdetjson = null;

            intStatus_IO = 401;
            ZonedTime ztimeStart;
            ZonedTime ztimeEnd;
            if (
                ZonedTimeTools.boolIsValidStartDateTimeAndEndDateTimeAccount(strStartDate_I, strStartTime_I,
                strEndDate_I, strEndTime_I, out ztimeStart, out ztimeEnd, ref strUserMessage_IO, ref strDevMessage_IO)
                )
            {
                //                                          //Create the connection.
                Odyssey2Context context = new Odyssey2Context();

                //                                          //Find account
                AccentityAccountEntityDB accentity = context.Account.FirstOrDefault(acc =>
                    acc.intPk == intPkAccount_I);

                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Account not found or account not valid.";
                if (
                    accentity != null
                    )
                {
                    //                                      //Find printshop's movements.
                    List<AccmoventityAccountMovementEntityDB> darraccmoveentity =
                        context.AccountMovement.Where(accmove => accmove.intPkAccount == accentity.intPk).ToList();

                    //                                      //List tha contains all movements between request date.
                    List<AccmoventityAccountMovementEntityDB> darraccmoveentityFiltered =
                        new List<AccmoventityAccountMovementEntityDB>();

                    foreach (AccmoventityAccountMovementEntityDB accmoventity in darraccmoveentity)
                    {
                        //                                  //Create movement ztime.
                        ZonedTime ztimeMovementStart = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                            accmoventity.strStartDate.ParseToDate(), accmoventity.strStartTime.ParseToTime(),
                            ps_I.strTimeZone);

                        if (
                            //                              //Movement's date must be inside period's range.
                            ztimeMovementStart >= ztimeStart &&
                            ztimeMovementStart <= ztimeEnd
                            )
                        {
                            darraccmoveentityFiltered.Add(accmoventity);
                            /*
                            //                              //Set date and time for the movement.
                            String strDate = accmoventity.strStartDate + " " + accmoventity.strStartTime;

                            //                              //LILIANA, TEMPORAL MIENTRAS SE HACE LA HISTORIA 
                            //                              //      DE DETAILS.
                            if (
                                accmoventity.numnIncrease != null
                                )
                            {
                                //                              //Create object.
                                AccmovjsonAccountMomeventJson accjson2 = new AccmovjsonAccountMomeventJson(strDate,
                                    accmoventity.strConcept, ((double)accmoventity.numnIncrease).Round(2),
                                    accmoventity.intnJobId, accmoventity.intnPkInvoice);
                                darraccjson_O.Add(accjson2);
                            }
                            else
                            {
                                //                              //Create object.
                                AccmovjsonAccountMomeventJson accjson2 = new AccmovjsonAccountMomeventJson(strDate,
                                    accmoventity.strConcept, ((double)accmoventity.numnDecrease).Round(2),
                                    accmoventity.intnJobId, accmoventity.intnPkInvoice);
                                darraccjson_O.Add(accjson2);
                            }*/
                        }
                    }
                    //darraccmoveentityFiltered.Sort();
                    darraccmoveentityFiltered.OrderBy(accmov => accmov.intPk);

                    //                                      //List that will contain all accounts found inside the
                    //                                      //      period.
                    List<AccmovjsonAccountMomeventJson2> darraccmovjson = new List<AccmovjsonAccountMomeventJson2>();

                    //                                      //To know if is an asset account type.
                    AcctypentityAccountTypeEntityDB acctypentity = context.AccountType.FirstOrDefault(acc =>
                            acc.intPk == accentity.intPkAccountType);
                    bool boolIsAsset = acctypentity.strType == AccAccounting.strAccountTypeAsset ? true : false;

                    //                                      //Go throug each element in the list and fill the 
                    //                                      //      needed information.
                    double numBalance = 0.0;
                    foreach (AccmoventityAccountMovementEntityDB accmoventity in darraccmoveentityFiltered)
                    {
                        //                                  //Create movement ztime.
                        ZonedTime ztimeMovementStart = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                            accmoventity.strStartDate.ParseToDate(), accmoventity.strStartTime.ParseToTime(),
                            ps_I.strTimeZone);
                        //                                  //Get movement´s date.
                        String strDate = ztimeMovementStart.Date + " " + ztimeMovementStart.Time;

                        //                                  //Get movement´s transaction type.                        
                        bool boolIsIncrease = accmoventity.numnIncrease != null ? true : false;
                        String strTransacctionType = AccAccounting.strGetTransactionType(acctypentity.strType,
                            boolIsIncrease);

                        //                                  //Get movement´s number
                        String strAccountMovementNumber = AccAccounting.strGetAccountMovementNumber(accmoventity);

                        //                                  //Get movement's customer's name.
                        String strCustomerName = AccAccounting.strGetAccountMovementCustomerName(accmoventity, ps_I, configuration_I);


                        //                                  //Get movement's memo.
                        String strMemo = accmoventity.strConcept;

                        //                                  //Get movement´s increase or decrease.
                        double? numnChargeOrIncrease = null;
                        double? numnPaymentOrDecrease = null;
                        if (
                            accmoventity.numnIncrease != null
                            )
                        {
                            numnChargeOrIncrease = accmoventity.numnIncrease;
                            numBalance = (double)(numBalance + accmoventity.numnIncrease);
                        }
                        else
                        {
                            numnPaymentOrDecrease = accmoventity.numnDecrease;
                            numBalance = (double)(numBalance - accmoventity.numnDecrease);
                        }

                        //                                  //Create element to add list to send back.
                        AccmovjsonAccountMomeventJson2 accmovjson = new AccmovjsonAccountMomeventJson2(strDate,
                            strTransacctionType, strAccountMovementNumber, strCustomerName, strMemo, 
                            numnChargeOrIncrease, numnPaymentOrDecrease, numBalance);

                        darraccmovjson.Add(accmovjson);
                    }

                    accdetjson = new AccdetjsonAccountDetailJson(boolIsAsset, darraccmovjson.ToArray());

                    intStatus_IO = 200;
                    strUserMessage_IO = "";
                    strDevMessage_IO = "";
                }
            }

            return accdetjson;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static String strGetTransactionType(
            //                                              //Get the transaction type for an account movement.

            String strAccountType_I,
            bool boolIsIncrease_I
            )
        {
            String strTransacctionType = "";
            /*CASE*/
            if (
                strAccountType_I == "Asset"
                )
            {
                if (
                    boolIsIncrease_I
                    )
                {
                    strTransacctionType = "Invoice";
                }
                else
                {
                    strTransacctionType = "Payment";
                }
            }else if (
                strAccountType_I == "Liability"
                )
            {
                if (
                    boolIsIncrease_I
                    )
                {
                    strTransacctionType = "Invoice";
                }
                else
                {
                    strTransacctionType = "Credit Memo";
                }
            }
            else if (
                strAccountType_I == "Revenue"
                )
            {
                strTransacctionType = "Invoice";
            }
            else if (
                strAccountType_I == "Expense"
                )
            {
                strTransacctionType = "Job Expense";
            }else if (
                strAccountType_I == "Bank"
                )
            {
                strTransacctionType = "";
            }
            /*END-CASE*/

            return strTransacctionType;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static String strGetAccountMovementNumber(
            //                                              //Get the number for an account movement.

            AccmoventityAccountMovementEntityDB accmoventity_I
            )
        {
            //                                              //Create the connection.
            Odyssey2Context context = new Odyssey2Context();

            String strAccountMovementNumber = "";
            /*CASE*/
            if (
                accmoventity_I.intnPkInvoice != null
                )
            {
                InvoInvoiceEntityDB inventity = context.Invoice.FirstOrDefault(inv =>
                    inv.intPk == accmoventity_I.intnPkInvoice);
                strAccountMovementNumber = (inventity.intOrderNumber).ToString();
            }
            else if (
               accmoventity_I.intnPkCreditMemo != null
               )
            {
                CrmentityCreditMemoEntityDB crmenentity = context.CreditMemo.FirstOrDefault(crm =>
                    crm.intPk == accmoventity_I.intnPkCreditMemo);
                strAccountMovementNumber = crmenentity.strCreditMemoNumber;
            }
            else if (
                accmoventity_I.intnPkBankDeposit != null
                )
            {
                strAccountMovementNumber = "";
            }
            else if (
                accmoventity_I.intnPkPayment != null
                )
            {
                PaymtPaymentEntityDB paymtentity = context.Payment.FirstOrDefault(pay =>
                    pay.intPk == accmoventity_I.intnPkPayment);
                strAccountMovementNumber = paymtentity.strReference;
            }
            else
            {
                strAccountMovementNumber = (accmoventity_I.intnJobId).ToString();
            }
            /*END-CASE*/

            return strAccountMovementNumber;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static String strGetAccountMovementCustomerName(
            //                                              //Get the number for an account movement.

            AccmoventityAccountMovementEntityDB accmoventity_I,
            PsPrintShop ps_I,
            IConfiguration configuration_I
            )
        {
            //                                              //Create the connection.
            Odyssey2Context context = new Odyssey2Context();

            int intStatus = 0;
            String strUserMessage = "";
            String strDevMessage = "";
            String strAccountMovementCustomerName = "";
            CusjsonCustomerJson cusjson;
            /*CASE*/
            if (
                accmoventity_I.intnPkInvoice != null
                )
            {
                InvoInvoiceEntityDB inventity = context.Invoice.FirstOrDefault(inv =>
                    inv.intPk == accmoventity_I.intnPkInvoice);

                CusCustomer.subGetOneCustomerFromPrintshop(ps_I, inventity.intContactId, out cusjson, ref intStatus,
                        ref strUserMessage, ref strDevMessage);
                if (
                    intStatus == 200
                    )
                {
                    strAccountMovementCustomerName = cusjson.strFirstName + " " + cusjson.strLastName;
                }
            }
            else if (
               accmoventity_I.intnPkCreditMemo != null
               )
            {
                CrmentityCreditMemoEntityDB crmenentity = context.CreditMemo.FirstOrDefault(crm =>
                    crm.intPk == accmoventity_I.intnPkCreditMemo);

                CusCustomer.subGetOneCustomerFromPrintshop(ps_I, crmenentity.intContactId, out cusjson, ref intStatus,
                        ref strUserMessage, ref strDevMessage);
                if (
                    intStatus == 200
                    )
                {
                    strAccountMovementCustomerName = cusjson.strFirstName + " " + cusjson.strLastName;
                }
            }
            else if (
                accmoventity_I.intnPkBankDeposit != null
                )
            {
                strAccountMovementCustomerName = "";
            }
            else if (
                accmoventity_I.intnPkPayment != null
                )
            {
                PaymtPaymentEntityDB paymtentity = context.Payment.FirstOrDefault(pay =>
                    pay.intPk == accmoventity_I.intnPkPayment);

                CusCustomer.subGetOneCustomerFromPrintshop(ps_I, paymtentity.intContactId, out cusjson, ref intStatus,
                        ref strUserMessage, ref strDevMessage);
                if (
                    intStatus == 200
                    )
                {
                    strAccountMovementCustomerName = cusjson.strFirstName + " " + cusjson.strLastName;
                }
            }
            else
            {
                JobjsonentityJobJsonEntityDB jobjsonentity = context.JobJson.FirstOrDefault(job =>
                    job.intJobID == accmoventity_I.intnJobId);

                if (
                    //                                      //Can be null if was just load to the jsontable and was
                    //                                      //      already added to the job table.
                    jobjsonentity.jobjson == null
                    )
                {
                    JobjsonJobJson jobjson1 = null;
                    if (
                        JobJob.boolIsValidJobId(jobjsonentity.intJobID, ps_I.strPrintshopId, configuration_I, out jobjson1,
                            ref strUserMessage, ref strDevMessage)
                        )
                    {
                        JobjsonJobJson jobjson = JsonSerializer.Deserialize<JobjsonJobJson>(jobjsonentity.jobjson);
                        strAccountMovementCustomerName = jobjson.strCustomerName;
                    }
                }
            }
            /*END-CASE*/

            return strAccountMovementCustomerName;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetPaymentMethods(
            //                                              //Get all the payment methods

            out PymtmtjsonPaymentMethodJson[] arrpymtmtjson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            arrpymtmtjson_O = null;

            Odyssey2Context context = new Odyssey2Context();

            List<PymtmtjsonPaymentMethodJson> darrpymtmtjson = new List<PymtmtjsonPaymentMethodJson>();

            //                                              //Find payment methods
            List<PymtmtentityPaymentMethodEntityDB> darrpaymtentity = 
                (from pymtmt in context.PaymentMethod select pymtmt).ToList();

            foreach (PymtmtentityPaymentMethodEntityDB pymtmtentity in darrpaymtentity)
            {
                //                                          //Create object.
                PymtmtjsonPaymentMethodJson pymtmtjson = new PymtmtjsonPaymentMethodJson(pymtmtentity.intPk, 
                    pymtmtentity.strName);
                darrpymtmtjson.Add(pymtmtjson);
            }

            arrpymtmtjson_O = darrpymtmtjson.ToArray();
            intStatus_IO = 200;
            strUserMessage_IO = "";
            strDevMessage_IO = "Success.";
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetBankAccounts(
            //                                              //Find all printshop's bank accounts available.

            //                                              //If boolUndepositedFunds_I is true, Undeposited Funds 
            //                                              //      account is added to list
            bool boolUndepositedFunds_I,
            PsPrintShop psPrintshop_I,
            out List<Accjson3AccountJson3> darraccjson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            darraccjson_O = new List<Accjson3AccountJson3>();

            //                                              //Create the connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Find printshop's bank accounts and available.
            List<AccentityAccountEntityDB> darraccentity =
                (from accentity in context.Account
                 join acctypentity in context.AccountType
                 on accentity.intPkAccountType equals acctypentity.intPk
                 where accentity.intPkPrintshop == psPrintshop_I.intPk &&
                 accentity.boolAvailable == true &&
                 acctypentity.strType == AccAccounting.strAccountTypeBank
                 select accentity).ToList();

            foreach (AccentityAccountEntityDB accentity in darraccentity)
            {
                //                                          //Build json.
                Accjson3AccountJson3 accjson = new Accjson3AccountJson3(accentity.intPk,
                    "(" + accentity.strNumber + ") " + accentity.strName);
                //                                          //Add to returning list.
                darraccjson_O.Add(accjson);
            }

            darraccjson_O = darraccjson_O.OrderBy(accexp => accexp.strName).ToList();

            if (
                //                                          //Add Undeposited Funds account
                boolUndepositedFunds_I
                )
            {
                //                                          //Find printshop's Undeposited Funds account.
                AccentityAccountEntityDB accentity = context.Account.FirstOrDefault(acc => 
                    acc.intPkPrintshop == psPrintshop_I.intPk && acc.boolAvailable == true &&
                    acc.strName == AccAccounting.strUndepositedFundsName && 
                    acc.strNumber == AccAccounting.strUndepositedFundsNumber);

                //                                          //Build json.
                Accjson3AccountJson3 accjson = new Accjson3AccountJson3(accentity.intPk, accentity.strName);

                //                                          //Add this account to the top of the list
                darraccjson_O.Insert(0, accjson);
            }

            intStatus_IO = 200;
            strUserMessage_IO = "";
            strDevMessage_IO = "";
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetBankDepositsInARange(
            //                                              //Get account's bank deposits in a range of time.

            String strStartDate_I,
            String strEndDate_I,
            int intPkBankAccount_I,
            PsPrintShop psPrintshop_I,
            out List<BankdepjsonBankDepositJson> darrbankdepjson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //List to return.
            darrbankdepjson_O = new List<BankdepjsonBankDepositJson>();

            intStatus_IO = 402;
            ZonedTime ztimeStart;
            ZonedTime ztimeEnd;
            if (
                ZonedTimeTools.boolIsValidStartDateTimeAndEndDateTimeAccount(strStartDate_I, "00:00:00",
                strEndDate_I, "00:00:00", out ztimeStart, out ztimeEnd, ref strUserMessage_IO, 
                ref strDevMessage_IO)
                )
            {
                //                                          //Establish connection.
                Odyssey2Context context = new Odyssey2Context();

                //                                          //Find printshop's bank account.
                //                                          //It is not necessary to verify if account's type is
                //                                          //      "bank", because only bank accounts have bank
                //                                          //      deposits.
                AccentityAccountEntityDB accentity = context.Account.FirstOrDefault(account => 
                    account.intPk == intPkBankAccount_I && account.intPkPrintshop == psPrintshop_I.intPk);

                intStatus_IO = 403;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Account not found or account not valid.";
                if (
                    accentity != null
                    )
                {
                    //                                      //Find all account's deposits.
                    List<BkdptentityBankDepositEntityDB> darrbkdpentity = context.BankDeposit.Where(depo =>
                        depo.intPkBankAccount == intPkBankAccount_I).ToList();

                    foreach (BkdptentityBankDepositEntityDB bkdptentity in darrbkdpentity)
                    {
                        //                                  //Create ztime to compare.
                        ZonedTime ztimeDate = ZonedTimeTools.NewZonedTime(
                            bkdptentity.strDate.ParseToDate(), "00:00:00".ParseToTime());

                        if (
                            ztimeDate >= ztimeStart &&
                            ztimeDate <= ztimeEnd
                            )
                        {
                            //                              //Create json.
                            BankdepjsonBankDepositJson bankdepjson = new BankdepjsonBankDepositJson(bkdptentity.intPk,
                                bkdptentity.strDate, bkdptentity.numAmount);
                            darrbankdepjson_O.Add(bankdepjson);
                        }
                    }

                    intStatus_IO = 200;
                    strUserMessage_IO = "";
                    strDevMessage_IO = "";
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetBankDepositSummary(
            //                                              //Find all payments that belong to a specific bank deposit.

            int intPkBankDeposit_I,
            PsPrintShop psPrintshop_I,
            out BankdepsummjsonBankDepositSummaryJson bankdepsummjson_O,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Json to return.
            bankdepsummjson_O = null;

            //                                              //Find bank deposit.
            BkdptentityBankDepositEntityDB bkdptentity = context_M.BankDeposit.FirstOrDefault(deposit =>
                deposit.intPk == intPkBankDeposit_I);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Bank deposit not found.";
            if (
                bkdptentity != null
                )
            {
                //                                          //Find bank account.
                AccentityAccountEntityDB accentity = context_M.Account.FirstOrDefault(account =>
                    account.intPk == bkdptentity.intPkBankAccount && 
                    account.intPkPrintshop == psPrintshop_I.intPk);

                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Bank account not found.";
                if (
                    accentity != null
                    )
                {
                    //                                      //Find deposit's payments.
                    List<PaymtPaymentEntityDB> darrpaymtentity = context_M.Payment.Where(payment =>
                        payment.intnPkBankDeposit == intPkBankDeposit_I).ToList();

                    //                                      //Find all prinshop's customers.
                    Cusjson2CustomerJson2[] arrcusjson2;
                    CusCustomer.subGetCustomers(psPrintshop_I, out arrcusjson2, ref intStatus_IO,
                        ref strUserMessage_IO, ref strDevMessage_IO);

                    //                                      //List of payments.
                    List<PayjsonPaymentJson> darrpayjson = new List<PayjsonPaymentJson>();

                    //                                      //Total amount of payments.
                    double numTotal = 0.0;

                    foreach (PaymtPaymentEntityDB paymtentity in darrpaymtentity)
                    {
                        //                                  //Find customer.
                        Cusjson2CustomerJson2 cusjson2 = arrcusjson2.FirstOrDefault(customer =>
                            customer.intContactId == paymtentity.intContactId);

                        String strCustomerFullName = "";
                        if (
                            cusjson2 != null
                            )
                        {
                            strCustomerFullName = cusjson2.strFullName;
                        }

                        //                                  //Find payment method.
                        PymtmtentityPaymentMethodEntityDB paymententity = context_M.PaymentMethod.FirstOrDefault(
                            payment => payment.intPk == paymtentity.intPkPaymentMethod);

                        String strPaymentMethod = "";
                        if (
                            paymententity != null
                            )
                        {
                            strPaymentMethod = paymententity.strName;
                        }

                        //                                  //Create payment json.
                        PayjsonPaymentJson payjson = new PayjsonPaymentJson(strCustomerFullName, strPaymentMethod,
                            paymtentity.strReference, paymtentity.numAmount.Round(2));
                        darrpayjson.Add(payjson);

                        //                                  //Increase sum of payments.
                        numTotal = numTotal + paymtentity.numAmount;
                    }

                    //                                      //Create json.
                    bankdepsummjson_O = new BankdepsummjsonBankDepositSummaryJson(accentity.strName, 
                        bkdptentity.strDate, Date.Now(ZonedTimeTools.timezone).ToString(), darrpayjson.ToArray(), 
                        numTotal);

                    intStatus_IO = 200;
                    strUserMessage_IO = "";
                    strDevMessage_IO = "";
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetCustomersBalances(
            //                                              //Get balances customers.

            PsPrintShop ps_I,
            String strBalanceStatus_I,
            Odyssey2Context context_M,
            out List<BalcusjsonBalanceCustomerJson> darrbalcusjson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                      
            darrbalcusjson_O = new List<BalcusjsonBalanceCustomerJson>();

            //                                          //Get payment not balanced from the printshop.
            List<PaymtPaymentEntityDB> darrpaymtentityPayment =
                context_M.Payment.Where(paymt => paymt.intPkPrintshop == ps_I.intPk &&
                paymt.boolBalanced == false).ToList();

            //                                          //Get Credit memo not balanced from the printshop.
            List<CrmentityCreditMemoEntityDB> darrcrmentityCreditMemo =
                context_M.CreditMemo.Where(crm => crm.intPkPrintshop == ps_I.intPk &&
                crm.boolBalanced == false).ToList();

            //                                          //Get invoice not balanced from the printshop.
            List<InvoInvoiceEntityDB> darrcusbalentityInvoiced =
                context_M.Invoice.Where(inv => inv.intPkPrintshop == ps_I.intPk &&
                inv.boolBalanced == false).ToList();

            //                                              //Init var.
            double numTotalPayment = 0.0;
            double numTotalCreditMemo = 0.0;
            double numTotalInvoice = 0.0;
            double numBalancePartialCustomer = 0.0;
            double numBalanceTotal = 0.0;
            String strNameCustomer = "";
            BalcusjsonBalanceCustomerJson balcusjson = null;

            //                                          //Find all prinshop's customers.
            Cusjson2CustomerJson2[] arrcusjson2;
            CusCustomer.subGetCustomers(ps_I, out arrcusjson2, ref intStatus_IO,
                ref strUserMessage_IO, ref strDevMessage_IO);
            /*CASE*/
            if (
                //                                          //Get balance status open.
                strBalanceStatus_I == strBalanceStatusOpen
                )
            {
                //                                          //Get balance customers from the printhop.
                List<CustbalentityCustomerBalanceEntityDB> darrcusbalentityCustomerBalanced =
                    context_M.CustomerBalance.Where(cusbal => cusbal.intPkPrintshop == ps_I.intPk).ToList();

                //                                          //Take each customer balanced.
                foreach (CustbalentityCustomerBalanceEntityDB custbalentity in darrcusbalentityCustomerBalanced)
                {
                    bool boolNotUsed = false;

                    //                                      //Get total payment from current customer.
                    numTotalPayment = AccAccounting.numTotalPaymentNotBalancedFromCustomer(
                        custbalentity.intContactId, ref boolNotUsed, ref darrpaymtentityPayment, context_M);

                    //                                      //Get total credit memo from current customer.
                    numTotalCreditMemo = AccAccounting.numTotalCreditMemoNotBalancedFromCustomer(
                        custbalentity.intContactId, ref boolNotUsed, ref darrcrmentityCreditMemo, context_M);

                    //                                      //Get total invoice from current customer.
                    numTotalInvoice = AccAccounting.numTotalInvoiceNotBalancedFromCustomer(
                        custbalentity.intContactId, ref boolNotUsed, ref darrcusbalentityInvoiced, context_M);

                    //                                      //Calculate the balance partial.
                    numBalancePartialCustomer = numTotalPayment + numTotalCreditMemo - numTotalInvoice;

                    //                                      //Calcualte the balance total of the customer.
                    custbalentity.numBalance = custbalentity.numBalance + numBalancePartialCustomer;

                    //                                      //Update the entityCustomer.
                    context_M.CustomerBalance.Update(custbalentity);
                    context_M.SaveChanges();

                    strNameCustomer = arrcusjson2.FirstOrDefault(cus => cus.intContactId == custbalentity.intContactId)
                        .strFullName;

                    balcusjson = new BalcusjsonBalanceCustomerJson(custbalentity.intContactId,
                        strNameCustomer, custbalentity.numBalance.Round(2));

                    //                                      //Add Customer balance to the list.
                    darrbalcusjson_O.Add(balcusjson);
                }

                //                                          //Work with customer notBalanced.

                //                                          //Get customer not balanced.
                List<int> darrintContactIdNotBalanced = darrintCustomerNotBalanced(darrpaymtentityPayment,
                    darrcrmentityCreditMemo, darrcusbalentityInvoiced);

                //                                          //Take each customer not balanced.
                foreach (int intContactIdNotBalanced in darrintContactIdNotBalanced)
                {
                    bool boolNotUsed = false;

                    //                                      //Get total payment from current customer.
                    numTotalPayment = AccAccounting.numTotalPaymentNotBalancedFromCustomer(
                        intContactIdNotBalanced, ref boolNotUsed, ref darrpaymtentityPayment, context_M);

                    //                                      //Get total credit memo from current customer.
                    numTotalCreditMemo = AccAccounting.numTotalCreditMemoNotBalancedFromCustomer(
                        intContactIdNotBalanced, ref boolNotUsed, ref darrcrmentityCreditMemo, context_M);

                    //                                      //Get total invoice from current customer.
                    numTotalInvoice = AccAccounting.numTotalInvoiceNotBalancedFromCustomer(
                        intContactIdNotBalanced, ref boolNotUsed, ref darrcusbalentityInvoiced, context_M);

                    //                                      //Calculate the balance total.
                    numBalanceTotal = numTotalPayment + numTotalCreditMemo - numTotalInvoice;

                    //                                      //Create the Balance to the customer.
                    CustbalentityCustomerBalanceEntityDB custbalentity = new CustbalentityCustomerBalanceEntityDB
                    {
                        intContactId = intContactIdNotBalanced,
                        numBalance = numBalanceTotal,
                        strDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                        intPkPrintshop = ps_I.intPk
                    };

                    context_M.CustomerBalance.Add(custbalentity);
                    context_M.SaveChanges();

                    strNameCustomer = arrcusjson2.FirstOrDefault(cus => cus.intContactId == intContactIdNotBalanced)
                        .strFullName;

                    balcusjson = new BalcusjsonBalanceCustomerJson(intContactIdNotBalanced,
                        strNameCustomer, numBalanceTotal.Round(2));

                    //                                      //Add Customer balance to the list.
                    darrbalcusjson_O.Add(balcusjson);
                }

                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "";
            }
            else if (
                //                                          //Get balance status open.
                strBalanceStatus_I == strBalanceStatusAll
                )
            {
                //                                          //All balances.

                foreach (Cusjson2CustomerJson2 cusjson2AllCustomer in arrcusjson2.ToList())
                {
                    bool boolCustomerHasPaymentOrCreditOrInvoice = false;

                    //                                      //Get CustBalance.
                    CustbalentityCustomerBalanceEntityDB custbalentity = context_M.CustomerBalance.FirstOrDefault(
                        cusbal => cusbal.intContactId == cusjson2AllCustomer.intContactId);

                    //                                      //Get total payment from current customer.
                    numTotalPayment = AccAccounting.numTotalPaymentNotBalancedFromCustomer(
                        cusjson2AllCustomer.intContactId, ref boolCustomerHasPaymentOrCreditOrInvoice, 
                        ref darrpaymtentityPayment, context_M);

                    //                                      //Get total credit memo from current customer.
                    numTotalCreditMemo = AccAccounting.numTotalCreditMemoNotBalancedFromCustomer(
                        cusjson2AllCustomer.intContactId, ref boolCustomerHasPaymentOrCreditOrInvoice, 
                        ref darrcrmentityCreditMemo, context_M);

                    //                                      //Get total invoice from current customer.
                    numTotalInvoice = AccAccounting.numTotalInvoiceNotBalancedFromCustomer(
                        cusjson2AllCustomer.intContactId, ref boolCustomerHasPaymentOrCreditOrInvoice, 
                        ref darrcusbalentityInvoiced, context_M);

                    //                                      //Calculate the balance partial.
                    numBalancePartialCustomer = numTotalPayment + numTotalCreditMemo - numTotalInvoice;

                    //                                      //Get Customer name.
                    strNameCustomer = arrcusjson2.FirstOrDefault(cus => 
                        cus.intContactId == cusjson2AllCustomer.intContactId).strFullName;

                    if (
                        //                                  //The customer has a balance partial.
                        custbalentity != null
                        )
                    {
                        custbalentity.numBalance = custbalentity.numBalance + numBalancePartialCustomer;
                        context_M.CustomerBalance.Update(custbalentity);
                        context_M.SaveChanges();

                        balcusjson = new BalcusjsonBalanceCustomerJson(cusjson2AllCustomer.intContactId,
                            strNameCustomer, custbalentity.numBalance.Round(2));
                    }
                    else
                    {
                        //                                  //The customer has not a balance partial.

                        if (
                            //                              //If the customer has a Payment or CreditMemo or Invoice
                            //                              //    it is create a balanceCustomer.
                            boolCustomerHasPaymentOrCreditOrInvoice
                            )
                        {
                            custbalentity = new CustbalentityCustomerBalanceEntityDB
                            {
                                intContactId = cusjson2AllCustomer.intContactId,
                                numBalance = numBalancePartialCustomer,
                                strDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                                intPkPrintshop = ps_I.intPk
                            };

                            context_M.CustomerBalance.Add(custbalentity);
                            context_M.SaveChanges();
                        }
                        

                        balcusjson = new BalcusjsonBalanceCustomerJson(cusjson2AllCustomer.intContactId,
                            strNameCustomer, numBalancePartialCustomer.Round(2));
                    }

                    //                                      //Add Customer balance to the list.
                    darrbalcusjson_O.Add(balcusjson);
                }

                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "";
            }
            else
            {
                intStatus_IO = 400;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Status Invalid";
            }
            /*END-CASE*/
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static double numTotalPaymentNotBalancedFromCustomer(
            //                                              //Get all payment of the customer and add the balances to a
            //                                              //    var numTotalPaymentNotBalanced.
            //                                              //Checked the pay how balanced.

            int intContactId_I, 
            ref bool boolCustomerHasPaymentOrCreditOrInvoice_IO,
            ref List<PaymtPaymentEntityDB> darrpaymtentityPayment_M,
            Odyssey2Context context_M
            )
        {
            double numTotalPaymentNotBalanced = 0;

            List<int> darrintPkPaymentToRemoveFromList = new List<int>();

            foreach (PaymtPaymentEntityDB paymnt in darrpaymtentityPayment_M)
            {
                if (
                    intContactId_I == paymnt.intContactId
                    )
                {
                    boolCustomerHasPaymentOrCreditOrInvoice_IO = true;

                    //                                      //Add payments.
                    numTotalPaymentNotBalanced = numTotalPaymentNotBalanced + paymnt.numAmount;
                    paymnt.boolBalanced = true;

                    //                                      //Update and save changes in database.
                    context_M.Payment.Update(paymnt);
                    context_M.SaveChanges();

                    darrintPkPaymentToRemoveFromList.Add(paymnt.intPk);
                }
            }

            //                                              //Remove payment from the list.
            foreach (int intPkPayment in darrintPkPaymentToRemoveFromList)
            {
                PaymtPaymentEntityDB paymtToRemove = darrpaymtentityPayment_M.FirstOrDefault(paymt => paymt.intPk ==
                    intPkPayment);

                darrpaymtentityPayment_M.Remove(paymtToRemove);
            }

            return numTotalPaymentNotBalanced;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static double numTotalCreditMemoNotBalancedFromCustomer(
            //                                              //Get all CreditMemo of the customer and add the balances
            //                                              //    to a var numTotalCreditMemoNotBalanced.
            //                                              //Checked the creditmemo how balanced.

            int intContactId_I,
            ref bool boolCustomerHasPaymentOrCreditOrInvoice_IO,
            ref List<CrmentityCreditMemoEntityDB> darrcrmentityCreditMemo_M,
            Odyssey2Context context_M
            )
        {
            double numTotalCreditMemoNotBalanced = 0.0;

            List<int> darrintPkCreditMemoToRemoveFromList = new List<int>();

            foreach (CrmentityCreditMemoEntityDB crmentity in darrcrmentityCreditMemo_M)
            {
                if (
                    intContactId_I == crmentity.intContactId
                    )
                {
                    boolCustomerHasPaymentOrCreditOrInvoice_IO = true;

                    //                                      //Add Credit memo.
                    numTotalCreditMemoNotBalanced = numTotalCreditMemoNotBalanced + crmentity.numOriginalAmount;
                    crmentity.boolBalanced = true;

                    //                                      //Update and save changes in database.
                    context_M.CreditMemo.Update(crmentity);
                    context_M.SaveChanges();

                    darrintPkCreditMemoToRemoveFromList.Add(crmentity.intPk);
                }
            }

            //                                              //Remove payment from the list.
            foreach (int intPkCrm in darrintPkCreditMemoToRemoveFromList)
            {
                CrmentityCreditMemoEntityDB crmToRemove = darrcrmentityCreditMemo_M.FirstOrDefault(crm => crm.intPk ==
                    intPkCrm);

                darrcrmentityCreditMemo_M.Remove(crmToRemove);
            }

            return numTotalCreditMemoNotBalanced;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static double numTotalInvoiceNotBalancedFromCustomer(
            //                                              //Get all invoice of the customer and add the balances
            //                                              //    to a var numTotalInvoiceNotBalanced.
            //                                              //Checked the invoice how balanced.

            int intContactId_I,
            ref bool boolCustomerHasPaymentOrCreditOrInvoice_IO,
            ref List<InvoInvoiceEntityDB> darrcusbalentityInvoiced_M,
            Odyssey2Context context_M
            )
        {
            double numTotalInvoiceNotBalanced = 0.0;

            List<int> darrintPkInvoiceToRemoveFromList = new List<int>();

            foreach (InvoInvoiceEntityDB inventity in darrcusbalentityInvoiced_M)
            {
                if (
                    intContactId_I == inventity.intContactId
                    )
                {
                    boolCustomerHasPaymentOrCreditOrInvoice_IO = true;

                    //                                      //Add Credit memo.
                    numTotalInvoiceNotBalanced = numTotalInvoiceNotBalanced + inventity.numAmount;
                    inventity.boolBalanced = true;

                    //                                      //Update and save changes in database.
                    context_M.Invoice.Update(inventity);
                    context_M.SaveChanges();

                    darrintPkInvoiceToRemoveFromList.Add(inventity.intPk);
                }
            }

            //                                              //Remove invoice from the list.
            foreach (int intPkInvoice in darrintPkInvoiceToRemoveFromList)
            {
                InvoInvoiceEntityDB invToRemove = darrcusbalentityInvoiced_M.FirstOrDefault(invo => invo.intPk ==
                    intPkInvoice);

                darrcusbalentityInvoiced_M.Remove(invToRemove);
            }

            return numTotalInvoiceNotBalanced;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static List<int> darrintCustomerNotBalanced(
            //                                              //Get Customer not balanced.

            //                                              //List of payment.
            List<PaymtPaymentEntityDB> darrpaymtentityPayment_I,
            //                                              //List of CreditMemos.
            List<CrmentityCreditMemoEntityDB> darrcrmentityCreditMemo_I,
            //                                              //List of invoice.
            List<InvoInvoiceEntityDB> darrcusbalentityInvoiced_I
            )
        {
            List<int> darrintCustomerNotBalanced = new List<int>();

            //                                              //Add customer that has payments.
            foreach (PaymtPaymentEntityDB paymtentity in darrpaymtentityPayment_I)
            {
                if (
                    //                                      //The client not exist still.
                    !darrintCustomerNotBalanced.Exists(intCus => intCus == paymtentity.intContactId)
                    )
                {
                    //                                      //Add contactId.
                    darrintCustomerNotBalanced.Add(paymtentity.intContactId);
                }
            }

            //                                              //Add customer that has creditMemos.
            foreach (CrmentityCreditMemoEntityDB crmentity in darrcrmentityCreditMemo_I)
            {
                if (
                    //                                      //The client not exist still.
                    !darrintCustomerNotBalanced.Exists(intCus => intCus == crmentity.intContactId)
                    )
                {
                    //                                      //Add contactId.
                    darrintCustomerNotBalanced.Add(crmentity.intContactId);
                }
            }

            //                                              //Add customer that has invoice.
            foreach (InvoInvoiceEntityDB inventity in darrcusbalentityInvoiced_I)
            {
                if (
                    //                                      //The client not exist still.
                    !darrintCustomerNotBalanced.Exists(intCus => intCus == inventity.intContactId)
                    )
                {
                    //                                      //Add contactId.
                    darrintCustomerNotBalanced.Add(inventity.intContactId);
                }
            }

            return darrintCustomerNotBalanced;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subAddBankDeposit(
            //                                              //Add deposit bank.

            PsPrintShop ps_I,
            String strDate_I,
            int intPkAccount_I,
            List<int> darrintPkPayment_I,
            out BankdepsummjsonBankDepositSummaryJson bankdepsummjson_O,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Json to return.
            bankdepsummjson_O = null;


            String strTime = "00:00:00";
            ZonedTime ztime;
            intStatus_IO = 402;
            strUserMessage_IO = "Select a date valid.";
            strDevMessage_IO = "Date not valid.";
            if (
                ZonedTimeTools.boolIsValidDateTime(strDate_I, strTime, out ztime, ref strUserMessage_IO,
                        ref strDevMessage_IO)
                )
            {
                bool boolPkPaymentValid = true;

                //                                              //Find pkAccountType bank.
                AcctypentityAccountTypeEntityDB acctyp = context_M.AccountType.FirstOrDefault(acctyp =>
                    acctyp.strType == strAccountTypeBank);

                AccentityAccountEntityDB accentityAccount = context_M.Account.FirstOrDefault(acc =>
                    acc.intPk == intPkAccount_I &&
                    //                                          //Verify that is be a account Type Bank.
                    acc.intPkAccountType == acctyp.intPk && acc.intPkPrintshop == ps_I.intPk);

                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "PkAccount not valid";
                if (
                    accentityAccount != null
                    )
                {
                    //                                          //List of payment.
                    List<PaymtPaymentEntityDB> darrpaymtPayments = new List<PaymtPaymentEntityDB>();

                    double numAmmount = 0.0;

                    int intI = 0;
                    //                                          //Get each payment.
                    while (
                        boolPkPaymentValid &&
                        //                                      //Take each payment.
                        intI < darrintPkPayment_I.Count
                        )
                    {
                        //                                      //All Payment to depositBank should have a reference.
                        PaymtPaymentEntityDB paymtPayment = context_M.Payment.FirstOrDefault(paymt =>
                            paymt.intPk == darrintPkPayment_I[intI]);

                        if (
                            //                                  //PkPayment valid.
                            paymtPayment != null
                            )
                        {
                            numAmmount = numAmmount + paymtPayment.numAmount;
                            darrpaymtPayments.Add(paymtPayment);
                        }
                        else
                        {
                            boolPkPaymentValid = false;
                        }

                        intI = intI + 1;
                    }

                    intStatus_IO = 403;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "PkPayment not valid";
                    if (
                        boolPkPaymentValid
                        )
                    {
                        BkdptentityBankDepositEntityDB bkdptDepositBank = new BkdptentityBankDepositEntityDB
                        {
                            strDate = strDate_I,
                            numAmount = numAmmount,
                            intPkBankAccount = accentityAccount.intPk
                        };

                        //                                      //Add BankDeposit.
                        context_M.BankDeposit.Add(bkdptDepositBank);
                        context_M.SaveChanges();

                        //                                      //Update the Payment with the deposit generate.
                        foreach (PaymtPaymentEntityDB paymt in darrpaymtPayments)
                        {
                            paymt.intnPkBankDeposit = bkdptDepositBank.intPk;
                            context_M.Payment.Update(paymt);
                        }

                        context_M.SaveChanges();

                        //                                      //Generate Movments.
                        AccmoventityAccountMovementEntityDB accmoveentityAddedBankAccount =
                            new AccmoventityAccountMovementEntityDB
                            {
                                strStartDate = strDate_I,
                                strStartTime = "12:00:00",
                                strConcept = "Bank Deposit",
                                numnIncrease = numAmmount,
                                intPkAccount = accentityAccount.intPk,
                                intnPkBankDeposit = bkdptDepositBank.intPk
                            };
                        context_M.AccountMovement.Add(accmoveentityAddedBankAccount);
                        context_M.SaveChanges();

                        //                                      //Generate an increase amount to the account's balance
                        AccAccounting.subUpdateAccountBalance(accentityAccount.intPk, null, numAmmount, context_M);

                        //                                      //GetPkUndepositFounds.
                        AccentityAccountEntityDB accentityAccUndepositFounds = context_M.Account.FirstOrDefault(acc =>
                            acc.strName == AccAccounting.strUndepositedFundsName &&
                            acc.boolGeneric == true && acc.strNumber == AccAccounting.strUndepositedFundsNumber);

                        //                                      //Ammount decreased from Undeposit founds.
                        AccmoventityAccountMovementEntityDB accmoveentityUndepositFoundsDecreaded =
                            new AccmoventityAccountMovementEntityDB
                            {
                                strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                                strStartTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                                strConcept = "Bank Deposit",
                                numnDecrease = numAmmount,
                                intPkAccount = accentityAccUndepositFounds.intPk,
                                intnPkBankDeposit = bkdptDepositBank.intPk
                            };
                        context_M.AccountMovement.Add(accmoveentityUndepositFoundsDecreaded);
                        context_M.SaveChanges();

                        //                                      //Generate an decrease amount to the account's balance
                        AccAccounting.subUpdateAccountBalance(accentityAccUndepositFounds.intPk, numAmmount, null,
                            context_M);

                        BankdepsummjsonBankDepositSummaryJson bankdepsummjson;
                        AccAccounting.subGetBankDepositSummary(bkdptDepositBank.intPk, ps_I, out bankdepsummjson,
                            context_M, ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);

                        bankdepsummjson_O = bankdepsummjson;

                        intStatus_IO = 200;
                        strUserMessage_IO = "";
                        strDevMessage_IO = "Success.";
                    }
                }
            }  
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetCreditMemo(
            //                                              //Get one credit memo from printshop.

            int intPkCreditMemo_I,
            PsPrintShop ps_I,
            out CredmemjsonCreditMemoJson credmemjson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Json to return.
            credmemjson_O = null;

            //                                              //Establish connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Find credit memo.
            CrmentityCreditMemoEntityDB crmentity = context.CreditMemo.FirstOrDefault(credit =>
                credit.intPk == intPkCreditMemo_I && credit.intPkPrintshop == ps_I.intPk);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Credit not found.";
            if (
                crmentity != null
                )
            {
                //                                          //Credit memo information.
                Memojson2MemoJson2 memoJson2 = JsonSerializer.Deserialize<Memojson2MemoJson2>(crmentity.strMemo);

                //                                          //Create json that will be used to generate PDF.
                credmemjson_O = AccAccounting.credmemjsonGenerateCreditMemoJson(memoJson2.strCustomerFullName,
                    memoJson2.strCreditMemoNumber, memoJson2.strDate, memoJson2.strBilledTo, memoJson2.strDescription,
                    memoJson2.numAmount, ps_I);

                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "Success.";
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public static void subUpdateAccountBalance(
            int intPkAccount_I,
            double? numnDecrease_I,
            double? numnIncrease_I,
            Odyssey2Context context_M
            )
        {
            //                                              //Find account.
            AccentityAccountEntityDB accentity = context_M.Account.FirstOrDefault(acc => acc.intPk == intPkAccount_I);

            if (
                accentity != null
                )
            {
                if (
                    //                                      //Account's balance will be decreased.
                    numnDecrease_I != null
                    )
                {
                    accentity.numBalance = accentity.numBalance - (int)numnDecrease_I;
                }
                else
                {
                    //                                      //Account's balance will be increased.
                    accentity.numBalance = accentity.numBalance + (int)numnIncrease_I;
                }

                context_M.Update(accentity);
                context_M.SaveChanges();
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetAccountBalance(
            //                                              //Get balance from account of the printshop.

            int intPk_I,
            PsPrintShop ps_I,
            out double? numBalanceAccount_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Balance to return.
            numBalanceAccount_O = null;

            //                                              //Establish connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Find pkAccountType bank.
            AcctypentityAccountTypeEntityDB acctyp = context.AccountType.FirstOrDefault(acctyp =>
                acctyp.strType == AccAccounting.strAccountTypeBank);

            //                                              //Find Account.
            AccentityAccountEntityDB accentity = context.Account.FirstOrDefault(acc =>
                acc.intPk == intPk_I && acc.intPkPrintshop == ps_I.intPk && acc.intPkAccountType == acctyp.intPk);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Account not found";
            if (
                accentity != null
                )
            {
                numBalanceAccount_O = accentity.numBalance;

                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "";
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetContactBillingAddress(
            //                                              //Get contact's billing address.

            int intContactId_I,
            String strPrintshopId_I,
            out Contaddjson2ContactAddressJson2 custaddjson2_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Json to return.
            custaddjson2_O = null;

            //                                              //Get data from Wisnet.
            String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                GetSection("Odyssey2Settings")["urlWisnetApi"];

            Task<ContaddjsonContactAddressJson> Task_custaddjsonFromWisnet = 
                HttpTools<ContaddjsonContactAddressJson>.GetOneAsyncToEndPoint(strUrlWisnet +
                "/Customer/ContactBillingAddress/" + strPrintshopId_I + "/" + intContactId_I);
            Task_custaddjsonFromWisnet.Wait();

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Wisnet connection lost.";
            if (
                Task_custaddjsonFromWisnet.Result != null
                )
            {
                custaddjson2_O = new Contaddjson2ContactAddressJson2(Task_custaddjsonFromWisnet.Result.strAddress);

                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "";
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetStatement(
            //                                              //Get statement.

            PsPrintShop ps_I,
            String strType_I,
            String strStartDate_I,
            String strEndDate_I,
            int intContactId_I,
            out StmjsonStatementJson stmjsonStatemet_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Statement to return.
            stmjsonStatemet_O = null;

            //                                              //Establish connection.
            Odyssey2Context context = new Odyssey2Context();

            CusjsonCustomerJson cusjson;
            //                      //Find customer.
            CusCustomer.subGetOneCustomerFromPrintshop(ps_I, intContactId_I,
                out cusjson, ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);

            intStatus_IO = 402;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Customer not exist";
            if (
                cusjson != null
                )
            {
                //                                          //Get info Gral for all statements.

                String strLogoUrl = AccAccounting.strLogoUrlFromPrintshop(ps_I.strPrintshopId);
                String strTitle = null;
                String strBilleTo = cusjson.strFirstName + " " + cusjson.strLastName;
                String strDate = Date.Now(ZonedTimeTools.timezone).ToString();
                String strDateFrom = null;
                String strDateTo = null;

                //                                          //These datas only will be use for an open statement.
                double numCurrentDue = 0.0;
                double num30DaysDue = 0.0;
                double num60DaysDue = 0.0;
                double num90DaysDue = 0.0;
                double numMore90DaysDue = 0.0;
                double numAmountDue = 0.0;

                double? numnTotalCharge = null;
                double? numnTotalPayment = null;
                double? numnTotalAmount = null;

                List<rwstmjsonRowStatementJson> darrrow = new List<rwstmjsonRowStatementJson>();

                /*CASE*/
                if (
                    //                                      //Statement Transaction..
                    strType_I == AccAccounting.strStatementTypeTransaction
                    )
                {
                    intStatus_IO = 402;
                    String strStartTime = "00:00:00";
                    String strEndTime = "23:59:59";
                    ZonedTime ztimeStart;
                    ZonedTime ztimeEnd;
                    bool boolIsValidStartDateTimeAndEndDateTimeAccount =
                        ZonedTimeTools.boolIsValidStartDateTimeAndEndDateTimeAccount(strStartDate_I, strStartTime,
                        strEndDate_I, strEndTime, out ztimeStart, out ztimeEnd, ref strUserMessage_IO, ref strDevMessage_IO);

                    if (
                        boolIsValidStartDateTimeAndEndDateTimeAccount
                        )
                    {
                        strDateFrom = strStartDate_I;
                        strDateTo = strEndDate_I;
                        numnTotalPayment = 0;
                        numnTotalCharge = 0;
                        strTitle = "Transaction Statement";
                        AccAccounting.subGetRowTransactionStatement(ps_I, intContactId_I, ztimeStart, ztimeEnd,
                            ref numnTotalCharge, ref numnTotalPayment, ref darrrow, ref intStatus_IO, 
                            ref strUserMessage_IO, ref strDevMessage_IO);
                    }
                }
                else if (
                    //                                      //Statement open.
                    strType_I == AccAccounting.strStatementTypeOpen
                   )
                {
                    numnTotalPayment = 0;
                    numnTotalCharge = 0;
                    strTitle = "Open Statement";
                    AccAccounting.subGetRowOpenBalance(ps_I, intContactId_I, strDate, ref numnTotalCharge,
                        ref numnTotalPayment, ref darrrow, ref numCurrentDue, ref num30DaysDue, ref num60DaysDue,
                        ref num90DaysDue, ref numMore90DaysDue, ref numAmountDue, ref intStatus_IO,
                        ref strUserMessage_IO, ref strDevMessage_IO);
                }
                else if (
                    //                                      //Statement forward.
                    strType_I == AccAccounting.strStatementTypeForward
                    )
                {
                    strDateFrom = strStartDate_I;
                    intStatus_IO = 402;
                    String strStartTime = "00:00:00";
                    String strEndDate = Date.Now(ZonedTimeTools.timezone).ToString();
                    String strEndTime = "23:59:59";
                    ZonedTime ztimeStart;
                    ZonedTime ztimeEnd;
                    bool boolIsValidStartDateTimeAndEndDateTimeAccount =
                        ZonedTimeTools.boolIsValidStartDateTimeAndEndDateTimeAccount(strStartDate_I, strStartTime,
                        strEndDate, strEndTime, out ztimeStart, out ztimeEnd, ref strUserMessage_IO, ref strDevMessage_IO);
                     
                    if (
                        boolIsValidStartDateTimeAndEndDateTimeAccount
                        )
                    {
                        strTitle = "Balance Forward Statement";
                        AccAccounting.subGetRowForwardStatement(ps_I, intContactId_I, ztimeStart, ztimeEnd, 
                            ref numnTotalAmount, ref darrrow, ref intStatus_IO, ref strUserMessage_IO, 
                            ref strDevMessage_IO);
                    }
                }
                /*END-CASE*/

                if (
                    //                                      //Statement generate sucesfully.
                    intStatus_IO == 200
                    )
                {
                    stmjsonStatemet_O = new StmjsonStatementJson(strLogoUrl, strTitle, strBilleTo, strDate, strDateFrom,
                        strDateTo, darrrow.ToArray(), numnTotalCharge, numnTotalPayment, numnTotalAmount,
                        numCurrentDue, num30DaysDue, num60DaysDue, num90DaysDue, numMore90DaysDue, numAmountDue);
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public static string strLogoUrlFromPrintshop(
            //                                              //Get strLogoUrl.

            String strPrintshopId_I
            )
        {
            //                                              //Find printshop's logo's url.

            String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                GetSection("Odyssey2Settings")["urlWisnetApi"];
            //                                              //Get info from wisnet.
            Task<PrintlogurljsonPrintshopLogoUrlJson> Task_printzipjsonFromWisnet =
            HttpTools<PrintlogurljsonPrintshopLogoUrlJson>.GetOneAsyncToEndPoint(strUrlWisnet +
                "/PrintShopData/PrintshopLogoUrl/" + strPrintshopId_I);
            Task_printzipjsonFromWisnet.Wait();

            String strLogoUrl = "";
            if (
                Task_printzipjsonFromWisnet.Result != null
                )
            {
                strLogoUrl = Task_printzipjsonFromWisnet.Result.strLogoUrl;
            }

            return strLogoUrl;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subGetRowTransactionStatement(
            //                                              //Get row for transaction statement.

            PsPrintShop ps_I,
            int intContactId_I,
            ZonedTime ztimeStart_I,
            ZonedTime ztimeEnd_I,
            ref double? numnTotalCharge_IO,
            ref double? numnTotalPayment_IO,
            ref List<rwstmjsonRowStatementJson> darrrow_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                          //Establish conection to DB.
            Odyssey2Context context = new Odyssey2Context();

            //                                          //Get payment.
            List<PaymtPaymentEntityDB> darrpaymtentityPayment =
                context.Payment.Where(paymt => 
                paymt.intPkPrintshop == ps_I.intPk &&
                paymt.intContactId == intContactId_I
                ).ToList();

            List<PaymtPaymentEntityDB> darrpaymtentityPaymentNew = new List<PaymtPaymentEntityDB>();
            foreach (PaymtPaymentEntityDB paymtNew in darrpaymtentityPayment)
            {
                ZonedTime ztimePaymt = ZonedTimeTools.NewZonedTime(paymtNew.strDate.ParseToDate(),
                "12:00:00".ParseToTime());

                if (
                    //                                      //Get beetwen ztimeFrom and ztimeTo.
                    ztimePaymt >= ztimeStart_I &&
                    ztimePaymt < ztimeEnd_I
                    )
                {
                    darrpaymtentityPaymentNew.Add(paymtNew);
                }
            }

            //                                          //Get Credit memo.
            List<CrmentityCreditMemoEntityDB> darrcrmentityCreditMemo =
                context.CreditMemo.Where(crm => crm.intPkPrintshop == ps_I.intPk &&
                crm.intContactId == intContactId_I
                ).ToList();

            List<CrmentityCreditMemoEntityDB> darrcrmentityCreditMemoNew = new List<CrmentityCreditMemoEntityDB>();
            foreach (CrmentityCreditMemoEntityDB crmentityNew in darrcrmentityCreditMemo)
            {
                ZonedTime ztimeCrm = ZonedTimeTools.NewZonedTime(crmentityNew.strDate.ParseToDate(),
                "12:00:00".ParseToTime());

                if (
                    //                                      //Get beetwen ztimeFrom and ztimeTo.
                    ztimeCrm >= ztimeStart_I &&
                    ztimeCrm < ztimeEnd_I
                    )
                {
                    darrcrmentityCreditMemoNew.Add(crmentityNew);
                }
            }

            //                                          //Get invoice.
            List<InvoInvoiceEntityDB> darrinventityInvoiced =
                context.Invoice.Where(inv => inv.intPkPrintshop == ps_I.intPk &&
                inv.intContactId == intContactId_I
                ).ToList();

            List<InvoInvoiceEntityDB> darrinventityInvoicedNew = new List<InvoInvoiceEntityDB>();

            foreach (InvoInvoiceEntityDB inventityNew in darrinventityInvoiced)
            {
                ZonedTime ztimeInv = ZonedTimeTools.NewZonedTime(inventityNew.strDate.ParseToDate(),
                "12:00:00".ParseToTime());

                if (
                    //                                      //Get beetwen ztimeFrom and ztimeTo.
                    ztimeInv >= ztimeStart_I &&
                    ztimeInv < ztimeEnd_I
                    )
                {
                    darrinventityInvoicedNew.Add(inventityNew);
                }
            }

            AccAccounting.subGetRowsForTransaction(darrpaymtentityPaymentNew, darrcrmentityCreditMemoNew,
                darrinventityInvoicedNew, ref darrrow_M, ref numnTotalCharge_IO, ref numnTotalPayment_IO);

            intStatus_IO = 200;
            strUserMessage_IO = "";
            strDevMessage_IO = "";            
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subGetRowsForTransaction(
            //                                              //Get rows.

            List<PaymtPaymentEntityDB> darrpaymtentityPayment_I,
            List<CrmentityCreditMemoEntityDB> darrcrmentityCreditMemo_I,
            List<InvoInvoiceEntityDB> darrinventityInvoiced_I,
            ref List<rwstmjsonRowStatementJson> darrrow_M,
            ref double? numnTotalCharge_IO,
            ref double? numnTotalPayment_IO
            )
        {
            //                                              //Generate rows Payment.
            foreach (PaymtPaymentEntityDB paymt in darrpaymtentityPayment_I)
            {
                numnTotalPayment_IO = numnTotalPayment_IO + paymt.numAmount;

                rwstmjsonRowStatementJson rwstm = new rwstmjsonRowStatementJson(paymt.strDate,
                    "Payment", paymt.strReference, null, paymt.numAmount.Round(2), null, null);

                darrrow_M.Add(rwstm);
            }

            //                                              //Generate rows CreditMemo.
            foreach (CrmentityCreditMemoEntityDB crm in darrcrmentityCreditMemo_I)
            {
                numnTotalPayment_IO = numnTotalPayment_IO + crm.numOriginalAmount;

                rwstmjsonRowStatementJson rwstm = new rwstmjsonRowStatementJson(crm.strDate,
                    "Credit memo", crm.strCreditMemoNumber, null, crm.numOriginalAmount.Round(2), null, null);

                darrrow_M.Add(rwstm);
            }

            //                                              //Generate rows invoie.
            foreach (InvoInvoiceEntityDB inv in darrinventityInvoiced_I)
            {
                numnTotalCharge_IO = numnTotalCharge_IO + inv.numAmount;

                rwstmjsonRowStatementJson rwstm = new rwstmjsonRowStatementJson(inv.strDate,
                    "Invoice", "" + inv.intOrderNumber, inv.numAmount.Round(2), null, null, null);

                darrrow_M.Add(rwstm);
            }

            darrrow_M.Sort();

            numnTotalCharge_IO = ((double)numnTotalCharge_IO).Round(2);
            numnTotalPayment_IO = ((double)numnTotalPayment_IO).Round(2);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetRowOpenBalance(
            //                                              //Get row for openBalance statement.

            PsPrintShop ps_I,
            int intContactId_I,
            String strOpenStatementDate_I,
            ref double? numnTotalCharge_IO,
            ref double? numnTotalPayment_IO,
            ref List<rwstmjsonRowStatementJson> darrrow_M,
            ref double numCurrentDue_IO,
            ref double num30DaysDue_IO,
            ref double num60DaysDue_IO,
            ref double num90DaysDue_IO,
            ref double numMore90DaysDue_IO,
            ref double numAmountDue_IO,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Establish conection to DB.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get payment.
            List<PaymtPaymentEntityDB> darrpaymtentityPayment =
                context.Payment.Where(paymt => paymt.intPkPrintshop == ps_I.intPk &&
                paymt.intContactId == intContactId_I &&
                paymt.numOpenBalance != 0
                ).ToList();

            //                                              //Get Credit memo.
            List<CrmentityCreditMemoEntityDB> darrcrmentityCreditMemo =
                context.CreditMemo.Where(crm => crm.intPkPrintshop == ps_I.intPk &&
                crm.intContactId == intContactId_I &&
                crm.numOpenBalance != 0
                ).ToList();

            //                                              //Get invoice.
            List<InvoInvoiceEntityDB> darrinventityInvoiced =
                context.Invoice.Where(inv => inv.intPkPrintshop == ps_I.intPk &&
                inv.intContactId == intContactId_I &&
                inv.numOpenBalance != 0
                ).ToList();

            AccAccounting.subGetRowsForOpenBalance(darrpaymtentityPayment, darrcrmentityCreditMemo,
                darrinventityInvoiced, strOpenStatementDate_I, ref darrrow_M, ref numnTotalCharge_IO,
                ref numnTotalPayment_IO, ref numCurrentDue_IO, ref num30DaysDue_IO, ref num60DaysDue_IO,
                ref num90DaysDue_IO, ref numMore90DaysDue_IO, ref numAmountDue_IO);

            intStatus_IO = 200;
            strUserMessage_IO = "";
            strDevMessage_IO = "";

        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subGetRowsForOpenBalance(
            //                                              //Get rows.

            List<PaymtPaymentEntityDB> darrpaymtentityPayment_I,
            List<CrmentityCreditMemoEntityDB> darrcrmentityCreditMemo_I,
            List<InvoInvoiceEntityDB> darrinventityInvoiced_I,
            String strOpenStatementDate_I,
            ref List<rwstmjsonRowStatementJson> darrrow_M,
            ref double? numnTotalCharge_IO,
            ref double? numnTotalPayment_IO,
            ref double numCurrentDue_IO,
            ref double num30DaysDue_IO,
            ref double num60DaysDue_IO,
            ref double num90DaysDue_IO,
            ref double numMore90DaysDue_IO,
            ref double numAmountDue_IO
            )
        {
            //                                              //Generate rows Payment.
            foreach (PaymtPaymentEntityDB paymt in darrpaymtentityPayment_I)
            {
                numnTotalPayment_IO = numnTotalPayment_IO + paymt.numOpenBalance;

                rwstmjsonRowStatementJson rwstm = new rwstmjsonRowStatementJson(paymt.strDate,
                    "Payment", paymt.strReference, null, paymt.numOpenBalance.Round(2), null, null);

                darrrow_M.Add(rwstm);
            }

            //                                              //Generate rows CreditMemo.
            foreach (CrmentityCreditMemoEntityDB crm in darrcrmentityCreditMemo_I)
            {
                numnTotalPayment_IO = numnTotalPayment_IO + crm.numOpenBalance;

                rwstmjsonRowStatementJson rwstm = new rwstmjsonRowStatementJson(crm.strDate,
                    "Credit memo", crm.strCreditMemoNumber, null, crm.numOpenBalance.Round(2), null, null);

                darrrow_M.Add(rwstm);
            }

            //                                              //Generate rows invoie.
            foreach (InvoInvoiceEntityDB inv in darrinventityInvoiced_I)
            {
                numnTotalCharge_IO = numnTotalCharge_IO + inv.numOpenBalance;

                rwstmjsonRowStatementJson rwstm = new rwstmjsonRowStatementJson(inv.strDate,
                    "Invoice", "" + inv.intOrderNumber, inv.numOpenBalance.Round(2), null, null, null);

                //                                          //To get total month distribution.
                AccAccounting.subGetMontTotalDistributionForOpenStatements(inv.strDate, strOpenStatementDate_I,
                    inv.numOpenBalance.Round(2), ref numCurrentDue_IO, ref num30DaysDue_IO, ref num60DaysDue_IO,
                    ref num90DaysDue_IO, ref numMore90DaysDue_IO, ref numAmountDue_IO);

                darrrow_M.Add(rwstm);
            }

            darrrow_M.Sort();

            numnTotalCharge_IO = ((double)numnTotalCharge_IO).Round(2);
            numnTotalPayment_IO = ((double)numnTotalPayment_IO).Round(2);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subGetMontTotalDistributionForOpenStatements(
            //                                              //

            String strInvoiceDate_I,
            String strOpenStatementDate_I,
            double numOpenBalance_I,
            ref double numCurrentDue_I,
            ref double num30DaysDue_I,
            ref double num60DaysDue_I,
            ref double num90DaysDue_I,
            ref double numMore90DaysDue_I,
            ref double numAmountDue_I
            )
        {
            Date dateInvoice = strInvoiceDate_I.ParseToDate();
            Date dateOpenStatement = strOpenStatementDate_I.ParseToDate();
            numAmountDue_I = numAmountDue_I + numOpenBalance_I;

            /*CASE*/
            if (
                dateInvoice == dateOpenStatement
                )
            {
                numCurrentDue_I = numCurrentDue_I + numOpenBalance_I;
            }
            else if (
               dateInvoice.IsBetween(dateOpenStatement - 30, dateOpenStatement - 1)
               )
            {
                num30DaysDue_I = num30DaysDue_I + numOpenBalance_I;
            }
            else if (
               dateInvoice.IsBetween(dateOpenStatement - 60, dateOpenStatement - 31)
               )
            {
                num60DaysDue_I = num60DaysDue_I + numOpenBalance_I;
            }
            else if (
               dateInvoice.IsBetween(dateOpenStatement - 90, dateOpenStatement - 61)
               )
            {
                num90DaysDue_I = num90DaysDue_I + numOpenBalance_I;
            }
            else if (
               dateInvoice <= dateOpenStatement - 91
               )
            {
                numMore90DaysDue_I = numMore90DaysDue_I + numOpenBalance_I;
            }
            /*END-CASE*/
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetRowForwardStatement(
            //                                              //Get row for transaction statement.

            PsPrintShop ps_I,
            int intContactId_I,
            ZonedTime ztimeStart_I,
            ZonedTime ztimeEnd_I,
            ref double? numnTotalAmount_I,
            ref List<rwstmjsonRowStatementJson> darrrow_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                          //Establish conection to DB.
            Odyssey2Context context = new Odyssey2Context();

            //                                          //Get payment.
            List<PaymtPaymentEntityDB> darrpaymtentityPayment = context.Payment.Where(paymt =>
                paymt.intPkPrintshop == ps_I.intPk && paymt.intContactId == intContactId_I).ToList();

            List<PaymtPaymentEntityDB> darrpaymtentityPaymentNew = new List<PaymtPaymentEntityDB>();
            foreach (PaymtPaymentEntityDB paymtentity in darrpaymtentityPayment)
            {
                ZonedTime ztimePaymt = ZonedTimeTools.NewZonedTime(paymtentity.strDate.ParseToDate(),
                "12:00:00".ParseToTime());

                if (
                    //                                      //Get beetwen ztimeFrom and ztimeTo.
                    ztimePaymt >= ztimeStart_I &&
                    ztimePaymt <= ztimeEnd_I
                    )
                {
                    darrpaymtentityPaymentNew.Add(paymtentity);
                }
            }

            //                                          //Get Credit memo.
            List<CrmentityCreditMemoEntityDB> darrcrmentityCreditMemo = context.CreditMemo.Where(crm => 
                crm.intPkPrintshop == ps_I.intPk && crm.intContactId == intContactId_I).ToList();

            List<CrmentityCreditMemoEntityDB> darrcrmentityCreditMemoNew = new List<CrmentityCreditMemoEntityDB>();
            foreach (CrmentityCreditMemoEntityDB crmentity in darrcrmentityCreditMemo)
            {
                ZonedTime ztimeCrm = ZonedTimeTools.NewZonedTime(crmentity.strDate.ParseToDate(),
                "12:00:00".ParseToTime());

                if (
                    //                                      //Get beetwen ztimeFrom and ztimeTo.
                    ztimeCrm >= ztimeStart_I &&
                    ztimeCrm <= ztimeEnd_I
                    )
                {
                    darrcrmentityCreditMemoNew.Add(crmentity);
                }
            }

            //                                          //Get invoice.
            List<InvoInvoiceEntityDB> darrinventityInvoiced = context.Invoice.Where(inv => 
                inv.intPkPrintshop == ps_I.intPk && inv.intContactId == intContactId_I).ToList();

            List<InvoInvoiceEntityDB> darrinventityInvoicedNew = new List<InvoInvoiceEntityDB>();

            foreach (InvoInvoiceEntityDB inventityNew in darrinventityInvoiced)
            {
                ZonedTime ztimeInv = ZonedTimeTools.NewZonedTime(inventityNew.strDate.ParseToDate(),
                "12:00:00".ParseToTime());

                if (
                    //                                      //Get beetwen ztimeFrom and ztimeTo.
                    ztimeInv >= ztimeStart_I &&
                    ztimeInv <= ztimeEnd_I
                    )
                {
                    darrinventityInvoicedNew.Add(inventityNew);
                }
            }

            AccAccounting.subGetRowsForBalanceForward(darrpaymtentityPaymentNew, darrcrmentityCreditMemoNew,
                darrinventityInvoicedNew, ref darrrow_M, ref numnTotalAmount_I);

            intStatus_IO = 200;
            strUserMessage_IO = "";
            strDevMessage_IO = "";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subGetRowsForBalanceForward(
            //                                              //Get rows.

            List<PaymtPaymentEntityDB> darrpaymtentityPayment_I,
            List<CrmentityCreditMemoEntityDB> darrcrmentityCreditMemo_I,
            List<InvoInvoiceEntityDB> darrinventityInvoiced_I,
            ref List<rwstmjsonRowStatementJson> darrrow_M,
            ref double? numnTotalAmount_I
            )
        {
            numnTotalAmount_I = 0;
            //                                              //Generate rows Payment.
            foreach (PaymtPaymentEntityDB paymt in darrpaymtentityPayment_I)
            {
                rwstmjsonRowStatementJson rwstmjson = new rwstmjsonRowStatementJson(paymt.strDate,
                    "Payment", paymt.strReference, null, null, paymt.numAmount.Round(2), null);

                darrrow_M.Add(rwstmjson);
            }

            //                                              //Generate rows CreditMemo.
            foreach (CrmentityCreditMemoEntityDB crm in darrcrmentityCreditMemo_I)
            {
                rwstmjsonRowStatementJson rwstmjson = new rwstmjsonRowStatementJson(crm.strDate,
                    "Credit memo", crm.strCreditMemoNumber, null, null, crm.numOriginalAmount.Round(2), null);

                darrrow_M.Add(rwstmjson);
            }

            //                                              //Generate rows invoie.
            foreach (InvoInvoiceEntityDB inv in darrinventityInvoiced_I)
            {
                rwstmjsonRowStatementJson rwstmjson = new rwstmjsonRowStatementJson(inv.strDate,
                    "Invoice", "" + inv.intOrderNumber, null, null, inv.numAmount.Round(2), null);

                darrrow_M.Add(rwstmjson);
            }

            //                                              //Sort rows by date
            darrrow_M.Sort();

            //                                              //Calculate balance.
            foreach (rwstmjsonRowStatementJson rwstmjson in darrrow_M)
            {
                if (
                    rwstmjson.strType == "Payment" ||
                    rwstmjson.strType == "Credit memo"
                    )
                {
                    //                                      //Update balance
                    numnTotalAmount_I = (numnTotalAmount_I + rwstmjson.numnAmount);
                    rwstmjson.numnBalance = numnTotalAmount_I;

                    //                                      //To show negative amount in pdf
                    rwstmjson.numnAmount = rwstmjson.numnAmount;
                }
                else if(
                    rwstmjson.strType == "Invoice"
                    )
                {
                    //                                      //Update balance
                    numnTotalAmount_I = (numnTotalAmount_I - rwstmjson.numnAmount);
                    rwstmjson.numnBalance = numnTotalAmount_I;
                }
            }

            numnTotalAmount_I = ((double)numnTotalAmount_I).Round(2);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetAccount(
            //                                              //Find one printshop's account.

            int intPkAccount_I,
            int intPkPrintshop_I,
            out Accjson4AccountJson4 accjson4_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            accjson4_O = null;

            //                                              //Create the connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Find printshop's account.
            AccentityAccountEntityDB accentity = context.Account.FirstOrDefault(acc =>
                acc.intPk == intPkAccount_I && acc.intPkPrintshop == intPkPrintshop_I);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Account not valid.";
            if (
                accentity != null
                )
            {
                //                                          //Build json.
                accjson4_O = new Accjson4AccountJson4(accentity.intPk, accentity.strNumber, accentity.strName, 
                    accentity.intPkAccountType);

                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "";
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subUpdateAccount(
            //                                              //Update an account.

            PsPrintShop ps_I,
            int intPk_I,
            String strName_I,
            String strNumber_I,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Get Account.
            AccentityAccountEntityDB accentityAccount = context_M.Account.FirstOrDefault(acc =>
                acc.intPkPrintshop == ps_I.intPk && acc.intPk == intPk_I && acc.boolGeneric == false);

            intStatus_IO = 402;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Account not exist how account not generic for this printshop.";
            if (
                //                                          //Account exist.
                accentityAccount != null
                )
            {
                if (
                    AccAccounting.boolIsValidAccountNumberAndName(strNumber_I, strName_I, intPk_I, 
                    ps_I, context_M, ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO)
                )
                {
                    accentityAccount.strName = strName_I;
                    accentityAccount.strNumber = strNumber_I;

                    context_M.SaveChanges();

                    intStatus_IO = 200;
                    strUserMessage_IO = "Success.";
                    strDevMessage_IO = "";
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
