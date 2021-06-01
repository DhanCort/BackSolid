/*TASK RP.ELEMENT*/
using Odyssey2Backend.DB_Odyssey2;
using Odyssey2Backend.Infrastructure;
using Odyssey2Backend.JsonTypes;
using Odyssey2Backend.PrintShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TowaStandard;
using System.IO;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: November 22, 2019. 

namespace Odyssey2Backend.XJDF
{
    //=================================================================================================================
    public abstract class EtElementTypeAbstract
    {
        //                                                  //Type of an element. This is used to generate concrete
        //                                                  //      classes of types.

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTANTS.

        //                                                  //Used to fill the fields AddedBy and ModifiedBy in the
        //                                                  //      database for the types.
        public const String strXJDFVersion = "XJDF2.0";

        //                                                  //Used to build the name of the XJDF types.
        public const String strXJDFPrefix = "XJDF";

        //                                                  //Used to fill the field ResOrPro in the database for the 
        //                                                  //      types.
        //                                                  //element is lower-case, to be XJDF compliant.
        public const String strResource = "Resource";
        public const String strProcess = "Process";
        public const String strIntent = "Intent";
        public const String strProduct = "Product";
        public const String strElement = "element";

        //                                                  //Used to fill the field CustomTypeId in the database for
        //                                                  //      identity a customType of a printshop.
        public const String strResCustomType = "Custom Resources";

        //                                                  //Used to fill the field CustomTypeId in the database for
        //                                                  //      identity a customType of a printshop.
        public const String strProCustomType = "Custom Processes";


        //                                                  //When a type was not built from a XJDF type this
        //                                                  //      constant is used to the XJDFTypeId field.
        public const String strNotXJDF = "None";

        //                                                  //Used to build the name of the MI4P types.
        public const String strMI4P = "MI4P";

        //                                                  //Separator used by the batch files.
        public const char strSeparator = ',';

        //                                                  //Separator used by the batch files.
        public const char strSeparatorPipe = '|';

        //                                                  //Use to check if a resource is Device type.
        public const String strResourceTypeDevice = "Device";

        //                                                  //Use to check if a resource is Media type.
        public const String strResourceTypeMedia = "Media";

        //                                                  //Use to check if a resource is Component type.
        public const String strResourceTypeComponent = "Component";

        //                                                  //Use to check if a resource is Tool type.
        public const String strResourceTypeTool = "Tool";

        //                                                  //Used to check wheter a resource type is consumable.
        public const String strResClasConsumable = "Consumable";

        //                                                  //Used to check wheter a resource type is Implementation.
        public const String strResClasImplementation = "Implementation";

        //                                                  //Used to check wheter a resource type is MiscConsumable.
        public const String strResClasMiscConsumable = "MiscConsumable";

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES

        //                                                  //Primary key in the database.
        private readonly int intPk_Z;
        public int intPk { get { return this.intPk_Z; } }

        //                                                  //Id to recognize the type in Odyssey.
        private readonly String strXJDFTypeId_Z;
        public String strXJDFTypeId { get { return this.strXJDFTypeId_Z; } }

        //                                                  //Who add the type.
        //                                                  //It could be:
        //                                                  //  XJDFx.x - It indicates that all the attributes are 
        //                                                  //              according with the XJDF specification.
        //                                                  //  MI4P    - It indicates that is a usual process but it 
        //                                                  //              does not accomplish with XJDF specification.
        //                                                  //  PID     - Printshop id that indicates that the printer
        //                                                  //              added its own process type.
        private readonly String strAddedBy_Z;
        public String strAddedBy { get { return this.strAddedBy_Z; } }

        //                                                  //This variable is relevant only when the strAddedBy 
        //                                                  //      variable has XJDFX.X as value.
        private readonly int? intPkPrintshop_Z;
        public int? intPkPrintshop { get { return this.intPkPrintshop_Z; } }

        //                                                  //Custom type id. This let the printshop to manage the 
        //                                                  //      name to recognize its own types.
        //                                                  //For the XJDF type, this is with XJDF prefix plus the 
        //                                                  //      type id. Example:
        //                                                  //  strXJDFTypeId = "Brochures"
        //                                                  //  strAddedBy = "XJDF2.0"
        //                                                  //  strCustomTypeId = "XJDF - Brochures"
        //                                                  //It is important that all the types that are not added 
        //                                                  //      by XJDF can not use XJDF before the custom type 
        //                                                  //      id. Also one printshop can not have more than one 
        //                                                  //      type with the same custom type id.
        private String strCustomTypeId_Z;
        public String strCustomTypeId { get { return this.strCustomTypeId_Z; } }

