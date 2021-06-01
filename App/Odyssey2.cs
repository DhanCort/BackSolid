
/*TASK RP.ODYSSEY2*/
using Odyssey2Backend.Catalogs;
using Odyssey2Backend.DB_Odyssey2;
using Odyssey2Backend.PrintShop;
using Odyssey2Backend.XJDF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TowaStandard;
using Odyssey2Backend.Utilities;

//                                                          //AUTHOR: Towa (VSTD - Victor Torres).
//                                                          //CO-AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //DATE: November 11, 2019.

namespace Odyssey2Backend.App
{

    //==================================================================================================================
    public static class Odyssey2
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //DYNAMIC VARIABLES.

        //                                                  //Printshops that are already in the Odyssey2 DB.
        //                                                  //Does not have all the ps, there are more in Wisnet DB.
        private static Dictionary<String, PsPrintShop> dicps_Z;
        public static Dictionary<String, PsPrintShop> dicps
        {
            get
            {
                Odyssey2.subGetPrintshopFromDB(out Odyssey2.dicps_Z);
                return Odyssey2.dicps_Z;
            }
        }

        //                                                  //Product types.
        private static Dictionary<String, ProdtypProductType> dicprodtem_Z;
        public static Dictionary<String, ProdtypProductType> dicprodtyp
        {
            get
            {
                Odyssey2.subGetProductTypeFromDB(out Odyssey2.dicprodtem_Z);
                return Odyssey2.dicprodtem_Z;
            }
        }

        //                                                  //Process types.
        private static Dictionary<int, ProtypProcessType> dicprotyp_Z;
        public static Dictionary<int, ProtypProcessType> dicprotyp
        {
            get
            {
                Odyssey2.subGetProcessTypeFromDB(out Odyssey2.dicprotyp_Z);
                return Odyssey2.dicprotyp_Z;
            }
        }

        //                                                  //Intent types.
        private static Dictionary<String, InttypIntentType> dicinttem_Z;
        public static Dictionary<String, InttypIntentType> dicinttyp
        {
            get
            {
                Odyssey2.subGetIntentTypeFromDB(out Odyssey2.dicinttem_Z);
                return Odyssey2.dicinttem_Z;
            }
        }

