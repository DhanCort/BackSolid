/*TASK RP. PRINTSHOP*/
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Odyssey2Backend.DB_Odyssey2;
using Odyssey2Backend.JsonTypes;
using Odyssey2Backend.PrintShop;
using Odyssey2Backend.Utilities;
using Odyssey2Backend.XJDF;
using System;
using System.Collections.Generic;
using System.Linq;
using TowaStandard;



//                                                          //AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //CO-AUTHOR: Towa (AQG-Andrea Quiroz).
//                                                          //DATE: April 25, 2020.

namespace Odyssey2Backend.Controllers
{
    //==================================================================================================================
    [Route("Odyssey2/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS METHODS.


        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]/")]
        public IActionResult ChangeConditionsToNewFormat(
            //                                              //PURPOSE:
            //                                              //To read data form old-fashion condition and fill new tables.

            //                                              //URL: http://localhost/Odyssey2/Test/
            //                                              //      ChangeConditionsToNewFormat
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get test.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            )
        {
            int intStatus = 200;
            String strUserMessage = "";
            String strDevMessage = "Good luck Lili.";
            Object obj = null;

            Odyssey2Context context = new Odyssey2Context();

            //                                              //1. Calculations.
            //List<CalentityCalculationEntityDB> darrcalentity = context.Calculation.Where(cal => 
            //    (cal.intnMinAmount != null) ||
            //    (cal.intnMaxAmount != null) ||
            //    (cal.strConditionToApply != null)
            //    ).ToList();

            //foreach (CalentityCalculationEntityDB cal in darrcalentity)
            //{
            //    String strConditionToApply = cal.strConditionToApply;
            //    bool boolConditionAnd = cal.boolConditionAnd;
            //    String strQuantityOperator = boolConditionAnd ? "AND" : "OR";
            //    int? intnMinAmount = cal.intnMinAmount;
            //    int? intnMaxAmount = cal.intnMaxAmount;

            //    GpcondjsonGroupConditionJson gpcondjson = TestController.gpcondjsonFromCalculation(strConditionToApply,
            //        strQuantityOperator, intnMinAmount, intnMaxAmount);

            //    Tools.subAddCondition(cal.intPk, null, null, null, gpcondjson, context);
            //}

            //                                              //2. InputsAndOutputs.
            List<IoentityInputsAndOutputsEntityDB> darrioentity = context.InputsAndOutputs.Where(io =>
                (io.strConditionQuantity != null) ||
                (io.strConditionToApply != null)).ToList();

            foreach (IoentityInputsAndOutputsEntityDB io in darrioentity)
            {
                String strConditionToApply = io.strConditionToApply;
                bool boolConditionAnd = io.boolConditionAnd;
                String strQuantityOperator = boolConditionAnd ? "AND" : "OR";

                String strConditionQuantity = io.strConditionQuantity;
                int? intnMinAmount = null;
                int? intnMaxAmount = null;
                if (
                    strConditionQuantity != null && strConditionQuantity.Length > 0
                    )
                {
                    int intIndexOfPipe = strConditionQuantity.IndexOf(Tools.charConditionSeparator);
                    intnMinAmount = (strConditionQuantity.Substring(0, intIndexOfPipe).Length > 0) ?
                        strConditionQuantity.Substring(0, intIndexOfPipe).ParseToInt() : (int?)null;
                    intnMaxAmount = (strConditionQuantity.Substring(intIndexOfPipe + 1).Length > 0) ?
                        strConditionQuantity.Substring(intIndexOfPipe + 1).ParseToInt() : (int?)null;
                }

                GpcondjsonGroupConditionJson gpcondjson = TestController.gpcondjsonFromCalculation(strConditionToApply,
                    strQuantityOperator, intnMinAmount, intnMaxAmount);

                Tools.subAddCondition(null, null, io.intPk, null, gpcondjson, context);
            }

            //                                              //3. TransformCalculation.
            List<TrfcalentityTransformCalculationEntityDB> darrtrfcalentity = context.TransformCalculation.Where(trfcal =>
                (trfcal.strConditionQuantity != null) ||
                (trfcal.strConditionToApply != null)).ToList();

            foreach (TrfcalentityTransformCalculationEntityDB trfcal in darrtrfcalentity)
            {
                String strConditionToApply = trfcal.strConditionToApply;
                bool boolConditionAnd = trfcal.boolConditionAnd;
                String strQuantityOperator = boolConditionAnd ? "AND" : "OR";

                String strConditionQuantity = trfcal.strConditionQuantity;
                int? intnMinAmount = null;
                int? intnMaxAmount = null;
                if (
                    strConditionQuantity != null && strConditionQuantity.Length > 0
                    )
                {
                    int intIndexOfPipe = strConditionQuantity.IndexOf(Tools.charConditionSeparator);
                    intnMinAmount = (strConditionQuantity.Substring(0, intIndexOfPipe).Length > 0) ?
                        strConditionQuantity.Substring(0, intIndexOfPipe).ParseToInt() : (int?)null;
                    intnMaxAmount = (strConditionQuantity.Substring(intIndexOfPipe + 1).Length > 0) ?
                        strConditionQuantity.Substring(intIndexOfPipe + 1).ParseToInt() : (int?)null;
                }

                GpcondjsonGroupConditionJson gpcondjson = TestController.gpcondjsonFromCalculation(strConditionToApply,
                    strQuantityOperator, intnMinAmount, intnMaxAmount);

                Tools.subAddCondition(null, null, null, trfcal.intPk, gpcondjson, context);
            }

            //                                              //4. LinkNode.
            List<LinknodLinkNodeEntityDB> darrlinknodentity = context.LinkNode.Where(linknod =>
                (linknod.strConditionQuantity != null) ||
                (linknod.strConditionToApply != null)).ToList();

            foreach (LinknodLinkNodeEntityDB linknod in darrlinknodentity)
            {
                String strConditionToApply = linknod.strConditionToApply;
                bool boolConditionAnd = linknod.boolConditionAnd;
                String strQuantityOperator = boolConditionAnd ? "AND" : "OR";

                String strConditionQuantity = linknod.strConditionQuantity;
                int? intnMinAmount = null;
                int? intnMaxAmount = null;
                if (
                    strConditionQuantity != null && strConditionQuantity.Length > 0
                    )
                {
                    int intIndexOfPipe = strConditionQuantity.IndexOf(Tools.charConditionSeparator);
                    intnMinAmount = (strConditionQuantity.Substring(0, intIndexOfPipe).Length > 0) ?
                        strConditionQuantity.Substring(0, intIndexOfPipe).ParseToInt() : (int?)null;
                    intnMaxAmount = (strConditionQuantity.Substring(intIndexOfPipe + 1).Length > 0) ?
                        strConditionQuantity.Substring(intIndexOfPipe + 1).ParseToInt() : (int?)null;
                }

                GpcondjsonGroupConditionJson gpcondjson = TestController.gpcondjsonFromCalculation(strConditionToApply,
                    strQuantityOperator, intnMinAmount, intnMaxAmount);

                Tools.subAddCondition(null, linknod.intPk, null, null, gpcondjson, context);
            }

            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
               obj);
            IActionResult aresult = base.Ok(respjson1);

            return aresult;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static GpcondjsonGroupConditionJson gpcondjsonFromCalculation(
            String strConditionToApply_I,
            String strQuantityOperator_I,
            int? intnMinAmount_I,
            int? intnMaxAmount_I
            )
        {
            String strOperator = "AND";
            CondjsonConditionJson[] arrcondjasonToReturn = null; 
            GpcondjsonGroupConditionJson[] arrgpcondjasonToReturn = null;

            /*CASE*/
            if (
                intnMinAmount_I == null &&
                intnMaxAmount_I == null &&
                (strConditionToApply_I == null || strConditionToApply_I == "")
                )
            {
                //Do nothing.
            }

            //                                              //Only quantities data.
            else if (
                intnMinAmount_I != null &&
                intnMaxAmount_I == null &&
                (strConditionToApply_I == null || strConditionToApply_I == "")
                )
            {
                CondjsonConditionJson condMinAmount = new CondjsonConditionJson(null, ">=", ((int)intnMinAmount_I) + "");
                arrcondjasonToReturn = new CondjsonConditionJson[1];
                arrcondjasonToReturn[0] = condMinAmount;
            }
            else if (
                intnMinAmount_I == null &&
                intnMaxAmount_I != null &&
                (strConditionToApply_I == null || strConditionToApply_I == "")
                )
            {
                CondjsonConditionJson condMaxAmount = new CondjsonConditionJson(null, "<=", ((int)intnMaxAmount_I) + "");
                arrcondjasonToReturn = new CondjsonConditionJson[1];
                arrcondjasonToReturn[0] = condMaxAmount;

            }
            else if (
                intnMinAmount_I != null &&
                intnMaxAmount_I != null &&
                (strConditionToApply_I == null || strConditionToApply_I == "")
                )
            {
                CondjsonConditionJson condMinAmount = new CondjsonConditionJson(null, ">=", ((int)intnMinAmount_I) + "");
                CondjsonConditionJson condMaxAmount = new CondjsonConditionJson(null, "<=", ((int)intnMaxAmount_I) + "");
                arrcondjasonToReturn = new CondjsonConditionJson[2];
                arrcondjasonToReturn[0] = condMinAmount;
                arrcondjasonToReturn[1] = condMaxAmount;
            }

            //                                              //Only condition data.
            else if (
                intnMinAmount_I == null &&
                intnMaxAmount_I == null &&
                //                                          //"3253|==|Express"
                (strConditionToApply_I != null && strConditionToApply_I != "")
                )
            {
                GpcondjsonGroupConditionJson gpcondjasonFromCondition = 
                    TestController.gpcondjsonFromCondition(strConditionToApply_I);
                strOperator = gpcondjasonFromCondition.strOperator;
                arrcondjasonToReturn = gpcondjasonFromCondition.arrcond;
                arrgpcondjasonToReturn = gpcondjasonFromCondition.arrgpcond;
            }

            //                                              //Both, data form quantities and condition.
            else if (
                intnMinAmount_I != null &&
                intnMaxAmount_I == null &&
                (strConditionToApply_I != null && strConditionToApply_I != "")
                )
            {
                CondjsonConditionJson condMinAmount = new CondjsonConditionJson(null, ">=", ((int)intnMinAmount_I) + "");

                GpcondjsonGroupConditionJson gpcondjasonFromCondition =
                    TestController.gpcondjsonFromCondition(strConditionToApply_I);

                if (
                    //                                      //Simple condition.
                    gpcondjasonFromCondition != null &&
                    gpcondjasonFromCondition.arrcond != null &&
                    gpcondjasonFromCondition.arrcond.Length == 1 &&
                    (gpcondjasonFromCondition.arrgpcond == null || gpcondjasonFromCondition.arrgpcond.Length == 0)
                    )
                {
                    arrcondjasonToReturn = new CondjsonConditionJson[2];
                    arrcondjasonToReturn[0] = condMinAmount;
                    arrcondjasonToReturn[1] = gpcondjasonFromCondition.arrcond[0];
                }
                else if (
                    //                                      //Simple group.
                    gpcondjasonFromCondition != null &&
                    (gpcondjasonFromCondition.arrcond == null || gpcondjasonFromCondition.arrcond.Length == 0) &&
                    gpcondjasonFromCondition.arrgpcond != null &&
                    gpcondjasonFromCondition.arrgpcond.Length == 1
                    )
                {
                    arrcondjasonToReturn = new CondjsonConditionJson[1];
                    arrcondjasonToReturn[0] = condMinAmount;
                    arrgpcondjasonToReturn = new GpcondjsonGroupConditionJson[1];
                    arrgpcondjasonToReturn[0] = gpcondjasonFromCondition.arrgpcond[0];
                }
                else if (
                    gpcondjasonFromCondition != null
                    )
                {
                    arrcondjasonToReturn = new CondjsonConditionJson[1];
                    arrcondjasonToReturn[0] = condMinAmount;
                    arrgpcondjasonToReturn = new GpcondjsonGroupConditionJson[1];
                    arrgpcondjasonToReturn[0] = gpcondjasonFromCondition;
                }
                strOperator = strQuantityOperator_I;
            }
            else if (
                intnMinAmount_I == null &&
                intnMaxAmount_I != null &&
                (strConditionToApply_I != null && strConditionToApply_I != "")
                )
            {
                CondjsonConditionJson condMaxAmount = new CondjsonConditionJson(null, "<=", ((int)intnMaxAmount_I) + "");

                GpcondjsonGroupConditionJson gpcondjasonFromCondition =
                    TestController.gpcondjsonFromCondition(strConditionToApply_I);

                if (
                    //                                      //Simple condition.
                    gpcondjasonFromCondition != null &&
                    gpcondjasonFromCondition.arrcond != null &&
                    gpcondjasonFromCondition.arrcond.Length == 1 &&
                    (gpcondjasonFromCondition.arrgpcond == null || gpcondjasonFromCondition.arrgpcond.Length == 0)
                    )
                {
                    arrcondjasonToReturn = new CondjsonConditionJson[2];
                    arrcondjasonToReturn[0] = condMaxAmount;
                    arrcondjasonToReturn[1] = gpcondjasonFromCondition.arrcond[0];
                }
                else if (
                    //                                      //Simple group.
                    gpcondjasonFromCondition != null &&
                    (gpcondjasonFromCondition.arrcond == null || gpcondjasonFromCondition.arrcond.Length == 0) &&
                    gpcondjasonFromCondition.arrgpcond != null &&
                    gpcondjasonFromCondition.arrgpcond.Length == 1
                    )
                {
                    arrcondjasonToReturn = new CondjsonConditionJson[1];
                    arrcondjasonToReturn[0] = condMaxAmount;
                    arrgpcondjasonToReturn = new GpcondjsonGroupConditionJson[1];
                    arrgpcondjasonToReturn[0] = gpcondjasonFromCondition.arrgpcond[0];
                }
                else if(
                    gpcondjasonFromCondition != null
                    )
                {
                    arrcondjasonToReturn = new CondjsonConditionJson[1];
                    arrcondjasonToReturn[0] = condMaxAmount;
                    arrgpcondjasonToReturn = new GpcondjsonGroupConditionJson[1];
                    arrgpcondjasonToReturn[0] = gpcondjasonFromCondition;
                }
                strOperator = strQuantityOperator_I;
            }
            else if (
                intnMinAmount_I != null &&
                intnMaxAmount_I != null &&
                (strConditionToApply_I != null && strConditionToApply_I != "")
                )
            {
                CondjsonConditionJson condMinAmount = new CondjsonConditionJson(null, ">=", ((int)intnMinAmount_I) + "");
                CondjsonConditionJson condMaxAmount = new CondjsonConditionJson(null, "<=", ((int)intnMaxAmount_I) + "");

                GpcondjsonGroupConditionJson gpcondjasonFromCondition =
                    TestController.gpcondjsonFromCondition(strConditionToApply_I);

                if (
                    //                                      //Simple condition.
                    gpcondjasonFromCondition != null &&
                    gpcondjasonFromCondition.arrcond != null &&
                    gpcondjasonFromCondition.arrcond.Length == 1 &&
                    (gpcondjasonFromCondition.arrgpcond == null || gpcondjasonFromCondition.arrgpcond.Length == 0)
                    )
                {
                    if (
                        strOperator == "AND"
                        )
                    {
                        arrcondjasonToReturn = new CondjsonConditionJson[3];
                        arrcondjasonToReturn[0] = condMinAmount;
                        arrcondjasonToReturn[1] = condMaxAmount;
                        arrcondjasonToReturn[2] = gpcondjasonFromCondition.arrcond[0];
                    }
                    else
                    {
                        CondjsonConditionJson[] arrcondjasonQuantities = new CondjsonConditionJson[2];
                        arrcondjasonQuantities[0] = condMinAmount;
                        arrcondjasonQuantities[1] = condMaxAmount;
                        arrgpcondjasonToReturn = new GpcondjsonGroupConditionJson[1];
                        arrgpcondjasonToReturn[0] = new GpcondjsonGroupConditionJson("AND", arrcondjasonQuantities, null);

                        arrcondjasonToReturn = new CondjsonConditionJson[1];
                        arrcondjasonToReturn[0] = gpcondjasonFromCondition.arrcond[0];
                    }
                }
                else if (
                    //                                      //Simple group.
                    gpcondjasonFromCondition != null &&
                    (gpcondjasonFromCondition.arrcond == null || gpcondjasonFromCondition.arrcond.Length == 0) &&
                    gpcondjasonFromCondition.arrgpcond != null &&
                    gpcondjasonFromCondition.arrgpcond.Length == 1
                    )
                {
                    CondjsonConditionJson[] arrcondjasonQuantities = new CondjsonConditionJson[2];
                    arrcondjasonQuantities[0] = condMinAmount;
                    arrcondjasonQuantities[1] = condMaxAmount;
                    arrgpcondjasonToReturn = new GpcondjsonGroupConditionJson[2];
                    arrgpcondjasonToReturn[0] = new GpcondjsonGroupConditionJson("AND", arrcondjasonQuantities, null);
                    arrgpcondjasonToReturn[1] = gpcondjasonFromCondition.arrgpcond[0];
                }
                else if (
                    gpcondjasonFromCondition != null
                    )
                {
                    CondjsonConditionJson[] arrcondjasonQuantities = new CondjsonConditionJson[2];
                    arrcondjasonQuantities[0] = condMinAmount;
                    arrcondjasonQuantities[1] = condMaxAmount;
                    arrgpcondjasonToReturn = new GpcondjsonGroupConditionJson[2];
                    arrgpcondjasonToReturn[0] = new GpcondjsonGroupConditionJson("AND", arrcondjasonQuantities, null);
                    arrgpcondjasonToReturn[1] = gpcondjasonFromCondition;
                }
                strOperator = strQuantityOperator_I;
            }
            /*END-CASE*/

            GpcondjsonGroupConditionJson gpcondjasonToReturn = new GpcondjsonGroupConditionJson(
                strOperator, arrcondjasonToReturn, arrgpcondjasonToReturn);

            return gpcondjasonToReturn;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static GpcondjsonGroupConditionJson gpcondjsonFromCondition(
            String strCondition_I
            )
        {
            String strOperator = "AND";
            CondjsonConditionJson[] arrcondjasonToReturn;
            GpcondjsonGroupConditionJson[] arrgpcondjasonToReturn = null;


            int intI = 0;
            char charC = Tools.charOpenParetheses;
            /*UNTIL-DO*/
            while (!(
                (intI >= strCondition_I.Length) ||
                (charC != Tools.charOpenParetheses)
                ))
            {
                charC = strCondition_I[intI];
                intI = intI + 1;
            }
            int intP = intI - 1;
            if (
                //                                      //There is no parentheses that means it is a relational 
                //                                      //      operation, the simpliest condition.
                //                                      //"3253|==|Express"
                intP == 0
                )
            {
                int intIndexNextPipe = strCondition_I.IndexOf(Tools.charConditionSeparator);
                int? intnPkAttribute =
                    strCondition_I.Substring(0, intIndexNextPipe).ParseToInt();
                String strConditionAndVAlue = strCondition_I.Substring(intIndexNextPipe + 1);
                intIndexNextPipe = strConditionAndVAlue.IndexOf(Tools.charConditionSeparator);
                String strCondition = strConditionAndVAlue.Substring(0, intIndexNextPipe);
                String strValue = strConditionAndVAlue.Substring(intIndexNextPipe + 1);

                CondjsonConditionJson condjson =
                    new CondjsonConditionJson(intnPkAttribute, strCondition, strValue);
                arrcondjasonToReturn = new CondjsonConditionJson[1];
                arrcondjasonToReturn[0] = condjson;
            }
            else
            {
                //                                      //It is a logical operation.
                //                                      //Get the two elements of the operation, its bool about
                //                                      //      if it applies and the logical operation between 
                //                                      //      that bools.

                List<CondjsonConditionJson> darrcondjsonLeftRigth = new List<CondjsonConditionJson>();
                List<GpcondjsonGroupConditionJson> darrgpcondjsonLeftRigth = new List<GpcondjsonGroupConditionJson>();

                //                                      //Get the left element.
                String strConditionLeft = Tools.strGetLeftElement(strCondition_I);

                int intIL = 0;
                char charCL = Tools.charOpenParetheses;
                /*UNTIL-DO*/
                while (!(
                    (intIL >= strConditionLeft.Length) ||
                    (charCL != Tools.charOpenParetheses)
                    ))
                {
                    charCL = strConditionLeft[intIL];
                    intIL = intIL + 1;
                }

                int intPL = intIL - 1;
                if (
                    //                                      //There is no parentheses that means it is a relational 
                    //                                      //      operation, the simpliest condition.
                    //                                      //"3253|==|Express"
                    intPL == 0
                    )
                {
                    int intIndexNextPipe = strConditionLeft.IndexOf(Tools.charConditionSeparator);
                    int? intnPkAttribute =
                        strConditionLeft.Substring(0, intIndexNextPipe).ParseToInt();
                    String strConditionAndVAlue = strConditionLeft.Substring(intIndexNextPipe + 1);
                    intIndexNextPipe = strConditionAndVAlue.IndexOf(Tools.charConditionSeparator);
                    String strCondition = strConditionAndVAlue.Substring(0, intIndexNextPipe);
                    String strValue = strConditionAndVAlue.Substring(intIndexNextPipe + 1);

                    CondjsonConditionJson condjsonLeft =
                        new CondjsonConditionJson(intnPkAttribute, strCondition, strValue);

                    darrcondjsonLeftRigth.Add(condjsonLeft);
                }
                else
                {
                    GpcondjsonGroupConditionJson gpcondjasonFromLeft =
                        TestController.gpcondjsonFromCondition(strConditionLeft);

                    darrgpcondjsonLeftRigth.Add(gpcondjasonFromLeft);
                }

                //                                      //Get the right element.
                String strConditionRight = Tools.strGetRightElement(strCondition_I);

                int intIR = 0;
                char charCR = Tools.charOpenParetheses;
                /*UNTIL-DO*/
                while (!(
                    (intIR >= strConditionRight.Length) ||
                    (charCR != Tools.charOpenParetheses)
                    ))
                {
                    charCR = strConditionRight[intIR];
                    intIR = intIR + 1;
                }

                int intPR = intIR - 1;
                if (
                    //                                      //There is no parentheses that means it is a relational 
                    //                                      //      operation, the simpliest condition.
                    //                                      //"3253|==|Express"
                    intPR == 0
                    )
                {
                    int intIndexNextPipe = strConditionRight.IndexOf(Tools.charConditionSeparator);
                    int? intnPkAttribute =
                        strConditionRight.Substring(0, intIndexNextPipe).ParseToInt();
                    String strConditionAndVAlue = strConditionRight.Substring(intIndexNextPipe + 1);
                    intIndexNextPipe = strConditionAndVAlue.IndexOf(Tools.charConditionSeparator);
                    String strCondition = strConditionAndVAlue.Substring(0, intIndexNextPipe);
                    String strValue = strConditionAndVAlue.Substring(intIndexNextPipe + 1);

                    CondjsonConditionJson condjsonRight =
                        new CondjsonConditionJson(intnPkAttribute, strCondition, strValue);

                    darrcondjsonLeftRigth.Add(condjsonRight);
                }
                else
                {
                    GpcondjsonGroupConditionJson gpcondjasonFromRight =
                        TestController.gpcondjsonFromCondition(strConditionRight);

                    darrgpcondjsonLeftRigth.Add(gpcondjasonFromRight);
                }

                arrcondjasonToReturn = darrcondjsonLeftRigth.ToArray();
                arrgpcondjasonToReturn = darrgpcondjsonLeftRigth.ToArray();

                //                                      //Get the operator.
                strOperator = strCondition_I.Substring(strConditionLeft.Length + 3);
                strOperator = strOperator.Substring(0, strOperator.IndexOf(Tools.charConditionSeparator));
            }

            GpcondjsonGroupConditionJson gpcondjasonToReturn = new GpcondjsonGroupConditionJson(strOperator,
                arrcondjasonToReturn, arrgpcondjasonToReturn);

            return gpcondjasonToReturn;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]/")]
        public IActionResult GetTest(
            //                                              //PURPOSE:
            //                                              //Get .

            //                                              //URL: http://localhost/Odyssey2/Test/GetTest
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get test.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            )
        {
            int intStatus = 200;
            String strUserMessage = "";
            String strDevMessage = "Just for test.";
            Object obj = null;

            //                                              //ztime example for start and end in the PERIOD table.
            //ZonedTime ztimeStart = ZonedTime.Now(TimeZoneX.US_CENTRAL_STANDARD_TIME);
            ZonedTime ztimeStart = new ZonedTime(
                "2020-04-24".ParseToDate(),
                "11:00:00".ParseToTime(),
                TimeZoneX.US_CENTRAL
            );
            ZonedTime ztimenow = ZonedTime.Now(ZonedTimeTools.timezone);

            //                                              //To store the ztime in database, convert to String.
            String strDateStart = ztimeStart.Date.ToText();
            String strTimeStart = ztimeStart.Time.ToText();

            String strZtime = ztimeStart.ToText();

            //ZonedTime ztimeX = strZtime.ParseToZtime();

            //                                              //String for date and time for start from PERIOD table.
            String strDateStartFromDB = "2020-04-23";
            String strTimeStartFromDB = "10:00:00";

            //                                              //With the strings from the database, create the ztime.
            ZonedTime ztimeAnotherStart = new ZonedTime(
                strDateStartFromDB.ParseToDate(),
                strTimeStartFromDB.ParseToTime(),
                TimeZoneX.US_CENTRAL
                );

            obj = new {
                ztimenow,
                strDateStart,
                strTimeStart,
                ztimeAnotherStart
            };
            /*
            //                                              //To store time to the DB.
            //Time timeStart = Time.Now(TimeZoneX.US_CENTRAL_STANDARD_TIME);
            Time timeStart = "11:00:00".ParseToTime();

            String strTimeMyStart = timeStart.ToString();

            //                                              //To get from DB.
            String strMyTimeStartFromDB = "10:00:00";
            Time timeStartFromDB = strMyTimeStartFromDB.ParseToTime();

            //                                              //If data come from the front, validate.
            if (
                "11:00:00".IsParsableToTime()
                )
            { 
                //                                          //Store in DB.
            }

            obj = new
            {
                timeStart,
                strTimeMyStart,
                strMyTimeStartFromDB,
                timeStartFromDB
            };
            */
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
               obj);
            IActionResult aresult = base.Ok(respjson1);
            
            return aresult;
        }

        //------------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]/")]
        public IActionResult TestCurrency(
            
            )
        {
            int intStatus = 200;
            String strUserMessage = "";
            String strDevMessage = "Just for test.";
            Object obj = null;

            //Currency curr = new Currency();

            obj = new
            {

            };

            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
               obj);
            IActionResult aresult = base.Ok(respjson1);

            return aresult;
        }

