/*TASK RP.JDF*/
using Odyssey2Backend.App;
using Odyssey2Backend.DB_Odyssey2;
using Odyssey2Backend.JsonTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TowaStandard;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: November 26, 2019. 

namespace Odyssey2Backend.XJDF
{
    //=================================================================================================================
    public class InttypIntentType : EtElementTypeAbstract
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //--------------------------------------------------------------------------------------------------------------
        public InttypIntentType(
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
            //                                              //Classification Intent.
            String strClassification_I
            )
            : base(intPk_I, strXJDFTypeId_I, strAddedBy_I, intPkPrintshop_I, strCustomTypeId_I, 
                  EtElementTypeAbstract.strIntent, strClassification_I)
        {
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //TRANSFORMATION METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public static void subAddInitialDataToDb()
        {
            //                                          //Get the types.
            PathX syspathA = DirectoryX.GetCurrent().GetPath().AddName("Z_BatchFiles");
            PathX syspath = syspathA.AddName("Intent.csv");
            FileInfo sysfile = FileX.New(syspath);
            String[] arrTypesData = sysfile.ReadAll();

            //                                              //Create a connection.
            Odyssey2Context context = new Odyssey2Context();

            foreach (String strTypeId in arrTypesData)
            {
                //                                              //Verify if the general type already exists.
                EtentityElementTypeEntityDB etentityGeneral = context.ElementType.FirstOrDefault(et =>
                    et.strCustomTypeId == strTypeId &&
                    et.strResOrPro == EtElementTypeAbstract.strIntent &&
                    et.intPrintshopPk == null);

                if (
                   //                                          //General type does not exist.
                   etentityGeneral == null
                   )
                {
                    //                                          //Add general type.
                    InttypIntentType.subAddGeneralTypeToDB(context, strTypeId, 
                        EtElementTypeAbstract.strIntent, null);
                }
            }

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
                et.strCustomTypeId == strTypeId_I &&
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
                    strCustomTypeId = strTypeId_I,
                    strResOrPro = strResOrPro_I
                };
                context_I.ElementType.Add(etentityGeneral);
                context_I.SaveChanges();

                intTypePk = etentityGeneral.intPk;

                //                                          //Add the attributes to db.
                InttypIntentType.subAddAttributesToGeneralTypeToDb(arrAttributesData, context_I,
                    intTypePk);
            }
            intTypePk = etentityGeneral.intPk;

