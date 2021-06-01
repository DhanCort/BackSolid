/*TASK RP.ATTRIBUTE*/
using Microsoft.Extensions.Configuration;
using Odyssey2Backend.App;
using Odyssey2Backend.DB_Odyssey2;
using Odyssey2Backend.JsonTypes;
using Odyssey2Backend.PrintShop;
using Odyssey2Backend.XJDF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//                                                          //AUTHOR: Towa (VSTD - Victor Torres).
//                                                          //CO-AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //DATE: November 11, 2019.

namespace Odyssey2Backend.Infrastructure
{

    //==================================================================================================================
    public class AttrAttribute
    {

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        private readonly int intPk_Z;
        public int intPk { get { return this.intPk_Z; } }

        private String strCustomName_Z;
        public String strCustomName { get { return this.strCustomName_Z; } }

        private String strXJDFName_Z;
        public String strXJDFName { get { return this.strXJDFName_Z; } }

        //                                                  //Attributes and elements is expressed using a simple
        //                                                  //      Extended Backus-Naur Form notation.

        //                                                  //The cardinality values are:
        //                                                  //            - Empty string, required
        //                                                  //  ?         - Optional
        //                                                  //  +         - Occur one or more times
        //                                                  //  *         - Occur zero or more times
        //                                                  //  T = "V"   - Default
        private String strCardinality_Z;
        public String strCardinality { get { return this.strCardinality_Z; } }

        private String strDatatype_Z;
        public String strDatatype { get { return this.strDatatype_Z; } }

        private String strDescription_Z { get; }
        public String strDescription { get { return this.strDescription_Z; } }

        private String strScope_Z;
        public String strScope { get { return this.strScope_Z; } }

        private int? intWebsiteElementId_Z = null;
        public int? intWebsiteElementId { get { return this.intWebsiteElementId_Z; } }
        public ValjsonValueJson[] arrstrValues { get; set; }


        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTOR.

        //--------------------------------------------------------------------------------------------------------------

