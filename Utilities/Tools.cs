/*TASK RP. UTILITIES*/
using Odyssey2Backend.Infrastructure;
using Odyssey2Backend.JsonTypes;
using Odyssey2Backend.XJDF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TowaStandard;
using System.IO;
using System.Diagnostics;
using Odyssey2Backend.DB_Odyssey2;

//                                                          //AUTHOR: Towa (DPG - Daniel Pena).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: November 11, 2019.

namespace Odyssey2Backend.Utilities
{
    //==================================================================================================================
    public static class Tools
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTANT VARIABLES.

        public const char charConditionSeparator = '|';
        public const char charOpenParetheses = '<';
        public const char charCloseParentheses = '>';
        public const String strMoreThanOperator = ">";
        public const String strLessThanOperator = "<";
        public const String strMoreAndEqualThanOperator = ">=";
        public const String strLessAndEqualThanOperator = "<=";

        public const String strEqualsOperator = "==";
        public const String strDistinctOperator = "!=";
        public const String strAndOperator = "AND";
        public const String strOrOperator = "OR";

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //TRANSFORMATION METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public static void subAddCondition(
            //                                              //Add a relation between a condition and a calculation or 
            //                                              //      link.
            //                                              //It is necessary that the intnPkCalculation_I,
            //                                              //      intnPkLinkNode_I or intnPkIoentity_I must be a valid
            //                                              //      pk.

