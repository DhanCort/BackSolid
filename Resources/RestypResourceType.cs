/*TASK RP.JDF*/
using Odyssey2Backend.DB_Odyssey2;
using Odyssey2Backend.Infrastructure;
using Odyssey2Backend.JsonTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TowaStandard;
using System.Threading.Tasks;
using Odyssey2Backend.PrintShop;
using Odyssey2Backend.Utilities;


//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: November 21, 2019. 

namespace Odyssey2Backend.XJDF
{
    //=================================================================================================================
    public class RestypResourceType : EtElementTypeAbstract
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTANTS.

        //                                                  //Classification Resource type.
        //                                                  //Physical.
        public const String strResourceTypeConsumable = "Consumable";
        public const String strResourceTypeHandling = "Handling";
        public const String strResourceTypeQuantity = "Quantity";
        public const String strResourceTypeImplementation = "Implementation";
        public const String strResourceTypeCustom = "Custom";
        //                                                  //Not Physical.
        public const String strResourceTypeParameter = "Parameter";

        //                                                  //Resource type.
        public const String strResourceTypeMedia = "Media";
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //--------------------------------------------------------------------------------------------------------------
        public RestypResourceType(
            //                                              //Receive the values of the instance variables and the attr 
            //                                              //      and set it. The dictionary of attr can be empty. 

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
            //                                              //Resource classification
            String strClassification_I
            )
            : base(intPk_I, strXJDFTypeId_I, strAddedBy_I, intPkPrintshop_I, strCustomTypeId_I,
                  EtElementTypeAbstract.strResource, strClassification_I)
        {
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //TRANSFORMATION METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public static void subAddInitialDataToDb(
            )
        {
            //                                          //Get the types.
            PathX syspathA = DirectoryX.GetCurrent().GetPath().AddName("Z_BatchFiles");
            PathX syspath = syspathA.AddName("Resources.csv");
            FileInfo sysfile = FileX.New(syspath);
            String[] arrTypesData = sysfile.ReadAll();

            //                                              //Create a connection.
            Odyssey2Context context = new Odyssey2Context();

            foreach (String strTypeId in arrTypesData)
            {
                //                                              //Verify if the general type already exists.
                EtentityElementTypeEntityDB etentityGeneral = context.ElementType.FirstOrDefault(et =>
                    et.strCustomTypeId == EtElementTypeAbstract.strXJDFPrefix + strTypeId &&
                    et.strResOrPro == EtElementTypeAbstract.strResource &&
                    et.intPrintshopPk == null);

                if (
                   //                                          //General type does not exist.
                   etentityGeneral == null
                   )
                {
                    //                                          //Add general type.
                    RestypResourceType.subAddGeneralTypeToDB(context, strTypeId,
                        EtElementTypeAbstract.strResource, null);
                }
            }

            RestypResourceType.subModifyLooseBindingProcessName();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subAddGeneralTypeToDB(
            Odyssey2Context context_I,
            String strTypeId_I,
            String strResOrPro_I,
            int? intTypeDadPk_I
            )
        {
            //                                              //Verify if the general type already exists.
            EtentityElementTypeEntityDB etentityGeneral = context_I.ElementType.FirstOrDefault(et =>
                et.strCustomTypeId == EtElementTypeAbstract.strXJDFPrefix + strTypeId_I &&
                et.strResOrPro == strResOrPro_I &&
                et.intPrintshopPk == null);

            int intTypePk;
            if (
               //                                          //General type does not exist.
               etentityGeneral == null
               )
            {
                //                                          //Get the attributes.
                PathX syspathA = DirectoryX.GetCurrent().GetPath().AddName("Z_BatchFiles");
                PathX syspath = syspathA.AddName(strTypeId_I + ".csv");
                FileInfo sysfile = FileX.New(syspath);
                String[] arrAttributesData = sysfile.ReadAll();

                //                                          //Create the type.
                etentityGeneral = new EtentityElementTypeEntityDB
                {
                    strXJDFTypeId = strTypeId_I,
                    strAddedBy = EtElementTypeAbstract.strXJDFVersion,
                    intPrintshopPk = null,
                    strCustomTypeId = EtElementTypeAbstract.strXJDFPrefix + strTypeId_I,
                    strResOrPro = strResOrPro_I
                };
                context_I.ElementType.Add(etentityGeneral);
                context_I.SaveChanges();

                intTypePk = etentityGeneral.intPk;

                //                                          //Add the attributes to db.
                RestypResourceType.subAddAttributesToGeneralTypeToDb(arrAttributesData, context_I,
                    etentityGeneral);
            }
            intTypePk = etentityGeneral.intPk;

            if (
                intTypeDadPk_I != null
                )
            {
                int intTypeDad = (int)intTypeDadPk_I;
                EtetentityElementTypeElementTypeEntityDB etetentity =
                    new EtetentityElementTypeElementTypeEntityDB
                    {
                        intPkElementTypeDad = intTypeDad,
                        intPkElementTypeSon = intTypePk
                    };
                context_I.ElementTypeElementType.Add(etetentity);
                context_I.SaveChanges();
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static void subAddAttributesToGeneralTypeToDb(
            String[] arrstrAttributesData_I,
            Odyssey2Context context_M,
            EtentityElementTypeEntityDB etentity_I
            )
        {
            List<String> darrstrAttributesData = arrstrAttributesData_I.ToList();
            if (
                etentity_I.strResOrPro == EtElementTypeAbstract.strResource
                )
            {
                //                                          //Get the attributes for a resource.
                PathX syspathA = DirectoryX.GetCurrent().GetPath().AddName("Z_BatchFiles");
                PathX syspath = syspathA.AddName("Resource" + ".csv");
                FileInfo sysfile = FileX.New(syspath);
                String[] arrAttributesData = sysfile.ReadAll();

                darrstrAttributesData.AddRange(arrAttributesData);

                //                                          //Get the attributes for a resource set.
                PathX syspathAB = DirectoryX.GetCurrent().GetPath().AddName("Z_BatchFiles");
                PathX syspathB = syspathAB.AddName("ResourceSet" + ".csv");
                FileInfo sysfileB = FileX.New(syspathB);
                String[] arrAttributesDataB = sysfileB.ReadAll();

                darrstrAttributesData.AddRange(arrAttributesDataB);
            }

            foreach (String strAttributeData in darrstrAttributesData)
            {
                //                                          //Get XJDF name.
                String strData = strAttributeData;

                //                                          //Get the name.
                String strXJDFName = strData.Substring(0, strData.IndexOf(EtElementTypeAbstract.strSeparator));
                strData = strData.Substring(strData.IndexOf(EtElementTypeAbstract.strSeparator) + 1);

                //                                          //Get the cardinality.
                String strCardinality = strData.Substring(0, strData.IndexOf(EtElementTypeAbstract.strSeparator));
                strData = strData.Substring(strData.IndexOf(EtElementTypeAbstract.strSeparator) + 1);

                //                                          //Get the datatype.
                String strDatatype = strData.Substring(0, strData.IndexOf(EtElementTypeAbstract.strSeparator));
                strData = strData.Substring(strData.IndexOf(EtElementTypeAbstract.strSeparator) + 1);

                //                                          //Get the datatype.
                String strEnumAssoc = strData.Substring(0, strData.IndexOf(EtElementTypeAbstract.strSeparator));
                strData = strData.Substring(strData.IndexOf(EtElementTypeAbstract.strSeparator) + 1);

                //                                          //Get the description.
                String strDescription = strData;

                if (
                    strDatatype != EtElementTypeAbstract.strElement
                    )
                {
                    Odyssey2Context contextConsult = new Odyssey2Context();

                    List<AttrentityAttributeEntityDB> darrattrentityExists =
                         (from attr in contextConsult.Attribute
                          join attret in contextConsult.AttributeElementType
                          on attr.intPk equals attret.intPkAttribute
                          where attret.intPkElementType == etentity_I.intPk
                          && attr.strCustomName == EtElementTypeAbstract.strXJDFPrefix + strXJDFName
                          && attr.strScope == "XJDF2.0"
                          select attr).ToList();

                    if (
                        darrattrentityExists.Count == 0
                        )
                    {
                        //                                      //Add the attr to db.
                        AttrentityAttributeEntityDB attrentity = new AttrentityAttributeEntityDB
                        {
                            strXJDFName = strXJDFName,
                            strCustomName = EtElementTypeAbstract.strXJDFPrefix + strXJDFName,
                            strCardinality = strCardinality,
                            strDatatype = strDatatype,
                            strEnumAssoc = strEnumAssoc,
                            strDescription = strDescription,
                            strScope = EtElementTypeAbstract.strXJDFVersion
                        };

                        context_M.Attribute.Add(attrentity);
                        context_M.SaveChanges();

                        int intAttributePk = attrentity.intPk;

                        //                                      //Create the relation between the type and the attr.
                        AttretentityAttributeElementTypeEntityDB attretentity = new AttretentityAttributeElementTypeEntityDB
                        {
                            intPkAttribute = intAttributePk,
                            intPkElementType = etentity_I.intPk
                        };

                        context_M.AttributeElementType.Add(attretentity);
                        context_M.SaveChanges();
                    }
                }
                else
                {
                    RestypResourceType.subAddGeneralTypeToDB(context_M, strXJDFName,
                        EtElementTypeAbstract.strElement, etentity_I.intPk);
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subModifyLooseBindingProcessName(
            )
        {
            Odyssey2Context context = new Odyssey2Context();
            EtentityElementTypeEntityDB etentity = context.ElementType.FirstOrDefault(etentity =>
                etentity.strCustomTypeId == "LooseBindingDuplicated" &&
                etentity.strAddedBy == EtElementTypeAbstract.strXJDFVersion);
            if (
                etentity != null
                )
            {
                etentity.strCustomTypeId = "LooseBinding";
                context.SaveChanges();
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subUpdateInitialClassificationResourceToDb(
            )
        {
            //                                          //Get the types.
            PathX syspathA = DirectoryX.GetCurrent().GetPath().AddName("Z_BatchFiles");
            PathX syspath = syspathA.AddName("ClassificationResource.csv");
            FileInfo sysfile = FileX.New(syspath);
            String[] arrResourceData = sysfile.ReadAll();

            //                                              //Create a connection.
            Odyssey2Context context = new Odyssey2Context();

            foreach (String strResource in arrResourceData)
            {
                //                                          //Get Resource'name 
                String strResourceName = strResource.Substring(0,
                    strResource.IndexOf(EtElementTypeAbstract.strSeparator));
                //                                          //Get Resource'classification 
                String strClassificationRes = strResource.Substring(
                    strResource.IndexOf(EtElementTypeAbstract.strSeparator) + 1);

                //                                          //Verify if the classification exist 
                //                                          //     in ResourceClassification table db.
                RcentityResourceClassification tcentity = context.ResourceClassification.FirstOrDefault(
                    tc => tc.strName == strClassificationRes);

                if (
                    tcentity == null
                    )
                {
                    //Add Classification to TypeClassification
                    tcentity = new RcentityResourceClassification();
                    tcentity.strName = strClassificationRes;
                    context.ResourceClassification.Add(tcentity);
                    context.SaveChanges();
                }

                //                                              //Verify if the resource already exists.
                EtentityElementTypeEntityDB etentityGeneral = context.ElementType.FirstOrDefault(et =>
                    et.strCustomTypeId == EtElementTypeAbstract.strXJDFPrefix + strResourceName &&
                    et.strResOrPro == EtElementTypeAbstract.strResource &&
                    et.intPrintshopPk == null);

                if (
                   //                                       //Resource exist and string classification is empty
                   //                                       //    therefor update the clasification.
                   etentityGeneral != null &&
                   etentityGeneral.strClassification == null
                   )
                {
                    //                                          //Update classification.
                    etentityGeneral.strClassification = strClassificationRes;
                    context.ElementType.Update(etentityGeneral);
                    context.SaveChanges();
                }

            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public ResjsonResourceJson[] arrresjsonTemplate(
            //                                              //Method that search all the resources associated to this 
            //                                              //      type and return the templates.
            )
        {
            //                                              //Establish the conection with tha database.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get all the resoures template for this type. 
            IQueryable<EleentityElementEntityDB> seteleentity = context.Element.Where(eleentity =>
                eleentity.intPkElementType == this.intPk &&
                eleentity.boolIsTemplate == true);

            List<ResjsonResourceJson> darrresjson = new List<ResjsonResourceJson>();
            foreach (EleentityElementEntityDB eleentity in seteleentity)
            {
                ResjsonResourceJson resjson = new ResjsonResourceJson(eleentity.intPk, eleentity.strElementName, null,
                    null, null, null, null, null, null);

                darrresjson.Add(resjson);
            }

            darrresjson = darrresjson.OrderBy(res => res.strTypeId).ToList();
            return darrresjson.ToArray();
        }

        //--------------------------------------------------------------------------------------------------------------
        public bool boolIsMedia(
            )
        {
            bool boolIsMedia = false;
            if (
                this.strXJDFTypeId == RestypResourceType.strResourceTypeMedia
                )
            {
                boolIsMedia = true;
            }
            return boolIsMedia;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool boolIsPhysical(

            //                                              //String to check
            String str_I
            )
        {
            bool boolIsPhysical = false;
            if (
                (str_I == RestypResourceType.strResourceTypeConsumable) ||
                str_I == RestypResourceType.strResourceTypeHandling ||
                str_I == RestypResourceType.strResourceTypeQuantity ||
                str_I == RestypResourceType.strResourceTypeImplementation ||
                str_I == RestypResourceType.strResourceTypeCustom
                )
            {
                boolIsPhysical = true;
            }
            return boolIsPhysical;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool boolIsDeviceToolOrCustom(

            //                                              //Type of resource.
            RestypResourceType restyp_I
            )
        {
            bool boolIsDeviceToolOrCustom = false;
            if (
                restyp_I.strXJDFTypeId == EtElementTypeAbstract.strResourceTypeDevice ||
                restyp_I.strXJDFTypeId == EtElementTypeAbstract.strResourceTypeTool ||
                restyp_I.strCustomTypeId == EtElementTypeAbstract.strResCustomType
                )
            {
                boolIsDeviceToolOrCustom = true;
            }
            return boolIsDeviceToolOrCustom;
        }

        public static bool boolIsCustom(

            int intPkResourceType_I,
            Odyssey2Context context_I
            )
        {
            EtentityElementTypeEntityDB etentity = context_I.ElementType.FirstOrDefault(type => 
                type.intPk == intPkResourceType_I);

            bool boolIsCustom = false;
            if (
                etentity.strCustomTypeId == EtElementTypeAbstract.strResCustomType
                )
            {
                boolIsCustom = true;
            }
            return boolIsCustom;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool boolIsDispensable(
            //                                              //Verify if the type has no more:
            //                                              //      -Resources or templates
            //                                              //      -Links
            //                                              //      -Is Input or Output of a process.

            int intPkResTyp_I,
            Odyssey2Context context_I
            )
        {
            bool boolIsDispensable = false;

            //                                              //Get the first resource.
            EleentityElementEntityDB eleentity = context_I.Element.FirstOrDefault(ele =>
                ele.intPkElementType == intPkResTyp_I);
            bool boolHasResources = eleentity != null;

            if (
                !boolHasResources
                )
            {
                //                                          //Get the first link.
                bool boolHasLinks = false;
                List<EtetentityElementTypeElementTypeEntityDB> darretetentity = context_I.ElementTypeElementType.Where(
                    etet => etet.intPkElementTypeSon == intPkResTyp_I).ToList();

                int intI = 0;
                /*UNTIL-DO*/
                while (!(
                    (intI >= darretetentity.Count()) ||
                    boolHasLinks
                    ))
                {
                    IoentityInputsAndOutputsEntityDB ioentity = context_I.InputsAndOutputs.FirstOrDefault(io =>
                        io.intnPkElementElementType == darretetentity[intI].intPk);
                    if (
                        ioentity != null
                        )
                    {
                        boolHasLinks = true;
                    }

                    intI = intI + 1;
                }

                if (
                    !boolHasLinks
                    )
                {
                    //                                          //Get where the type is IO.
                    EleetentityElementElementTypeEntityDB eleetentity = context_I.ElementElementType.FirstOrDefault(
                        eleet => eleet.intPkElementTypeSon == intPkResTyp_I);
                    bool boolIsIO = eleetentity != null;
                    if (
                        !boolIsIO
                        )
                    {
                        boolIsDispensable = true;
                    }
                }
            }
            return boolIsDispensable;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subAddResourceType(

            int intPkBaseResourceType_I,
            PsPrintShop ps_I,
            Odyssey2Context context_M,
            ref int intPkNewResourceType_IO
            )
        {
            //                                          //Find base workflow resource type
            EtentityElementTypeEntityDB etentityBaseResourceType = context_M.ElementType.FirstOrDefault(et =>
                et.intPk == intPkBaseResourceType_I);

            //                                          //Find new workflow resource type
            EtentityElementTypeEntityDB etentityNewResourceType = context_M.ElementType.FirstOrDefault(et =>
                et.strCustomTypeId == etentityBaseResourceType.strCustomTypeId &&
                et.strResOrPro == EtElementTypeAbstract.strResource &&
                et.intPrintshopPk == ps_I.intPk);

            int intStatus = 400;
            if (
                //                                      //There is not a clone.
                (etentityNewResourceType == null)
                )
            {
                //                                      //Find generic resource type
                EtentityElementTypeEntityDB etentity = context_M.ElementType.FirstOrDefault(et =>
                    et.strCustomTypeId == etentityBaseResourceType.strCustomTypeId &&
                    et.strResOrPro == EtElementTypeAbstract.strResource &&
                    et.intPrintshopPk == null);

                if (
                    etentity != null
                    )
                {
                    RestypResourceType restyp = new RestypResourceType(etentity.intPk, etentity.strXJDFTypeId,
                        etentity.strAddedBy, etentity.intPrintshopPk, etentity.strCustomTypeId, 
                        etentity.strClassification);

                    if (
                        //                                  //The type is pass as ref, because it can be 
                        //                                  //      changed from the XJDF to the clone.
                        ResResource.boolIsValidType(ps_I, context_M, ref restyp)
                        )
                    {
                        if (
                            restyp.intPkPrintshop == ps_I.intPk
                            )
                        {
                            intPkNewResourceType_IO = restyp.intPk;
                            intStatus = 200;
                        }
                    }
                }
                else
                //                                          //It is a custom type
                {
                    //                                      //Verify if the custom type for the printshop exists.
                    EtentityElementTypeEntityDB etentityCustom = context_M.ElementType.FirstOrDefault(
                        eleet => eleet.strCustomTypeId == RestypResourceType.strResCustomType && 
                        eleet.intPrintshopPk == ps_I.intPk);

                    if (
                        etentityCustom != null
                        )
                    {
                        //                                  //Do nothing
                    }
                    else
                    {
                        //                                  //Add CustomType to printshop
                        //                                  //Create the entity
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

                    intPkNewResourceType_IO = etentityCustom.intPk;
                    intStatus = 200;
                }
            }
            else
            {
                intPkNewResourceType_IO = etentityNewResourceType.intPk;
                intStatus = 200;
            }

            if (
                !(intStatus == 200)
                )
            {
                throw new CustomException("The resource type could not be added.");
            }
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