        public AttrAttribute(
            int intPk_I,
            //                                              //Name of the element or attribute.
            String strCustomName_I,
            //                                              //XJDF name.
            String strXJDFName_I,
            //                                              //Cardinality of the element or attribute.

            String strCardinality_I,
            //                                              //Data type of the element or attribute.
            String strDatatype_I,
            //                                              //Description of the element or attribute.
            String strDescription_I,
            //                                              //Scope of the element or attribute
            String strScope_I,
            //                                              //Id from Wisnet.
            int? intWebsiteElementId_I,
            //                                              //Values of the attributes.
            ValjsonValueJson[] arrstrValues_I
            )
        {
            this.intPk_Z = intPk_I;
            this.strCustomName_Z = strCustomName_I;
            this.strXJDFName_Z = strXJDFName_I;
            this.strCardinality_Z = strCardinality_I;
            this.strDatatype_Z = strDatatype_I;
            this.strDescription_Z = strDescription_I;
            this.strScope_Z = strScope_I;
            this.intWebsiteElementId_Z = intWebsiteElementId_I;
            this.arrstrValues = arrstrValues_I;
        }
        
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public static String[] arrstrGetValues(
            //                                              //Get the values for an attr, if returns null the values are
            //                                              //      not a enumaration.

            //                                              //Pk of the attr.
            int intPk_I
            )
        {
            String[] arrstr = new String[0];

            Odyssey2Context context = new Odyssey2Context();
            AttrentityAttributeEntityDB attrentity = context.Attribute.FirstOrDefault(attrentity =>
                attrentity.intPk == intPk_I);
            if (
                attrentity != null &&
                attrentity.strEnumAssoc != null &&
                attrentity.strEnumAssoc != ""
                )
            {
                String strEnumName = attrentity.strEnumAssoc;
                arrstr = Odyssey2.arrstrGetEnumFromDB(strEnumName);
            }

            return arrstr;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static AttrAttribute attrFromDB(
            int intPk_I
            )
        {
            AttrAttribute attr = null;
            if (
                //                                          //It is a invalid primary key.
                intPk_I > 0
                )
            {
                //                                          //Create the connection.
                Odyssey2Context context = new Odyssey2Context();

                AttrentityAttributeEntityDB attrentity = context.Attribute.FirstOrDefault(attrentity =>
                    attrentity.intPk == intPk_I);

                if (
                    attrentity != null
                    )
                {
                    attr = new AttrAttribute(attrentity.intPk, attrentity.strCustomName, attrentity.strXJDFName,
                        attrentity.strCardinality, attrentity.strDatatype, attrentity.strDescription,
                        attrentity.strScope, attrentity.intWebsiteAttributeId, null);
                }
            }
            return attr;
        }

        //--------------------------------------------------------------------------------------------------------------
        public List<ValjsonValueJson> darrvaljsonGetValuesFromWisnet(
            )
        {
            //                                          //Get values form Wisnet.
            String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                GetSection("Odyssey2Settings")["urlWisnetApi"];
            Task<List<ValjsonValueJson>> Task_darrvaljson = HttpTools<ValjsonValueJson>.GetListAsyncToEndPoint(
                  strUrlWisnet + "/PrintshopData/attributeValues/" + this.intWebsiteElementId);
            Task_darrvaljson.Wait();

            //                                          //Initialize the array
            List<ValjsonValueJson> darrstrValues = null;
            if (
                Task_darrvaljson.Result != null
                )
            {
                darrstrValues = Task_darrvaljson.Result;
            }

            return darrstrValues;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetValue(
            //                                              //Pk of the value or Pk of the template.
            int intValuePk_I,
            bool boolIsCost_I,
            bool boolIsAvailability_I,
            bool boolIsUnit_I,
            out Inhedatajson1InheritanceDataJson1 inhedatajson1_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Establish the connection with db.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Jsons which will hold data.
            CostinhejsonCostInheritanceJson costinhejson = null;
            UnitinhejsonUnitInheritanceJson unitinhejson = null;
            AvainhejsonAvailabilityInheritanceJson avainhejson = null;
            Attrjson3AttributeJson3 attrjson = null;

            if (
                //                                          //Reset cost.
                boolIsCost_I &&
                boolIsAvailability_I == false &&
                boolIsUnit_I == false
                )
            {
                AttrAttribute.subResetCost(intValuePk_I, ref costinhejson, ref intStatus_IO, ref strUserMessage_IO,
                    ref strDevMessage_IO);
            }
            else if (
                //                                          //Reset unit.
                boolIsUnit_I &&
                boolIsCost_I == false &&
                boolIsAvailability_I == false
                )
            {
                AttrAttribute.subResetUnit(intValuePk_I, ref unitinhejson, ref intStatus_IO, ref strUserMessage_IO,
                    ref strDevMessage_IO);
            }
            else if (
                //                                          //Reset availability.
                boolIsAvailability_I &&
                boolIsCost_I == false &&
                boolIsUnit_I == false
                )
            {
                AttrAttribute.subResetAvailability(intValuePk_I, ref avainhejson, ref intStatus_IO,
                    ref strUserMessage_IO, ref strDevMessage_IO);
            }
            else if (
                //                                          //Reset attribute value.
                boolIsCost_I == false &&
                boolIsAvailability_I == false &&
                boolIsUnit_I == false
                )
            {
                AttrAttribute.subResetValue(intValuePk_I, ref attrjson, ref intStatus_IO, ref strUserMessage_IO,
                    ref strDevMessage_IO);
            }
            else
            {
                intStatus_IO = 407;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "No case found.";
            }

            //                                              //Json returned.
            inhedatajson1_O = new Inhedatajson1InheritanceDataJson1(unitinhejson, costinhejson, avainhejson, 
                attrjson);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static void subResetCost(
            int intValuePk_I,
            ref CostinhejsonCostInheritanceJson costinhejson_IO,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Get template.
            ResResource resTemplateDad = ResResource.resFromDB(intValuePk_I, true);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "No template found.";
            if (
                resTemplateDad != null
                )
            {
                //                                          //Get current template´s cost.
                CostentityCostEntityDB costentityCostDad;
                resTemplateDad.subGetCurrentCost(out costentityCostDad);

                intStatus_IO = 402;
                strUserMessage_IO = "No inherited cost found.";
                strDevMessage_IO = "";
                if (
                    costentityCostDad != null
                    )
                {
                    String strAccountName = null;
                    if (
                        //                                  //There is an associated account 
                        costentityCostDad.intPkAccount != null
                        )
                    {
                        Odyssey2Context context = new Odyssey2Context();
                        //                                  //Find account name
                        strAccountName = context.Account.FirstOrDefault(acc =>
                            acc.intPk == costentityCostDad.intPkAccount).strName;
                    }

                    //                                      //Json to return.
                    costinhejson_IO = new CostinhejsonCostInheritanceJson(costentityCostDad.numnCost, 
                        costentityCostDad.numnQuantity, costentityCostDad.numnMin, costentityCostDad.numnBlock, null, 
                        null, costentityCostDad.intPkAccount, strAccountName, costentityCostDad.numnHourlyRate,
                        costentityCostDad.boolnArea);

                    intStatus_IO = 200;
                    strUserMessage_IO = "Success.";
                    strDevMessage_IO = "";
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static void subResetUnit(
            int intValuePk_I,
            ref UnitinhejsonUnitInheritanceJson unitinhejson_IO,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Establish the connection with db.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get template.
            ResResource resTemplateDad = ResResource.resFromDB(intValuePk_I, true);

            intStatus_IO = 403;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "No template found.";
            if (
                resTemplateDad != null
                )
            {
                //                                          //Find unit attribute of the template.
                IQueryable<AttrentityAttributeEntityDB> setattr =
                   from attrentity in context.Attribute
                   join attretentity in context.AttributeElementType
                   on attrentity.intPk equals attretentity.intPkAttribute
                   where attretentity.intPkElementType == resTemplateDad.restypBelongsTo.intPk
                   select attrentity;
                List<AttrentityAttributeEntityDB> darrattr = setattr.ToList();
                //                                          //Get by Unit.
                String strNameOrUnitAttribute = "Unit";
                AttrentityAttributeEntityDB attrentityNameOrUnit = darrattr.FirstOrDefault(a =>
                    a.strXJDFName == strNameOrUnitAttribute);

                //                                          //Find current value entity.
                List<ValentityValueEntityDB> darrvalentityTemplateDad = context.Value.Where(val =>
                    val.intPkElement == intValuePk_I &&
                    val.intPkAttribute == attrentityNameOrUnit.intPk).ToList();
                darrvalentityTemplateDad.Sort();
                ValentityValueEntityDB valentityTemplateDad = darrvalentityTemplateDad.Last();

                intStatus_IO = 404;
                strUserMessage_IO = "No inherited unit found.";
                strDevMessage_IO = "";
                if (
                    valentityTemplateDad != null
                    )
                {
                    //                                      //Json to return.
                    unitinhejson_IO = new UnitinhejsonUnitInheritanceJson(valentityTemplateDad.strValue, null, null,
                        valentityTemplateDad.boolnIsDecimal);

                    intStatus_IO = 200;
                    strUserMessage_IO = "Success.";
                    strDevMessage_IO = "";

                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static void subResetAvailability(
            int intValuePk_I,
            ref AvainhejsonAvailabilityInheritanceJson avainhejson_IO,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Get template.
            ResResource resTemplateDad = ResResource.resFromDB(intValuePk_I, true);

            intStatus_IO = 405;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "No template found.";
            if (
                resTemplateDad != null
                )
            {
                //                                          //Json to return.
                avainhejson_IO = new AvainhejsonAvailabilityInheritanceJson(resTemplateDad.boolnIsCalendar, null, null);

                intStatus_IO = 200;
                strUserMessage_IO = "Success.";
                strDevMessage_IO = "";
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static void subResetValue(
            int intValuePk_I,
            ref Attrjson3AttributeJson3 attrjson_IO,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Establish the connection with db.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Find value entity.
            ValentityValueEntityDB valentity = context.Value.FirstOrDefault(valentity =>
                valentity.intPk == intValuePk_I);

            intStatus_IO = 406;
            strUserMessage_IO = "No inherited value found.";
            strDevMessage_IO = "";
            if (
                valentity != null
                )
            {
                //                                          //Json to return.
                attrjson_IO = new Attrjson3AttributeJson3();
                attrjson_IO.intValuePk = intValuePk_I;
                attrjson_IO.strValue = valentity.strValue;

                intStatus_IO = 200;
                strUserMessage_IO = "Success.";
                strDevMessage_IO = "";
            }
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