        //------------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]/")]
        public IActionResult TestSaveChangesWithTryCatch1(
            //                                              //Use correct of save changes with the try-catch.

            //                                              //Create a new resource and delete this resource 
            //                                              //    for test.

            int intPkResource
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;

            Odyssey2Context context = new Odyssey2Context();

            if (
                intPkResource > 0
                )
            {
                try
                {
                    EleentityElementEntityDB eleentity = context.Element.FirstOrDefault(ele => ele.intPk == intPkResource);

                    intStatus = 401;
                    strUserMessage = "Something is wrong.";
                    strDevMessage = "Resource does not found.";
                    if (
                        //                                      //Exist this resoruce.
                        eleentity != null
                        )
                    {
                        //                                      //Get the calculations that are related to the res.
                        List<CalentityCalculationEntityDB> darrcalentity = context.Calculation.Where(calentity =>
                            calentity.intnPkResource == intPkResource).ToList();

                        //                                      //Delete the calculations.
                        foreach (CalentityCalculationEntityDB calentity in darrcalentity)
                        {
                            context.Calculation.Remove(calentity);
                        }

                        //                                      //Get the values that are related to the res.
                        List<ValentityValueEntityDB> darrvalentity = context.Value.Where(valentity =>
                            valentity.intPkElement == intPkResource).ToList();

                        //                                      //Delete the values of the resource or template.
                        foreach (ValentityValueEntityDB valentity in darrvalentity)
                        {
                            context.Value.Remove(valentity);
                        }

                        ////                                      //Get asscendants
                        //List<AscentityAscendantsEntityDB> darrascentity = context.Ascendants.Where(ascentity =>
                        //    ascentity.intPkElement == intPkResource).ToList();

                        ////                                      //Delete the ascendants.
                        //foreach (AscentityAscendantsEntityDB ascentity in darrascentity)
                        //{
                        //    context.Ascendants.Remove(ascentity);
                        //}

                        context.Element.Remove(eleentity);

                        //                                      //if the ascendats is not delete, 
                        //                                      //    the save changes trown a exception
                        //                                      //    beacuse the table ascendants
                        //                                      //    has a foring key with the resource.

                        //                                      //The save changes is a transaction.
                        context.SaveChanges();
                        intStatus = 200;
                        strUserMessage = "Success.";
                        strDevMessage = "";
                    }

                }
                catch (Exception e)
                {
                    intStatus = 402;
                    strUserMessage = "Something is wrong.";
                    strDevMessage = "A exception ocurred.";
                }
            }

            obj = new
            {

            };

            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
               obj);
            IActionResult aresult = base.Ok(respjson1);

