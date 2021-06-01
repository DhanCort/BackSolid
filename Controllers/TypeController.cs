/*TASK RP. PROCESS TYPE*/
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Odyssey2Backend.App;
using Odyssey2Backend.Infrastructure;
using Odyssey2Backend.JsonTypes;
using Odyssey2Backend.PrintShop;
using Odyssey2Backend.Utilities;
using Odyssey2Backend.XJDF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa (VSTD - Victor Torres).
//                                                          //DATE: December 03, 2019.

namespace Odyssey2Backend.Controllers
{
    //                                                      //To obtain the strPrintshopId from token:
    //                                                      //  var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
    //                                                      //  String strPrintshopId = idClaim.Value;

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //==================================================================================================================
    [Route("Odyssey2/[controller]")]
    public class TypeController : Controller
    {
        //                                                  //Controller associated with the actions for a process 
        //                                                  //      type.
        //                                                  //The post methods receive a json, for the validations are 
        //                                                  //      used the ValueKind property that could have the 
        //                                                  //      following values.
        //                                                  //JsonElement.ValueKind:
        //                                                  //      1 - Json Object
        //                                                  //      2 - Json array
        //                                                  //      3 - Json string
        //                                                  //      4 - Json number
        //                                                  //      5 - Json value true
        //                                                  //      6 - Json value false
        //                                                  //      7 - Json value null
        //                                                  //      0 - There is no value

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //TRANSFORMATION METHODS.

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult AddTypeOrTemplateToProcess(
            //                                              //PURPOSE:
            //                                              //Relate a resource type or resource template with a 
            //                                              //      proccess.

            //                                              //URL: http://localhost/Odyssey2/Type
            //                                              //      /AddTypeOrTemplateToProcess
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intPkProcess" : 1,
            //                                              //          "intnPkType" : 5,
            //                                              //          "intnPkTemplate" : null,
            //                                              //          "strInputOrOutput" : "Input"    
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Relate a resource type or template with a process and 
            //                                              //      indicates whether the type or template is an input 
            //                                              //      or an output.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            //                                              //Json that contqains a Json string with all data.
            [FromBody] JsonElement jsonIO
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !Object.ReferenceEquals(null, jsonIO) &&
                //                                          //Verify if the data is not null or empty.
                jsonIO.TryGetProperty("intPkProcess", out json) &&
                (int)jsonIO.GetProperty("intPkProcess").ValueKind == 4 &&
                jsonIO.TryGetProperty("strInputOrOutput", out json) &&
                (int)jsonIO.GetProperty("strInputOrOutput").ValueKind == 3
                )
            {
                //                                          //Get data.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;
                int intPkProcess = jsonIO.GetProperty("intPkProcess").GetInt32();
                String strInputOrOutput = jsonIO.GetProperty("strInputOrOutput").GetString();

                int? intnPkType = null;
                if (
                    jsonIO.TryGetProperty("intnPkType", out json) &&
                    (int)jsonIO.GetProperty("intnPkType").ValueKind == 4
                    )
                {
                    intnPkType = jsonIO.GetProperty("intnPkType").GetInt32();
                }

                int? intnPkTemplate = null;
                if (
                    jsonIO.TryGetProperty("intnPkTemplate", out json) &&
                    (int)jsonIO.GetProperty("intnPkTemplate").ValueKind == 4
                    )
                {
                    intnPkTemplate = jsonIO.GetProperty("intnPkTemplate").GetInt32();
                }

                //                                          //using is to release connection at the end
                using (Odyssey2Context context = new Odyssey2Context())
                {
                    //                                      //Starts a new transaction.
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            int intPkProcessLast;
                            //                              //Assign a process type to a printshop.
                            ProtypProcessType.subAddResourceTypeOrTemplateToProcess(strPrintshopId, intPkProcess, 
                                intnPkType, intnPkTemplate, strInputOrOutput, context, out intPkProcessLast, 
                                ref intStatus, ref strUserMessage, ref strDevMessage);
                            obj = intPkProcessLast;

                            //                              //Commits all changes made to the database in the current
                            //                              //      transaction.
                            if (
                                intStatus == 200
                                )
                            {
                                dbContextTransaction.Commit();
                            }
                            else
                            {
                                dbContextTransaction.Rollback();
                            }
                        }
                        catch (Exception ex)
                        {
                            //                              //Discards all changes made to the database in the current
                            //                              //      transaction.
                            dbContextTransaction.Rollback();

                            //                              //Making a log for the exception.
                            Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                        }
                    }
                }
            }
            //                                              //Generate the response.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult AddCustomTypeToProcess(
            //                                              //PURPOSE:
            //                                              //Relate a resource type or resource template to a 
            //                                              //      proccess.

            //                                              //URL: http://localhost/Odyssey2/Type
            //                                              //      /AddCustomTypeToProcess.
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intPkProcess" : 1,
            //                                              //          "strInputOrOutput" : "Input"    
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Relate a custom resource type as an input or an output.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            //                                              //      400 - BadRequest("Invalid data.").

            //                                              //Json with all data.
            [FromBody] JsonElement jsonRelation
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !Object.ReferenceEquals(null, jsonRelation) &&
                //                                          //Verify if the data is not null or empty.
                jsonRelation.TryGetProperty("intPkProcess", out json) &&
                (int)jsonRelation.GetProperty("intPkProcess").ValueKind == 4 &&
                jsonRelation.TryGetProperty("strInputOrOutput", out json) &&
                (int)jsonRelation.GetProperty("strInputOrOutput").ValueKind == 3
                )
            {
                //                                          //Get data.
                int intPkProcess = jsonRelation.GetProperty("intPkProcess").GetInt32();
                String strInputOrOutput = jsonRelation.GetProperty("strInputOrOutput").GetString();

                //                                          //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);
                //                                          //using is to release connection at the end
                using (Odyssey2Context context = new Odyssey2Context())
                {
                    //                                      //Starts a new transaction.
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            int intPkProcessLast;
                            //                              //Assign a process type to a printshop.
                            ProtypProcessType.subAddCustomTypeToProcess(ps, intPkProcess, strInputOrOutput, context, 
                                out intPkProcessLast, ref intStatus, ref strUserMessage, ref strDevMessage);
                            obj = intPkProcessLast;

                            //                              //Commits all changes made to the database in the current
                            //                              //      transaction.
                            if (
                                intStatus == 200
                                )
                            {
                                dbContextTransaction.Commit();
                            }
                            else
                            {
                                dbContextTransaction.Rollback();
                            }
                        }
                        catch (Exception ex)
                        {
                            //                              //Discards all changes made to the database in the current
                            //                              //      transaction.
                            dbContextTransaction.Rollback();

                            //                              //Making a log for the exception.
                            Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                        }
                    }
                }
            }
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage, obj);

            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]/")]
        public IActionResult GetPrintshopTypes(
            //                                              //PURPOSE:
            //                                              //Get all the types associated with a printshop.

            //                                              //URL: http://localhost/Odyssey2/Type
            //                                              //      /GetPrintshopTypes?strResOrPro=Process
            //                                              //      &boolIsPhysical 
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get all the types from a printshop. It can be 
            //                                              //      a resource, intent, process or product.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            String strResOrPro,
            bool? boolnIsPhysical
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;

            if (
                //                                          //Verify if the printshop is not null.
                !String.IsNullOrEmpty(strResOrPro)
                )
            {
                //                                          //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;

                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);
                intStatus = 401;
                strUserMessage = "Printshop invalid.";
                strDevMessage = "Printshop not found.";
                //                                          //Since the true of ps is in Wisnet.
                if (
                    ps != null
                    )
                {
                    intStatus = 402;
                    strUserMessage = "Something is wrong.";
                    strDevMessage = "The ResOrPro is invalid.";
                    if (
                        strResOrPro == EtElementTypeAbstract.strProcess ||
                        strResOrPro == EtElementTypeAbstract.strProduct ||
                        strResOrPro == EtElementTypeAbstract.strIntent ||
                        strResOrPro == EtElementTypeAbstract.strResource
                        )
                    {
                        try
                        {
                            /*CASE*/
                            if (
                                strResOrPro == EtElementTypeAbstract.strProcess
                                )
                            {
                                //                          //Get the dictionary of products.
                                Dictionary<int, ProtypProcessType> dicprotyp = ps.dicprotyp;

                                //                          //List to add the product.
                                List<Etjson2ElementTypeJson2> darretjson1 = new List<Etjson2ElementTypeJson2>();
                                foreach (KeyValuePair<int, ProtypProcessType> protyp in dicprotyp)
                                {
                                    Etjson2ElementTypeJson2 etjson2 = new Etjson2ElementTypeJson2();

                                    String strName = (protyp.Value.strXJDFTypeId == "None") ? protyp.Value.strCustomTypeId :
                                        protyp.Value.strXJDFTypeId;
                                    etjson2.strTypeId = strName;
                                    etjson2.intPk = protyp.Value.intPk;
                                    etjson2.boolIsXJDF = protyp.Value.strAddedBy == EtElementTypeAbstract.strXJDFVersion;

                                    darretjson1.Add(etjson2);
                                }

                                darretjson1 = darretjson1.OrderBy(ret => ret.strTypeId).ToList();
                                intStatus = 200;
                                strUserMessage = "Success.";
                                strDevMessage = "";
                                obj = darretjson1;
                            }
                            else if (
                                strResOrPro == EtElementTypeAbstract.strProduct
                                )
                            {
                                List<Prodtypjson2ProductTypeJson2> darrprodtypjson2 = new List<Prodtypjson2ProductTypeJson2>();

                                //                          //Get the dictionary of products.
                                Dictionary<int, ProdtypProductType> dicprodtyp = ps.dicprodtyp;

                                intStatus = 403;
                                strUserMessage = "Something is wrong.";
                                strDevMessage = "Connection failed to obtain the products..";
                                if (
                                    dicprodtyp != null
                                    )
                                {
                                    String strWebsiteUrl = ps.strUrl;
                                    foreach (KeyValuePair<int, ProdtypProductType> prodtyp in dicprodtyp)
                                    {
                                        //                  //Get Product account.
                                        int? intnPkAccount = prodtyp.Value.intnPkAccount != null ? 
                                            prodtyp.Value.intnPkAccount : null;

                                        //                  //Get link.
                                        String strUrl = "http://" + strWebsiteUrl + "/printing/" +
                                            prodtyp.Value.strCategory.Replace(" ",  "-") + "/" + prodtyp.Value.intWebsiteProductKey;

                                        Prodtypjson2ProductTypeJson2 prodtypjson2 = new Prodtypjson2ProductTypeJson2(
                                            prodtyp.Value.intPk, prodtyp.Value.strCustomTypeId,
                                            prodtyp.Value.strXJDFTypeId, prodtyp.Value.strCategory, intnPkAccount,
                                            strUrl);

                                        darrprodtypjson2.Add(prodtypjson2);
                                    }
                                }

                                darrprodtypjson2 = darrprodtypjson2.OrderBy(prodtype => prodtype.strTypeId).ToList();

                                intStatus = 200;
                                strUserMessage = "Success.";
                                strDevMessage = "";
                                obj = darrprodtypjson2;
                            }
                            else if (
                                strResOrPro == EtElementTypeAbstract.strIntent
                                )
                            {
                                //                          //Get the dictionary of intents.
                                Dictionary<String, InttypIntentType> dicinttyp = Odyssey2.dicinttyp;

                                //                          //List to add the product.
                                List<Etjson1ElementTypeJson1> darretjson1 = new List<Etjson1ElementTypeJson1>();
                                foreach (KeyValuePair<String, InttypIntentType> inttyp in dicinttyp)
                                {
                                    Etjson1ElementTypeJson1 etjson1 = new Etjson1ElementTypeJson1();

                                    etjson1.strTypeId = inttyp.Value.strCustomTypeId;
                                    etjson1.intPk = inttyp.Value.intPk;

                                    darretjson1.Add(etjson1);
                                }

                                darretjson1 = darretjson1.OrderBy(ret => ret.strTypeId).ToList();

                                intStatus = 200;
                                strUserMessage = "Success.";
                                strDevMessage = "";
                                obj = darretjson1;
                            }
                            else if (
                                strResOrPro == EtElementTypeAbstract.strResource
                                )
                            {
                                //                          //Get the dictionary of resources.
                                Dictionary<String, RestypResourceType> dicrestyp = ps.dicrestyp;

                                //                          //List to add the resource.
                                List<Restypjson1ResourceTypeJson1> darrrestypjson1 =
                                    new List<Restypjson1ResourceTypeJson1>();

                                /*CASE*/
                                if (
                                    boolnIsPhysical == true
                                    )
                                {
                                    foreach (KeyValuePair<String, RestypResourceType> restyp in dicrestyp)
                                    {
                                        if (
                                            //              //Only returns the type if is Physical.
                                            RestypResourceType.boolIsPhysical(restyp.Value.strClassification)
                                            )
                                        {
                                            String strName = (restyp.Value.strXJDFTypeId == "None") ?
                                                restyp.Value.strCustomTypeId : restyp.Value.strXJDFTypeId;
                                            Restypjson1ResourceTypeJson1 restypjson1 = new Restypjson1ResourceTypeJson1(
                                                restyp.Value.intPk, strName, true);
                                            darrrestypjson1.Add(restypjson1);
                                        }
                                    }
                                }
                                else if (
                                    boolnIsPhysical == false
                                    )
                                {
                                    foreach (KeyValuePair<String, RestypResourceType> restyp in dicrestyp)
                                    {
                                        if (
                                            //              //Only returns the type if is Physical.
                                            !(RestypResourceType.boolIsPhysical(restyp.Value.strClassification))
                                            )
                                        {
                                            Restypjson1ResourceTypeJson1 restypjson1 = new Restypjson1ResourceTypeJson1(
                                                restyp.Value.intPk, restyp.Value.strXJDFTypeId, true);
                                            darrrestypjson1.Add(restypjson1);
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (KeyValuePair<String, RestypResourceType> restyp in dicrestyp)
                                    {
                                        Restypjson1ResourceTypeJson1 restypjson1 = new Restypjson1ResourceTypeJson1(
                                                restyp.Value.intPk, restyp.Value.strXJDFTypeId, true);
                                        darrrestypjson1.Add(restypjson1);
                                    }
                                }
                                /*END-CASE*/
                                intStatus = 200;
                                strUserMessage = "Success.";
                                strDevMessage = "";

                                darrrestypjson1 = darrrestypjson1.OrderBy(restyp => restyp.strTypeId).ToList();
                                obj = darrrestypjson1;
                            }
                            /*END-CASE*/
                        }
                        catch (Exception ex)
                        {
                            //                              //Making a log for the exception.
                            Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                        }
                    }
                }
            }
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);

            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]/")]
        public IActionResult GetPrintshopXJDFProcess(
            //                                              //PURPOSE:
            //                                              //Get all the XJDF process and indicates wich were already 
            //                                              //      selected.

            //                                              //URL: http://localhost/Odyssey2/Type
            //                                              //      /GetPrintshopXJDFProcess
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get all the XJDF process types.

            //                                              //RETURNS:
            //                                              //      200 - Ok(). 
            )
        {
            //                                              //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;

            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);
            int intStatus = 401;
            String strUserMessage = "Printshop invalid.";
            String strDevMessage = "Printshop not found.";
            Object obj = null;
            //                                              //Since the true of ps is in Wisnet.
            if (
                ps != null
                )
            {
                try
                {
                    obj = ps.arrprotypjsonGetXJDFProcess(ref intStatus, ref strUserMessage, ref strDevMessage);
                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }
            }

            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);

            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]/")]
        public IActionResult GetPrintshopXJDFResources(
            //                                              //PURPOSE:
            //                                              //Get all the XJDF resources and indicates wich were already 
            //                                              //      selected.

            //                                              //URL: http://localhost/Odyssey2/Type
            //                                              //      /GetPrintshopXJDFResources
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get all the XJDF resource types.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            )
        {
            //                                              //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;

            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);
            int intStatus = 401;
            String strUserMessage = "Printshop invalid.";
            String strDevMessage = "Printshop not found.";
            Object obj = null;
            //                                          //Since the true of ps is in Wisnet.
            if (
                ps != null
                )
            {
                try
                {
                    obj = ps.arretjsonGetXJDFResources(ref intStatus, ref strUserMessage, ref strDevMessage);
                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }
            }

            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);

            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]/")]
        public IActionResult GetXJDFResourcesByProcess(
            //                                              //PURPOSE:
            //                                              //Get all the XJDF resources associated with the given 
            //                                              //      process.

            //                                              //URL: http://localhost/Odyssey2/Type
            //                                              //      /GetXJDFResourcesByProcess?intPkProcess=0
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get the resource for the given process.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            //                                              //intPkProcessType.
            int intPkProcess,
            bool boolIsPhysical
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            if (
                intPkProcess > 0
                )
            {
                //                                          //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                intStatus = 401;
                strUserMessage = "Something is wrong.";
                strDevMessage = "Printshop not found.";
                if (
                    ps != null
                    )
                {
                    try
                    {
                        Protypjson5ProcessTypeJson5 rbpjson;
                        ps.subGetXJDFResourcesByProcess(intPkProcess, boolIsPhysical, out rbpjson, ref intStatus,
                            ref strUserMessage, ref strDevMessage);
                        obj = rbpjson;
                    }
                    catch (Exception ex)
                    {
                        //                                  //Making a log for the exception.
                        Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                    }
                }
            }

            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]/")]
        public IActionResult GetPrintshopProcessWithTypesAndTemplates(
            //                                              //PURPOSE:
            //                                              //Get a process with its types and templates (input and output).

            //                                              //URL: http://localhost/Odyssey2/Type/
            //                                              //      GetPrintshopProcessWithTypesAndTemplates?
            //                                              //      strPrintshopId=13832&intPk=12
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get one process and its resource-types and templates for
            //                                              //      a printshop.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            //                                              //Pk process.
            int intPk
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            if (
                //                                          //Verify there´s a process.
                intPk > 0
                )
            {
                //                                          //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                intStatus = 401;
                strUserMessage = "Something is wrong.";
                strDevMessage = "Printshop not found.";
                if (
                    ps != null
                    )
                {
                    try
                    {
                        Proelejson2ProcessElementJson2 proelejson2;
                        ps.proelejson2GetProcessWithTypesAndTemplates(intPk, out proelejson2, ref intStatus,
                            ref strUserMessage, ref strDevMessage);
                        obj = proelejson2;
                    }
                    catch (Exception ex)
                    {
                        //                                  //Making a log for the exception.
                        Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                    }
                }
            }
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]/")]
        public IActionResult GetPrintshopTypesOrTemplates(
            //                                              //PURPOSE:
            //                                              //Get type or template of a printshop
            //                                              //     filter for physical or not physical
            //                                              //     and suggested.

            //                                              //URL: http://localhost/Odyssey2/Type/
            //                                              //      GetPrintshopTypesOrTemplates?
            //                                              //      intPkProcess=12&
            //                                              //      boolIsType=true&
            //                                              //      boolIsTemplate=true&
            //                                              //      boolIsPhysical=true&
            //                                              //      boolIsNotPhysical=true&
            //                                              //      boolIsSuggested=true
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get Types or Template filtered by the parameter.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            //                                              //Receive the pk of type the process to show the suggested
            //                                              //It can be a negative number.
            int intPkProcessType,
            //                                              //and a set of boolean for filter.
            bool boolIsType,
            bool boolIsTemplate,
            bool boolIsPhysical,
            bool boolIsNotPhysical,
            bool boolIsSuggested
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;

            //                                          //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

            try
            {
                TyptemjsonTypeTemplateJson[] darrtyptemjson = ps.GetPrintshopTypesOrTemplates(intPkProcessType,
                    boolIsType, boolIsTemplate, boolIsPhysical, boolIsNotPhysical, boolIsSuggested, ref intStatus,
                    ref strUserMessage, ref strDevMessage);
                obj = darrtyptemjson;
            }
            catch (Exception ex)
            {
                //                                          //Making a log for the exception.
                Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
            }

            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]/")]
        public IActionResult GetResourcesClassification(
            //                                              //PURPOSE:
            //                                              //Get all the classes in the resource classification.

            //                                              //URL: http://localhost/Odyssey2/Type/
            //                                              //      GetResourcesClassification
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get all the classes in the resources classification.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            )
        {
            IActionResult aresult = base.BadRequest();

            try
            {
                aresult = base.Ok(EtElementTypeAbstract.arrstrResourceClasses());
            }
            catch (Exception ex)
            {
                //                                          //TO FIX. Hacer que este método retorne jsonresponse.
                int intStatus = 0;
                String strUserMessage = "", strDevMessage = "";

                //                                          //Making a log for the exception.
                Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
            }
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetTemplates(
            //                                              //PURPOSE:
            //                                              //Get the templates associated to the type selected.

            //                                              //URL: http://localhost/Odyssey2/Type
            //                                              //      /GetTemplates?intPk=2

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get an array of templates.

            //                                              //RETURNS:
            //                                              //      200 - Ok
            int intPk
            )
        {
            IActionResult aresult = BadRequest("Invalid data");

            if (
                intPk > 0
                )
            {
                //FIX IT. Toma el pk dek template como si fuera el pk del type.
                EtElementTypeAbstract et = EtElementTypeAbstract.etFromDB(intPk);
                RestypResourceType restyp = (RestypResourceType)et;
                aresult = BadRequest("Resource not found.");
                if (
                    restyp != null
                    )
                {
                    try
                    {
                        aresult = Ok(restyp.arrresjsonTemplate());
                    }
                    catch (Exception ex)
                    {
                        //                                  //TO FIX. Hacer que este método retorne jsonresponse.
                        int intStatus = 0;
                        String strUserMessage = "", strDevMessage = "";

                        //                                  //Making a log for the exception.
                        Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                    }
                }
            }

            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
