/*TASK RP. PRINTSHOP*/
using Microsoft.Extensions.Configuration;
using Odyssey2Backend.App;
using Odyssey2Backend.DB_Odyssey2;
using Odyssey2Backend.Job;
using Odyssey2Backend.JsonTypes;
using Odyssey2Backend.XJDF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TowaStandard;
using Odyssey2Backend.JsonTemplates;
using System.Text.Json;
using Odyssey2Backend.Utilities;
using System.Text.Json.Serialization;
using Odyssey2Backend.Customer;

//                                                          //AUTHOR: Towa (VSTD - Victor Torres).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: November 11, 2019.

namespace Odyssey2Backend.PrintShop
{
    //==================================================================================================================
    public class PsPrintShop
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTANTS.

        //                                                  //Url complement to send a proof.
        public const String strSendAProofUrl = "/manage/workflow/sendproof/?Login_As=0";

        //                                                  //Data set for report filter
        public const String strJobs = "Jobs";
        public const String strCustomers = "Customers";
        public const String strAccounts = "Accounts";

        //                                                  //Complement to customer list url.
        public const String strComplementToCustomerUrl = "/manage/accounts/customers/?Login_As=0";

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        //                                                  //Primary key in the database.
        private readonly int intPk_Z;
        public int intPk { get { return this.intPk_Z; } }

        //                                                  //
        private String strPrintshopId_Z;
        public String strPrintshopId { get { return this.strPrintshopId_Z; } }

        //                                                  //
        private String strPrintshopName_Z;
        public String strPrintshopName { get { return this.strPrintshopName_Z; } }

        //                                                  //
        private String strSpecialPassword_Z;
        public String strSpecialPassword { get { return this.strSpecialPassword_Z; } }

        //                                                  //Offset
        private bool boolOffset_Z;
        public bool boolOffset { get { return this.boolOffset_Z; } }

        //                                                  //Printshop's timezone.
        public String strTimeZone { get; set; }        

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //DYNAMIC VARIABLES.

        //                                                  //Dictionary of product types.
        //                                                  //Created from OrderForms.
        private Dictionary<int, ProdtypProductType> dicprodtyp_Z;
        public Dictionary<int, ProdtypProductType> dicprodtyp
        {
            get
            {
                this.subGetProductTypeFromDB(out this.dicprodtyp_Z);
                return this.dicprodtyp_Z;
            }
        }

        //                                                  //Dictionary of process types.
        private Dictionary<int, ProtypProcessType> dicprotyp_Z;
        public Dictionary<int, ProtypProcessType> dicprotyp
        {
            get
            {
                this.subGetProcessTypeFromDB(out this.dicprotyp_Z);
                return this.dicprotyp_Z;
            }
        }

        //                                                  //Resources types.
        private Dictionary<String, RestypResourceType> dicrestyp_Z;
        public Dictionary<String, RestypResourceType> dicrestyp
        {
            get
            {
                this.subGetResourceTypeFromDB(out this.dicrestyp_Z);
                return this.dicrestyp_Z;
            }
        }

        //                                                  //Processes.
        private Dictionary<int, ProProcess> dicpro_Z;
        public Dictionary<int, ProProcess> dicpro
        {
            get
            {
                this.subGetProcessFromDB(out this.dicpro_Z);
                return this.dicpro_Z;
            }
        }

        //                                                  //Resources.
        private Dictionary<int, ResResource> dicres_Z;
        public Dictionary<int, ResResource> dicres
        {
            get
            {
                this.subGetResourceFromDB(out this.dicres_Z);
                return this.dicres_Z;
            }
        }

        //                                                  //Dictionary of resource calculations.
        private Dictionary<int, CalCalculation> diccalResource_Z;
        public Dictionary<int, CalCalculation> diccalResource
        {
            get
            {
                this.subGetResourceCalculationsFromDB(out this.diccalResource_Z);
                return this.diccalResource_Z;
            }
        }

        //                                                  //Dictionary of process calculations.
        private Dictionary<int, CalCalculation> diccalProcess_Z;
        public Dictionary<int, CalCalculation> diccalProcess
        {
            get
            {
                this.subGetProcessCalculationsFromDB(out this.diccalProcess_Z);
                return this.diccalProcess_Z;
            }
        }

        //                                                  //Dictionary of Jobs in Progress.
        private Dictionary<int, JobJob> dicjobInProgress_Z;
        public Dictionary<int, JobJob> dicjobInProgress
        {
            get
            {
                this.subGetJobInProgressFromDB(out this.dicjobInProgress_Z);
                return this.dicjobInProgress_Z;
            }
        }

        //                                                  //Dictionary of Jobs in Completed.
        private Dictionary<int, JobJob> dicjobCompleted_Z;
        public Dictionary<int, JobJob> dicjobCompleted
        {
            get
            {
                this.subGetJobCompletedFromDB(out this.dicjobCompleted_Z);
                return this.dicjobCompleted_Z;
            }
        }

        //                                                  //URL
        private String strUrl_Z;
        public String strUrl
        {
            get
            {
                this.subGetPrintshopUrl(out this.strUrl_Z);
                return this.strUrl_Z;
            }
        }

