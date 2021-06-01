/*TASK RP.RESOURCE*/
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.Configuration;
using Odyssey2Backend.Alert;
using Odyssey2Backend.App;
using Odyssey2Backend.DB_Odyssey2;
using Odyssey2Backend.Job;
using Odyssey2Backend.JsonTemplates.Out;
using Odyssey2Backend.JsonTypes;
using Odyssey2Backend.PrintShop;
using Odyssey2Backend.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TowaStandard;
using Microsoft.AspNetCore.SignalR;
using Odyssey2Backend.Customer;
using System.IO;
using System.Text;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: January 20, 2020. 

namespace Odyssey2Backend.XJDF
{
    //=================================================================================================================
    public class ResResource
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTANTS.

        //                                                  //Group resource..
        public const String strGroup = "Group";

        //                                                  //Frecuency of rules from resource.
        public const String strOnce = "once";
        public const String strDaily = "daily";
        public const String strWeekly = "weekly";
        public const String strMonthly = "monthly";
        public const String strAnnually = "annually";

        public static readonly String[] arrstrWeekdays = { "sun", "mon", "tues", "wed", "thurs",
                                                         "fri", "sat"};

        public static readonly String[] arrstrMonths = { "Jan", "Feb", "Mar", "Apr", "May",
                                                         "Jun", "Jul", "Aug", "Sep", "Oct",
                                                         "Nov", "Dec"};

        public const String strDevice = "XJDFDevice";
        public const String strMiscConsumable = "XJDFMiscConsumable";
        public const String strComponent = "XJDFComponent";
        public const String strMedia = "XJDFMedia";
        public const String strTool = "XJDFTool";
        public const String strCustom = "Custom Resources";

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        private readonly int intPk_Z;
        public int intPk { get { return this.intPk_Z; } }

        private readonly String strName_Z;
        public String strName { get { return this.strName_Z; } }

        private readonly bool boolIsTemplate_Z;
        public bool boolIsTemplate { get { return this.boolIsTemplate_Z; } }

        private readonly RestypResourceType restemBelongsTo_Z;
        public RestypResourceType restypBelongsTo { get { return this.restemBelongsTo_Z; } }

        private readonly ResResource resInherited_Z;
        public ResResource resinherited { get { return this.resInherited_Z; } }

        private readonly bool? boolnIsCalendar_Z;
        public bool? boolnIsCalendar { get { return this.boolnIsCalendar_Z; } }

        private readonly bool? boolnIsAvailable_Z;
        public bool? boolnIsAvailable { get { return this.boolnIsAvailable_Z; } }

        private readonly bool? boolnCalendarIsChangeable_Z;
        public bool? boolnCalendarIsChangeable { get { return this.boolnCalendarIsChangeable_Z; } }

        private readonly bool? boolnCalendarIsInherited_Z;
        public bool? boolnCalendarIsInherited { get { return this.boolnCalendarIsInherited_Z; } }


        //--------------------------------------------------------------------------------------------------------------
        //                                                  //DYNAMIC VARIABLES.

        //                                                  //Dictionary of calculations.
        private Dictionary<int, CalCalculation> diccal_Z;
        public Dictionary<int, CalCalculation> diccal
        {
            get
            {
                this.subGetCalculationsFromDB(out this.diccal_Z);
                return this.diccal_Z;
            }
        }

        //                                                  //Current cost.
        private CostentityCostEntityDB costentityCurrent_Z;
        public CostentityCostEntityDB costentityCurrent
        {
            get
            {
                this.subGetCurrentCost(out this.costentityCurrent_Z);
                return this.costentityCurrent_Z;
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //SUPPORT METHODS TO DYNAMIC VARIABLES.
        //--------------------------------------------------------------------------------------------------------------
        private void subGetCalculationsFromDB(
            //                                              //Get all cal for this res from db.

            //                                              //Dic where the cal will be saved.
            out Dictionary<int, CalCalculation> diccal_O
            )
        {
            //                                              //Initialize the diccal.
            diccal_O = new Dictionary<int, CalCalculation>();

            //                                              //Create the context.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get calculations for this res.
            List<CalentityCalculationEntityDB> darrcalentity = (from cal in context.Calculation select cal).ToList();

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
                    calentity.intnPkElementElement, null, null, null, null, calentity.boolnFromThickness,
                    calentity.boolnIsBlock, calentity.boolnByArea);

                diccal_O.Add(cal.intPk, cal);
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public void subGetCurrentCost(
            Odyssey2Context context_I,
            //                                              //Get the current cost.
            out CostentityCostEntityDB costentity_O
            )
        {
            //                                              //Initialize the costentity variable.
            costentity_O = null;

            //                                              //Get all the costs for the resource.
            IQueryable<CostentityCostEntityDB> setcostentityCurrentMin = context_I.Cost.Where(cost =>
                    cost.intPkResource == this.intPk);
            List<CostentityCostEntityDB> darrcostentityCurrentMin = setcostentityCurrentMin.ToList();

            /*CASE*/
            if (
                //                                          //The cost exists and there is only one cost
                darrcostentityCurrentMin.Count() == 1
                )
            {
                //                                          //We will back the cost.
                costentity_O = darrcostentityCurrentMin[0];
            }
            else if (
               //                                           //The cost exists and there is more than one cost.
               darrcostentityCurrentMin.Count() > 1
               )
            {
                //                                          //Sort list of costs.
                darrcostentityCurrentMin.Sort();
                //                                          //Back the last resource's cost.
                costentity_O = darrcostentityCurrentMin[darrcostentityCurrentMin.Count() - 1];
            }
            /*END-CASE*/
        }

        //--------------------------------------------------------------------------------------------------------------
        public void subGetCurrentCost(
            //                                              //Get the current cost.
            out CostentityCostEntityDB costentity_O
            )
        {
            //                                              //Initialize the costentity variable.
            costentity_O = null;

            //                                              //Establish connection.
            Odyssey2Context context_I = new Odyssey2Context();

            //                                              //Get all the costs for the resource.
            IQueryable<CostentityCostEntityDB> setcostentityCurrentMin = context_I.Cost.Where(cost =>
                    cost.intPkResource == this.intPk);
            List<CostentityCostEntityDB> darrcostentityCurrentMin = setcostentityCurrentMin.ToList();

            /*CASE*/
            if (
                //                                          //The cost exists and there is only one cost
                darrcostentityCurrentMin.Count() == 1
                )
            {
                //                                          //We will back the cost.
                costentity_O = darrcostentityCurrentMin[0];
            }
            else if (
               //                                           //The cost exists and there is more than one cost.
               darrcostentityCurrentMin.Count() > 1
               )
            {
                //                                          //Sort list of costs.
                darrcostentityCurrentMin.Sort();
                //                                          //Back the last resource's cost.
                costentity_O = darrcostentityCurrentMin[darrcostentityCurrentMin.Count() - 1];
            }
            /*END-CASE*/
        }

        //--------------------------------------------------------------------------------------------------------------
        public bool subFunIsMediaTypeMediaUnitRoll(
            //                                              //If the resource is Media Type Roll, 
            //                                              //    return the attribute With and UnitOfMeasurement
            //                                              //    from resource.

            out String strWidthUnit_O,
            out String strUnitOfMeasurement_O
            )
        {
            strWidthUnit_O = null;
            strUnitOfMeasurement_O = null;
            bool boolIsMediaTypeMediaUnitRoll = false;

            Odyssey2Context context = new Odyssey2Context();

            RestypResourceType restyp = this.restypBelongsTo;

            if (
                restyp.strCustomTypeId == "XJDFMedia"
                )
            {
                //                                          //Attributes for Resource LengthUnit;
                List<AttrentityAttributeEntityDB> darrattrResource =
                    (from attrentity in context.Attribute
                     join attretentity in context.AttributeElementType
                     on attrentity.intPk equals attretentity.intPkAttribute
                     where attretentity.intPkElementType == restyp.intPk
                     select attrentity).ToList();

                //                                          //Get the MediaUnit attribute.
                AttrentityAttributeEntityDB attrentityMediaUnit = darrattrResource.FirstOrDefault(a =>
                    a.strXJDFName == "MediaUnit" || a.strCustomName == "XJDFMediaUnit");

                //                              //Get the unit attribute.
                AttrentityAttributeEntityDB attrentityUnit = darrattrResource.FirstOrDefault(a =>
                    a.strXJDFName == "Unit");

                AttrentityAttributeEntityDB attrentityWidthUnit = darrattrResource.FirstOrDefault(a =>
                    a.strXJDFName == "WidthUnit");

                if (
                    attrentityMediaUnit != null && attrentityUnit != null && attrentityWidthUnit != null
                    )
                {
                    //                                      //Get the current ascendants.
                    AscentityAscendantsEntityDB darrascentityMediaUnit = context.Ascendants.FirstOrDefault(asc =>
                        asc.intPkElement == this.intPk && 
                        asc.strAscendants.EndsWith("" + attrentityMediaUnit.intPk));

                    if (
                        darrascentityMediaUnit != null
                        )
                    {
                        String strValueMediaUnit = context.Value.FirstOrDefault(val =>
                                            val.intPkAttribute == attrentityMediaUnit.intPk &&
                                            val.intPkElement == this.intPk).strValue;
                        if (
                            strValueMediaUnit == "Roll"
                            )
                        {
                            boolIsMediaTypeMediaUnitRoll = true;

                            //                              //Get the UnitOfMeasurement.
                            strUnitOfMeasurement_O = context.Value.FirstOrDefault(val =>
                                            val.intPkAttribute == attrentityUnit.intPk &&
                                            val.intPkElement == this.intPk).strValue;

                            ValentityValueEntityDB valentityWithUnit = context.Value.FirstOrDefault(val =>
                                            val.intPkAttribute == attrentityWidthUnit.intPk &&
                                            val.intPkElement == this.intPk);

                            if (
                                valentityWithUnit != null
                                )
                            {
                                //                          //Get the WidhLenght.
                                strWidthUnit_O = valentityWithUnit.strValue;
                            }
                            else
                            {
                                //                          //Get the dimesionUnit.
                                AttrentityAttributeEntityDB attrentityDimUnit = darrattrResource.FirstOrDefault(a =>
                                    a.strXJDFName == "DimensionsUnit");

                                strWidthUnit_O = context.Value.FirstOrDefault(val =>
                                            val.intPkAttribute == attrentityDimUnit.intPk &&
                                            val.intPkElement == this.intPk).strValue;
                            }
                        }
                    }
                }
            }

            return boolIsMediaTypeMediaUnitRoll;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //--------------------------------------------------------------------------------------------------------------
        public ResResource(
            //                                              //Primary key of the resource.
            int intPk_I,
            //                                              //Resource Name
            String strResourceName_I,
            bool boolIsTemplate_I,
            RestypResourceType restemBelongsTo_I,
            ResResource resInherited_I,
            bool? boolnIsCalendar_I,
            bool? boolnIsAvailable_I,
            bool? boolnCalendarIsChangeable_I,
            bool? boolnCalendarIsInherited_I
            )
        {
            this.intPk_Z = intPk_I;
            this.strName_Z = strResourceName_I;
            this.boolIsTemplate_Z = boolIsTemplate_I;
            this.restemBelongsTo_Z = restemBelongsTo_I;
            this.resInherited_Z = resInherited_I;
            this.boolnIsCalendar_Z = boolnIsCalendar_I;
            this.boolnIsAvailable_Z = boolnIsAvailable_I;
            this.boolnCalendarIsChangeable_Z = boolnCalendarIsChangeable_I;
            this.boolnCalendarIsInherited_Z = boolnCalendarIsInherited_I;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //TRANSFORMATION METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public static void subAddTemplateAndResourceToPrintshopFromFile(
            //                                              //Add Template and resource from template.

            PsPrintShop ps_I,
            String strFileName_I,
            String strAccountNumber_I,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Find printshop's Paper account.
            AccentityAccountEntityDB accentityPaper = context_M.Account.FirstOrDefault(acc =>
                acc.intPkPrintshop == ps_I.intPk && acc.boolAvailable == true &&
                acc.strNumber == strAccountNumber_I);

            if (
                accentityPaper == null
                )
            {
                intStatus_IO = 402;
                strUserMessage_IO = "Account not exist or not available:" + strAccountNumber_I;
                strDevMessage_IO = "Account not exist or not available:" + strAccountNumber_I;

                throw new Exception("Something error. Account not exist or not available:" + strAccountNumber_I);
            }
            int intPkAccount = accentityPaper.intPk;

            //                                              //Get Template and resources.
            PathX syspathA = DirectoryX.GetCurrent().GetPath().AddName("Z_ResourceLoading");
            PathX syspath = syspathA.AddName(strFileName_I);
            FileInfo sysfile = FileX.New(syspath);

            String[] arrRowData = sysfile.ReadAll();

            /*REPEAT-WHILE*/
            int intLength = arrRowData.Length;

            //                                              //The position zero is the header and it is not 
            //                                              //    procesed.
            //                                              //Get Data Row Header.
            String strDataHeader = arrRowData[0].ToString();

            RestempattrjsonResourceTemplateAttributeJson restempattrjsonHeader;
            ResResource.getFieldsDataJson(strDataHeader, out restempattrjsonHeader);

            String strTypeOfResource = "XJDFMedia";
            int intPerQuantityOfThePrice = 1000;

            //                                              //Init the list of template added to db.
            List<TempjsonTemplateJson> darrtempTemplateAdded = new List<TempjsonTemplateJson>();

            int intI = 1;
            while (intI < intLength)
            {
                //                                          //Get Data Row.
                String strData = arrRowData[intI].ToString();


                RestempattrjsonResourceTemplateAttributeJson restempattrjson;
                ResResource.getFieldsDataJson(strData, out restempattrjson);

                bool boolIsTemplate = false;
                String strNameTemplate = restempattrjson.strTemplateName;
                String strAliasTemplate = restempattrjson.strAliasTemplate;
                String strNameResource = restempattrjson.strResourceName;
                String strPriceResourceOrTemplate = restempattrjson.strPricePerThousand;
                int intPkResOrTempAdded = 0;

                //                                          //Init the list of attributes.
                List<Attrjson5AttributeJson5> darrattrjsonAttributesBuild = new List<Attrjson5AttributeJson5>();
                
                /*CASE*/
                if (
                    //                                      //It is a resource.
                    !String.IsNullOrEmpty(strNameResource) &&
                    String.IsNullOrEmpty(strNameTemplate)
                    )
                {
                    boolIsTemplate = false;
                    int? intnPkInheritFrom;
                    //                                      //Build the list of attributes for the resource.
                    darrattrjsonAttributesBuild = ResResource.darrattrjson5GetAttributesForResourceOrTemplate(strTypeOfResource, restempattrjsonHeader,
                        restempattrjson, darrtempTemplateAdded, out intnPkInheritFrom,  context_M);

                    int intPkResource =
                        ps_I.intAddResource(strNameResource, strTypeOfResource,
                        "Sheets", false, ref darrattrjsonAttributesBuild, intnPkInheritFrom, null, context_M);

                    if (
                        intPkResource == 0
                        )
                    {
                        intStatus_IO = 403;
                        strUserMessage_IO = "i: "+intI+" Resource not added:" + strNameResource;
                        strDevMessage_IO = "i: " + intI + " Resource not added:" + strNameResource;

                        throw new Exception("i: " + intI + " Something error. Resource not added:" + strNameResource);
                    }

                    intPkResOrTempAdded = intPkResource;
                }
                else if (
                    //                                      //It is a Template.
                    String.IsNullOrEmpty(strNameResource) &&
                    !String.IsNullOrEmpty(strNameTemplate)
                    )
                {
                    boolIsTemplate = true;

                    int? intnPkInheritFrom;
                    //                                      //Build the list of attributes for the template.
                    darrattrjsonAttributesBuild = ResResource.darrattrjson5GetAttributesForResourceOrTemplate(strTypeOfResource, restempattrjsonHeader,
                        restempattrjson, darrtempTemplateAdded, out intnPkInheritFrom, context_M);

                    //                                      //Create the template.
                    int intPkTemplate = ps_I.intAddTemplate(strNameTemplate, strTypeOfResource, "Sheets", false,
                        ref darrattrjsonAttributesBuild, intnPkInheritFrom, context_M);

                    if (
                        intPkTemplate == 0
                        )
                    {
                        intStatus_IO = 404;
                        strUserMessage_IO = "i: " + intI + " Template not added:" + strNameTemplate;
                        strDevMessage_IO = "i: " + intI + " Template not added:" + strNameTemplate;

                        throw new Exception("i: " + intI + " Something error with the template" + strNameTemplate);
                    }

                    intPkResOrTempAdded = intPkTemplate;
                }
                else
                {
                    intStatus_IO = 405;
                    strUserMessage_IO = "This Element at the position: " + intI +
                        " is not valid.";
                    strDevMessage_IO = "This Element at the position: " + intI +
                        " is not valid.";

                    throw new Exception("i: " + intI + " Something error: This Element at the position: " + intI + 
                        " is not valid." );
                }
                /*END-CASE*/

                //                                          //Add cost to Resource or template.

                bool? boolnPriceInheritedFromTemplate;
                double? numnPriceResourceOrTemplate = null;
                TempjsonTemplateJson tempjsonTemplateInherit;

                //                                          //Verify if the cost is inherited from any template.
                ResResource.subfunValueInheritedFromTemplate(strPriceResourceOrTemplate, darrtempTemplateAdded,
                    out boolnPriceInheritedFromTemplate, out tempjsonTemplateInherit);

                if (
                    //                                      //Cost inherited from any template.
                    boolnPriceInheritedFromTemplate == true
                    )
                {
                    //                                      //Inherited cost from template.

                    ResResource res = ResResource.resFromDB(context_M, intPkResOrTempAdded, boolIsTemplate);
                    numnPriceResourceOrTemplate = tempjsonTemplateInherit.numnPrice;

                    ResResource.subAddInheritedCost(tempjsonTemplateInherit.intPk, res,
                        numnPriceResourceOrTemplate, intPerQuantityOfThePrice,
                        null, null, intPkAccount, null, null,
                        true, false, ps_I, context_M, ref intStatus_IO, ref strUserMessage_IO,
                        ref strDevMessage_IO);
                }
                else if (
                    boolnPriceInheritedFromTemplate == false
                    )
                {
                    //                                      //Get the price.
                    numnPriceResourceOrTemplate = String.IsNullOrEmpty(strPriceResourceOrTemplate) == true ?
                        null : (double?)strPriceResourceOrTemplate.ParseToNum();

                    if (
                        numnPriceResourceOrTemplate != null
                        )
                    {
                        ResResource.subAddCost(intPkResOrTempAdded, intPerQuantityOfThePrice,
                        numnPriceResourceOrTemplate, null, null, intPkAccount, null, false, ps_I, false, context_M,
                         ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);
                    }
                }

                if (
                    //                                      //Template was added.
                    boolIsTemplate
                    )
                {
                    TempjsonTemplateJson tempjson = new TempjsonTemplateJson(intPkResOrTempAdded, strNameTemplate,
                        strAliasTemplate, numnPriceResourceOrTemplate);
                    darrtempTemplateAdded.Add(tempjson);
                }

                intI = intI + 1;
            }

            intStatus_IO = 200;
            strUserMessage_IO = "Sucessfully.";
            strDevMessage_IO = "Sucessfully.";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void getFieldsDataJson(
            //                                              //Get fields from String.
            //                                              //The first, second and last item, these are item own of 
            //                                              //    resource or template.
            //                                              //The items at middle, these are attributes XJDF.

            String strData_I,
            out RestempattrjsonResourceTemplateAttributeJson restempattrjson_O
            )
        {
            //                                              //Init the list of attributes.
            List<String> darrstrValuesFromAttribute = new List<string>();

            //                                              //Get the list of data.
            List<String> darrdata = strData_I.Split('|').ToList();

            //                                              //Get Template name.
            String strTemplateName = darrdata[0];

            //                                              //Get Alias Template
            String strAliasTemplate = darrdata[1];

            //                                              //Get Resource Name.
            String strResourceName = darrdata[2];

            //                                              //Get price per thousand.
            String strPricePerThousand = darrdata[darrdata.Count() - 1 ];

            //                                              //Remove Template name.
            darrdata.RemoveAt(0);
            //                                              //Remove AliasTemplate
            darrdata.RemoveAt(0);
            //                                              //Remove Resource Name.
            darrdata.RemoveAt(0);
            //                                              //Remove priceperthousand.
            darrdata.RemoveAt(darrdata.Count() - 1);

            //                                              //start working with attributes XJDF.
            foreach (String strAttribute in darrdata)
            {
                darrstrValuesFromAttribute.Add(strAttribute);
            }            

            restempattrjson_O =
                new RestempattrjsonResourceTemplateAttributeJson (strTemplateName, strAliasTemplate, strResourceName,
                darrstrValuesFromAttribute, strPricePerThousand);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static List<Attrjson5AttributeJson5> darrattrjson5GetAttributesForResourceOrTemplate(
            //                                              //Build the attributes for the resource or
            //                                              //    or the template.

            String strTypeOfResource_I,
            RestempattrjsonResourceTemplateAttributeJson restempattrjsonHeader_I,
            RestempattrjsonResourceTemplateAttributeJson restempattrjsonInfoResOrTemplate_I,
            //                                              //List of template added.
            List<TempjsonTemplateJson> darrtempTemplateAdded_I, 
            out int? intnPkInheritFrom_O,
            Odyssey2Context context_M
            )
        {
            intnPkInheritFrom_O = null;
            List<Attrjson5AttributeJson5> darrjsonAttributeResource = new List<Attrjson5AttributeJson5>();

            //                                              //Get Attributes names.
            List<String> darrstrAttributesName = restempattrjsonHeader_I.darrstrValuesFromAttribute.ToList();

            //                                              //Get values of the attributes.
            List<String> darrstrValues = restempattrjsonInfoResOrTemplate_I.darrstrValuesFromAttribute.ToList();

            int intI = 0;
            int intLength = darrstrValues.Count();
            
            /*WHILE-DO*/
            while (
                //                                          //Take each value of this resource or template.
                intI < intLength
                )
            {
                //                                          //Take the values.
                String strValue = darrstrValues[intI];

                bool? boolnAttributeValueInheritedFromTemplate;
                TempjsonTemplateJson tempjsonTemplateInherit;

                //                                          //Verify if the attribute value
                //                                          //    is inherited from any template.
                ResResource.subfunValueInheritedFromTemplate(strValue, darrtempTemplateAdded_I,
                    out boolnAttributeValueInheritedFromTemplate, out tempjsonTemplateInherit);

                if (
                    //                                      //Attribute value inherited from any template.
                    boolnAttributeValueInheritedFromTemplate == true
                    )
                {
                    //                                      //Identify the attribute to inherit.
                    String strXJDFInherit = darrstrAttributesName[intI];

                    ResResource.subCreateInheritedAttribute(tempjsonTemplateInherit.intPk, strTypeOfResource_I,
                        strXJDFInherit, false, ref darrjsonAttributeResource, context_M);

                    intnPkInheritFrom_O = tempjsonTemplateInherit.intPk;
                }
                else if(boolnAttributeValueInheritedFromTemplate == false)
                {
                    //                                      //Get Attribute name.
                    String strXJDFAttributeOwn = darrstrAttributesName[intI];

                    //                                      //It is attribute own of the resource.
                    PsPrintShop.subCreateAttribute(strTypeOfResource_I, strXJDFAttributeOwn,
                    strValue, null, true, ref darrjsonAttributeResource, context_M);
                }

                intI = intI + 1;
            }

            return darrjsonAttributeResource;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subfunValueInheritedFromTemplate(
            //                                              //Verify if the value inherited from any
            //                                              //    attribute of the template.

            //                                              //strValue_I can be:
            //                                              //    1.- Attribute value,
            //                                              //    2.- Cost of the resource.
            String strValue_I,
            //                                              //List of template added.
            List<TempjsonTemplateJson> darrtempTemplateAdded_I,

            //                                              //OutputsVar.
            out bool? boolnAttributeValueOrCostInheritedFromTemplate_O,
            out TempjsonTemplateJson tempjsonTemplateInherit_O
            )
        {
            //                                              //Init the var Output.
            boolnAttributeValueOrCostInheritedFromTemplate_O = null;
            tempjsonTemplateInherit_O = null;

            if (
                //                                          //There is a value.
                !String.IsNullOrEmpty(strValue_I)
                )
            {

                if (
                    (
                        //                                  //Init and end with {TemplateName}
                        strValue_I.StartsWith('{') &&
                        strValue_I.EndsWith('}') &&
                        strValue_I.Length > 2
                    )
                    ||
                    (
                        //                                  //Value not inherited from template.
                        !strValue_I.StartsWith('{') &&
                        !strValue_I.EndsWith('}')
                    )
                    )
                {
                    if (
                        //                                  //Values Inherited from template.
                        strValue_I.StartsWith('{')
                        )
                    {
                        String strAliasTemplate_O = strValue_I.ToString();

                        //                                  //Remove the first and last character.
                        strAliasTemplate_O = strAliasTemplate_O.Substring(1, strAliasTemplate_O.Length - 1);

                        strAliasTemplate_O = strAliasTemplate_O.Substring(0, strAliasTemplate_O.Length - 1);
                        boolnAttributeValueOrCostInheritedFromTemplate_O = true;

                        //                                  //Find the pk of the template.
                        TempjsonTemplateJson tempjson = darrtempTemplateAdded_I.FirstOrDefault(temp =>
                            temp.strAliasTemplate == strAliasTemplate_O);

                        if (
                            //                              //Find the template.
                            tempjson != null
                            )
                        {
                            tempjsonTemplateInherit_O = tempjson;
                        }
                        else
                        {
                            throw new Exception("Something error. Template not loaded in " +
                                "memory:" + strAliasTemplate_O);
                        }
                    }
                    else
                    {
                        boolnAttributeValueOrCostInheritedFromTemplate_O = false;
                    }
                }
                else
                {
                    throw new Exception("Something error. Invalid format:" + strValue_I);
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subCreateInheritedAttribute(
            //                                              //Inherited attribute from a template.

            int intPKTemplateToInherited_I, 
            String strTypeOfResource_I,
            String strCustomName_I,
            bool boolChangeable_I,
            ref List<Attrjson5AttributeJson5> darrjsonAttribute_M, 
            Odyssey2Context context_M
            )
        {
            ResResource res = ResResource.resFromDB(context_M, intPKTemplateToInherited_I, true);

            int intPkAttribute = ResResource.intGetPkAttribute(strTypeOfResource_I,
                strCustomName_I, context_M);

            Attrjson3AttributeJson3 attrjson3 = res.attrjson3Get(intPkAttribute, context_M);

            if (
                attrjson3 != null
                )
            {
                Attrjson5AttributeJson5 attrjsonInherited = new Attrjson5AttributeJson5(
                "" + intPkAttribute, attrjson3.strValue, attrjson3.intValuePk, false, null);

                darrjsonAttribute_M.Add(attrjsonInherited);
            }  
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static int intGetPkAttribute(
            //                                              //GetPkAttribute.

            String strType_I,
            String strCustomName_I,
            Odyssey2Context context_I
            )
        {
            int intPkAttribute = 0;

            //                                              //Find type.
            EtentityElementTypeEntityDB etentityType = context_I.ElementType.FirstOrDefault(et =>
                et.strCustomTypeId == strType_I && et.intPrintshopPk == null &&
                et.strAddedBy == "XJDF2.0" && et.strResOrPro == EtElementTypeAbstract.strResource);

            //                                              //Attributes for this type, this name, no printshop.
            List<int> darrintPkAttributes = (
                from attr in context_I.Attribute
                join attret in context_I.AttributeElementType
                on attr.intPk equals attret.intPkAttribute
                where attret.intPkElementType == etentityType.intPk &&
                attr.strCustomName == strCustomName_I &&
                attr.strScope == "XJDF2.0"
                select attr.intPk).ToList();

            if (
                darrintPkAttributes.Count == 1
                )
            {
                intPkAttribute = darrintPkAttributes[0];
            }

            return intPkAttribute;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static int subAdd(
            //                                              //Add a resource or template (element) to the database and
            //                                              //      the values of the attributes that describe it.

            //                                              //Name of the attribute.
            String strName_I,
            //                                              //Unit of the attribute.
            String strUnit_I,
            //                                              // To know if the resource allow decimals
            bool? boolnIsDecimal_I,
            //                                              //Type which it belongs to.
            RestypResourceType restypBelongsTo_I,
            //                                              //Printshop id.
            String strPrintshopId_I,
            //                                              //It is a template or a resource.
            bool boolIsTemplate_I,
            //                                              //Template wich this inherited.
            int? intnPkDad_I,
            //                                              //Cost inherited or set.
            double? numnCost_I,
            //                                              //Cost quantity.
            double? numnQuantity_I,
            //                                              //Cost min.
            double? numnMin_I,
            //                                              //Cost block.
            double? numnBlock_I,
            //                                              //Account.
            int? intnPkAccount_I,
            //                                              //Hourly rate.
            double? numnHourlyRate_I,
            //                                              //Is Area inherited.
            bool? boolnArea_I,
            //                                              //Is cost inherited.
            bool? boolnCostInherited_I,
            //                                              //Is cost changeable.
            bool? boolnCostChangeable_I,
            //                                              //Unit of measurement.
            String strUnitInherited_I,
            //                                              //Is unit inherited.
            bool? boolnUnitInherited_I,
            //                                              //Is unit changeable.
            bool? boolnUnitChangeable_I,
            //                                              //Type of availability - calendarized.
            bool? boolnCalendarized_I,
            //                                              //Is availabily inherited.
            bool? boolnAvailabilityInherited_I,
            //                                              //Is availability changeable.
            bool? boolnAvailabilityChangeable_I,
            Odyssey2Context context_M,
            //                                              //Attributes that define the resource.
            //                                              //  {
            //                                              //      "strAscendant":"3|6|9",
            //                                              //      "strValue":"Valor1",
            //                                              //      "intnInheritedValuePk":15,
            //                                              //      "boolChangeable":false,
            //                                              //      "intPkValue":1
            //                                              //  }
            ref Attrjson5AttributeJson5[] arrattrjson5_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO,
            out Pejson1PathElementJson1 pejson_O
            )
        {
            int intPkElementAdded = 0;

            //                                              //Get psprintshop.
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);

            intStatus_IO = 401;
            strUserMessage_IO = "Invalid type.";
            strDevMessage_IO = "";
            pejson_O = null;

            //                                              //The type is sent as ref, because it can be changed from
            //                                              //      the XJDF to the clone.
            RestypResourceType restypBelongsTo = restypBelongsTo_I;
            if (
                ResResource.boolIsValidType(ps, context_M, ref restypBelongsTo)
                )
            {
                intStatus_IO = 402;
                strUserMessage_IO = "Invalid name or unit.";
                if (
                    ResResource.boolIsValidResource(strName_I, restypBelongsTo, intnPkDad_I,
                        arrattrjson5_M, context_M, ref strUnit_I, ref strUserMessage_IO)
                    )
                {
                    intStatus_IO = 403;
                    strUserMessage_IO = "Invalid inheritance data.";
                    if (
                        ResResource.boolIsInheritanceDataValid(numnCost_I, numnQuantity_I, numnMin_I, intnPkAccount_I,
                            numnHourlyRate_I, boolnArea_I, strUnit_I, strUnitInherited_I, boolnCalendarized_I, restypBelongsTo, 
                            intnPkDad_I, boolnCostInherited_I, boolnCostChangeable_I, boolnUnitInherited_I, 
                            boolnUnitChangeable_I, boolnAvailabilityInherited_I, boolnAvailabilityChangeable_I,
                            restypBelongsTo.strClassification, context_M)
                        )
                    {
                        intStatus_IO = 404;
                        strUserMessage_IO = "Invalid attributes.";
                        if (
                            (arrattrjson5_M != null &&
                            ResResource.boolHasValidAttributes(arrattrjson5_M, restypBelongsTo, context_M)) ||
                            arrattrjson5_M == null
                            )
                        {
                            //                              //Default availability data.
                            bool? boolnIsCalendar = false;
                            bool? boolnIsAvailable = true;
                            /*CASE*/
                            if (
                                //                          //Set calendarized for Tool and Device.
                                RestypResourceType.boolIsDeviceToolOrCustom(restypBelongsTo)
                                )
                            {
                                boolnIsCalendar = true;
                                boolnIsAvailable = null;
                            }
                            else if (
                               !RestypResourceType.boolIsPhysical(restypBelongsTo.strClassification)
                               )
                            {
                                boolnIsCalendar = null;
                                boolnIsAvailable = null;
                            }
                            /*END-CASE*/

                            //                              //Add the resource.
                            EleentityElementEntityDB elentityAdded = new EleentityElementEntityDB
                            {
                                strElementName = strName_I,
                                intPkElementType = restypBelongsTo.intPk,
                                intnPkElementInherited = intnPkDad_I,
                                boolIsTemplate = boolIsTemplate_I,
                                boolnIsCalendar = boolnIsCalendar,
                                boolnIsAvailable = boolnIsAvailable,
                                strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                                strStartTime = Time.Now(ZonedTimeTools.timezone).ToString()
                            };
                            context_M.Element.Add(elentityAdded);
                            context_M.SaveChanges();

                            intPkElementAdded = elentityAdded.intPk;
                            ResResource res = ResResource.resFromDB(context_M, intPkElementAdded, boolIsTemplate_I);

                            //                              //Add the name attribute.
                            ResResource.subAddNameAttribute(restypBelongsTo, res, strName_I, context_M);

                            //                              //Add the unit attribute.
                            ResResource.subAddUnitAttribute(restypBelongsTo, res, strUnit_I, boolnIsDecimal_I,
                                boolnUnitChangeable_I, boolnUnitInherited_I, intnPkDad_I, context_M);

                            //                              //Add each attribute when receive it.
                            foreach (Attrjson5AttributeJson5 attrjson5 in arrattrjson5_M)
                            {
                                Attrjson5AttributeJson5 attrjson5ToPassAsRef = attrjson5;
                                ResResource.subAddOneAttribute(context_M, res, ref attrjson5ToPassAsRef);
                            }

                            //                              //Update inheritance data.
                            if (
                                intnPkDad_I != null &&
                                RestypResourceType.boolIsPhysical(restypBelongsTo.strClassification)
                                )
                            {
                                int intPkDad = (int)intnPkDad_I;

                                ResResource.subAddInheritedCost(intPkDad, res, numnCost_I, numnQuantity_I,
                                    numnMin_I, numnBlock_I, intnPkAccount_I, numnHourlyRate_I, boolnArea_I,
                                    boolnCostInherited_I, boolnCostChangeable_I, ps, context_M, ref intStatus_IO, ref strUserMessage_IO,
                                    ref strDevMessage_IO);

                                ResResource.subAddInheritedAvailability(intPkDad, intPkElementAdded,
                                    boolnCalendarized_I, boolnAvailabilityInherited_I,
                                    boolnAvailabilityChangeable_I, context_M);
                            }

                            intStatus_IO = 200;
                            strUserMessage_IO = "Success.";
                            if (
                                res.resinherited != null
                                )
                            {
                                pejson_O = new Pejson1PathElementJson1(res.resinherited.intPk, false);
                            }
                            else
                            {
                                pejson_O = new Pejson1PathElementJson1(res.restypBelongsTo.intPk, true);
                            }
                        }
                    }
                }
            }

            return intPkElementAdded;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static bool boolIsValidType(
            //                                              //Verify if the type for the resource/template is not null 
            //                                              //      and verify if the type is already a clone for the 
            //                                              //      printshop, if it is not then create the clone and
            //                                              //      add it to the printshop and update the type with the
            //                                              //      clone.

            //                                              //Printshop.
            PsPrintShop ps_I,
            Odyssey2Context context_M,
            //                                              //Type.
            ref RestypResourceType restyp_M
            )
        {
            bool boolValidType = false;

            if (
                restyp_M != null
                )
            {
                //                                          //Easy code.
                RestypResourceType restyp = restyp_M;

                //                                          //Search for a type with the same data.
                EtentityElementTypeEntityDB etentityResourceType = context_M.ElementType.FirstOrDefault(
                                et => et.strAddedBy == restyp.strAddedBy &&
                                et.strCustomTypeId == restyp.strCustomTypeId &&
                                et.intPrintshopPk == ps_I.intPk);

                int intStatus = -1;
                int intTypePk = restyp.intPk;
                /*CASE*/
                if (
                    //                                      //The printshop has not the clone.
                    (restyp.intPkPrintshop == null) &&
                    (etentityResourceType == null)
                    )
                {
                    Odyssey2.subAddTypeToPrintshop(restyp.intPk, ps_I, context_M, out intStatus, out intTypePk);
                }
                else if (
                    //                                      //The printshop has the clone but the object is about the 
                    //                                      //      generic type.
                    (restyp.intPkPrintshop == null) &&
                    (etentityResourceType != null)
                    )
                {
                    intStatus = 0;
                    intTypePk = etentityResourceType.intPk;
                }
                else if (
                    //                                      //The printshop has the clone and the object is that clone.
                    (restyp.intPkPrintshop != null) &&
                    (restyp.intPkPrintshop == ps_I.intPk)
                    )
                {
                    intStatus = 0;
                }
                /*END-CASE*/

                if (
                    intStatus == 0
                    )
                {
                    boolValidType = true;
                    restyp_M = (RestypResourceType)EtElementTypeAbstract.etFromDB(context_M, intTypePk);
                }
            }

            return boolValidType;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static bool boolIsValidResource(
            //                                              //Verify that the resource can be added.
            //                                              //1. The name should no be empty.
            //                                              //2. If physical, unit shouldn´t be empty.
            //                                              //3. Tha name doesn´t exit for the ps and type.
            //                                              //4. If dad, should be a template and form the same type.

            //                                              //Name of the resource to add.
            String strName_I,
            //                                              //Type of the resource.
            RestypResourceType restyp_I,
            //                                              //Pk of the inherited type or template.
            int? intnPkDad_I,
            Attrjson5AttributeJson5[] arrattrjson5_I,
            Odyssey2Context context_M,
            //                                              //Unit of the resource to add.
            ref String strUnit_IO,
            //                                              //Updated user message.
            ref String strUserMessage_IO
            )
        {
            bool boolValidResource = false;
            strUserMessage_IO = "Name cannot be empty";
            if (
                (strName_I != null) &&
                (strName_I != "")
                )
            {
                //                              //Attributes for Resource.
                List<AttrentityAttributeEntityDB> darrattrResource =
                    (from attrentity in context_M.Attribute
                     join attretentity in context_M.AttributeElementType
                     on attrentity.intPk equals attretentity.intPkAttribute
                     where attretentity.intPkElementType == restyp_I.intPk
                     select attrentity).ToList();

                //                              //Get the MediaUnit attribute.
                AttrentityAttributeEntityDB attrentityMediaUnit = darrattrResource.FirstOrDefault(a =>
                    a.strXJDFName == "MediaUnit" || a.strCustomName == "XJDFMediaUnit");

                //                              //Get the LengthUnit attribute.
                AttrentityAttributeEntityDB attrentityLength = darrattrResource.FirstOrDefault(a =>
                    a.strXJDFName == "LengthUnit" || a.strCustomName == "XJDFLengthUnit");

                //                              //Get the DimensionsUnit attribute.
                AttrentityAttributeEntityDB attrentityDimUnit = darrattrResource.FirstOrDefault(a =>
                    a.strXJDFName == "DimensionsUnit" || a.strCustomName == "XJDFDimensionsUnit");

                bool boolIsRoll = false;
                if (
                    attrentityMediaUnit != null && attrentityLength != null && attrentityDimUnit != null
                    ) 
                {
                    //                                  //Verify if exist the MediaUnit.
                    Attrjson5AttributeJson5 attrjson5MediaUnit = arrattrjson5_I.FirstOrDefault(attr =>
                        attr.strAscendant == "" + attrentityMediaUnit.intPk);

                    //                                  //Verify if exist the LengthUnit.
                    Attrjson5AttributeJson5 attrjson5LengthUnit = arrattrjson5_I.FirstOrDefault(attr =>
                        attr.strAscendant == "" + attrentityLength.intPk);

                    boolIsRoll = (attrjson5MediaUnit != null) ? attrjson5MediaUnit.strValue == "Roll" : false;

                    //                                  //Assig the new UnitOfMeasurement from LengthUnit..
                    strUnit_IO = restyp_I.strCustomTypeId == "XJDFMedia" &&
                        boolIsRoll && attrjson5LengthUnit != null && attrjson5LengthUnit.strValue != null ? 
                        attrjson5LengthUnit.strValue : strUnit_IO;

                    if (
                        strUnit_IO == null
                        )
                    {
                        //                                  //Verify if exist the DimUnit.
                        Attrjson5AttributeJson5 attrjson5DimUnit = arrattrjson5_I.FirstOrDefault(attr =>
                            attr.strAscendant == "" + attrentityDimUnit.intPk);

                        //                                  //Assig the new UnitOfMeasurement from DimensionUnit.
                        strUnit_IO = restyp_I.strCustomTypeId == "XJDFMedia" &&
                            boolIsRoll && attrjson5DimUnit != null && attrjson5DimUnit.strValue != null ?
                            attrjson5DimUnit.strValue : strUnit_IO;
                    }
                }

                strUserMessage_IO = "Unit cannot be empty if the resource is physical unless it is not physical.";
                if (
                    //                                      //Resource is physical and unit different of empty.
                    (RestypResourceType.boolIsPhysical(restyp_I.strClassification) && (strUnit_IO != null) &&
                    (strUnit_IO != "")) ||
                    //                                      //Resource is physical and unit empty.
                    (!RestypResourceType.boolIsPhysical(restyp_I.strClassification) && (strUnit_IO == null))
                    )
                {
                    bool boolIsParseableToInt = false;
                    bool boolIsParseableToNum = false;
                    if (
                        //                                  //Applies only to resources with unit of measurement.
                        RestypResourceType.boolIsPhysical(restyp_I.strClassification) &&
                        strUnit_IO != null &&
                        strUnit_IO != ""
                        )
                    {
                        //                                  //Validate Unit not only have numbers.
                        boolIsParseableToInt = strUnit_IO.IsParsableToInt();
                        boolIsParseableToNum = strUnit_IO.IsParsableToNum();
                    }

                    strUserMessage_IO = "Unit of Measurement cannot contain only numbers.";
                    if (
                        !boolIsParseableToInt &&
                        !boolIsParseableToNum
                        )
                    {
                        if (
                            !ResResource.boolResourceSameNameAndTypeAndSize(null, restyp_I, strName_I, arrattrjson5_I,
                                context_M, ref strUserMessage_IO)
                            )
                        {
                            EleentityElementEntityDB eleentityDad = null;
                            if (
                                (intnPkDad_I != null) &&
                                (intnPkDad_I > 0)
                                )
                            {
                                //                          //Look for the Dad, if any.
                                eleentityDad = context_M.Element.FirstOrDefault(ele => ele.intPk == intnPkDad_I);
                            }

                            strUserMessage_IO = "Dad is no valid.";
                            if (
                                //                          //No Dad.
                                (intnPkDad_I == null) ||
                                //                          //Dad found in database and it is a template.
                                ((intnPkDad_I != null) && (eleentityDad != null) && eleentityDad.boolIsTemplate &&
                                (eleentityDad.intPkElementType == restyp_I.intPk))
                                )
                            {
                                boolValidResource = true;
                            }
                        }
                    }
                }
            }
            return boolValidResource;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static bool boolResourceSameNameAndTypeAndSize(
            //                                              //Rerurns true if there is another resource with the same
            //                                              //      name for this type.

            //                                              //If resource is media, it can have the same name,
            //                                              //      but diferent dimensions.
            ResResource resToEdit_I,
            RestypResourceType restyp_I,
            String strName_I,
            Attrjson5AttributeJson5[] arrattrjson5_I,
            Odyssey2Context context_M,
            ref String strUserMessage_IO
            )
        {
            bool boolResourceSameNameAndTypeAndSize = false;

            List<EleentityElementEntityDB> darreleentityResourceSameNameAndType;
            RestypResourceType restyp;
            if (
                //                                          //Adding a resource.
                resToEdit_I == null
                )
            {
                restyp = restyp_I;

                darreleentityResourceSameNameAndType = context_M.Element.Where(ele =>
                    ele.intPkElementType == restyp.intPk &&
                    ele.strElementName == strName_I
                    ).ToList();
            }
            else
            {
                restyp = resToEdit_I.restypBelongsTo;

                //                                          //Editing a resource.
                darreleentityResourceSameNameAndType = context_M.Element.Where(ele =>
                    ele.intPkElementType == restyp.intPk &&
                    ele.strElementName == strName_I &&
                    ele.intPk != resToEdit_I.intPk
                    ).ToList();
            }

            if (
                darreleentityResourceSameNameAndType.Count > 0
                )
            {
                if (
                    !restyp.boolIsMedia()
                    )
                {
                    boolResourceSameNameAndTypeAndSize = true;
                    strUserMessage_IO = "A resource with the same name was found.";
                }
                else
                {
                    //                                  //Get the system restyp for media.
                    int intPkSystemMediaType = context_M.ElementType.FirstOrDefault(etentity =>
                        etentity.strCustomTypeId == "XJDFMedia" &&
                        etentity.intPrintshopPk == null &&
                        etentity.strResOrPro == "Resource").intPk;

                    //                                      //Get dimensions to Add.
                    double? numnWidthToAdd; double? numnLengthToAdd; String strDimensionsUnitToAdd;
                    ResResource.GetWidthLengthAndDimensionsUnitToAdd(arrattrjson5_I, intPkSystemMediaType, context_M,
                        out numnWidthToAdd, out numnLengthToAdd, out strDimensionsUnitToAdd);

                    int intI = 0;
                    while (
                        intI < darreleentityResourceSameNameAndType.Count &&
                        !boolResourceSameNameAndTypeAndSize
                        )
                    {
                        EleentityElementEntityDB eleentityResourceAlreadyExits = 
                            darreleentityResourceSameNameAndType[intI];

                        //                                  //Get dimension for this resource with teh same name.
                        double? numnWidthAlreadyExits; double? numnLengthAlreadyExits;
                        String strDimensionsUnitAlreadyExits;
                        ResResource.GetWidthLengthAndDimensionsUnitAlreadyExits(eleentityResourceAlreadyExits,
                            context_M, out numnWidthAlreadyExits, out numnLengthAlreadyExits,
                            out strDimensionsUnitAlreadyExits);

                        if (
                            //                              //No dimensions.
                            ((numnWidthToAdd == null) &&
                            (numnLengthToAdd == null) &&
                            (strDimensionsUnitToAdd == null) &&
                            (numnWidthAlreadyExits == null) &&
                            (numnLengthAlreadyExits == null) &&
                            (strDimensionsUnitAlreadyExits == null))
                            ||
                            //                              //Same dimensions.
                            ((numnWidthToAdd != null) &&
                            (numnWidthAlreadyExits != null) &&
                            (numnWidthToAdd == numnWidthAlreadyExits) &&
                            (numnLengthToAdd != null) &&
                            (numnLengthAlreadyExits != null) &&
                            (numnLengthToAdd == numnLengthAlreadyExits) &&
                            (strDimensionsUnitToAdd != null) &&
                            (strDimensionsUnitAlreadyExits != null) &&
                            (strDimensionsUnitToAdd == strDimensionsUnitAlreadyExits))
                            )
                        {
                            boolResourceSameNameAndTypeAndSize = true;
                            strUserMessage_IO = "A resource with the same name and dimensions was found.";
                        }
                        intI++;
                    }
                }
            }
            return boolResourceSameNameAndTypeAndSize;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void GetWidthLengthAndDimensionsUnitToAdd(
            Attrjson5AttributeJson5[] arrattrjson5_I,
            int intPkSystemMediaType_I,
            Odyssey2Context context_M,
            out double? numnWidthToAdd_O,
            out double? numnLengthToAdd_O,
            out String strDimensionsUnitToAdd_O
            )
        {
            numnWidthToAdd_O = null;
            numnLengthToAdd_O = null;
            strDimensionsUnitToAdd_O = null;

            int intPkWidthAttribute =
                (from attrentity in context_M.Attribute
                 join attretentity in context_M.AttributeElementType
                 on attrentity.intPk equals attretentity.intPkAttribute
                 where attretentity.intPkElementType == intPkSystemMediaType_I &&
                 attrentity.strXJDFName == "Width"
                 select attrentity.intPk).ToList()[0];

            int intPkLengthAttribute =
                (from attrentity in context_M.Attribute
                 join attretentity in context_M.AttributeElementType
                 on attrentity.intPk equals attretentity.intPkAttribute
                 where attretentity.intPkElementType == intPkSystemMediaType_I &&
                 attrentity.strXJDFName == "Length"
                 select attrentity.intPk).ToList()[0];

            int intPkDimensionsUnitAttribute =
                (from attrentity in context_M.Attribute
                 join attretentity in context_M.AttributeElementType
                 on attrentity.intPk equals attretentity.intPkAttribute
                 where attretentity.intPkElementType == intPkSystemMediaType_I &&
                 attrentity.strXJDFName == "DimensionsUnit"
                 select attrentity.intPk).ToList()[0];

            foreach (Attrjson5AttributeJson5 attrjon5 in arrattrjson5_I)
            {
                int intAttributeInTheList = arrattrjson5_I.Count(json =>
                    json.strAscendant == attrjon5.strAscendant);

                if (
                    intAttributeInTheList == 1
                    )
                {
                    //                                      //Easy code to get the attribute pk.
                    String strAttrPk = attrjon5.strAscendant;
                    if (
                        strAttrPk.LastIndexOf(Tools.charConditionSeparator) != -1
                        )
                    {
                        strAttrPk = strAttrPk.Substring(strAttrPk.LastIndexOf(Tools.charConditionSeparator) + 1);
                    }

                    if (
                        //                                  //The attribute pk is an int.
                        strAttrPk.IsParsableToInt()
                        )
                    {
                        AttrentityAttributeEntityDB attrentityAttribute = context_M.Attribute.FirstOrDefault(attr =>
                        attr.intPk == strAttrPk.ParseToInt());

                        if (
                            //                              //The attr exists.
                            attrentityAttribute != null
                            )
                        {
                            if (
                                attrentityAttribute.intPk == intPkWidthAttribute
                                )
                            {
                                if (
                                    attrjon5.strValue.IsParsableToNum()
                                    )
                                {
                                    numnWidthToAdd_O = attrjon5.strValue.ParseToNum();
                                }
                            }
                            if (
                                attrentityAttribute.intPk == intPkLengthAttribute
                                )
                            {
                                if (
                                    attrjon5.strValue.IsParsableToNum()
                                    )
                                {
                                    numnLengthToAdd_O = attrjon5.strValue.ParseToNum();
                                }
                            }
                            if (
                                attrentityAttribute.intPk == intPkDimensionsUnitAttribute
                                )
                            {
                                strDimensionsUnitToAdd_O = attrjon5.strValue;
                            }
                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void GetWidthLengthAndDimensionsUnitAlreadyExits(
            EleentityElementEntityDB eleentityResourceAlreadyExits_I,
            Odyssey2Context context_M,
            out double? numnWidthAlreadyExits_O,
            out double? numnLengthAlreadyExits_O,
            out String strDimensionsUnitAlreadyExits_O
            )
        {
            numnWidthAlreadyExits_O = null;
            numnLengthAlreadyExits_O = null;
            strDimensionsUnitAlreadyExits_O = null;

            //                                          //Get the Width attribute.
            int intPkWidthAttribute =
                (from attrentity in context_M.Attribute
                 join attretentity in context_M.AttributeElementType
                 on attrentity.intPk equals attretentity.intPkAttribute
                 where attretentity.intPkElementType == eleentityResourceAlreadyExits_I.intPkElementType &&
                 attrentity.strXJDFName == "Width"
                 select attrentity.intPk).ToList()[0];

            //                                          //Get the Width value.
            ValentityValueEntityDB valentityWidth = context_M.Value.FirstOrDefault(val =>
                val.intPkElement == eleentityResourceAlreadyExits_I.intPk &&
                val.intPkAttribute == intPkWidthAttribute);

            if (
                valentityWidth != null &&
                valentityWidth.strValue.IsParsableToNum()
            )
            {
                numnWidthAlreadyExits_O = valentityWidth.strValue.ParseToNum();
            }

            //                                          //Get the Length attribute.
            int intPkLengthAttribute =
                (from attrentity in context_M.Attribute
                 join attretentity in context_M.AttributeElementType
                 on attrentity.intPk equals attretentity.intPkAttribute
                 where attretentity.intPkElementType == eleentityResourceAlreadyExits_I.intPkElementType &&
                 attrentity.strXJDFName == "Length"
                 select attrentity.intPk).ToList()[0];

            //                                          //Get the Length value.
            ValentityValueEntityDB valentityLength = context_M.Value.FirstOrDefault(val =>
                val.intPkElement == eleentityResourceAlreadyExits_I.intPk &&
                val.intPkAttribute == intPkLengthAttribute);

            if (
                valentityLength != null &&
                valentityLength.strValue.IsParsableToNum()
            )
            {
                numnLengthAlreadyExits_O = valentityLength.strValue.ParseToNum();
            }

            //                                          //Get the DimensionUnit attribute.
            int intPkDimensionsUnitAttribute =
                (from attrentity in context_M.Attribute
                 join attretentity in context_M.AttributeElementType
                 on attrentity.intPk equals attretentity.intPkAttribute
                 where attretentity.intPkElementType == eleentityResourceAlreadyExits_I.intPkElementType &&
                 attrentity.strXJDFName == "DimensionsUnit"
                 select attrentity.intPk).ToList()[0];

            //                                          //Get the Width value.
            ValentityValueEntityDB valentityDimensionsUnit = context_M.Value.FirstOrDefault(val =>
                val.intPkElement == eleentityResourceAlreadyExits_I.intPk &&
                val.intPkAttribute == intPkDimensionsUnitAttribute);

            if (
                valentityDimensionsUnit != null
            )
            {
                strDimensionsUnitAlreadyExits_O = valentityDimensionsUnit.strValue;
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static bool boolHasValidAttributes(
            //                                              //Verify all the attributes to add to the resource to 
            //                                              //      confirm that they are from the correct type and 
            //                                              //      they have valid values.

            //                                              //Array of attributes. Every element is like:
            //                                              //  {
            //                                              //      "strAscendant":"3|6|9",
            //                                              //      "strValue":"Valor1",
            //                                              //      "intnInheritedValuePk":15,
            //                                              //      "boolChangeable":false
            //                                              //  }
            Attrjson5AttributeJson5[] arrattrjson5_I,
            //                                              //Type of the resource, to validate that every attribute
            //                                              //      comes from that.
            RestypResourceType restyp_I, 
            Odyssey2Context context_I
            )
        {
            bool boolValidAttributes = true;
            
            //                                              //Get the pks num att to validate.
            List<int> darrintPkNumericalAttributesToValidate = new List<int>();
            ResResource.GetNumericalAttributesToValidate(restyp_I, context_I, ref darrintPkNumericalAttributesToValidate);

            int intI = 0;
            /*UNTIL-DO*/
            while (!(
                (intI >= arrattrjson5_I.Length) ||
                !boolValidAttributes
                ))
            {
                int intAttributeInTheList = arrattrjson5_I.Count(json =>
                    json.strAscendant == arrattrjson5_I[intI].strAscendant);

                if (
                    intAttributeInTheList == 1
                    )
                {
                    //                                      //Easy code to get the attribute pk.
                    String strAttrPk = arrattrjson5_I[intI].strAscendant;
                    if (
                        strAttrPk.LastIndexOf(Tools.charConditionSeparator) != -1
                        )
                    {
                        strAttrPk = strAttrPk.Substring(strAttrPk.LastIndexOf(Tools.charConditionSeparator) + 1);
                    }

                    if (
                        //                                  //The attribute pk is an int.
                        strAttrPk.IsParsableToInt()
                        )
                    {
                        AttrentityAttributeEntityDB attrentityAttribute = context_I.Attribute.FirstOrDefault(a =>
                        a.intPk == strAttrPk.ParseToInt());

                        if (
                            //                              //The attr exists.
                            attrentityAttribute != null
                            )
                        {
                            if (
                                attrentityAttribute.strEnumAssoc != ""
                                )
                            {
                                String strEnumName = attrentityAttribute.strEnumAssoc;
                                String[] arrstr = Odyssey2.arrstrGetEnumFromDB(strEnumName);
                                List<String> darrstr = arrstr.ToList();
                                String strValueUnikeRef = arrattrjson5_I[intI].strValue.ToString();

                                String strEvaluate = darrstr.FirstOrDefault(str => str == strValueUnikeRef);
                                if (
                                    strEvaluate == null
                                    //!darrstr.Exists(strAttr => strAttr == strValueUnikeRef)
                                    //!arrstr.Contains(strValueUnikeRef)
                                    )
                                {
                                    boolValidAttributes = false;
                                }
                            }
                            if (
                                darrintPkNumericalAttributesToValidate != null
                                &&
                                darrintPkNumericalAttributesToValidate.Exists(intPkattribute => 
                                    intPkattribute == attrentityAttribute.intPk)
                                &&
                                !arrattrjson5_I[intI].strValue.IsParsableToNum()
                                )
                            {
                                boolValidAttributes = false;
                            }
                        }
                        else
                        {
                            boolValidAttributes = false;
                        }
                    }
                    else
                    {
                        boolValidAttributes = false;
                    }

                    if (
                        boolValidAttributes
                        )
                    {
                        //                                  //Getting the intnInherited property.
                        int? intnInheritedPk = arrattrjson5_I[intI].intnInheritedValuePk;

                        //                                  //Validation for the inheritance.
                        if (
                            (intnInheritedPk != null) &&
                            (intnInheritedPk > 0)
                            )
                        {
                            ValentityValueEntityDB valentity = context_I.Value.FirstOrDefault(valentity =>
                                valentity.intPk == intnInheritedPk);

                            if (
                                valentity == null
                                )
                            {
                                boolValidAttributes = false;
                            }
                            else
                            {
                                if (
                                    (!arrattrjson5_I[intI].boolChangeable) &&
                                    (arrattrjson5_I[intI].strValue != valentity.strValue)
                                    )
                                {
                                    boolValidAttributes = false;
                                }
                            }
                        }
                    }
                }
                else
                {
                    boolValidAttributes = false;
                }
                intI = intI + 1;
            }

            return boolValidAttributes;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static bool boolIsInheritanceDataValid(
            double? numnCost_I,
            double? numnQuantity_I,
            double? numnMin_I,
            int? intnPkAccount_I,
            double? numnHourlyRate_I,
            bool? boolnArea_I,
            String strUnit_I,
            String strUnitInherited_I,
            bool? boolnCalendarized_I,
            RestypResourceType restyp_I,
            int? intnPkDad_I,
            bool? boolnCostInherited_I,
            bool? boolnCostChangeable_I,
            bool? boolnUnitInherited_I,
            bool? boolnUnitChangeable_I,
            bool? boolnAvailabilityInherited_I,
            bool? boolnAvailabilityChangeable_I,
            String strClassification_I,
            Odyssey2Context context_I
            )
        {
            bool boolIsDataValid = false;

            if (
                intnPkDad_I == null
                )
            {
                boolIsDataValid = true;
            }
            else
            {
                if (
                    intnPkDad_I > 0
                    )
                {
                    if (
                        //                                  //It is not physical. Not physical resources have 
                        //                                  //      no cost, availability and unit.
                        !RestypResourceType.boolIsPhysical(strClassification_I) &&
                        (numnCost_I == null || numnCost_I == 0) &&
                        (numnQuantity_I == null || numnQuantity_I  == 0) &&
                        intnPkAccount_I == null &&
                        (strUnit_I == null || strUnit_I == "") &&
                        numnMin_I == null &&
                        numnHourlyRate_I == null &&
                        boolnArea_I == null &&
                        strUnitInherited_I == null &&
                        boolnCalendarized_I == null &&
                        boolnCostInherited_I == null &&
                        boolnCostChangeable_I == null &&
                        boolnUnitInherited_I == null &&
                        boolnUnitChangeable_I == null &&
                        (boolnAvailabilityInherited_I == null || boolnAvailabilityInherited_I == false) &&
                        boolnAvailabilityChangeable_I == null
                        )
                    {
                        boolIsDataValid = true;
                    }

                    //                                      //Account.
                    AccentityAccountEntityDB accentity = null;

                    if (
                        intnPkAccount_I > 0
                        )
                    {
                        //                                  //Find account.
                        accentity = context_I.Account.FirstOrDefault(acc =>
                            acc.intPk == intnPkAccount_I);
                    }
                    
                    if (
                        //                                  //Inheritance cost data valid.
                        ((numnCost_I == null) || ((numnCost_I != null) && (numnCost_I >= 0))) &&
                        ((numnQuantity_I == null) || ((numnQuantity_I != null) && (numnQuantity_I >= 0))) &&
                        ((numnMin_I == null) || ((numnMin_I != null) && (numnMin_I >= 0))) &&
                        ((numnHourlyRate_I == null) || ((numnHourlyRate_I != null) && (numnHourlyRate_I >= 0))) &&
                        ((intnPkAccount_I == null) || ((intnPkAccount_I != null) && (accentity != null))) &&
                        //                                  //Inheritance unit data valid.
                        ((strUnitInherited_I == null) || (strUnitInherited_I == strUnit_I))
                        &&
                        //                                  //Availability type data valid.
                        ((boolnCalendarized_I == false) ||
                        ((boolnCalendarized_I == true) && RestypResourceType.boolIsDeviceToolOrCustom(restyp_I))
                        )
                        &&
                        (boolnCostInherited_I != null) && (boolnCostChangeable_I != null) &&
                        (boolnUnitInherited_I != null) && (boolnUnitChangeable_I != null) &&
                        (boolnAvailabilityInherited_I != null) && (boolnAvailabilityChangeable_I != null)
                        )
                    {
                        //****************************************************
                        //                                  //Se descomentará cuando se agregue en el modal
                        //                                  // de herencia en ByArea
                        //if(
                        //    //                                  //It is media or component.
                        //    (restyp_I.strCustomTypeId == ResResource.strComponent) ||
                        //    (restyp_I.strCustomTypeId == ResResource.strMedia)
                        //    )
                        //{
                        //    //                              //Valid the area.
                        //    boolIsDataValid = boolnArea_I != null ? true : false;
                        //}
                        //else
                        //{
                        //    //                              //Valid the area.
                        //    boolIsDataValid = boolnArea_I == null ? true : false; 
                        //}
                        boolIsDataValid = true;
                    }
                }
            }

            return boolIsDataValid;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subAddInheritedCost(
            int intPkDad_I,
            ResResource resResource_I,
            double? numnCost_I,
            double? numnQuantity_I,
            double? numnMin_I,
            double? numnBlock_I,
            int? intnPkAccount_I,
            double?  numnHourlyRate_I,
            bool? boolnArea_I,
            bool? boolnCostInherited_I,
            bool? boolnCostChangeable_I,
            PsPrintShop ps_I,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            int? intnPkCostInherited = null;

            //                                              //Get template dad.
            ResResource resTemplateInherited = ResResource.resFromDB(context_M, intPkDad_I, true);
            //                                              //Get current cost of template dad.
            CostentityCostEntityDB costentityCurrentTemplateDadCost;
            resTemplateInherited.subGetCurrentCost(context_M, out costentityCurrentTemplateDadCost);

            bool boolIsDummyCost = false;
            if (
                //                                          //Dad does not have a cost.
                costentityCurrentTemplateDadCost == null
                )
            {
                boolIsDummyCost = true;
                //                                          //Add dummy cost to template dad.
                ResResource.subAddCost(intPkDad_I, null, null, null, null, null, null, boolIsDummyCost, ps_I,
                    null, context_M, ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);
                //                                          //Update variables.
                resTemplateInherited.subGetCurrentCost(context_M, out costentityCurrentTemplateDadCost);
            }
            else if(
                costentityCurrentTemplateDadCost.numnCost == null &&
                costentityCurrentTemplateDadCost.numnQuantity == null &&
                costentityCurrentTemplateDadCost.numnHourlyRate == null &&
                costentityCurrentTemplateDadCost.boolnArea == null
                )
            {
                boolIsDummyCost = true;
            }

            intnPkCostInherited = costentityCurrentTemplateDadCost.intPk;

            //                                              //Add cost to the new resource.
            ResResource.subAddCost(resResource_I.intPk, numnQuantity_I, numnCost_I, numnMin_I, numnBlock_I,
                intnPkAccount_I, numnHourlyRate_I, boolIsDummyCost, ps_I, boolnArea_I, context_M, ref intStatus_IO,
                ref strUserMessage_IO, ref strDevMessage_IO);

            //                                              //Get current cost of the new resource.
            CostentityCostEntityDB costentityCurrentResourceCost;
            resResource_I.subGetCurrentCost(context_M, out costentityCurrentResourceCost);

            if (
                costentityCurrentResourceCost != null
                )
            {
                CostentityCostEntityDB costentityToUpdate = context_M.Cost.FirstOrDefault(cost =>
                    cost.intPk == costentityCurrentResourceCost.intPk);

                costentityToUpdate.boolnIsChangeable = boolnCostChangeable_I;

                if (
                    boolnCostInherited_I == true &&
                    boolnCostChangeable_I == false
                    )
                {
                    costentityToUpdate.numnQuantity = costentityCurrentTemplateDadCost.numnQuantity;
                    costentityToUpdate.numnCost = costentityCurrentTemplateDadCost.numnCost;
                    costentityToUpdate.numnMin = costentityCurrentTemplateDadCost.numnMin;
                    costentityToUpdate.numnBlock = costentityCurrentTemplateDadCost.numnBlock;
                    costentityToUpdate.intnPkCostInherited = intnPkCostInherited;
                    costentityToUpdate.intPkAccount = costentityCurrentTemplateDadCost.intPkAccount;
                    costentityToUpdate.numnHourlyRate = costentityCurrentTemplateDadCost.numnHourlyRate;
                    costentityToUpdate.boolnArea = costentityCurrentTemplateDadCost.boolnArea;
                }

                if (
                    boolnCostInherited_I == true &&
                    boolnCostChangeable_I == true
                    )
                {
                    costentityToUpdate.intnPkCostInherited = intnPkCostInherited;
                }

                if (
                    boolnCostInherited_I != true
                    )
                {
                    costentityToUpdate.intnPkCostInherited = null;
                }
            }

            context_M.SaveChanges();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subAddInheritedAvailability(
            int intPkDad_I,
            int intPkResource_I,
            bool? boolnCalendarized_I,
            bool? boolnAvailabilityInherited_I,
            bool? boolnAvailabilityChangeable_I,
            Odyssey2Context context_M
            )
        {
            EleentityElementEntityDB eleentityResource = context_M.Element.FirstOrDefault(ele =>
               ele.intPk == intPkResource_I);

            //                                              //Update de inheritance data for calendarized.
            bool boolCalendarized = boolnCalendarized_I == null ? false : (bool)boolnCalendarized_I;
            eleentityResource.boolnIsCalendar = boolCalendarized;
            eleentityResource.boolnIsAvailable = boolCalendarized == false ? (bool?)true : null;

            //                                              //Initial inheritance data.
            eleentityResource.boolnCalendarIsChangeable = boolnAvailabilityChangeable_I;
            eleentityResource.intnPkElementCalendarInherited = null;

            //                                              //Get template dad.
            ResResource resTemplateInherited = ResResource.resFromDB(context_M, intPkDad_I, true);

            if (
                boolnAvailabilityInherited_I == true
                )
            {
                eleentityResource.intnPkElementCalendarInherited = intPkDad_I;

                if (
                    boolnAvailabilityChangeable_I == false
                    )
                {
                    eleentityResource.boolnIsCalendar = resTemplateInherited.boolnIsCalendar;
                }
            }
            context_M.SaveChanges();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subAddNameAttribute(
            RestypResourceType restypBelongsTo_I,
            ResResource res_I,
            String strName_I,
            Odyssey2Context context_M
            )
        {
            if (
                strName_I != null
                )
            {
                //                                          //Get the Name attribute.
                int intPkAttributeName =
                    (from attrentity in context_M.Attribute
                     join attretentity in context_M.AttributeElementType
                     on attrentity.intPk equals attretentity.intPkAttribute
                     where attretentity.intPkElementType == restypBelongsTo_I.intPk &&
                     attrentity.strXJDFName == "Name"
                     select attrentity.intPk).ToList()[0];

                //                                          //Add the name attribute.
                ValentityValueEntityDB valentity = new ValentityValueEntityDB
                {
                    strValue = strName_I,
                    intPkAttribute = intPkAttributeName,
                    intPkElement = res_I.intPk,
                    intnPkValueInherited = null,
                    boolnIsChangeable = true
                };

                context_M.Value.Add(valentity);

                //                                          //Add the ascendant for the name/unit attribute.
                AscentityAscendantsEntityDB ascentity = new AscentityAscendantsEntityDB
                {
                    strAscendants = "" + intPkAttributeName,
                    intPkElement = res_I.intPk
                };
                context_M.Ascendants.Add(ascentity);
                context_M.SaveChanges();
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subAddUnitAttribute(
            RestypResourceType restypBelongsTo_I,
            ResResource res_I,
            String strUnit_I,
            bool? boolnIsDecimal_I,
            bool? boolnUnitChangeable_I,
            bool? boolnUnitInherited_I,
            int? intnPkDad_I,
            Odyssey2Context context_M
            )
        {
            if (
                strUnit_I != null
                )
            {
                //                                          //Get the Unit attribute.
                int intPkAttributeUnit =
                    (from attrentity in context_M.Attribute
                     join attretentity in context_M.AttributeElementType
                     on attrentity.intPk equals attretentity.intPkAttribute
                     where attretentity.intPkElementType == restypBelongsTo_I.intPk &&
                     attrentity.strXJDFName == "Unit"
                     select attrentity.intPk).ToList()[0];

                //                                          //Add the unit attribute.
                ValentityValueEntityDB valentity = new ValentityValueEntityDB
                {
                    strValue = strUnit_I,
                    intPkAttribute = intPkAttributeUnit,
                    intPkElement = res_I.intPk,
                    intnPkValueInherited = null,
                    boolnIsChangeable = true,
                    boolnIsDecimal = boolnIsDecimal_I
                };

                //                                          //Get the Pk of the inherited value.
                int? intnPkValueInherited = null;
                if (
                    boolnUnitInherited_I == true &&
                    intnPkDad_I != null &&
                    intnPkDad_I > 0
                    )
                {
                    List<ValentityValueEntityDB> darrvalentityTemplateDad = context_M.Value.Where(val =>
                        val.intPkElement == intnPkDad_I &&
                        val.intPkAttribute == intPkAttributeUnit).ToList();
                    darrvalentityTemplateDad.Sort();
                    ValentityValueEntityDB valentityTemplateDad = darrvalentityTemplateDad.Last();
                    intnPkValueInherited = valentityTemplateDad.intPk;
                }

                //                                          //Add the unit attribute and hist.
                valentity = new ValentityValueEntityDB
                {
                    strValue = strUnit_I,
                    intPkAttribute = intPkAttributeUnit,
                    intPkElement = res_I.intPk,
                    intnPkValueInherited = intnPkValueInherited,
                    boolnIsChangeable = boolnUnitChangeable_I,
                    strSetDate = Date.Now(ZonedTimeTools.timezone).ToText(),
                    strSetTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                    boolnIsDecimal = boolnIsDecimal_I
                };

                context_M.Value.Add(valentity);

                //                                          //Add the ascendant for the name/unit attribute.
                AscentityAscendantsEntityDB ascentity = new AscentityAscendantsEntityDB
                {
                    strAscendants = "" + intPkAttributeUnit,
                    intPkElement = res_I.intPk
                };
                context_M.Ascendants.Add(ascentity);
                context_M.SaveChanges();
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subAddOneAttribute(
            //                                              //Add one value for the attribute in the db and add the 
            //                                              //      ascendants to the correct table for eery value.

            Odyssey2Context context_M,
            //                                              //Resource.
            ResResource res_I,
            //                                              //Json with the attribute data.
            //                                              //  {
            //                                              //      "strAscendant":"3|6|9",
            //                                              //      "strValue":"Valor1",
            //                                              //      "intnInheritedValuePk":15,
            //                                              //      "boolChangeable":false,
            //                                              //      "intPkValue":1
            //                                              //  }
            ref Attrjson5AttributeJson5 attrjson5_M
            )
        {
            //                                              //Easy code to get the attribute pk.
            String strAttrPk = attrjson5_M.strAscendant;
            if (
                strAttrPk.LastIndexOf(Tools.charConditionSeparator) != -1
                )
            {
                strAttrPk = strAttrPk.Substring(strAttrPk.LastIndexOf(Tools.charConditionSeparator) + 1);
            }

            int? intnInheritedPk = attrjson5_M.intnInheritedValuePk;

            //                                              //Adding the value.
            ValentityValueEntityDB valentityAdded = new ValentityValueEntityDB
            {
                strValue = attrjson5_M.strValue,
                intPkAttribute = strAttrPk.ParseToInt(),
                intPkElement = res_I.intPk,
                intnPkValueInherited = intnInheritedPk,
                boolnIsChangeable = attrjson5_M.boolChangeable
            };
            context_M.Value.Add(valentityAdded);
            context_M.SaveChanges();
            attrjson5_M.intnPkValue = valentityAdded.intPk;

            //                                              //Adding the ascendants.
            AscentityAscendantsEntityDB ascentity = new AscentityAscendantsEntityDB
            {
                strAscendants = attrjson5_M.strAscendant,
                intPkElement = res_I.intPk
            };
            context_M.Ascendants.Add(ascentity);
            context_M.SaveChanges();
        }

        //--------------------------------------------------------------------------------------------------------------
        public static int? intRestrictionLevelOfATemplateFromType(
            //                                              //Description Of this method:
            //                                              //    From a template, I go up the tree hierarchy 
            //                                              //    until reaching restriction level one.

            //                                              //                                           LevelRestrict
            //                                              //             -------- Type -------------
            //                                              //            |                           |
            //                                              //     ---*Template1*--              *Template4*       1
            //                                              //     |               |                  |
            //                                              //   *Template2*  *Template3*        *Template5*       2
            //                                              //                                        |
            //                                              //                                  *Template6*        3

            //                                              //Level Restrict.
            //                                              //    Name Template.        Level Restrict.
            //                                              //    Template1       --       1  
            //                                              //    Template2       --       2  
            //                                              //    Template3       --       2  
            //                                              //    Template4       --       1  
            //                                              //    Template5       --       2  
            //                                              //    Template6       --       3  

            //                                              //Pk Template .
            int intPkTemplate_I,
            //                                              //Pk Type.
            int intPkType_I
            )
        {
            //                                              //Establish conection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Initial Data.
            bool boolIsLevelRestrictionOne = false;
            int? intLevelRestrictionOne = null;

            //                                              //Get all template from this Type.
            List<EleentityElementEntityDB> darreleentityTemplate = context.Element.Where(ele =>
            ele.intPkElementType == intPkType_I && ele.boolIsTemplate == true).ToList();

            EleentityElementEntityDB eleentityTemplate = darreleentityTemplate.FirstOrDefault(
                ele => ele.intPk == intPkTemplate_I);

            if (
                //                                          //Valid pk template_I
                eleentityTemplate != null
                )
            {
                intLevelRestrictionOne = 1;

                int intPkFind = eleentityTemplate.intPk;

                /*REPEAT-WHILE*/
                while (
                    //                                      //continue cycling until restriction one is 1.
                    boolIsLevelRestrictionOne == false
                    )
                {
                    //                                      //Get entity of template current.
                    EleentityElementEntityDB eleentityTemplateFinded = darreleentityTemplate.FirstOrDefault(
                    ele => ele.intPk == intPkFind);

                    if (
                        //                                  //PkInherited is different that null.
                        eleentityTemplateFinded.intnPkElementInherited != null
                        )
                    {
                        //                                  //It Is not level Restrict one.
                        intLevelRestrictionOne = intLevelRestrictionOne + 1;
                        intPkFind = (int)eleentityTemplateFinded.intnPkElementInherited;
                        boolIsLevelRestrictionOne = false;
                    }
                    else
                    {
                        //                                  //It Is level Restrict one.
                        boolIsLevelRestrictionOne = true;
                    }
                }
            }
            return intLevelRestrictionOne;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subAddCustom(
            //                                              //Add a type to printshop.

            String strResourceName_I,
            String strUnit_I,
            bool? boolnIsDecimal_I,
            String[] arrstrAttribute_I,
            String[] arrstrValue_I,
            PsPrintShop ps_I,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO,
            out int intPrintshopTypePk_O
            )
        {
            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Printshop not found.";
            intPrintshopTypePk_O = -1;
            if (
                ps_I != null
                )
            {
                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Not all the attributes have value.";
                if (
                    //                                      //Each attribute should to have a value.
                    arrstrAttribute_I.Length == arrstrValue_I.Length
                    )
                {
                    intStatus_IO = 403;
                    strUserMessage_IO = "A name is necessary.";
                    strDevMessage_IO = "";
                    if (
                        (strResourceName_I != null) &&
                        (strResourceName_I != "")
                        )
                    {
                        intStatus_IO = 404;
                        strUserMessage_IO = "A unit is necessary.";
                        if (
                            (strUnit_I != null) &&
                            (strUnit_I != "")
                            )
                        {
                            //                              //Find the custom type of a printshop.
                            EtentityElementTypeEntityDB etentityCustom = context_M.ElementType.FirstOrDefault(
                                ele => ele.strCustomTypeId == RestypResourceType.strResCustomType
                                && ele.intPrintshopPk == ps_I.intPk);

                            if (
                                etentityCustom == null
                                )
                            {
                                //                          //Add CustomType to printshop
                                //                          //Create the entity
                                etentityCustom = new EtentityElementTypeEntityDB()
                                {
                                    strCustomTypeId = EtElementTypeAbstract.strResCustomType,
                                    intPrintshopPk = ps_I.intPk,
                                    strXJDFTypeId = EtElementTypeAbstract.strNotXJDF,
                                    strAddedBy = ps_I.strPrintshopId,
                                    strResOrPro = EtElementTypeAbstract.strResource,
                                    strClassification = RestypResourceType.strResourceTypeCustom
                                };

                                context_M.ElementType.Add(etentityCustom);
                                context_M.SaveChanges();

                            }
                            //                              //Get the pk of the customType added.
                            int intPkCustomType = etentityCustom.intPk;

                            //                              //Filter for that the name of resourse do not be duplicate.
                            EleentityElementEntityDB eleentity = context_M.Element.FirstOrDefault(
                                ele => ele.strElementName == strResourceName_I &&
                                ele.intPkElementType == intPkCustomType);

                            intStatus_IO = 405;
                            strUserMessage_IO = "A resource with this name already exists.";
                            if (
                                //                          //Resource name should not be repeated
                                //                          //      for a printshop
                                eleentity == null
                                )
                            {
                                //                          //Add Resource
                                eleentity = new EleentityElementEntityDB()
                                {
                                    strElementName = strResourceName_I,
                                    intPkElementType = intPkCustomType,
                                    boolnIsCalendar = false,
                                    boolnIsAvailable = true
                                };
                                context_M.Element.Add(eleentity);
                                context_M.SaveChanges();
                                //                          //Get the pk of resource added.
                                int intPkResource = eleentity.intPk;

                                //                          //Add unit 
                                ResResource.subAddUnitAttributeToCustom(eleentity.intPkElementType, eleentity.intPk,
                                    strUnit_I, boolnIsDecimal_I, context_M);

                                List<String> darrstrAttr = new List<String>();
                                bool boolAllAttributeAreValid = true;
                                int intI = 0;
                                //                          //Add all attribute and values
                                /*WHILE-DO*/
                                while (
                                    intI < arrstrAttribute_I.Length && boolAllAttributeAreValid
                                    )
                                {
                                    String strCurrentAtribute = arrstrAttribute_I[intI];
                                    String strCurrentValue = arrstrValue_I[intI];

                                    if (
                                        strCurrentAtribute.ToLower() == "unit"
                                        )
                                    {
                                        //                  //The Unit attribute cannot be added
                                        boolAllAttributeAreValid = false;
                                        intStatus_IO =406;
                                        strUserMessage_IO = "The Unit attribute cannot be added.";
                                        strDevMessage_IO = "";
                                    }

                                    if (
                                        //                  //The attribute repeats
                                        darrstrAttr.Contains(strCurrentAtribute.ToLower())
                                        )
                                    {
                                        boolAllAttributeAreValid = false;
                                        intStatus_IO = 407;
                                        strUserMessage_IO = "An attribute repeats.";
                                        strDevMessage_IO = "";
                                    }
                                    else
                                    {
                                        darrstrAttr.Add(strCurrentAtribute.ToLower());

                                        //                  //Add Attribute to database.
                                        //                  //Create the entity attribute.
                                        AttrentityAttributeEntityDB attrentity = new AttrentityAttributeEntityDB()
                                        {
                                            strCustomName = strCurrentAtribute,
                                            strXJDFName = "",
                                            strCardinality = "",
                                            strDatatype = "string",
                                            strEnumAssoc = "",
                                            strDescription = "",
                                            strScope = ""
                                        };
                                        context_M.Attribute.Add(attrentity);
                                        context_M.SaveChanges();
                                        int intpkAttribute = attrentity.intPk;

                                        //                  //Associate the attribute with the elementType.
                                        //                  //Create the entity.
                                        AttretentityAttributeElementTypeEntityDB attretentity =
                                            new AttretentityAttributeElementTypeEntityDB()
                                            {
                                                intPkAttribute = intpkAttribute,
                                                intPkElementType = intPkCustomType
                                            };
                                        context_M.AttributeElementType.Add(attretentity);
                                        context_M.SaveChanges();

                                        //                  //Add Values and associate with the resource.
                                        ValentityValueEntityDB valentity = new ValentityValueEntityDB()
                                        {
                                            strValue = strCurrentValue,
                                            intPkAttribute = intpkAttribute,
                                            intPkElement = intPkResource
                                        };
                                        context_M.Value.Add(valentity);
                                        context_M.SaveChanges();

                                        //                  //Add the ascendants for the value and resource.
                                        AscentityAscendantsEntityDB ascentity = new AscentityAscendantsEntityDB
                                        {
                                            intPkElement = intPkResource,
                                            strAscendants = intpkAttribute + ""
                                        };
                                        context_M.Ascendants.Add(ascentity);
                                        context_M.SaveChanges();
                                    }

                                    intI++;
                                }

                                if (
                                    boolAllAttributeAreValid
                                    )
                                {
                                    intStatus_IO = 200;
                                    strUserMessage_IO = "Success.";
                                    strDevMessage_IO = "";
                                }
                            }
                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subAddUnitAttributeToCustom(
            int intPkRestypCustomType_I,
            int intPkRes_I,
            String strUnit_I,
            bool? boolnIsDecimal_I,
            Odyssey2Context context_M
            )
        {
            String strUnitAttribute = "Unit";

            //                                              //Get the Unit attribute.
            IQueryable<AttrentityAttributeEntityDB> setattr =
                from attrentity in context_M.Attribute
                join attretentity in context_M.AttributeElementType
                on attrentity.intPk equals attretentity.intPkAttribute
                where attretentity.intPkElementType == intPkRestypCustomType_I
                select attrentity;
            List<AttrentityAttributeEntityDB> darrattr = setattr.ToList();
            AttrentityAttributeEntityDB attrentityUnit = darrattr.FirstOrDefault(a =>
                a.strCustomName == strUnitAttribute);

            if (
                attrentityUnit == null
                )
            {
                attrentityUnit = new AttrentityAttributeEntityDB()
                {
                    strCustomName = strUnitAttribute,
                    strXJDFName = "",
                    strCardinality = "",
                    strDatatype = "string",
                    strEnumAssoc = "",
                    strDescription = "",
                    strScope = ""
                };
                context_M.Attribute.Add(attrentityUnit);
                context_M.SaveChanges();

                //                                          //Associate the attribute with the elementType.
                //                                          //Create the entity.
                AttretentityAttributeElementTypeEntityDB attretentityUnit =
                    new AttretentityAttributeElementTypeEntityDB()
                    {
                        intPkAttribute = attrentityUnit.intPk,
                        intPkElementType = intPkRestypCustomType_I
                    };
                context_M.AttributeElementType.Add(attretentityUnit);
                context_M.SaveChanges();
            }

            //                                              //Add Values and associate with the resource.
            ValentityValueEntityDB valentity = new ValentityValueEntityDB()
            {
                strValue = strUnit_I,
                intPkAttribute = attrentityUnit.intPk,
                intPkElement = intPkRes_I,
                boolnIsDecimal = boolnIsDecimal_I
            };
            context_M.Value.Add(valentity);
            context_M.SaveChanges();

            //                                              //Add the ascendants for the value and resource.
            AscentityAscendantsEntityDB ascentity = new AscentityAscendantsEntityDB
            {
                intPkElement = intPkRes_I,
                strAscendants = attrentityUnit.intPk + ""
            };
            context_M.Ascendants.Add(ascentity);
            context_M.SaveChanges();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subAddAttributeToCustom(

            int intPkNewRestyp_I,
            int intPkNewRes_I,
            AttrentityAttributeEntityDB attrentityBase_I,
            ValentityValueEntityDB valentityBase_I,
            Odyssey2Context context_M
            )
        {
            AttrentityAttributeEntityDB attrentityNew;
            if (
                attrentityBase_I.strCustomName == "Unit"
                )
            {
                //                                          //Get the Unit attribute.
                List<AttrentityAttributeEntityDB> darrattr =
                    (from attrentity in context_M.Attribute
                    join attretentity in context_M.AttributeElementType
                    on attrentity.intPk equals attretentity.intPkAttribute
                    where attretentity.intPkElementType == intPkNewRestyp_I
                    select attrentity).ToList();

                attrentityNew = darrattr.FirstOrDefault(attr => attr.strCustomName == attrentityBase_I.strCustomName);

                if (
                    attrentityNew == null
                    )
                {
                    //                                      //Add Attribute to database.
                    //                                      //Create the entity attribute.
                    attrentityNew = new AttrentityAttributeEntityDB()
                    {
                        strCustomName = attrentityBase_I.strCustomName,
                        strXJDFName = "",
                        strCardinality = "",
                        strDatatype = "string",
                        strEnumAssoc = "",
                        strDescription = "",
                        strScope = ""
                    };
                    context_M.Attribute.Add(attrentityNew);
                    context_M.SaveChanges();

                    //                                      //Associate the attribute with the elementType.
                    //                                      //Create the entity.
                    AttretentityAttributeElementTypeEntityDB attretentityNew =
                        new AttretentityAttributeElementTypeEntityDB()
                        {
                            intPkAttribute = attrentityNew.intPk,
                            intPkElementType = intPkNewRestyp_I
                        };
                    context_M.AttributeElementType.Add(attretentityNew);
                    context_M.SaveChanges();
                }
            }
            else
            {
                //                                          //Add Attribute to database.
                //                                          //Create the entity attribute.
                attrentityNew = new AttrentityAttributeEntityDB()
                {
                    strCustomName = attrentityBase_I.strCustomName,
                    strXJDFName = "",
                    strCardinality = "",
                    strDatatype = "string",
                    strEnumAssoc = "",
                    strDescription = "",
                    strScope = ""
                };
                context_M.Attribute.Add(attrentityNew);
                context_M.SaveChanges();

                //                                          //Associate the attribute with the elementType.
                //                                          //Create the entity.
                AttretentityAttributeElementTypeEntityDB attretentityNew = 
                    new AttretentityAttributeElementTypeEntityDB()
                {
                    intPkAttribute = attrentityNew.intPk,
                    intPkElementType = intPkNewRestyp_I
                };
                context_M.AttributeElementType.Add(attretentityNew);
                context_M.SaveChanges();
            }

            //                                              //Add Values and associate with the resource.
            ValentityValueEntityDB valentityNew = new ValentityValueEntityDB()
            {
                strValue = valentityBase_I.strValue,
                boolnIsDecimal = valentityBase_I.boolnIsDecimal,
                intPkAttribute = attrentityNew.intPk,
                intPkElement = intPkNewRes_I
            };
            context_M.Value.Add(valentityNew);
            context_M.SaveChanges();

            //                                              //Add the ascendants for the value and resource.
            AscentityAscendantsEntityDB ascentityNew = new AscentityAscendantsEntityDB
            {
                strAscendants = attrentityNew.intPk + "",
                intPkElement = intPkNewRes_I,
            };
            context_M.Ascendants.Add(ascentityNew);
            context_M.SaveChanges();
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subDelete(
            //                                              //Method that delete a resource from the db.
            //                                              //1. Delete the fathers of templates or resources
            //                                              //      that are inheritors.
            //                                              //2. Delete the fathers of values that are inheritors of 
            //                                              //      values of this resource or template.
            //                                              //3. Delete the values of the resource or template.
            //                                              //4. Delete all the ascendants associated to the resource
            //                                              //      or template.
            //                                              //5. Delete the calculations associated to the res.
            //                                              //6. Delete the entry in InputsAndOutputs, this means:
            //                                              //      a) Delete the row if there is no link.
            //                                              //      b) Delete the foreign key if there is a link.
            //                                              //7. Delete the resource or template.
            //                                              //8. If the type has not more elements associated, delete
            //                                              //      the type.

            int intPk_I,
            String strPrintshopId_I,
            IHubContext<ConnectionHub> iHubContext_I,
            Odyssey2Context context_M,
            ref int intStatus_O,
            ref String strUserMessage_O,
            ref String strDevMessage_O
            )
        {
            //                                              //Get the template or resource.
            EleentityElementEntityDB eleentity = context_M.Element.FirstOrDefault(eleentity =>
                eleentity.intPk == intPk_I);

            intStatus_O = 401;
            strUserMessage_O = "Something is wrong.";
            strDevMessage_O = "No resource or template found.";
            if (
                eleentity != null
                )
            {
                //                                          //Get printshop.
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);

                //                                          //List of not deleted wf where this resource is in
                //                                          //      and have a job.
                List<WfentityWorkflowEntityDB> darrWorkflowsResourceIsInWithJob;
                if (
                    //                                      //There are jobs with this resource.
                    //                                      //History for some wfs needs to be done.
                    ResResource.boolResourceIsInWorkflowsWithThisResourceAndJobsInProgressOrCompleted(
                        context_M, eleentity, out darrWorkflowsResourceIsInWithJob)
                    )
                {
                    List<WfentityWorkflowEntityDB> darrwfentityToModifyIO = new List<WfentityWorkflowEntityDB>();
                    //                                      //Clone each workflow in the list.
                    foreach (WfentityWorkflowEntityDB wfentityBase in darrWorkflowsResourceIsInWithJob)
                    {
                        WfentityWorkflowEntityDB wfentityNew;
                        //                                  //Method to clone the workflow.
                        ProdtypProductType.subAddWorkflowIfItIsNecessary(ps, wfentityBase, context_M, out wfentityNew);

                        darrwfentityToModifyIO.Add(wfentityNew);
                    }
                    context_M.SaveChanges();

                    //                                      //Get the workflows where the resource is set and they have
                    //                                      //      do not have a job In Progress or Completed.
                    List<WfentityWorkflowEntityDB> darrWorkflowsResourceIsInWithoutJob =
                        ResResource.darrwfentityGetWorkflowsWithoutJob(intPk_I, context_M);

                    //                                      //In new clones wf and wf that do not have a job, delete the 
                    //                                      //      resource in the io entry.
                    darrwfentityToModifyIO.AddRange(darrWorkflowsResourceIsInWithoutJob);

                    //                                      //After clone, set resource as deleted.
                    ResResource.subDeleteResourceLogically(eleentity, strPrintshopId_I, darrwfentityToModifyIO,
                        iHubContext_I, context_M, ref intStatus_O, ref strUserMessage_O, ref strDevMessage_O);
                }
                else
                {
                    //                                      //The resource is used in a deleted wf (point of history).
                    List<IoentityInputsAndOutputsEntityDB> darrioentity =
                        (from ioentityA in context_M.InputsAndOutputs
                         join wfentityA in context_M.Workflow
                         on ioentityA.intPkWorkflow equals wfentityA.intPk
                         join jobentity in context_M.Job
                         on wfentityA.intPk equals jobentity.intPkWorkflow
                         where ioentityA.intnPkResource == intPk_I &&
                         wfentityA.boolDeleted == true
                         select ioentityA).ToList();

                    //                                      //The resource is used by a job in progress o completed that uses
                    //                                      //      a deleted wf.
                    List<IojentityInputsAndOutputsForAJobEntityDB> darriojentity =
                        (from iojentityA in context_M.InputsAndOutputsForAJob
                         join jobenity in context_M.Job
                         on iojentityA.intJobId equals jobenity.intJobID
                         join wfentityA in context_M.Workflow
                         on jobenity.intPkWorkflow equals wfentityA.intPk
                         where iojentityA.intPkResource == intPk_I &&
                         wfentityA.boolDeleted == true
                         select iojentityA).ToList();
                    if (
                        //                                  //Used in a point of history.
                        (darrioentity.Count > 0) ||
                        //                                  //Used in a job in progress o deleted with a job deleted.
                        (darriojentity.Count > 0)
                        )
                    {
                        List<WfentityWorkflowEntityDB> darrwfentityToModifyForIO =
                            (from wfentityToModify in context_M.Workflow
                             join ioentityUseRes in context_M.InputsAndOutputs
                             on wfentityToModify.intPk equals ioentityUseRes.intPkWorkflow
                             where ioentityUseRes.intnPkResource == intPk_I &&
                             wfentityToModify.boolDeleted == false
                             select wfentityToModify).ToList();

                        List<WfentityWorkflowEntityDB> darrwfentityToModifyForIOJ =
                            (from wfentityToModify in context_M.Workflow
                             join piwentityToModify in context_M.ProcessInWorkflow
                             on wfentityToModify.intPk equals piwentityToModify.intPkWorkflow
                             join iojentityUseRes in context_M.InputsAndOutputsForAJob
                             on piwentityToModify.intPk equals iojentityUseRes.intPkProcessInWorkflow
                             where iojentityUseRes.intPkResource == intPk_I &&
                             wfentityToModify.boolDeleted == false
                             select wfentityToModify).ToList();

                        darrwfentityToModifyForIO.AddRange(darrwfentityToModifyForIOJ);
                        darrwfentityToModifyForIO = darrwfentityToModifyForIO.Distinct().ToList();

                        ResResource.subDeleteResourceLogically(eleentity, strPrintshopId_I, darrwfentityToModifyForIO,
                            iHubContext_I, context_M, ref intStatus_O, ref strUserMessage_O, ref strDevMessage_O);
                    }
                    else
                    {
                        ResResource.subDeleteEstimationDataEntries(context_M, eleentity, intPk_I);

                        //                                  //There is not necessary to clone the workflow.
                        //                                  //Delete the resource phisically.
                        ResResource.SubDeleteResourcePhysically(intPk_I, strPrintshopId_I, iHubContext_I, context_M,
                            ref intStatus_O, ref strUserMessage_O, ref strDevMessage_O);
                    }
                }
                ResResource.subDeleteEstimationDataEntries(context_M, eleentity, intPk_I);
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static List<WfentityWorkflowEntityDB> darrwfentityGetWorkflowsWithoutJob(
            //                                              //Get the workflows where the resource is set and they have
            //                                              //      do not have a job In Progress or Completed.

            int intPkResource_I,
            Odyssey2Context context_I
            )
        {
            List<WfentityWorkflowEntityDB> darrwfentityAllWorkflows =
                (from wfentity in context_I.Workflow
                 join ioentity in context_I.InputsAndOutputs
                 on wfentity.intPk equals ioentity.intPkWorkflow
                 where ioentity.intnPkResource == intPkResource_I
                 select wfentity).ToList();

            List<WfentityWorkflowEntityDB> darrwfentityGetWorkflowsWithoutJob = new List<WfentityWorkflowEntityDB>();
            darrwfentityGetWorkflowsWithoutJob.AddRange(darrwfentityAllWorkflows);

            foreach (WfentityWorkflowEntityDB wfentity in darrwfentityAllWorkflows)
            {
                JobentityJobEntityDB jobentity = context_I.Job.FirstOrDefault(job =>
                    job.intPkWorkflow == wfentity.intPk);

                if (
                    jobentity != null
                    )
                {
                    darrwfentityGetWorkflowsWithoutJob.Remove(wfentity);
                }

            }
            return darrwfentityGetWorkflowsWithoutJob;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subDeleteEstimationDataEntries(
            //                                              //Delete estimation data entries related to resource.
            //                                              //Delete estimates entries related to resource.

            Odyssey2Context context_M,
            EleentityElementEntityDB eleentity_I,
            int intPkResource_I
            )
        {
            /*CASE*/
            if (
                //                                          //It's a resource.
                !eleentity_I.boolIsTemplate
                )
            {
                //                                          //Get EstimateData for this resource.
                List<EstdataentityEstimationDataEntityDB> darrestdataentity = context_M.EstimationData.Where(estdata =>
                estdata.intPkResource == intPkResource_I).ToList();

                //                                          //Take each EstimateData.
                foreach (EstdataentityEstimationDataEntityDB estdataWithJobId in darrestdataentity)
                {
                    //                                      //Get the PIW for this EstimateData.
                    PiwentityProcessInWorkflowEntityDB piwentity = context_M.ProcessInWorkflow.FirstOrDefault(piw =>
                        piw.intPk == estdataWithJobId.intPkProcessInWorkflow);

                    //                                      //
                    List<EstdataentityEstimationDataEntityDB> darrestdataToDelete = (
                        from estdataentity in context_M.EstimationData
                        join piwentityA in context_M.ProcessInWorkflow
                        on estdataentity.intPkProcessInWorkflow equals piwentityA.intPk
                        join wfentityA in context_M.Workflow
                        on piwentity.intPkWorkflow equals wfentityA.intPk
                        where wfentityA.intPk == piwentity.intPkWorkflow &&
                        estdataentity.intJobId == estdataWithJobId.intJobId &&
                        estdataentity.intId == estdataWithJobId.intId
                        select estdataentity).ToList();

                    foreach (EstdataentityEstimationDataEntityDB estToDelete in darrestdataToDelete)
                    {
                        context_M.EstimationData.Remove(estToDelete);
                    }

                    EstentityEstimateEntityDB estentity = context_M.Estimate.FirstOrDefault(est =>
                        est.intJobId == estdataWithJobId.intJobId && est.intId == estdataWithJobId.intId &&
                        est.intPkWorkflow == piwentity.intPkWorkflow);
                    if (
                        estentity != null
                        )
                    {
                        //                                  //Get Prices For this estimate.
                        List<PriceentityPriceEntityDB> darrpriceentityFromEstimate = context_M.Price.Where(price =>
                            price.intnPkEstimate == estentity.intPk).ToList();

                        foreach(PriceentityPriceEntityDB priceentityFromEstimate in darrpriceentityFromEstimate)
                        {
                            //                              //Remove each price of the estimate.
                            context_M.Price.Remove(priceentityFromEstimate);
                        }

                        context_M.Estimate.Remove(estentity);
                        context_M.SaveChanges();
                    }
                }
                context_M.SaveChanges();

            }
            else if (
                //                                          //It's a template.
                eleentity_I.boolIsTemplate
                )
            {
                ResResource.subDeleteEstimationWhenDeleteTemplate(context_M, eleentity_I);
            }
            /*END-CASE*/
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static bool boolResourceIsInWorkflowsWithThisResourceAndJobsInProgressOrCompleted(
            //                                                  //Check is we need to create a copy of the workflow.

            Odyssey2Context context_I,
            EleentityElementEntityDB eleentity_I,
            out List<WfentityWorkflowEntityDB> darrwfentity_O
            )
        {
            bool boolIsIn = false;
            darrwfentity_O = new List<WfentityWorkflowEntityDB>();

            List<WfentityWorkflowEntityDB> darrwfentity = new List<WfentityWorkflowEntityDB>();
            if (
                //                                          //Is a resource.
                !eleentity_I.boolIsTemplate
                )
            {
                //                                          //Get the wfjob where the resource was set directly.
                List<WfentityWorkflowEntityDB> darrwfentityFromInputsAndOutputsForAJob = (
                    from iojentity in context_I.InputsAndOutputsForAJob
                    join jobentity in context_I.Job
                    on iojentity.intJobId equals jobentity.intJobID
                    join wfentity in context_I.Workflow
                    on jobentity.intPkWorkflow equals wfentity.intPk
                    where iojentity.intPkResource == eleentity_I.intPk &&
                    wfentity.boolDeleted == false
                    select wfentity).Distinct().ToList();

                darrwfentity.AddRange(darrwfentityFromInputsAndOutputsForAJob);

                //                                          //Get the workflows where the resource is set and they have
                //                                          //      a job.
                List<WfentityWorkflowEntityDB> darrwfentityFromInputsAndOutputs =
                    (from wfentity in context_I.Workflow
                     join ioentity in context_I.InputsAndOutputs
                     on wfentity.intPk equals ioentity.intPkWorkflow
                     join jobentity in context_I.Job
                     on wfentity.intPk equals jobentity.intPkWorkflow
                     where ioentity.intnPkResource == eleentity_I.intPk &&
                     wfentity.boolDeleted == false
                     select wfentity).ToList();

                darrwfentity.AddRange(darrwfentityFromInputsAndOutputs);

                boolIsIn = (darrwfentityFromInputsAndOutputsForAJob.Count > 0) ||
                            (darrwfentityFromInputsAndOutputs.Count > 0);
            }
            else
            {
                darrwfentity =
                    (from wfentity in context_I.Workflow
                     join piwentity in context_I.ProcessInWorkflow
                     on wfentity.intPk equals piwentity.intPkWorkflow
                     join eleeleentity in context_I.ElementElement
                     on piwentity.intPkProcess equals eleeleentity.intPkElementDad
                     join jobentity in context_I.Job
                     on wfentity.intPk equals jobentity.intPkWorkflow
                     where eleeleentity.intPkElementSon == eleentity_I.intPk &&
                     wfentity.boolDeleted == false
                     select wfentity).ToList();

                boolIsIn = darrwfentity.Count > 0;
            }

            darrwfentity_O = darrwfentity.Distinct().ToList();

            return boolIsIn;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void SubDeleteResourcePhysically(
            //                                              //Delete resource when is not necessary to clone 
            //                                              //      the workflow.

            //                                              //PkResource to delete.
            //                                              //Method that delete a resource from the db.
            //                                              //1. Delete the fathers of templates or resources
            //                                              //      that are inheritors.
            //                                              //2. Delete the fathers of values that are inheritors of 
            //                                              //      values of this resource or template.
            //                                              //3. Delete the values of the resource or template.
            //                                              //4. Delete all the ascendants associated to the resource
            //                                              //      or template.
            //                                              //5. Delete paper trans associated to the res.
            //                                              //6. Delete the calculations associated to the res.
            //                                              //7. Delete the entry in InputsAndOutputs, this means:
            //                                              //      a) Delete the row if there is no link.
            //                                              //      b) Delete the foreign key if there is a link.
            //                                              //8. Delete the resource or template.
            //                                              //9. If the type has not more elements associated, delete
            //                                              //      the type.
            //                                              //10. Delete res time.
            //                                              //11. Delete the cost associated with this resource.
            //                                              //12. Delete the alerts associated to this resource.
            //                                              //13. Delete all periods relate with this resource.
            //                                              //14. Delete the resource or template.
            int intPk_I,
            String strPrintshopId_I,
            IHubContext<ConnectionHub> iHubContext_I,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //1. Delete the fathers of templates of resources
            //                                              //      that are inheritors.
            //                                              //Get the templates or resources that are inheritors.
            IQueryable<EleentityElementEntityDB> seteleentity = context_M.Element.Where(eleentity =>
                eleentity.intnPkElementInherited == intPk_I);
            List<EleentityElementEntityDB> darreleentityInheritors = seteleentity.ToList();

            //                                              //Delete the fathers.
            foreach (EleentityElementEntityDB eleentity1 in darreleentityInheritors)
            {
                eleentity1.intnPkElementInherited = null;
            }

            //                                              //2. Delete the fathers of values that are inheritors of 
            //                                              //      values of this resource or template.

            //                                              //Get the values that are related to the res.
            IQueryable<ValentityValueEntityDB> setvalentity = context_M.Value.Where(valentity =>
                valentity.intPkElement == intPk_I);
            List<ValentityValueEntityDB> darrvalentity = setvalentity.ToList();

            //                                              //Get the values that are inheritors of this values.
            List<ValentityValueEntityDB> darrvalentityInheritors = new List<ValentityValueEntityDB>();
            foreach (ValentityValueEntityDB valentity in darrvalentity)
            {
                IQueryable<ValentityValueEntityDB> setvalentityInheritors = context_M.Value.Where(val =>
                    val.intnPkValueInherited == valentity.intPk);

                darrvalentityInheritors.AddRange(setvalentityInheritors.ToList());
            }

            //                                              //Delete the fathers.
            foreach (ValentityValueEntityDB valentity in darrvalentityInheritors)
            {
                valentity.intnPkValueInherited = null;
            }
            context_M.SaveChanges();

            //                                              //3. Delete the values of the resource or template.
            foreach (ValentityValueEntityDB valentity in darrvalentity)
            {
                context_M.Value.Remove(valentity);
            }
            context_M.SaveChanges();

            //                                              //4. Delete all the ascendants associated to the resource
            //                                              //      or template.
            //                                              //Get the ascendants that are related to the res.
            IQueryable<AscentityAscendantsEntityDB> setascentity = context_M.Ascendants.Where(ascentity =>
                ascentity.intPkElement == intPk_I);
            List<AscentityAscendantsEntityDB> darrascentity = setascentity.ToList();

            //                                              //Delete the ascendants.
            foreach (AscentityAscendantsEntityDB ascentity in darrascentity)
            {
                context_M.Ascendants.Remove(ascentity);
            }
            context_M.SaveChanges();

            //                                              //5. Delete paper trans associated to the res.
            List<PatransPaperTransformationEntityDB> darrpapertransentityToDelete = context_M.PaperTransformation.Where(
                paper => paper.intPkResourceI == intPk_I || paper.intnPkResourceO == intPk_I).ToList();

            foreach (PatransPaperTransformationEntityDB patransentityToDelete in darrpapertransentityToDelete)
            {
                context_M.PaperTransformation.Remove(patransentityToDelete);
            }
            context_M.SaveChanges();

            //                                              //6. Delete the calculations associated to the res also 
            //                                              //      paper trans related to the calculations.

            //                                              //Get the calculations that are related to the res.
            List<CalentityCalculationEntityDB> darrcalentity = context_M.Calculation.Where(calentity =>
                calentity.intnPkResource == intPk_I ||
                calentity.intnPkQFromResource == intPk_I
                ).ToList();

            //                                              //Delete the calculations.
            foreach (CalentityCalculationEntityDB calentity in darrcalentity)
            {
                //                                          //Delete paper transformations related to calculations.
                List<PatransPaperTransformationEntityDB> darrpapertransentity = context_M.PaperTransformation.Where(
                    paper => 
                    paper.intnPkCalculationOwn == calentity.intPk ||
                    paper.intnPkCalculationLink == calentity.intPk
                    ).ToList();

                foreach (PatransPaperTransformationEntityDB patransentity in darrpapertransentity)
                {
                    context_M.PaperTransformation.Remove(patransentity);
                }

                Tools.subDeleteCondition(calentity.intPk, null, null, null, context_M);
                context_M.Calculation.Remove(calentity);
            }
            context_M.SaveChanges();

            //                                              //7. Delete transform calculations associated to the res.

            //                                              //Find transform calculation.
            List<TrfcalentityTransformCalculationEntityDB> darrtrfcalentity = context_M.TransformCalculation.Where(
                trf => 
                trf.intPkResourceI == intPk_I ||
                trf.intPkResourceO == intPk_I
                ).ToList();

            //                                              //Delete transform calculations.
            foreach (TrfcalentityTransformCalculationEntityDB trfcalentity in darrtrfcalentity)
            {
                Tools.subDeleteCondition(null, null, null, trfcalentity.intPk, context_M);
                context_M.TransformCalculation.Remove(trfcalentity);
            }
            context_M.SaveChanges();

            //                                              //5.5. Delete resource from groups.
            ResResource.subDeleteResourceFromGroup(intPk_I, context_M);

            //                                              //8. Delete IOs e IOsj.
            ResResource.subDeleteIOS(intPk_I, context_M);

            //                                              //Places where this resource(template) is IO.
            List<EleeleentityElementElementEntityDB> darreleeleentity = context_M.ElementElement.Where(
                eleele => eleele.intPkElementSon == intPk_I).ToList();

            foreach (EleeleentityElementElementEntityDB eleeleentity in darreleeleentity)
            {
                //                                          //Delete EstimationData entries.            
                if (
                    eleeleentity.boolUsage == true
                    )
                {
                    //                                      //Find process in PIW table.
                    IQueryable<PiwentityProcessInWorkflowEntityDB> setpiwentity = context_M.ProcessInWorkflow.Where(piw =>
                        piw.intPkProcess == eleeleentity.intPkElementDad);
                    List<PiwentityProcessInWorkflowEntityDB> darrpiwentityAll = setpiwentity.ToList();

                    //                                      //Delete EstimationData entries.
                    JobJob.subDeleteEstimationDataEntriesForAWorkflow(context_M, darrpiwentityAll);
                }

                //                                          //Delete IO´s e IOj's with the pkeleele where this resource 
                //                                          //      is IO.
                ResResource.subDeleteIOsOfThisPkeleele(eleeleentity.intPk, context_M);

                context_M.ElementElement.Remove(eleeleentity);
                context_M.SaveChanges();
            }

            //                                              //9. Delete res time.
            //                                              //Get register from time table.
            List<TimeentityTimeEntityDB> darrtimeentity = context_M.Time.Where(time => 
                time.intPkResource == intPk_I).ToList();
            foreach(TimeentityTimeEntityDB timeentity in darrtimeentity)
            {
                context_M.Time.Remove(timeentity);                
            }
            context_M.SaveChanges();

            //                                              //10. Delete the cost associated with this resource.
            CostentityCostEntityDB costentity = context_M.Cost.FirstOrDefault(cost => cost.intPkResource == intPk_I);

            if (
                costentity != null
                )
            {
                //                                          //Change the cost that has the resource cost as 
                //                                          //      inheritance.
                List<CostentityCostEntityDB> darrcostentity = context_M.Cost.Where(cost =>
                cost.intnPkCostInherited == costentity.intPk).ToList();
                foreach (CostentityCostEntityDB costentityToChange in darrcostentity)
                {
                    costentityToChange.intnPkCostInherited = null;
                    context_M.Update(costentityToChange);
                    context_M.SaveChanges();
                }

                context_M.Cost.Remove(costentity);
                context_M.SaveChanges();
            }

            //                                              //11. Delete the alerts associated to this resource.

            //                                              //Find alert type related to resources.
            AlerttypeentityAlertTypeEntityDB alerttypeentity = context_M.AlertType.FirstOrDefault(alerttype =>
                alerttype.strType == AlerttypeentityAlertTypeEntityDB.strAvailability);

            List<AlertentityAlertEntityDB> darralertentity = context_M.Alert.Where(alert =>
                alert.intnPkResource == intPk_I && alert.intPkAlertType == alerttypeentity.intPk).ToList();

            //                                              //Get all contactIds from printshop.
            int[] arrintContactIdsFromPrintshop = PsPrintShop.arrintGetAllContactIdsFromPrintshop(
                strPrintshopId_I, ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);

            foreach (AlertentityAlertEntityDB alertentity in darralertentity)
            {
                foreach (int intContactId in arrintContactIdsFromPrintshop)
                {
                    if (
                        //                                  //Notification not read.
                        !PsPrintShop.boolNotificationReadByUser(alertentity, intContactId)
                        )
                    {
                        //                                  //Reduce the notification to specific contacts.
                        AlnotAlertNotification.subReduceToOne(intContactId, iHubContext_I);
                    }
                }

                context_M.Alert.Remove(alertentity);
            }

            //                                              //12. Delete all periods relate with this resource.
            ResResource.subDeletePeriodsRelateToResource(intPk_I, context_M);

            //                                              //13. Delete the resource or template.
            EleentityElementEntityDB eleentity = context_M.Element.FirstOrDefault(ele => ele.intPk == intPk_I);
            context_M.Element.Remove(eleentity);
            context_M.SaveChanges();

            intStatus_IO = 200;
            if (
                eleentity.boolIsTemplate
                )
            {
                strUserMessage_IO = "Template deleted.";
            }
            else
            {
                strUserMessage_IO = "Resource deleted.";
            }

            //                                              //13. If the type has not more:
            //                                              //      -Resources
            //                                              //      -Calculations
            //                                              //      -Links
            //                                              //  Then delete the type.

            //                                              //To easy code. 
            int intTypePk = eleentity.intPkElementType;
            bool boolResTypeIsDispensable = RestypResourceType.boolIsDispensable(intTypePk, context_M);

            if (
                //                                          //Is dispensable
                boolResTypeIsDispensable
                )
            {
                //                                          //Remove the type from the printshop.
                int intStatus;
                String strUserMessage;
                int intXJDFTypePk;
                Odyssey2.subRemoveTypeFromPrintshop(intTypePk, context_M, out intStatus, out strUserMessage,
                    out intXJDFTypePk);
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subDeletePeriodsRelateToResource(
            //                                              //Find and delete periods related to this resource.

            int intPkResource_I,
            Odyssey2Context context_I
            )
        {
            //                                              //Find temporary periods.
            List<PerentityPeriodEntityDB> darrperentity = context_I.Period.Where(per =>
                per.intPkElement == intPkResource_I).ToList();

            foreach (PerentityPeriodEntityDB perentity in darrperentity)
            {
                context_I.Period.Remove(perentity);
                context_I.SaveChanges();
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static void subDeleteResourceFromGroup(
            int intPk_I,
            Odyssey2Context context_I
            )
        {
            //                                              //Get Group resource per WF.
            List<GpresentityGroupResourceEntityDB> darrgpresentityGroupResourcePerWorkflow =
                context_I.GroupResource.Where(gpres => gpres.intPkResource == intPk_I).ToList();

            foreach (GpresentityGroupResourceEntityDB gpresentity in darrgpresentityGroupResourcePerWorkflow)
            {
                List<GpresentityGroupResourceEntityDB> darrgpentity = (
                    from gpentity in context_I.GroupResource
                    where gpentity.intId == gpresentity.intId &&
                    gpentity.intPkWorkflow == gpresentity.intPkWorkflow
                    select gpentity
                    ).ToList();

                if (
                    //                                      //Group with only two resources.
                    darrgpentity.Count == 2
                    )
                {
                    //                                      //Resource to delete.
                    GpresentityGroupResourceEntityDB gpentityToDelete = darrgpentity.FirstOrDefault(
                        gpentity => gpentity.intPkResource == intPk_I && 
                        gpentity.intPkWorkflow == gpresentity.intPkWorkflow);

                    context_I.GroupResource.Remove(gpentityToDelete);
                    context_I.SaveChanges();

                    //                                      //Resource not to delete.
                    GpresentityGroupResourceEntityDB gpentityNotToDelete = darrgpentity.FirstOrDefault(
                        gpentity => gpentity.intPkResource != intPk_I 
                        && gpentity.intPkWorkflow == gpresentity.intPkWorkflow);

                    int intPkResourceNotToDelete = gpentityNotToDelete.intPkResource;

                    context_I.GroupResource.Remove(gpentityNotToDelete);
                    context_I.SaveChanges();

                    //                                      //Change de IO table, gp por res.

                    //                                      //IO to change,
                    IoentityInputsAndOutputsEntityDB ioentityToChange = context_I.InputsAndOutputs.FirstOrDefault(
                        ioentity => ioentity.intnGroupResourceId == gpresentity.intId && 
                        ioentity.intPkWorkflow == gpresentity.intPkWorkflow);
                    if (
                        ioentityToChange.strLink != null
                        )
                    {
                        //                                  //IOs with link.
                        IQueryable<IoentityInputsAndOutputsEntityDB> setioentityLink =
                            from ioentityLink in context_I.InputsAndOutputs
                            where ioentityLink.strLink == ioentityToChange.strLink &&
                            ioentityLink.intnGroupResourceId == ioentityToChange.intnGroupResourceId &&
                            ioentityLink.intPkWorkflow == ioentityToChange.intPkWorkflow
                            select ioentityLink;

                        List<IoentityInputsAndOutputsEntityDB> darrioentityLink = setioentityLink.ToList();

                        foreach (IoentityInputsAndOutputsEntityDB ioentityLink in darrioentityLink)
                        {
                            ioentityLink.intnPkResource = intPkResourceNotToDelete;
                            ioentityLink.intnGroupResourceId = null;
                        }
                    }
                    else
                    {
                        ioentityToChange.intnPkResource = intPkResourceNotToDelete;
                        ioentityToChange.intnGroupResourceId = null;
                    }
                }
                else
                {
                    //                                      //Group with more than two resources.

                    //                                      //Resource to delete.
                    GpresentityGroupResourceEntityDB gpentityToDelete = darrgpentity.FirstOrDefault(
                        gpentity => gpentity.intPkResource == intPk_I &&
                        gpentity.intPkWorkflow == gpresentity.intPkWorkflow);

                    context_I.GroupResource.Remove(gpentityToDelete);
                }
                context_I.SaveChanges();
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subDeleteIOS(

            int intPkResource_I,
            Odyssey2Context context_I

            )
        {
            //                                              //Delete the InputsAndOutputs.
            List<IoentityInputsAndOutputsEntityDB> darrioentity = context_I.InputsAndOutputs.Where(io =>
                io.intnPkResource == intPkResource_I).ToList();

            foreach (IoentityInputsAndOutputsEntityDB ioentity in darrioentity)
            {
                if (
                    ioentity.intnGroupResourceId == null
                    )
                {
                    if (
                        ioentity.strLink == null
                        )
                    {
                        Tools.subDeleteCondition(null, null, ioentity.intPk, null, context_I);
                        context_I.InputsAndOutputs.Remove(ioentity);
                    }
                    else
                    {
                        ioentity.intnPkResource = null;
                       
                    }
                }
            }
            context_I.SaveChanges();

            //                                              //Delete the InputsAndOutputsFor a Job
            List<IojentityInputsAndOutputsForAJobEntityDB> darriojentity = context_I.InputsAndOutputsForAJob.Where(
                ioj => ioj.intPkResource == intPkResource_I).ToList();

            foreach (IojentityInputsAndOutputsForAJobEntityDB iojentity in darriojentity)
            {
                context_I.InputsAndOutputsForAJob.Remove(iojentity);
                context_I.SaveChanges();
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subDeleteIOsOfThisPkeleele(
            int intPkeleele_I,
            Odyssey2Context context_M
            )
        {
            //                                              //Delete the InputsAndOutputs.
            List<IoentityInputsAndOutputsEntityDB> darrioentity = context_M.InputsAndOutputs.Where(
                io => io.intnPkElementElement == intPkeleele_I).ToList();

            foreach (IoentityInputsAndOutputsEntityDB ioentity in darrioentity)
            {
                Tools.subDeleteCondition(null, null, ioentity.intPk, null, context_M);
                context_M.InputsAndOutputs.Remove(ioentity);
            }
            context_M.SaveChanges();

            //                                              //Delete the InputsAndOutputsFor a Job
            List<IojentityInputsAndOutputsForAJobEntityDB> darriojentity = context_M.InputsAndOutputsForAJob.Where(
                ioj => ioj.intnPkElementElement == intPkeleele_I).ToList();

            foreach (IojentityInputsAndOutputsForAJobEntityDB iojentity in darriojentity)
            {
                context_M.InputsAndOutputsForAJob.Remove(iojentity);
            }
            context_M.SaveChanges();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subDeleteResourceLogically(
            //                                              //Delete the reference to resource in the workflows and 
            //                                              //      delete the resource logically.

            EleentityElementEntityDB eleentity_I,
            String strPrintshopId_I,
            List<WfentityWorkflowEntityDB> darrwfentityNew_I,
            IHubContext<ConnectionHub> iHubContext_I,
            Odyssey2Context context_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            if (
                //                                          //It is a resource.
                !eleentity_I.boolIsTemplate
                )
            {
                foreach (WfentityWorkflowEntityDB wfentity in darrwfentityNew_I)
                {
                    List<IoentityInputsAndOutputsEntityDB> darrioentity = context_I.InputsAndOutputs.Where(io =>
                        io.intnPkResource == eleentity_I.intPk &&
                        io.intPkWorkflow == wfentity.intPk
                        ).ToList();

                    foreach (IoentityInputsAndOutputsEntityDB ioentity in darrioentity)
                    {
                        Tools.subDeleteCondition(null, null, ioentity.intPk, null, context_I);
                        context_I.InputsAndOutputs.Remove(ioentity);
                    }
                    context_I.SaveChanges();
                }

                //                                          //Delete the InputsAndOutputsForaJob pending. 
                ResResource.subDeleteIOjForPendingJobs(eleentity_I.intPk, context_I);

                EleentityElementEntityDB eleentityResource = context_I.Element.FirstOrDefault(ele =>
                    ele.intPk == eleentity_I.intPk);
                eleentityResource.boolDeleted = true;
                eleentityResource.strEndDate = Date.Now(ZonedTimeTools.timezone).ToString();
                eleentityResource.strEndTime = Time.Now(ZonedTimeTools.timezone).ToString();
                context_I.Element.Update(eleentityResource);
                context_I.SaveChanges();

                //                                          //Delete resource from groups.
                ResResource.subDeleteResourceFromGroup(eleentity_I.intPk, context_I);
            }
            else
            {
                //                                          //It is a template.
                List<EleeleentityElementElementEntityDB> darreleeleentity =
                    (from wfentity in context_I.Workflow
                     join piwentity in context_I.ProcessInWorkflow
                     on wfentity.intPk equals piwentity.intPkWorkflow
                     join eleeleentity in context_I.ElementElement
                     on piwentity.intPkProcess equals eleeleentity.intPkElementDad
                     join jobentity in context_I.Job
                     on wfentity.intPk equals jobentity.intPkWorkflow
                     where eleeleentity.intPkElementSon == eleentity_I.intPk
                     select eleeleentity).ToList();

                foreach (EleeleentityElementElementEntityDB eleeleentity in darreleeleentity)
                {
                    ProProcess pro = ProProcess.proFromDB(eleeleentity.intPkElementDad);
                    ProProcess proNew;
                    ProtypProcessType.subCopyAProcess(pro, context_I, out proNew);
                    ProtypProcessType.subUpdateProcessPkInActiveWorkflows(pro, proNew, context_I);

                    EleeleentityElementElementEntityDB eleeleentityOfNewProcess = 
                        context_I.ElementElement.FirstOrDefault(
                        eleele => eleele.intPkElementDad == proNew.intPk &&
                        eleele.intPkElementSon == eleentity_I.intPk);

                    //                                      //Find IOs to delete.
                    List<IoentityInputsAndOutputsEntityDB> darrioentity = context_I.InputsAndOutputs.Where(io =>
                        io.intnPkElementElement == eleeleentityOfNewProcess.intPk).ToList();

                    foreach (IoentityInputsAndOutputsEntityDB ioentity in darrioentity)
                    {
                        Tools.subDeleteCondition(null, null, ioentity.intPk, null, context_I);
                        context_I.InputsAndOutputs.Remove(ioentity);
                    }
                    context_I.SaveChanges();

                    //                                      //Find IOJs to delete.
                    List<IojentityInputsAndOutputsForAJobEntityDB> darriojentityAll = context_I.InputsAndOutputsForAJob.
                        Where(ioj => ioj.intnPkElementElement == eleeleentityOfNewProcess.intPk).ToList();

                    foreach (IojentityInputsAndOutputsForAJobEntityDB iojentity in darriojentityAll)
                    {
                        context_I.InputsAndOutputsForAJob.Remove(iojentity);
                    }
                    context_I.SaveChanges();

                    context_I.ElementElement.Remove(eleeleentityOfNewProcess);
                    context_I.SaveChanges();
                }

                EleentityElementEntityDB eleentityResource = context_I.Element.FirstOrDefault(ele =>
                    ele.intPk == eleentity_I.intPk);

                eleentityResource.boolDeleted = true;
                eleentityResource.strEndDate = Date.Now(ZonedTimeTools.timezone).ToString();
                eleentityResource.strEndTime = Time.Now(ZonedTimeTools.timezone).ToString();
                context_I.Element.Update(eleentityResource);
                context_I.SaveChanges();
            }

            //                                              //11. Delete the alerts associated with this resource.

            //                                              //Find alert type related to resource.
            AlerttypeentityAlertTypeEntityDB alerttypeentity = context_I.AlertType.FirstOrDefault(alerttype =>
                alerttype.strType == AlerttypeentityAlertTypeEntityDB.strAvailability);

            List<AlertentityAlertEntityDB> darralertentity = context_I.Alert.Where(alert =>
                alert.intnPkResource == eleentity_I.intPk && alert.intPkAlertType == alerttypeentity.intPk).ToList();

            //                                              //Get all contactIds from printshop.
            int[] arrintContactIdsFromPrintshop = PsPrintShop.arrintGetAllContactIdsFromPrintshop(
                strPrintshopId_I, ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);

            foreach (AlertentityAlertEntityDB alertentity in darralertentity)
            {
                foreach (int intContactId in arrintContactIdsFromPrintshop)
                {
                    if (
                        //                              //Notification not read by user.
                        !PsPrintShop.boolNotificationReadByUser(alertentity, intContactId)
                        )
                    {
                        //                              //Reduce the notification to specific contacts.
                        AlnotAlertNotification.subReduceToOne(intContactId, iHubContext_I);
                    }
                }

                context_I.Alert.Remove(alertentity);
            }

            intStatus_IO = 200;
            strUserMessage_IO = "Success.";
            strDevMessage_IO = "Delete Resource Logically";
        }
        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subDeleteIOjForPendingJobs(
            int intPkResource_I,
            Odyssey2Context context_M
            )
        {
            List<IojentityInputsAndOutputsForAJobEntityDB> darriojentityForThisResource = (
                from iojentity in context_M.InputsAndOutputsForAJob
                where iojentity.intPkResource == intPkResource_I
                select iojentity
                ).ToList();

            foreach (IojentityInputsAndOutputsForAJobEntityDB iojentity in darriojentityForThisResource)
            {
                JobentityJobEntityDB jobentity = context_M.Job.FirstOrDefault(job => 
                    job.intJobID == iojentity.intJobId);
                if (
                    //                                      //Job is pending.
                    jobentity == null
                    )
                {
                    context_M.InputsAndOutputsForAJob.Remove(iojentity);
                }
            }
            context_M.SaveChanges();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subDeleteEstimationWhenDeleteTemplate(
            Odyssey2Context context_M,
            EleentityElementEntityDB eleentity_I
            )
        {
            //                                              //Get list of workflow to make history.
            List<WfentityWorkflowEntityDB> darrWorkflowsResourceIsIn;
            ResResource.boolResourceIsInWorkflowsWithThisResourceAndJobsInProgressOrCompleted(context_M, eleentity_I,
                out darrWorkflowsResourceIsIn);

            foreach (WfentityWorkflowEntityDB wfToCheck in darrWorkflowsResourceIsIn)
            {
                List<EleeleentityElementElementEntityDB> darreleeleentity =
                    (from wfentity in context_M.Workflow
                     join piwentity in context_M.ProcessInWorkflow
                     on wfentity.intPk equals piwentity.intPkWorkflow
                     join eleeleentity in context_M.ElementElement
                     on piwentity.intPkProcess equals eleeleentity.intPkElementDad
                     join jobentity in context_M.Job
                     on wfentity.intPk equals jobentity.intPkWorkflow
                     where eleeleentity.intPkElementSon == eleentity_I.intPk
                     select eleeleentity).ToList();

                /*UNTIL-WHILE*/
                bool boolIsIn = false;
                int intI = 0;
                while (
                    boolIsIn == false &&
                    intI < darreleeleentity.Count()
                    )
                {
                    //                                      //To easy code.
                    EleeleentityElementElementEntityDB eleeleentity = darreleeleentity[intI];
                    if (
                        eleeleentity.boolUsage == true
                        )
                    {
                        //                                  //Find process in PIW table.
                        IQueryable<PiwentityProcessInWorkflowEntityDB> setpiwentity = context_M.ProcessInWorkflow.Where(
                            piw => piw.intPkProcess == eleeleentity.intPkElementDad);
                        List<PiwentityProcessInWorkflowEntityDB> darrpiwentityAll = setpiwentity.ToList();

                        //                                  //Delete EstimationData entries.
                        JobJob.subDeleteEstimationDataEntriesForAWorkflow(context_M, darrpiwentityAll);
                        boolIsIn = true;
                    }
                    intI = intI + 1;
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subEdit(
            //                                              //Modify the attribues, name, unit and inheritance data.

            //                                              //Pk of the resource to edit.
            int intPkResource_I,
            //                                              //New/same name of the resource.
            String strName_I,
            //                                              //New/same unit of the resource.
            String strUnit_I,
            //                                              //Unit allows decimal.
            bool? boolnIsDecimal_I,
            //                                              //Inheritance cost.
            double? numnCost_I,
            //                                              //Cost quantity.
            double? numnQuantity_I,
            //                                              //Cost min.
            double? numnMin_I,
            //                                              //Cost block.
            double? numnBlock_I,
            //                                              //Account.
            int? intnPkAccount_I,
            //                                              //HourlyRate.
            double? numnHourlyRate_I,
            //                                              //Boolean Area.
            bool? boolnArea_I,
            //                                              //Is cost inherited.
            bool? boolnCostInherited_I,
            //                                              //Is cost changeable.
            bool? boolnCostChangeable_I,
            //                                              //Unit of measurement.
            String strUnitInherited_I,
            //                                              //Is unit inherited.
            bool? boolnUnitInherited_I,
            //                                              //Is unit changeable.
            bool? boolnUnitChangeable_I,
            //                                              //Type of availability - calendarized.
            bool? boolnCalendarized_I,
            //                                              //Is availabily inherited.
            bool? boolnAvailabilityInherited_I,
            //                                              //Is availability changeable.
            bool? boolnAvailabilityChangeable_I,
            //                                              //Json like this:
            //                                              //      {
            //                                              //          "strAscendant":"883",
            //                                              //          "strValue":"SheetFedConventionalPress",
            //                                              //          "intnPkInheritedValue":2,
            //                                              //          "boolChangeable":false,
            //                                              //          "intnPKValueToDeleteToAddANewOne":"3",
            //                                              //      }
            Attrjson5AttributeJson5[] arrattrjson5_I,
            PsPrintShop ps_I,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO,
            out Pejson1PathElementJson1 pejson1_O
            )
        {
            //                                              //Resource to edit.
            EleentityElementEntityDB eleentityResource = context_M.Element.FirstOrDefault(ele =>
                ele.intPk == intPkResource_I);

            intStatus_IO = 402;
            strUserMessage_IO = "Resource not found.";
            strDevMessage_IO = "";
            pejson1_O = null;
            if (
                eleentityResource != null
                )
            {
                ResResource resToEdit = resFromDB(context_M, eleentityResource.intPk, 
                    eleentityResource.boolIsTemplate);

                RestypResourceType restyp = (RestypResourceType)EtElementTypeAbstract.etFromDB(context_M, 
                    eleentityResource.intPkElementType);

                //                              //Attributes for Resource LengthUnit;
                List<AttrentityAttributeEntityDB> darrattrResource =
                    (from attrentity in context_M.Attribute
                     join attretentity in context_M.AttributeElementType
                     on attrentity.intPk equals attretentity.intPkAttribute
                     where attretentity.intPkElementType == eleentityResource.intPkElementType
                     select attrentity).ToList();

                //                              //Get the MediaUnit attribute.
                AttrentityAttributeEntityDB attrentityMediaUnit1 = darrattrResource.FirstOrDefault(a =>
                    a.strXJDFName == "MediaUnit" || a.strCustomName == "XJDFMediaUnit");

                //                              //Get the LengthUnit attribute.
                AttrentityAttributeEntityDB attrentityLength = darrattrResource.FirstOrDefault(a =>
                    a.strXJDFName == "LengthUnit" || a.strCustomName == "XJDFLengthUnit");

                bool boolIsRoll = false;
                if (
                    attrentityMediaUnit1 != null && attrentityLength != null
                    )
                {
                    //                                  //Verify if exist the MediaUnit.
                    Attrjson5AttributeJson5 attrjson5MediaUnit = arrattrjson5_I.FirstOrDefault(attr =>
                        attr.strAscendant == "" + attrentityMediaUnit1.intPk);

                    //                                  //Verify if exist the LengthUnit.
                    Attrjson5AttributeJson5 attrjson5LengthUnit = arrattrjson5_I.FirstOrDefault(attr =>
                    attr.strAscendant == "" + attrentityLength.intPk);

                    boolIsRoll = (attrjson5MediaUnit != null) ? attrjson5MediaUnit.strValue == "Roll" : false;

                    //                                  //Assig the new UnitOfMeasurement.
                    strUnit_I = restyp.strCustomTypeId == "XJDFMedia" &&
                        boolIsRoll && attrjson5LengthUnit != null ? attrjson5LengthUnit.strValue : strUnit_I;
                }

                intStatus_IO = 403;
                strUserMessage_IO = "Invalid name or unit.";
                if (
                    ResResource.boolIsValidEditData(strName_I, strUnit_I, resToEdit, arrattrjson5_I, context_M,
                        ref strUserMessage_IO)
                    )
                {
                    int? intnPkDad = resToEdit.resinherited != null ? (int?)resToEdit.resinherited.intPk : null;

                    intStatus_IO = 403;
                    strUserMessage_IO = "Invalid inheritance data.";
                    if (
                        ResResource.boolIsInheritanceDataValid(numnCost_I, numnQuantity_I, numnMin_I,intnPkAccount_I,
                            numnHourlyRate_I, boolnArea_I, strUnit_I, strUnitInherited_I, boolnCalendarized_I,
                            resToEdit.restypBelongsTo, intnPkDad, boolnCostInherited_I, boolnCostChangeable_I,
                            boolnUnitInherited_I, boolnUnitChangeable_I, boolnAvailabilityInherited_I,
                            boolnAvailabilityChangeable_I, restyp.strClassification, context_M)
                        )
                    {
                        intStatus_IO = 403;
                        strUserMessage_IO = "Attributes invalid.";
                        if (
                            ResResource.boolHasValidAttributes(arrattrjson5_I, restyp, context_M)
                            )
                        {
                            //                              //Update the name.
                            eleentityResource.strElementName = strName_I;
                            context_M.Element.Update(eleentityResource);
                            context_M.SaveChanges();

                            //                              //Get the current attributes to get the name and unit
                            //                              //      attribute.
                            IQueryable<AttrentityAttributeEntityDB> setattr =
                                from attrentity in context_M.Attribute
                                join attretentity in context_M.AttributeElementType
                                on attrentity.intPk equals attretentity.intPkAttribute
                                where attretentity.intPkElementType == eleentityResource.intPkElementType
                                select attrentity;
                            List<AttrentityAttributeEntityDB> darrattr = setattr.ToList();

                            //                              //Get the name attribute.
                            AttrentityAttributeEntityDB attrentityName = darrattr.FirstOrDefault(a =>
                                a.strXJDFName == "Name");
                            int intPkAttributeName = attrentityName.intPk;

                            //                              //Get the unit attribute.
                            AttrentityAttributeEntityDB attrentityUnit = darrattr.FirstOrDefault(a =>
                                a.strXJDFName == "Unit");
                            int intPkAttributeUnit = attrentityUnit.intPk;

                            //                              //Get the MediaUnit attribute.
                            AttrentityAttributeEntityDB attrentityMediaUnit = darrattr.FirstOrDefault(a =>
                                a.strXJDFName == "MediaUnit");

                            int? intnPkAttributeMediaUnit = attrentityMediaUnit != null ? (int?)attrentityMediaUnit.intPk :
                                null;

                            //                              //Get the current ascendants.
                            List<AscentityAscendantsEntityDB> darrascentity = context_M.Ascendants.Where(asc =>
                                asc.intPkElement == intPkResource_I).ToList();

                            bool boolExistValueMediaUnitInDB = false;
                            String strValueMediaUnit = null;
                            ValentityValueEntityDB valentityMediaUnit;

                            //                              //Delete de ascendants.
                            foreach (AscentityAscendantsEntityDB ascentity in darrascentity)
                            {
                                if (
                                    //                      //Not exist Media Unit.
                                    !boolExistValueMediaUnitInDB
                                    )
                                {
                                    boolExistValueMediaUnitInDB = intnPkAttributeMediaUnit != null ?
                                    ascentity.strAscendants.EndsWith("" + intnPkAttributeMediaUnit) : false;
                                    if (
                                        boolExistValueMediaUnitInDB
                                        )
                                    {
                                        strValueMediaUnit = context_M.Value.FirstOrDefault(val =>
                                            val.intPkAttribute == intnPkAttributeMediaUnit && 
                                            val.intPkElement == eleentityResource.intPk).strValue;
                                    }
                                }

                                if (
                                    !ascentity.strAscendants.EndsWith("" + intPkAttributeName) &&
                                    !ascentity.strAscendants.EndsWith("" + intPkAttributeUnit)
                                    )
                                {
                                    context_M.Ascendants.Remove(ascentity);
                                }
                            }

                            //                              //Get the current values before the new ones are added.
                            List<ValentityValueEntityDB> darrvalentityValuesToDelete = context_M.Value.Where(val =>
                                val.intPkElement == intPkResource_I).ToList();

                            ResResource res = ResResource.resFromDB(context_M, intPkResource_I, 
                                eleentityResource.boolIsTemplate);

                            //                              //Add the new attributes.
                            foreach (Attrjson5AttributeJson5 attrjson5 in arrattrjson5_I)
                            {
                                if (
                                    boolExistValueMediaUnitInDB &&
                                    attrjson5.strAscendant != null &&
                                    attrjson5.strAscendant == (intnPkAttributeMediaUnit + "")
                                    )
                                {
                                    if (
                                        attrjson5.strValue != strValueMediaUnit
                                        )
                                    {
                                        intStatus_IO = 404;
                                        strUserMessage_IO = "It is not possible change the media unit.";
                                        strDevMessage_IO = "It is not possible change the media unit.";

                                        throw new Exception("It is not possible change the media unit.");
                                    }
                                }

                                //                          //Keep the PkValue to delete and that added againg with the
                                //                          //      modification.
                                int? intnPkValueToDelete = attrjson5.intnPkValue;
                                ValentityValueEntityDB valentityToDelete = context_M.Value.FirstOrDefault(val =>
                                    val.intPk == intnPkValueToDelete);

                                //                          //Add the new atribute, the PkValue will be updated.
                                Attrjson5AttributeJson5 attrjson5ToPassAsRef = attrjson5;
                                ResResource.subAddOneAttribute(context_M, res, ref attrjson5ToPassAsRef);

                                int intPkValueNew = (int)attrjson5ToPassAsRef.intnPkValue;

                                if (
                                    //                      //It was an edition, not a new one.
                                    intnPkValueToDelete != null
                                    )
                                {
                                    int intPkValueToDelete = (int)intnPkValueToDelete;

                                    //                      //Look for children.
                                    List<ValentityValueEntityDB> darrvalentityChildren = context_M.Value.Where(val =>
                                        val.intnPkValueInherited == intPkValueToDelete).ToList();

                                    //                      //Update children to the new father.
                                    foreach (ValentityValueEntityDB valentityChild in darrvalentityChildren)
                                    {
                                        valentityChild.intnPkValueInherited = intPkValueNew;
                                        if (
                                            valentityChild.boolnIsChangeable == false &&
                                            valentityChild.strValue == valentityToDelete.strValue
                                            )
                                        {
                                            valentityChild.strValue = attrjson5.strValue;
                                        }
                                        context_M.Value.Update(valentityChild);
                                    }
                                }
                            }
                            context_M.SaveChanges();

                            //                              //Delete the current values except for name and unit.

                            //                              //bool to create the his of unit only with the first
                            //                              //      valentity for unit attribute.
                            //                              //      Can be more that one because of the his.
                            bool boolFisrtValentityForUnit = true;

                            foreach (ValentityValueEntityDB valentity in darrvalentityValuesToDelete)
                            {
                                //                          //Look for children.
                                List<ValentityValueEntityDB> darrvalentityChildren = context_M.Value.Where(val =>
                                    val.intnPkValueInherited == valentity.intPk).ToList();

                                /*CASE*/
                                if (
                                    //                      //Attribute is name attribute.
                                    valentity.intPkAttribute == intPkAttributeName
                                    )
                                {
                                    //                      //Update the name.
                                    valentity.strValue = strName_I;
                                    context_M.Value.Update(valentity);
                                }
                                else if (
                                    //                      //Attribute is unit attribute.
                                    (valentity.intPkAttribute == intPkAttributeUnit) &&
                                    boolFisrtValentityForUnit
                                    )
                                {
                                    boolFisrtValentityForUnit = false;

                                    //                      //Create history for unit of measurement.
                                    ResResource resDad = ResResource.resFromDB(context_M, intnPkDad, true);
                                    ValentityValueEntityDB valentityDad = null;
                                    if (
                                        resDad != null
                                        )
                                    {
                                        List<ValentityValueEntityDB> darrvalentityDad = context_M.Value.Where(val =>
                                        val.intPkElement == resDad.intPk &&
                                        val.intPkAttribute == valentity.intPkAttribute).ToList();
                                        darrvalentityDad.Sort();

                                        valentityDad = darrvalentityDad.Last();
                                    }

                                    int intPkLastValue;
                                    ResResource.subCreateNewUnitHistory(eleentityResource, valentityDad, valentity,
                                        strUnit_I, boolnIsDecimal_I, boolnUnitChangeable_I, boolnUnitInherited_I, 
                                        context_M, out intPkLastValue);
                                    strUserMessage_IO = "You changed your unit of measurement." +
                                        " Please verify your calculations.";

                                    //                      //Update Unit in children.
                                    ResResource.subUpdateUnitInChildren(intPkLastValue, strUnit_I, boolnIsDecimal_I,
                                        context_M);
                                }
                                else if (
                                    valentity.intPkAttribute != intPkAttributeUnit
                                    )
                                {
                                    if (
                                        darrvalentityChildren.Count > 0
                                        )
                                    {
                                        //                  //Leave the children orphans.
                                        foreach (ValentityValueEntityDB valentityToBeOrphan in darrvalentityChildren)
                                        {
                                            valentityToBeOrphan.intnPkValueInherited = null;
                                            context_M.Value.Update(valentityToBeOrphan);
                                        }
                                        strDevMessage_IO = "Some resources will not inherite from this resource any more.";
                                    }
                                    //                      //Delete the actual values.
                                    context_M.Value.Remove(valentity);
                                }
                                context_M.SaveChanges();
                            }

                            //                              //Update Inheritance data.
                            if (
                                intnPkDad != null &&
                                RestypResourceType.boolIsPhysical(resToEdit.restypBelongsTo.strClassification)
                                )
                            {
                                int intPkDad = (int)intnPkDad;

                                ResResource.subAddInheritedCost(intPkDad, res, numnCost_I, numnQuantity_I,
                                    numnMin_I, numnBlock_I, intnPkAccount_I, numnHourlyRate_I, boolnArea_I,
                                    boolnCostInherited_I, boolnCostChangeable_I, ps_I, context_M, 
                                    ref intStatus_IO, ref strUserMessage_IO,
                                    ref strDevMessage_IO);

                                ResResource.subAddInheritedAvailability(intPkDad, intPkResource_I,
                                    boolnCalendarized_I, boolnAvailabilityInherited_I,
                                    boolnAvailabilityChangeable_I, context_M);
                            }

                            //                              //Update cost and availability in children.
                            ResResource.subUpdateCostAndAvailabilityInChildren(intPkResource_I, res, null, context_M);

                            intStatus_IO = 200;
                            strUserMessage_IO = "";

                            if (
                                res.resinherited != null
                                )
                            {
                                pejson1_O = new Pejson1PathElementJson1(res.resinherited.intPk, false);
                            }
                            else
                            {
                                pejson1_O = new Pejson1PathElementJson1(res.restypBelongsTo.intPk, true);
                            }

                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static bool boolIsValidEditData(
            //                                              //Verify that the resource can be added.
            //                                              //1. The name should no be empty.
            //                                              //2. If physical, unit shouldn´t be empty.
            //                                              //3. Tha name doesn´t exit for the ps and type.
            //                                              //4. If dad, should be a template and form the same type.

            //                                              //New name.
            String strName_I,
            //                                              //New Unit.
            String strUnit_I,
            //                                              //Resource to edit.
            ResResource resToEdit_I,
            Attrjson5AttributeJson5[] arrattrjson5_I,
            Odyssey2Context context_M,
            //                                              //Updated user message.
            ref String strUserMessage_IO
            )
        {
            bool boolValidData = false;

            strUserMessage_IO = "Name cannot be empty";
            if (
                (strName_I != null) &&
                (strName_I != "")
                )
            {
                strUserMessage_IO = "Unit cannot be empty if the resource is physical unless it is not physical.";
                if (
                    //                                      //Resource is physical and unit different of empty.
                    (RestypResourceType.boolIsPhysical(resToEdit_I.restypBelongsTo.strClassification) && 
                        (strUnit_I != null) && (strUnit_I != "")) ||
                    //                                      //Resource is physical and unit empty.
                    (!RestypResourceType.boolIsPhysical(resToEdit_I.restypBelongsTo.strClassification) &&
                        ((strUnit_I == null) || (strUnit_I == "")))
                    )
                {
                    //                                      //Validate Unit not only have numbers.
                    bool boolIsParseableToInt = strUnit_I.IsParsableToInt();
                    bool boolIsParseableToNum = strUnit_I.IsParsableToNum();

                    strUserMessage_IO = "Unit of Measurement cannot start with a number.";
                    if (
                        !boolIsParseableToInt &&
                        !boolIsParseableToNum
                        )
                    {
                        if (
                            !ResResource.boolResourceSameNameAndTypeAndSize(resToEdit_I, null, strName_I,
                            arrattrjson5_I, context_M, ref strUserMessage_IO)
                            )
                        {
                            ResResource resDad = resToEdit_I.resinherited;

                            strUserMessage_IO = "Dad is no valid.";
                            if (
                                //                          //No Dad.
                                (resDad == null) ||
                                //                          //Dad is temaplte and from the same type.
                                ((resDad != null) && resDad.boolIsTemplate &&
                                (resDad.restypBelongsTo.intPk == resToEdit_I.restypBelongsTo.intPk))
                                )
                            {
                                boolValidData = true;
                            }
                        }
                    }
                }
            }
            return boolValidData;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subUpdateUnitInChildren(
            int intPkValue_I,
            String strUnit_I,
            bool? boolnIsDecimal_I,
            Odyssey2Context context_M
            )
        {
            List<ValentityValueEntityDB> darrvalentityChildren = context_M.Value.Where(val =>
                val.intnPkValueInherited == intPkValue_I).ToList();

            if (
                darrvalentityChildren.Count > 0
                )
            {
                foreach (ValentityValueEntityDB valentityChild in darrvalentityChildren)
                {
                    ValentityValueEntityDB valentityChildNew = context_M.Value.FirstOrDefault(val =>
                        val.intPk == valentityChild.intPk);

                    valentityChildNew.strValue = strUnit_I;
                    valentityChildNew.boolnIsDecimal = boolnIsDecimal_I;
                    context_M.Value.Update(valentityChildNew);

                    context_M.SaveChanges();

                    ResResource.subUpdateUnitInChildren(valentityChild.intPk, strUnit_I, boolnIsDecimal_I, context_M);
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subUpdateCostAndAvailabilityInChildren(
            //                                              //Update cost and availability in children.

            int intPkResource_I,
            ResResource res_I,
            //                                              //true - update cost.
            //                                              //false - update availability.
            //                                              //null - update both.
            bool? boolnUpdateCostAvailabilityOrBoth_I,
            Odyssey2Context context_M
            )
        {
            //                                              //Get the current cost before to update children.
            CostentityCostEntityDB costentityCurrentCost;
            res_I.subGetCurrentCost(context_M, out costentityCurrentCost);

            //                                              //Get the children.
            List<EleentityElementEntityDB> darreleentityChildren = context_M.Element.Where(eleentity =>
                (eleentity.intnPkElementInherited == intPkResource_I) &&
                (eleentity.strEndDate == null) &&
                (eleentity.strEndTime == null)).ToList();

            if (
                darreleentityChildren.Count > 0
                )
            {
                foreach (EleentityElementEntityDB eleentityChild in darreleentityChildren)
                {
                    if (
                        //                                  //Update cost.
                        (boolnUpdateCostAvailabilityOrBoth_I == null) ||
                        (boolnUpdateCostAvailabilityOrBoth_I == true)
                        )
                    {
                        //                                  //Get cost entity of the child.
                        List<CostentityCostEntityDB> darrcostentityChildrenCost = context_M.Cost.Where(
                        costentity => costentity.intPkResource == eleentityChild.intPk &&
                        costentity.boolnIsChangeable == false).ToList();

                        if (
                            darrcostentityChildrenCost.Count > 0
                            )
                        {
                            darrcostentityChildrenCost.Sort();
                            CostentityCostEntityDB costentityChildCurrentCost = darrcostentityChildrenCost.Last();

                            costentityChildCurrentCost.numnCost = costentityCurrentCost.numnCost;
                            costentityChildCurrentCost.numnQuantity = costentityCurrentCost.numnQuantity;
                            costentityChildCurrentCost.numnBlock = costentityCurrentCost.numnBlock;
                            costentityChildCurrentCost.numnMin = costentityCurrentCost.numnMin;
                            costentityChildCurrentCost.intPkAccount = costentityCurrentCost.intPkAccount;
                            costentityChildCurrentCost.numnHourlyRate = costentityCurrentCost.numnHourlyRate;
                        }
                    }

                    if (
                        //                                  //Update availability.
                        (boolnUpdateCostAvailabilityOrBoth_I == null) ||
                        (boolnUpdateCostAvailabilityOrBoth_I == false)
                        )
                    {
                        eleentityChild.boolnIsCalendar = res_I.boolnIsCalendar;
                        eleentityChild.boolnIsAvailable = res_I.boolnIsCalendar == false ? (bool?)true : null;
                    }

                    context_M.SaveChanges();

                    //                                      //Update in children of child.
                    ResResource resChild = ResResource.resFromDB(context_M, eleentityChild.intPk, 
                        eleentityChild.boolIsTemplate);
                    ResResource.subUpdateCostAndAvailabilityInChildren(eleentityChild.intPk, resChild, null, 
                        context_M);
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subCreateNewUnitHistory(
            //                                              //Method to create a new register into value table if the
            //                                              //      new unit and the unit value in db are different.

            //                                              //If new unit and the unit value in db are the same, just
            //                                              //      only update de booleans.

            EleentityElementEntityDB eleentityResource_I,
            ValentityValueEntityDB valentityDad_I,
            ValentityValueEntityDB valentity_I,
            String strUnit_I,
            bool? boolnIsDecimal_I,
            bool? boolnUnitChangeable_I,
            bool? boolnUnitInherited_I,
            Odyssey2Context context_M,
            out int intPkLastValue_O
            )
        {
            intPkLastValue_O = valentity_I.intPk;

            //                                              //Get all the units for the resource to update.
            List<ValentityValueEntityDB> darrvalentity = context_M.Value.Where(val =>
                val.intPkAttribute == valentity_I.intPkAttribute &&
                val.intPkElement == valentity_I.intPkElement).ToList();

            if (
                //                                           //There is two or more register.
                darrvalentity.Count() >= 1
                )
            {
                //                                          //Sort list.
                darrvalentity.Sort();
                //                                          //Get the last unit.
                ValentityValueEntityDB valentityLast = darrvalentity[darrvalentity.Count() - 1];
                intPkLastValue_O = valentityLast.intPk;
                String strLastValue = valentityLast.strValue;

                int? intnPkValueInherited = null;
                if (
                    boolnUnitInherited_I == true
                    )
                {
                    intnPkValueInherited = valentityDad_I.intPk;
                }

                List<WfentityWorkflowEntityDB> darrwfentity;
                if (
                    //                                      //The new unit and the unit in db are different.
                    !eleentityResource_I.boolIsTemplate &&
                    strLastValue != strUnit_I &&
                    ResResource.boolResourceIsInWorkflowsWithThisResourceAndJobsInProgressOrCompleted(context_M,
                        eleentityResource_I, out darrwfentity)
                    )
                {
                    //                                      //Create new register in value table.
                    ValentityValueEntityDB valentity = new ValentityValueEntityDB
                    {
                        strValue = strUnit_I,
                        boolnIsChangeable = boolnUnitChangeable_I,
                        strSetDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                        strSetTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                        intPkAttribute = valentity_I.intPkAttribute,
                        intPkElement = valentity_I.intPkElement,
                        intnPkValueInherited = intnPkValueInherited,
                        boolnIsDecimal = boolnIsDecimal_I
                    };
                    context_M.Value.Add(valentity);
                }
                else
                {
                    //                                      //The new unit and the unit in db are the same.
                    //                                      //Do not create his, only update de booleans.
                    ValentityValueEntityDB valentityToUpdate = context_M.Value.FirstOrDefault(val =>
                        val.intPk == valentityLast.intPk);
                    valentityToUpdate.strValue = strUnit_I;
                    valentityToUpdate.boolnIsChangeable = boolnUnitChangeable_I;
                    valentityToUpdate.intnPkValueInherited = intnPkValueInherited;
                    valentityToUpdate.boolnIsDecimal = boolnIsDecimal_I;
                    context_M.Update(valentityToUpdate);
                }
                context_M.SaveChanges();

                if (
                    //                                      //The new unit and the unit in db are different.
                    !eleentityResource_I.boolIsTemplate &&
                    strLastValue != strUnit_I
                    )
                {
                    //                                      //Method to deleted calculations associated with the
                    //                                      //      resource.
                    CalCalculation.subDeleteCalculationsWhenUnitChange(valentity_I.intPkElement, context_M);
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subAddCost(
            //                                              //Method that add or update the cost for a given resource.

            int intPkResource_I,
            double? numnQuantity_I,
            double? numnCost_I,
            double? numnMin_I,
            double? numnBlock_I,
            int? intnPkAccount_I,
            double? numnHourlyRate_I,
            bool boolIsDummyCost_I,
            PsPrintShop ps_I,
            bool? boolnArea_I,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Find resource.
            ResResource resResource = ResResource.resFromDB(context_M, intPkResource_I, false);
            //                                              //Find template.
            ResResource resTemplate = ResResource.resFromDB(context_M, intPkResource_I, true);
            //                                              //Make them one variable.
            ResResource resResOrTemp = resResource == null ? resTemplate : resResource;


            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Resurce not found.";
            if (
                //                                          //The resource exists.
                resResOrTemp != null
                )
            {
                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Resurce is not physical.";
                if (
                    //                                      //The resource is physical.
                    RestypResourceType.boolIsPhysical(resResOrTemp.restypBelongsTo.strClassification)
                    )
                {
                    intStatus_IO = 403;
                    strUserMessage_IO = "Cost, Quantity, Min to use, In blocks of and HourlyRate cannot be" +
                        " less than 0.";
                    strDevMessage_IO = "";
                    if (
                        //                                  //Add just cost.
                        ( (numnCost_I > 0) && (numnQuantity_I > 0) &&
                        (numnMin_I == null || (numnMin_I != null && numnMin_I >= 0)) &&
                        (numnBlock_I == null || (numnBlock_I != null && numnBlock_I >= 0)) &&
                        numnHourlyRate_I == null )
                        ||
                        //                                  //Add just an hourlyRate
                        ( (numnHourlyRate_I != null) && (numnHourlyRate_I > 0) &&
                        (resResOrTemp.restypBelongsTo.strCustomTypeId == ResResource.strDevice ||
                        resResOrTemp.restypBelongsTo.strCustomTypeId == ResResource.strTool ||
                        resResOrTemp.restypBelongsTo.strCustomTypeId == ResResource.strCustom) &&
                        (numnCost_I == null) && (numnQuantity_I == null) && (numnMin_I == null) && (numnBlock_I == null) )
                        ||
                        //                                  //Add both cost and hourlyRate.
                        ((numnCost_I > 0) && (numnQuantity_I > 0) &&
                        (numnMin_I == null || (numnMin_I != null && numnMin_I >= 0)) &&
                        (numnBlock_I == null || (numnBlock_I != null && numnBlock_I >= 0)) &&
                        (numnHourlyRate_I != null) && (numnHourlyRate_I > 0) &&
                        (resResOrTemp.restypBelongsTo.strCustomTypeId == ResResource.strDevice ||
                        resResOrTemp.restypBelongsTo.strCustomTypeId == ResResource.strTool ||
                        resResOrTemp.restypBelongsTo.strCustomTypeId == ResResource.strCustom) )
                        ||
                        boolIsDummyCost_I
                        )
                    {
                        //                                  //To easy code.
                        double? numnCostToAdd = null;
                        double? numnQuantityToAdd = null;
                        double? numnMinToAdd = null;
                        double? numnBlockToAdd = null;
                        /*CASE*/
                        if (
                            numnCost_I == 0
                            )
                        {
                            numnCostToAdd = 0;
                            intStatus_IO = 200;
                            strUserMessage_IO = "You set a cost to 0.";
                            strDevMessage_IO = "";
                        }
                        else if (
                           numnCost_I > 0
                           )
                        {
                            numnCostToAdd = (double)numnCost_I;
                            numnQuantityToAdd = (double)numnQuantity_I;
                            numnMinToAdd = (numnMin_I != null && numnMin_I < 0) ? 0 : numnMin_I;
                            numnBlockToAdd = (numnBlock_I != null && numnBlock_I < 0) ? 0 : numnBlock_I;

                            intStatus_IO = 200;
                            strUserMessage_IO = "";
                            strDevMessage_IO = "";

                            if (
                                numnBlockToAdd != null && numnBlockToAdd > 0
                                )
                            {
                                //                          //Calculate Min to use according ByBlock´s value.
                                numnMinToAdd = ResResource.numnGetMinToUse(numnMinToAdd, numnBlockToAdd);

                                intStatus_IO = 200;
                                strUserMessage_IO = "";
                                strDevMessage_IO = "numnMin was calculated using the value of numnBlock";
                            }
                        }
                        /*END-CASE*/

                        //                                  //Is account came null, get the generic account.
                        int intPkAccount = intnPkAccount_I != null ? (int)intnPkAccount_I :
                            ps_I.intGetExpensePkAccount(context_M);

                        intStatus_IO = 404;
                        strUserMessage_IO = "Something is wrong.";
                        strDevMessage_IO = "Account was not found or is not expense type.";
                        if (
                            AccAccounting.boolIsExpenseValid(intPkAccount, context_M)
                            )
                        {
                            //                              //Only if there is a Media resource a boolnArea can be add.

                            intStatus_IO = 405;
                            if (
                                ResResource.boolCostForThisResourceCanBeAdd(resResOrTemp, boolnArea_I,
                                    context_M, ref strUserMessage_IO, ref strDevMessage_IO)
                                )
                            {
                                //                              //Search if there is a cost already for that resource.
                                List<CostentityCostEntityDB> darrcostentity = context_M.Cost.Where(cost =>
                                    cost.intPkResource == intPkResource_I).ToList();

                                if (
                                    //                          //No cost found.
                                    darrcostentity.Count == 0
                                    )
                                {
                                    //                          //Create the cost entity and save it.
                                    CostentityCostEntityDB costentity = new CostentityCostEntityDB
                                    {
                                        intPkResource = intPkResource_I,
                                        numnQuantity = numnQuantityToAdd,
                                        numnCost = numnCostToAdd,
                                        numnMin = numnMinToAdd,
                                        numnBlock = numnBlockToAdd,
                                        strSetDate = Date.Now(ZonedTimeTools.timezone).ToText(),
                                        strSetTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                                        intnPkCostInherited = null,
                                        intPkAccount = intPkAccount,
                                        numnHourlyRate = numnHourlyRate_I,
                                        boolnArea = boolnArea_I
                                    };
                                    context_M.Cost.Add(costentity);
                                    context_M.SaveChanges();
                                }
                                else
                                {
                                    //                          //More than one cost found.
                                    //                          //Get the last cost of the found resource.
                                    CostentityCostEntityDB costentityCurrentCost =
                                        ResResource.costentityCurrentResourceCost(context_M, resResOrTemp);

                                    if (
                                        //                      //Verify if we need to create a new register
                                        ResResource.boolCostNeedsToBeCopy(context_M, costentityCurrentCost)
                                        )
                                    {
                                        //                      //Create the cost entity and save it.
                                        CostentityCostEntityDB costentity = new CostentityCostEntityDB
                                        {
                                            intPkResource = intPkResource_I,
                                            numnQuantity = numnQuantityToAdd,
                                            numnCost = numnCostToAdd,
                                            numnMin = numnMinToAdd,
                                            numnBlock = numnBlockToAdd,
                                            strSetDate = Date.Now(ZonedTimeTools.timezone).ToText(),
                                            strSetTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                                            intPkAccount = intPkAccount,
                                            numnHourlyRate = numnHourlyRate_I,
                                            boolnArea = boolnArea_I
                                        };
                                        context_M.Cost.Add(costentity);
                                        context_M.SaveChanges();
                                    }
                                    else
                                    {
                                        //                      //Find cost to update.
                                        CostentityCostEntityDB costentityToUpdate = context_M.Cost.FirstOrDefault(cost =>
                                            cost.intPk == costentityCurrentCost.intPk);

                                        //                      //Update register.
                                        costentityToUpdate.numnQuantity = numnQuantityToAdd;
                                        costentityToUpdate.numnCost = numnCostToAdd;
                                        costentityToUpdate.numnMin = numnMinToAdd;
                                        costentityToUpdate.numnBlock = numnBlockToAdd;
                                        costentityToUpdate.intPkAccount = intPkAccount;
                                        costentityToUpdate.numnHourlyRate = numnHourlyRate_I;
                                        costentityToUpdate.boolnArea = boolnArea_I;
                                        context_M.SaveChanges();
                                    }
                                }

                                //                              //This method will update hourlyRate too.
                                bool? boolnOnlyUpdateCost = true;
                                ResResource.subUpdateCostAndAvailabilityInChildren(intPkResource_I, resResOrTemp,
                                    boolnOnlyUpdateCost, context_M);

                                intStatus_IO = 200;
                                strUserMessage_IO = "";
                                strDevMessage_IO = "";
                            }                            
                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static bool boolCostForThisResourceCanBeAdd(
            //                                              //Verify if a cost for a resource can be added follow next
            //                                              //      rules.
            //                                              //1.-Media Resources.
            //                                              //Need boolnArea true or false (not null) and need to has
            //                                              //      dimensionUnit attribute.
            //                                              //2.-The rest of resource's type need to have
            //                                              //      boolnArea = null

            ResResource resResOrTemp_I,
            bool? boolnArea_I,
            Odyssey2Context context_M,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            bool boolCostForThisResourceCanBeAdd = false;

            /*CASE*/
            if (
                resResOrTemp_I.restypBelongsTo.strCustomTypeId == ResResource.strMedia
                )
            {
                if (
                    boolnArea_I == null || boolnArea_I == false
                    )
                {
                    boolCostForThisResourceCanBeAdd = true;
                }
                else
                {
                    String strMediaResDimUnit = ResResource.strGetMediaResourceDimensionUnit(resResOrTemp_I.intPk);

                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "To set a media resource as area the resource needs to have dimensionUnit" +
                        " attribute";
                    if (
                        (strMediaResDimUnit != null)
                        )
                    {
                        boolCostForThisResourceCanBeAdd = true;
                    }
                }
            }
            else
            {
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "This resource type can not be set with an area value.";
                boolCostForThisResourceCanBeAdd = boolnArea_I == null ? true : false;
            }
            /*END-CASE*/

            return boolCostForThisResourceCanBeAdd;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static double? numnGetMinToUse(
            //                                              //Give the value of Min To Use considering ByBlock's value.
            //                                              //double, new Min to use.

            double? numnMinToAdd_I,
            double? numnBlockToAdd_I
            )
        {
            //                                              //To easy code.
            double? numnMinToAdd = (numnMinToAdd_I == null) ? numnBlockToAdd_I : numnMinToAdd_I;

            double? numnNewMinToAdd;
            int intI = 1;
            do
            {
                numnNewMinToAdd = (numnBlockToAdd_I * intI);

                intI = intI + 1;
            }
            /*REPEAT-UNTIL*/
            while (!(
                numnNewMinToAdd >= numnMinToAdd
                ));

            return numnNewMinToAdd;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static CostentityCostEntityDB costentityCurrentResourceCost(
            Odyssey2Context context_I,
            //                                              //Can not be null.
            ResResource resResOrTemp_I
            )
        {
            //                                              //Initialize the costentity variable.
            CostentityCostEntityDB costentity = new CostentityCostEntityDB();

            //                                              //Get all the costs for the resource.
            List<CostentityCostEntityDB> darrcostentityCosts = context_I.Cost.Where(cost =>
                    cost.intPkResource == resResOrTemp_I.intPk).ToList();

            if (
                darrcostentityCosts.Count() > 0
                )
            {
                //                                          //Sort list of costs.
                darrcostentityCosts.Sort();
                //                                          //Last resource's cost.
                costentity = darrcostentityCosts.Last();
            }

            return costentity;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static bool boolCostNeedsToBeCopy(
            Odyssey2Context context_I,
            CostentityCostEntityDB costentity_I
            )
        {
            bool boolCostNeedsToBeCopied = false;

            //                                              //Create cost ztime object.
            ZonedTime ztimeCostDate = ZonedTimeTools.NewZonedTime(costentity_I.strSetDate.ParseToDate(),
                    costentity_I.strSetTime.ParseToTime());

            //                                              //Find Job in progress or completed using io job table.
            IQueryable<IojentityInputsAndOutputsForAJobEntityDB> setiojentity =
                context_I.InputsAndOutputsForAJob.Where(ioj => ioj.intPkResource == costentity_I.intPkResource);
            List<IojentityInputsAndOutputsForAJobEntityDB> darriojentity = setiojentity.ToList();

            int intI = 0;
            /*WHILE-DO*/
            while (
                 intI < darriojentity.Count &&
                !boolCostNeedsToBeCopied
                )
            {
                //                                          //Use JobId from io job table.
                JobentityJobEntityDB jobentity = context_I.Job.FirstOrDefault(job =>
                    job.intJobID == darriojentity[intI].intJobId);
                if (
                    //                                      //workflow´s job is in progress or done.
                    jobentity != null
                    )
                {
                    //                                      //Get jobs date.
                    ZonedTime ztimeJobDate = ZonedTimeTools.NewZonedTime(jobentity.strStartDate.ParseToDate(),
                            jobentity.strStartTime.ParseToTime());
                    //                                      //Create ztime now.
                    ZonedTime ztimeDateNow = ZonedTimeTools.NewZonedTime(Date.Now(ZonedTimeTools.timezone),
                            Time.Now(ZonedTimeTools.timezone));
                    if (
                        //                                  //Cost fix with job's date.
                        ztimeJobDate >= ztimeCostDate &&
                        ztimeJobDate < ztimeDateNow
                        )
                    {
                        boolCostNeedsToBeCopied = true;
                    }
                }
                intI = intI + 1;
            }

            List<WfentityWorkflowEntityDB> darrwfentity = new List<WfentityWorkflowEntityDB>();
            if (
                boolCostNeedsToBeCopied == false
                )
            {
                //                                          //Find PkWorkflow using io table.
                IQueryable<IoentityInputsAndOutputsEntityDB> setioentity = context_I.InputsAndOutputs.Where(io =>
                    io.intnPkResource == costentity_I.intPkResource);
                List<IoentityInputsAndOutputsEntityDB> darrioentity = setioentity.ToList();
                foreach (IoentityInputsAndOutputsEntityDB io in darrioentity)
                {
                    //                                      //Find io´s workflows.
                    WfentityWorkflowEntityDB wfentity = context_I.Workflow.FirstOrDefault(wf =>
                        wf.intPk == io.intPkWorkflow && wf.boolDeleted == false);

                    if (
                        wfentity != null
                        )
                    {
                        darrwfentity.Add(wfentity);
                    }
                }
            }

            int intJ = 0;
            /*WHILE-DO*/
            while (
                 intJ < darrwfentity.Count &&
                !boolCostNeedsToBeCopied
                )
            {
                //                                          //Find workflow´s jobs.
                List<JobentityJobEntityDB> darrjobentity = context_I.Job.Where(job =>
                    job.intPkWorkflow == darrwfentity[intJ].intPk).ToList();

                foreach (JobentityJobEntityDB jobentity in darrjobentity)
                {
                    if (
                        //                                  //workflow´s job is in progress or done.
                        jobentity != null
                        )
                    {
                        //                                  //Get jobs date.
                        ZonedTime ztimeJobDate = ZonedTimeTools.NewZonedTime(jobentity.strStartDate.ParseToDate(),
                                jobentity.strStartTime.ParseToTime());
                        //                                  //Create ztime now.
                        ZonedTime ztimeDateNow = ZonedTimeTools.NewZonedTime(Date.Now(ZonedTimeTools.timezone),
                                Time.Now(ZonedTimeTools.timezone));

                        if (
                            //                              //Cost fix with job's date.
                            ztimeJobDate >= ztimeCostDate &&
                            ztimeJobDate < ztimeDateNow
                            )
                        {
                            boolCostNeedsToBeCopied = true;
                        }
                    }
                }
                intJ = intJ + 1;
            }
            return boolCostNeedsToBeCopied;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subAvailability(
            //                                              //Change resource´s availability or Type of availability.

            String strPrintshopId_I,
            int intContactId_I,
            int intPkResource_I,
            bool boolIsCalendar_I,
            bool? boolnIsAvailable_I,
            IHubContext<ConnectionHub> iHubContext_I,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Get the resource.
            EleentityElementEntityDB eleentity = context_M.Element.FirstOrDefault(ele => ele.intPk == intPkResource_I);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "No resource found.";
            if (
                //                                          //Verify if the resource exists.
                eleentity != null
                )
            {
                //                                          //Get the ElementType.
                EtentityElementTypeEntityDB etentity = context_M.ElementType.FirstOrDefault(et =>
                    et.intPk == eleentity.intPkElementType);

                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Is not a Resource.";
                if (
                    //                                      //Verify if is a resource.
                    etentity != null &&
                    etentity.strResOrPro == EtElementTypeAbstract.strResource
                    )
                {
                    //                                      //Update availability and type of availability.
                    if (
                        //                                  //Is calendar.
                        boolIsCalendar_I
                        )
                    {
                        intStatus_IO = 403;
                        strUserMessage_IO = "Something is wrong.";
                        strDevMessage_IO = "Resource's type not allow for this availability.";
                        if (
                            RestypResourceType.boolIsDeviceToolOrCustom(
                                (RestypResourceType)EtElementTypeAbstract.etFromDB(context_M, 
                                eleentity.intPkElementType))
                            )
                        {
                            eleentity.boolnIsCalendar = boolIsCalendar_I;
                            eleentity.boolnIsAvailable = null;
                            context_M.Element.Update(eleentity);

                            intStatus_IO = 200;
                            strUserMessage_IO = "Success.";
                            strDevMessage_IO = "Success.";
                        }
                    }
                    else
                    {
                        intStatus_IO = 404;
                        strUserMessage_IO = "Something is wrong.";
                        strDevMessage_IO = "Availability must be specified.";
                        if (
                            //                              //Verify if the availability is specified.
                            boolnIsAvailable_I != null
                            )
                        {
                            eleentity.boolnIsCalendar = boolIsCalendar_I;
                            eleentity.boolnIsAvailable = boolnIsAvailable_I;
                            context_M.Element.Update(eleentity);

                            intStatus_IO = 200;
                            strUserMessage_IO = "Success.";
                            strDevMessage_IO = "Success.";
                        }

                        //                                  //Delete estimation data and estimates.
                        ResResource.subDeleteEstimationDataEntries(context_M, eleentity, intPkResource_I);
                    }
                    context_M.SaveChanges();

                    if (
                        //                                  //It is for availability type.
                        !boolIsCalendar_I && boolnIsAvailable_I != null
                        )
                    {
                        //                                  //Get the printshop.
                        PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);

                        //                                  //Find alert type related to resources.
                        AlerttypeentityAlertTypeEntityDB alerttypeentity = context_M.AlertType.FirstOrDefault(
                            alerttype => alerttype.strType == AlerttypeentityAlertTypeEntityDB.strAvailability);

                        //                                  //Get the alerts.
                        List<AlertentityAlertEntityDB> darralertentity = context_M.Alert.Where(alert =>
                            alert.intnPkResource == intPkResource_I && 
                            alert.intPkAlertType == alerttypeentity.intPk).ToList();

                        /*CASE*/
                        if (
                            //                              //It is available and the alert exists.
                            (bool)boolnIsAvailable_I && darralertentity.Count > 0
                            )
                        {
                            //                              //Get all contactIds from printshop.
                            int[] arrintContactIdsFromPrintshop = PsPrintShop.arrintGetAllContactIdsFromPrintshop(
                                strPrintshopId_I, ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);

                            foreach (AlertentityAlertEntityDB alertentity in darralertentity)
                            {
                                foreach (int intContactId in arrintContactIdsFromPrintshop)
                                {
                                    if (
                                        //                  //Notification not read by user.
                                        !PsPrintShop.boolNotificationReadByUser(alertentity, intContactId)
                                        )
                                    {
                                        //                  //Reduce the notification to specific contacts.
                                        AlnotAlertNotification.subReduceToOne(intContactId, iHubContext_I);
                                    }
                                }
                                
                                context_M.Alert.Remove(alertentity);
                                context_M.SaveChanges();
                            }
                        }
                        else if (
                            //                              //It is unavailable and the alert does not exist.
                            !(bool)boolnIsAvailable_I && darralertentity.Count == 0
                            )
                        {
                            AlertentityAlertEntityDB alertentity = new AlertentityAlertEntityDB
                            {
                                intPkPrintshop = ps.intPk,
                                intPkAlertType = alerttypeentity.intPk,
                                intnPkResource = eleentity.intPk
                            };
                            context_M.Alert.Add(alertentity);
                            context_M.SaveChanges();

                            //                              //Send the notification to all contacts from Printshop.
                            AlnotAlertNotification.subSendToAll(intContactId_I + "", strPrintshopId_I.ParseToInt(),
                                eleentity.strElementName + alerttypeentity.strDescription, iHubContext_I);                         
                        }
                        /*END-CASE*/
                    }

                    //                                      //Update availability in children.
                    ResResource resResOrTemp = ResResource.resFromDB(context_M, intPkResource_I,
                        eleentity.boolIsTemplate);

                    //                                      //false means update only availability, not cost.
                    bool? boolnOnlyUpdateCost = false;
                    ResResource.subUpdateCostAndAvailabilityInChildren(intPkResource_I, resResOrTemp,
                        boolnOnlyUpdateCost, context_M);
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subAddTime(
            //                                              //Method that add or update the time for a given resource.
            int intPkResource_I,
            double numQuantity_I,
            int intHours_I,
            int intMinutes_I,
            int intSeconds_I,
            double? numnMinThickness_I,
            double? numnMaxThickness_I,
            String strThicknessUnit_I,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            ResResource res = ResResource.resFromDB(intPkResource_I, false);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Resurce not found.";
            if (
                //                                          //The resource exists.
                res != null
                )
            {
                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Resurce is not Device, tool or custom.";
                if (
                    //                                      //Verify if is a Device, Tool or custom resource.
                    res.restypBelongsTo.strXJDFTypeId == EtElementTypeAbstract.strResourceTypeDevice ||
                    res.restypBelongsTo.strXJDFTypeId == EtElementTypeAbstract.strResourceTypeTool ||
                    res.restypBelongsTo.strCustomTypeId == EtElementTypeAbstract.strResCustomType
                    )
                {
                    intStatus_IO = 403;
                    strUserMessage_IO = "Invalid thickness values.";
                    strDevMessage_IO = "";
                    if (
                        //                                  //Thickness was not specified.
                        (numnMinThickness_I == null && numnMaxThickness_I == null) ||
                        //                                  //Only MinThickness was specified.
                        (numnMinThickness_I != null && numnMaxThickness_I == null) && numnMinThickness_I > 0 ||
                        //                                  //Only MaxThickness was specified.
                        (numnMaxThickness_I != null && numnMinThickness_I == null) && numnMaxThickness_I > 0 ||
                        
                        //                                  //Both min and max thickness were specified.
                        ((numnMinThickness_I != null && numnMaxThickness_I != null) &&
                        (numnMinThickness_I > 0 && numnMaxThickness_I > 0) &&
                        (numnMinThickness_I <= numnMaxThickness_I))
                        )
                    {
                        bool boolIsValidUnit = false;
                        if (
                            numnMinThickness_I == null && numnMaxThickness_I == null
                            )
                        {
                            boolIsValidUnit = true;
                            strThicknessUnit_I = "";
                        }
                        else
                        {
                            EnumentityEnumerationEntityDB enumentity = context_M.Enumeration.FirstOrDefault(enu =>
                            enu.strEnumName == "ThicknessUnit" && enu.strEnumValue == strThicknessUnit_I);
                            boolIsValidUnit = enumentity != null ? true : false;
                        }                        

                        intStatus_IO = 404;
                        strUserMessage_IO = "Invalid thickness unit.";
                        strDevMessage_IO = "";
                        if (
                            boolIsValidUnit
                            )
                        {
                            //                              //To easy code.
                            double? numnMinThicknessToPass;
                            double? numnMaxThicknessToPass;
                            ResResource.subConvertThicknessToUM(strThicknessUnit_I, numnMinThickness_I,
                                numnMaxThickness_I,  out numnMinThicknessToPass, out numnMaxThicknessToPass);                            

                            bool boolTimeCanBeAdded = ResResource.boolTimeCanBeAdded(intPkResource_I,
                                numnMinThicknessToPass, numnMaxThicknessToPass, context_M, ref strUserMessage_IO);

                            intStatus_IO = 405;
                            strDevMessage_IO = "";
                            if (
                                boolTimeCanBeAdded
                                )
                            {
                                //                          //Create the time entity and save it.
                                TimeentityTimeEntityDB timeentity = new TimeentityTimeEntityDB
                                {
                                    intPkResource = intPkResource_I,
                                    numQuantity = numQuantity_I,
                                    intHours = intHours_I,
                                    intMinutes = intMinutes_I,
                                    intSeconds = intSeconds_I,
                                    strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                                    strStartTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                                    numnMinThickness = numnMinThickness_I,
                                    numnMaxThickness = numnMaxThickness_I,
                                    strThicknessUnit = strThicknessUnit_I
                                };
                                context_M.Time.Add(timeentity);
                                context_M.SaveChanges();

                                intStatus_IO = 200;
                                strUserMessage_IO = "Success.";
                                strDevMessage_IO = "";
                            }

                        }                        
                    }                    
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static void subConvertThicknessToUM(
            //                                              //Method to verify if a time for a resource can be add
            //                                              //      verifying if exists a time for a resource with a
            //                                              //      given thickness.

            String strThicknessUnit_I,
            double? numnMinThickness_I,
            double? numnMaxThickness_I,
            out double? numnMinThicknessCoverted_O,
            out double? numnMaxThicknessConverted_O
            )
        {
            numnMinThicknessCoverted_O = null;
            numnMaxThicknessConverted_O = null;
            if (
                strThicknessUnit_I == "mm"
                )
            {
                //                          //Thickness in milimeters into micrometers.
                numnMinThicknessCoverted_O = numnMinThickness_I == null ? null : numnMinThickness_I * 1000;
                numnMaxThicknessConverted_O = numnMaxThickness_I == null ? null : numnMaxThickness_I * 1000;
            }
            else if (
                strThicknessUnit_I == "point"
                )
            {
                //                          //Thicness in points(1/1000)in into micrometers.
                //                          //1000 points = 1 in
                //                          //1 point = (1/1000) in
                //                          //1 in = 25400 um
                numnMinThicknessCoverted_O = numnMinThickness_I == null ? null : numnMinThickness_I * 25.4;
                numnMaxThicknessConverted_O = numnMaxThickness_I == null ? null : numnMaxThickness_I * 25.4;
            }
            else
            {
                //                          //Thickness in micrometros.
                numnMinThicknessCoverted_O = numnMinThickness_I == null ? null : numnMinThickness_I;
                numnMaxThicknessConverted_O = numnMaxThickness_I == null ? null : numnMaxThickness_I;
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static bool boolTimeCanBeAdded(
            //                                              //Method to verify if a time for a resource can be add
            //                                              //      verifying if exists a time for a resource with a
            //                                              //      given thickness.

            int intPkResource_I,
            double? numnMinThickness_I,
            double? numnMaxThickness_I,
            Odyssey2Context context_M,
            ref String strUserMessage_IO
            )
        {
            strUserMessage_IO = "";
            bool boolTimeCanBeAdded = true;
            if (
                numnMinThickness_I == null && numnMaxThickness_I == null
                )
            {
                //                                          //Verify if there is already a default time.
                TimeentityTimeEntityDB timeentity = context_M.Time.FirstOrDefault(time =>
                time.intPkResource == intPkResource_I && time.numnMinThickness == numnMinThickness_I &&
                time.numnMaxThickness == numnMaxThickness_I &&
                time.strEndDate == null);

                boolTimeCanBeAdded = timeentity != null ? false : true;
                strUserMessage_IO = timeentity != null ? "A default time already exists." : "";
            }
            else
            {
                List<TimeentityTimeEntityDB> darrtimeentity = context_M.Time.Where(time =>
                    time.intPkResource == intPkResource_I && time.strEndDate == null &&
                    (time.numnMinThickness != null || time.numnMaxThickness != null)).ToList();

                /*CASE*/
                if (
                    numnMinThickness_I != null && numnMaxThickness_I == null
                    )
                {
                    int intI = 0;
                    /*WHILE*/
                    while (
                        boolTimeCanBeAdded &&
                        intI < darrtimeentity.Count
                        )
                    {
                        TimeentityTimeEntityDB timeentity = darrtimeentity[intI];

                        //                                  //To easy code.
                        double? numnMinThickness;
                        double? numnMaxThickness;
                        ResResource.subConvertThicknessToUM(timeentity.strThicknessUnit, timeentity.numnMinThickness,
                            timeentity.numnMaxThickness, out numnMinThickness, out numnMaxThickness);

                        if (
                            (numnMinThickness != null && numnMaxThickness == null) 
                            ||
                            (numnMinThickness != null && numnMaxThickness != null &&
                            numnMinThickness_I <= numnMaxThickness) 
                            ||
                            (numnMinThickness == null && numnMaxThickness != null &&
                            numnMinThickness_I <= numnMaxThickness)
                            )
                        {
                            boolTimeCanBeAdded = false;
                            strUserMessage_IO = "There is already a time with this thickness range.";
                        }

                        intI++;
                    }
                }
                else if (
                   numnMinThickness_I == null && numnMaxThickness_I != null
                   )
                {
                    int intI = 0;
                    /*WHILE*/
                    while (
                        boolTimeCanBeAdded &&
                        intI < darrtimeentity.Count
                        )
                    {
                        TimeentityTimeEntityDB timeentity = darrtimeentity[intI];

                        //                                  //To easy code.
                        double? numnMinThickness;
                        double? numnMaxThickness;
                        ResResource.subConvertThicknessToUM(timeentity.strThicknessUnit, timeentity.numnMinThickness,
                            timeentity.numnMaxThickness, out numnMinThickness, out numnMaxThickness);

                        if (
                            (numnMinThickness != null && numnMaxThickness == null &&
                            numnMaxThickness_I >= numnMinThickness) 
                            ||
                            (numnMinThickness != null && numnMaxThickness != null &&
                            numnMaxThickness_I >= numnMinThickness) 
                            ||
                            (numnMinThickness == null && numnMaxThickness != null)
                            )
                        {
                            boolTimeCanBeAdded = false;
                            strUserMessage_IO = "There is already a time with this thickness range.";
                        }

                        intI++;
                    }
                }
                else if (
                    numnMinThickness_I != null && numnMaxThickness_I != null
                    )
                {
                    int intI = 0;
                    /*WHILE*/
                    while (
                        boolTimeCanBeAdded &&
                        intI < darrtimeentity.Count
                        )
                    {
                        TimeentityTimeEntityDB timeentity = darrtimeentity[intI];

                        //                                  //To easy code.
                        double? numnMinThickness;
                        double? numnMaxThickness;
                        ResResource.subConvertThicknessToUM(timeentity.strThicknessUnit, timeentity.numnMinThickness,
                            timeentity.numnMaxThickness, out numnMinThickness, out numnMaxThickness);

                        if (
                            (numnMinThickness != null && numnMaxThickness == null &&
                            numnMaxThickness_I >= numnMinThickness)
                            ||
                            (
                            numnMinThickness != null && numnMaxThickness != null &&
                            ((numnMinThickness_I >= numnMinThickness &&
                            numnMinThickness_I <= numnMaxThickness) ||
                            (numnMaxThickness_I >= numnMinThickness &&
                            numnMaxThickness_I <= numnMaxThickness)))
                            ||
                            (numnMinThickness == null && numnMaxThickness != null &&
                            numnMinThickness_I <= numnMaxThickness)
                            )
                        {
                            boolTimeCanBeAdded = false;
                            strUserMessage_IO = "There is already a time with this thickness range.";
                        }

                        intI++;
                    }
                }
                /*END-CASE*/
            }
            return boolTimeCanBeAdded;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subUpdateTime(
            //                                              //Method that add or update the time for a given resource.
            int intPkResource_I,
            double numQuantity_I,
            int intHours_I,
            int intMinutes_I,
            int intSeconds_I,
            double? numnMinThickness_I,
            double? numnMaxThickness_I,
            String strThicknessUnit_I,
            int intPkTime_I,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            ResResource res = ResResource.resFromDB(intPkResource_I, false);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Resurce not found.";
            if (
                //                                          //The resource exists.
                res != null
                )
            {
                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Resurce is not Device, tool or custom.";
                if (
                    //                                      //Verify if is a Device, Tool or custom resource.
                    res.restypBelongsTo.strXJDFTypeId == EtElementTypeAbstract.strResourceTypeDevice ||
                    res.restypBelongsTo.strXJDFTypeId == EtElementTypeAbstract.strResourceTypeTool ||
                    res.restypBelongsTo.strCustomTypeId == EtElementTypeAbstract.strResCustomType
                    )
                {
                    intStatus_IO = 403;
                    strUserMessage_IO = "Thickness should be greather than 0.";
                    strDevMessage_IO = "Thickness should be greather than 0.";
                    if (
                        //                                  //Thickness was not specified.
                        (numnMinThickness_I == null && numnMaxThickness_I == null) ||
                        //                                  //Only MinThickness was specified.
                        (numnMinThickness_I != null && numnMaxThickness_I == null) && numnMinThickness_I > 0 ||
                        //                                  //Only MaxThickness was specified.
                        (numnMaxThickness_I != null && numnMinThickness_I == null) && numnMaxThickness_I > 0 ||
                        //                                  //Both min and max thickness were specified.
                        ((numnMinThickness_I != null && numnMaxThickness_I != null) &&
                        (numnMinThickness_I > 0 && numnMaxThickness_I > 0) &&
                        (numnMinThickness_I <= numnMaxThickness_I))
                        )
                    {
                        //                                  //Validate time to edit exists.
                        TimeentityTimeEntityDB timeentity = context_M.Time.FirstOrDefault(time =>
                        time.intPk == intPkTime_I && time.intPkResource == intPkResource_I &&
                        time.strEndDate == null);

                        intStatus_IO = 402;
                        strUserMessage_IO = "Something wrong";
                        strDevMessage_IO = "Invalid pkTime.";
                        if (
                            timeentity != null
                            )
                        {
                            EnumentityEnumerationEntityDB enumentity = context_M.Enumeration.FirstOrDefault(enu =>
                            enu.strEnumName == "ThicknessUnit" && enu.strEnumValue == strThicknessUnit_I);

                            intStatus_IO = 404;
                            strUserMessage_IO = "Invalid thickness unit.";
                            strDevMessage_IO = "Invalid thickness unit.";
                            if (
                                ((numnMinThickness_I == null) && (numnMaxThickness_I == null)) ||
                                (enumentity != null)
                                )
                            {
                                //                          //To easy code.
                                double? numnMinThicknessToPass = null;
                                double? numnMaxThicknessToPass = null;
                                if (
                                    strThicknessUnit_I == "mm"
                                    )
                                {
                                    //                      //Thickness in milimeters into micrometers.
                                    numnMinThicknessToPass = numnMinThickness_I == null ? null : 
                                        numnMinThickness_I * 1000;
                                    numnMaxThicknessToPass = numnMaxThickness_I == null ? null : 
                                        numnMaxThickness_I * 1000;
                                }
                                else if (
                                    strThicknessUnit_I == "point"
                                    )
                                {
                                    //                      //Thicness in points(1/1000)in into micrometers.
                                    //                      //1000 points = 1 in
                                    //                      //1 point = (1/1000) in
                                    //                      //1 in = 25400 um
                                    numnMinThicknessToPass = numnMinThickness_I == null ? null : 
                                        numnMinThickness_I * 25.4;
                                    numnMaxThicknessToPass = numnMaxThickness_I == null ? null : 
                                        numnMaxThickness_I * 25.4;
                                }
                                else
                                {
                                    //                          //Thickness in micrometros.
                                    numnMinThicknessToPass = numnMinThickness_I == null ? null : numnMinThickness_I;
                                    numnMaxThicknessToPass = numnMaxThickness_I == null ? null : numnMaxThickness_I;
                                }

                                bool boolTimeCanBeAdded = ResResource.boolTimeCanBeAddedEdit(intPkResource_I,
                                numnMinThicknessToPass, numnMaxThicknessToPass, intPkTime_I, context_M);

                                intStatus_IO = 403;
                                strUserMessage_IO = "There is already a default time or there is already a time for" +
                                        " the resource with this thickness.";
                                strDevMessage_IO = "There is already a default time or there is already a time for" +
                                        " the resource with this thickness.";
                                if (
                                    boolTimeCanBeAdded
                                    )
                                {
                                    timeentity.numQuantity = numQuantity_I;
                                    timeentity.intHours = intHours_I;
                                    timeentity.intMinutes = intMinutes_I;
                                    timeentity.intSeconds = intSeconds_I;
                                    timeentity.numnMinThickness = numnMinThickness_I;
                                    timeentity.numnMaxThickness = numnMaxThickness_I;
                                    timeentity.strThicknessUnit = strThicknessUnit_I;
                                    context_M.Time.Update(timeentity);
                                    context_M.SaveChanges();

                                    intStatus_IO = 200;
                                    strUserMessage_IO = "Success.";
                                    strDevMessage_IO = "";
                                }
                            }       
                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static bool boolTimeCanBeAddedEdit(
            //                                              //Method to verify if a time for a resource can be add
            //                                              //      verifying if exists a time for a resource with a
            //                                              //      given thickness.

            int intPkResource_I,
            double? numnMinThickness_I,
            double? numnMaxThickness_I,
            int intPkTime_I,
            Odyssey2Context context_M
            )
        {
            bool boolTimeCanBeAdded = true;
            if (
                numnMinThickness_I == null && numnMaxThickness_I == null
                )
            {
                //                                          //Verify if there is already a default time.
                TimeentityTimeEntityDB timeentity = context_M.Time.FirstOrDefault(time =>
                time.intPkResource == intPkResource_I && time.numnMinThickness == numnMinThickness_I &&
                time.numnMaxThickness == numnMaxThickness_I &&
                time.strEndDate == null && time.intPk != intPkTime_I);

                boolTimeCanBeAdded = timeentity != null ? false : true;
            }
            else
            {
                List<TimeentityTimeEntityDB> darrtimeentity = context_M.Time.Where(time =>
                    time.intPkResource == intPkResource_I && time.strEndDate == null &&
                    time.intPk != intPkTime_I &&
                    (time.numnMinThickness != null || time.numnMaxThickness != null)).ToList();

                /*CASE*/
                if (
                   numnMinThickness_I != null && numnMaxThickness_I == null
                   )
                {
                    int intI = 0;
                    /*WHILE*/
                    while (
                        boolTimeCanBeAdded &&
                        intI < darrtimeentity.Count
                        )
                    {
                        TimeentityTimeEntityDB timeentity = darrtimeentity[intI];

                        if (
                            (timeentity.numnMinThickness != null && timeentity.numnMaxThickness == null) ||
                            (timeentity.numnMinThickness != null && timeentity.numnMaxThickness != null &&
                            numnMinThickness_I <= timeentity.numnMaxThickness) ||
                            (timeentity.numnMinThickness == null && timeentity.numnMaxThickness != null &&
                            numnMinThickness_I <= timeentity.numnMaxThickness)
                            )
                        {
                            boolTimeCanBeAdded = false;
                        }

                        intI++;
                    }
                }
                else if (
                   numnMinThickness_I == null && numnMaxThickness_I != null
                   )
                {
                    int intI = 0;
                    /*WHILE*/
                    while (
                        boolTimeCanBeAdded &&
                        intI < darrtimeentity.Count
                        )
                    {
                        TimeentityTimeEntityDB timeentity = darrtimeentity[intI];

                        if (
                            (timeentity.numnMinThickness != null && timeentity.numnMaxThickness == null &&
                            numnMaxThickness_I >= timeentity.numnMinThickness) ||
                            (timeentity.numnMinThickness != null && timeentity.numnMaxThickness != null &&
                            numnMaxThickness_I >= timeentity.numnMinThickness) ||
                            (timeentity.numnMinThickness == null && timeentity.numnMaxThickness != null)
                            )
                        {
                            boolTimeCanBeAdded = false;
                        }

                        intI++;
                    }
                }
                else if (
                    numnMinThickness_I != null && numnMaxThickness_I != null
                    )
                {
                    int intI = 0;
                    /*WHILE*/
                    while (
                        boolTimeCanBeAdded &&
                        intI < darrtimeentity.Count
                        )
                    {
                        TimeentityTimeEntityDB timeentity = darrtimeentity[intI];

                        if (
                            (timeentity.numnMinThickness != null && timeentity.numnMaxThickness == null &&
                            numnMaxThickness_I >= timeentity.numnMinThickness) ||
                            (timeentity.numnMinThickness != null && timeentity.numnMaxThickness != null &&
                            numnMinThickness_I <= timeentity.numnMaxThickness) ||
                            (timeentity.numnMinThickness == null && timeentity.numnMaxThickness != null &&
                            numnMinThickness_I <= timeentity.numnMaxThickness)
                            )
                        {
                            boolTimeCanBeAdded = false;
                        }

                        intI++;
                    }
                }
                /*END-CASE*/
            }
            return boolTimeCanBeAdded;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subDeleteTime(
            //                                              //Method to delete a register from time table.
            int intPkTime_I,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                      //Validate if the pkTime belongs to a register in DB.
            TimeentityTimeEntityDB timeentity = context_M.Time.FirstOrDefault(time => time.intPk == intPkTime_I);

            intStatus_IO = 401;
            strUserMessage_IO = "Something wrong";
            strDevMessage_IO = "PkTime is not valid.";
            if (
                timeentity != null &&
                timeentity.strEndDate == null
                )
            {
                //                                      //Set endDate and endTime
                timeentity.strEndDate = Date.Now(ZonedTimeTools.timezone).ToString();
                timeentity.strEndTime = Time.Now(ZonedTimeTools.timezone).ToString();
                context_M.Time.Update(timeentity);
                context_M.SaveChanges();

                intStatus_IO = 200;
                strUserMessage_IO = "Success.";
                strDevMessage_IO = "";
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subRuleIsAddable(
            bool boolIsEmployee_I,
            int? intnContactId_I,
            int? intnPkResource_I,
            String strPrintshopId_I,
            String strFrecuency_I,
            String strStartTime_I,
            String strEndTime_I,
            String strStartDate_I,
            String strEndDate_I,
            String strRangeStartDate_I,
            String strRangeStartTime_I,
            String strRangeEndDate_I,
            String strRangeEndTime_I,
            int[] arrintDays_I,
            String strDay_I,
            IConfiguration configuration_I,
            out bool boolRuleIsAddable_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            boolRuleIsAddable_O = false;

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Range data is invalid.";
            //                                              //Local variables for range dates and times.
            String strRangeStartDate = strRangeStartDate_I;
            String strRangeStartTime = strRangeStartTime_I;
            String strRangeEndDate = strRangeEndDate_I;
            String strRangeEndTime = strRangeEndTime_I;
            if (
                //                                          //Change null data to valid data.
                ResResource.boolCorrectRangeDates(ref strRangeStartDate, ref strRangeStartTime,
                    ref strRangeEndDate, ref strRangeEndTime)
                )
            {
                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "No res or no ps or not valid contact.";
                ResResource res;
                PsPrintShop ps;
                if (
                //                                          //Get res or ps and validate contat.
                ResResource.boolValidResPsorContact(intnPkResource_I, strPrintshopId_I, intnContactId_I,
                    boolIsEmployee_I, configuration_I, out res, out ps)
                )
                {
                    List<PerentityPeriodEntityDB> darrperentity = null;

                    intStatus_IO = 403;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "Invalid frecuency.";
                    /*CASE*/
                    if (
                        strFrecuency_I == ResResource.strOnce
                        )
                    {
                        ResResource.subGetForOnceRule(res, ps, boolIsEmployee_I, intnContactId_I, strStartTime_I,
                            strEndTime_I, strStartDate_I, strEndDate_I, out darrperentity, ref intStatus_IO,
                            ref strUserMessage_IO, ref strDevMessage_IO);
                    }
                    else if (
                        strFrecuency_I == ResResource.strDaily
                        )
                    {
                        ResResource.subGetForDailyRule(res, ps, boolIsEmployee_I, intnContactId_I, strStartTime_I,
                            strEndTime_I, strRangeStartDate, strRangeStartTime, strRangeEndDate, strRangeEndTime,
                            out darrperentity, ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);
                    }
                    else if (
                        strFrecuency_I == ResResource.strWeekly
                        )
                    {
                        ResResource.subGetForWeeklyRule(res, ps, boolIsEmployee_I, intnContactId_I, strStartTime_I,
                            strEndTime_I, strRangeStartDate, strRangeStartTime, strRangeEndDate, strRangeEndTime,
                            arrintDays_I, out darrperentity, ref intStatus_IO, ref strUserMessage_IO, 
                            ref strDevMessage_IO);
                    }
                    else if (
                        strFrecuency_I == ResResource.strMonthly
                        )
                    {
                        ResResource.subGetForMonthlyRule(res, ps, boolIsEmployee_I, intnContactId_I,
                            strStartTime_I, strEndTime_I, strRangeStartDate, strRangeStartTime, strRangeEndDate,
                            strRangeEndTime, arrintDays_I, out darrperentity, ref intStatus_IO, ref strUserMessage_IO,
                            ref strDevMessage_IO);
                    }
                    else if (
                        strFrecuency_I == ResResource.strAnnually
                        )
                    {
                        ResResource.subGetForAnnuallyRule(res, ps, boolIsEmployee_I, intnContactId_I, strStartTime_I,
                            strEndTime_I, strDay_I, strRangeStartDate, strRangeStartTime, strRangeEndDate, 
                            strRangeEndTime, out darrperentity, ref intStatus_IO, ref strUserMessage_IO, 
                            ref strDevMessage_IO);
                    }
                    /*END-CASE*/

                    if (
                        darrperentity != null
                        )
                    {
                        List<PerentityPeriodEntityDB> darrperentityFinal = new List<PerentityPeriodEntityDB>();
                        foreach (PerentityPeriodEntityDB perentity in darrperentity)
                        {
                            if (
                                !darrperentityFinal.Exists(per => per.intJobId == perentity.intJobId)
                                )
                            {
                                darrperentityFinal.Add(perentity);
                            }
                        }

                        if (
                            //                              //There are "NotTemporaryPeriods".
                            darrperentityFinal.Count > 0
                            )
                        {
                            Odyssey2Context context = new Odyssey2Context();
                            boolRuleIsAddable_O = false;

                            strUserMessage_IO = "Adding this rule will imply deleting scheduled periods and estimates" +
                                " for the following jobs: ";
                            for (int intI = 0; intI < darrperentityFinal.Count; intI = intI + 1)
                            {
                                String strJobNumber = JobJob.strGetJobNumber(null, darrperentityFinal[intI].intJobId, 
                                    ps.strPrintshopId, context);
                                /*CASE*/
                                if (
                                    (intI == 0) &&
                                    (darrperentityFinal.Count == 1)
                                    )
                                {                               
                                    strUserMessage_IO = strUserMessage_IO +
                                        strJobNumber + ".";
                                }
                                else if (
                                    intI == 0
                                    )
                                {
                                    strUserMessage_IO = strUserMessage_IO +
                                        strJobNumber;
                                }
                                else if (
                                    intI == (darrperentityFinal.Count - 1)
                                    )
                                {
                                    strUserMessage_IO = strUserMessage_IO + " and " +
                                        strJobNumber + ".";
                                }
                                else
                                {                                    
                                    strUserMessage_IO = strUserMessage_IO + ", " +
                                        strJobNumber;
                                }
                                /*END-CASE*/
                            }
                        }
                        else if (
                            intStatus_IO == 200
                            )
                        {
                            intStatus_IO = 200;
                            strUserMessage_IO = "The rule is addable.";
                            strDevMessage_IO = " ";
                            boolRuleIsAddable_O = true;
                        }
                        /*END-CASE*/
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subGetForOnceRule(
            ResResource res_I,
            PsPrintShop ps_I,
            bool boolIsEmployee_I,
            int? intnContactId_I,
            String strStartTime_I,
            String strEndTime_I,
            String strStartDate_I,
            String strEndDate_I,
            out List<PerentityPeriodEntityDB> darrperentity_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            darrperentity_O = new List<PerentityPeriodEntityDB>();

            intStatus_IO = 404;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "The dates have an invalid format.";
            if (
                !String.IsNullOrEmpty(strStartDate_I) &&
                !String.IsNullOrEmpty(strEndDate_I)
                )
            {
                if (
                    strStartDate_I.IsParsableToDate() &&
                    strEndDate_I.IsParsableToDate()
                    )
                {
                    //                                      //To easy code.
                    Date dateStart = strStartDate_I.ParseToDate();
                    Date dateEnd = strEndDate_I.ParseToDate();
                    Time timeStart = strStartTime_I.ParseToTime();
                    Time timeEnd = strEndTime_I.ParseToTime();
                    ZonedTime ztimeStart = ZonedTimeTools.NewZonedTime(dateStart, timeStart);
                    ZonedTime ztimeEnd = ZonedTimeTools.NewZonedTime(dateEnd, timeEnd);

                    intStatus_IO = 405;
                    strUserMessage_IO = "The end must be greater than the start.";
                    strDevMessage_IO = "";
                    if (
                        ztimeEnd > ztimeStart
                        )
                    {
                        //                                  //Set pkresource and pkprintshop.
                        int? intnPkResource = null;
                        int? intnPkPrintshop = null;
                        ResResource.subGetPks(boolIsEmployee_I, res_I, ps_I, out intnPkResource, out intnPkPrintshop);

                        //                                  //Establish the connection.
                        Odyssey2Context context = new Odyssey2Context();

                        RuleentityRuleEntityDB ruleentity = context.Rule.FirstOrDefault(rule =>
                            (rule.intnPkResource == intnPkResource) &&
                            (rule.intnPkPrintshop == intnPkPrintshop) &&
                            (rule.strFrecuency == ResResource.strOnce) &&
                            (rule.strFrecuencyValue == strStartDate_I + "|" + strEndDate_I) &&
                            (rule.strStartTime == strStartTime_I) &&
                            (rule.strEndTime == strEndTime_I));

                        intStatus_IO = 406;
                        strUserMessage_IO = "The rule already exists.";
                        strDevMessage_IO = "";
                        if (
                            ruleentity == null
                            )
                        {
                            ResResource.subGetPeriodsToDeleteForOnceRule(res_I, ps_I, boolIsEmployee_I, intnContactId_I,
                                ztimeStart, ztimeEnd, out darrperentity_O);

                            intStatus_IO = 200;
                            strUserMessage_IO = "Success.";
                            strDevMessage_IO = "";
                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subGetForDailyRule(
            ResResource res_I,
            PsPrintShop ps_I,
            bool boolIsEmployee_I,
            int? intnContactId_I,
            String strStartTime_I,
            String strEndTime_I,
            String strRangeStartDate_I,
            String strRangeStartTime_I,
            String strRangeEndDate_I,
            String strRangeEndTime_I,
            out List<PerentityPeriodEntityDB> darrperentity_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            darrperentity_O = new List<PerentityPeriodEntityDB>();

            //                                              //To easy code.
            Time timeStart = strStartTime_I.ParseToTime();
            Time timeEnd = strEndTime_I.ParseToTime();

            ResResource.subChangeTimeEnd00ToLastTime(ref timeEnd);
            strEndTime_I = timeEnd.ToString();

            intStatus_IO = 406;
            strUserMessage_IO = "The end must be greater than the start.";
            strDevMessage_IO = "";
            if (
                timeEnd > timeStart
                )
            {
                //                                          //Set pkresource and pkprintshop.
                int? intnPkResource = null;
                int? intnPkPrintshop = null;
                ResResource.subGetPks(boolIsEmployee_I, res_I, ps_I, out intnPkResource, out intnPkPrintshop);

                //                                          //Establish the connection.
                Odyssey2Context context = new Odyssey2Context();

                RuleentityRuleEntityDB ruleentity = context.Rule.FirstOrDefault(rule =>
                    (rule.intnPkResource == intnPkResource) &&
                    (rule.intnPkPrintshop == intnPkPrintshop) &&
                    (rule.strFrecuency == ResResource.strDaily) &&
                    (rule.strFrecuencyValue == null) &&
                    (rule.strStartTime == strStartTime_I) &&
                    (rule.strEndTime == strEndTime_I) &&
                    (rule.strRangeStartDate == strRangeStartDate_I) &&
                    (rule.strRangeStartTime == strRangeStartTime_I) &&
                    (rule.strRangeEndDate == strRangeEndDate_I) &&
                    (rule.strRangeEndTime == strRangeEndTime_I));

                intStatus_IO = 406;
                strUserMessage_IO = "The rule already exists.";
                strDevMessage_IO = "";
                if (
                    ruleentity == null
                    )
                {
                    ResResource.subGetPeriodsToDeleteForDailyRule(res_I, ps_I, boolIsEmployee_I, intnContactId_I,
                        timeStart, timeEnd, strRangeStartDate_I, strRangeStartTime_I, strRangeEndDate_I,
                        strRangeEndTime_I, out darrperentity_O);

                    intStatus_IO = 200;
                    strUserMessage_IO = "Success.";
                    strDevMessage_IO = "";
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subGetForWeeklyRule(
            ResResource res_I,
            PsPrintShop ps_I,
            bool boolIsEmployee_I,
            int? intnContactId_I,
            String strStartTime_I,
            String strEndTime_I,
            String strRangeStartDate_I,
            String strRangeStartTime_I,
            String strRangeEndDate_I,
            String strRangeEndTime_I,
            int[] arrintDays_I,
            out List<PerentityPeriodEntityDB> darrperentity_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            darrperentity_O = new List<PerentityPeriodEntityDB>();

            //                                              //To easy code.
            Time timeStart = strStartTime_I.ParseToTime();
            Time timeEnd = strEndTime_I.ParseToTime();

            ResResource.subChangeTimeEnd00ToLastTime(ref timeEnd);
            strEndTime_I = timeEnd.ToString();

            intStatus_IO = 406;
            strUserMessage_IO = "The end must be greater than the start.";
            strDevMessage_IO = "";
            if (
                timeEnd > timeStart
                )
            {
                intStatus_IO = 407;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "The array of int must have 7 int.";
                if (
                    arrintDays_I.Length == 7
                    )
                {
                    String strFrecuencyValue = "";
                    foreach (int intI in arrintDays_I)
                    {
                        strFrecuencyValue = strFrecuencyValue + intI;
                    }

                    intStatus_IO = 408;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "The int in the array should be 0 or 1.";
                    if (
                        strFrecuencyValue.Count(c => c == '1') + strFrecuencyValue.Count(c => c == '0') == 7
                        )
                    {
                        //                                  //Set pkresource and pkprintshop.
                        int? intnPkResource = null;
                        int? intnPkPrintshop = null;
                        ResResource.subGetPks(boolIsEmployee_I, res_I, ps_I, out intnPkResource, out intnPkPrintshop);

                        //                                  //Establish the connection.
                        Odyssey2Context context = new Odyssey2Context();

                        RuleentityRuleEntityDB ruleentity = context.Rule.FirstOrDefault(rule =>
                            (rule.intnPkResource == intnPkResource) &&
                            (rule.intnPkPrintshop == intnPkPrintshop) &&
                            (rule.strFrecuency == ResResource.strWeekly) &&
                            (rule.strFrecuencyValue == strFrecuencyValue) &&
                            (rule.strStartTime == strStartTime_I) &&
                            (rule.strEndTime == strEndTime_I) &&
                            (rule.strRangeStartDate == strRangeStartDate_I) &&
                            (rule.strRangeStartTime == strRangeStartTime_I) &&
                            (rule.strRangeEndDate == strRangeEndDate_I) &&
                            (rule.strRangeEndTime == strRangeEndTime_I));

                        intStatus_IO = 406;
                        strUserMessage_IO = "The rule already exists.";
                        strDevMessage_IO = "";
                        if (
                            ruleentity == null
                            )
                        {
                            ResResource.subGetPeriodsToDeleteForWeeklyRule(res_I, ps_I, boolIsEmployee_I, intnContactId_I,
                                timeStart, timeEnd, strFrecuencyValue, strRangeStartDate_I, strRangeStartTime_I,
                                strRangeEndDate_I, strRangeEndTime_I, out darrperentity_O);

                            intStatus_IO = 200;
                            strUserMessage_IO = "Success.";
                            strDevMessage_IO = "";
                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subGetForMonthlyRule(
            ResResource res_I,
            PsPrintShop ps_I,
            bool boolIsEmployee_I,
            int? intnContactId_I,
            String strStartTime_I,
            String strEndTime_I,
            String strRangeStartDate_I,
            String strRangeStartTime_I,
            String strRangeEndDate_I,
            String strRangeEndTime_I,
            int[] arrintDays_I,
            out List<PerentityPeriodEntityDB> darrperentity_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            darrperentity_O = new List<PerentityPeriodEntityDB>();

            //                                              //To easy code.
            Time timeStart = strStartTime_I.ParseToTime();
            Time timeEnd = strEndTime_I.ParseToTime();

            ResResource.subChangeTimeEnd00ToLastTime(ref timeEnd);
            strEndTime_I = timeEnd.ToString();

            intStatus_IO = 406;
            strUserMessage_IO = "The end must be greater than the start.";
            strDevMessage_IO = "";
            if (
                timeEnd > timeStart
                )
            {
                intStatus_IO = 409;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "The array of int must have less than 31 int.";
                if (
                    arrintDays_I.Length <= 31
                    )
                {
                    String strFrecuencyValue = "";
                    List<int> darrint = new List<int>();
                    for (int intI = 1; intI <= 31; intI = intI + 1)
                    {
                        if (
                            arrintDays_I.Contains(intI)
                            )
                        {
                            strFrecuencyValue = strFrecuencyValue + "1";
                            darrint.Add(intI);
                        }
                        else
                        {
                            strFrecuencyValue = strFrecuencyValue + "0";
                        }
                    }

                    intStatus_IO = 410;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "The number of the day has to be between 1 and 31.";
                    if (
                        arrintDays_I.Length == darrint.Count()
                        )
                    {
                        //                                  //Set pkresource and pkprintshop.
                        int? intnPkResource = null;
                        int? intnPkPrintshop = null;
                        ResResource.subGetPks(boolIsEmployee_I, res_I, ps_I, out intnPkResource, out intnPkPrintshop);

                        //                                  //Establish the connection.
                        Odyssey2Context context = new Odyssey2Context();

                        RuleentityRuleEntityDB ruleentity = context.Rule.FirstOrDefault(rule =>
                            (rule.intnPkResource == intnPkResource) &&
                            (rule.intnPkPrintshop == intnPkPrintshop) &&
                            (rule.strFrecuency == ResResource.strMonthly) &&
                            (rule.strFrecuencyValue == strFrecuencyValue) &&
                            (rule.strStartTime == strStartTime_I) &&
                            (rule.strEndTime == strEndTime_I) &&
                            (rule.strRangeStartDate == strRangeStartDate_I) &&
                            (rule.strRangeStartTime == strRangeStartTime_I) &&
                            (rule.strRangeEndDate == strRangeEndDate_I) &&
                            (rule.strRangeEndTime == strRangeEndTime_I));

                        intStatus_IO = 406;
                        strUserMessage_IO = "The rule already exists.";
                        strDevMessage_IO = "";
                        if (
                            ruleentity == null
                            )
                        {
                            ResResource.subGetPeriodsToDeleteForMonthlyRule(res_I, ps_I, boolIsEmployee_I, 
                                intnContactId_I, timeStart, timeEnd, strFrecuencyValue, strRangeStartDate_I, 
                                strRangeStartTime_I, strRangeEndDate_I, strRangeEndTime_I, out darrperentity_O);

                            intStatus_IO = 200;
                            strUserMessage_IO = "Success.";
                            strDevMessage_IO = "";
                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subGetForAnnuallyRule(
            ResResource res_I,
            PsPrintShop ps_I,
            bool boolIsEmployee_I,
            int? intnContactId_I,
            String strStartTime_I,
            String strEndTime_I,
            String strDay_I,
            String strRangeStartDate_I,
            String strRangeStartTime_I,
            String strRangeEndDate_I,
            String strRangeEndTime_I,
            out List<PerentityPeriodEntityDB> darrperentity_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            darrperentity_O = new List<PerentityPeriodEntityDB>();

            //                                              //To easy code.
            Time timeStart = strStartTime_I.ParseToTime();
            Time timeEnd = strEndTime_I.ParseToTime();

            ResResource.subChangeTimeEnd00ToLastTime(ref timeEnd);
            strEndTime_I = timeEnd.ToString();

            intStatus_IO = 406;
            strUserMessage_IO = "The end must be greater than the start.";
            strDevMessage_IO = "";
            if (
                timeEnd > timeStart
                )
            {
                intStatus_IO = 406;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "There is no day and is an annualy";
                if (
                    (strDay_I != null) &&
                    (strDay_I.Length > 2)
                    )
                {
                    String strMonth = strDay_I.Substring(0, 2);
                    String strDay = strDay_I.Substring(2);

                    intStatus_IO = 411;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "The day has an invalid format. The format es MMdd";
                    if (
                        strMonth.IsParsableToInt() &&
                        strDay.IsParsableToInt()
                        )
                    {
                        int intMonth = strMonth.ParseToInt();
                        int intDay = strDay.ParseToInt();

                        intStatus_IO = 412;
                        strUserMessage_IO = "Something is wrong.";
                        strDevMessage_IO = "The first two digits must be a number greater than 0 and smaller than 13. " +
                            "The next two digits must be a number greater tha 0 and smaller than 32.";
                        if (
                            (intMonth > 0) && (intMonth < 13) &&
                            (intDay > 0) && (intDay < 32)
                            )
                        {
                            intStatus_IO = 413;
                            strUserMessage_IO = "Something is wrong.";
                            strDevMessage_IO = "In february the day must be smaller than 30.";
                            if (
                                ((intMonth == 2) &&
                                (intDay < 30)) ||
                                (intMonth != 2)
                                )
                            {
                                //                                      //Set pkresource and pkprintshop.
                                int? intnPkResource = null;
                                int? intnPkPrintshop = null;
                                ResResource.subGetPks(boolIsEmployee_I, res_I, ps_I, out intnPkResource, out intnPkPrintshop);

                                //                                      //Establish the connection.
                                Odyssey2Context context = new Odyssey2Context();

                                RuleentityRuleEntityDB ruleentity = context.Rule.FirstOrDefault(rule =>
                                    (rule.intnPkResource == intnPkResource) &&
                                    (rule.intnPkPrintshop == intnPkPrintshop) &&
                                    (rule.strFrecuency == ResResource.strAnnually) &&
                                    (rule.strFrecuencyValue == strDay_I) &&
                                    (rule.strStartTime == strStartTime_I) &&
                                    (rule.strEndTime == strEndTime_I) &&
                                    (rule.strRangeStartDate == strRangeStartDate_I) &&
                                    (rule.strRangeStartTime == strRangeStartTime_I) &&
                                    (rule.strRangeEndDate == strRangeEndDate_I) &&
                                    (rule.strRangeEndTime == strRangeEndTime_I));

                                intStatus_IO = 406;
                                strUserMessage_IO = "The rule already exists.";
                                strDevMessage_IO = "";
                                if (
                                    ruleentity == null
                                    )
                                {
                                    ResResource.subGetPeriodsToDeleteForAnnuallyRule(res_I, ps_I, boolIsEmployee_I, intnContactId_I,
                                        intMonth, intDay, timeStart, timeEnd, strRangeStartDate_I, strRangeStartTime_I,
                                        strRangeEndDate_I, strRangeEndTime_I, out darrperentity_O);

                                    intStatus_IO = 200;
                                    strUserMessage_IO = "Success.";
                                    strDevMessage_IO = "";
                                }
                            }
                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subChangeTimeEnd00ToLastTime(
            //                                              //End time.
            ref Time timeEnd_IO
            )
        {
            if (
               //                                          //If the timeEnd is 00:00, therefore take value 23:59:59.
               timeEnd_IO.ToString() == Time.MinValue.ToString()
               )
            {
                timeEnd_IO = Time.MaxValue;
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subAddRule(
            bool boolIsEmployee_I,
            int? intnContactId_I,
            int? intnPkResource_I,
            String strPrintshopId_I,
            String strFrecuency_I,
            String strStartTime_I,
            String strEndTime_I,
            String strStartDate_I,
            String strEndDate_I,
            String strRangeStartDate_I,
            String strRangeStartTime_I,
            String strRangeEndDate_I,
            String strRangeEndTime_I,
            int[] arrintDays_I,
            String strDay_I,
            IConfiguration configuration_I,
            IHubContext<ConnectionHub> iHubContext_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            intStatus_IO = 402;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "The times have an invalid format.";
            if (
                strStartTime_I.IsParsableToTime() &&
                strEndTime_I.IsParsableToTime()
                )
            {
                intStatus_IO = 403;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Range data is invalid.";
                //                                              //Local variables for range dates and times.
                String strRangeStartDate = strRangeStartDate_I;
                String strRangeStartTime = strRangeStartTime_I;
                String strRangeEndDate = strRangeEndDate_I;
                String strRangeEndTime = strRangeEndTime_I;
                if (
                    //                                      //Change null data to valid data.
                    ResResource.boolCorrectRangeDates(ref strRangeStartDate, ref strRangeStartTime,
                        ref strRangeEndDate, ref strRangeEndTime)
                    )
                {
                    intStatus_IO = 406;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "No res or no ps or not valid contact.";
                    ResResource res;
                    PsPrintShop ps;
                    if (
                        //                                  //Get res or ps and validate contat.
                        ResResource.boolValidResPsorContact(intnPkResource_I, strPrintshopId_I, intnContactId_I,
                            boolIsEmployee_I, configuration_I, out res, out ps)
                        )
                    {
                        if (
                            //                              //SE NECESITA ESTE OBJETO PARA LA CONVERSION DEL 
                            //                              //      TIMEZONE
                            ps == null
                            )
                        {
                            ps = PsPrintShop.psGet(strPrintshopId_I);
                        }

                        intStatus_IO = 403;
                        strUserMessage_IO = "Something is wrong.";
                        strDevMessage_IO = "Invalid frecuency.";

                        /*CASE*/
                        if (
                            strFrecuency_I == ResResource.strOnce
                            )
                        {
                            ResResource.subAddAOnceRule(res, ps, boolIsEmployee_I, intnContactId_I, strStartTime_I,
                                strEndTime_I, strStartDate_I, strEndDate_I, iHubContext_I, ref intStatus_IO, 
                                ref strUserMessage_IO, ref strDevMessage_IO);
                        }
                        else if (
                            strFrecuency_I == ResResource.strDaily
                            )
                        {
                            ResResource.subAddADailyRule(res, ps, boolIsEmployee_I, intnContactId_I, strStartTime_I,
                                strEndTime_I, strRangeStartDate, strRangeStartTime, strRangeEndDate, strRangeEndTime,
                                iHubContext_I, ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);
                        }
                        else if (
                            strFrecuency_I == ResResource.strWeekly
                            )
                        {
                            ResResource.subAddAWeeklyRule(res, ps, boolIsEmployee_I, intnContactId_I, strStartTime_I,
                                strEndTime_I, strRangeStartDate, strRangeStartTime, strRangeEndDate, strRangeEndTime,
                                arrintDays_I, iHubContext_I, ref intStatus_IO, ref strUserMessage_IO, 
                                ref strDevMessage_IO);
                        }
                        else if (
                            strFrecuency_I == ResResource.strMonthly
                            )
                        {
                            ResResource.subAddAMonthlyRule(res, ps, boolIsEmployee_I, intnContactId_I, strStartTime_I,
                                strEndTime_I, strRangeStartDate, strRangeStartTime, strRangeEndDate, strRangeEndTime,
                                arrintDays_I, iHubContext_I, ref intStatus_IO, ref strUserMessage_IO,
                                ref strDevMessage_IO);
                        }
                        else if (
                            strFrecuency_I == ResResource.strAnnually
                            )
                        {
                            ResResource.subAddAnAnnuallyRule(res, ps, boolIsEmployee_I, intnContactId_I, strStartTime_I,
                                strEndTime_I, strRangeStartDate, strRangeStartTime, strRangeEndDate, strRangeEndTime,
                                strDay_I, iHubContext_I, ref intStatus_IO, ref strUserMessage_IO, 
                                ref strDevMessage_IO);
                        }
                        /*END-CASE*/
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static bool boolValidResPsorContact(
            int? intnPkResource_I,
            String strPrintshopId_I,
            int? intnContactId_I,
            bool boolIsEmployee_I,
            IConfiguration configuration_I,
            out ResResource res_O,
            out PsPrintShop ps_O
            )
        {
            bool boolToReturn = false;

            //                                              //Get res and ps if boolIsEmployee.
            res_O = null;
            ps_O = null;
            if (
                !boolIsEmployee_I
                )
            {
                if (
                    intnPkResource_I != null
                    )
                {
                    res_O = ResResource.resFromDB(intnPkResource_I, false);
                }
                else
                {
                    ps_O = PsPrintShop.psGet(strPrintshopId_I);
                }
                if (
                    (res_O != null) || (ps_O != null)
                    )
                {
                    boolToReturn = true;
                }
            }

            if (
                boolIsEmployee_I &&
                (intnContactId_I != null)
                )
            {
                boolToReturn =
                    ResResource.boolEmployeeIsFromPrintshop(strPrintshopId_I, (int)intnContactId_I);
            }
            return boolToReturn;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static bool boolCorrectRangeDates(

            ref String strRangeStartDate_IO,
            ref String strRangeStartTime_IO,
            ref String strRangeEndDate_IO,
            ref String strRangeEndTime_IO
            )
        {
            bool boolToReturn = false;

            if (
                //                                          //1.
                ((strRangeStartDate_IO != null) && strRangeStartDate_IO.IsParsableToDate() &&
                (strRangeStartTime_IO != null) && strRangeStartTime_IO.IsParsableToTime() &&
                (strRangeEndDate_IO != null) && strRangeEndDate_IO.IsParsableToDate() &&
                (strRangeEndTime_IO != null) && strRangeEndTime_IO.IsParsableToTime())
                ||
                //                                          //2.
                ((strRangeStartDate_IO != null) && strRangeStartDate_IO.IsParsableToDate() &&
                (strRangeStartTime_IO != null) && strRangeStartTime_IO.IsParsableToTime() &&
                (strRangeEndDate_IO == null) &&
                (strRangeEndTime_IO == null))
                ||
                //                                          //3.
                ((strRangeStartDate_IO == null) &&
                (strRangeStartTime_IO == null) &&
                (strRangeEndDate_IO != null) && strRangeEndDate_IO.IsParsableToDate() &&
                (strRangeEndTime_IO != null) && strRangeEndTime_IO.IsParsableToTime())
                ||
                //                                          //4.
                ((strRangeStartDate_IO == null) &&
                (strRangeStartTime_IO == null) &&
                (strRangeEndDate_IO == null) &&
                (strRangeEndTime_IO == null))
                )
            {
                if (
                    (strRangeStartDate_IO == null) &&
                    (strRangeStartTime_IO == null)
                    )
                {
                    strRangeStartDate_IO = Date.Now(ZonedTimeTools.timezone).ToString();
                    strRangeStartTime_IO = Time.Now(ZonedTimeTools.timezone).ToString();
                }

                if (
                    strRangeEndDate_IO == null &&
                    strRangeEndTime_IO == null
                    )
                {
                    strRangeEndDate_IO = Date.MaxValue.ToString();
                    strRangeEndTime_IO = Time.MinValue.ToString();
                }
                boolToReturn = true;
            }

            return boolToReturn;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subAddAOnceRule(
            ResResource res_I,
            PsPrintShop ps_I,
            bool boolIsEmployee_I,
            int? intnContactId_I,
            String strStartTime_I,
            String strEndTime_I,
            String strStartDate_I,
            String strEndDate_I,
            IHubContext<ConnectionHub> iHubContext_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            intStatus_IO = 404;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "The dates have an invalid format.";
            if (
                strStartDate_I.IsParsableToDate() &&
                strEndDate_I.IsParsableToDate()
                )
            {
                //                                          //To easy code.
                Date dateStart = strStartDate_I.ParseToDate();
                Date dateEnd = strEndDate_I.ParseToDate();
                Time timeStart = strStartTime_I.ParseToTime();
                Time timeEnd = strEndTime_I.ParseToTime();
                ZonedTime ztimeStart = ZonedTimeTools.NewZonedTimeConsideringPrintshopTimeZone(dateStart, timeStart,
                    ps_I.strTimeZone);
                ZonedTime ztimeEnd = ZonedTimeTools.NewZonedTimeConsideringPrintshopTimeZone(dateEnd, timeEnd,
                    ps_I.strTimeZone);

                //                                          //Easy code
                String strStartDate = ztimeStart.Date.ToString();
                String strStartTime = ztimeStart.Time.ToString();
                String strEndDate = ztimeEnd.Date.ToString();
                String strEndTime = ztimeEnd.Time.ToString();

                intStatus_IO = 405;
                strUserMessage_IO = "The end must be greater than the start.";
                strDevMessage_IO = "";
                if (
                    ztimeEnd > ztimeStart
                    )
                {
                    //                                      //Set pkresource and pkprintshop.
                    int? intnPkResource = null;
                    int? intnPkPrintshop = null;
                    ResResource.subGetPks(boolIsEmployee_I, res_I, ps_I, out intnPkResource, out intnPkPrintshop);

                    //                                      //Establish the connection.
                    Odyssey2Context context = new Odyssey2Context();

                    RuleentityRuleEntityDB ruleentity = context.Rule.FirstOrDefault(rule =>
                        (rule.intnPkResource == intnPkResource) &&
                        (rule.intnPkPrintshop == intnPkPrintshop) &&
                        (rule.intnContactId == intnContactId_I) &&
                        (rule.strFrecuency == ResResource.strOnce) &&
                        (rule.strFrecuencyValue == strStartDate + "|" + strEndDate) &&
                        (rule.strStartTime == strStartTime) &&
                        (rule.strEndTime == strEndTime));

                    intStatus_IO = 406;
                    strUserMessage_IO = "The rule already exists.";
                    strDevMessage_IO = "";
                    if (
                        ruleentity == null
                        )
                    {
                        //                                  //Get periods to delete if we receive pkresource.
                        List<PerentityPeriodEntityDB> darrperentity;
                        ResResource.subGetPeriodsToDeleteForOnceRule(res_I, ps_I, boolIsEmployee_I, intnContactId_I,
                            ztimeStart, ztimeEnd, out darrperentity);

                        //                                  //Delete the periods that are at the same time.
                        foreach (PerentityPeriodEntityDB perentity in darrperentity)
                        {
                            if (
                                //                          //Period temporary.
                                perentity.intnEstimateId != null
                                )
                            {
                                //                          //Get process 
                                int intPkProcessInWorkflow = context.ProcessInWorkflow.FirstOrDefault(
                                    piw => piw.intPkWorkflow == perentity.intPkWorkflow &&
                                    piw.intProcessInWorkflowId == perentity.intProcessInWorkflowId).intPk;

                                List<EstdataentityEstimationDataEntityDB> darrestdata = context.EstimationData.Where(estdata =>
                                    estdata.intJobId == perentity.intJobId &&
                                    estdata.intId == perentity.intnEstimateId &&
                                    estdata.intPkProcessInWorkflow == intPkProcessInWorkflow).ToList();

                                if (
                                    darrestdata != null
                                    )
                                {
                                    //                      //Delete estimation data.
                                    foreach (EstdataentityEstimationDataEntityDB estdataentity in darrestdata)
                                    {
                                        context.EstimationData.Remove(estdataentity);
                                    }
                                }

                                List<EstentityEstimateEntityDB> darrestentity = context.Estimate.Where(
                                    est => est.intJobId == perentity.intJobId &&
                                    est.intId == perentity.intnEstimateId &&
                                    est.intPkWorkflow == perentity.intPkWorkflow
                                    ).ToList();

                                if (
                                    darrestentity != null
                                    )
                                {
                                    //                      //Delete estimate.
                                    foreach (EstentityEstimateEntityDB estentity in darrestentity)
                                    {
                                        context.Estimate.Remove(estentity);
                                    }
                                }
                            }

                            //                              //Find alerts about this period.
                            List<AlertentityAlertEntityDB> darralertentity = context.Alert.Where(alert =>
                                alert.intnPkPeriod == perentity.intPk).ToList();

                            foreach (AlertentityAlertEntityDB alertentity in darralertentity)
                            {
                                //                          //Delete alerts about this period.

                                if (
                                    //                      //Notification not read.
                                    !PsPrintShop.boolNotificationReadByUser(alertentity, (int)alertentity.intnContactId)
                                    )
                                {
                                    AlnotAlertNotification.subReduceToOne((int)alertentity.intnContactId,
                                        iHubContext_I);
                                }

                                context.Alert.Remove(alertentity);
                            }

                            //                              //Delete period.
                            context.Period.Remove(perentity);
                        }

                        context.SaveChanges();

                        //                                  //Add the rule.
                        ruleentity = new RuleentityRuleEntityDB
                        {
                            strFrecuency = ResResource.strOnce,
                            strFrecuencyValue = strStartDate + "|" + strEndDate,
                            strStartTime = strStartTime,
                            strEndTime = strEndTime,
                            intnPkResource = intnPkResource,
                            intnPkPrintshop = intnPkPrintshop,
                            intnContactId = intnContactId_I
                        };
                        context.Rule.Add(ruleentity);
                        context.SaveChanges();

                        intStatus_IO = 200;
                        strUserMessage_IO = "Success.";
                        strDevMessage_IO = "";
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subGetPeriodsToDeleteForOnceRule(
            ResResource res_I,
            PsPrintShop ps_I,
            bool boolIsEmployee_I,
            int? intnContactId_I,
            ZonedTime ztimeStart_I,
            ZonedTime ztimeEnd_I,
            out List<PerentityPeriodEntityDB> darrperentity_O
            )
        {
            darrperentity_O = new List<PerentityPeriodEntityDB>();

            List<PerentityPeriodEntityDB> darrperentityAllPeriods =
                ResResource.darrperentityAllPeriodsForResourceOrPrintshopOrContact(res_I, ps_I, boolIsEmployee_I,
                intnContactId_I);

            //                                              //Delete the periods that the rule is over.
            foreach (PerentityPeriodEntityDB perentity in darrperentityAllPeriods)
            {
                Date dateStartPeriod = perentity.strStartDate.ParseToDate();
                Date dateEndPeriod = perentity.strEndDate.ParseToDate();
                Time timeStartPeriod = perentity.strStartTime.ParseToTime();
                Time timeEndPeriod = perentity.strEndTime.ParseToTime();

                ZonedTime ztimeStartPeriod = ZonedTimeTools.NewZonedTime(dateStartPeriod, timeStartPeriod);
                ZonedTime ztimeEndPeriod = ZonedTimeTools.NewZonedTime(dateEndPeriod, timeEndPeriod);

                if (
                    //                                      //The period is over the start of the rule.
                    (((ztimeStart_I > ztimeStartPeriod) && (ztimeStart_I < ztimeEndPeriod)) ||
                    //                                      //The priod is over the end of the rule.
                    ((ztimeEnd_I < ztimeEndPeriod) && (ztimeEnd_I > ztimeStartPeriod)) ||
                    //                                      //The period is in the rule.
                    ((ztimeStartPeriod >= ztimeStart_I) && (ztimeStartPeriod <= ztimeEnd_I))) &&
                    (perentity.boolIsException == false)
                    )
                {
                    darrperentity_O.Add(perentity);
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subAddADailyRule(
            ResResource res_I,
            PsPrintShop ps_I,
            bool boolIsEmployee_I,
            int? intnContactId_I,
            String strStartTime_I,
            String strEndTime_I,
            String strRangeStartDate_I,
            String strRangeStartTime_I,
            String strRangeEndDate_I,
            String strRangeEndTime_I,
            IHubContext<ConnectionHub> iHubContext_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //To easy code.
            Time timeStart = strStartTime_I.ParseToTime();
            Time timeEnd = strEndTime_I.ParseToTime();

            ResResource.subChangeTimeEnd00ToLastTime(ref timeEnd);
            strEndTime_I = timeEnd.ToString();

            //                                              //Create objec Ztime in order to use to compare dates,
            //                                              //  only if we dont receive it.
            ZonedTime ztimenStartDate = ZonedTimeTools.NewZonedTimeConsideringPrintshopTimeZone(
                strRangeStartDate_I.ParseToDate(), strRangeStartTime_I.ParseToTime(), ps_I.strTimeZone);
            ZonedTime ztimenEndDate = ZonedTimeTools.NewZonedTimeConsideringPrintshopTimeZone(
                strRangeEndDate_I.ParseToDate(), strRangeEndTime_I.ParseToTime(), ps_I.strTimeZone);

            //                                              //Easy code
            String strRangeStartDate = ztimenStartDate.Date.ToString();
            String strRangeStartTime = ztimenStartDate.Time.ToString();
            String strRangeEndDate = ztimenEndDate.Date.ToString();
            String strRangeEndTime = ztimenEndDate.Time.ToString();

            intStatus_IO = 406;
            strUserMessage_IO = "The end must be greater than the start.";
            strDevMessage_IO = "";
            if (
                //                                          //Verify that enddate be greater than enddate, for the time
                //                                          // of the rule and when we receive the strRangeEndDate.
                (timeEnd > timeStart) &&
                ((ztimenEndDate != null &&
                ztimenEndDate > ztimenStartDate) ||
                (ztimenEndDate == null))
                )
            {
                //                                          //Set pkresource and pkprintshop.
                int? intnPkResource = null;
                int? intnPkPrintshop = null;
                ResResource.subGetPks(boolIsEmployee_I, res_I, ps_I, out intnPkResource, out intnPkPrintshop);

                //                                          //Establish the connection.
                Odyssey2Context context = new Odyssey2Context();

                RuleentityRuleEntityDB ruleentity = context.Rule.FirstOrDefault(rule =>
                (rule.intnPkResource == intnPkResource) &&
                (rule.intnPkPrintshop == intnPkPrintshop) &&
                (rule.intnContactId == intnContactId_I) &&
                (rule.strFrecuency == ResResource.strDaily) &&
                (rule.strFrecuencyValue == null) &&
                (rule.strStartTime == strStartTime_I) &&
                (rule.strEndTime == strEndTime_I) &&
                (rule.strRangeStartDate == strRangeStartDate) &&
                (rule.strRangeStartTime == strRangeStartTime) &&
                (rule.strRangeEndDate == strRangeEndDate) &&
                (rule.strRangeEndTime == strRangeEndTime));

                intStatus_IO = 406;
                strUserMessage_IO = "The rule already exists.";
                strDevMessage_IO = "";
                if (
                    ruleentity == null
                    )
                {
                    //                                      //Get period to delete.
                    List<PerentityPeriodEntityDB> darrperentity;
                    ResResource.subGetPeriodsToDeleteForDailyRule(res_I, ps_I, boolIsEmployee_I, intnContactId_I,
                        timeStart, timeEnd, strRangeStartDate, strRangeStartTime, strRangeEndDate,
                        strRangeEndTime, out darrperentity);

                    //                                      //Delete the periods that are at the same time.
                    foreach (PerentityPeriodEntityDB perentity in darrperentity)
                    {
                        if (
                                //                          //Period temporary.
                                perentity.intnEstimateId != null
                                )
                        {
                            //                              //Get process 
                            int intPkProcessInWorkflow = context.ProcessInWorkflow.FirstOrDefault(
                                piw => piw.intPkWorkflow == perentity.intPkWorkflow &&
                                piw.intProcessInWorkflowId == perentity.intProcessInWorkflowId).intPk;

                            List<EstdataentityEstimationDataEntityDB> darrestdata = context.EstimationData.Where(estdata =>
                                estdata.intJobId == perentity.intJobId &&
                                estdata.intId == perentity.intnEstimateId &&
                                estdata.intPkProcessInWorkflow == intPkProcessInWorkflow).ToList();

                            if (
                                darrestdata != null
                                )
                            {
                                //                          //Delete estimation data.
                                foreach (EstdataentityEstimationDataEntityDB estdataentity in darrestdata)
                                {
                                    context.EstimationData.Remove(estdataentity);
                                }
                            }

                            List<EstentityEstimateEntityDB> darrestentity = context.Estimate.Where(
                                est => est.intJobId == perentity.intJobId &&
                                est.intId == perentity.intnEstimateId &&
                                est.intPkWorkflow == perentity.intPkWorkflow
                                ).ToList();

                            if (
                                darrestentity != null
                                )
                            {
                                //                          //Delete estimate.
                                foreach (EstentityEstimateEntityDB estentity in darrestentity)
                                {
                                    context.Estimate.Remove(estentity);
                                }
                            }
                        }

                        //                                  //Find alerts about this period.
                        List<AlertentityAlertEntityDB> darralertentity = context.Alert.Where(alert =>
                            alert.intnPkPeriod == perentity.intPk).ToList();

                        foreach (AlertentityAlertEntityDB alertentity in darralertentity)
                        {
                            //                              //Delete alerts about this period.

                            if (
                                //                          //Notification not read.
                                !PsPrintShop.boolNotificationReadByUser(alertentity, (int)alertentity.intnContactId)
                                )
                            {
                                AlnotAlertNotification.subReduceToOne((int)alertentity.intnContactId,
                                    iHubContext_I);
                            }

                            context.Alert.Remove(alertentity);
                        }

                        context.Period.Remove(perentity);
                    }
                    context.SaveChanges();

                    //                                      //Add the rule.
                    ruleentity = new RuleentityRuleEntityDB
                    {
                        strFrecuency = ResResource.strDaily,
                        strFrecuencyValue = null,
                        strStartTime = strStartTime_I,
                        strEndTime = strEndTime_I,
                        strRangeStartDate = strRangeStartDate,
                        strRangeEndDate = strRangeEndDate,
                        strRangeStartTime = strRangeStartTime,
                        strRangeEndTime = strRangeEndTime,
                        intnPkResource = intnPkResource,
                        intnPkPrintshop = intnPkPrintshop,
                        intnContactId = intnContactId_I
                    };
                    context.Rule.Add(ruleentity);
                    context.SaveChanges();

                    intStatus_IO = 200;
                    strUserMessage_IO = "Success.";
                    strDevMessage_IO = "";
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subGetPeriodsToDeleteForDailyRule(
            ResResource res_I,
            PsPrintShop ps_I,
            bool boolIsEmployee_I,
            int? intnContactId_I,
            Time timeStart_I,
            Time timeEnd_I,
            String strRangeStartDate_I,
            String strRangeStartTime_I,
            String strRangeEndDate_I,
            String strRangeEndTime_I,
            out List<PerentityPeriodEntityDB> darrperentity_O
            )
        {
            darrperentity_O = new List<PerentityPeriodEntityDB>();

            List<PerentityPeriodEntityDB> darrperentityAllPeriods =
                ResResource.darrperentityAllPeriodsForResourceOrPrintshopOrContact(res_I, ps_I, boolIsEmployee_I,
                intnContactId_I);

            //                                              //To easy code.
            Date dateRangeStart = strRangeStartDate_I.ParseToDate();
            Time timeRangeStart = strRangeStartTime_I.ParseToTime();
            ZonedTime ztimeRangeStart = ZonedTimeTools.NewZonedTime(dateRangeStart, timeRangeStart);

            Date dateRangeEnd = strRangeEndDate_I.ParseToDate();
            Time timeRangeEnd = strRangeEndTime_I.ParseToTime();
            ZonedTime ztimeRangeEnd = ZonedTimeTools.NewZonedTime(dateRangeEnd, timeRangeEnd);

            //                                              //Delete the periods that the rule is over.
            foreach (PerentityPeriodEntityDB perentity in darrperentityAllPeriods)
            {
                Date dateStartPeriod = perentity.strStartDate.ParseToDate();
                Date dateEndPeriod = perentity.strEndDate.ParseToDate();
                Time timeStartPeriod = perentity.strStartTime.ParseToTime();
                Time timeEndPeriod = perentity.strEndTime.ParseToTime();
                ZonedTime ztimeStartPeriod = ZonedTimeTools.NewZonedTime(dateStartPeriod, timeStartPeriod);
                ZonedTime ztimeEndPeriod = ZonedTimeTools.NewZonedTime(dateEndPeriod, timeEndPeriod);

                ZonedTime ztimeStartRuleDay1 = ZonedTimeTools.NewZonedTime(dateStartPeriod, timeStart_I);
                ZonedTime ztimeEndRuleDay1 = ZonedTimeTools.NewZonedTime(dateStartPeriod, timeEnd_I);
                ZonedTime ztimeStartRuleDay2 = ZonedTimeTools.NewZonedTime(dateEndPeriod, timeStart_I);
                ZonedTime ztimeEndRuleDay2 = ZonedTimeTools.NewZonedTime(dateEndPeriod, timeEnd_I);

                if (
                    //                                      //The period or a part of it is between the validity range 
                    //                                      //      of the rule.
                    ((((ztimeStartPeriod >= ztimeRangeStart) && (ztimeEndPeriod <= ztimeRangeEnd)) ||
                    ((ztimeStartPeriod < ztimeRangeStart) && (ztimeEndPeriod > ztimeRangeEnd)) ||
                    ((ztimeStartPeriod >= ztimeRangeStart) && (ztimeStartPeriod <= ztimeRangeEnd)) ||
                    ((ztimeEndPeriod >= ztimeRangeStart) && (ztimeEndPeriod <= ztimeRangeEnd))) &&
                    //                                      //The period is over the start of the rule.
                    (((ztimeStartRuleDay1 > ztimeStartPeriod) && (ztimeStartRuleDay2 < ztimeEndPeriod)) ||
                    ((ztimeStartRuleDay2 < ztimeEndPeriod) && (dateEndPeriod > dateStartPeriod)) ||
                    //                                      //The period is over the end of the rule.
                    ((ztimeEndRuleDay2 < ztimeEndPeriod) && (ztimeEndRuleDay1 > ztimeStartPeriod)) ||
                    ((ztimeEndRuleDay1 > ztimeStartPeriod) && (dateEndPeriod > dateStartPeriod)) ||
                    //                                      //The period is over the rule in a intermediate day.
                    (dateEndPeriod - dateStartPeriod > 1) ||
                    ((ztimeStartPeriod >= ztimeStartRuleDay1) && (ztimeEndPeriod <= ztimeEndRuleDay1)))) &&
                    (perentity.boolIsException == false)
                    )
                {
                    darrperentity_O.Add(perentity);
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subAddAWeeklyRule(
            ResResource res_I,
            PsPrintShop ps_I,
            bool boolIsEmployee_I,
            int? intnContactId_I,
            String strStartTime_I,
            String strEndTime_I,
            String strRangeStartDate_I,
            String strRangeStartTime_I,
            String strRangeEndDate_I,
            String strRangeEndTime_I,
            int[] arrintDays_I,
            IHubContext<ConnectionHub> iHubContext_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //To easy code.
            Time timeStart = strStartTime_I.ParseToTime();
            Time timeEnd = strEndTime_I.ParseToTime();

            ResResource.subChangeTimeEnd00ToLastTime(ref timeEnd);
            strEndTime_I = timeEnd.ToString();

            //                                              //Create objec Ztime in order to use to compare dates,
            //                                              //  only if we dont receive it.
            ZonedTime ztimenStartDate = ZonedTimeTools.NewZonedTimeConsideringPrintshopTimeZone(
                strRangeStartDate_I.ParseToDate(), strRangeStartTime_I.ParseToTime(), ps_I.strTimeZone);
            ZonedTime ztimenEndDate = ZonedTimeTools.NewZonedTimeConsideringPrintshopTimeZone(
                strRangeEndDate_I.ParseToDate(), strRangeEndTime_I.ParseToTime(), ps_I.strTimeZone);

            //                                              //Easy code
            String strRangeStartDate = ztimenStartDate.Date.ToString();
            String strRangeStartTime = ztimenStartDate.Time.ToString();
            String strRangeEndDate = ztimenEndDate.Date.ToString();
            String strRangeEndTime = ztimenEndDate.Time.ToString();

            intStatus_IO = 406;
            strUserMessage_IO = "The end must be greater than the start.";
            strDevMessage_IO = "";
            if (
                //                                          //Verify that enddate be greater than enddate, for the time
                //                                          // of the rule and when we receive the strRangeEndDate.
                (timeEnd > timeStart) &&
                ((ztimenEndDate != null &&
                ztimenEndDate > ztimenStartDate) ||
                (ztimenEndDate == null))
                )
            {
                intStatus_IO = 407;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "The array of int must have 7 int.";
                if (
                    arrintDays_I.Length == 7
                    )
                {
                    String strFrecuencyValue = "";
                    foreach (int intI in arrintDays_I)
                    {
                        strFrecuencyValue = strFrecuencyValue + intI;
                    }

                    intStatus_IO = 408;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "The int in the array should be 0 or 1.";
                    if (
                        strFrecuencyValue.Count(c => c == '1') + strFrecuencyValue.Count(c => c == '0') == 7
                        )
                    {
                        //                                  //Set pkresource and pkprintshop.
                        int? intnPkResource = null;
                        int? intnPkPrintshop = null;
                        ResResource.subGetPks(boolIsEmployee_I, res_I, ps_I, out intnPkResource, out intnPkPrintshop);

                        //                                  //Establish the connection.
                        Odyssey2Context context = new Odyssey2Context();

                        RuleentityRuleEntityDB ruleentity = context.Rule.FirstOrDefault(rule =>
                        (rule.intnPkResource == intnPkResource) &&
                        (rule.intnPkPrintshop == intnPkPrintshop) &&
                        (rule.intnContactId == intnContactId_I) &&
                        (rule.strFrecuency == ResResource.strWeekly) &&
                        (rule.strFrecuencyValue == strFrecuencyValue) &&
                        (rule.strStartTime == strStartTime_I) &&
                        (rule.strEndTime == strEndTime_I) &&
                        (rule.strRangeStartDate == strRangeStartDate) &&
                        (rule.strRangeStartTime == strRangeStartTime) &&
                        (rule.strRangeEndDate == strRangeEndDate) &&
                        (rule.strRangeEndTime == strRangeEndTime));

                        intStatus_IO = 406;
                        strUserMessage_IO = "The rule already exists.";
                        strDevMessage_IO = "";
                        if (
                            ruleentity == null
                            )
                        {
                            List<PerentityPeriodEntityDB> darrperentity;
                            ResResource.subGetPeriodsToDeleteForWeeklyRule(res_I, ps_I, boolIsEmployee_I, intnContactId_I,
                                timeStart, timeEnd, strFrecuencyValue, strRangeStartDate, strRangeStartTime,
                                strRangeEndDate, strRangeEndTime, out darrperentity);

                            //                              //Delete the periods that are at the same time.
                            foreach (PerentityPeriodEntityDB perentity in darrperentity)
                            {
                                if (
                                //                          //Period temporary.
                                perentity.intnEstimateId != null
                                )
                                {
                                    //                      //Get process 
                                    int intPkProcessInWorkflow = context.ProcessInWorkflow.FirstOrDefault(
                                        piw => piw.intPkWorkflow == perentity.intPkWorkflow &&
                                        piw.intProcessInWorkflowId == perentity.intProcessInWorkflowId).intPk;

                                    List<EstdataentityEstimationDataEntityDB> darrestdata = context.EstimationData.Where(estdata =>
                                        estdata.intJobId == perentity.intJobId &&
                                        estdata.intId == perentity.intnEstimateId &&
                                        estdata.intPkProcessInWorkflow == intPkProcessInWorkflow).ToList();

                                    if (
                                        darrestdata != null
                                        )
                                    {
                                        //                  //Delete estimation data.
                                        foreach (EstdataentityEstimationDataEntityDB estdataentity in darrestdata)
                                        {
                                            context.EstimationData.Remove(estdataentity);
                                        }
                                    }

                                    List<EstentityEstimateEntityDB> darrestentity = context.Estimate.Where(
                                        est => est.intJobId == perentity.intJobId &&
                                        est.intId == perentity.intnEstimateId &&
                                        est.intPkWorkflow == perentity.intPkWorkflow
                                        ).ToList();

                                    if (
                                        darrestentity != null
                                        )
                                    {
                                        //                  //Delete estimate.
                                        foreach (EstentityEstimateEntityDB estentity in darrestentity)
                                        {
                                            context.Estimate.Remove(estentity);
                                        }
                                    }
                                }

                                //                              //Find alerts about this period.
                                List<AlertentityAlertEntityDB> darralertentity = context.Alert.Where(alert =>
                                    alert.intnPkPeriod == perentity.intPk).ToList();

                                foreach (AlertentityAlertEntityDB alertentity in darralertentity)
                                {
                                    //                          //Delete alerts about this period.

                                    if (
                                        //                      //Notification not read.
                                        !PsPrintShop.boolNotificationReadByUser(alertentity, (int)alertentity.intnContactId)
                                        )
                                    {
                                        AlnotAlertNotification.subReduceToOne((int)alertentity.intnContactId,
                                            iHubContext_I);
                                    }

                                    context.Alert.Remove(alertentity);
                                }

                                context.Period.Remove(perentity);
                            }
                            context.SaveChanges();

                            //                              //Add the rule.
                            ruleentity = new RuleentityRuleEntityDB
                            {
                                strFrecuency = ResResource.strWeekly,
                                strFrecuencyValue = strFrecuencyValue,
                                strStartTime = strStartTime_I,
                                strEndTime = strEndTime_I,
                                strRangeStartDate = strRangeStartDate,
                                strRangeEndDate = strRangeEndDate,
                                strRangeStartTime = strRangeStartTime,
                                strRangeEndTime = strRangeEndTime,
                                intnPkResource = intnPkResource,
                                intnPkPrintshop = intnPkPrintshop,
                                intnContactId = intnContactId_I
                            };
                            context.Rule.Add(ruleentity);
                            context.SaveChanges();

                            intStatus_IO = 200;
                            strUserMessage_IO = "Success.";
                            strDevMessage_IO = "";
                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subGetPeriodsToDeleteForWeeklyRule(
            ResResource res_I,
            PsPrintShop ps_I,
            bool boolIsEmployee_I,
            int? intnContactId_I,
            Time timeStart_I,
            Time timeEnd_I,
            String strFrecuencyValue_I,
            String strRangeStartDate_I,
            String strRangeStartTime_I,
            String strRangeEndDate_I,
            String strRangeEndTime_I,
            out List<PerentityPeriodEntityDB> darrperentity_O
            )
        {
            darrperentity_O = new List<PerentityPeriodEntityDB>();

            List<PerentityPeriodEntityDB> darrperentityAllPeriods =
                ResResource.darrperentityAllPeriodsForResourceOrPrintshopOrContact(res_I, ps_I, boolIsEmployee_I,
                intnContactId_I);

            //                                              //To easy code.
            Date dateRangeStart = strRangeStartDate_I.ParseToDate();
            Time timeRangeStart = strRangeStartTime_I.ParseToTime();
            ZonedTime ztimeRangeStart = ZonedTimeTools.NewZonedTime(dateRangeStart, timeRangeStart);

            Date dateRangeEnd = strRangeEndDate_I.ParseToDate();
            Time timeRangeEnd = strRangeEndTime_I.ParseToTime();
            ZonedTime ztimeRangeEnd = ZonedTimeTools.NewZonedTime(dateRangeEnd, timeRangeEnd);

            //                                              //Delete the periods that the rule is over.
            foreach (PerentityPeriodEntityDB perentity in darrperentityAllPeriods)
            {
                Date dateStartPeriod = perentity.strStartDate.ParseToDate();
                Date dateEndPeriod = perentity.strEndDate.ParseToDate();
                Time timeStartPeriod = perentity.strStartTime.ParseToTime();
                Time timeEndPeriod = perentity.strEndTime.ParseToTime();
                ZonedTime ztimeStartPeriod = ZonedTimeTools.NewZonedTime(dateStartPeriod, timeStartPeriod);
                ZonedTime ztimeEndPeriod = ZonedTimeTools.NewZonedTime(dateEndPeriod, timeEndPeriod);

                int intStartDay = (int)dateStartPeriod.DayOfWeek;
                int intEndDay = (int)dateEndPeriod.DayOfWeek;

                ZonedTime ztimeStartRuleDay1 = ZonedTimeTools.NewZonedTime(dateStartPeriod, timeStart_I);
                ZonedTime ztimeEndRuleDay1 = ZonedTimeTools.NewZonedTime(dateStartPeriod, timeEnd_I);
                ZonedTime ztimeStartRuleDay2 = ZonedTimeTools.NewZonedTime(dateEndPeriod, timeStart_I);
                ZonedTime ztimeEndRuleDay2 = ZonedTimeTools.NewZonedTime(dateEndPeriod, timeEnd_I);

                bool boolPeriodNeedsToBeDeleted = false;
                if (
                    //                                      //The period or a part of it is between the validity range 
                    //                                      //      of the rule.
                    ((((ztimeStartPeriod >= ztimeRangeStart) && (ztimeEndPeriod <= ztimeRangeEnd)) ||
                    ((ztimeStartPeriod < ztimeRangeStart) && (ztimeEndPeriod > ztimeRangeEnd)) ||
                    ((ztimeStartPeriod >= ztimeRangeStart) && (ztimeStartPeriod <= ztimeRangeEnd)) ||
                    ((ztimeEndPeriod >= ztimeRangeStart) && (ztimeEndPeriod <= ztimeRangeEnd))) &&
                    //                                      //The period is over the start of the rule.
                    (((strFrecuencyValue_I[intStartDay] == '1') && (ztimeStartRuleDay1 > ztimeStartPeriod) &&
                    (strFrecuencyValue_I[intEndDay] == '1') && (ztimeStartRuleDay2 < ztimeEndPeriod)) ||
                    ((strFrecuencyValue_I[intEndDay] == '1') && (ztimeStartRuleDay2 < ztimeEndPeriod) &&
                    (dateEndPeriod > dateStartPeriod)) ||
                    //                                      //The period is over the end of the rule.
                    ((strFrecuencyValue_I[intEndDay] == '1') && (ztimeEndRuleDay2 < ztimeEndPeriod) &&
                    (strFrecuencyValue_I[intStartDay] == '1') && (ztimeEndRuleDay1 > ztimeStartPeriod)) ||
                    ((strFrecuencyValue_I[intStartDay] == '1') && (ztimeEndRuleDay1 > ztimeStartPeriod) &&
                    (dateEndPeriod > dateStartPeriod)) ||
                    ((strFrecuencyValue_I[intStartDay] == '1') && (ztimeStartPeriod >= ztimeStartRuleDay1) &&
                    (strFrecuencyValue_I[intEndDay] == '1') && (ztimeEndPeriod <= ztimeEndRuleDay1)))) &&
                    (perentity.boolIsException == false)
                    )
                {
                    boolPeriodNeedsToBeDeleted = true;
                }

                intStartDay = intStartDay + 1;
                /*UNTIL-DO*/
                while (!(
                    (intStartDay >= intEndDay) ||
                    boolPeriodNeedsToBeDeleted
                    ))
                {
                    if (
                        strFrecuencyValue_I[intStartDay] == 1
                        )
                    {
                        boolPeriodNeedsToBeDeleted = true;
                    }

                    intStartDay = intStartDay + 1;
                }

                if (
                    boolPeriodNeedsToBeDeleted
                    )
                {
                    darrperentity_O.Add(perentity);
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subAddAMonthlyRule(
            ResResource res_I,
            PsPrintShop ps_I,
            bool boolIsEmployee_I,
            int? intnContactId_I,
            String strStartTime_I,
            String strEndTime_I,
            String strRangeStartDate_I,
            String strRangeStartTime_I,
            String strRangeEndDate_I,
            String strRangeEndTime_I,
            int[] arrintDays_I,
            IHubContext<ConnectionHub> iHubContext_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //To easy code.
            Time timeStart = strStartTime_I.ParseToTime();
            Time timeEnd = strEndTime_I.ParseToTime();

            ResResource.subChangeTimeEnd00ToLastTime(ref timeEnd);
            strEndTime_I = timeEnd.ToString();

            //                                              //Create objec Ztime in order to use to compare dates,
            //                                              //  only if we dont receive it.
            ZonedTime ztimenStartDate = ZonedTimeTools.NewZonedTimeConsideringPrintshopTimeZone(
                strRangeStartDate_I.ParseToDate(), strRangeStartTime_I.ParseToTime(), ps_I.strTimeZone); ;
            ZonedTime ztimenEndDate = ZonedTimeTools.NewZonedTimeConsideringPrintshopTimeZone(
                strRangeEndDate_I.ParseToDate(), strRangeEndTime_I.ParseToTime(), ps_I.strTimeZone);

            //                                              //Easy code
            String strRangeStartDate = ztimenStartDate.Date.ToString();
            String strRangeStartTime = ztimenStartDate.Time.ToString();
            String strRangeEndDate = ztimenEndDate.Date.ToString();
            String strRangeEndTime = ztimenEndDate.Time.ToString();

            intStatus_IO = 406;
            strUserMessage_IO = "The end must be greater than the start.";
            strDevMessage_IO = "";
            if (
                //                                          //Verify that enddate be greater than enddate, for the time
                //                                          // of the rule and when we receive the strRangeEndDate.
                (timeEnd > timeStart) &&
                ((ztimenEndDate != null &&
                ztimenEndDate > ztimenStartDate) ||
                (ztimenEndDate == null))
                )
            {
                intStatus_IO = 409;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "The array of int must have less than 31 int.";
                if (
                    (arrintDays_I != null) && (arrintDays_I.Length <= 31)
                    )
                {
                    String strFrecuencyValue = "";
                    List<int> darrint = new List<int>();
                    for (int intI = 1; intI <= 31; intI = intI + 1)
                    {
                        if (
                            arrintDays_I.Contains(intI)
                            )
                        {
                            strFrecuencyValue = strFrecuencyValue + "1";
                            darrint.Add(intI);
                        }
                        else
                        {
                            strFrecuencyValue = strFrecuencyValue + "0";
                        }
                    }

                    intStatus_IO = 410;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "The number of the day has to be between 1 and 31.";
                    if (
                        arrintDays_I.Length == darrint.Count()
                        )
                    {
                        //                                      //Set pkresource and pkprintshop.
                        int? intnPkResource = null;
                        int? intnPkPrintshop = null;
                        ResResource.subGetPks(boolIsEmployee_I, res_I, ps_I, out intnPkResource, out intnPkPrintshop);

                        //                                      //Establish the connection.
                        Odyssey2Context context = new Odyssey2Context();

                        RuleentityRuleEntityDB ruleentity = context.Rule.FirstOrDefault(rule =>
                        (rule.intnPkResource == intnPkResource) &&
                        (rule.intnPkPrintshop == intnPkPrintshop) &&
                        (rule.intnContactId == intnContactId_I) &&
                        (rule.strFrecuency == ResResource.strMonthly) &&
                        (rule.strFrecuencyValue == strFrecuencyValue) &&
                        (rule.strStartTime == strStartTime_I) &&
                        (rule.strEndTime == strEndTime_I) &&
                        (rule.strRangeStartDate == strRangeStartDate) &&
                        (rule.strRangeStartTime == strRangeStartTime) &&
                        (rule.strRangeEndDate == strRangeEndDate) &&
                        (rule.strRangeEndTime == strRangeEndTime));

                        intStatus_IO = 406;
                        strUserMessage_IO = "The rule already exists.";
                        strDevMessage_IO = "";
                        if (
                            ruleentity == null
                            )
                        {
                            List<PerentityPeriodEntityDB> darrperentity;
                            ResResource.subGetPeriodsToDeleteForMonthlyRule(res_I, ps_I, boolIsEmployee_I,
                                intnContactId_I, timeStart, timeEnd, strFrecuencyValue, strRangeStartDate,
                                strRangeStartTime, strRangeEndDate, strRangeEndTime, out darrperentity);

                            //                              //Delete the periods that are at the same time.
                            foreach (PerentityPeriodEntityDB perentity in darrperentity)
                            {
                                if (
                                //                          //Period temporary.
                                perentity.intnEstimateId != null
                                )
                                {
                                    //                      //Get process 
                                    int intPkProcessInWorkflow = context.ProcessInWorkflow.FirstOrDefault(
                                        piw => piw.intPkWorkflow == perentity.intPkWorkflow &&
                                        piw.intProcessInWorkflowId == perentity.intProcessInWorkflowId).intPk;

                                    List<EstdataentityEstimationDataEntityDB> darrestdata = context.EstimationData.Where(estdata =>
                                        estdata.intJobId == perentity.intJobId &&
                                        estdata.intId == perentity.intnEstimateId &&
                                        estdata.intPkProcessInWorkflow == intPkProcessInWorkflow).ToList();

                                    if (
                                        darrestdata != null
                                        )
                                    {
                                        //                  //Delete estimation data.
                                        foreach (EstdataentityEstimationDataEntityDB estdataentity in darrestdata)
                                        {
                                            context.EstimationData.Remove(estdataentity);
                                        }
                                    }

                                    List<EstentityEstimateEntityDB> darrestentity = context.Estimate.Where(
                                        est => est.intJobId == perentity.intJobId &&
                                        est.intId == perentity.intnEstimateId &&
                                        est.intPkWorkflow == perentity.intPkWorkflow
                                        ).ToList();

                                    if (
                                        darrestentity != null
                                        )
                                    {
                                        //                  //Delete estimate.
                                        foreach (EstentityEstimateEntityDB estentity in darrestentity)
                                        {
                                            context.Estimate.Remove(estentity);
                                        }
                                    }
                                }

                                //                          //Find alerts about this period.
                                List<AlertentityAlertEntityDB> darralertentity = context.Alert.Where(alert =>
                                    alert.intnPkPeriod == perentity.intPk).ToList();

                                foreach (AlertentityAlertEntityDB alertentity in darralertentity)
                                {
                                    //                      //Delete alerts about this period.

                                    if (
                                        //                  //Notification not read.
                                        !PsPrintShop.boolNotificationReadByUser(alertentity, (int)alertentity.intnContactId)
                                        )
                                    {
                                        AlnotAlertNotification.subReduceToOne((int)alertentity.intnContactId,
                                            iHubContext_I);
                                    }

                                    context.Alert.Remove(alertentity);
                                }

                                context.Period.Remove(perentity);
                            }
                            context.SaveChanges();

                            //                              //Add the rule.
                            ruleentity = new RuleentityRuleEntityDB
                            {
                                strFrecuency = ResResource.strMonthly,
                                strFrecuencyValue = strFrecuencyValue,
                                strStartTime = strStartTime_I,
                                strEndTime = strEndTime_I,
                                strRangeStartDate = strRangeStartDate,
                                strRangeEndDate = strRangeEndDate,
                                strRangeStartTime = strRangeStartTime,
                                strRangeEndTime = strRangeEndTime,
                                intnPkResource = intnPkResource,
                                intnPkPrintshop = intnPkPrintshop,
                                intnContactId = intnContactId_I
                            };
                            context.Rule.Add(ruleentity);
                            context.SaveChanges();

                            intStatus_IO = 200;
                            strUserMessage_IO = "Success.";
                            strDevMessage_IO = "";
                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subGetPeriodsToDeleteForMonthlyRule(
            ResResource res_I,
            PsPrintShop ps_I,
            bool boolIsEmployee_I,
            int? intnContactId_I,
            Time timeStart_I,
            Time timeEnd_I,
            String strFrecuencyValue_I,
            String strRangeStartDate_I,
            String strRangeStartTime_I,
            String strRangeEndDate_I,
            String strRangeEndTime_I,
            out List<PerentityPeriodEntityDB> darrperentity_O
            )
        {
            darrperentity_O = new List<PerentityPeriodEntityDB>();

            List<PerentityPeriodEntityDB> darrperentityAllPeriods =
                ResResource.darrperentityAllPeriodsForResourceOrPrintshopOrContact(res_I, ps_I, boolIsEmployee_I,
                intnContactId_I);

            //                                              //To easy code.
            Date dateRangeStart = strRangeStartDate_I.ParseToDate();
            Time timeRangeStart = strRangeStartTime_I.ParseToTime();
            ZonedTime ztimeRangeStart = ZonedTimeTools.NewZonedTime(dateRangeStart, timeRangeStart);

            Date dateRangeEnd = strRangeEndDate_I.ParseToDate();
            Time timeRangeEnd = strRangeEndTime_I.ParseToTime();
            ZonedTime ztimeRangeEnd = ZonedTimeTools.NewZonedTime(dateRangeEnd, timeRangeEnd);

            //                                              //Delete the periods that the rule is over.
            foreach (PerentityPeriodEntityDB perentity in darrperentityAllPeriods)
            {
                Date dateStartPeriod = perentity.strStartDate.ParseToDate();
                Date dateEndPeriod = perentity.strEndDate.ParseToDate();
                Time timeStartPeriod = perentity.strStartTime.ParseToTime();
                Time timeEndPeriod = perentity.strEndTime.ParseToTime();
                ZonedTime ztimeStartPeriod = ZonedTimeTools.NewZonedTime(dateStartPeriod, timeStartPeriod);
                ZonedTime ztimeEndPeriod = ZonedTimeTools.NewZonedTime(dateEndPeriod, timeEndPeriod);

                int intStartDay = dateStartPeriod.Day - 1;
                int intEndDay = dateEndPeriod.Day - 1;

                ZonedTime ztimeStartRuleDay1 = ZonedTimeTools.NewZonedTime(dateStartPeriod, timeStart_I);
                ZonedTime ztimeEndRuleDay1 = ZonedTimeTools.NewZonedTime(dateStartPeriod, timeEnd_I);
                ZonedTime ztimeStartRuleDay2 = ZonedTimeTools.NewZonedTime(dateEndPeriod, timeStart_I);
                ZonedTime ztimeEndRuleDay2 = ZonedTimeTools.NewZonedTime(dateEndPeriod, timeEnd_I);

                bool boolPeriodNeedsToBeDeleted = false;
                if (
                    //                                      //The period or a part of it is between the validity range 
                    //                                      //      of the rule.
                    ((((ztimeStartPeriod >= ztimeRangeStart) && (ztimeEndPeriod <= ztimeRangeEnd)) ||
                    ((ztimeStartPeriod < ztimeRangeStart) && (ztimeEndPeriod > ztimeRangeEnd)) ||
                    ((ztimeStartPeriod >= ztimeRangeStart) && (ztimeStartPeriod <= ztimeRangeEnd)) ||
                    ((ztimeEndPeriod >= ztimeRangeStart) && (ztimeEndPeriod <= ztimeRangeEnd))) &&
                    //                                      //The period is over the start of the rule.
                    (((strFrecuencyValue_I[intStartDay] == '1') && (ztimeStartRuleDay1 > ztimeStartPeriod) &&
                    (strFrecuencyValue_I[intEndDay] == '1') && (ztimeStartRuleDay2 < ztimeEndPeriod)) ||
                    ((strFrecuencyValue_I[intEndDay] == '1') && (ztimeStartRuleDay2 < ztimeEndPeriod) &&
                    (dateEndPeriod > dateStartPeriod)) ||
                    //                                      //The period is over the end of the rule.
                    ((strFrecuencyValue_I[intEndDay] == '1') && (ztimeEndRuleDay2 < ztimeEndPeriod) &&
                    (strFrecuencyValue_I[intStartDay] == '1') && (ztimeEndRuleDay1 > ztimeStartPeriod)) ||
                    ((strFrecuencyValue_I[intStartDay] == '1') && (ztimeEndRuleDay1 > ztimeStartPeriod) &&
                    (dateEndPeriod > dateStartPeriod)) ||
                    ((strFrecuencyValue_I[intStartDay] == '1') && (ztimeStartPeriod >= ztimeStartRuleDay1) &&
                    (strFrecuencyValue_I[intEndDay] == '1') && (ztimeEndPeriod <= ztimeEndRuleDay1)))) &&
                    (perentity.boolIsException == false)
                    )
                {
                    boolPeriodNeedsToBeDeleted = true;
                }

                intStartDay = intStartDay + 1;
                /*UNTIL-DO*/
                while (!(
                    (intStartDay >= intEndDay) ||
                    boolPeriodNeedsToBeDeleted
                    ))
                {
                    if (
                        strFrecuencyValue_I[intStartDay] == 1
                        )
                    {
                        boolPeriodNeedsToBeDeleted = true;
                    }

                    intStartDay = intStartDay + 1;
                }

                if (
                    boolPeriodNeedsToBeDeleted
                    )
                {
                    darrperentity_O.Add(perentity);
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subAddAnAnnuallyRule(
            ResResource res_I,
            PsPrintShop ps_I,
            bool boolIsEmployee_I,
            int? intnContactId_I,
            String strStartTime_I,
            String strEndTime_I,
            String strRangeStartDate_I,
            String strRangeStartTime_I,
            String strRangeEndDate_I,
            String strRangeEndTime_I,
            String strDay_I,
            IHubContext<ConnectionHub> iHubContext_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //To easy code.
            Time timeStart = strStartTime_I.ParseToTime();
            Time timeEnd = strEndTime_I.ParseToTime();

            ResResource.subChangeTimeEnd00ToLastTime(ref timeEnd);
            strEndTime_I = timeEnd.ToString();

            //                                              //Create objec Ztime in order to use to compare dates,
            //                                              //  only if we dont receive it.
            ZonedTime ztimenStartDate = ZonedTimeTools.NewZonedTimeConsideringPrintshopTimeZone(
                strRangeStartDate_I.ParseToDate(), strRangeStartTime_I.ParseToTime(), ps_I.strTimeZone); 
            ZonedTime ztimenEndDate = ZonedTimeTools.NewZonedTimeConsideringPrintshopTimeZone(
                strRangeEndDate_I.ParseToDate(), strRangeEndTime_I.ParseToTime(), ps_I.strTimeZone);

            //                                              //Easy code
            String strRangeStartDate = ztimenStartDate.Date.ToString();
            String strRangeStartTime = ztimenStartDate.Time.ToString();
            String strRangeEndDate = ztimenEndDate.Date.ToString();
            String strRangeEndTime = ztimenEndDate.Time.ToString();

            intStatus_IO = 406;
            strUserMessage_IO = "The end must be greater than the start.";
            strDevMessage_IO = "";
            if (
                //                                          //Verify that enddate be greater than enddate, for the time
                //                                          // of the rule and when we receive the strRangeEndDate.
                (timeEnd > timeStart) &&
                ((ztimenEndDate != null &&
                ztimenEndDate > ztimenStartDate) ||
                (ztimenEndDate == null))
                )
            {
                String strMonth = strDay_I.Substring(0, 2);
                String strDay = strDay_I.Substring(2);

                intStatus_IO = 411;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "The day has an invalid format. The format es MMdd";
                if (
                    strMonth.IsParsableToInt() &&
                    strDay.IsParsableToInt()
                    )
                {
                    int intMonth = strMonth.ParseToInt();
                    int intDay = strDay.ParseToInt();

                    intStatus_IO = 412;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "The first two digits must be a number greater than 0 and smaller than 13. " +
                        "The next two digits must be a number greater tha 0 and smaller than 32.";
                    if (
                        (intMonth > 0) && (intMonth < 13) &&
                        (intDay > 0) && (intDay < 32)
                        )
                    {
                        intStatus_IO = 413;
                        strUserMessage_IO = "Something is wrong.";
                        strDevMessage_IO = "In february the day must be smaller than 30.";
                        if (
                            ((intMonth == 2) &&
                            (intDay < 30)) ||
                            (intMonth != 2)
                            )
                        {
                            //                                      //Set pkresource and pkprintshop.
                            int? intnPkResource = null;
                            int? intnPkPrintshop = null;
                            ResResource.subGetPks(boolIsEmployee_I, res_I, ps_I, out intnPkResource, out intnPkPrintshop);

                            //                                      //Establish the connection.
                            Odyssey2Context context = new Odyssey2Context();

                            RuleentityRuleEntityDB ruleentity = context.Rule.FirstOrDefault(rule =>
                            (rule.intnPkResource == intnPkResource) &&
                            (rule.intnPkPrintshop == intnPkPrintshop) &&
                            (rule.intnContactId == intnContactId_I) &&
                            (rule.strFrecuency == ResResource.strAnnually) &&
                            (rule.strFrecuencyValue == strDay_I) &&
                            (rule.strStartTime == strStartTime_I) &&
                            (rule.strEndTime == strEndTime_I) &&
                            (rule.strRangeStartDate == strRangeStartDate) &&
                            (rule.strRangeStartTime == strRangeStartTime) &&
                            (rule.strRangeEndDate == strRangeEndDate) &&
                            (rule.strRangeEndTime == strRangeEndTime));

                            intStatus_IO = 406;
                            strUserMessage_IO = "The rule already exists.";
                            strDevMessage_IO = "";
                            if (
                                ruleentity == null
                                )
                            {
                                List<PerentityPeriodEntityDB> darrperentity;
                                ResResource.subGetPeriodsToDeleteForAnnuallyRule(res_I, ps_I, boolIsEmployee_I,
                                    intnContactId_I, intMonth, intDay, timeStart, timeEnd, strRangeStartDate,
                                    strRangeStartTime, strRangeEndDate, strRangeEndTime, out darrperentity);

                                //                          //Delete the periods that are at the same time.
                                foreach (PerentityPeriodEntityDB perentity in darrperentity)
                                {
                                    if (
                                        //                          //Period temporary.
                                        perentity.intnEstimateId != null
                                      )
                                    {
                                        //                  //Get process 
                                        int intPkProcessInWorkflow = context.ProcessInWorkflow.FirstOrDefault(
                                            piw => piw.intPkWorkflow == perentity.intPkWorkflow &&
                                            piw.intProcessInWorkflowId == perentity.intProcessInWorkflowId).intPk;

                                        List<EstdataentityEstimationDataEntityDB> darrestdata = context.EstimationData.Where(estdata =>
                                            estdata.intJobId == perentity.intJobId &&
                                            estdata.intId == perentity.intnEstimateId &&
                                            estdata.intPkProcessInWorkflow == intPkProcessInWorkflow).ToList();

                                        if (
                                            darrestdata != null
                                            )
                                        {
                                            //              //Delete estimation data.
                                            foreach (EstdataentityEstimationDataEntityDB estdataentity in darrestdata)
                                            {
                                                context.EstimationData.Remove(estdataentity);
                                            }
                                        }

                                        List<EstentityEstimateEntityDB> darrestentity = context.Estimate.Where(
                                            est => est.intJobId == perentity.intJobId &&
                                            est.intId == perentity.intnEstimateId &&
                                            est.intPkWorkflow == perentity.intPkWorkflow
                                            ).ToList();

                                        if (
                                            darrestentity != null
                                            )
                                        {
                                            //              //Delete estimate.
                                            foreach (EstentityEstimateEntityDB estentity in darrestentity)
                                            {
                                                context.Estimate.Remove(estentity);
                                            }
                                        }
                                    }

                                    //                      //Find alerts about this period.
                                    List<AlertentityAlertEntityDB> darralertentity = context.Alert.Where(alert =>
                                        alert.intnPkPeriod == perentity.intPk).ToList();

                                    foreach (AlertentityAlertEntityDB alertentity in darralertentity)
                                    {
                                        //                  //Delete alerts about this period.

                                        if (
                                            //              //Notification not read.
                                            !PsPrintShop.boolNotificationReadByUser(alertentity, (int)alertentity.intnContactId)
                                            )
                                        {
                                            AlnotAlertNotification.subReduceToOne((int)alertentity.intnContactId,
                                                iHubContext_I);
                                        }

                                        context.Alert.Remove(alertentity);
                                    }

                                    context.Period.Remove(perentity);
                                }
                                context.SaveChanges();

                                //                          //Add the rule.
                                ruleentity = new RuleentityRuleEntityDB
                                {
                                    strFrecuency = ResResource.strAnnually,
                                    strFrecuencyValue = strDay_I,
                                    strStartTime = strStartTime_I,
                                    strEndTime = strEndTime_I,
                                    strRangeStartDate = strRangeStartDate,
                                    strRangeEndDate = strRangeEndDate,
                                    strRangeStartTime = strRangeStartTime,
                                    strRangeEndTime = strRangeEndTime,
                                    intnPkResource = intnPkResource,
                                    intnPkPrintshop = intnPkPrintshop,
                                    intnContactId = intnContactId_I
                                };
                                context.Rule.Add(ruleentity);
                                context.SaveChanges();

                                intStatus_IO = 200;
                                strUserMessage_IO = "Success.";
                                strDevMessage_IO = "";
                            }
                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subGetPeriodsToDeleteForAnnuallyRule(
            ResResource res_I,
            PsPrintShop ps_I,
            bool boolIsEmployee_I,
            int? intnContactId_I,
            int intMonth_I,
            int intDay_I,
            Time timeStart_I,
            Time timeEnd_I,
            String strRangeStartDate_I,
            String strRangeStartTime_I,
            String strRangeEndDate_I,
            String strRangeEndTime_I,
            out List<PerentityPeriodEntityDB> darrperentity_O
            )
        {
            darrperentity_O = new List<PerentityPeriodEntityDB>();

            List<PerentityPeriodEntityDB> darrperentityAllPeriods =
                ResResource.darrperentityAllPeriodsForResourceOrPrintshopOrContact(res_I, ps_I, boolIsEmployee_I,
                intnContactId_I);

            //                                              //To easy code.
            Date dateRangeStart = strRangeStartDate_I.ParseToDate();
            Time timeRangeStart = strRangeStartTime_I.ParseToTime();
            ZonedTime ztimeRangeStart = ZonedTimeTools.NewZonedTime(dateRangeStart, timeRangeStart);

            Date dateRangeEnd = strRangeEndDate_I.ParseToDate();
            Time timeRangeEnd = strRangeEndTime_I.ParseToTime();
            ZonedTime ztimeRangeEnd = ZonedTimeTools.NewZonedTime(dateRangeEnd, timeRangeEnd);

            //                                              //Delete the periods that the rule is over.
            foreach (PerentityPeriodEntityDB perentity in darrperentityAllPeriods)
            {
                Date dateStartPeriod = perentity.strStartDate.ParseToDate();
                Date dateEndPeriod = perentity.strEndDate.ParseToDate();
                Time timeStartPeriod = perentity.strStartTime.ParseToTime();
                Time timeEndPeriod = perentity.strEndTime.ParseToTime();
                ZonedTime ztimeStartPeriod = ZonedTimeTools.NewZonedTime(dateStartPeriod, timeStartPeriod);
                ZonedTime ztimeEndPeriod = ZonedTimeTools.NewZonedTime(dateEndPeriod, timeEndPeriod);

                Date dateRuleDay1 = new Date(dateStartPeriod.Year, intMonth_I, intDay_I);
                Date dateRuleDay2 = new Date(dateEndPeriod.Year, intMonth_I, intDay_I);

                ZonedTime ztimeStartRuleDay1 = ZonedTimeTools.NewZonedTime(dateRuleDay1, timeStart_I);
                ZonedTime ztimeEndRuleDay1 = ZonedTimeTools.NewZonedTime(dateRuleDay1, timeEnd_I);
                ZonedTime ztimeStartRuleDay2 = ZonedTimeTools.NewZonedTime(dateRuleDay2, timeStart_I);
                ZonedTime ztimeEndRuleDay2 = ZonedTimeTools.NewZonedTime(dateRuleDay2, timeEnd_I);

                if (
                    //                                      //The period or a part of it is between the validity range 
                    //                                      //      of the rule.
                    ((((ztimeStartPeriod >= ztimeRangeStart) && (ztimeEndPeriod <= ztimeRangeEnd)) ||
                    ((ztimeStartPeriod < ztimeRangeStart) && (ztimeEndPeriod > ztimeRangeEnd)) ||
                    ((ztimeStartPeriod >= ztimeRangeStart) && (ztimeStartPeriod <= ztimeRangeEnd)) ||
                    ((ztimeEndPeriod >= ztimeRangeStart) && (ztimeEndPeriod <= ztimeRangeEnd))) &&
                    //                                      //The period is over the start of the rule.
                    (((ztimeStartRuleDay1 > ztimeStartPeriod) && (ztimeStartRuleDay2 < ztimeEndPeriod)) ||
                    ((ztimeStartRuleDay2 < ztimeEndPeriod) &&
                    (dateEndPeriod.Year > dateStartPeriod.Year)) ||
                    //                                      //The period is over the end of the rule.
                    ((ztimeEndRuleDay2 < ztimeEndPeriod) && (ztimeEndRuleDay1 > ztimeStartPeriod)) ||
                    ((ztimeEndRuleDay1 > ztimeStartPeriod) &&
                    (dateEndPeriod.Year > dateStartPeriod.Year)) ||
                    ((ztimeStartPeriod >= ztimeStartRuleDay1) && (ztimeEndPeriod <= ztimeEndRuleDay1)))) &&
                    (perentity.boolIsException == false)
                    )
                {
                    darrperentity_O.Add(perentity);
                }
            }
        }
        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subGetPks(
            //                                              //Just to get the Pks, it is used more that 8 times.

            bool boolIsEmployee_I,
            ResResource res_I,
            PsPrintShop ps_I,
            out int? intnPkResource_O,
            out int? intnPkPrintshop_O
            )
        {
            intnPkResource_O = null;
            intnPkPrintshop_O = null;
            if (
                !boolIsEmployee_I
                )
            {
                if (
                    res_I != null
                    )
                {
                    intnPkResource_O = res_I.intPk;
                }
                else
                {
                    intnPkPrintshop_O = ps_I.intPk;
                }
            }
        }
        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -- - -
        private static List<PerentityPeriodEntityDB> darrperentityAllPeriodsForResourceOrPrintshopOrContact(
            //                                              //Get all the periods for a res or for the ps
            //                                              //      or for an employee.

            ResResource res_I,
            PsPrintShop ps_I,
            bool boolIsEmployee_I,
            int? intnContactId_I
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            List<PerentityPeriodEntityDB> darrperentityAllPeriods = new List<PerentityPeriodEntityDB>();

            if (
                !boolIsEmployee_I
                )
            {
                if (
                    //                                      //It is a Resource rule.
                    res_I != null
                    )
                {
                    //                                      //Get the periods for that resource.
                    IQueryable<PerentityPeriodEntityDB> setperentity = context.Period.Where(per =>
                        per.intPkElement == res_I.intPk && per.strFinalEndDate == null);
                    darrperentityAllPeriods = setperentity.ToList();
                }
                else
                {
                    //                                      //Is a printshop rule.
                    //                                      //Get printshop resources and procsses.
                    IQueryable<EleentityElementEntityDB> seteleentity =
                    from eleentity in context.Element
                    join etentity in context.ElementType
                    on eleentity.intPkElementType equals etentity.intPk
                    where etentity.intPrintshopPk == ps_I.intPk &&
                    (etentity.strResOrPro == ProdtypProductType.strResource ||
                    etentity.strResOrPro == ProdtypProductType.strProcess)
                    select eleentity;
                    List<EleentityElementEntityDB> darreleentity = seteleentity.ToList();

                    foreach (EleentityElementEntityDB eleentity in darreleentity)
                    {
                        //                                  //Get the periods for that resource.
                        IQueryable<PerentityPeriodEntityDB> setperentity = context.Period.Where(per =>
                            per.intPkElement == eleentity.intPk && per.strFinalEndDate == null);
                        List<PerentityPeriodEntityDB> darrperentity = setperentity.ToList();
                        //                                  //Add each period to list of all periods.
                        foreach (PerentityPeriodEntityDB perentity in darrperentity)
                        {
                            darrperentityAllPeriods.Add(perentity);
                        }
                    }
                }
            }
            else
            {
                //                                          //It is for an employee.
                //                                          //Get the periods for that resource.
                IQueryable<PerentityPeriodEntityDB> setperentity = context.Period.Where(per =>
                    per.intnContactId == intnContactId_I);
                darrperentityAllPeriods = setperentity.ToList();
            }
            return darrperentityAllPeriods;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subDeleteRule(
            //                                              //Delete a rule from db.
            int intPkRule_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Establish the connection with the db.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get the rule.
            RuleentityRuleEntityDB ruleentity = context.Rule.FirstOrDefault(rule => rule.intPk == intPkRule_I);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Rule not found.";
            if (
                //                                          //If the pk exists.
                ruleentity != null
                )
            {
                //                                          //Remove rule.
                context.Remove(ruleentity);
                context.SaveChanges();
                intStatus_IO = 200;
                strUserMessage_IO = " ";
                strDevMessage_IO = "Success.";
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subAddPeriod(
            //                                              //Add a resource period to DB.

            String strPrintshopId_I,
            int intPkResource_I,
            int? intnContactId_I,
            String strPassword_I,
            String strStartDate_I,
            String strStartTime_I,
            String strEndDate_I,
            String strEndTime_I,
            int intJobId_I,
            int intPkProcessInWorkflow_I,
            int intPkEleetOrEleele_I,
            bool boolIsEleet_I,
            //                                              //True -> this method is used to confirm temporary periods.
            //                                              //False -> this method is used from the controller,
            //                                              //      to verify if a period from job workflow is addable.    
            bool boolIsTemporary_I,
            int intMinsBeforeDelete_I,
            IConfiguration configuration_I,
            IHubContext<ConnectionHub> iHubContext_I,
            out String strLastSunday_O,
            out int intPkPeriod_O,
            out String strEstimatedDate_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            strLastSunday_O = "";
            intPkPeriod_O = 0;
            strEstimatedDate_O = "";

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "No job found on Wisnet.";
            JobjsonJobJson jobjsonJob;
            if (
                JobJob.boolIsValidJobId(intJobId_I, strPrintshopId_I, configuration_I, out jobjsonJob, 
                ref strUserMessage_IO, ref strDevMessage_IO)
                )
            {
                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Date or time format is not valid.";
                ZonedTime ztimeStartPeriodToAdd;
                ZonedTime ztimeEndPeriodToAdd;
                //                                          //To easy code.
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);
                if (
                    ZonedTimeTools.boolIsValidStartDateTimeAndEndDateTime(strStartDate_I, strStartTime_I, strEndDate_I,
                    strEndTime_I, ps.strTimeZone, out ztimeStartPeriodToAdd, out ztimeEndPeriodToAdd,
                    ref strUserMessage_IO, ref strDevMessage_IO)
                    )
                {
                    //                                      //To easy code.
                    int? intnPkElementElementType = null;
                    int? intnPkElementElement = null;
                    if (
                        boolIsEleet_I
                        )
                    {
                        intnPkElementElementType = intPkEleetOrEleele_I;
                    }
                    else
                    {
                        intnPkElementElement = intPkEleetOrEleele_I;
                    }

                    //                                      //Establish the connection to the db.
                    Odyssey2Context context = new Odyssey2Context();

                    //                                      //Get the res.
                    EleentityElementEntityDB eleentity = context.Element.FirstOrDefault(ele =>
                        ele.intPk == intPkResource_I);

                    //                                      //Get the type.
                    EtentityElementTypeEntityDB etentity = context.ElementType.FirstOrDefault(et =>
                        et.intPk == eleentity.intPkElementType);

                    intStatus_IO = 405;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "Pk not found, or is not a res or is not calendar.";
                    if (
                        eleentity != null &&
                        eleentity.boolnIsCalendar == true &&
                        etentity.strResOrPro == EtElementTypeAbstract.strResource
                        )
                    {
                        PiwentityProcessInWorkflowEntityDB piwentity = context.
                                                ProcessInWorkflow.FirstOrDefault(piw =>
                                                piw.intPk == intPkProcessInWorkflow_I);

                        //              //Get job's correct processes.
                        List<PiwentityProcessInWorkflowEntityDB> darrpiwentityAllProcesses;
                        List<DynLkjsonDynamicLinkJson> darrdynlkjson;
                        ProdtypProductType.subGetWorkflowValidWay(piwentity.intPkWorkflow, jobjsonJob,
                            out darrpiwentityAllProcesses, out darrdynlkjson);

                        intStatus_IO = 406;
                        strUserMessage_IO = "Previous processes will not have been completed by" +
                            " this time. Please set a different period.";
                        strDevMessage_IO = "";
                        if (
                            //                              //Do not overlap with periods from previous processes.
                            ProProcess.boolIsValidConsideringThePreviousProcesses(
                                intPkProcessInWorkflow_I, intJobId_I, ztimeStartPeriodToAdd,
                                boolIsTemporary_I, darrpiwentityAllProcesses)
                            )
                        {
                            intStatus_IO = 407;
                            strUserMessage_IO = "There is a similar period already set.";
                            strDevMessage_IO = "";
                            if (
                                //                          //Do not overlap with any other period of the same type.
                                ResResource.boolNotOverlapWithOtherPeriod(null, intPkResource_I,
                                    ztimeStartPeriodToAdd, ztimeEndPeriodToAdd, intJobId_I,
                                    boolIsTemporary_I)
                                )
                            {
                                intStatus_IO = 408;
                                strUserMessage_IO = "Something is wrong.";
                                strDevMessage_IO = "No employee found.";
                                if (
                                    (intnContactId_I == null) ||
                                    ResResource.boolEmployeeIsFromPrintshop(strPrintshopId_I,
                                    (int)intnContactId_I)
                                    )
                                {
                                    //                      //Get PkPrintshop.
                                    PsentityPrintshopEntityDB psentity = context.Printshop.
                                        FirstOrDefault(ps => ps.strPrintshopId == strPrintshopId_I);
                                    int intPkPrintshop = psentity.intPk;
                                    //                      //Do not overlap with rules.
                                    bool boolDoNotOverlapRule = ResResource.boolNotOverlapWithRule
                                        (intPkPrintshop, intPkResource_I, ztimeStartPeriodToAdd,
                                        ztimeEndPeriodToAdd);

                                    bool boolIsValidForEmployee = ResResource.boolEmployeeIsValid(
                                        intnContactId_I, ztimeStartPeriodToAdd, ztimeEndPeriodToAdd);

                                    intStatus_IO = 409;
                                    strUserMessage_IO = "The times are for an unavailable period " +
                                        "of time or incorrect password.";
                                    strDevMessage_IO = "";
                                    if (
                                        (boolDoNotOverlapRule && boolIsValidForEmployee) ||
                                        ((!boolDoNotOverlapRule || !boolIsValidForEmployee) &&
                                        psentity.strSpecialPassword == strPassword_I)
                                        )
                                    {
                                        String strDeleteDate;
                                        String strDeleteHour;
                                        String strDeleteMinute;
                                        if (
                                            ProProcess.boolIsMinsBeforeDeleteCorrect(strStartDate_I,
                                                strStartTime_I, strEndDate_I,
                                                strEndTime_I, intMinsBeforeDelete_I,
                                                out strDeleteDate, out strDeleteHour,
                                                out strDeleteMinute, ref strUserMessage_IO)
                                            )
                                        {
                                            bool boolIsException = false;
                                            if (
                                                (!boolDoNotOverlapRule || !boolIsValidForEmployee) &&
                                                psentity.strSpecialPassword == strPassword_I
                                                )
                                            {
                                                boolIsException = true;
                                            }

                                            //              //Transform str to date. 
                                            Date dateStartDate = strStartDate_I.ParseToDate();
                                            //      //Get the day of the week of that date. 
                                            int intDayOfWeek = (int)dateStartDate.DayOfWeek;
                                            if (
                                                //          //Verify if it is a sunday. 
                                                intDayOfWeek == 0
                                                )
                                            {
                                                //          //Return the same date. 
                                                strLastSunday_O = strStartDate_I;
                                            }
                                            else
                                            {
                                                //          //Calculate last sunday.
                                                Date dateLastSunday = dateStartDate - intDayOfWeek;
                                                strLastSunday_O = dateLastSunday.ToText();
                                            }

                                            PerentityPeriodEntityDB perentity = new PerentityPeriodEntityDB
                                            {
                                                strStartDate = ztimeStartPeriodToAdd.Date.ToString(),
                                                strStartTime = ztimeStartPeriodToAdd.Time.ToString(),
                                                strEndDate = ztimeEndPeriodToAdd.Date.ToString(),
                                                strEndTime = ztimeEndPeriodToAdd.Time.ToString(),
                                                intJobId = intJobId_I,
                                                intPkWorkflow = piwentity.intPkWorkflow,
                                                intProcessInWorkflowId = piwentity.intProcessInWorkflowId,
                                                intPkElement = intPkResource_I,
                                                intnPkElementElementType = intnPkElementElementType,
                                                intnPkElementElement = intnPkElementElement,
                                                boolIsException = boolIsException,
                                                intnContactId = intnContactId_I,
                                                intMinsBeforeDelete = intMinsBeforeDelete_I,
                                                strDeleteDate = strDeleteDate,
                                                strDeleteHour = strDeleteHour,
                                                strDeleteMinute = strDeleteMinute
                                            };
                                            context.Period.Add(perentity);

                                            String strProcesses;
                                            List<int> darrintPkPeriodToDelete;
                                            if (
                                                !ProProcess.boolIsValidConsideringTheNextProcesses(
                                                    intPkProcessInWorkflow_I, intJobId_I,
                                                    ztimeEndPeriodToAdd, boolIsTemporary_I,
                                                    darrpiwentityAllProcesses, out strProcesses, 
                                                    out darrintPkPeriodToDelete)
                                                )
                                            {
                                                //          //Delete following periods.
                                                foreach (int intPkPeriod in darrintPkPeriodToDelete)
                                                {
                                                    PerentityPeriodEntityDB perentityToDelete =
                                                        context.Period.FirstOrDefault(per =>
                                                        per.intPk == intPkPeriod);

                                                    //      //Find alerts about this period.
                                                    List<AlertentityAlertEntityDB> darralertentity =
                                                        context.Alert.Where(alert =>
                                                        alert.intnPkPeriod ==
                                                        perentityToDelete.intPk).ToList();

                                                    foreach (AlertentityAlertEntityDB alertentity in darralertentity)
                                                    {
                                                        //  //Delete alerts about this period.

                                                        if (
                                                            //Notification not read.
                                                            !PsPrintShop.boolNotificationReadByUser(alertentity,
                                                                (int)alertentity.intnContactId)
                                                            )
                                                        {
                                                            AlnotAlertNotification.subReduceToOne(
                                                                (int)alertentity.intnContactId,
                                                                iHubContext_I);
                                                        }

                                                        context.Alert.Remove(alertentity);
                                                    }
                                                    context.Period.Remove(perentityToDelete);
                                                }
                                            }
                                            context.SaveChanges();

                                            intPkPeriod_O = perentity.intPk;

                                            strEstimatedDate_O = ProProcess.strEstimateDateJob(perentity.intPkWorkflow,
                                                strPrintshopId_I, jobjsonJob, context);

                                            intStatus_IO = 200;
                                            strUserMessage_IO = "";
                                            strDevMessage_IO = "Success.";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subPeriodIsAddable(
            String strPrintshopId_I,
            int? intnPkPeriod_I,
            int intPkResource_I,
            int? intnContactId_I,
            String strStartDate_I,
            String strStartTime_I,
            String strEndDate_I,
            String strEndTime_I,
            int intJobId_I,
            int intPkProcessInWorkflow_I,
            //                                              //True -> this method is used to confirm temporary periods.
            //                                              //False -> this method is used from the controller,
            //                                              //      to verify if a period from job workflow is addable.    
            bool boolIsTemporary_I,
            IConfiguration configuration_I,
            out Perisaddablejson2PeriodIsAddableJson2 perisaddablejson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            perisaddablejson_O = null;

            //                                          
            Odyssey2Context context = new Odyssey2Context();

            JobjsonJobJson jobjsonJob_O = null;
            intStatus_IO = 401;
            if (
                JobJob.boolIsValidJobId(
                    intJobId_I, strPrintshopId_I, configuration_I, out jobjsonJob_O, ref strUserMessage_IO,
                    ref strDevMessage_IO
                    )
                )
            {
                PiwentityProcessInWorkflowEntityDB piwentity = context.ProcessInWorkflow.FirstOrDefault(piw =>
                    piw.intPk == intPkProcessInWorkflow_I);

                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "No process in workflow found.";
                if (
                    piwentity != null
                    )
                {
                    ResResource resResource = ResResource.resFromDB(intPkResource_I, false);

                    intStatus_IO = 402;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "No resource found.";
                    if (
                        resResource != null
                        )
                    {
                        ZonedTime ztimeStartPeriodToAdd = new ZonedTime();
                        ZonedTime ztimeEndPeriodToAdd = new ZonedTime();

                        //                                  //To easy code.
                        PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);
                        intStatus_IO = 403;
                        if (
                            ZonedTimeTools.boolIsValidStartDateTimeAndEndDateTime(strStartDate_I, strStartTime_I,
                            strEndDate_I, strEndTime_I, ps.strTimeZone, out ztimeStartPeriodToAdd,
                            out ztimeEndPeriodToAdd, ref strUserMessage_IO, ref strDevMessage_IO)
                            )
                        {
                            intStatus_IO = 404;
                            strUserMessage_IO = "Something is wrong.";
                            strDevMessage_IO = "Res is not calendar.";
                            if (
                                 resResource.boolnIsCalendar == true
                                )
                            {
                                //              //Get job's correct processes.
                                List<PiwentityProcessInWorkflowEntityDB> darrpiwentityAllProcesses;
                                List<DynLkjsonDynamicLinkJson> darrdynlkjson;
                                ProdtypProductType.subGetWorkflowValidWay(piwentity.intPkWorkflow, jobjsonJob_O,
                                    out darrpiwentityAllProcesses, out darrdynlkjson);

                                intStatus_IO = 404;
                                strUserMessage_IO = "Previous processes will not have been completed by" +
                                        " this time. Please set a different period.";
                                strDevMessage_IO = "";
                                if (
                                    ProProcess.boolIsValidConsideringThePreviousProcesses(
                                        intPkProcessInWorkflow_I, intJobId_I, ztimeStartPeriodToAdd, boolIsTemporary_I,
                                        darrpiwentityAllProcesses)
                                    )
                                {
                                    intStatus_IO = 405;
                                    strUserMessage_IO = "Somthing wrong.";
                                    strDevMessage_IO = "No employee found.";
                                    if (
                                        (intnContactId_I == null) ||
                                        ResResource.boolEmployeeIsFromPrintshop(strPrintshopId_I, (int)intnContactId_I)
                                        )
                                    {
                                        bool boolIsAddableAboutPeriods = true;

                                        String strProcesses;
                                        String strForProcesses = "";
                                        List<int> darrintPkPeriodToDelete;
                                        if (
                                            !ProProcess.boolIsValidConsideringTheNextProcesses(
                                                intPkProcessInWorkflow_I, intJobId_I, ztimeEndPeriodToAdd,
                                                boolIsTemporary_I, darrpiwentityAllProcesses, out strProcesses, 
                                                out darrintPkPeriodToDelete)
                                            )
                                        {
                                            ProProcess.subCreateTheStringsForProcessesAndResources(
                                                strProcesses, out strForProcesses);

                                            boolIsAddableAboutPeriods = false;
                                            strUserMessage_IO = strForProcesses;
                                        }

                                        //                  //Considering the rules.
                                        bool boolIsAddableAboutRules = true;

                                        bool boolDoNotOverlapPeriod = ResResource.boolNotOverlapWithOtherPeriod(
                                            intnPkPeriod_I, intPkResource_I, ztimeStartPeriodToAdd,
                                            ztimeEndPeriodToAdd, intJobId_I, boolIsTemporary_I);

                                        intStatus_IO = 409;
                                        strUserMessage_IO = "There is a period already set.";
                                        strDevMessage_IO = "";
                                        if (
                                            boolDoNotOverlapPeriod
                                            )
                                        {
                                            strUserMessage_IO = "";
                                            //              //Get PkPrintshop.
                                            PsentityPrintshopEntityDB psentity = context.Printshop.FirstOrDefault(ps =>
                                            ps.strPrintshopId == strPrintshopId_I);
                                            int intPkPrintshop = psentity.intPk;

                                            bool boolDoNotOverlapRule = ResResource.boolNotOverlapWithRule
                                                (intPkPrintshop, intPkResource_I, ztimeStartPeriodToAdd,
                                                ztimeEndPeriodToAdd);

                                            bool boolIsValidForEmployee = ResResource.boolEmployeeIsValid(
                                                intnContactId_I, ztimeStartPeriodToAdd, ztimeEndPeriodToAdd);

                                            String strJobsOfPeriods;
                                            bool boolDoNotOverlapWithEmployeePeriods = ResResource.
                                                boolDoNotOverlapWithEmployeePeriods(intnContactId_I, 
                                                ztimeStartPeriodToAdd, ztimeEndPeriodToAdd, intnPkPeriod_I, intJobId_I,
                                                boolIsTemporary_I, out strJobsOfPeriods);

                                            if (
                                                !boolDoNotOverlapRule ||
                                                !boolIsValidForEmployee
                                                )
                                            {
                                                if (
                                                    !boolIsAddableAboutPeriods
                                                    )
                                                {
                                                    strForProcesses = strForProcesses.TrimEnd('.') + " and " +
                                                        "the times are for an unavailable period of time. You " +
                                                        "would need a password to set the period.";
                                                }
                                                else
                                                {
                                                    strForProcesses = "The times are for an unavailable period " +
                                                        "of time. You would need a password to set the period.";
                                                }

                                                //                      //Period is addable.
                                                boolIsAddableAboutRules = false;
                                            }

                                            if (
                                                !boolDoNotOverlapWithEmployeePeriods
                                                )
                                            {
                                                strForProcesses = strForProcesses + "This period overlaps with another" +
                                                    " employee's period.";
                                                boolIsAddableAboutPeriods = false;
                                            }

                                            strUserMessage_IO = strUserMessage_IO + strForProcesses;

                                            perisaddablejson_O = new Perisaddablejson2PeriodIsAddableJson2(
                                                        boolIsAddableAboutPeriods, boolIsAddableAboutRules);

                                            intStatus_IO = 200;
                                            strDevMessage_IO = "";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subModifyPeriod(
            //                                              //Edit a resource's period.
            String strPrintshopId_I,
            int intPkPeriod_I,
            int intPkResource_I,
            int? intnContactId_I,
            int intJobId_I,
            String strPassword_I,
            String strStartDate_I,
            String strStartTime_I,
            String strEndDate_I,
            String strEndTime_I,
            int intPkProcessInWorkflow_I,
            //                                              //True -> this method is used to confirm temporary periods.
            //                                              //False -> this method is used from the controller,
            //                                              //      to verify if a period from job workflow is addable.    
            bool boolIsTemporary_I,
            int intMinsBeforeDelete_I,
            IConfiguration configuration_I,
            IHubContext<ConnectionHub> iHubContext_I,
            out String strLastSunday_O,
            out String strEstimatedDate_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Date or time format is not valid.";
            strLastSunday_O = null;
            strEstimatedDate_O = "";
            if (
                //                                          //Validate date and time strings are parseable.
                strStartDate_I.IsParsableToDate() && strStartTime_I.IsParsableToTime() &&
                strEndDate_I.IsParsableToDate() && strEndTime_I.IsParsableToTime()
                )
            {
                //                                          //To easy code.
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);
                ZonedTime ztimeStartPeriodToAdd = 
                    ZonedTimeTools.NewZonedTimeConsideringPrintshopTimeZone(strStartDate_I.ParseToDate(),
                    strStartTime_I.ParseToTime(), ps.strTimeZone);
                strStartDate_I = ztimeStartPeriodToAdd.Date.ToString();
                strStartTime_I = ztimeStartPeriodToAdd.Time.ToString();

                ZonedTime ztimeEndPeriodToAdd = 
                    ZonedTimeTools.NewZonedTimeConsideringPrintshopTimeZone(strEndDate_I.ParseToDate(),
                    strEndTime_I.ParseToTime(), ps.strTimeZone);
                strEndDate_I = ztimeEndPeriodToAdd.Date.ToString();
                strEndTime_I = ztimeEndPeriodToAdd.Time.ToString();

                intStatus_IO = 402;
                strUserMessage_IO = "Select valid dates.";
                strDevMessage_IO = "Dates are before now.";
                if (
                    //                                      //Date and time are in the future.
                    ztimeStartPeriodToAdd >= ZonedTimeTools.ztimeNow &&
                    ztimeEndPeriodToAdd >= ZonedTimeTools.ztimeNow
                    )
                {
                    intStatus_IO = 403;
                    strUserMessage_IO = "Select valid dates.";
                    strDevMessage_IO = "Dates are not in correct order.";
                    if (
                        //                                  //End of the period is after the start.
                        ztimeStartPeriodToAdd < ztimeEndPeriodToAdd
                        )
                    {
                        //                                  //Establish the connection to the db.
                        Odyssey2Context context = new Odyssey2Context();

                        //                                          //Get the process in workflow.
                        PiwentityProcessInWorkflowEntityDB piwentity = context.ProcessInWorkflow.FirstOrDefault(piw =>
                            piw.intPk == intPkProcessInWorkflow_I);

                        //                                  //Get period.
                        PerentityPeriodEntityDB perentity = context.Period.FirstOrDefault(per =>
                            per.intPk == intPkPeriod_I);

                        intStatus_IO = 404;
                        strUserMessage_IO = "Something is wrong.";
                        strDevMessage_IO = "Period not found.";
                        if (
                            perentity != null
                            )
                        {
                            bool boolIsPeriodDone = perentity.strFinalEndDate != null ? true : false;
                            bool boolIsPeriodStarted = perentity.strFinalStartDate != null ? true : false;

                            intStatus_IO = 405;
                            strUserMessage_IO = "Something is wrong.";
                            strDevMessage_IO = "Period already is done, it can not be edit.";
                            if (
                                //                          //Period is not done.
                                !boolIsPeriodDone &&
                                !boolIsPeriodStarted
                                )
                            {
                                //                          //Get the res.
                                EleentityElementEntityDB eleentity = context.Element.FirstOrDefault(ele =>
                                    ele.intPk == intPkResource_I);

                                //                          //Get the type.
                                EtentityElementTypeEntityDB etentity = context.ElementType.FirstOrDefault(et =>
                                    et.intPk == eleentity.intPkElementType);

                                intStatus_IO = 406;
                                strUserMessage_IO = "Something is wrong.";
                                strDevMessage_IO = "Pk not found, or is not a res or is not calendar.";
                                if (
                                    eleentity != null &&
                                    eleentity.boolnIsCalendar == true &&
                                    etentity.strResOrPro == EtElementTypeAbstract.strResource
                                    )
                                {
                                    JobjsonJobJson jobjson;
                                    JobJob.boolIsValidJobId(intJobId_I, strPrintshopId_I, configuration_I, out jobjson,
                                        ref strUserMessage_IO, ref strDevMessage_IO);

                                    //              //Get job's correct processes.
                                    List<PiwentityProcessInWorkflowEntityDB> darrpiwentityAllProcesses;
                                    List<DynLkjsonDynamicLinkJson> darrdynlkjson;
                                    ProdtypProductType.subGetWorkflowValidWay(piwentity.intPkWorkflow, jobjson,
                                        out darrpiwentityAllProcesses, out darrdynlkjson);

                                    intStatus_IO = 407;
                                    strUserMessage_IO = "Previous processes will not have been completed by" +
                                        " this time. Please set a different period.";
                                    strDevMessage_IO = "";
                                    if (
                                        //                  //Do not overlap with periods from previous processes.
                                        ProProcess.boolIsValidConsideringThePreviousProcesses(intPkProcessInWorkflow_I,
                                            intJobId_I, ztimeStartPeriodToAdd, boolIsTemporary_I, 
                                            darrpiwentityAllProcesses)
                                        )
                                    {
                                        intStatus_IO = 408;
                                        strUserMessage_IO = "There is a same period already set.";
                                        strDevMessage_IO = "";
                                        if (
                                            //              //Do not overlap with any other period of the same type.
                                            ResResource.boolNotOverlapWithOtherPeriod(intPkPeriod_I, intPkResource_I,
                                                ztimeStartPeriodToAdd, ztimeEndPeriodToAdd, intJobId_I, 
                                                boolIsTemporary_I)
                                            )
                                        {
                                            intStatus_IO = 409;
                                            strUserMessage_IO = "Something is wrong.";
                                            strDevMessage_IO = "No employee found.";
                                            if (
                                                (intnContactId_I == null) ||
                                                ResResource.boolEmployeeIsFromPrintshop(strPrintshopId_I,
                                                (int)intnContactId_I)
                                                )
                                            {
                                                //          //Get PkPrintshop.
                                                PsentityPrintshopEntityDB psentity = context.Printshop.FirstOrDefault(
                                                ps => ps.strPrintshopId == strPrintshopId_I);
                                                int intPkPrintshop = psentity.intPk;
                                                //          //Do not overlap with rules.
                                                bool boolDoNotOverlapRule = ResResource.boolNotOverlapWithRule
                                                    (intPkPrintshop, intPkResource_I, ztimeStartPeriodToAdd,
                                                    ztimeEndPeriodToAdd);

                                                bool boolIsValidForEmployee = ResResource.boolEmployeeIsValid(
                                                     intnContactId_I, ztimeStartPeriodToAdd, ztimeEndPeriodToAdd);

                                                intStatus_IO = 410;
                                                strUserMessage_IO = "The times are for an unavailable period of time" +
                                                    " or incorrect password.";
                                                strDevMessage_IO = "";
                                                if (
                                                    (boolDoNotOverlapRule && boolIsValidForEmployee) ||
                                                    ((!boolDoNotOverlapRule || !boolIsValidForEmployee) &&
                                                    psentity.strSpecialPassword == strPassword_I)
                                                    )
                                                {
                                                    String strDeleteDate;
                                                    String strDeleteHour;
                                                    String strDeleteMinute;
                                                    if (
                                                        ProProcess.boolIsMinsBeforeDeleteCorrect(strStartDate_I,
                                                            strStartTime_I, strEndDate_I, strEndTime_I,
                                                            intMinsBeforeDelete_I, out strDeleteDate, out strDeleteHour,
                                                            out strDeleteMinute, ref strUserMessage_IO)
                                                        )
                                                    {
                                                        //      //Transform str to date. 
                                                        Date dateStartDate = strStartDate_I.ParseToDate();
                                                        //      //Get the day of the week of that date. 
                                                        int intDayOfWeek = (int)dateStartDate.DayOfWeek;
                                                        if (
                                                            //  //Verify if it is a sunday. 
                                                            intDayOfWeek == 0
                                                            )
                                                        {
                                                            //  //Return the same date. 
                                                            strLastSunday_O = strStartDate_I;
                                                        }
                                                        else
                                                        {
                                                            //  //Calculate last sunday.
                                                            Date dateLastSunday = dateStartDate - intDayOfWeek;
                                                            strLastSunday_O = dateLastSunday.ToText();
                                                        }

                                                        perentity.strStartDate = strStartDate_I;
                                                        perentity.strStartTime = strStartTime_I;
                                                        perentity.strEndDate = strEndDate_I;
                                                        perentity.strEndTime = strEndTime_I;
                                                        perentity.intnContactId = intnContactId_I;
                                                        perentity.intMinsBeforeDelete = intMinsBeforeDelete_I;
                                                        perentity.strDeleteDate = strDeleteDate;
                                                        perentity.strDeleteHour = strDeleteHour;
                                                        perentity.strDeleteMinute = strDeleteMinute;

                                                        context.Period.Update(perentity);

                                                        String strProcesses;
                                                        List<int> darrintPkPeriodToDelete;
                                                        if (
                                                            !ProProcess.boolIsValidConsideringTheNextProcesses(
                                                                intPkProcessInWorkflow_I, intJobId_I, 
                                                                ztimeEndPeriodToAdd,
                                                                boolIsTemporary_I, darrpiwentityAllProcesses, 
                                                                out strProcesses, out darrintPkPeriodToDelete)
                                                            )
                                                        {
                                                            //  //Delete following periods.
                                                            foreach (int intPkPeriod in darrintPkPeriodToDelete)
                                                            {
                                                                PerentityPeriodEntityDB perentityToDelete = 
                                                                    context.Period.FirstOrDefault(per => 
                                                                    per.intPk == intPkPeriod);

                                                                // //Find alerts about this period.
                                                                List<AlertentityAlertEntityDB> darralertentity =
                                                                    context.Alert.Where(alert =>
                                                                    alert.intnPkPeriod == 
                                                                    perentityToDelete.intPk).ToList();

                                                                foreach (AlertentityAlertEntityDB alertentity in darralertentity)
                                                                {
                                                                    // //Delete alerts about this period.

                                                                    if (
                                                                        // //Notification not read.
                                                                        !PsPrintShop.boolNotificationReadByUser(alertentity,
                                                                            (int)alertentity.intnContactId)
                                                                        )
                                                                    {
                                                                        AlnotAlertNotification.subReduceToOne(
                                                                            (int)alertentity.intnContactId,
                                                                            iHubContext_I);
                                                                    }

                                                                    context.Alert.Remove(alertentity);
                                                                }

                                                                context.Period.Remove(perentityToDelete);
                                                            }
                                                        }

                                                        context.SaveChanges();

                                                        //  //Get the JobJson.
                                                        JobjsonJobJson jobjsonJob;
                                                        JobJob.boolIsValidJobId(intJobId_I, strPrintshopId_I, 
                                                            configuration_I, out jobjsonJob, ref strUserMessage_IO, 
                                                            ref strDevMessage_IO);

                                                        //  //Get the estimate date.
                                                        strEstimatedDate_O = ProProcess.strEstimateDateJob(
                                                            perentity.intPkWorkflow, strPrintshopId_I, jobjsonJob,
                                                            context);

                                                        intStatus_IO = 200;
                                                        strUserMessage_IO = "";
                                                        strDevMessage_IO = "Success.";
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (
                            ztimeStartPeriodToAdd == ztimeEndPeriodToAdd
                            )
                        {
                            intStatus_IO = 409;
                            strUserMessage_IO = "Empty period.";
                            strDevMessage_IO = "";

                            if (
                            ztimeStartPeriodToAdd.DaylightSavingTimeTypeOfDay ==
                            ZonedTimeDstTypeOfDayEnum.START_DAYLIGHT_SAVING_TIME &&
                            ztimeStartPeriodToAdd.Time == "01:00:00".ParseToTime()
                                )
                            {
                                intStatus_IO = 410;
                                strUserMessage_IO = "On daylight saving time.";
                                strDevMessage_IO = "Starts in hour that does not exists becuase of the dayligth saving time.";
                            }
                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static bool boolNotOverlapWithOtherPeriod(
            //                                              //Returns true if period do not overlap with other.

            int? intnPkPeriodToEdit_I,
            int intPkResource_I,
            ZonedTime ztimeStartPeriodToAdd_I,
            ZonedTime ztimeEndPeriodToAdd_I,
            int intJobId_I,
            bool boolIsTemporary_I
            )
        {
            bool boolNotOverlap = true;

            //                                              //Establish the connection with the db.
            Odyssey2Context context = new Odyssey2Context();

            List<PerentityPeriodEntityDB> darrperentity;
            if (
                boolIsTemporary_I
                )
            {
                //                                              //Bring all periods of the res.
                    darrperentity = context.Period.Where(per =>
                    per.intPkElement == intPkResource_I &&
                    per.intJobId != intJobId_I &&
                    per.intnEstimateId == null).ToList();
            }
            else
            {
                //                                              //Bring all periods not completed of the res.
                    darrperentity = context.Period.Where(per =>
                    per.intPkElement == intPkResource_I && per.strFinalEndDate == null &&
                    per.intnEstimateId == null).ToList();
            }

            if (
                 //                                         //There are periods for that res in DB.
                 darrperentity.Count() > 0
                 )
            {
                int intI = 0;
                /*WHILE-DO*/
                while (
                    (boolNotOverlap == true) &&
                    (intI < darrperentity.Count())
                    )
                {
                    //                                      //Parsing ztime from array to correct format.
                    ZonedTime ztimeStartPeriodDBI = ZonedTimeTools.NewZonedTime(darrperentity[intI].strStartDate.ParseToDate(),
                        darrperentity[intI].strStartTime.ParseToTime());
                    ZonedTime ztimeEndPeriodDBI = ZonedTimeTools.NewZonedTime(darrperentity[intI].strEndDate.ParseToDate(),
                        darrperentity[intI].strEndTime.ParseToTime());

                    if (!(
                        //                                  //Do not overlap.
                        (ztimeStartPeriodToAdd_I >= ztimeEndPeriodDBI) ||
                        (ztimeEndPeriodToAdd_I <= ztimeStartPeriodDBI)
                       ))
                    {
                        if (
                            (intnPkPeriodToEdit_I == null)
                            ||
                            ((intnPkPeriodToEdit_I != null) &&
                            (intnPkPeriodToEdit_I != darrperentity[intI].intPk))
                            )
                        {
                            boolNotOverlap = false;
                        }
                    }

                    intI = intI + 1;
                }
            }
            return boolNotOverlap;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static bool boolEmployeeIsValid(
            int? intnContactId_I,
            ZonedTime ztimeStartPeriod_I,
            ZonedTime ztimeEndPeriod_I
            )
        {
            bool boolIsValid = true;
            if (
                intnContactId_I != null
                )
            {
                Odyssey2Context context = new Odyssey2Context();

                List<RuleentityRuleEntityDB> darrruleentity = context.Rule.Where(rule => rule.intnPkPrintshop == null &&
                rule.intnPkResource == null && rule.intnContactId == intnContactId_I).ToList();

                int intI = 0;
                /*UNTIL-DO*/
                while (!(
                    (intI >= darrruleentity.Count) ||
                    (!boolIsValid)
                    ))
                {
                    //                                      //To easy code.
                    RuleentityRuleEntityDB ruleentity = darrruleentity[intI];

                    if (
                        ruleentity.strFrecuency == ResResource.strOnce
                        )
                    {
                        //                                  //start and end date of the rule.
                        Date dateStartRule = (ruleentity.strFrecuencyValue.Substring(0, 10)).ParseToDate();
                        Date dateEndRule = (ruleentity.strFrecuencyValue.Substring(11, 10)).ParseToDate();

                        //                                  //start and end of the rule.
                        ZonedTime ztimeStartRule = ZonedTimeTools.NewZonedTime(dateStartRule,
                            ruleentity.strStartTime.ParseToTime());

                        ZonedTime ztimeEndRule = ZonedTimeTools.NewZonedTime(dateEndRule,
                            ruleentity.strEndTime.ParseToTime());
                        if (
                            ((ztimeStartPeriod_I >= ztimeStartRule) && (ztimeStartPeriod_I < ztimeEndRule)) ||
                            ((ztimeEndPeriod_I > ztimeStartRule) && (ztimeEndPeriod_I <= ztimeEndRule))
                            )
                        {
                            boolIsValid = false;
                        }
                    }
                    else
                    {
                        //                                  //To easy code.
                        ZonedTime ztimeRuleRangeStart = ZonedTimeTools.NewZonedTime(ruleentity.strRangeStartDate.
                            ParseToDate(), ruleentity.strRangeStartTime.ParseToTime());
                        ZonedTime ztimeRuleRangeEnd = ZonedTimeTools.NewZonedTime(ruleentity.strRangeEndDate.ParseToDate(),
                            ruleentity.strRangeEndTime.ParseToTime());

                        if (
                            //                              //The period is between the start and end of the range.
                            ((ztimeStartPeriod_I >= ztimeRuleRangeStart) && (ztimeStartPeriod_I < ztimeRuleRangeEnd)) ||
                            ((ztimeEndPeriod_I > ztimeRuleRangeStart) && (ztimeEndPeriod_I <= ztimeRuleRangeEnd))
                            )
                        {
                            int intJ = 0;
                            Date date = ztimeStartPeriod_I.Date;
                            /*UNTIL-DO*/
                            while (!(
                                ((date + intJ) > ztimeEndPeriod_I.Date) ||
                                !boolIsValid
                                ))
                            {
                                ZonedTime ztimeStartRule = ZonedTimeTools.NewZonedTime(date,
                                    ruleentity.strStartTime.ParseToTime());
                                ZonedTime ztimeEndRule = ZonedTimeTools.NewZonedTime(date,
                                    ruleentity.strEndTime.ParseToTime());

                                if (
                                    //                          //DAILY RULE.
                                    ((ruleentity.strFrecuency == ResResource.strDaily) ||
                                    //                          //WEEKLY.
                                    ((ruleentity.strFrecuency == ResResource.strWeekly) &&
                                    (ruleentity.strFrecuencyValue[(int)date.DayOfWeek] == '1')) ||
                                    //                          //MONTHLY.
                                    ((ruleentity.strFrecuency == ResResource.strMonthly) &&
                                    (ruleentity.strFrecuencyValue[date.Day - 1] == '1')) ||
                                    //                          //ANNUALLY.
                                    ((ruleentity.strFrecuency == ResResource.strAnnually) &&
                                    (ruleentity.strFrecuencyValue == date.ToString("MMdd")))) &&
                                    //                          //The start is over the rule.
                                    (((ztimeStartPeriod_I >= ztimeStartRule) && (ztimeStartPeriod_I < ztimeEndRule)) ||
                                    //                          //The end is over the rule.
                                    ((ztimeEndPeriod_I > ztimeStartRule) && (ztimeEndPeriod_I <= ztimeEndRule)))
                                    )
                                {
                                    boolIsValid = false;
                                }

                                intJ = intJ + 1;
                            }
                        }
                    }
                    intI = intI + 1;
                }
            }

            return boolIsValid;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static bool boolEmployeeIsFromPrintshop(
            String strPrintshopId_I,
            int intContactId_I
            )
        {
            bool boolIsValid = false;
            //                                              //Get data from Wisnet.
            String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                GetSection("Odyssey2Settings")["urlWisnetApi"];
            Task<List<ContactjsonContactJson>> Task_darrcontactjsonFromWisnet = HttpTools<ContactjsonContactJson>.
                    GetListAsyncToEndPoint(strUrlWisnet + "/Contacts/" +
                    strPrintshopId_I);
            Task_darrcontactjsonFromWisnet.Wait();

            if (
                //                                          //There is access to the service of Wisnet.
                Task_darrcontactjsonFromWisnet.Result != null
                )
            {
                //                                          //Final array of products from Wisnet.
                List<ContactjsonContactJson> darrcontactjsonFromWisnet = Task_darrcontactjsonFromWisnet.Result;

                if (
                    darrcontactjsonFromWisnet.Exists(contact => contact.intContactId == intContactId_I &&
                    (contact.intPrintshopEmployee == 1 || contact.intPrintshopAdmin == 1) &&
                    contact.intPrintshopOwner == 0)
                    )
                {
                    boolIsValid = true;
                }
            }

            return boolIsValid;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static bool boolEmployeeOrOwnerIsFromPrintshop(
            String strPrintshopId_I,
            int intContactId_I
            )
        {
            bool boolIsValid = false;
            //                                              //Get data from Wisnet.
            String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                GetSection("Odyssey2Settings")["urlWisnetApi"];
            Task<List<ContactjsonContactJson>> Task_darrcontactjsonFromWisnet = HttpTools<ContactjsonContactJson>.
                    GetListAsyncToEndPoint(strUrlWisnet + "/Contacts/" + strPrintshopId_I);
            Task_darrcontactjsonFromWisnet.Wait();

            if (
                //                                          //There is access to the service of Wisnet.
                Task_darrcontactjsonFromWisnet.Result != null
                )
            {
                //                                          //Final array of products from Wisnet.
                List<ContactjsonContactJson> darrcontactjsonFromWisnet = Task_darrcontactjsonFromWisnet.Result;

                if (
                    darrcontactjsonFromWisnet.Exists(contact => contact.intContactId == intContactId_I &&
                    //                                      //In the Wisnet database can be a contact with zero in these
                    //                                      //      three values, this mean is deleted in wisnet 
                    (contact.intPrintshopEmployee == 1 || contact.intPrintshopAdmin == 1 ||
                    contact.intPrintshopOwner == 1))
                    )
                {
                    boolIsValid = true;
                }
            }

            return boolIsValid;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static bool boolDoNotOverlapWithEmployeePeriods(
            int? intnContactId_I,
            ZonedTime ztimeStartPeriod_I,
            ZonedTime ztimeEndPeriod_I,
            int? intnPkPeriod_I,
            int intJobId_I,
            bool boolIsTemporary_I,
            out String strJobs_O
            )
        {
            bool boolIsValid = true;
            strJobs_O = null;
            if (
                intnContactId_I != null
                )
            {
                Odyssey2Context context = new Odyssey2Context();

                List<PerentityPeriodEntityDB> darrperentity;
                if (
                    boolIsTemporary_I
                    )
                {
                    darrperentity = context.Period.Where(per =>
                        per.intnContactId == intnContactId_I &&
                        per.intPk != intnPkPeriod_I &&
                        per.intJobId != intJobId_I &&
                        per.intnEstimateId == null).ToList();
                }
                else
                {
                    darrperentity = context.Period.Where(per =>
                        per.intnContactId == intnContactId_I &&
                        per.intPk != intnPkPeriod_I && per.strFinalEndDate == null &&
                        per.intnEstimateId == null).ToList();
                }

                for (int intI = 0; intI < darrperentity.Count; intI = intI + 1)
                {
                    PerentityPeriodEntityDB perentity = darrperentity[intI];

                    ZonedTime ztimeStart = ZonedTimeTools.NewZonedTime(perentity.strStartDate.ParseToDate(),
                        perentity.strStartTime.ParseToTime());
                    ZonedTime ztimeEnd = ZonedTimeTools.NewZonedTime(perentity.strEndDate.ParseToDate(),
                        perentity.strEndTime.ParseToTime());

                    if (
                        //                                  //Period starts over the start.
                        ((ztimeStartPeriod_I >= ztimeStart) && (ztimeStartPeriod_I < ztimeEnd)) ||
                        //                                  //Period ends over the end.
                        ((ztimeEndPeriod_I > ztimeStart) && (ztimeEndPeriod_I <= ztimeEnd)) ||
                        //                                  //Period is over the other period.
                        ((ztimeStartPeriod_I < ztimeStart) && (ztimeEndPeriod_I > ztimeEnd))
                        )
                    {
                        strJobs_O = strJobs_O + perentity.intJobId + ", ";
                    }
                }

                if (
                    strJobs_O != null
                    )
                {
                    boolIsValid = false;
                    strJobs_O = strJobs_O.Substring(0, strJobs_O.LastIndexOf(','));

                    if (
                        strJobs_O.LastIndexOf(',') > 0
                        )
                    {
                        String str1 = strJobs_O.Substring(0, strJobs_O.LastIndexOf(','));
                        String str2 = strJobs_O.Substring(strJobs_O.LastIndexOf(',') + 1);

                        strJobs_O = str1 + " and " + str2 + ".";
                    }
                }
            }

            return boolIsValid;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static bool boolNotOverlapWithRule(
            //                                              //Returns true if period do not overlap with rule.

            int intPkPrintshop_I,
            int intPkResource_I,
            ZonedTime ztimeStartPeriodToAdd_I,
            ZonedTime ztimeEndPeriodToAdd_I
            )
        {
            //                                              //Establish the connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get the rules.
            List<RuleentityRuleEntityDB> darrruleentity = context.Rule.Where(rule =>
                rule.intnPkResource == intPkResource_I || rule.intnPkPrintshop == intPkPrintshop_I).ToList();

            bool boolNotOverlap = true;

            if (
                darrruleentity.Count() > 0
                )
            {
                int intI = 0;
                /*WHILE-DO*/
                while (
                    (boolNotOverlap == true) &&
                    (intI < darrruleentity.Count())
                    )
                {
                    //                                      //To easy code.
                    RuleentityRuleEntityDB ruleentity = darrruleentity[intI];

                    //                                      //Get ztime values from db in order to compare with
                    //                                      //  new period.

                    ZonedTime? ztimenRangeStartPeriodDB = null;
                    if (
                        ruleentity.strRangeStartDate != null
                        )
                    {
                        //                                  //There is a RangeStarDate and time in DB.                       
                        ztimenRangeStartPeriodDB = ZonedTimeTools.NewZonedTime(ruleentity.strRangeStartDate.ParseToDate(),
                        ruleentity.strRangeStartTime.ParseToTime());
                    }

                    ZonedTime? ztimenRangeEndPeriodDB = null;
                    if (
                        ruleentity.strRangeEndDate != null
                        )
                    {
                        //                                  //There is a RangeEndDate and time in DB.
                        ztimenRangeEndPeriodDB = ZonedTimeTools.NewZonedTime(ruleentity.strRangeEndDate.ParseToDate(),
                        ruleentity.strRangeEndTime.ParseToTime());
                    }

                    if (
                        //                                  //We will check the rule only if the new period to add:
                        //                                  //  1.-Is a once rule.
                        //                                  //  2.-The new period to add is between a RangeStart 
                        //                                  //  and RangeEnd of a rule if the rule has an end.
                        //                                  //  3.-The new period to add is after a RangeStar of
                        //                                  //  a rule and the rule does not has an end.

                        //                                  //1.
                        (ruleentity.strRangeStartDate == null && ruleentity.strRangeEndDate == null)
                        ||
                        //                                  //2.
                        ((ruleentity.strRangeStartDate != null && ruleentity.strRangeEndDate != null) &&
                        (ztimeStartPeriodToAdd_I >= ztimenRangeStartPeriodDB &&
                        ztimeEndPeriodToAdd_I <= ztimenRangeEndPeriodDB))
                        ||
                        //                                  //3.
                        ((ruleentity.strRangeStartDate != null && ruleentity.strRangeEndDate == null) &&
                        (ztimeStartPeriodToAdd_I >= ztimenRangeStartPeriodDB))
                        )
                    {
                        if (
                            ruleentity.strFrecuency == ResResource.strOnce
                            )
                        {
                            //                                  //Start and End date of the rule the day the PeriodToAdd Start.
                            Date dateStartRule = (ruleentity.strFrecuencyValue.Substring(0, 10)).ParseToDate();
                            Date dateEndRule = (ruleentity.strFrecuencyValue.Substring(11, 10)).ParseToDate();

                            //                                  //Start and End of the rule the day the PeriodToAdd Start.
                            ZonedTime ztimeStartRule = ZonedTimeTools.NewZonedTime(dateStartRule,
                                ruleentity.strStartTime.ParseToTime());

                            ZonedTime ztimeEndRule = ZonedTimeTools.NewZonedTime(dateEndRule, ruleentity.strEndTime.ParseToTime());

                            if (!(
                                //                              //Do not overlap.
                                (ztimeStartPeriodToAdd_I >= ztimeEndRule) ||
                                (ztimeEndPeriodToAdd_I <= ztimeStartRule)
                               ))
                            {
                                boolNotOverlap = false;
                            }
                        }
                        else
                        {
                            //                                  //Start and End of the rule the day the PeriodToAdd Start.
                            ZonedTime ztime_DayStartPeriodToAdd_TimeStartRule;
                            ZonedTime ztime_DayStartPeriodToAdd_TimeEndRule;

                            //                                  //Next Start of the rule.
                            ZonedTime ztime_NextDayStartRule_TimeStartRule;

                            ResResource.subGetStartAndEndOfRuleDayPeriodStartAndNextDay(ztimeStartPeriodToAdd_I, ruleentity,
                                out ztime_DayStartPeriodToAdd_TimeStartRule, out ztime_DayStartPeriodToAdd_TimeEndRule,
                                out ztime_NextDayStartRule_TimeStartRule);

                            if (!(
                                //                              //Do not overlap.

                                //                              //PeriodToAdd Start before rule start.
                                (ztimeStartPeriodToAdd_I < ztime_DayStartPeriodToAdd_TimeStartRule &&
                                //                              //PeriodToAdd End before or when rule start.
                                ztimeEndPeriodToAdd_I <= ztime_DayStartPeriodToAdd_TimeStartRule)
                                ||
                                //                              //PeriodToAdd Start when or after rule end.
                                (ztimeStartPeriodToAdd_I >= ztime_DayStartPeriodToAdd_TimeEndRule &&
                                //                              //PeriodToAdd End before or when next start of the rule.
                                ztimeEndPeriodToAdd_I <= ztime_NextDayStartRule_TimeStartRule)
                                ))
                            {
                                boolNotOverlap = false;
                            }
                        }
                    }
                    intI = intI + 1;
                }
            }

            return boolNotOverlap;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subGetStartAndEndOfRuleDayPeriodStartAndNextDay(

            ZonedTime ztimeStartPeriodToAdd_I,
            RuleentityRuleEntityDB ruleentity_I,
            out ZonedTime ztimeDateStartPeriodToAddTimeStartRule_O,
            out ZonedTime ztimeDateStartPeriodToAddTimeEndRule_O,
            out ZonedTime ztimeDateNextStartRuleTimeStartRule_O
            )
        {
            //                                              //Start day of period to add.
            int intStartDayOfPeriod;
            //                                              //Start month of period to add.
            //                                              //Only useful for annualy.
            int intStartMonthOfPeriod = 0;
            /*CASE*/
            if (
                //                                          //Daily or weekly.
                ruleentity_I.strFrecuency == ResResource.strDaily ||
                ruleentity_I.strFrecuency == ResResource.strWeekly
                )
            {
                intStartDayOfPeriod = (int)ztimeStartPeriodToAdd_I.Date.DayOfWeek;
            }
            else if (
                //                                          //Monthly.
                ruleentity_I.strFrecuency == ResResource.strMonthly
                )
            {
                intStartDayOfPeriod = (int)ztimeStartPeriodToAdd_I.Date.Day;
            }
            else
            {
                //                                          //Annualy.
                intStartDayOfPeriod = (int)ztimeStartPeriodToAdd_I.Date.Day;
                intStartMonthOfPeriod = (int)ztimeStartPeriodToAdd_I.Date.Month;
            }
            /*END-CASE*/

            //                                              //Days in the rule.
            List<int> darrintDaysInRule;
            String strMonthAndDayInRule;
            bool boolRuleInStartDay;

            ResResource.subGetDaysInRule(ruleentity_I, intStartDayOfPeriod, intStartMonthOfPeriod,
                out darrintDaysInRule, out strMonthAndDayInRule, out boolRuleInStartDay);

            if (
                boolRuleInStartDay
                )
            {
                //                                          //Start and end of the rule the day the PeriodToAddStart.
                ztimeDateStartPeriodToAddTimeStartRule_O = ZonedTimeTools.NewZonedTime(ztimeStartPeriodToAdd_I.Date,
                    ruleentity_I.strStartTime.ParseToTime());
                ztimeDateStartPeriodToAddTimeEndRule_O = ZonedTimeTools.NewZonedTime(ztimeStartPeriodToAdd_I.Date,
                    ruleentity_I.strEndTime.ParseToTime());
            }
            else
            {
                //                                          //No rule the day the PeriodToAddStart.
                //                                          //The period will start always after the rule ends.
                ztimeDateStartPeriodToAddTimeStartRule_O = ZonedTimeTools.NewZonedTime(ztimeStartPeriodToAdd_I.Date,
                    Time.MinValue);
                ztimeDateStartPeriodToAddTimeEndRule_O = ZonedTimeTools.NewZonedTime(ztimeStartPeriodToAdd_I.Date,
                    Time.MinValue);
            }

            ztimeDateNextStartRuleTimeStartRule_O = ResResource.ztimeNextStartOfRule(ruleentity_I,
                ztimeStartPeriodToAdd_I, ztimeDateStartPeriodToAddTimeStartRule_O, intStartDayOfPeriod,
                darrintDaysInRule, strMonthAndDayInRule, boolRuleInStartDay);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subGetDaysInRule(
            //                                              //Return days in the rule.

            RuleentityRuleEntityDB ruleentity_I,
            int intStartDayOfPeriod_I,
            int intStartMonthOfPeriod_I,
            //                                              //  If daily and weekly from 0 to 6.
            //                                              //  If monthtly from 1 to 31.

            out List<int> darrintDaysInRule_O,
            //                                              //  If annualy mmdd.
            out String strDayAndMonthInRule_O,
            //                                              //True means period start in day with rule.
            out bool boolRuleInStartDay_O
            )
        {
            darrintDaysInRule_O = new List<int>();
            strDayAndMonthInRule_O = "";
            boolRuleInStartDay_O = false;

            /*CASE*/
            if (
                ruleentity_I.strFrecuency == ResResource.strDaily
                )
            {
                for (int intDay = 0; intDay < 7; intDay = intDay + 1)
                {
                    darrintDaysInRule_O.Add(intDay);
                }
                boolRuleInStartDay_O = true;
            }
            else if (
                ruleentity_I.strFrecuency == ResResource.strWeekly
                )
            {
                for (int intDay = 0; intDay < ruleentity_I.strFrecuencyValue.Length; intDay = intDay + 1)
                {
                    if (
                        ruleentity_I.strFrecuencyValue[intDay] == '1'
                        )
                    {
                        darrintDaysInRule_O.Add(intDay);
                    }
                }
                boolRuleInStartDay_O = intStartDayOfPeriod_I.IsInSet(darrintDaysInRule_O.ToArray());
            }
            else if (
                ruleentity_I.strFrecuency == ResResource.strMonthly
                )
            {
                for (int intDay = 0; intDay < ruleentity_I.strFrecuencyValue.Length; intDay = intDay + 1)
                {
                    if (
                        ruleentity_I.strFrecuencyValue[intDay] == '1'
                        )
                    {
                        //                                  //Monthtly, base 1.
                        darrintDaysInRule_O.Add(intDay + 1);
                    }
                }
                boolRuleInStartDay_O = intStartDayOfPeriod_I.IsInSet(darrintDaysInRule_O.ToArray());
            }
            else
            {
                //                                          //Annualy.

                int intDayRule = ruleentity_I.strFrecuencyValue.Substring(0, 2).ParseToInt();
                int intMonthRule = ruleentity_I.strFrecuencyValue.Substring(2, 2).ParseToInt();

                strDayAndMonthInRule_O = ruleentity_I.strFrecuencyValue;

                if (
                    intStartDayOfPeriod_I == intDayRule &&
                    intStartMonthOfPeriod_I == intMonthRule
                    )
                {
                    boolRuleInStartDay_O = true;
                }
            }
            /*END-CASE*/
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static ZonedTime ztimeNextStartOfRule(
            RuleentityRuleEntityDB ruleentity_I,
            ZonedTime ztimeStartPeriodToAdd_I,
            ZonedTime ztimeDateStartPeriodToAddTimeStartRule_I,
            int intStartDayOfPeriod_I,
            List<int> darrintDaysInRule_I,
            String strMonthAndDayInRule_I,
            bool boolRuleInStartDay_I
            )
        {
            ZonedTime ztimeNextStartRule;

            if (
                //                                          //Annualy.
                ruleentity_I.strFrecuency == ResResource.strAnnually
                )
            {
                Date dateStartRuleThisYear = (ztimeStartPeriodToAdd_I.Date.Year.ToString() +
                    strMonthAndDayInRule_I).ParseToDate();
                Time timeStartRule = ruleentity_I.strStartTime.ParseToTime();

                ZonedTime ztimeStartRuleThisYear = ZonedTimeTools.NewZonedTime(dateStartRuleThisYear, timeStartRule);

                if (
                    ztimeStartRuleThisYear > ztimeStartPeriodToAdd_I
                    )
                {
                    ztimeNextStartRule = ztimeStartRuleThisYear;
                }
                else
                {
                    ztimeNextStartRule = ZonedTimeTools.NewZonedTime(ztimeStartRuleThisYear.Date.AddYears(1),
                        timeStartRule);
                }
            }
            else
            {
                //                                          //Daily or weekly or monthly.

                int intNextDayInTheRule;
                if (
                    boolRuleInStartDay_I &&
                    ztimeStartPeriodToAdd_I < ztimeDateStartPeriodToAddTimeStartRule_I
                    )
                {
                    intNextDayInTheRule = intStartDayOfPeriod_I;
                }
                else
                {
                    int intCurrentDayInTheRule;
                    if (
                        boolRuleInStartDay_I
                        )
                    {
                        intCurrentDayInTheRule = intStartDayOfPeriod_I;
                    }
                    else
                    {
                        int intJ = 0;
                        bool boolCurrentDayFound = false;
                        intCurrentDayInTheRule = 0;
                        while (intJ < darrintDaysInRule_I.Count &&
                            boolCurrentDayFound == false
                            )
                        {
                            if (
                                darrintDaysInRule_I[intJ] < intStartDayOfPeriod_I
                                )
                            {
                                intCurrentDayInTheRule = darrintDaysInRule_I[intJ];
                            }
                            else
                            {
                                boolCurrentDayFound = true;
                            }
                            intJ = intJ + 1;
                        }
                    }

                    if (
                        //                                  //Not last day in the rule.
                        darrintDaysInRule_I.IndexOf(intCurrentDayInTheRule) < darrintDaysInRule_I.Count() - 1
                        )
                    {
                        intNextDayInTheRule = darrintDaysInRule_I[darrintDaysInRule_I.IndexOf(intCurrentDayInTheRule) + 1];
                    }
                    else
                    {
                        //                                  //Last day of the week, or last day of the month.

                        intNextDayInTheRule = darrintDaysInRule_I[0];
                        if (
                            //                              //Daily or weekly.
                            ruleentity_I.strFrecuency == ResResource.strDaily ||
                            ruleentity_I.strFrecuency == ResResource.strWeekly
                            )
                        {
                            intNextDayInTheRule = intNextDayInTheRule + 7;
                        }
                        else
                        {
                            //                              //Monthly.
                            if (
                                ztimeStartPeriodToAdd_I.Date.Month == 2
                                )
                            {
                                intNextDayInTheRule = intNextDayInTheRule + 28;
                                if (
                                    //                      //Leap.
                                    ztimeStartPeriodToAdd_I.Date.IsLeapYear
                                    )
                                {
                                    intNextDayInTheRule = intNextDayInTheRule + 1;
                                }
                            }
                            else if (
                                ztimeStartPeriodToAdd_I.Date.Month == 4 ||
                                ztimeStartPeriodToAdd_I.Date.Month == 6 ||
                                ztimeStartPeriodToAdd_I.Date.Month == 9 ||
                                ztimeStartPeriodToAdd_I.Date.Month == 11
                                )
                            {
                                intNextDayInTheRule = intNextDayInTheRule + 30;
                            }
                            else
                            {
                                intNextDayInTheRule = intNextDayInTheRule + 31;
                            }
                        }
                    }
                }

                //                                      //Days to add to reach the next start in the rule.
                int intDaysToAddToGetNextDay = intNextDayInTheRule - intStartDayOfPeriod_I;

                //                                      //Next start in the rule.
                Date dateStartNextRule = ztimeStartPeriodToAdd_I.Date + intDaysToAddToGetNextDay;
                Time timeStartNextRule = ruleentity_I.strStartTime.ParseToTime();
                ztimeNextStartRule = ZonedTimeTools.NewZonedTime(dateStartNextRule, timeStartNextRule);
            }

            return ztimeNextStartRule;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subDeletePeriod(
            //                                              //Delete a period from Period table.
            int intPkPeriod_I,
            String strPrintshopId_I,
            IConfiguration configuration_I,
            IHubContext<ConnectionHub> iHubContext_I,
            out String strLastSunday_O,
            out String strEstimatedDate_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            strLastSunday_O = null;
            strEstimatedDate_O = "";

            //                                              //Establish the connection with the db.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get period to delete.
            PerentityPeriodEntityDB perentity = context.Period.FirstOrDefault(per => per.intPk == intPkPeriod_I);

            //                                              //Only resource periods have eleet or eleele.
            int? intnPkEleetOrEleele = perentity.intnPkElementElementType != null ?
                perentity.intnPkElementElementType : perentity.intnPkElementElement;

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Period not found.";
            if (
                //                                          //Period exists.
                perentity != null &&
                intnPkEleetOrEleele > 0
                )
            {
                //                                          //Transform str to date. 
                Date dateStartDate = perentity.strStartDate.ParseToDate();
                //                                          //Get the day of the week of that date. 
                int intDayOfWeek = (int)dateStartDate.DayOfWeek;
                if (
                    //                                      //Verify if it is a sunday. 
                    intDayOfWeek == 0
                    )
                {
                    //                                      //Return the same date. 
                    strLastSunday_O = perentity.strStartDate;
                }
                else
                {
                    //                                      //Calculate last sunday.
                    Date dateLastSunday = dateStartDate - intDayOfWeek;
                    strLastSunday_O = dateLastSunday.ToText();
                }

                bool boolIsPeriodDone = perentity.strFinalEndDate != null ? true : false;
                bool boolIsPeriodStarted = perentity.strFinalStartDate != null ? true : false;

                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Period already is done or started, it can not be delete.";
                if (
                    //                                      //Period is not done.
                    !boolIsPeriodDone &&
                    //                                      //Period is not started.
                    !boolIsPeriodStarted
                    )
                {
                    //                                      //Find alerts about this period.
                    List<AlertentityAlertEntityDB> darralertentity = context.Alert.Where(alert =>
                        alert.intnPkPeriod == perentity.intPk).ToList();

                    foreach (AlertentityAlertEntityDB alertentity in darralertentity)
                    {
                        //                                  //Delete alerts about this period.

                        if (
                            //                              //Notification not read.
                            !PsPrintShop.boolNotificationReadByUser(alertentity, (int)alertentity.intnContactId)
                            )
                        {
                            AlnotAlertNotification.subReduceToOne((int)alertentity.intnContactId,
                                iHubContext_I);
                        }
                        
                        context.Alert.Remove(alertentity);
                    }

                    //                                      //Remove period from db.
                    context.Period.Remove(perentity);
                    context.SaveChanges();

                    //                                      //Get the JobJson.
                    JobjsonJobJson jobjsonJob;
                    JobJob.boolIsValidJobId(perentity.intJobId, strPrintshopId_I,
                        configuration_I, out jobjsonJob, ref strUserMessage_IO,
                        ref strDevMessage_IO);

                    //                                      //Get the estimate date.
                    strEstimatedDate_O = ProProcess.strEstimateDateJob(perentity.intPkWorkflow, strPrintshopId_I, 
                        jobjsonJob, context);

                    intStatus_IO = 200;
                    strUserMessage_IO = "Success.";
                    strDevMessage_IO = "";
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subAddGenericResource(

            int intPkNewResourceType_I,
            String strResourceName_I,
            EleentityElementEntityDB eleentityBaseResource_I,
            Odyssey2Context context_M,
            out EleentityElementEntityDB eleentityNewResource_O,
            ref Dictionary<int, int> dicResource_M
            )
        {
            int? intnPkElementInheritedNew = null;
            int? intnPkElementCalendarInheritedNew = null;
            if (
                eleentityBaseResource_I.intnPkElementInherited != null
                )
            {
                //                                      //Find base template
                EleentityElementEntityDB eleentityBaseTemplate = context_M.Element.FirstOrDefault(ele =>
                    ele.intPk == eleentityBaseResource_I.intnPkElementInherited);

                if (
                    //                                      //The template is not yet in the dictionary
                    !dicResource_M.ContainsKey(eleentityBaseTemplate.intPk)
                    )
                {
                    String strTemplateName = eleentityBaseTemplate.strElementName + " (Generic)";
                    //                                      //Find new template
                    EleentityElementEntityDB eleentityNewTemplate = context_M.Element.FirstOrDefault(ele =>
                        ele.strElementName == strTemplateName &&
                        ele.intPkElementType == intPkNewResourceType_I);

                    if (
                        //                                  //The template already exists
                        eleentityNewTemplate != null
                        )
                    {
                        //                                  //Add relationship between the base and the new resource 
                        //                                  //      to the dictionary
                        dicResource_M.Add(eleentityBaseTemplate.intPk, eleentityNewTemplate.intPk);
                    }
                    else
                    {
                        ResResource.subAddGenericResource(intPkNewResourceType_I, strTemplateName,
                            eleentityBaseTemplate, context_M, out eleentityNewTemplate, ref dicResource_M);
                    }

                    intnPkElementInheritedNew = eleentityNewTemplate.intPk;
                }
                else
                {
                    //                                      //Get new template from dicResource
                    intnPkElementInheritedNew = dicResource_M.FirstOrDefault(resource =>
                        resource.Key == eleentityBaseResource_I.intnPkElementInherited).Value;
                }

                if (
                    eleentityBaseResource_I.intnPkElementCalendarInherited != null
                    )
                {
                    intnPkElementCalendarInheritedNew = intnPkElementInheritedNew;
                }
            }

            //                                              //Add the resource.
            EleentityElementEntityDB eleentityNewResource = new EleentityElementEntityDB
            {
                strElementName = strResourceName_I,
                intPkElementType = intPkNewResourceType_I,
                intnPkElementInherited = intnPkElementInheritedNew,
                intnPkElementCalendarInherited = intnPkElementCalendarInheritedNew,
                boolIsTemplate = eleentityBaseResource_I.boolIsTemplate,
                boolnIsCalendar = eleentityBaseResource_I.boolnIsCalendar,
                boolnIsAvailable = eleentityBaseResource_I.boolnIsAvailable,
                boolDeleted = false,
                strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                strStartTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                boolnCalendarIsChangeable = eleentityBaseResource_I.boolnCalendarIsChangeable
            };
            context_M.Element.Add(eleentityNewResource);
            context_M.SaveChanges();

            //                                              //Add relationship between the base and the new resource 
            //                                              //      to the dictionary
            dicResource_M.Add(eleentityBaseResource_I.intPk, eleentityNewResource.intPk);

            eleentityNewResource_O = eleentityNewResource;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public static ResResource resFromDB(
            int? intnPk_I,
            bool boolIsTemplate_I
            )
        {
            ResResource res = null;
            if (
                //                                          //It is a invalid primary key.
                intnPk_I > 0
                )
            {
                //                                          //Create the connection.
                Odyssey2Context context = new Odyssey2Context();

                EleentityElementEntityDB eleentity = context.Element.FirstOrDefault(eleentity =>
                    eleentity.intPk == intnPk_I && eleentity.boolIsTemplate == boolIsTemplate_I);

                if (
                    eleentity != null
                    )
                {
                    EtElementTypeAbstract et = EtElementTypeAbstract.etFromDB(eleentity.intPkElementType);
                    if (
                        et.strResOrPro == EtElementTypeAbstract.strResource
                        )
                    {
                        RestypResourceType restyp = (RestypResourceType)et;

                        ResResource resInherited = ResResource.resFromDB(eleentity.intnPkElementInherited, true);

                        res = new ResResource(eleentity.intPk, eleentity.strElementName, eleentity.boolIsTemplate,
                            restyp, resInherited, eleentity.boolnIsCalendar, eleentity.boolnIsAvailable,
                            eleentity.boolnCalendarIsChangeable, eleentity.intnPkElementCalendarInherited != null);
                    }
                }
            }

            return res;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static ResResource resFromDB(
            Odyssey2Context context_I,
            int? intnPk_I,
            bool boolIsTemplate_I
            )
        {
            ResResource res = null;
            if (
                //                                          //It is a invalid primary key.
                intnPk_I > 0
                )
            {
                EleentityElementEntityDB eleentity = context_I.Element.FirstOrDefault(eleentity =>
                    eleentity.intPk == intnPk_I && eleentity.boolIsTemplate == boolIsTemplate_I);

                if (
                    eleentity != null
                    )
                {
                    EtElementTypeAbstract et = EtElementTypeAbstract.etFromDB(context_I, eleentity.intPkElementType);

                    if (
                        et.strResOrPro == EtElementTypeAbstract.strResource
                        )
                    {
                        RestypResourceType restyp = (RestypResourceType)et;

                        ResResource resInherited = ResResource.resFromDB(context_I, eleentity.intnPkElementInherited, true);

                        res = new ResResource(eleentity.intPk, eleentity.strElementName, eleentity.boolIsTemplate,
                            restyp, resInherited, eleentity.boolnIsCalendar, eleentity.boolnIsAvailable,
                            eleentity.boolnCalendarIsChangeable, eleentity.intnPkElementCalendarInherited != null);
                    }
                }
            }

            return res;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetPrintshopResources(
            PsPrintShop ps_I,
            int? intnPkType_I,
            out List<ResjsonResourceJson> darrresjson_O,
            //                                              //Status:
            //                                              //      200 - Success.
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            darrresjson_O = new List<ResjsonResourceJson>();

            Odyssey2Context context = new Odyssey2Context();
            String strResourceName;
            //                                              //Get the dictionary of resource.
            Dictionary<int, ResResource> dicres = ps_I.dicres;
            foreach (KeyValuePair<int, ResResource> res in dicres)
            {
                if (
                    intnPkType_I != null
                    )
                {
                    //                                      //Filter for pk of the type.
                    if (
                        res.Value.restypBelongsTo.intPk == intnPkType_I &&
                        res.Value.boolIsTemplate == false
                        )
                    {
                        EtentityElementTypeEntityDB etentity = context.ElementType.FirstOrDefault(et =>
                            et.intPk == res.Value.restypBelongsTo.intPk);

                        if (
                            etentity.strXJDFTypeId == "Media"
                            )
                        {
                            strResourceName = ResResource.strGetMediaResourceName(res.Value.intPk);
                        }
                        else
                        {
                            //                                              //Set resource's name.
                            strResourceName = res.Value.strName;
                        }

                        //                                  //Get the current unit of measurement.
                        ValentityValueEntityDB valentity = ResResource.GetResourceUnitOfMeasurement(res.Value.intPk);

                        String strUnit = null;
                        if (
                            valentity != null
                            )
                        {
                            strUnit = valentity.strValue;
                        }

                        double? numnQuantity = null;
                        double? numnCost = null;
                        double? numnMin = null;
                        double? numnBlock = null;
                        //                                  //Get the cost.
                        ResResource resGetCost = ResResource.resFromDB(res.Value.intPk, false);
                        CostentityCostEntityDB costentity = resGetCost.costentityCurrent;
                        if (
                            costentity != null
                            )
                        {
                            //                              //Set the cost data.
                            numnQuantity = costentity.numnQuantity;
                            numnCost = costentity.numnCost;
                            numnMin = costentity.numnMin;
                            numnBlock = costentity.numnBlock;
                        }
                        ResjsonResourceJson resjson = new ResjsonResourceJson(res.Value.intPk, strResourceName,
                            strUnit, numnQuantity, numnCost, numnMin, numnBlock, res.Value.boolnIsCalendar,
                            res.Value.boolnIsAvailable);
                        darrresjson_O.Add(resjson);
                    }
                }
                else
                {
                    EtentityElementTypeEntityDB etentity = context.ElementType.FirstOrDefault(et =>
                        et.intPk == res.Value.restypBelongsTo.intPk);
                    if (
                        (res.Value.boolIsTemplate == false) &&
                        (RestypResourceType.boolIsPhysical(etentity.strClassification))
                        )
                    {
                        if (
                            etentity.strXJDFTypeId == "Media"
                            )
                        {
                            strResourceName = ResResource.strGetMediaResourceName(res.Value.intPk);
                        }
                        else
                        {
                            //                                              //Set resource's name.
                            strResourceName = res.Value.strName;
                        }

                        //                                  //Get the current unit of measurement.
                        ValentityValueEntityDB valentity = ResResource.GetResourceUnitOfMeasurement(res.Value.intPk);

                        String strUnit = null;
                        if (
                            valentity != null
                            )
                        {
                            strUnit = valentity.strValue;
                        }

                        double? numnQuantity = null;
                        double? numnCost = null;
                        double? numnMin = null;
                        double? numnBlock = null;
                        //                                  //Get the cost.
                        ResResource resGetCost = ResResource.resFromDB(res.Value.intPk, false);
                        CostentityCostEntityDB costentity = resGetCost.costentityCurrent;
                        if (
                            costentity != null
                            )
                        {
                            //                          //Set the cost data.
                            numnQuantity = costentity.numnQuantity;
                            numnCost = costentity.numnCost;
                            numnMin = costentity.numnMin;
                            numnBlock = costentity.numnBlock;
                        }
                        ResjsonResourceJson resjson = new ResjsonResourceJson(res.Value.intPk, strResourceName,
                            strUnit, numnQuantity, numnCost, numnMin, numnBlock, res.Value.boolnIsCalendar,
                            res.Value.boolnIsAvailable);

                        darrresjson_O.Add(resjson);
                    }
                }
            }

            intStatus_IO = 200;
            strUserMessage_IO = "Success";
            strDevMessage_IO = "";

        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetInheritanceData(
            int intPkTemplateOrResource_I,
            out InhedatajsonInheritanceDataJson inhedatajson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            inhedatajson_O = null;
            //                                              //Establish the connection with db.
            Odyssey2Context context = new Odyssey2Context();

            EleentityElementEntityDB eleentityTemplateOrResource = context.Element.FirstOrDefault(ele =>
                ele.intPk == intPkTemplateOrResource_I);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Template or Resource not found.";
            if (
                eleentityTemplateOrResource != null
                )
            {
                ResResource resTemplateOrResource;
                if (
                    eleentityTemplateOrResource.boolIsTemplate == true
                    )
                {
                    resTemplateOrResource = ResResource.resFromDB(intPkTemplateOrResource_I, true);
                }
                else
                {
                    resTemplateOrResource = ResResource.resFromDB(intPkTemplateOrResource_I, false);
                }

                ResResource.subGetTemplateOrResourceInheritanceData(resTemplateOrResource, out inhedatajson_O,
                    ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);

                if (
                    inhedatajson_O != null
                    )
                {
                    inhedatajson_O.boolIsDeviceToolOrCustom =
                        RestypResourceType.boolIsDeviceToolOrCustom(resTemplateOrResource.restypBelongsTo);
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetTemplateOrResourceInheritanceData(
            ResResource resTemplateOrResource_I,
            out InhedatajsonInheritanceDataJson inhedatajson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Establish connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get resource's availability.
            AvainhejsonAvailabilityInheritanceJson avainhejson = new AvainhejsonAvailabilityInheritanceJson(
               resTemplateOrResource_I.boolnIsCalendar, resTemplateOrResource_I.boolnCalendarIsInherited,
               resTemplateOrResource_I.boolnCalendarIsChangeable);

            //                                              //Get resource's unit of measurement.
            ValentityValueEntityDB valentity = ResResource.GetResourceUnitOfMeasurement(resTemplateOrResource_I.intPk);
            UnitinhejsonUnitInheritanceJson unitinhejson = null;
            if (
                valentity != null
                )
            {
                unitinhejson = new UnitinhejsonUnitInheritanceJson(valentity.strValue, 
                    valentity.intnPkValueInherited != null, valentity.boolnIsChangeable, valentity.boolnIsDecimal);
            }

            //                                              //Get resource's cost.
            CostinhejsonCostInheritanceJson costinhejson;
            if (
                resTemplateOrResource_I.costentityCurrent != null
                )
            {
                //                                          //Find account name.
                String strAccountName = context.Account.FirstOrDefault(acc =>
                        acc.intPk == resTemplateOrResource_I.costentityCurrent.intPkAccount).strName;
                
                costinhejson = new CostinhejsonCostInheritanceJson(resTemplateOrResource_I.costentityCurrent.numnCost,
                    resTemplateOrResource_I.costentityCurrent.numnQuantity,
                    resTemplateOrResource_I.costentityCurrent.numnMin,
                    resTemplateOrResource_I.costentityCurrent.numnBlock,
                    resTemplateOrResource_I.costentityCurrent.intnPkCostInherited != null,
                    resTemplateOrResource_I.costentityCurrent.boolnIsChangeable,
                    resTemplateOrResource_I.costentityCurrent.intPkAccount,
                    strAccountName, resTemplateOrResource_I.costentityCurrent.numnHourlyRate,
                    resTemplateOrResource_I.costentityCurrent.boolnArea);
            }
            else
            {
                costinhejson = new CostinhejsonCostInheritanceJson(null, null, null, null, null, null, null, null, 
                    null, null);
            }

            int? intnPkResourceInherided = null;
            if (
                resTemplateOrResource_I.resinherited != null
                )
            {
                intnPkResourceInherided = resTemplateOrResource_I.resinherited.intPk;
            }

            //                                              //Second value is false, the father will change this value.
            inhedatajson_O = new InhedatajsonInheritanceDataJson(intnPkResourceInherided, false, unitinhejson,
                costinhejson, avainhejson);

            intStatus_IO = 200;
            strUserMessage_IO = "";
            strDevMessage_IO = "";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subVerifyResourceInfo(
            //                                              //Returns true when the resource has calculations for the io
            //                                              //      and hast cost, time or availability.
            //                                              //If the resource has not one of those things a message will
            //                                              //      be returned in the array of strings.

            int intPkResource_I,
            JobjsonJobJson jobjson_I,
            int intPkWorkflow_I,
            PiwentityProcessInWorkflowEntityDB piwentity_I,
            int intPkEleetOrEleele_I,
            bool boolIsEleet_I,
            IConfiguration configuration_I,
            String strPrintshopId_I,
            int? intnEstimationId_I,
            ref String[] arrstrInfo_M,
            ref bool boolCompleted_IO
            )
        {
            List<String> darrstr = new List<string>();
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Verify the cost.
            CostentityCostEntityDB costentity = context.Cost.FirstOrDefault(cost =>
            cost.intPkResource == intPkResource_I);

            String strCostMessage = (costentity != null) ? "" : "No cost found.";

            if (
                //                                          //There is not cost.
                strCostMessage != ""
                )
            {
                darrstr.Add(strCostMessage);
            }

            //                                              //Verify the availability/time.
            EleentityElementEntityDB eleentity = context.Element.FirstOrDefault(ele => ele.intPk == intPkResource_I);

            String strTimeAvailabilityMessage = "";
            /*CASE*/
            if (
                eleentity.boolnIsAvailable != null
                )
            {
                strTimeAvailabilityMessage = ((bool)eleentity.boolnIsAvailable == true) ? "" : "Not available.";
            }
            else if (
                eleentity.boolnIsCalendar != null
                )
            {
                TimeentityTimeEntityDB timeentity = ResResource.timeentityGet(intPkResource_I, jobjson_I,
                    piwentity_I, intPkEleetOrEleele_I, boolIsEleet_I, configuration_I, strPrintshopId_I, null,
                    intnEstimationId_I, null);

                strTimeAvailabilityMessage = (timeentity != null) ? "" : "No time found.";
            }
            /*END-CASE*/

            if (
                //                                          //Is not available or has not time.
                strTimeAvailabilityMessage != ""
                )
            {
                darrstr.Add(strTimeAvailabilityMessage);
            }

            //                                              //To easy code.
            int? intnPkEleet = null;
            int? intnPkEleele = intPkEleetOrEleele_I;
            if (
                boolIsEleet_I
                )
            {
                intnPkEleet = intPkEleetOrEleele_I;
                intnPkEleele = null;
            }

            //                                              //Verify the calculations.
            CalentityCalculationEntityDB calentity = context.Calculation.FirstOrDefault(cal =>
            cal.intnPkResource == intPkResource_I && cal.intnPkElementElementType == intnPkEleet &&
            cal.intnPkElementElement == intnPkEleele && cal.intnPkWorkflow == intPkWorkflow_I);

            String strCalculationMessage = (calentity != null) ? "" : "No calculation found.";

            if (
                //                                          //No calculations.
                strCalculationMessage != ""
                )
            {
                darrstr.Add(strCalculationMessage);
            }

            boolCompleted_IO = darrstr.Count == 0 || (darrstr.Count == 1 && darrstr[0] == "Not available.");
            arrstrInfo_M = darrstr.ToArray();
        }

        //--------------------------------------------------------------------------------------------------------------
        public bool boolIsDispensable(
            //                                              //True if the resource is available to delete.

            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            intStatus_IO = 401;
            strUserMessage_IO = "This " + (this.boolIsTemplate ? "template:" : "resource:");
            strDevMessage_IO = "";

            bool boolHasCalculationsAssociated = this.boolHasCalculationsAssociated(ref intStatus_IO,
                ref strUserMessage_IO);

            bool boolHasInheritors = this.boolHasInheritors(ref intStatus_IO, ref strUserMessage_IO);

            bool boolMyValuesHaveInheritors = this.boolMyValuesHaveInheritors(ref intStatus_IO, ref strUserMessage_IO);

            bool boolHasLinks = this.boolHasLinks(ref intStatus_IO, ref strUserMessage_IO);

            bool boolIsIO = this.boolIsIO(ref intStatus_IO, ref strUserMessage_IO);

            bool boolIsInWorkflowsWithEstimates = this.boolIsInWorkflowsWithEstimates(ref intStatus_IO,
                ref strUserMessage_IO);

            if (
                intStatus_IO == 401
                )
            {
                intStatus_IO = 200;
                strUserMessage_IO = "";
            }

            return !boolHasCalculationsAssociated && !boolHasInheritors && !boolMyValuesHaveInheritors && !boolHasLinks
                && !boolIsIO && !boolIsInWorkflowsWithEstimates;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public bool boolHasCalculationsAssociated(
            //                                              //Return true if there is calculations for some product 
            //                                              //      associated to this.

            ref int intStatus_IO,
            ref String strUserMessage_IO
            )
        {
            bool boolHasCalculationAssociated = false;

            //                                              //Establish the connection with db.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get the first calculation associated.
            CalentityCalculationEntityDB calentity = context.Calculation.FirstOrDefault(calentity =>
                calentity.intnPkResource == this.intPk);

            //                                              //If the calculation is null means that there is no 
            //                                              //      calculations associated to this.
            boolHasCalculationAssociated = calentity != null;

            if (
                boolHasCalculationAssociated
                )
            {
                intStatus_IO = 402;
                strUserMessage_IO = strUserMessage_IO + " Has calculations associated.";
            }

            return boolHasCalculationAssociated;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public bool boolHasInheritors(
            //                                              //Return true if there is other elements that have this as 
            //                                              //      resource inherited from.
            ref int intStatus_IO,
            ref String strUserMessage_IO
            )
        {
            bool boolHasInheritors = false;

            //                                              //Establish the connection with db.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get the first element inheritor.
            EleentityElementEntityDB eleentity = context.Element.FirstOrDefault(ele =>
                ele.intnPkElementInherited == this.intPk);

            //                                              //If the element is null means that there is no 
            //                                              //      elements associated to this.
            boolHasInheritors = eleentity != null;

            if (
               boolHasInheritors
               )
            {
                intStatus_IO = 402;
                strUserMessage_IO = strUserMessage_IO + " Has Inheritors.";
            }

            return boolHasInheritors;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public bool boolMyValuesHaveInheritors(
            //                                              //Return true if there is other elements that have 
            //                                              //      attributes values that are inheritors from my 
            //                                              //      values.
            ref int intStatus_IO,
            ref String strUserMessage_IO
            )
        {
            bool boolMyValuesHasInheritors = false;

            //                                              //Establish the connection with db.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get all my attribute values.
            IQueryable<ValentityValueEntityDB> setvalentity = context.Value.Where(valentity =>
                valentity.intPkElement == this.intPk);

            List<ValentityValueEntityDB> darrvalentity = setvalentity.ToList();

            int intI = 0;
            /*UNTIL-DO*/
            while (!(
                (intI >= darrvalentity.Count()) ||
                boolMyValuesHasInheritors
                ))
            {
                //                                              //Get the first element inheritor.
                ValentityValueEntityDB valentity = context.Value.FirstOrDefault(val =>
                    val.intnPkValueInherited == darrvalentity[intI].intPk);

                //                                              //If the element is null means that there is no 
                //                                              //      elements associated to this.
                boolMyValuesHasInheritors = valentity != null;

                intI = intI + 1;
            }

            if (
               boolMyValuesHasInheritors
               )
            {
                intStatus_IO = 402;
                strUserMessage_IO = strUserMessage_IO + " Has values with inheritors.";
            }

            return boolMyValuesHasInheritors;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public bool boolHasLinks(
            //                                              //Return true if there is links for some product 
            //                                              //      associated to this.
            ref int intStatus_IO,
            ref String strUserMessage_IO
            )
        {
            bool boolHasLinks = false;

            //                                              //Establish the connection with db.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Look for links if res is res, but not template.
            if (
                //                                          //res is res.
                !this.boolIsTemplate
                )
            {
                //                                          //Get the first link associated.
                IoentityInputsAndOutputsEntityDB ioentity = context.InputsAndOutputs.FirstOrDefault(io =>
                    (io.intnPkResource == this.intPk) && io.strLink != null);

                if (
                    ioentity != null
                    )
                {
                    boolHasLinks = true;
                }
            }
            else
            {
                //                                          //Look for links as template.

                //                                          //eleele where this res is son.
                IQueryable<EleeleentityElementElementEntityDB> seteleeleentity = context.ElementElement.Where(
                    eleeleentity => eleeleentity.intPkElementSon == this.intPk);

                List<EleeleentityElementElementEntityDB> darreleeleentity = seteleeleentity.ToList();

                int intI = 0;
                /*WHILE-DO*/
                while (
                    intI < darreleeleentity.Count &&
                    !boolHasLinks
                    )
                {
                    int intPkEleele = darreleeleentity[intI].intPk;

                    //                                          //IO with this eleele.
                    IQueryable<IoentityInputsAndOutputsEntityDB> setioentity = context.InputsAndOutputs.Where(
                        ioentity => ioentity.intnPkElementElement == intPkEleele);

                    List<IoentityInputsAndOutputsEntityDB> darrioentity = setioentity.ToList();

                    int intJ = 0;
                    /*WHILE-DO*/
                    while (
                        intJ < darrioentity.Count &&
                        !boolHasLinks
                        )
                    {
                        if (
                            darrioentity[intJ].strLink != null
                            )
                        {
                            boolHasLinks = true;
                        }
                        intJ = intJ + 1;
                    }
                    intI = intI + 1;
                }
            }
            if (
                boolHasLinks
                )
            {
                intStatus_IO = 402;
                strUserMessage_IO = strUserMessage_IO + " Has links associated.";
            }

            return boolHasLinks;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public bool boolIsIO(
            //                                              //Return true if there is other elements that have this as 
            //                                              //      resource inherited from.
            ref int intStatus_IO,
            ref String strUserMessage_IO
            )
        {
            bool boolIsIO = false;

            //                                              //Establish the connection with db.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get the first element inheritor.
            EleeleentityElementElementEntityDB eleeleentity = context.ElementElement.FirstOrDefault(eleele =>
                eleele.intPkElementSon == this.intPk);

            //                                              //If the element is null means that there is no 
            //                                              //      elements associated to this.
            boolIsIO = eleeleentity != null;

            if (
               boolIsIO
               )
            {
                intStatus_IO = 402;
                strUserMessage_IO = strUserMessage_IO + " Is Input or Output of a process.";
            }

            return boolIsIO;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public bool boolIsInWorkflowsWithEstimates(
            //                                              //Return true if this is used by workflows with estimates.

            ref int intStatus_IO,
            ref String strUserMessage_IO
            )
        {
            Odyssey2Context context = new Odyssey2Context();
            List<EstentityEstimateEntityDB> darrestentity = new List<EstentityEstimateEntityDB>();

            bool boolIs = false;
            if (
                RestypResourceType.boolIsPhysical(this.restypBelongsTo.strClassification)
                )
            {
                if (
                    //                                      //This is a resource.
                    !this.boolIsTemplate
                    )
                {
                    //                                      //From IO table in PkResource column.
                    List<EstentityEstimateEntityDB> darrestentityFromIOWithResource = (
                        from estentity in context.Estimate
                        join wfentity in context.Workflow
                        on estentity.intPkWorkflow equals wfentity.intPk
                        join ioentity in context.InputsAndOutputs
                        on wfentity.intPk equals ioentity.intPkWorkflow
                        where wfentity.boolDeleted == false && ioentity.intnPkResource == this.intPk
                        select estentity).ToList();
                    darrestentity.AddRange(darrestentityFromIOWithResource);

                    //                                      //From IO table in a group.
                    List<EstentityEstimateEntityDB> darrestentityFromIOInAGroup = (
                        from estentity in context.Estimate
                        join wfentity in context.Workflow
                        on estentity.intPkWorkflow equals wfentity.intPk
                        join ioentity in context.InputsAndOutputs
                        on wfentity.intPk equals ioentity.intPkWorkflow
                        join gpresentity in context.GroupResource
                        on ioentity.intnGroupResourceId equals gpresentity.intId
                        where wfentity.boolDeleted == false && ioentity.intPkWorkflow == gpresentity.intPkWorkflow &&
                        gpresentity.intPkResource == this.intPk
                        select estentity).ToList();
                    darrestentity.AddRange(darrestentityFromIOInAGroup);

                    //                                      //From IO table in PkResource column.
                    List<EstentityEstimateEntityDB> darrestentityFromIOJ = (
                        from estentity in context.Estimate
                        join wfentity in context.Workflow
                        on estentity.intPkWorkflow equals wfentity.intPk
                        join piwentity in context.ProcessInWorkflow
                        on wfentity.intPk equals piwentity.intPkWorkflow
                        join iojentity in context.InputsAndOutputsForAJob
                        on piwentity.intPk equals iojentity.intPkProcessInWorkflow
                        where wfentity.boolDeleted == false && iojentity.intPkResource == this.intPk
                        select estentity).ToList();
                    darrestentity.AddRange(darrestentityFromIOJ);
                }
                else
                {
                    //                                      //This is a template.
                    List<EstentityEstimateEntityDB> darrestentityFromEleele = (
                        from estentity in context.Estimate
                        join wfentity in context.Workflow
                        on estentity.intPkWorkflow equals wfentity.intPk
                        join piwentity in context.ProcessInWorkflow
                        on wfentity.intPk equals piwentity.intPkWorkflow
                        join eleentity in context.Element
                        on piwentity.intPkProcess equals eleentity.intPk
                        join eleeleentity in context.ElementElement
                        on eleentity.intPk equals eleeleentity.intPkElementDad
                        where eleeleentity.intPkElementSon == this.intPk && wfentity.boolDeleted == false
                        select estentity).ToList();
                    darrestentity.AddRange(darrestentityFromEleele);
                }

                intStatus_IO = darrestentity.Count == 0 ? intStatus_IO : 402;
                strUserMessage_IO = darrestentity.Count == 0 ? strUserMessage_IO : strUserMessage_IO + " Is in " +
                    "workflows associated to estimates and these will be deleted.";
                boolIs = darrestentity.Count > 0;
            }
            return boolIs;
        }

        //--------------------------------------------------------------------------------------------------------------
        public List<AttrvaljsonAttributeValueJson> darrattrvalGet(
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            IQueryable<ValentityValueEntityDB> setvalentity = context.Value.Where(valentity =>
                valentity.intPkElement == this.intPk);
            List<ValentityValueEntityDB> darrvalentity = setvalentity.ToList();

            List<AttrvaljsonAttributeValueJson> darrattrval = new List<AttrvaljsonAttributeValueJson>();
            foreach (ValentityValueEntityDB valentity in darrvalentity)
            {
                AttrentityAttributeEntityDB attrentity = context.Attribute.FirstOrDefault(attrentity =>
                    attrentity.intPk == valentity.intPkAttribute);

                //                                          //Get attribute value.
                String strValue = valentity.strValue;
                if (
                    //                                      //If the attribute is unit, get the last value.
                    (attrentity.strXJDFName == "Unit") ||
                    (attrentity.strCustomName == "Unit")
                    )
                {
                    ValentityValueEntityDB valentityUnit = ResResource.GetResourceUnitOfMeasurement(
                        valentity.intPkElement);
                    strValue = valentityUnit.strValue;
                }


                AttrvaljsonAttributeValueJson attrvaljson = new AttrvaljsonAttributeValueJson();
                attrvaljson.intPk = attrentity.intPk;
                attrvaljson.strName = attrentity.strXJDFName;
                attrvaljson.strValue = strValue;

                if (
                    (attrvaljson.strName != "Name") &&
                    (attrvaljson.strName != "Unit")
                    )
                {
                    darrattrval.Add(attrvaljson);
                }
            }

            return darrattrval;
        }

        //--------------------------------------------------------------------------------------------------------------
        public Attrjson3AttributeJson3 attrjson3Get(
            int intPk_I,
            Odyssey2Context context_I
            )
        {
            ValentityValueEntityDB valentity = context_I.Value.FirstOrDefault(valentity =>
                valentity.intPkAttribute == intPk_I &&
                valentity.intPkElement == this.intPk);

            Attrjson3AttributeJson3 attrjson3 = null;
            if (
                valentity != null
                )
            {
                attrjson3 = new Attrjson3AttributeJson3();
                AscentityAscendantsEntityDB ascentity = context_I.Ascendants.FirstOrDefault(ascentity =>
                    ascentity.strAscendants.EndsWith("" + intPk_I) && ascentity.intPkElement == this.intPk);

                if (
                    ascentity != null
                    )
                {
                    attrjson3.arrAscendantPk = Tools.arrintElementPk(ascentity.strAscendants);
                    attrjson3.strValue = valentity.strValue;
                    attrjson3.intValuePk = valentity.intPk;
                }
            }

            return attrjson3;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static EleascjsonElementAscendantJson[] arreleascjsonGet(
            int intPk_I
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            //                                          //List of Resource of a elementType
            List<EleascjsonElementAscendantJson> darreleascjson = new List<EleascjsonElementAscendantJson>();

            //                                          //Get each Resource.
            IQueryable<EleentityElementEntityDB> seteleentity = context.Element.Where(ele =>
            ele.intPkElementType == intPk_I && ele.boolIsTemplate == false);

            foreach (EleentityElementEntityDB ele in seteleentity.ToArray())
            {
                EleascjsonElementAscendantJson eleascjson = new EleascjsonElementAscendantJson();
                eleascjson.intPk = ele.intPk;
                eleascjson.strName = ele.strElementName;

                //                                      //List of ascendantElements of each element
                List<AscvaljsonAscendantsValueJson> darrascval = new List<AscvaljsonAscendantsValueJson>();

                //                                      //Get each AscendatElement associate to Element. 
                IQueryable<AscentityAscendantsEntityDB> setascentity = context.Ascendants.Where(
                    asc => asc.intPkElement == ele.intPk);

                foreach (AscentityAscendantsEntityDB asc in setascentity.ToArray())
                {
                    AscvaljsonAscendantsValueJson ascvaljson = new AscvaljsonAscendantsValueJson();
                    //                                  //Get name of the ascendats.
                    ascvaljson.arrAscendant = Tools.arrstrAscendantName(asc.strAscendants);

                    //                                  //Get the last pk of the string
                    int intPkValue = Tools.arrintElementPk(asc.strAscendants).Last();

                    //                                  //Get the value of the database.
                    ValentityValueEntityDB valentity = context.Value.FirstOrDefault(val =>
                        val.intPkAttribute == intPkValue);

                    ascvaljson.strValue = valentity.strValue;

                    //                                  //Add to the list 
                    if (
                        ascvaljson.arrAscendant[0] != "XJDFName"
                        )
                    {
                        darrascval.Add(ascvaljson);
                    }
                }
                eleascjson.arrasc = darrascval.ToArray();
                darreleascjson.Add(eleascjson);
            }

            return darreleascjson.ToArray();
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetAttributesTemplatesAndResources(
            //                                              //Used in MyResources to present the attributes, templates 
            //                                              //      and resources.

            int intPk_I,
            bool boolIsType_I,
            out TyportemjsonTypeOrTemplateJson typortemjson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            typortemjson_O = null;

            if (
                //                                          //Is a type.
                boolIsType_I
                )
            {
                intStatus_IO = 401;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Type not found.";
                EtElementTypeAbstract et = EtElementTypeAbstract.etFromDB(intPk_I);
                if (
                    et != null &&
                    et.strResOrPro == EtElementTypeAbstract.strResource
                    )
                {
                    RestypResourceType restyp = (RestypResourceType)et;
                    //                                      //Create the path element. Is a type so it has only one 
                    //                                      //      element in the path.
                    String strName = (restyp.strXJDFTypeId == "None") ? restyp.strCustomTypeId : restyp.strXJDFTypeId;
                    PejsonPathElementJson pejson = new PejsonPathElementJson(restyp.intPk, strName, true, false);
                    PejsonPathElementJson[] arrpejson = new PejsonPathElementJson[1];
                    arrpejson[0] = pejson;

                    //                                      //The array of attributes is empty because a type has not
                    //                                      //      values for attributes.
                    Attrjson10AttributeJson10[] arrattr = new Attrjson10AttributeJson10[0];

                    //                                      //Get all the templates.
                    ResortemjsonResourceOrTemplateJsonsourceJson[] arrtem =
                        ResResource.arrresortemjsonGetFromType(restyp, true);

                    //                                      //Get all the resources.
                    ResortemjsonResourceOrTemplateJsonsourceJson[] arrres =
                        ResResource.arrresortemjsonGetFromType(restyp, false);

                    bool boolIsDeviceToolOrCustom = RestypResourceType.boolIsDeviceToolOrCustom(restyp);
                    bool boolIsPhysical = RestypResourceType.boolIsPhysical(restyp.strClassification);

                    typortemjson_O = new TyportemjsonTypeOrTemplateJson(arrpejson, arrattr, arrtem, arrres,
                        boolIsPhysical, boolIsDeviceToolOrCustom);

                    intStatus_IO = 200;
                    strUserMessage_IO = "";
                    strDevMessage_IO = "";
                }
            }
            else
            {
                //                                          //Is a template or a resource.

                ResResource res = ResResource.resFromDB(intPk_I, false);
                if (
                    res == null
                    )
                {
                    res = ResResource.resFromDB(intPk_I, true);
                }

                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Template or resource not found.";
                if (
                    res != null
                    )
                {
                    //                                      //Create the path element array.
                    PejsonPathElementJson[] arrpejson = ResResource.arrpejsonGetPath(res);

                    //                                      //Get the array of attributes.
                    Attrjson10AttributeJson10[] arrattr = ResResource.arrattrjsonGet(res);

                    //                                      //Get all the templates.
                    ResortemjsonResourceOrTemplateJsonsourceJson[] arrtem =
                        ResResource.arrresortemjsonGetFromRes(res, true);

                    //                                      //Get all the resources.
                    ResortemjsonResourceOrTemplateJsonsourceJson[] arrres =
                        ResResource.arrresortemjsonGetFromRes(res, false);

                    bool boolIsDeviceToolOrCustom = RestypResourceType.boolIsDeviceToolOrCustom(res.restypBelongsTo);
                    bool boolIsPhysical = RestypResourceType.boolIsPhysical(res.restypBelongsTo.strClassification);

                    typortemjson_O = new TyportemjsonTypeOrTemplateJson(arrpejson, arrattr, arrtem, arrres,
                        boolIsPhysical, boolIsDeviceToolOrCustom);

                    intStatus_IO = 200;
                    strUserMessage_IO = "";
                    strDevMessage_IO = "Data obtained successfully.";
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static ResortemjsonResourceOrTemplateJsonsourceJson[] arrresortemjsonGetFromType(
            //                                              //Get all the resources or templates from the given type.

            RestypResourceType restyp_I,
            //                                              //true, return templates. 
            //                                              //false, return resources.
            bool boolIsTemplate_I
            )
        {
            List<ResortemjsonResourceOrTemplateJsonsourceJson> darrresortemjson =
                new List<ResortemjsonResourceOrTemplateJsonsourceJson>();

            Odyssey2Context context = new Odyssey2Context();
            IQueryable<EleentityElementEntityDB> seteleentity = context.Element.Where(eleentity =>
                eleentity.intPkElementType == restyp_I.intPk &&
                eleentity.intnPkElementInherited == null &&
                eleentity.boolIsTemplate == boolIsTemplate_I &&
                eleentity.boolDeleted == false);

            foreach (EleentityElementEntityDB eleentity in seteleentity)
            {
                ResResource res = ResResource.resFromDB(eleentity.intPk, boolIsTemplate_I);
                if (
                    res != null
                    )
                {
                    bool boolCostIsChangeable = true;
                    if (
                        (res.costentityCurrent != null) &&
                        (res.costentityCurrent.boolnIsChangeable != null)
                        )
                    {
                        boolCostIsChangeable = (bool)res.costentityCurrent.boolnIsChangeable;
                    }

                    bool boolCalendarIsChangeable = true;
                    if (
                        eleentity.boolnCalendarIsChangeable != null
                        )
                    {
                        boolCalendarIsChangeable = (bool)eleentity.boolnCalendarIsChangeable;
                    }

                    //                                  //Get the current unit of measurement.
                    ValentityValueEntityDB valentity = ResResource.GetResourceUnitOfMeasurement(res.intPk);
                    String strUnit = valentity != null ? valentity.strValue : " ";

                    String strResourceName = ResResource.strGetMediaResourceName(eleentity.intPk);

                    ResortemjsonResourceOrTemplateJsonsourceJson resortem =
                        new ResortemjsonResourceOrTemplateJsonsourceJson(eleentity.intPk, strResourceName,
                        strUnit, false, eleentity.boolnIsCalendar, eleentity.boolnIsAvailable, boolCalendarIsChangeable,
                        boolCostIsChangeable);
                    darrresortemjson.Add(resortem);
                }
            }

            return darrresortemjson.ToArray();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static PejsonPathElementJson[] arrpejsonGetPath(
            ResResource res_I
            )
        {
            List<PejsonPathElementJson> darrpejson = new List<PejsonPathElementJson>();

            //                                              //Add the current.
            PejsonPathElementJson pejson = new PejsonPathElementJson(res_I.intPk, res_I.strName, false,
                res_I.boolIsTemplate == false);
            darrpejson.Add(pejson);

            ResResource res = res_I;
            /*WHILE-DO*/
            while (
                res.resinherited != null
                )
            {
                pejson = new PejsonPathElementJson(res.resInherited_Z.intPk, res.resInherited_Z.strName, false,
                    res.resinherited.boolIsTemplate == false);
                darrpejson.Add(pejson);

                res = res.resinherited;
            }

            //                                              //Add the type.
            String strName = (res.restypBelongsTo.strXJDFTypeId == "None") ? res.restypBelongsTo.strCustomTypeId :
                res.restypBelongsTo.strXJDFTypeId;
            pejson = new PejsonPathElementJson(res.restypBelongsTo.intPk, strName, true,
                false);
            darrpejson.Add(pejson);

            List<PejsonPathElementJson> darrpejsonOrdered = new List<PejsonPathElementJson>();
            //                                              //Invert the array.
            for (int intI = darrpejson.Count() - 1; intI >= 0; intI = intI - 1)
            {
                darrpejsonOrdered.Add(darrpejson[intI]);
            }

            return darrpejsonOrdered.ToArray();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static Attrjson10AttributeJson10[] arrattrjsonGet(
            ResResource res_I
            )
        {
            Odyssey2Context context = new Odyssey2Context();
            IQueryable<ValentityValueEntityDB> setvalentity = context.Value.Where(valentity =>
                valentity.intPkElement == res_I.intPk);
            List<ValentityValueEntityDB> darrvalentity = setvalentity.ToList();

            List<Attrjson10AttributeJson10> darrattrjson = new List<Attrjson10AttributeJson10>();
            bool boolUnitAdded = false;
            foreach (ValentityValueEntityDB valentity in darrvalentity)
            {
                AttrentityAttributeEntityDB attrentity = context.Attribute.FirstOrDefault(attr =>
                    attr.intPk == valentity.intPkAttribute);

                if (
                    attrentity.strCustomName != "XJDFName"
                    )
                {
                    if (
                        attrentity.strCustomName != "XJDFUnit"
                        )
                    {
                        String strName = (attrentity.strXJDFName == "") ? attrentity.strCustomName :
                            attrentity.strXJDFName;
                        Attrjson10AttributeJson10 attrjson = new Attrjson10AttributeJson10(attrentity.intPk,
                            strName, valentity.strValue);

                        darrattrjson.Add(attrjson);
                    }
                    else if (
                        !boolUnitAdded
                        )
                    {
                        //                                      //Get the current unit of measurement.
                        ValentityValueEntityDB valentityUnit = ResResource.GetResourceUnitOfMeasurement(res_I.intPk);

                        String strName = (attrentity.strXJDFName == "") ? attrentity.strCustomName :
                            attrentity.strXJDFName;
                        Attrjson10AttributeJson10 attrjson = new Attrjson10AttributeJson10(attrentity.intPk,
                            strName, valentityUnit.strValue);

                        darrattrjson.Add(attrjson);
                        boolUnitAdded = true;

                    }
                }
            }
            return darrattrjson.ToArray();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static ResortemjsonResourceOrTemplateJsonsourceJson[] arrresortemjsonGetFromRes(
            //                                              //Get all the resources or templates from the given template.
            ResResource res_I,
            //                                              //true, return templates. 
            //                                              //false, return resources.
            bool boolIsTemplate_I
            )
        {
            List<ResortemjsonResourceOrTemplateJsonsourceJson> darrresortemjson =
                new List<ResortemjsonResourceOrTemplateJsonsourceJson>();

            Odyssey2Context context = new Odyssey2Context();
            IQueryable<EleentityElementEntityDB> seteleentity = context.Element.Where(eleentity =>
                eleentity.intnPkElementInherited == res_I.intPk &&
                eleentity.boolIsTemplate == boolIsTemplate_I &&
                eleentity.boolDeleted == false);

            foreach (EleentityElementEntityDB eleentity in seteleentity)
            {
                ResResource res = ResResource.resFromDB(eleentity.intPk, boolIsTemplate_I);
                bool boolCostIsChangeable = true;
                if (
                    (res.costentityCurrent != null) &&
                    (res.costentityCurrent.boolnIsChangeable != null)
                    )
                {
                    boolCostIsChangeable = (bool)res.costentityCurrent.boolnIsChangeable;
                }

                bool boolCalendarIsChangeable = true;
                if (
                    eleentity.boolnCalendarIsChangeable != null
                    )
                {
                    boolCalendarIsChangeable = (bool)eleentity.boolnCalendarIsChangeable;
                }

                //                                  //Get the current unit of measurement.
                ValentityValueEntityDB valentity = ResResource.GetResourceUnitOfMeasurement(res.intPk);
                String strUnit = valentity != null ? valentity.strValue : " ";

                //                                          //Get Resource's name.
                String strResourceName = ResResource.strGetMediaResourceName(eleentity.intPk);

                ResortemjsonResourceOrTemplateJsonsourceJson resortem =
                    new ResortemjsonResourceOrTemplateJsonsourceJson(eleentity.intPk, strResourceName,
                    strUnit, false, eleentity.boolnIsCalendar, eleentity.boolnIsAvailable, boolCalendarIsChangeable,
                    boolCostIsChangeable);
                darrresortemjson.Add(resortem);
            }

            return darrresortemjson.ToArray();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String strGetMediaResourceName(
            //                                              //Get the concanate name of the media resource given.

            int intPkResource_I
            )
        {
            //                                              //Connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get the resource.
            EleentityElementEntityDB eleentity = context.Element.FirstOrDefault(ele => ele.intPk == intPkResource_I);

            //                                              //Get type of the resource.
            EtentityElementTypeEntityDB etentity = context.ElementType.FirstOrDefault(et =>
                et.intPk == eleentity.intPkElementType);

            //                                              //Set resource's name.
            String strResourceName = eleentity.strElementName;

            if (
                etentity.strXJDFTypeId == "Media"
                )
            {
                //                                          //Obtain attributes for this media resource.
                List<ResattrjsonResourceAttributesJson> darrmedattrjson =
                                                                (from attrEntity in context.Attribute
                                                                 join valentity in context.Value
                                                                 on attrEntity.intPk equals valentity.intPkAttribute
                                                                 where valentity.intPkElement == intPkResource_I
                                                                 select new ResattrjsonResourceAttributesJson(
                                                                 attrEntity.intPk,
                                                                 attrEntity.strXJDFName,
                                                                 valentity.strValue)).ToList();
                /*CASE*/
                if (
                    //                                      //Media Resource has Width, Length and DimensionsUnit.
                    darrmedattrjson.Exists(attr => attr.strAttrName == "Width") &&
                    darrmedattrjson.Exists(attr => attr.strAttrName == "Length") &&
                    darrmedattrjson.Exists(attr => attr.strAttrName == "DimensionsUnit")
                    )
                {
                    String strWidthValue = darrmedattrjson.FirstOrDefault(attr => 
                        attr.strAttrName == "Width").strAttrValue;
                    String strLengthValue = darrmedattrjson.FirstOrDefault(attr =>
                        attr.strAttrName == "Length").strAttrValue;
                    String strUnit = darrmedattrjson.FirstOrDefault(attr =>
                        attr.strAttrName == "DimensionsUnit").strAttrValue;

                    //                                          //Set new resource's name.
                    strResourceName = eleentity.strElementName + " " + "-" + " " + strWidthValue + strUnit + " " +
                        "x" + " " + strLengthValue + strUnit;
                }else if (
                    //                                      //Media resource has Width, WidthUnit, Length and
                    //                                      //      LengthUnit.
                    darrmedattrjson.Exists(attr => attr.strAttrName == "Width") &&
                    darrmedattrjson.Exists(attr => attr.strAttrName == "WidthUnit") &&
                    darrmedattrjson.Exists(attr => attr.strAttrName == "Length") &&
                    darrmedattrjson.Exists(attr => attr.strAttrName == "LengthUnit")
                    )
                {
                    String strWidthValue = darrmedattrjson.FirstOrDefault(attr =>
                        attr.strAttrName == "Width").strAttrValue;
                    String strWidthUnit = darrmedattrjson.FirstOrDefault(attr =>
                        attr.strAttrName == "WidthUnit").strAttrValue;
                    String strLengthValue = darrmedattrjson.FirstOrDefault(attr =>
                        attr.strAttrName == "Length").strAttrValue;
                    String strLengthUnit = darrmedattrjson.FirstOrDefault(attr =>
                        attr.strAttrName == "LengthUnit").strAttrValue;

                    //                                          //Set new resource's name.
                    strResourceName = eleentity.strElementName + " " + "-" + " " + strWidthValue + strWidthUnit + " " +
                        "x" + " " + strLengthValue + strLengthUnit;
                }
                /*END-CASE*/
            }

            return strResourceName;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String strGetMediaResourceName(
            //                                              //Get the concanate name of the media resource given.

            EleentityElementEntityDB eleentity_I,
            //                                              //Attribute from Type.
            int[] darrintPkDimensionsAttribute_I
            )
        {
            //                                              //Connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Set resource's name.
            String strResourceName = eleentity_I.strElementName;

            List<ValentityValueEntityDB> darrvalentity = context.Value.Where(val =>
                    val.intPkElement == eleentity_I.intPk).ToList();

            //                                              //Look for the value of the dimension attributes.
            //                                              //Media resource has to have three attibutes or none of
            //                                              //      them.

            ValentityValueEntityDB valentityWidth = darrvalentity.FirstOrDefault(val =>
                val.intPkAttribute == darrintPkDimensionsAttribute_I[0]);

            ValentityValueEntityDB valentityLength = darrvalentity.FirstOrDefault(val =>
                val.intPkAttribute == darrintPkDimensionsAttribute_I[1]);

            ValentityValueEntityDB valentityDimensionsUnit = darrvalentity.FirstOrDefault(val =>
                val.intPkAttribute == darrintPkDimensionsAttribute_I[2]);

            if (
                //                                          //Resource has all the dimension attribute.
                valentityWidth != null &&
                valentityLength != null &&
                valentityDimensionsUnit != null
                )
            {
                //                                          //Set new resource's name.
                strResourceName = eleentity_I.strElementName + " " + "-" + " " + valentityWidth.strValue +
                    valentityDimensionsUnit.strValue + " " + "x" + " " + valentityLength.strValue +
                    valentityDimensionsUnit.strValue;
            }

            return strResourceName;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetTypeOrTemplateAllResources(
            //                                              //Get all resources for a template.
            int intPkEleetOrEleele_I,
            bool boolIsEleet_I,
            int intPkProcessInWorkflow_I,
            int? intnJobId_I,
            String strPrintshopId_I,
            out List<TyportempresjsonTypeOrTemplateResourceJson> darrtyportempresjson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);
            darrtyportempresjson_O = new List<TyportempresjsonTypeOrTemplateResourceJson>();

            Odyssey2Context context = new Odyssey2Context();

            if (
                boolIsEleet_I
                )
            {
                //                                          //Valid the input/output.
                EleetentityElementElementTypeEntityDB eleetentity = context.ElementElementType.FirstOrDefault(eleet =>
                    eleet.intPk == intPkEleetOrEleele_I);

                PiwentityProcessInWorkflowEntityDB piwentity = context.ProcessInWorkflow.FirstOrDefault(piw =>
                            piw.intPk == intPkProcessInWorkflow_I);

                intStatus_IO = 401;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Info about the IO in the workflow is not valid.";
                if (
                    piwentity != null &&
                    eleetentity != null
                    )
                {
                    EtElementTypeAbstract et = EtElementTypeAbstract.etFromDB(eleetentity.intPkElementTypeSon);

                    RestypResourceType restyp = null;
                    if (
                        (et != null) && (et.strResOrPro == EtElementTypeAbstract.strResource)
                        )
                    {
                        restyp = (RestypResourceType)et;
                    }

                    intStatus_IO = 402;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "Resource type not found.";
                    if (
                        restyp != null
                        )
                    {
                        //                                  //Get job register.
                        JobentityJobEntityDB jobentity = context.Job.FirstOrDefault(job => job.intJobID ==
                            intnJobId_I && job.intPkPrintshop == ps.intPk);

                        List<EleentityElementEntityDB> darreleentity = new List<EleentityElementEntityDB>();
                        if (
                            //                              //Job still pending.
                            (intnJobId_I != null &&
                            jobentity == null) ||
                            //                              //Job is inProgress.
                            (intnJobId_I != null &&
                            jobentity != null &&
                            jobentity.intStage == JobJob.intInProgressStage)
                            )
                        {
                            IoentityInputsAndOutputsEntityDB ioentity = context.InputsAndOutputs.FirstOrDefault(io =>
                                io.intnPkElementElementType == intPkEleetOrEleele_I &&
                                io.intnProcessInWorkflowId == piwentity.intProcessInWorkflowId &&
                                io.intPkWorkflow == piwentity.intPkWorkflow);
                            /*CASE*/
                            if (
                                //                          //There is not a group.
                                (ioentity == null) ||
                                (ioentity != null &&
                                (ioentity.intnPkResource == null && ioentity.intnGroupResourceId == null))
                                )
                            {
                                //                          //Get all resources from the type.
                                IQueryable<EleentityElementEntityDB> seteleentityResource =
                                    context.Element.Where(ele => ele.intPkElementType == restyp.intPk &&
                                    ele.boolIsTemplate == false && ele.boolDeleted == false);
                                darreleentity = seteleentityResource.ToList();
                            }
                            else if (
                                //                          //There is a group.
                                ioentity != null &&
                                ioentity.intnGroupResourceId != null
                                )
                            {
                                List<GpresentityGroupResourceEntityDB> darrgpresentity = context.GroupResource.Where(
                                    gp => gp.intId == ioentity.intnGroupResourceId &&
                                    gp.intPkWorkflow == piwentity.intPkWorkflow).ToList();

                                foreach (GpresentityGroupResourceEntityDB gpresentity in darrgpresentity)
                                {
                                    EleentityElementEntityDB eleentity = context.Element.FirstOrDefault(ele =>
                                        ele.intPk == gpresentity.intPkResource);
                                    if (
                                        eleentity.boolDeleted == false
                                        )
                                    {
                                        darreleentity.Add(eleentity);
                                    }
                                }
                            }
                            else if (
                               //                          //There is a resource set.
                               ioentity != null &&
                               ioentity.intnPkResource != null
                               )
                            {
                                EleentityElementEntityDB eleentity = context.Element.FirstOrDefault(ele =>
                                        ele.intPk == ioentity.intnPkResource);
                                if (
                                    eleentity.boolDeleted == false
                                    )
                                {
                                    darreleentity.Add(eleentity);
                                }
                            }
                            /*END-CASE*/
                        }
                        else if (
                           intnJobId_I == null
                           )
                        {
                            //                              //Get all resources from the type.
                            IQueryable<EleentityElementEntityDB> seteleentityResource =
                                context.Element.Where(ele => ele.intPkElementType == restyp.intPk &&
                                ele.boolIsTemplate == false && ele.boolDeleted == false);
                            darreleentity = seteleentityResource.ToList();
                        }
                        /*END-CASE*/

                        if (
                            darreleentity.Count > 0
                            )
                        {
                            //                                      //Get the pks of dimensions attributes.
                            int[] arrintPkDimensionsAttribute = restyp.strXJDFTypeId == "Media" ?
                                ResResource.arrintPkDimensionsAttributeGet(restyp.intPk) : null;

                            foreach (EleentityElementEntityDB eleentity in darreleentity)
                            {
                                String strResourceName;

                                if (
                                    restyp.strXJDFTypeId == "Media" &&
                                    arrintPkDimensionsAttribute != null &&
                                    //                          //The dimension are considered how a Unit.
                                    arrintPkDimensionsAttribute.Length == 3
                                    )
                                {
                                    //strResourceName = ResResource.strGetMediaResourceName(eleentity,
                                    //arrintPkDimensionsAttribute);
                                    strResourceName = ResResource.strGetMediaResourceName(eleentity.intPk);
                                }
                                else
                                {
                                    //                                              //Set resource's name.
                                    strResourceName = eleentity.strElementName;
                                }

                                TyportempresjsonTypeOrTemplateResourceJson typeortempresjson = new
                                    TyportempresjsonTypeOrTemplateResourceJson(eleentity.intPk,
                                    strResourceName);

                                darrtyportempresjson_O.Add(typeortempresjson);
                            }
                            intStatus_IO = 200;
                            strUserMessage_IO = "Success.";
                            strDevMessage_IO = "";
                        }
                        else
                        {
                            intStatus_IO = 200;
                            strUserMessage_IO = "Success.";
                            strDevMessage_IO = "";
                        }
                    }
                }
            }
            else
            {
                //                                          //Valid the input/output.
                EleeleentityElementElementEntityDB eleeleentity = context.ElementElement.FirstOrDefault(etel =>
                   etel.intPk == intPkEleetOrEleele_I);

                PiwentityProcessInWorkflowEntityDB piwentity = context.ProcessInWorkflow.FirstOrDefault(piw =>
                            piw.intPk == intPkProcessInWorkflow_I);

                intStatus_IO = 403;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Info about the IO in the workflow is not valid.";

                if (
                    piwentity != null &&
                    eleeleentity != null
                    )
                {
                    //                                      //Get the template.
                    ResResource resTemplate = ResResource.resFromDB(eleeleentity.intPkElementSon, true);
                    if (
                        resTemplate != null
                        )
                    {
                        RestypResourceType restyp = resTemplate.restypBelongsTo;

                        //                                  //Get job register.
                        JobentityJobEntityDB jobentity = context.Job.FirstOrDefault(job => job.intJobID ==
                            intnJobId_I && job.intPkPrintshop == ps.intPk);

                        List<EleentityElementEntityDB> darreleentity = new List<EleentityElementEntityDB>();
                        /*CASE*/
                        if (
                            (intnJobId_I != null &&
                            jobentity == null) ||
                            (intnJobId_I != null &&
                            jobentity != null &&
                            jobentity.intStage == JobJob.intInProgressStage)
                            )
                        {
                            IoentityInputsAndOutputsEntityDB ioentity = context.InputsAndOutputs.FirstOrDefault(io =>
                                io.intnPkElementElement == intPkEleetOrEleele_I &&
                                io.intnProcessInWorkflowId == piwentity.intProcessInWorkflowId &&
                                io.intPkWorkflow == piwentity.intPkWorkflow);
                            /*CASE*/
                            if (
                                //                          //There is not a group.
                                (ioentity == null) ||
                                (ioentity != null &&
                                (ioentity.intnPkResource == null && ioentity.intnGroupResourceId == null))
                                )
                            {
                                //                          //Get All resources from template and derivate template
                                //                          //    resources.
                                ResResource.subGetAllResourcesFromTemplateAndDerivateTempResources(resTemplate.intPk,
                                ref darrtyportempresjson_O, ref context);
                            }
                            else if (
                                //                          //There is a group.
                                ioentity != null &&
                                ioentity.intnGroupResourceId != null
                                )
                            {
                                List<GpresentityGroupResourceEntityDB> darrgpresentity = context.GroupResource.Where(
                                    gp => gp.intId == ioentity.intnGroupResourceId).ToList();

                                foreach (GpresentityGroupResourceEntityDB gpresentity in darrgpresentity)
                                {
                                    EleentityElementEntityDB eleentity = context.Element.FirstOrDefault(ele =>
                                        ele.intPk == gpresentity.intPkResource);
                                    if (
                                        eleentity.boolDeleted == false
                                        )
                                    {
                                        darreleentity.Add(eleentity);
                                    }
                                }
                            }
                            else if (
                               //                          //There is a resource set.
                               ioentity != null &&
                               ioentity.intnPkResource != null
                               )
                            {
                                EleentityElementEntityDB eleentity = context.Element.FirstOrDefault(ele =>
                                        ele.intPk == ioentity.intnPkResource);
                                if (
                                    eleentity.boolDeleted == false
                                    )
                                {
                                    darreleentity.Add(eleentity);
                                }
                            }
                            /*END-CASE*/
                        }
                        else if (
                           intnJobId_I == null
                           )
                        {
                            //                              //Get All resources from template and derivate template
                            //                              //    resources.
                            ResResource.subGetAllResourcesFromTemplateAndDerivateTempResources(resTemplate.intPk,
                            ref darrtyportempresjson_O, ref context);
                        }
                        /*END-CASE*/

                        if (
                            darreleentity.Count > 0
                            )
                        {
                            //                                      //Get the pks of dimensions attributes.
                            int[] arrintPkDimensionsAttribute = restyp.strXJDFTypeId == "Media" ?
                                ResResource.arrintPkDimensionsAttributeGet(restyp.intPk) : null;

                            foreach (EleentityElementEntityDB eleentity in darreleentity)
                            {
                                String strResourceName;

                                if (
                                    restyp.strXJDFTypeId == "Media" &&
                                    arrintPkDimensionsAttribute != null &&
                                    //                          //The dimension are considered how a Unit.
                                    arrintPkDimensionsAttribute.Length == 2
                                    )
                                {
                                    //strResourceName = ResResource.strGetMediaResourceName(eleentity,
                                    //arrintPkDimensionsAttribute);
                                    strResourceName = ResResource.strGetMediaResourceName(eleentity.intPk);
                                }
                                else
                                {
                                    //                                              //Set resource's name.
                                    strResourceName = eleentity.strElementName;
                                }

                                TyportempresjsonTypeOrTemplateResourceJson typeortempresjson = new
                                    TyportempresjsonTypeOrTemplateResourceJson(eleentity.intPk,
                                    strResourceName);

                                darrtyportempresjson_O.Add(typeortempresjson);
                            }
                            intStatus_IO = 200;
                            strUserMessage_IO = "Success.";
                            strDevMessage_IO = "";
                        }
                        else
                        {
                            intStatus_IO = 200;
                            strUserMessage_IO = "Success.";
                            strDevMessage_IO = "";
                        }
                    }
                }
            }
            darrtyportempresjson_O = darrtyportempresjson_O.OrderBy(typeortemp => typeortemp.strResourceName).ToList();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subGetAllResourcesFromTemplateAndDerivateTempResources(
            //                                              //Pk Template.
            int intPkTemplate_I,
            //                                              //ArrResources to add.
            ref List<TyportempresjsonTypeOrTemplateResourceJson> darrtyportempresjson_IO,
            //                                              //false, return resources.
            ref Odyssey2Context context_IO
            )
        {
            //                                              //Get all the resource from this template.
            List<EleentityElementEntityDB> darreleentityResources = context_IO.Element.Where(ele =>
                ele.intnPkElementInherited == intPkTemplate_I &&
                ele.boolIsTemplate == false).ToList();

            foreach (EleentityElementEntityDB eleentityResource in darreleentityResources)
            {
                String strResourceName = ResResource.strGetMediaResourceName(eleentityResource.intPk);

                TyportempresjsonTypeOrTemplateResourceJson typeortempresjson = new
                    TyportempresjsonTypeOrTemplateResourceJson(eleentityResource.intPk,
                    strResourceName);

                typeortempresjson.intPk = eleentityResource.intPk;
                //typeortempresjson.strResourceName = eleentityResource.strElementName;
                typeortempresjson.strResourceName = ResResource.strGetMediaResourceName(eleentityResource.intPk);

                //                                          //Add resources.
                darrtyportempresjson_IO.Add(typeortempresjson);
            }

            //                                              //Get all the Template from this template.
            List<EleentityElementEntityDB> darreleentityTemplate = context_IO.Element.Where(ele =>
                ele.intnPkElementInherited == intPkTemplate_I &&
                ele.boolIsTemplate == true).ToList();

            foreach (EleentityElementEntityDB eleentityTemplate in darreleentityTemplate)
            {
                ResResource.subGetAllResourcesFromTemplateAndDerivateTempResources(eleentityTemplate.intPk,
                    ref darrtyportempresjson_IO, ref context_IO);
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static List<EleentityElementEntityDB> GetResourcesDependsJobDate(
            JobentityJobEntityDB jobentity_I,
            RestypResourceType restyp_I
            )
        {
            Odyssey2Context context = new Odyssey2Context();
            List<EleentityElementEntityDB> darreleentityReturn = new List<EleentityElementEntityDB>();
            //                                              //Ztime object job's date.
            ZonedTime ztimeJobDate = ZonedTimeTools.NewZonedTime(jobentity_I.strStartDate.ParseToDate(),
                    jobentity_I.strStartTime.ParseToTime());

            //                                  //Get all resources from the type.
            IQueryable<EleentityElementEntityDB> seteleentityResource =
                context.Element.Where(ele => ele.intPkElementType == restyp_I.intPk &&
                ele.boolIsTemplate == false);
            List<EleentityElementEntityDB> darreleentity = seteleentityResource.ToList();

            if (
                darreleentity.Count() > 0
                )
            {
                foreach (EleentityElementEntityDB eleentityCheck in darreleentity)
                {
                    /*CASE*/
                    if (
                        eleentityCheck.strEndDate == null
                        )
                    {
                        darreleentityReturn.Add(eleentityCheck);

                    }
                    else if (
                       eleentityCheck.strEndDate != null
                       )
                    {
                        //                                  //Create ztime object.
                        ZonedTime ztimeStartDateRes = ZonedTimeTools.NewZonedTime(
                                eleentityCheck.strStartDate.ParseToDate(),
                                eleentityCheck.strStartTime.ParseToTime());
                        ZonedTime ztimeEndDateRes = ZonedTimeTools.NewZonedTime(
                                eleentityCheck.strEndDate.ParseToDate(),
                                eleentityCheck.strEndTime.ParseToTime());
                        if (
                            ztimeJobDate >= ztimeStartDateRes &&
                            ztimeJobDate < ztimeEndDateRes
                            )
                        {
                            darreleentityReturn.Add(eleentityCheck);
                        }
                    }
                }
            }

            return darreleentityReturn;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetData(
            int intPk_I,
            out Resjson1ResourceJson1 resjson1_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            resjson1_O = null;
            
            //                                              //Establish connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get the resource from db.
            EleentityElementEntityDB eleentityResource = context.Element.FirstOrDefault(ele => ele.intPk == intPk_I);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Resource not found.";
            if (
                eleentityResource != null
                )
            {
                //                                          //Get the type of the resource.
                EtentityElementTypeEntityDB etentityType = context.ElementType.FirstOrDefault(et =>
                    et.intPk == eleentityResource.intPkElementType);

                int? intnPkInherited = null;
                String strInheritedName = null;
                if (
                    eleentityResource.intnPkElementInherited != null
                    )
                {
                    EleentityElementEntityDB eleentityInherited = context.Element.FirstOrDefault(ele =>
                        ele.intPk == eleentityResource.intnPkElementInherited);

                    intnPkInherited = eleentityInherited.intPk;
                    strInheritedName = eleentityInherited.strElementName;
                }

                //                                          //Attributes for this type of resource.
                List<AttrentityAttributeEntityDB> darrattr =
                    (from attrentity in context.Attribute
                    join attretentity in context.AttributeElementType
                    on attrentity.intPk equals attretentity.intPkAttribute
                    where attretentity.intPkElementType == eleentityResource.intPkElementType
                    select attrentity).ToList();

                //                                          //Get the Name attribute.
                AttrentityAttributeEntityDB attrentityName = darrattr.FirstOrDefault(a =>
                    a.strXJDFName == "Name");
                int intPkAttributeName = attrentityName.intPk;

                //                                          //Get the Unit attribute.
                AttrentityAttributeEntityDB attrentityUnit = darrattr.FirstOrDefault(a =>
                    a.strXJDFName == "Unit");
                int intPkAttributeUnit = attrentityUnit.intPk;

                //                                          //Attributes with value for the resorce.
                List<ValentityValueEntityDB> darrvalentity = context.Value.Where(valentity =>
                    valentity.intPkElement == eleentityResource.intPk).ToList();

                //                                          //Get the pks attributes to block.
                List<int> darrintPkAttributesToBlock = new List<int>();
                ResResource.subGetAttributesToBlock(etentityType, ref darrintPkAttributesToBlock);

                String strUnit = "";
                bool? boolnIsDecimal = null;
                bool boolIsChangeable = true;
                List<Attrjson4AttributeJson4> darrattrjson4 = new List<Attrjson4AttributeJson4>();
                foreach (ValentityValueEntityDB valentity in darrvalentity)
                {
                    AscentityAscendantsEntityDB ascentity = context.Ascendants.FirstOrDefault(asc =>
                        (asc.strAscendants.EndsWith("" + Tools.charConditionSeparator + valentity.intPkAttribute) || 
                        asc.strAscendants == ("" + valentity.intPkAttribute)) && 
                        asc.intPkElement == eleentityResource.intPk);

                    //                                      //Block attributes, so they can not be edited.
                    bool boolIsBlocked = false;
                    if (
                        darrintPkAttributesToBlock != null &&
                        darrintPkAttributesToBlock.Exists(intPkattribute => intPkattribute == valentity.intPkAttribute)
                        )
                    {
                        boolIsBlocked = true;
                    }

                    Attrjson4AttributeJson4 attrjson4 = new Attrjson4AttributeJson4(valentity.intPk,
                        valentity.intnPkValueInherited, valentity.strValue,
                        Tools.arrintElementPk(ascentity.strAscendants), valentity.boolnIsChangeable,
                        boolIsBlocked);

                    if (
                        (valentity.intPkAttribute != intPkAttributeName) &&
                        (valentity.intPkAttribute != intPkAttributeUnit)
                        )
                    {
                        darrattrjson4.Add(attrjson4);
                    }

                    if (
                        valentity.intPkAttribute == intPkAttributeUnit
                        )
                    {
                        //                                  //Get the current unit of measurement.
                        ValentityValueEntityDB valentityCurrentUnit =
                                ResResource.GetResourceUnitOfMeasurement(valentity.intPkElement);
                        strUnit = valentityCurrentUnit.strValue;
                        boolIsChangeable = (valentityCurrentUnit.boolnIsChangeable != null) ?
                            (bool)valentityCurrentUnit.boolnIsChangeable : true;
                        boolnIsDecimal = valentityCurrentUnit.boolnIsDecimal;
                    }
                }

                bool boolIsPhysical = RestypResourceType.boolIsPhysical(etentityType.strClassification);

                InhedatajsonInheritanceDataJson inhedatajson;
                ResResource.subGetInheritanceData(intPk_I, out inhedatajson, ref intStatus_IO, ref strUserMessage_IO,
                        ref strDevMessage_IO);

                String strXJDFTypeId =
                    (etentityType.strXJDFTypeId == "None") ? etentityType.strCustomTypeId : etentityType.strXJDFTypeId;

                bool boolIsDecimal = (boolnIsDecimal == null) ? true : (bool)boolnIsDecimal;
                resjson1_O = new Resjson1ResourceJson1(etentityType.intPk, strXJDFTypeId, intnPkInherited,
                    strInheritedName, eleentityResource.intPk, eleentityResource.strElementName,
                    strUnit, eleentityResource.boolIsTemplate, darrattrjson4.ToArray(), boolIsPhysical,
                    boolIsChangeable, inhedatajson.unitinhe, inhedatajson.costinhe, inhedatajson.avainhe,
                    boolIsDecimal);

                intStatus_IO = 200;
                strUserMessage_IO = "Success.";
                strDevMessage_IO = "";
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void GetUnitsMeasurement(
            out List<String> darrstrUnitsMeasurement_O,
            ref int intStatus_IO,
            ref string strUserMessage_IO,
            ref string strDevMessage_IO
            )
        {
            darrstrUnitsMeasurement_O = new List<String>();

            Odyssey2Context context = new Odyssey2Context();

            IQueryable<String> setenumentitityUnits =
                from enumentity in context.Enumeration
                where enumentity.strEnumName == "NMUnits"
                select enumentity.strEnumValue;

            darrstrUnitsMeasurement_O = setenumentitityUnits.ToList();

            intStatus_IO = 200;
            strUserMessage_IO = "Success.";
            strDevMessage_IO = "Success.";

        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetCostAndUnitOfMeasurement(
            //                                              //Get the unit of measurement for a given resource. Validate
            //                                              //      that the resource is physical to obtain the unit.

            int intPkResource_I,
            out CostjsonCostJson costjson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Find resource.
            ResResource resResource = ResResource.resFromDB(intPkResource_I, false);
            //                                              //Find template.
            ResResource resTemplate = ResResource.resFromDB(intPkResource_I, true);
            //                                              //Make them one variable.
            ResResource resResOrTemp = resResource == null ? resTemplate : resResource;

            costjson_O = null;
            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Resource not found.";
            if (
                //                                          //Res or temp found.
                resResOrTemp != null
                )
            {
                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Resource not physical.";
                if (
                    //                                      //Physical resource.
                    RestypResourceType.boolIsPhysical(resResOrTemp.restypBelongsTo.strClassification)
                    )
                {
                    //                                      //Get the current unit of measurement.
                    ValentityValueEntityDB valentity = ResResource.GetResourceUnitOfMeasurement(resResOrTemp.intPk);

                    intStatus_IO = 403;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "Unit not found.";
                    if (
                        valentity != null
                        )
                    {
                        //                                  //To easy code.
                        double? numnQuantity = null;
                        double? numnCost = null;
                        double? numnMin = null;
                        double? numnBlock = null;
                        int? intnPkAccount = null;
                        double? numnHourlyRate = null;
                        bool? boolnArea = null;
                        String strDimensionUnit = "";

                        EtentityElementTypeEntityDB etentityTypeQFrom = context.ElementType.FirstOrDefault(et =>
                                    et.intPk == resResOrTemp.restypBelongsTo.intPk);

                        bool boolPaper = etentityTypeQFrom.strCustomTypeId == ResResource.strMedia;

                        if (
                            //                              //It is media or component resource.
                            boolPaper
                            )
                        {
                            strDimensionUnit = ResResource.strGetMediaResourceDimensionUnit(resResOrTemp.intPk);
                        }
                        
                        //                          //Get the cost.
                        CostentityCostEntityDB costentity = resResOrTemp.costentityCurrent;
                        if (
                            costentity != null
                            )
                        {
                            numnQuantity = costentity.numnQuantity;
                            numnCost = costentity.numnCost;
                            numnMin = costentity.numnMin;
                            numnBlock = costentity.numnBlock;
                            intnPkAccount = costentity.intPkAccount;
                            numnHourlyRate = costentity.numnHourlyRate;
                            boolnArea = costentity.boolnArea;
                        }

                        //                                  //Json to return.
                        costjson_O = new CostjsonCostJson(resResOrTemp.intPk, valentity.strValue, numnQuantity, 
                            numnCost, numnMin, numnBlock, intnPkAccount, numnHourlyRate, boolnArea,
                            strDimensionUnit, boolPaper);

                        intStatus_IO = 200;
                        strUserMessage_IO = "Success.";
                        strDevMessage_IO = "";
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        private static String strGetMediaResourceDimensionUnit(
            //                                              //Get the dimensionUnit for a media resource if has.

            int intPkResource_I
            )
        {
            String strMediaResDimUnit = null;

            Odyssey2Context context = new Odyssey2Context();
            //                                              //Get the resource's type.
            EtentityElementTypeEntityDB etentity = (from element in context.Element
                                                    join elementtype in context.ElementType on
                                                    element.intPkElementType equals elementtype.intPk
                                                    where element.intPk == intPkResource_I
                                                    select elementtype).FirstOrDefault();

            List<ValentityValueEntityDB> darrvalentity = context.Value.Where(val =>
                val.intPkElement == intPkResource_I).ToList();

            //                                      //Get the pks of dimensions attributes.
            int[] arrintPkDimensionsAttribute = ResResource.arrintPkDimensionsAttributeGet(etentity.intPk);

            //                                      //Find the first attribute.
            ValentityValueEntityDB valentityWidth = darrvalentity.FirstOrDefault(val =>
            val.intPkAttribute == arrintPkDimensionsAttribute[0]);

            //                                      //Find the second attribute.
            ValentityValueEntityDB valentityLength = darrvalentity.FirstOrDefault(val =>
            val.intPkAttribute == arrintPkDimensionsAttribute[1]);

            //                                      //Find the third attribute.
            ValentityValueEntityDB valentityDimUnit = darrvalentity.FirstOrDefault(val =>
            val.intPkAttribute == arrintPkDimensionsAttribute[2]);

            if (
                valentityWidth != null &&
                valentityLength != null &&
                valentityDimUnit != null
                )
            {
                strMediaResDimUnit = valentityDimUnit.strValue;
            }

            return strMediaResDimUnit;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetTimeAndUnitOfMeasurement(
            //                                              //Get the unit of measurement for a given resource. Validate
            //                                              //      that the resource is physical to obtain the unit.

            int intPkTime_I,
            out TimejsonTimeJson timejson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Establish the connection.
            Odyssey2Context context = new Odyssey2Context();            

            //                                              //Validate pktime
            TimeentityTimeEntityDB timeentity = context.Time.FirstOrDefault(time => time.intPk == intPkTime_I);

            timejson_O = null;
            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "PkTime not valid or time had been closed.";
            if (
                //                                          //Invalid pk time
                timeentity != null &&
                timeentity.strEndDate == null
                )
            {
                ResResource res = ResResource.resFromDB(timeentity.intPkResource, false);

                //                                  //Get the current unit of measurement.
                ValentityValueEntityDB valentity = ResResource.GetResourceUnitOfMeasurement(res.intPk);

                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Unit not found.";
                if (
                    valentity != null
                    )
                {
                    timejson_O = new TimejsonTimeJson(valentity.strValue, timeentity.numQuantity, timeentity.intHours,
                            timeentity.intMinutes, timeentity.intSeconds, timeentity.numnMinThickness,
                            timeentity.numnMaxThickness, timeentity.strThicknessUnit, timeentity.intPk);

                    intStatus_IO = 200;
                    strUserMessage_IO = "Success.";
                    strDevMessage_IO = "";
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static ValentityValueEntityDB GetResourceUnitOfMeasurement(
            //                                              //Get the current unit of measurement.
            int intPkResource_I
            )
        {
            ValentityValueEntityDB valentity = null;

            //                                              //Establish the connection.
            Odyssey2Context context = new Odyssey2Context();

            EleentityElementEntityDB eleentityResource = context.Element.FirstOrDefault(ele => ele.intPk == intPkResource_I);

            int intPkElementType = EtElementTypeAbstract.etFromDB(eleentityResource.intPkElementType).intPk;

            //                                              //Get the unit attribute.
            IQueryable<AttrentityAttributeEntityDB> setattr =
                from attrentity in context.Attribute
                join attretentity in context.AttributeElementType
                on attrentity.intPk equals attretentity.intPkAttribute
                where attretentity.intPkElementType == intPkElementType
                select attrentity;
            List<AttrentityAttributeEntityDB> darrattr = setattr.ToList();

            //                                              //Get the Unit attribute.
            AttrentityAttributeEntityDB attrentityUnit = darrattr.FirstOrDefault(a =>
                a.strXJDFName == "Unit" || a.strCustomName == "Unit");

            if (
                attrentityUnit != null
                )
            {
                //                                              //Get all the units for the resource to update.
                IQueryable<ValentityValueEntityDB> setvalEntity = context.Value.Where(val => val.intPkAttribute ==
                    attrentityUnit.intPk && val.intPkElement == intPkResource_I);
                List<ValentityValueEntityDB> darrvalentity = setvalEntity.ToList();

                if (
                    //                                          //There is only one register.
                    darrvalentity.Count() >= 1
                    )
                {
                    darrvalentity.Sort();
                    //                                          //Get the last unit of measurement.
                    valentity = darrvalentity.Last();
                }
            }
            return valentity;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetTimes(
            //                                              //Get all times asociated to an specific resource.

            int intPkResource_I,
            out List<TimejsonTimeJson> darrtimejson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            darrtimejson_O = new List<TimejsonTimeJson>();
            ResResource res = ResResource.resFromDB(intPkResource_I, false);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Resource not found.";
            if (
                //                                          //Res found.
                res != null
                )
            {
                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Resource not physical.";
                if (
                    //                                      //Physical resource.
                    RestypResourceType.boolIsPhysical(res.restypBelongsTo.strClassification)
                    )
                {
                    intStatus_IO = 403;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "Not calendar.";
                    if (
                        res.boolnIsCalendar == true
                        )
                    {
                        Odyssey2Context context = new Odyssey2Context();

                        //                                  //Get the current unit of measurement.
                        ValentityValueEntityDB valentity = ResResource.GetResourceUnitOfMeasurement(res.intPk);

                        intStatus_IO = 404;
                        strUserMessage_IO = "Something is wrong.";
                        strDevMessage_IO = "Unit not found.";
                        if (
                            valentity != null
                            )
                        {
                            List<TimeentityTimeEntityDB> darrtimeentity = context.Time.Where(time =>
                                time.intPkResource == res.intPk && time.strEndDate == null).ToList();

                            foreach(TimeentityTimeEntityDB timeentity in darrtimeentity)
                            {
                                TimejsonTimeJson timejson = new TimejsonTimeJson(valentity.strValue,
                                    timeentity.numQuantity, timeentity.intHours, timeentity.intMinutes,
                                    timeentity.intSeconds, timeentity.numnMinThickness, timeentity.numnMaxThickness,
                                    timeentity.strThicknessUnit, timeentity.intPk);
                                darrtimejson_O.Add(timejson);
                            }

                            intStatus_IO = 200;
                            strUserMessage_IO = "Success.";
                            strDevMessage_IO = "";
                        }
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static RuljsonRuleJson[] arrruljsonGetRules(
            //                                              //Get rules from a resource, printshop or employee.
            int? intnPkResource_I,
            PsPrintShop ps_I,
            bool boolIsEmployee_I,
            int? intnContactId_I,
            IConfiguration configuration_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Conection to DB.
            Odyssey2Context context = new Odyssey2Context();

            List<RuljsonRuleJson> darrruljson = new List<RuljsonRuleJson>();
            List<RuleentityRuleEntityDB> darrruleentity = null;
            /*CASE*/
            if (
                //                                          //Resource's rules.
                intnPkResource_I != null
                )
            {
                //                                          //Get resource.
                ResResource res = ResResource.resFromDB(intnPkResource_I, false);
                intStatus_IO = 401;
                strUserMessage_IO = "";
                strDevMessage_IO = "Resource not found.";
                if (
                    res != null
                    )
                {
                    //                                      //Get all rules of a resource.
                    darrruleentity = context.Rule.Where(rule => rule.intnPkResource == intnPkResource_I).ToList();
                    intStatus_IO = 402;
                    strUserMessage_IO = "";
                    strDevMessage_IO = "No resource´s rules found.";
                    if (
                        darrruleentity != null && darrruleentity.Count() > 0
                        )
                    {
                        darrruljson = ResResource.getdarrruljson(ps_I.strTimeZone, darrruleentity, ref intStatus_IO,
                                ref strUserMessage_IO, ref strDevMessage_IO);
                    }
                }
            }
            else if (
                //                                          //Printshop's rules.
                intnPkResource_I == null &&
                !boolIsEmployee_I
                )
            {
                //                                          //Get all rules of a printshop.
                darrruleentity = context.Rule.Where(rule => rule.intnPkPrintshop == ps_I.intPk).ToList();
                intStatus_IO = 403;
                strUserMessage_IO = "";
                strDevMessage_IO = "No printshop´s rules found.";
                if (
                    darrruleentity != null && darrruleentity.Count() > 0
                    )
                {
                    darrruljson = ResResource.getdarrruljson(ps_I.strTimeZone, darrruleentity, ref intStatus_IO,
                                ref strUserMessage_IO, ref strDevMessage_IO);
                }
            }
            else if (
                //                                          //Employee's rules.
                boolIsEmployee_I &&
                intnContactId_I > 0
                )
            {
                intStatus_IO = 404;
                strUserMessage_IO = "";
                strDevMessage_IO = "Employee not valid.";
                if (
                    ResResource.boolEmployeeIsFromPrintshop(ps_I.strPrintshopId, (int)intnContactId_I)
                    )
                {
                    //                                      //Get all rules of a employee.
                    darrruleentity = context.Rule.Where(rule => rule.intnContactId == intnContactId_I).ToList();
                    intStatus_IO = 405;
                    strUserMessage_IO = "";
                    strDevMessage_IO = "No employee´s rules found.";
                    if (
                        darrruleentity != null && darrruleentity.Count() > 0
                        )
                    {
                        darrruljson = ResResource.getdarrruljson(ps_I.strTimeZone, darrruleentity, ref intStatus_IO,
                                    ref strUserMessage_IO, ref strDevMessage_IO);
                    }
                }
            }
            return darrruljson.ToArray();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static List<RuljsonRuleJson> getdarrruljson(
            String strTimeZoneId_I,
            List<RuleentityRuleEntityDB> darrruleentity_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            List<RuljsonRuleJson> darrruljson = new List<RuljsonRuleJson>();

            String strFrecuencyValues = "";
            List<String> darrFrecuencyWeekly = new List<string>();
            List<String> darrFrecuencyMonthly = new List<string>();
            foreach (RuleentityRuleEntityDB ruleentity in darrruleentity_I)
            {
                RuljsonRuleJson ruljson;
                if (
                    //                                  //Frecuency Once.
                    ruleentity.strFrecuency == ResResource.strOnce
                    )
                {
                    ZonedTime ztimeStartRule = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                        ResResource.getStrDateStartOrEnd(true, ruleentity.strFrecuencyValue).ParseToDate(),
                        ruleentity.strStartTime.ParseToTime(), strTimeZoneId_I);

                    ZonedTime ztimeEndRule = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                        ResResource.getStrDateStartOrEnd(false, ruleentity.strFrecuencyValue).ParseToDate(),
                        ruleentity.strEndTime.ParseToTime(), strTimeZoneId_I);

                    strFrecuencyValues = "-";
                    String strStartDateAndHour = ztimeStartRule.Date + " " + ztimeStartRule.Time;
                    String strEndDateAndHour = ztimeEndRule.Date + " " + ztimeEndRule.Time;

                    ruljson = new RuljsonRuleJson(ruleentity.intPk, ruleentity.strFrecuency,
                        strFrecuencyValues, strStartDateAndHour, strEndDateAndHour, ruleentity.intnPkResource,
                        null, null, null, null);
                }
                else
                {
                    /*CASE*/
                    if (
                        //                              //Frecuency daily.
                        ruleentity.strFrecuency == ResResource.strDaily
                        )
                    {
                        strFrecuencyValues = "-";
                    }
                    else if (
                        //                              //Frecuency Weekly.
                        ruleentity.strFrecuency == ResResource.strWeekly
                        )
                    {
                        darrFrecuencyWeekly = ResResource.darrstrGetDailyWeekly(ruleentity.strFrecuencyValue);
                        strFrecuencyValues = ResResource.strGetFrecuencyBuildedWithPreposition(darrFrecuencyWeekly);
                    }
                    else if (
                        //                              //Frecuency Monthly.
                        ruleentity.strFrecuency == ResResource.strMonthly
                        )
                    {
                        darrFrecuencyMonthly = ResResource.darrstrGetDailyMonthly(ruleentity.strFrecuencyValue);
                        strFrecuencyValues = ResResource.strGetFrecuencyBuildedWithPreposition(darrFrecuencyMonthly);
                    }
                    else if (
                        //                              //Frecuency Annually.
                        ruleentity.strFrecuency == ResResource.strAnnually
                        )
                    {
                        strFrecuencyValues = ResResource.strGetDayAnnually(ruleentity.strFrecuencyValue);
                    }
                    /*END-CASE*/

                    ZonedTime ztimeStartRule = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                        ruleentity.strRangeStartDate.ParseToDate(), ruleentity.strRangeStartTime.ParseToTime(), 
                        strTimeZoneId_I);

                    ZonedTime ztimeEndRule = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                        ruleentity.strRangeEndDate.ParseToDate(), ruleentity.strRangeEndTime.ParseToTime(), 
                        strTimeZoneId_I);

                    String strRangeEndDate = ztimeEndRule.Date.ToString();
                    String strRangeEndTime = ztimeEndRule.Time.ToString();
                    if (
                        strRangeEndDate == "9999-12-31"
                        )
                    {
                        strRangeEndDate = "-";
                        strRangeEndTime = "-";
                    }

                    ruljson = new RuljsonRuleJson(ruleentity.intPk, ruleentity.strFrecuency,
                        strFrecuencyValues, ruleentity.strStartTime, ruleentity.strEndTime,
                        ruleentity.intnPkResource, ztimeStartRule.Date.ToString(), ztimeStartRule.Time.ToString(),
                        strRangeEndDate, strRangeEndTime);
                }
                //                                      //Add Rule to list rules from resource.
                darrruljson.Add(ruljson);

                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "";
            }
            return darrruljson;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String getStrDateStartOrEnd(
            //                                              //If boolIsStarDate_I == true, get startDate.
            //                                              //If boolIsStarDate_I == false, get endDate.
            bool boolIsStartDate_I,
            //                                              //Date complex with next format -> 2020-04-28|2020-04-28
            String strDateStartAndEndComplex_I
            )
        {
            String strDate = "";
            if (
                //                                          //startDate.
                boolIsStartDate_I
                )
            {
                strDate = strDateStartAndEndComplex_I.Substring(0, 10);
            }
            else
            {
                //                                          //endDate.
                strDate = strDateStartAndEndComplex_I.Substring(11, 10);
            }

            return strDate;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static List<String> darrstrGetDailyWeekly(
            //                                              //The length should be 7 characters(char is 0 or 1).
            String strFrecuencyValue_I
            )
        {
            List<String> darrstrDaysWeekly = new List<string>();
            int intI = 0;

            /*REPEAT-WHILE*/
            while (intI < strFrecuencyValue_I.Length)
            {
                if (
                    strFrecuencyValue_I.ElementAt(intI) == '1'
                    )
                {
                    //                                      //Get Day name.
                    darrstrDaysWeekly.Add(ResResource.arrstrWeekdays[intI]);
                }
                intI = intI + 1;
            }
            return darrstrDaysWeekly;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static List<String> darrstrGetDailyMonthly(
            //                                              //The length should be 31 characters(char is 0 or 1).
            String strFrecuencyValue_I
            )
        {
            List<String> darrstrDaysMonthly = new List<string>();
            int intI = 0;

            /*REPEAT-WHILE*/
            while (intI < strFrecuencyValue_I.Length)
            {
                if (
                    strFrecuencyValue_I.ElementAt(intI) == '1'
                    )
                {
                    //                                      //Get Day number.
                    darrstrDaysMonthly.Add(intI + 1 + "");
                }
                intI = intI + 1;
            }
            return darrstrDaysMonthly;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String strGetDayAnnually(
            //                                              //The length should be 4 characters(first pair is the month).
            //                                              //    and second pair is the day).
            String strFrecuencyValue_I
            )
        {
            String strFrecuencyAnnually = "";
            //                                              //Get month and day annually.
            int intMonth = strFrecuencyValue_I.Substring(0, 2).ParseToInt();
            int intDay = strFrecuencyValue_I.Substring(2, 2).ParseToInt();

            if (
                intMonth > 0 &&
                intDay > 0
                )
            {
                strFrecuencyAnnually = arrstrMonths[intMonth - 1] + ", " + intDay + ".";
            }
            return strFrecuencyAnnually;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String strGetFrecuencyBuildedWithPreposition(
            //                                              //The length should be 4 characters(first pair is the month).
            //                                              //    and second pair is the day).
            List<String> darrFrecuency_I
            )
        {
            String strFrecuencyValue = "";
            if (
                //                                          //arr has not element-.
                darrFrecuency_I.Count == 0
                )
            {
                //                                          //Not has rules of date.
            }
            else if (
                //                                          //Arr Only has one element.
                darrFrecuency_I.Count == 1
                )
            {
                strFrecuencyValue = darrFrecuency_I[0] + ".";
            }
            else
            {
                //                                          //arr has more of one element.
                int intI = 0;
                int intLengthArrFrecuency = darrFrecuency_I.Count;
                /*REPEAT-WHILE*/
                while (intI < intLengthArrFrecuency)
                {
                    if (
                        //                                  //First element.
                        intI == 0
                        )
                    {
                        strFrecuencyValue = darrFrecuency_I[0];
                    }
                    else if (
                        //                                  //Middle elements
                        intI < (intLengthArrFrecuency - 1)
                        )
                    {
                        strFrecuencyValue = strFrecuencyValue + ", " + darrFrecuency_I[intI];
                    }
                    else
                    {
                        //                                  //The last element.
                        strFrecuencyValue = strFrecuencyValue + " and " + darrFrecuency_I[intI] + ".";
                    }
                    intI = intI + 1;
                }
            }
            return strFrecuencyValue;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetPeriod(
            //                                              //Method to get a specif period.

            int intPkPeriod_I,
            PsPrintShop ps_I,
            out PerjsonPeriodJson perjson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            perjson_O = null;
            //                                              //Establish the conection with tha database.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get period.
            PerentityPeriodEntityDB perentity = context.Period.FirstOrDefault(per => per.intPk == intPkPeriod_I);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Period not found.";
            if (
                //                                          //If period exists.
                perentity != null
                )
            {
                //                                          //Get strJobNumber.
                String strJobNumber = JobJob.strGetJobNumber(null, perentity.intJobId, ps_I.strPrintshopId, context);
                //                                          //Create json to back.
                perjson_O = new PerjsonPeriodJson(
                    perentity.intPk,
                    perentity.strStartDate,
                    perentity.strStartTime,
                    perentity.strEndDate,
                    perentity.strEndTime,
                    perentity.intJobId,
                    strJobNumber,
                    perentity.intPkElement,
                    perentity.intnContactId,
                    perentity.intMinsBeforeDelete
                    );

                intStatus_IO = 200;
                strUserMessage_IO = "Information retrieved. Please make your changes.";
                strDevMessage_IO = "";
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetWeek(
            int intPkResource_I,
            String strDate_I,
            String strPrintshopId_I,
            IConfiguration configuration_I,
            out DayjsonDayJson[] arrdayjson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            arrdayjson_O = new DayjsonDayJson[7];

            ResResource res = ResResource.resFromDB(intPkResource_I, false);
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Resource not found.";
            if (
                res != null
                )
            {
                //                                              //To easy code.
                Date date = strDate_I.ParseToDate();

                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Date is not from a Sunday.";
                if (
                    (int)date.DayOfWeek == 0
                    )
                {
                    //                                              //Establish the connection.
                    Odyssey2Context context = new Odyssey2Context();

                    //                                              //Get all jobs.
                    List<JobjsonJobJson> darrjobjsonJobOfAPrintshopFromWisnet = 
                        JobJob.darrjobjsonOfAPrintshopFromWisnet(ps.strPrintshopId, configuration_I);

                    date = date - 1;

                    for (int intI = 0; intI < 7; intI = intI + 1)
                    {
                        date = date + 1;

                        List<PorjsonPeriodOrRuleJson> darrporjson = new List<PorjsonPeriodOrRuleJson>();

                        ResResource.subAddPeriods(strPrintshopId_I, ps.strTimeZone, res, date, 
                            darrjobjsonJobOfAPrintshopFromWisnet, ref darrporjson);

                        ResResource.subAddRules(res, date, intI, ps, ref darrporjson);

                        //                                  //This verified if each period is overlap with a rule.
                        ResResource.subVerifyPeriodIsOverlapWhitARule(date, ref darrporjson, strPrintshopId_I);

                        PorjsonPeriodOrRuleJson[] arrporjson = darrporjson.ToArray();
                        Array.Sort(arrporjson);

                        arrdayjson_O[intI] = new DayjsonDayJson(ResResource.arrstrWeekdays[intI], date.ToText(),
                            arrporjson);

                        intStatus_IO = 200;
                        strUserMessage_IO = "Success.";
                        strDevMessage_IO = "";
                    }
                }
            }

        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetDay(
            int intPkResource_I,
            String strDate_I,
            String strPrintshopId_I,
            IConfiguration configuration_I,
            out DayjsonDayJson arrdayjson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            arrdayjson_O = null;

            ResResource res = ResResource.resFromDB(intPkResource_I, false);
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Resource not found.";
            if (
                res != null
                )
            {
                //                                          //To easy code.
                Date date = strDate_I.ParseToDate();

                //                                          //Day of week.
                int intDayOfWeek = (int)date.DayOfWeek;

                //                                          //Establish the connection.
                Odyssey2Context context = new Odyssey2Context();

                List<PorjsonPeriodOrRuleJson> darrporjson = new List<PorjsonPeriodOrRuleJson>();

                //                                          //Get all jobs.
                List<JobjsonJobJson> darrjobjsonJobOfAPrintshopFromWisnet =
                    JobJob.darrjobjsonOfAPrintshopFromWisnet(ps.strPrintshopId, configuration_I);

                ResResource.subAddPeriods(strPrintshopId_I, ps.strTimeZone, res, date, 
                    darrjobjsonJobOfAPrintshopFromWisnet, ref darrporjson);

                ResResource.subAddRules(res, date, intDayOfWeek, ps, ref darrporjson);

                //                                          //This verified if each period is overlap with a rule.
                ResResource.subVerifyPeriodIsOverlapWhitARule(date, ref darrporjson, strPrintshopId_I);

                //                                          //Order time darr for proper operation for the next method.
                darrporjson.Sort();
                //                                          //Create empty time available for .
                ResResource.subAddTimeAvailable(ref darrporjson, strDate_I);

                PorjsonPeriodOrRuleJson[] arrporjson = darrporjson.ToArray();

                arrdayjson_O = new DayjsonDayJson(ResResource.arrstrWeekdays[intDayOfWeek],
                    date.ToText(), arrporjson);

                intStatus_IO = 200;
                strUserMessage_IO = "Success.";
                strDevMessage_IO = "";
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subAddPeriods(
            String strPrintshopId_I,
            String strTimeZoneId_I,
            ResResource res_I,
            Date date_I,
            List<JobjsonJobJson> darrjobjsonJobOfAPrintshopFromWisnet_I,
            ref List<PorjsonPeriodOrRuleJson> darrporjson_M
            )
        {
            //                                              //Establish the connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get the periods excluding temporary ones.
            IQueryable<PerentityPeriodEntityDB> setperentity = context.Period.Where(per =>
                per.intPkElement == res_I.intPk && per.intnEstimateId == null);
            List<PerentityPeriodEntityDB> darrperentity = setperentity.ToList();

            //                                              //To easy code.
            ZonedTime ztimeStartDate = ZonedTimeTools.NewZonedTime(date_I, Time.MinValue);
            ZonedTime ztimeEndDate = ZonedTimeTools.NewZonedTime(date_I, Time.MaxValue);

            List<ContactjsonContactJson> darrcontactjson = ResResource.darrcontactjsonGetAllEmployee(strPrintshopId_I);

            //                                              //Add the periods to the list.
            foreach (PerentityPeriodEntityDB perentity in darrperentity)
            {
                //                                          //To easy code.
                Date dateStartPeriod = perentity.strStartDate.ParseToDate();
                Date dateEndPeriod = perentity.strEndDate.ParseToDate();
                Time timeStartPeriod = perentity.strStartTime.ParseToTime();
                Time timeEndPeriod = perentity.strEndTime.ParseToTime();

                //                                          //Period already is realized in a Job.
                bool boolIsPeriodDone = perentity.strFinalEndDate != null ? true : false;

                //                                          //Period has been started.
                bool boolPeriodStarted = (perentity.strFinalStartDate != null &&
                    perentity.strFinalEndDate == null) ? true : false;

                ZonedTime ztimeStartPeriod = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                    dateStartPeriod, timeStartPeriod, strTimeZoneId_I);
                ZonedTime ztimeEndPeriod = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                    dateEndPeriod, timeEndPeriod, strTimeZoneId_I);

                String strFirstName = null;
                String strLastName = null;
                if (
                    perentity.intnContactId != null
                    )
                {
                    ResResource.subGetFirstAndLastNameOfEmployee(strPrintshopId_I, (int)perentity.intnContactId, 
                        darrcontactjson, ref strFirstName, ref strLastName);
                }

                PiwentityProcessInWorkflowEntityDB piwentity = context.ProcessInWorkflow.FirstOrDefault(piw =>
                piw.intPkWorkflow == perentity.intPkWorkflow &&
                piw.intProcessInWorkflowId == perentity.intProcessInWorkflowId);

                ProProcess pro = ProProcess.proFromDB(piwentity.intPkProcess);
                String strProcess = pro.strName + ((piwentity.intnId != null) ? ("(" + piwentity.intnId + ")") : "");

                JobjsonJobJson jobjson = darrjobjsonJobOfAPrintshopFromWisnet_I.FirstOrDefault(Job =>
                Job.intJobId == perentity.intJobId);
                String strJobName = (jobjson == null) ? "" : jobjson.strJobTicket;

                ResResource.subAddAPeriodOrOnceRule(ztimeStartDate, ztimeEndDate, ztimeStartPeriod, ztimeEndPeriod,
                    perentity.intPk, strFirstName, strLastName, perentity.intJobId + "", strProcess, strJobName, 
                    perentity.intMinsBeforeDelete, boolIsPeriodDone, boolPeriodStarted, strPrintshopId_I,
                    ref darrporjson_M);
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subAddAPeriodOrOnceRule(
            ZonedTime ztimeStartDate_I,
            ZonedTime ztimeEndDate_I,
            ZonedTime ztimeStartPeriodOrOnceRule_I,
            ZonedTime ztimeEndPeriodOrOnceRule_I,
            int? intnIntPkPeriod_I,
            String strFirstName_I,
            String strLastName_I,
            String strJobId_I,
            String strProcess_I,
            String strJobName_I,
            int intMinsBeforeDelete_I,
            bool? boolnIsPeriodDone_I,
            bool boolPeriodStarted_I,
            String strPrintshopId_I,
            ref List<PorjsonPeriodOrRuleJson> darrporjson_M
            )
        {
            //                                              //Establish the connection.
            Odyssey2Context context = new Odyssey2Context();

            String strJobNumber = null;
            if (
                strJobId_I != null && strJobId_I != ""
                )
            {
                //                                          //Get strJobNumber.
                strJobNumber = JobJob.strGetJobNumber(null, strJobId_I.ParseToInt(),
                                strPrintshopId_I, context);
            }
            //                                              //To easy code.
            //String strTimeMax = "24:00:00";
            String strTimeMax = Time.MaxValue.ToString(); 

            /*CASE*/
            if (
                //                                          //The period is completely in the day.
                (ztimeStartPeriodOrOnceRule_I >= ztimeStartDate_I) &&
                (ztimeEndPeriodOrOnceRule_I <= ztimeEndDate_I)
                )
            {
                PorjsonPeriodOrRuleJson porjson = new PorjsonPeriodOrRuleJson(intnIntPkPeriod_I,
                    ztimeStartPeriodOrOnceRule_I.Time.ToString(), ztimeEndPeriodOrOnceRule_I.Time.ToString(),
                    strJobId_I, strJobNumber, false, strFirstName_I, strLastName_I, strProcess_I, strJobName_I,
                    intMinsBeforeDelete_I, boolnIsPeriodDone_I, boolPeriodStarted_I);
                darrporjson_M.Add(porjson);
            }
            else if (
                //                                          //The period is bigger than the day and contains the day.
                (ztimeStartPeriodOrOnceRule_I < ztimeStartDate_I) &&
                (ztimeEndPeriodOrOnceRule_I > ztimeEndDate_I)
                )
            {
                PorjsonPeriodOrRuleJson porjson = new PorjsonPeriodOrRuleJson(intnIntPkPeriod_I,
                    Time.MinValue.ToString(), strTimeMax, strJobId_I, strJobNumber, false, strFirstName_I,
                    strLastName_I, strProcess_I, strJobName_I, intMinsBeforeDelete_I, boolnIsPeriodDone_I,
                    boolPeriodStarted_I);
                darrporjson_M.Add(porjson);
            }
            else if (
                //                                          //The period starts a day before and ends in this day.
                (ztimeEndPeriodOrOnceRule_I > ztimeStartDate_I) &&
                (ztimeEndPeriodOrOnceRule_I <= ztimeEndDate_I)
                )
            {
                PorjsonPeriodOrRuleJson porjson = new PorjsonPeriodOrRuleJson(intnIntPkPeriod_I,
                    Time.MinValue.ToString(), ztimeEndPeriodOrOnceRule_I.Time.ToString(), strJobId_I, strJobNumber,
                    false, strFirstName_I, strLastName_I, strProcess_I, strJobName_I, intMinsBeforeDelete_I,
                    boolnIsPeriodDone_I, boolPeriodStarted_I);
                darrporjson_M.Add(porjson);
            }
            else if (
                //                                          //The period starts in this day  and ends in a day after.
                (ztimeStartPeriodOrOnceRule_I >= ztimeStartDate_I) &&
                (ztimeStartPeriodOrOnceRule_I <= ztimeEndDate_I)
                )
            {
                PorjsonPeriodOrRuleJson porjson = new PorjsonPeriodOrRuleJson(intnIntPkPeriod_I,
                    ztimeStartPeriodOrOnceRule_I.Time.ToString(), strTimeMax, strJobId_I, strJobNumber, false,
                    strFirstName_I, strLastName_I, strProcess_I, strJobName_I, intMinsBeforeDelete_I,
                    boolnIsPeriodDone_I, boolPeriodStarted_I);
                darrporjson_M.Add(porjson);
            }
            /*END-CASE*/
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subAddRules(
            ResResource res_I,
            Date date_I,
            int intDay_I,
            PsPrintShop ps_I,
            ref List<PorjsonPeriodOrRuleJson> darrporjson_M
            )
        {
            //                                              //Establish the connection. 
            Odyssey2Context context = new Odyssey2Context();

            String strDate = date_I.ToText();

            //                                              //Get the daily rules. (resource and printshop)
            IQueryable<RuleentityRuleEntityDB> setruleentityDaily = context.Rule.Where(rule =>
                rule.intnPkResource == res_I.intPk && rule.strFrecuency == ResResource.strDaily);
            List<RuleentityRuleEntityDB> darrruleentity = setruleentityDaily.ToList();

            //                                              //Get daily printshop's rules.
            IQueryable<RuleentityRuleEntityDB> setruleentityPrintshopDaily = context.Rule.Where(rule =>
                rule.intnPkPrintshop == ps_I.intPk && rule.strFrecuency == ResResource.strDaily);
            List<RuleentityRuleEntityDB> darrruleentityPrintshop = setruleentityPrintshopDaily.ToList();
            foreach (RuleentityRuleEntityDB rulePrintshop in darrruleentityPrintshop)
            {
                darrruleentity.Add(rulePrintshop);
            }

            //                                              //Get the weekly rules.
            IQueryable<RuleentityRuleEntityDB> setruleentityWeekly = context.Rule.Where(rule =>
                rule.intnPkResource == res_I.intPk && rule.strFrecuency == ResResource.strWeekly);
            List<RuleentityRuleEntityDB> darrruleentityWeekly = setruleentityWeekly.ToList();

            //                                              //Get weekly printshop's rules.
            IQueryable<RuleentityRuleEntityDB> setruleentityPrintshopWeekly = context.Rule.Where(rule =>
                rule.intnPkPrintshop == ps_I.intPk && rule.strFrecuency == ResResource.strWeekly);
            List<RuleentityRuleEntityDB> darrruleentityPrintshopWeekly = setruleentityPrintshopWeekly.ToList();

            foreach (RuleentityRuleEntityDB rulePrintshop in darrruleentityPrintshopWeekly)
            {
                //                                          //Add each weekly printshop rule to list.
                darrruleentityWeekly.Add(rulePrintshop);
            }

            foreach (RuleentityRuleEntityDB ruleentity in darrruleentityWeekly)
            {
                if (
                    ruleentity.strFrecuencyValue[intDay_I] == '1'
                    )
                {
                    darrruleentity.Add(ruleentity);
                }
            }

            //                                              //Get the monthly rules.
            IQueryable<RuleentityRuleEntityDB> setruleentityMonthly = context.Rule.Where(rule =>
                rule.intnPkResource == res_I.intPk && rule.strFrecuency == ResResource.strMonthly);
            List<RuleentityRuleEntityDB> darrruleentityMonthly = setruleentityMonthly.ToList();

            //                                              //Get monthly printshop's rules.
            IQueryable<RuleentityRuleEntityDB> setruleentityPrintshopMonthly = context.Rule.Where(rule =>
                rule.intnPkPrintshop == ps_I.intPk && rule.strFrecuency == ResResource.strMonthly);
            List<RuleentityRuleEntityDB> darrruleentityPrintshopMonthly = setruleentityPrintshopMonthly.ToList();

            foreach (RuleentityRuleEntityDB rulePrintshop in darrruleentityPrintshopMonthly)
            {
                //                                          //Add each monthly printshop rule to list.
                darrruleentityMonthly.Add(rulePrintshop);
            }

            foreach (RuleentityRuleEntityDB ruleentity in darrruleentityMonthly)
            {
                if (
                    ruleentity.strFrecuencyValue[date_I.Day - 1] == '1'
                    )
                {
                    darrruleentity.Add(ruleentity);
                }
            }

            //                                              //Get the annualy rules.
            IQueryable<RuleentityRuleEntityDB> setruleentityAnnually = context.Rule.Where(rule =>
                rule.intnPkResource == res_I.intPk && rule.strFrecuency == ResResource.strAnnually);
            List<RuleentityRuleEntityDB> darrruleentityAnnually = setruleentityAnnually.ToList();

            //                                              //Get annualy printshop's rules.
            IQueryable<RuleentityRuleEntityDB> setruleentityPrintshopAnnually = context.Rule.Where(rule =>
                rule.intnPkPrintshop == ps_I.intPk && rule.strFrecuency == ResResource.strAnnually);
            List<RuleentityRuleEntityDB> darrruleentityPrintshopAnnually = setruleentityPrintshopAnnually.ToList();

            foreach (RuleentityRuleEntityDB rulePrintshop in darrruleentityPrintshopAnnually)
            {
                //                                          //Add each annualy printshop rule to list.
                darrruleentityAnnually.Add(rulePrintshop);
            }

            foreach (RuleentityRuleEntityDB ruleentity in darrruleentityAnnually)
            {
                if (
                    ruleentity.strFrecuencyValue == date_I.ToString("MMdd")
                    )
                {
                    darrruleentity.Add(ruleentity);
                }
            }

            List<PorjsonPeriodOrRuleJson> darrporjsonRules = new List<PorjsonPeriodOrRuleJson>();
            foreach (RuleentityRuleEntityDB ruleentity in darrruleentity)
            {
                ResResource.subVerifyStartAndEndOfTheRuleForADate(ps_I.strTimeZone, date_I, ruleentity,
                    ref darrporjsonRules);
            }

            //                                              //Add unavailable when daylight saving time.
            ZonedTime ztimeTestDay = new ZonedTime(date_I, "01:00:00".ParseToTime(), ZonedTimeTools.timezone);
            if (
                intDay_I == 0 &&
                ztimeTestDay.DaylightSavingTimeTypeOfDay == ZonedTimeDstTypeOfDayEnum.START_DAYLIGHT_SAVING_TIME
                )
            {
                PorjsonPeriodOrRuleJson porjson = new PorjsonPeriodOrRuleJson(null, "00:00:00", "01:00:00", null, 
                    "",false, null, null, null, null, 0, null, false);
                darrporjsonRules.Add(porjson);
            }

            //                                              //Get once rules.
            IQueryable<RuleentityRuleEntityDB> setruleentityOnce = context.Rule.Where(rule =>
                rule.intnPkResource == res_I.intPk && rule.strFrecuency == ResResource.strOnce);
            List<RuleentityRuleEntityDB> darrruleentityOnce = setruleentityOnce.ToList();

            //                                              //Get once printshop's rules.
            IQueryable<RuleentityRuleEntityDB> setruleentityPrintshopOnce = context.Rule.Where(rule =>
                rule.intnPkPrintshop == ps_I.intPk && rule.strFrecuency == ResResource.strOnce);
            List<RuleentityRuleEntityDB> darrruleentityPrintshopOnce = setruleentityPrintshopOnce.ToList();

            foreach (RuleentityRuleEntityDB rulePrintshop in darrruleentityPrintshopOnce)
            {
                //                                          //Add each once printshop rule to once rule list.
                darrruleentityOnce.Add(rulePrintshop);
            }

            //                                              //To easy code.
            ZonedTime ztimeStartDate = ZonedTimeTools.NewZonedTime(date_I, Time.MinValue);
            ZonedTime ztimeEndDate = ZonedTimeTools.NewZonedTime(date_I, Time.MaxValue);

            foreach (RuleentityRuleEntityDB ruleentity in darrruleentityOnce)
            {
                //                                          //To easy code.
                Date dateStartRule = ruleentity.strFrecuencyValue.Substring(0, 10).ParseToDate();
                Date dateEndRule = ruleentity.strFrecuencyValue.Substring(11).ParseToDate();
                Time timeStartRule = ruleentity.strStartTime.ParseToTime();
                Time timeEndRule = ruleentity.strEndTime.ParseToTime();

                ZonedTime ztimeStartRule = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                        dateStartRule, timeStartRule, ps_I.strTimeZone);
                ZonedTime ztimeEndRule = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                        dateEndRule, timeEndRule, ps_I.strTimeZone);

                ResResource.subAddAPeriodOrOnceRule(ztimeStartDate, ztimeEndDate, ztimeStartRule, ztimeEndRule,
                    null, null, null, null, null, null, 0, null, false, ps_I.strPrintshopId, ref darrporjsonRules);
            }

            ResResource.subUnifyTheRulesForTheSameTime(ref darrporjsonRules);

            darrporjson_M.AddRange(darrporjsonRules);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subVerifyStartAndEndOfTheRuleForADate(
            String strTimeZoneId_I,
            Date date_I,
            RuleentityRuleEntityDB ruleentity_I,
            ref List<PorjsonPeriodOrRuleJson> darrporjson_M
            )
        {
            ZonedTime ztimeStartRule = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                ruleentity_I.strRangeStartDate.ParseToDate(), ruleentity_I.strRangeStartTime.ParseToTime(),
                strTimeZoneId_I);

            //                                          //To easy code. The range of the rule
            Date dateStartRule = ztimeStartRule.Date;
            Time timeStartRule = ztimeStartRule.Time;

            Date dateEndRule = Date.MaxValue;
            Time timeEndRule = Time.MaxValue;
            if (
                ruleentity_I.strRangeEndDate != null
                )
            {
                ZonedTime ztimeEndRule = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                    ruleentity_I.strRangeEndDate.ParseToDate(), ruleentity_I.strRangeEndTime.ParseToTime(),
                    strTimeZoneId_I);

                dateEndRule = ztimeEndRule.Date;
                timeEndRule = ztimeEndRule.Time;
            }

            //                                          //The rule in that day.
            Time timeStartRuleForDate = ruleentity_I.strStartTime.ParseToTime();
            Time timeEndRuleForDate = ruleentity_I.strEndTime.ParseToTime();

            /*CASE*/
            if (
                //                                      //The date is in the validity of the rule.
                (date_I > dateStartRule) &&
                (date_I < dateEndRule)
                )
            {
                PorjsonPeriodOrRuleJson porjson = new PorjsonPeriodOrRuleJson(null, ruleentity_I.strStartTime,
                    ruleentity_I.strEndTime, null, "", false, null, null, null, null, 0, null, false);
                darrporjson_M.Add(porjson);
            }
            else if (
                //                                      //The date is the start of the rule.
                (dateStartRule == date_I) &&
                (date_I < dateEndRule)
                )
            {
                String strStartTime = timeStartRule.ToString();
                if (
                    timeStartRuleForDate > timeStartRule
                    )
                {
                    strStartTime = timeStartRuleForDate.ToString();
                }

                //                                          //Verify if this rule is expired.
                if (
                    strStartTime.ParseToTime() < ruleentity_I.strEndTime.ParseToTime()
                    )
                {
                    PorjsonPeriodOrRuleJson porjson = new PorjsonPeriodOrRuleJson(null, strStartTime,
                    ruleentity_I.strEndTime, null, "", false, null, null, null, null, 0, null, false);
                    darrporjson_M.Add(porjson);
                }
            }
            else if (
                //                                      //The date is the end of the rule.
                (date_I > dateStartRule) &&
                (date_I == dateEndRule)
                )
            {
                String strEndTime = timeEndRule.ToString();
                if (
                    timeEndRuleForDate < timeEndRule
                    )
                {
                    strEndTime = timeEndRuleForDate.ToString();
                }

                PorjsonPeriodOrRuleJson porjson = new PorjsonPeriodOrRuleJson(null, ruleentity_I.strStartTime,
                    strEndTime, null, "", false, null, null, null, null, 0, null, false);
                darrporjson_M.Add(porjson);
            }
            else if (
                //                                      //The date is the start and end of the rule.
                (date_I == dateStartRule) &&
                (date_I == dateEndRule)
                )
            {
                String strStartTime = timeStartRule.ToString();
                if (
                    timeStartRuleForDate > timeStartRule
                    )
                {
                    strStartTime = timeStartRuleForDate.ToString();
                }

                String strEndTime = timeEndRule.ToString();
                if (
                    timeEndRuleForDate < timeEndRule
                    )
                {
                    strEndTime = timeEndRuleForDate.ToString();
                }

                PorjsonPeriodOrRuleJson porjson = new PorjsonPeriodOrRuleJson(null, strStartTime, strEndTime, null,
                    "", false, null, null, null, null, 0, null, false);
                darrporjson_M.Add(porjson);
            }
            /*END-CASE*/

        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subAddTimeAvailable(
            //                                              //Note. The arr should be ordered by time
            //                                              //    for proper operation.
            //                                              //Rule or period arr.
            ref List<PorjsonPeriodOrRuleJson> darrporjson_M,
            //                                              //strDate.
            String strDate_I
            )
        {
            //                                              //To easy code.
            ZonedTime ztimeEndDate = ZonedTimeTools.NewZonedTime(strDate_I.ParseToDate(), Time.MaxValue);
            String strTimeMax000000Equiv240000 = "00:00:00 (Next day)";
            String strTimeMax = "24:00:00";
            String strTimeMax2 = "23:59:59";

            //                                              //Initial data for Available time.
            String strDateStartAvailable = strDate_I;
            String strTimeStartAvailable = Time.MinValue.ToString();

            ZonedTime ztimeStartAvailable = ZonedTimeTools.NewZonedTime(
                strDateStartAvailable.ParseToDate(),
                strTimeStartAvailable.ParseToTime()
                );

            //                                              //Copy data of the arr to easy code.
            List<PorjsonPeriodOrRuleJson> darrporjson = new List<PorjsonPeriodOrRuleJson>();
            foreach (PorjsonPeriodOrRuleJson porjson in darrporjson_M)
            {
                darrporjson.Add(porjson);
            }

            int intI = 0;
            /*REPEAT-WHILE*/
            while (
                //                                          //Take each RuleOrPeriod of the arr.
                intI < darrporjson.Count
                )
            {
                String strStartTimeRuleOrPeriod = darrporjson[intI].strStartTime;
                String strEndTimeRuleOrPeriod = darrporjson[intI].strEndTime == strTimeMax ? Time.MaxValue.ToString() :
                    darrporjson[intI].strEndTime;

                //                                          //RuleOrPeriod Start
                ZonedTime ztimeStartRulOrPeriod = ZonedTimeTools.NewZonedTime(
                strDateStartAvailable.ParseToDate(),
                strStartTimeRuleOrPeriod.ParseToTime()
                );
                //                                          //RuleOrPeriod End
                ZonedTime ztimeEndRulOrPeriod = ZonedTimeTools.NewZonedTime(
                strDateStartAvailable.ParseToDate(),
                strEndTimeRuleOrPeriod.ParseToTime()
                );

                if (
                    //                                      //Time INIT is 00:00:00, and second iteration the value
                    //                                      //    is ztimeStartAvailable.
                    ztimeStartRulOrPeriod == ztimeStartAvailable
                    )
                {
                    //                                      //There is a rule or period to the start of the day.
                }
                else
                {
                    //                                      //There is not a rule or period to the start of the day.

                    //                                      //Create time available to the star of the day.
                    PorjsonPeriodOrRuleJson porjson = new PorjsonPeriodOrRuleJson(null,
                       strTimeStartAvailable, strStartTimeRuleOrPeriod, null, "", true, null, null, null, null, 0,
                       null, false);
                    darrporjson_M.Add(porjson);
                }
                if (
                    //                                      //are there more rules or periods?.
                    darrporjson.Count > (intI + 1)
                    )
                {
                    //                                      //There are more rules or periods.
                    strDateStartAvailable = strDate_I;
                    strTimeStartAvailable = strEndTimeRuleOrPeriod;

                    ztimeStartAvailable = ZonedTimeTools.NewZonedTime(
                        strDateStartAvailable.ParseToDate(),
                        strTimeStartAvailable.ParseToTime()
                        );
                }
                else
                {
                    //                                      //This RuleOrPeriod is the last of the arr.
                    if (
                        ztimeEndRulOrPeriod < ztimeEndDate &&
                        ztimeEndRulOrPeriod > ztimeStartAvailable
                    )
                    {
                        //                                  //This RuleOrPeriod do not cover 23:59:59 hours.
                        //                                  //    Therefor added a timeAvailable until the last 
                        //                                  //    hour day.
                        PorjsonPeriodOrRuleJson porjson = new PorjsonPeriodOrRuleJson(null, strEndTimeRuleOrPeriod,
                            strTimeMax000000Equiv240000, null, "", true, null, null, null, null, 0, null, false);
                        darrporjson_M.Add(porjson);
                    }
                }
                intI = intI + 1;
            }

            //                                          //Reordering is necesary because new items were added.
            //                                          //   this arr is order by time.
            darrporjson_M.Sort();

            int intLength = darrporjson_M.Count;
            if (
                (intLength > 0) &&
                (darrporjson_M.Last().strEndTime == strTimeMax ||
                (darrporjson_M.Last().strEndTime == strTimeMax2))
                )
            {
                darrporjson_M.Last().strEndTime = strTimeMax000000Equiv240000;
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subUnifyTheRulesForTheSameTime(
            ref List<PorjsonPeriodOrRuleJson> darrporjsonRules_M
            )
        {
            PorjsonPeriodOrRuleJson[] arrporjson = darrporjsonRules_M.ToArray();
            Array.Sort(arrporjson);

            darrporjsonRules_M = new List<PorjsonPeriodOrRuleJson>();
            foreach (PorjsonPeriodOrRuleJson porjson in arrporjson)
            {
                if (
                    darrporjsonRules_M.Count == 0
                    )
                {
                    darrporjsonRules_M.Add(porjson);
                }
                else
                {
                    PorjsonPeriodOrRuleJson porjsonLast = darrporjsonRules_M.Last();
                    Time timeStartLast = porjsonLast.strStartTime.ParseToTime();
                    Time timeEndLast = porjsonLast.strEndTime.ParseToTime();

                    Time timeStart = porjson.strStartTime.ParseToTime();
                    Time timeEnd = porjson.strEndTime.ParseToTime();

                    if (
                        timeStart > timeEndLast
                        )
                    {
                        darrporjsonRules_M.Add(porjson);
                    }
                    else if (
                        timeStart <= timeEndLast
                        )
                    {
                        darrporjsonRules_M.Remove(porjsonLast);

                        if (
                            timeEnd > timeEndLast
                            )
                        {
                            porjsonLast.strEndTime = timeEnd.ToString();
                        }

                        darrporjsonRules_M.Add(porjsonLast);
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public static void subGetFirstAndLastNameOfEmployee(
            String strPrintshopId_I,
            int intContactId_I,
            List<ContactjsonContactJson> darrcontactjson_I,
            ref String strFirstName_IO,
            ref String strLastName_IO)
        {
            ContactjsonContactJson contactjson = darrcontactjson_I.FirstOrDefault(contact =>
            contact.intContactId == intContactId_I);

            if (
                contactjson != null
                )
            {
                strFirstName_IO = contactjson.strFirstName;
                strLastName_IO = contactjson.strLastName;
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public static List<ContactjsonContactJson> darrcontactjsonGetAllEmployee(
            //                                              //Get all printhop's employees.

            String strPrintshopId_I
            )
        {
            List<ContactjsonContactJson> darrcontactjsonFromWisnet = new List<ContactjsonContactJson>();

            //                                              //Get data from Wisnet.
            String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                GetSection("Odyssey2Settings")["urlWisnetApi"];
            Task<List<ContactjsonContactJson>> Task_darrcontactjsonFromWisnet = HttpTools<ContactjsonContactJson>.
                    GetListAsyncToEndPoint(strUrlWisnet + "/Contacts/" +
                    strPrintshopId_I);
            Task_darrcontactjsonFromWisnet.Wait();

            if (
                //                                          //There is access to the service of Wisnet.
                Task_darrcontactjsonFromWisnet.Result != null
                )
            {
                //                                          //Final array of products from Wisnet.
                darrcontactjsonFromWisnet = Task_darrcontactjsonFromWisnet.Result;               
            }

            return darrcontactjsonFromWisnet;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subVerifyPeriodIsOverlapWhitARule(
            Date date_I,
            ref List<PorjsonPeriodOrRuleJson> darrporjson_M,
            String strPrintshopId_I
            )
        {
            //                                              //Establish the connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get Period and Rules.
            List<PorjsonPeriodOrRuleJson> darrporjsonPeriod = darrporjson_M.Where(por => por.intnPkPeriod != null).
                ToList();
            List<PorjsonPeriodOrRuleJson> darrporjsonRule = darrporjson_M.Where(por => por.intnPkPeriod == null).
                ToList();

            foreach (PorjsonPeriodOrRuleJson porjsonPeriod in darrporjsonPeriod)
            {
                //                                          //Get start and end Ztime of the period.
                ZonedTime ztimeStartPeriod = ZonedTimeTools.NewZonedTime(date_I, porjsonPeriod.strStartTime.
                    ParseToTime());
                ZonedTime ztimeEndPeriod = ZonedTimeTools.NewZonedTime(Date.MaxValue, Time.MinValue);
                if (
                    porjsonPeriod.strEndTime != "24:00:00"
                    )
                {
                    ztimeEndPeriod = ZonedTimeTools.NewZonedTime(date_I, porjsonPeriod.strEndTime.ParseToTime());
                }

                int intLengthRule = darrporjsonRule.Count();
                int intI = 0;
                /*REPEAT-WHILE*/
                while (
                    //                                      //take each rule.
                    intI < intLengthRule
                    )
                {
                    ZonedTime ztimeStartRule = ZonedTimeTools.NewZonedTime(date_I, darrporjsonRule[intI].strStartTime.
                        ParseToTime());
                    ZonedTime ztimeEndRule = ZonedTimeTools.NewZonedTime(Date.MaxValue, Time.MinValue);
                    if (
                        porjsonPeriod.strEndTime != "24:00:00"
                        )
                    {
                        ztimeEndRule = ZonedTimeTools.NewZonedTime(date_I, darrporjsonRule[intI].strEndTime.
                        ParseToTime());
                    }

                    /*CASE*/
                    if (
                        //                                  //The period start before that the start rule and ends 
                        //                                  //    between rule range.
                        ztimeStartPeriod < ztimeStartRule &&
                        ztimeEndPeriod > ztimeStartRule &&
                        ztimeEndPeriod < ztimeEndRule
                        )
                    {
                        darrporjsonRule[intI].strStartTime = porjsonPeriod.strEndTime;
                    }
                    else if (
                        //                                  //The period is in the rule range.
                        ztimeStartPeriod >= ztimeStartRule &&
                        ztimeEndPeriod < ztimeEndRule
                        )
                    {
                        //                                  //Get strJobNumber.
                        String strJobNumber = JobJob.strGetJobNumber(null, darrporjsonRule[intI].strJobId.ParseToInt(),
                            strPrintshopId_I, context);

                        //                                  //Rule Cloned.
                        PorjsonPeriodOrRuleJson porjsonRuleCloned = new PorjsonPeriodOrRuleJson(
                            darrporjsonRule[intI].intnPkPeriod, darrporjsonRule[intI].strStartTime,
                            darrporjsonRule[intI].strEndTime, darrporjsonRule[intI].strJobId, strJobNumber,
                            darrporjsonRule[intI].boolIsAvailable, darrporjsonRule[intI].strFirstName,
                            darrporjsonRule[intI].strLastName, darrporjsonRule[intI].strProcess,
                            darrporjsonRule[intI].strJobName, darrporjsonRule[intI].intMinsBeforeDelete,
                            null, false);

                        darrporjsonRule[intI].strEndTime = porjsonPeriod.strStartTime;
                        porjsonRuleCloned.strStartTime = porjsonPeriod.strEndTime;

                        //                                  //Add rule cloned.
                        darrporjsonRule.Add(porjsonRuleCloned);

                        //                                  //Update the legth,
                        intLengthRule = darrporjsonRule.Count();
                    }
                    else if (
                        //                                  //The period start after that the star rule and ends 
                        //                                  //    after rule end.
                        ztimeStartPeriod >= ztimeStartRule &&
                        ztimeStartPeriod < ztimeEndRule &&
                        ztimeEndPeriod > ztimeEndRule
                        )
                    {
                        darrporjsonRule[intI].strEndTime = porjsonPeriod.strStartTime;
                    }
                    else if (
                        //                                  //The period covers all rule range.
                        ztimeStartPeriod < ztimeStartRule &&
                        ztimeEndPeriod >= ztimeEndRule
                        )
                    {
                        darrporjsonRule.Remove(darrporjsonRule[intI]);
                        intI = intI - 1;

                        //                                  //Update the legth,
                        intLengthRule = darrporjsonRule.Count();
                    }
                    /*END-CASE*/

                    //                                      //Continue searching if the period is  
                    //                                      //    greater than the rule or if
                    //                                      //    the period covers more than one
                    //                                      //    rule.
                    intI = intI + 1;
                }
            }

            //                                              //Update the new rules at the reference variable.
            darrporjson_M = darrporjsonRule.Concat(darrporjsonPeriod).ToList();
        }

        //--------------------------------------------------------------------------------------------------------------
        public CostentityCostEntityDB GetCostDependingDate(
            ZonedTime ztimeJobDate_I
            )
        {
            //                                              //Establish the connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get all the resource's costs.
            List<CostentityCostEntityDB> darrcostentity = context.Cost.Where(cost => cost.intPkResource ==
                this.intPk).ToList();

            CostentityCostEntityDB costentityCost = null;
            if (
               //                                          //There is more than one cost, verify dates in order to 
               //                                          //  get the cost than apply to the job's date.
               darrcostentity.Count() >= 1
               )
            {
                //                                          //Sort list of costs.
                darrcostentity.Sort();

                bool boolAuxiliary = true;
                int intI = darrcostentity.Count() - 1;
                /*WHILE-DO*/
                while (
                    boolAuxiliary == true &&
                    intI >= 0
                    )
                {
                    CostentityCostEntityDB costentity = darrcostentity[intI];
                    //                                      //Create ztime object for the cost in db.
                    Date dateSetDate = costentity.strSetDate.ParseToDate();
                    Time timeSetTime = costentity.strSetTime.ParseToTime();
                    ZonedTime ztimeCostDateDB = ZonedTimeTools.NewZonedTime(dateSetDate, timeSetTime);

                    if (
                        ztimeJobDate_I > ztimeCostDateDB
                        )
                    {
                        costentityCost = darrcostentity[intI];
                        boolAuxiliary = false;
                    }
                    intI = intI - 1;
                }
            }
            return costentityCost;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetAvailableTime(
            //                                              //Get the next available period of time.
            //                                              //If the resorce has a time, it find the next
            //                                              //      period of this size.

            String strPrintshopId_I,
            int intPkResource_I,
            int intJobId_I,
            int intPkProcessInWorkflow_I,
            int intPkEleetOrEleele_I,
            bool boolIsEleet_I,
            ZonedTime ztimeBase_I,
            int intOffsetTimeInMinutes_I,
            List<PerentityPeriodEntityDB> darrperentityTemporary_I,
            IConfiguration configuration_I,
            int? intnEstimateId_I,
            out TimesjsonTimesJson timesjson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO,
            ref long longMilisecondNeeded_I
            )
        {
            timesjson_O = null;

            ResResource res = ResResource.resFromDB(intPkResource_I, false);

            Odyssey2Context context = new Odyssey2Context();

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "No resource found.";
            if (
                res != null
                )
            {
                JobjsonJobJson jobjson;
                if (
                    JobJob.boolIsValidJobId(intJobId_I, strPrintshopId_I, configuration_I, out jobjson,
                    ref strUserMessage_IO, ref strDevMessage_IO)
                    )
                {
                    PiwentityProcessInWorkflowEntityDB piwentity = context.ProcessInWorkflow.FirstOrDefault(piw =>
                        piw.intPk == intPkProcessInWorkflow_I);

                    intStatus_IO = 402;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "Process in workflow no found.";
                    if (
                        piwentity != null
                        )
                    {

                        //                                  //Get the time entity.
                        //                                  //Not used in calculus. Only no know if the resource
                        //                                  //      has a time that applies.
                        TimeentityTimeEntityDB timeentity = ResResource.timeentityGet(intPkResource_I, jobjson,
                            piwentity, intPkEleetOrEleele_I, boolIsEleet_I, configuration_I, strPrintshopId_I, null,
                            intnEstimateId_I, null);

                        PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);

                        //                                  //Get job's correct processes.
                        List<PiwentityProcessInWorkflowEntityDB> darrpiwentityAllProcesses;
                        List<DynLkjsonDynamicLinkJson> darrdynlkjson;
                        ProdtypProductType.subGetWorkflowValidWay(piwentity.intPkWorkflow, jobjson,
                            out darrpiwentityAllProcesses, out darrdynlkjson);

                        if (
                            timeentity != null
                            )
                        {
                            ResResource.subGetTimesForAResourceWithTime(intnEstimateId_I, intOffsetTimeInMinutes_I,
                                intPkEleetOrEleele_I, ps.intPk, boolIsEleet_I, ps.strPrintshopId, jobjson, piwentity,
                                res, ztimeBase_I, darrperentityTemporary_I, darrpiwentityAllProcesses, configuration_I, 
                                ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO, ref longMilisecondNeeded_I, 
                                ref timesjson_O);
                        }
                        else
                        {
                            ResResource.subGetTimesForAGivenTime(intnEstimateId_I, intOffsetTimeInMinutes_I, ps.intPk,
                                0, 0, 0, jobjson, piwentity, res, ztimeBase_I, darrperentityTemporary_I, 
                                darrpiwentityAllProcesses, ref longMilisecondNeeded_I, ref timesjson_O);
                        }

                        if (
                            timesjson_O != null
                            )
                        {
                            ZonedTime ztimeStart = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                                timesjson_O.strStartDate.ParseToDate(), timesjson_O.strStartTime.ParseToTime(),
                                ps.strTimeZone);
                            ZonedTime ztimeEnd = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                                timesjson_O.strEndDate.ParseToDate(), timesjson_O.strEndTime.ParseToTime(),
                                ps.strTimeZone);

                            timesjson_O.strStartDate = ztimeStart.Date.ToString();
                            timesjson_O.strStartTime = ztimeStart.Time.ToString();
                            timesjson_O.strEndDate = ztimeEnd.Date.ToString();
                            timesjson_O.strEndTime = ztimeEnd.Time.ToString();

                        }

                        intStatus_IO = 200;
                        strUserMessage_IO = "Next available times set.";
                        strDevMessage_IO = "";
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetAvailableTime(
            //                                              //Get the next available period of time.
            //                                              //If the resoorce has a time, it find the next
            //                                              //      period of this size.

            int? intnEstimateId_I,
            int intOffsetTimeInMinutes_I,
            RecbdgjsonResourceBudgetJson recbdgjson_I,
            JobjsonJobJson jobjson_I,
            PiwentityProcessInWorkflowEntityDB piwentity_I,
            PsPrintShop ps_I,
            ResResource res_I,
            ZonedTime ztimeBase_I,
            List<PerentityPeriodEntityDB> darrperentityTemporary_I,
            out TimesjsonTimesJson timesjson_O
            )
        {
            timesjson_O = null;

            long longMilisecondNeeded = 0;
            //                                              //To easy code.
            int intHours = recbdgjson_I.intHours;
            int intMinutes = recbdgjson_I.intMinutes;
            int intSeconds = recbdgjson_I.intSeconds;

            //              //Get job's correct processes.
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentityAllProcesses;
            List<DynLkjsonDynamicLinkJson> darrdynlkjson;
            ProdtypProductType.subGetWorkflowValidWay(piwentity_I.intPkWorkflow, jobjson_I,
                out darrpiwentityAllProcesses, out darrdynlkjson);

            ResResource.subGetTimesForAGivenTime(intnEstimateId_I, intOffsetTimeInMinutes_I, ps_I.intPk,
                intHours, intMinutes, intSeconds, jobjson_I, piwentity_I, res_I, ztimeBase_I, darrperentityTemporary_I,
                darrpiwentityAllProcesses, ref longMilisecondNeeded, ref timesjson_O);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static TimeentityTimeEntityDB timeentityGet(
            //                                              //Get the proper time entity.
            //                                              //For this resource.
            //                                              //Valid accoding the start datetime of the job.
            //                                              //Valid according the thickness of the resource used as 
            //                                              //      thickness in calculations for this resource, in 
            //                                              //      this IO, in this wf.

            int intPkResource_I,
            JobjsonJobJson jobjson_I,
            PiwentityProcessInWorkflowEntityDB piwentity_I,
            int intPkEleetOrEleele_I,
            bool boolIsEleet_I,
            IConfiguration configuration_I,
            String strPrintshopId_I,
            List<CalCalculation> darrcalApply_I,
            int? intnEstimateIdThatInvokeThisMethod_I,
            List<IojsoninInputOrOutputJsonInternal> darriojsoninInputsCombinationsAndInputsSelected_I
            )
        {
            Odyssey2Context context = new Odyssey2Context();
            
            List<CalCalculation> darrcalApply;
            if (
                darrcalApply_I != null
                )
            {
                darrcalApply = darrcalApply_I;
            }
            else
            {
                darrcalApply = ResResource.darrcalApplyForAJob(intPkResource_I, intPkEleetOrEleele_I,
                    boolIsEleet_I, piwentity_I, jobjson_I);
            }

            //                                              //Time entities for this resource.
            List<TimeentityTimeEntityDB> darrtimeentity = context.Time.Where(time => 
                time.intPkResource == intPkResource_I).ToList();

            //                                              //Job entity.
            JobentityJobEntityDB jobentity = context.Job.FirstOrDefault(job => job.intJobID == jobjson_I.intJobId);
            ZonedTime ztimeJobDate; ProdtypProductType.GetJobDate(jobentity, out ztimeJobDate);
            //                                              //Time default.
            TimeentityTimeEntityDB timeentityDefault = null;
            //                                              //Times with thickness.
            List <TimeentityTimeEntityDB> darrtimeentityWithValidThickness = new List<TimeentityTimeEntityDB>();
            foreach (TimeentityTimeEntityDB timeentity in darrtimeentity)
            {
                //                                          //Get start and end for the time.
                ZonedTime ztimeTimeStart = ZonedTimeTools.NewZonedTime(timeentity.strStartDate.ParseToDate(),
                    timeentity.strStartTime.ParseToTime());

                bool boolIsValidAccordingTheStartTimeOfTheJob;
                if (
                    timeentity.strEndDate != null
                    )
                {
                    ZonedTime ztimeTimeEnd = ZonedTimeTools.NewZonedTime(timeentity.strEndDate.ParseToDate(),
                        timeentity.strEndTime.ParseToTime());

                    boolIsValidAccordingTheStartTimeOfTheJob =
                        ((ztimeJobDate >= ztimeTimeStart) && (ztimeJobDate <= ztimeTimeEnd)) ? true : false;
                }
                else
                {
                    boolIsValidAccordingTheStartTimeOfTheJob = (ztimeJobDate >= ztimeTimeStart) ? true : false;
                }

                bool boolIsValidAccordingThickness = false;
                if (
                    boolIsValidAccordingTheStartTimeOfTheJob
                    )
                {
                    if (
                        //                                  //Time have a range of thinckness.
                        (timeentity.numnMinThickness == null) ||
                        (timeentity.numnMaxThickness == null)
                        )
                    {
                        timeentityDefault = timeentity;
                    }
                    else if (
                        //                                  //Time that have a range of thinckness.
                        (timeentity.numnMinThickness != null) ||
                        (timeentity.numnMaxThickness != null)
                        )
                    {
                        //                                  //Range of thinckness.
                        double? numnTimeMinThickness;
                        double? numnTimeMaxThickness;
                        String strUnit = timeentity.strThicknessUnit;

                        ResResource.subConvertThicknessToUM(strUnit, timeentity.numnMinThickness,
                            timeentity.numnMaxThickness, out numnTimeMinThickness, out numnTimeMaxThickness);

                        //                                  //Look for calculations with QFrom
                        //                                  //      in the range of thickness.

                        if (
                            //                              //If there are not calculations, the bool remains false.
                            //                              //If there are calculations, the bool can be true.
                            darrcalApply.Count >= 0
                            )
                        {
                            List<CalCalculation> darrcalWithValidThickness = new List<CalCalculation>();
                            foreach (CalCalculation cal in darrcalApply)
                            {
                                //                          //Get the Resource QFrom.
                                int intPkQfromResource = (int)cal.intnPkQFromResourceElementBelongsTo;
                                EleentityElementEntityDB entityResourceQFrom = context.Element.FirstOrDefault(qfrom =>
                                    qfrom.intPk == intPkQfromResource);
                                EtentityElementTypeEntityDB etentityTypeQFrom = context.ElementType.FirstOrDefault(et =>
                                    et.intPk == entityResourceQFrom.intPkElementType);

                                bool boolIsPaper = (etentityTypeQFrom.strCustomTypeId == ResResource.strComponent) ||
                                    (etentityTypeQFrom.strCustomTypeId == ResResource.strMedia);

                                if (
                                    //                      //If QFrom is not paper, calculation not valid.
                                    //                      //If QFrom is paper, it can have thickness.
                                    boolIsPaper
                                    )
                                {
                                    //                      //Get resource working as thickness in a workflow.
                                    EleentityElementEntityDB eleentityThicknessResource =
                                        ResResource.eleentityResourceWorkingAsThickness(
                                        (int)cal.intnPkWorkflowBelongsTo, jobjson_I.intJobId, context);

                                    if (
                                        eleentityThicknessResource != null
                                        )
                                    {
                                        //                                      //Get the pk of Thickness attribute.
                                        int intPkAttributeThickness = (from attrentity in context.Attribute
                                                                       join attretentity in context.AttributeElementType
                                                                       on attrentity.intPk equals attretentity.intPkAttribute
                                                                       where attretentity.intPkElementType ==
                                                                       eleentityThicknessResource.intPkElementType &&
                                                                       attrentity.strXJDFName == "Thickness"
                                                                       select attrentity).FirstOrDefault().intPk;

                                        //                                      //Get the pk of ThicknessUnit attribute.
                                        int intPkAttributeThicknessUnit = (from attrentity in context.Attribute
                                                                           join attretentity in context.AttributeElementType
                                                                           on attrentity.intPk equals attretentity.intPkAttribute
                                                                           where attretentity.intPkElementType ==
                                                                           eleentityThicknessResource.intPkElementType &&
                                                                           attrentity.strXJDFName == "ThicknessUnit"
                                                                           select attrentity).FirstOrDefault().intPk;

                                        ValentityValueEntityDB valentityThickness = context.Value.FirstOrDefault(val =>
                                            val.intPkAttribute == intPkAttributeThickness &&
                                            val.intPkElement == eleentityThicknessResource.intPk);

                                        ValentityValueEntityDB valentityThicknessUnit = context.Value.FirstOrDefault(val =>
                                            val.intPkAttribute == intPkAttributeThicknessUnit &&
                                            val.intPkElement == eleentityThicknessResource.intPk);

                                        if (
                                            //                  //WF Resource has thickness.
                                            valentityThickness != null
                                            )
                                        {
                                            double? numnWorkflowThickness;
                                            double? numnDummy;
                                            ResResource.subConvertThicknessToUM(valentityThicknessUnit.strValue,
                                                valentityThickness.strValue.ParseToNum(), null, out numnWorkflowThickness,
                                                out numnDummy);

                                            if (
                                                //              //Thickness is in range.
                                                ((numnTimeMinThickness == null) ||
                                                (numnWorkflowThickness >= numnTimeMinThickness))
                                                &&
                                                ((numnTimeMaxThickness == null) ||
                                                (numnWorkflowThickness <= numnTimeMaxThickness))
                                                )
                                            {
                                                int intStatus = 200;
                                                String strUserMessage = "";
                                                String strDevMessage = "";
                                                bool boolQFromIsInThisJob =
                                                    ProdtypProductType.boolQFromIsInThisWFJobOrEstimateData(
                                                    cal.intnPkResourceElementBelongsTo,
                                                    cal.intnPkElementElementTypeBelongsTo,
                                                    cal.intnPkElementElementBelongsTo,
                                                    cal.intnPkQFromResourceElementBelongsTo,
                                                    cal.intnPkQFromElementElementTypeBelongsTo,
                                                    cal.intnPkQFromElementElementBelongsTo,
                                                    configuration_I, jobjson_I.intJobId, strPrintshopId_I,
                                                    piwentity_I.intPk,
                                                    intnEstimateIdThatInvokeThisMethod_I,
                                                    darriojsoninInputsCombinationsAndInputsSelected_I,
                                                    ref intStatus, ref strUserMessage, ref strDevMessage);
                                                if (
                                                    boolQFromIsInThisJob
                                                    )
                                                {
                                                    darrcalWithValidThickness.Add(cal);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (
                                darrcalWithValidThickness.Count > 0
                                )
                            {
                                boolIsValidAccordingThickness = true;
                                int? intnPkQFromResourcePrevious = null;
                                foreach (CalCalculation cal in darrcalWithValidThickness)
                                {
                                    int? intnPkQFromResource = cal.intnPkQFromResourceElementBelongsTo;
                                    if (
                                        (intnPkQFromResourcePrevious != null) &&
                                        (intnPkQFromResource == intnPkQFromResourcePrevious)
                                        )
                                    {
                                        boolIsValidAccordingThickness = false;
                                    }
                                    intnPkQFromResourcePrevious = intnPkQFromResource;
                                }
                            }
                        }
                    }
                }
                if (
                    boolIsValidAccordingTheStartTimeOfTheJob &&
                    boolIsValidAccordingThickness
                    )
                {
                    darrtimeentityWithValidThickness.Add(timeentity);
                }
            }

            //                                              //Pick the time to return.
            TimeentityTimeEntityDB timeentityToReturn = null;
            if (
                darrtimeentityWithValidThickness.Count == 1
                )
            {
                timeentityToReturn = darrtimeentityWithValidThickness[0];
            }
            else if (
                (darrtimeentityWithValidThickness.Count) == 0 &&
                (timeentityDefault != null)
                )
            {
                timeentityToReturn = timeentityDefault;
            }
            return timeentityToReturn;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public static List<CalCalculation> darrcalApplyForAJob(
            int intPkResource_I,
            int intPkEleetOrEleele_I,
            bool boolIsEleet_I,
            PiwentityProcessInWorkflowEntityDB piwentity_I,
            JobjsonJobJson jobjson_I
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Job entity.
            //                                              //To get the proper calculations depending on the stage 
            //                                              //      and startdate of the job.
            JobentityJobEntityDB jobentity = context.Job.FirstOrDefault(job => job.intJobID == jobjson_I.intJobId);
            ZonedTime ztimeJobDate; ProdtypProductType.GetJobDate(jobentity, out ztimeJobDate);

            //                                              //Modificacion por la historia de scratch.
            WfentityWorkflowEntityDB wfentity = context.Workflow.FirstOrDefault(wf => wf.intPk == piwentity_I.intPkWorkflow);

            int intPkProduct = 0;
            if (
                wfentity.intnPkProduct != null
                )
            {
                intPkProduct = (int)(from Workflow in context.Workflow
                                         join ProcessInWorkflow in context.ProcessInWorkflow
                                         on Workflow.intPk equals ProcessInWorkflow.intPkWorkflow
                                         where ProcessInWorkflow.intPk == piwentity_I.intPk
                                         select Workflow.intnPkProduct).ToList()[0];
            }

            //                                              //Get all the calculations.
            //                                              //Depending on the job stage.
            List<CalCalculation> darrcalFromDB = new List<CalCalculation>();
            if (
                intPkProduct > 0
                )
            {
                if (
                (jobentity != null) &&
                //                                          //Job is InProgress or Completed.
                ((jobentity.intStage == JobJob.intInProgressStage ||
                jobentity.intStage == JobJob.intCompletedStage))
                )
                {
                    darrcalFromDB = ProdtypProductType.GetCalculationsDependingDate(intPkProduct, ztimeJobDate);
                }
                else
                {
                    darrcalFromDB = ProdtypProductType.subGetCurrentCalculationsFromDB(intPkProduct);
                }
            }

            //                                              //To easy code.
            int? intnPkEleet_I = boolIsEleet_I ? (int?)intPkEleetOrEleele_I : null;
            int? intnPkEleele_I = !boolIsEleet_I ? (int?)intPkEleetOrEleele_I : null;

            //                                              //Get calculation for this IO.
            List<CalCalculation> darrcalApply = darrcalFromDB.Where(cal =>
                //                                          //Per Quantity calculation, take quantity the Job.
                (cal.strCalculationType == CalCalculation.strPerQuantity
                || cal.strCalculationType == CalCalculation.strPerQuantityBase)
                &&
                (cal.strByX == CalCalculation.strByResource) &&
                (cal.intnPkResourceElementBelongsTo == intPkResource_I) &&
                (cal.intnPkWorkflowBelongsTo == piwentity_I.intPkWorkflow) &&
                (cal.intnProcessInWorkflowId == piwentity_I.intProcessInWorkflowId) &&
                (cal.intnPkElementElementTypeBelongsTo == intnPkEleet_I) &&
                (cal.intnPkElementElementBelongsTo == intnPkEleele_I) &&
                //                                          //Calculation is enable.
                cal.boolIsEnable &&
                cal.strCalculationType == CalCalculation.strPerQuantity &&
                //                                          //Condition to apply and quantity condition apply for 
                //                                          //      this cal.
                Tools.boolCalculationOrLinkApplies(cal.intPk, null, null, null, jobjson_I)).ToList();

            return darrcalApply;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subGetTimesForAResourceWithTime(

            int? intnEstimateId_I,
            int intOffsetTimeInMinutes_I,
            int intPkEleetOrEleele_I,
            int intPkPrintshop_I,
            bool boolIsEleet_I,
            String strPrintshopId_I,
            JobjsonJobJson jobjson_I,
            PiwentityProcessInWorkflowEntityDB piwentity_I,
            ResResource res_I,
            ZonedTime ztimeBase_I,
            List<PerentityPeriodEntityDB> darrperentityTemporary_I,
            //                                              //Process valids for the Job.
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentityAllProcesses_I,
            IConfiguration configuration_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO,
            ref long longMilisecondNeeded_I,
            ref TimesjsonTimesJson timesjson_M
            )
        {

            Odyssey2Context context = new Odyssey2Context();

            //                                          //To easy code.
            EleentityElementEntityDB eleentity = context.Element.FirstOrDefault(ele =>
                ele.intPk == res_I.intPk);

            EleetentityElementElementTypeEntityDB eleetentity = null;
            EleeleentityElementElementEntityDB eleeleentity = null;
            if (
                boolIsEleet_I
                )
            {
                eleetentity = context.ElementElementType.FirstOrDefault(eleet =>
                    eleet.intPk == intPkEleetOrEleele_I);
            }
            else
            {
                eleeleentity = context.ElementElement.FirstOrDefault(eleele =>
                    eleele.intPk == intPkEleetOrEleele_I);
            }

            intStatus_IO = 403;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Info about the IO in the workflow is not valid.";
            if (
                (piwentity_I != null) &&
                ((eleetentity != null) || (eleeleentity != null))
                )
            {
                //                                      //Get pkProduct from the workflow.
                int intPkProduct = (int)context.Workflow.FirstOrDefault(wf =>
                wf.intPk == piwentity_I.intPkWorkflow).intnPkProduct;

                //                                      //Get prodtyp.
                ProdtypProductType prodtyp = (ProdtypProductType)EtElementTypeAbstract.etFromDB(
                    intPkProduct);

                int intHours = 0;
                int intMinutes = 0;
                int intSeconds = 0;

                int intPkEleetOrEleele = eleetentity != null ? eleetentity.intPk : eleeleentity.intPk;

                bool boolIsEleet = eleetentity != null ? true : false;

                //                                      //Get Info from a PIW for get the time for the resource. 
                ResResource.subGetInfoByResource(intPkEleetOrEleele, boolIsEleet, eleentity, piwentity_I, jobjson_I,
                    strPrintshopId_I, prodtyp, configuration_I, ref intHours, ref intMinutes, ref intSeconds);

                ResResource.subGetTimesForAGivenTime(intnEstimateId_I, intOffsetTimeInMinutes_I, intPkPrintshop_I,
                    intHours, intMinutes, intSeconds, jobjson_I, piwentity_I, res_I, ztimeBase_I, 
                    darrperentityTemporary_I, darrpiwentityAllProcesses_I, ref longMilisecondNeeded_I, 
                    ref timesjson_M);
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subGetTimesForAGivenTime(

            int? intnEstimateId_I,
            int intOffsetTimeInMinutes_I,
            int intPkPrintshop_I,
            int intHours_I,
            int intMinutes_I,
            int intSeconds_I,
            JobjsonJobJson jobjson_I,
            PiwentityProcessInWorkflowEntityDB piwentity_I,
            ResResource res_I,
            ZonedTime ztimeBase_I,
            List<PerentityPeriodEntityDB> darrperentityTemporary_I,
            //                                              //Process valids for the Job.
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentityAllProcesses_I,
            ref long longMilisecondNeeded_I,
            ref TimesjsonTimesJson timesjson_M
            )
        {
            //                                              //To easy code.
            long longMilisecondsNeeded = (intSeconds_I + (intMinutes_I * 60) + (intHours_I * 3600)) * 1000;
            longMilisecondsNeeded = (longMilisecondsNeeded > 0) ? longMilisecondsNeeded : (60 * 1000);
            longMilisecondNeeded_I = longMilisecondsNeeded;

            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get previous or next processes.
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentity = ProProcess.darrpiwentityPreviousOrNext(
                piwentity_I, true, darrpiwentityAllProcesses_I);

            List<PerentityPeriodEntityDB> darrperentityPreviousProcesses = new List<PerentityPeriodEntityDB>();
            foreach (PiwentityProcessInWorkflowEntityDB piwentityProcessPrevious in darrpiwentity)
            {
                darrperentityPreviousProcesses.AddRange(context.Period.Where(per =>
                    per.intPkWorkflow == piwentity_I.intPkWorkflow &&
                    per.intProcessInWorkflowId == piwentityProcessPrevious.intProcessInWorkflowId &&
                    per.intJobId == jobjson_I.intJobId &&
                    per.intnEstimateId == null).ToList());
            }

            ZonedTime ztimeEndLastPreviousPeriod = ZonedTimeTools.NewZonedTime(ztimeBase_I.Date,
                Time.MinValue);
            if (
                darrperentityPreviousProcesses.Count > 0
                )
            {
                //                                              //Get the last period of the previous processes.
                foreach (PerentityPeriodEntityDB perentity in darrperentityPreviousProcesses)
                {
                    ZonedTime ztimeEndPeriod = ZonedTimeTools.NewZonedTime(perentity.strEndDate.ParseToDate(),
                        perentity.strEndTime.ParseToTime());

                    if (
                        ztimeEndPeriod > ztimeEndLastPreviousPeriod
                        )
                    {
                        ztimeEndLastPreviousPeriod = ztimeEndPeriod;
                    }
                }
            }

            List<Porjson2PeriodOrRuleJson2> darrporjson2 = ResResource.darrporjson2GetPeriodsAndRulesFromDate(
                intPkPrintshop_I, res_I, ztimeEndLastPreviousPeriod.Date, darrperentityTemporary_I, intnEstimateId_I,
                jobjson_I.intJobId);

            if (
                darrporjson2.Count > 0
                )
            {
                int intI = 0;
                /*UNTIL-DO*/
                while (!(
                    (intI >= darrporjson2.Count) ||
                    (timesjson_M != null)
                    ))
                {
                    ZonedTime ztimeEndPeriod = darrporjson2[intI].ztimeEnd;
                    ZonedTime ztimeStartPeriod = darrporjson2[intI].ztimeStart;

                    ZonedTime ztimeStartNextPeriod = ZonedTimeTools.NewZonedTime(Date.MaxValue, Time.MinValue);
                    if (
                        (intI + 1) < darrporjson2.Count
                        )
                    {
                        ztimeStartNextPeriod = darrporjson2[intI + 1].ztimeStart;
                    }

                    ZonedTime ztimeNow = ztimeBase_I;
                    ztimeNow = ztimeNow + (intOffsetTimeInMinutes_I * 60 * 1000);

                    /*CASE*/
                    if (
                        //                                  //Now is before the first period and the time fits.
                        (intI == 0) &&
                        (ztimeStartPeriod > ztimeNow) &&
                        ((ztimeStartPeriod - ztimeNow) >= longMilisecondsNeeded) &&
                        (ztimeNow >= ztimeEndLastPreviousPeriod)
                        )
                    {
                        ZonedTime ztimeNowPlusTime = ztimeNow + longMilisecondsNeeded;

                        String strStartDate = ztimeNow.Date.ToText();
                        String strStartTime = ztimeNow.Time.ToString();
                        String strEndDate = ztimeNowPlusTime.Date.ToText();
                        String strEndTime = ztimeNowPlusTime.Time.ToString();

                        timesjson_M = new TimesjsonTimesJson(strStartDate, strStartTime, strEndDate, strEndTime);
                    }
                    else if (
                        //                                  //Now is between the two periods but not immediatly after 
                        //                                  //      the first one.
                        (ztimeEndPeriod < ztimeNow) && (ztimeNow < ztimeStartNextPeriod) &&
                        ((ztimeStartNextPeriod - ztimeNow) >= longMilisecondsNeeded) &&
                        (ztimeNow >= ztimeEndLastPreviousPeriod)
                        )
                    {
                        ZonedTime ztimeNowPlusTime = ztimeNow + longMilisecondsNeeded;

                        String strStartDate = ztimeNow.Date.ToText();
                        String strStartTime = ztimeNow.Time.ToString();
                        String strEndDate = ztimeNowPlusTime.Date.ToText();
                        String strEndTime = ztimeNowPlusTime.Time.ToString();

                        timesjson_M = new TimesjsonTimesJson(strStartDate, strStartTime, strEndDate, strEndTime);
                    }
                    else if (
                        //                                  //Now is before the two periods.
                        (ztimeNow > ztimeEndLastPreviousPeriod) &&
                        (ztimeEndPeriod > ztimeNow) &&
                        ((ztimeStartNextPeriod - ztimeEndPeriod) >= longMilisecondsNeeded) &&
                        (ztimeEndPeriod >= ztimeEndLastPreviousPeriod)
                        )
                    {
                        ZonedTime ztimeEndPeriodPlusTime = ztimeEndPeriod + longMilisecondsNeeded;

                        String strStartDate = ztimeEndPeriod.Date.ToText();
                        String strStartTime = ztimeEndPeriod.Time.ToString();
                        String strEndDate = ztimeEndPeriodPlusTime.Date.ToText();
                        String strEndTime = ztimeEndPeriodPlusTime.Time.ToString();

                        timesjson_M = new TimesjsonTimesJson(strStartDate, strStartTime, strEndDate, strEndTime);
                    }
                    else if (
                        //                                  //Now is immediatly after the first period.
                        (ztimeEndPeriod.Date == ztimeNow.Date) &&
                        (ztimeEndPeriod.Time.Minutes == ztimeNow.Time.Minutes) &&
                        ((ztimeStartNextPeriod - ztimeEndPeriod) >= longMilisecondsNeeded) &&
                        (ztimeEndPeriod >= ztimeEndLastPreviousPeriod)
                        )
                    {
                        ZonedTime ztimeStart = ztimeBase_I;
                        ztimeStart = ztimeStart + (intOffsetTimeInMinutes_I * 60 * 1000);
                        ZonedTime ztimeStartPlusTime = ztimeStart + longMilisecondsNeeded;

                        String strStartDate = ztimeEndPeriod.Date.ToText();
                        String strStartTime = ztimeEndPeriod.Time.ToString();
                        String strEndDate = ztimeStartPlusTime.Date.ToText();
                        String strEndTime = ztimeStartPlusTime.Time.ToString();

                        timesjson_M = new TimesjsonTimesJson(strStartDate, strStartTime, strEndDate, strEndTime);
                    }
                    else if (
                        //                                  //Now is after all periods.
                        ((intI + 1) == darrporjson2.Count) &&
                        (ztimeNow > ztimeEndPeriod) &&
                        (ztimeNow >= ztimeEndLastPreviousPeriod)
                        )
                    {
                        ZonedTime ztimeStart = ztimeBase_I;
                        ztimeStart = ztimeStart + (intOffsetTimeInMinutes_I * 60 * 1000);
                        ZonedTime ztimeStartPlusTime = ztimeStart + longMilisecondsNeeded;

                        String strStartDate = ztimeStart.Date.ToText();
                        String strStartTime = ztimeStart.Time.ToString();
                        String strEndDate = ztimeStartPlusTime.Date.ToText();
                        String strEndTime = ztimeStartPlusTime.Time.ToString();

                        timesjson_M = new TimesjsonTimesJson(strStartDate, strStartTime, strEndDate, strEndTime);
                    }
                    else if (
                        (ztimeEndLastPreviousPeriod > ztimeNow) &&
                        (ztimeEndLastPreviousPeriod < ztimeStartNextPeriod) &&
                        (ztimeEndLastPreviousPeriod > ztimeEndPeriod) &&
                        ((ztimeStartNextPeriod - ztimeEndLastPreviousPeriod) > longMilisecondsNeeded)
                        )
                    {
                        ZonedTime ztimeStartPlusTime = ztimeEndLastPreviousPeriod + longMilisecondsNeeded;

                        String strStartDate = ztimeEndLastPreviousPeriod.Date.ToText();
                        String strStartTime = ztimeEndLastPreviousPeriod.Time.ToString();
                        String strEndDate = ztimeStartPlusTime.Date.ToText();
                        String strEndTime = ztimeStartPlusTime.Time.ToString();

                        timesjson_M = new TimesjsonTimesJson(strStartDate, strStartTime, strEndDate, strEndTime);
                    }
                    else if (
                        (ztimeEndLastPreviousPeriod > ztimeNow) &&
                        (ztimeEndLastPreviousPeriod < ztimeStartPeriod) &&
                        ((ztimeStartPeriod - ztimeEndLastPreviousPeriod) > longMilisecondsNeeded)
                        )
                    {
                        ZonedTime ztimeStartPlusTime = ztimeEndLastPreviousPeriod + longMilisecondsNeeded;

                        String strStartDate = ztimeEndLastPreviousPeriod.Date.ToText();
                        String strStartTime = ztimeEndLastPreviousPeriod.Time.ToString();
                        String strEndDate = ztimeStartPlusTime.Date.ToText();
                        String strEndTime = ztimeStartPlusTime.Time.ToString();

                        timesjson_M = new TimesjsonTimesJson(strStartDate, strStartTime, strEndDate, strEndTime);
                    }
                    /*END-CASE*/

                    intI = intI + 1;
                }
            }
            else
            {
                ZonedTime ztimeStart = ztimeBase_I;
                ztimeStart = ztimeStart + (intOffsetTimeInMinutes_I * 60 * 1000);
                if (
                    ztimeStart < ztimeEndLastPreviousPeriod
                    )
                {
                    ztimeStart = ztimeEndLastPreviousPeriod;
                }
                ZonedTime ztimeStartPlusTime = ztimeStart + longMilisecondsNeeded;

                String strStartDate = ztimeStart.Date.ToText();
                String strStartTime = ztimeStart.Time.ToString();
                String strEndDate = ztimeStartPlusTime.Date.ToText();
                String strEndTime = ztimeStartPlusTime.Time.ToString();

                timesjson_M = new TimesjsonTimesJson(strStartDate, strStartTime, strEndDate, strEndTime);
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static void subGetInfoByResource(
            //                                              //Get all info from resource.

            int intPkEleetOrEleele_I,
            bool boolIsEleet_I,
            EleentityElementEntityDB eleentity_I,
            PiwentityProcessInWorkflowEntityDB piwentity_I,
            JobjsonJobJson jobjsonJob_I,
            String strPrintshopId_I,
            ProdtypProductType prodtyp_I,
            IConfiguration configuration_I,
            ref int intHours_IO,
            ref int intMinutes_IO,
            ref int intSeconds_IO
            )
        {
            //                                              //Establish connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get all the processes.
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentityAllProcesses;
            List<DynLkjsonDynamicLinkJson> darrdynlkjson;
            ProdtypProductType.subGetWorkflowValidWay(piwentity_I.intPkWorkflow, jobjsonJob_I,
                out darrpiwentityAllProcesses, out darrdynlkjson);
            //                                              //Dictionary to store inputs and outputs of a process.
            prodtyp_I.dicProcessIOs = new Dictionary<int, List<Iofrmpiwjson2IOFromPIWJson2>>();
            //                                              //List to store resource thickness.
            prodtyp_I.darrresthkjsonResThickness = new List<ResthkjsonResourceThicknessJson>();

            List<Iojson1InputOrOutputJson1> darriojson1Input = new List<Iojson1InputOrOutputJson1>();

            //
            if (
                //                                          //It is Postprocess.
                piwentity_I.boolIsPostProcess
                )
            {
                //                                          //It is PostProcess..
                ResResource.subGetInfoResourceByPostProcess(darrpiwentityAllProcesses, piwentity_I,
                    darrdynlkjson, prodtyp_I, jobjsonJob_I, strPrintshopId_I, configuration_I, ref darriojson1Input);
            }
            else
            {
                //                                          //It is normal process.
                ResResource.subGetInfoResourceByNormalProcess(darrpiwentityAllProcesses, piwentity_I,
                    darrdynlkjson, prodtyp_I, jobjsonJob_I, strPrintshopId_I, configuration_I, ref darriojson1Input);
            }

            //                                              //Calculates the needed time for a resource to do a task.
            Iojson1InputOrOutputJson1 iojson1Input = darriojson1Input.FirstOrDefault(io => 
                io.intPkEleetOrEleele == intPkEleetOrEleele_I && io.boolIsEleet == boolIsEleet_I &&
                io.intnPkResource == eleentity_I.intPk);

            if (
                //                                          //Exist the IO.
                iojson1Input != null
                )
            {
                intHours_IO = iojson1Input.intHours;
                intMinutes_IO = iojson1Input.intMinutes;
                intSeconds_IO = iojson1Input.intSeconds;
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subGetInfoResourceByNormalProcess(
            //                                              //Get Info resources from a normal process.

            List<PiwentityProcessInWorkflowEntityDB> darrpiwentityAllProcess_I,
            PiwentityProcessInWorkflowEntityDB piwentityNormalProcess_I,
            List<DynLkjsonDynamicLinkJson> darrdynlkjson_I,
            ProdtypProductType prodtyp_I,
            JobjsonJobJson jobjsonJob_I,
            String strPrintshopId_I,
            IConfiguration configuration_I,
            ref List<Iojson1InputOrOutputJson1> darriojson1Input_M
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get job in order to get date.
            JobentityJobEntityDB jobentity = context.Job.FirstOrDefault(job => job.intJobID == jobjsonJob_I.intJobId);

            prodtyp_I.subAddCalculationsBasedOnJobStatus(jobentity, darrpiwentityAllProcess_I, context);

            //                                              //List of quantityInputs and quantityOutputs.
            //                                              //    for optimization.
            List<IoqytjsonIOQuantityJson> darrioqytjsonIOQuantity = new List<IoqytjsonIOQuantityJson>();

            //                                              //List of waste to propagate.                          
            List<WstpropjsonWasteToPropagateJson> darrwstpropjson = new List<WstpropjsonWasteToPropagateJson>();

            //                                              //The lists are for optimization

            //                                              //Get eleet-s.
            List<EleetentityElementElementTypeEntityDB> darreleetentityAllEleEt = context.ElementElementType.Where(
                eleet => eleet.intPkElementDad == piwentityNormalProcess_I.intPkProcess).ToList();

            //                                              //Get eleele-s.
            List<EleeleentityElementElementEntityDB> darreleeleentityAllEleEle = context.ElementElement.Where(
                eleele => eleele.intPkElementDad == piwentityNormalProcess_I.intPkProcess).ToList();

            //                                              //Get io-s.
            List<IoentityInputsAndOutputsEntityDB> darrioentityAllIO = context.InputsAndOutputs.Where(io =>
                io.intPkWorkflow == piwentityNormalProcess_I.intPkWorkflow &&
                io.intnProcessInWorkflowId == piwentityNormalProcess_I.intProcessInWorkflowId).ToList();

            //                                              //Get ioj-s.
            List<IojentityInputsAndOutputsForAJobEntityDB> darriojentityAllIOJ = context.InputsAndOutputsForAJob.Where(
                ioj =>  ioj.intPkProcessInWorkflow == piwentityNormalProcess_I.intPk &&
                ioj.intJobId == jobjsonJob_I.intJobId).ToList();

            if (
                !prodtyp_I.dicProcessIOs.ContainsKey(piwentityNormalProcess_I.intPk)
                )
            {
                List<Iofrmpiwjson2IOFromPIWJson2> darrioinfrmpiwjson2IosFromPIW;
                ProdtypProductType.subGetProcessInputsAndOutputs(jobjsonJob_I, piwentityNormalProcess_I, prodtyp_I,
                    darreleeleentityAllEleEle, darreleetentityAllEleEt, out darrioinfrmpiwjson2IosFromPIW);

                prodtyp_I.dicProcessIOs.Add(piwentityNormalProcess_I.intPk, darrioinfrmpiwjson2IosFromPIW);
            }

            //                                              //Get the process.
            EleentityElementEntityDB eleentityProcess = context.Element.
                FirstOrDefault(ele => ele.intPk == piwentityNormalProcess_I.intPkProcess);

            double numJobFinalCostNotUsed = 0;

            bool boolWorkflowJobIsReadyNotUsed = true;
            ////                                              //Get the input types.
            //darriojson1Input_IO.AddRange(prodtyp_I.arriojson1GetTypes(piwentityNormalProcess_I, jobjsonJob_I,
            //    strPrintshopId_I, true, true, darrpiwentityAllProcess_I, darrdynlkjson_I, configuration_I, 
            //    ref numJobFinalCostNotUsed, ref darrwstpropjson, ref darrioqytjsonIOQuantity, 
            //    ref boolWorkflowJobIsReadyNotUsed));

            ////                                              //Get the input templates.
            //darriojson1Input_IO.AddRange(prodtyp_I.arriojson1GetTemplates(piwentityNormalProcess_I, jobjsonJob_I,
            //    strPrintshopId_I, true, true, darrpiwentityAllProcess_I, darrdynlkjson_I, configuration_I,
            //    ref numJobFinalCostNotUsed, ref darrwstpropjson, ref darrioqytjsonIOQuantity, 
            //    ref boolWorkflowJobIsReadyNotUsed));

            darriojson1Input_M.AddRange(prodtyp_I.arriojson1GetTypes(true, jobentity, jobjsonJob_I, 
                piwentityNormalProcess_I, darrdynlkjson_I, darreleetentityAllEleEt, darrioentityAllIO, 
                darriojentityAllIOJ, darrpiwentityAllProcess_I, darrioqytjsonIOQuantity, darrwstpropjson, 
                ref numJobFinalCostNotUsed, ref boolWorkflowJobIsReadyNotUsed));

            //                                              //Get the input templates.
            darriojson1Input_M.AddRange(prodtyp_I.arriojson1GetTemplates(true, jobentity, jobjsonJob_I,
                piwentityNormalProcess_I, darrdynlkjson_I, darreleeleentityAllEleEle, darrioentityAllIO, 
                darriojentityAllIOJ, darrpiwentityAllProcess_I, darrioqytjsonIOQuantity, darrwstpropjson, 
                ref numJobFinalCostNotUsed, ref boolWorkflowJobIsReadyNotUsed));

            IoqytjsonIOQuantityJson ioqytjsonWasPropagate = darrioqytjsonIOQuantity.FirstOrDefault(
                ioqyt => ioqyt.intPkProcessInWorkflow == piwentityNormalProcess_I.intPk);

            //                                              //This PIW was not analized or is the first PIW.
            ProdtypProductType.subPropagateWaste(jobjsonJob_I, piwentityNormalProcess_I, prodtyp_I,
                darrwstpropjson, configuration_I, strPrintshopId_I, null, ref darriojson1Input_M);

            ProdtypProductType.CalculateTime(jobjsonJob_I, piwentityNormalProcess_I, configuration_I, strPrintshopId_I,
                ref darriojson1Input_M, null);

            //                                              //Only resources without link have calculations.
            darriojson1Input_M = darriojson1Input_M.Where(res => res.strLink == null).ToList();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subGetInfoResourceByPostProcess(
            //                                              //Get Info resoucese from a postprocess.

            List<PiwentityProcessInWorkflowEntityDB> darrpiwentityAllProcess_I,
            PiwentityProcessInWorkflowEntityDB piwentityPostProcess_I,
            List<DynLkjsonDynamicLinkJson> darrdynlkjson_I,
            ProdtypProductType prodtyp_I,
            JobjsonJobJson jobjsonJob_I,
            String strPrintshopId_I,
            IConfiguration configuration_I,
            ref List<Iojson1InputOrOutputJson1> darriojson1Input_M
            )
        {
            Odyssey2Context context = new Odyssey2Context();
            //                                              //Get job in order to get date.
            JobentityJobEntityDB jobentity = context.Job.FirstOrDefault(job => job.intJobID == jobjsonJob_I.intJobId);

            //                                              //List of quantityInputs and quantityOutputs
            //                                              //    for optimization.
            List<IoqytjsonIOQuantityJson> darrioqytjsonIOQuantity = new List<IoqytjsonIOQuantityJson>();

            //                                              //Get eleet-s.
            List<EleetentityElementElementTypeEntityDB> darreleetentityAll = context.ElementElementType.Where(
                eleet => eleet.intPkElementDad == piwentityPostProcess_I.intPkProcess).ToList();

            //                                              //Get eleele-s.
            List<EleeleentityElementElementEntityDB> darreleeleentityAll = context.ElementElement.Where(
                eleele => eleele.intPkElementDad == piwentityPostProcess_I.intPkProcess).ToList();

            //                                              //Get io-s.
            List<IoentityInputsAndOutputsEntityDB> darrioentityAllIO = context.InputsAndOutputs.Where(io =>
                io.intPkWorkflow == piwentityPostProcess_I.intPkWorkflow &&
                io.intnProcessInWorkflowId == piwentityPostProcess_I.intProcessInWorkflowId).ToList();

            //                                              //Get ioj-s.
            List<IojentityInputsAndOutputsForAJobEntityDB> darriojentityAllIOJ = context.InputsAndOutputsForAJob.Where(
                ioj =>  ioj.intPkProcessInWorkflow == piwentityPostProcess_I.intPk &&
                ioj.intJobId == jobjsonJob_I.intJobId).ToList();

            if (
                !prodtyp_I.dicProcessIOs.ContainsKey(piwentityPostProcess_I.intPk)
                )
            {
                List<Iofrmpiwjson2IOFromPIWJson2> darrioinfrmpiwjson2IosFromPIW;
                ProdtypProductType.subGetProcessInputsAndOutputs(jobjsonJob_I, piwentityPostProcess_I, prodtyp_I,
                    darreleeleentityAll, darreleetentityAll, out darrioinfrmpiwjson2IosFromPIW);

                prodtyp_I.dicProcessIOs.Add(piwentityPostProcess_I.intPk, darrioinfrmpiwjson2IosFromPIW);
            }

            //                                              //Get the inputs and outputs for the process.
            //                                              //Get the process.
            EleentityElementEntityDB eleentityProcess = context.Element.
                FirstOrDefault(ele => ele.intPk == piwentityPostProcess_I.intPkProcess);

            double numJobFinalCost = 0.0;

            ////                                              //Get the input types.
            //darriojson1Input_IO.AddRange(prodtyp_I.arriojson1GetTypesPostProcess(piwentityPostProcess_I, jobjsonJob_I,
            //    strPrintshopId_I, true, true, darrpiwentityAllProcess_I, darrdynlkjson_I, configuration_I,
            //    ref numJobFinalCost, ref darrioqytjsonIOQuantity));

            ////                                              //Get the input templates.
            //darriojson1Input_IO.AddRange(prodtyp_I.arriojson1GetTemplatesPostProcess(piwentityPostProcess_I,
            //    jobjsonJob_I, strPrintshopId_I, true, true, darrpiwentityAllProcess_I, darrdynlkjson_I, configuration_I,
            //    ref numJobFinalCost, ref darrioqytjsonIOQuantity));

            //                                              //Get the input types.
            darriojson1Input_M.AddRange(prodtyp_I.arriojson1GetTypesPostProcess(true, strPrintshopId_I, jobentity,
                jobjsonJob_I, piwentityPostProcess_I, darrdynlkjson_I, darreleetentityAll, darrioentityAllIO,
                darriojentityAllIOJ, darrpiwentityAllProcess_I, configuration_I, darrioqytjsonIOQuantity,
                ref numJobFinalCost));

            //                                              //Get the input templates.
            darriojson1Input_M.AddRange(prodtyp_I.arriojson1GetTemplatesPostProcess(true, strPrintshopId_I, jobentity,
                jobjsonJob_I, piwentityPostProcess_I, darrdynlkjson_I, darreleeleentityAll, darrioentityAllIO,
                darriojentityAllIOJ, darrpiwentityAllProcess_I, configuration_I, darrioqytjsonIOQuantity,
                ref numJobFinalCost));

            ProdtypProductType.CalculateTime(jobjsonJob_I, piwentityPostProcess_I, configuration_I, strPrintshopId_I,
                ref darriojson1Input_M, prodtyp_I.darriojsoninInputsCombinationsAndInputsSelected);

            //                                              //Only resources without link have calculations.
            darriojson1Input_M = darriojson1Input_M.Where(res => res.strLink == null).ToList();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static List<Porjson2PeriodOrRuleJson2> darrporjson2GetPeriodsAndRulesFromDate(
            int intPkPrintshop_I,
            ResResource res_I,
            Date dateFrom_I,
            List<PerentityPeriodEntityDB> darrperentityTemporary_I,
            int? intnEstimateId_I,
            int intJobId_I
            )
        {
            List<Porjson2PeriodOrRuleJson2> darrporjson2 = new List<Porjson2PeriodOrRuleJson2>();

            ResResource.subGetPeriodsFromDate(res_I, dateFrom_I, darrperentityTemporary_I, intnEstimateId_I,
                intJobId_I, ref darrporjson2);

            ResResource.subGetRulesBetweenPeriodsUntilDate(intPkPrintshop_I, res_I, dateFrom_I, ref darrporjson2);

            darrporjson2.Sort();

            return darrporjson2;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subGetPeriodsFromDate(
            
            ResResource res_I,
            Date dateFrom_I,
            List<PerentityPeriodEntityDB> darrperentityTemporary_I,
            int? intnEstimateId_I,
            int intJobId_I,
            ref List<Porjson2PeriodOrRuleJson2> darrporjson2_M
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            List<PerentityPeriodEntityDB> darrperentity;
            if (
                //                                          //From job-workflow.                                   
                intnEstimateId_I == null
                )
            {
                //                                          //Get periods not completed.
                darrperentity = context.Period.Where(per =>
                    per.intPkElement == res_I.intPk && per.strFinalEndDate == null &&
                    //                                      //Not temporary.
                    per.intnEstimateId == null).ToList();
            }
            else
            {
                //                                          //From an estimate.  
                //                                          //Get periods.
                darrperentity = context.Period.Where(per =>
                    per.intPkElement == res_I.intPk &&
                    //                                      //Nor from this job.
                    per.intJobId != intJobId_I &&
                    //                                      //Not temporary.
                    per.intnEstimateId == null).ToList();
            }

            //                                              //Add Periods temporary.
            darrperentity.AddRange(darrperentityTemporary_I);

            //                                              //Get Only periods for this resource.
            darrperentity = darrperentity.Where(per => per.intPkElement == res_I.intPk).ToList();

            foreach (PerentityPeriodEntityDB perentity in darrperentity)
            {
                Date dateStart = perentity.strStartDate.ParseToDate();
                if (
                    dateStart >= dateFrom_I
                    )
                {
                    Time timeStart = perentity.strStartTime.ParseToTime();
                    ZonedTime ztimeStart = ZonedTimeTools.NewZonedTime(dateStart, timeStart);

                    Date dateEnd = perentity.strEndDate.ParseToDate();
                    Time timeEnd = perentity.strEndTime.ParseToTime();
                    ZonedTime ztimeEnd = ZonedTimeTools.NewZonedTime(dateEnd, timeEnd);

                    Porjson2PeriodOrRuleJson2 porjson2 = new Porjson2PeriodOrRuleJson2(ztimeStart, ztimeEnd);

                    darrporjson2_M.Add(porjson2);
                }
            }

            darrporjson2_M.Sort();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subGetRulesBetweenPeriodsUntilDate(
            int intPkPrintshop_I,
            ResResource res_I,
            Date dateFrom_I,
            ref List<Porjson2PeriodOrRuleJson2> darrporjson2_M
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get once rules.
            List<RuleentityRuleEntityDB> darrruleentityOnce = context.Rule.Where(rule =>
                (rule.intnPkResource == res_I.intPk && rule.strFrecuency == ResResource.strOnce) ||
                (rule.intnPkPrintshop == intPkPrintshop_I && rule.intnPkResource == null &&
                rule.strFrecuency == ResResource.strOnce)).ToList();

            foreach (RuleentityRuleEntityDB ruleentity in darrruleentityOnce)
            {
                Date dateStart = ruleentity.strFrecuencyValue.Substring(0, 10).ParseToDate();
                if (
                    dateStart >= dateFrom_I
                    )
                {
                    Time timeStart = ruleentity.strStartTime.ParseToTime();
                    ZonedTime ztimeStart = ZonedTimeTools.NewZonedTime(dateStart, timeStart);

                    Date dateEnd = ruleentity.strFrecuencyValue.Substring(11).ParseToDate();
                    Time timeEnd = ruleentity.strEndTime.ParseToTime();
                    ZonedTime ztimeEnd = ZonedTimeTools.NewZonedTime(dateEnd, timeEnd);

                    Porjson2PeriodOrRuleJson2 porjson2 = new Porjson2PeriodOrRuleJson2(ztimeStart, ztimeEnd);

                    darrporjson2_M.Add(porjson2);
                }
            }

            //                                              //Get all rules.
            List<RuleentityRuleEntityDB> darrruleentity = context.Rule.Where(rule =>
                (rule.intnPkResource == res_I.intPk && rule.strFrecuency != ResResource.strOnce) ||
                (rule.intnPkPrintshop == intPkPrintshop_I && rule.intnPkResource == null &&
                rule.strFrecuency != ResResource.strOnce)).ToList();

            Porjson2PeriodOrRuleJson2 porjson2LastPeriod = (darrporjson2_M.Count > 0) ? darrporjson2_M.Last() : null;
            foreach (RuleentityRuleEntityDB ruleentity in darrruleentity)
            {
                Date dateStartRange = ruleentity.strRangeStartDate.ParseToDate();
                Time timeStartRange = ruleentity.strRangeStartTime.ParseToTime();
                ZonedTime ztimeStartRange = ZonedTimeTools.NewZonedTime(dateStartRange, timeStartRange);

                ZonedTime ztimeEndRange = ZonedTimeTools.NewZonedTime(Date.MaxValue, Time.MinValue);
                if (
                    ruleentity.strRangeEndDate != null
                    )
                {
                    Date dateEndRange = ruleentity.strRangeEndDate.ParseToDate();
                    Time timeEndRange = ruleentity.strRangeEndTime.ParseToTime();
                    ztimeEndRange = ZonedTimeTools.NewZonedTime(dateEndRange, timeEndRange);
                }

                if (
                    (ztimeStartRange.Date > dateFrom_I) ||
                    (ztimeEndRange.Date > dateFrom_I)
                    )
                {
                    Date date = dateFrom_I;
                    int intI = 0;
                    do
                    {
                        date = date + intI;

                        Time timeStart = ruleentity.strStartTime.ParseToTime();
                        ZonedTime ztimeStart = ZonedTimeTools.NewZonedTime(date, timeStart);

                        Time timeEnd = ruleentity.strEndTime.ParseToTime();
                        ZonedTime ztimeEnd = ZonedTimeTools.NewZonedTime(date, timeEnd);

                        if (
                            //                              //Is a daily rule and the day is in the range.
                            ((ruleentity.strFrecuency == ResResource.strDaily) &&
                            (date >= ztimeStartRange.Date)) ||
                            //                              //Is a weekly rule, the day is in the range and applies.
                            ((ruleentity.strFrecuency == ResResource.strWeekly) &&
                            (date >= ztimeStartRange.Date) &&
                            (ruleentity.strFrecuencyValue[(int)date.DayOfWeek] == '1')) ||
                            //                              //Is a monthly rule, the day is in the range and applies.
                            ((ruleentity.strFrecuency == ResResource.strMonthly) &&
                            (date >= ztimeStartRange.Date) &&
                            (ruleentity.strFrecuencyValue[(int)date.Day - 1] == '1')) ||
                            //                              //Is a annually rule, the day is in the range and applies.
                            ((ruleentity.strFrecuency == ResResource.strAnnually) &&
                            (date >= ztimeStartRange.Date) &&
                            (ruleentity.strFrecuencyValue == date.ToString("MMdd")))
                            )
                        {
                            Porjson2PeriodOrRuleJson2 porjson2 = new Porjson2PeriodOrRuleJson2(ztimeStart, ztimeEnd);
                            darrporjson2_M.Add(porjson2);
                        }

                        intI = intI + 1;
                    }
                    /*REPEAT-UNTIL*/
                    while (!(
                        (darrporjson2_M.Count == 0) ||
                        (date >= (dateFrom_I + 1)) ||
                        ((porjson2LastPeriod != null) && (date >= (porjson2LastPeriod.ztimeEnd.Date + 1)))
                        ));
                }
            }

            darrporjson2_M.Sort();
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetResourcePeriodsInIoFromJob(
            int intJobId_I,
            String strPrintshopId_I,
            int intPkProcessInWorkflow_I,
            int intPkResource_I,
            int intPkEleetOrEleele_I,
            bool boolIsEleet_I,
            IConfiguration configuration_I,
            out PeriresjsonPeriodResourceJson[] periresjson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            periresjson_O = null;
            Odyssey2Context context = new Odyssey2Context();

            List<PeriresjsonPeriodResourceJson> darrperiresjson = new List<PeriresjsonPeriodResourceJson>();

            JobjsonJobJson jobjsonJob_O = null;
            intStatus_IO = 401;
            if (
                JobJob.boolIsValidJobId(
                    intJobId_I, strPrintshopId_I, configuration_I, out jobjsonJob_O, ref strUserMessage_IO, 
                    ref strDevMessage_IO)
                )
            {
                ResResource res = ResResource.resFromDB(intPkResource_I, false);

                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Resource not found.";
                if (
                    res != null
                    )
                {
                    intStatus_IO = 403;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "ProcessInWorkflow no found.";

                    PiwentityProcessInWorkflowEntityDB piwentity = context.ProcessInWorkflow.FirstOrDefault(
                        piw => piw.intPk == intPkProcessInWorkflow_I);

                    if (
                        piwentity != null
                        )
                    {
                        PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);
                        int? intnPkEleet = null;
                        int? intnPkEleele = null;
                        if (
                            boolIsEleet_I
                            )
                        {
                            intnPkEleet = intPkEleetOrEleele_I;
                        }
                        else
                        {
                            intnPkEleele = intPkEleetOrEleele_I;
                        }

                        //                                  //Get all Resource´Periods.
                        List<PerentityPeriodEntityDB> darrperentityPeriod = context.Period.Where(per =>
                            per.intJobId == intJobId_I && per.intPkElement == res.intPk && 
                            per.intnPkCalculation == null &&
                            per.intProcessInWorkflowId == piwentity.intProcessInWorkflowId &&
                            per.intnPkElementElementType == intnPkEleet &&
                            per.intnPkElementElement == intnPkEleele).ToList();

                        foreach (PerentityPeriodEntityDB perentity in darrperentityPeriod)
                        {
                            //                              //Get Date.
                            Date dateStartPeriod = perentity.strStartDate.ParseToDate();
                            Time timeStartPeriod = perentity.strStartTime.ParseToTime();
                            ZonedTime ztimeStartPeriod = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                                dateStartPeriod, timeStartPeriod, ps.strTimeZone);

                            int intDayOfWeek = (int)ztimeStartPeriod.Date.DayOfWeek;

                            //                              //Get Sunday date.
                            Date dateSunday = dateStartPeriod - intDayOfWeek;

                            PeriresjsonPeriodResourceJson periresjson = new PeriresjsonPeriodResourceJson(
                                perentity.intPk, ztimeStartPeriod.Date.ToString(), ztimeStartPeriod.Time.ToString(),
                                dateSunday.ToText());
                            darrperiresjson.Add(periresjson);
                        }

                        intStatus_IO = 200;
                        strUserMessage_IO = "";
                        strDevMessage_IO = "";
                    }
                }
            }

            periresjson_O = darrperiresjson.ToArray();
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool boolIsAddableAlKindOfResources(
            //                                              //Evaluate resources and return true if resource can be
            //                                              //      added.
            //                                              //True when:
            //                                              //1.- Media type.
            //                                              //      -Is a media type without attributes.
            //                                              //      -Is a media type with attributes but has not 
            //                                              //          dimension attributes.
            //                                              //      -Is a media type with attributes and all the 
            //                                              //          dimensions attributes.
            //                                              //2.- Component type.
            //                                              //      -Has Thickness attribute and has unit.
            //                                              //      -Has not Thickness.
            //                                              //3.-MiscConsumable type.
            //                                              //      -Has Height attribute and has unit.
            //                                              //      -Has not Height.
            //                                              //4.-Device type.
            //                                              //      -Has Lift attribute and has unit.
            //                                              //      -Has not Lift.
            //                                              //5.- Other type.
            //                                              //      -Always true.

            int intPkType_I,
            List<Attrjson5AttributeJson5> darrattjson5_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            bool boolIsAddable = false;

            //                                              //Get the resource type.
            EtElementTypeAbstract et = EtElementTypeAbstract.etFromDB(intPkType_I);
            RestypResourceType restyp = (et.strResOrPro == EtElementTypeAbstract.strResource) ? (RestypResourceType)et :
                null;
            intStatus_IO = 401;
            strUserMessage_IO = "Something wrong.";
            strDevMessage_IO = "Resource type not found.";
            if (
                restyp != null
                )
            {
                /*CASE*/
                if (
                    //                                      //Media type.
                    (restyp.strXJDFTypeId == "Media") &&
                    //                                      //It has attributes.
                    (darrattjson5_I.Count > 0)
                    )
                {
                    boolIsAddable = ResResource.boolMediaIsAddable(intPkType_I, darrattjson5_I, ref intStatus_IO,
                        ref strUserMessage_IO, ref strDevMessage_IO);
                }
                else if (
                    //                                      //Component type.
                    (restyp.strXJDFTypeId == "Component") &&
                    //                                      //It has attributes.
                    (darrattjson5_I.Count > 0)
                    )
                {
                    boolIsAddable = ResResource.boolComponentIsAddable(intPkType_I, darrattjson5_I, ref intStatus_IO,
                        ref strUserMessage_IO, ref strDevMessage_IO);
                }
                else if (
                    //                                      //MiscConsumable type.
                    (restyp.strXJDFTypeId == "MiscConsumable") &&
                    //                                      //It has attributes.
                    (darrattjson5_I.Count > 0)
                    )
                {
                    boolIsAddable = ResResource.boolMiscConsumableIsAddable(intPkType_I, darrattjson5_I,
                        ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);
                }
                else if (
                    //                                      //Device type.
                    (restyp.strXJDFTypeId == "Device") &&
                    //                                      //It has attributes.
                    (darrattjson5_I.Count > 0)
                    )
                {
                    boolIsAddable = ResResource.boolDeviceIsAddable(intPkType_I, darrattjson5_I,
                        ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);
                }
                else
                {
                    //                                      //All others resource´s type.
                    boolIsAddable = true;
                }
                /*END-CASE*/
            }

            intStatus_IO = boolIsAddable ? 200 : 401;
            return boolIsAddable;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static bool boolMediaIsAddable(
            //                                              //True:
            //                                              //      -Is a not media type.
            //                                              //      -Is a media type without attributes.
            //                                              //      -Is a media type with attributes but has not 
            //                                              //          dimension attributes.
            //                                              //      -Is a media type with attributes and all the 
            //                                              //          dimensions attributes.

            int intPkType_I,
            List<Attrjson5AttributeJson5> darrattjson5_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            bool boolIsAddable = false;
            //                                              //Connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get mediaUnit attribute in order to know if is a media
            //                                              //      roll or not.
            //                                              //MediaUnit.
            int intPkMediaUnitAttribute = (from attrentity in context.Attribute
                                           join attretentity in context.AttributeElementType
                                           on attrentity.intPk equals attretentity.intPkAttribute
                                           where attretentity.intPkElementType == intPkType_I &&
                                           attrentity.strXJDFName == "MediaUnit"
                                           select attrentity).ToList()[0].intPk;

            //                                              //Find mediaUnit attribute .
            Attrjson5AttributeJson5 attrjson5MediaUnitAttribute = darrattjson5_I.FirstOrDefault(attr =>
            attr.strAscendant.StartsWith(intPkMediaUnitAttribute + ""));

            //                                              //Get WidthUnit attribute.
            //                                              //WidthUnit.
            int intPkWidthUnitAttribute = (from attrentity in context.Attribute
                                           join attretentity in context.AttributeElementType
                                           on attrentity.intPk equals attretentity.intPkAttribute
                                           where attretentity.intPkElementType == intPkType_I &&
                                           attrentity.strXJDFName == "WidthUnit"
                                           select attrentity).ToList()[0].intPk;

            //                                              //Find WidthUnit attribute .
            Attrjson5AttributeJson5 attrjson5WidthUnitAttribute = darrattjson5_I.FirstOrDefault(attr =>
            attr.strAscendant.StartsWith(intPkWidthUnitAttribute + ""));

            //                                              //Get LengthUnit attribute.
            //                                              //LengthhUnit.
            int intPkLengthUnitAttribute = (from attrentity in context.Attribute
                                            join attretentity in context.AttributeElementType
                                            on attrentity.intPk equals attretentity.intPkAttribute
                                            where attretentity.intPkElementType == intPkType_I &&
                                            attrentity.strXJDFName == "LengthUnit"
                                            select attrentity).ToList()[0].intPk;

            //                                              //Find LengthUnit attribute .
            Attrjson5AttributeJson5 attrjson5LengthUnitAttribute = darrattjson5_I.FirstOrDefault(attr =>
            attr.strAscendant.StartsWith(intPkLengthUnitAttribute + ""));

            //                                              //Get the pks of dimensions attributes.
            int[] arrintPkDimensionsAttribute = ResResource.arrintPkDimensionsAttributeGet(intPkType_I);

            //                                              //Find the first attribute.
            Attrjson5AttributeJson5 attrjson5DimensionsAttribute1 = darrattjson5_I.FirstOrDefault(attr =>
            attr.strAscendant.StartsWith(arrintPkDimensionsAttribute[0] + ""));

            //                                              //Find the second attribute.
            Attrjson5AttributeJson5 attrjson5DimensionsAttribute2 = darrattjson5_I.FirstOrDefault(attr =>
            attr.strAscendant.StartsWith(arrintPkDimensionsAttribute[1] + ""));

            //                                              //Find the third attribute.
            Attrjson5AttributeJson5 attrjson5DimensionsAttribute3 = darrattjson5_I.FirstOrDefault(attr =>
            attr.strAscendant.StartsWith(arrintPkDimensionsAttribute[2] + ""));

            //                                              //Get the pk of Thickness attribute.
            int intPkThickness = (from attrentity in context.Attribute
                                  join attretentity in context.AttributeElementType
                                  on attrentity.intPk equals attretentity.intPkAttribute
                                  where attretentity.intPkElementType == intPkType_I &&
                                  attrentity.strXJDFName == "Thickness"
                                  select attrentity).FirstOrDefault().intPk;

            //                                              //Get the pk of ThicknessUnit attribute.
            int intPkThicknessUnit = (from attrentity in context.Attribute
                                      join attretentity in context.AttributeElementType
                                      on attrentity.intPk equals attretentity.intPkAttribute
                                      where attretentity.intPkElementType == intPkType_I &&
                                      attrentity.strXJDFName == "ThicknessUnit"
                                      select attrentity).FirstOrDefault().intPk;

            //                                              //Find the Thickness attribute.
            Attrjson5AttributeJson5 attrjson5ThicknessAttribute = darrattjson5_I.FirstOrDefault(attr =>
            attr.strAscendant.StartsWith(intPkThickness + ""));

            //                                              //Find the ThicknessUnit attribute.
            Attrjson5AttributeJson5 attrjsonThicknessUnitAttribute = darrattjson5_I.FirstOrDefault(attr =>
            attr.strAscendant.StartsWith(intPkThicknessUnit + ""));

            //                                              //Media's resource can has thickness or not.
            intStatus_IO = 401;
            strUserMessage_IO = "You need to add Thicknees  and ThicknessUnit or none of them.";
            strDevMessage_IO = "";
            if (
                //                                          //It contains neither.
                ((attrjson5ThicknessAttribute == null) && (attrjsonThicknessUnitAttribute == null)) ||
                //                                          //It contains both
                ((attrjson5ThicknessAttribute != null) && (attrjsonThicknessUnitAttribute != null))
                )
            {
                if (
                    //                                      //It´s not roll.
                    attrjson5MediaUnitAttribute == null ||
                    (attrjson5MediaUnitAttribute != null && attrjson5MediaUnitAttribute.strValue == "Sheet")
                    )
                {
                    intStatus_IO = 402;
                    strUserMessage_IO = "You need to add the 3 dimension attributes or none of them. Dimension " +
                        "attributes: Width, Length and Dimension Unit or you need to add 4 dimension attributes or" +
                        " none of them. Dimensions attribute: Width, Length, WidthUnit, LengthUnit.";
                    strDevMessage_IO = "";
                    if (
                    //                                      //It contains none of them.
                    ((attrjson5DimensionsAttribute1 == null) && (attrjson5DimensionsAttribute2 == null) &&
                    (attrjson5DimensionsAttribute3 == null) && (attrjson5LengthUnitAttribute == null) &&
                    (attrjson5WidthUnitAttribute == null)) ||
                    //                                      //It contains all of them.
                    ((attrjson5DimensionsAttribute1 != null) && (attrjson5DimensionsAttribute2 != null) &&
                    ((attrjson5DimensionsAttribute3 != null && attrjson5LengthUnitAttribute == null &&
                    attrjson5WidthUnitAttribute == null) ||
                    (attrjson5DimensionsAttribute3 == null && attrjson5LengthUnitAttribute != null &&
                    attrjson5WidthUnitAttribute != null)))
                    )
                    {
                        boolIsAddable = true;

                        intStatus_IO = 200;
                        strUserMessage_IO = "";
                        strDevMessage_IO = "";
                    }
                }
                else
                {
                    //                                      //To easy code.
                    if (
                        //                                  //There are not attibutes.
                        darrattjson5_I.Count == 1 && attrjson5MediaUnitAttribute.strValue != null
                        )
                    {
                        strUserMessage_IO = "A roll media must have at least lengthUnit or DimensionsUnit";
                    }
                    else
                    {
                        strUserMessage_IO = "You need to add the 3 dimension attributes or none of them. Dimension" +
                            " attributes: Width, Length and Dimension Unit or you need to add 4 dimension attributes" +
                            " or none of them. Dimensions attribute: Width, Length, WidthUnit, LengthUnit.";
                    }
                    //                                      //It's roll.
                    intStatus_IO = 402;
                    strDevMessage_IO = "";
                    if (
                        //                                  //Must have to containt lengthUnit or DimensionsUnit.
                        (
                        (attrjson5DimensionsAttribute1 == null) && (attrjson5DimensionsAttribute2 == null) &&
                        (attrjson5WidthUnitAttribute == null) &&
                        ((attrjson5DimensionsAttribute3 != null && attrjson5LengthUnitAttribute == null) ||
                        (attrjson5LengthUnitAttribute != null && attrjson5DimensionsAttribute3 == null))
                        ) ||
                        //                                  //It contains all of them.
                        ((attrjson5DimensionsAttribute1 != null) && (attrjson5DimensionsAttribute2 != null) &&
                        ((attrjson5DimensionsAttribute3 != null && attrjson5LengthUnitAttribute == null &&
                        attrjson5WidthUnitAttribute == null) ||
                        (attrjson5DimensionsAttribute3 == null && attrjson5LengthUnitAttribute != null &&
                        attrjson5WidthUnitAttribute != null)))
                        )
                    {
                        boolIsAddable = true;

                        intStatus_IO = 200;
                        strUserMessage_IO = "";
                        strDevMessage_IO = "";
                    }
                }
            }

            return boolIsAddable;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static bool boolComponentIsAddable(
            //                                              //Verify is a resource type component has both attributes
            //                                              //      Thicknees and ThicknessUnit or none of them.

            int intPkType_I,
            List<Attrjson5AttributeJson5> darrattjson5_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Connection.
            Odyssey2Context context = new Odyssey2Context();

            bool boolIsAddable = false;

            boolIsAddable = false;
            strUserMessage_IO = "You need to add the Thickness and ThicknessUnit attributes or none of them";

            //                                      //Get the pk of Thickness attribute.
            int intPkThickness = (from attrentity in context.Attribute
                                  join attretentity in context.AttributeElementType
                                  on attrentity.intPk equals attretentity.intPkAttribute
                                  where attretentity.intPkElementType == intPkType_I &&
                                  attrentity.strXJDFName == "Thickness"
                                  select attrentity).FirstOrDefault().intPk;

            //                                      //Get the pk of ThicknessUnit attribute.
            int intPkThicknessUnit = (from attrentity in context.Attribute
                                      join attretentity in context.AttributeElementType
                                      on attrentity.intPk equals attretentity.intPkAttribute
                                      where attretentity.intPkElementType == intPkType_I &&
                                      attrentity.strXJDFName == "ThicknessUnit"
                                      select attrentity).FirstOrDefault().intPk;


            //                                      //Find the Thickness attribute.
            Attrjson5AttributeJson5 attrjson5ThicknessAttribute = darrattjson5_I.FirstOrDefault(attr =>
            attr.strAscendant.StartsWith(intPkThickness + ""));

            //                                      //Find the ThicknessUnit attribute.
            Attrjson5AttributeJson5 attrjsonThicknessUnitAttribute = darrattjson5_I.FirstOrDefault(attr =>
            attr.strAscendant.StartsWith(intPkThicknessUnit + ""));

            if (
                //                                  //It contains neither.
                ((attrjson5ThicknessAttribute == null) && (attrjsonThicknessUnitAttribute == null)) ||
                //                                  //It contains both
                ((attrjson5ThicknessAttribute != null) && (attrjsonThicknessUnitAttribute != null))
                )
            {
                boolIsAddable = true;
                strUserMessage_IO = "";
            }

            return boolIsAddable;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static bool boolMiscConsumableIsAddable(
            //                                              //Verify is a resource type MiscConsumable has both 
            //                                              //      attributes Height and HeightUnit or none of them.

            int intPkType_I,
            List<Attrjson5AttributeJson5> darrattjson5_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Connection.
            Odyssey2Context context = new Odyssey2Context();

            bool boolIsAddable = false;

            boolIsAddable = false;
            strUserMessage_IO = "You need to add the Height and HeightUnit attributes or none of them";

            //                                      //Get the pk of Thickness attribute.
            int intPkHeight = (from attrentity in context.Attribute
                                  join attretentity in context.AttributeElementType
                                  on attrentity.intPk equals attretentity.intPkAttribute
                                  where attretentity.intPkElementType == intPkType_I &&
                                  attrentity.strXJDFName == "Height"
                                  select attrentity).FirstOrDefault().intPk;

            //                                      //Get the pk of ThicknessUnit attribute.
            int intPkHeightUnit = (from attrentity in context.Attribute
                                      join attretentity in context.AttributeElementType
                                      on attrentity.intPk equals attretentity.intPkAttribute
                                      where attretentity.intPkElementType == intPkType_I &&
                                      attrentity.strXJDFName == "HeightUnit"
                                      select attrentity).FirstOrDefault().intPk;


            //                                      //Find the Thickness attribute.
            Attrjson5AttributeJson5 attrjson5HeightAttribute = darrattjson5_I.FirstOrDefault(attr =>
            attr.strAscendant.StartsWith(intPkHeight + ""));

            //                                      //Find the ThicknessUnit attribute.
            Attrjson5AttributeJson5 attrjsonHeightUnitAttribute = darrattjson5_I.FirstOrDefault(attr =>
            attr.strAscendant.StartsWith(intPkHeightUnit + ""));

            if (
                //                                  //It contains neither.
                ((attrjson5HeightAttribute == null) && (attrjsonHeightUnitAttribute == null)) ||
                //                                  //It contains both
                ((attrjson5HeightAttribute != null) && (attrjsonHeightUnitAttribute != null))
                )
            {
                boolIsAddable = true;
                strUserMessage_IO = "";
            }

            return boolIsAddable;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static bool boolDeviceIsAddable(
            //                                              //Verify is a resource type Device has both 
            //                                              //      attributes Lift and LiftUnit or none of them.

            int intPkType_I,
            List<Attrjson5AttributeJson5> darrattjson5_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Connection.
            Odyssey2Context context = new Odyssey2Context();

            bool boolIsAddable = false;

            boolIsAddable = false;
            strUserMessage_IO = "You need to add the Lift and LiftUnit attributes or none of them";

            //                                      //Get the pk of Thickness attribute.
            int intPkLift = (from attrentity in context.Attribute
                               join attretentity in context.AttributeElementType
                               on attrentity.intPk equals attretentity.intPkAttribute
                               where attretentity.intPkElementType == intPkType_I &&
                               attrentity.strXJDFName == "Lift"
                               select attrentity).FirstOrDefault().intPk;

            //                                      //Get the pk of ThicknessUnit attribute.
            int intPkLiftUnit = (from attrentity in context.Attribute
                                   join attretentity in context.AttributeElementType
                                   on attrentity.intPk equals attretentity.intPkAttribute
                                   where attretentity.intPkElementType == intPkType_I &&
                                   attrentity.strXJDFName == "LiftUnit"
                                   select attrentity).FirstOrDefault().intPk;


            //                                      //Find the Thickness attribute.
            Attrjson5AttributeJson5 attrjson5LiftAttribute = darrattjson5_I.FirstOrDefault(attr =>
            attr.strAscendant.StartsWith(intPkLift + ""));

            //                                      //Find the ThicknessUnit attribute.
            Attrjson5AttributeJson5 attrjsonLiftUnitAttribute = darrattjson5_I.FirstOrDefault(attr =>
            attr.strAscendant.StartsWith(intPkLiftUnit + ""));

            if (
                //                                  //It contains neither.
                ((attrjson5LiftAttribute == null) && (attrjsonLiftUnitAttribute == null)) ||
                //                                  //It contains both
                ((attrjson5LiftAttribute != null) && (attrjsonLiftUnitAttribute != null))
                )
            {
                boolIsAddable = true;
                strUserMessage_IO = "";
            }

            return boolIsAddable;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subGetAttributesToBlock(
            EtentityElementTypeEntityDB etentityResourceType_I,
            ref List<int> darrintPkAttributesToBlock_M
            )
        {
            //                                              //Connection.
            Odyssey2Context context = new Odyssey2Context();

            if (
                etentityResourceType_I.strXJDFTypeId == "Media"
                )
            {
                darrintPkAttributesToBlock_M =
                    (from attrentity in context.Attribute
                     join attretentity in context.AttributeElementType
                     on attrentity.intPk equals attretentity.intPkAttribute
                     where attretentity.intPkElementType == etentityResourceType_I.intPk &&
                     (attrentity.strXJDFName == "Width" ||
                     attrentity.strXJDFName == "Length" ||
                     attrentity.strXJDFName == "DimensionsUnit" ||
                     attrentity.strXJDFName == "Thickness" ||
                     attrentity.strXJDFName == "ThicknessUnit" ||
                     attrentity.strXJDFName == "WidthUnit" ||
                     attrentity.strXJDFName == "LengthUnit" ||
                     attrentity.strXJDFName == "MediaUnit")
                     select attrentity.intPk).ToList();
            }
            else if (
                etentityResourceType_I.strXJDFTypeId == "Device"
                )
            {
                darrintPkAttributesToBlock_M =
                    (from attrentity in context.Attribute
                     join attretentity in context.AttributeElementType
                     on attrentity.intPk equals attretentity.intPkAttribute
                     where attretentity.intPkElementType == etentityResourceType_I.intPk &&
                     (attrentity.strXJDFName == "Lift" ||
                     attrentity.strXJDFName == "LiftUnit")
                     select attrentity.intPk).ToList();
            }
            else if (
                etentityResourceType_I.strXJDFTypeId == "Component"
                )
            {
                darrintPkAttributesToBlock_M =
                    (from attrentity in context.Attribute
                     join attretentity in context.AttributeElementType
                     on attrentity.intPk equals attretentity.intPkAttribute
                     where attretentity.intPkElementType == etentityResourceType_I.intPk &&
                     (attrentity.strXJDFName == "Thickness" ||
                     attrentity.strXJDFName == "ThicknessUnit")
                     select attrentity.intPk).ToList();
            }
            else if (
                etentityResourceType_I.strXJDFTypeId == "MiscConsumable"
                )
            {
                darrintPkAttributesToBlock_M =
                    (from attrentity in context.Attribute
                     join attretentity in context.AttributeElementType
                     on attrentity.intPk equals attretentity.intPkAttribute
                     where attretentity.intPkElementType == etentityResourceType_I.intPk &&
                     (attrentity.strXJDFName == "Height" ||
                     attrentity.strXJDFName == "HeightUnit")
                     select attrentity.intPk).ToList();
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void GetNumericalAttributesToValidate(
            RestypResourceType restyp_I,
            Odyssey2Context context_I,
            ref List<int> darrintPkNumericalAttributesToValidate_M
            )
        {
            if (
                restyp_I.boolIsMedia()
                //restyp_I.strXJDFTypeId == "Media"
                )
            {
                darrintPkNumericalAttributesToValidate_M =
                    (from attrentity in context_I.Attribute
                     join attretentity in context_I.AttributeElementType
                     on attrentity.intPk equals attretentity.intPkAttribute
                     where attretentity.intPkElementType == restyp_I.intPk &&
                     (attrentity.strXJDFName == "Width" ||
                     attrentity.strXJDFName == "Length" ||
                     attrentity.strXJDFName == "Thickness")
                     select attrentity.intPk).ToList();
            }
            else if (
                restyp_I.strXJDFTypeId == "Device"
                )
            {
                darrintPkNumericalAttributesToValidate_M =
                    (from attrentity in context_I.Attribute
                     join attretentity in context_I.AttributeElementType
                     on attrentity.intPk equals attretentity.intPkAttribute
                     where attretentity.intPkElementType == restyp_I.intPk &&
                     attrentity.strXJDFName == "Lift"
                     select attrentity.intPk).ToList();
            }
            else if (
                restyp_I.strXJDFTypeId == "Component"
                )
            {
                darrintPkNumericalAttributesToValidate_M =
                    (from attrentity in context_I.Attribute
                     join attretentity in context_I.AttributeElementType
                     on attrentity.intPk equals attretentity.intPkAttribute
                     where attretentity.intPkElementType == restyp_I.intPk &&
                     attrentity.strXJDFName == "Thickness"
                     select attrentity.intPk).ToList();
            }
            else if (
                restyp_I.strXJDFTypeId == "MiscConsumable"
                )
            {
                darrintPkNumericalAttributesToValidate_M =
                    (from attrentity in context_I.Attribute
                     join attretentity in context_I.AttributeElementType
                     on attrentity.intPk equals attretentity.intPkAttribute
                     where attretentity.intPkElementType == restyp_I.intPk &&
                     attrentity.strXJDFName == "Height"
                     select attrentity.intPk).ToList();
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static int[] arrintPkDimensionsAttributeGet(
            //                                              //Returns:
            //                                              //      Pk for Width attribute.
            //                                              //      Pk for Height attribute.
            //                                              //      Pk for DimensionsUnit attribute.

            //                                              //Pk of a Media Type.
            int intPkType_I
            )
        {
            int[] arrintPks = new int[3];

            //                                              //Connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Width.
            arrintPks[0] = (from attrentity in context.Attribute
                            join attretentity in context.AttributeElementType
                            on attrentity.intPk equals attretentity.intPkAttribute
                            where attretentity.intPkElementType == intPkType_I &&
                            attrentity.strXJDFName == "Width"
                            select attrentity).ToList()[0].intPk;

            //                                              //Length.
            arrintPks[1] = (from attrentity in context.Attribute
                            join attretentity in context.AttributeElementType
                            on attrentity.intPk equals attretentity.intPkAttribute
                            where attretentity.intPkElementType == intPkType_I &&
                            attrentity.strXJDFName == "Length"
                            select attrentity).ToList()[0].intPk;

            //                                              //DimensionsUnit.
            arrintPks[2] = (from attrentity in context.Attribute
                            join attretentity in context.AttributeElementType
                            on attrentity.intPk equals attretentity.intPkAttribute
                            where attretentity.intPkElementType == intPkType_I && 
                            attrentity.strXJDFName == "DimensionsUnit"
                            select attrentity).ToList()[0].intPk;

            return arrintPks;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static EleentityElementEntityDB eleentityResourceWorkingAsThickness(
            int intPkWorkflow_I,
            int intJobId_I,
            Odyssey2Context context_I
            )
        {
            //                                              //Resource to return.
            EleentityElementEntityDB eleentityThicknessResource = null;

            //                                              //Find IO working as thickness.
            IoentityInputsAndOutputsEntityDB ioentity = context_I.InputsAndOutputs.FirstOrDefault(io => 
                io.intPkWorkflow == intPkWorkflow_I && io.boolnThickness == true);

            if (
                ioentity != null
                )
            {
                //                                          //Verify if the IO has a resource set at IOJ.
                //                                          //Can be a resource setted in the WFJob.
                IojentityInputsAndOutputsForAJobEntityDB iojentityWithThickness =
                         (from piwentity in context_I.ProcessInWorkflow
                         join iojentity in context_I.InputsAndOutputsForAJob
                         on piwentity.intPk equals iojentity.intPkProcessInWorkflow
                         where piwentity.intPkWorkflow == intPkWorkflow_I &&
                         piwentity.intProcessInWorkflowId == ioentity.intnProcessInWorkflowId &&
                         iojentity.intnPkElementElementType == ioentity.intnPkElementElementType &&
                         iojentity.intnPkElementElement == ioentity.intnPkElementElement &&
                         iojentity.intJobId == intJobId_I
                         select iojentity).FirstOrDefault();
                /*CASE*/
                if (
                    //                                      //Resource thickeness is setted en wfjob.
                    iojentityWithThickness != null
                    )
                {
                    //                                      //Find resource.
                    eleentityThicknessResource = context_I.Element.FirstOrDefault(ele =>
                        ele.intPk == iojentityWithThickness.intPkResource);
                }
                else if (
                    ioentity.intnPkResource != null
                    )
                {
                    //                                      //Find resource.
                    eleentityThicknessResource = context_I.Element.FirstOrDefault(ele =>
                        ele.intPk == ioentity.intnPkResource);
                }
                /*END-CASE*/
            }

            return eleentityThicknessResource;
        }

        //--------------------------------------------------------------------------------------------------------------
        public bool boolMediaRoll(
            //                                              //Receive a resource an verify if is Media roll type.

            )
        {
            bool boolMediaRoll = false;

            //                                              //Connection.
            Odyssey2Context context = new Odyssey2Context();

            RestypResourceType restyp = this.restypBelongsTo;

            if (
                restyp.strCustomTypeId == "XJDFMedia"
                )
            {
                //                                              //Get mediaUnit attribute in order to know if is a media
                //                                              //      roll or not.
                //                                              //MediaUnit.
                int intPkMediaUnitAttribute = (from attrentity in context.Attribute
                                               join attretentity in context.AttributeElementType
                                               on attrentity.intPk equals attretentity.intPkAttribute
                                               where attretentity.intPkElementType == this.restypBelongsTo.intPk &&
                                               attrentity.strXJDFName == "MediaUnit"
                                               select attrentity).ToList()[0].intPk;

                ValentityValueEntityDB valentity = context.Value.FirstOrDefault(val =>
                    val.intPkAttribute == intPkMediaUnitAttribute && val.intPkElement == this.intPk);

                if (
                    valentity != null &&
                    valentity.strValue == "Roll"
                    )
                {
                    boolMediaRoll = true;
                }
            }

            return boolMediaRoll;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/