            if (
                intTypeDadPk_I != null
                )
            {
                EtetentityElementTypeElementTypeEntityDB etetentity = 
                    new EtetentityElementTypeElementTypeEntityDB
                {
                    intPkElementTypeDad = (int)intTypeDadPk_I,
                    intPkElementTypeSon = intTypePk
                };
                context_I.ElementTypeElementType.Add(etetentity);
                context_I.SaveChanges();
            }
            
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static void subAddAttributesToGeneralTypeToDb(
            String[] arrstrAttributesData_I,
            Odyssey2Context context_I,
            int intTypePk_I
            )
        {
            foreach (String strAttributeData in arrstrAttributesData_I)
            {
                //                                          //Get XJDF name.
                String strData = strAttributeData;

                //                                      //Get the name.
                String strXJDFName = strData.Substring(0, strData.IndexOf(EtElementTypeAbstract.strSeparator));
                strData = strData.Substring(strData.IndexOf(EtElementTypeAbstract.strSeparator) + 1);

                //                                      //Get the cardinality.
                String strCardinality = strData.Substring(0, strData.IndexOf(EtElementTypeAbstract.strSeparator));
                strData = strData.Substring(strData.IndexOf(EtElementTypeAbstract.strSeparator) + 1);

                //                                      //Get the datatype.
                String strDatatype = strData.Substring(0, strData.IndexOf(EtElementTypeAbstract.strSeparator));
                strData = strData.Substring(strData.IndexOf(EtElementTypeAbstract.strSeparator) + 1);

                //                                      //Get the datatype.
                String strEnumAssoc = strData.Substring(0, strData.IndexOf(EtElementTypeAbstract.strSeparator));
                strData = strData.Substring(strData.IndexOf(EtElementTypeAbstract.strSeparator) + 1);

                //                                      //Get the description.
                String strDescription = strData;

                if (
                    strDatatype != EtElementTypeAbstract.strElement
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

                    context_I.Attribute.Add(attrentity);
                    context_I.SaveChanges();

                    int intAttributePk = attrentity.intPk;

                    //                                      //Create the relation between the type and the attr.
                    AttretentityAttributeElementTypeEntityDB attretentity = new AttretentityAttributeElementTypeEntityDB
                    {
                        intPkAttribute = intAttributePk,
                        intPkElementType = intTypePk_I
                    };

                    context_I.AttributeElementType.Add(attretentity);
                    context_I.SaveChanges();
                }
                else
                {
                    InttypIntentType.subAddGeneralTypeToDB(context_I, strXJDFName, 
                        EtElementTypeAbstract.strElement, intTypePk_I);
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public static Eleorattrjson1ElementOrAttributeJson2[] arreleorattrjson1Get(
            //                                              //Method that returns an array of attributes and elements of
            //                                              //      one element that starts beeing an intent type.

            //                                              //Primary key of the element.
            int intPk_I
            )
        {
            List<Eleorattrjson1ElementOrAttributeJson2> darreleorattrjson1 = new List<Eleorattrjson1ElementOrAttributeJson2>();
            //                                          //Get intent from database.
            Odyssey2Context context = new Odyssey2Context();
            EtentityElementTypeEntityDB etentity = context.ElementType.FirstOrDefault(etentity =>
                etentity.intPk == intPk_I);

            if (
                etentity != null
                )
            {
                //                                          //Get all attr relations with this.
                List<AttretentityAttributeElementTypeEntityDB> setattretentityCurrent = 
                    context.AttributeElementType.Where(attretentity => 
                    attretentity.intPkElementType == intPk_I).ToList();

                InttypIntentType.subGetAttributes(setattretentityCurrent, ref darreleorattrjson1);

                //                                          //Get all attr relations with this.
                List<EtetentityElementTypeElementTypeEntityDB> setetetentityCurrent =
                    context.ElementTypeElementType.Where(etetentity => 
                    etetentity.intPkElementTypeDad == intPk_I).ToList();

                InttypIntentType.subGetElements(setetetentityCurrent, ref darreleorattrjson1);
            }

            darreleorattrjson1 = darreleorattrjson1.OrderBy(eleorattr => eleorattr.strName).ToList();

            return darreleorattrjson1.ToArray();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subGetAttributes(
            //                                              //Get the attributes of an element.

            //                                              //List of attrentities that will be saved as json.
            List<AttretentityAttributeElementTypeEntityDB> darrattretentity_I,
            //                                              //List where the attr will be saved.
            ref List<Eleorattrjson1ElementOrAttributeJson2> darreleorattrjson1_M
            )
        {
            Odyssey2Context context = new Odyssey2Context();
            //                                          //Get all attr.
            foreach (AttretentityAttributeElementTypeEntityDB attretentity in darrattretentity_I)
            {
                AttrentityAttributeEntityDB attrentity = context.Attribute.FirstOrDefault(attrentity =>
                attrentity.intPk == attretentity.intPkAttribute);

                if (
                    (attrentity.strXJDFName != "Name") &&
                    (attrentity.strXJDFName != "Unit")
                    )
                {
                    Eleorattrjson1ElementOrAttributeJson2 eleorattrjson1 = new Eleorattrjson1ElementOrAttributeJson2();
                    eleorattrjson1.intPk = attrentity.intPk;
                    eleorattrjson1.strName = attrentity.strXJDFName;
                    eleorattrjson1.boolIsAttribute = true;

                    if (
                        darreleorattrjson1_M.FirstOrDefault(eleorattr => eleorattr.strName == eleorattrjson1.strName) == null
                        )
                    {
                        darreleorattrjson1_M.Add(eleorattrjson1);
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subGetElements(
            //                                              //Get the elements of an element.

            //                                              //List of etentities that will be saved as json.
            List<EtetentityElementTypeElementTypeEntityDB> darretetentity_I,
            //                                              //List where the attr will be saved.
            ref List<Eleorattrjson1ElementOrAttributeJson2> darreleorattrjson1_M
            )
        {
            Odyssey2Context context = new Odyssey2Context();
            foreach (EtetentityElementTypeElementTypeEntityDB etetetentity in darretetentity_I)
            {
                EtentityElementTypeEntityDB etentity = context.ElementType.FirstOrDefault(etentity =>
                etentity.intPk == etetetentity.intPkElementTypeSon);

                Eleorattrjson1ElementOrAttributeJson2 eleorattrjson1 = new Eleorattrjson1ElementOrAttributeJson2();
                eleorattrjson1.intPk = etentity.intPk;
                eleorattrjson1.strName = etentity.strXJDFTypeId;
                eleorattrjson1.boolIsAttribute = false;

                if (
                    darreleorattrjson1_M.FirstOrDefault(eleorattr => eleorattr.strName == eleorattrjson1.strName) == null
                    )
                {
                    darreleorattrjson1_M.Add(eleorattrjson1);
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
