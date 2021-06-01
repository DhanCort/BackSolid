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

//                                                          //AUTHOR: Towa (DTC - Daniel Texon).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: September 04, 2020.

namespace Odyssey2Backend.Controllers
{
    //                                                      //To obtain the strPrintshopId from token:
    //                                                      //  var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
    //                                                      //  String strPrintshopId = idClaim.Value;

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //==================================================================================================================
    [Route("Odyssey2/[controller]")]
    public class ProductController : Controller
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
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]/")]
        public IActionResult GetFromWisnet(
            //                                              //PURPOSE:
            //                                              //Get products from Wisnet related to a printshop.

            //                                              //URL: http://localhost/Odyssey2/Product
            //                                              //      /GetFromWisnet?intnOrderType=2&strCategory=Envelopes
            //                                              //Method: GET.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intnOrderType":1,  
            //                                              //          "strCategory":"Category", 
            //                                              //          "strKeyWord":"annou",
            //                                              //          "intnCompanyId":353118,
            //                                              //          "intnBranchId":31908,
            //                                              //          "intnContactId":996307
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Get products from Wisnet for a printshop.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            //                                              //1 - Public Order Form.
            //                                              //2 - Private Order Form.
            //                                              //3 - Guided Order Form.
            [FromBody] JsonElement jsonFilter            
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if jsonFilter is not null or a value is not empty.
                !((int)jsonFilter.ValueKind == 7) &&
                !((int)jsonFilter.ValueKind == 0) &&
                jsonFilter.TryGetProperty("intnOrderType", out json) &&
                jsonFilter.TryGetProperty("strCategory", out json) &&
                jsonFilter.TryGetProperty("strKeyword", out json) &&
                jsonFilter.TryGetProperty("intnCompanyId", out json) &&
                jsonFilter.TryGetProperty("intnBranchId", out json) &&
                jsonFilter.TryGetProperty("intnContactId", out json)
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
                    //                                      //Get data from body.
                    int? intnOrderType = null;
                    if (
                        (int)jsonFilter.GetProperty("intnOrderType").ValueKind == 4
                        )
                    {
                        intnOrderType = jsonFilter.GetProperty("intnOrderType").GetInt32();
                    }

                    String strCategory = null;
                    if (
                        (int)jsonFilter.GetProperty("strCategory").ValueKind == 3
                        )
                    {
                        strCategory = jsonFilter.GetProperty("strCategory").GetString();
                    }

                    String strKeyword = null;
                    if (
                        (int)jsonFilter.GetProperty("strKeyword").ValueKind == 3
                        )
                    {
                        strKeyword = jsonFilter.GetProperty("strKeyword").GetString();
                    }

                    int? intnCompanyId = null;
                    if (
                        (int)jsonFilter.GetProperty("intnCompanyId").ValueKind == 4
                        )
                    {
                        intnCompanyId = jsonFilter.GetProperty("intnCompanyId").GetInt32();
                    }

                    int? intnBranchId = null;
                    if (
                        (int)jsonFilter.GetProperty("intnBranchId").ValueKind == 4
                        )
                    {
                        intnBranchId = jsonFilter.GetProperty("intnBranchId").GetInt32();
                    }

                    int? intnContactId = null;
                    if (
                        (int)jsonFilter.GetProperty("intnContactId").ValueKind == 4
                        )
                    {
                        intnContactId = jsonFilter.GetProperty("intnContactId").GetInt32();
                    }

                    intStatus = 402;
                    strUserMessage = "Something is worng.";
                    strDevMessage = "Invalid order type.";
                    if (
                        intnOrderType == null ||
                        (((int)intnOrderType >= 1) && ((int)intnOrderType <= 3))
                        )
                    {
                        try
                        {
                            List<Prodtypjson2ProductTypeJson2> darrprodtypjson2 = new List<Prodtypjson2ProductTypeJson2>();

                            /*CASE*/
                            if (
                                //                              //Order forms.
                                intnOrderType == null
                                )
                            {
                                darrprodtypjson2 = ps.darrprodtypjson2GetProducts(strCategory, strKeyword, ps,
                                    ref intStatus, ref strUserMessage, ref strDevMessage);
                            }
                            else if (
                                //                              //Public order forms.
                                intnOrderType == 1
                                )
                            {
                                darrprodtypjson2 = ps.darrprodtypjson2GetPublicProducts(strCategory, strKeyword, ps,
                                    ref intStatus, ref strUserMessage, ref strDevMessage);
                            }
                            else if (
                                //                              //Private order forms.
                                intnOrderType == 2
                                )
                            {
                                darrprodtypjson2 = ps.darrprodtypjson2GetPrivateProducts(strCategory, strKeyword,
                                    intnCompanyId, intnBranchId, intnContactId, ps, ref intStatus, ref strUserMessage,
                                    ref strDevMessage);
                            }
                            else
                            {
                                darrprodtypjson2 = ps.darrprodtypjson2GetGuidedProducts(strCategory, strKeyword,
                                    ref intStatus, ref strUserMessage, ref strDevMessage);
                            }
                            /*END-CASE*/
                            obj = darrprodtypjson2.OrderBy(prodtyp => prodtyp.strTypeId);
                        }
                        catch (Exception ex)
                        {
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
    }
    //==================================================================================================================
}
/*END-TASK*/
