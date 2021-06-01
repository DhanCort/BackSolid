/*TASK RP.CALCULATION*/
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Odyssey2Backend.JsonTypes;
using Odyssey2Backend.PrintShop;
using System.Threading.Tasks;
using System.Text.Json;
using Odyssey2Backend.XJDF;
using Odyssey2Backend.Infrastructure;
using Odyssey2Backend.Utilities;
using System.Linq;
using Microsoft.Extensions.Configuration;
using TowaStandard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Odyssey2Backend.Job;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa (lGF - Liliana Gutierrez).
//                                                          //DATE: December 03, 2019.

namespace Odyssey2Backend.Controllers
{
    //                                                      //To obtain the strPrintshopId from token:
    //                                                      //  var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
    //                                                      //  String strPrintshopId = idClaim.Value;

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //==================================================================================================================
    [Route("Odyssey2/[controller]")]
    public class CalculationController : ControllerBase
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        private IConfiguration configuration;

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        public CalculationController(IConfiguration iConfiguration_I)
        {
            this.configuration = iConfiguration_I;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //TRANSFORMATION METHODS.

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult Add(
            //                                              //PURPOSE:
            //                                              //Add calculation

            //                                              //URL: http://localhost/Odyssey2/Calculation
            //                                              //     /Add
            //                                              //Method: POST.
            //                                              //Use a JSON CaljsonCalculationJson:

            //                                              //DESCRIPTION:
            //                                              //Add calculation based on field CalculationType of 
            //                                              //      the Table calculation.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            //                                              //      400 - BadRequest().

            //                                              //Dynamic object that contains a json with the calculation data.
            [FromBody] JsonElement calData
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
            //                                              //Valids fields required for each calculation.
            if (
                //                                          //Verify if the object is not null.
                !Object.ReferenceEquals(null, calData) &&
                //                                          //Verify if the data is not null or empty.
                calData.TryGetProperty("strCalculationType", out json) &&
                (int)calData.GetProperty("strCalculationType").ValueKind == 3 &&
                calData.TryGetProperty("strBy", out json) &&
                (int)calData.GetProperty("strBy").ValueKind == 3 &&
                calData.TryGetProperty("boolIsEnable", out json) &&
                ((int)calData.GetProperty("boolIsEnable").ValueKind == 6 ||
                (int)calData.GetProperty("boolIsEnable").ValueKind == 5) 
                )
            {
                //                                          //Get the general info of the calculation.
                bool boolIsEnable = calData.GetProperty("boolIsEnable").GetBoolean();
                String strCalculationType = calData.GetProperty("strCalculationType").GetString();
                String strByX = calData.GetProperty("strBy").GetString();

                //                                          //Set the other properties, if they come, get them from 
                //                                          //      the json.

                //                                          //Set the description, if it comes, get it from the json.
                String strDescription = null;
                if (
                    calData.TryGetProperty("strDescription", out json) &&
                    (int)calData.GetProperty("strDescription").ValueKind == 3
                    )
                {
                    strDescription = calData.GetProperty("strDescription").GetString();
                    strDescription = strDescription.TrimExcel();
                }

                //                                          //Set the product, if it comes, get it from the json.
                int? intnJobId = null;
                if (
                    calData.TryGetProperty("intnJobId", out json) &&
                    (int)calData.GetProperty("intnJobId").ValueKind == 4
                    )
                {
                    intnJobId = calData.GetProperty("intnJobId").GetInt32();
                }

                //                                          //Set the product, if it comes, get it from the json.
                int? intnPkProduct = null;
                if (
                    calData.TryGetProperty("intnPkProduct", out json) &&
                    (int)calData.GetProperty("intnPkProduct").ValueKind == 4
                    )
                {
                    intnPkProduct = calData.GetProperty("intnPkProduct").GetInt32();
                }

                //                                          //Profit.
                double? numnProfit = null;
                if (
                    calData.TryGetProperty("numnProfit", out json) &&
                    (int)calData.GetProperty("numnProfit").ValueKind == 4
                    )
                {
                    numnProfit = calData.GetProperty("numnProfit").GetDouble();
                }

                //                                          //Cost.
                double? numnCost = null;
                if (
                    calData.TryGetProperty("numnCost", out json) &&
                    (int)calData.GetProperty("numnCost").ValueKind == 4
                    )
                {
                    numnCost = calData.GetProperty("numnCost").GetDouble();
                }

                //                                          //Condition to apply.
                GpcondjsonGroupConditionJson gpcondCondition = null;
                if (
                    calData.TryGetProperty("condition", out json) &&
                    (int)calData.GetProperty("condition").ValueKind != 7
                    )
                {
                    String strCondition = calData.GetProperty("condition").GetString();

                    gpcondCondition = JsonSerializer.Deserialize<GpcondjsonGroupConditionJson>(
                        strCondition);
                }

                //                                          //Minimum quantity or untis.
                double? numnMin = null;
                if (
                    (strByX == CalCalculation.strByProduct) ||
                    (strByX == CalCalculation.strByIntent)
                    )
                {
                    numnMin = 1;
                }
                if (
                    calData.TryGetProperty("numnMin", out json) &&
                    (int)calData.GetProperty("numnMin").ValueKind == 4
                    )
                {
                    numnMin = calData.GetProperty("numnMin").GetDouble();
                }

                //                                          //Block default.
                double? numnBlock = null;
                if (
                    (strByX == CalCalculation.strByProduct) ||
                    (strByX == CalCalculation.strByIntent)
                    )
                {
                    numnBlock = 1;
                }

                //                                          //Is block.
                if (
                   calData.TryGetProperty("numnBlock", out json) &&
                   (int)calData.GetProperty("numnBlock").ValueKind == 4
                   )
                {
                    numnBlock = calData.GetProperty("numnBlock").GetDouble();
                }

                //                                          //Quantity.              
                double? numnQuantity = null;
                if (
                   calData.TryGetProperty("numnQuantity", out json) &&
                   (int)calData.GetProperty("numnQuantity").ValueKind == 4
                   )
                {
                    numnQuantity = calData.GetProperty("numnQuantity").GetDouble();
                }

                //                                          //Unit of measurement.
                String strUnit = null;
                if (
                    calData.TryGetProperty("strUnit", out json) &&
                    (int)calData.GetProperty("strUnit").ValueKind == 3
                    )
                {
                    strUnit = calData.GetProperty("strUnit").GetString();
                }

                //                                          //Needed to produce.
                double? numnNeeded = null;
                if (
                    calData.TryGetProperty("numnNeeded", out json) &&
                    (int)calData.GetProperty("numnNeeded").ValueKind == 4
                    )
                {
                    numnNeeded = calData.GetProperty("numnNeeded").GetDouble();
                }

                //                                          //Units to produce with the needed.
                double? numnPerUnits = null;
                if (
                    calData.TryGetProperty("numnPerUnits", out json) &&
                    (int)calData.GetProperty("numnPerUnits").ValueKind == 4
                    )
                {
                    numnPerUnits = calData.GetProperty("numnPerUnits").GetDouble();
                }

                //                                          //Ascendant elements (only for byIntent).
                String strAscendantElements = null;
                if (
                    calData.TryGetProperty("strAscendantElements", out json) &&
                    (int)calData.GetProperty("strAscendantElements").ValueKind == 3
                    )
                {
                    strAscendantElements = calData.GetProperty("strAscendantElements").GetString();
                }

                //                                          //Value for attribute saved in the ascendant elements.
                String strValue = null;
                if (
                    calData.TryGetProperty("strValue", out json) &&
                    (int)calData.GetProperty("strValue").ValueKind == 3
                    )
                {
                    strValue = calData.GetProperty("strValue").GetString();
                }

                //                                          //Process associated.
                int? intnPkProcess = null;
                if (
                    calData.TryGetProperty("intnPkProcess", out json) &&
                    (int)calData.GetProperty("intnPkProcess").ValueKind == 4
                    )
                {
                    intnPkProcess = calData.GetProperty("intnPkProcess").GetInt32();
                }

                //                                          //PIW associated.
                int? intnPkProcessInWorkflow = null;
                if (
                    calData.TryGetProperty("intnPkProcessInWorkflow", out json) &&
                    (int)calData.GetProperty("intnPkProcessInWorkflow").ValueKind == 4
                    )
                {
                    intnPkProcessInWorkflow = calData.GetProperty("intnPkProcessInWorkflow").GetInt32();
                }

                //                                          //EleetOrEleele associated.
                int? intnPkEleetOrEleeleI = null;
                if (
                    calData.TryGetProperty("intnPkEleetOrEleeleI", out json) &&
                    (int)calData.GetProperty("intnPkEleetOrEleeleI").ValueKind == 4
                    )
                {
                    intnPkEleetOrEleeleI = calData.GetProperty("intnPkEleetOrEleeleI").GetInt32();
                }

                //                                          //Is an Eleet or Eleele associated.
                bool? boolnIsEleetI = null;
                if (
                    calData.TryGetProperty("boolnIsEleetI", out json) &&
                    ((int)calData.GetProperty("boolnIsEleetI").ValueKind == 5 ||
                    (int)calData.GetProperty("boolnIsEleetI").ValueKind == 6)
                    )
                {
                    boolnIsEleetI = calData.GetProperty("boolnIsEleetI").GetBoolean();
                }

                //                                          //Resource element associated.
                int? intnPkResourceI = null;
                if (
                    calData.TryGetProperty("intnPkResourceI", out json) &&
                    (int)calData.GetProperty("intnPkResourceI").ValueKind == 4
                    )
                {
                    intnPkResourceI = calData.GetProperty("intnPkResourceI").GetInt32();
                }

                //                                          //Quantity waste.
                double? numnQuantityWaste = null;
                if (
                    calData.TryGetProperty("numnQuantityWaste", out json) &&
                    (int)calData.GetProperty("numnQuantityWaste").ValueKind == 4
                    )
                {
                    numnQuantityWaste = calData.GetProperty("numnQuantityWaste").GetDouble();
                }

                //                                          //Percent waste.
                double? numnPercentWaste = null;
                if (
                    calData.TryGetProperty("numnPercentWaste", out json) &&
                    (int)calData.GetProperty("numnPercentWaste").ValueKind == 4
                    )
                {
                    numnPercentWaste = calData.GetProperty("numnPercentWaste").GetDouble();
                }

                //                                          //Quantity from PKQeleet.
                int? intnPkEleetOrEleeleO = null;
                if (
                    calData.TryGetProperty("intnPkEleetOrEleeleO", out json) &&
                    (int)calData.GetProperty("intnPkEleetOrEleeleO").ValueKind == 4
                    )
                {
                    intnPkEleetOrEleeleO = calData.GetProperty("intnPkEleetOrEleeleO").GetInt32();
                }

                //                                          //Is an Eleet or Eleele associated.
                bool? boolnIsEleetO = null;
                if (
                    calData.TryGetProperty("boolnIsEleetO", out json) &&
                    ((int)calData.GetProperty("boolnIsEleetO").ValueKind == 5 ||
                    (int)calData.GetProperty("boolnIsEleetO").ValueKind == 6)
                    )
                {
                    boolnIsEleetO = calData.GetProperty("boolnIsEleetO").GetBoolean();
                }

                //                                          //Resource element associated.
                int? intnPkResourceO = null;
                if (
                    calData.TryGetProperty("intnPkResourceO", out json) &&
                    (int)calData.GetProperty("intnPkResourceO").ValueKind == 4
                    )
                {
                    intnPkResourceO = calData.GetProperty("intnPkResourceO").GetInt32();
                }

                //                                          //Account
                int? intnPkAccount = null;
                if (
                    calData.TryGetProperty("intnPkAccount", out json) &&
                    (int)calData.GetProperty("intnPkAccount").ValueKind == 4
                    )
                {
                    intnPkAccount = calData.GetProperty("intnPkAccount").GetInt32();
                }

                //                                          //Is an Eleet or Eleele associated.
                bool? boolnFromThickness = null;
                if (
                    calData.TryGetProperty("boolnFromThickness", out json) &&
                    ((int)calData.GetProperty("boolnFromThickness").ValueKind == 5 ||
                    (int)calData.GetProperty("boolnFromThickness").ValueKind == 6)
                    )
                {
                    boolnFromThickness = calData.GetProperty("boolnFromThickness").GetBoolean();
                }

                //                                          //Is ByBlock.
                bool? boolnIsBlock = null;
                if (
                    calData.TryGetProperty("boolnIsBlock", out json) &&
                    ((int)calData.GetProperty("boolnIsBlock").ValueKind == 5 ||
                    (int)calData.GetProperty("boolnIsBlock").ValueKind == 6)
                    )
                {
                    boolnIsBlock = calData.GetProperty("boolnIsBlock").GetBoolean();
                }

                //                                          //Is ByArea.
                bool? boolnByArea = null;
                if (
                    calData.TryGetProperty("boolnByArea", out json) &&
                    ((int)calData.GetProperty("boolnByArea").ValueKind == 5 ||
                    (int)calData.GetProperty("boolnByArea").ValueKind == 6)
                    )
                {
                    boolnByArea = calData.GetProperty("boolnByArea").GetBoolean();
                }

                try
                {
                    CalCalculation.subAddCalculation(ps, intnJobId, strUnit,
                        numnQuantity, numnCost, numnBlock, boolIsEnable, strValue, strAscendantElements,
                        strDescription, numnProfit, strCalculationType, gpcondCondition,
                        intnPkProduct, intnPkProcess, intnPkResourceI, intnPkProcessInWorkflow, intnPkEleetOrEleeleI, 
                        boolnIsEleetI, numnNeeded, numnPerUnits, strByX, numnMin, numnQuantityWaste, numnPercentWaste, 
                        intnPkEleetOrEleeleO, boolnIsEleetO, intnPkResourceO, configuration, intnPkAccount,
                        boolnFromThickness, boolnIsBlock, boolnByArea, ref intStatus, ref strUserMessage, 
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
        [HttpPost("[action]")]
        public IActionResult Modify(
            //                                              //PURPOSE:
            //                                              //Modify a calculation.

            //                                              //URL: http://localhost/Odyssey2/Calculation
            //                                              //      /Modify
            //                                              //Method: POST.

            //                                              //DESCRIPTION:
            //                                              //Delete a calculation from db.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            //                                              //Receive a json with all necessary data.
            [FromBody] JsonElement calData
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
                //                                          //Verify if pk is not null and greater that 0.
                !Object.ReferenceEquals(null, calData) &&
                //                                          //Verify the necessary properties.
                calData.TryGetProperty("intPk", out json) &&
                (int)calData.GetProperty("intPk").ValueKind == 4 &&
                calData.TryGetProperty("strCalculationType", out json) &&
                (int)calData.GetProperty("strCalculationType").ValueKind == 3 &&
                calData.TryGetProperty("strBy", out json) &&
                (int)calData.GetProperty("strBy").ValueKind == 3 &&
                calData.TryGetProperty("boolIsEnable", out json) &&
                ((int)calData.GetProperty("boolIsEnable").ValueKind == 5 ||
                (int)calData.GetProperty("boolIsEnable").ValueKind == 6)
                )
            {
                //                                          //Get the necessary properties.
                int intPk = calData.GetProperty("intPk").GetInt32();

                String strCalculationType = calData.GetProperty("strCalculationType").GetString();
                bool boolIsEnable = calData.GetProperty("boolIsEnable").GetBoolean();
                String strByX = calData.GetProperty("strBy").GetString();

                //                                          //Get the properties that can be used.
                String strDescription = null;
                if (
                    calData.TryGetProperty("strDescription", out json) &&
                    (int)calData.GetProperty("strDescription").ValueKind == 3
                    )
                {
                    strDescription = calData.GetProperty("strDescription").GetString();
                    strDescription = strDescription.TrimExcel();
                }

                //                                          //Minimum quantity or untis.
                double? numnMin = null;
                if (
                    (strByX == CalCalculation.strByProduct) ||
                    (strByX == CalCalculation.strByIntent)
                    )
                {
                    numnMin = 1;
                }
                if (
                    calData.TryGetProperty("numnMin", out json) &&
                    (int)calData.GetProperty("numnMin").ValueKind == 4
                    )
                {
                    numnMin = calData.GetProperty("numnMin").GetDouble();
                }


                double? numnPerUnits = null;
                if (
                    calData.TryGetProperty("numnPerUnits", out json) &&
                    (int)calData.GetProperty("numnPerUnits").ValueKind == 4
                    )
                {
                    numnPerUnits = calData.GetProperty("numnPerUnits").GetDouble();
                }

                double? numnNeeded = null;
                if (
                    calData.TryGetProperty("numnNeeded", out json) &&
                    (int)calData.GetProperty("numnNeeded").ValueKind == 4
                    )
                {
                    numnNeeded = calData.GetProperty("numnNeeded").GetDouble();
                }

                double? numnProfit = null;
                if (
                    calData.TryGetProperty("numnProfit", out json) &&
                    (int)calData.GetProperty("numnProfit").ValueKind == 4
                    )
                {
                    numnProfit = calData.GetProperty("numnProfit").GetDouble();
                }

                double? numnCost = null;
                if (
                    calData.TryGetProperty("numnCost", out json) &&
                    (int)calData.GetProperty("numnCost").ValueKind == 4
                    )
                {
                    numnCost = calData.GetProperty("numnCost").GetDouble();
                }

                double? numnQuantity = null;
                if (
                    calData.TryGetProperty("numnQuantity", out json) &&
                    (int)calData.GetProperty("numnQuantity").ValueKind == 4
                    )
                {
                    numnQuantity = calData.GetProperty("numnQuantity").GetDouble();
                }

                String strUnit = null;
                if (
                    calData.TryGetProperty("strUnit", out json) &&
                    (int)calData.GetProperty("strUnit").ValueKind == 3
                    )
                {
                    strUnit = calData.GetProperty("strUnit").GetString();
                }

                //                                          //Block default.
                double? numnBlock = null;
                if (
                    (strByX == CalCalculation.strByProduct) ||
                    (strByX == CalCalculation.strByIntent)
                    )
                {
                    numnBlock = 1;
                }
                //                                          //Is block
                if (
                   calData.TryGetProperty("numnBlock", out json) &&
                   (int)calData.GetProperty("numnBlock").ValueKind == 4
                   )
                {
                    numnBlock = calData.GetProperty("numnBlock").GetDouble();
                }

                //                                          //Condition to apply.
                GpcondjsonGroupConditionJson gpcondCondition = null;
                if (
                    calData.TryGetProperty("condition", out json) &&
                    (int)calData.GetProperty("condition").ValueKind != 7
                    )
                {
                    String strCondition = calData.GetProperty("condition").GetString();

                    gpcondCondition = JsonSerializer.Deserialize<GpcondjsonGroupConditionJson>(
                        strCondition);
                }

                double? numnQuantityWaste = null;
                if (
                    calData.TryGetProperty("numnQuantityWaste", out json) &&
                    (int)calData.GetProperty("numnQuantityWaste").ValueKind == 4
                    )
                {
                    numnQuantityWaste = calData.GetProperty("numnQuantityWaste").GetDouble();
                }

                double? numnPercentWaste = null;
                if (
                    calData.TryGetProperty("numnPercentWaste", out json) &&
                    (int)calData.GetProperty("numnPercentWaste").ValueKind == 4
                    )
                {
                    numnPercentWaste = calData.GetProperty("numnPercentWaste").GetDouble();
                }

                //                                          //Quantity from PKeleet.
                int? intnPkEleetOrEleeleO = null;
                if (
                    calData.TryGetProperty("intnPkEleetOrEleeleO", out json) &&
                    (int)calData.GetProperty("intnPkEleetOrEleeleO").ValueKind == 4
                    )
                {
                    intnPkEleetOrEleeleO = calData.GetProperty("intnPkEleetOrEleeleO").GetInt32();
                }

                int? intnPkElementElementTypeO = null;
                int? intnPkElementElementO = null;
                //                                          //Quantity from is an Eleet or Eleele associated.
                bool? boolnIsEleetO = null;
                if (
                    calData.TryGetProperty("boolnIsEleetO", out json) &&
                    ((int)calData.GetProperty("boolnIsEleetO").ValueKind == 5 ||
                    (int)calData.GetProperty("boolnIsEleetO").ValueKind == 6)
                    )
                {
                    boolnIsEleetO = calData.GetProperty("boolnIsEleetO").GetBoolean();

                    if (
                        boolnIsEleetO == true
                        )
                    {
                        intnPkElementElementTypeO = intnPkEleetOrEleeleO;
                    }
                    else
                    {
                        intnPkElementElementO = intnPkEleetOrEleeleO;
                    }
                }

                //                                          //Quantity from resource element associated.
                int? intnPkResourceO = null;
                if (
                    calData.TryGetProperty("intnPkResourceO", out json) &&
                    (int)calData.GetProperty("intnPkResourceO").ValueKind == 4
                    )
                {
                    intnPkResourceO = calData.GetProperty("intnPkResourceO").GetInt32();
                }

                //                                          //Account
                int? intnPkAccount = null;
                if (
                    calData.TryGetProperty("intnPkAccount", out json) &&
                    (int)calData.GetProperty("intnPkAccount").ValueKind == 4
                    )
                {
                    intnPkAccount = calData.GetProperty("intnPkAccount").GetInt32();
                }

                //                                          //Is an Eleet or Eleele associated.
                bool? boolnFromThickness = null;
                if (
                    calData.TryGetProperty("boolnFromThickness", out json) &&
                    ((int)calData.GetProperty("boolnFromThickness").ValueKind == 5 ||
                    (int)calData.GetProperty("boolnFromThickness").ValueKind == 6)
                    )
                {
                    boolnFromThickness = calData.GetProperty("boolnFromThickness").GetBoolean();
                }

                //                                          //Is ByBlock.
                bool? boolnIsBlock = null;
                if (
                    calData.TryGetProperty("boolnIsBlock", out json) &&
                    ((int)calData.GetProperty("boolnIsBlock").ValueKind == 5 ||
                    (int)calData.GetProperty("boolnIsBlock").ValueKind == 6)
                    )
                {
                    boolnIsBlock = calData.GetProperty("boolnIsBlock").GetBoolean();
                }

                //                                          //Is ByArea.
                bool? boolnByArea = null;
                if (
                    calData.TryGetProperty("boolnByArea", out json) &&
                    ((int)calData.GetProperty("boolnByArea").ValueKind == 5 ||
                    (int)calData.GetProperty("boolnByArea").ValueKind == 6)
                    )
                {
                    boolnByArea = calData.GetProperty("boolnByArea").GetBoolean();
                }

                try
                {
                    //                                      //Create a cal with the new values (the values that can not 
                    //                                      //      be changed are set in empty string or null because 
                    //                                      //      they won't be used).
                    CalCalculation calNew = new CalCalculation(intPk, strUnit, numnQuantity, numnCost, null, null, null, numnBlock, boolIsEnable, null, null, strDescription,
                        numnProfit, null, null, null, strCalculationType, strByX, 
                        null, null, null, null, numnNeeded, numnPerUnits, numnMin, numnQuantityWaste, numnPercentWaste,     
                        null, null, null, null, intnPkElementElementTypeO, intnPkElementElementO, intnPkResourceO, 
                        intnPkAccount, boolnFromThickness, boolnIsBlock, boolnByArea);

                    CalCalculation.subModify(ps, calNew, gpcondCondition, ref intStatus, ref strUserMessage, ref strDevMessage);
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
        public IActionResult Delete(
            //                                              //PURPOSE:
            //                                              //Delete a calculation.

            //                                              //URL: http://localhost/Odyssey2/Calculation
            //                                              //      /Delete
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intPk":00
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Delete a calculation from db.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            //                                              //      400 - BadRequest().

            //                                              //Receive the pk of the element.
            [FromBody] JsonElement calData
            )
        {
            int intStatus = 400;
            String strUserMessage = "Invalid data.";
            String strDevMessage = "";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if pk is not null and greater that 0.
                !Object.ReferenceEquals(null, calData) &&
                calData.TryGetProperty("intPk", out json) &&
                (int)calData.GetProperty("intPk").ValueKind == 4
                )
            {
                int intPk = calData.GetProperty("intPk").GetInt32();

                try
                {
                    CalCalculation.subDelete(intPk, ref intStatus, ref strUserMessage, ref strDevMessage);
                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }
            }
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                    obj);
            IActionResult aresult = Ok(respjson1);

            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult Group(
            //                                              //PURPOSE:
            //                                              //Create a group of calculations.

            //                                              //URL: http://localhost/Odyssey2/Calculation/Group
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "arrintPk":
            //                                              //              [
            //                                              //                  1,
            //                                              //                  2,
            //                                              //                  3
            //                                              //              ]
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Create a group of calculations with the pks.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            //                                              //      400 - BadRequest().

            //                                              //Json with all data required.
            [FromBody] JsonElement jsonCal
            )
        {
            int intStatus = 400;
            String strUserMessage = "Invalid data.";
            String strDevMessage = "";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify data.
                !Object.ReferenceEquals(null, jsonCal) &&
                jsonCal.TryGetProperty("arrintPk", out json) &&
                (int)jsonCal.GetProperty("arrintPk").ValueKind == 2
                )
            {
                //                                          //
                int[] arrintPk = new int[jsonCal.GetProperty("arrintPk").GetArrayLength()];
                for (int intI = 0; intI < arrintPk.Length; intI = intI + 1)
                {
                    arrintPk[intI] = jsonCal.GetProperty("arrintPk")[intI].GetInt32();
                }

                try
                {
                    CalCalculation.subGroup(arrintPk, ref intStatus, ref strUserMessage, ref strDevMessage);
                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }
            }

            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                    obj);
            IActionResult aresult = Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult Ungroup(
            //                                              //PURPOSE:
            //                                              //Ungroup calculations.

            //                                              //URL: http://localhost/Odyssey2/Calculation
            //                                              //      /Ungroup?intGroupId=1
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Ungroup.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            //                                              //      400 - BadRequest().

            //                                              //Receive the group.
            [FromBody] JsonElement calData
            )
        {
            IActionResult aresult = BadRequest("Invalid data.");
            JsonElement json;
            if (
                //                                          //Verify if pk is not null and greater that 0.
                !Object.ReferenceEquals(null, calData) &&
                calData.TryGetProperty("intGroupId", out json) &&
                (int)calData.GetProperty("intGroupId").ValueKind == 4
                )
            {
                int intGroupId = calData.GetProperty("intGroupId").GetInt32();

                if (
                    intGroupId > 0
                    )
                {
                    try
                    {
                        int intStatus;
                        CalCalculation.subUngroup(intGroupId, out intStatus);

                        if (
                            intStatus == 0
                            )
                        {
                            aresult = Ok();
                        }
                    }
                    catch (Exception ex)
                    {
                        //                                  //TO FIX. Hacer que este servicio retorne un jsonresponse.
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
        [HttpPost("[action]")]
        public IActionResult AddProcessTime(
            //                                              //PURPOSE:
            //                                              //Add a time calculation to a process.

            //                                              //URL: http://localhost/Odyssey2/Calculation
            //                                              //      /AddProcessTime
            //                                              //Method: POST.
            //                                              //Use a JSON like this:

            //                                              //DESCRIPTION:
            //                                              //Add a time calculation to the calculation table in DB.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            //                                              //      400 - BadRequest().

            //                                              //Receive Json with all the needed information.
            [FromBody] JsonElement calTimeData
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            //                                              //Valids fields required for each calculation.
            if (
                //                                          //Verify if the object is not null.
                !Object.ReferenceEquals(null, calTimeData) &&
                //                                          //Verify if the data is not null or empty.
                calTimeData.TryGetProperty("strCalculationType", out json) &&
                (int)calTimeData.GetProperty("strCalculationType").ValueKind == 3 &&
                calTimeData.TryGetProperty("strBy", out json) &&
                (int)calTimeData.GetProperty("strBy").ValueKind == 3 &&
                calTimeData.TryGetProperty("boolIsEnable", out json) &&
                ((int)calTimeData.GetProperty("boolIsEnable").ValueKind == 6 ||
                (int)calTimeData.GetProperty("boolIsEnable").ValueKind == 5)
                )
            {
                //                                          //Get the general info of the calculation.
                String strCalculationType = calTimeData.GetProperty("strCalculationType").GetString();
                String strByX = calTimeData.GetProperty("strBy").GetString();
                bool boolIsEnable = calTimeData.GetProperty("boolIsEnable").GetBoolean();

                //                                          //Set the other properties, if they come, get them from 
                //                                          //      the json.

                //                                          //Set the description, if it comes, get it from the json.
                String strDescription = null;
                if (
                    calTimeData.TryGetProperty("strDescription", out json) &&
                    (int)calTimeData.GetProperty("strDescription").ValueKind == 3
                    )
                {
                    strDescription = calTimeData.GetProperty("strDescription").GetString();
                    strDescription = strDescription.TrimExcel();
                }

                int? intPkProduct = null;
                if (
                    calTimeData.TryGetProperty("intPkProduct", out json) &&
                    (int)calTimeData.GetProperty("intPkProduct").ValueKind == 4
                    )
                {
                    intPkProduct = calTimeData.GetProperty("intPkProduct").GetInt32();
                }

                //                                          Hours.         
                int? intnHours = null;
                if (
                   calTimeData.TryGetProperty("intnHours", out json) &&
                   (int)calTimeData.GetProperty("intnHours").ValueKind == 4
                   )
                {
                    intnHours = calTimeData.GetProperty("intnHours").GetInt32();
                }

                //                                          //Minutes.
                int? intnMinutes = null;
                if (
                   calTimeData.TryGetProperty("intnMinutes", out json) &&
                   (int)calTimeData.GetProperty("intnMinutes").ValueKind == 4
                   )
                {
                    intnMinutes = calTimeData.GetProperty("intnMinutes").GetInt32();
                }

                //                                          //Seconds.
                int? intnSeconds = null;
                if (
                   calTimeData.TryGetProperty("intnSeconds", out json) &&
                   (int)calTimeData.GetProperty("intnSeconds").ValueKind == 4
                   )
                {
                    intnSeconds = calTimeData.GetProperty("intnSeconds").GetInt32();
                }

                //                                          //Quantity.              
                double? numnQuantity = null;
                if (
                   calTimeData.TryGetProperty("numnQuantity", out json) &&
                   (int)calTimeData.GetProperty("numnQuantity").ValueKind == 4
                   )
                {
                    numnQuantity = calTimeData.GetProperty("numnQuantity").GetDouble();
                }

                //                                          //Unit of measurement.
                String strUnit = null;
                if (
                    calTimeData.TryGetProperty("strUnit", out json) &&
                    (int)calTimeData.GetProperty("strUnit").ValueKind == 3
                    )
                {
                    strUnit = calTimeData.GetProperty("strUnit").GetString();
                }

                //                                          //Needed to produce.
                double? numnNeeded = null;
                if (
                    calTimeData.TryGetProperty("numnNeeded", out json) &&
                    (int)calTimeData.GetProperty("numnNeeded").ValueKind == 4
                    )
                {
                    numnNeeded = calTimeData.GetProperty("numnNeeded").GetDouble();
                }

                //                                          //Units to produce with the needed.
                double? numnPerUnits = null;
                if (
                    calTimeData.TryGetProperty("numnPerUnits", out json) &&
                    (int)calTimeData.GetProperty("numnPerUnits").ValueKind == 4
                    )
                {
                    numnPerUnits = calTimeData.GetProperty("numnPerUnits").GetDouble();
                }

                //                                          //Process associated.
                int? intnPkProcess = null;
                if (
                    calTimeData.TryGetProperty("intnPkProcess", out json) &&
                    (int)calTimeData.GetProperty("intnPkProcess").ValueKind == 4
                    )
                {
                    intnPkProcess = calTimeData.GetProperty("intnPkProcess").GetInt32();
                }

                //                                          //PIW associated.
                int? intnPkProcessInWorkflow = null;
                if (
                    calTimeData.TryGetProperty("intnPkProcessInWorkflow", out json) &&
                    (int)calTimeData.GetProperty("intnPkProcessInWorkflow").ValueKind == 4
                    )
                {
                    intnPkProcessInWorkflow = calTimeData.GetProperty("intnPkProcessInWorkflow").GetInt32();
                }

                //                                          //Get conditions to apply.
                GpcondjsonGroupConditionJson gpcondition = null;
                if (
                    calTimeData.TryGetProperty("condition", out json) &&
                    (int)calTimeData.GetProperty("condition").ValueKind == 3
                    )
                {
                    gpcondition = JsonSerializer.Deserialize<GpcondjsonGroupConditionJson>(
                        calTimeData.GetProperty("condition").ToString());
                }

                //                                      //using is to release connection at the end
                using (Odyssey2Context context = new Odyssey2Context())
                {
                    //                                  //Starts a new transaction.
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            CalCalculation.subAddProcessTime(intPkProduct, intnHours, intnMinutes, intnSeconds,
                                strUnit, numnQuantity, boolIsEnable, strDescription, strCalculationType, intnPkProcess,
                                intnPkProcessInWorkflow, numnNeeded, numnPerUnits, strByX, gpcondition, context,
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

                            //                          //Making a log for the exception.
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
        public IActionResult ModifyProcessTime(
            //                                              //PURPOSE:
            //                                              //Modify time calculation to a process.

            //                                              //URL: http://localhost/Odyssey2/Calculation
            //                                              //      /AddProcessTime
            //                                              //Method: POST.
            //                                              //Use a JSON like this:

            //                                              //DESCRIPTION:
            //                                              //Modify time calculation to the calculation table in DB.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            //                                              //      400 - BadRequest().

            //                                              //Receive Json with all the needed information.
            [FromBody] JsonElement calTimeData
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            //                                              //Valids fields required for each calculation.
            if (
                //                                          //Verify if the object is not null.
                !Object.ReferenceEquals(null, calTimeData) &&
                //                                          //Verify if the data is not null or empty.
                calTimeData.TryGetProperty("intPk", out json) &&
                (int)calTimeData.GetProperty("intPk").ValueKind == 4 &&
                calTimeData.TryGetProperty("strDescription", out json) &&
                (int)calTimeData.GetProperty("strDescription").ValueKind == 3 &&
                calTimeData.TryGetProperty("strCalculationType", out json) &&
                (int)calTimeData.GetProperty("strCalculationType").ValueKind == 3 &&
                calTimeData.TryGetProperty("strBy", out json) &&
                (int)calTimeData.GetProperty("strBy").ValueKind == 3 &&
                calTimeData.TryGetProperty("boolIsEnable", out json) &&
                ((int)calTimeData.GetProperty("boolIsEnable").ValueKind == 6 ||
                (int)calTimeData.GetProperty("boolIsEnable").ValueKind == 5) 
                )
            {
                //                                          //Get the general info of the calculation.
                int intPk = calTimeData.GetProperty("intPk").GetInt32();
                String strDescription = calTimeData.GetProperty("strDescription").GetString();
                String strCalculationType = calTimeData.GetProperty("strCalculationType").GetString();
                String strByX = calTimeData.GetProperty("strBy").GetString();
                bool boolIsEnable = calTimeData.GetProperty("boolIsEnable").GetBoolean();

                //                                          //Set the other properties, if they come, get them from 
                //                                          //      the json.

                //                                          Hours.         
                int? intnHours = null;
                if (
                   calTimeData.TryGetProperty("intnHours", out json) &&
                   (int)calTimeData.GetProperty("intnHours").ValueKind == 4
                   )
                {
                    intnHours = calTimeData.GetProperty("intnHours").GetInt32();
                }

                //                                          //Minutes.
                int? intnMinutes = null;
                if (
                   calTimeData.TryGetProperty("intnMinutes", out json) &&
                   (int)calTimeData.GetProperty("intnMinutes").ValueKind == 4
                   )
                {
                    intnMinutes = calTimeData.GetProperty("intnMinutes").GetInt32();
                }

                //                                          //Seconds.
                int? intnSeconds = null;
                if (
                   calTimeData.TryGetProperty("intnSeconds", out json) &&
                   (int)calTimeData.GetProperty("intnSeconds").ValueKind == 4
                   )
                {
                    intnSeconds = calTimeData.GetProperty("intnSeconds").GetInt32();
                }

                //                                          //Quantity.              
                double? numnQuantity = null;
                if (
                   calTimeData.TryGetProperty("numnQuantity", out json) &&
                   (int)calTimeData.GetProperty("numnQuantity").ValueKind == 4
                   )
                {
                    numnQuantity = calTimeData.GetProperty("numnQuantity").GetDouble();
                }

                //                                          //Unit of measurement.
                String strUnit = null;
                if (
                    calTimeData.TryGetProperty("strUnit", out json) &&
                    (int)calTimeData.GetProperty("strUnit").ValueKind == 3
                    )
                {
                    strUnit = calTimeData.GetProperty("strUnit").GetString();
                }

                //                                          //Needed to produce.
                double? numnNeeded = null;
                if (
                    calTimeData.TryGetProperty("numnNeeded", out json) &&
                    (int)calTimeData.GetProperty("numnNeeded").ValueKind == 4
                    )
                {
                    numnNeeded = calTimeData.GetProperty("numnNeeded").GetDouble();
                }

                //                                          //Units to produce with the needed.
                double? numnPerUnits = null;
                if (
                    calTimeData.TryGetProperty("numnPerUnits", out json) &&
                    (int)calTimeData.GetProperty("numnPerUnits").ValueKind == 4
                    )
                {
                    numnPerUnits = calTimeData.GetProperty("numnPerUnits").GetDouble();
                }

                //                                          //Condition to apply.
                GpcondjsonGroupConditionJson gpcondCondition = null;
                if (
                    calTimeData.TryGetProperty("condition", out json) &&
                    (int)calTimeData.GetProperty("condition").ValueKind != 7
                    )
                {
                    String strCondition = calTimeData.GetProperty("condition").GetString();

                    gpcondCondition = JsonSerializer.Deserialize<GpcondjsonGroupConditionJson>(
                        strCondition);
                }

                try
                {
                    CalCalculation calNew = new CalCalculation(intPk, strUnit, 
                        numnQuantity, null, intnHours, intnMinutes, intnSeconds, null, boolIsEnable, null, null, 
                        strDescription, null, null, null, null, 
                        strCalculationType, strByX, null, null, null, null, numnNeeded, numnPerUnits, null, null, null,
                        null, null, null, null, null, null, null, null, null, null, null);

                    CalCalculation.subModifyProcessTime(calNew, gpcondCondition, ref intStatus, ref strUserMessage, ref strDevMessage);
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
        public IActionResult SetTransform(
            //                                              //PURPOSE:
            //                                              //Set a transformation calculation in the DB.

            //                                              //URL: http://localhost/Odyssey2/Calculation/SetTransform
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //{
            //                                              //  "intnPk":8,
            //                                              //  "intPkProcessInWorkflow":5,
            //                                              //  "intPkEleetOrEleeleI":7,
            //                                              //  "boolIsEleetI":true,
            //                                              //  "intPkEleetOrEleeleO":8,
            //                                              //  "boolIsEleetO":true,
            //                                              //  "numNeeded":1.0,
            //                                              //  "numPerUnit":4.0,
            //                                              //  "intPkResourceI": 15,
            //                                              //  "intPkResourceO":16
            //                                              //  "cndition":
            //                                              //      {
            //                                              //          "strOperator":"AND",
            //                                              //          "arrcond":[
            //                                              //              {
            //                                              //                  "intnPkAttribute":1,
            //                                              //                  "strCondition": "=",
            //                                              //                  "strValue":"Opcion1"
            //                                              //              }
            //                                              //          ],
            //                                              //          "arrgpcond":[
            //                                              //              {
            //                                              //                  "strOperator":"OR",
            //                                              //                  "arrcond":[
            //                                              //                      {
            //                                              //                          "intnPkAttribute":3,
            //                                              //                          "strCondition": "=",
            //                                              //                          "strValue":"A"
            //                                              //                      },
            //                                              //                      {
            //                                              //                          "intnPkAttribute":3,
            //                                              //                          "strCondition": "=",
            //                                              //                          "strValue":"B"
            //                                              //                      }
            //                                              //                  ]
            //                                              //              }
            //                                              //          ]
            //                                              //      }
            //                                              //  }

            //                                              //DESCRIPTION:
            //                                              //Add or update a transformation calculation in the DB.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            //                                              //      400 - BadRequest().

            //                                              //Receive Json with all the needed information.
            [FromBody] JsonElement jsonTrfCal
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !Object.ReferenceEquals(null, jsonTrfCal) &&
                //                                          //Verify if the data is not null or empty.
                jsonTrfCal.TryGetProperty("intPkProcessInWorkflow", out json) &&
                (int)jsonTrfCal.GetProperty("intPkProcessInWorkflow").ValueKind == 4 &&
                jsonTrfCal.TryGetProperty("intPkEleetOrEleeleI", out json) &&
                (int)jsonTrfCal.GetProperty("intPkEleetOrEleeleI").ValueKind == 4 &&
                jsonTrfCal.TryGetProperty("boolIsEleetI", out json) &&
                ((int)jsonTrfCal.GetProperty("boolIsEleetI").ValueKind == 6 ||
                (int)jsonTrfCal.GetProperty("boolIsEleetI").ValueKind == 5) &&
                jsonTrfCal.TryGetProperty("intPkEleetOrEleeleO", out json) &&
                (int)jsonTrfCal.GetProperty("intPkEleetOrEleeleO").ValueKind == 4 &&
                jsonTrfCal.TryGetProperty("boolIsEleetO", out json) &&
                ((int)jsonTrfCal.GetProperty("boolIsEleetO").ValueKind == 6 ||
                (int)jsonTrfCal.GetProperty("boolIsEleetO").ValueKind == 5) &&
                jsonTrfCal.TryGetProperty("numNeeded", out json) &&
                (int)jsonTrfCal.GetProperty("numNeeded").ValueKind == 4 &&
                jsonTrfCal.TryGetProperty("numPerUnit", out json) &&
                (int)jsonTrfCal.GetProperty("numPerUnit").ValueKind == 4 &&
                jsonTrfCal.TryGetProperty("intPkResourceI", out json) &&
                (int)jsonTrfCal.GetProperty("intPkResourceI").ValueKind == 4 &&
                jsonTrfCal.TryGetProperty("intPkResourceO", out json) &&
                (int)jsonTrfCal.GetProperty("intPkResourceO").ValueKind == 4
                )
            {
                //                                          //Get the info of the calculation.
                int intPkProcessInWorkflow = jsonTrfCal.GetProperty("intPkProcessInWorkflow").GetInt32();
                int intPkEleetOrEleeleI = jsonTrfCal.GetProperty("intPkEleetOrEleeleI").GetInt32();
                bool boolIsEleetI = jsonTrfCal.GetProperty("boolIsEleetI").GetBoolean();
                int intPkEleetOrEleeleO = jsonTrfCal.GetProperty("intPkEleetOrEleeleO").GetInt32();
                bool boolIsEleetO = jsonTrfCal.GetProperty("boolIsEleetO").GetBoolean();
                double numNeeded = jsonTrfCal.GetProperty("numNeeded").GetDouble();
                double numPerUnit = jsonTrfCal.GetProperty("numPerUnit").GetDouble();
                int intPkResourceI = jsonTrfCal.GetProperty("intPkResourceI").GetInt32();
                int intPkResourceO = jsonTrfCal.GetProperty("intPkResourceO").GetInt32();

                //                                          //Set intnPk, if it come, get it from the json.        
                int? intnPk = null;
                if (
                   jsonTrfCal.TryGetProperty("intnPk", out json) &&
                   (int)jsonTrfCal.GetProperty("intnPk").ValueKind == 4
                   )
                {
                    intnPk = jsonTrfCal.GetProperty("intnPk").GetInt32();
                }

                //                                          //Condition.
                GpcondjsonGroupConditionJson gpcondjson = null;
                if (
                    jsonTrfCal.TryGetProperty("condition", out json) &&
                    (int)jsonTrfCal.GetProperty("condition").ValueKind == 3
                    )
                {
                    gpcondjson = JsonSerializer.Deserialize<GpcondjsonGroupConditionJson>(jsonTrfCal.GetProperty(
                        "condition").GetString());
                }

                //                                          //using is to release connection at the end
                using (Odyssey2Context context = new Odyssey2Context())
                {
                    //                                      //Starts a new transaction.
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            CalCalculation.subSetTransform(intnPk, intPkProcessInWorkflow, intPkEleetOrEleeleI, 
                                boolIsEleetI, intPkEleetOrEleeleO, boolIsEleetO, numNeeded, numPerUnit, intPkResourceI, 
                                intPkResourceO, gpcondjson, context, ref intStatus, ref strUserMessage, 
                                ref strDevMessage);

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
                        catch (CustomException custex)
                        {
                            //                              //Discards all changes made to the database in the current
                            //                              //      transaction.
                            dbContextTransaction.Rollback();

                            //                              //Making a log for the exception.
                            Tools.subExceptionHandler(custex, ref intStatus, ref strUserMessage, ref strDevMessage);
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
        public IActionResult DeleteTransform(
            //                                              //PURPOSE:
            //                                              //Delete a transformation calculation.

            //                                              //URL: http://localhost/Odyssey2/Calculation/DeleteTransform
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //  {
            //                                              //      "intPk":8,
            //                                              //  }

            //                                              //DESCRIPTION:
            //                                              //Delete one transform calculation.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            //                                              //      400 - BadRequest().

            //                                              //Receive Json with all the needed information.
            [FromBody] JsonElement jsonTrfCal
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !Object.ReferenceEquals(null, jsonTrfCal) &&
                //                                          //Verify if the data is not null or empty.
                jsonTrfCal.TryGetProperty("intPk", out json) &&
                (int)jsonTrfCal.GetProperty("intPk").ValueKind == 4
                )
            {
                //                                          //Get the info of the calculation.
                int intPk = jsonTrfCal.GetProperty("intPk").GetInt32();

                using (Odyssey2Context context = new Odyssey2Context())
                {
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            CalCalculation.subDeleteTransform(intPk, context, ref intStatus, ref strUserMessage, 
                                ref strDevMessage);

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
                            //                              //Discards all changes made to the database in the 
                            //                              //      current transaction.
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
        public IActionResult DeletePaperTransformation(
            //                                              //PURPOSE:
            //                                              //Deletes a paper transformation from DB.

            //                                              //URL: http://localhost/Odyssey2/Calculation/
            //                                              //      DeletePaperTransformation
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //  {
            //                                              //      "intPkPaTrans":1
            //                                              //  }

            //                                              //DESCRIPTION:
            //                                              //Deletes a paper transformation from DB.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            //                                              //      400 - BadRequest().

            //                                              //Receive Json with all the needed information.
            [FromBody] JsonElement trfCal
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !Object.ReferenceEquals(null, trfCal) &&
                //                                          //Verify if the data is not null or empty.
                trfCal.TryGetProperty("intPkPaTrans", out json) &&
                (int)trfCal.GetProperty("intPkPaTrans").ValueKind == 4 &&
                trfCal.TryGetProperty("boolFromClose", out json) &&
                ((int)trfCal.GetProperty("boolFromClose").ValueKind == 5 ||
                (int)trfCal.GetProperty("boolFromClose").ValueKind == 6)
                )
            {
                //                                          //Get intPkPaTrans.
                int intPkPaTrans = trfCal.GetProperty("intPkPaTrans").GetInt32();
                bool boolFromClose = trfCal.GetProperty("boolFromClose").GetBoolean();

                int? intnPkCalculation = null;
                if (
                    trfCal.TryGetProperty("intnPkCalculation", out json) &&
                    (int)trfCal.GetProperty("intnPkCalculation").ValueKind == 4
                    )
                {
                    intnPkCalculation = trfCal.GetProperty("intnPkCalculation").GetInt32();
                }

                try
                {
                    CalCalculation.subDeletePaperTransformation(intPkPaTrans, intnPkCalculation, boolFromClose,
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
        [HttpPost("[action]")]
        public IActionResult SavePaperTransformation(
            //                                              //PURPOSE:
            //                                              //Saves  a temporary paper transformation.

            //                                              //URL: http://localhost/Odyssey2/Calculation/
            //                                              //      SavePaperTransformation
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //  {
            //                                              //      "intnPkPaTrans": null,
            //                                              //      "intnPkCalculation": null,
            //                                              //      "numWidth": 15,
            //                                              //      "numHeigth": 9,
            //                                              //      "numCutWidth": 15,
            //                                              //      "numCutHeigth": 3,
            //                                              //      "numnMarginTop": 6,
            //                                              //      "numnMarginBottom": 8,
            //                                              //      "numnMarginLeft": 9,
            //                                              //      "numnMarginRight":  10,
            //                                              //      "numnVerticalGap": 34,
            //                                              //      "numnHorizontalGap": 6,
            //                                              //      "strUnit": "in" ,
            //                                              //      "boolIsEleetI": true,
            //                                              //      "intPkEleetOrEleeleI": 3,
            //                                              //      "boolIsEleetO": true,
            //                                              //      "intPkEleetOrEleeleO": 3,
            //                                              //      "intPkResource": 1,
            //                                              //      "intPkProcessInWorkflow": 2 ,
            //                                              //      "boolIsOptimized": true 
            //                                              //  }

            //                                              //DESCRIPTION:
            //                                              //Save a temporary paper transformation.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            //                                              //      400 - BadRequest().

            //                                              //Receive Json with all the needed information.
            [FromBody] JsonElement paperjson
            )
        {
            int intStatus = 400;
            String strUserMessage = "Data is not complete or is invalid.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !((int)paperjson.ValueKind == 7) &&
                //                                          //Verify if the data is not null or empty.
                !((int)paperjson.ValueKind == 0) &&
                paperjson.TryGetProperty("intnPkPaTrans", out json) &&
                paperjson.TryGetProperty("intnPkCalculation", out json) &&
                paperjson.TryGetProperty("numWidth", out json) &&
                (int)paperjson.GetProperty("numWidth").ValueKind == 4 &&
                /*paperjson.TryGetProperty("numHeight", out json) &&
                (int)paperjson.GetProperty("numHeight").ValueKind == 4 &&*/
                paperjson.TryGetProperty("strUnit", out json) &&
                (int)paperjson.GetProperty("strUnit").ValueKind == 3 &&
                paperjson.TryGetProperty("intPkProcessInWorkflow", out json) &&
                (int)paperjson.GetProperty("intPkProcessInWorkflow").ValueKind == 4 &&
                paperjson.TryGetProperty("intPkEleetOrEleeleI", out json) &&
                (int)paperjson.GetProperty("intPkEleetOrEleeleI").ValueKind == 4 &&
                paperjson.TryGetProperty("boolIsEleetI", out json) &&
                ((int)paperjson.GetProperty("boolIsEleetI").ValueKind == 6 ||
                (int)paperjson.GetProperty("boolIsEleetI").ValueKind == 5) &&
                paperjson.TryGetProperty("intPkResourceI", out json) &&
                (int)paperjson.GetProperty("intPkResourceI").ValueKind == 4 &&
                paperjson.TryGetProperty("intPkEleetOrEleeleO", out json) &&
                (int)paperjson.GetProperty("intPkEleetOrEleeleO").ValueKind == 4 &&
                paperjson.TryGetProperty("boolIsEleetO", out json) &&
                ((int)paperjson.GetProperty("boolIsEleetO").ValueKind == 6 ||
                (int)paperjson.GetProperty("boolIsEleetO").ValueKind == 5) &&
                paperjson.TryGetProperty("intPkResourceO", out json) &&
                (int)paperjson.GetProperty("intPkResourceO").ValueKind == 4 &&
                paperjson.TryGetProperty("numnMarginTop", out json) &&
                paperjson.TryGetProperty("numnMarginBottom", out json) &&
                paperjson.TryGetProperty("numnMarginLeft", out json) &&
                paperjson.TryGetProperty("numnMarginRight", out json) &&
                paperjson.TryGetProperty("numnVerticalGap", out json) &&
                paperjson.TryGetProperty("numnHorizontalGap", out json) &&
                paperjson.TryGetProperty("boolIsOptimized", out json) &&
                ((int)paperjson.GetProperty("boolIsOptimized").ValueKind == 6 ||
                (int)paperjson.GetProperty("boolIsOptimized").ValueKind == 5) &&
                paperjson.TryGetProperty("boolCut", out json) &&
                ((int)paperjson.GetProperty("boolCut").ValueKind == 6 ||
                (int)paperjson.GetProperty("boolCut").ValueKind == 5)
                )
            {
                //                                          //Get values.
                double numWidth = paperjson.GetProperty("numWidth").GetDouble();
                //double numHeigth = paperjson.GetProperty("numHeight").GetDouble();
                String strUnit = paperjson.GetProperty("strUnit").GetString();
                int intPkResourceI = paperjson.GetProperty("intPkResourceI").GetInt32();
                int intPkProcessInWorkflow = paperjson.GetProperty("intPkProcessInWorkflow").GetInt32();
                int intPkEleetOrEleeleI = paperjson.GetProperty("intPkEleetOrEleeleI").GetInt32();
                bool boolIsEleetI = paperjson.GetProperty("boolIsEleetI").GetBoolean();
                int intPkEleetOrEleeleO = paperjson.GetProperty("intPkEleetOrEleeleO").GetInt32();
                bool boolIsEleetO = paperjson.GetProperty("boolIsEleetO").GetBoolean();
                int intPkResourceO = paperjson.GetProperty("intPkResourceO").GetInt32();
                bool boolIsOptimized = paperjson.GetProperty("boolIsOptimized").GetBoolean();
                bool boolCut = paperjson.GetProperty("boolCut").GetBoolean();

                //                                          //Get nullables values.
                int? intnPkPaTrans = null;
                if (
                    (int)paperjson.GetProperty("intnPkPaTrans").ValueKind == 4
                    )
                {
                    intnPkPaTrans = paperjson.GetProperty("intnPkPaTrans").GetInt32();
                }

                int? intnPkCalculation = null;
                if (
                    (int)paperjson.GetProperty("intnPkCalculation").ValueKind == 4
                    )
                {
                    intnPkCalculation = paperjson.GetProperty("intnPkCalculation").GetInt32();
                }

                double? numnHeight = null;
                if (
                    paperjson.TryGetProperty("numnHeight", out json) &&
                    (int)paperjson.GetProperty("numnHeight").ValueKind == 4
                    )
                {
                    numnHeight = paperjson.GetProperty("numnHeight").GetDouble();
                }

                double numCutWidth = 0;
                if (
                    paperjson.TryGetProperty("numCutWidth", out json) &&
                    (int)paperjson.GetProperty("numCutWidth").ValueKind == 4
                    )
                {
                    numCutWidth = paperjson.GetProperty("numCutWidth").GetDouble();
                }

                double numCutHeigth = 0;
                if (
                    paperjson.TryGetProperty("numCutHeight", out json) &&
                    (int)paperjson.GetProperty("numCutHeight").ValueKind == 4
                    )
                {
                    numCutHeigth = paperjson.GetProperty("numCutHeight").GetDouble();
                }

                double? numnMarginTop = null;
                if (
                    (int)paperjson.GetProperty("numnMarginTop").ValueKind == 4
                    )
                {
                    numnMarginTop = paperjson.GetProperty("numnMarginTop").GetDouble();
                }

                double? numnMarginBottom = null;
                if (
                    (int)paperjson.GetProperty("numnMarginBottom").ValueKind == 4
                    )
                {
                    numnMarginBottom = paperjson.GetProperty("numnMarginBottom").GetDouble();
                }

                double? numnMarginLeft = null;
                if (
                    (int)paperjson.GetProperty("numnMarginLeft").ValueKind == 4
                    )
                {
                    numnMarginLeft = paperjson.GetProperty("numnMarginLeft").GetDouble();
                }

                double? numnMarginRight = null;
                if (
                    (int)paperjson.GetProperty("numnMarginRight").ValueKind == 4
                    )
                {
                    numnMarginRight = paperjson.GetProperty("numnMarginRight").GetDouble();
                }

                double? numnVerticalGap = null;
                if (
                    (int)paperjson.GetProperty("numnVerticalGap").ValueKind == 4
                    )
                {
                    numnVerticalGap = paperjson.GetProperty("numnVerticalGap").GetDouble();
                }

                double? numnHorizontalGap = null;
                if (
                    (int)paperjson.GetProperty("numnHorizontalGap").ValueKind == 4
                    )
                {
                    numnHorizontalGap = paperjson.GetProperty("numnHorizontalGap").GetDouble();
                }

                try
                {
                    Patransjson2PaperTransformationJson2 patransjson;
                    CalCalculation.subSavePaperTransformation(intnPkPaTrans, intnPkCalculation, numWidth, numnHeight,
                        numCutWidth, numCutHeigth, boolCut, numnMarginTop, numnMarginBottom, numnMarginLeft,
                        numnMarginRight, numnVerticalGap, numnHorizontalGap, strUnit, intPkEleetOrEleeleI,
                        boolIsEleetI, intPkResourceI, intPkEleetOrEleeleO, boolIsEleetO, intPkResourceO,
                        intPkProcessInWorkflow, boolIsOptimized, out patransjson, ref intStatus, ref strUserMessage,
                        ref strDevMessage);
                    obj = patransjson;
                }
                catch (NullReferenceException ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
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
        public IActionResult UpdateStatus(
            //                                              //PURPOSE:
            //                                              //Saves  a temporary paper transformation.

            //                                              //URL: http://localhost/Odyssey2/Calculation/
            //                                              //      UpdateStatus
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //  {
            //                                              //      "intPkCalculation": 2, 
            //                                              //  }

            //                                              //DESCRIPTION:
            //                                              //Update the boolIsEnable value for  a calculation.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            //                                              //      400 - BadRequest().

            //                                              //Receive Json with all the needed information.
            [FromBody] JsonElement paperjson
            )
        {
            int intStatus = 400;
            String strUserMessage = "Invalid Data.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !((int)paperjson.ValueKind == 7) &&
                //                                          //Verify if the data is not null or empty.
                !((int)paperjson.ValueKind == 0) &&
                paperjson.TryGetProperty("intPkCalculation", out json) &&
                (int)paperjson.GetProperty("intPkCalculation").ValueKind == 4
                )
            {
                //                                          //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                //                                          //Get values.
                int intPkCalculation = paperjson.GetProperty("intPkCalculation").GetInt32();

                if (
                    intPkCalculation > 0
                    )
                {
                    using (Odyssey2Context context = new Odyssey2Context())
                    {
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                CalCalculation.subUpdateCalculationStatus(intPkCalculation, ps, context, ref intStatus,
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
                                //                              //Discards all changes made to the database in the 
                                //                              //      current transaction.
                                dbContextTransaction.Rollback();

                                //                              //Making a log for the exception.
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
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetOne(
            //                                              //PURPOSE:
            //                                              //Get one calculation.

            //                                              //URL: http://localhost/Odyssey2/Calculation/
            //                                              //     GetOne?intPk=00
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get one calculation.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            //                                              //      400 - BadRequest().

            //                                              //Receives the intPk.
            int intPk
            )
        {
            int intStatus = 400;
            String strUserMessage = "Invalid data";
            String strDevMessage = "";
            Object obj = null;
            if (
                //                                          //Verify the Pk.
                intPk > 0
                )
            {
                try
                {
                    CaljsonCalculationJson caljson;
                    CalCalculation.subGetOneData(intPk, out caljson, ref intStatus, ref strUserMessage, ref strDevMessage);
                    obj = caljson;
                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }
            }
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetProfit(
            //                                              //PURPOSE:
            //                                              //Get profit calculations.

            //                                              //URL: http://localhost/Odyssey2/Calculation/
            //                                              //     GetProfit?intnPkProduct=00
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get profit calculations from a product.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            //                                              //      400 - BadRequest().

            //                                              //Receives the intnPkProduct.
            int intPkProduct
        )
        {
            //                                              //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;

            int intStatus = 400;
            String strUserMessage = "Invalid data";
            String strDevMessage = "";
            Object obj = null;
            if (
                intPkProduct > 0
                )
            {
                try
                {
                    List<CaljsonCalculationJson> darrcaljson;
                    CalCalculation.subGetProfitCalculationsForAProduct(intPkProduct, strPrintshopId, out darrcaljson,
                            ref intStatus, ref strUserMessage, ref strDevMessage);
                    obj = darrcaljson;
                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }
            }

            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetBase(
            //                                              //PURPOSE:
            //                                              //Get base calculations.

            //                                              //URL: http://localhost/Odyssey2/Calculation/
            //                                              //     GetBase?intPkProduct=00&boolnByProcess=true
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get base calculations from a product.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            //                                              //Receives the intPkProduct.
            int? intPkProduct,
            bool? boolnByProcess,
            bool? boolnByTime,
            int? intnJobId,
            int? intnPkWorkflow
            )
        {
            int intStatus = 400;
            String strUserMessage = "Invalid data.";
            String strDevMessage = "";
            Object obj = null;

            //                                              //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;

            try
            {
                obj = CalCalculation.arrcaljsonGetBase(intPkProduct, boolnByProcess, boolnByTime, intnJobId,
                    strPrintshopId, intnPkWorkflow, this.configuration, ref intStatus, ref strUserMessage, 
                    ref strDevMessage);
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
        [HttpGet("[action]")]
        public IActionResult GetPerUnit(
            //                                              //PURPOSE:
            //                                              //Get per unit calculations.

            //                                              //URL: http://localhost/odyssey/Calculation/
            //                                              //     GetPerUnit?
            //                                              //      intPkProduct=00&boolByIntent=true&boolByProcess=true
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get per unit calculations of a product.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            //                                              //      400 - BadRequest().

            //                                              //Receives the intPkProduct.
            int intPkProduct,
            bool? boolnByProduct,
            bool? boolnByIntent
            )
        {
            //                                              //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;

            int intStatus = 400;
            String strUserMessage = "Invalid data";
            String strDevMessage = "";
            Object obj = null;
            if (
                intPkProduct > 0
                )
            {
                try
                {
                    //                                      //List of calculations to return.
                    List<CaljsonCalculationJson> darrcaljson;
                    CalCalculation.subGetPerUnitCalculationsForAProduct(intPkProduct, strPrintshopId, boolnByProduct,
                            boolnByIntent, out darrcaljson, ref intStatus, ref strUserMessage, ref strDevMessage);
                    obj = darrcaljson;
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
        public IActionResult GetPerQuantity(
            //                                              //PURPOSE:
            //                                              //Get per quantity calculations.

            //                                              //URL: http://localhost/odyssey/Calculation/
            //                                              //     GetPerQuantity?intPkProduct=8&boolnByResource
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get per quantity calculations of a product for a given  
            //                                              //      process.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            int? intPkProduct,
            bool? boolnByProcess,
            bool? boolnByResource,
            bool? boolnByProduct,
            bool? boolnByIntent,
            bool? boolnByTime,
            int? intnJobId,
            int? intnPkWorkflow,
            int? intnPkProcessInWorkflow
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;

            //                                              //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;

            try
            {
                obj = CalCalculation.arrcaljsonGetPerQuantity(intPkProduct, boolnByProcess, boolnByResource,
                    boolnByProduct, boolnByIntent, boolnByTime, intnJobId, strPrintshopId, intnPkWorkflow,
                    intnPkProcessInWorkflow, this.configuration, ref intStatus, ref strUserMessage, 
                    ref strDevMessage);
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
        [HttpGet("[action]")]
        public IActionResult GetAttributes(
            //                                              //PURPOSE:
            //                                              //Get attributes from an element.

            //                                              //URL: http://localhost/Odyssey2/Calculation
            //                                              //      /GetAttributes?intPk=4
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get attributes from an element.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            //                                              //Receive the pk of the element.
            int intPk
            )
        {
            IActionResult aresult = BadRequest("Invalid data.");
            if (
                //                                          //Verify if pk is not null and greater that 0.
                intPk > 0
                )
            {
                try
                {
                    Eleorattrjson1ElementOrAttributeJson2[] arreleorattrjson1 = InttypIntentType.arreleorattrjson1Get(
                        intPk);
                    aresult = Ok(arreleorattrjson1);
                }
                catch (Exception ex)
                {
                    //                                      //TO FIX. Hacer que este servicio regrese el jsonresponse.
                    int intStatus = 0;
                    String strUserMessage = "", strDevMessage = "";

                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }
            }
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetValues(
            //                                              //PURPOSE:
            //                                              //Get values from an attr.

            //                                              //URL: http://localhost/Odyssey2/Calculation
            //                                              //      /GetValues?intPk=4
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get values from an attribute.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            //                                              //Receive the pk of the attr.
            int intPk
            )
        {
            IActionResult aresult = BadRequest("Invalid data.");
            if (
                //                                          //Verify if pk is not null and greater that 0.
                intPk > 0
                )
            {
                try
                {
                    aresult = Ok(AttrAttribute.arrstrGetValues(intPk));
                }
                catch (Exception ex)
                {
                    //                                      //TO FIX. Hacer que este servicio regrese el jsonresponse.
                    int intStatus = 0;
                    String strUserMessage = "", strDevMessage = "";

                    //                              //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }
            }
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetAttributesAndValues(
            //                                              //PURPOSE:
            //                                              //Get attributes and possible values(fields).

            //                                              //URL: http://localhost/Odyssey2/Calculation
            //                                              //      /GetAttributesAndValues?=intPkProduct
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get attributes and possible values for a field.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            //                                              //      400 - Bad Request().

            //                                              //Receive the Pk of the product (orderform).
            int intPkProduct
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            if (
                intPkProduct > 0
                )
            {
                //                                              //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                try
                {
                    List<Attrjson2AttributeJson2> darrattrjson2;
                    CalCalculation.subGetAttributesAndValues(intPkProduct, this.configuration, out darrattrjson2,
                        ref intStatus, ref strUserMessage, ref strDevMessage);
                    obj = darrattrjson2;
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
        public IActionResult GetValuesForAnAttribute(
            //                                              //PURPOSE:
            //                                              //Get possible values for an attribute (field).

            //                                              //URL: http://localhost/Odyssey2/Calculation
            //                                              //      /GetValuesForAnAttribute?intPkAttribute=45

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get possible values for a field, it is valid only
            //                                              //      when the print buyer has to make a choice to
            //                                              //      choose the value.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            //                                              //Receive the attributeId of the order form.
            int intPkAttribute
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            if (
                //                                          //Verify if the pk is not null.
                intPkAttribute > 0
                )
            {
                try
                {
                    List<ValjsonValueJson> darrvaljson;
                    CalCalculation.subGetValuesForAnAttribute(intPkAttribute, this.configuration, out darrvaljson,
                        ref intStatus, ref strUserMessage, ref strDevMessage);
                    obj = darrvaljson;
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
        public IActionResult CalculateJob(
            //                                              //PURPOSE:
            //                                              //Make an estimation of the job.

            //                                              //URL: http://localhost/Odyssey2/Calculation
            //                                              //      /CalculateJob?intJobId=
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Calculate a Job.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            //                                              //Receive the JobId.
            int intJobId,
            String strPrintshopId
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            if (
                //                                          //Valid data.
                (intJobId > 0) &&
                ((strPrintshopId != null) && (strPrintshopId != ""))
                )
            {
                try
                {
                    JobjsonJobJson jobjson;
                    ProdtypProductType.subCalculateJob(intJobId, strPrintshopId, this.configuration, out jobjson,
                        ref intStatus, ref strUserMessage, ref strDevMessage);
                    obj = jobjson;
                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }
            }
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                    obj);
            IActionResult aresult = Ok(respjson1);

            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetProcessDefaults(
            //                                              //PURPOSE:
            //                                              //Get the default calculations of a process.

            //                                              //URL: http://localhost/Odyssey2/Calculation
            //                                              //      /GetProcessDefaults?strPrintshopId?
            //                                              //      strCalculationType
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get calculations from a process.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            //                                              //      400 - BadRequest().

            //                                              //Receives the type of calculation.
            String strCalculationType
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
                ps != null &&
                ((strCalculationType != null) && (strCalculationType != "")) &&
                ((strCalculationType == CalCalculation.strBase) ||
                (strCalculationType == CalCalculation.strPerQuantity) ||
                (strCalculationType == CalCalculation.strPerUnit) ||
                (strCalculationType == CalCalculation.strProfit))
                )
            {
                try
                {
                    List<CaljsonCalculationJson> darrcaljson;
                    CalCalculation.subGetProcessDefaults(strCalculationType, ps, out darrcaljson,ref intStatus, 
                        ref strUserMessage, ref strDevMessage);
                    obj = darrcaljson;
                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }
            }

            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                    obj);
            IActionResult aresult = Ok(respjson1);

            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetResourceDefaults(
            //                                              //PURPOSE:
            //                                              //Get the default calculations of a resource.

            //                                              //URL: http://localhost/Odyssey2/Calculation
            //                                              //      /GetProcessDefaults?strPrintshopId=13832
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get calculations from a resource.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            //                                              //      400 - BadRequest().

            //                                              //Receives the printshop id.
            String strPrintshopId
            )
        {
            int intStatus = 400;
            String strUserMessage = "Invalid data.";
            String strDevMessage = "";
            Object obj = null;
            if (
                (strPrintshopId != null) && (strPrintshopId != "")
                )
            {
                try
                {
                    obj = CalCalculation.arrcaljsonGetResourceDefaults(strPrintshopId);

                    intStatus = 200;
                    strUserMessage = "Success.";
                    strDevMessage = "";
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
        public IActionResult GetGroup(
            //                                              //PURPOSE:
            //                                              //Get the groups of a pkproduct

            //                                              //URL: http://localhost/Odyssey2/Calculation
            //                                              //      /GetGroup?intpk=2
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get group from a product.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            //                                              //Receives the pk of the product.
            int intPk
            )
        {
            IActionResult aresult = BadRequest("Invalid data.");
            if (
                //                                          //Verify if pk is not null and greater that 0.
                intPk > 0
                )
            {
                try
                {
                    //                                          //Get group pk
                    int[] arrIntId = CalCalculation.arrintIdGroup(intPk);
                    //                                          //Validate that it has elements
                    if (
                        arrIntId.Count() > 0
                        )
                    {
                        Array.Sort(arrIntId);
                        aresult = Ok(arrIntId);
                    }
                }
                catch (Exception ex)
                {
                    //                                      //TO FIX. Hacer que este método retorne jsonresponse.
                    int intStatus = 0;
                    String strUserMessage = "", strDevMessage = "";

                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }
            }
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetOneTransform(
            //                                              //PURPOSE:
            //                                              //Get one transform calculation from DB.

            //                                              //URL: http://localhost/Odyssey2/Calculation
            //                                              //      /GetOneTransform?intnPk=1
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get one row from tranform calculation table.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            //                                              //Pk of the calculation is not null.
            int intnPk
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            if (
                intnPk > 0
                )
            {
                try
                {
                    TranscaljsonTransformCalculationJson transcaljson;
                    CalCalculation.subGetOneTransformCalculation(intnPk, out transcaljson, ref intStatus,
                        ref strUserMessage, ref strDevMessage);
                    obj = transcaljson;
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
        public IActionResult GetCalculatedCuts(
        //                                              //PURPOSE:
        //                                              //Return an array with data to print cuts in paper.

        //                                              //URL: http://localhost/Odyssey2/Calculation
        //                                              //      /GetCalculatedCuts
        //                                              //Method: GET.
        //                                              //Use a JSON like this:
        //                                              //{
        //                                              //    "numWidth": 15,
        //                                              //    "numHeight": 9,
        //                                              //    "numCutWidth": 4,
        //                                              //    "numCutHeight": 2,
        //                                              //    "numnMarginTop": 2,
        //                                              //    "numnMarginBottom": 2,
        //                                              //    "numnMarginLeft": 1,
        //                                              //    "numnMarginRight":  1,
        //                                              //    "numnVerticalGap": 0.1,
        //                                              //    "numnHorizontalGap": 0.2,
        //                                              //    "boolIsOptimized":true
        //                                              //}

        //                                              //DESCRIPTION:
        //                                              //Return an array with data to print cuts in paper.

        //                                              //RETURNS:
        //                                              //      200 - Ok().

        //                                              //Receive Json with all the needed information.
        [FromBody] JsonElement jsonData
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;

            JsonElement json;
            if (
                //                                          //Verify if jsonData is not null or a value is not empty.
                !((int)jsonData.ValueKind == 7) &&
                !((int)jsonData.ValueKind == 0) &&
                //                                          //Verify if the data is not null or empty.
                jsonData.TryGetProperty("intPkResource", out json) &&
                (int)jsonData.GetProperty("intPkResource").ValueKind == 4 &&
                jsonData.TryGetProperty("numWidth", out json) &&
                (int)jsonData.GetProperty("numWidth").ValueKind == 4 &&
                jsonData.TryGetProperty("numCutWidth", out json) &&
                (int)jsonData.GetProperty("numCutWidth").ValueKind == 4 &&
                jsonData.TryGetProperty("numCutHeight", out json) &&
                (int)jsonData.GetProperty("numCutHeight").ValueKind == 4 &&
                jsonData.TryGetProperty("boolIsOptimized", out json) &&
                (((int)jsonData.GetProperty("boolIsOptimized").ValueKind == 5) ||
                ((int)jsonData.GetProperty("boolIsOptimized").ValueKind == 6))
                )
            {
                
                int intPkResource = jsonData.GetProperty("intPkResource").GetInt32();
                double numWidth = jsonData.GetProperty("numWidth").GetDouble();
                double numCutWidth = jsonData.GetProperty("numCutWidth").GetDouble();
                double numCutHeight = jsonData.GetProperty("numCutHeight").GetDouble();
                bool boolIsOptimized = jsonData.GetProperty("boolIsOptimized").GetBoolean();

                double? numnHeight = null;
                if (
                   jsonData.TryGetProperty("numnHeight", out json) &&
                   (int)jsonData.GetProperty("numnHeight").ValueKind == 4
                   )
                {
                    numnHeight = jsonData.GetProperty("numnHeight").GetDouble();
                }

                double? numnMarginTop = null;
                if (
                   jsonData.TryGetProperty("numnMarginTop", out json) &&
                   (int)jsonData.GetProperty("numnMarginTop").ValueKind == 4
                   )
                {
                    numnMarginTop = jsonData.GetProperty("numnMarginTop").GetDouble();
                }
                double? numnMarginBottom = null;
                if (
                   jsonData.TryGetProperty("numnMarginBottom", out json) &&
                   (int)jsonData.GetProperty("numnMarginBottom").ValueKind == 4
                   )
                {
                    numnMarginBottom = jsonData.GetProperty("numnMarginBottom").GetDouble();
                }
                double? numnMarginLeft = null;
                if (
                   jsonData.TryGetProperty("numnMarginLeft", out json) &&
                   (int)jsonData.GetProperty("numnMarginLeft").ValueKind == 4
                   )
                {
                    numnMarginLeft = jsonData.GetProperty("numnMarginLeft").GetDouble();
                }
                double? numnMarginRight = null;
                if (
                   jsonData.TryGetProperty("numnMarginRight", out json) &&
                   (int)jsonData.GetProperty("numnMarginRight").ValueKind == 4
                   )
                {
                    numnMarginRight = jsonData.GetProperty("numnMarginRight").GetDouble();
                }
                double? numnVerticalGap = null;
                if (
                   jsonData.TryGetProperty("numnVerticalGap", out json) &&
                   (int)jsonData.GetProperty("numnVerticalGap").ValueKind == 4
                   )
                {
                    numnVerticalGap = jsonData.GetProperty("numnVerticalGap").GetDouble();
                }
                double? numnHorizontalGap = null;
                if (
                   jsonData.TryGetProperty("numnHorizontalGap", out json) &&
                   (int)jsonData.GetProperty("numnHorizontalGap").ValueKind == 4
                   )
                {
                    numnHorizontalGap = jsonData.GetProperty("numnHorizontalGap").GetDouble();
                }

                try
                {
                    CutdatajsonCutDataJson cutdatajson;

                    CalCalculation.subfunCalculateCutsOrFoldedFactor(intPkResource, numWidth, numnHeight, numCutWidth,
                        numCutHeight, numnMarginTop, numnMarginBottom, numnMarginLeft, numnMarginRight, numnVerticalGap,
                        numnHorizontalGap, boolIsOptimized, out cutdatajson, ref intStatus, ref strUserMessage,
                        ref strDevMessage);
                    obj = cutdatajson;
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
        public IActionResult GetOnePaperTransformation(
            //                                              //PURPOSE:
            //                                              //Get one paper transformation from DB.

            //                                              //URL: http://localhost/Odyssey2/Calculation
            //                                              //      /GetOnePaperTransformation
            //                                              //Method: GET.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intnPkPaTrans":1,  
            //                                              //          "intPkEleetOrEleele":5, 
            //                                              //          "boolIsEleet":true,
            //                                              //          "intPkProcessInWorkflow":6,
            //                                              //          "intPkResource":3
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Get one row from paper transformation table.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            [FromBody] JsonElement jsonPaTrans  
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if jsonFilter is not null or a value is not empty.
                !((int)jsonPaTrans.ValueKind == 7) &&
                !((int)jsonPaTrans.ValueKind == 0) &&
                jsonPaTrans.TryGetProperty("intnPkPaTrans", out json) &&
                jsonPaTrans.TryGetProperty("intPkEleetOrEleele", out json) &&
                (int)jsonPaTrans.GetProperty("intPkEleetOrEleele").ValueKind == 4 &&
                jsonPaTrans.TryGetProperty("boolIsEleet", out json) &&
                ((int)jsonPaTrans.GetProperty("boolIsEleet").ValueKind == 6 ||
                (int)jsonPaTrans.GetProperty("boolIsEleet").ValueKind == 5) &&

                jsonPaTrans.TryGetProperty("intPkEleetOrEleeleO", out json) &&
                (int)jsonPaTrans.GetProperty("intPkEleetOrEleeleO").ValueKind == 4 &&
                jsonPaTrans.TryGetProperty("boolIsEleetO", out json) &&
                ((int)jsonPaTrans.GetProperty("boolIsEleetO").ValueKind == 6 ||
                (int)jsonPaTrans.GetProperty("boolIsEleetO").ValueKind == 5) &&


                jsonPaTrans.TryGetProperty("intPkProcessInWorkflow", out json) &&
                (int)jsonPaTrans.GetProperty("intPkProcessInWorkflow").ValueKind == 4 && 
                jsonPaTrans.TryGetProperty("intPkResource", out json) &&
                (int)jsonPaTrans.GetProperty("intPkResource").ValueKind == 4
                )
            {
                //                                          //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                //                                          //Get data from body.
                int intPkEleetOrEleele = jsonPaTrans.GetProperty("intPkEleetOrEleele").GetInt32();
                bool boolIsEleet = jsonPaTrans.GetProperty("boolIsEleet").GetBoolean();
                int intPkEleetOrEleeleO = jsonPaTrans.GetProperty("intPkEleetOrEleeleO").GetInt32();
                bool boolIsEleetO = jsonPaTrans.GetProperty("boolIsEleetO").GetBoolean();
                int intPkProcessInWorkflow = jsonPaTrans.GetProperty("intPkProcessInWorkflow").GetInt32();
                int intPkResource = jsonPaTrans.GetProperty("intPkResource").GetInt32();

                int? intnPkPaTrans = null;
                if (
                    (int)jsonPaTrans.GetProperty("intnPkPaTrans").ValueKind == 4
                    )
                {
                    intnPkPaTrans = jsonPaTrans.GetProperty("intnPkPaTrans").GetInt32();
                }

                int? intnJobId = null;
                if (
                    jsonPaTrans.TryGetProperty("intnJobId", out json) &&
                    (int)jsonPaTrans.GetProperty("intnJobId").ValueKind == 4
                    )
                {
                    intnJobId = jsonPaTrans.GetProperty("intnJobId").GetInt32();
                }

                try
                {
                    PatransjsonPaperTransformationJson patransjson;
                    CalCalculation.subGetOnePaperTransformation(intnPkPaTrans, intPkEleetOrEleele,
                        boolIsEleet, intPkEleetOrEleeleO, boolIsEleetO, intPkProcessInWorkflow, intPkResource, intnJobId,
                        this.configuration, ps, out patransjson, ref intStatus, ref strUserMessage, ref strDevMessage);
                    obj = patransjson;
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
        public IActionResult GetBasePerQuantity(
            //                                              //PURPOSE:
            //                                              //Get base per quantity calculations.

            //                                              //URL: http://localhost/odyssey2/Calculation/
            //                                              //      GetBasePerQuantity?intnPkProduct=8&
            //                                              //      intPkWorkflow=1&intnJobId=22342
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get base per quantity calculations of a product for a 
            //                                              //      given process.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            int intPkWorkflow,
            int? intnPkProduct,
            int? intnJobId
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;

            //                                              //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;

            try
            {
                obj = CalCalculation.arrcaljsonGetBasePerQuantity(intnPkProduct, intnJobId, strPrintshopId, intPkWorkflow, 
                    this.configuration, ref intStatus, ref strUserMessage, ref strDevMessage);
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

        /*//--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetPerUnitFromThickness(
            //                                              //PURPOSE:
            //                                              //Get Needed depending on thickness.

            //                                              //URL: http://localhost/odyssey2/Calculation/
            //                                              //      GetPerUnitFromThickness?intPkResource=8&
            //                                              //      intPkResourceQFrom=1&intPkWorkflow=1
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get the thickness from resource that has boolThickness. 
            //                                              //If Resource is a device, gets the lift.
            //                                              //If Resource is a MiscConsumable, gets the height.
            //                                              //Calculate PerUnits.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            int intPkResource,
            int intPkResourceQFrom,
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

            try
            {
                obj = CalCalculation.intGetPerUnit(intPkResource, intPkResourceQFrom, intPkWorkflow, ref intStatus,
                    ref strUserMessage, ref strDevMessage);
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
        }*/

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetTransformCalculations(
            //                                              //PURPOSE:
            //                                              //Get transform calculations.

            //                                              //URL: http://localhost/odyssey/Calculation/
            //                                              //     GetTransformCalculations?intnJobId=null&
            //                                              //      intPkProcessInWorkflow=1
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get transformation calculations
            //                                              //       for a given process.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            int? intnJobId,
            int intPkProcessInWorkflow
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;

            //                                              //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;

            try
            {
                TranscaljsonTransformCalculationJson[] arrtranscaljson;
                CalCalculation.subGetTransformCalculations(intnJobId, intPkProcessInWorkflow, strPrintshopId, 
                    out arrtranscaljson, ref intStatus, ref strUserMessage, ref strDevMessage);
                obj = arrtranscaljson;
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
    }

    //==================================================================================================================
}
/*END-TASK*/
