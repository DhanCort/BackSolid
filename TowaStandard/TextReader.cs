/*TASK TextReader*/
using System;
using System.IO;
using System.Text;

//                                                          //AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: 18-Febrero-2014.

namespace TowaStandard
{
    //==================================================================================================================
    public static class TextReaderX
    {
        //--------------------------------------------------------------------------------------------------------------
        public static StreamReader New(
            //                                              //Genera el StreamReader para un archivo de texto, si no
            //                                              //      existe abortará.

            //                                              //systextreader, StreamReader ready.

            //                                              //FileInfo del archivo.
            FileInfo TextFile_M
            )
        {
            if (
                TextFile_M == null
                )
                Test.Abort(Test.ToLog(TextFile_M, "TextFile_M") + " can not be null");

            //                                              //Confirma la existencia el archivo.
            PathX syspathTextFile = TextFile_M.GetPath();
            if (
                //                                          //No existe como archivo.
                !syspathTextFile.IsFile
                )
                Test.Abort(Test.ToLog(syspathTextFile.FullPath, "syspathTextFile.FullPath") + " file do not exist",
                    Test.ToLog(TextFile_M, "TextFile_M"), Test.ToLog(syspathTextFile, "syspathTextFile"));

            //                                              //Creo el acceso al archivo.
            StreamReader systextreaderNewTextReader;
            try
            {
                //                                          //Creo el stream reader.
                //                                          //El encoding se va a determinar DESPUÉS del primer read.
                systextreaderNewTextReader = new StreamReader(TextFile_M.FullName, /*Encoding.Default*/Encoding.UTF8);
            }
            catch (Exception sysexcepError)
            {
                Test.Abort(
                    Test.ToLog(sysexcepError, "sysexcepError") +
                        " error in \"new StreamReader(encoding.FullName, Encoding.UTF8)\"",
                    Test.ToLog(TextFile_M, "TextFile_M"));

                systextreaderNewTextReader = null;
            }

            return systextreaderNewTextReader;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String ReadLine(
            //                                              //Leer una línea de texto.

            //                                              //str, Línea leída.

            //                                              //StreamReader del archivo.
            this StreamReader TextReader_M
            )
        {
            if (
                TextReader_M == null
                )
                Test.Abort(Test.ToLog(TextReader_M, "TextReader_M") + " can not be null");

            //                                              //Leo una línea del archivo.
            String strReadLine;
            try
            {
                //                                          //Leo una línea.
                strReadLine = TextReader_M.ReadLine();
            }
            catch (Exception sysexcepError)
            {
                Test.Abort(
                    Test.ToLog(sysexcepError, "sysexcepError") + " error in \"TextReader_M.ReadLine()\"",
                    Test.ToLog(TextReader_M, "TextReader_M"));

                strReadLine = null;

                TextReader_M.Dispose();
            }

            return strReadLine;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
