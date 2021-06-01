/*TASK RP.WORKFLOW*/
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Odyssey2Backend.Job;
using Odyssey2Backend.JsonTypes;
using Odyssey2Backend.JsonTypes.Out;
using Odyssey2Backend.PrintShop;
using Odyssey2Backend.XJDF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using TowaStandard;
using Microsoft.AspNetCore.SignalR;
using Odyssey2Backend.Alert;
using Odyssey2Backend.DB_Odyssey2;
using Odyssey2Backend.Utilities;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: February 28, 2020. 

namespace Odyssey2Backend.Controllers
{
    //                                                      //To obtain the strPrintshopId from token:
    //                                                      //  var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
    //                                                      //  String strPrintshopId = idClaim.Value;

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //==================================================================================================================
    [Route("Odyssey2/[controller]")]
    public class WorkflowController : Controller
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

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        private IConfiguration configuration;
        private readonly IHubContext<ConnectionHub> iHubContext;

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        public WorkflowController(
            IConfiguration iConfiguration_I,
            IHubContext<ConnectionHub> iHubContext_I
            )
        {
            this.configuration = iConfiguration_I;
            this.iHubContext = iHubContext_I;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //TRANSFORMATION METHODS.

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult AddNewWorkflow(
            //                                              //PURPOSE:
            //                                              //Add a new workflow for the given process. It could be a 
            //                                              //      new one or a copy from another one.

            //                                              //URL: http://localhost/Odyssey2/Workflow/AddNewWorkflow
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intnPkPrduct":0,
            //                                              //          "strWorkflowName":"Banners alternative",
            //                                              //          "intnPkWorkflow":0,
            //                                              //          "arrintPkPiw":
            //                                              //              [
            //                                                                  1,
            //                                              //                  2
            //                                              //              ]
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Add a new workflow to the product.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            //                                              //Json that contains all necessary data.
            [FromBody] JsonElement jsonWorkflow
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null or a value is not empty. 
                !((int)jsonWorkflow.ValueKind == 7) &&
                !((int)jsonWorkflow.ValueKind == 0) &&
                //                                          //Verify if the data is not null or empty.
                jsonWorkflow.TryGetProperty("strWorkflowName", out json) &&
                (int)jsonWorkflow.GetProperty("strWorkflowName").ValueKind == 3
                )
            {
                int? intnPkProduct = null;
                if (
                    jsonWorkflow.TryGetProperty("intnPkProduct", out json) &&
                    (int)jsonWorkflow.GetProperty("intnPkProduct").ValueKind == 4
                    )
                {
                    intnPkProduct = jsonWorkflow.GetProperty("intnPkProduct").GetInt32();
                }

                String strWorkflowName = jsonWorkflow.GetProperty("strWorkflowName").GetString();

                int? intnPkWorkflow = null;
                if (
                    jsonWorkflow.TryGetProperty("intnPkWorkflow", out json) &&
                    (int)jsonWorkflow.GetProperty("intnPkWorkflow").ValueKind == 4
                    )
                {
                    intnPkWorkflow = jsonWorkflow.GetProperty("intnPkWorkflow").GetInt32();
                }

                bool boolAreIntPkPiws = true;
                List<int> darrintPkPiw = null;
                if (
                    jsonWorkflow.TryGetProperty("arrintPkPiw", out json) &&
                    (int)jsonWorkflow.GetProperty("arrintPkPiw").ValueKind == 2
                    )
                {
                    darrintPkPiw = new List<int>();
                    int intI = 0;
                    //                                          //Check if all pk are int
                    /*WHILE-DO*/
                    while (
                        intI < jsonWorkflow.GetProperty("arrintPkPiw").GetArrayLength() && boolAreIntPkPiws
                        )
                    {
                        JsonElement json1 = jsonWorkflow.GetProperty("arrintPkPiw")[intI];

                        int intPkProcessInWorkflow;
                        if (
                            json1.TryGetInt32(out intPkProcessInWorkflow)
                            )
                        {
                            darrintPkPiw.Add(intPkProcessInWorkflow);
                        }
                        else
                        {
                            boolAreIntPkPiws = false;
                        }

                        intI = intI + 1;
                    }
                }

                if (
                    //                                      //All pk of piws are int
                    boolAreIntPkPiws
                    )
                {
                    //                                      //Get the printshop id from token.
                    var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                    String strPrintshopId = idClaim.Value;
                    PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                    //                                      //using is to release connection at the end
                    using (Odyssey2Context context = new Odyssey2Context())
                    {
                        //                                  //Starts a new transaction.
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                ProdtypProductType.subAddNewWorkflow(intnPkProduct, strWorkflowName, intnPkWorkflow, ps,
                                    darrintPkPiw, context, ref intStatus, ref strUserMessage, ref strDevMessage);

                                //                          //Commits all changes made to the database in the current
                                //                          //      transaction.
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
                            catch (CustomException custex)
                            {
                                //                          //Discards all changes made to the database in the current
                                //                          //      transaction.
                                dbContextTransaction.Rollback();

                                //                          //Making a log for the exception.
                                Tools.subExceptionHandler(custex, ref intStatus, ref strUserMessage, ref strDevMessage);
                            }
                            catch (Exception ex)
                            {
                                //                          //Discards all changes made to the database in the current
                                //                          //      transaction.
                                dbContextTransaction.Rollback();

                                //                          //Making a log for the exception.
                                Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                            }
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
        [HttpPost("[action]")]
        public IActionResult AddProcess(
            //                                              //PURPOSE:
            //                                              //Add one process to the given workflow.

            //                                              //URL: http://localhost/Odyssey2/Workflow/AddProcess
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intPkWorkflow":0,
            //                                              //          "intPkProcess":0
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Add a process to the workflow and returns the process 
            //                                              //      with its inputs and outputs.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            //                                              //      400 - BadRequest().

            //                                              //Json that contains all necessary data.
            [FromBody] JsonElement jsonProcess
            )
        {
            int intStatus = 400 ;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !Object.ReferenceEquals(null, jsonProcess) &&
                //                                          //Verify if the data is not null or empty.
                jsonProcess.TryGetProperty("intPkWorkflow", out json) &&
                (int)jsonProcess.GetProperty("intPkWorkflow").ValueKind == 4 &&
                jsonProcess.TryGetProperty("intPkProcess", out json) &&
                (int)jsonProcess.GetProperty("intPkProcess").ValueKind == 4
                )
            {
                int intPkWorkflow = jsonProcess.GetProperty("intPkWorkflow").GetInt32();
                int intPkProcess = jsonProcess.GetProperty("intPkProcess").GetInt32();

                //                                          //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                var idClaimSuperAdmin = User.Claims.FirstOrDefault(c => c.Type == "boolIsSuperAdmin");
                bool boolSuperAdmin = idClaimSuperAdmin.Value.ParseToBool(); ;

                intStatus = 401;
                strUserMessage = "Something is wrong.";
                strDevMessage = "Invalid data.";
                if (
                    ps != null
                    )
                {
                    try
                    {
                        int intPkWorkflowFinal;
                        ProdtypProductType.subAddProcess(intPkProcess, intPkWorkflow, ps, boolSuperAdmin, 
                            out intPkWorkflowFinal, ref intStatus, ref strUserMessage, ref strDevMessage);
                        obj = intPkWorkflowFinal;
                    }
                    catch (Exception ex)
                    {
                        //                                      //Making a log for the exception.
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
        [HttpPost("[action]")]
        public IActionResult DeleteProcess(
            //                                              //PURPOSE:
            //                                              //Delete a piw or a node from a workflow.

            //                                              //URL: http://localhost/Odyssey2/Workflow/DeleteProcess
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intPkProcessInWorkflow":null,
            //                                              //          "intnPkNode" : 31
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Delete a process or a node from a workflow.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            //                                              //Json that contains all necessary data.
            [FromBody] JsonElement jsonProcess
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;

            if (
                //                                          //Verify if jsonProcess is not null or a value is not empty.
                !((int)jsonProcess.ValueKind == 7) &&
                !((int)jsonProcess.ValueKind == 0)
                )
            {

                JsonElement json;
                int? intnPkProcessInWorkflow = null;
                if (
                    jsonProcess.TryGetProperty("intnPkProcessInWorkflow", out json) &&
                    (int)jsonProcess.GetProperty("intnPkProcessInWorkflow").ValueKind == 4
                    )
                {
                    intnPkProcessInWorkflow = jsonProcess.GetProperty("intnPkProcessInWorkflow").GetInt32();
                }

                int? intnPkNode = null;
                if (
                    jsonProcess.TryGetProperty("intnPkNode", out json) &&
                    (int)jsonProcess.GetProperty("intnPkNode").ValueKind == 4
                    )
                {
                    intnPkNode = jsonProcess.GetProperty("intnPkNode").GetInt32();
                }

                //                                          //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                var idClaimSuperAdmin = User.Claims.FirstOrDefault(c => c.Type == "boolIsSuperAdmin");
                bool boolSuperAdmin = idClaimSuperAdmin.Value.ParseToBool(); ;

                intStatus = 401;
                strUserMessage = "Something is wrong.";
                strDevMessage = "Invalid data.";
                if (
                    ps != null
                    )
                {
                    try
                    {
                        int intPkWorkflow;
                        ProtypProcessType.subDeleteProcess(intnPkProcessInWorkflow, intnPkNode, ps, boolSuperAdmin, 
                            this.iHubContext, out intPkWorkflow, ref intStatus, ref strUserMessage, 
                            ref strDevMessage);
                        obj = intPkWorkflow;
                    }
                    catch (Exception ex)
                    {
                        //                                      //Making a log for the exception.
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
        [HttpPost("[action]")]
        public IActionResult AddResourceAndGroup(
            //                                              //PURPOSE:
            //                                              //Add a resource to IO.

            //                                              //URL: http://localhost/Odyssey2/Workflow/
            //                                              //      AddResourceAndGroup
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intPkProcessInWorkflow": 1,
            //                                              //          "intPkResource": 135,
            //                                              //          "intPkEleetOrEleele":165,
            //                                              //          "boolIsEleet":false
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Add a resource to IO. Create groups of resources.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            //                                              //Json that contains all necessary data.
            [FromBody] JsonElement jsonResourceInWorkflow
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if jsonFilter is not null or a value is not empty.
                !((int)jsonResourceInWorkflow.ValueKind == 7) &&
                !((int)jsonResourceInWorkflow.ValueKind == 0) &&
                //                                          //Verify if the data is not null or empty.
                jsonResourceInWorkflow.TryGetProperty("intPkProcessInWorkflow", out json) &&
                (int)jsonResourceInWorkflow.GetProperty("intPkProcessInWorkflow").ValueKind == 4 &&
                jsonResourceInWorkflow.TryGetProperty("intPkResource", out json) &&
                (int)jsonResourceInWorkflow.GetProperty("intPkResource").ValueKind == 4 &&
                jsonResourceInWorkflow.TryGetProperty("intPkEleetOrEleele", out json) &&
                (int)jsonResourceInWorkflow.GetProperty("intPkEleetOrEleele").ValueKind == 4 &&
                jsonResourceInWorkflow.TryGetProperty("boolIsEleet", out json) &&
                (((int)jsonResourceInWorkflow.GetProperty("boolIsEleet").ValueKind == 5) ||
                ((int)jsonResourceInWorkflow.GetProperty("boolIsEleet").ValueKind == 6))
                )
            {
                int intPkProcessInWorkflow = jsonResourceInWorkflow.GetProperty("intPkProcessInWorkflow").GetInt32();
                int intPkResource = jsonResourceInWorkflow.GetProperty("intPkResource").GetInt32();
                int intPkEleetOrEleele = jsonResourceInWorkflow.GetProperty("intPkEleetOrEleele").GetInt32();
                bool boolIsEleet = jsonResourceInWorkflow.GetProperty("boolIsEleet").GetBoolean();

                //                                          //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                intStatus = 401;
                strUserMessage = "Something is wrong.";
                strDevMessage = "Invalid data.";
                if (
                    ps != null
                    )
                {
                    //                                      //using is to release connection at the end
                    using (Odyssey2Context context = new Odyssey2Context())
                    {
                        //                                  //Starts a new transaction.
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                Resjson3ResourceJson3 resjson3;
                                ProdtypProductType.subAddResourceAndCreateGroup(intPkProcessInWorkflow, intPkResource,
                                    intPkEleetOrEleele, boolIsEleet, ps, out resjson3, context, ref intStatus, 
                                    ref strUserMessage, ref strDevMessage);
                                obj = resjson3;

                                //                          //Commits all changes made to the database in the current
                                //                          //      transaction.
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
                                //                          //Discards all changes made to the database in the current
                                //                          //      transaction.
                                dbContextTransaction.Rollback();

                                //                          //Making a log for the exception.
                                Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                            }
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
        [HttpPost("[action]")]
        public IActionResult SetFinalProduct(
            //                                              //PURPOSE:
            //                                              //Set an IO as FinalProduct.

            //                                              //URL: http://localhost/Odyssey2/Workflow/
            //                                              //      SetFinalProduct
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intPkProcessInWorkflow": 1,
            //                                              //          "intPkEleetOrEleele":165,
            //                                              //          "boolIsEleet":false,
            //                                              //          "boolIsFinalProduct": null    
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            //                                              //Json that contains all necessary data.
            [FromBody] JsonElement jsonFinalProduct
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if jsonFilter is not null or a value is not empty.
                !((int)jsonFinalProduct.ValueKind == 7) &&
                !((int)jsonFinalProduct.ValueKind == 0) &&
                //                                          //Verify if the data is not null or empty.
                jsonFinalProduct.TryGetProperty("intPkProcessInWorkflow", out json) &&
                (int)jsonFinalProduct.GetProperty("intPkProcessInWorkflow").ValueKind == 4 &&
                jsonFinalProduct.TryGetProperty("intPkEleetOrEleele", out json) &&
                (int)jsonFinalProduct.GetProperty("intPkEleetOrEleele").ValueKind == 4 &&
                jsonFinalProduct.TryGetProperty("boolIsEleet", out json) &&
                (((int)jsonFinalProduct.GetProperty("boolIsEleet").ValueKind == 5) ||
                ((int)jsonFinalProduct.GetProperty("boolIsEleet").ValueKind == 6)) &&
                jsonFinalProduct.TryGetProperty("boolIsFinalProduct", out json) &&
                (((int)jsonFinalProduct.GetProperty("boolIsFinalProduct").ValueKind == 5) ||
                ((int)jsonFinalProduct.GetProperty("boolIsFinalProduct").ValueKind == 6))
                )
            {
                int intPkProcessInWorkflow = jsonFinalProduct.GetProperty("intPkProcessInWorkflow").GetInt32();
                int intPkEleetOrEleele = jsonFinalProduct.GetProperty("intPkEleetOrEleele").GetInt32();
                bool boolIsEleet = jsonFinalProduct.GetProperty("boolIsEleet").GetBoolean();
                bool boolIsFinalProduct = jsonFinalProduct.GetProperty("boolIsFinalProduct").GetBoolean();

                //                                          //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                var idClaimSuperAdmin = User.Claims.FirstOrDefault(c => c.Type == "boolIsSuperAdmin");
                bool boolSuperAdmin = idClaimSuperAdmin.Value.ParseToBool(); ;

                intStatus = 401;
                strUserMessage = "Something is wrong.";
                strDevMessage = "Invalid data.";
                if (
                    ps != null
                    )
                {
                    //                                      //using is to release connection at the end
                    using (Odyssey2Context context = new Odyssey2Context())
                    {
                        //                                  //Starts a new transaction.
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                int intPkWorkflow;
                                ProdtypProductType.subSetFinalProduct(intPkProcessInWorkflow, intPkEleetOrEleele,
                                    boolIsEleet, boolIsFinalProduct, ps, boolSuperAdmin, context, out intPkWorkflow, 
                                    ref intStatus, ref strUserMessage, ref strDevMessage);
                                obj = intPkWorkflow;

                                //                          //Commits all changes made to the database in the current
                                //                          //      transaction.
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
                                //                          //Discards all changes made to the database in the current
                                //                          //      transaction.
                                dbContextTransaction.Rollback();

                                //                          //Making a log for the exception.
                                Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                            }
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
        [HttpPost("[action]")]
        public IActionResult Link(
            //                                              //PURPOSE:
            //                                              //Link two IO.

            //                                              //URL: http://localhost/Odyssey2/Workflow/Link
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intnPkProcessInWorkflowO": 41,
            //                                              //          "intnPkEleetOrEleeleO": 135,
            //                                              //          "boolnIsEleetO": true,
            //                                              //          "intnPkNodeO":34,
            //                                              //          "intnPkProcessInWorkflowI": 41,
            //                                              //          "intnPkEleetOrEleeleI": 135,
            //                                              //          "boolnIsEleetI": true,
            //                                              //          "intnPkNodeI":23,
            //                                              //          "strConditionToApply":null,
            //                                              //          "numnSuperiorLimit": 2,
            //                                              //          "numnInferiorLimit": 3,
            //                                              //          "boolConditionAnd": true
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //To add a link in a workflow, to link the O of a process
            //                                              //      with the Input of another.
            //                                              //See detailed description and status in the method:  
            //                                              //      ProdtypProductType.subLinkProcessInWorkflow.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            //                                              //Json that contains all necessary data.
            [FromBody] JsonElement jsonLinkData
            )
        {
            int intStatus = 400;
            String strDevMessage = "Invalid data.";
            String strUserMessage = "Something is wrong.";
            Object obj = null;

            JsonElement json;
            if (
                //                                          //Verify if jsonFilter is not null or a value is not empty.
                !((int)jsonLinkData.ValueKind == 7) &&
                !((int)jsonLinkData.ValueKind == 0) 
                //jsonLinkData.TryGetProperty("boolConditionAnd", out json) &&
                //(((int)jsonLinkData.GetProperty("boolConditionAnd").ValueKind == 5) ||
                //((int)jsonLinkData.GetProperty("boolConditionAnd").ValueKind == 6))
                )
            {
                //                                          //Find property.
                //bool boolConditionAnd = jsonLinkData.GetProperty("boolConditionAnd").GetBoolean();

                //                                          //Get the data form the json.
                int? intnPkProcessInWorkflowO = null;
                if (
                    jsonLinkData.TryGetProperty("intnPkProcessInWorkflowO", out json) &&
                    (int)jsonLinkData.GetProperty("intnPkProcessInWorkflowO").ValueKind == 4
                    )
                {
                    intnPkProcessInWorkflowO = jsonLinkData.GetProperty("intnPkProcessInWorkflowO").GetInt32();
                }

                int? intnPkEleetOrEleeleO = null;
                if (
                    jsonLinkData.TryGetProperty("intnPkEleetOrEleeleO", out json) &&
                    (int)jsonLinkData.GetProperty("intnPkEleetOrEleeleO").ValueKind == 4
                    )
                {
                    intnPkEleetOrEleeleO = jsonLinkData.GetProperty("intnPkEleetOrEleeleO").GetInt32();
                }


                bool? boolnIsEleetO = null;
                if (
                    jsonLinkData.TryGetProperty("boolnIsEleetO", out json) &&
                    ((int)jsonLinkData.GetProperty("boolnIsEleetO").ValueKind == 5 ||
                    (int)jsonLinkData.GetProperty("boolnIsEleetO").ValueKind == 6)
                    )
                {
                    boolnIsEleetO = jsonLinkData.GetProperty("boolnIsEleetO").GetBoolean();
                }

                int? intnPkNodeO = null;
                if (
                    jsonLinkData.TryGetProperty("intnPkNodeO", out json) &&
                    (int)jsonLinkData.GetProperty("intnPkNodeO").ValueKind == 4
                    )
                {
                    intnPkNodeO = jsonLinkData.GetProperty("intnPkNodeO").GetInt32();
                }

                int? intnPkProcessInWorkflowI = null;
                if (
                    jsonLinkData.TryGetProperty("intnPkProcessInWorkflowI", out json) &&
                    (int)jsonLinkData.GetProperty("intnPkProcessInWorkflowI").ValueKind == 4
                    )
                {
                    intnPkProcessInWorkflowI = jsonLinkData.GetProperty("intnPkProcessInWorkflowI").GetInt32();
                }

                int? intnPkEleetOrEteleI = null;
                if (
                    jsonLinkData.TryGetProperty("intnPkEleetOrEleeleI", out json) &&
                    (int)jsonLinkData.GetProperty("intnPkEleetOrEleeleI").ValueKind == 4
                    )
                {
                    intnPkEleetOrEteleI = jsonLinkData.GetProperty("intnPkEleetOrEleeleI").GetInt32();
                }

                
                bool? boolnIsEleetI = null;
                if (
                    jsonLinkData.TryGetProperty("boolnIsEleetI", out json) &&
                    ((int)jsonLinkData.GetProperty("boolnIsEleetI").ValueKind == 5 ||
                    (int)jsonLinkData.GetProperty("boolnIsEleetI").ValueKind == 6)
                    )
                {
                    boolnIsEleetI = jsonLinkData.GetProperty("boolnIsEleetI").GetBoolean();
                }

                int? intnPkNodeI = null;
                if (
                    jsonLinkData.TryGetProperty("intnPkNodeI", out json) &&
                    (int)jsonLinkData.GetProperty("intnPkNodeI").ValueKind == 4
                    )
                {
                    intnPkNodeI = jsonLinkData.GetProperty("intnPkNodeI").GetInt32();
                }

                //String strConditionToApply = null;
                //if (
                //    jsonLinkData.TryGetProperty("strConditionToApply", out json) &&
                //    (int)jsonLinkData.GetProperty("strConditionToApply").ValueKind == 3
                //    )
                //{
                //    strConditionToApply = jsonLinkData.GetProperty("strConditionToApply").GetString();
                //}

                //double? numnSuperiorLimit = null;
                //if (
                //    jsonLinkData.TryGetProperty("numnSuperiorLimit", out json) &&
                //    (int)jsonLinkData.GetProperty("numnSuperiorLimit").ValueKind == 4
                //    )
                //{
                //    numnSuperiorLimit = jsonLinkData.GetProperty("numnSuperiorLimit").GetDouble();
                //}

                //double? numnInferiorLimit = null;
                //if (
                //    jsonLinkData.TryGetProperty("numnInferiorLimit", out json) &&
                //    (int)jsonLinkData.GetProperty("numnInferiorLimit").ValueKind == 4
                //    )
                //{
                //    numnInferiorLimit = jsonLinkData.GetProperty("numnInferiorLimit").GetDouble();
                //}

                //                                          //Get conditions to apply.
                GpcondjsonGroupConditionJson gpcondition = null;
                if (
                    jsonLinkData.TryGetProperty("condition", out json) &&
                    (int)jsonLinkData.GetProperty("condition").ValueKind == 3
                    )
                {
                    gpcondition = JsonSerializer.Deserialize<GpcondjsonGroupConditionJson>(
                        jsonLinkData.GetProperty("condition").ToString());
                }

                //                                              //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                var idClaimSuperAdmin = User.Claims.FirstOrDefault(c => c.Type == "boolIsSuperAdmin");
                bool boolSuperAdmin = idClaimSuperAdmin.Value.ParseToBool(); ;

                intStatus = 401;
                strDevMessage = "Invalid data.";
                strUserMessage = "Something is wrong.";
                if (
                    ps != null
                    )
                {
                    try
                    {
                        WfandlinkjsonWorkflowPkAndLinkJson wfandlinkjson;
                        ProdtypProductType.subLinkProcessInWorkflow(ps, boolSuperAdmin, intnPkProcessInWorkflowO,
                            intnPkEleetOrEleeleO, boolnIsEleetO, intnPkNodeO, intnPkProcessInWorkflowI, intnPkEleetOrEteleI,
                            boolnIsEleetI, intnPkNodeI, gpcondition, ref intStatus, ref strDevMessage, ref strUserMessage,
                            out wfandlinkjson);
                        obj = wfandlinkjson;
                    }
                    catch (Exception ex)
                    {
                        //                                      //Making a log for the exception.
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
        [HttpPost("[action]")]
        public IActionResult SetResourceForAJob(
            //                                              //PURPOSE:
            //                                              //Set a resource in a job workflow.

            //                                              //URL: http://localhost/Odyssey2/Workflow/SetResourceForAJob
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intJobId": 414789,
            //                                              //          "intPkProcessInWorkflow": 41,
            //                                              //          "intnPkResource": 135,
            //                                              //          "intPkEleetOrEleele":165,
            //                                              //          "boolIsEleet":false
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Set a resource in a process for a job workflow.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            //                                              //Dynamic object that contains all necessary data.
            [FromBody] JsonElement jsonResourceInJobWorkflow
            )
        {
            int intStatus = 400;
            String strUserMessage = "Invalid data.";
            String strDevMessage = "";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !Object.ReferenceEquals(null, jsonResourceInJobWorkflow) &&
                //                                          //Verify if the data is not null or empty.
                jsonResourceInJobWorkflow.TryGetProperty("intJobId", out json) &&
                (int)jsonResourceInJobWorkflow.GetProperty("intJobId").ValueKind == 4 &&
                jsonResourceInJobWorkflow.TryGetProperty("intPkProcessInWorkflow", out json) &&
                (int)jsonResourceInJobWorkflow.GetProperty("intPkProcessInWorkflow").ValueKind == 4 &&
                jsonResourceInJobWorkflow.TryGetProperty("intPkEleetOrEleele", out json) &&
                (int)jsonResourceInJobWorkflow.GetProperty("intPkEleetOrEleele").ValueKind == 4 &&
                jsonResourceInJobWorkflow.TryGetProperty("boolIsEleet", out json) &&
                (((int)jsonResourceInJobWorkflow.GetProperty("boolIsEleet").ValueKind == 5) ||
                ((int)jsonResourceInJobWorkflow.GetProperty("boolIsEleet").ValueKind == 6))
                )
            {
                int intJobId = jsonResourceInJobWorkflow.GetProperty("intJobId").GetInt32();
                int intPkProcessInWorkflow = jsonResourceInJobWorkflow.GetProperty("intPkProcessInWorkflow").GetInt32();
                int? intnPkResource = null;
                if (
                    jsonResourceInJobWorkflow.TryGetProperty("intnPkResource", out json) &&
                    (int)jsonResourceInJobWorkflow.GetProperty("intnPkResource").ValueKind == 4
                    )
                {
                    intnPkResource = jsonResourceInJobWorkflow.GetProperty("intnPkResource").GetInt32();
                }
                int intPkEleetOrEleele = jsonResourceInJobWorkflow.GetProperty("intPkEleetOrEleele").GetInt32();
                bool boolIsEleet = jsonResourceInJobWorkflow.GetProperty("boolIsEleet").GetBoolean();

                //                                              //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;

                try
                {
                    ProdtypProductType.subSetResourceForAJob(intJobId, strPrintshopId, intPkProcessInWorkflow,
                    intnPkResource, intPkEleetOrEleele, boolIsEleet, this.configuration, ref intStatus,
                    ref strUserMessage, ref strDevMessage);
                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }                
            }
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage, obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult SetPriceForAJob(
            //                                              //PURPOSE:
            //                                              //Set and update job´s price.

            //                                              //URL: http://localhost/Odyssey2/Workflow/SetPriceForAJob
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intJobId":0,
            //                                              //          "numPrice":0,
            //                                              //          "intPkWorkflow":0
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Set and update the price for a Job.
            //                                              //

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            //                                              //Json that contains all data.
            [FromBody] JsonElement jsonPrice
            )
        {
            //                                              //Get the contact id from token.
            var idClaimContact = User.Claims.FirstOrDefault(c => c.Type == "intContactId");
            int intContactId = idClaimContact.Value.ParseToInt();

            //                                              //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;

            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "A value is not valid.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !((int)jsonPrice.ValueKind == 7) &&
                !((int)jsonPrice.ValueKind == 0) &&
                //                                          //Verify if the data is not null or empty.
                jsonPrice.TryGetProperty("intJobId", out json) &&
                (int)jsonPrice.GetProperty("intJobId").ValueKind == 4 &&
                jsonPrice.TryGetProperty("numPrice", out json) &&
                (int)jsonPrice.GetProperty("numPrice").ValueKind == 4 &&
                jsonPrice.TryGetProperty("intPkWorkflow", out json) &&
                (int)jsonPrice.GetProperty("intPkWorkflow").ValueKind == 4
                )
            {
                int intJobId = jsonPrice.GetProperty("intJobId").GetInt32();
                double numPrice = jsonPrice.GetProperty("numPrice").GetDouble();
                int intPkWorkflow = jsonPrice.GetProperty("intPkWorkflow").GetInt32();

                //                                          //Value for attribute saved in the ascendant elements.
                String strDescription = "";
                if (
                    jsonPrice.TryGetProperty("strDescription", out json) &&
                    (int)jsonPrice.GetProperty("strDescription").ValueKind == 3
                    )
                {
                    strDescription = jsonPrice.GetProperty("strDescription").GetString();
                }

                int? intnEstimateId = null;
                if (
                    jsonPrice.TryGetProperty("intnEstimateId", out json) &&
                    (int)jsonPrice.GetProperty("intnEstimateId").ValueKind == 4
                    )
                {
                    intnEstimateId = jsonPrice.GetProperty("intnEstimateId").GetInt32();
                }

                int? intnCopyNumber = null;
                if (
                    jsonPrice.TryGetProperty("intnCopyNumber", out json) &&
                    (int)jsonPrice.GetProperty("intnCopyNumber").ValueKind == 4
                    )
                {
                    intnCopyNumber = jsonPrice.GetProperty("intnCopyNumber").GetInt32();
                }

                //                                          //using is to release connection at the end
                using (Odyssey2Context context = new Odyssey2Context())
                {
                    //                                      //Starts a new transaction.
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            ProdtypProductType.subSetPrice(intJobId, strPrintshopId, numPrice, strDescription, 
                                intContactId, this.configuration, intPkWorkflow, intnEstimateId, intnCopyNumber, 
                                context, ref intStatus, ref strUserMessage, ref strDevMessage);
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

                            intStatus = 407;
                            strUserMessage = "Something is wrong.";
                            strDevMessage = ex.Message;
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
        [HttpPost("[action]")]
        public IActionResult ResetPrice(
            //                                              //PURPOSE:
            //                                              //Rseset price in a job or estimate.

            //                                              //URL: http://localhost/Odyssey2/Workflow/ResetPrice
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intJobId":0,
            //                                              //          "intPkWorkflow": 0,
            //                                              //          "intnEstimateId": 0
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Add a special row to DB showing that someone reset a 
            //                                              //      price.
            //                                              //

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            //                                              //Json that contains all data.
            [FromBody] JsonElement jsonPrice
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "A value is not valid.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !((int)jsonPrice.ValueKind == 7) &&
                !((int)jsonPrice.ValueKind == 0) &&
                //                                          //Verify if the data is not null or empty.
                jsonPrice.TryGetProperty("intJobId", out json) &&
                (int)jsonPrice.GetProperty("intJobId").ValueKind == 4 &&
                jsonPrice.TryGetProperty("intPkWorkflow", out json) &&
                (int)jsonPrice.GetProperty("intPkWorkflow").ValueKind == 4
                )
            {
                int intJobId = jsonPrice.GetProperty("intJobId").GetInt32();
                int intPkWorkflow = jsonPrice.GetProperty("intPkWorkflow").GetInt32();

                int? intnEstimateId = null;
                if (
                    jsonPrice.TryGetProperty("intnEstimateId", out json) &&
                    (int)jsonPrice.GetProperty("intnEstimateId").ValueKind == 4
                    )
                {
                    intnEstimateId = jsonPrice.GetProperty("intnEstimateId").GetInt32();
                }

                int? intnCopyNumber = null;
                if (
                    jsonPrice.TryGetProperty("intnCopyNumber", out json) &&
                    (int)jsonPrice.GetProperty("intnCopyNumber").ValueKind == 4
                    )
                {
                    intnCopyNumber = jsonPrice.GetProperty("intnCopyNumber").GetInt32();
                }

                //                                          //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;

                //                                          //Get the contact id from token.
                var idClaimContact = User.Claims.FirstOrDefault(c => c.Type == "intContactId");
                int intContactId = idClaimContact.Value.ParseToInt();

                //                                          //using is to release connection at the end
                using (Odyssey2Context context = new Odyssey2Context())
                {
                    //                                      //Starts a new transaction.
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            ProdtypProductType.subResetPrice(intJobId, intPkWorkflow, intnEstimateId, intnCopyNumber,
                                strPrintshopId, intContactId, this.configuration, context, ref intStatus,
                                ref strUserMessage, ref strDevMessage);
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

                            intStatus = 407;
                            strUserMessage = "Something is wrong.";
                            strDevMessage = ex.Message;
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
        [HttpPost("[action]")]
        public IActionResult DeleteResourcesForIO(
           //                                              //PURPOSE:
           //                                              //Delete the resource or group in an IO.

           //                                              //URL: http://localhost/Odyssey2/Workflow
           //                                              //      /DeleteResourcesForIO/?intPk=328

           //                                              //Use a JSON like this:
           //                                              //      {
           //                                              //          "intPkProcessInWorkflow": 1,
           //                                              //          "intPkResource": 135,
           //                                              //          "intPkEleetOrEleele":165,
           //                                              //          "boolIsEleet":false,
           //                                              //          "boolConfirmDelete":true
           //                                              //      }

           //                                              //DESCRIPTION:
           //                                              //Delete the resource or group in an IO.

           //                                              //RETURNS:
           //                                              //      200 - Ok

           //                                              //Json that contains all data.
           [FromBody] JsonElement jsonResource
           )
        {
            //                                              //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

            int intStatus = 400;
            String strDevMessage = "Invalid data.";
            String strUserMessage = "Something is wrong.";
            Object obj = null;
            JsonElement json;

            if (
                //                                          //Verify if the object is not null.
                !Object.ReferenceEquals(null, jsonResource) &&
                //                                          //Verify if the data is not null or empty.
                jsonResource.TryGetProperty("intPkProcessInWorkflow", out json) &&
                (int)jsonResource.GetProperty("intPkProcessInWorkflow").ValueKind == 4 &&
                jsonResource.TryGetProperty("intPkEleetOrEleele", out json) &&
                (int)jsonResource.GetProperty("intPkEleetOrEleele").ValueKind == 4 &&
                jsonResource.TryGetProperty("intPkResource", out json) &&
                (int)jsonResource.GetProperty("intPkResource").ValueKind == 4 &&
                jsonResource.TryGetProperty("boolIsEleet", out json) &&
                (((int)jsonResource.GetProperty("boolIsEleet").ValueKind == 5) ||
                ((int)jsonResource.GetProperty("boolIsEleet").ValueKind == 6)) 
                )
            {
                int intPkProcessInWorkflow = jsonResource.GetProperty("intPkProcessInWorkflow").GetInt32();
                int intPkEleetOrEleele = jsonResource.GetProperty("intPkEleetOrEleele").GetInt32();
                int intPkResource = jsonResource.GetProperty("intPkResource").GetInt32();
                bool boolIsEleet = jsonResource.GetProperty("boolIsEleet").GetBoolean();

                intStatus = 401;
                strUserMessage = "Something is wrong.";
                strDevMessage = "A value is not valid.";
                if (
                    ps != null
                    )
                {
                    //                                      //using is to release connection at the end
                    using (Odyssey2Context context = new Odyssey2Context())
                    {
                        //                                  //Starts a new transaction.
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                int intPkWorkflow;
                                ProdtypProductType.subDeleteResourcesForIO(intPkProcessInWorkflow, intPkEleetOrEleele,
                                    intPkResource, boolIsEleet, ps, out intPkWorkflow, context, ref intStatus,
                                    ref strUserMessage, ref strDevMessage);
                                obj = intPkWorkflow;
                                //                          //Commits all changes made to the database in the current
                                //                          //      transaction.
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
                                //                          //Discards all changes made to the database in the current
                                //                          //      transaction.
                                dbContextTransaction.Rollback();

                                //                          //Making a log for the exception.
                                Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                            }
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
        [HttpGet("[action]")]
        public IActionResult ResourceIsAddable(
            //                                              //PURPOSE:
            //                                              //Service to confirm if the resource is addable and if the 
            //                                              //      the workflow has or not estimates.

            //                                              //URL: http://localhost/Odyssey2/Workflow
            //                                              //      /ResourceIsAddable/

            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intPkProcessInWorkflow": 1,
            //                                              //          "intPkResource": 135,
            //                                              //          "intPkEleetOrEleele":165,
            //                                              //          "boolIsEleet":false
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Validate if this resource is addable and if the workflow
            //                                              //      has estimates.

            //                                              //RETURNS:
            //                                              //      200 - Ok

            //                                              //Json that contains all data.
            [FromBody] JsonElement jsonResource
            )
        {
            int intStatus = 400;
            String strDevMessage = "Invalid data.";
            String strUserMessage = "Something is wrong.";
            Object obj = null;
            JsonElement json;

            if (
                //                                          //Verify if the object is not null.
                !Object.ReferenceEquals(null, jsonResource) &&
                //                                          //Verify if the data is not null or empty.
                jsonResource.TryGetProperty("intPkProcessInWorkflow", out json) &&
                (int)jsonResource.GetProperty("intPkProcessInWorkflow").ValueKind == 4 &&
                jsonResource.TryGetProperty("intPkEleetOrEleele", out json) &&
                (int)jsonResource.GetProperty("intPkEleetOrEleele").ValueKind == 4 &&
                jsonResource.TryGetProperty("intPkResource", out json) &&
                (int)jsonResource.GetProperty("intPkResource").ValueKind == 4 &&
                jsonResource.TryGetProperty("intJobId", out json) &&
                (int)jsonResource.GetProperty("intJobId").ValueKind == 4 &&
                jsonResource.TryGetProperty("boolIsEleet", out json) &&
                (((int)jsonResource.GetProperty("boolIsEleet").ValueKind == 5) ||
                ((int)jsonResource.GetProperty("boolIsEleet").ValueKind == 6))
                )
            {
                int intPkProcessInWorkflow = jsonResource.GetProperty("intPkProcessInWorkflow").GetInt32();
                int intPkEleetOrEleele = jsonResource.GetProperty("intPkEleetOrEleele").GetInt32();
                int intPkResource = jsonResource.GetProperty("intPkResource").GetInt32();
                int intJobId = jsonResource.GetProperty("intJobId").GetInt32();
                bool boolIsEleet = jsonResource.GetProperty("boolIsEleet").GetBoolean();

                try
                {
                    bool boolResourceIsAddable;
                    ProdtypProductType.subResourceIsAddable(intPkProcessInWorkflow, intPkEleetOrEleele, intPkResource,
                        intJobId, boolIsEleet, out boolResourceIsAddable, ref intStatus, ref strUserMessage,
                        ref strDevMessage);
                    obj = boolResourceIsAddable;
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
        [HttpGet("[action]")]
        public IActionResult ResourceIsDispensable(
            //                                              //PURPOSE:
            //                                              //Service to confirm if the resource has periods and if the 
            //                                              //      the workflow has or not estimates.

            //                                              //URL: http://localhost/Odyssey2/Workflow
            //                                              //      /ResourceIsDispensable/

            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intPkProcessInWorkflow": 1,
            //                                              //          "intPkResource": 135,
            //                                              //          "intPkEleetOrEleele":165,
            //                                              //          "boolIsEleet":false
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Validate if this resource has periods and if the workflow
            //                                              //      has estimates.

            //                                              //RETURNS:
            //                                              //      200 - Ok

            //                                              //Json that contains all data.
            [FromBody] JsonElement jsonResource
            )
        {
            int intStatus = 400;
            String strDevMessage = "Invalid data.";
            String strUserMessage = "Something is wrong.";
            Object obj = null;
            JsonElement json;

            if (
                //                                          //Verify if the object is not null.
                !Object.ReferenceEquals(null, jsonResource) &&
                //                                          //Verify if the data is not null or empty.
                jsonResource.TryGetProperty("intPkProcessInWorkflow", out json) &&
                (int)jsonResource.GetProperty("intPkProcessInWorkflow").ValueKind == 4 &&
                jsonResource.TryGetProperty("intPkEleetOrEleele", out json) &&
                (int)jsonResource.GetProperty("intPkEleetOrEleele").ValueKind == 4 &&
                jsonResource.TryGetProperty("intPkResource", out json) &&
                (int)jsonResource.GetProperty("intPkResource").ValueKind == 4 &&
                jsonResource.TryGetProperty("boolIsEleet", out json) &&
                (((int)jsonResource.GetProperty("boolIsEleet").ValueKind == 5) ||
                ((int)jsonResource.GetProperty("boolIsEleet").ValueKind == 6))
                )
            {
                int intPkProcessInWorkflow = jsonResource.GetProperty("intPkProcessInWorkflow").GetInt32();
                int intPkEleetOrEleele = jsonResource.GetProperty("intPkEleetOrEleele").GetInt32();
                int intPkResource = jsonResource.GetProperty("intPkResource").GetInt32();
                bool boolIsEleet = jsonResource.GetProperty("boolIsEleet").GetBoolean();

                try
                {
                    ResdisjsonResourceIsDispensableJson reshperjsonPeriodsInJobs;
                    ProdtypProductType.subResourceIsDispensable(intPkProcessInWorkflow, intPkEleetOrEleele, intPkResource,
                        boolIsEleet, out reshperjsonPeriodsInJobs, ref intStatus, ref strUserMessage, ref strDevMessage);
                    obj = reshperjsonPeriodsInJobs;
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
        [HttpPost("[action]")]
        public IActionResult UpdateProcessStage(
           //                                               //PURPOSE:
           //                                               //Set or update a processinworkflowforajob' stage.

           //                                               //URL: http://localhost/Odyssey2/Process
           //                                               //      /ChangeStageForAProcessInWorkflowJob
           //                                               //Method: POST.
           //                                               //Use a JSON like this:
           //                                               //      {
           //                                               //          "intPkProcessInWorkflow" : 3,
           //                                               //          "intJobId" : 425147,
           //                                               //          "intStage" : 1,
           //                                               //      }

           //                                               //DESCRIPTION:
           //                                               //Change the process's stage.
           //                                               //  (Started or Completed).

           //                                               //RETURNS:
           //                                               //      200 - Ok

           //                                               //Dynamic json that contains a Json string with all data.

           [FromBody] JsonElement proData
           )
        {
            //                                              //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if pk is not null and greater that 0.
                !Object.ReferenceEquals(null, proData) &&
                proData.TryGetProperty("intPkProcessInWorkflow", out json) &&
                (int)proData.GetProperty("intPkProcessInWorkflow").ValueKind == 4 &&
                proData.TryGetProperty("intStage", out json) &&
                (int)proData.GetProperty("intStage").ValueKind == 4 &&
                ps != null
                )
            {
                //                                          //Get the data.
                int intPkProcessInWorkflow = proData.GetProperty("intPkProcessInWorkflow").GetInt32();
                int intJobId = proData.GetProperty("intJobId").GetInt32();
                int intStage = proData.GetProperty("intStage").GetInt32();

                //                                              //Get the contact id from token.
                var idClaimContact = User.Claims.FirstOrDefault(c => c.Type == "intContactId");
                int intContactId = idClaimContact.Value.ParseToInt();

                //                                          //using is to release connection at the end
                using (Odyssey2Context context = new Odyssey2Context())
                {
                    //                                      //Starts a new transaction.
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            bool boolAskEmailNeedsToBeSent = false;
                            //                                          //Method to set or update process's stage.
                            JobJob.subUpdateProcessStage(ps, intContactId, intPkProcessInWorkflow, intJobId, intStage,
                                context, this.configuration, this.iHubContext, ref boolAskEmailNeedsToBeSent,
                                ref intStatus, ref strUserMessage, ref strDevMessage);
                            obj = boolAskEmailNeedsToBeSent;

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

                            intStatus = 407;
                            strUserMessage = "Something is wrong.";
                            strDevMessage = ex.Message;
                        }
                    }
                }
            }

            //                                              //Response to the frontend.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult UpdateName(
            //                                              //PURPOSE:
            //                                              //Change workflow´s name.

            //                                              //URL: http://localhost/Odyssey2/Workflow
            //                                              //      /UpdateName
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intPkWorkflow":1,
            //                                              //          "strName": "Announcements2"
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Change workflow´s name.
            //                                              //

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            //                                              //Json that contains all data.
            [FromBody] JsonElement jsonName
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "A value is not valid.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !Object.ReferenceEquals(null, jsonName) &&
                //                                          //Verify if the data is not null or empty.
                jsonName.TryGetProperty("intPkWorkflow", out json) &&
                (int)jsonName.GetProperty("intPkWorkflow").ValueKind == 4 &&
                jsonName.TryGetProperty("strWorkflowName", out json) &&
                (int)jsonName.GetProperty("strWorkflowName").ValueKind == 3
                )
            {
                int intPkWorkflow = jsonName.GetProperty("intPkWorkflow").GetInt32();
                String strWorkflowName = jsonName.GetProperty("strWorkflowName").GetString();

                //                                          //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                intStatus = 401;
                strUserMessage = "Something is wrong.";
                strDevMessage = "Invalid data.";
                if (
                    ps != null
                    )
                {
                    try
                    {
                        ProdtypProductType.subUpdateName(intPkWorkflow, strWorkflowName, ps, ref intStatus,
                            ref strUserMessage, ref strDevMessage);
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
        [HttpPost("[action]")]
        public IActionResult Delete(
            //                                              //PURPOSE:
            //                                              //Delete a workflow.

            //                                              //URL: http://localhost/Odyssey2/Workflow
            //                                              //      /Delete
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intPkWorkflow":1
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Delete a workflow.
            //                                              //

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            //                                              //Json that contains all data.
            [FromBody] JsonElement jsonWorkflow
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "A value is not valid.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !Object.ReferenceEquals(null, jsonWorkflow) &&
                //                                          //Verify if the data is not null or empty.
                jsonWorkflow.TryGetProperty("intPkWorkflow", out json) &&
                (int)jsonWorkflow.GetProperty("intPkWorkflow").ValueKind == 4 
                )
            {
                int intPkWorkflow = jsonWorkflow.GetProperty("intPkWorkflow").GetInt32();

                var idClaimSuperAdmin = User.Claims.FirstOrDefault(c => c.Type == "boolIsSuperAdmin");
                bool boolIsSuperAdmin = idClaimSuperAdmin.Value.ParseToBool();

                //                                          //using is to release connection at the end
                using (Odyssey2Context context = new Odyssey2Context())
                {
                    //                                      //Starts a new transaction.
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            ProdtypProductType.subDelete(intPkWorkflow, boolIsSuperAdmin, context, ref intStatus,
                                ref strUserMessage, ref strDevMessage);
                            //                              //Commits all changes made to the database in the current
                            //                              //      transaction.
                            dbContextTransaction.Commit();
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
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult MakeDefault(
            //                                              //PURPOSE:
            //                                              //Transforms a workflow into default.

            //                                              //URL: http://localhost/Odyssey2/Workflow
            //                                              //      /MakeDefault
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intPkWorkflow":1
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Transforms a workflow into default.
            //                                              //

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            //                                              //Json that contains all data.
            [FromBody] JsonElement jsonWorkflow
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "A value is not valid.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !((int)jsonWorkflow.ValueKind == 7) &&
                !((int)jsonWorkflow.ValueKind == 0) &&
                //                                          //Verify if the data is not null or empty.
                jsonWorkflow.TryGetProperty("intPkWorkflow", out json) &&
                (int)jsonWorkflow.GetProperty("intPkWorkflow").ValueKind == 4
                )
            {
                int intPkWorkflow = jsonWorkflow.GetProperty("intPkWorkflow").GetInt32();

                //                                          //using is to release connection at the end
                using (Odyssey2Context context = new Odyssey2Context())
                {
                    //                                      //Starts a new transaction.
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            ProdtypProductType.subMakeDefault(intPkWorkflow, context, ref intStatus,
                                ref strUserMessage, ref strDevMessage);

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

            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult AddNode(
            //                                              //PURPOSE:
            //                                              //Add a node in the workflow.

            //                                              //URL: http://localhost/Odyssey2/Workflow
            //                                              //      /AddNode
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intPkWorkflow":1,
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Add a node in the workflow.
            //                                              //

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            //                                              //Json that contains all data.
            [FromBody] JsonElement jsonNode
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "A value is not valid.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !((int)jsonNode.ValueKind == 7) &&
                !((int)jsonNode.ValueKind == 0) &&
                //                                          //Verify if the data is not null or empty.
                jsonNode.TryGetProperty("intPkWorkflow", out json) &&
                (int)jsonNode.GetProperty("intPkWorkflow").ValueKind == 4
                )
            {
                int intPkWorkflow = jsonNode.GetProperty("intPkWorkflow").GetInt32();

                //                                          //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                var idClaimSuperAdmin = User.Claims.FirstOrDefault(c => c.Type == "boolIsSuperAdmin");
                bool boolSuperAdmin = idClaimSuperAdmin.Value.ParseToBool(); ;

                intStatus = 401;
                strUserMessage = "Something is wrong.";
                strDevMessage = "A value is not valid.";
                if (
                    ps != null
                    )
                {
                    //                                      //using is to release connection at the end
                    using (Odyssey2Context context = new Odyssey2Context())
                    {
                        //                                      //Starts a new transaction.
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                NodejsonNodeJson nodejson;
                                ProdtypProductType.subAddNode(intPkWorkflow, ps, boolSuperAdmin, context, out nodejson, 
                                    ref intStatus, ref strUserMessage, ref strDevMessage);
                                obj = nodejson;
                                //                          //Commits all changes made to the database in the current
                                //                          //      transaction.
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
                                //                          //Discards all changes made to the database in the current
                                //                          //      transaction.
                                dbContextTransaction.Rollback();

                                //                          //Making a log for the exception.
                                Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                            }
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
        [HttpPost("[action]")]
        public IActionResult Unlink(
            //                                              //PURPOSE:
            //                                              //Unlink.

            //                                              //URL: http://localhost/Odyssey2/Workflow
            //                                              //      /Unlink

            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intPkOut":1,
            //                                              //          "intPkIn":2
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Unlink.

            //                                              //RETURNS:
            //                                              //      200 - Ok

            [FromBody] JsonElement jsonLink
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !((int)jsonLink.ValueKind == 7) &&
                !((int)jsonLink.ValueKind == 0) &&
                //                                          //Verify if the data is not null or empty.
                jsonLink.TryGetProperty("intPkOut", out json) &&
                (int)jsonLink.GetProperty("intPkOut").ValueKind == 4 &&
                jsonLink.TryGetProperty("intPkIn", out json) &&
                (int)jsonLink.GetProperty("intPkIn").ValueKind == 4
                )
            {
                int intPkOut = jsonLink.GetProperty("intPkOut").GetInt32();
                int intPkIn = jsonLink.GetProperty("intPkIn").GetInt32();

                //                                          //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                intStatus = 401;
                strUserMessage = "Something is wrong.";
                strDevMessage = "A value is not valid.";
                if (
                    ps != null
                    )
                {
                    //                                      //using is to release connection at the end
                    using (Odyssey2Context context = new Odyssey2Context())
                    {
                        //                                  //Starts a new transaction.
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                int intPkWorkflowFinal;
                                ProdtypProductType.subUnlink(intPkOut, intPkIn, ps, out intPkWorkflowFinal, context,
                                    ref intStatus, ref strUserMessage, ref strDevMessage);
                                obj = intPkWorkflowFinal;

                                //                          //Commits all changes made to the database in the current
                                //                          //      transaction.
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
                                //                          //Discards all changes made to the database in the current
                                //                          //      transaction.
                                dbContextTransaction.Rollback();

                                //                          //Making a log for the exception.
                                Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                            }
                        }
                    }
                }                    
            }

            //                                              //Response for the front web.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult SetConditionToLink(
            //                                              //PURPOSE:
            //                                              //Add or update condition to apply, condition quantity
            //                                              //      and conditionand.

            //                                              //URL: http://localhost/Odyssey2/Workflow
            //                                              //      /SetConditionToLink

            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intPkOut":1,
            //                                              //          "intPkIn":2
            //                                              //          "strConditionToApply":"(Size|==|8 x 5 in.)|&&|
            //                                              //          (Size|==|8 x 11 in.)",
            //                                              //          "numnSuperiorLimit": 100,
            //                                              //          "numnInferiorLimit": 10,
            //                                              //          "boolConditionAnd": true
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Add or update condition to apply and condition quantity.

            //                                              //RETURNS:
            //                                              //      200 - Ok

            [FromBody] JsonElement jsonCondition
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !((int)jsonCondition.ValueKind == 7) &&
                !((int)jsonCondition.ValueKind == 0) &&
                //                                          //Verify if the data is not null or empty.
                jsonCondition.TryGetProperty("intPkOut", out json) &&
                (int)jsonCondition.GetProperty("intPkOut").ValueKind == 4 &&
                jsonCondition.TryGetProperty("intPkIn", out json) &&
                (int)jsonCondition.GetProperty("intPkIn").ValueKind == 4
                )
            {
                int intPkOut = jsonCondition.GetProperty("intPkOut").GetInt32();
                int intPkIn = jsonCondition.GetProperty("intPkIn").GetInt32();                

                //                                          //Get conditions to apply.
                GpcondjsonGroupConditionJson gpcondition = null;
                if (
                    jsonCondition.TryGetProperty("condition", out json) &&
                    (int)jsonCondition.GetProperty("condition").ValueKind == 3
                    )
                {
                    gpcondition = JsonSerializer.Deserialize<GpcondjsonGroupConditionJson>(
                        jsonCondition.GetProperty("condition").ToString());
                }

                //                                          //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                intStatus = 401;
                strUserMessage = "Something is wrong.";
                strDevMessage = "A value is not valid.";
                if (
                    ps != null
                    )
                {
                    //                                      //using is to release connection at the end
                    using (Odyssey2Context context = new Odyssey2Context())
                    {
                        //                                  //Starts a new transaction.
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                int intPkWorkflowFinal;
                                ProdtypProductType.subSetConditionToLink(intPkOut, intPkIn, ps, gpcondition, context,
                                    out intPkWorkflowFinal, ref intStatus, ref strUserMessage, ref strDevMessage);
                                obj = intPkWorkflowFinal;

                                //                          //Commits all changes made to the database in the current
                                //                          //      transaction.
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
                                //                          //Discards all changes made to the database in the current
                                //                          //      transaction.
                                dbContextTransaction.Rollback();

                                //                          //Making a log for the exception.
                                Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                            }
                        }
                    }
                }                    
            }
            //                                              //Response for the front web.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult SetAsThickness(
            //                                              //PURPOSE:
            //                                              //Set a media input IO as thickness.

            //                                              //URL: http://localhost/Odyssey2/Workflow
            //                                              //      /SetConditionToLink

            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intPkEleetOrEleele":1,
            //                                              //          "boolIsEleet":true,
            //                                              //          "intPkProcessInWorkflow":3,
            //                                              //          "boolThickness":true
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Set a media input IO as thickness.

            //                                              //RETURNS:
            //                                              //      200 - Ok

            [FromBody] JsonElement jsonThickness
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !((int)jsonThickness.ValueKind == 7) &&
                !((int)jsonThickness.ValueKind == 0) &&
                //                                          //Verify if the data is not null or empty.
                jsonThickness.TryGetProperty("intPkEleetOrEleele", out json) &&
                (int)jsonThickness.GetProperty("intPkEleetOrEleele").ValueKind == 4 &&
                jsonThickness.TryGetProperty("boolIsEleet", out json) &&
                (((int)jsonThickness.GetProperty("boolIsEleet").ValueKind == 5) ||
                ((int)jsonThickness.GetProperty("boolIsEleet").ValueKind == 6)) &&
                jsonThickness.TryGetProperty("intPkProcessInWorkflow", out json) &&
                (int)jsonThickness.GetProperty("intPkProcessInWorkflow").ValueKind == 4 &&
                jsonThickness.TryGetProperty("boolThickness", out json) &&
                (((int)jsonThickness.GetProperty("boolThickness").ValueKind == 5) ||
                ((int)jsonThickness.GetProperty("boolThickness").ValueKind == 6))
                )
            {
                int intPkEleetOrEleele = jsonThickness.GetProperty("intPkEleetOrEleele").GetInt32();
                bool boolIsEleet = jsonThickness.GetProperty("boolIsEleet").GetBoolean();
                int intPkProcessInWorkflow = jsonThickness.GetProperty("intPkProcessInWorkflow").GetInt32();
                bool boolThickness = jsonThickness.GetProperty("boolThickness").GetBoolean();

                //                                          //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                //                                          //using is to release connection at the end.
                using (Odyssey2Context context = new Odyssey2Context())
                {
                    //                                      //Starts a new transaction.
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            int intPkWorkflowFinal;
                            ProdtypProductType.subSetAsThickness(intPkEleetOrEleele, boolIsEleet,
                                intPkProcessInWorkflow, boolThickness, ps, context, out intPkWorkflowFinal,
                                ref intStatus, ref strUserMessage, ref strDevMessage);
                            obj = intPkWorkflowFinal;

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
            //                                              //Response for the front web.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult SetSize(
            //                                              //PURPOSE:
            //                                              //Set a component output IO as size.

            //                                              //URL: http://localhost/Odyssey2/Workflow
            //                                              //      /SetConditionToLink

            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intPkEleetOrEleele":1,
            //                                              //          "boolIsEleet":true,
            //                                              //          "intPkProcessInWorkflow":3,
            //                                              //          "boolSize":true
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Set a component output IO as size.

            //                                              //RETURNS:
            //                                              //      200 - Ok

            [FromBody] JsonElement jsonSize
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !((int)jsonSize.ValueKind == 7) &&
                !((int)jsonSize.ValueKind == 0) &&
                //                                          //Verify if the data is not null or empty.
                jsonSize.TryGetProperty("intPkEleetOrEleele", out json) &&
                (int)jsonSize.GetProperty("intPkEleetOrEleele").ValueKind == 4 &&
                jsonSize.TryGetProperty("boolIsEleet", out json) &&
                (((int)jsonSize.GetProperty("boolIsEleet").ValueKind == 5) ||
                ((int)jsonSize.GetProperty("boolIsEleet").ValueKind == 6)) &&
                jsonSize.TryGetProperty("intPkProcessInWorkflow", out json) &&
                (int)jsonSize.GetProperty("intPkProcessInWorkflow").ValueKind == 4 &&
                jsonSize.TryGetProperty("boolSize", out json) &&
                (((int)jsonSize.GetProperty("boolSize").ValueKind == 5) ||
                ((int)jsonSize.GetProperty("boolSize").ValueKind == 6))
                )
            {
                int intPkEleetOrEleele = jsonSize.GetProperty("intPkEleetOrEleele").GetInt32();
                bool boolIsEleet = jsonSize.GetProperty("boolIsEleet").GetBoolean();
                int intPkProcessInWorkflow = jsonSize.GetProperty("intPkProcessInWorkflow").GetInt32();
                bool boolSize = jsonSize.GetProperty("boolSize").GetBoolean();

                //                                          //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                var idClaimSuperAdmin = User.Claims.FirstOrDefault(c => c.Type == "boolIsSuperAdmin");
                bool boolSuperAdmin = idClaimSuperAdmin.Value.ParseToBool(); ;

                if (
                    ps != null
                    )
                {
                    //                                      //using is to release connection at the end.
                    using (Odyssey2Context context = new Odyssey2Context())
                    {
                        //                                  //Starts a new transaction.
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                int intPkWorkflowFinal;
                                ProdtypProductType.subSetSize(intPkEleetOrEleele, boolIsEleet, intPkProcessInWorkflow,
                                    boolSize, ps, boolSuperAdmin, context, out intPkWorkflowFinal, ref intStatus, ref strUserMessage,
                                    ref strDevMessage);
                                obj = intPkWorkflowFinal;

                                //                          //Commits all changes made to the database in the current
                                //                          //      transaction.
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
                                //                          //Discards all changes made to the database in the current
                                //                          //      transaction.
                                dbContextTransaction.Rollback();

                                //                          //Making a log for the exception.
                                Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                            }
                        }
                    }
                }                
            }
            //                                              //Response for the front web.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult SetGeneric(
            //                                              //PURPOSE:
            //                                              //Set a workflow as generic..

            //                                              //URL: http://localhost/Odyssey2/Workflow
            //                                              //      /SetGeneric

            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intPkWorkflow":2
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Set a workflow as generic.

            //                                              //RETURNS:
            //                                              //      200 - Ok

            [FromBody] JsonElement jsonGeneric
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !((int)jsonGeneric.ValueKind == 7) &&
                !((int)jsonGeneric.ValueKind == 0) &&
                //                                          //Verify if the data is not null or empty.
                jsonGeneric.TryGetProperty("intPkWorkflow", out json) &&
                (int)jsonGeneric.GetProperty("intPkWorkflow").ValueKind == 4 
                )
            {
                int intPkWorkflow = jsonGeneric.GetProperty("intPkWorkflow").GetInt32();;

                //                                          //Get the printshop id from token.
                var idClaimPrintshop = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaimPrintshop.Value;
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                var idClaimSuperAdmin = User.Claims.FirstOrDefault(c => c.Type == "boolIsSuperAdmin");
                bool boolIsSuperAdmin = idClaimSuperAdmin.Value.ParseToBool();

                intStatus = 401;
                strUserMessage = "Something is wrong.";
                strDevMessage = "Is not an admin user.";
                if (
                    boolIsSuperAdmin
                    )
                {
                    //                                      //using is to release connection at the end.
                    using (Odyssey2Context context = new Odyssey2Context())
                    {
                        //                                  //Starts a new transaction.
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                ProdtypProductType.subSetGeneric(intPkWorkflow, ps, context, ref intStatus,
                                    ref strUserMessage, ref strDevMessage);

                                //                          //Commits all changes made to the database in the current
                                //                          //      transaction.
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
                                //                          //Discards all changes made to the database in the current
                                //                          //      transaction.
                                dbContextTransaction.Rollback();

                                //                          //Making a log for the exception.
                                Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                            }
                        }
                    }
                }
            }

            //                                              //Response for the front web.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult Get(
            //                                              //PURPOSE:
            //                                              //Get all the processes with their inputs and outputs.

            //                                              //URL: http://localhost/Odyssey2/Workflow/Get?intPkWorkflow=2

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get an array of process.

            //                                              //RETURNS:
            //                                              //      200 - Ok
            int intJobId
            )
        {
            int intStatus = 400;
            String strUserMessage = "Invalid data.";
            String strDevMessage = "";
            Object obj = null;
            if (
                intJobId > 0
                )
            {
                //                                          //Get the printshop id from token.
                var idClaimPrintshop = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaimPrintshop.Value;
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                //                                          //using is to release connection at the end.
                using (Odyssey2Context context = new Odyssey2Context())
                {
                    //                                      //Starts a new transaction.
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            //obj = ProdtypProductType.subGetEstimateWorkflow(intJobId, ps, context, this.configuration,
                              //   ref intStatus, ref strUserMessage, ref strDevMessage);

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

            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetProcessDetails(
            //                                              //PURPOSE:
            //                                              //Get one processes with their inputs and outputs.

            //                                              //URL: http://localhost/Odyssey2/Workflow/GetProcessDetails?
            //                                              //      intPkProcessInWorkflow=1

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get one process in workflow.

            //                                              //RETURNS:
            //                                              //      200 - Ok
            int intPkProcessInWorkflow
            )
        {
            int intStatus = 400;
            String strUserMessage = "Invalid data.";
            String strDevMessage = "";
            Object obj = null;
            if (
                intPkProcessInWorkflow > 0
                )
            {
                try
                {
                    PiwjsonProcessInWorkflowJson piwjson = 
                        ProdtypProductType.piwjsonGetProcessDetails(intPkProcessInWorkflow, ref intStatus,
                        ref strUserMessage, ref strDevMessage);
                    obj = piwjson;
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
        [HttpGet("[action]")]
        public IActionResult GetProcessesAndNodes(
            //                                              //PURPOSE:
            //                                              //Get the processes and nodes for a workflow.

            //                                              //URL: http://localhost/Odyssey2/Workflow/
            //                                              //      GetProcessesAndNodes?intPkWorkflow=2

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get an array of process and nodes.

            //                                              //RETURNS:
            //                                              //      200 - Ok
            int intPkWorkflow
            )
        {
            int intStatus = 400;
            String strUserMessage = "Invalid data.";
            String strDevMessage = "";
            Object obj = null;
            if (
                intPkWorkflow > 0
                )
            {
                try
                {
                    obj = ProdtypProductType.pronodjsonGetProcessesAndNodes(intPkWorkflow, ref intStatus,
                        ref strUserMessage, ref strDevMessage);
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
        [HttpGet("[action]")]
        public IActionResult GetNodes(
            //                                              //PURPOSE:
            //                                              //Get the nodes for a workflow.

            //                                              //URL: http://localhost/Odyssey2/Workflow/
            //                                              //      GetNodes?intPkWorkflow=2

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get an array of nodes.

            //                                              //RETURNS:
            //                                              //      200 - Ok
            int intPkWorkflow
            )
        {
            int intStatus = 400;
            String strUserMessage = "Invalid data.";
            String strDevMessage = "";
            Object obj = null;
            if (
                intPkWorkflow > 0
                )
            {
                try
                {
                    obj = ProdtypProductType.pronodjsonGeNodes(intPkWorkflow, ref intStatus,
                        ref strUserMessage, ref strDevMessage);
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
        [HttpGet("[action]")]
        public IActionResult GetProcessIOs(
            //                                              //PURPOSE:
            //                                              //Get IOs (inputs or outpus) for a specific process.

            //                                              //URL: http://localhost/Odyssey2/Workflow/
            //                                              //      GetProcessIOs?intPkWorkflow=2&boolIsInput= true

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get an array of process's IOs.

            //                                              //RETURNS:
            //                                              //      200 - Ok
            int intPkProcessInWorkflow,
            bool boolIsInput
            )
        {
            int intStatus = 400;
            String strUserMessage = "Invalid data.";
            String strDevMessage = "";
            Object obj = null;
            if (
                intPkProcessInWorkflow > 0
                )
            {
                try
                {
                    obj = ProdtypProductType.darrproiojsonGetProcessIOs(intPkProcessInWorkflow, boolIsInput,
                        ref intStatus, ref strUserMessage, ref strDevMessage);
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
        [HttpGet("[action]")]
        public IActionResult GetLinks(
            //                                              //PURPOSE:
            //                                              //Get all Link of a workflow.

            //                                              //URL: http://localhost/Odyssey2/Workflow
            //                                              //      /GetLinks?intPkWorkflow=2

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get an array of Links.

            //                                              //RETURNS:
            //                                              //      200 - Ok

            //                                              //Pk product.
            int intPkWorkflow
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            if (
                intPkWorkflow > 0
                )
            {
                try
                {
                    List<LkjsonLinkJson> darrlkjsonLink;
                    ProdtypProductType.subGetLinks(intPkWorkflow, out darrlkjsonLink, ref intStatus,
                        ref strUserMessage, ref strDevMessage);
                    obj = darrlkjsonLink;
                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }                
            }

            //                                              //Response.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult ProcessOrNodeIsDispensable(
            //                                              //PURPOSE:
            //                                              //Gets true if the ProcessInWorkflow have links.

            //                                              //URL: http://localhost/Odyssey2/Workflow/
            //                                              //     ProcessOrNodeIsDispensable

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Gets true or false if the processInWorkflow have links.

            //                                              //RETURNS:
            //                                              //      200 - Ok

            //                                              //Pk process in workflow.
            [FromBody] JsonElement jsonProcOrNode
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;

            if (
                //                                          //Verify if jsonProcess is not null or a value is not empty.
                !((int)jsonProcOrNode.ValueKind == 7) &&
                !((int)jsonProcOrNode.ValueKind == 0)
                )
            {

                JsonElement json;
                int? intnPkProcessInWorkflow = null;
                if (
                    jsonProcOrNode.TryGetProperty("intnPkProcessInWorkflow", out json) &&
                    (int)jsonProcOrNode.GetProperty("intnPkProcessInWorkflow").ValueKind == 4
                    )
                {
                    intnPkProcessInWorkflow = jsonProcOrNode.GetProperty("intnPkProcessInWorkflow").GetInt32();
                }

                int? intnPkNode = null;
                if (
                    jsonProcOrNode.TryGetProperty("intnPkNode", out json) &&
                    (int)jsonProcOrNode.GetProperty("intnPkNode").ValueKind == 4
                    )
                {
                    intnPkNode = jsonProcOrNode.GetProperty("intnPkNode").GetInt32();
                }

                if (
                    (intnPkProcessInWorkflow != null) ||
                    (intnPkNode != null)
                    )
                {
                    try
                    {
                        bool? boolnProcessOrNodeIsDispensable;
                        ProdtypProductType.subProcessOrNodeIsDispensable(intnPkProcessInWorkflow, intnPkNode, 
                            out boolnProcessOrNodeIsDispensable, ref intStatus, ref strUserMessage, ref strDevMessage);
                        obj = boolnProcessOrNodeIsDispensable;
                    }
                    catch (Exception ex)
                    {
                        //                                      //Making a log for the exception.
                        Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                    }                    
                }
            }

            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")] 
        public IActionResult GetForAJob(
            //                                              //PURPOSE:
            //                                              //Get all the processes with their inputs and outputs for an
            //                                              //      specific job.

            //                                              //URL: http://localhost/Odyssey2/Workflow
            //                                              //      /GetForAJob?intJobId=5402228&intPkWorkflow=2

            //                                              //Get an array of process.

            //                                              //RETURNS:
            //                                              //      200 - Ok
            int intJobId,
            int intPkWorkflow
            )
        {
           
            //                                              //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

            int intStatus = 400;
            String strUserMessage = "Invalid data.";
            String strDevMessage = "";
            Object obj = null;
            //                                              //Since the true of ps is in Wisnet.
            if (
                (ps != null) &&
                (intJobId > 0) &&
                (intPkWorkflow > 0)
                )
            {
                try
                {
                    WfjjsonWorkflowJobJson wfjjson = ProdtypProductType.wfjjsonGet(intJobId, intPkWorkflow, ps,
                    this.configuration, this.iHubContext, ref intStatus, ref strUserMessage, ref strDevMessage,
                    //                                      //This null value is for the getPrice functionality.
                    null);
                    obj = wfjjson;
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
        [HttpGet("[action]")]
        public IActionResult GetWorkflowInformation(
           //                                              //PURPOSE:
           //                                              //Get processes's name that make workflow not ready.

           //                                              //URL: http://localhost/Odyssey2/Workflow
           //                                              //      /GetWorkflowInformation/?intPkWorkflow=328

           //                                              //Method: GET.

           //                                              //DESCRIPTION:
           //                                              //Get processes's name that make workflow not ready.

           //                                              //RETURNS:
           //                                              //      200 - Ok

           //                                              //Pk Product.
           int intPkWorkflow
            )
        {
            int intStatus = 400;
            String strDevMessage = "Invalid data.";
            String strUserMessage = "Something is wrong.";
            Object obj = null;
            if (
                intPkWorkflow > 0
                )
            {
                try
                {
                    WnrjsonWorkflowNotReadyJson wnrjsonWFIsReady;
                    ProdtypProductType.subGetWorkflowInformation(intPkWorkflow, out wnrjsonWFIsReady, ref intStatus,
                        ref strUserMessage, ref strDevMessage);
                    obj = wnrjsonWFIsReady;
                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }                
            }

            //                                              //Response for the front web.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------

        [HttpGet("[action]")]
        public IActionResult GetResourcesForIO(
            //                                              //PURPOSE:
            //                                              //Get the resource or the resources in the group for this 
            //                                              //      IO.

            //                                              //URL: http://localhost/Odyssey2/Workflow
            //                                              //      /GetResourcesForIO?intPkProcessInWorkflow=2&
            //                                              //      intPkEleetOrEleele=1&boolIsEleet=true

            //                                              //DESCRIPTION:
            //                                              //With IO data go to IO table to get the resource or the 
            //                                              //      groupid, with the groupid go to the gpres table to
            //                                              //      get the resources.

            //                                              //RETURNS:
            //                                              //      200 - Ok
            int intPkProcessInWorkflow,
            int intPkEleetOrEleele,
            bool boolIsEleet
            )
        {
            int intStatus = 400;
            String strUserMessage = "Invalid data.";
            String strDevMessage = "";
            Object obj = null;
            if (
                (intPkProcessInWorkflow > 0) &&
                (intPkEleetOrEleele > 0)
                )
            {
                try
                {
                    Resjson2ResourceJson2[] arrresjson2;
                    ProdtypProductType.subGetResourcesForIO(intPkProcessInWorkflow, intPkEleetOrEleele, boolIsEleet,
                        out arrresjson2, ref intStatus, ref strUserMessage, ref strDevMessage);
                    obj = arrresjson2;
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
        [HttpGet("[action]")]
        public IActionResult GetProductWorkflows(
            //                                              //PURPOSE:
            //                                              //Get all the workflows of a product.

            //                                              //URL: http://localhost/Odyssey2/Workflow
            //                                              //      /GetProductWorkflows
            //                                              //      {
            //                                              //          intPkProduct = 338,
            //                                              //          intnJobId = 5570373
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Get all the workflows of a product. If there´s any
            //                                              //      //create the first workflow

            //                                              //RETURNS:
            //                                              //      200 - Ok
            [FromBody] JsonElement jsonProdOrJob
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;

            //                                              //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

            if (
                //                                          //Verify if json is not null or a value is not empty.
                !((int)jsonProdOrJob.ValueKind == 7) &&
                !((int)jsonProdOrJob.ValueKind == 0) &&
                (((int)jsonProdOrJob.GetProperty("boolEstimate").ValueKind == 5) ||
                ((int)jsonProdOrJob.GetProperty("boolEstimate").ValueKind == 6)) &&
                (int)jsonProdOrJob.GetProperty("intPkProduct").ValueKind == 4
                )
            {
                JsonElement json;
                bool boolEstimate = jsonProdOrJob.GetProperty("boolEstimate").GetBoolean();
                int intPkProduct = jsonProdOrJob.GetProperty("intPkProduct").GetInt32();

                int? intnJobId = null;
                if (
                    jsonProdOrJob.TryGetProperty("intnJobId", out json) &&
                    (int)jsonProdOrJob.GetProperty("intnJobId").ValueKind == 4
                    )
                {
                    intnJobId = jsonProdOrJob.GetProperty("intnJobId").GetInt32();
                    intnJobId = intnJobId <= 0 ? null : intnJobId;
                }

                //                                      //using is to release connection at the end
                using (Odyssey2Context context = new Odyssey2Context())
                {
                    //                                  //Starts a new transaction.
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            List<Wfjson2WorkflowJson2> darrwfjson2;
                            ProdtypProductType.subGetProductWorkflows(intnJobId, intPkProduct, boolEstimate,
                                this.configuration, ps, out darrwfjson2, context, ref intStatus, ref strUserMessage,
                                ref strDevMessage);
                            obj = darrwfjson2;

                            //                          //Commits all changes made to the database in the current
                            //                          //      transaction.
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
                            //                          //Discards all changes made to the database in the current
                            //                          //      transaction.
                            dbContextTransaction.Rollback();

                            //                                      //Making a log for the exception.
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
        [HttpGet("[action]")]
        public IActionResult GetProcessInputs(
            //                                              //PURPOSE:
            //                                              //Get IOs from a PIW.

            //                                              //URL: http://localhost/Odyssey2/Workflow
            //                                              //      /GetProcessInputs

            //                                              //Use a json like this:
            //                                              //      {
            //                                              //      "intnJobId": 2342,
            //                                              //      "intPkProcessInWorkflow": 26,
            //                                              //      "intPkeleetOrEleele": 4,
            //                                              //      "boolIsEleet": true,
            //                                              //      "intPkResource": 3
            //                                              //      }

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get Ios from a PIW.
            //                                              //if it get JobId,  this mean that it 
            //                                              //    is in wfJob,
            //                                              //if it dont get,  this mean that it 
            //                                              //    is in wfProducto.                                     

            //                                              //RETURNS:
            //                                              //      200 - Ok
            [FromBody] JsonElement jsonProInputs
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;           

            if (
                //                                          //Verify if jsonFilter is not null or a value is not empty.
                !((int)jsonProInputs.ValueKind == 7) &&
                !((int)jsonProInputs.ValueKind == 0) &&
                jsonProInputs.TryGetProperty("intnJobId", out json) &&
                jsonProInputs.TryGetProperty("intPkProcessInWorkflow", out json) &&
                (int)jsonProInputs.GetProperty("intPkProcessInWorkflow").ValueKind == 4 &&
                jsonProInputs.TryGetProperty("intPkeleetOrEleele", out json) &&
                (int)jsonProInputs.GetProperty("intPkeleetOrEleele").ValueKind == 4 &&
                jsonProInputs.TryGetProperty("boolIsEleet", out json) &&
                (((int)jsonProInputs.GetProperty("boolIsEleet").ValueKind == 5) ||
                ((int)jsonProInputs.GetProperty("boolIsEleet").ValueKind == 6)) &&
                jsonProInputs.TryGetProperty("intPkResource", out json) &&
                (int)jsonProInputs.GetProperty("intPkResource").ValueKind == 4
                )
            {
                //                                          //Get data.
                int intPkProcessInWorkflow = jsonProInputs.GetProperty("intPkProcessInWorkflow").GetInt32();
                int intPkeleetOrEleele = jsonProInputs.GetProperty("intPkeleetOrEleele").GetInt32();
                bool boolIsEleet = jsonProInputs.GetProperty("boolIsEleet").GetBoolean();
                int intPkResource = jsonProInputs.GetProperty("intPkResource").GetInt32();

                int? intnJobId = null;
                if (
                    (int)jsonProInputs.GetProperty("intnJobId").ValueKind == 4 
                    )
                {
                    intnJobId = jsonProInputs.GetProperty("intnJobId").GetInt32();
                }

                //                                          //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;

                try
                {
                    List<IofrmpiwjsonIOFromPIWJson> darriofrmpiwjsonIosFromPIW;
                    ProdtypProductType.subGetProcessInputs(intnJobId, strPrintshopId, intPkProcessInWorkflow,
                        this.configuration, intPkeleetOrEleele, boolIsEleet, intPkResource,
                        out darriofrmpiwjsonIosFromPIW, ref intStatus, ref strUserMessage, ref strDevMessage);
                    obj = darriofrmpiwjsonIosFromPIW;
                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }                
            }

            //                                              //Response.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult IsPriceChangeable(
            //                                              //PURPOSE:
            //                                              //Get a boolean to know is the price of a job can be
            //                                              //  change.

            //                                              //URL: http://localhost/Odyssey2/Workflow/IspriceChangeable
            //                                              //  ?intJobId=548752

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get a true or false.

            //                                              //RETURNS:
            //                                              //      200 - Ok
            int intJobId
            )
        {
            //                                              //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

            int intStatus = 400;
            String strUserMessage = "Invalid data.";
            String strDevMessage = "";
            Object obj = null;
            if (
                intJobId > 0 &&
                ps != null
                )
            {
                try
                {
                    bool? boolnIsPriceChangeable;
                    ProdtypProductType.subIsPriceChangeableForWorkflow(intJobId, ps, this.configuration,
                            out boolnIsPriceChangeable, ref intStatus, ref strUserMessage, ref strDevMessage);
                    obj = boolnIsPriceChangeable;
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
        [HttpGet("[action]")]
        public IActionResult IsModifiable(
            //                                              //PURPOSE:
            //                                              //Get a boolean to know if the workflow has estimates and 
            //                                              //      a modification could delete these estimates.

            //                                              //URL: http://localhost/Odyssey2/Workflow/IsModifiable?
            //                                              //      intPkWorkflow=21

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get a true if the workflow is modifiable or false if it 
            //                                              //      is not.

            //                                              //RETURNS:
            //                                              //      200 - Ok
            int intPkWorkflow
            )
        {
            //                                              //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;

            int intStatus = 400;
            String strUserMessage = "Invalid data.";
            String strDevMessage = "";
            Object obj = null;
            if (
                intPkWorkflow > 0
                )
            {
                try
                {
                    bool boolIsWorkflowModifiable;
                    ProdtypProductType.subWorkflowHasEstimates(intPkWorkflow, null, out boolIsWorkflowModifiable,
                        ref intStatus, ref strUserMessage, ref strDevMessage);
                    obj = boolIsWorkflowModifiable;
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
        [HttpGet("[action]")]
        public IActionResult GetWorkflow(
            //                                              //PURPOSE:
            //                                              //Get all the processes, nodes and links.

            //                                              //URL: http://localhost/Odyssey2/Workflow/GetWorkflow
            //                                              //      ?intPkWorkflow=2

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get all the processes, nodes and links.

            //                                              //RETURNS:
            //                                              //      200 - Ok
            int intPkWorkflow
            )
        {
            int intStatus = 400;
            String strUserMessage = "Invalid data.";
            String strDevMessage = "";
            Object obj = null;
            if (
                intPkWorkflow > 0
                )
            {
                //                                          //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                if (
                    ps != null
                    )
                {
                    try
                    {
                        Wfjson3WorkflowJson3 wfjson3;
                        ProdtypProductType.subGetWorkflow(intPkWorkflow, ps, out wfjson3, ref intStatus,
                            ref strUserMessage, ref strDevMessage);
                        obj = wfjson3;
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
        [HttpGet("[action]")]
        public IActionResult GetAllBase(
            //                                              //PURPOSE:
            //                                              //Get all printshop's base workflows.

            //                                              //URL: http://localhost/Odyssey2/Workflow/GetAllBase

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get all printshop's base workflows.

            //                                              //RETURNS:
            //                                              //      200 - Ok
            )
        {
            //                                              //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

            int intStatus = 400;
            String strUserMessage = "Invalid data.";
            String strDevMessage = "";
            Object obj = null;
            if (
                ps != null
                )
            {
                try
                {
                    List<Wfjson4WorkflowJson4> darrwfjson4;
                    ProdtypProductType.subGetAllBase(ps, out darrwfjson4, ref intStatus, ref strUserMessage,
                        ref strDevMessage);
                    obj = darrwfjson4;
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
        [HttpGet("[action]")]
        public IActionResult GetJobWorkflowInformation(
            //                                              //PURPOSE:
            //                                              //Get information about the items in the job's workflow
            //                                              //       that make job not ready to start.

            //                                              //URL: http://localhost/Odyssey2/Workflow
            //                                              //      /GetJobWorkflowInformation/?intJobId=32853&
            //                                              //      intPkWorkflow=1

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get information about the items in the job's workflow
            //                                              //       that make job not ready to start.

            //                                              //RETURNS:
            //                                              //      200 - Ok

            int intJobId,
            int intPkWorkflow
            )
        {
            //                                              //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

            int intStatus = 400;
            String strDevMessage = "Invalid data.";
            String strUserMessage = "Something is wrong.";
            Object obj = null;
            if (
                intJobId > 0 &&
                ps != null
                )
            {
                try
                {
                    JobwnrjsonJobWorkflowNotReadyJson jobwnrjsonJobWfNotReady;
                    ProdtypProductType.subGetJobWorkflowInformation(intJobId, intPkWorkflow, ps, this.configuration,
                        this.iHubContext, out jobwnrjsonJobWfNotReady, ref intStatus, ref strUserMessage,
                        ref strDevMessage);
                    obj = jobwnrjsonJobWfNotReady;
                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }                
            }

            //                                              //Response for the front web.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetProcessesInWorkflow(
            //                                              //PURPOSE:
            //                                              //Get all the processes of a workflow .

            //                                              //URL: http://localhost/Odyssey2/Workflow/
            //                                              //      GetProcessesInWorkflow?intPkWorkflow=1

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get all the processes.

            //                                              //RETURNS:
            //                                              //      200 - Ok
            int intPkWorkflow
            )
        {
            int intStatus = 400;
            String strUserMessage = "Invalid data.";
            String strDevMessage = "";
            Object obj = null;
            if (
                intPkWorkflow > 0
                )
            {
                try
                {
                    Piwjson3ProcessInWorkflowJson3[] arrPiwjson3;
                    ProdtypProductType.subGetProcessesInWorkflow(intPkWorkflow, out arrPiwjson3, ref intStatus,
                        ref strUserMessage, ref strDevMessage);
                    obj = arrPiwjson3;
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
        [HttpGet("[action]")]
        public IActionResult HasNotcalculations(
            //                                              //PURPOSE:
            //                                              //Verify if the workflow already has setted a IO how size. 
            //                                              //    Return true if there is'nt IO setted how size
            //                                              //    and false if already there is a IO setted how size
            //                                              //    and if has paperTransformation.

            //                                              //URL: http://localhost/Odyssey2/Workflow/HasNotPaperTransformation
            //                                              //    ?intPkWorkflow=2

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Verify if the workflow already has setted a IO how size. 

            //                                              //RETURNS:
            //                                              //      200 - Ok
            [FromBody] JsonElement jsonSize
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !((int)jsonSize.ValueKind == 7) &&
                !((int)jsonSize.ValueKind == 0) &&
                //                                          //Verify if the data is not null or empty.
                jsonSize.TryGetProperty("intPkEleetOrEleele", out json) &&
                (int)jsonSize.GetProperty("intPkEleetOrEleele").ValueKind == 4 &&
                jsonSize.TryGetProperty("boolIsEleet", out json) &&
                (((int)jsonSize.GetProperty("boolIsEleet").ValueKind == 5) ||
                ((int)jsonSize.GetProperty("boolIsEleet").ValueKind == 6)) &&
                jsonSize.TryGetProperty("boolSize", out json) &&
                (((int)jsonSize.GetProperty("boolSize").ValueKind == 5) ||
                ((int)jsonSize.GetProperty("boolSize").ValueKind == 6)) &&
                jsonSize.TryGetProperty("intPkProcessInWorkflow", out json) &&
                (int)jsonSize.GetProperty("intPkProcessInWorkflow").ValueKind == 4
                )
            {
                int intPkEleetOrEleele = jsonSize.GetProperty("intPkEleetOrEleele").GetInt32();
                bool boolIsEleet = jsonSize.GetProperty("boolIsEleet").GetBoolean();
                int intPkProcessInWorkflow = jsonSize.GetProperty("intPkProcessInWorkflow").GetInt32();
                bool boolSize = jsonSize.GetProperty("boolSize").GetBoolean();

                if (
                    intPkEleetOrEleele > 0 &&
                    intPkProcessInWorkflow > 0
                    )
                {
                    try
                    {
                        bool boolHasNotCalculations = 
                            ProdtypProductType.boolHasNotCalculations(intPkProcessInWorkflow, intPkEleetOrEleele, 
                            boolIsEleet, boolSize, ref intStatus, ref strUserMessage, ref strDevMessage);
                        obj = boolHasNotCalculations;
                    }
                    catch (Exception ex)
                    {
                        //                                      //Making a log for the exception.
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
        [HttpGet("[action]")]
        public IActionResult GetGenerics(
            //                                              //PURPOSE:
            //                                              //Get all the generic workflows.

            //                                              //URL: http://localhost/Odyssey2/Workflow/GetGenerics

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get all the generic workflows.

            //                                              //RETURNS:
            //                                              //      200 - Ok
            )
        {
            //                                              //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            if (
                //                                          //Verify if the printshop is not null.
                ps != null
                )
            {
                try
                {
                    Wfjson4WorkflowJson4[] arrwfjson4;
                    ProdtypProductType.subGetGenericWorkflows(out arrwfjson4, ref intStatus, ref strUserMessage,
                        ref strDevMessage);
                    obj = arrwfjson4;
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
        [HttpGet("[action]")]
        public IActionResult GetConditions(
            //                                              //PURPOSE:
            //                                              //Get a condition or group conditions.

            //                                              //URL: http://localhost/Odyssey2/Workflow/GetConditions

            //                                              //Use a json like this:
            //                                              //      {
            //                                              //      "intnPkCalculation": 1,
            //                                              //      "intnPkOut": null,
            //                                              //      "intnPkIn": null,
            //                                              //      "intnPkTransformCalculation": null
            //                                              //      }

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get the gpconditionjson

            //                                              //RETURNS:
            //                                              //      200 - Ok
            [FromBody] JsonElement jsonCond
            )
        {
            int intStatus = 400;
            String strUserMessage = "Invalid data.";
            String strDevMessage = "";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !((int)jsonCond.ValueKind == 7) &&
                !((int)jsonCond.ValueKind == 0)
                )
            {
                int? intnPkCalculation = null;
                if (
                    jsonCond.TryGetProperty("intnPkCalculation", out json) &&
                    (int)jsonCond.GetProperty("intnPkCalculation").ValueKind == 4
                    )
                {
                    intnPkCalculation = jsonCond.GetProperty("intnPkCalculation").GetInt32();
                }

                int? intnPkOut = null;
                if (
                    jsonCond.TryGetProperty("intnPkOut", out json) &&
                    (int)jsonCond.GetProperty("intnPkOut").ValueKind == 4
                    )
                {
                    intnPkOut = jsonCond.GetProperty("intnPkOut").GetInt32();
                }

                int? intnPkIn = null;
                if (
                    jsonCond.TryGetProperty("intnPkIn", out json) &&
                    (int)jsonCond.GetProperty("intnPkIn").ValueKind == 4
                    )
                {
                    intnPkIn = jsonCond.GetProperty("intnPkIn").GetInt32();
                }

                int? intnPkTransformCalculation = null;
                if (
                    jsonCond.TryGetProperty("intnPkTransformCalculation", out json) &&
                    (int)jsonCond.GetProperty("intnPkTransformCalculation").ValueKind == 4
                    )
                {
                    intnPkTransformCalculation = jsonCond.GetProperty("intnPkTransformCalculation").GetInt32();
                }

                //                                          //Get the printshop id from token.
                var idClaimPrintshop = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaimPrintshop.Value;
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                var idClaimSuperAdmin = User.Claims.FirstOrDefault(c => c.Type == "boolIsSuperAdmin");
                bool boolIsSuperAdmin = idClaimSuperAdmin.Value.ParseToBool();

                try
                {
                    obj = ProdtypProductType.GpcondjsonsubGetConditions(intnPkCalculation, intnPkOut,
                        intnPkIn, intnPkTransformCalculation, boolIsSuperAdmin, ps, ref intStatus, ref strUserMessage,
                        ref strDevMessage);
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
    }

    //==================================================================================================================
}
/*END-TASK*/