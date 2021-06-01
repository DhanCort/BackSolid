/*TASK RP. CUSTOMER*/
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Configuration;
using Odyssey2Backend.DB_Odyssey2;
using Odyssey2Backend.JsonTypes;
using Odyssey2Backend.PrintShop;
using Odyssey2Backend.Utilities;
using Odyssey2Backend.XJDF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TowaStandard;

//                                                          //AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: August 07, 2020.

namespace Odyssey2Backend.Customer
{
    //==================================================================================================================
    public class CusCustomer
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTANTS.

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        //                                                  //Customer Pk.
        private readonly int intPk_Z;
        public int intPk { get { return this.intPk_Z; } }

        private readonly int intPkPrintshop_Z;
        public int intPkPrintshop { get { return this.intPkPrintshop_Z; } }


        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTOR.

        //--------------------------------------------------------------------------------------------------------------
        public CusCustomer(
            //                                              //Pk Customer.
            int intPk_I,
            //                                              //PkPrintshop.
            int intPkPrintshop_I
            )
        {
            this.intPk_Z = intPk_I;
            this.intPkPrintshop_Z = intPkPrintshop_I;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //TRANSFORMATION METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public static Cusjson2CustomerJson2 subAddCustomer(
            //                                              //Add a new cusomer to wisnet.

            String strFirstName_I,
            String strLastName_I,
            String strEmail_I,
            String strPassword_I,
            PsPrintShop ps_I,
            IConfiguration configuration_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            Cusjson2CustomerJson2 cusjson = null;

            intStatus_IO = 401;
            strUserMessage_IO = "First name, Last name, Email and Password can not be empty";
            strDevMessage_IO = "First name, Last name, Email and Password can not be empty";
            if (
                strFirstName_I.Length > 0 &&
                strLastName_I.Length > 0 &&
                strEmail_I.Length > 0 &&
                strPassword_I.Length > 0
                )
            {
                intStatus_IO = 402;
                strUserMessage_IO = "Invalid email.";
                strDevMessage_IO = "Invalid email.";
                if (
                    CusCustomer.boolIsValidEmail(strEmail_I)
                    )
                {
                    //                                      //Add customer to wisnet.
                    Task<String> Task_strResult = HttpTools<TjsonTJson>.PostAddCustomer(
                        configuration_I.GetValue<String>("Odyssey2Settings:urlWisnetApi") + "/Customer/Add",
                        ps_I.strPrintshopId.ParseToInt(), strFirstName_I, strLastName_I, strEmail_I, strPassword_I);
                    Task_strResult.Wait();

                    intStatus_IO = 403;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "Wisnet database connection lost.";
                    if (
                        Task_strResult.Result != null
                        )
                    {
                        if (
                            Task_strResult.Result.IsParsableToInt()
                            )
                        {
                            //                              //To easy code.
                            int intContactId = Task_strResult.Result.ParseToInt();
                            String strFullName = strFirstName_I + " " + strLastName_I;

                            //                              //Fill to return with new customer's info.
                            cusjson = new Cusjson2CustomerJson2(intContactId, strFullName);

                            intStatus_IO = 200;
                            strUserMessage_IO = "";
                            strDevMessage_IO = "";
                        }
                        else
                        {
                            intStatus_IO = 404;
                            strUserMessage_IO = Task_strResult.Result;
                            strDevMessage_IO = Task_strResult.Result;
                        }
                    }
                }
            }

            return cusjson;
        }