        private String strResOrPro_Z;
        public String strResOrPro { get { return this.strResOrPro_Z; } }
        private String strClassification_Z;
        public String strClassification { get { return this.strClassification_Z; } }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //DYNAMIC VARIABLES.

        //                                                  //Dictionary of attributes for this type.
        protected Dictionary<int, AttrAttribute> dicattr_Z;
        public Dictionary<int, AttrAttribute> dicattr
        {
            get
            {
                this.subGetAttributesFromDB(out this.dicattr_Z);
                return this.dicattr_Z;
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //SUPPORT METHODS FOR DYNAMIC VARIABLES.

        //--------------------------------------------------------------------------------------------------------------
        private void subGetAttributesFromDB(
            //                                              //Get all attr for this type from db.

            //                                              //Dic where the attr will be saved.
            out Dictionary<int, AttrAttribute> dicattr_O
            )
        {
            //                                              //Initialize the dicattr.
            dicattr_O = new Dictionary<int, AttrAttribute>();

            //                                              //Create the context.
            Odyssey2Context context = new Odyssey2Context();

            bool boolIsValidData = true;

            List<Field2jsonField2Json> darrfield2json = new List<Field2jsonField2Json>();

            if (
                this.strResOrPro == EtElementTypeAbstract.strProduct
                )
            {
                ProdtypProductType prodtypThis = (ProdtypProductType)this;

                boolIsValidData = this.boolAddAttributesToExistingProduct(
                    (int)prodtypThis.intWebsiteProductKey, prodtypThis.intPk, context,
                    out darrfield2json);
            }

            if (
                boolIsValidData
                )
            {
                //                                              //Get all PKs from the attributes.
                List<AttretentityAttributeElementTypeEntityDB> darrattretentity = context.AttributeElementType
                    .Where(attretentity => attretentity.intPkElementType == this.intPk).ToList();

                foreach (AttretentityAttributeElementTypeEntityDB attretentity in darrattretentity)
                {
                    //                                          //Get the attr from db.
                    AttrentityAttributeEntityDB attrentity = context.Attribute.FirstOrDefault(attrentity =>
                        attrentity.intPk == attretentity.intPkAttribute);

                    //                                          //Get values from attribute.
                    Field2jsonField2Json field2json = darrfield2json.FirstOrDefault(field2 => 
                        field2.intElementId == attrentity.intWebsiteAttributeId);

                    ValjsonValueJson[] arrstrValues = field2json.arrstrValues;

                    //                                          //Build the attr.
                    AttrAttribute attr = new AttrAttribute(attrentity.intPk, attrentity.strCustomName, 
                        attrentity.strXJDFName, attrentity.strCardinality, attrentity.strDatatype,
                        attrentity.strDescription, attrentity.strScope, attrentity.intWebsiteAttributeId, 
                        arrstrValues);

                    //                                          //Add the attr to the dic.
                    dicattr_O.Add(attrentity.intPk, attr);
                }
            }
            else
            {
                dicattr_O = null;
            }

        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public bool boolAddAttributesToExistingProduct(
            //                                              //Returns true if all attributes were added to Wisnet.

            //                                              //To go to Wisnet and bring the attributes.
            int intProductKey_I,

            //                                              //To link the attribute to this product in the Odyssey DB.
            int intEtentityPk_I,

            Odyssey2Context context_M,
            //                                              //Get all values from each attribute from product.
            out List<Field2jsonField2Json> darrfield2json_O
            )
        {
            darrfield2json_O = new List<Field2jsonField2Json>();

            bool boolIsValidData = true;

            String strPrintShopId = (PsPrintShop.psGet((int)this.intPkPrintshop, context_M)).strPrintshopId;

            //                                              //Bring the attributes for the product.
            String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                GetSection("Odyssey2Settings")["urlWisnetApi"];
            Task<List<Field2jsonField2Json>> Task_darrfield2json = HttpTools<Field2jsonField2Json>.
                GetListAsyncToEndPoint(strUrlWisnet + "/PrintShopData/attributes/" + strPrintShopId + "/" + 
                intProductKey_I);
            Task_darrfield2json.Wait();
            if (
                Task_darrfield2json.Result != null
                )
            {
                List<Field2jsonField2Json> darrfield2json = Task_darrfield2json.Result;

                //                                          //Assign List to output.
                darrfield2json_O = darrfield2json;

                //                                          //1. Get darrattrentityAttributesInDatabase from the
                //                                          //      database.
                IQueryable<AttrentityAttributeEntityDB> setattrentityAttributesInDatabase =
                    from attrentity in context_M.Attribute
                    join attretentity in context_M.AttributeElementType
                    on attrentity.intPk equals attretentity.intPkAttribute
                    where attretentity.intPkElementType == intEtentityPk_I
                    select attrentity;
                //                                          //2. Change the IQueryable to List.
                List<AttrentityAttributeEntityDB> darrattrentityAttributesInDatabase = 
                    setattrentityAttributesInDatabase.ToList<AttrentityAttributeEntityDB>();

                //                                          //Add attributes associated to the fields.
                int intStatus;
                foreach (Field2jsonField2Json field2json in darrfield2json)
                {
                    //                                      //Search fieldjson in the 
                    //                                      //      darrattrentityAttributesInDatabase.     .
                    AttrentityAttributeEntityDB attrentity = darrattrentityAttributesInDatabase.FirstOrDefault(
                        attrentity => attrentity.intWebsiteAttributeId == field2json.intElementId);
                    if (
                        //                                  //Not yet in the DB, new attribute.
                        //                                  //The order form was edited!
                        attrentity == null
                        )
                    {
                        if (
                            field2json.intElementId != -1
                            )
                        {
                            EtElementTypeAbstract.subAddAttributeToType(strPrintShopId, intEtentityPk_I,
                                field2json.strAttributeName, field2json.intElementId, "", "String",
                                "From Wisnet Product (Order Form).", context_M, out intStatus);
                        }
                    }
                    else
                    {
                        AttretentityAttributeElementTypeEntityDB attretentity =
                            context_M.AttributeElementType.FirstOrDefault(attretentity =>
                            (attretentity.intPkAttribute == attrentity.intPk) && 
                            (attretentity.intPkElementType == this.intPk));
                        if (
                            attretentity == null
                            )
                        {
                            attretentity = new AttretentityAttributeElementTypeEntityDB
                            {
                                intPkAttribute = attrentity.intPk,
                                intPkElementType = this.intPk
                            };
                            context_M.AttributeElementType.Add(attretentity);
                            context_M.SaveChanges();
                        }
                        darrattrentityAttributesInDatabase.Remove(attrentity);
                    }
                }

                if (
                    //                                      //There are attributes in the Odyssey2 DB that are not any
                    //                                      //      longer in the Wisnet database.
                    darrattrentityAttributesInDatabase.Count > 0
                    )
                {
                    foreach (AttrentityAttributeEntityDB attrentity in darrattrentityAttributesInDatabase)
                    {
                        this.deleteAttributeFromDB(attrentity, context_M);
                    }
                }
            }
            else
            {
                boolIsValidData = false;
            }
            return boolIsValidData;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private void deleteAttributeFromDB(
            AttrentityAttributeEntityDB attrentityToDelete_I,
            Odyssey2Context context_M
            )
        {
            //                                              //3. Delete the conditions related to the attribute.
            List<CondentityConditionEntityDB> darrcondentity = context_M.Condition.Where(
                cond => cond.intnPkAttribute == attrentityToDelete_I.intPk).ToList();
            foreach (CondentityConditionEntityDB condentity in darrcondentity)
            {
                context_M.Condition.Remove(condentity);
            }

            //                                              //2. Delete the attret link.
            AttretentityAttributeElementTypeEntityDB attretentityToDelete =
                context_M.AttributeElementType.FirstOrDefault(attretentity => attretentity.intPkAttribute ==
                attrentityToDelete_I.intPk);
            context_M.AttributeElementType.Remove(attretentityToDelete);

            //                                              //1. Delete the attribute.
            AttrentityAttributeEntityDB attrentityAttributeToDelete =
                context_M.Attribute.FirstOrDefault(attrentity => attrentity.intPk == attrentityToDelete_I.intPk);
            context_M.Attribute.Remove(attrentityAttributeToDelete);

            context_M.SaveChanges();
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.
        
        //--------------------------------------------------------------------------------------------------------------
        public EtElementTypeAbstract(
            //                                              //Primary key of the type.
            int intPk_I,
            //                                              //Specific type of process when it is a XJDF type, it 
            //                                              //      can be empty string when this is a printshop 
            //                                              //      type.
            String strXJDFTypeId_I,
            //                                              //Added by: XJDFX.X, MI4P or printshop id.
            String strAddedBy_I,
            //                                              //Modified by: XJDFX.X, MI4P or printshop id
            int? intPkPrintshop_I,
            //                                              //Custom type id.
            String strCustomTypeId_I,
            //                                              //Resource, Process, Intent or Element.
            String strResOrPro_I,
            //                                              //Type Classification 
            String strClassification_I
            )
        {
            this.intPk_Z = intPk_I;
            this.strXJDFTypeId_Z = strXJDFTypeId_I;
            this.strAddedBy_Z = strAddedBy_I;
            this.intPkPrintshop_Z = intPkPrintshop_I;
            this.strCustomTypeId_Z = strCustomTypeId_I;
            this.strResOrPro_Z = strResOrPro_I;
            this.strClassification_Z = strClassification_I;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //TRANSFORMATION METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public static void subAddXJDFType(
            //                                              //Add a new XJDF type.

            //                                              //Type name.
            String strXJDFTypeId_I,
            //                                              //ResOrPro (Process, Product, etc).
            String strResOrPro_I,
            //                                              //Pk of the element added, if the element coudl not be added
            //                                              //      this is -1.
            out int intPk_O,
            //                                              //Status:
            //                                              //      0 - Added successfully.
            //                                              //      1 - The type already exist.
            //                                              //      2 - The ResOrPro is not valid.
            out int intStatus_O
            )
        {
            intPk_O = -1;
            intStatus_O = 1;
            //                                              //Make the connection.
            Odyssey2Context context = new Odyssey2Context();
            EtentityElementTypeEntityDB etentity = context.ElementType.FirstOrDefault(etentity =>
                (etentity.strCustomTypeId == EtElementTypeAbstract.strXJDFPrefix + strXJDFTypeId_I) &&
                (etentity.intPrintshopPk == null));

            if (
                etentity == null
                )
            {
                intStatus_O = 2;
                if (
                    strResOrPro_I == EtElementTypeAbstract.strProcess ||
                    strResOrPro_I == EtElementTypeAbstract.strProduct
                    )
                {
                    //                                      //Create the entity.
                    etentity = new EtentityElementTypeEntityDB
                    {
                        strXJDFTypeId = strXJDFTypeId_I,
                        strResOrPro = strResOrPro_I,
                        intPrintshopPk = null,
                        strAddedBy = EtElementTypeAbstract.strXJDFVersion,
                        strCustomTypeId = EtElementTypeAbstract.strXJDFPrefix + strXJDFTypeId_I
                    };

                    //                                      //Add it to the database.
                    context.ElementType.Add(etentity);
                    context.SaveChanges();

                    intPk_O = etentity.intPk;
                    intStatus_O = 0;
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subAddProcessType(
            //                                              //Add a new process type.

            //                                              //PrintshopId that create the type.
            String strPrintshopId_I,
            int intPkPrintshop_I,
            //                                              //Process type name.
            String strCustomTypeId_I,
            //                                              //Primary key of the process created. If it is -1 the 
            //                                              //      type was not added to the db.
            out int intProcessTypePk_O,
            //                                              //Status:
            //                                              //      0 - Added successfully.
            //                                              //      1 - The type already exist.
            //                                              //      2 - The name can not start with XJDF as a prefix.
            out int intStatus_O
            )
        {
            intStatus_O = 2;
            intProcessTypePk_O = -1;
            if (
                //                                          //Have the XJDF prefix.
                !strCustomTypeId_I.StartsWith(EtElementTypeAbstract.strXJDFPrefix)
                )
            {
                //                                              //Make the connection.
                Odyssey2Context context = new Odyssey2Context();

                intStatus_O = 1;
                //                                              //Search if the type already exists.
                EtentityElementTypeEntityDB etentity = context.ElementType.FirstOrDefault(etentity =>
                    etentity.strCustomTypeId == strCustomTypeId_I && 
                    etentity.intPrintshopPk == intPkPrintshop_I);
                if (
                    etentity == null
                    )
                {
                    //                                              //Create the entity.
                    etentity = new EtentityElementTypeEntityDB
                    {
                        strXJDFTypeId = EtElementTypeAbstract.strNotXJDF,
                        strResOrPro = EtElementTypeAbstract.strProcess,
                        intPrintshopPk = intPkPrintshop_I,
                        strAddedBy = strPrintshopId_I,
                        strCustomTypeId = strCustomTypeId_I
                    };

                    //                                              //Add it to the database.
                    context.ElementType.Add(etentity);
                    context.SaveChanges();
                    
                    intStatus_O = 0;
                    intProcessTypePk_O = etentity.intPk;
                }
            }
        }        
        //--------------------------------------------------------------------------------------------------------------
        public static void subAddAttributeToType(
            //                                              //Add an attr to the db for a type.

            String strPrintshopId_I,
            int intTypePk_I,
            String strCustomName_I,
            int intWebsiteAttribute_I,
            String strCardinality_I,
            String strDatatype_I,
            String strDescription_I,
            Odyssey2Context context_M,
            //                                              //Status:
            //                                              //  0 - Added successfully.
            //                                              //  1 - The name is used by another attr for this kind of
            //                                              //          type or for this type.
            //                                              //  2 - Type was not found.
            out int intStatus_O
            )
        {
            //                                              //Get the type which the attr will be added.
            EtentityElementTypeEntityDB etentityCurrentType = context_M.ElementType.FirstOrDefault(etentity =>
            etentity.intPk == intTypePk_I);

            intStatus_O = 2;
            if (
                //                                          //Type found.
                etentityCurrentType != null
                )
            {
                String strCustomName = strCustomName_I;
                String strXJDFName = EtElementTypeAbstract.strNotXJDF;
                bool boolIsAvailableToBeAdded = true;
                if (
                    //                                      //Is a XJDF type.
                    etentityCurrentType.intPrintshopPk == null
                    )
                {
                    //                                      //Get what kind of type it is.
                    String strResOrPro = etentityCurrentType.strResOrPro;
                    strCustomName = EtElementTypeAbstract.strXJDFPrefix + strCustomName_I;
                    strXJDFName = strCustomName_I;

                    boolIsAvailableToBeAdded = EtElementTypeAbstract.boolTheNameIsAvailableForXJDF(strCustomName_I,
                        strResOrPro, context_M);
                }

                //                                          //Verify too the attributes that this type already has.
                boolIsAvailableToBeAdded = EtElementTypeAbstract.boolTheNameIsAvailable(strCustomName_I,
                    etentityCurrentType.intPk, context_M) && boolIsAvailableToBeAdded;

                intStatus_O = 1;
                if (
                    //                                      //The name is available to be added.
                    boolIsAvailableToBeAdded
                    )
                {
                    AttrentityAttributeEntityDB attrentity = new AttrentityAttributeEntityDB
                    {
                        strXJDFName = strXJDFName,
                        strCustomName = strCustomName_I,
                        intWebsiteAttributeId =intWebsiteAttribute_I,
                        strCardinality = strCardinality_I,
                        strDatatype = strDatatype_I,
                        strDescription = strDescription_I,
                        strScope = strPrintshopId_I,
                    };

                    context_M.Attribute.Add(attrentity);
                    context_M.SaveChanges();

                    AttretentityAttributeElementTypeEntityDB attrentityAttr =
                        new AttretentityAttributeElementTypeEntityDB
                        {
                            intPkAttribute = attrentity.intPk,
                            intPkElementType = intTypePk_I
                        };

                    context_M.AttributeElementType.Add(attrentityAttr);
                    context_M.SaveChanges();

                    intStatus_O = 0;
                }

            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subAddAttributeToType(
            //                                              //Add an attr to the db for a type.

            String strPrintshopId_I,
            int intTypePk_I,
            String strCustomName_I,
            String strCardinality_I,
            String strDatatype_I,
            String strDescription_I,
            //                                              //Status:
            //                                              //  0 - Added successfully.
            //                                              //  1 - The name is used by another attr for this kind of
            //                                              //          type or for this type.
            //                                              //  2 - Type was not found.
            out int intStatus_O
            )
        {
            //                                              //Create the connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get the type which the attr will be added.
            EtentityElementTypeEntityDB etentityCurrentType = context.ElementType.FirstOrDefault(etentity =>
            etentity.intPk == intTypePk_I);

            intStatus_O = 2;
            if (
                //                                          //Type found.
                etentityCurrentType != null
                )
            {
                String strCustomName = strCustomName_I;
                String strXJDFName = EtElementTypeAbstract.strNotXJDF;
                bool boolIsAvailableToBeAdded = true;
                if (
                    //                                      //Is a XJDF type.
                    etentityCurrentType.intPrintshopPk == null
                    )
                {
                    //                                      //Get what kind of type it is.
                    String strResOrPro = etentityCurrentType.strResOrPro;
                    strCustomName = EtElementTypeAbstract.strXJDFPrefix + strCustomName_I;
                    strXJDFName = strCustomName_I;

                    boolIsAvailableToBeAdded = EtElementTypeAbstract.boolTheNameIsAvailableForXJDF(strCustomName_I,
                        strResOrPro, context);
                }

                //                                          //Verify too the attributes that this type already has.
                boolIsAvailableToBeAdded = EtElementTypeAbstract.boolTheNameIsAvailable(strCustomName_I,
                    etentityCurrentType.intPk, context) && boolIsAvailableToBeAdded;

                intStatus_O = 1;
                if (
                    //                                      //The name is available to be added.
                    boolIsAvailableToBeAdded
                    )
                {
                    AttrentityAttributeEntityDB attrentity = new AttrentityAttributeEntityDB
                    {
                        strXJDFName = strXJDFName,
                        strCustomName = strCustomName,
                        strCardinality = strCardinality_I,
                        strDatatype = strDatatype_I,
                        strDescription = strDescription_I,
                        strScope = strPrintshopId_I,
                    };

                    context.Attribute.Add(attrentity);
                    context.SaveChanges();

                    AttretentityAttributeElementTypeEntityDB attrentityAttr =
                        new AttretentityAttributeElementTypeEntityDB
                        {
                            intPkAttribute = attrentity.intPk,
                            intPkElementType = intTypePk_I
                        };

                    context.AttributeElementType.Add(attrentityAttr);
                    context.SaveChanges();

                    intStatus_O = 0;
                }

            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static bool boolTheNameIsAvailableForXJDF(
            String strName_I,
            String strResOrPro_I,
            Odyssey2Context context_I
            )
        {
            //                                      //Get the general type of XJDF of this kind of type.
            EtentityElementTypeEntityDB etentity = context_I.ElementType.FirstOrDefault(etentity =>
                etentity.strResOrPro == strResOrPro_I &&
                etentity.intPrintshopPk == null &&
                etentity.strXJDFTypeId == strResOrPro_I + " General Attributes");

            int intTypePk = etentity.intPk;
                        
            //                                      //Get all relations with parameters.
            IQueryable<AttretentityAttributeElementTypeEntityDB> setattrentity = context_I.
                AttributeElementType.Where(attretentity =>
                attretentity.intPkElementType == intTypePk);

            List<int> darrint = new List<int>();
            foreach (AttretentityAttributeElementTypeEntityDB attretentity in setattrentity)
            {
                darrint.Add((int)attretentity.intPkAttribute);
            }

            List<String> darrstr = new List<String>();
            //                                      //Get the name for every attribute associated with the general 
            //                                      //      type.
            foreach (int intPk in darrint)
            {
                AttrentityAttributeEntityDB attrentity = context_I.Attribute.FirstOrDefault(attr =>
                attr.intPk == intPk);

                darrstr.Add(attrentity.strCustomName);
            }

            return !darrstr.Contains(strName_I);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static bool boolTheNameIsAvailable(
            String strName_I,
            int intTypePk_I,
            Odyssey2Context context_I
            )
        {
            //                                  //Get all relations with parameters.
            IQueryable<AttretentityAttributeElementTypeEntityDB> setattrentity = context_I.
                AttributeElementType.Where(attretentity =>
                attretentity.intPkElementType == intTypePk_I);

            List<int> darrint = new List<int>();
            foreach (AttretentityAttributeElementTypeEntityDB attretentity in setattrentity)
            {
                darrint.Add((int)attretentity.intPkAttribute);
            }

            List<String> darrstr = new List<String>();
            //                                  //Get the name for every attribute associated with every 
            //                                  //      type.
            foreach (int intPk in darrint)
            {
                AttrentityAttributeEntityDB attrentity = context_I.Attribute.FirstOrDefault(attr =>
                attr.intPk == intPk);

                darrstr.Add(attrentity.strCustomName);
            }

            return !darrstr.Contains(strName_I);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subAddExistingAttributesToType(
            //                                              //Create the relations between the attributes and a 
            //                                              //      type.

            //                                              //Type where the attributes will be added.
            int intTypePk_I,
            //                                              //Pks of the attributes that will be added.
            int[] arrintAttributePk_I,
            //                                              //Status:
            //                                              //      0 - Attributes added successfully.
            //                                              //      1 - Type not found.
            //                                              //      2 - At least one attribute was not found.
            out int intStatus_O
            )
        {
            //                                              //Create the connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get the type which the attr will be added.
            EtentityElementTypeEntityDB etentityCurrentType = context.ElementType.FirstOrDefault(etentity =>
            etentity.intPk == intTypePk_I);

            intStatus_O = 1;
            if (
                //                                          //Type found.
                etentityCurrentType != null
                )
            {
                //                                          //Verifying that all pk for attributes are valid.
                bool boolAllAttributesExists = true;
                foreach(int intAttributePk in arrintAttributePk_I)
                {
                    AttrentityAttributeEntityDB attrentity = context.Attribute.FirstOrDefault(attr => 
                    attr.intPk == intAttributePk);

                    if (
                        (attrentity == null) ||
                        !boolAllAttributesExists
                        )
                    {
                        boolAllAttributesExists = false;
                    }
                }

                intStatus_O = 2;
                if (
                    //                                      //All the attributes pk are valid.
                    boolAllAttributesExists
                    )
                {
                    //                                      //Create the relations.
                    foreach(int intAttributePk in arrintAttributePk_I)
                    {
                        //                                  //Add the relations to db.
                        AttretentityAttributeElementTypeEntityDB attret = 
                            new AttretentityAttributeElementTypeEntityDB
                        {
                            intPkAttribute = intAttributePk,
                            intPkElementType = intTypePk_I
                        };

                        context.AttributeElementType.Add(attret);
                        context.SaveChanges();
                    }

                    intStatus_O = 0;
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static EtElementTypeAbstract etFromDB(
            int? intnPk_I
            )
        {
            EtElementTypeAbstract et = null;
            if (
                //                                          //It is a valid primary key.
                intnPk_I > 0
                )
            {
                //                                          //Create the connection.
                Odyssey2Context context = new Odyssey2Context();

                EtentityElementTypeEntityDB etentity = context.ElementType.FirstOrDefault(etentity =>
                    etentity.intPk == intnPk_I);

                if (
                    etentity != null
                    )
                {
                    String strResOrPro = etentity.strResOrPro;
                    /*CASE*/
                    if (
                        //                                  //It is a process.
                        strResOrPro == EtElementTypeAbstract.strProcess
                        )
                    {
                        et = new ProtypProcessType(etentity.intPk, etentity.strXJDFTypeId, etentity.strAddedBy, 
                            etentity.intPrintshopPk, etentity.strCustomTypeId, 
                            etentity.strClassification);
                    }
                    else if (
                        //                                  //It is an intent.
                        strResOrPro == EtElementTypeAbstract.strIntent
                        )
                    {
                        et = new InttypIntentType(etentity.intPk, etentity.strXJDFTypeId, etentity.strAddedBy,
                            etentity.intPrintshopPk, etentity.strCustomTypeId, etentity.strClassification);
                    }
                    else if (
                        //                                  //It is a product.
                        strResOrPro == EtElementTypeAbstract.strProduct
                        )
                    {
                        et = new ProdtypProductType(etentity.intPk, etentity.strXJDFTypeId, etentity.strAddedBy,
                            etentity.intPrintshopPk, etentity.strCustomTypeId, etentity.intWebsiteProductKey,
                            etentity.strCategory, etentity.strClassification, etentity.boolnIsPublic,
                            etentity.intnPkAccount);
                    }
                    else if (
                        //                                  //It is an element.
                        strResOrPro == EtElementTypeAbstract.strElement
                        )
                    {
                        et = new EletemElementType(etentity.intPk, etentity.strXJDFTypeId, etentity.strAddedBy,
                            etentity.intPrintshopPk, etentity.strCustomTypeId, 
                            etentity.strClassification);
                    }
                    else if (
                        //                                  //It is a resource.
                        strResOrPro == EtElementTypeAbstract.strResource
                        )
                    {
                        et = new RestypResourceType(etentity.intPk, etentity.strXJDFTypeId, etentity.strAddedBy,
                            etentity.intPrintshopPk, etentity.strCustomTypeId, 
                            etentity.strClassification);
                    }
                    /*END-CASE*/
                }
            }

            return et;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static EtElementTypeAbstract etFromDB(
            Odyssey2Context context_I,
            int? intnPk_I
            )
        {
            EtElementTypeAbstract et = null;
            if (
                //                                          //It is a invalid primary key.
                intnPk_I > 0
                )
            {
                EtentityElementTypeEntityDB etentity = context_I.ElementType.FirstOrDefault(etentity =>
                    etentity.intPk == intnPk_I);

                if (
                    etentity != null
                    )
                {
                    String strResOrPro = etentity.strResOrPro;
                    /*CASE*/
                    if (
                        //                                  //It is a process.
                        strResOrPro == EtElementTypeAbstract.strProcess
                        )
                    {
                        et = new ProtypProcessType(etentity.intPk, etentity.strXJDFTypeId, etentity.strAddedBy,
                            etentity.intPrintshopPk, etentity.strCustomTypeId,
                            etentity.strClassification);
                    }
                    else if (
                        //                                  //It is an intent.
                        strResOrPro == EtElementTypeAbstract.strIntent
                        )
                    {
                        et = new InttypIntentType(etentity.intPk, etentity.strXJDFTypeId, etentity.strAddedBy,
                            etentity.intPrintshopPk, etentity.strCustomTypeId, etentity.strClassification);
                    }
                    else if (
                        //                                  //It is a product.
                        strResOrPro == EtElementTypeAbstract.strProduct
                        )
                    {
                        et = new ProdtypProductType(etentity.intPk, etentity.strXJDFTypeId, etentity.strAddedBy,
                            etentity.intPrintshopPk, etentity.strCustomTypeId, etentity.intWebsiteProductKey,
                            etentity.strCategory, etentity.strClassification, etentity.boolnIsPublic,
                            etentity.intnPkAccount);
                    }
                    else if (
                        //                                  //It is an element.
                        strResOrPro == EtElementTypeAbstract.strElement
                        )
                    {
                        et = new EletemElementType(etentity.intPk, etentity.strXJDFTypeId, etentity.strAddedBy,
                            etentity.intPrintshopPk, etentity.strCustomTypeId,
                            etentity.strClassification);
                    }
                    else if (
                        //                                  //It is a resource.
                        strResOrPro == EtElementTypeAbstract.strResource
                        )
                    {
                        et = new RestypResourceType(etentity.intPk, etentity.strXJDFTypeId, etentity.strAddedBy,
                            etentity.intPrintshopPk, etentity.strCustomTypeId,
                            etentity.strClassification);
                    }
                    /*END-CASE*/
                }
            }

            return et;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String[] arrstrResourceClasses(
            )
        {
            String[] arrstrClassification;

            //                                          //Create the connection.
            Odyssey2Context context = new Odyssey2Context();

            IQueryable<String> setstrClassification = (
                 from rcentity in context.ResourceClassification
                 orderby rcentity.strName
                 select rcentity.strName
                 );

            arrstrClassification = setstrClassification.ToArray();

            return arrstrClassification;
        }

        //--------------------------------------------------------------------------------------------------------------
        public bool boolHasCalculationAssociated(
            )
        {
            bool boolHasCalculationAssociated = false;
            Odyssey2Context context = new Odyssey2Context();

            CalentityCalculationEntityDB calentity = null;
            /*CASE*/
            if (
                this.strResOrPro == EtElementTypeAbstract.strResource
                )
            {
                //                                          //Do not do anything.
            }
            else if (
                this.strResOrPro == EtElementTypeAbstract.strProcess
                )
            {
                calentity = context.Calculation.FirstOrDefault(calentity =>
                calentity.intnPkProcess == this.intPk);
            }
            /*END-CASE*/

            boolHasCalculationAssociated = calentity != null;

            return boolHasCalculationAssociated;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
