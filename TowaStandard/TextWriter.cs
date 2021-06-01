/*TASK TextWriter*/
using System;
using System.IO;

//                                                          //AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: 18-Febrero-2014.

namespace TowaStandard
{
    //==================================================================================================================
    public static class TextWriterX
    {
        //--------------------------------------------------------------------------------------------------------------
        public static StreamWriter New(
            //                                              //Genera el StreamWriter para un archivo de texto.

            //                                              //systextwriter, StreamWriter ready.

            //                                              //FileInfo del archivo.
            FileInfo TextFile_M
            )
        {
            if (
                TextFile_M == null
                )
                Test.Abort(Test.ToLog(TextFile_M, "TextFile_M") + " can not be null");

            //                                              //Confirma la existencia de algo en el path y que se le
            //                                              //      pueda reescribir
            PathX syspathTextFile = TextFile_M.GetPath();
            if (
                syspathTextFile.IsDirectory
                )
                Test.Abort(
                    Test.ToLog(syspathTextFile.FullPath, "syspathTextFile.FullPath") + " can not be a directory",
                    Test.ToLog(TextFile_M, "TextFile_M"),
                    Test.ToLog(syspathTextFile, "syspathTextFile"));

            if (
                TextFile_M.Exists() && TextFile_M.IsReadOnly
                )
                Test.Abort(Test.ToLog(syspathTextFile.FullPath, "syspathTextFile.FullPath") + " is ReadOnly",
                    Test.ToLog(TextFile_M, "TextFile_M"),  Test.ToLog(syspathTextFile, "syspathTextFile"));

            //                                              //Creo el acceso al archivo.
            StreamWriter systextwriterNew;
            try
            {
                //                                          //Creo el stream reader.
                systextwriterNew = TextFile_M.CreateText();
                //NewTextWriterForRewrite = new StreamWriter(TextFile_M.FullName, false, Encoding.UTF8);
            }
            catch (Exception sysexcepError)
            {
                Test.Abort(
                    Test.ToLog(sysexcepError, "sysexcepError") + " error in \"TextFile_M.CreateText()\"",
                    Test.ToLog(syspathTextFile, "syspathTextFile"));

                systextwriterNew = null;
            }

            return systextwriterNew;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void WriteLine(
            //                                              //Escribe una línea de texto.
            //                                              //Solo son válidos los caracteres USEFUL_IN_TEXT y ESCAPE.

            //                                              //StreamWriter del archivo.
            this StreamWriter TextWriter_M,
            //                                              //Línea que se va a escribir.
            String Line_I
            )
        {
            if (
                Line_I == null
                )
                Test.Abort(Test.ToLog(Line_I, "Line_I") + " can not be null");

            if (
                TextWriter_M == null
                )
                Test.Abort(Test.ToLog(TextWriter_M, "TextWriter_M") + " can not be null");

            //                                              //Verifico que sean válidos todos los caracteres
            for (int intC = 0; intC < Line_I.Length; intC = intC + 1)
            {
                char charToVerify = Line_I[intC];

                if (!(
                    charToVerify.IsInSortedSet(Std.CHARS_USEFUL_IN_TEXT) ||
                    charToVerify.IsInSortedSet(Std.arrt2charESCAPE)
                    ))
                    Test.Abort(Test.ToLog(Line_I[intC], "Line_I[" + intC + "]") + " is a nonvalid character");
            }

            //                                              //Escribe una línea en el archivo.
            try
            {
                //                                          //Escribo una línea.
                TextWriter_M.WriteLine(Line_I);
            }
            catch (Exception sysexcepError)
            {
                Test.Abort(
                    Test.ToLog(sysexcepError, "sysexcepError") + " error in \"TextWriter_M.WriteLine(strLine_I)\"",
                    Test.ToLog(TextWriter_M, "TextWriter_M"), Test.ToLog(Line_I, "Line_I"));
            }
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