        //--------------------------------------------------------------------------------------------------------------
        private static bool boolIsValidEmail(
            //                                              //validate a given email.

            String strEmail_I
            )
        {
            //                                          //Create RegularExpresion to validate email.
            Regex regexToEmail = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)
                    |(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");

            bool boolValidEmail = regexToEmail.IsMatch(strEmail_I) ? true : false;

            return boolValidEmail;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetAllForAPrintshop(
            //                                              //Get all printshop's customers.

            PsPrintShop psPrintshop_I,
            out List<CusjsonCustomerJson> darrcusjson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            darrcusjson_O = null;

            //                                              //Get data from Wisnet.
            String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                GetSection("Odyssey2Settings")["urlWisnetApi"];

            Task<List<CusjsonCustomerJson>> Task_darrcusjsonFromWisnet = HttpTools<CusjsonCustomerJson>.
                GetListAsyncToEndPoint(strUrlWisnet + "/Customer/" + psPrintshop_I.strPrintshopId);

            Task_darrcusjsonFromWisnet.Wait();

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Wisnet database connection lost.";
            if (
                Task_darrcusjsonFromWisnet.Result != null
                )
            {
                darrcusjson_O = Task_darrcusjsonFromWisnet.Result;

                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "Success.";
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetOneCustomerFromPrintshop(
            PsPrintShop psPrintshop_I,
            int intCustomerId_I,
            out CusjsonCustomerJson cusjson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            cusjson_O = null;

            //                                              //Get data from Wisnet.
            String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                GetSection("Odyssey2Settings")["urlWisnetApi"];

            Task<List<CusjsonCustomerJson>> Task_arrcusjsonFromWisnet = HttpTools<CusjsonCustomerJson>.
                GetListAsyncToEndPoint(strUrlWisnet + "/Customer/" + psPrintshop_I.strPrintshopId + "/" +
                intCustomerId_I);

            Task_arrcusjsonFromWisnet.Wait();

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Wisnet database connection lost.";
            if (
                Task_arrcusjsonFromWisnet.Result != null
                )
            {
                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "No customer found.";
                if (
                    Task_arrcusjsonFromWisnet.Result.Count > 0
                    )
                {
                    cusjson_O = Task_arrcusjsonFromWisnet.Result[0];

                    intStatus_IO = 200;
                    strUserMessage_IO = "";
                    strDevMessage_IO = "Success.";
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool boolCustomerIsFromPrintshop(
            String strPrintshopId_I,
            int intCustomerId_I
            )
        {
            bool boolIsValid = false;
            //                                              //Get data from Wisnet.
            String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                GetSection("Odyssey2Settings")["urlWisnetApi"];

            Task<List<CusjsonCustomerJson>> Task_arrcusjsonFromWisnet = HttpTools<CusjsonCustomerJson>.
                GetListAsyncToEndPoint(strUrlWisnet + "/Customer/" + strPrintshopId_I + "/" +
                intCustomerId_I);

            Task_arrcusjsonFromWisnet.Wait();

            if (
                Task_arrcusjsonFromWisnet.Result != null && Task_arrcusjsonFromWisnet.Result.Count > 0
                )
            {
                boolIsValid = true;
            }

            return boolIsValid;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetCustomers(
            //                                              //Get all printshop's customers.

            PsPrintShop psPrintshop_I,
            out Cusjson2CustomerJson2[] arrcusjson2_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            arrcusjson2_O = null;

            //                                              //Get data from Wisnet.
            String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                GetSection("Odyssey2Settings")["urlWisnetApi"];

            Task<List<CusjsonCustomerJson>> Task_darrcusjsonFromWisnet = HttpTools<CusjsonCustomerJson>.
                GetListAsyncToEndPoint(strUrlWisnet + "/Customer/" + psPrintshop_I.strPrintshopId);

            Task_darrcusjsonFromWisnet.Wait();

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Wisnet database connection lost.";
            if (
                Task_darrcusjsonFromWisnet.Result != null
                )
            {
                List<Cusjson2CustomerJson2> darrcusjson2 = new List<Cusjson2CustomerJson2>();

                foreach (CusjsonCustomerJson cusjson in Task_darrcusjsonFromWisnet.Result)
                {
                    Cusjson2CustomerJson2 cusjson2 = new Cusjson2CustomerJson2(
                        cusjson.intContactId, cusjson.strFirstName + " " + cusjson.strLastName);
                    darrcusjson2.Add(cusjson2);
                }

                darrcusjson2 = darrcusjson2.OrderBy(cus => cus.strFullName).ToList();

                arrcusjson2_O = darrcusjson2.ToArray();

                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "Success.";
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetCreditMemos(
            //                                              //Get all the credit memos from a customer or get all the
            //                                              //      credit memos from the customers of a printshop

            int? intnContactId_I,
            PsPrintShop ps_I,
            out CrmjsonCreditMemoJson[] arrcrmjson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            arrcrmjson_O = null;

            List<CrmjsonCreditMemoJson> darrcrmjson = new List<CrmjsonCreditMemoJson>();
            if (
                //                                          //Get credit memos from one customer
                intnContactId_I != null
                )
            {
                intStatus_IO = 401;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Customer not valid.";
                if (
                    CusCustomer.boolCustomerIsFromPrintshop(ps_I.strPrintshopId, (int)intnContactId_I)
                    )
                {
                    Odyssey2Context context = new Odyssey2Context();

                    //                                      //Find customer credit memos
                    List<CrmentityCreditMemoEntityDB> darrcrmentity = context.CreditMemo.Where(crm => 
                        crm.intContactId == intnContactId_I && crm.intPkPrintshop == ps_I.intPk &&
                        crm.numOpenBalance > 0).ToList();

                    CusjsonCustomerJson cusjson;
                    //                                      //Find customer data
                    CusCustomer.subGetOneCustomerFromPrintshop(ps_I, (int)intnContactId_I, out cusjson,
                        ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);

                    foreach (CrmentityCreditMemoEntityDB crmentity in darrcrmentity)
                    {
                        //                                  //Create json object.
                        CrmjsonCreditMemoJson crmjson = new CrmjsonCreditMemoJson(crmentity.intPk, 
                            crmentity.strCreditMemoNumber, cusjson.strFirstName + " " + cusjson.strLastName);
                        darrcrmjson.Add(crmjson);
                    }

                    intStatus_IO = 200;
                    strUserMessage_IO = "";
                    strDevMessage_IO = "";
                }
            }
            else
            {
                Odyssey2Context context = new Odyssey2Context();

                //                                          //Find credit memos from the customers of a printshop
                List<CrmentityCreditMemoEntityDB> darrcrmentity = context.CreditMemo.Where(crm =>
                    crm.intPkPrintshop == ps_I.intPk && crm.numOpenBalance > 0).ToList();

                //                                          //Find customers data
                List<CusjsonCustomerJson> darrcusjson;
                CusCustomer.subGetAllForAPrintshop(ps_I, out darrcusjson, ref intStatus_IO, ref strUserMessage_IO, 
                    ref strDevMessage_IO);

                CusjsonCustomerJson cusjson;
                foreach (CrmentityCreditMemoEntityDB crmentity in darrcrmentity)
                {
                    cusjson = darrcusjson.FirstOrDefault(cus => cus.intContactId == crmentity.intContactId);
                    //                                      //Create json object.
                    CrmjsonCreditMemoJson crmjson = new CrmjsonCreditMemoJson(crmentity.intPk,
                        crmentity.strCreditMemoNumber, cusjson.strFirstName + " " + cusjson.strLastName);
                    darrcrmjson.Add(crmjson);
                }

                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "";
            }

            darrcrmjson = darrcrmjson.OrderBy(crm => crm.strCustomerFullName).ToList();
            arrcrmjson_O = darrcrmjson.ToArray();
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetOpenInvoices(
            //                                              //Get all customer's unpaid invoices. 

            int? intnContactId_I,
            int? intnOrderNumber_I,
            PsPrintShop ps_I,
            out OpinvosjsonOpenInvoicesJson opinvosjson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Json to return.
            opinvosjson_O = null;

            //                                              //Establish connection.
            Odyssey2Context context = new Odyssey2Context();

            int? intnContactId = intnContactId_I;
            if (
                intnOrderNumber_I != null
                )
            {
                //                                          //Find contact id that belongs to current order number.

                //                                          //Find invoice.
                InvoInvoiceEntityDB invoentity = context.Invoice.FirstOrDefault(invo =>
                    invo.intOrderNumber == intnOrderNumber_I && invo.intPkPrintshop == ps_I.intPk);

                if (
                    invoentity != null
                    )
                {
                    intnContactId = invoentity.intContactId;
                }
            }

            intStatus_IO = 401;
            strUserMessage_IO = "No invoices found.";
            strDevMessage_IO = "";
            if (
                intnContactId != null
                )
            {
                //                                          //Find customer's open invoices.
                List<InvoInvoiceEntityDB> darrinvoentity = context.Invoice.Where(invo =>
                    invo.intContactId == intnContactId &&
                    invo.intPkPrintshop == ps_I.intPk &&
                    //                                      //Invoices with an open balance amount have not been paid
                    //                                      //      yet.
                    invo.numOpenBalance > 0
                    ).ToList();

                //                                          //Find receivable account.
                AccentityAccountEntityDB accentityReceivable =
                    (from accentityAccount in context.Account
                    join acctypeentity in context.AccountType
                    on accentityAccount.intPkAccountType equals acctypeentity.intPk
                    where acctypeentity.strType == AccAccounting.strAccountTypeAsset &&
                    accentityAccount.strNumber == AccAccounting.strAccountsReceivableNumber &&
                    accentityAccount.intPkPrintshop == ps_I.intPk &&
                    accentityAccount.boolGeneric
                    select accentityAccount).FirstOrDefault();

                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Receivable account not found.";
                if (
                    accentityReceivable != null
                    )
                {
                    //                                      //List to add open invoices.
                    List<OpinvojsonOpenInvoiceJson> darropinvo = new List<OpinvojsonOpenInvoiceJson>();

                    foreach (InvoInvoiceEntityDB invoentity in darrinvoentity)
                    {
                        //                                  //Create json object.
                        OpinvojsonOpenInvoiceJson opinvojson = new OpinvojsonOpenInvoiceJson(invoentity.intPk, 
                            invoentity.intOrderNumber, invoentity.numAmount.Round(2), invoentity.numOpenBalance.Round(2));
                        darropinvo.Add(opinvojson);
                    }

                    opinvosjson_O = new OpinvosjsonOpenInvoicesJson(darropinvo.ToArray(), (int)intnContactId);

                    intStatus_IO = 200;
                    strUserMessage_IO = "";
                    strDevMessage_IO = "";
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetCredits(
            //                                              //Get all the unapplied credit memos and payments from a 
            //                                              //      customer
            int? intnContactId_I,
            int? intnOrderNumber_I,
            PsPrintShop ps_I,
            out CrjsonCreditJson[] arrcrmjson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            arrcrmjson_O = null;

            Odyssey2Context context = new Odyssey2Context();

            int intContactId = 0;
            bool boolIsValidContactId = true;
            /*CASE*/
            if (
                intnContactId_I != null
                )
            {
                if (
                    CusCustomer.boolCustomerIsFromPrintshop(ps_I.strPrintshopId, (int)intnContactId_I)
                    )
                {
                    intContactId = (int)intnContactId_I;
                }
                else
                {
                    boolIsValidContactId = false;
                }
            }
            else if(
                intnOrderNumber_I != null
                )
            {
                InvoInvoiceEntityDB invoentity = context.Invoice.FirstOrDefault(invo => 
                    invo.intOrderNumber == (int)intnOrderNumber_I);

                if (
                    invoentity != null
                    )
                {
                    intContactId = (int)invoentity.intContactId;
                }
                else
                {
                    boolIsValidContactId = false;
                }
            } 
            else
            {
                boolIsValidContactId = false;
            }
            /*END-CASE*/

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Customer not valid.";
            if (
                boolIsValidContactId
                )
            {
                List<CrjsonCreditJson> darrcrjson = new List<CrjsonCreditJson>();

                //                                          //Find unapplied customer credit memos
                List<CrmentityCreditMemoEntityDB> darrcrmentity = context.CreditMemo.Where(crm =>
                    crm.intContactId == intContactId && crm.intPkPrintshop == ps_I.intPk &&
                    crm.numOpenBalance > 0).ToList();

                foreach (CrmentityCreditMemoEntityDB crmentity in darrcrmentity)
                {
                    //                                      //Create json object.
                    CrjsonCreditJson crmjson = new CrjsonCreditJson(crmentity.intPk, crmentity.strCreditMemoNumber,
                        crmentity.numOriginalAmount.Round(2), crmentity.numOpenBalance.Round(2), true);
                    darrcrjson.Add(crmjson);
                }

                //                                          //Find unapplied customer payments
                List<PaymtPaymentEntityDB> darrpaymtentity = context.Payment.Where(paymt =>
                    paymt.intContactId == intContactId && paymt.intPkPrintshop == ps_I.intPk &&
                    paymt.numOpenBalance > 0).ToList();

                foreach (PaymtPaymentEntityDB paymtentity in darrpaymtentity)
                {
                    //                                      //Create json object.
                    CrjsonCreditJson crmjson = new CrjsonCreditJson(paymtentity.intPk, paymtentity.strReference,
                        paymtentity.numAmount.Round(2), paymtentity.numOpenBalance.Round(2), false);
                    darrcrjson.Add(crmjson);
                }

                arrcrmjson_O = darrcrjson.ToArray();

                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "";
            }
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