            return aresult;
        }

        //------------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]/")]
        public IActionResult TestSaveChangesWithTryCatch2(
            //                                              //Use correct of save changes.

            //                                              //Create a new resource and delete this resource 
            //                                              //    for test.

            int intPkResource
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;

            if (
                intPkResource > 0
                )
            {
                try
                {
                    //                                      //Connect to DB.
                    Odyssey2Context context = new Odyssey2Context();

                    TestController.subTestSaveChangesWithTryCatch2(intPkResource, context, ref intStatus, ref strUserMessage, 
                        ref strDevMessage);

                    //String strNextInstruction = "Next instruction.";
                }
                catch (Exception e)
                {
                    intStatus = 402;
                    strUserMessage = "Something is wrong.";
                    strDevMessage = e.Message;
                }
            }

            obj = new
            {

            };

            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
               obj);
            IActionResult aresult = base.Ok(respjson1);

            return aresult;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subTestSaveChangesWithTryCatch2(
            int intPkResource_I, 
            Odyssey2Context context_I, 
            ref int intStatus_IO, 
            ref String strUserMessage_IO, 
            ref String strDevMessage_IO
            )
        {
            EleentityElementEntityDB eleentity = context_I.Element.FirstOrDefault(ele => ele.intPk == intPkResource_I);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Resource does not found.";
            if (
                //                                      //Exist this resoruce.
                eleentity != null
                )
            {
                //                                      //Get the calculations that are related to the res.
                List<CalentityCalculationEntityDB> darrcalentity = context_I.Calculation.Where(calentity =>
                    calentity.intnPkResource == intPkResource_I).ToList();

                //                                      //Delete the calculations.
                foreach (CalentityCalculationEntityDB calentity in darrcalentity)
                {
                    context_I.Calculation.Remove(calentity);
                }

                //                                      //Get the values that are related to the res.
                List<ValentityValueEntityDB> darrvalentity = context_I.Value.Where(valentity =>
                    valentity.intPkElement == intPkResource_I).ToList();

                //                                      //Delete the values of the resource or template.
                foreach (ValentityValueEntityDB valentity in darrvalentity)
                {
                    context_I.Value.Remove(valentity);
                }

                ////                                      //Get asscendants
                //List<AscentityAscendantsEntityDB> darrascentity = context_I.Ascendants.Where(ascentity =>
                //    ascentity.intPkElement == intPkResource_I).ToList();

                ////                                      //Delete the ascendants.
                //foreach (AscentityAscendantsEntityDB ascentity in darrascentity)
                //{
                //    context_I.Ascendants.Remove(ascentity);
                //}

                context_I.Element.Remove(eleentity);

                //                                      //if the ascendats is not delete, 
                //                                      //    the save changes trown a exception
                //                                      //    beacuse the table ascendants
                //                                      //    has a foring key with the resource.

                //                                      //The save changes is a transaction.
                context_I.SaveChanges();
                intStatus_IO = 200;
                strUserMessage_IO = "Success.";
                strDevMessage_IO = "";
            }
        }

        //------------------------------------------------------------------------------------------------------------------
    }

    //======================================================================================================================
}
/*END-TASK*/