        //                                                  //Resources types.
        private static Dictionary<String, RestypResourceType> dicrestem_Z;
        public static Dictionary<String, RestypResourceType> dicrestem
        {
            get
            {
                Odyssey2.subGetResourceTypeFromDB(out Odyssey2.dicrestem_Z);
                return Odyssey2.dicrestem_Z;
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //SUPPORT METHODS FOR DYNAMIC VARIABLES.

        //--------------------------------------------------------------------------------------------------------------
        private static void subGetPrintshopFromDB(
            //                                              //To get data from the database and fill the dic.

            //                                              //Dic to be fill.
            out Dictionary<String, PsPrintShop> dicps_O
            )
        {
            //                                              //Empty the dictionary.
            dicps_O = new Dictionary<String, PsPrintShop>();
            //                                              //Get te ps using the Dany´s service. 

            //                                              //Get Printshop from Wisnet. 
            //                                              //GetAsyncToEndPoint() returns null if there is no access 
            //                                              //    to the service of wisnet

            Odyssey2Context context = new Odyssey2Context();
            IQueryable<PsentityPrintshopEntityDB> setpsentityCurrent = context.Printshop;
            foreach (PsentityPrintshopEntityDB psentity in setpsentityCurrent)
            {
                PsPrintShop psToAdd = new PsPrintShop(psentity.intPk, psentity.strPrintshopId, psentity.strName,
                    psentity.strSpecialPassword, psentity.strUrl, psentity.boolOffsetNumber, psentity.strTimeZone);
                dicps_O.Add(psentity.strPrintshopId, psToAdd);
            }
        }

        //-------------------------------------------------------------------------------------------------------------- 
        private static void subGetProcessTypeFromDB(
            //                                              //To get data from the database and fill the dic.

            //                                              //Dic to be fill.
            out Dictionary<int, ProtypProcessType> dicprotyp_O
            )
        {
            //                                              //Get all the types from database.
            Odyssey2Context context = new Odyssey2Context();
            IQueryable<EtentityElementTypeEntityDB> setetentity = context.ElementType.Where(etentity =>
                etentity.strResOrPro == EtElementTypeAbstract.strProcess &&
                etentity.strAddedBy == EtElementTypeAbstract.strXJDFVersion &&
                etentity.intPrintshopPk == null);

            //                                              //Empty the dic.
            dicprotyp_O = new Dictionary<int, ProtypProcessType>();

            //                                              //Create and add all the types to the dic.
            foreach (EtentityElementTypeEntityDB etentity in setetentity)
            {
                if (
                    etentity.strCustomTypeId != EtElementTypeAbstract.strXJDFPrefix +
                    ProtypProcessType.strGeneralTypeId
                    )
                {
                    ProtypProcessType protyp = new ProtypProcessType(etentity.intPk, etentity.strXJDFTypeId,
                        etentity.strAddedBy, etentity.intPrintshopPk, etentity.strCustomTypeId,
                        etentity.strClassification);
                    dicprotyp_O.Add(etentity.intPk, protyp);
                }
            }
        }

        //-------------------------------------------------------------------------------------------------------------- 
        private static void subGetProductTypeFromDB(
            //                                              //To get data from the database and fill the dic.
            //                                              //      ONLY PRODDUCTS THAT ARE ACTUALLY IN THE DB.                                            

            //                                              //Dic to be fill.
            out Dictionary<String, ProdtypProductType> dicprodtem_O
            )
        {
            //                                              //Get all the types from database.
            Odyssey2Context context = new Odyssey2Context();
            IQueryable<EtentityElementTypeEntityDB> setetentity = context.ElementType.Where(etentity =>
                etentity.strResOrPro == EtElementTypeAbstract.strProduct);

            //                                              //Empty the dic.
            dicprodtem_O = new Dictionary<String, ProdtypProductType>();

            //                                              //Create and add all the types to the dic.
            foreach (EtentityElementTypeEntityDB etentity in setetentity)
            {
                ProdtypProductType prodtem = new ProdtypProductType(etentity.intPk, etentity.strXJDFTypeId,
                    etentity.strAddedBy, etentity.intPrintshopPk, etentity.strCustomTypeId,
                    etentity.intWebsiteProductKey, "", etentity.strClassification, etentity.boolnIsPublic,
                    etentity.intnPkAccount);
                dicprodtem_O.Add(etentity.strCustomTypeId, prodtem);
            }
        }

        //-------------------------------------------------------------------------------------------------------------- 
        private static void subGetIntentTypeFromDB(
            //                                              //To get data from the database and fill the dic.

            //                                              //Dic to be fill.
            out Dictionary<String, InttypIntentType> dicinttem_O
            )
        {
            //                                              //Get all the types from database.
            Odyssey2Context context = new Odyssey2Context();
            IQueryable<EtentityElementTypeEntityDB> setetentity = context.ElementType.Where(etentity =>
                etentity.strResOrPro == EtElementTypeAbstract.strIntent);

            //                                              //Empty the dic.
            dicinttem_O = new Dictionary<String, InttypIntentType>();

            //                                              //Create and add all the types to the dic.
            foreach (EtentityElementTypeEntityDB etentity in setetentity)
            {
                InttypIntentType inttem = new InttypIntentType(etentity.intPk, etentity.strXJDFTypeId,
                    etentity.strAddedBy, etentity.intPrintshopPk, etentity.strCustomTypeId,
                    etentity.strClassification);
                dicinttem_O.Add(etentity.strCustomTypeId, inttem);
            }
        }

        //-------------------------------------------------------------------------------------------------------------- 
        private static void subGetResourceTypeFromDB(
            //                                              //To get data from the database and fill the dic.

            //                                              //Dic to be fill.
            out Dictionary<String, RestypResourceType> dicrestem_O
            )
        {
            //                                              //Get all the types from database.
            Odyssey2Context context = new Odyssey2Context();
            IQueryable<EtentityElementTypeEntityDB> setetentity = context.ElementType.Where(etentity =>
                etentity.strResOrPro == EtElementTypeAbstract.strResource &&
                etentity.intPrintshopPk == null &&
                etentity.strAddedBy == EtElementTypeAbstract.strXJDFVersion);

            //                                              //Empty the dic.
            dicrestem_O = new Dictionary<String, RestypResourceType>();

            //                                              //Create and add all the types to the dic.
            foreach (EtentityElementTypeEntityDB etentity in setetentity)
            {
                RestypResourceType restem = new RestypResourceType(etentity.intPk, etentity.strXJDFTypeId,
                    etentity.strAddedBy, etentity.intPrintshopPk, etentity.strCustomTypeId,
                    etentity.strClassification);
                dicrestem_O.Add(etentity.strCustomTypeId, restem);
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String[] arrstrGetEnumFromDB(
            //                                              //To get data from the database and fill the dic.

            //                                              //EnumName
            String strEnumName_I
        )
        {
            //                                              //Get all enum values from database.
            Odyssey2Context context = new Odyssey2Context();
            IQueryable<EnumentityEnumerationEntityDB> setenumentity = context.Enumeration.Where(
                enumentity => enumentity.strEnumName == strEnumName_I);

            List<String> darrstrEnumValues = new List<String>();
            //                                              //Add values to the list.
            foreach (EnumentityEnumerationEntityDB enumentity in setenumentity)
            {
                String strEnumValUnikeRef = enumentity.strEnumValue.ToString();
                darrstrEnumValues.Add(strEnumValUnikeRef);
            }
            return darrstrEnumValues.ToArray();
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //TRANSFORMATION METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public static void subAddXJDFResourceType(
            //                                              //Add a new XJDF type.

            int intTypePk_I,
            //                                              //Intent type name.
            String strXJDFTypeId_I
            )
        {
            //                                              //Make the connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Create the entity.
            EtentityElementTypeEntityDB etentity = new EtentityElementTypeEntityDB
            {
                strXJDFTypeId = strXJDFTypeId_I,
                strResOrPro = EtElementTypeAbstract.strResource,
                intPrintshopPk = null,
                strAddedBy = EtElementTypeAbstract.strXJDFVersion,
                strCustomTypeId = EtElementTypeAbstract.strXJDFPrefix + strXJDFTypeId_I
            };

            //                                              //Add it to the database.
            context.ElementType.Add(etentity);
            context.SaveChanges();

            //                                              //Primary key of the resource.
            int intResourceTypePk = etentity.intPk;

            //                                              //Relationship between the resource and the type which 
            //                                              //      belongs to.
            EtetentityElementTypeElementTypeEntityDB etetentity =
                new EtetentityElementTypeElementTypeEntityDB
                {
                    intPkElementTypeDad = intTypePk_I,
                    intPkElementTypeSon = intResourceTypePk
                };

            //                                              //Add it to the database.
            context.ElementTypeElementType.Add(etetentity);
            context.SaveChanges();
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subAddResourceType(
            //                                              //Add a new intent type.

            int intTypePk_I,
            //                                              //PrintshopId that create the type.
            String strPrintshopId_I,
            //                                              //Process type name.
            String strCustomTypeId_I
            )
        {
            //                                              //Make the connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Verify if the process type is from MI4P.
            String strCustomTypeId = strCustomTypeId_I;
            if (
                strPrintshopId_I == EtElementTypeAbstract.strMI4P
                )
            {
                //                                          //The name uses the MI4P prefix.
                strCustomTypeId = EtElementTypeAbstract.strMI4P + strCustomTypeId_I;
            }

            int intPkPrintshop = PsPrintShop.psGet(strPrintshopId_I).intPk;

            //                                              //Create the entity.
            EtentityElementTypeEntityDB etentity = new EtentityElementTypeEntityDB
            {
                strXJDFTypeId = EtElementTypeAbstract.strNotXJDF,
                strResOrPro = EtElementTypeAbstract.strResource,
                intPrintshopPk = intPkPrintshop,
                strAddedBy = strPrintshopId_I,
                strCustomTypeId = strCustomTypeId
            };

            //                                              //Add it to the database.
            context.ElementType.Add(etentity);
            context.SaveChanges();

            //                                              //Primary key of the resource.
            int intResourceTypePk = etentity.intPk;

            //                                              //Relationship between the resource and the type which 
            //                                              //      belongs to.
            EtetentityElementTypeElementTypeEntityDB etetentity =
                new EtetentityElementTypeElementTypeEntityDB
                {
                    intPkElementTypeDad = intTypePk_I,
                    intPkElementTypeSon = intResourceTypePk
                };

            //                                              //Add it to the database.
            context.ElementTypeElementType.Add(etetentity);
            context.SaveChanges();

            //                                              //If the process type is from a printshop, it is 
            //                                              //      necessary to add the relation.
            if (
                !strCustomTypeId.StartsWith(EtElementTypeAbstract.strMI4P)
                )
            {
                int intStatus;
                int intPk;
                Odyssey2.subAddTypeToPrintshop(strPrintshopId_I, intResourceTypePk, out intStatus, out intPk);
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subAddTypeToPrintshop(
            //                                              //Add a type to printshop.

            //                                              //Printshop id.
            String strPrintshopId_I,
            //                                              //Type pk.
            int intTypePk_I,
            //                                              //Status:
            //                                              //      0 - Relation added successfully.
            //                                              //      1 - Type not found.
            //                                              //      2 - Printshop not found.
            out int intStatus_O,
            out int intPrintshopTypePk_O
            )
        {
            intStatus_O = 2;
            intPrintshopTypePk_O = -1;
            //                                              //Make the connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get the printshop.
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);
            if (
                ps != null
                )
            {
                intStatus_O = 1;
                //                                          //Get the entity of product type.
                EtentityElementTypeEntityDB etentity = context.ElementType.FirstOrDefault(et =>
                    et.intPk == intTypePk_I);
                if (
                    etentity != null
                    )
                {
                    int intPkPrintshop = PsPrintShop.psGet(strPrintshopId_I).intPk;
                    //                                      //Create the copy type to the printshop.
                    EtentityElementTypeEntityDB etentityToPrintshop = new EtentityElementTypeEntityDB
                    {
                        strCustomTypeId = etentity.strCustomTypeId,
                        strAddedBy = etentity.strAddedBy,
                        strXJDFTypeId = etentity.strXJDFTypeId,
                        strResOrPro = etentity.strResOrPro,
                        intWebsiteProductKey = etentity.intWebsiteProductKey,
                        strCategory = etentity.strCategory,
                        strClassification = etentity.strClassification,
                        intPrintshopPk = intPkPrintshop
                    };
                    context.ElementType.Add(etentityToPrintshop);
                    context.SaveChanges();

                    intStatus_O = 0;

                    intPrintshopTypePk_O = etentityToPrintshop.intPk;
                    if (
                        //                                  //It is a resource.
                        etentityToPrintshop.strResOrPro == EtElementTypeAbstract.strResource
                        )
                    {
                        Odyssey2.subAddAttributesForTheType(intTypePk_I, intPrintshopTypePk_O, context);
                        Odyssey2.subAddElementsForTheType(intTypePk_I, intPrintshopTypePk_O, context);
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subAddTypeToPrintshop(
            //                                              //Add a type to printshop.

            //                                              //Type pk.
            int intTypePk_I,
            PsPrintShop ps_I,
            Odyssey2Context context_M,
            //                                              //Status:
            //                                              //      0 - Relation added successfully.
            //                                              //      1 - Type not found.
            //                                              //      2 - Printshop not found.
            out int intStatus_O,
            out int intPrintshopTypePk_O
            )
        {
            intStatus_O = 2;
            intPrintshopTypePk_O = -1;

            if (
                ps_I != null
                )
            {
                intStatus_O = 1;
                //                                          //Get the entity of product type.
                EtentityElementTypeEntityDB etentity = context_M.ElementType.FirstOrDefault(et =>
                    et.intPk == intTypePk_I);
                if (
                    etentity != null
                    )
                {
                    int intPkPrintshop = ps_I.intPk;
                    //                                      //Create the copy type to the printshop.
                    EtentityElementTypeEntityDB etentityToPrintshop = new EtentityElementTypeEntityDB
                    {
                        strCustomTypeId = etentity.strCustomTypeId,
                        strAddedBy = etentity.strAddedBy,
                        strXJDFTypeId = etentity.strXJDFTypeId,
                        strResOrPro = etentity.strResOrPro,
                        intWebsiteProductKey = etentity.intWebsiteProductKey,
                        strCategory = etentity.strCategory,
                        strClassification = etentity.strClassification,
                        intPrintshopPk = intPkPrintshop
                    };
                    context_M.ElementType.Add(etentityToPrintshop);
                    context_M.SaveChanges();

                    intStatus_O = 0;

                    intPrintshopTypePk_O = etentityToPrintshop.intPk;
                    if (
                        //                                  //It is a resource.
                        etentityToPrintshop.strResOrPro == EtElementTypeAbstract.strResource
                        )
                    {
                        Odyssey2.subAddAttributesForTheType(intTypePk_I, intPrintshopTypePk_O, context_M);
                        Odyssey2.subAddElementsForTheType(intTypePk_I, intPrintshopTypePk_O, context_M);
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subAddAttributesForTheType(
            int intTypePk_I,
            int intPrintshopTypePk_I,
            Odyssey2Context context_M
            )
        {
            //                                  //Get the attributes for the type.
            IQueryable<AttretentityAttributeElementTypeEntityDB> setattretentity =
                context_M.AttributeElementType.Where(attretentity =>
                attretentity.intPkElementType == intTypePk_I);

            //                                  //Associate the attributes to the printshop type too.
            List<AttretentityAttributeElementTypeEntityDB> darrattretentity = setattretentity.ToList();
            foreach (AttretentityAttributeElementTypeEntityDB attretentity in darrattretentity)
            {
                AttretentityAttributeElementTypeEntityDB attretentityToPrintshop =
                    new AttretentityAttributeElementTypeEntityDB
                    {
                        intPkAttribute = attretentity.intPkAttribute,
                        intPkElementType = intPrintshopTypePk_I
                    };
                context_M.AttributeElementType.Add(attretentityToPrintshop);
                context_M.SaveChanges();
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subAddElementsForTheType(
            int intTypePk_I,
            int intPrintshopTypePk_I,
            Odyssey2Context context_M
            )
        {
            //                                  //Get the attributes for the type.
            IQueryable<EtetentityElementTypeElementTypeEntityDB> setetetentity =
                context_M.ElementTypeElementType.Where(etetentity =>
                etetentity.intPkElementTypeDad == intTypePk_I);

            //                                  //Associate the attributes to the printshop type too.
            List<EtetentityElementTypeElementTypeEntityDB> darretetentity = setetetentity.ToList();
            foreach (EtetentityElementTypeElementTypeEntityDB etetentity in darretetentity)
            {
                EtetentityElementTypeElementTypeEntityDB etetentityToPrintshop =
                    new EtetentityElementTypeElementTypeEntityDB
                    {
                        intPkElementTypeDad = intPrintshopTypePk_I,
                        intPkElementTypeSon = etetentity.intPkElementTypeSon
                    };
                context_M.ElementTypeElementType.Add(etetentityToPrintshop);
                context_M.SaveChanges();
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subAddProcessCustomNotXJDFToPrintshop(
            //                                              //Add a type to printshop.

            //                                              //Printshop id.
            String strPrintshopId_I,
            //                                              //Resource name.
            String strProcessName_I,
            //                                              //Arr of attribute.
            //                                              //Status:
            //                                              //      0 - Relation added successfully.
            //                                              //      1 - the name of process not should repeated.
            //                                              //      2 - Printshop not found.
            out int intStatus_O
            )
        {
            intStatus_O = 2;

            //                                              //Make the connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get the printshop.
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);
            if (
                ps != null
                )
            {
                intStatus_O = 1;

                //                                      //Find the custom type process of a printshop.
                EtentityElementTypeEntityDB etentityCustom = context.ElementType.FirstOrDefault(
                    ele => ele.strCustomTypeId == strProcessName_I
                    && ele.intPrintshopPk == ps.intPk && ele.strXJDFTypeId == EtElementTypeAbstract.strNotXJDF
                    && ele.intWebsiteProductKey == null);

                if (
                    etentityCustom == null
                    )
                {
                    intStatus_O = 0;
                    //                                  //Add process CustomType to printshop
                    //                                  //Create the entity
                    etentityCustom = new EtentityElementTypeEntityDB()
                    {
                        strCustomTypeId = strProcessName_I,
                        intPrintshopPk = ps.intPk,
                        strXJDFTypeId = EtElementTypeAbstract.strNotXJDF,
                        strAddedBy = ps.strPrintshopId,
                        strResOrPro = EtElementTypeAbstract.strProcess
                    };
                    context.ElementType.Add(etentityCustom);
                    context.SaveChanges();
                }
                else
                {
                    //The custom type process can not be duplicate.

                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subRemoveTypeFromPrintshop(
            //                                              //Remove type form printshop.
            //                                              //a) A type of resource.
            //                                              //b) A process.

            //                                              //Pk Type to remove from the prontshop.
            int intTypePk_I,
            Odyssey2Context context_I,
            //                                              //Status:
            //                                              //      200 - Type removed successfully.
            //                                              //      401 - Type not found.
            //                                              //      402 - Printshop not found.
            //                                              //      403 - Type has calculations and is a type of resource.
            //                                              //      404 - Type configured as IO of a process.
            out int intStatus_O,
            out String strUserMessage_O,

            //                                              //When remove the type of resource, it returns the pk 
            //                                              //      of the XJDF type original, the one that is not
            //                                              //      associated to any printshop.
            out int intXJDFTypePk_O
            )
        {
            //                                              //Get the type to delete.
            EtentityElementTypeEntityDB etentityTypeOrProcessToDelete = context_I.ElementType.FirstOrDefault(
                etentity => etentity.intPk == intTypePk_I);

            intXJDFTypePk_O = -1;
            intStatus_O = 401;
            strUserMessage_O = "Type not found.";
            if (
                //                                          //Type found.
                etentityTypeOrProcessToDelete != null
                )
            {
                //                                          //Get the printshop of the type to delete.
                PsPrintShop ps = PsPrintShop.psGet((int)etentityTypeOrProcessToDelete.intPrintshopPk, context_I);

                intStatus_O = 402;
                strUserMessage_O = "Printshop not found.";
                if (
                    //                                      //Printshop found.
                    ps != null
                    )
                {
                    CalentityCalculationEntityDB calentity = context_I.Calculation.FirstOrDefault(calentity =>
                        calentity.intnPkResource == intTypePk_I);

                    intStatus_O = 403;
                    strUserMessage_O = "Type has calculations.";
                    if (
                        //                              //Type does not have calculations or it is a process.
                        (calentity == null) ||
                        (etentityTypeOrProcessToDelete.strResOrPro == EtElementTypeAbstract.strProcess)
                        )
                    {
                        //                              //Places where the type is IO of a process.
                        EtetentityElementTypeElementTypeEntityDB etetentityIO = context_I.ElementTypeElementType.
                            FirstOrDefault(etetentity =>
                            etetentity.intPkElementTypeSon == etentityTypeOrProcessToDelete.intPk);

                        intStatus_O = 404;
                        strUserMessage_O = "Type is IO of a process.";
                        if (
                            //                          //It is a Type of resource that is not IO of a process.
                            ((etentityTypeOrProcessToDelete.strResOrPro == EtElementTypeAbstract.strResource) &&
                            (etetentityIO == null)) ||
                            //                          //Ot it is a Process.
                            (etentityTypeOrProcessToDelete.strResOrPro == EtElementTypeAbstract.strProcess)
                            )
                        {
                            //                          //Look for IOs or sons.
                            IQueryable<EtetentityElementTypeElementTypeEntityDB> setetetentityIOs =
                                    context_I.ElementTypeElementType.Where(etetentity =>
                                    etetentity.intPkElementTypeDad == etentityTypeOrProcessToDelete.intPk);
                            List<EtetentityElementTypeElementTypeEntityDB> darretetentityIOs =
                                setetetentityIOs.ToList();
                            //                          //Remove the sons.
                            foreach (EtetentityElementTypeElementTypeEntityDB etetentity in darretetentityIOs)
                            {
                                context_I.ElementTypeElementType.Remove(etetentity);
                            }
                            context_I.SaveChanges();

                            //                          //Delete the calculations.
                            Odyssey2.DeleteCalculations(etentityTypeOrProcessToDelete.intPk, context_I);

                            //                          //Remove attributes of the type.
                            IQueryable<AttretentityAttributeElementTypeEntityDB> setattretentity =
                                context_I.AttributeElementType.Where(attretentity =>
                                attretentity.intPkElementType == etentityTypeOrProcessToDelete.intPk);

                            List<AttretentityAttributeElementTypeEntityDB> darrattretentity = setattretentity.ToList();
                            foreach (AttretentityAttributeElementTypeEntityDB attretentity in darrattretentity)
                            {
                                context_I.AttributeElementType.Remove(attretentity);
                            }
                            context_I.SaveChanges();

                            EtentityElementTypeEntityDB etentityXJDF = context_I.ElementType.FirstOrDefault(et =>
                                et.strCustomTypeId == etentityTypeOrProcessToDelete.strCustomTypeId &&
                                et.PkPrintshopId == null &&
                                et.intWebsiteProductKey == null);
                            //                                      //Delete type.
                            context_I.ElementType.Remove(etentityTypeOrProcessToDelete);
                            context_I.SaveChanges();

                            intStatus_O = 200;
                            if (
                                etentityXJDF != null
                                )
                            {
                                intXJDFTypePk_O = etentityXJDF.intPk;
                            }
                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void DeleteCalculations(
            //                                      //Delete calculation associate with type of the 
            //                                      //     Printshop.

            int intPkType_I,
            Odyssey2Context context_I
            )
        {
            IQueryable<CalentityCalculationEntityDB> setcalentity = context_I.Calculation.Where(calentity =>
                calentity.intnPkProcess == intPkType_I);
            List<CalentityCalculationEntityDB> darrcalentity = setcalentity.ToList();

            foreach (CalentityCalculationEntityDB calentity in darrcalentity)
            {
                //                                      //Delete group associate whit the calculation.
                IQueryable<GpcalentityGroupCalculationEntityDB> setgpcal =
                    context_I.GroupCalculation.Where(gpcalentity =>
                    gpcalentity.intPkCalculation == calentity.intPk);

                //                                      //List of Group associate whit calculation pk.
                List<GpcalentityGroupCalculationEntityDB> darrgpcalentity = setgpcal.ToList();

                foreach (GpcalentityGroupCalculationEntityDB gpcalentity in darrgpcalentity)
                {
                    //                                  //Delete group
                    context_I.GroupCalculation.Remove(gpcalentity);
                    context_I.SaveChanges();
                }
            }
            //                                          //Delete all calculation associate to type.
            foreach (CalentityCalculationEntityDB calentity in darrcalentity)
            {
                Tools.subDeleteCondition(calentity.intPk, null, null, null, context_I);
                context_I.Calculation.Remove(calentity);
            }
            context_I.SaveChanges();
        }
        //--------------------------------------------------------------------------------------------------------------
        public static void subAddInitialDataToDb()
        {
            //                                          //Get the catalogs.
            PathX syspathA = DirectoryX.GetCurrent().GetPath().AddName("Z_BatchFiles");
            PathX syspath = syspathA.AddName("Catalog.csv");
            FileInfo sysfile = FileX.New(syspath);
            String[] arrEnumsData = sysfile.ReadAll();

            //                                              //Create a connection.
            Odyssey2Context context = new Odyssey2Context();

            foreach (String strEnum in arrEnumsData)
            {
                String strEnumName = strEnum.Substring(0, strEnum.IndexOf(EtElementTypeAbstract.strSeparator));
                String strEnumValue = strEnum.Substring(strEnum.IndexOf(EtElementTypeAbstract.strSeparator) + 1);

                EnumentityEnumerationEntityDB enumentity = context.Enumeration.FirstOrDefault(enumentity =>
                    enumentity.strEnumName == strEnumName && enumentity.strEnumValue == strEnumValue);

                if (
                    enumentity == null
                    )
                {
                    enumentity = new EnumentityEnumerationEntityDB
                    {
                        strEnumName = strEnumName,
                        strEnumValue = strEnumValue
                    };
                    context.Enumeration.Add(enumentity);
                    context.SaveChanges();
                }
            }


            //                                          //Get the catalogs.
            syspath = syspathA.AddName("NMTOKENCatalog.csv");
            sysfile = FileX.New(syspath);
            arrEnumsData = sysfile.ReadAll();

            foreach (String strEnum in arrEnumsData)
            {
                String strEnumName = "NM" + strEnum.Substring(0, strEnum.IndexOf(EtElementTypeAbstract.strSeparator));
                String strEnumValue = strEnum.Substring(strEnum.IndexOf(EtElementTypeAbstract.strSeparator) + 1);

                EnumentityEnumerationEntityDB enumentity = context.Enumeration.FirstOrDefault(enumentity =>
                    enumentity.strEnumName == strEnumName && enumentity.strEnumValue == strEnumValue);

                if (
                    enumentity == null
                    )
                {
                    enumentity = new EnumentityEnumerationEntityDB
                    {
                        strEnumName = strEnumName,
                        strEnumValue = strEnumValue
                    };
                    context.Enumeration.Add(enumentity);
                    context.SaveChanges();
                }
            }

            //                                          //Get the catalogs.
            syspath = syspathA.AddName("NMTOKENSCatalog.csv");
            sysfile = FileX.New(syspath);
            arrEnumsData = sysfile.ReadAll();

            foreach (String strEnum in arrEnumsData)
            {
                String strEnumName = "NMS" + strEnum.Substring(0, strEnum.IndexOf(EtElementTypeAbstract.strSeparator));
                String strEnumValue = strEnum.Substring(strEnum.IndexOf(EtElementTypeAbstract.strSeparator) + 1);

                EnumentityEnumerationEntityDB enumentity = context.Enumeration.FirstOrDefault(enumentity =>
                    enumentity.strEnumName == strEnumName && enumentity.strEnumValue == strEnumValue);

                if (
                    enumentity == null
                    )
                {
                    enumentity = new EnumentityEnumerationEntityDB
                    {
                        strEnumName = strEnumName,
                        strEnumValue = strEnumValue
                    };
                    context.Enumeration.Add(enumentity);
                    context.SaveChanges();
                }
            }

        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subAddAdmin()
        {
            //                                              //Create a connection.
            Odyssey2Context context = new Odyssey2Context();
            AdminentityAdministratorEntityDB adminentity = new AdminentityAdministratorEntityDB
            {
                strEmail = "odyssey2@mi4p.com",
                strName = "Dave",
                strLastName = "Hultin",
                strPassword = "fy$W$al22H!LMkz&"
            };
            context.Administrator.Add(adminentity);
            context.SaveChanges();

        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subAddAlertType()
        {
            //                                              //Create a connection.
            Odyssey2Context context = new Odyssey2Context();

            AlerttypeentityAlertTypeEntityDB alerttypeentity = new AlerttypeentityAlertTypeEntityDB
            {
                strType = AlerttypeentityAlertTypeEntityDB.strAvailability,
                strDescription = " is unavailable."
            };
            context.AlertType.Add(alerttypeentity);
            context.SaveChanges();

            alerttypeentity = new AlerttypeentityAlertTypeEntityDB
            {
                strType = AlerttypeentityAlertTypeEntityDB.strPeriod,
                strDescription = "You have a task in a process ready to start in the job: "
            };
            context.AlertType.Add(alerttypeentity);
            context.SaveChanges();

            alerttypeentity = new AlerttypeentityAlertTypeEntityDB
            {
                strType = AlerttypeentityAlertTypeEntityDB.strNewOrder,
                strDescription = "There is a new order with the Id: "
            };
            context.AlertType.Add(alerttypeentity);
            context.SaveChanges();

            alerttypeentity = new AlerttypeentityAlertTypeEntityDB
            {
                strType = AlerttypeentityAlertTypeEntityDB.strNewEstimate,
                strDescription = "There is a new estimate with the Id: "
            };
            context.AlertType.Add(alerttypeentity);
            context.SaveChanges();

            alerttypeentity = new AlerttypeentityAlertTypeEntityDB
            {
                strType = AlerttypeentityAlertTypeEntityDB.strTask,
                strDescription = "You have a task starting soon: "
            };
            context.AlertType.Add(alerttypeentity);
            context.SaveChanges();

            alerttypeentity = new AlerttypeentityAlertTypeEntityDB
            {
                strType = AlerttypeentityAlertTypeEntityDB.strDueDateAtRisk,
                strDescription = "A period was deleted. Due date for this job is at risk: "
            };
            context.AlertType.Add(alerttypeentity);
            context.SaveChanges();

            alerttypeentity = new AlerttypeentityAlertTypeEntityDB
            {
                strType = AlerttypeentityAlertTypeEntityDB.strDueDateInThePast,
                strDescription = "Due date for this job is at risk: "
            };
            context.AlertType.Add(alerttypeentity);
            context.SaveChanges();

            alerttypeentity = new AlerttypeentityAlertTypeEntityDB
            {
                strType = AlerttypeentityAlertTypeEntityDB.strItemsToAnswer,
                strDescription = " item(s) need(s) to be answer."
            };
            context.AlertType.Add(alerttypeentity);
            context.SaveChanges();

            alerttypeentity = new AlerttypeentityAlertTypeEntityDB
            {
                strType = AlerttypeentityAlertTypeEntityDB.strReadyToGo,
                strDescription = ". It's ready to go."
            };
            context.AlertType.Add(alerttypeentity);
            context.SaveChanges();

            alerttypeentity = new AlerttypeentityAlertTypeEntityDB
            {
                strType = AlerttypeentityAlertTypeEntityDB.strMentioned,
                strDescription = "You have been mentioned in a note for process "
            };
            context.AlertType.Add(alerttypeentity);
            context.SaveChanges();
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}

/*END-TASK*/