        //                                                  //TimesZones list.
        private List<TimzonjsonTimesZonesJson> darrTimzonList_Z;
        public List<TimzonjsonTimesZonesJson> darrTimzonList 
        { 
            get 
            {
                this.subGetTimesZoneList(out this.darrTimzonList_Z);
                return this.darrTimzonList_Z; 
            } 
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //SUPPORT METHODS TO DYNAMIC VARIABLES.

        //--------------------------------------------------------------------------------------------------------------
        private void subGetPrintshopUrl(
            //                                              //Get the current printshop's url.

            out String strUrl_O
            )
        {
            strUrl_O = null;

            String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                    GetSection("Odyssey2Settings")["urlWisnetApi"];
            Task<String> Task_strUrl = HttpTools<TjsonTJson>.GetStringAsyncToEndPoint(strUrlWisnet +
                "/PrintShopData/GetPrintshopUrl/" + this.strPrintshopId);
            Task_strUrl.Wait();
            if (
                Task_strUrl.Result != null
                )
            {
                strUrl_O = Task_strUrl.Result;
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        private void subGetTimesZoneList(
            //                                              //Get the current printshop's url.

            out List<TimzonjsonTimesZonesJson> darrTimzonList_O
            )
        {
            darrTimzonList_O = new List<TimzonjsonTimesZonesJson>();

            //                                              //US_HAWAIIAN Time Zone
            TimzonjsonTimesZonesJson timzonjsonHaw = new TimzonjsonTimesZonesJson("US_HAWAIIAN",
                "Hawaii", false);
            darrTimzonList_O.Add(timzonjsonHaw);

            //                                              //US_ALASKAN Time Zone
            TimzonjsonTimesZonesJson timzonjsonAla = new TimzonjsonTimesZonesJson("US_ALASKAN",
                "Alaska", false);
            darrTimzonList_O.Add(timzonjsonAla);

            //                                              //US_PACIFIC Time Zone
            TimzonjsonTimesZonesJson timzonjsonPac = new TimzonjsonTimesZonesJson("US_PACIFIC",
                "Pacific", false);
            darrTimzonList_O.Add(timzonjsonPac);

            //                                              //US_MOUNTAIN Time Zone
            TimzonjsonTimesZonesJson timzonjsonMou = new TimzonjsonTimesZonesJson("US_MOUNTAIN",
                "Mountain", false);
            darrTimzonList_O.Add(timzonjsonMou);

            //                                              //US_CENTRAL Time Zone
            TimzonjsonTimesZonesJson timzonjsonCen = new TimzonjsonTimesZonesJson("US_CENTRAL",
                "Central", false);
            darrTimzonList_O.Add(timzonjsonCen);

            //                                              //US_EASTERN Time Zone
            TimzonjsonTimesZonesJson timzonjsonEas = new TimzonjsonTimesZonesJson("US_EASTERN",
                "Eastern", false);
            darrTimzonList_O.Add(timzonjsonEas);

            //                                              //EU_WESTERN_BERLIN Time Zone
            TimzonjsonTimesZonesJson timzonjsonBer = new TimzonjsonTimesZonesJson("EU_WESTERN_BERLIN",
                "Berlin", false);
            darrTimzonList_O.Add(timzonjsonBer);

            //                                              //GREENWICH Time Zone
            TimzonjsonTimesZonesJson timzonjsonGre = new TimzonjsonTimesZonesJson("GREENWICH",
                "Greenwich", false);
            darrTimzonList_O.Add(timzonjsonGre);

            //                                              //AZORES Time Zone
            TimzonjsonTimesZonesJson timzonjsonAzo = new TimzonjsonTimesZonesJson("AZORES",
                "Azores", false);
            darrTimzonList_O.Add(timzonjsonAzo);

            //                                              //MID_ATLANTIC Time Zone
            TimzonjsonTimesZonesJson timzonjsonMid = new TimzonjsonTimesZonesJson("MID_ATLANTIC",
                "Mid-Atlantic", false);
            darrTimzonList_O.Add(timzonjsonMid);

            //                                              //GREENLAND Time Zone
            TimzonjsonTimesZonesJson timzonjsonGrl = new TimzonjsonTimesZonesJson("GREENLAND",
                "Greenland", false);
            darrTimzonList_O.Add(timzonjsonGrl);

            //                                              //ATLANTIC Time Zone
            TimzonjsonTimesZonesJson timzonjsonAtl = new TimzonjsonTimesZonesJson("ATLANTIC",
                "Atlantic", false);
            darrTimzonList_O.Add(timzonjsonAtl);

            
        }

        //--------------------------------------------------------------------------------------------------------------
        private void subGetProductTypeFromDB(
           //                                               //Get the products from wisnet and save all in Odyssey 
           //                                               //      database.

           //                                               //Dictionary of products. It is null when the connection 
           //                                               //      was not possible with Wisnet.
           out Dictionary<int, ProdtypProductType> dicprodtem_O
           )
        {
            if
               (
               this.boolGetWisnetData()
               )
            {
                //                                          //Establish the connection.
                Odyssey2Context context = new Odyssey2Context();

                JobJob.subCreateProductDummy(this.strPrintshopId);

                dicprodtem_O = new Dictionary<int, ProdtypProductType>();

                IQueryable<EtentityElementTypeEntityDB> setetentity = context.ElementType.Where(etentity =>
                    (etentity.strAddedBy == this.strPrintshopId) &&
                    (etentity.strResOrPro == EtElementTypeAbstract.strProduct) &&
                    (etentity.boolDeleted == false));

                //                                          //Add database data to the dicprodtem.
                foreach (EtentityElementTypeEntityDB etentityToAdd in setetentity)
                {
                    ProdtypProductType prodtemToAdd = new ProdtypProductType(etentityToAdd.intPk,
                        etentityToAdd.strXJDFTypeId, etentityToAdd.strAddedBy, etentityToAdd.intPrintshopPk,
                        etentityToAdd.strCustomTypeId, etentityToAdd.intWebsiteProductKey,
                        etentityToAdd.strCategory, etentityToAdd.strClassification, etentityToAdd.boolnIsPublic,
                        etentityToAdd.intnPkAccount);
                    dicprodtem_O.Add(etentityToAdd.intPk, prodtemToAdd);
                }
            }
            else
            {
                dicprodtem_O = null;
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private bool boolGetWisnetData(
            //                                              //Add new peoducts.
            //                                              //Update actual products.
            //                                              //Get rid of product not in Wisnet any longer.

            //                                              //Important!. The GetFromWisnet, que usa el 
            //                                              //      darrprodtypjson2GetProducts doesn´t delete the
            //                                              //      products that are not in WIsnet any more.
            //                                              
            )
        {
            bool boolIsValidData = true;

            //                                              //Get data from Wisnet.
            String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
            GetSection("Odyssey2Settings")["urlWisnetApi"];

            Task<List<ProdjsonProductJson>> Task_darrprodjsonFromWisnet = HttpTools<ProdjsonProductJson>.
                    GetListAsyncToEndPoint(strUrlWisnet + "/PrintShopData/printshopCategories/" +
                    this.strPrintshopId);

            Task_darrprodjsonFromWisnet.Wait();

            if (
                //                                          //There is access to the service of Wisnet.
                Task_darrprodjsonFromWisnet.Result != null
                )
            {
                //                                          //Final array of products from Wisnet.
                List<ProdjsonProductJson> darrprodjsonFromWisnet = Task_darrprodjsonFromWisnet.Result;
                ProdjsonProductJson[] arrprodjsonFromWisnet = darrprodjsonFromWisnet.ToArray();

                //                                          //Get data from Odyssey2 database.
                //                                          //1. Get darretentityProductsInDatabase from the database.
                Odyssey2Context context = new Odyssey2Context();

                List<EtentityElementTypeEntityDB> darretentityProductsInDatabase =
                    context.ElementType.Where(etentity => etentity.intPrintshopPk == this.intPk &&
                    etentity.strResOrPro == EtElementTypeAbstract.strProduct).ToList<EtentityElementTypeEntityDB>();

                /*WHILE DO*/
                int intI = 0;
                while (
                    (intI < arrprodjsonFromWisnet.Length) &&
                    boolIsValidData
                    )
                {
                    String strProductName = arrprodjsonFromWisnet[intI].strProductName;
                    if (
                        //                                  //Productname is null or empty.
                        string.IsNullOrEmpty(arrprodjsonFromWisnet[intI].strProductName)
                        )
                    {
                        strProductName = "Unassigned";
                    }

                    //                                      //Search prodjsonFromWisnet and remove it from the
                    //                                      //      darretentityProductsInDatabase.
                    //                                      //At the end of the loop, this list will have only the
                    //                                      //      products in the Odysseys2 database that were
                    //                                      //      removed from Wisnet.
                    EtentityElementTypeEntityDB etentity = darretentityProductsInDatabase.FirstOrDefault(etentity =>
                        (etentity.intWebsiteProductKey == arrprodjsonFromWisnet[intI].intProductKey) &&
                        (etentity.intPrintshopPk == this.intPk) &&
                        (etentity.strCustomTypeId == strProductName));

                    //                                      //Add product type from Wisnet, only if not yet in the DB.           
                    if (
                        //                                  //The product is not in db.
                        etentity == null
                        )
                    {
                        if (
                            arrprodjsonFromWisnet[intI].intProductKey > 0
                            )
                        {
                            EtentityElementTypeEntityDB etentityDuplicate =
                                context.ElementType.FirstOrDefault(etentity =>
                                (etentity.intWebsiteProductKey == arrprodjsonFromWisnet[intI].intProductKey) &&
                                (etentity.intPrintshopPk == this.intPk) &&
                                (etentity.strCustomTypeId == strProductName));

                            if (
                                etentityDuplicate == null
                                )
                            {
                                //                          //Create the entity.
                                etentity = new EtentityElementTypeEntityDB
                                {
                                    strXJDFTypeId = EtElementTypeAbstract.strNotXJDF,
                                    strAddedBy = this.strPrintshopId,
                                    intPrintshopPk = this.intPk,
                                    strCustomTypeId = strProductName,
                                    strCategory = arrprodjsonFromWisnet[intI].strCategory,
                                    strResOrPro = EtElementTypeAbstract.strProduct,
                                    intWebsiteProductKey = arrprodjsonFromWisnet[intI].intProductKey,
                                    boolnIsPublic = arrprodjsonFromWisnet[intI].boolIsPublic
                                };
                                context.ElementType.Add(etentity);
                                context.SaveChanges();
                            }
                            else
                            {
                                //                          //Nothing to do.
                            }
                        }
                    }
                    else
                    {
                        darretentityProductsInDatabase.Remove(etentity);

                        //                                  //Update the boolnPublic and the category.
                        etentity.boolnIsPublic = arrprodjsonFromWisnet[intI].boolIsPublic;
                        etentity.strCategory = arrprodjsonFromWisnet[intI].strCategory;
                        etentity.boolDeleted = false;
                        context.ElementType.Update(etentity);
                        context.SaveChanges();
                    }
                    intI = intI + 1;
                }

                context.SaveChanges();
                if (
                    //                                      //There are products in the Odyssey2 DB that are not any
                    //                                      //      longer in the Wisnet database.
                    darretentityProductsInDatabase.Count > 0
                    )
                {
                    foreach (EtentityElementTypeEntityDB etentity in darretentityProductsInDatabase)
                    {
                        //                                  //FIX IT.
                        //                                  //To validate if the product should be logially or 
                        //                                  //      phisycally deleted.
                        /*List<JobentityJobEntityDB> darrjobentity =
                            (from jobentity in context.Job
                             join wfentity in context.Workflow
                             on jobentity.intPkWorkflow equals wfentity.intPk
                             where wfentity.intPkProduct == this.intPk
                             select jobentity).ToList();*/

                        etentity.boolDeleted = true;
                        context.ElementType.Update(etentity);
                        context.SaveChanges();
                    }
                }
            }
            else
            {
                //                                      //No data from Wisnet.
                boolIsValidData = false;
            }
            return boolIsValidData;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private void deleteProductFromDB(
            EtentityElementTypeEntityDB etentityToDelete_I
            )
        {
            //                                              //FIX IT. TO CONSIDER WITH DAVE (21/08/2020)
            //                                              //Summary.
            //                                              //1. Delete calculation of this product.
            //                                              //2. Delete its attributes.
            //                                              //3. Delete form attret, Pk of the product.
            //                                              //4. Delete from etet its link as Dad or Son.
            //                                              //5. Delete product from et.

            Odyssey2Context context = new Odyssey2Context();

            //                                              //1. Delete calculation of this product.                                          
            IQueryable<CalentityCalculationEntityDB> setcalentityCalculationsToDelete =
                from calentity in context.Calculation
                where calentity.intnPkProduct == etentityToDelete_I.intPk
                select calentity;
            foreach (CalentityCalculationEntityDB calentity in setcalentityCalculationsToDelete)
            {
                context.Calculation.Remove(calentity);
            }
            context.SaveChanges();

            //                                              //3. Delete attret links.
            IQueryable<AttretentityAttributeElementTypeEntityDB> setattretentityToDelete =
                from attretentity in context.AttributeElementType
                where attretentity.intPkElementType == etentityToDelete_I.intPk
                select attretentity;
            foreach (AttretentityAttributeElementTypeEntityDB attretentity in setattretentityToDelete)
            {
                context.AttributeElementType.Remove(attretentity);
            }
            context.SaveChanges();

            //                                              //2. Delete its attributes.
            IQueryable<AttrentityAttributeEntityDB> setattrentityAttributesToDelete =
                from attrentity in context.Attribute
                join attretentity in context.AttributeElementType
                on attrentity.intPk equals attretentity.intPkAttribute
                where attretentity.intPkElementType == etentityToDelete_I.intPk
                select attrentity;
            foreach (AttrentityAttributeEntityDB attrentity in setattrentityAttributesToDelete)
            {
                context.Attribute.Remove(attrentity);
            }
            context.SaveChanges();

            //                                              //4. Delete etet links.
            IQueryable<EtetentityElementTypeElementTypeEntityDB> setetetentityToDelete =
                from etetentity in context.ElementTypeElementType
                where
                (etetentity.intPkElementTypeDad == etentityToDelete_I.intPk) ||
                (etetentity.intPkElementTypeSon == etentityToDelete_I.intPk)
                select etetentity;
            foreach (EtetentityElementTypeElementTypeEntityDB etetentity in setetetentityToDelete)
            {
                context.ElementTypeElementType.Remove(etetentity);
            }
            context.SaveChanges();

            //                                              //5. Finally, delete the product.
            EtentityElementTypeEntityDB etProductToDelete =
                context.ElementType.FirstOrDefault(etentity => etentity.intPk == etentityToDelete_I.intPk);
            context.ElementType.Remove(etProductToDelete);
            context.SaveChanges();
        }

        //--------------------------------------------------------------------------------------------------------------
        public List<Prodtypjson2ProductTypeJson2> darrprodtypjson2GetProducts(
            String strCategory_I,
            String strKeyword_I,
            PsPrintShop ps_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            List<Prodtypjson2ProductTypeJson2> darrprodtypjson2 = new List<Prodtypjson2ProductTypeJson2>();

            Odyssey2Context context = new Odyssey2Context();

            List<EtentityElementTypeEntityDB> darretentityProductsInDatabase = new List<EtentityElementTypeEntityDB>();
            if (
                strCategory_I != null
                )
            {
                darretentityProductsInDatabase =
                    context.ElementType.Where(etentity => etentity.intPrintshopPk == this.intPk &&
                    etentity.strResOrPro == EtElementTypeAbstract.strProduct && etentity.strCategory == strCategory_I &&
                    etentity.boolDeleted == false).ToList();
            }
            else
            {
                darretentityProductsInDatabase =
                    context.ElementType.Where(etentity => etentity.intPrintshopPk == this.intPk &&
                    etentity.strResOrPro == EtElementTypeAbstract.strProduct && etentity.boolDeleted == false).ToList();
            }

            //                                              //To easy code.
            String strWebsiteUrl = ps_I.strUrl;

            foreach (EtentityElementTypeEntityDB etentity in darretentityProductsInDatabase)
            {
                String strOrderUrl;
                if (
                    etentity.boolnIsPublic == true
                    )
                {
                    //                                          //Create order's form link.
                    strOrderUrl = "http://" + strWebsiteUrl + "/printing/" + etentity.strCategory.Replace(" ", "-")
                    + "/" + etentity.intWebsiteProductKey;
                }
                else
                {
                    strOrderUrl = "http://" + strWebsiteUrl + "/docLib/" + etentity.intWebsiteProductKey;
                }
                
               
                Prodtypjson2ProductTypeJson2 prodjson2 = new Prodtypjson2ProductTypeJson2(etentity.intPk,
                    etentity.strCustomTypeId, "", etentity.strCategory, etentity.intnPkAccount, strOrderUrl);

                darrprodtypjson2.Add(prodjson2);
            }

            if (
                strKeyword_I != null
                )
            {
                darrprodtypjson2 = darrprodtypjson2.Where(prod =>
                prod.strCategory.ToLower().Contains(strKeyword_I.ToLower()) ||
                prod.strTypeId.ToLower().Contains(strKeyword_I.ToLower())).ToList();
            }

            intStatus_IO = 200;
            strUserMessage_IO = "";
            strDevMessage_IO = "Success.";

            return darrprodtypjson2;
        }

        //--------------------------------------------------------------------------------------------------------------
        public List<Prodtypjson2ProductTypeJson2> darrprodtypjson2GetPublicProducts(
            String strCategory_I,
            String strKeyword_I,
            PsPrintShop ps_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            List<Prodtypjson2ProductTypeJson2> darrprodtypjson2 = new List<Prodtypjson2ProductTypeJson2>();

            Odyssey2Context context = new Odyssey2Context();

            List<EtentityElementTypeEntityDB> darretentityProductsInDatabase = new List<EtentityElementTypeEntityDB>();
            if (
                strCategory_I != null
                )
            {
                darretentityProductsInDatabase =
                    context.ElementType.Where(etentity => etentity.intPrintshopPk == this.intPk &&
                    etentity.strResOrPro == EtElementTypeAbstract.strProduct && etentity.boolnIsPublic == true &&
                    etentity.strCategory == strCategory_I && etentity.boolDeleted == false).ToList();
            }
            else
            {
                darretentityProductsInDatabase =
                    context.ElementType.Where(etentity => etentity.intPrintshopPk == this.intPk &&
                    etentity.strResOrPro == EtElementTypeAbstract.strProduct && etentity.boolnIsPublic == true && 
                    etentity.boolDeleted == false).ToList();
            }

            //                                              //To easy code.
            String strWebsiteUrl = ps_I.strUrl;

            foreach (EtentityElementTypeEntityDB etentity in darretentityProductsInDatabase)
            {
                //                                          //Create order's form link.
                String strOrderUrl = "http://" + strWebsiteUrl + "/printing/" + etentity.strCategory.Replace(" ", "-") 
                    + "/" + etentity.intWebsiteProductKey;

                Prodtypjson2ProductTypeJson2 prodjson2 = new Prodtypjson2ProductTypeJson2(etentity.intPk,
                    etentity.strCustomTypeId, "", etentity.strCategory, etentity.intnPkAccount, strOrderUrl);

                darrprodtypjson2.Add(prodjson2);
            }

            if (
                strKeyword_I != null
                )
            {
                darrprodtypjson2 = darrprodtypjson2.Where(prod =>
                prod.strCategory.ToLower().Contains(strKeyword_I.ToLower()) ||
                prod.strTypeId.ToLower().Contains(strKeyword_I.ToLower())).ToList();
            }

            intStatus_IO = 200;
            strUserMessage_IO = "";
            strDevMessage_IO = "Success.";
            return darrprodtypjson2;
        }

        //--------------------------------------------------------------------------------------------------------------
        public List<Prodtypjson2ProductTypeJson2> darrprodtypjson2GetPrivateProducts(
            String strCategory_I,
            String strKeyword_I,
            int? intnCompanyId_I,
            int? intnBranchId_I,
            int? intnContactId_I,
            PsPrintShop ps_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            List<Prodtypjson2ProductTypeJson2> darrprodtypjson2 = new List<Prodtypjson2ProductTypeJson2>();

            //                                              //Get data from Wisnet.
            String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                GetSection("Odyssey2Settings")["urlWisnetApi"];
            Task<List<ProdprijsonProducPrivatetJson>> Task_darrprodjsonFromWisnet =
                HttpTools<ProdprijsonProducPrivatetJson>.GetListAsyncToEndPoint(strUrlWisnet +
                "/PrintShopData/PrivateProducts/" + this.strPrintshopId);
            Task_darrprodjsonFromWisnet.Wait();

            if (
                //                                          //There is access to the service of Wisnet.
                Task_darrprodjsonFromWisnet.Result != null
                )
            {
                List<ProdprijsonProducPrivatetJson> darrprodjsonFromWisnet = Task_darrprodjsonFromWisnet.Result;
                List<ProdprijsonProducPrivatetJson> darrprodjsonFromWisnetToFront =
                    new List<ProdprijsonProducPrivatetJson>();
                if (
                    //                                      //It was not information found in Wisnet DB.
                    darrprodjsonFromWisnet.Count() == 1 && darrprodjsonFromWisnet[0].intProductKey == -1
                    )
                {
                    //                                      //Nothing to do.
                }
                else
                {
                    //                                          //Before update information in Odyssey2 DB, apply
                    //                                          //      filters.
                    if (
                        !(intnCompanyId_I == null && intnBranchId_I != null)
                        )
                    {
                        darrprodjsonFromWisnet = PsPrintShop.darrprodprijsonFilterPrivateProducts(darrprodjsonFromWisnet,
                        strCategory_I, strKeyword_I, intnCompanyId_I, intnBranchId_I, intnContactId_I);

                        //                                  //Delete duplicates.
                        List<int> intProductKey = new List<int>();
                        foreach (ProdprijsonProducPrivatetJson prodprivElement in darrprodjsonFromWisnet)
                        {
                            if (
                                !intProductKey.Contains(prodprivElement.intProductKey)
                                )
                            {
                                darrprodjsonFromWisnetToFront.Add(prodprivElement);
                                intProductKey.Add(prodprivElement.intProductKey);
                            }
                        }

                        darrprodtypjson2 = this.darrprodtypjson2UpdatePrivateProducts(darrprodjsonFromWisnetToFront,
                            ps_I);

                        intStatus_IO = 200;
                        strUserMessage_IO = "";
                        strDevMessage_IO = "Success.";
                    }
                    else
                    {
                        intStatus_IO = 401;
                        strUserMessage_IO = "";
                        strDevMessage_IO = "A company branch cannot exits without a company.";
                    }
                }
            }
            return darrprodtypjson2;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static List<ProdprijsonProducPrivatetJson> darrprodprijsonFilterPrivateProducts(
            //                                              //Apply filters to a list of private products.

            List<ProdprijsonProducPrivatetJson> darrprodjsonFromWisnet_I,
            String strCategory_I,
            String strKeyword_I,
            int? intnCompanyId_I,
            int? intnBranchId_I,
            int? intnContactId_I
            )
        {
            List<ProdprijsonProducPrivatetJson> darrprodjsonFromWisnetFiltered = darrprodjsonFromWisnet_I;

            if (
                strCategory_I != null
                )
            {
                //                                          //Filter by Category.
                darrprodjsonFromWisnetFiltered = darrprodjsonFromWisnet_I.Where(pro => pro.strCategory == strCategory_I)
                    .ToList();

                if (
                    //                                      //We receive a companyId.
                    intnCompanyId_I != null
                    )
                {
                    //                                      //Filter by company.
                    darrprodjsonFromWisnetFiltered = darrprodjsonFromWisnetFiltered.Where(pro => pro.intnCompanyId ==
                    intnCompanyId_I).ToList();

                    if (
                        //                                  //We receive a branchId.
                        intnBranchId_I != null
                        )
                    {
                        //                                  //Filter by branch.
                        darrprodjsonFromWisnetFiltered = darrprodjsonFromWisnetFiltered.Where(pro => pro.intnBranchId ==
                            intnBranchId_I).ToList();

                        if (
                            //                              //We receive a contactId.
                            intnContactId_I != null
                            )
                        {
                            //                              //Filter by contact.
                            darrprodjsonFromWisnetFiltered = darrprodjsonFromWisnetFiltered.Where(pro => pro.intnContactId ==
                            intnContactId_I).ToList();
                        }
                    }
                    else
                    {
                        //                                  //We not receive a branchId.
                        if (
                            //                              //We receive a contactId.
                            intnContactId_I != null
                            )
                        {
                            //                              //Filter by contact.
                            darrprodjsonFromWisnetFiltered = darrprodjsonFromWisnetFiltered.Where(pro => pro.intnContactId ==
                            intnContactId_I).ToList();
                        }
                    }
                }
                else
                {
                    //                                      //We not receive a companyId, means there will not be a
                    //                                      //      branch, so we just have to check if we receive a
                    //                                      //      a contactId.

                    if (
                        //                                  //We receive a contactId.
                        intnContactId_I != null
                        )
                    {
                        //                                  //Filter by contact.
                        darrprodjsonFromWisnetFiltered = darrprodjsonFromWisnetFiltered.Where(pro => pro.intnContactId ==
                            intnContactId_I).ToList();
                    }
                }
            }
            else
            {
                //                                          //We not receive a category. 
                if (
                    //                                      //We receive a companyId.
                    intnCompanyId_I != null
                    )
                {
                    //                                      //Filter by company.
                    darrprodjsonFromWisnetFiltered = darrprodjsonFromWisnetFiltered.Where(pro => pro.intnCompanyId ==
                    intnCompanyId_I).ToList();

                    if (
                        //                                  //We receive a branchId.
                        intnBranchId_I != null
                        )
                    {
                        //                                  //Filter by branch
                        darrprodjsonFromWisnetFiltered = darrprodjsonFromWisnetFiltered.Where(pro => pro.intnBranchId ==
                            intnBranchId_I).ToList();

                        if (
                            //                              //We receive a contactId.
                            intnContactId_I != null
                            )
                        {
                            //                              //Filter by contact.
                            darrprodjsonFromWisnetFiltered = darrprodjsonFromWisnetFiltered.Where(pro => pro.intnContactId ==
                            intnContactId_I).ToList();
                        }
                    }
                    else
                    {
                        //                                  //We not receive a branchId.
                        if (
                            //                              //We receive a contactId.
                            intnContactId_I != null
                            )
                        {
                            //                              //Filter by contact.
                            darrprodjsonFromWisnetFiltered = darrprodjsonFromWisnetFiltered.Where(pro => pro.intnContactId ==
                            intnContactId_I).ToList();
                        }
                    }
                }
                else
                {
                    //                                      //We not receive a companyId, means there will not be a
                    //                                      //      branch, so we just have to check if we receive a
                    //                                      //      a contactId.

                    if (
                        //                                  //We receive a contactId.
                        intnContactId_I != null
                        )
                    {
                        //                                  //Filter by contact.
                        darrprodjsonFromWisnetFiltered = darrprodjsonFromWisnetFiltered.Where(pro => pro.intnContactId ==
                            intnContactId_I).ToList();
                    }
                }
            }

            //                                              //After apply necessary filter, verify if we need to
            //                                              //      filter by a keyword.
            if (
                (strKeyword_I != null && strKeyword_I.Length > 0)
                )
            {
                darrprodjsonFromWisnetFiltered = PsPrintShop.darrprodtypjson2FilterPrivateProductsByKeyword(
                    darrprodjsonFromWisnetFiltered, strKeyword_I);
            }

            return darrprodjsonFromWisnetFiltered;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static List<ProdprijsonProducPrivatetJson> darrprodtypjson2FilterPrivateProductsByKeyword(
            //                                              //Apply keyword filter to a list of private products.

            List<ProdprijsonProducPrivatetJson> darrprodjsonFromWisnetFiltered_I,
            String strKeyword_I
            )
        {
            List<ProdprijsonProducPrivatetJson> darrprodjsonFromWisnetFilteredByKeyword =
                darrprodjsonFromWisnetFiltered_I;

            darrprodjsonFromWisnetFilteredByKeyword = darrprodjsonFromWisnetFilteredByKeyword.Where(pro =>
            pro.strCategory.ToLower().Contains(strKeyword_I.ToLower()) ||
            pro.strProductName.ToLower().Contains(strKeyword_I.ToLower())).ToList();

            return darrprodjsonFromWisnetFilteredByKeyword;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static List<ProdjsonProductJson> darrprodtypjson2FilterProductsByKeyword(
            //                                              //Apply keyword filter to a list of private products.

            List<ProdjsonProductJson> darrprodjsonFromWisnet_I,
            String strKeyword_I
            )
        {
            List<ProdjsonProductJson> darrprodjsonFromWisnetFilteredPublicByKeyword =
                darrprodjsonFromWisnet_I;

            darrprodjsonFromWisnetFilteredPublicByKeyword = darrprodjsonFromWisnetFilteredPublicByKeyword.Where(pro =>
                pro.strCategory.ToLower().Contains(strKeyword_I.ToLower()) ||
                pro.strProductName.ToLower().Contains(strKeyword_I.ToLower())).ToList();

            return darrprodjsonFromWisnetFilteredPublicByKeyword;
        }

        //--------------------------------------------------------------------------------------------------------------
        public List<Prodtypjson2ProductTypeJson2> darrprodtypjson2GetGuidedProducts(
            //                                              //Guided products.

            String strCategory_I,
            String strKeyword_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            List<Prodtypjson2ProductTypeJson2> darrprodtypjson2 = new List<Prodtypjson2ProductTypeJson2>();

            //                                              //Get data from Wisnet.
            String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                GetSection("Odyssey2Settings")["urlWisnetApi"];
            Task<List<ProdjsonProductJson>> Task_darrprodjsonFromWisnet = HttpTools<ProdjsonProductJson>.
                    GetListAsyncToEndPoint(strUrlWisnet + "/PrintShopData/guidedProducts/" +
                    this.strPrintshopId + "/" + strCategory_I);
            Task_darrprodjsonFromWisnet.Wait();

            if (
                //                                          //There is access to the service of Wisnet.
                Task_darrprodjsonFromWisnet.Result != null
                )
            {
                List<ProdjsonProductJson> darrprodjsonFromWisnet = Task_darrprodjsonFromWisnet.Result;
                if (
                    //                                      //It was not information found in Wisnet DB.
                    darrprodjsonFromWisnet.Count() == 1 && darrprodjsonFromWisnet[0].intProductKey == -1
                    )
                {
                    //                                      //Nothing to do.
                }
                else
                {
                    if (
                        strKeyword_I != null
                        )
                    {
                        darrprodjsonFromWisnet = PsPrintShop.darrprodtypjson2FilterProductsByKeyword(
                            darrprodjsonFromWisnet, strKeyword_I);
                    }

                    darrprodtypjson2 = this.darrprodtypjson2UpdateProducts(darrprodjsonFromWisnet);
                }

                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "Success.";
            }
            return darrprodtypjson2;
        }


        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private List<Prodtypjson2ProductTypeJson2> darrprodtypjson2UpdateProducts(

            //                                              //List of ProdjsonProductJson.
            List<ProdjsonProductJson> darrprodjsonFromWisnet_I
            )
        {
            List<Prodtypjson2ProductTypeJson2> darrprodtypjson2 = new List<Prodtypjson2ProductTypeJson2>();

            Odyssey2Context context = new Odyssey2Context();


            List<EtentityElementTypeEntityDB> darretentity = context.ElementType.Where(et =>
                et.intPrintshopPk == this.intPk && et.intWebsiteProductKey != null).ToList();
            String strWebsiteUrl = this.strUrl;

            foreach (ProdjsonProductJson prodjson in darrprodjsonFromWisnet_I)
            {
                EtentityElementTypeEntityDB etentity = darretentity.FirstOrDefault(et =>
                et.intWebsiteProductKey == prodjson.intProductKey && et.strCustomTypeId == prodjson.strProductName);

                String strUrl = "http://" + strWebsiteUrl + "/docLib/" + prodjson.intProductKey;

                if (
                    etentity != null
                    )
                {
                    //                                  //Update the boolnPublic and the category.
                    etentity.boolnIsPublic = prodjson.boolIsPublic;
                    etentity.strCategory = prodjson.strCategory;
                    context.ElementType.Update(etentity);
                    context.SaveChanges();

                    Prodtypjson2ProductTypeJson2 prodtypjson2 = new Prodtypjson2ProductTypeJson2(etentity.intPk,
                        etentity.strCustomTypeId, etentity.strXJDFTypeId, etentity.strCategory, etentity.intnPkAccount, 
                        strUrl);
                    darrprodtypjson2.Add(prodtypjson2);
                }
                else
                {
                    //                                  //Create the entity.
                    etentity = new EtentityElementTypeEntityDB
                    {
                        strXJDFTypeId = EtElementTypeAbstract.strNotXJDF,
                        strAddedBy = this.strPrintshopId,
                        intPrintshopPk = this.intPk,
                        strCustomTypeId = prodjson.strProductName,
                        strCategory = prodjson.strCategory,
                        strResOrPro = EtElementTypeAbstract.strProduct,
                        intWebsiteProductKey = prodjson.intProductKey,
                        boolnIsPublic = prodjson.boolIsPublic
                    };
                    context.ElementType.Add(etentity);
                    context.SaveChanges();

                    Prodtypjson2ProductTypeJson2 prodtypjson2 = new Prodtypjson2ProductTypeJson2(etentity.intPk,
                        etentity.strCustomTypeId, etentity.strXJDFTypeId, etentity.strCategory, null, strUrl);
                    darrprodtypjson2.Add(prodtypjson2);
                }
            }
            return darrprodtypjson2;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private List<Prodtypjson2ProductTypeJson2> darrprodtypjson2UpdatePrivateProducts(

            //                                              //List of ProdprijsonProducPrivatetJson.
            List<ProdprijsonProducPrivatetJson> darrprodjsonFromWisnet_I,
            PsPrintShop ps_I
            )
        {
            List<Prodtypjson2ProductTypeJson2> darrprodtypjson2 = new List<Prodtypjson2ProductTypeJson2>();

            Odyssey2Context context = new Odyssey2Context();

            List<EtentityElementTypeEntityDB> darretentity = context.ElementType.Where(et =>
                et.intPrintshopPk == this.intPk && et.boolnIsPublic == false &&
                et.intWebsiteProductKey != null).ToList();

            //                                              //To easy code.
            String strWebsiteUrl = ps_I.strUrl;

            foreach (ProdprijsonProducPrivatetJson prodjson in darrprodjsonFromWisnet_I)
            {
                EtentityElementTypeEntityDB etentity = darretentity.FirstOrDefault(et =>
                et.intWebsiteProductKey == prodjson.intProductKey &&
                et.strCustomTypeId == prodjson.strProductName);

                //                                          //Create order's form link.
                String strOrderUrl = "http://" + strWebsiteUrl + "/docLib/" + prodjson.intProductKey;

                if (
                    etentity != null
                    )
                {
                    //                                  //Update the boolnPublic and the category.
                    etentity.boolnIsPublic = prodjson.boolIsPublic;
                    etentity.strCategory = prodjson.strCategory;
                    context.ElementType.Update(etentity);
                    context.SaveChanges();

                    Prodtypjson2ProductTypeJson2 prodtypjson2 = new Prodtypjson2ProductTypeJson2(etentity.intPk,
                        etentity.strCustomTypeId, etentity.strXJDFTypeId, etentity.strCategory, etentity.intnPkAccount,
                        strOrderUrl);
                    darrprodtypjson2.Add(prodtypjson2);
                }
                else
                {
                    //                                  //Create the entity.
                    etentity = new EtentityElementTypeEntityDB
                    {
                        strXJDFTypeId = EtElementTypeAbstract.strNotXJDF,
                        strAddedBy = this.strPrintshopId,
                        intPrintshopPk = this.intPk,
                        strCustomTypeId = prodjson.strProductName,
                        strCategory = prodjson.strCategory,
                        strResOrPro = EtElementTypeAbstract.strProduct,
                        intWebsiteProductKey = prodjson.intProductKey,
                        boolnIsPublic = prodjson.boolIsPublic
                    };
                    context.ElementType.Add(etentity);
                    context.SaveChanges();

                    Prodtypjson2ProductTypeJson2 prodtypjson2 = new Prodtypjson2ProductTypeJson2(etentity.intPk,
                        etentity.strCustomTypeId, etentity.strXJDFTypeId, etentity.strCategory, null, strOrderUrl);
                    darrprodtypjson2.Add(prodtypjson2);
                }
            }
            return darrprodtypjson2;
        }

        //--------------------------------------------------------------------------------------------------------------
        private void subGetProcessTypeFromDB(
            //                                              //Get the process types associated with this.

            out Dictionary<int, ProtypProcessType> dicprotyp_O
            )
        {
            //                                              //Get the connection.
            Odyssey2Context context = new Odyssey2Context();

            IQueryable<EtentityElementTypeEntityDB> setetentity = context.ElementType.Where(etentity =>
                etentity.intPrintshopPk == this.intPk && etentity.boolDeleted == false &&
                etentity.strResOrPro == EtElementTypeAbstract.strProcess);

            //                                              //Initialize the dic.
            dicprotyp_O = new Dictionary<int, ProtypProcessType>();
            foreach (EtentityElementTypeEntityDB etentity in setetentity)
            {
                //                                          //Create the process and add it to the dic.
                ProtypProcessType protyp = new ProtypProcessType(etentity.intPk, etentity.strXJDFTypeId,
                    etentity.strAddedBy, etentity.intPrintshopPk, etentity.strCustomTypeId,
                    etentity.strClassification);
                dicprotyp_O.Add(protyp.intPk, protyp);
            }
        }

        //-------------------------------------------------------------------------------------------------------------- 
        private void subGetResourceTypeFromDB(
            //                                              //To get data from the database and fill the dic.

            //                                              //Dic to be fill.
            out Dictionary<String, RestypResourceType> dicrestyp_O
            )
        {
            //                                              //Get all the types from database.
            Odyssey2Context context = new Odyssey2Context();
            IQueryable<EtentityElementTypeEntityDB> setetentity = context.ElementType.Where(etentity =>
                etentity.strResOrPro == EtElementTypeAbstract.strResource &&
                etentity.intPrintshopPk == this.intPk &&
                etentity.boolDeleted == false);

            //                                              //Empty the dic.
            dicrestyp_O = new Dictionary<String, RestypResourceType>();

            //                                              //Create and add all the types to the dic.
            foreach (EtentityElementTypeEntityDB etentity in setetentity)
            {
                RestypResourceType restem = new RestypResourceType(etentity.intPk, etentity.strXJDFTypeId,
                    etentity.strAddedBy, etentity.intPrintshopPk, etentity.strCustomTypeId,
                    etentity.strClassification);
                dicrestyp_O.Add(etentity.strCustomTypeId, restem);
            }
        }

        //-------------------------------------------------------------------------------------------------------------- 
        private void subGetResourceFromDB(
            //                                              //To get data from the database and fill the dic.

            //                                              //Dic to be fill.
            out Dictionary<int, ResResource> dicres_O
            )
        {
            dicres_O = new Dictionary<int, ResResource>();

            List<RestypResourceType> darrrestem = new List<RestypResourceType>();
            foreach (KeyValuePair<String, RestypResourceType> restem in this.dicrestyp)
            {
                darrrestem.Add(restem.Value);
            }
            //                                              //Get all the types from database.
            Odyssey2Context context = new Odyssey2Context();
            foreach (RestypResourceType restem in darrrestem)
            {
                IQueryable<EleentityElementEntityDB> seteeleentity = context.Element.Where(eleentity =>
                    eleentity.intPkElementType == restem.intPk);
                foreach (EleentityElementEntityDB eleentity in seteeleentity.ToArray())
                {
                    ResResource resInherited = ResResource.resFromDB(eleentity.intnPkElementInherited, false);

                    ResResource res = new ResResource(eleentity.intPk, eleentity.strElementName,
                        eleentity.boolIsTemplate, restem, resInherited, eleentity.boolnIsCalendar,
                        eleentity.boolnIsAvailable, eleentity.boolnCalendarIsChangeable,
                        eleentity.intnPkElementCalendarInherited != null);
                    dicres_O.Add(eleentity.intPk, res);
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        private void subGetResourceCalculationsFromDB(
            //                                              //Get all cal for this ps from db.

            //                                              //Dic where the cal will be saved.
            out Dictionary<int, CalCalculation> diccalResource_O
            )
        {
            //                                              //Initialize the diccal.
            diccalResource_O = new Dictionary<int, CalCalculation>();

            //                                              //Create the context.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get resource calculations for this.
            List<CalentityCalculationEntityDB> darrcalentity = (
                from ps in context.Printshop
                join et in context.ElementType
                on ps.intPk equals et.intPrintshopPk
                join e in context.Element
                on et.intPk equals e.intPkElementType
                join cal in context.Calculation
                on e.intPk equals cal.intnPkResource
                where cal.intnPkProduct == null
                && cal.intnPkProcess == null
                && ps.strPrintshopId == this.strPrintshopId
                && cal.strByX == CalCalculation.strByResource
                select cal).ToList();

            foreach (CalentityCalculationEntityDB calentity in darrcalentity)
            {
                CalCalculation cal = new CalCalculation(calentity.intPk, 
                    calentity.strUnit, calentity.numnQuantity, calentity.numnCost,
                    calentity.intnHours, calentity.intnMinutes, calentity.intnSeconds, calentity.numnBlock,
                    calentity.boolIsEnable, calentity.strValue, calentity.strAscendants,
                    calentity.strDescription, calentity.numnProfit,
                    calentity.intnPkProduct, calentity.intnPkProcess,
                    calentity.intnPkResource, calentity.strCalculationType, calentity.strByX, calentity.strStartDate,
                    calentity.strStartTime, calentity.strEndDate, calentity.strEndTime, calentity.numnNeeded,
                    calentity.numnPerUnits, calentity.numnMin, null, null, calentity.intnPkWorkflow,
                    calentity.intnProcessInWorkflowId, calentity.intnPkElementElementType,
                    calentity.intnPkElementElement, null, null, null, null, calentity.boolnFromThickness,
                    calentity.boolnIsBlock, calentity.boolnByArea);
                diccalResource_O.Add(cal.intPk, cal);
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        private void subGetProcessCalculationsFromDB(
            //                                              //Get all cal for this ps from db.

            //                                              //Dic where the cal will be saved.
            out Dictionary<int, CalCalculation> diccalProcess_O
            )
        {
            //                                              //Initialize the diccal.
            diccalProcess_O = new Dictionary<int, CalCalculation>();

            //                                              //Create the context.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get process calculations for this.
            List<CalentityCalculationEntityDB> darrcalentity = (
                from ps in context.Printshop
                join protyp in context.ElementType
                on ps.intPk equals protyp.intPrintshopPk
                join pro in context.Element
                on protyp.intPk equals pro.intPkElementType
                join cal in context.Calculation
                on pro.intPk equals cal.intnPkProcess
                where cal.intnPkProduct == null
                && cal.intnPkResource == null
                && ps.strPrintshopId == this.strPrintshopId
                && cal.strByX == CalCalculation.strByProcess
                select cal).ToList();

            foreach (CalentityCalculationEntityDB calentity in darrcalentity)
            {
                CalCalculation cal = new CalCalculation(calentity.intPk,
                    calentity.strUnit, calentity.numnQuantity, calentity.numnCost,
                    calentity.intnHours, calentity.intnMinutes, calentity.intnSeconds,
                    calentity.numnBlock, calentity.boolIsEnable, calentity.strValue, calentity.strAscendants,
                    calentity.strDescription, calentity.numnProfit, 
                    calentity.intnPkProduct, calentity.intnPkProcess,
                    calentity.intnPkResource, calentity.strCalculationType, calentity.strByX, calentity.strStartDate,
                    calentity.strStartTime, calentity.strEndDate, calentity.strEndTime, calentity.numnNeeded,
                    calentity.numnPerUnits, calentity.numnMin, null, null, calentity.intnPkWorkflow,
                    calentity.intnProcessInWorkflowId, calentity.intnPkElementElementType,
                    calentity.intnPkElementElement, null, null, null, calentity.intnPkAccount,
                    calentity.boolnFromThickness, calentity.boolnIsBlock, calentity.boolnByArea);
                diccalProcess_O.Add(cal.intPk, cal);
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        private void subGetJobInProgressFromDB(
            //                                              //Get job in progress for this ps from db.

            //                                              //Dic where the cal will be saved.
            out Dictionary<int, JobJob> dicjobInProgress_O
            )
        {
            //                                              //Initialize the Job in progress.
            dicjobInProgress_O = new Dictionary<int, JobJob>();

            //                                              //Create the context.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get jobInProgress.
            List<JobentityJobEntityDB> darrjobentityJobInProgress = context.Job.Where(job =>
                job.intPkPrintshop == this.intPk && job.intStage == JobJob.intInProgressStage).ToList();

            foreach (JobentityJobEntityDB jobentityInProgress in darrjobentityJobInProgress)
            {
                JobJob jobInProgress = new JobJob(jobentityInProgress.intPk, jobentityInProgress.intJobID,
                    jobentityInProgress.intStage, jobentityInProgress.intPkPrintshop);

                dicjobInProgress_O.Add(jobInProgress.intPk, jobInProgress);
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        private void subGetJobCompletedFromDB(
            //                                              //Get all job completes for this ps from db.

            //                                              //Dic where the cal will be saved.
            out Dictionary<int, JobJob> dicjobCompleted_O
            )
        {
            //                                              //Initialize the job completed.
            dicjobCompleted_O = new Dictionary<int, JobJob>();

            //                                              //Create the context.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get job Completed.
            List<JobentityJobEntityDB> darrjobentityJobCompleted = context.Job.Where(job =>
                job.intPkPrintshop == this.intPk && job.intStage == JobJob.intCompletedStage).ToList();

            foreach (JobentityJobEntityDB jobentityCompleted in darrjobentityJobCompleted)
            {
                JobJob jobCompleted = new JobJob(jobentityCompleted.intPk, jobentityCompleted.intJobID,
                    jobentityCompleted.intStage, jobentityCompleted.intPkPrintshop);

                dicjobCompleted_O.Add(jobCompleted.intPk, jobCompleted);
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTOR.

        //--------------------------------------------------------------------------------------------------------------
        public PsPrintShop(
            //                                              //Set the instance variables.

            //                                              //    
            int intPK_I,
            //                                              //Receives the printshop_ID.
            String strPrintshopId_I,
            //                                              //Receives the name of the printshop.
            String strPrintshopName_I,
            //                                              //Receives the special password of the printshop.
            String strSpecialPassword_I,
            String strUrl_I,
            bool boolOffset_I,
            String strTimeZone_I
            )
        {
            this.intPk_Z = intPK_I;
            this.strPrintshopId_Z = strPrintshopId_I;
            this.strPrintshopName_Z = strPrintshopName_I;
            this.strSpecialPassword_Z = strSpecialPassword_I;
            this.strUrl_Z = strUrl_I;
            this.boolOffset_Z = boolOffset_I;
            this.strTimeZone = strTimeZone_I;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static PsPrintShop psGet(
           //                                               //Look for the ps in the DB, if not exist, look for it
           //                                               //       in the Wisnet DB and add to the Odyssey2 DB.

           //                                               //Printshop to get.
           String strPrintshopId_I
           )
        {
            PsPrintShop ps = null;

            //                                              //Establish connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Search for printshop in the database.
            PsentityPrintshopEntityDB psentity = context.Printshop.FirstOrDefault(
                psentity => psentity.strPrintshopId == strPrintshopId_I);

            if (
                //                                          //Printshop is not in the DB.
                psentity == null
                )
            {
                String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                    GetSection("Odyssey2Settings")["urlWisnetApi"];
                Task<PsjsonPrintshopJson> Task_psjsonFromWisnet = HttpTools<PsjsonPrintshopJson>.GetOneAsyncToEndPoint(
                    strUrlWisnet + "/PrintShopData/printshopData/" + strPrintshopId_I);
                Task_psjsonFromWisnet.Wait();
                if (
                    Task_psjsonFromWisnet.Result != null
                    )
                {
                    PsjsonPrintshopJson psjsonFromWisnet = Task_psjsonFromWisnet.Result;
                    if (
                        psjsonFromWisnet.strPrintshopId != "-1"
                        )
                    {
                        //                                  //Printshop´s id.
                        String strPrintshopId = psjsonFromWisnet.strPrintshopId;
                        //                                  //Special password.
                        String strSpecialPassword = PsPrintShop.strCreatePrintshopSpecialPassword(strPrintshopId);

                        //                                  //Add printshop to DB.
                        psentity = new PsentityPrintshopEntityDB
                        {
                            strPrintshopId = strPrintshopId,
                            strName = psjsonFromWisnet.strPrintshopName,
                            strUrl = psjsonFromWisnet.strUrl,
                            strSpecialPassword = strSpecialPassword,
                            boolOffsetNumber = false
                        };
                        context.Printshop.Add(psentity);
                        context.SaveChanges();

                        //                                  //Create printshop object.
                        ps = new PsPrintShop(psentity.intPk, psentity.strPrintshopId, psentity.strName,
                            psentity.strSpecialPassword, psentity.strUrl, psentity.boolOffsetNumber,
                            psentity.strTimeZone);
                        //                                  //Add default data (wf, processes, etc.)
                        ps.AddDefaultData();
                    }
                }
                else
                {
                    //                                      //Not in the DB, Wisnet api not available.
                }
            }
            else
            {
                ps = new PsPrintShop(psentity.intPk, psentity.strPrintshopId, psentity.strName,
                    psentity.strSpecialPassword, psentity.strUrl, psentity.boolOffsetNumber, psentity.strTimeZone);
            }

            return ps;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static String strCreatePrintshopSpecialPassword(

            String strPrintshopId_I
            )
        {
            //                                              //Create special password.
            String strSpecialPassword = "ps" + strPrintshopId_I;

            return strSpecialPassword;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static PsPrintShop psGet(
            //                                              //Find printshop in DB by its PK.

            //                                              //Printshop to get.
            int intPk_I,
            Odyssey2Context context_M
           )
        {
            PsPrintShop ps = null;

            //                                              //Find printshop.
            PsentityPrintshopEntityDB psentity = context_M.Printshop.FirstOrDefault(
                psentity => psentity.intPk == intPk_I);

            if (
                psentity != null
                )
            {
                ps = new PsPrintShop(psentity.intPk, psentity.strPrintshopId, psentity.strName,
                    psentity.strSpecialPassword, psentity.strUrl, psentity.boolOffsetNumber, psentity.strTimeZone);
            }

            return ps;
        }

        //--------------------------------------------------------------------------------------------------------------
        public int intGetExpensePkAccount(
            //                                              //Return pritnshop's Pk expense Account.
            Odyssey2Context context_M
           )
        {
            //                                              //Get pk of Expense account type.

            int intPkAccountTypeExpense = context_M.AccountType.FirstOrDefault(acctype =>
                acctype.strType == AccAccounting.strAccountTypeExpense).intPk;

            //                                              //Get pritnshops Generic Expense account.
            int intPkPrintshopExpenseAccount = context_M.Account.FirstOrDefault(acc =>
                acc.intPkPrintshop == this.intPk &&
                acc.intPkAccountType == intPkAccountTypeExpense &&
                acc.boolAvailable == true &&
                acc.boolGeneric == true
                ).intPk;

            return intPkPrintshopExpenseAccount;
        }

        //--------------------------------------------------------------------------------------------------------------
        public ProtypjsonProcessTypeJson[] arrprotypjsonGetXJDFProcess(
             ref int intStatus_IO,
             ref String strUserMessage_IO,
             ref String strDevMessage_IO
             )
        {
            intStatus_IO = 402;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "XJDF process type not found.";
            //                                              //Get the XJDF process types.
            Dictionary<int, ProtypProcessType> dicprotypXJDF = Odyssey2.dicprotyp;

            //                                              //Get the printshop process types.
            List<ProtypProcessType> darrprotypPrintshop = new List<ProtypProcessType>();
            foreach (KeyValuePair<int, ProtypProcessType> pro in this.dicprotyp)
            {
                darrprotypPrintshop.Add(pro.Value);
            }

            List<ProtypjsonProcessTypeJson> darrprotypjson = new List<ProtypjsonProcessTypeJson>();
            //                                              //Verify if the XJDF protyp is already in the printshop.
            foreach (KeyValuePair<int, ProtypProcessType> protyp in dicprotypXJDF)
            {
                bool boolHasIt = darrprotypPrintshop.Exists(pro => pro.strCustomTypeId == protyp.Value.strCustomTypeId);

                int intPk = protyp.Value.intPk;
                if (
                    boolHasIt
                    )
                {
                    intPk = darrprotypPrintshop.FirstOrDefault(pro =>
                    pro.strCustomTypeId == protyp.Value.strCustomTypeId).intPk;
                }

                String strTypeId = protyp.Value.strXJDFTypeId;
                String strClassification = protyp.Value.strClassification;
                bool boolCommon = strTypeId.IsInSet(ProtypProcessType.arrstrCommon);
                ProtypjsonProcessTypeJson protypjson = new ProtypjsonProcessTypeJson(intPk, strTypeId, boolHasIt,
                    strClassification, boolCommon);
                darrprotypjson.Add(protypjson);
            }

            ProtypjsonProcessTypeJson[] arrprotypjson = darrprotypjson.ToArray();
            Std.Sort(arrprotypjson);

            intStatus_IO = 200;
            strUserMessage_IO = "";
            strDevMessage_IO = "Success.";
            return arrprotypjson;
        }

        //--------------------------------------------------------------------------------------------------------------
        public EtjsonElementTypeJson[] arretjsonGetXJDFResources(
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            intStatus_IO = 402;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "XJDF resource not found.";
            //                                              //Get the XJDF resources.
            Dictionary<String, RestypResourceType> dicrestemXJDF = Odyssey2.dicrestem;
            //                                              //Array containing the processes found.

            //                                              //Get the printshop resources.
            List<RestypResourceType> darrrestemPrintshop = new List<RestypResourceType>();
            foreach (KeyValuePair<String, RestypResourceType> res in this.dicrestyp)
            {
                darrrestemPrintshop.Add(res.Value);
            }

            List<EtjsonElementTypeJson> darretjson = new List<EtjsonElementTypeJson>();
            //                                              //Verify if the XJDF resource is already in the printshop.
            foreach (KeyValuePair<String, RestypResourceType> restem in dicrestemXJDF)
            {
                EtjsonElementTypeJson etjson = new EtjsonElementTypeJson();
                etjson.boolHasIt = darrrestemPrintshop.Exists(process =>
                process.strCustomTypeId == restem.Value.strCustomTypeId);

                etjson.intPk = restem.Value.intPk;
                if (
                    etjson.boolHasIt
                    )
                {
                    etjson.intPk = darrrestemPrintshop.FirstOrDefault(process =>
                    process.strCustomTypeId == restem.Value.strCustomTypeId).intPk;
                }

                etjson.strTypeId = restem.Value.strXJDFTypeId;
                etjson.strClassification = restem.Value.strClassification;
                etjson.boolIsPhysical = RestypResourceType.boolIsPhysical(restem.Value.strClassification);
                darretjson.Add(etjson);
            }
            //                                              //Sort array for strTypeId.  
            darretjson.Sort((p, q) => string.Compare(p.strTypeId, q.strTypeId));
            EtjsonElementTypeJson[] arretjson = darretjson.ToArray();

            intStatus_IO = 200;
            strUserMessage_IO = "";
            strDevMessage_IO = "Success.";

            return arretjson;
        }

        //--------------------------------------------------------------------------------------------------------------
        public void subGetXJDFResourcesByProcess(
            int intPkProcessType_I,
            bool boolIsPhysical_I,
            out Protypjson5ProcessTypeJson5 rbpjson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            rbpjson_O = null;

            //                                              //Establish connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get the elementType.
            EtElementTypeAbstract et = EtElementTypeAbstract.etFromDB(intPkProcessType_I);

            intStatus_IO = 402;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Process not found.";
            if (
                (et != null) &&
                (et.strResOrPro == EtElementTypeAbstract.strProcess)
                )
            {
                //                                          //Process type object.
                ProtypProcessType protyp = (ProtypProcessType)et;
                if (
                    //                                      //Is a process from a printshop.
                    protyp.intPkPrintshop != null
                    )
                {
                    //                                      //Get the generic XJDF Process type.
                    EtentityElementTypeEntityDB etentityProcessXJDF = context.ElementType.FirstOrDefault(et =>
                        et.intPrintshopPk == null &&
                        et.strAddedBy == EtElementTypeAbstract.strXJDFVersion &&
                        et.strXJDFTypeId == protyp.strXJDFTypeId);

                    if (
                        //                                  //Is a XJDF process.
                        etentityProcessXJDF != null
                        )
                    {
                        protyp = (ProtypProcessType)EtElementTypeAbstract.etFromDB(etentityProcessXJDF.intPk);
                    }
                    else
                    {
                        protyp = null;
                    }
                }

                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Process not XJDF.";
                if (
                    //                                      //The XJDF process is not null.
                    protyp != null
                    )
                {
                    //                                      //Get resources XJDF of a process XJDF.
                    IQueryable<EtetentityElementTypeElementTypeEntityDB> setetetentity =
                        context.ElementTypeElementType.Where(etetentity =>
                        etetentity.intPkElementTypeDad == protyp.intPk);
                    List<EtetentityElementTypeElementTypeEntityDB> darretet = setetetentity.ToList();

                    //                                      //Get the printshop resources.
                    List<RestypResourceType> darrrestypPrintshop = new List<RestypResourceType>();
                    foreach (KeyValuePair<String, RestypResourceType> res in this.dicrestyp)
                    {
                        darrrestypPrintshop.Add(res.Value);
                    }

                    List<Restypjson2ResourceTypeJson2> darrrestypjson2 = new List<Restypjson2ResourceTypeJson2>();

                    foreach (EtetentityElementTypeElementTypeEntityDB etet in darretet)
                    {
                        //                                  //Find the resource XJDF.
                        EtentityElementTypeEntityDB etResource = context.ElementType.FirstOrDefault(et =>
                            et.intPk == etet.intPkElementTypeSon);

                        if (
                            etResource != null
                            )
                        {
                            //                                  //Valid if the resource exists as a Prinstshop resource.
                            bool boolHasIt = darrrestypPrintshop.Exists(resource =>
                                resource.strCustomTypeId == et.strCustomTypeId);

                            int intPkResource = etet.intPkElementTypeSon;
                            if (
                                boolHasIt
                                )
                            {
                                //                              //Get the pk of the printshop type.
                                intPkResource = darrrestypPrintshop.FirstOrDefault(process =>
                                    process.strCustomTypeId == et.strCustomTypeId).intPk;
                            }

                            if (
                                RestypResourceType.boolIsPhysical(etResource.strClassification) == boolIsPhysical_I
                                )
                            {
                                Restypjson2ResourceTypeJson2 restypjson2 = new Restypjson2ResourceTypeJson2(intPkResource,
                                    etResource.strXJDFTypeId, boolHasIt, etResource.strClassification, etet.boolnUsage,
                                    RestypResourceType.boolIsPhysical(etResource.strClassification));

                                darrrestypjson2.Add(restypjson2);
                            }
                        }
                    }

                    rbpjson_O = new Protypjson5ProcessTypeJson5(protyp.intPk, protyp.strXJDFTypeId,
                        darrrestypjson2.ToArray());

                    intStatus_IO = 200;
                    strUserMessage_IO = "Success.";
                    strDevMessage_IO = "";
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public void proelejson2GetProcessWithTypesAndTemplates(

            //                                              //Pk of the process.
            int intProcessPk_I,
            out Proelejson2ProcessElementJson2 proelejson2_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            proelejson2_O = null;

            //                                              //Get the process.
            ProProcess pro = ProProcess.proFromDB(intProcessPk_I);

            intStatus_IO = 402;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Product not found.";
            if (
                pro != null
                )
            {
                //                                          //Input and output list of restyportem to be filled.
                List<Restyportemjson1ResourceTypeOrTemplateJson1> darrrestyportemInput = new
                    List<Restyportemjson1ResourceTypeOrTemplateJson1>();
                List<Restyportemjson1ResourceTypeOrTemplateJson1> darrrestyportemOutput = new
                    List<Restyportemjson1ResourceTypeOrTemplateJson1>();

                Odyssey2Context context = new Odyssey2Context();

                //                                          //Searchs for the restyp for this process.
                IQueryable<EleetentityElementElementTypeEntityDB> seteleetentity = context.ElementElementType.Where(
                    eleetentity => eleetentity.intPkElementDad == pro.intPk);
                List<EleetentityElementElementTypeEntityDB> darreleetentity = seteleetentity.ToList();

                //                                          //Add restyp to the restyportem list.
                foreach (EleetentityElementElementTypeEntityDB eleetentity in darreleetentity)
                {
                    EtentityElementTypeEntityDB etentityResourceType = context.ElementType.FirstOrDefault(
                        etentity => etentity.intPk == eleetentity.intPkElementTypeSon);

                    String strName = (etentityResourceType.strXJDFTypeId == "None") ?
                        etentityResourceType.strCustomTypeId : etentityResourceType.strXJDFTypeId;
                    Restyportemjson1ResourceTypeOrTemplateJson1 restyportemjson1 = new
                        Restyportemjson1ResourceTypeOrTemplateJson1(eleetentity.intPk, strName, true);

                    if (
                        eleetentity.boolUsage
                        )
                    {
                        darrrestyportemInput.Add(restyportemjson1);
                    }
                    else
                    {
                        darrrestyportemOutput.Add(restyportemjson1);
                    }
                }

                //                                          //Search for the tem for this process.
                IQueryable<EleeleentityElementElementEntityDB> seteleeentity = context.ElementElement.Where(
                    eleeentity => eleeentity.intPkElementDad == pro.intPk);
                List<EleeleentityElementElementEntityDB> darreteleentity = seteleeentity.ToList();

                //                                          //Add tem to the restyportem list.
                foreach (EleeleentityElementElementEntityDB eteleentity in darreteleentity)
                {
                    EleentityElementEntityDB eleentityTemplate = context.Element.FirstOrDefault(eleentity =>
                        eleentity.intPk == eteleentity.intPkElementSon &&
                        eleentity.boolIsTemplate);

                    Restyportemjson1ResourceTypeOrTemplateJson1 restyportemjson1 = new
                        Restyportemjson1ResourceTypeOrTemplateJson1(eteleentity.intPk, eleentityTemplate.strElementName,
                        false);

                    if (
                        eteleentity.boolUsage
                        )
                    {
                        darrrestyportemInput.Add(restyportemjson1);
                    }
                    else
                    {
                        darrrestyportemOutput.Add(restyportemjson1);
                    }
                }

                //                                          //Create the Json of the process.
                proelejson2_O = new Proelejson2ProcessElementJson2(pro.intPk, pro.strName,
                    darrrestyportemInput.ToArray(), darrrestyportemOutput.ToArray());

                intStatus_IO = 200;
                strUserMessage_IO = "Success.";
                strDevMessage_IO = "";
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public TyptemjsonTypeTemplateJson[] GetPrintshopTypesOrTemplates(
            int intPkProcessType_I,
            //                                              //booleans for filter.
            bool boolIsType_I,
            bool boolIsTemplate_I,
            bool boolIsPhysical_I,
            bool boolIsNotPhysical_I,
            bool boolIsSuggested_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            //                                              //List containing all resources and templates.
            List<TyptemjsonTypeTemplateJson> darrtyptemjson = new List<TyptemjsonTypeTemplateJson>();

            //                                              //Method to filter suggested.
            TyptemjsonTypeTemplateJson[] arrtyptemjson = arrtyptemjsonFiltered(intPkProcessType_I, boolIsSuggested_I,
                boolIsPhysical_I, boolIsNotPhysical_I);

            //                                              //Get each type or template.
            foreach (TyptemjsonTypeTemplateJson typtempjson in arrtyptemjson)
            {
                //                                          //Filter for type physical.
                if (
                    (boolIsPhysical_I &&
                    //                                      //It is Physical.
                    (
                    RestypResourceType.boolIsPhysical(typtempjson.strClassification)
                    )) ||
                    (boolIsNotPhysical_I &&
                    //
                    (                                       //It is Not Physical.
                        typtempjson.strClassification == RestypResourceType.strResourceTypeParameter
                    )
                    ))
                {
                    if (
                        boolIsType_I
                        )
                    {
                        TyptemjsonTypeTemplateJson typtemjson =
                                new TyptemjsonTypeTemplateJson();

                        //                                  //Add usage before of the nameCustomType.
                        if (boolIsSuggested_I)
                        {
                            typtemjson.strTypeId = typtempjson.strUsage + " - " + typtempjson.strTypeId;
                        }
                        else
                        {
                            typtemjson.strTypeId = typtempjson.strTypeId;
                        }
                        typtemjson.intPk = typtempjson.intPk;
                        typtemjson.strUsage = typtempjson.strUsage;
                        typtemjson.boolIsType = true;
                        //                                  //Add type.
                        if (
                            darrtyptemjson.Count(t => t.intPk == typtemjson.intPk &&
                            t.strUsage == typtemjson.strUsage) == 0
                            )
                        {
                            darrtyptemjson.Add(typtemjson);
                        }
                    }
                    if (
                        boolIsTemplate_I
                        )
                    {
                        //                                  //Get the entity whith same customType but of the printshop.
                        EtentityElementTypeEntityDB etentity = context.ElementType.FirstOrDefault(
                            et => et.strXJDFTypeId == typtempjson.strTypeId && et.intPrintshopPk == this.intPk);

                        if (
                            etentity != null
                            )
                        {
                            //                              //Get all Template.
                            IQueryable<EleentityElementEntityDB> seteleentityTemp = context.Element.Where(
                                ele => ele.intPkElementType == etentity.intPk && ele.boolIsTemplate == true);
                            List<EleentityElementEntityDB> darreleentityTemp = seteleentityTemp.ToList();

                            //                              //take each template.
                            foreach (EleentityElementEntityDB eleTemplate in darreleentityTemp)
                            {
                                TyptemjsonTypeTemplateJson typtemjson =
                                new TyptemjsonTypeTemplateJson();

                                typtemjson.intPk = eleTemplate.intPk;
                                //                          //Add usage before of the nameCustomType.
                                if (boolIsSuggested_I)
                                {
                                    typtemjson.strTypeId = typtempjson.strUsage + " - " + eleTemplate.strElementName;
                                }
                                else
                                {
                                    typtemjson.strTypeId = eleTemplate.strElementName;
                                }

                                typtemjson.strUsage = typtempjson.strUsage;
                                typtemjson.boolIsType = false;
                                //                          //Add type not physical.
                                if (
                                    darrtyptemjson.Count(t => t.intPk == typtemjson.intPk &&
                                        t.strUsage == typtemjson.strUsage) == 0
                                    )
                                {
                                    darrtyptemjson.Add(typtemjson);
                                }
                            }
                        }
                    }
                }
            }

            darrtyptemjson = darrtyptemjson.OrderBy(typtem => typtem.strTypeId).ToList();

            intStatus_IO = 200;
            strUserMessage_IO = "Success.";
            strDevMessage_IO = "";

            return darrtyptemjson.ToArray();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private TyptemjsonTypeTemplateJson[] arrtyptemjsonFiltered(
            //                                              //Pk of process
            int intPkProcessType_I,
            //                                              //Booleans by choose the list of type.
            bool boolIsSuggested_I,
            bool boolIsPhysical_I,
            bool boolIsNotPhysical_I
            )
        {
            //                                              //Establish connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //List that will hold types or templates.
            List<TyptemjsonTypeTemplateJson> darrtyptemjson = new List<TyptemjsonTypeTemplateJson>();

            if (
                //                                          //Physical and params.
                intPkProcessType_I == -1
                )
            {
                darrtyptemjson = darrtyptemjsonAllTypePhysicalXJDFAndNotPhysicalXJDF();
            }
            else if (
                //                                          //Not suggested.
                !boolIsSuggested_I
                )
            {
                //                                          //If the process exist, return 
                //                                          //      all template and type. 

                //                                          //Get the process.
                EtentityElementTypeEntityDB etentityProcess = context.ElementType.FirstOrDefault(etentity =>
                    etentity.intPk == intPkProcessType_I && etentity.strResOrPro == EtElementTypeAbstract.strProcess);

                if (
                    etentityProcess != null
                    )
                {
                    //                                      //Get all types Physical XJDF
                    //                                      //    and all params XJDF(Not Physical).
                    darrtyptemjson = darrtyptemjsonAllTypePhysicalXJDFAndNotPhysicalXJDF();
                }
            }
            else
            {
                //                                          //It is Sugested.

                //                                          //Get the process.
                EtentityElementTypeEntityDB etentityProcess = context.ElementType.FirstOrDefault(etentity =>
                    etentity.intPk == intPkProcessType_I && etentity.strResOrPro == EtElementTypeAbstract.strProcess);

                if (
                    etentityProcess != null
                    )
                {
                    //                                      //Get type suggested for this process.
                    darrtyptemjson = arrtyptemjsonInputOrOutputSugessted(etentityProcess, boolIsPhysical_I,
                        boolIsNotPhysical_I);
                }
            }

            return darrtyptemjson.ToArray();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private List<TyptemjsonTypeTemplateJson> darrtyptemjsonAllTypePhysicalXJDFAndNotPhysicalXJDF()
        {
            //                                              //Establish connection.
            Odyssey2Context context = new Odyssey2Context();

            List<TyptemjsonTypeTemplateJson> darrtyptemjson = new List<TyptemjsonTypeTemplateJson>();

            //                                              //GetTypeParamsXJDF(Not Physical) 
            //                                              //    and TypeXJDF Physical
            IQueryable<EtentityElementTypeEntityDB> setetentityXJDPhysicalAndNotPhysical =
                context.ElementType.Where(etentity => etentity.strResOrPro == EtElementTypeAbstract.strResource &&
                etentity.strAddedBy == EtElementTypeAbstract.strXJDFVersion && etentity.intPrintshopPk == null &&
                etentity.strCustomTypeId != "XJDFResourceSet");

            List<EtentityElementTypeEntityDB> darretentity = setetentityXJDPhysicalAndNotPhysical.ToList();

            //                                              //Add all types params for return.
            foreach (EtentityElementTypeEntityDB etentity in darretentity)
            {
                TyptemjsonTypeTemplateJson typetemjson = new TyptemjsonTypeTemplateJson();

                typetemjson.intPk = etentity.intPk;
                typetemjson.strTypeId = etentity.strXJDFTypeId;
                typetemjson.strClassification = etentity.strClassification;
                typetemjson.strUsage = null;
                darrtyptemjson.Add(typetemjson);
            }
            return darrtyptemjson;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private List<TyptemjsonTypeTemplateJson> arrtyptemjsonInputOrOutputSugessted(
            //                                              //Entity of a process.
            EtentityElementTypeEntityDB etentityProcess_I,
            //                                              //Booleans used for the choose the query.
            bool boolIsPhysical_I,
            bool boolIsNotPhysical_I
            )
        {
            Odyssey2Context context = new Odyssey2Context();
            List<EtentityElementTypeEntityDB> darretentity = new List<EtentityElementTypeEntityDB>();

            /*CASE*/
            if (
                boolIsPhysical_I &&
                boolIsNotPhysical_I
                )
            {
                //                                          //Get all the types Physical and 
                //                                          //    get type params XJDF(Not Physical).
                IQueryable<EtentityElementTypeEntityDB> setetentity = context.ElementType.Where(etentity =>
                    (
                    (
                    //                                      //condition for get all the types physical XJDF.
                    etentity.strResOrPro == EtElementTypeAbstract.strResource &&
                    etentity.intPrintshopPk == null &&
                    (etentity.strClassification == RestypResourceType.strResourceTypeConsumable ||
                    etentity.strClassification == RestypResourceType.strResourceTypeHandling ||
                    etentity.strClassification == RestypResourceType.strResourceTypeImplementation ||
                    etentity.strClassification == RestypResourceType.strResourceTypeQuantity
                    )
                     )
                    ) ||
                    (
                    //                                      //Condition for GetTypeParamsXJDF(Not Physical).
                    etentity.strResOrPro == EtElementTypeAbstract.strResource &&
                    etentity.strAddedBy == EtElementTypeAbstract.strXJDFVersion &&
                    etentity.intPrintshopPk == null && etentity.strClassification == RestypResourceType.strResourceTypeParameter
                    ));
                darretentity = setetentity.ToList();
            }
            else if (
                boolIsPhysical_I
                )
            {
                IQueryable<EtentityElementTypeEntityDB> setetentity = context.ElementType.Where(etentity =>
                //                                      //condition for get all the types physical XJDF.
                    etentity.strResOrPro == EtElementTypeAbstract.strResource &&
                    etentity.intPrintshopPk == null &&
                    (etentity.strClassification == RestypResourceType.strResourceTypeConsumable ||
                    etentity.strClassification == RestypResourceType.strResourceTypeHandling ||
                    etentity.strClassification == RestypResourceType.strResourceTypeImplementation ||
                    etentity.strClassification == RestypResourceType.strResourceTypeQuantity)
                );
                darretentity = setetentity.ToList();
            }
            else if (
                boolIsNotPhysical_I
                )
            {
                //                                          //GetTypeParamsXJDF
                IQueryable<EtentityElementTypeEntityDB> setetentity = context.ElementType.Where(etentity =>
                etentity.strResOrPro == EtElementTypeAbstract.strResource &&
                etentity.strAddedBy == EtElementTypeAbstract.strXJDFVersion &&
                etentity.intPrintshopPk == null && etentity.strClassification == RestypResourceType.strResourceTypeParameter);
                darretentity = setetentity.ToList();
            }

            List<EtentityElementTypeEntityDB> darretentityXJDFInputResources =
                        PsPrintShop.arretentityXJDFTypeResources(etentityProcess_I, true);

            List<EtentityElementTypeEntityDB> darretentityXJDFOutputResources =
                PsPrintShop.arretentityXJDFTypeResources(etentityProcess_I, false);

            List<TyptemjsonTypeTemplateJson> darrrestyptem =
                new List<TyptemjsonTypeTemplateJson>();

            //                                              //Check type of Input.
            if (
                darretentityXJDFInputResources.FirstOrDefault(res =>
                res.strCustomTypeId == "XJDFResourceSet") != null
                )
            {
                foreach (EtentityElementTypeEntityDB et in darretentity)
                {
                    if (
                        et.strCustomTypeId != "XJDFResourceSet"
                        )
                    {
                        TyptemjsonTypeTemplateJson restypetemjson =
                            new TyptemjsonTypeTemplateJson();

                        restypetemjson.intPk = et.intPk;
                        restypetemjson.strTypeId = et.strXJDFTypeId;
                        restypetemjson.strClassification = et.strClassification;
                        restypetemjson.strUsage = ProtypProcessType.strInput;
                        darrrestyptem.Add(restypetemjson);
                    }
                }
            }
            else
            {
                foreach (EtentityElementTypeEntityDB et in darretentity)
                {
                    foreach (EtentityElementTypeEntityDB etentity in darretentityXJDFInputResources)
                    {
                        if (
                            et.strAddedBy == etentity.strAddedBy &&
                            et.strCustomTypeId == etentity.strCustomTypeId &&
                            et.strXJDFTypeId == etentity.strXJDFTypeId
                            )
                        {
                            TyptemjsonTypeTemplateJson restypetemjson =
                            new TyptemjsonTypeTemplateJson();

                            restypetemjson.intPk = et.intPk;
                            restypetemjson.strTypeId = et.strXJDFTypeId;
                            restypetemjson.strClassification = et.strClassification;
                            restypetemjson.strUsage = ProtypProcessType.strInput;
                            darrrestyptem.Add(restypetemjson);
                        }
                    }
                }
            }
            //                                      //Check type of Output.
            if (
                darretentityXJDFOutputResources.FirstOrDefault(res =>
                res.strCustomTypeId == "XJDFResourceSet") != null
                )
            {
                foreach (EtentityElementTypeEntityDB et in darretentity)
                {
                    if (
                        et.strCustomTypeId != "XJDFResourceSet"
                        )
                    {
                        TyptemjsonTypeTemplateJson restypetemjson =
                             new TyptemjsonTypeTemplateJson();

                        restypetemjson.intPk = et.intPk;
                        restypetemjson.strTypeId = et.strXJDFTypeId;
                        restypetemjson.strClassification = et.strClassification;
                        restypetemjson.strUsage = ProtypProcessType.strOutput;
                        darrrestyptem.Add(restypetemjson);
                    }
                }
            }
            else
            {
                foreach (EtentityElementTypeEntityDB et in darretentity)
                {
                    foreach (EtentityElementTypeEntityDB etentity in darretentityXJDFOutputResources)
                    {
                        if (
                            et.strAddedBy == etentity.strAddedBy &&
                            et.strCustomTypeId == etentity.strCustomTypeId &&
                            et.strXJDFTypeId == etentity.strXJDFTypeId
                            )
                        {
                            TyptemjsonTypeTemplateJson restypetemjson =
                             new TyptemjsonTypeTemplateJson();

                            restypetemjson.intPk = et.intPk;
                            restypetemjson.strTypeId = et.strXJDFTypeId;
                            restypetemjson.strClassification = et.strClassification;
                            restypetemjson.strUsage = ProtypProcessType.strOutput;
                            darrrestyptem.Add(restypetemjson);
                        }
                    }
                }
            }
            return darrrestyptem;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static List<EtentityElementTypeEntityDB> arretentityXJDFTypeResources(
            EtentityElementTypeEntityDB etentityPrintshopProcess_I,
            bool boolIsInput_I
            )
        {
            Odyssey2Context context = new Odyssey2Context();
            //                                          //Get the XJDF process type.
            EtentityElementTypeEntityDB etentityProcessXJDF = context.ElementType.FirstOrDefault(etentity =>
                etentity.intPrintshopPk == null &&
                etentity.strAddedBy == EtElementTypeAbstract.strXJDFVersion &&
                etentity.strCustomTypeId == etentityPrintshopProcess_I.strCustomTypeId);

            //                                          //Get the relation with resources of the XJDF type.
            IQueryable<EtetentityElementTypeElementTypeEntityDB> setetetentity =
                        context.ElementTypeElementType.Where(etetentity =>
                        etetentity.intPkElementTypeDad == etentityProcessXJDF.intPk
                        && etetentity.boolnUsage == boolIsInput_I
                        );
            List<EtetentityElementTypeElementTypeEntityDB> darretetentity = setetetentity.ToList();

            List<EtentityElementTypeEntityDB> darretentityResources = new List<EtentityElementTypeEntityDB>();
            foreach (EtetentityElementTypeElementTypeEntityDB etetentity in darretetentity)
            {
                EtentityElementTypeEntityDB etentityResource = context.ElementType.FirstOrDefault(etentity =>
                    etentity.intPk == etetentity.intPkElementTypeSon &&
                    etentity.strResOrPro == EtElementTypeAbstract.strResource);
                if (
                    etentityResource != null
                    )
                {
                    darretentityResources.Add(etentityResource);
                }
            }
            return darretentityResources;
        }

        //--------------------------------------------------------------------------------------------------------------
        private void subGetProcessFromDB(
            //                                              //Get the process associated with this.

            out Dictionary<int, ProProcess> dicpro_O
            )
        {
            //                                              //Get the connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Look for processes and printshopPk.
            IQueryable<EtentityElementTypeEntityDB> setetentityProcessType = context.ElementType.Where(etentity =>
                etentity.strResOrPro == "Process" && etentity.intPrintshopPk == this.intPk);

            //                                              //Initialize the dic.
            dicpro_O = new Dictionary<int, ProProcess>();
            foreach (EtentityElementTypeEntityDB etentityProcessType in setetentityProcessType.ToList())
            {
                //                                          //Compare Pks in Element table.
                IQueryable<EleentityElementEntityDB> seteleentityProcess = context.Element.Where(eleentity =>
                 eleentity.intPkElementType == etentityProcessType.intPk && eleentity.boolDeleted == false);

                foreach (EleentityElementEntityDB eleProcess in seteleentityProcess.ToList())
                {
                    //                                      //Create the Process object.
                    ProProcess proProcess = new ProProcess(eleProcess.intPk, eleProcess.strElementName);
                    dicpro_O.Add(proProcess.intPk, proProcess);
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public void AddDefaultData(
            )
        {
            //                                              //This is to get data from Wisnet.
            Dictionary<int, ProdtypProductType> dicprodtyp = this.dicprodtyp;

            //----------------------------------------------------------------------------------------------------------
            //                                              //DEFAULT ACCOUNTS.
            //----------------------------------------------------------------------------------------------------------

            //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            Odyssey2Context context = new Odyssey2Context();
            int intStatus = 200;
            String strUserMessage = "";
            String strDevMessage = "";

            //                                              //Find account type pks.
            int intPkAccountTypeBank = context.AccountType.FirstOrDefault(acctype =>
                acctype.strType == AccAccounting.strAccountTypeBank).intPk;

            int intPkAccountTypeAsset = context.AccountType.FirstOrDefault(acctype =>
                acctype.strType == AccAccounting.strAccountTypeAsset).intPk;

            int intPkAccountTypeExpense = context.AccountType.FirstOrDefault(acctype =>
                acctype.strType == AccAccounting.strAccountTypeExpense).intPk;

            int intPkAccountTypeRevenue = context.AccountType.FirstOrDefault(acctype =>
                acctype.strType == AccAccounting.strAccountTypeRevenue).intPk;

            int intPkAccountTypeLiability = context.AccountType.FirstOrDefault(acctype =>
                acctype.strType == AccAccounting.strAccountTypeLiability).intPk;

            //                                              //Add default Bank accounts.
            AccAccounting.subAddAccount(AccAccounting.strCashInBackAccountNumber, AccAccounting.strCashInBackAccountName,
                intPkAccountTypeBank, true, this, context, ref intStatus, ref strUserMessage, ref strDevMessage);

            //                                              //Add default Asset accounts.
            AccAccounting.subAddAccount(AccAccounting.strAccountsReceivableNumber, AccAccounting.strAccountsReceivableName,
                intPkAccountTypeAsset, true, this, context, ref intStatus, ref strUserMessage, ref strDevMessage);

            AccAccounting.subAddAccount(AccAccounting.strUndepositedFundsNumber, AccAccounting.strUndepositedFundsName,
                intPkAccountTypeAsset, true, this, context, ref intStatus, ref strUserMessage, ref strDevMessage);

            //                                              //Add default Liability accounts.
            AccAccounting.subAddAccount(AccAccounting.strAccountsPayableNumber, AccAccounting.strAccountsPayableName,
                intPkAccountTypeLiability, true, this, context, ref intStatus, ref strUserMessage, ref strDevMessage);

            AccAccounting.subAddAccount(AccAccounting.strSalesTaxPayableNumber, AccAccounting.strSalesTaxPayableName,
                intPkAccountTypeLiability, true, this, context, ref intStatus, ref strUserMessage, ref strDevMessage);

            //                                              //Add default Revenue accounts.
            AccAccounting.subAddAccount(AccAccounting.strGeneralSalesNumber, AccAccounting.strGeneralSalesName,
                intPkAccountTypeRevenue, false, this, context, ref intStatus, ref strUserMessage, ref strDevMessage);

            AccAccounting.subAddAccount(AccAccounting.strDigitalSalesNumber, AccAccounting.strDigitalSalesName,
                intPkAccountTypeRevenue, false, this, context, ref intStatus, ref strUserMessage, ref strDevMessage);

            AccAccounting.subAddAccount(AccAccounting.strOffsetPrintingSalesNumber, AccAccounting.strOffsetPrintingSalesName,
                intPkAccountTypeRevenue, false, this, context, ref intStatus, ref strUserMessage, ref strDevMessage);

            AccAccounting.subAddAccount(AccAccounting.strFreightNumber, AccAccounting.strFreightName,
                intPkAccountTypeRevenue, false, this, context, ref intStatus, ref strUserMessage, ref strDevMessage);

            //                                              //Important! Only one generic Revenue account.
            AccAccounting.subAddAccount(AccAccounting.strUncategorizedRevenueNumber, AccAccounting.strUncategorizedRevenueName,
                intPkAccountTypeRevenue, true, this, context, ref intStatus, ref strUserMessage, ref strDevMessage);

            //                                              //Add default Expense accounts.
            AccAccounting.subAddAccount(AccAccounting.strPaperNumber, AccAccounting.strPaperName,
                intPkAccountTypeExpense, false, this, context, ref intStatus, ref strUserMessage, ref strDevMessage);

            AccAccounting.subAddAccount(AccAccounting.strEnvelopesNumber, AccAccounting.strEnvelopesName,
                intPkAccountTypeExpense, false, this, context, ref intStatus, ref strUserMessage, ref strDevMessage);

            AccAccounting.subAddAccount(AccAccounting.strPlatesNumber, AccAccounting.strPlatesName,
                intPkAccountTypeExpense, false, this, context, ref intStatus, ref strUserMessage, ref strDevMessage);

            AccAccounting.subAddAccount(AccAccounting.strInkNumber, AccAccounting.strInkName,
                intPkAccountTypeExpense, false, this, context, ref intStatus, ref strUserMessage, ref strDevMessage);

            //                                              //Important! Only one generic Expense account.
            AccAccounting.subAddAccount(AccAccounting.strUncategorizedExpenseNumber, AccAccounting.strUncategorizedExpenseName,
                intPkAccountTypeExpense, true, this, context, ref intStatus, ref strUserMessage, ref strDevMessage);

            //----------------------------------------------------------------------------------------------------------
            //                                              //TEMPLATES.
            //----------------------------------------------------------------------------------------------------------

            //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            //                                              //Process Ink.

            List<Attrjson5AttributeJson5> darrjsonAttribute0 = new List<Attrjson5AttributeJson5>();

            PsPrintShop.subCreateAttribute("XJDFInk", "XJDFInkType", "Ink", null, true, ref darrjsonAttribute0, context);
            int intPkTemplateProcessInk = this.intAddTemplate("Process Ink", "XJDFInk", "g", false,
                ref darrjsonAttribute0, null, context);

            //----------------------------------------------------------------------------------------------------------
            //                                              //RESOURCES.
            //----------------------------------------------------------------------------------------------------------

            //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            //                                              //XJDFMedia.
            //                                              //Generic Offset Paper 19x25.

            List<Attrjson5AttributeJson5> darrjsonAttribute1 = new List<Attrjson5AttributeJson5>();

            PsPrintShop.subCreateAttribute("XJDFMedia", "XJDFMediaType", "Paper", null, true, ref darrjsonAttribute1, context);
            int intPkResourceGenericOffsetPaper19x25 = this.intAddResource("Generic Offset Paper 19x25", "XJDFMedia",
                "Cm", false, ref darrjsonAttribute1, null, null, context);

            //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            //                                              //XJDFComponent.
            //                                              //Unprinted Press Sheet.

            List<Attrjson5AttributeJson5> darrjsonAttribute2 = new List<Attrjson5AttributeJson5>();

            PsPrintShop.subCreateAttribute("XJDFComponent", "XJDFDescriptiveName", "Generic Name", null, true,
                ref darrjsonAttribute2, context);
            int intPkResourceUnprintedPressSheet = this.intAddResource("Unprinted Press Sheet", "XJDFComponent", "gsm",
                false, ref darrjsonAttribute2, null, null, context);

            //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            //                                              //XJDFComponent.            
            //                                              //Printed Press Sheet.

            List<Attrjson5AttributeJson5> darrjsonAttribute3 = new List<Attrjson5AttributeJson5>();

            PsPrintShop.subCreateAttribute("XJDFComponent", "XJDFDescriptiveName", "Generic Name", null, true,
                ref darrjsonAttribute3, context);
            int intPkResourcePrintedPressSheet = this.intAddResource("Printed Press Sheet", "XJDFComponent", "gsm",
                false, ref darrjsonAttribute3, null, null, context);

            //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            //                                              //XJDFComponent.            
            //                                              //Trimmed Press Sheet.

            List<Attrjson5AttributeJson5> darrjsonAttribute4 = new List<Attrjson5AttributeJson5>();

            PsPrintShop.subCreateAttribute("XJDFComponent", "XJDFDescriptiveName", "Generic Name", null, true,
                ref darrjsonAttribute4, context);
            int intPkResourceTrimmedPressSheet = this.intAddResource("Trimmed Press Sheet", "XJDFComponent", "gsm",
                false, ref darrjsonAttribute4, null, null, context);

            //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            //                                              //XJDFComponent.           
            //                                              //Finished Product.

            List<Attrjson5AttributeJson5> darrjsonAttribute5 = new List<Attrjson5AttributeJson5>();

            PsPrintShop.subCreateAttribute("XJDFComponent", "XJDFDescriptiveName", "Generic Name", null, true,
                ref darrjsonAttribute5, context);
            int intPkResourceFinishedProduct = this.intAddResource("Finished Product", "XJDFComponent", "gsm", false,
                ref darrjsonAttribute5, null, null, context);

            //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            //                                              //XJDFInk.
            //                                              //Process Ink - C.

            List<Attrjson5AttributeJson5> darrjsonAttribute6 = new List<Attrjson5AttributeJson5>();

            PsPrintShop.subCreateAttribute("XJDFInk", "XJDFInkType", "Ink", darrjsonAttribute0[0].intnPkValue, true,
                ref darrjsonAttribute6, context);
            PsPrintShop.subCreateAttribute("XJDFInk", "XJDFDescriptiveName", "Cyan", null, true,
                ref darrjsonAttribute6, context);
            int intPkResourceProcessInkC = this.intAddResource("Process Ink - C", "XJDFInk", "g", false,
                ref darrjsonAttribute6, intPkTemplateProcessInk, true, context);

            //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            //                                              //XJDFInk.
            //                                              //Process Ink - M.

            List<Attrjson5AttributeJson5> darrjsonAttribute7 = new List<Attrjson5AttributeJson5>();

            PsPrintShop.subCreateAttribute("XJDFInk", "XJDFInkType", "Ink", darrjsonAttribute0[0].intnPkValue, true,
                ref darrjsonAttribute7, context);
            PsPrintShop.subCreateAttribute("XJDFInk", "XJDFDescriptiveName", "Magenta", null, true,
                ref darrjsonAttribute7, context);
            int intPkResourceProcessInkM = this.intAddResource("Process Ink - M", "XJDFInk", "g", false,
                ref darrjsonAttribute7, intPkTemplateProcessInk, true, context);

            //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            //                                              //XJDFInk.
            //                                              //Process Ink - Y.

            List<Attrjson5AttributeJson5> darrjsonAttribute8 = new List<Attrjson5AttributeJson5>();

            PsPrintShop.subCreateAttribute("XJDFInk", "XJDFInkType", "Ink", darrjsonAttribute0[0].intnPkValue, true,
                ref darrjsonAttribute8, context);
            PsPrintShop.subCreateAttribute("XJDFInk", "XJDFDescriptiveName", "Yellow", null, true,
                ref darrjsonAttribute8, context);
            int intPkResourceProcessInkY = this.intAddResource("Process Ink - Y", "XJDFInk", "g", false,
                ref darrjsonAttribute8, intPkTemplateProcessInk, true, context);

            //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            //                                              //XJDFInk.
            //                                              //Process Ink - K.

            List<Attrjson5AttributeJson5> darrjsonAttribute9 = new List<Attrjson5AttributeJson5>();

            PsPrintShop.subCreateAttribute("XJDFInk", "XJDFInkType", "Ink", darrjsonAttribute0[0].intnPkValue, true,
                ref darrjsonAttribute9, context);
            PsPrintShop.subCreateAttribute("XJDFInk", "XJDFDescriptiveName", "Black", null, true,
                ref darrjsonAttribute9, context);
            int intPkResourceProcessInkK = this.intAddResource("Process Ink - K", "XJDFInk", "g", false,
                ref darrjsonAttribute9, intPkTemplateProcessInk, true, context);

            //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            //                                              //XJDFDevice.
            //                                              //Generic Offset Press.

            List<Attrjson5AttributeJson5> darrjsonAttribute10 = new List<Attrjson5AttributeJson5>();

            PsPrintShop.subCreateAttribute("XJDFDevice", "XJDFDeviceClass", "SheetFedConventionalPress", null, true,
                ref darrjsonAttribute10, context);
            int intPkResourceGenericOffsetPress = this.intAddResource("Generic Offset Press", "XJDFDevice", "Sheets",
                false, ref darrjsonAttribute10, null, null, context);

            //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            //                                              //XJDFDevice.
            //                                              //Generic Digital Press.

            List<Attrjson5AttributeJson5> darrjsonAttribute11 = new List<Attrjson5AttributeJson5>();

            PsPrintShop.subCreateAttribute("XJDFDevice", "XJDFDeviceClass", "SheetFedDigitalPrinter", null, true,
                ref darrjsonAttribute11, context);
            int intPkResourceGenericDigitalPress = this.intAddResource("Generic Digital Press", "XJDFDevice", "Sheets",
                false, ref darrjsonAttribute11, null, null, context);

            //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            //                                              //XJDFDevice.
            //                                              //Generic Folder.

            List<Attrjson5AttributeJson5> darrjsonAttribute12 = new List<Attrjson5AttributeJson5>();

            PsPrintShop.subCreateAttribute("XJDFDevice", "XJDFDeviceClass", "Folder", null, true,
                ref darrjsonAttribute12, context);
            int intPkResourceGenericFolder = this.intAddResource("Generic Folder", "XJDFDevice", "Sheets", false,
                ref darrjsonAttribute12, null, null, context);

            //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            //                                              //XJDFDevice.
            //                                              //Generic Cutter.

            List<Attrjson5AttributeJson5> darrjsonAttribute13 = new List<Attrjson5AttributeJson5>();

            PsPrintShop.subCreateAttribute("XJDFDevice", "XJDFDeviceClass", "Cutter", null, true,
                ref darrjsonAttribute13, context);
            int intPkResourceGenericCutter = this.intAddResource("Generic Cutter", "XJDFDevice", "Sheets", false,
                ref darrjsonAttribute13, null, null, context);

            //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 


            //----------------------------------------------------------------------------------------------------------
            //                                              //PROCESSES.
            //----------------------------------------------------------------------------------------------------------

            //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            //                                              //XJDFCutting.
            //                                              //Cut Parent to Run Size.

            int intPkProcessCutParenttoRunSize = this.intAddProcess(
            //   NAME                      TYPE
                "Cut Parent to Run Size", "XJDFCutting");

            //                                              //IOs
            int intPkEleetCutParenttoRunSizeI1 = this.intAddTypeToProcess(intPkProcessCutParenttoRunSize,
            //   TYPE OF      INPUT    
                "XJDFMedia", "Input");

            int intPkEleetCutParenttoRunSizeO1 = this.intAddTypeToProcess(intPkProcessCutParenttoRunSize,
            //   TYPE OF          OUTPUT
                "XJDFComponent", "Output");

            int intPkEleetCutParenttoRunSizeI2 = this.intAddTypeToProcess(intPkProcessCutParenttoRunSize,
            //   TYPE OF          INPUT
                "XJDFDevice", "Input");

            //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            //                                              //XJDFConventionalPrinting.
            //                                              //Sheetfed Offset Printing.

            int intPkProcessSheetfedOffsetPrinting = this.intAddProcess(
            //   NAME                        TYPE
                "Sheetfed Offset Printing", "XJDFConventionalPrinting");

            //                                              //IOs
            int intPkEleetSheetfedOffsetPrintingI1 = this.intAddTypeToProcess(intPkProcessSheetfedOffsetPrinting,
            //   TYPE OF          INPUT
                "XJDFComponent", "Input");

            int intPkEleetSheetfedOffsetPrintingO1 = this.intAddTypeToProcess(intPkProcessSheetfedOffsetPrinting,
            //   TYPE OF          OUTPUT
                "XJDFComponent", "Output");

            int intPkEleetSheetfedOffsetPrintingI2 = this.intAddTypeToProcess(intPkProcessSheetfedOffsetPrinting,
            //   TYPE OF          INPUT
                "XJDFDevice", "Input");


            int intPkEleeleSheetfedOffsetPrintingI3 = this.intAddTemplateToProcess(intPkProcessSheetfedOffsetPrinting,
            //  TEMPLATE                  INPUT
                intPkTemplateProcessInk, "Input");

            int intPkEleeleSheetfedOffsetPrintingI4 = this.intAddTemplateToProcess(intPkProcessSheetfedOffsetPrinting,
            //  TEMPLATE                  INPUT
                intPkTemplateProcessInk, "Input");

            int intPkEleeleSheetfedOffsetPrintingI5 = this.intAddTemplateToProcess(intPkProcessSheetfedOffsetPrinting,
            //  TEMPLATE                  INPUT
                intPkTemplateProcessInk, "Input");

            int intPkEleeleSheetfedOffsetPrintingI6 = this.intAddTemplateToProcess(intPkProcessSheetfedOffsetPrinting,
            //  TEMPLATE                  INPUT
                intPkTemplateProcessInk, "Input");

            //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            //                                              //XJDFCutting.
            //                                              //Trim Press Sheet to Finished Size.

            int intPkProcessTrimPressSheettoFinishedSize = this.intAddProcess(
            //   NAME                                 TYPE
                "Trim Press Sheet to Finished Size", "XJDFCutting");

            //                                              //IOs
            int intPkEleetTrimPressSheettoFinishedSizeI1 = this.intAddTypeToProcess(
                //                                         TYPE OF          INPUT
                intPkProcessTrimPressSheettoFinishedSize, "XJDFComponent", "Input");

            int intPkEleetTrimPressSheettoFinishedSizeO1 = this.intAddTypeToProcess(
                //                                         TYPE OF          OUTPUT
                intPkProcessTrimPressSheettoFinishedSize, "XJDFComponent", "Output");

            int intPkEleetTrimPressSheettoFinishedSizeI2 = this.intAddTypeToProcess(
                //                                         TYPE OF          INPUT
                intPkProcessTrimPressSheettoFinishedSize, "XJDFDevice", "Input");

            //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            //                                              //XJDFFolding.                                    
            //                                              //Half Fold.

            int intPkProcessHalfFold = this.intAddProcess(
            //   NAME         TYPE
                "Half Fold", "XJDFFolding");

            //                                              //IOs
            int intPkEleetHalfFoldI1 = this.intAddTypeToProcess(intPkProcessHalfFold,
            //   TYPE             INPUT
                "XJDFComponent", "Input");

            int intPkEleetHalfFoldO1 = this.intAddTypeToProcess(intPkProcessHalfFold,
            //   TYPE             OUTPUT
                "XJDFComponent", "Output");

            int intPkEleetHalfFoldI2 = this.intAddTypeToProcess(intPkProcessHalfFold,
            //   TYPE OF       INPUT
                "XJDFDevice", "Input");

            //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            //                                              //XJDFDigitalPrinting.
            //                                              //Digital Printing.

            int intPkProcessDigitalPrinting = this.intAddProcess(
            //   NAME                     TYPE
                "Digital Printing", "XJDFDigitalPrinting");

            int intPkEleetDigitalPrintingI1 = this.intAddTypeToProcess(intPkProcessDigitalPrinting,
            //   TYPE             INPUT
                "XJDFComponent", "Input");

            int intPkEleetDigitalPrintingI2 = this.intAddTypeToProcess(intPkProcessDigitalPrinting,
            //   TYPE             INPUT
                "XJDFDevice", "Input");

            int intPkEleetDigitalPrintingO1 = this.intAddTypeToProcess(intPkProcessDigitalPrinting,
            //   TYPE             OUTPUT
                "XJDFComponent", "Output");

            //----------------------------------------------------------------------------------------------------------
            //                                              //WORKFLOW Brochures-Offset
            //----------------------------------------------------------------------------------------------------------

            //                                              //Create workflow.
            int intPkWfBrochuresOffset = this.intCreateWorkflow("Brochures", "Brochures-Offset", 1, false);

            if (
                intPkWfBrochuresOffset > 0
                )
            {
                //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
                //                                          //Add Processes to the workflow.

                int intPkPiwBrochuresOffset_CutParenttoRunSize = this.subAddProcessToWorkflow(
                    //  WORKFLOW                PROCESS                
                    this, intPkWfBrochuresOffset, intPkProcessCutParenttoRunSize);
                int intPkPiwBrochuresOffset_SheetfedOffsetPrinting = this.subAddProcessToWorkflow(
                    //  WORKFLOW                PROCESS                
                    this, intPkWfBrochuresOffset, intPkProcessSheetfedOffsetPrinting);
                int intPkPiwBrochuresOffset_TrimPressSheettoFinishedSize = this.subAddProcessToWorkflow(
                    //  WORKFLOW                PROCESS                
                    this, intPkWfBrochuresOffset, intPkProcessTrimPressSheettoFinishedSize);
                int intPkPiwBrochuresOffset_HalfFold = this.subAddProcessToWorkflow(
                    //  WORKFLOW                PROCESS                
                    this, intPkWfBrochuresOffset, intPkProcessHalfFold);

                //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
                //                                              //Set Resources.

                //                                              //Set Resources to Cut Parent to Run Size.
                this.subSetResourceInWorkflow(this, intPkPiwBrochuresOffset_CutParenttoRunSize,
                    //   ELEET-INPUT1                            RESOURCE           ELEET
                    intPkEleetCutParenttoRunSizeI1, intPkResourceGenericOffsetPaper19x25, true);

                this.subSetResourceInWorkflow(this, intPkPiwBrochuresOffset_CutParenttoRunSize,
                    //   ELEET-OUTPUT1                           RESOURCE           ELEET
                    intPkEleetCutParenttoRunSizeO1, intPkResourceUnprintedPressSheet, true);

                this.subSetResourceInWorkflow(this, intPkPiwBrochuresOffset_CutParenttoRunSize,
                    //   ELEET-INPUT2                           RESOURCE           ELEET
                    intPkEleetCutParenttoRunSizeI2, intPkResourceGenericCutter, true);

                //                                              //Set Resources to Sheetfed Offset Printing.
                this.subSetResourceInWorkflow(this, intPkPiwBrochuresOffset_SheetfedOffsetPrinting,
                    //  ELEET-INPUT1                        RESOURCE                          ELEET
                    intPkEleetSheetfedOffsetPrintingI1, intPkResourceUnprintedPressSheet, true);

                this.subSetResourceInWorkflow(this, intPkPiwBrochuresOffset_SheetfedOffsetPrinting,
                    //  ELEET-INPUT2                        RESOURCE                         ELEET
                    intPkEleetSheetfedOffsetPrintingI2, intPkResourceGenericOffsetPress, true);

                this.subSetResourceInWorkflow(this, intPkPiwBrochuresOffset_SheetfedOffsetPrinting,
                    //  ELEELE-INPUT3                        RESOURCE                  ELEET
                    intPkEleeleSheetfedOffsetPrintingI3, intPkResourceProcessInkC, false);

                this.subSetResourceInWorkflow(this, intPkPiwBrochuresOffset_SheetfedOffsetPrinting,
                    //  ELEELE-INPUT4                        RESOURCE                  ELEET
                    intPkEleeleSheetfedOffsetPrintingI4, intPkResourceProcessInkM, false);

                this.subSetResourceInWorkflow(this, intPkPiwBrochuresOffset_SheetfedOffsetPrinting,
                    //  ELEELE-INPUT5                        RESOURCE                  ELEET
                    intPkEleeleSheetfedOffsetPrintingI5, intPkResourceProcessInkY, false);

                this.subSetResourceInWorkflow(this, intPkPiwBrochuresOffset_SheetfedOffsetPrinting,
                    //  ELEELE-INPUT6                        RESOURCE                  ELEET
                    intPkEleeleSheetfedOffsetPrintingI6, intPkResourceProcessInkK, false);

                this.subSetResourceInWorkflow(this, intPkPiwBrochuresOffset_SheetfedOffsetPrinting,
                    //  ELEET-OUTPUT1                       RESOURCE                        ELEET
                    intPkEleetSheetfedOffsetPrintingO1, intPkResourcePrintedPressSheet, true);

                //                                              //Set Resources to Trim Press Sheet to Finished Size.
                this.subSetResourceInWorkflow(this, intPkPiwBrochuresOffset_TrimPressSheettoFinishedSize,
                    //  ELEET-INPUT1                              RESOURCE                        ELEET
                    intPkEleetTrimPressSheettoFinishedSizeI1, intPkResourcePrintedPressSheet, true);

                this.subSetResourceInWorkflow(this, intPkPiwBrochuresOffset_TrimPressSheettoFinishedSize,
                    //  ELEET-OUTPUT1                             RESOURCE                        ELEET
                    intPkEleetTrimPressSheettoFinishedSizeO1, intPkResourceTrimmedPressSheet, true);

                this.subSetResourceInWorkflow(this, intPkPiwBrochuresOffset_TrimPressSheettoFinishedSize,
                     //   ELEET-INPUT2                           RESOURCE           ELEET
                     intPkEleetTrimPressSheettoFinishedSizeI2, intPkResourceGenericCutter, true);

                //                                              //Set Resources to Half Fold.
                this.subSetResourceInWorkflow(this, intPkPiwBrochuresOffset_HalfFold,
                    //  ELEET-INPUT1          RESOURCE                        ELEET
                    intPkEleetHalfFoldI1, intPkResourceTrimmedPressSheet, true);

                this.subSetResourceInWorkflow(this, intPkPiwBrochuresOffset_HalfFold,
                    //  ELEET-OUTPUT1         RESOURCE                      ELEET
                    intPkEleetHalfFoldO1, intPkResourceFinishedProduct, true);

                this.subSetResourceInWorkflow(this, intPkPiwBrochuresOffset_HalfFold,
                    //  ELEET-INPUT2         RESOURCE                      ELEET
                    intPkEleetHalfFoldI2, intPkResourceGenericFolder, true);

                //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
                //                                              //Set Links.
                this.subAddLink(this,
                    //  PROCESS                                     OUTPUT-1
                    intPkPiwBrochuresOffset_CutParenttoRunSize, intPkEleetCutParenttoRunSizeO1, true,
                    //  PROCESS                                         INPUT-1
                    intPkPiwBrochuresOffset_SheetfedOffsetPrinting, intPkEleetSheetfedOffsetPrintingI1, true);

                this.subAddLink(this,
                    //  PROCESS                                         OUTPUT-1
                    intPkPiwBrochuresOffset_SheetfedOffsetPrinting, intPkEleetSheetfedOffsetPrintingO1, true,
                    //                       PROCESS                          INPUT-1
                    intPkPiwBrochuresOffset_TrimPressSheettoFinishedSize, intPkEleetTrimPressSheettoFinishedSizeI1, true);

                this.subAddLink(this,
                    //  PROCESS                                               OUTPUT-1
                    intPkPiwBrochuresOffset_TrimPressSheettoFinishedSize, intPkEleetTrimPressSheettoFinishedSizeO1, true,
                    //  PROCESS                           INPUT-1
                    intPkPiwBrochuresOffset_HalfFold, intPkEleetHalfFoldI1, true);

                //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            }

            //----------------------------------------------------------------------------------------------------------
            //                                              //WORKFLOW Create Brochures-Digital
            //----------------------------------------------------------------------------------------------------------

            //                                              //Create workflow.
            int intPkWfBrochuresDigital = this.intCreateWorkflow("Brochures", "Brochures-Digital", 2, true);
            if (
                intPkWfBrochuresDigital > 0
                )
            {

                //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
                //                                              //Add Processes to the workflow.

                int intPkPiwBrochuresDigital_CutParenttoRunSize = this.subAddProcessToWorkflow(
                //  WORKFLOW                 PROCESS                
                this, intPkWfBrochuresDigital, intPkProcessCutParenttoRunSize);

                int intPkPiwBrochuresDigital_DigitalPrinting = this.subAddProcessToWorkflow(
                    //  WORKFLOW                 PROCESS  
                    this, intPkWfBrochuresDigital, intPkProcessDigitalPrinting);

                int intPkPiwBrochuresDigital_TrimPressSheeettoFinishedSize = this.subAddProcessToWorkflow(
                    //  WORKFLOW                 PROCESS  
                    this, intPkWfBrochuresDigital, intPkProcessTrimPressSheettoFinishedSize);

                int intPkPiwBrochuresDigital_HalfFold = this.subAddProcessToWorkflow(
                   //  WORKFLOW                 PROCESS  
                   this, intPkWfBrochuresDigital, intPkProcessHalfFold);

                //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
                //                                              //Set Resources.

                //                                              //Set Resources to Piw Cut Parent to Run Size.
                this.subSetResourceInWorkflow(this, intPkPiwBrochuresDigital_CutParenttoRunSize,
                    //  ELEET-INPUT1                    RESOURCE                               ELEET
                    intPkEleetCutParenttoRunSizeI1, intPkResourceGenericOffsetPaper19x25, true);

                this.subSetResourceInWorkflow(this, intPkPiwBrochuresDigital_CutParenttoRunSize,
                     //   ELEET-OUTPUT1                   RESOURCE                          ELEET
                     intPkEleetCutParenttoRunSizeO1, intPkResourceUnprintedPressSheet, true);

                this.subSetResourceInWorkflow(this, intPkPiwBrochuresDigital_CutParenttoRunSize,
                    //   ELEET-INPUT2                   RESOURCE                          ELEET
                    intPkEleetCutParenttoRunSizeI2, intPkResourceGenericCutter, true);

                //                                              //Set Resources to Piw Digital Printing.
                this.subSetResourceInWorkflow(this, intPkPiwBrochuresDigital_DigitalPrinting,
                    //  ELEET-INPUT1              RESOURCE                          ELEET
                    intPkEleetDigitalPrintingI1, intPkResourceUnprintedPressSheet, true);

                this.subSetResourceInWorkflow(this, intPkPiwBrochuresDigital_DigitalPrinting,
                    //  ELEET-INPUT2              RESOURCE                          ELEET
                    intPkEleetDigitalPrintingI2, intPkResourceGenericDigitalPress, true);

                this.subSetResourceInWorkflow(this, intPkPiwBrochuresDigital_DigitalPrinting,
                    //  ELEET-OUTPUT1             RESOURCE                        ELEET
                    intPkEleetDigitalPrintingO1, intPkResourcePrintedPressSheet, true);


                //                                              //Set Resources to Piw Trim Press Sheet to Finished Size.
                this.subSetResourceInWorkflow(this, intPkPiwBrochuresDigital_TrimPressSheeettoFinishedSize,
                    //  ELEET-INPUT1                              RESOURCE                        ELEET
                    intPkEleetTrimPressSheettoFinishedSizeI1, intPkResourcePrintedPressSheet, true);

                this.subSetResourceInWorkflow(this, intPkPiwBrochuresDigital_TrimPressSheeettoFinishedSize,
                    //  ELEET-OUTPUT1                             RESOURCE           ELEET
                    intPkEleetTrimPressSheettoFinishedSizeO1, intPkResourceTrimmedPressSheet, true);

                this.subSetResourceInWorkflow(this, intPkPiwBrochuresDigital_TrimPressSheeettoFinishedSize,
                    //   ELEET-INPUT2                   RESOURCE                          ELEET
                    intPkEleetTrimPressSheettoFinishedSizeI2, intPkResourceGenericCutter, true);

                //                                              //Set Resources to Piw Half Fold.
                this.subSetResourceInWorkflow(this, intPkPiwBrochuresDigital_HalfFold,
                    //  ELEET-INPUT1          RESOURCE                        ELEET
                    intPkEleetHalfFoldI1, intPkResourceTrimmedPressSheet, true);

                this.subSetResourceInWorkflow(this, intPkPiwBrochuresDigital_HalfFold,
                    //  ELEET-OUTPUT1         RESOURCE                      ELEET
                    intPkEleetHalfFoldO1, intPkResourceFinishedProduct, true);

                this.subSetResourceInWorkflow(this, intPkPiwBrochuresDigital_HalfFold,
                    //  ELEET-INPUT2         RESOURCE                      ELEET
                    intPkEleetHalfFoldI2, intPkResourceGenericFolder, true);

                //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
                //                                              //Set Links.

                this.subAddLink(this,
                    //  PROCESS                                      OUTPUT-1
                    intPkPiwBrochuresDigital_CutParenttoRunSize, intPkEleetCutParenttoRunSizeO1, true,
                    //  PROCESS                                   INPUT-1
                    intPkPiwBrochuresDigital_DigitalPrinting, intPkEleetDigitalPrintingI1, true);

                this.subAddLink(this,
                    //  PROCESS                                   OUTPUT-1
                    intPkPiwBrochuresDigital_DigitalPrinting, intPkEleetDigitalPrintingO1, true,
                    //  PROCESS                                                 INPUT-1
                    intPkPiwBrochuresDigital_TrimPressSheeettoFinishedSize, intPkEleetTrimPressSheettoFinishedSizeI1, true);

                this.subAddLink(this,
                    //  PROCESS                                                 OUTPUT-1
                    intPkPiwBrochuresDigital_TrimPressSheeettoFinishedSize, intPkEleetTrimPressSheettoFinishedSizeO1, true,
                    //  PROCESS                            INPUT-1
                    intPkPiwBrochuresDigital_HalfFold, intPkEleetHalfFoldI1, true);

                //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            }

            //----------------------------------------------------------------------------------------------------------
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subCreateAttribute(
            String strType_I,
            String strCustomName_I,
            String strValue_I,
            int? intnInheritedValuePk_I,
            bool boolChangeable_I,
            ref List<Attrjson5AttributeJson5> darrjsonAttribute_M,
            Odyssey2Context context_M
            )
        {
            if (
                !string.IsNullOrEmpty(strValue_I)
                )
            {
                //                                              //Find type.
                EtentityElementTypeEntityDB etentityType = context_M.ElementType.FirstOrDefault(et =>
                    et.strCustomTypeId == strType_I && et.intPrintshopPk == null &&
                    et.strAddedBy == "XJDF2.0" && et.strResOrPro == EtElementTypeAbstract.strResource);

                //                                              //Attributes for this type, this name, no printshop.
                List<int> darrintPkAttributes = (
                    from attr in context_M.Attribute
                    join attret in context_M.AttributeElementType
                    on attr.intPk equals attret.intPkAttribute
                    where attret.intPkElementType == etentityType.intPk &&
                    attr.strCustomName == strCustomName_I &&
                    attr.strScope == "XJDF2.0"
                    select attr.intPk).ToList();

                if (
                    darrintPkAttributes.Count == 1
                    )
                {
                    //                                              //Get Ascendant´s Pk.
                    //                                              //It only will work to attributes without ascendants.
                    //                                              //If Dave would like some attribute with ascendants,
                    //                                              //      the strCustomName will be a arrstr with the ascendants
                    //                                              //      and the CustomName at the end.
                    String strAscendant = darrintPkAttributes[0].ToString();

                    //                                              //Create Json.
                    Attrjson5AttributeJson5 attjson5 = new Attrjson5AttributeJson5(strAscendant, strValue_I,
                        intnInheritedValuePk_I, boolChangeable_I, null);

                    darrjsonAttribute_M.Add(attjson5);
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public int intAddTemplate(
            String strName_I,
            String strTypeOfResource_I,
            String strUnit_I,
            bool boolIsDecimal,
            ref List<Attrjson5AttributeJson5> darrattrjson5_M,
            int? intnPkDad_I,
            Odyssey2Context context_M
            )

        {
            int intStatus = 200;
            String strUserMessage = "";
            String strDevMessage = "";

            //                                              //Find Type.
            EtentityElementTypeEntityDB etentity = context_M.ElementType.FirstOrDefault(et =>
                et.strCustomTypeId == strTypeOfResource_I && et.strAddedBy == "XJDF2.0" &&
                et.strResOrPro == EtElementTypeAbstract.strResource);

            bool boolIsTemplate = true;
            Pejson1PathElementJson1 pejson1;
            //                                              //ref to get the Pk of the values added.
            Attrjson5AttributeJson5[] arrattrjson5_IO = darrattrjson5_M.ToArray();
            //                                              //Add template.
            int intPkResource = ResResource.subAdd(strName_I, strUnit_I, boolIsDecimal,
                (RestypResourceType)EtElementTypeAbstract.etFromDB(context_M, etentity.intPk), this.strPrintshopId,
                boolIsTemplate, intnPkDad_I, null, null, null, null, null, null, null, null, null,
                strUnit_I, null, null, null, null, null, context_M, ref arrattrjson5_IO, ref intStatus,
                ref strUserMessage, ref strDevMessage, out pejson1);
            darrattrjson5_M = arrattrjson5_IO.ToList();

            return intPkResource;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public int intAddResource(

            String strName_I,
            String strTypeOfResource_I,
            String strUnit_I,
            bool boolIsDecimal_I,
            ref List<Attrjson5AttributeJson5> darrattrjson5_M,
            int? intnDadPk_I,
            bool? boolnCostInherited_I,
            Odyssey2Context context_M
            )
        {
            int intStatus = 200;
            String strUserMessage = "";
            String strDevMessage = "";

            //                                              //Find type.
            EtentityElementTypeEntityDB etentity = context_M.ElementType.FirstOrDefault(et =>
                et.strCustomTypeId == strTypeOfResource_I && et.intPrintshopPk == null &&
                et.strAddedBy == "XJDF2.0" && et.strResOrPro == EtElementTypeAbstract.strResource);

            bool? boolnCostInherited = null;
            bool? boolnCostChangeable = null;
            bool? boolnUnitInherited = null;
            bool? boolnUnitChangeable = null;
            bool? boolnAvailabilityInherited = null;
            bool? boolnAvailabilityChangeable = null;
            bool? boolnArea = null;

            if (
                intnDadPk_I != null
                )
            {
                boolnCostInherited = boolnCostInherited_I == null ? false : boolnCostInherited_I;
                boolnCostChangeable = boolnCostInherited_I == true ? false : true;
                boolnUnitInherited = true;
                boolnUnitChangeable = false;
                boolnAvailabilityInherited = true;
                boolnAvailabilityChangeable = false;
            }
            else
            {

            }

            bool boolIsTemplate = false;
            Pejson1PathElementJson1 pejson1;
            //                                              //ref to get the Pk of the values added.
            Attrjson5AttributeJson5[] arrattrjson5IO = darrattrjson5_M.ToArray();
            //                                              //Add resource.
            int intPkResource = ResResource.subAdd(strName_I, strUnit_I, boolIsDecimal_I,
                (RestypResourceType)EtElementTypeAbstract.etFromDB(etentity.intPk), this.strPrintshopId,
                boolIsTemplate, intnDadPk_I, null, null, null, null, null, null, boolnArea, boolnCostInherited,
                boolnCostChangeable, strUnit_I, boolnUnitInherited, boolnUnitChangeable, false, boolnAvailabilityInherited,
                boolnAvailabilityChangeable, context_M, ref arrattrjson5IO, ref intStatus, ref strUserMessage,
                ref strDevMessage, out pejson1);

            darrattrjson5_M = arrattrjson5IO.ToList();

            return intPkResource;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private int intAddProcess(
            String strProcessName_I,
            String StrTypeOfProcess_I
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            int intStatus = 200;
            String strUserMessage = "";
            String strDevMessage = "";

            EtentityElementTypeEntityDB etentity = context.ElementType.FirstOrDefault(et =>
                    et.strCustomTypeId == StrTypeOfProcess_I && et.intPrintshopPk == null &&
                    et.strAddedBy == "XJDF2.0");

            int intPkProcess =
                ProProcess.subAdd(strProcessName_I,
                (ProtypProcessType)EtElementTypeAbstract.etFromDB(etentity.intPk), this.strPrintshopId,
                ref intStatus, ref strUserMessage, ref strDevMessage);

            return intPkProcess;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private int intAddTypeToProcess(
            int intPkProcess_I,
            String StrTypeOfResource_I,
            String strInputOrOutput_I
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            int intStatus = 200;
            String strUserMessage = "";
            String strDevMessage = "";

            EtentityElementTypeEntityDB etentity = context.ElementType.FirstOrDefault(et =>
                et.strCustomTypeId == StrTypeOfResource_I && et.intPrintshopPk == this.intPk &&
                et.strAddedBy == "XJDF2.0" && et.strResOrPro == EtElementTypeAbstract.strResource);

            int intX;
            int intPkEleetOrEleele = ProtypProcessType.subAddResourceTypeOrTemplateToProcess(this.strPrintshopId,
                intPkProcess_I, etentity.intPk, null, strInputOrOutput_I, context, out intX, ref intStatus, ref
                strUserMessage, ref strDevMessage);

            return intPkEleetOrEleele;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private int intAddTemplateToProcess(
            int intPkProcess_I,
            int intPkTemplate_I,
            String strInputOrOutput_I
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            int intStatus = 200;
            String strUserMessage = "";
            String strDevMessage = "";

            int intX;
            int intPkEleetOrEleele = ProtypProcessType.subAddResourceTypeOrTemplateToProcess(this.strPrintshopId,
                intPkProcess_I, null, intPkTemplate_I, strInputOrOutput_I, context, out intX, ref intStatus, ref
                strUserMessage, ref strDevMessage);

            return intPkEleetOrEleele;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private int intCreateWorkflow(
            String strProduct_I,
            String strName_I,
            int intWorkflowId_I,
            bool boolDefault_I
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            int intPkwf = 0;

            //                                              //Find product.    
            EtentityElementTypeEntityDB eletentity = context.ElementType.FirstOrDefault(elet =>
                elet.strCustomTypeId == strProduct_I && elet.intPrintshopPk == this.intPk &&
                elet.strResOrPro == EtElementTypeAbstract.strProduct);

            if (
                eletentity != null
                )
            {
                //                                              //Create workflow.
                WfentityWorkflowEntityDB wfentityFirstWorkflow = new WfentityWorkflowEntityDB
                {
                    intnPkProduct = eletentity.intPk,
                    strName = strName_I,
                    intWorkflowId = intWorkflowId_I,
                    strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                    strStartTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                    intPkPrintshop = this.intPk,
                    boolDeleted = false,
                    boolDefault = boolDefault_I
                };
                context.Workflow.Add(wfentityFirstWorkflow);
                context.SaveChanges();                

                intPkwf = wfentityFirstWorkflow.intPk;

                //                                          //Add the default workflow history.
                if (
                    boolDefault_I
                    )
                {
                    //                                      //Create register into defaultWorkflowHistory table.
                    DefwfhisentityDefaultWorkflowHistoryEntityDB defwfhisentity =
                        new DefwfhisentityDefaultWorkflowHistoryEntityDB
                        {
                            intPkWorkflow = intPkwf,
                            strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                            strStartTime = Time.Now(ZonedTimeTools.timezone).ToString()
                        };
                    context.DefaultWorkflowHistory.Add(defwfhisentity);
                    context.SaveChanges();
                }
            }
            return intPkwf;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private int subAddProcessToWorkflow(
            PsPrintShop ps_I,
            int intPkWorkflow_I,
            int intPkProcess_I
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            int intStatus = 200;
            String strUserMessage = "";
            String strDevMessage = "";

            //                                              //Add process to workflow.
            int intPkWorkflowFinal;
            int intPkPiwAdded = ProdtypProductType.subAddProcess(intPkProcess_I, intPkWorkflow_I, ps_I,
                false, out intPkWorkflowFinal, ref intStatus, ref strUserMessage, ref strDevMessage);

            return intPkPiwAdded;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private void subSetResourceInWorkflow(
            PsPrintShop ps_I,
            int intPkProcessInWorkflow_I,
            int intPkEleetOrEleele_I,
            int intPkResource_I,
            bool boolIsEleet_I
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            int intStatus = 200;
            String strUserMessage = "";
            String strDevMessage = "";

            Resjson3ResourceJson3 resjson3;
            ProdtypProductType.subAddResourceAndCreateGroup(intPkProcessInWorkflow_I, intPkResource_I,
                intPkEleetOrEleele_I, boolIsEleet_I, ps_I, out resjson3, context, ref intStatus, ref strUserMessage,
                ref strDevMessage);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private void subAddLink(

            PsPrintShop ps_I,
            int intPkProcessInWorkflowO_I,
            int intPkEleetOrEleeleO_I,
            bool boolIsEleetO_I,
            int intPkProcessInWorkflowI_I,
            int intPkEleetOrEleeleI_I,
            bool boolIsEleetI_I
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            int intStatus = 200;
            String strUserMessage = "";
            String strDevMessage = "";

            WfandlinkjsonWorkflowPkAndLinkJson wfandlinkjson;

            ProdtypProductType.subLinkProcessInWorkflow(ps_I, false, intPkProcessInWorkflowO_I, intPkEleetOrEleeleO_I,
                boolIsEleetO_I, null, intPkProcessInWorkflowI_I, intPkEleetOrEleeleI_I, boolIsEleetI_I, null, null,
                ref intStatus, ref strDevMessage, ref strUserMessage, out wfandlinkjson);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subModifySpecialPassword(

            PsPrintShop ps_I,
            String strCurrentPassword_I,
            String strNewPassword_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Establish connection.
            Odyssey2Context context = new Odyssey2Context();

            intStatus_IO = 402;
            strUserMessage_IO = "Fill the gaps.";
            strDevMessage_IO = "";
            if (
                strCurrentPassword_I.Length > 0 &&
                strNewPassword_I.Length > 0
                )
            {
                intStatus_IO = 403;
                strUserMessage_IO = "Current password does not match.";
                strDevMessage_IO = "";
                if (
                    ps_I.strSpecialPassword == strCurrentPassword_I
                    )
                {
                    //                                      //Find data on DB.
                    PsentityPrintshopEntityDB psentity = context.Printshop.FirstOrDefault(ps =>
                        ps.intPk == ps_I.intPk);
                    //                                      //Reassign password.
                    psentity.strSpecialPassword = strNewPassword_I;
                    context.SaveChanges();

                    intStatus_IO = 200;
                    strUserMessage_IO = "Success.";
                    strDevMessage_IO = "";
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subSetReportFilter(
            //                                              //Add or update a reportFilter.

            int? intnPkReport_I,
            bool boolSuperAdmin_I,
            String strDataSet_I,
            String strName_I,
            String strfilter_I,
            PsPrintShop ps_I,
            out int intPkReport_O,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            intPkReport_O = -1;
            int? intnPkPrintshop = null;
            intnPkPrintshop = boolSuperAdmin_I == false ? ps_I.intPk : intnPkPrintshop;

            if (
                //                                          //Need to add a new reportFilter
                intnPkReport_I == null
                )
            {
                //                                          //Validate there is not other filter with the same name.
                List<CusrepentityCustomResportEntityDB> darrcusrepentity = context_M.CustomReport.Where(cr =>
                    cr.strName == strName_I && cr.intnPkPrintshop == intnPkPrintshop &&
                    cr.strDataSet == strDataSet_I).ToList();

                intStatus_IO = 402;
                strUserMessage_IO = "";
                strDevMessage_IO = "Data set invalid.";
                if (
                    strDataSet_I == PsPrintShop.strJobs || strDataSet_I == PsPrintShop.strCustomers ||
                    strDataSet_I == PsPrintShop.strAccounts
                    )
                {
                    intStatus_IO = 403;
                    strUserMessage_IO = "A report with this name already exists.";
                    strDevMessage_IO = "";
                    if (
                        !(darrcusrepentity.Count > 0)
                        )
                    {
                        //                                      //Add new reportFilter.
                        CusrepentityCustomResportEntityDB cusrepentity = new CusrepentityCustomResportEntityDB
                        {
                            intnPkPrintshop = intnPkPrintshop,
                            strName = strName_I,
                            strDataSet = strDataSet_I,
                            strFilter = strfilter_I
                        };
                        context_M.Add(cusrepentity);
                        context_M.SaveChanges();

                        intPkReport_O = cusrepentity.intPk;
                        intStatus_IO = 200;
                        strUserMessage_IO = "";
                        strDevMessage_IO = "";
                    }
                }
            }
            else
            {
                //                                          //Update a reportFilter.
                //                                          //Get reportFilter.
                CusrepentityCustomResportEntityDB cusrepentityToUpdate = context_M.CustomReport.FirstOrDefault(cr =>
                    cr.intPk == intnPkReport_I);

                intStatus_IO = 403;
                strUserMessage_IO = "Custom report not found.";
                strDevMessage_IO = "";
                if (
                    cusrepentityToUpdate != null
                    )
                {
                    intStatus_IO = 404;
                    strUserMessage_IO = "Something wrong.";
                    strDevMessage_IO = "A report created by superAdmin only can be edited by superAdmin.";
                    if (
                        //                                  //Reports was not created by superAdmin.
                        cusrepentityToUpdate.PkPrintshop != null ||
                        //                                  //Reports was created by superAdmin, only superAdmin can
                        //                                  //      modify it.
                        (
                        cusrepentityToUpdate.PkPrintshop == null && boolSuperAdmin_I
                        )
                        )
                    {
                        //                                  //Validate there is not other filter with the same name.
                        List<CusrepentityCustomResportEntityDB> darrcusrepentity = context_M.CustomReport.Where(cr =>
                            cr.strName == strName_I && cr.intnPkPrintshop == intnPkPrintshop &&
                            cr.intPk != intnPkReport_I && cr.strDataSet == strDataSet_I).ToList();

                        intStatus_IO = 405;
                        strUserMessage_IO = "A report with this name already exists.";
                        strDevMessage_IO = "";
                        if (
                            !(darrcusrepentity.Count > 0)
                            )
                        {
                            cusrepentityToUpdate.strName = strName_I;
                            cusrepentityToUpdate.strFilter = strfilter_I;

                            context_M.Update(cusrepentityToUpdate);
                            context_M.SaveChanges();

                            intPkReport_O = (int)intnPkReport_I;
                            intStatus_IO = 200;
                            strUserMessage_IO = "";
                            strDevMessage_IO = "";
                        }
                    }                    
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subDeleteReportFilter(
            //                                              //Find a delete a report filter from DB.

            int intPkReport_I,
            bool boolSuperAdmin_I,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Find report filter.
            CusrepentityCustomResportEntityDB cusrepentity = context_M.CustomReport.FirstOrDefault(report =>
                report.intPk == intPkReport_I);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Report not found.";
            if (
                (cusrepentity != null) && ((cusrepentity.intnPkPrintshop != null) || (
                (cusrepentity.intnPkPrintshop == null) && boolSuperAdmin_I))
                )
            {
                //                                          //Delete report.
                context_M.Remove(cusrepentity);
                context_M.SaveChanges();

                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "";
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subSetOffsetNumber(
            //                                              //Find a delete a report filter from DB.

            int intOffsetNumber_I,
            PsPrintShop ps_I,
            Odyssey2Context context_M,
            IConfiguration configuration_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Verify if the offsetNumber was already set.
            if (
                //                                          //Offset has not been set yet.
                !ps_I.boolOffset
                )
            {
                intStatus_IO = 401;
                strUserMessage_IO = "Offset can not be less than 0.";
                strDevMessage_IO = "Offset can not be less than 0.";
                if (
                    //                                      //OffsetNumber can not be less than 0.
                    intOffsetNumber_I >= 0
                    )
                {
                    int intOffsetToStart = intOffsetNumber_I - 1;

                    //                                          //Verify if exists register for this printshop in jobjson
                    //                                          //      table.
                    List<JobjsonentityJobJsonEntityDB> darrjobjsonentity = context_M.JobJson.Where(job =>
                        job.strPrintshopId == ps_I.strPrintshopId).ToList();

                    if (
                        //                                      //There is already information. Update order number.
                        darrjobjsonentity.Count > 0
                        )
                    {
                        foreach (JobjsonentityJobJsonEntityDB jobjson in darrjobjsonentity)
                        {
                            if (
                                //                          //There is already a orderNumber
                                jobjson.intnOrderNumber != null
                                )
                            {
                                //                          //Update.
                                jobjson.intnOrderNumber = jobjson.intnOrderNumber + intOffsetToStart;
                                context_M.JobJson.Update(jobjson);
                            }
                        }
                        context_M.SaveChanges();
                    }
                    else
                    {
                        //                                  //There is not information, bring jobs.
                        //                                  //Json that specifies all the jobs are required.
                        StagesjsonStagesJsonInternal stagesjsonAll = new StagesjsonStagesJsonInternal(
                            ps_I.strPrintshopId.ParseToInt(), null, null, null, null, null, null, null, true);

                        //                                  //Get all jobs' basic info.
                        JobJob.subGetAllJobsToSetOrderNumber(ps_I.strPrintshopId, configuration_I,
                            intOffsetNumber_I, context_M, ref intStatus_IO, ref strUserMessage_IO,
                            ref strDevMessage_IO);
                    }

                    //                                      //Update OffsetNumber in Printshop table.
                    PsentityPrintshopEntityDB psentity = context_M.Printshop.FirstOrDefault(ps => ps.intPk == ps_I.intPk);

                    psentity.boolOffsetNumber = true;
                    context_M.Printshop.Update(psentity);
                    context_M.SaveChanges();

                    intStatus_IO = 200;
                    strUserMessage_IO = "";
                    strDevMessage_IO = "";
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subUpdateTimeZone(
            //                                              //Update a prinshop's time zone.

            String strTimezoneId_I,
            PsPrintShop ps_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Timezones's list.
            List<TimzonjsonTimesZonesJson> darrtimzonjson = ps_I.darrTimzonList;

            intStatus_IO = 402;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Zone time not found.";
            if (
                darrtimzonjson.Exists(timzon => timzon.strTimeZoneId == strTimezoneId_I)
                )
            {
                Odyssey2Context context = new Odyssey2Context();

                //                                          //Update timezone in Printshop table.
                PsentityPrintshopEntityDB psentity = context.Printshop.FirstOrDefault(ps => ps.intPk == ps_I.intPk);

                intStatus_IO = 403;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Printshop not found.";
                if (
                    psentity != null
                    )
                {
                    psentity.strTimeZone = strTimezoneId_I;
                    context.Printshop.Update(psentity);
                    context.SaveChanges();

                    intStatus_IO = 200;
                    strUserMessage_IO = "";
                    strDevMessage_IO = "";
                }
            }  
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetEmployees(

            bool boolOwnerIncluded_I,
            PsPrintShop ps_I,
            out Empljson2EmployeeJson2 empljson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            empljson_O = null;

            //                                              //Get data from Wisnet.
            String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                GetSection("Odyssey2Settings")["urlWisnetApi"];
            Task<List<ContactjsonContactJson>> Task_darrcontactjsonFromWisnet = HttpTools<ContactjsonContactJson>.
                GetListAsyncToEndPoint(strUrlWisnet + "/Contacts/" + ps_I.strPrintshopId);
            Task_darrcontactjsonFromWisnet.Wait();

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Wisnet connection lost.";
            if (
                //                                          //There is access to the service of Wisnet.
                Task_darrcontactjsonFromWisnet.Result != null
                )
            {
                //                                          //Establish connection.
                Odyssey2Context context = new Odyssey2Context();

                //                                          //Final array of products from Wisnet.
                List<ContactjsonContactJson> darrcontactjsonFromWisnet = Task_darrcontactjsonFromWisnet.Result;
                //                                          //To be fill inside foreach.
                List<EmpljsonEmployeeJson> darrempljsonEmployee = new List<EmpljsonEmployeeJson>();

                //                                          //To easy code.
                int intOwnerIncluded = boolOwnerIncluded_I ? 1 : 0;

                foreach (ContactjsonContactJson contactjson in darrcontactjsonFromWisnet)
                {
                    if (
                        (intOwnerIncluded == 0 &&
                        (contactjson.intPrintshopEmployee == 1 ||
                        contactjson.intPrintshopAdmin == 1) &&
                        contactjson.intPrintshopOwner == 0
                        )
                        ||
                        (intOwnerIncluded == 1 &&
                        (contactjson.intPrintshopEmployee == 1 ||
                        contactjson.intPrintshopAdmin == 1) &&
                        (contactjson.intPrintshopOwner == 0 ||
                        contactjson.intPrintshopOwner == 1)
                        )
                        )
                    {
                        //                                  //Verify if the contact has roles.
                        RolentityRoleEntityDB roleentity = context.Role.FirstOrDefault(role =>
                            role.intContactId == contactjson.intContactId &&
                            role.intPkPrintshop == ps_I.intPk);

                        //                                  //Add contact to Employees list.
                        if (
                            //                              //Contact has roles.
                            roleentity != null
                            )
                        {
                            EmpljsonEmployeeJson empljson = new EmpljsonEmployeeJson(contactjson.strFirstName,
                                contactjson.strLastName, contactjson.intContactId, contactjson.strPhotoUrl,
                                roleentity.boolSupervisor, roleentity.boolAccountant);
                            darrempljsonEmployee.Add(empljson);
                        }
                        else
                        {
                            EmpljsonEmployeeJson empljson = new EmpljsonEmployeeJson(contactjson.strFirstName,
                                contactjson.strLastName, contactjson.intContactId, contactjson.strPhotoUrl, false,
                                false);
                            darrempljsonEmployee.Add(empljson);
                        }
                    }
                }

                //                                          //Json to return with list of employees.
                empljson_O = new Empljson2EmployeeJson2(darrempljsonEmployee.ToArray());

                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "";
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetPrintshopCompanies(
            //                                              //Get printshop's companies from wisnet DB.

            PsPrintShop ps_I,
            out List<ComjsonCompanyJson> darrcomjson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            darrcomjson_O = new List<ComjsonCompanyJson>();

            //                                              //Get data from Wisnet.
            String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                GetSection("Odyssey2Settings")["urlWisnetApi"];
            Task<List<ComjsonCompanyJson>> Task_darrcomjsonFromWisnet = HttpTools<ComjsonCompanyJson>.
                GetListAsyncToEndPoint(strUrlWisnet + "/Company/getPrintshopCompanies/" + ps_I.strPrintshopId);
            Task_darrcomjsonFromWisnet.Wait();

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Wisnet connection lost.";
            if (
                //                                          //There is access to the service of Wisnet.
                Task_darrcomjsonFromWisnet.Result != null
                )
            {
                //                                          //Final array of companies from Wisnet.
                List<ComjsonCompanyJson> darrcomjsonFromWisnet = Task_darrcomjsonFromWisnet.Result;

                if (
                    //                                      //There is only one register and the Id is -1, means
                    //                                      //      printshop has not companies.
                    (darrcomjsonFromWisnet.Count() == 1 && darrcomjsonFromWisnet[0].intCompanyId == -1)
                    )
                {
                    //                                      //Nothing to do.
                }
                else
                {
                    foreach (ComjsonCompanyJson comjsonCompany in darrcomjsonFromWisnet)
                    {
                        ComjsonCompanyJson comjson = new ComjsonCompanyJson(comjsonCompany.intCompanyId,
                            comjsonCompany.strName);
                        darrcomjson_O.Add(comjson);
                    }

                    intStatus_IO = 200;
                    strUserMessage_IO = "";
                    strDevMessage_IO = "";
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetPrintshopCompanyBranches(
            //                                              //Get companies branches from wisnet DB.

            PsPrintShop ps_I,
            int? intnCompanyId_I,
            out List<BrajsonBranchJson> darrbrajson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            darrbrajson_O = new List<BrajsonBranchJson>();

            if (
                //                                          //Search branches only if CompanyId >= 0
                intnCompanyId_I != null
                )
            {
                //                                          //Get data from Wisnet.
                String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                    GetSection("Odyssey2Settings")["urlWisnetApi"];
                Task<List<BrajsonBranchJson>> Task_darrcomjsonFromWisnet = HttpTools<BrajsonBranchJson>.
                    GetListAsyncToEndPoint(strUrlWisnet + "/Company/getCompanyBranches/" + ps_I.strPrintshopId +
                    "/" + intnCompanyId_I);
                Task_darrcomjsonFromWisnet.Wait();

                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Wisnet connection lost.";
                if (
                    //                                      //There is access to the service of Wisnet.
                    Task_darrcomjsonFromWisnet.Result != null
                    )
                {
                    //                                      //Final array of companies from Wisnet.
                    List<BrajsonBranchJson> darrbrajsonFromWisnet = Task_darrcomjsonFromWisnet.Result;

                    if (
                    //                                      //There is only one register and the Id is -1, means
                    //                                      //      company has not branches.
                    (darrbrajsonFromWisnet.Count() == 1 && darrbrajsonFromWisnet[0].intBranchId == -1)
                    )
                    {
                        //                                  //Nothing to do.
                        intStatus_IO = 200;
                        strUserMessage_IO = "";
                        strDevMessage_IO = "";
                    }
                    else
                    {
                        foreach (BrajsonBranchJson brajsonBranch in darrbrajsonFromWisnet)
                        {
                            BrajsonBranchJson brajson = new BrajsonBranchJson(brajsonBranch.intBranchId,
                                brajsonBranch.strName);
                            darrbrajson_O.Add(brajson);
                        }

                        intStatus_IO = 200;
                        strUserMessage_IO = "";
                        strDevMessage_IO = "";
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetCompanyBranchContacts(
            //                                              //Get companies branches from wisnet DB.

            PsPrintShop ps_I,
            int? intnCompanyId_I,
            int? intnBranchId_I,
            out List<ContjsonContactJson> darrcontjson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            darrcontjson_O = new List<ContjsonContactJson>();

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Branch is not valid.";
            if (
                //                                          //Validate Branch
                (intnBranchId_I != null && intnCompanyId_I != null) ||
                intnBranchId_I == null
                )
            {
                PsPrintShop.subGetCompanyBranchContacts(intnBranchId_I, intnCompanyId_I, ps_I.strPrintshopId,
                    out darrcontjson_O, ref intStatus_IO, ref strUserMessage_IO,
                    ref strDevMessage_IO);

                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "";
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subGetCompanyBranchContacts(
            //                                              //Get the contacts for a printshop, a company or branch.

            int? intnBranchId_I,
            int? intnCompanyId_I,
            //                                              //PrintshopId
            String strPrintshopId_I,
            out List<ContjsonContactJson> darrcontjson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            darrcontjson_O = new List<ContjsonContactJson>();

            //                                              //Get data from Wisnet.
            String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                GetSection("Odyssey2Settings")["urlWisnetApi"];

            Task<List<ContjsonContactJson>> Task_darrcontjsonFromWisnet;
            if (
                intnBranchId_I == null && intnCompanyId_I == null
                )
            {
                Task_darrcontjsonFromWisnet = HttpTools<ContjsonContactJson>.
                GetListAsyncToEndPoint(strUrlWisnet + "/Company/getPrintshopContacts/" + strPrintshopId_I);
            }
            else
            {
                Task_darrcontjsonFromWisnet = HttpTools<ContjsonContactJson>.
                GetListAsyncToEndPoint(strUrlWisnet + "/Company/getPrintshopContacts/" + strPrintshopId_I +
                "/" + intnCompanyId_I + "/" + intnBranchId_I);
            }
            Task_darrcontjsonFromWisnet.Wait();

            intStatus_IO = 403;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Wisnet connection lost.";
            if (
                //                                          //There is access to the service of Wisnet.
                Task_darrcontjsonFromWisnet.Result != null
                )
            {
                //                                          //Final array of companies from Wisnet.
                List<ContjsonContactJson> darrcontjsonFromWisnet = Task_darrcontjsonFromWisnet.Result;

                if (
                //                                          //There is only one register and the Id is -1, means
                //                                          //      company has not contacts.
                (darrcontjsonFromWisnet.Count() == 1 && darrcontjsonFromWisnet[0].intContactId == -1)
                )
                {
                    //                                      //Nothing to do.
                }
                else
                {
                    foreach (ContjsonContactJson contjsonContact in darrcontjsonFromWisnet)
                    {
                        darrcontjson_O.Add(contjsonContact);
                    }

                    intStatus_IO = 200;
                    strUserMessage_IO = "";
                    strDevMessage_IO = "";
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetPrintshopCategories(
            //                                              //Get printshop's categories from wisnet DB.

            String strPrintshopId_I,
            out List<CatejsonCategoryJson> darrcatejson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            darrcatejson_O = new List<CatejsonCategoryJson>();

            //                                              //Get data from Wisnet.
            String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                GetSection("Odyssey2Settings")["urlWisnetApi"];
            Task<List<CatjsonCategoryJson>> Task_darrcatjsonFromWisnet = HttpTools<CatjsonCategoryJson>.
                GetListAsyncToEndPoint(strUrlWisnet + "/PrintshopData/categories/" + strPrintshopId_I);
            Task_darrcatjsonFromWisnet.Wait();

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Wisnet connection lost.";
            if (
                //                                          //There is access to the service of Wisnet.
                Task_darrcatjsonFromWisnet.Result != null
                )
            {
                //                                          //Final array of categories from Wisnet.
                //                                          //If there is only one element and the companyId is -1,
                //                                          //      means the printshop has not categories associated.
                List<CatjsonCategoryJson> darrcatjsonFromWisnet = Task_darrcatjsonFromWisnet.Result;

                foreach (CatjsonCategoryJson catjsonCategory in darrcatjsonFromWisnet)
                {
                    CatejsonCategoryJson catejson = new CatejsonCategoryJson(catjsonCategory.Category);
                    darrcatejson_O.Add(catejson);
                }

                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "";
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static List<PsorderjsonPrintshopOrderJson> darrpsorderjsonGetPrintshopOrders(
            //                                              //Get printshop's completed orders.

            PsPrintShop ps_I,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //List of completed orders to send back.
            List<PsorderjsonPrintshopOrderJson> darrpsorderjson = new List<PsorderjsonPrintshopOrderJson>();

            //                                              //Get list of ids for completed jobs.
            List<int> darrintJobsIdsCompleted = (from jobentity in context_M.Job
                                                 where jobentity.intPkPrintshop == ps_I.intPk &&
                                                 jobentity.intStage == JobJob.intCompletedStage
                                                 select jobentity.intJobID).ToList();

            JobidsjsonJobIdsJson jobidsjson = new JobidsjsonJobIdsJson(darrintJobsIdsCompleted.ToArray());

            //                                              //Get data from Wisnet.
            String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                GetSection("Odyssey2Settings")["urlWisnetApi"];

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

                    InvoInvoiceEntityDB inventity = context_M.Invoice.FirstOrDefault(inv =>
                        inv.intOrderNumber == darrpsorderjson[intI].intOrderId);
                    if (
                        inventity != null
                        )
                    {
                        darrpsorderjson[intI].intnPkInvoice = inventity.intPk;
                    }
                }

                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "";
            }

            return darrpsorderjson;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static int intGetUnreadNotificationsNumber(
            PsPrintShop ps_I,
            int intContactId_I
            )
        {
            int intUnReadNotificationsNumber = 0;

            //                                              //Establish connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get all the alerts for the printshop or the specic 
            //                                              //      contact.
            List<AlertentityAlertEntityDB> darralertentity = context.Alert.Where(alert =>
                alert.intPkPrintshop == ps_I.intPk && (alert.intnContactId == null ||
                alert.intnContactId == intContactId_I)).ToList();

            foreach (AlertentityAlertEntityDB alertentity in darralertentity)
            {
                if (
                    //                                      //Notification not read by user.
                    !PsPrintShop.boolNotificationReadByUser(alertentity, intContactId_I)
                    )
                {
                    intUnReadNotificationsNumber = intUnReadNotificationsNumber + 1;
                }
            }

            return intUnReadNotificationsNumber;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static bool boolNotificationReadByUser(
            //                                              //Verify if contactid exists in Readby data from alert
            //                                              //      entity.

            AlertentityAlertEntityDB alertentity_I,
            int intContactId_I
            )
        {
            bool boolNotificationReadByUser = false;

            if (
                alertentity_I.strReadBy != null
                )
            {
                boolNotificationReadByUser = alertentity_I.strReadBy.Contains("|" + intContactId_I);
            }

            return boolNotificationReadByUser;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static int[] arrintGetAllContactIdsFromPrintshop(
            //                                              //Get all contact ids from a printshop.

            String strPrintshopId_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            List<int> darrintContactIdsFromPrintshop = new List<int>();

            //                                              //Get data from Wisnet.
            String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                GetSection("Odyssey2Settings")["urlWisnetApi"];
            Task<List<PrintcontjsonPrintshopContactJson>> Task_darrprintcontjsonFromWisnet =
                HttpTools<PrintcontjsonPrintshopContactJson>.GetListAsyncToEndPoint(
                strUrlWisnet + "/Company/getPrintshopContacts/" + strPrintshopId_I);
            Task_darrprintcontjsonFromWisnet.Wait();

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Wisnet connection lost.";
            if (
                //                                          //There is access to the service of Wisnet.
                Task_darrprintcontjsonFromWisnet.Result != null
                )
            {
                //                                          //Final array of products from Wisnet.
                List<PrintcontjsonPrintshopContactJson> darrprintcontjsonFromWisnet =
                    Task_darrprintcontjsonFromWisnet.Result;

                foreach (PrintcontjsonPrintshopContactJson printcontjson in darrprintcontjsonFromWisnet)
                {
                    darrintContactIdsFromPrintshop.Add(printcontjson.intContactId);
                }

                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "";
            }

            return darrintContactIdsFromPrintshop.ToArray();
        }

        //--------------------------------------------------------------------------------------------------------------
        public static CusrepjsonCustomReportJson cusrepjsonGet(
            int intPk_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            CusrepjsonCustomReportJson cusrepjson = null;

            //                                              //Connection.
            Odyssey2Context context = new Odyssey2Context();

            CusrepentityCustomResportEntityDB cusrepentity = context.CustomReport.FirstOrDefault(cusrep =>
                cusrep.intPk == intPk_I);

            intStatus_IO = 401;
            strUserMessage_IO = "Something wrong.";
            strDevMessage_IO = "No report filter found.";
            if (
                cusrepentity != null
                )
            {
                cusrepjson = new CusrepjsonCustomReportJson(cusrepentity.intPk, cusrepentity.strName,
                    JsonSerializer.Deserialize<JsonElement>(cusrepentity.strFilter));

                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "";
            }

            return cusrepjson;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetReportFilters(
            //                                              //Get report filters of a data set.

            PsPrintShop ps_I,
            String strDataSet_I,
            out RepjsonReportsFiltersJson repjson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //List of the report filters
            List<RepftrjsonReportFilterJson> darrrepftrjson = new List<RepftrjsonReportFilterJson>();
            List<RepftrjsonReportFilterJson> darrrepftrjsonReadyToUse = new List<RepftrjsonReportFilterJson>();

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Data set is not valid.";
            if (
                //                                          //Validate Data set
                strDataSet_I == PsPrintShop.strJobs || strDataSet_I == PsPrintShop.strCustomers || 
                strDataSet_I == PsPrintShop.strAccounts
                )
            {
                //                                          //Establish connection.
                Odyssey2Context context = new Odyssey2Context();

                //                                          //Find report filters for the data set
                List<CusrepentityCustomResportEntityDB> darrcusrepentity = context.CustomReport.Where(cusrep =>
                    cusrep.intnPkPrintshop == ps_I.intPk && cusrep.strDataSet == strDataSet_I).ToList();

                foreach (CusrepentityCustomResportEntityDB cusrepentity in darrcusrepentity)
                {
                    RepftrjsonReportFilterJson repftrjson = new RepftrjsonReportFilterJson(cusrepentity.intPk,
                        cusrepentity.strName);
                    //                                      //Add record to list
                    darrrepftrjson.Add(repftrjson);
                }

                //                                          //Find report filters for the data set
                List<CusrepentityCustomResportEntityDB> darrcusrepentityReady = context.CustomReport.Where(cusrep =>
                    cusrep.intnPkPrintshop == null && cusrep.strDataSet == strDataSet_I).ToList();

                foreach (CusrepentityCustomResportEntityDB cusrepentity in darrcusrepentityReady)
                {
                    RepftrjsonReportFilterJson repftrjson = new RepftrjsonReportFilterJson(cusrepentity.intPk,
                        cusrepentity.strName);
                    //                                      //Add record to list
                    darrrepftrjsonReadyToUse.Add(repftrjson);
                }

                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "";
            }

            darrrepftrjson = darrrepftrjson.OrderBy(name => name.strName).ToList();
            repjson_O = new RepjsonReportsFiltersJson(darrrepftrjson.ToArray(), darrrepftrjsonReadyToUse.ToArray());
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetOpenPayments(
            //                                              //Get all payments that have not been deposited.

            PsPrintShop ps_I,
            out List<OppayjsonOpenPaymentJson> darroppayjson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //List to return.
            darroppayjson_O = new List<OppayjsonOpenPaymentJson>();

            //                                              //Establish connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Find printshop's undeposited payments.
            //List<PaymtPaymentEntityDB> darrpaymtentity = context.Payment.Where(pay =>
            //    pay.intPkPrintshop == ps_I.intPk && pay.intnPkBankDeposit == null).ToList();

            List<PaymtPaymentEntityDB> darrpaymtentity =
                (from paymtentity in context.Payment
                 join accmoventity in context.AccountMovement
                 on paymtentity.intPk equals accmoventity.intnPkPayment
                 join accentity in context.Account
                 on accmoventity.intPkAccount equals accentity.intPk
                 where paymtentity.intPkPrintshop == ps_I.intPk &&
                 paymtentity.intnPkBankDeposit == null &&
                 accentity.strNumber == AccAccounting.strUndepositedFundsNumber &&
                 accentity.strName == AccAccounting.strUndepositedFundsName
                 select paymtentity).ToList();

            if (
                darrpaymtentity.Count > 0
                )
            {
                //                                          //Find all printshop's customers.
                Cusjson2CustomerJson2[] arrcusjson2;
                CusCustomer.subGetCustomers(ps_I, out arrcusjson2, ref intStatus_IO, ref strUserMessage_IO,
                    ref strDevMessage_IO);

                foreach (PaymtPaymentEntityDB paymententity in darrpaymtentity)
                {
                    //                                      //Find customer's full name.

                    //                                      //Find customer.
                    Cusjson2CustomerJson2 cusjson2 = arrcusjson2.FirstOrDefault(customer =>
                        customer.intContactId == paymententity.intContactId);

                    String strCustomerFullName = "";
                    if (
                        cusjson2 != null
                        )
                    {
                        strCustomerFullName = cusjson2.strFullName;
                    }

                    //                                      //Find payment method.
                    PymtmtentityPaymentMethodEntityDB pymtmtentity = context.PaymentMethod.FirstOrDefault(method =>
                        method.intPk == paymententity.intPkPaymentMethod);

                    String strPaymentMethod = "";
                    if (
                        pymtmtentity != null
                        )
                    {
                        strPaymentMethod = pymtmtentity.strName;
                    }

                    //                                      //Create json.
                    OppayjsonOpenPaymentJson oppayjson = new OppayjsonOpenPaymentJson(paymententity.intPk,
                        strCustomerFullName, paymententity.strDate, strPaymentMethod, paymententity.strReference,
                        paymententity.numAmount);
                    darroppayjson_O.Add(oppayjson);
                }
            }

            intStatus_IO = 200;
            strUserMessage_IO = "";
            strDevMessage_IO = "";
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String[] arrstrGetWorkInProgressStatus(
            //                                              //Get substages for workInProgress.

            PsPrintShop ps_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //To store wisnet response.
            List<String> darrstrSubStages = new List<string>();

            //                                          //Get data from Wisnet.
            String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                GetSection("Odyssey2Settings")["urlWisnetApi"];
            Task<String[]> Task_arrstrSubStages = HttpTools<TjsonTJson>.
                GetWorkInProgressSubStages(strUrlWisnet + "/PrintShopData/WorkInProgressStatus/" +
                ps_I.strPrintshopId);
            Task_arrstrSubStages.Wait();

            intStatus_IO = 402;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Wisnet connection lost.";
            if (
                //                                      //There is access to the service of Wisnet.
                Task_arrstrSubStages.Result != null
                )
            {
                //                                          //Obtain subStages.
                darrstrSubStages = Task_arrstrSubStages.Result.ToList();
                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "";
            }

            return darrstrSubStages.ToArray();
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
