/*TASK RP. JOB*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Odyssey2Backend.App;
using Odyssey2Backend.Job;
using Odyssey2Backend.JsonTypes;
using Odyssey2Backend.XJDF;
using System;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Linq;
using Odyssey2Backend.PrintShop;
using System.Collections.Generic;
using Odyssey2Backend.JsonTemplates.Out;
using TowaStandard;
using System.Xml.XPath;
using System.Xml;
using Microsoft.AspNetCore.SignalR;
using Odyssey2Backend.Alert;
using System.Xml.Schema;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using System.IO;
using Odyssey2Backend.Utilities;

//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia).
//                                                          //CO-AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //DATE: April 29, 2020. 

namespace Odyssey2Backend.Controllers
{
    //                                                      //To obtain the strPrintshopId from token:
    //                                                      //  var idClaim = User.Claims.FirstOrDefault(c => c.Type == 
    //                                                      //      "strPrintshopId");
    //                                                      //  String strPrintshopId = idClaim.Value;

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //==================================================================================================================
    [Route("Odyssey2/[controller]")]
    public class JobController : Controller
    {
        //                                                  //Controller associated with the actions for a Job.
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
        //                                                  //CONSTANTS.

        public const String strEstimate = "Estimate";
        public const String strOrder = "Order";
        public const String strNone = "None";

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        private IConfiguration configuration;
        private readonly IHubContext<ConnectionHub> hubContext;
        private readonly IWebHostEnvironment hostingEnvironment;

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        public JobController(
            IConfiguration iConfiguration_I,
            IHubContext<ConnectionHub> iHubContext_I,
            IWebHostEnvironment iHostingEnvironment_I
            )
        {
            this.configuration = iConfiguration_I;
            this.hubContext = iHubContext_I;
            this.hostingEnvironment = iHostingEnvironment_I;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //TRANSFORMATION METHODS.

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult ModifyJobStagePendingToProgress(
            //                                              //PURPOSE:
            //                                              //Modify stage to progress.

            //                                              //URL: http://localhost/Odyssey2/Job
            //                                              //      /ModifyJobStagePendingToProgress
            //                                              //Method: POST.
            //                                              //Json:
            //                                              //  {
            //                                              //      "intJobId": 1022,
            //                                              //      "intPkWorkflow": 2
            //                                              //  }

            //                                              //DESCRIPTION:
            //                                              //Modify stage to Inprogress.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            [FromBody] JsonElement jobData
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !((int)jobData.ValueKind == 7) &&
                !((int)jobData.ValueKind == 0) &&
                //                                         //Verify the necessary properties.
                jobData.TryGetProperty("intJobId", out json) &&
                (int)jobData.GetProperty("intJobId").ValueKind == 4 &&
                jobData.TryGetProperty("intPkWorkflow", out json) &&
                (int)jobData.GetProperty("intPkWorkflow").ValueKind == 4
                )
            {
                int intJobId = jobData.GetProperty("intJobId").GetInt32();
                int intPkWorkflow = jobData.GetProperty("intPkWorkflow").GetInt32();

                //                                              //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;

                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                if (
                    //                                          //Verify the Pk.
                    ps != null
                    )
                {
                    try
                    {
                        JobJob.subModifyJobStagePendingToProgress(ps, intJobId, intPkWorkflow,
                            this.configuration, this.hubContext, ref intStatus, ref strUserMessage, ref strDevMessage);
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
        public IActionResult SetDueDate(
            //                                              //PURPOSE:
            //                                              //Add a due date for a job.

            //                                              //URL: http://localhost/Odyssey2/Job/SetDueDate
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intJobId":15487,
            //                                              //          "strDueDate":"2020-05-20",
            //                                              //          "strDueTime":"10:00:00"
            //                                              //          "strDescription": "Why change dueDate"
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Create a DueDate job's history.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            //                                              //      400 - BadRequest().

            //                                              //Json that contains all necessary data.
            [FromBody] JsonElement jsonDueDate
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !Object.ReferenceEquals(null, jsonDueDate) &&
                //                                          //Verify if the data is not null or empty.
                jsonDueDate.TryGetProperty("intJobId", out json) &&
                (int)jsonDueDate.GetProperty("intJobId").ValueKind == 4 &&
                jsonDueDate.TryGetProperty("strDueDate", out json) &&
                (int)jsonDueDate.GetProperty("strDueDate").ValueKind == 3 &&
                jsonDueDate.TryGetProperty("strDueTime", out json) &&
                (int)jsonDueDate.GetProperty("strDueTime").ValueKind == 3
                )
            {
                int intJobId = jsonDueDate.GetProperty("intJobId").GetInt32();
                String strDueDate = jsonDueDate.GetProperty("strDueDate").GetString();
                String strDueTime = jsonDueDate.GetProperty("strDueTime").GetString();

                String strDescription = "";
                if (
                    jsonDueDate.TryGetProperty("strDescription", out json) &&
                    (int)jsonDueDate.GetProperty("strDescription").ValueKind == 3
                    )
                {
                    strDescription = jsonDueDate.GetProperty("strDescription").GetString();
                }

                //                                          //Get the contact id from token.
                var idClaimContact = User.Claims.FirstOrDefault(c => c.Type == "intContactId");
                int intContactId = idClaimContact.Value.ParseToInt();

                //                                          //Get printshop id from token
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;

                try
                {
                    JobJob.subSetDueDate(intJobId, strDueDate, strDueTime, intContactId, strDescription, strPrintshopId,
                        this.configuration, this.hubContext, ref intStatus, ref strUserMessage, ref strDevMessage);
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

        //------------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult SetNote(
           //                                              //PURPOSE:
           //                                              //Add/update note to a job.

           //                                              //URL: http://localhost/Odyssey2/Job
           //                                              //      /SetNote
           //                                              //Method: POST.

           //                                              //Use a JSON like this:
           //                                              //{
           //                                              //    "intnPkNote" : 1,
           //                                              //    "strOdyssey2Note" : "My first note",
           //                                              //}

           //                                               //DESCRIPTION:
           //                                               //Save a note to a job if the job does not has one or
           //                                               //      update it if exists.

           //                                               //RETURNS:
           //                                               //      200 - Ok

           //                                               //Receives JSON with data.
           [FromBody] JsonElement jsonNote
           )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if jsonFilter is not null or a value is not empty.
                !((int)jsonNote.ValueKind == 7) &&
                !((int)jsonNote.ValueKind == 0) &&
                jsonNote.TryGetProperty("intJobId", out json) &&
                (int)jsonNote.GetProperty("intJobId").ValueKind == 4 &&
                jsonNote.TryGetProperty("intnPkNote", out json) &&
                jsonNote.TryGetProperty("strOdyssey2Note", out json)
                )
            {
                //                                          //Take value from json.
                int intJobId = jsonNote.GetProperty("intJobId").GetInt32();

                int? intnPkNote = null;
                if (
                    (int)jsonNote.GetProperty("intnPkNote").ValueKind == 4
                    )
                {
                    intnPkNote = jsonNote.GetProperty("intnPkNote").GetInt32();
                }

                String strOdyssey2Note = "";
                if (
                    (int)jsonNote.GetProperty("strOdyssey2Note").ValueKind == 3
                    )
                {
                    strOdyssey2Note = jsonNote.GetProperty("strOdyssey2Note").GetString();
                    strOdyssey2Note = strOdyssey2Note.TrimExcel();
                }

                //                                          //Get printshop id from token
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;

                //                                              //Get the contact id from token.
                var idClaimContact = User.Claims.FirstOrDefault(c => c.Type == "intContactId");
                int intContactId = idClaimContact.Value.ParseToInt();

                //                                      //using is to release connection at the end
                using (Odyssey2Context context = new Odyssey2Context())
                {
                    //                                  //Starts a new transaction.
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            JobJob.subSetNote(strPrintshopId, intJobId, intnPkNote, strOdyssey2Note, intContactId,
                                context, this.configuration, ref intStatus, ref strUserMessage, ref strDevMessage);

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
        public IActionResult AddProcessNotes(
        //                                                  //PURPOSE:
        //                                                  //Add a note to a process.

        //                                                  //URL: http://localhost/Odyssey2/Job/AddProcessNotes

        //                                                  //Method: POST.
        //                                                  //Use a body like this:
        //                                                  //{
        //                                                  //    "intPkProcessInWorkflow":1,
        //                                                  //    "strNote":"Mi primera nota",
        //                                                  //    "arrContactsId":[11252, 225485, 224512]
        //                                                  //}

        //                                                  //DESCRIPTION:
        //                                                  //Add a note to an especific process in a workflow job.

        [FromBody] JsonElement proNoteData
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !((int)proNoteData.ValueKind == 7) &&
                !((int)proNoteData.ValueKind == 0) &&
                //                                         //Verify the necessary properties.
                proNoteData.TryGetProperty("strNote", out json) &&
                (int)proNoteData.GetProperty("strNote").ValueKind == 3 &&
                proNoteData.TryGetProperty("intPkProcessInWorkflow", out json) &&
                (int)proNoteData.GetProperty("intPkProcessInWorkflow").ValueKind == 4 &&
                proNoteData.TryGetProperty("intJobId", out json) &&
                (int)proNoteData.GetProperty("intJobId").ValueKind == 4
                )
            {
                String strNote = proNoteData.GetProperty("strNote").GetString();
                strNote = strNote.TrimExcel();
                int intPkProcessInWorkflow = proNoteData.GetProperty("intPkProcessInWorkflow").GetInt32();
                int intJobId = proNoteData.GetProperty("intJobId").GetInt32();

                List<int> darrintContactsId = new List<int>();
                if (
                    proNoteData.TryGetProperty("arrContactsIds", out json) &&
                    (int)proNoteData.GetProperty("arrContactsIds").ValueKind == 2
                    )
                {
                    for (int intU = 0; intU < proNoteData.GetProperty("arrContactsIds").GetArrayLength();
                            intU = intU + 1)
                    {
                        darrintContactsId.Add(proNoteData.GetProperty("arrContactsIds")[intU].GetInt32());
                    }
                }
                else
                {
                    darrintContactsId = null;
                }

                if (
                    strNote.Length > 0 &&
                    intPkProcessInWorkflow > 0 &&
                    intJobId > 0
                    )
                {
                    //                                      //Get data from token.
                    var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                    String strPrintshopId = idClaim.Value;
                    PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                    var idClaimContact = User.Claims.FirstOrDefault(c => c.Type == "intContactId");
                    int intContactId = idClaimContact.Value.ParseToInt();

                    using (Odyssey2Context context = new Odyssey2Context())
                    {
                        //                                  //Starts a new transaction.
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                JobJob.subSetProcessNote(intJobId, intPkProcessInWorkflow, intContactId, strNote,
                                    darrintContactsId, ps, this.configuration, context, this.hubContext, ref intStatus,
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

                                intStatus = 407;
                                strUserMessage = "Something is wrong.";
                                strDevMessage = ex.Message;
                            }
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

        //------------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult AddEstimates(
           //                                              //PURPOSE:
           //                                              //Add/overwrite estimations for the job and the wf.                                 
           //                                              //URL: http://localhost/Odyssey2/Job
           //                                              //      /AddEstimates
           //                                              //Method: POST.

           //                                              //Use a JSON like this:
           //                                              //{
           //                                              //    "intJobId" : 5519145,
           //                                              //    "strBaseDate" : "2020-07-17",
           //                                              //    "strBaseTime" : "10:00:00",
           //                                              //    "arrestim":
           //                                              //    [
           //                                              //        [
           //                                              //           {
           //                                              //               "intPkProcessInWorkflow":2,
           //                                              //               "intPkEleetOrEleele": 2,
           //                                              //               "boolIsEleet": true,
           //                                              //               "intPkResource": 34
           //                                              //           },
           //                                              //           {
           //                                              //               "intPkProcessInWorkflow":2,
           //                                              //               "intPkEleetOrEleele": 4,
           //                                              //               "boolIsEleet": false,
           //                                              //               "intPkResource": 12
           //                                              //            }
           //                                              //        ],
           //                                              //        [
           //                                              //           {
           //                                              //               "intPkProcessInWorkflow":2,
           //                                              //               "intPkEleetOrEleele": 2,
           //                                              //               "boolIsEleet": true,
           //                                              //               "intPkResource": 34
           //                                              //           },
           //                                              //           {
           //                                              //               "intPkProcessInWorkflow":4,
           //                                              //               "intPkEleetOrEleele": 4,
           //                                              //               "boolIsEleet": false,
           //                                              //               "intPkResource": 14
           //                                              //            }
           //                                              //        ]
           //                                              //    ]
           //                                              //}

           //                                               //DESCRIPTION:
           //                                               //It validates if the data correspods to a existing workflow, 
           //                                               //      then adds the estimation in the estimation table 
           //                                               //      overwriting if another exists.

           //                                               //RETURNS:
           //                                               //      200 - Ok

           //                                               //Receives JSON with data.
           [FromBody] JsonElement jsonEstimatesData
           )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                !Object.ReferenceEquals(null, jsonEstimatesData) &&
                jsonEstimatesData.TryGetProperty("intJobId", out json) &&
                (int)jsonEstimatesData.GetProperty("intJobId").ValueKind == 4 &&
                jsonEstimatesData.TryGetProperty("strBaseDate", out json) &&
                (int)jsonEstimatesData.GetProperty("strBaseDate").ValueKind == 3 &&
                jsonEstimatesData.TryGetProperty("strBaseTime", out json) &&
                (int)jsonEstimatesData.GetProperty("strBaseTime").ValueKind == 3
                )
            {
                //                                          //Get the intJobId.
                int intJobId = jsonEstimatesData.GetProperty("intJobId").GetInt32();
                String strBaseDate = jsonEstimatesData.GetProperty("strBaseDate").GetString();
                String strBaseTime = jsonEstimatesData.GetProperty("strBaseTime").GetString();

                //                                          //List to keep the resources for the estimation.
                List<EstjsonEstimationDataJson> darrestjson = new List<EstjsonEstimationDataJson>();

                bool boolValidData = true;
                if (
                    //                                      //Verify arrestim is an array.
                    jsonEstimatesData.TryGetProperty("arrestim", out json) &&
                    (int)jsonEstimatesData.GetProperty("arrestim").ValueKind == 2 &&
                    jsonEstimatesData.GetProperty("arrestim").GetArrayLength() > 0
                    )
                {
                    //                                      //Get arrestim data.
                    JsonElement jsonEstimationsData = jsonEstimatesData.GetProperty("arrestim");

                    int intEstimationsCount = jsonEstimationsData.GetArrayLength();
                    int intIEstimation = 0;
                    while (
                        (intIEstimation < intEstimationsCount) &&
                        boolValidData
                        )
                    {
                        JsonElement jsonEstimationData = jsonEstimationsData[intIEstimation];
                        if (
                            //                              //Verify arrestim items are arrays.
                            (int)jsonEstimationData.ValueKind == 2 &&
                            jsonEstimationData.GetArrayLength() > 0
                            )
                        {
                            //                              //arrestim items contains resources data.
                            JsonElement jsonResoucesData = jsonEstimationData;

                            int intResourceCountI = jsonResoucesData.GetArrayLength();
                            int intIResource = 0;
                            while (
                                (intIResource < intResourceCountI) &&
                                boolValidData
                                )
                            {
                                JsonElement jsonResouceData = jsonResoucesData[intIResource];

                                if (
                                    //                      //Verify if resouce data.
                                    (int)jsonResouceData.ValueKind == 1 &&
                                    jsonResouceData.TryGetProperty("intPkProcessInWorkflow", out json) &&
                                    (int)jsonResouceData.GetProperty("intPkProcessInWorkflow").ValueKind == 4 &&
                                    jsonResouceData.TryGetProperty("intPkEleetOrEleele", out json) &&
                                    (int)jsonResouceData.GetProperty("intPkEleetOrEleele").ValueKind == 4 &&
                                    jsonResouceData.TryGetProperty("boolIsEleet", out json) &&
                                    ((int)jsonResouceData.GetProperty("boolIsEleet").ValueKind == 5 ||
                                    (int)jsonResouceData.GetProperty("boolIsEleet").ValueKind == 6) &&
                                    jsonResouceData.TryGetProperty("intPkResource", out json) &&
                                    (int)jsonResouceData.GetProperty("intPkResource").ValueKind == 4
                                    )
                                {
                                    int intPkProcessInWorkflow =
                                        jsonResouceData.GetProperty("intPkProcessInWorkflow").GetInt32();
                                    int intPkEleetOrEleele =
                                        jsonResouceData.GetProperty("intPkEleetOrEleele").GetInt32();
                                    bool boolIsEleet = jsonResouceData.GetProperty("boolIsEleet").GetBoolean();
                                    int intPkResource = jsonResouceData.GetProperty("intPkResource").GetInt32();
                                    int intEstimationId = intIEstimation + 1;

                                    //                      //Create Json.
                                    EstjsonEstimationDataJson estjson = new EstjsonEstimationDataJson(intJobId,
                                        intEstimationId, intPkProcessInWorkflow, intPkEleetOrEleele, boolIsEleet,
                                        intPkResource);

                                    darrestjson.Add(estjson);
                                }
                                else
                                {
                                    boolValidData = false;
                                    strDevMessage = "Invalid resource.";
                                }
                                intIResource = intIResource + 1;
                            }
                        }
                        else
                        {
                            boolValidData = false;
                            strDevMessage = "Invalid arrestim items.";
                        }

                        intIEstimation = intIEstimation + 1;
                    }
                }
                else
                {
                    boolValidData = false;
                    strDevMessage = "Invalid arrestim.";
                }

                //                                          //Add estimations if the data is valid.
                if (
                    boolValidData
                    )
                {
                    //                                      //Get Printshop.
                    var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                    String strPrintshopId = idClaim.Value;

                    //                                      //using is to release connection at the end
                    using (Odyssey2Context context = new Odyssey2Context())
                    {
                        //                                  //Starts a new transaction.
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                JobJob.subAddEstimates(strPrintshopId, darrestjson, strBaseDate, strBaseTime,
                                    context, this.configuration, ref intStatus, ref strUserMessage,
                                    ref strDevMessage);

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
            else
            {
                strDevMessage = "Invalid data or JobId.";
            }

            //                                              //Response to the frontend.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = Ok(respjson1);

            return aresult;
        }

        //------------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult ConfirmResources(
           //                                              //PURPOSE:
           //                                              //Confirm the resources of an estimation as the resources for
           //                                              //      the wf of the job.

           //                                              //URL: http://localhost/Odyssey2/Job
           //                                              //      /ConfirmResources
           //                                              //Method: POST.

           //                                              //Use a JSON like this:
           //                                              //{
           //                                              //    "intJobId":45490,
           //                                              //    "intEstimationId":1, 
           //                                              //    "strPassword":"49894655",
           //                                              //    "arrpro":
           //                                              //    [
           //                                              //        {
           //                                              //            "intPkProcessInWorkflow":2,
           //                                              //            "arrres":
           //                                              //            [
           //                                              //                {
           //                                              //                    "intPkEleetOrEleele": 2,
           //                                              //                    "boolIsEleet": true,
           //                                              //                    "intPkResource": 34
           //                                              //                },
           //                                              //                {
           //                                              //                    "intPkEleetOrEleele": 4,
           //                                              //                    "boolIsEleet": false,
           //                                              //                    "intPkresource": 34
           //                                              //                }
           //                                              //            ]
           //                                              //        },
           //                                              //        {
           //                                              //            "intPkProcessInWorkflow":3,
           //                                              //            "arrres":
           //                                              //            [
           //                                              //                {
           //                                              //                    "intPkEleetOrEleele": 4,
           //                                              //                    "boolIsEleet": true,
           //                                              //                    "intPkResource": 12
           //                                              //                },
           //                                              //                {
           //                                              //                    "intPkEleetOrEleele": 5,
           //                                              //                    "boolIsEleet": false,
           //                                              //                    "intPkResource": 17
           //                                              //                }
           //                                              //            ]
           //                                              //        }
           //                                              //    ]
           //                                              //}

           //                                               //DESCRIPTION:
           //                                               //If the data from the front is the same as the data in the
           //                                               //      estimationdata table, the registers are added to
           //                                               //      the ioj table. 

           //                                               //RETURNS:
           //                                               //      200 - Ok

           //                                               //Receives JSON with data.
           [FromBody] JsonElement jsonConfirmData
           )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if jobId is a number.
                !Object.ReferenceEquals(null, jsonConfirmData) &&

                jsonConfirmData.TryGetProperty("intJobId", out json) &&
                (int)jsonConfirmData.GetProperty("intJobId").ValueKind == 4 &&
                jsonConfirmData.TryGetProperty("intEstimationId", out json) &&
                (int)jsonConfirmData.GetProperty("intEstimationId").ValueKind == 4 &&
                jsonConfirmData.TryGetProperty("arrpro", out json) &&
                (int)jsonConfirmData.GetProperty("arrpro").ValueKind == 2
                )
            {
                //                                          //Get data.
                int intJobId = jsonConfirmData.GetProperty("intJobId").GetInt32();
                int intEstimationId = jsonConfirmData.GetProperty("intEstimationId").GetInt32();
                JsonElement jsonProcessData = jsonConfirmData.GetProperty("arrpro");

                String strPassword = null;
                if (
                    jsonConfirmData.TryGetProperty("strPassword", out json) &&
                    (int)jsonConfirmData.GetProperty("strPassword").ValueKind == 3
                    )
                {
                    strPassword = jsonConfirmData.GetProperty("strPassword").GetString();
                }

                //                                          //List to keep the resources to confirm.
                List<EstjsonEstimationDataJson> darrestjson = new List<EstjsonEstimationDataJson>();

                bool boolValidData = true;

                int intProcessCount = jsonProcessData.GetArrayLength();
                int intI = 0;
                while (
                    (intI < intProcessCount) &&
                    boolValidData
                    )
                {
                    JsonElement jsonProcessDataI = jsonProcessData[intI];

                    int intPkProcessInWorkflowI = 0;
                    if (
                        //                              //Verify if intPkProcessInWorkflow is a number.
                        jsonProcessDataI.TryGetProperty("intPkProcessInWorkflow", out json) &&
                        (int)jsonProcessDataI.GetProperty("intPkProcessInWorkflow").ValueKind == 4
                        )
                    {
                        //                              //Get the intPkProcessInWorkflow.
                        intPkProcessInWorkflowI = jsonProcessDataI.GetProperty("intPkProcessInWorkflow").GetInt32();
                    }
                    else
                    {
                        boolValidData = false;
                    }

                    if (
                        boolValidData
                        )
                    {
                        if (
                            //                              //Verify arrres is different from null.
                            jsonProcessDataI.TryGetProperty("arrres", out json) &&
                            (int)jsonProcessDataI.GetProperty("arrres").ValueKind != 7
                            )
                        {
                            //                              //Get arrres data.
                            JsonElement jsonResouceDataI = jsonProcessDataI.GetProperty("arrres");

                            int intResourceCountI = jsonResouceDataI.GetArrayLength();
                            int intJ = 0;
                            while (
                                (intJ < intResourceCountI) &&
                                boolValidData
                                )
                            {
                                JsonElement jsonResouceDataIJ = jsonResouceDataI[intJ];

                                if (
                                    //                      //Verify if resouce data.
                                    jsonResouceDataIJ.TryGetProperty("intPkEleetOrEleele", out json) &&
                                    (int)jsonResouceDataIJ.GetProperty("intPkEleetOrEleele").ValueKind == 4 &&
                                    jsonResouceDataIJ.TryGetProperty("boolIsEleet", out json) &&
                                    ((int)jsonResouceDataIJ.GetProperty("boolIsEleet").ValueKind == 5 ||
                                    (int)jsonResouceDataIJ.GetProperty("boolIsEleet").ValueKind == 6) &&
                                    jsonResouceDataIJ.TryGetProperty("intPkResource", out json) &&
                                    (int)jsonResouceDataIJ.GetProperty("intPkResource").ValueKind == 4
                                    )
                                {
                                    int intPkEleetOrEleeleIJ =
                                        jsonResouceDataIJ.GetProperty("intPkEleetOrEleele").GetInt32();
                                    bool boolIsEleetIJ = jsonResouceDataIJ.GetProperty("boolIsEleet").GetBoolean();
                                    int intPkResourceIJ = jsonResouceDataIJ.GetProperty("intPkResource").GetInt32();

                                    //                      //Create Json.
                                    EstjsonEstimationDataJson estjson = new EstjsonEstimationDataJson(intJobId,
                                        intEstimationId, intPkProcessInWorkflowI, intPkEleetOrEleeleIJ,
                                        boolIsEleetIJ, intPkResourceIJ);

                                    darrestjson.Add(estjson);
                                }
                                else
                                {
                                    //                      //IO and resource data invalid.
                                    boolValidData = false;
                                }
                                intJ = intJ + 1;
                            }
                        }
                        else
                        {
                            //                              //No resources.
                            boolValidData = false;
                        }
                    }
                    intI = intI + 1;
                }

                if (
                    boolValidData
                    )
                {
                    //                                      //Get Printshop.
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
                                //                          //Method to confirm resources.
                                bool boolAreAllPeriodsAddables;
                                JobJob.subConfirmResources(ps, darrestjson, this.configuration, strPassword, context,
                                    out boolAreAllPeriodsAddables, ref intStatus, ref strUserMessage, ref strDevMessage);
                                obj = boolAreAllPeriodsAddables;

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

            //                                              //Response to the frontend.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = Ok(respjson1);
            return aresult;
        }

        //------------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult UpdateCostOrQuantity(
           //                                               //PURPOSE:
           //                                               //Set a final cost or final quantity of a resource or 
           //                                               //  process.                                        
           //                                               //URL: http://localhost/Odyssey2/Job
           //                                               //      /UpdateCostOrQuantity
           //                                               //Method: POST.
           //                                               //Use a JSON like this:
           //                                               //      {
           //                                               //          "intnPkCalculation" : 3,
           //                                               //          "intnPkResource" : null,
           //                                               //          "intPkEleetOrEleele" : 5,
           //                                               //          "boolIsEleet" : true,
           //                                               //          "intJobId": 159652 ,
           //                                               //          "intPkProcessInWorkflow": 12,
           //                                               //          "numnFinalQuantity": 120,
           //                                               //          "numnFinalCost": 0,
           //                                               //          "strDescription": "Por qué cambiamos el costo.",
           //                                               //          "intPkAccountMovement": 1
           //                                               //      }

           //                                               //DESCRIPTION:
           //                                               //Update finalCost table ir order to get a final cost
           //                                               //      of a process or resource.

           //                                               //RETURNS:
           //                                               //      200 - Ok

           //                                               //Receives JSON with data.
           [FromBody] JsonElement jsonFinalData
           )
        {
            //                                              //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;

            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null or a value is not empty.
                !((int)jsonFinalData.ValueKind == 7) &&
                !((int)jsonFinalData.ValueKind == 0) &&
                jsonFinalData.TryGetProperty("intJobId", out json) &&
                (int)jsonFinalData.GetProperty("intJobId").ValueKind == 4 &&
                jsonFinalData.TryGetProperty("intPkProcessInWorkflow", out json) &&
                (int)jsonFinalData.GetProperty("intPkProcessInWorkflow").ValueKind == 4 &&
                jsonFinalData.TryGetProperty("intPkAccountMovement", out json) &&
                (int)jsonFinalData.GetProperty("intPkAccountMovement").ValueKind == 4
                )
            {
                //                                          //Get values.                
                int intJobId = jsonFinalData.GetProperty("intJobId").GetInt32();
                int intPkProcessInWorkflow = jsonFinalData.GetProperty("intPkProcessInWorkflow").GetInt32();
                int intPkAccountMovement = jsonFinalData.GetProperty("intPkAccountMovement").GetInt32();

                int? intnPkCalculation = null;
                if (
                    jsonFinalData.TryGetProperty("intnPkCalculation", out json) &&
                    (int)jsonFinalData.GetProperty("intnPkCalculation").ValueKind == 4
                    )
                {
                    intnPkCalculation = jsonFinalData.GetProperty("intnPkCalculation").GetInt32();
                }

                int? intnPkResource = null;
                if (
                    jsonFinalData.TryGetProperty("intnPkResource", out json) &&
                    (int)jsonFinalData.GetProperty("intnPkResource").ValueKind == 4
                    )
                {
                    intnPkResource = jsonFinalData.GetProperty("intnPkResource").GetInt32();
                }

                double? numnFinalQuantity = null;
                if (
                    jsonFinalData.TryGetProperty("numnFinalQuantity", out json) &&
                    (int)jsonFinalData.GetProperty("numnFinalQuantity").ValueKind == 4
                    )
                {
                    numnFinalQuantity = jsonFinalData.GetProperty("numnFinalQuantity").GetDouble();
                }

                double? numnFinalCost = null;
                if (
                    jsonFinalData.TryGetProperty("numnFinalCost", out json) &&
                    (int)jsonFinalData.GetProperty("numnFinalCost").ValueKind == 4
                    )
                {
                    numnFinalCost = jsonFinalData.GetProperty("numnFinalCost").GetDouble();
                }

                String strDescription = "";
                if (
                    jsonFinalData.TryGetProperty("strDescription", out json) &&
                    (int)jsonFinalData.GetProperty("strDescription").ValueKind == 3
                    )
                {
                    strDescription = jsonFinalData.GetProperty("strDescription").GetString();
                }

                int? intnPkEleetOrEleele = null;
                if (
                    jsonFinalData.TryGetProperty("intnPkEleetOrEleele", out json) &&
                    (int)jsonFinalData.GetProperty("intnPkEleetOrEleele").ValueKind == 4
                    )
                {
                    intnPkEleetOrEleele = jsonFinalData.GetProperty("intnPkEleetOrEleele").GetInt32();
                }

                bool? boolnIsEleet = null;
                if (
                    (((int)jsonFinalData.GetProperty("boolnIsEleet").ValueKind == 5) ||
                    ((int)jsonFinalData.GetProperty("boolnIsEleet").ValueKind == 6))
                    )
                {
                    boolnIsEleet = jsonFinalData.GetProperty("boolnIsEleet").GetBoolean();
                }

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
                            //                              //Method to set a final cost.
                            double numFinalCalculatedCost;
                            JobJob.subUpdateCostOrQuantity(intContactId, intnPkCalculation, intnPkResource,
                                strDescription, intnPkEleetOrEleele, boolnIsEleet, intJobId, intPkProcessInWorkflow,
                                strPrintshopId, numnFinalQuantity, numnFinalCost, intPkAccountMovement,
                                this.configuration, context, out numFinalCalculatedCost, ref intStatus,
                                ref strUserMessage, ref strDevMessage);
                            obj = numFinalCalculatedCost;

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

            //                                              //Response to the frontend.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult ConfirmResourceAutomaticallySet(
            //                                              //PURPOSE:
            //                                              //Delete a workflow.

            //                                              //URL: http://localhost/Odyssey2/Job
            //                                              //      /ConfirmResourceAutomaticallySet
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intJobId":5519145,
            //                                              //          "intPkProcessInWorkflow":5,
            //                                              //          "intPkEleetOrEleele":3,
            //                                              //          "boolIsEleet":true
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Confirm a resource that was automatically set.
            //                                              //

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            //                                              //Json that contains all data.
            [FromBody] JsonElement jsonIO
            )
        {
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;

            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !Object.ReferenceEquals(null, jsonIO) &&
                //                                          //Verify if the data is not null or empty.
                jsonIO.TryGetProperty("intJobId", out json) &&
                (int)jsonIO.GetProperty("intJobId").ValueKind == 4 &&
                jsonIO.TryGetProperty("intPkProcessInWorkflow", out json) &&
                (int)jsonIO.GetProperty("intPkProcessInWorkflow").ValueKind == 4 &&
                jsonIO.TryGetProperty("intPkEleetOrEleele", out json) &&
                (int)jsonIO.GetProperty("intPkEleetOrEleele").ValueKind == 4 &&
                jsonIO.TryGetProperty("boolIsEleet", out json) &&
                ((int)jsonIO.GetProperty("boolIsEleet").ValueKind == 5 ||
                (int)jsonIO.GetProperty("boolIsEleet").ValueKind == 6)
                )
            {
                int intJobId = jsonIO.GetProperty("intJobId").GetInt32();
                int intPkProcessInWorkflow = jsonIO.GetProperty("intPkProcessInWorkflow").GetInt32();
                int intPkEleetOrEleele = jsonIO.GetProperty("intPkEleetOrEleele").GetInt32();
                bool boolIsEleet = jsonIO.GetProperty("boolIsEleet").GetBoolean();

                try
                {
                    JobJob.subConfirmResourceAutomaticallySet(strPrintshopId, intJobId, intPkProcessInWorkflow,
                        intPkEleetOrEleele, boolIsEleet, this.configuration, ref intStatus, ref strUserMessage,
                        ref strDevMessage);
                }
                catch (Exception ex)
                {
                    //                          //Making a log for the exception.
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
        public IActionResult CopyConfirmedEstimate(
        //                                              //PURPOSE:
        //                                              //Copy a confirmed estimate.

        //                                              //URL: http://localhost/Odyssey2/Job/
        //                                              //      CopyConfirmedEstimate

        //                                              //Method: GET.

        //                                              //DESCRIPTION:
        //                                              //Copy a confirmed estimate.

        //                                              //Receive a json.  
        //                                              //{
        //                                              //  "intJobId":5723931,
        //                                              //  "intPkWorkflow":3
        //                                              //}
        //                                              //RETURNS:
        //                                              //      200 - Ok

        [FromBody] JsonElement jobData
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !((int)jobData.ValueKind == 7) &&
                !((int)jobData.ValueKind == 0) &&
                //                                         //Verify the necessary properties.
                jobData.TryGetProperty("intJobId", out json) &&
                (int)jobData.GetProperty("intJobId").ValueKind == 4 &&
                jobData.TryGetProperty("intPkWorkflow", out json) &&
                (int)jobData.GetProperty("intPkWorkflow").ValueKind == 4
                )
            {
                int intJobId = jobData.GetProperty("intJobId").GetInt32();
                int intPkWorkflow = jobData.GetProperty("intPkWorkflow").GetInt32();

                if (
                (intJobId > 0) &&
                (intPkWorkflow > 0)
                )
                {
                    //                                          //Get the printshop id from token.
                    var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                    String strPrintshopId = idClaim.Value;
                    PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                    using (Odyssey2Context context = new Odyssey2Context())
                    {
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                //                          //Get list of Estimation's id.
                                JobJob.subCopyConfirmedEstimate(intJobId, intPkWorkflow, ps, this.configuration,
                                    context, ref intStatus, ref strUserMessage, ref strDevMessage);
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
                                //                                      //Making a log for the exception.
                                Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                            }
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
        public IActionResult SetQuantityForEstimate(
            //                                              //PURPOSE:
            //                                              //Set Quantity to a Confirmed Estimate.

            //                                              //URL: http://localhost/Odyssey2/Job/
            //                                              //      SetQuantityForEstimate

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Set a Quantity to confirmed estimate.

            //                                              //Receive a json.

            //                                              //RETURNS:
            //                                              //      200 - Ok

            [FromBody] JsonElement jobData
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !((int)jobData.ValueKind == 7) &&
                !((int)jobData.ValueKind == 0) &&
                //                                         //Verify the necessary properties.
                jobData.TryGetProperty("intJobId", out json) &&
                (int)jobData.GetProperty("intJobId").ValueKind == 4 &&
                jobData.TryGetProperty("intPkWorkflow", out json) &&
                (int)jobData.GetProperty("intPkWorkflow").ValueKind == 4 &&
                jobData.TryGetProperty("intQuantity", out json) &&
                (int)jobData.GetProperty("intQuantity").ValueKind == 4 &&
                jobData.TryGetProperty("intCopyNumber", out json) &&
                (int)jobData.GetProperty("intCopyNumber").ValueKind == 4
                )
            {
                int intJobId = jobData.GetProperty("intJobId").GetInt32();
                int intPkWorkflow = jobData.GetProperty("intPkWorkflow").GetInt32();
                int intQuatity = jobData.GetProperty("intQuantity").GetInt32();
                int intCopyNumber = jobData.GetProperty("intCopyNumber").GetInt32();

                if (
                (intJobId > 0) &&
                (intPkWorkflow > 0)
                )
                {
                    //                                          //Get the printshop id from token.
                    var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                    String strPrintshopId = idClaim.Value;
                    PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                    using (Odyssey2Context context = new Odyssey2Context())
                    {
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                //                          //Get list of Estimation's id.
                                JobJob.subSetQuantityForEstimate(intJobId, intPkWorkflow, intQuatity, intCopyNumber,
                                    ps, this.configuration,
                                    context, ref intStatus, ref strUserMessage, ref strDevMessage);
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
                                //                                      //Making a log for the exception.
                                Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                            }
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
        public IActionResult CreateNewEstimate(
            //                                              //PURPOSE:
            //                                              //Create an estimate from scratch.

            //                                              //URL: http://localhost/Odyssey2/Job/
            //                                              //      CreateNewEstimate

            //                                              //Method: Post.

            //                                              //Receive a json like this.
            //                                              //  {
            //                                              //      "strName": EstimationName,
            //                                              //      "intContactId": 1030575,
            //                                              //      "intQuantity": 20
            //                                              //  }

            //                                              //DESCRIPTION:
            //                                              //Create an estimate from scratch.

            //                                              //RETURNS:
            //                                              //      200 - Ok

            [FromBody] JsonElement estimateData
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !((int)estimateData.ValueKind == 7) &&
                !((int)estimateData.ValueKind == 0) &&
                //                                         //Verify the necessary properties.
                estimateData.TryGetProperty("strName", out json) &&
                (int)estimateData.GetProperty("strName").ValueKind == 3 &&
                estimateData.TryGetProperty("intContactId", out json) &&
                (int)estimateData.GetProperty("intContactId").ValueKind == 4 &&
                estimateData.TryGetProperty("intQuantity", out json) &&
                (int)estimateData.GetProperty("intQuantity").ValueKind == 4
                )
            {
                String strName = estimateData.GetProperty("strName").GetString();
                strName = strName.TrimExcel();
                int intContactId = estimateData.GetProperty("intContactId").GetInt32();
                int intQuantity = estimateData.GetProperty("intQuantity").GetInt32();

                //                                              //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                intStatus = 400;
                strUserMessage = "Something is wrong.";
                strDevMessage = "Printshop not valid.";
                obj = null;
                if (
                    ps != null &&
                    intContactId > 0 &&
                    intQuantity > 0
                    )
                {
                    using (Odyssey2Context context = new Odyssey2Context())
                    {
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                int intJobId;
                                JobJob.subCreateNewEstimate(intQuantity, ps, strName, intContactId, context,
                                    out intJobId, ref intStatus, ref strUserMessage, ref strDevMessage);
                                obj = intJobId;

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
                                //                              //Making a log for the exception.
                                Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                            }
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
        public IActionResult SetResourceEstimate(
            //                                              //PURPOSE:
            //                                              //Create an estimate from scratch.

            //                                              //URL: http://localhost/Odyssey2/Job/
            //                                              //      CreateNewEstimate

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Create an estimate from scratch, only validate the 
            //                                              //      printshop.

            //                                              //RETURNS:
            //                                              //      200 - Ok

            [FromBody] JsonElement resEstData
            )
        {
            //                                          //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !((int)resEstData.ValueKind == 7) &&
                !((int)resEstData.ValueKind == 0) &&
                //                                         //Verify the necessary properties.
                resEstData.TryGetProperty("intJobId", out json) &&
                (int)resEstData.GetProperty("intJobId").ValueKind == 4 &&
                resEstData.TryGetProperty("intPkProcessInWorkflow", out json) &&
                (int)resEstData.GetProperty("intPkProcessInWorkflow").ValueKind == 4 &&
                resEstData.TryGetProperty("intPkResource", out json) &&
                (int)resEstData.GetProperty("intPkResource").ValueKind == 4 &&
                resEstData.TryGetProperty("intPkEleetOrEleele", out json) &&
                (int)resEstData.GetProperty("intPkEleetOrEleele").ValueKind == 4 &&
                resEstData.TryGetProperty("boolIsEleet", out json) &&
                ((int)resEstData.GetProperty("boolIsEleet").ValueKind == 5 ||
                (int)resEstData.GetProperty("boolIsEleet").ValueKind == 6)
                )
            {
                //                                          //Get info.
                int intJobId = resEstData.GetProperty("intJobId").GetInt32();
                int intPkProcessInWorkflow = resEstData.GetProperty("intPkProcessInWorkflow").GetInt32();
                int intPkResource = resEstData.GetProperty("intPkResource").GetInt32();
                int intPkEleetOrEleele = resEstData.GetProperty("intPkEleetOrEleele").GetInt32();
                bool boolIsEleet = resEstData.GetProperty("boolIsEleet").GetBoolean();

                if (
                    ps != null
                    )
                {
                    using (Odyssey2Context context = new Odyssey2Context())
                    {
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                JobJob.subSetResourceEstimate(intJobId, intPkProcessInWorkflow, intPkResource,
                                    intPkEleetOrEleele, boolIsEleet, ps, this.configuration, context, ref intStatus,
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
                                //                                      //Making a log for the exception.
                                Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                            }
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
        public IActionResult RenameEstimation(
            //                                              //PURPOSE:
            //                                              //Modify an estimation's name.

            //                                              //URL: http://localhost/Odyssey2/Job
            //                                              //      /RenameEstimation
            //                                              //Method: POST.
            //                                              //Json:
            //                                              //  {
            //                                              //      "intJobId":2652563,
            //                                              //      "intEstimationId":1,
            //                                              //      "intPkWorkflow":1,
            //                                              //      "strName": "My Estimation"
            //                                              //   }

            //                                              //DESCRIPTION:
            //                                              //Modify an estimation's name.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            //                                              //Json that contains all data.
            [FromBody] JsonElement estimData
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !Object.ReferenceEquals(null, estimData) &&
                //                                          //Verify the necessary properties.
                estimData.TryGetProperty("intJobId", out json) &&
                (int)estimData.GetProperty("intJobId").ValueKind == 4 &&
                estimData.TryGetProperty("intEstimationId", out json) &&
                (int)estimData.GetProperty("intEstimationId").ValueKind == 4 &&
                estimData.TryGetProperty("intPkWorkflow", out json) &&
                (int)estimData.GetProperty("intPkWorkflow").ValueKind == 4 &&
                estimData.TryGetProperty("strName", out json) &&
                (int)estimData.GetProperty("strName").ValueKind == 3
                )
            {
                int intJobId = estimData.GetProperty("intJobId").GetInt32();
                int intEstimationId = estimData.GetProperty("intEstimationId").GetInt32();
                int intPkWorkflow = estimData.GetProperty("intPkWorkflow").GetInt32();
                String strName = estimData.GetProperty("strName").GetString();

                if (
                    intJobId > 0 &&
                    intEstimationId >= 0 &&
                    intPkWorkflow > 0
                   )
                {
                    //                                      //Get the printshop id from token.
                    var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                    String strPrintshopId = idClaim.Value;
                    PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                    try
                    {
                        JobJob.subRenameEstimation(intJobId, intEstimationId, intPkWorkflow, strName, ps,
                            this.configuration, ref intStatus, ref strUserMessage, ref strDevMessage);
                    }
                    catch (Exception ex)
                    {
                        //                                  //Making a log for the exception.
                        Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                    }
                }
            }

            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage, obj);
            IActionResult aresult = base.Ok(respjson1);

            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult SendJobPrice(
        //                                              //PURPOSE:
        //                                              //Send job's price to wisnet.

        //                                              //URL: http://localhost/Odyssey2/Job/
        //                                              //      SendJobPrice

        //                                              //Method: POST.

        //                                              //DESCRIPTION:
        //                                              //Send the estimate prices to wisnet.

        //                                              //Receive a json.  
        //                                              //{
        //                                              //  "intJobId":5723931,
        //                                              //  "intPkWorkflow":3
        //                                              //}

        //                                              //RETURNS:
        //                                              //      200 - Ok

        [FromBody] JsonElement jobData
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !((int)jobData.ValueKind == 7) &&
                !((int)jobData.ValueKind == 0) &&
                //                                         //Verify the necessary properties.
                jobData.TryGetProperty("intJobId", out json) &&
                (int)jobData.GetProperty("intJobId").ValueKind == 4 &&
                jobData.TryGetProperty("intPkWorkflow", out json) &&
                (int)jobData.GetProperty("intPkWorkflow").ValueKind == 4 &&
                jobData.TryGetProperty("boolSendEmail", out json) &&
                ((int)jobData.GetProperty("boolSendEmail").ValueKind == 5 ||
                (int)jobData.GetProperty("boolSendEmail").ValueKind == 6)
                )
            {
                int intJobId = jobData.GetProperty("intJobId").GetInt32();
                int intPkWorkflow = jobData.GetProperty("intPkWorkflow").GetInt32();
                bool boolSendEmail = jobData.GetProperty("boolSendEmail").GetBoolean();

                if (
                    (intJobId > 0) &&
                    (intPkWorkflow > 0)
                    )
                {
                    //                                      //Get the printshop id from token.
                    var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                    String strPrintshopId = idClaim.Value;
                    PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                    //                                      //Get the contact id from token.
                    var idClaimContact = User.Claims.FirstOrDefault(c => c.Type == "intContactId");
                    int intContactId = idClaimContact.Value.ParseToInt();

                    try
                    {
                        JobJob.subSendJobPrice(intJobId, intContactId, intPkWorkflow, boolSendEmail, ps,
                            this.configuration, this.hubContext, ref intStatus, ref strUserMessage, ref strDevMessage);
                    }
                    catch (Exception ex)
                    {
                        //                                  //Making a log for the exception.
                        Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
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
        public IActionResult SetJobAsPending(
        //                                              //PURPOSE:
        //                                              //Convert a job in eswtimating or waiting for price approval in 
        //                                              //      a job pending.

        //                                              //URL: http://localhost/Odyssey2/Job/SetJobAsPending

        //                                              //Method: POST.
        //                                              //Use a body like this:
        //                                              //{
        //                                              //    "intJobId":556789,
        //                                              //    "intPkWorkflow":5,
        //                                              //    "intEstimationId":0
        //                                              //}

        //                                              //DESCRIPTION:
        //                                              //Approve an estimate from Odyssey.

        [FromBody] JsonElement jobData
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !((int)jobData.ValueKind == 7) &&
                !((int)jobData.ValueKind == 0) &&
                //                                         //Verify the necessary properties.
                jobData.TryGetProperty("intJobId", out json) &&
                (int)jobData.GetProperty("intJobId").ValueKind == 4 &&
                jobData.TryGetProperty("intPkWorkflow", out json) &&
                (int)jobData.GetProperty("intPkWorkflow").ValueKind == 4 &&
                jobData.TryGetProperty("intEstimationId", out json) &&
                (int)jobData.GetProperty("intEstimationId").ValueKind == 4
                )
            {
                int intJobId = jobData.GetProperty("intJobId").GetInt32();
                int intPkWorkflow = jobData.GetProperty("intPkWorkflow").GetInt32();
                int intEstimationId = jobData.GetProperty("intEstimationId").GetInt32();

                if (
                    intJobId > 0 &&
                    intPkWorkflow > 0
                    )
                {
                    //                                      //Get data from token.
                    var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                    String strPrintshopId = idClaim.Value;
                    PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                    var idClaimContact = User.Claims.FirstOrDefault(c => c.Type == "intContactId");
                    int intContactId = idClaimContact.Value.ParseToInt();

                    using (Odyssey2Context context = new Odyssey2Context())
                    {
                        //                                  //Starts a new transaction.
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                JobJob.subSetJobAsPending(intJobId, intEstimationId, intPkWorkflow, intContactId, ps,
                                    context, this.configuration, this.hubContext, ref intStatus, ref strUserMessage,
                                    ref strDevMessage);

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

                                intStatus = 407;
                                strUserMessage = "Something is wrong.";
                                strDevMessage = ex.Message;
                            }
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
        public IActionResult UpdateWorkInProgressStatus(
            //                                              //PURPOSE:
            //                                              //Update a workInProgress's subStage.

            //                                              //URL: http://localhost/Odyssey2/Job/UpdateWorkInProgressStatus

            //                                              //Method: POST.
            //                                              //Use a body like this:
            //                                              //{
            //                                              //    "intJobId":556789,
            //                                              //    "strStatus": "AQGStatus"
            //                                              //}

            //                                              //DESCRIPTION:
            //                                              //Update a subStage for a job in progress at wisnet.

            [FromBody] JsonElement jobData
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !((int)jobData.ValueKind == 7) &&
                !((int)jobData.ValueKind == 0) &&
                //                                          //Verify the necessary properties.
                jobData.TryGetProperty("intJobId", out json) &&
                (int)jobData.GetProperty("intJobId").ValueKind == 4 &&
                jobData.TryGetProperty("strStatus", out json) &&
                (int)jobData.GetProperty("strStatus").ValueKind == 3 &&
                jobData.TryGetProperty("boolSendEmail", out json) &&
                ((int)jobData.GetProperty("boolSendEmail").ValueKind == 5 ||
                (int)jobData.GetProperty("boolSendEmail").ValueKind == 6)
                )
            {
                int intJobId = jobData.GetProperty("intJobId").GetInt32();
                String strStatus = jobData.GetProperty("strStatus").GetString();
                strStatus = strStatus.TrimExcel();
                bool boolSendEmail = jobData.GetProperty("boolSendEmail").GetBoolean();

                if (
                    intJobId > 0 &&
                    strStatus.Length > 0
                    )
                {
                    //                                      //Get data from token.
                    var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                    String strPrintshopId = idClaim.Value;
                    PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                    var idClaimContact = User.Claims.FirstOrDefault(c => c.Type == "intContactId");
                    int intContactId = idClaimContact.Value.ParseToInt();

                    using (Odyssey2Context context = new Odyssey2Context())
                    {
                        try
                        {
                            JobJob.subUpdateWorkInProgressStatus(intJobId, strStatus, boolSendEmail, ps, intContactId,
                                this.configuration, context, ref intStatus, ref strUserMessage, ref strDevMessage);

                        }
                        catch (Exception ex)
                        {
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
        public IActionResult DeleteProcessNote(
            //                                              //PURPOSE:
            //                                              //Delete a process note.

            //                                              //URL: http://localhost/Odyssey2/Job/
            //                                              //      DeleteProcessNote

            //                                              //Method: POST.

            //                                              //DESCRIPTION:
            //                                              //Delete a process note from a job.

            //                                              //Receive a json.  
            //                                              //{
            //                                              //  "intPkNote":3
            //                                              //}

            //                                              //RETURNS:
            //                                              //      200 - Ok

            [FromBody] JsonElement jobData
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !((int)jobData.ValueKind == 7) &&
                !((int)jobData.ValueKind == 0) &&
                //                                         //Verify the necessary properties.
                jobData.TryGetProperty("intPkNote", out json) &&
                (int)jobData.GetProperty("intPkNote").ValueKind == 4
                )
            {
                int intPkNote = jobData.GetProperty("intPkNote").GetInt32();

                if (
                    (intPkNote > 0)
                    )
                {
                    //                                      //Get the printshop id from token.
                    var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                    String strPrintshopId = idClaim.Value;

                    //                                      //Get the contact id from token.
                    var idClaimContact = User.Claims.FirstOrDefault(c => c.Type == "intContactId");
                    int intContactId = idClaimContact.Value.ParseToInt();

                    try
                    {
                        JobJob.subDeleteProcessNote(intPkNote, intContactId, strPrintshopId, ref intStatus,
                            ref strUserMessage, ref strDevMessage);
                    }
                    catch (Exception ex)
                    {
                        //                                  //Making a log for the exception.
                        Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
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
        public IActionResult SendEmailToCustomer(
            //                                              //PURPOSE:
            //                                              //Send an email to a customer

            //                                              //URL: http://localhost/Odyssey2/Job/SendEmailToCustomer

            //                                              //Method: POST.
            //                                              //Use a body like this:
            //                                              //{
            //                                              //    "intJobId":556789
            //                                              //}

            //                                              //DESCRIPTION:
            //                                              //Send an email to a customer when a order is completed.

            [FromBody] JsonElement jobData
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !((int)jobData.ValueKind == 7) &&
                !((int)jobData.ValueKind == 0) &&
                //                                          //Verify the necessary properties.
                jobData.TryGetProperty("intJobId", out json) &&
                (int)jobData.GetProperty("intJobId").ValueKind == 4
                )
            {
                int intJobId = jobData.GetProperty("intJobId").GetInt32();

                List<int> darrintOrdersIdPaid = new List<int>();
                if (
                    jobData.TryGetProperty("arrintOrdersId", out json) &&
                    (int)jobData.GetProperty("arrintOrdersId").ValueKind == 2
                    )
                {
                    for (int intU = 0; intU < jobData.GetProperty("arrintOrdersId").GetArrayLength();
                            intU = intU + 1)
                    {
                        JsonElement json1 = jobData.GetProperty("arrintOrdersId")[intU];

                        darrintOrdersIdPaid.Add(json1.GetInt32());
                    }
                }
                else
                {
                    darrintOrdersIdPaid = null;
                }

                if (
                    intJobId >= 0
                    )
                {
                    //                                      //Get data from token.
                    var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                    String strPrintshopId = idClaim.Value;
                    PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                    var idClaimContact = User.Claims.FirstOrDefault(c => c.Type == "intContactId");
                    int intContactId = idClaimContact.Value.ParseToInt();

                    using (Odyssey2Context context = new Odyssey2Context())
                    {
                        try
                        {
                            JobJob.subSendEmailToCustomer(intJobId, ps, intContactId, darrintOrdersIdPaid,
                                this.configuration, ref intStatus, ref strUserMessage, ref strDevMessage);
                        }
                        catch (Exception ex)
                        {
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
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult Get(
           //                                              //PURPOSE:
           //                                              //Get info for an specific job.

           //                                              //URL: http://localhost:5001/Odyssey2/Job
           //                                              //      /Get?intJobId=5515963

           //                                              //Method: GET.

           //                                              //DESCRIPTION:
           //                                              //Get the information of just 1 job.

           //                                              //RETURNS:
           //                                              //      200 - Ok().

           //                                              //
           int intJobId,
           int intPkWorkflow
           )
        {
            //                                          //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;

            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;

            if (
                //                                          //Verify if the printshop is not null, job and workflow is 
                //                                          //      not less than 0.
                ps != null && intJobId > 0 && intPkWorkflow > 0
                )
            {
                try
                {
                    JobticjsonJobTicketJson jobticjson = JobJob.jobticjsonGetJobInfo(ps, intJobId, intPkWorkflow,
                        this.configuration, ref intStatus, ref strUserMessage, ref strDevMessage);
                    obj = jobticjson;
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
        public IActionResult GetEstimateWorkflow(
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
                            obj = JobJob.subGetEstimateWorkflow(intJobId, ps, context, this.configuration,
                                 ref intStatus, ref strUserMessage, ref strDevMessage);

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
        public IActionResult GetPrintshopJobs(
            //                                              //PURPOSE:
            //                                              //Get all jobs of a printshop.

            //                                              //URL: http://localhost:5001/Odyssey2/Job
            //                                              //      /GetPrintshopJobs?boolnPendingStage=null&
            //                                              //      boolnInProgressStage=null&
            //                                              //      boolnCompletedStage=true
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get jobs filters of a printshop.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            [FromBody] JsonElement jsonStages
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
                //                                          //Verify if the object is not null.
                !((int)jsonStages.ValueKind == 7) &&
                !((int)jsonStages.ValueKind == 0)
                )
            {
                try
                {
                    bool? boolnUnsubmitted = null;
                    if (
                        jsonStages.TryGetProperty("boolnUnsubmitted", out json) &&
                        (int)jsonStages.GetProperty("boolnUnsubmitted").ValueKind == 5
                        )
                    {
                        boolnUnsubmitted = jsonStages.GetProperty("boolnUnsubmitted").GetBoolean();
                    }

                    bool? boolnInEstimating = null;
                    if (
                        jsonStages.TryGetProperty("boolnInEstimating", out json) &&
                        (int)jsonStages.GetProperty("boolnInEstimating").ValueKind == 5
                        )
                    {
                        boolnInEstimating = jsonStages.GetProperty("boolnInEstimating").GetBoolean();
                    }

                    bool? boolnWaitingForPriceApproval = null;
                    if (
                        jsonStages.TryGetProperty("boolnWaitingForPriceApproval", out json) &&
                        (int)jsonStages.GetProperty("boolnWaitingForPriceApproval").ValueKind == 5
                        )
                    {
                        boolnWaitingForPriceApproval = jsonStages.GetProperty("boolnWaitingForPriceApproval").
                            GetBoolean();
                    }

                    bool? boolnPending = null;
                    if (
                        jsonStages.TryGetProperty("boolnPending", out json) &&
                        (int)jsonStages.GetProperty("boolnPending").ValueKind == 5
                        )
                    {
                        boolnPending = jsonStages.GetProperty("boolnPending").GetBoolean();
                    }

                    bool? boolnInProgress = null;
                    if (
                        jsonStages.TryGetProperty("boolnInProgress", out json) &&
                        (int)jsonStages.GetProperty("boolnInProgress").ValueKind == 5
                        )
                    {
                        boolnInProgress = jsonStages.GetProperty("boolnInProgress").GetBoolean();
                    }

                    bool? boolnCompleted = null;
                    if (
                        jsonStages.TryGetProperty("boolnCompleted", out json) &&
                        (int)jsonStages.GetProperty("boolnCompleted").ValueKind == 5
                        )
                    {
                        boolnCompleted = jsonStages.GetProperty("boolnCompleted").GetBoolean();
                    }

                    bool? boolnNotPaid = null;
                    if (
                        jsonStages.TryGetProperty("boolnNotPaid", out json) &&
                        (int)jsonStages.GetProperty("boolnNotPaid").ValueKind == 5
                        )
                    {
                        boolnNotPaid = jsonStages.GetProperty("boolnNotPaid").GetBoolean();
                    }

                    bool? boolnAll = null;
                    if (
                        jsonStages.TryGetProperty("boolnAll", out json) &&
                        (int)jsonStages.GetProperty("boolnAll").ValueKind == 5
                        )
                    {
                        boolnAll = jsonStages.GetProperty("boolnAll").GetBoolean();
                    }

                    //                                          //using is to release connection at the end
                    using (Odyssey2Context context = new Odyssey2Context())
                    {
                        //                                      //Starts a new transaction.
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                List<JobjsonJobJson> darrjobjson;
                                JobJob.subGetPrintshopJobs(ps, boolnUnsubmitted, boolnInEstimating,
                                    boolnWaitingForPriceApproval, boolnPending, boolnInProgress, boolnCompleted,
                                    boolnNotPaid, boolnAll, this.configuration, this.hubContext, context,
                                    out darrjobjson, ref intStatus, ref strUserMessage, ref strDevMessage);
                                obj = darrjobjson;

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
        public IActionResult GetPrintshopEstimates(
            //                                              //PURPOSE:
            //                                              //Get all estimate of a printshop.

            //                                              //URL: http://localhost:5001/Odyssey2/Job
            //                                              //      /GetPrintshopEstimate
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get Esimate filters of a printshop.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            bool boolRequested,
            bool boolWaitingForCustResponse,
            bool boolRejected
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

                using (Odyssey2Context context = new Odyssey2Context())
                {
                    //                                      //Starts a new transaction.
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            List<EstimjsonEstimateJson> darrestimjson;
                            JobJob.subGetPrintshopEstimates(ps, boolRequested, boolWaitingForCustResponse,
                                boolRejected, this.configuration, out darrestimjson, context, ref intStatus,
                                ref strUserMessage, ref strDevMessage);
                            obj = darrestimjson;

                            //                              //Commits all changes made to the database in the current
                            //                              //      transaction.
                            if (
                                intStatus == 200
                                )
                            {
                                dbContextTransaction.Commit();
                                strUserMessage = "";
                                strDevMessage = "";
                            }
                            else
                            {
                                dbContextTransaction.Rollback();
                            }
                        }
                        catch (Exception ex)
                        {
                            //                              //Making a log for the exception.
                            Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);

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
        [HttpGet("[action]")]
        public IActionResult GetFilesUrl(
            //                                              //PURPOSE:
            //                                              //Get url for job's files.

            //                                              //URL: http://localhost:5001/Odyssey2/Job
            //                                              //      /GetFilesUrl?intJobId=5515963
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get the url for each job's file.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            //                                              //
            int intJobId
            )
        {
            //                                          //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;

            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            if (
                //                                          //Verify if the printshop is not null
                ps != null &&
                intJobId > 0
                )
            {
                try
                {
                    List<FileurljsonFileUrlJson> darrfileurljson;
                    JobJob.subGetFilesUrl(ps, intJobId,
                        this.configuration, out darrfileurljson, ref intStatus, ref strUserMessage, ref strDevMessage);
                    obj = darrfileurljson;
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
        public IActionResult GetNotes(
            //                                              //PURPOSE:
            //                                              //Get notes for an specific job

            //                                              //URL: http://localhost:5001/Odyssey2/Job
            //                                              //      /GetNotes?intJobId=5515963

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Look notes for a job, if the job has notes send them
            //                                              //      back.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            int intJobId,
            int intPkWorkflow
            )
        {
            //                                          //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;

            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            if (
                //                                          //Verify if the printshop is not null.
                ps != null &&
                intJobId > 0 &&
                intPkWorkflow > 0
                )
            {
                obj = JobJob.subGetNotes(intJobId, intPkWorkflow, ps, this.configuration, ref intStatus,
                    ref strUserMessage, ref strDevMessage);
            }
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetProcessNotes(
            //                                              //PURPOSE:
            //                                              //Get notes for an specific process

            //                                              //URL: http://localhost:5001/Odyssey2/Job
            //                                              //      /GetProcessNotes?intnPkPeriod=2&
            //                                              //      intnPkProcessInWorkflow=12&intJobId=5816655

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Look fot all the notes for a process.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            int? intnPkPeriod,
            int? intnPkProcessInWorkflow,
            int intJobId
            )
        {
            //                                          //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;

            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            try
            {
                obj = JobJob.pronotesjsonGetProcessNotes(intJobId, intnPkPeriod, intnPkProcessInWorkflow, 
                    strPrintshopId, this.configuration, ref intStatus, ref strUserMessage, ref strDevMessage);
            }
            catch (Exception ex)
            {
                //                                      //Making a log for the exception.
                Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
            }

            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetQuantities(
           //                                              //PURPOSE:
           //                                              //Get job types and the quantity of jobs for those types.

           //                                              //URL: http://localhost/Odyssey2/Workflow/GetQuantities

           //                                              //Method: GET.

           //                                              //DESCRIPTION:
           //                                              //Get the types and quantities.

           //                                              //RETURNS:
           //                                              //      200 - Ok
           )
        {
            //                                              //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;

            //                                          //Get the contact id from token.
            var idClaimContact = User.Claims.FirstOrDefault(c => c.Type == "intContactId");
            int intContactId = idClaimContact.Value.ParseToInt();

            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

            int intStatus = 400;
            String strUserMessage = "Invalid data.";
            String strDevMessage = "";
            Object obj = null;
            if (
                ps != null
                )
            {
                using (Odyssey2Context context = new Odyssey2Context())
                {
                    //                                  //Starts a new transaction.
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            JobandqtysonJobTypeAndQuantityJson[] arrjobandqtyjson;
                            JobJob.subCountJobs(ps, intContactId, this.configuration, out arrjobandqtyjson, context, ref intStatus,
                                ref strUserMessage, ref strDevMessage);

                            obj = arrjobandqtyjson;

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
        [HttpGet("[action]")]
        public IActionResult GetEstimatesQuantities(
           //                                              //PURPOSE:
           //                                              //Get estimates and their quantities.

           //                                              //URL: http://localhost/Odyssey2/Job/GetEstimatesQuantities

           //                                              //Method: GET.

           //                                              //DESCRIPTION:
           //                                              //Get the types and quantities.

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
                    JobandqtysonJobTypeAndQuantityJson[] arrestquantityjson;
                    JobJob.subCountEstimates(ps, this.configuration, out arrestquantityjson, ref intStatus,
                        ref strUserMessage, ref strDevMessage);
                    obj = arrestquantityjson;
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
        public IActionResult GetOptions(
            //                                              //PURPOSE:
            //                                              //Get the options availables for one job according with the 
            //                                              //      resources set as possible in or out in a workflow.

            //                                              //URL: http://localhost/Odyssey2/Job/GetOptions

            //                                              //Method: GET.
            //                                              //Use a JSON like this:
            //                                              //  {
            //                                              //      "intJobId":19144,
            //                                              //      "intPkWorkflow":3,
            //                                              //      "arrresSelected":
            //                                              //      [
            //                                              //          {
            //                                              //              "intProcessInWorkflowId":2,
            //                                              //              "intPkEleetOrEleele":1,
            //                                              //              "intnPkResource":null
            //                                              //          },
            //                                              //          {
            //                                              //              "intProcessInWorkflowId":3,
            //                                              //              "intPkEleetOrEleele":2,
            //                                              //              "intnPkResource":null
            //                                              //          }
            //                                              //      ]
            //                                              //  }

            //                                              //DESCRIPTION:
            //                                              //Get the types and quantities.

            //                                              //RETURNS:
            //                                              //      200 - Ok

            [FromBody] JsonElement jsonJob
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
                //                                          //Verify if the object is not null.
                !Object.ReferenceEquals(null, jsonJob) &&
                //                                          //Verify the necessary properties.
                jsonJob.TryGetProperty("intJobId", out json) &&
                (int)jsonJob.GetProperty("intJobId").ValueKind == 4 &&
                jsonJob.TryGetProperty("intPkWorkflow", out json) &&
                (int)jsonJob.GetProperty("intPkWorkflow").ValueKind == 4 &&
                jsonJob.TryGetProperty("arrresSelected", out json) &&
                ((int)jsonJob.GetProperty("arrresSelected").ValueKind == 2 ||
                (int)jsonJob.GetProperty("arrresSelected").ValueKind == 7) &&
                jsonJob.TryGetProperty("intId", out json) &&
                (int)jsonJob.GetProperty("intId").ValueKind == 4
                )
            {
                int intPkWorkflow = jsonJob.GetProperty("intPkWorkflow").GetInt32();
                int intJobId = jsonJob.GetProperty("intJobId").GetInt32();
                int intId = jsonJob.GetProperty("intId").GetInt32();

                //                                          //Get date properties.
                String strBaseDate;
                if (
                    jsonJob.TryGetProperty("strBaseDate", out json) &&
                    (int)jsonJob.GetProperty("strBaseDate").ValueKind == 3
                    )
                {
                    strBaseDate = jsonJob.GetProperty("strBaseDate").GetString();
                }
                else
                {
                    strBaseDate = Date.Now(ZonedTimeTools.timezone).ToString();
                }

                String strBaseTime;
                if (
                    jsonJob.TryGetProperty("strBaseTime", out json) &&
                    (int)jsonJob.GetProperty("strBaseTime").ValueKind == 3
                    )
                {
                    strBaseTime = jsonJob.GetProperty("strBaseTime").GetString();

                    ZonedTime zonedTime = ZonedTimeTools.NewZonedTimeConsideringPrintshopTimeZone(
                        strBaseDate.ParseToDate(), strBaseTime.ParseToTime(),ps.strTimeZone);
                    strBaseDate = zonedTime.Date.ToString();
                    strBaseTime = zonedTime.Time.ToString();
                }
                else
                {
                    strBaseTime = (Time.Now(ZonedTimeTools.timezone) + 3600).ToString();
                }

                List<ResestimjsonResourceEstimatedJson> darrresestimjson2ResourceSelected = new
                    List<ResestimjsonResourceEstimatedJson>();

                if (
                    (int)jsonJob.GetProperty("arrresSelected").ValueKind == 2
                    )
                {
                    for (int intU = 0; intU < jsonJob.GetProperty("arrresSelected").GetArrayLength(); intU = intU + 1)
                    {
                        JsonElement json1 = jsonJob.GetProperty("arrresSelected")[intU];

                        int intPkProcessInWorkflow = json1.GetProperty("intPkProcessInWorkflow").GetInt32();
                        int intPkEleetOrEleele = json1.GetProperty("intPkEleetOrEleele").GetInt32();
                        bool boolIsEleet = json1.GetProperty("boolIsEleet").GetBoolean();
                        int? intnPkResource = null;
                        if (
                            json1.TryGetProperty("intnPk", out json) &&
                            (int)json1.GetProperty("intnPk").ValueKind == 4
                            )
                        {
                            intnPkResource = json1.GetProperty("intnPk").GetInt32();
                        }

                        ResestimjsonResourceEstimatedJson resestimjson2 = new ResestimjsonResourceEstimatedJson(
                                intPkProcessInWorkflow, intPkEleetOrEleele, boolIsEleet, intnPkResource, null, null,
                                null, null, false);

                        darrresestimjson2ResourceSelected.Add(resestimjson2);
                    }
                }

                try
                {
                    EstimopjsonEstimationOptionsJson estimopjson;
                    JobJob.subGetOptionsForEstimate(strPrintshopId, intJobId, intId, intPkWorkflow,
                        darrresestimjson2ResourceSelected, strBaseDate, strBaseTime, this.configuration,
                        out estimopjson, ref intStatus, ref strUserMessage, ref strDevMessage);
                    obj = estimopjson;
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
        public IActionResult GetCalendar(
            //                                              //PURPOSE:
            //                                              //Get the calendar of the job.

            //                                              //URL: http://localhost/Odyssey2/Job/GetCalendar?intJobId=1
            //                                              //      &intPkWorkflow=3

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get the job calendar

            //                                              //RETURNS:
            //                                              //      200 - Ok

            int intJobId,
            int intPkWorkflow,
            String strSunday
            )
        {
            //                                              //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;

            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            if (
                (intJobId > 0) &&
                (intPkWorkflow > 0)
                )
            {
                try
                {
                    LvlpiwjsonLevelAndPIWJson lvlpiwjson;

                    //                                      //Method that gets the calendar.
                    JobJob.subGetCalendar(strPrintshopId, intJobId, intPkWorkflow, strSunday, this.configuration,
                        out lvlpiwjson, ref intStatus, ref strDevMessage, ref strUserMessage);
                    obj = lvlpiwjson;
                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }
            }

            //                                              //Response to the frontend.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetPeriodsFromOneProcess(
            //                                              //PURPOSE:
            //                                              //Get Process'periods.

            //                                              //URL: http://localhost/Odyssey2/Job/
            //                                              //      GetPeriodsFromOneProcess?
            //                                              //      intJobId=3653511&intPkPIW=3

            //                                              //Method: GET.

            //                                              //DESCRIPTION:

            //                                              //RETURNS:
            //                                              //      200 - Ok

            int intJobId,
            int intPkPIW
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
                (intJobId > 0) &&
                (intPkPIW > 0)
                )
            {
                try
                {
                    PerfrmpiwjsonPeriodFromPIWJson perfrmpiwjson;

                    //                                      //Method that gets periods from a PIW.
                    JobJob.subGetPeriodsFromOneProcess(ps, intJobId, intPkPIW, this.configuration, out perfrmpiwjson,
                        ref intStatus, ref strDevMessage, ref strUserMessage);
                    obj = perfrmpiwjson;
                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }
            }

            //                                              //Response to the frontend.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetPeriodsForAJobAndWorkflow(
            //                                              //PURPOSE:
            //                                              //Get periods for a job with an especific workflow.

            //                                              //URL: http://localhost/Odyssey2/Job/
            //                                              //      GetPeriodsForAJobAndWorkflow?
            //                                              //      intJobId=3653511&intPkWorkflow=3

            //                                              //Method: GET.

            //                                              //DESCRIPTION:

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
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            if (
                (intJobId > 0) &&
                (intPkWorkflow > 0)
                )
            {
                try
                {
                    PerjobjsonPeriodsJobJson perjobjson;

                    //                                      //Method that gets periods from a job and an especific
                    //                                      //      workflow.
                    JobJob.subGetPeriodsForAJobAndWorkflow(ps, intJobId, intPkWorkflow, this.configuration,
                        out perjobjson, ref intStatus, ref strDevMessage, ref strUserMessage);
                    obj = perjobjson;
                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }
            }

            //                                              //Response to the frontend.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetProcessesAndCalculationsWithPeriods(
            //                                              //PURPOSE:
            //                                              //Get job's processes with calculations and periods.

            //                                              //URL: http://localhost/Odyssey2/Job/GetProcessesAnd
            //                                              //      CalculationsWithPeriods?intJobId=1&intPkWorkflow=3

            //                                              //Method: GET.

            //                                              //DESCRIPTION:

            //                                              //RETURNS:
            //                                              //      200 - Ok

            int intJobId,
            int intPkWorkflow,
            String strSunday
            )
        {
            //                                              //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;

            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            if (
                (intJobId > 0) &&
                (intPkWorkflow > 0)
                )
            {
                try
                {
                    Projson2ProcessJson2[] arrprojson2;

                    //                                      //Method that gets the calendar.
                    JobJob.subGetProcessesAndCalculationsWithPeriods(strPrintshopId, intJobId, intPkWorkflow, strSunday,
                        this.configuration, out arrprojson2, ref intStatus, ref strDevMessage, ref strUserMessage);
                    obj = arrprojson2;
                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }
            }

            //                                              //Response to the frontend.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetProcessCostEstimateAndFinalFromJob(
           //                                              //PURPOSE:
           //                                              //Get Estimate cost and Final cost of a each process from 
           //                                              //     a Job.

           //                                              //URL: http://localhost/Odyssey2/job/
           //                                              //   GetProcessCostEstimateAndFinalFromJob/intJobId=1562
           //                                              //   &intPkWorkflow=2

           //                                              //Method: GET.

           //                                              //DESCRIPTION:
           //                                              //Get Estimate cost and Final Cost of each process.

           //                                              //RETURNS:
           //                                              //      200 - Ok
           int intJobId,
           int intPkWorkflow
           )
        {
            //                                          //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;

            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

            int intStatus = 400;
            String strUserMessage = "Invalid data.";
            String strDevMessage = "";
            Object obj = null;
            if (
                ps != null &&
                intJobId > 0 &&
                intPkWorkflow > 0
                )
            {
                try
                {
                    FcsumjsonFinalCostsSummaryJson fcsumjson;
                    JobJob.subGetProcessCostEstimateAndFinalFromJob(ps, intJobId, intPkWorkflow, this.configuration,
                        out fcsumjson, ref intStatus, ref strUserMessage, ref strDevMessage);

                    obj = fcsumjson;
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
        public IActionResult GetProcessFinalCostLog(
           //                                              //PURPOSE:
           //                                              //Get log related to a final.

           //                                              //URL: http://localhost/Odyssey2/Job/
           //                                              //       GetProcessFinalCostLog?intPkFinal

           //                                              //Method: GET.

           //                                              //DESCRIPTION:
           //                                              //Get all entries in the final table for a specific cla or res.

           //                                              //RETURNS:
           //                                              //      200 - Ok

           int intPkFinal
           )
        {
            //                                          //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;

            int intStatus = 400;
            String strUserMessage = "Invalid data.";
            String strDevMessage = "";
            Object obj = null;
            if (
                intPkFinal > 0
                )
            {
                try
                {
                    Fnlcostjson2FinalCostJson2[] arrfnlcostjson2;
                    JobJob.subGetProcessFinalCostLog(strPrintshopId, intPkFinal, out arrfnlcostjson2, ref intStatus,
                        ref strUserMessage, ref strDevMessage);
                    obj = arrfnlcostjson2;
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
        public IActionResult GetProcessFinalCostData(
           //                                              //PURPOSE:
           //                                              //Get all costs realated to a process in workflow.

           //                                              //URL: http://localhost/Odyssey2/Job/
           //                                              //       GetProcessFinalCostData?intJobId=9&
           //                                              //       intPkProcessInWorkflow=34&intPkProduct=334

           //                                              //Method: GET.

           //                                              //DESCRIPTION:
           //                                              //Get all resources and process costs in an specific 
           //                                              //       process in workflow to change their properties.

           //                                              //RETURNS:
           //                                              //      200 - Ok

           int intJobId,
           int intPkProcessInWorkflow,
           int intPkProduct
           )
        {
            int intStatus = 400;
            String strUserMessage = "Invalid data.";
            String strDevMessage = "";
            Object obj = null;
            if (
                intJobId > 0 &&
                intPkProcessInWorkflow > 0 &&
                intPkProduct > 0
                )
            {
                //                                          //Get Printshop.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;

                try
                {
                    FnlcostjsonFinalCostJson fnlcostjsonFinalCostJson;
                    JobJob.subGetProcessFinalCostData(strPrintshopId, intJobId, intPkProduct, intPkProcessInWorkflow,
                        this.configuration, out fnlcostjsonFinalCostJson, ref intStatus, ref strUserMessage,
                        ref strDevMessage);
                    obj = fnlcostjsonFinalCostJson;
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
        public IActionResult GetResourcePeriodsInIoFromJob(
            //                                              //PURPOSE:
            //                                              //Get resource'periods in IOJ from Job.

            //                                              //URL: http://localhost/Odyssey2/Job/
            //                                              //      GetResourcePeriodsInIojFromJob?intJobId=1

            //                                              //Method: GET.

            //                                              //DESCRIPTION:

            //                                              //RETURNS:
            //                                              //      200 - Ok

            int intJobId,
            int intPkProcessInWorkflow,
            int intPkResource,
            int intPkEleetOrEleele,
            bool boolIsEleet
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;

            if (
                (intJobId > 0) &&
                (intPkResource > 0) &&
                (intPkEleetOrEleele > 0)
                )
            {
                //                                          //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;

                try
                {
                    PeriresjsonPeriodResourceJson[] periresjson;
                    ResResource.subGetResourcePeriodsInIoFromJob(intJobId, strPrintshopId, intPkProcessInWorkflow,
                        intPkResource, intPkEleetOrEleele, boolIsEleet, this.configuration, out periresjson, ref intStatus,
                        ref strUserMessage, ref strDevMessage);

                    obj = periresjson;
                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }
            }

            //                                              //Response to the frontend.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetDueDateLog(
           //                                              //PURPOSE:
           //                                              //Get log related to the due date.

           //                                              //URL: http://localhost/Odyssey2/Job/
           //                                              //       GetDueDateLog?intJobId=000

           //                                              //Method: GET.

           //                                              //DESCRIPTION:
           //                                              //Get all entries in the due date table for a given job.

           //                                              //RETURNS:
           //                                              //      200 - Ok

           int intJobId
           )
        {
            //                                          //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;

            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            if (
                intJobId > 0
                )
            {
                try
                {
                    DuedatejsonDueDateJson[] arrduedatejson;
                    JobJob.subGetDueDateLog(strPrintshopId, intJobId, this.configuration, out arrduedatejson, ref intStatus,
                        ref strUserMessage, ref strDevMessage);
                    obj = arrduedatejson;
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
        public IActionResult GetPriceLog(
           //                                              //PURPOSE:
           //                                              //Get log related to a job's or estimate's price.

           //                                              //URL: http://localhost/Odyssey2/Job/
           //                                              //       GetPriceLog?intJobId=000&intPkWorkflow=2&
           //                                              //       intnEstimateId=null

           //                                              //Method: GET.

           //                                              //DESCRIPTION:
           //                                              //Get all entries in the price table for a given job or
           //                                              //       estimate.

           //                                              //RETURNS:
           //                                              //      200 - Ok

           int intJobId,
           int intPkWorkflow,
           int? intnEstimateId,
           int? intnCopyNumber
           )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            if (
                intJobId > 0 &&
                intPkWorkflow > 0
                )
            {
                //                                          //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;

                try
                {
                    PricejsonPriceJson[] arrpricejson;
                    JobJob.subGetPriceLog(strPrintshopId, intJobId, intPkWorkflow, intnEstimateId, intnCopyNumber,
                        this.configuration, out arrpricejson, ref intStatus, ref strUserMessage, ref strDevMessage);
                    obj = arrpricejson;
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
        public IActionResult GetResourcesFromIoGroup(
            //                                              //PURPOSE:
            //                                              //Get resources inside a io group.

            //                                              //URL: http://localhost/Odyssey2/Job/
            //                                              //      GetResourcesFromIoGroup?intJobId=53564&
            //                                              //      intPkProcessInWorkflow=1&
            //                                              //      intPkEleetOrEleele=13&
            //                                              //      boolIsEleet=true

            //                                              //Method: GET.

            //                                              //DESCRIPTION:

            //                                              //RETURNS:
            //                                              //      200 - Ok

            int intJobId,
            int intPkProcessInWorkflow,
            int intPkEleetOrEleele,
            bool boolIsEleet
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            if (
                (intPkProcessInWorkflow > 0) &&
                (intJobId > 0) &&
                (intPkEleetOrEleele > 0)
                )
            {
                //                                          //Get Printshop.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;

                try
                {
                    obj = JobJob.arrresjson4GetResourcesFromIoGroup(strPrintshopId, intPkProcessInWorkflow, intJobId,
                        intPkEleetOrEleele, boolIsEleet, this.configuration, ref intStatus, ref strUserMessage,
                        ref strDevMessage);
                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }
            }

            //                                              //Response to the frontend.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetBudgetEstimation(
            //                                              //PURPOSE:
            //                                              //Get Budget estimation from job.

            //                                              //URL: http://localhost/Odyssey2/Job/
            //                                              //      GetBudgetEstimation?

            //                                              //Method: GET.

            //                                              //DESCRIPTION:

            //                                              //RETURNS:
            //                                              //      200 - Ok

            int intJobId,
            int intPkWorkflow,
            int? intnEstimationId,
            int? intnCopyNumber,
            String strBaseDate,
            String strBaseTime
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            if (
                (intJobId > 0) &&
                (intPkWorkflow > 0) &&
                (intnEstimationId >= -1)
                )
            {
                //                                          //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                using (Odyssey2Context context = new Odyssey2Context())
                {
                    //                                      //Starts a new transaction.
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            BdgestjsonBudgetEstimationJson bdgestjson;
                            JobJob.subGetBudgetEstimation(intJobId, intPkWorkflow, intnEstimationId, intnCopyNumber,
                                strBaseDate, strBaseTime, ps, this.configuration, out bdgestjson, context,
                                ref intStatus, ref strUserMessage, ref strDevMessage);

                            obj = bdgestjson;

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
        [HttpGet("[action]")]
        public IActionResult GetEstimationsIds(
            //                                              //PURPOSE:
            //                                              //Get a list of Estimation's id.

            //                                              //URL: http://localhost/Odyssey2/Job/
            //                                              //      GetEstimationsIds?intJobId=5515963&intPkWorkflow=3

            //                                              //Method: GET.

            //                                              //DESCRIPTION:

            //                                              //RETURNS:
            //                                              //      200 - Ok

            int intJobId,
            int intPkWorkflow
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
                (intJobId > 0) &&
                (intPkWorkflow > 0)
                )
            {
                try
                {
                    EstjsonEstimationJson estjson;
                    JobJob.subGetEstimationsIds(intJobId, intPkWorkflow, ps, this.configuration, out estjson,
                            ref intStatus, ref strUserMessage, ref strDevMessage);

                    obj = estjson;
                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }
            }

            //                                              //Response to the frontend.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetEstimations(
            //                                              //PURPOSE:
            //                                              //Get Estimations.

            //                                              //URL: http://localhost/Odyssey2/Job/
            //                                              //      GetEstimations

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get an array of estimations for a specific job and 
            //                                              //      workflow.

            //                                              //RETURNS:
            //                                              //      200 - Ok

            int intJobId,
            int intPkWorkflow
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            if (
                (intJobId > 0) &&
                (intPkWorkflow > 0)
                )
            {
                //                                          //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                try
                {
                    //                                      //Get list of Estimation's id.
                    EstjsonEstimationJson estjson;
                    JobJob.subGetEstimationsIds(intJobId, intPkWorkflow, ps, this.configuration, out estjson,
                            ref intStatus, ref strUserMessage, ref strDevMessage);
                    if (
                        intStatus == 200 &&
                        estjson.arrest.Count() == 0
                        )
                    {
                        intStatus = 404;
                        strUserMessage = "Something is wrong.";
                        strDevMessage = "There are not estimations for the given Job or Workflow.";

                    }
                    else if (
                      intStatus == 200 &&
                      estjson.arrest.Count() > 0
                      )
                    {
                        Odyssey2Context context = new Odyssey2Context();
                        List<BdgestjsonBudgetEstimationJson> darrbdgestjson = new List<BdgestjsonBudgetEstimationJson>();
                        foreach (Estjson2EstimationDataJson2 estjson2 in estjson.arrest)
                        {
                            BdgestjsonBudgetEstimationJson bdgestjson;
                            JobJob.subGetBudgetEstimation(intJobId, intPkWorkflow, estjson2.intnEstimationId,
                                estjson2.intnCopyNumber, null, null, ps, this.configuration, out bdgestjson,
                                context, ref intStatus, ref strUserMessage, ref strDevMessage);

                            bdgestjson.strName = bdgestjson.strName + (estjson2.intnCopyNumber == null ? "" :
                                "(" + estjson2.intnCopyNumber + ")");

                            darrbdgestjson.Add(bdgestjson);
                        }
                        obj = darrbdgestjson;
                    }
                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }
            }

            //                                              //Response to the frontend.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetEstimationsDetails(
            //                                              //PURPOSE:
            //                                              //Get Estimations details.

            //                                              //URL: http://localhost/Odyssey2/Job/
            //                                              //      GetEstimationsDetails

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get details for an estimation confirmed and their copies.
            //                                              //Details are name, quantity and price.

            //                                              //RETURNS:
            //                                              //      200 - Ok

            int intJobId,
            int intPkWorkflow
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            if (
                (intJobId > 0) &&
                (intPkWorkflow > 0)
                )
            {
                //                                          //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                try
                {
                    //                                      //Get estimation's details.
                    List<EstdetjsonEstimationDetailsJson> darrestdetjson;
                    JobJob.subGetEstimationsDetails(intJobId, intPkWorkflow, ps, this.configuration, out darrestdetjson,
                            ref intStatus, ref strUserMessage, ref strDevMessage);
                    obj = darrestdetjson;
                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
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
        public IActionResult SendEstimatesPrices(
        //                                              //PURPOSE:
        //                                              //Send estimate prices to wisnet.

        //                                              //URL: http://localhost/Odyssey2/Job/
        //                                              //      SendEstimatesPrices

        //                                              //Method: GET.

        //                                              //DESCRIPTION:
        //                                              //Send the estimate prices to wisnet.

        //                                              //Receive a json.  
        //                                              //{
        //                                              //  "intJobId":5723931,
        //                                              //  "intPkWorkflow":3
        //                                              //}

        //                                              //RETURNS:
        //                                              //      200 - Ok

        [FromBody] JsonElement jobData
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !((int)jobData.ValueKind == 7) &&
                !((int)jobData.ValueKind == 0) &&
                //                                         //Verify the necessary properties.
                jobData.TryGetProperty("intJobId", out json) &&
                (int)jobData.GetProperty("intJobId").ValueKind == 4 &&
                jobData.TryGetProperty("intPkWorkflow", out json) &&
                (int)jobData.GetProperty("intPkWorkflow").ValueKind == 4 &&
                jobData.TryGetProperty("boolSendEmail", out json) &&
                ((int)jobData.GetProperty("boolSendEmail").ValueKind == 5 ||
                (int)jobData.GetProperty("boolSendEmail").ValueKind == 6)
                )
            {
                int intJobId = jobData.GetProperty("intJobId").GetInt32();
                int intPkWorkflow = jobData.GetProperty("intPkWorkflow").GetInt32();
                bool boolSendEmail = jobData.GetProperty("boolSendEmail").GetBoolean();

                if (
                    (intJobId > 0) &&
                    (intPkWorkflow > 0)
                    )
                {
                    //                                      //Get the printshop id from token.
                    var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                    String strPrintshopId = idClaim.Value;
                    PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                    //                                          //Get the contact id from token.
                    var idClaimContact = User.Claims.FirstOrDefault(c => c.Type == "intContactId");
                    int intContactId = idClaimContact.Value.ParseToInt();

                    using (Odyssey2Context context = new Odyssey2Context())
                    {
                        //                                  //Starts a new transaction.
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                JobJob.subSendEstimatesPrices(intJobId, intContactId, intPkWorkflow, ps,
                                    boolSendEmail, this.configuration, this.hubContext, context, ref intStatus,
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

                                intStatus = 407;
                                strUserMessage = "Something is wrong.";
                                strDevMessage = ex.Message;
                            }
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
        public IActionResult EstimateToOrder(
        //                                              //PURPOSE:
        //                                              //Convert Estimate to Order.

        //                                              //URL: http://localhost/Odyssey2/Job/
        //                                              //      EstimateToOrder

        //                                              //Method: GET.

        //                                              //DESCRIPTION:
        //                                              //Turn Estimate into Order.

        //                                              //Receive a json.  
        //                                              //{
        //                                              //  "intJobId":5723931,
        //                                              //}

        //                                              //RETURNS:
        //                                              //      200 - Ok

        [FromBody] JsonElement jobData
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !((int)jobData.ValueKind == 7) &&
                !((int)jobData.ValueKind == 0) &&
                //                                         //Verify the necessary properties.
                jobData.TryGetProperty("intJobId", out json) &&
                (int)jobData.GetProperty("intJobId").ValueKind == 4 &&
                jobData.TryGetProperty("intPkWorkflow", out json) &&
                (int)jobData.GetProperty("intPkWorkflow").ValueKind == 4 &&
                jobData.TryGetProperty("intEstimationId", out json) &&
                (int)jobData.GetProperty("intEstimationId").ValueKind == 4
                )
            {
                int intJobId = jobData.GetProperty("intJobId").GetInt32();
                int intPkWorkflow = jobData.GetProperty("intPkWorkflow").GetInt32();
                int intEstimationId = jobData.GetProperty("intEstimationId").GetInt32();

                //                                          //Set the product, if it comes, get it from the json.
                int? intnCopyNumber = null;
                if (
                    jobData.TryGetProperty("intnCopyNumber", out json) &&
                    (int)jobData.GetProperty("intnCopyNumber").ValueKind == 4
                    )
                {
                    intnCopyNumber = jobData.GetProperty("intnCopyNumber").GetInt32();
                }

                if (
                    intJobId > 0 &&
                    intPkWorkflow > 0
                    )
                {
                    //                                      //Get the printshop id from token.
                    var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                    String strPrintshopId = idClaim.Value;
                    PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                    //                                          //Get the contact id from token.
                    var idClaimContact = User.Claims.FirstOrDefault(c => c.Type == "intContactId");
                    int intContactId = idClaimContact.Value.ParseToInt();

                    using (Odyssey2Context context = new Odyssey2Context())
                    {
                        //                                  //Starts a new transaction.
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                JobJob.subEstimateToOrder(intJobId, intEstimationId, intPkWorkflow, intnCopyNumber,
                                    intContactId, ps, context, this.configuration, ref intStatus, ref strUserMessage,
                                    ref strDevMessage);

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

                                intStatus = 407;
                                strUserMessage = "Something is wrong.";
                                strDevMessage = ex.Message;
                            }
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
        public IActionResult EstimateToRejected(
        //                                              //PURPOSE:
        //                                              //Convert Estimate to rejected.

        //                                              //URL: http://localhost/Odyssey2/Job/
        //                                              //      EstimateToRejected

        //                                              //Method: GET.

        //                                              //DESCRIPTION:
        //                                              //Turn Estimate into Rejected.

        //                                              //Receive a json.  
        //                                              //{
        //                                              //  "intJobId":5723931,
        //                                              //}

        //                                              //RETURNS:
        //                                              //      200 - Ok

        [FromBody] JsonElement jobData
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !((int)jobData.ValueKind == 7) &&
                !((int)jobData.ValueKind == 0) &&
                //                                         //Verify the necessary properties.
                jobData.TryGetProperty("intJobId", out json) &&
                (int)jobData.GetProperty("intJobId").ValueKind == 4
                )
            {
                int intJobId = jobData.GetProperty("intJobId").GetInt32();

                if (
                    intJobId > 0
                    )
                {
                    //                                      //Get the printshop id from token.
                    var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                    String strPrintshopId = idClaim.Value;
                    PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                    //                                          //Get the contact id from token.
                    var idClaimContact = User.Claims.FirstOrDefault(c => c.Type == "intContactId");
                    int intContactId = idClaimContact.Value.ParseToInt();

                    using (Odyssey2Context context = new Odyssey2Context())
                    {
                        //                                  //Starts a new transaction.
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                JobJob.subEstimateToRejected(intJobId, ps, intContactId, context, this.configuration,
                                    ref intStatus, ref strUserMessage, ref strDevMessage);

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

                                intStatus = 407;
                                strUserMessage = "Something is wrong.";
                                strDevMessage = ex.Message;
                            }
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
        [AllowAnonymous]
        [HttpPost("[action]")]
        public IActionResult NotifyNewOrderOrEstimate(
            //                                              //PURPOSE:
            //                                              //Send notifications when an order or estimate has been
            //                                              //      requested from printer site.

            //                                              //URL: http://localhost/Odyssey2/Job/
            //                                              //      NotifyNewOrderOrEstimate

            //                                              //Method: POST.

            //                                              //DESCRIPTION:
            //                                              //This service will be requested by Harmony system.

            //                                              //RETURNS:
            //                                              //      200 - Ok

            [FromBody] XmlElement xmlelement
            )
        {
            //                                              //Object response.
            Object obj = null;

            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid xml content.";

            String strXml;
            XmlDocument xmldocument;
            String strTypeOfSchema;
            if (
                xmlelement != null &&
                boolIsValidXml(xmlelement, out strXml, out xmldocument, ref intStatus, ref strUserMessage,
                    ref strDevMessage) &&
                boolIsOrderOrEstimate(xmldocument, out strTypeOfSchema, ref intStatus, ref strUserMessage,
                    ref strDevMessage) &&
                boolIsValidXmlComparedToSchema(strXml, strTypeOfSchema, out xmldocument, ref intStatus,
                    ref strUserMessage, ref strDevMessage)
                )
            {
                try
                {
                    XPathNavigator xpathnav = xmldocument.CreateNavigator();
                    xpathnav.MoveToRoot();

                    if (
                        strTypeOfSchema == JobController.strEstimate
                        )
                    {
                        JobJob.subNotifyNewEstimate(xpathnav, ref intStatus, ref strUserMessage, ref strDevMessage,
                            this.configuration, this.hubContext);
                    }
                    else if (
                      strTypeOfSchema == JobController.strOrder
                      )
                    {
                        JobJob.subNotifyNewOrder(xpathnav, ref intStatus, ref strUserMessage, ref strDevMessage,
                            this.configuration, this.hubContext);
                    }

                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }
            }

            //                                              //Response to the frontend.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = Ok(respjson1);
            return aresult;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private bool boolIsValidXml(
            //                                              //PURPOSE:
            //                                              //Helper method to validate an XML. It returns the 
            //                                              //      XmlDocument instance if XML passed as string is
            //                                              //      valid.

            XmlElement xmlelement_I,
            out String strXml_O,
            out XmlDocument xmldocument_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO

            )
        {
            bool boolIsValidXml = false;
            strXml_O = null;
            xmldocument_O = null;

            try
            {
                //                                          //The XML is validated when it's invoking the 
                //                                          //      XmlReader.Create method.
                strXml_O = xmlelement_I.OuterXml;
                TextReader textreader = new StringReader(strXml_O);
                XmlReader xmlreader = XmlReader.Create(textreader);

                //                                          //Once the XML has been validated the
                //                                          //      XmlDocument is created.
                XmlDocument xmldocument = new XmlDocument();
                xmldocument.PreserveWhitespace = true;
                xmldocument.Load(xmlreader);
                textreader.Close();
                xmlreader.Close();

                //                                          //Returned values.
                xmldocument_O = xmldocument;
                boolIsValidXml = true;
            }
            catch (Exception exception)
            {
                intStatus_IO = 400;
                strUserMessage_IO = "Something went wrong.";
                strDevMessage_IO = "Error trying to validate the XML is getting in:  " + exception.Message;

                //                                      //Making a log for the exception.
                Tools.subExceptionHandler(exception, ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);
            }

            return boolIsValidXml;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private bool boolIsOrderOrEstimate(
            //                                              //PURPOSE:
            //                                              //Helper method to validate the XML is getting in is Order
            //                                              //      or Estimate. It returns the type of schema we need 
            //                                              //      to use to validate the XML.

            XmlDocument xmldocument_I,
            out String strTypeOfSchema_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            bool boolIsOrderOrEstimate = false;
            strTypeOfSchema_O = JobController.strNone;

            try
            {
                int intNodesEstimate = intGetNumberOfNodes(xmldocument_I, "/cXML/Request/RFQ");

                int intNodesOrder = intGetNumberOfNodes(xmldocument_I, "/cXML/Request/OrderRequest");

                if (
                    intNodesEstimate > 0
                    )
                {
                    boolIsOrderOrEstimate = true;
                    strTypeOfSchema_O = JobController.strEstimate;
                }
                else if (
                  intNodesOrder > 0
                  )
                {
                    boolIsOrderOrEstimate = true;
                    strTypeOfSchema_O = JobController.strOrder;
                }
            }
            catch (Exception exception)
            {
                intStatus_IO = 400;
                strUserMessage_IO = "Something went wrong.";
                strDevMessage_IO = "Something went wrong trying to figure it out the XML schema: " + exception.Message;

                //                                      //Making a log for the exception.
                Tools.subExceptionHandler(exception, ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);
            }

            return boolIsOrderOrEstimate;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private int intGetNumberOfNodes(
            XmlDocument xmldocument_I,
            String strPath_I
            )
        {
            XPathNavigator xpathnav = xmldocument_I.CreateNavigator();
            xpathnav.MoveToRoot();

            XPathExpression xpathexp = xpathnav.Compile(strPath_I);
            XPathNodeIterator xpniterator = xpathnav.Select(xpathexp);

            return xpniterator.Count;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private bool boolIsValidXmlComparedToSchema(
            //                                              //PURPOSE:
            //                                              //Helper method to validate the XML againts an especific 
            //                                              //      Schema.

            String strXml_I,
            String strTypeOfSchema_I,
            out XmlDocument xmldocument_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO

            )
        {
            bool boolIsValidXmlComparedToSchema = false;
            xmldocument_O = null;

            try
            {
                //                                          //Getting the XML schema.
                XmlSchema xmlschema = xmlschemaGetSchema(strTypeOfSchema_I);

                //                                          //Set the configuration to validate the XML.
                XmlReaderSettings xmlreadersettings = new XmlReaderSettings();
                xmlreadersettings.ValidationType = ValidationType.Schema;
                xmlreadersettings.Schemas.Add(xmlschema);
                xmlreadersettings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
                xmlreadersettings.ValidationFlags |= XmlSchemaValidationFlags.AllowXmlAttributes;

                //                                          //The XML is validated when it's invoking the 
                //                                          //      XmlReader.Create method.
                TextReader textreader = new StringReader(strXml_I);
                XmlReader xmlreader = XmlReader.Create(textreader, xmlreadersettings);

                //                                          //Once the XML has been validated the
                //                                          //      XmlDocument is created.
                XmlDocument xmldocument = new XmlDocument();
                xmldocument.PreserveWhitespace = true;
                xmldocument.Load(xmlreader);
                textreader.Close();
                xmlreader.Close();

                //                                          //Returned values.
                xmldocument_O = xmldocument;
                boolIsValidXmlComparedToSchema = true;
            }
            catch (Exception exception)
            {
                intStatus_IO = 400;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Error related with the validation with the schema: " + exception.Message;

                //                                          //Making a log for the exception.
                Tools.subExceptionHandler(exception, ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);
            }

            return boolIsValidXmlComparedToSchema;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private XmlSchema xmlschemaGetSchema(
            //                                              //PURPOSE:
            //                                              //Helper method to get the schema to validate XML input
            //                                              //      from Wiznet.

            String strSchema
            )
        {
            XmlSchemaSet xmlschset = new XmlSchemaSet();
            XmlSchema xmlschema;

            //                                          // Get schema Uri.
            String strFilePath = Path.Combine(this.hostingEnvironment.ContentRootPath,
                $"xmlSchemas\\{strSchema}.xsd");
            String strFileUri = (new Uri(strFilePath)).AbsoluteUri;

            //                                          // Register a provider to recognize 
            //                                          //      encoding="Windows-1252" to creater the schema.
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            xmlschema = xmlschset.Add(null, strFileUri);

            return xmlschema;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/