            int? intnPkCalculation_I,
            int? intnPkLinkNode_I,
            int? intnPkIoentity_I,
            int? intnPkTransformCalculation_I,
            GpcondjsonGroupConditionJson gpcondjson_I,
            Odyssey2Context context_M
            )
        {
            if (
                //                                          //There is a condition to add.
                gpcondjson_I != null &&
                //                                          //There is a calculation, link or io to associate.
                ((intnPkCalculation_I != null && intnPkLinkNode_I == null && intnPkIoentity_I == null && 
                intnPkTransformCalculation_I == null) ||
                (intnPkCalculation_I == null && intnPkLinkNode_I != null && intnPkIoentity_I == null && 
                intnPkTransformCalculation_I == null) ||
                (intnPkCalculation_I == null && intnPkLinkNode_I == null && intnPkIoentity_I != null && 
                intnPkTransformCalculation_I == null) ||
                (intnPkCalculation_I == null && intnPkLinkNode_I == null && intnPkIoentity_I == null && 
                intnPkTransformCalculation_I != null))
                )
            {
                //                                          //Add the condition with all the single conditions and the
                //                                          //      groups.
                int intPkGroupCondition;
                Tools.subAddGroupConditionRecursive(null, gpcondjson_I, out intPkGroupCondition, context_M);

                //                                          //Once the condition was added, create the relation between 
                //                                          //      the condition and the calculation or link.
                ColcondentityCalculationOrLinkConditionEntityDB colcondentity = new
                    ColcondentityCalculationOrLinkConditionEntityDB
                {
                    intnPkCalculation = intnPkCalculation_I,
                    intnPkLinkNode = intnPkLinkNode_I,
                    intnPkInputOrOutput = intnPkIoentity_I,
                    intnPkTransformCalculation = intnPkTransformCalculation_I,
                    intPkGroupCondition = intPkGroupCondition
                };
                context_M.CalculationOrLinkCondition.Add(colcondentity);
                context_M.SaveChanges();
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subAddGroupConditionRecursive(
            //                                              //Add a group condition, it verifies if the condition is a
            //                                              //      group or a single condition to create the group.
            //                                              //It returns the intnPkCondition or intnPkGroup that will be
            //                                              //      associated to the calculation or link.

            //                                              //The first time this variable is null.
            int? intnPkGroupBelongsTo_I,
            //                                              //Group to be added, the first group must contain one
            //                                              //      condition or one group.
            GpcondjsonGroupConditionJson gpcondjson_I,
            out int intPkGroupCondition_O,
            Odyssey2Context context_M
            )
        {
            //                                              //Add the group.
            GpcondentityGroupConditionEntityDB gpcondentity = new GpcondentityGroupConditionEntityDB
            {
                strOperator = gpcondjson_I.strOperator,
                intnPkGroupCondition = intnPkGroupBelongsTo_I
            };
            context_M.GroupCondition.Add(gpcondentity);
            context_M.SaveChanges();

            intPkGroupCondition_O = gpcondentity.intPk;

            if (
                gpcondjson_I.arrcond != null
                )
            {

                //                                              //Add the single conditions.
                foreach (CondjsonConditionJson conjson in gpcondjson_I.arrcond)
                {
                    CondentityConditionEntityDB condentity = new CondentityConditionEntityDB
                    {
                        intnPkAttribute = conjson.intnPkAttribute,
                        strCondition = conjson.strCondition,
                        strValue = conjson.strValue,
                        intPkGroupCondition = gpcondentity.intPk
                    };
                    context_M.Condition.Add(condentity);
                }
            }

            if (
                gpcondjson_I.arrgpcond != null
                )
            {
                //                                              //Add the groups.
                foreach (GpcondjsonGroupConditionJson gpcondjson in gpcondjson_I.arrgpcond)
                {
                    int intPkGroupCondition;
                    Tools.subAddGroupConditionRecursive(gpcondentity.intPk, gpcondjson,
                        out intPkGroupCondition, context_M);
                }
            }
            context_M.SaveChanges();
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subDeleteCondition(
            //                                              //Delete the condition for a calculation or link.
            //                                              //It is necessary to receive the correct Pk of the
            //                                              //      calculation or link.

            int? intnPkCalculation_I,
            int? intnPkLinkNode_I,
            int? intnPkIoentity_I,
            int? intnPkTransformCalculation_I,
            Odyssey2Context context_M
            )
        {
            ColcondentityCalculationOrLinkConditionEntityDB colcondentity = context_M.CalculationOrLinkCondition
                .FirstOrDefault(colcond => colcond.intnPkCalculation == intnPkCalculation_I &&
                colcond.intnPkLinkNode == intnPkLinkNode_I && colcond.intnPkInputOrOutput == intnPkIoentity_I &&
                colcond.intnPkTransformCalculation == intnPkTransformCalculation_I);

            if (
                //                                          //There is a condition to delete.
                colcondentity != null
                )
            {
                //                                          //Get the group condition.
                GpcondentityGroupConditionEntityDB gpcondentity = context_M.GroupCondition.FirstOrDefault(gpcond =>
                    gpcond.intPk == colcondentity.intPkGroupCondition);

                //                                          //Delete the group condition.
                Tools.subDeleteGroupConditionRecursive(gpcondentity, context_M);

                context_M.CalculationOrLinkCondition.Remove(colcondentity);
                context_M.GroupCondition.Remove(gpcondentity);
                context_M.SaveChanges();
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subDeleteGroupConditionRecursive(
            //                                              //Delete a group condition.

            GpcondentityGroupConditionEntityDB gpcondentity_I,
            Odyssey2Context context_M
            )
        {
            //                                              //Delete the single conditions.
            IQueryable<CondentityConditionEntityDB> setcondentity = context_M.Condition.Where(cond =>
            cond.intPkGroupCondition == gpcondentity_I.intPk);

            context_M.Condition.RemoveRange(setcondentity);

            //                                              //Delete the groups.
            List<GpcondentityGroupConditionEntityDB> darrgpcondentity = context_M.GroupCondition.Where(gpcond =>
            gpcond.intnPkGroupCondition == gpcondentity_I.intPk).ToList();

            foreach (GpcondentityGroupConditionEntityDB gpcondentity in darrgpcondentity)
            {
                Tools.subDeleteGroupConditionRecursive(gpcondentity, context_M);

                context_M.GroupCondition.Remove(gpcondentity);
            }
            context_M.SaveChanges();
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public static String[] arrstrAscendantName(
            //                                              //Returns an array of strings with the names of the 
            //                                              //      ascendant elements according with the pks.

            //                                              //String with the ascendant elements pk order.
            //                                              //Example:
            //                                              //      "1|2|3|4"
            //                                              //The string must have one pk at least that belongs to an 
            //                                              //      attribute.
            //                                              //If the string has more than one number, the last one 
            //                                              //      belongst to an attribute and the others belong to 
            //                                              //      the ascendant elements.
            String strElementsPks_I
            )
        {
            List<String> darrstr = new List<String>();
            if (
                strElementsPks_I != null
                )
            {
                //                                          //Get name of each ascendant element.
                int[] arrintPk = arrintElementPk(strElementsPks_I);

                if (
                    //                                      //There is ascendant elements.
                    arrintPk.Length > 1
                    )
                {
                    if (
                        EtElementTypeAbstract.etFromDB(arrintPk[0]).strResOrPro == EtElementTypeAbstract.strIntent
                        )
                    {
                        InttypIntentType inttem = (InttypIntentType)EtElementTypeAbstract.etFromDB(arrintPk[0]);
                        darrstr.Add(inttem.strCustomTypeId);
                    }
                    else
                    {
                        EletemElementType et = (EletemElementType)EtElementTypeAbstract.etFromDB(arrintPk[0]);
                        darrstr.Add(et.strCustomTypeId);
                    }

                    //                                      //Get middle ascendants which are elements.
                    for (int intI = 1; intI < (arrintPk.Length - 1); intI = intI + 1)
                    {
                        EletemElementType et = (EletemElementType)EtElementTypeAbstract.etFromDB(arrintPk[intI]);
                        darrstr.Add(et.strCustomTypeId);
                    }
                }

                AttrAttribute attr = AttrAttribute.attrFromDB(arrintPk[arrintPk.Length - 1]);
                darrstr.Add(attr.strCustomName);
            }
            return darrstr.ToArray();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static int[] arrintElementPk(
            String strPkElementsInAscendant_I
            )
        {
            List<int> darrintPkElements = new List<int>();
            if (
                (strPkElementsInAscendant_I != null) &&
                (strPkElementsInAscendant_I != "")
                )
            {
                if (
                    strPkElementsInAscendant_I.Contains("|")
                    )
                {
                    //                                              //get pk element of each element in of the string                                   
                    String[] arrstrPkElements = strPkElementsInAscendant_I.Split("|");

                    //                                              //Parse to int the string number pk
                    foreach (var strPk in arrstrPkElements)
                    {
                        darrintPkElements.Add(strPk.ParseToInt());
                    }
                }
                else
                {
                    if (
                        strPkElementsInAscendant_I.IsParsableToInt()
                        )
                    {
                        darrintPkElements.Add(strPkElementsInAscendant_I.ParseToInt());
                    }
                }
            }
            return darrintPkElements.ToArray();
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subExceptionHandler(
            //                                              //Creates a log about the exceptions.

            //                                              //Exception to log.
            Exception ex_I,
            ref int intStatus_M,
            ref String strUserMessage_M,
            ref String strDevMessage_M
            )
        {
            intStatus_M = 499;
            strUserMessage_M = "Something is wrong.";
            strDevMessage_M = ex_I.Message;

            StreamWriter systextwriterLog;
            try
            {
                //                                          //To save file.
                PathX syspathLogFiles = DirectoryX.GetCurrent().GetPath().AddName("Z_LogFiles");

                if (
                    //                                      //Directory does not exist yet
                    !Directory.Exists(syspathLogFiles.FullPath)
                    )
                {
                    //                                      //Create directory
                    syspathLogFiles = new PathX(Directory.CreateDirectory(syspathLogFiles.FullPath).FullPath());
                }

                String strCurrentDate = ZonedTimeTools.ztimeNow.Date.ToString();

                PathX syspathFileForErrorLog = syspathLogFiles.AddName(strCurrentDate + " - Odyssey2 Log" + ".txt");

                if (
                    //                                      //The File does not exist yet
                    !File.Exists(syspathFileForErrorLog.FullPath)
                    )
                {
                    //                                      //Generate file
                    FileInfo sysfileNew = new FileInfo(syspathFileForErrorLog.FullPath);
                    systextwriterLog = sysfileNew.CreateText();
                }
                else
                {
                    //                                      //Add text to file
                    systextwriterLog = new StreamWriter(syspathFileForErrorLog.FullPath, true);
                }

                // StreamWriter systextwriterLog = TextWriterX.New(FileX.New(syspathFileForErrorLog));

                //                                          //Write first line in log
                //String strNameUser = PathX.GetUserPath().Name;
                String strCurrentTime = ZonedTimeTools.ztimeNow.Time.ToString();
                String strTextErrorInfoToLog = String.Format("Date: {0}", strCurrentDate + " " +
                    strCurrentTime);

                //                                          //Write error info 
                systextwriterLog.WriteLine(strTextErrorInfoToLog);

                //                                          //To easy code
                String strTextErrorToLog = (strDevMessage_M.Length == 0) ? "<NO INFO TO LOG>" :
                    String.Join(Environment.NewLine, "Error message: " + strDevMessage_M);

                //                                          //Write error message
                systextwriterLog.WriteLine(strTextErrorToLog);

                if (
                    ex_I.InnerException != null
                    )
                {
                    //                                      //Write inner exception
                    systextwriterLog.WriteLine("Inner exception message:");
                    systextwriterLog.WriteLine("    {0}", ex_I.InnerException.Message);
                }

                //                                          //Get error stack trace
                StackTrace st = new StackTrace(ex_I, true);

                var thisasm = System.Reflection.Assembly.GetExecutingAssembly();
                var method = st.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;

                StackFrame methodFrame = st.GetFrames().First(frame => frame.GetMethod().Name == method);
                //                                          //Get index of the last user method
                int index = Array.IndexOf(st.GetFrames(), methodFrame);

                //                                          //Overwrite error stack trace with user methods
                st = new StackTrace(ex_I, index, true);

                //                                          //Get the last frame in the stack. In this case the service.
                StackFrame sfService = st.GetFrame(st.FrameCount - 1);
                String strStackTrace = sfService.GetMethod().Name;

                systextwriterLog.WriteLine("Exception in:");
                if (st.FrameCount > 1)
                {
                    StackFrame sf2;
                    int intErrorLineNumber = 0;
                    String strFileName = "";
                    int intI = st.FrameCount - 2;
                    /*WHILE-DO*/
                    while (
                        intI >= 0
                        )
                    {
                        sf2 = st.GetFrame(intI);
                        strStackTrace = strStackTrace + "." + sf2.GetMethod().Name;
                        intErrorLineNumber = sf2.GetFileLineNumber();
                        strFileName = sf2.GetFileName();
                        intI = intI - 1;
                    }

                    systextwriterLog.WriteLine("  Method: {0}", strStackTrace);
                    systextwriterLog.WriteLine("    Line Number: {0}", intErrorLineNumber);
                    systextwriterLog.WriteLine("    File: {0}", strFileName);
                }

                systextwriterLog.WriteLine("  Service: {0}", sfService.GetMethod().Name);
                systextwriterLog.WriteLine("    Line Number: {0}", sfService.GetFileLineNumber());
                systextwriterLog.WriteLine("    File: {0}", sfService.GetFileName());
                systextwriterLog.WriteLine("----------------------------------------------------------------------------" +
                    "---------------------------------------------------\n");
                systextwriterLog.Dispose();
                systextwriterLog.Close();
            }
            catch (Exception exc)
            {
                //                                          //To save file.
                PathX syspathLogFiles = DirectoryX.GetCurrent().GetPath().AddName("Z_LogFiles");

                if (
                    //                                      //Directory does not exist yet
                    !Directory.Exists(syspathLogFiles.FullPath)
                    )
                {
                    //                                      //Create directory
                    syspathLogFiles = new PathX(Directory.CreateDirectory(syspathLogFiles.FullPath).FullPath());
                }

                String strCurrentDate = ZonedTimeTools.ztimeNow.Date.ToString();

                PathX syspathFileForErrorLog = syspathLogFiles.AddName(strCurrentDate + " - Error generating file"
                    + ".txt");

                if (
                    //                                      //The File does not exist yet
                    !File.Exists(syspathFileForErrorLog.FullPath)
                    )
                {
                    //                                      //Generate file
                    FileInfo sysfileNew = new FileInfo(syspathFileForErrorLog.FullPath);
                    systextwriterLog = sysfileNew.CreateText();
                }
                else
                {
                    //                                      //Add text to file
                    systextwriterLog = new StreamWriter(syspathFileForErrorLog.FullPath, true);
                }

                //                                          //Write first line in log
                String strCurrentTime = ZonedTimeTools.ztimeNow.Time.ToString();
                String strTextErrorInfoToLog = String.Format("{0}", strCurrentDate + " " + strCurrentTime);

                //                                          //Write error message
                systextwriterLog.WriteLine("Error message: ");
                systextwriterLog.WriteLine("{0}", exc.Message);
                systextwriterLog.WriteLine("Error stack trace: ");
                systextwriterLog.WriteLine("{0}", exc.StackTrace);
                systextwriterLog.WriteLine("----------------------------------------------------------------------" +
                    "---------------------------------------------------------\n");
                systextwriterLog.Dispose();
                systextwriterLog.Close();
            }

            if (
                systextwriterLog != null
                )
            {
                systextwriterLog.Dispose();
                systextwriterLog.Close();
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool boolValidConditionList(
            //                                              //Verify if all condition in a list are valid.

            GpcondjsonGroupConditionJson gpcondjson_I
            )
        {
            bool boolValidConditionList = false;

            if (gpcondjson_I != null)
            {
                if (
                    //                                      //The condition operator at first level has to be AND/OR.
                    (gpcondjson_I.strOperator == null) ||
                    (gpcondjson_I.strOperator != null &&
                    (gpcondjson_I.strOperator.ToUpper() == Tools.strAndOperator ||
                    gpcondjson_I.strOperator.ToUpper() == Tools.strOrOperator))
                    )
                {
                    if (
                        //                                  //We dont have single conditions.
                        gpcondjson_I.arrcond.Length == 0 ||
                        //                                  //We have single conditions we have to evaluate and verify
                        //                                  //      all of them are valid.
                        (gpcondjson_I.arrcond.Length > 0 &&
                        Tools.boolValidArrCond(gpcondjson_I.arrcond))
                        )
                    {
                        if (
                            //                              //There are not more groups.
                            gpcondjson_I.arrgpcond.Length == 0 ||
                            //                              //Thera are more groups, evaluate them and verify all are
                            //                              //      valid.
                            (
                            gpcondjson_I.arrgpcond.Length > 0 && Tools.boolValidConditionGroups(gpcondjson_I.arrgpcond)
                            )
                            )
                        {
                            boolValidConditionList = true;
                        }
                    }
                }
            }
            else
            {
                boolValidConditionList = true;
            }

            return boolValidConditionList;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static bool boolValidArrCond(
            //                                              //Verify a list of conditions and check if all of them are
            //                                              //      valid.

            CondjsonConditionJson[] arrcond_I
            )
        {
            bool boolValidArrCond = true;

            /*WHILE-DO*/
            int intI = 0;
            while (
                //                                          //There are valid conditions.
                boolValidArrCond &&
                //                                          //There´re still conditions to evaluate
                intI < arrcond_I.Length
                )
            {
                //                                          //Current condition.
                CondjsonConditionJson conjson = arrcond_I[intI];

                //                                          //If pkattribute is null, means there is a quantity 
                //                                          //      condition.
                AttrAttribute attr = null;
                if (
                    conjson.intnPkAttribute != null
                    )
                {
                    //                                      //Get attribute instance.
                    attr = AttrAttribute.attrFromDB((int)conjson.intnPkAttribute);
                    //                                      //Is a valid attribute, we keep consider valid condition.
                    boolValidArrCond = attr != null ? true : false;
                }
                else
                {
                    //                                      //It is a quantity condition.
                    boolValidArrCond = true;
                }

                if (
                    //                                      //Only if the attribute is valid, we can continue.
                    boolValidArrCond
                    )
                {
                    //                                      //Verify if the condition of this attribute is valid.
                    bool boolQuantityCondition = attr == null ? true : false;
                    boolValidArrCond = Tools.boolValidStrCondition(conjson.strCondition, boolQuantityCondition) ?
                        true : false;

                    if (
                        //                                  //Only if the attribute's condition is valid we can
                        //                                  //      continue.
                        boolValidArrCond
                        )
                    {
                        if (
                            //                              //We have an attribute instance
                            attr != null
                            )
                        {
                            //                              //Verify attributes's value.
                            boolValidArrCond = Tools.boolValidStrCondtionValue(conjson.strValue, attr) ? true : false;
                        }
                        else
                        {
                            boolValidArrCond = Tools.boolValidStrQuantityCondition(conjson.strValue) ? true : false;
                        }
                    }
                }

                intI++;
            }


            return boolValidArrCond;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static bool boolValidStrQuantityCondition(
            //                                              //Verify if condtion string is valid

            String strQuantityCondition_I
            )
        {
            bool boolValidArrCond = false;

            //                                              //To easy code.
            String strQuantityCondition = strQuantityCondition_I.TrimExcel();

            if (
                //                                          //There is a value and we can get a number from it.
                strQuantityCondition.Length > 0 && strQuantityCondition.IsParsableToInt()
                )
            {
                int intQuantity = strQuantityCondition.ParseToInt();
                boolValidArrCond = intQuantity >= 0 ? true : false;
            }

            return boolValidArrCond;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static bool boolValidStrCondition(
            //                                              //Verify if condtion string is valid

            String strCondition_I,
            bool boolQuantityCondition_I
            )
        {
            bool boolValidArrCond = false;

            if (
                //                                          //Valid when is a quantity condition.
                (boolQuantityCondition_I && 
                (
                strCondition_I == Tools.strLessThanOperator ||
                strCondition_I == Tools.strMoreThanOperator ||
                strCondition_I == Tools.strEqualsOperator ||
                strCondition_I == Tools.strDistinctOperator ||
                strCondition_I == Tools.strMoreAndEqualThanOperator ||
                strCondition_I == Tools.strLessAndEqualThanOperator
                ))
                ||
                //                                          //Valid when is an attribute condition.
                (!boolQuantityCondition_I && 
                (
                strCondition_I == Tools.strEqualsOperator ||
                strCondition_I == Tools.strDistinctOperator
                ))                 
                )
            {
                boolValidArrCond = true;
            }

            return boolValidArrCond;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static bool boolValidStrCondtionValue(
            //                                              //Verify if a condtionValue is valid

            String boolValidStrCondtionValue_I,
            AttrAttribute attr_I
            )
        {
            bool boolValidStrCondtionValue = false;

            List<ValjsonValueJson> darrvaljson = attr_I.darrvaljsonGetValuesFromWisnet();
            if (
                (darrvaljson != null)
                )
            {
                int intI = 0;
                /*UNTIL-DO*/
                while (!(
                    (intI >= darrvaljson.Count()) ||
                    boolValidStrCondtionValue
                    ))
                {
                    if (
                        boolValidStrCondtionValue_I == darrvaljson[intI].strValue
                        )
                    {
                        boolValidStrCondtionValue = true;
                    }

                    intI = intI + 1;
                }
            }

            return boolValidStrCondtionValue;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static bool boolValidConditionGroups(
            //                                              //Verify if a groups of conditions are valid.

            GpcondjsonGroupConditionJson[] arrgpcondjson_I
            )
        {
            bool boolValidConditionGroups = true;

            int intI = 0;
            /*UNTIL-DO*/
            while (
                //                                          //There still elements to evaluate.
                intI < arrgpcondjson_I.Length &&
                //              
                boolValidConditionGroups
                )
            {
                //                                          //Get the group to evaluate.
                GpcondjsonGroupConditionJson gpcondjson = arrgpcondjson_I[intI];

                //                                          //Recursive Method to evaluate a group.
                boolValidConditionGroups = Tools.boolValidConditionList(gpcondjson) ? true : false;

                intI = intI + 1;
            }

            return boolValidConditionGroups;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String strGetLeftElement(
            String strCondition_I
            )
        {
            int intOpenParentheses = 0;
            int intCloseParentheses = 0;
            int intI = 0;

            do
            {
                char charA = strCondition_I[intI];
                /*CASE*/
                if (
                    charA == Tools.charOpenParetheses
                    )
                {
                    intOpenParentheses = intOpenParentheses + 1;
                }
                else if (
                    charA == Tools.charCloseParentheses
                    )
                {
                    intCloseParentheses = intCloseParentheses + 1;
                }
                /*END-CASE*/

                intI = intI + 1;
            }
            /*REPEAT-UNTIL*/
            while (!(
                //                                          //The string was 
                (intI >= strCondition_I.Length) ||
                (intOpenParentheses == intCloseParentheses)
                ));

            String strLeftElement = strCondition_I.Substring(1, intI - 2);
            return strLeftElement;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String strGetRightElement(
            String strCondition_I
            )
        {
            int intOpenParentheses = 0;
            int intCloseParentheses = 0;
            int intI = 0;

            do
            {
                char charA = strCondition_I[intI];
                /*CASE*/
                if (
                    charA == Tools.charOpenParetheses
                    )
                {
                    intOpenParentheses = intOpenParentheses + 1;
                }
                else if (
                    charA == Tools.charCloseParentheses
                    )
                {
                    intCloseParentheses = intCloseParentheses + 1;
                }
                /*END-CASE*/

                intI = intI + 1;
            }
            /*REPEAT-UNTIL*/
            while (!(
                //                                          //The string was 
                (intI >= strCondition_I.Length) ||
                (intOpenParentheses == intCloseParentheses)
                ));

            String strRightElement = strCondition_I.Substring(intI);
            strRightElement = strRightElement.Substring(strRightElement.IndexOf(Tools.charOpenParetheses) + 1);
            strRightElement = strRightElement.Substring(0, strRightElement.Length - 1);
            return strRightElement;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static GpcondjsonGroupConditionJson gpcondjsonGetCondition(
            //                                              //Get one complete condition as an object. If the condition
            //                                              //      is not found returns null.

            int? intnPkCalculation_I,
            int? intnPkLinkNode_I,
            int? intnPkIoentity_I,
            int? intnPkTransformCalculation_I
            )
        {
            GpcondjsonGroupConditionJson gpcondjson = null;

            Odyssey2Context context = new Odyssey2Context();

            ColcondentityCalculationOrLinkConditionEntityDB colcondentity = context.CalculationOrLinkCondition.FirstOrDefault(
                colcond => colcond.intnPkCalculation == intnPkCalculation_I && colcond.intnPkLinkNode == intnPkLinkNode_I &&
                colcond.intnPkInputOrOutput == intnPkIoentity_I &&
                colcond.intnPkTransformCalculation == intnPkTransformCalculation_I);

            if (
                colcondentity != null
                )
            {
                gpcondjson = Tools.gpcondjsonGetRecursive(colcondentity.intPkGroupCondition);
            }
            
            return gpcondjson;
        }

        //--------------------------------------------------------------------------------------------------------------
        private static GpcondjsonGroupConditionJson gpcondjsonGetRecursive(
            //                                              //Get one complete condition as an object. If the group is
            //                                              //      not found returns null.

            int intPkGroupCondition_I
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            GpcondentityGroupConditionEntityDB gpcondentity = context.GroupCondition.FirstOrDefault(gpcond =>
            gpcond.intPk == intPkGroupCondition_I);

            GpcondjsonGroupConditionJson gpcondjson = null;
            if (
                //                                          //Group found.
                gpcondentity != null
                )
            {
                //                                          //Get all single conditions.
                List<CondentityConditionEntityDB> darrcondentity = context.Condition.Where(cond =>
                cond.intPkGroupCondition == intPkGroupCondition_I).ToList();

                List<CondjsonConditionJson> darrcondjson = new List<CondjsonConditionJson>();
                foreach(CondentityConditionEntityDB condentity in darrcondentity)
                {
                    CondjsonConditionJson condjson = new CondjsonConditionJson(condentity.intnPkAttribute, 
                        condentity.strCondition, condentity.strValue);
                    darrcondjson.Add(condjson);
                }

                //                                          //Get all groups.
                List<GpcondentityGroupConditionEntityDB> darrgpcondentity = context.GroupCondition.Where(cond =>
                cond.intnPkGroupCondition == intPkGroupCondition_I).ToList();

                List<GpcondjsonGroupConditionJson> darrgpcondjson = new List<GpcondjsonGroupConditionJson>();
                foreach (GpcondentityGroupConditionEntityDB gpcondentity1 in darrgpcondentity)
                {
                    GpcondjsonGroupConditionJson gpcondjson1 = Tools.gpcondjsonGetRecursive(gpcondentity1.intPk);
                    darrgpcondjson.Add(gpcondjson1);
                }

                gpcondjson = new GpcondjsonGroupConditionJson(gpcondentity.strOperator, darrcondjson.ToArray(),
                    darrgpcondjson.ToArray());
            }

            return gpcondjson;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool boolCalculationOrLinkApplies(
            //                                              //Search if the calculation or link has condition. If it has
            //                                              //      not then returns true, if it has then verify the 
            //                                              //      values of the job to determine if it applies or not.

            int? intnPkCalculation_I,
            int? intnPkLinkNode_I,
            int? intnPkIoentity_I,
            int? intnPkTransformCalculation_I,
            //                                              //Values of the job.
            JobjsonJobJson jobjson_I
            )
        {
            bool boolApplies = false;
//>>>>>>> 58cac905f788543d7a88d447235b5bf409f619c4

            Odyssey2Context context = new Odyssey2Context();

            //                                              //Find the condition.
            ColcondentityCalculationOrLinkConditionEntityDB colcondentity = context.CalculationOrLinkCondition.
                FirstOrDefault(colcond => colcond.intnPkCalculation == intnPkCalculation_I &&
                colcond.intnPkLinkNode == intnPkLinkNode_I && colcond.intnPkInputOrOutput == intnPkIoentity_I &&
                colcond.intnPkTransformCalculation == intnPkTransformCalculation_I);

            if (
                //                                          //It has not condition.
                colcondentity == null
                )
            {
                boolApplies = true;
            }
            else
            {
                //                                          //Verify the attributes and values.
                GpcondjsonGroupConditionJson gpcondjson = Tools.gpcondjsonGetRecursive(
                    colcondentity.intPkGroupCondition);

                boolApplies = Tools.boolGroupConditionAppliesRecursive(gpcondjson, jobjson_I);
            }

            return boolApplies;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static bool boolGroupConditionAppliesRecursive(
            //                                              //Verify if a group condition applies. 
            //                                              //First it virifies all the single conditions. if all of
            //                                              //      them apply then it verifies each group.

            GpcondjsonGroupConditionJson gpcondjson_I,
            JobjsonJobJson jobjson_I
            )
        {
            //                                              //To easy code.
            bool boolAndOperator = gpcondjson_I.strOperator == Tools.strAndOperator;
            bool boolApplies = boolAndOperator ? true : false;

            if (
                //                                          //It´s an AND operator.
                boolAndOperator && boolApplies
                )
            {
                //                                          //Evaluation of the single conditions.
                int intA = 0;
                /*LOOP*/
                while (true)
                {
                    /*EXIT-IF*/
                    if (
                        intA >= gpcondjson_I.arrcond.Length ||
                        !boolApplies
                        )
                    {
                        break;
                    }
                    bool boolConditionApplies = Tools.boolSingleconditionApplies(gpcondjson_I.arrcond[intA],
                        jobjson_I);
                    boolApplies = boolApplies && boolConditionApplies;
                    intA++;
                }
                /*END-LOOP*/

            }
            else if (
                //                                          //It´s an OR operator.
                !boolAndOperator && !boolApplies
                )
            {
                //                                          //Evaluation of the single conditions.
                int intA = 0;
                /*LOOP*/
                while (true)
                {
                    /*EXIT-IF*/
                    if (
                        intA >= gpcondjson_I.arrcond.Length ||
                        boolApplies
                        )
                    {
                        break;
                    }
                    bool boolConditionApplies = Tools.boolSingleconditionApplies(gpcondjson_I.arrcond[intA],
                        jobjson_I);
                    boolApplies = boolApplies || boolConditionApplies;
                    intA++;
                }
                /*END-LOOP*/
            }                

            if (
                //                                          //When is an AND operator.
                boolAndOperator &&
                //                                          //All single conditions must apply.
                boolApplies
                )
            {
                //                                          //Evaluation of the group conditions.
                int intB = 0;
                /*LOOP*/
                while (true)
                {
                    /*EXIT-IF*/
                    if (
                        intB >= gpcondjson_I.arrgpcond.Length ||
                        !boolApplies
                        )
                    {
                        break;
                    }
                    bool boolGroupApplies = Tools.boolGroupConditionAppliesRecursive(gpcondjson_I.arrgpcond[intB],
                        jobjson_I);
                    boolApplies = boolApplies && boolGroupApplies;
                    intB++;
                }
                /*END-LOOP*/
            }
            else if (
                //                                          //When is an OR operator.
                !boolAndOperator &&
                //                                          //If not single condition apply.
                !boolApplies
                )
            {
                //                                          //Evaluation of the group conditions.
                int intB = 0;
                /*LOOP*/
                while (true)
                {
                    /*EXIT-IF*/
                    if (
                        intB >= gpcondjson_I.arrgpcond.Length ||
                        boolApplies
                        )
                    {
                        break;
                    }
                    bool boolGroupApplies = Tools.boolGroupConditionAppliesRecursive(gpcondjson_I.arrgpcond[intB],
                        jobjson_I);
                    boolApplies = (boolApplies || boolGroupApplies);
                        
                    intB++;
                }
                /*END-LOOP*/
            }

            return boolApplies;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static bool boolSingleconditionApplies(
            //                                              //Verify if a condition applies for a job.

            CondjsonConditionJson condjson_I,
            JobjsonJobJson jobjson_I
            )
        {
            bool boolApplies = true;

            if (
                //                                          //It is for quantity.
                condjson_I.intnPkAttribute == null
                )
            {
                /*CASE*/
                if (
                    condjson_I.strCondition == Tools.strEqualsOperator
                    )
                {
                    boolApplies = jobjson_I.intnQuantity == condjson_I.strValue.ParseToInt();
                }
                else if (
                    condjson_I.strCondition == Tools.strDistinctOperator
                    )
                {
                    boolApplies = jobjson_I.intnQuantity != condjson_I.strValue.ParseToInt();
                }
                else if (
                    condjson_I.strCondition == Tools.strMoreThanOperator 
                    //condjson_I.strCondition == Tools.strMoreAndEqualThanOperator
                    )
                {
                    boolApplies = jobjson_I.intnQuantity > condjson_I.strValue.ParseToInt();
                }
                else if(
                    condjson_I.strCondition == Tools.strLessThanOperator 
                    //condjson_I.strCondition == Tools.strLessAndEqualThanOperator
                    )
                {
                    boolApplies = jobjson_I.intnQuantity < condjson_I.strValue.ParseToInt();
                }
                else if (
                    condjson_I.strCondition == Tools.strMoreAndEqualThanOperator
                    )
                {
                    boolApplies = jobjson_I.intnQuantity >= condjson_I.strValue.ParseToInt();
                }
                else if (
                    condjson_I.strCondition == Tools.strLessAndEqualThanOperator
                    )
                {
                    boolApplies = jobjson_I.intnQuantity <= condjson_I.strValue.ParseToInt();
                }
                /*END-CASE*/
            }
            else
            {
                //                                          //To easy code.
                bool boolEquals = condjson_I.strCondition == Tools.strEqualsOperator;
                AttrAttribute attrDB = AttrAttribute.attrFromDB((int)condjson_I.intnPkAttribute);
                AttrjsonAttributeJson attrjson = jobjson_I.darrattrjson.FirstOrDefault(attr =>
                attr.strAttributeName == attrDB.strCustomName);

                boolApplies = boolEquals ? (attrjson.strValue == condjson_I.strValue) : 
                    (attrjson.strValue != condjson_I.strValue);
            }
            
            return boolApplies;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
