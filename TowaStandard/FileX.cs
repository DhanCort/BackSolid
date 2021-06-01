/*TASK File*/
using System;
using System.IO;

//                                                          //AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: 18-Febrero-2014.

namespace TowaStandard
{
    //==================================================================================================================
    public static class FileX
    {
        //--------------------------------------------------------------------------------------------------------------
        public static FileInfo New(
            //                                              //Crea un FileInfo, el archivo puede no existir.

            //                                              //sysfile, FileInfo creado.

            //                                              //Path (completo y válido) del archivo a crear.
            PathX FilePath_L
            )
        {
            Test.AbortIfNull(FilePath_L, "FilePath_L");

            //                                              //Creo el FileInfo.
            FileInfo sysfileNew;
            try
            {
                sysfileNew = new FileInfo(FilePath_L.FullPath);
            }
            catch (Exception sysexcepError)
            {
                Test.Abort(
                    Test.ToLog(sysexcepError, "sysexcepError") +
                        " error in \"new FileInfo(syspathFile_I.FullPath)\"", Test.ToLog(FilePath_L, "FilePath_L"));

                sysfileNew = null;
            }

            return sysfileNew;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static PathX GetPath(
            //                                              //Extrae el syspath correspondiente al archivo.

            //                                              //syspath, similar al que se uso para crear el sysfile con
            //                                              //      su estado actualizado.

            //                                              //FileInfo del cual se quiere información.
            this FileInfo File_L
            )
        {
            Test.AbortIfNull(File_L, "File_L");

            //                                              //Regresa el syspath con su estado actualizado.
            return new PathX(File_L.FullName);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool Exists(
            //                                              //Find out if file exists.

            //                                              //bool, true if exists.

            this FileInfo File_M
            )
        {
            Test.AbortIfNull(File_M, "File_M");

            //                                              //Need to refresh.
            try
            {
                File_M.Refresh();
            }
            catch (Exception sysexcepError)
            {
                Test.Abort(Test.ToLog(sysexcepError, "sysexcepError") + " error in \"File_L.Refresh();\"",
                        Test.ToLog(File_M, "File_M"));
            }

            return File_M.Exists;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static long GetLength(
            //                                              //Extrae DEL DISCO la longitud del archivo.

            //                                              //long, longitud del archivo (la toma del DISCO).

            //                                              //FileInfo del cual se quiere información.
            this FileInfo File_M
            )
        {
            Test.AbortIfNull(File_M, "File_M");

            //                                              //Need to refresh.
            try
            {
                File_M.Refresh();
            }
            catch (Exception sysexcepError)
            {
                Test.Abort(Test.ToLog(sysexcepError, "sysexcepError") + " error in \"File_L.Refresh();\"",
                        Test.ToLog(File_M, "File_M"));
            }

            return File_M.Length;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool IsReadOnly(

            //                                              //bool, true if is read only.

            this FileInfo File_M
            )
        {
            Test.AbortIfNull(File_M, "File_M");

            //                                              //Need to refresh.
            try
            {
                File_M.Refresh();
            }
            catch (Exception sysexcepError)
            {
                Test.Abort(Test.ToLog(sysexcepError, "sysexcepError") + " error in \"File_L.Refresh();\"",
                        Test.ToLog(File_M, "File_M"));
            }

            return File_M.IsReadOnly;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static DateTime GetCreationTime(
            //                                              //dt, creation time. (refreshed)

            this FileInfo File_M
            )
        {
            Test.AbortIfNull(File_M, "File_M");

            //                                              //Need to refresh.
            try
            {
                File_M.Refresh();
            }
            catch (Exception sysexcepError)
            {
                Test.Abort(Test.ToLog(sysexcepError, "sysexcepError") + " error in \"File_L.Refresh();\"",
                        Test.ToLog(File_M, "File_M"));
            }

            return File_M.CreationTime;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static DateTime GetLastAccessTime(
            //                                              //dt, creation time. (refreshed)

            this FileInfo File_M
            )
        {
            Test.AbortIfNull(File_M, "File_M");

            //                                              //Need to refresh.
            try
            {
                File_M.Refresh();
            }
            catch (Exception sysexcepError)
            {
                Test.Abort(Test.ToLog(sysexcepError, "sysexcepError") + " error in \"File_L.Refresh();\"",
                        Test.ToLog(File_M, "File_M"));
            }

            return File_M.LastAccessTime;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String[] ReadAll(
             //                                              //Carga la totalidad de un archivo de texto a memoria.

             //                                              //arrstr, archivo de texto en formato de arreglo de Strings.

             //                                              //FileInfo del archivo a cargar a memoría.
             this FileInfo TextFile_M
             )
        {
            if (
                TextFile_M == null
                )
                Test.Abort(Test.ToLog(TextFile_M, "TextFile_M") + " can not be null");

            //                                              //Crea el syspath del directorio, esto solo para confirmar
            //                                              //      que todo sigue bien y tener un mejor diagnóstico en
            //                                              //      caso de problemas.
            PathX syspathTextFile = TextFile_M.GetPath();

            if (
                !syspathTextFile.IsFile
                )
                Test.Abort(Test.ToLog(syspathTextFile.FullPath, "syspathTextFile.FullPath") + " is not a file",
                    Test.ToLog(TextFile_M, "TextFile_M"), Test.ToLog(syspathTextFile, "syspathTextFile"));

            StreamReader systextreader = TextReaderX.New(TextFile_M);

            //                                              //Paso el archivo a memoria (un String).
            String strTextFile;
            try
            {
                //                                          //Lee TODO un archivo.
                strTextFile = systextreader.ReadToEnd();
            }
            catch (Exception sysexcepError)
            {
                Test.Abort(Test.ToLog(sysexcepError, "sysexcepError") + " error in \"systextreader.ReadToEnd()\"",
                    Test.ToLog(systextreader, "systextreader"));

                strTextFile = null;

                systextreader.Dispose();
            }

            //                                              //Es necesaro cerrar el systextreader
            systextreader.Dispose();

            //                                              //Elimina las líneas finales que no tengan información (solo
            //                                              //      serían el String "")
            /*CASE*/
            if (
                strTextFile.EndsWith(Environment.NewLine, StringComparison.Ordinal)
                )
            {
                /*WHILE-DO*/
                while (
                    strTextFile.EndsWith(Environment.NewLine, StringComparison.Ordinal)
                    )
                {
                    strTextFile = strTextFile.Substring(0, strTextFile.Length - Environment.NewLine.Length);
                }
            }
            else if (
                    strTextFile.EndsWith("\n", StringComparison.Ordinal)
                )
            {
                /*WHILE-DO*/
                while (
                    strTextFile.EndsWith("\n", StringComparison.Ordinal)
                    )
                {
                    strTextFile = strTextFile.Substring(0, strTextFile.Length - "\n".Length);
                }
            }
            else
            {
                //                                          //No hay lineas finales sin información
            }
            /*END-CASE*/

            //                                              //Formo arreglo con lo leído.
            String[] arrstrLine;
            arrstrLine = strTextFile.Split(Environment.NewLine, "\n");

            return arrstrLine;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void RenameOnDisk(
            //                                              //Modifica el nombre de un archivo, usará MoveTo, pero
            //                                              //      que solo cambie el nombre.

            //                                              //FileInfo del archivo que se quiere renombrar.
            this FileInfo File_M,
            //                                              //Nuevo nombre del archivo (sin el path).
            String strNewName_I
            )
        {
            if (
                !PathX.IsPathNameValid(strNewName_I)
                )
                Test.Abort(Test.ToLog(strNewName_I, "strNewName_I") + " is not valid");

            //                                              //Crea el syspath del archivo, esto solo para confirmar
            //                                              //      que todo sigue bien y tener un mejor diagnóstico en
            //                                              //      caso de problemas.
            PathX syspathFile = File_M.GetPath();

            if (
                !syspathFile.IsFile
                )
                Test.Abort(Test.ToLog(syspathFile, "syspathFile") + " do not exist as file");

            if (
                //                                          //El nuevo nombre es el mismo.
                strNewName_I == File_M.Name
                )
                Test.Abort(Test.ToLog(syspathFile, "syspathFile") + " & " +
                    Test.ToLog(strNewName_I, "strNewName_I") + " are both the same name, can not rename");

            //                                              //Crea el nuevo path para confirmar que su forma es válida.
            PathX syspathFileRanamed = syspathFile.GetDirectoryPath().AddName(strNewName_I);

            if (
                //                                          //Ya existe un archivo o directorio con el mismo nombre.
                syspathFileRanamed.Exists
                )
                Test.Abort(
                    Test.ToLog(syspathFileRanamed, "syspathFileRanamed") +
                        " can not rename, already exist a directory or a file with same name",
                    Test.ToLog(syspathFileRanamed.Exists, "syspathFileRanamed.boolExists"));

            //                                              //Renombre el archivo usando el MoveTo.
            //                                              //Nótese que ya se hicieron muchas verificaciones para hacer
            //                                              //      esto en forma segura.
            try
            {
                File_M.MoveTo(syspathFileRanamed.FullPath);
            }
            catch (Exception sysexcepError)
            {
                Test.Abort(
                    Test.ToLog(sysexcepError, "sysexcepError") +
                        " error in \"File_M.MoveTo(syspathFileRanamed.FullPath);\"",
                    Test.ToLog(File_M, "File_M"), Test.ToLog(syspathFileRanamed, "syspathFileRanamed"));
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void DeleteFromDisk(
            //                                              //Elimina un archivo del disco.

            //                                              //FileInfo que se desea eliminar.
            this FileInfo File_I
            )
        {
            if (
                File_I == null
                )
                Test.Abort(Test.ToLog(File_I, "File_I") + " can not be null");

            //                                              //Crea el syspath del directorio, esto solo para confirmar
            //                                              //      que todo sigue bien y tener un mejor diagnóstico en
            //                                              //      caso de problemas.
            PathX syspathFile = File_I.GetPath();

            if (
                !syspathFile.IsFile
                )
                Test.Abort(Test.ToLog(syspathFile.FullPath, "syspathFile.FullPath") + " is not a file",
                    Test.ToLog(File_I, "File_I"), Test.ToLog(syspathFile, "syspathFile"));

            //                                              //Hace el delete.
            try
            {
                File_I.Delete();
            }
            catch (Exception sysexcepError)
            {
                Test.Abort(
                    Test.ToLog(sysexcepError, "sysexcepError") + " error in \"File_I.Delete();\"",
                    Test.ToLog(File_I, "File_I"));
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void MoveToOnDisk(
            //                                              //Mueve el archivo a otro directorio, se mueve con el mismo
            //                                              //      nombre.

            //                                              //FileInfo del archivo que se quiere mover.
            this FileInfo File_M,
            //                                              //DirectoryInfo del directorio al cual se desea mover el
            //                                              //      archivo, este será el nuevo padre.
            DirectoryInfo DirectoryReceiving_M
            )
        {
            if (
                File_M == null
                )
                Test.Abort(Test.ToLog(File_M, "File_M") + " can not be null");

            //                                              //Verifica que el archivo y el directorio existan.
            PathX syspathFile = File_M.GetPath();
            if (
                //                                          //No es un archivo.
                !syspathFile.IsFile
                )
                Test.Abort(Test.ToLog(syspathFile.FullPath, "syspathFile.FullPath") + " is not a file",
                    Test.ToLog(File_M, "File_M"), Test.ToLog(syspathFile, "syspathFile"));

            if (
                DirectoryReceiving_M == null
                )
                Test.Abort(Test.ToLog(DirectoryReceiving_M, "DirectoryReceiving_M") + " can not be null");

            PathX syspathDirectoryReceiving = DirectoryReceiving_M.GetPath();
            if (
                //                                          //No es un directorio.
                !syspathDirectoryReceiving.IsDirectory
                )
                Test.Abort(
                    Test.ToLog(syspathDirectoryReceiving.FullPath, "syspathDirectoryReceiving.FullPath") +
                        " is not a directory",
                    Test.ToLog(DirectoryReceiving_M, "DirectoryReceiving_M"),
                    Test.ToLog(syspathDirectoryReceiving, "syspathDirectoryReceiving"));

            if (
                //                                          //Están en raices distintas.
                syspathFile.Root != syspathDirectoryReceiving.Root
                )
                Test.Abort(
                    Test.ToLog(syspathFile.FullPath, "syspathFile.FullPath") + " can not move to " +
                        Test.ToLog(syspathDirectoryReceiving.FullPath, "syspathDirectoryReceiving.FullPath") +
                        " they are not in the same root",
                    Test.ToLog(File_M, "File_M"), Test.ToLog(syspathFile, "syspathFile"),
                    Test.ToLog(DirectoryReceiving_M, "DirectoryReceiving_M"),
                    Test.ToLog(syspathDirectoryReceiving, "syspathDirectoryReceiving"));

            //                                              //Forma el syspath del directorio ya movido.
            PathX syspathFileMoved = syspathDirectoryReceiving.AddName(syspathFile.Name);

            //                                              //Aborta si existe otro directorio o archivo con el mismo
            //                                              //      nombre en el el disco.
            if (
                //                                          //El nuevo syspath, ya existe.
                syspathFileMoved.Exists
                )
                Test.Abort(
                    Test.ToLog(syspathFile.Name, "syspathFile.Name") + " can not move to " +
                        Test.ToLog(syspathDirectoryReceiving.FullPath, "syspathDirectoryReceiving.FullPath") + ", " +
                        Test.ToLog(syspathFileMoved.FullPath, "syspathFileMoved.FullPath") +
                        " already exists (it is a directory or file with same name)",
                    Test.ToLog(syspathFileMoved, "syspathFileMoved"), Test.ToLog(File_M, "File_M"),
                    Test.ToLog(syspathFile, "syspathFile"), Test.ToLog(DirectoryReceiving_M, "DirectoryReceiving_M"),
                    Test.ToLog(syspathDirectoryReceiving, "syspathDirectoryReceiving"));

            //                                              //Mueve el archivo de directorio usando el MoveTo.
            //                                              //Nótese que ya se hicieron muchas verificaciones para hacer
            //                                              //      esto en forma segura.
            try
            {
                File_M.MoveTo(syspathFileMoved.FullPath);
            }
            catch (Exception sysexcepError)
            {
                Test.Abort(
                    Test.ToLog(sysexcepError, "sysexcepError") +
                        " error in \"File_M.MoveTo(syspathFileMoved.FullPath);\"",
                    Test.ToLog(File_M, "File_M"), Test.ToLog(syspathFileMoved, "syspathFileMoved"));
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void CopyOnDisk(
            //                                              //Copia un archivo a otro directorio donde ya existe y se
            //                                              //      debe reescribir.
            //                                              //No se permite reescribir el archivo si el receptor es
            //                                              //      ReadOnly.

            //                                              //FileInfo del archivo que se quiere copiar.
            this FileInfo File_I,
            //                                              //DirectoryInfo del directorio al cual se desea copiar el
            //                                              //      archivo, este será el padre que recibe el file.
            DirectoryInfo DirectoryReceiving_M,
            //                                              //FileInfo del archivo donde se va a regresar.
            out FileInfo FileRewrited_O
            )
        {
            Test.AbortIfNull(File_I, "File_I");
            PathX syspathFile = File_I.GetPath();
            if (
                //                                          //No es un archivo.
                !syspathFile.IsFile
                )
                Test.Abort(Test.ToLog(syspathFile.FullPath, "syspathFile.FullPath") + " file do not exist",
                    Test.ToLog(File_I, "File_I"), Test.ToLog(syspathFile, "syspathFile"));

            Test.AbortIfNull(DirectoryReceiving_M, "DirectoryReceiving_M");
            PathX syspathDirectoryReceiving = DirectoryReceiving_M.GetPath();
            if (
                //                                          //No es un directorio.
                !syspathDirectoryReceiving.IsDirectory
                )
                Test.Abort(
                    Test.ToLog(syspathDirectoryReceiving.FullPath, "syspathDirectoryReceiving.FullPath") +
                        " directory do not exist",
                    Test.ToLog(DirectoryReceiving_M, "DirectoryReceiving_M"),
                    Test.ToLog(syspathDirectoryReceiving, "syspathDirectoryReceiving"), Test.ToLog(File_I, "File_I"),
                    Test.ToLog(syspathFile, "syspathFile"));

            //                                              //Verifica que el archivo receptor, si existe se pueda
            //                                              //      reescribir.
            PathX syspathFileToRewrite = syspathDirectoryReceiving.AddName(syspathFile.Name);
            if (
                syspathFileToRewrite.IsDirectory
                )
                Test.Abort(
                    Test.ToLog(syspathFileToRewrite.FullPath, "syspathFileToRewrite.FullPath") + " is a DIRECTORY",
                    Test.ToLog(syspathFileToRewrite, "syspathFileToRewrite"),
                    Test.ToLog(DirectoryReceiving_M, "DirectoryReceiving_M"),
                    Test.ToLog(syspathDirectoryReceiving, "syspathDirectoryReceiving"), Test.ToLog(File_I, "File_I"),
                    Test.ToLog(syspathFile, "syspathFile"));

            FileInfo sysfileToWrite = FileX.New(syspathFileToRewrite);

            try
            {
                File_I.CopyTo(syspathFileToRewrite.FullPath, true);
            }
            catch (Exception sysexcepError)
            {
                Test.Abort(
                    Test.ToLog(sysexcepError, "sysexcepError") +
                        " error in \"File_I.CopyTo(syspathFileToRewrite.FullPath, true);\"",
                    Test.ToLog(File_I, "File_I"), Test.ToLog(syspathFileToRewrite, "syspathFileToRewrite"));
            }

            //                                              //Regresa el nuevo FileInfo.
            sysfileToWrite.Refresh();
            FileRewrited_O = sysfileToWrite;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void SetReadOnly(
            //                                              //Set ReadOnly del archivo a true.

            //                                              //FileInfo del archivo que se quiere modificar.
            this FileInfo File_M
            )
        {
            //                                              //Crea el syspath del archivo, esto solo para confirmar
            //                                              //      que todo sigue bien y tener un mejor diagnóstico en
            //                                              //      caso de problemas.
            PathX syspathFile = File_M.GetPath();

            if (
                !syspathFile.IsFile
                )
                Test.Abort(Test.ToLog(syspathFile, "syspathFile") + " it is not a file");

            //                                              //Modifica propiedad.
            try
            {
                File_M.IsReadOnly = true;
            }
            catch (Exception sysexcepError)
            {
                Test.Abort(Test.ToLog(sysexcepError, "sysexcepError") + " error in \"File_M.IsReadOnly = true;\"",
                    Test.ToLog(File_M, "sysfileToUpdate_M"));
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void ResetReadOnly(
            //                                              //Permite que se puede escribir en el archivo.

            //                                              //FileInfo del archivo que se quiere modificar.
            this FileInfo File_M
            )
        {
            //                                              //Crea el syspath del archivo, esto solo para confirmar
            //                                              //      que todo sigue bien y tener un mejor diagnóstico en
            //                                              //      caso de problemas.
            PathX syspathFile = File_M.GetPath();

            if (
                !syspathFile.IsFile
                )
                Test.Abort(Test.ToLog(syspathFile, "syspathFile") + " it is not a file");

            //                                              //Modifica propiedad.
            try
            {
                File_M.IsReadOnly = false;
            }
            catch (Exception sysexcepError)
            {
                Test.Abort(Test.ToLog(sysexcepError, "sysexcepError") + " error in \"File_M.IsReadOnly = false;\"",
                    Test.ToLog(File_M, "File_M"));
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void WriteAll(
            //                                              //Sube la totalidad de un arreglo en memoria a un archivo
            //                                              //      de texto que puede o no ya existir.

            //                                              //FileInfo del archivo a al cual se sube lo que se tiene en
            //                                              //      memoría.
            this FileInfo TextFile_M,
            //                                              //arrstr, archivo de texto en formato de arreglo de Strings.
            String[] SetOfLines_I
            )
        {
            if (
                TextFile_M == null
                )
                Test.Abort(Test.ToLog(TextFile_M, "TextFile_M") + " can not be null");

            if (
                SetOfLines_I == null
                )
                Test.Abort(Test.ToLog(SetOfLines_I, "SetOfLines_I") + " can not be null");

            //                                              //Tomo el path para analizarlo y poder dar un mejor
            //                                              //      diagnostico.
            PathX syspathTextFile = TextFile_M.GetPath();
            if (
                syspathTextFile.IsDirectory
                )
                Test.Abort(Test.ToLog(syspathTextFile.FullPath, "syspathTextFile.FullPath") + " can not be a directory",
                    Test.ToLog(TextFile_M, "TextFile_M"), Test.ToLog(syspathTextFile, "syspathTextFile"));

            StreamWriter systextwriter = TextWriterX.New(TextFile_M);

            //                                              //Paso todo el arreglo a un solo String de líneas.
            String strTextFile = String.Join(Environment.NewLine, SetOfLines_I);

            //                                              //Escribo el String al archivo (un solo WriteLine).
            systextwriter.WriteLine(strTextFile);

            systextwriter.Dispose();
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
