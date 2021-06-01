/*TASK Directory*/
using System;
using System.IO;

//                                                          //AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: 18-Febrero-2014.

namespace TowaStandard
{
    //==================================================================================================================
    public static class DirectoryX
    {
        //--------------------------------------------------------------------------------------------------------------
        public static DirectoryInfo New(
            //                                              //Crea un DirectoryInfo, el directorio puede no existir.

            //                                              //sysdir, DirectoryInfo creado.

            //                                              //Path (completo y válido) del directorio a crear.
            PathX DirectoryPath_I
            )
        {
            //                                              //Creo el DirectoryInfo.
            DirectoryInfo sysdirNew;
            try
            {
                sysdirNew = new DirectoryInfo(DirectoryPath_I.FullPath);
            }
            catch (Exception sysexcepError)
            {
                Test.Abort(
                    Test.ToLog(sysexcepError, "sysexcepError") +
                        " error in \"new DirectoryInfo(syspathDirectory_I.FullPath)\"",
                    Test.ToLog(DirectoryPath_I, "DirectoryPath_I"));

                sysdirNew = null;
            }

            //                                              //Regresa el DirectoryInfo creado.
            return sysdirNew;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static DirectoryInfo GetCurrent(
            //                                              //Localiza el directorio sobre el que se encuentra
            //                                              //      posicionada la aplicación.

            //                                              //sysdir, Current Directory.
            )
        {
            //                                              //Busco el current directory.
            String strCurrentDirectory;
            try
            {
                strCurrentDirectory = Directory.GetCurrentDirectory();
            }
            catch (Exception sysexcepError)
            {
                Test.Abort(
                    Test.ToLog(sysexcepError, "sysexcepError") + " error in \"Directory.GetCurrentDirectory()\"");

                strCurrentDirectory = null;
            }

            //                                              //Regresa un DirectoryInfo.
            PathX syspathCurrentDirectory = new PathX(strCurrentDirectory);
            return DirectoryX.New(syspathCurrentDirectory);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String FullPath(
            //                                              //Extrae el fullpath correspondiente al directorio.
            //                                              //(Glg,9.Oct,2018) There is a PROBLEM with
            //                                              //      {diretory}.FullName some time return an \ at the
            //                                              //      end, some time no.

            //                                              //str, fullpath.

            //                                              //Directory del cual se quiere información.
            this DirectoryInfo Directory_I
            )
        {
            //                                              //FullName: .....\LastDirectoryName[\]
            String strFullPath = Directory_I.FullName;

            int intLengthMinus1 = strFullPath.Length - 1;
            if (
                strFullPath[intLengthMinus1] == PathX.DIRECTORY_SEPARATOR_CHAR
                )
            {
                strFullPath = strFullPath.Substring(0, intLengthMinus1);
            }

            return strFullPath;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static PathX GetPath(
            //                                              //Extrae el syspath correspondiente al directorio.

            //                                              //syspath, similar al que se uso para crear el sysdir con su
            //                                              //      estado actualizado.

            //                                              //DirectoryInfo del cual se quiere información.
            this DirectoryInfo Directory_I
            )
        {
            //                                              //Regresa el path con su estado actualizado.
            return new PathX(Directory_I.FullPath());
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool Exists(
            //                                              //Finds out if directory exists.

            //                                              //bool, true if exists.

            //                                              //Directory del cual se quiere información.
            this DirectoryInfo Directory_I
            )
        {
            try
            {
                Directory_I.Refresh();
            }
            catch (Exception sysexcepError)
            {
                Test.Abort(Test.ToLog(sysexcepError, "sysexcepError") + " error in \"Directory_I.Refresh();\"",
                    Test.ToLog(Directory_I, "Directory_I"));
            }

            return Directory_I.Exists;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static DateTime GetCreationTime(
            //                                              //Refresh a get created time.

            //                                              //utc, created time (updated).

            this DirectoryInfo Directory_M
            )
        {
            try
            {
                Directory_M.Refresh();
            }
            catch (Exception sysexcepError)
            {
                Test.Abort(Test.ToLog(sysexcepError, "sysexcepError") + " error in \"Directory_I.Refresh();\"",
                    Test.ToLog(Directory_M, "Directory_M"));
            }

            return Directory_M.CreationTime;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static DateTime GetLastAccessTime(
            //                                              //Refresh a get last access time.

            //                                              //utc, created time (updated).

            this DirectoryInfo Directory_M
            )
        {
            try
            {
                Directory_M.Refresh();
            }
            catch (Exception sysexcepError)
            {
                Test.Abort(Test.ToLog(sysexcepError, "sysexcepError") + " error in \"Directory_I.Refresh();\"",
                    Test.ToLog(Directory_M, "Directory_M"));
            }

            return Directory_M.LastAccessTime;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static DateTime GetLastWriteTime(
            //                                              //Refresh a get last write time.

            //                                              //utc, created time (updated).

            this DirectoryInfo Directory_M
            )
        {
            try
            {
                Directory_M.Refresh();
            }
            catch (Exception sysexcepError)
            {
                Test.Abort(Test.ToLog(sysexcepError, "sysexcepError") + " error in \"Directory_I.Refresh();\"",
                    Test.ToLog(Directory_M, "Directory_M"));
            }

            return Directory_M.LastWriteTime;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static DirectoryInfo[] GetDirectories(
            //                                              //Extrae el conjunto de subdirectorios de un directorio.

            //                                              //DirectoryInfo del cual se quiere información.
            this DirectoryInfo Directory_I
            )
        {
            if (
                Directory_I == null
                )
                Test.Abort(Test.ToLog(Directory_I, "Directory_I") + " can not be null");

            //                                              //Crea el syspath del directorio, esto solo para confirmar
            //                                              //      que todo sigue bien y tener un mejor diagnóstico en
            //                                              //      caso de problemas.
            PathX syspathDirectory = Directory_I.GetPath();

            if (
                !syspathDirectory.IsDirectory
                )
                Test.Abort(
                    Test.ToLog(syspathDirectory.FullPath, "syspathDirectory.FullPath") + " is not a directory",
                    Test.ToLog(Directory_I, "Directory_I"), Test.ToLog(syspathDirectory, "syspathDirectory"));

            //                                              //Extrae subdirectorios.
            DirectoryInfo[] arrsysdirGetDirectories;
            try
            {
                arrsysdirGetDirectories = Directory_I.GetDirectories();
            }
            catch (Exception sysexcepError)
            {
                Test.Abort(
                    Test.ToLog(sysexcepError, "sysexcepError") + " error in \"sysdirToSearch_I.GetDirectories()\"",
                    Test.ToLog(Directory_I, "Directory_I"));

                arrsysdirGetDirectories = null;
            }

            //                                              //Regresa el conjunto de subdirectorios.
            return arrsysdirGetDirectories;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static FileInfo[] GetFiles(
            //                                              //Extrae el conjunto de archivos de un directorio.

            //                                              //DirectoryInfo del cual se quiere información.
            DirectoryInfo Directory_I
            )
        {
            if (
                Directory_I == null
                )
                Test.Abort(Test.ToLog(Directory_I, "Directory_I") + " can not be null");

            //                                              //Crea el syspath del directorio, esto solo para confirmar
            //                                              //      que todo sigue bien y tener un mejor diagnóstico en
            //                                              //      caso de problemas.
            PathX syspathDirectory = Directory_I.GetPath();

            if (
                !syspathDirectory.IsDirectory
                )
                Test.Abort(
                    Test.ToLog(syspathDirectory.FullPath, "syspathDirectory.FullPath") +
                        " is not a directory",
                    Test.ToLog(Directory_I, "Directory_I"),
                    Test.ToLog(syspathDirectory, "syspathDirectory"));

            //                                              //Extrae Archivos.
            FileInfo[] arrsysfileGetFiles;
            try
            {
                arrsysfileGetFiles = Directory_I.GetFiles();
            }
            catch (Exception sysexcepError)
            {
                Test.Abort(
                    Test.ToLog(sysexcepError, "sysexcepError") + " error in \"sysdirToSearch_I.GetFiles()\"",
                    Test.ToLog(Directory_I, "Directory_I"));

                arrsysfileGetFiles = null;
            }

            //                                              //Regresa el conjunto de archivos.
            return arrsysfileGetFiles;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void SetCurrent(
            //                                              //Establece el Current Durectory.

            //                                              //DirectroyInfo sobre el que se desea posicionar.
            this DirectoryInfo DirectoryToSet_I
            )
        {
            //                                              //Crea el syspath del directorio, esto solo para confirmar
            //                                              //      que todo sigue bien y tener un mejor diagnóstico en
            //                                              //      caso de problemas.
            PathX syspathToSet = DirectoryToSet_I.GetPath();

            if (
                !syspathToSet.IsDirectory
                )
                Test.Abort(Test.ToLog(syspathToSet, "syspathToSet") + " do not exist as directory");

            //                                              //Establece el Current Directory a partir de un path.
            try
            {
                Directory.SetCurrentDirectory(syspathToSet.FullPath);
            }
            catch (Exception sysexcepError)
            {
                Test.Abort(
                    Test.ToLog(sysexcepError, "sysexcepError") +
                        " error in \"Directory.SetCurrentDirectory(syspathToSet.FullPath)\"",
                    Test.ToLog(syspathToSet.FullPath, "syspathToSet.FullPath"),
                    Test.ToLog(syspathToSet, "syspathToSet"));
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void CreateOnDisk(
            //                                              //A partir de un DirectoryInfo CREA el directorio en disco.

            //                                              //DirectroyInfo con el cual se desea crear directorio en
            //                                              //      disco.
            this DirectoryInfo Directory_M
            )
        {
            //                                              //Necesito un syspath para verificar la existencia ya sea
            //                                              //      como directorio o archivo.
            PathX syspathDirectory = Directory_M.GetPath();

            if (
                //                                          //Ya existe como directorio o como archivo.
                syspathDirectory.Exists
                )
                Test.Abort(Test.ToLog(syspathDirectory, "syspathDirectory") +
                    " can not create a directory, already exist as a directory o as a file");

            //                                              //Crea el directorio en disco.
            try
            {
                Directory_M.Create();
            }
            catch (Exception sysexcepError)
            {
                Test.Abort(Test.ToLog(sysexcepError, "sysexcepError") + " error in \"Directory_I.Create();\"",
                    Test.ToLog(Directory_M, "sysdirToCreateOnDisk_M"));
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void RenameOnDisk(
            //                                              //Modifica el nombre de un directorio (último
            //                                              //      subdirectorio), usará MoveTo, pero para que solo
            //                                              //      cambie el nombre del último subdirectorio.

            //                                              //DirectoryInfo del directorio que se quiere renombrar.
            this DirectoryInfo Directory_M,
            //                                              //Nuevo nombre del subdirectorio (sin el path).
            String strNewName_I
            )
        {
            if (
                !PathX.IsPathNameValid(strNewName_I)
                )
                Test.Abort(Test.ToLog(strNewName_I, "strNewName_I") + " is not valid");

            //                                              //Crea el syspath del directorio, esto solo para confirmar
            //                                              //      que todo sigue bien y tener un mejor diagnóstico en
            //                                              //      caso de problemas.
            PathX syspathDirectory = Directory_M.GetPath();

            if (
                !syspathDirectory.IsDirectory
                )
                Test.Abort(Test.ToLog(syspathDirectory, "syspathDirectory") + " do not exist as directory");

            if (
                //                                          //El nuevo nombre es el mismo.
                strNewName_I == Directory_M.Name
                )
                Test.Abort(Test.ToLog(syspathDirectory, "syspathDirectory") + " & " +
                    Test.ToLog(strNewName_I, "strNewSubdirectoryName_I") + " both are the same name, can not rename");

            //                                              //Crea el nuevo path para confirmar que su forma es válida.
            PathX syspathDirectoryRanamed = syspathDirectory.GetDirectoryPath().AddName(strNewName_I);

            if (
                //                                          //Ya existe un archivo o directorio con el mismo nombre.
                syspathDirectoryRanamed.Exists
                )
                Test.Abort(Test.ToLog(syspathDirectoryRanamed, "syspathRanamed") +
                    " can not rename, already exists as directory o file");

            //                                              //Renombra el subdirectorio usando el MoveTo.
            //                                              //Nótese que ya se hicieron muchas verificaciones para hacer
            //                                              //      esto en forma segura.
            try
            {
                //                                          //Nótese que se mueve SOBRE el directorio renombrado (NO 
                //                                          //      queda abajo como un subdirectorio).
                Directory_M.MoveTo(syspathDirectoryRanamed.FullPath);
            }
            catch (Exception sysexcepError)
            {
                Test.Abort(
                    Test.ToLog(sysexcepError, "sysexcepError") +
                        " error in \"Directory_M.MoveTo(syspathDirectoryRanamed.FullPath);\"",
                    Test.ToLog(Directory_M, "Directory_M"),
                    Test.ToLog(syspathDirectoryRanamed, "syspathDirectoryRanamed"));
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void DeleteFromDisk(
            //                                              //Elimina un directorio del disco.

            //                                              //DirectoryInfo que se desea eliminar.
            this DirectoryInfo Directory_M,
            //                                              //Si es true, también borra su contenido, si es false, solo
            //                                              //      puede borrar si está vacío, en su defecto aborta.
            bool DeleteSubdirectoriesAndFiles_I
            )
        {
            if (
                Directory_M == null
                )
                Test.Abort(Test.ToLog(Directory_M, "Directory_M") + " can not be null");

            //                                              //Crea el syspath del directorio, esto solo para confirmar
            //                                              //      que todo sigue bien y tener un mejor diagnóstico en
            //                                              //      caso de problemas.
            PathX syspathDirectory = Directory_M.GetPath();

            if (
                !syspathDirectory.IsDirectory
                )
                Test.Abort(
                    Test.ToLog(syspathDirectory.FullPath, "syspathDirectory.FullPath") + " is not a directory",
                    Test.ToLog(Directory_M, "Directory_M"), Test.ToLog(syspathDirectory, "syspathDirectory"));

            //                                              //Hace el delete.
            try
            {
                Directory_M.Delete(DeleteSubdirectoriesAndFiles_I);
            }
            catch (Exception sysexcepError)
            {
                Test.Abort(
                    Test.ToLog(sysexcepError, "sysexcepError") +
                        " error in \"sysdirToDelete_M.Delete(DeleteSubdirectoriesAndFiles_I);\"",
                    Test.ToLog(Directory_M, "Directory_M"),
                    Test.ToLog(DeleteSubdirectoriesAndFiles_I, "DeleteSubdirectoriesAndFiles_I"));
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void MoveOnDisk(
            //                                              //Mueve un directorio a ser parte de otro directorio, no
            //                                              //      no debe existir en el nuevo padre.
            //                                              //Este move solo puede ser al mismo dispositivo.

            //                                              //DirectoryInfo del directorio que se quiere mover, lo
            //                                              //      actualiza a la nueva ubicación.
            this DirectoryInfo Directory_M,
            //                                              //DirectoryInfo del directorio al cual se desea mover el
            //                                              //      directorio anterior, este será el nuevo padre.
            DirectoryInfo DirectoryNewParent_M
            )
        {
            if (
                Directory_M == null
                )
                Test.Abort(Test.ToLog(Directory_M, "Directory_M") + " can not be null");

            //                                              //Verifica que ambos directorios existan.
            PathX syspathDirectory = Directory_M.GetPath();
            if (
                //                                          //No es un directorio.
                !syspathDirectory.IsDirectory
                )
                Test.Abort(
                    Test.ToLog(syspathDirectory.FullPath, "syspathDirectory.FullPath") + " is not a directory",
                    Test.ToLog(Directory_M, "Directory_M"), Test.ToLog(syspathDirectory, "syspathDirectory"));

            if (
                DirectoryNewParent_M == null
                )
                Test.Abort(Test.ToLog(DirectoryNewParent_M, "DirectoryNewParent_M") + " can not be null");

            PathX syspathDirectoryNewParent = DirectoryNewParent_M.GetPath();
            if (
                //                                          //No es un directorio.
                !syspathDirectoryNewParent.IsDirectory
                )
                Test.Abort(
                    Test.ToLog(syspathDirectoryNewParent.FullPath, "syspathDirectoryNewParent.FullPath") +
                        " is not a directory",
                    Test.ToLog(DirectoryNewParent_M, "DirectoryNewParent_M"),
                    Test.ToLog(syspathDirectoryNewParent, "syspathDirectoryNewParent"));

            if (
                //                                          //Están en raices distintas.
                syspathDirectory.Root != syspathDirectoryNewParent.Root
                )
                Test.Abort(
                    Test.ToLog(syspathDirectory.Name, "syspathDirectory.Name") + " can not move to " +
                        Test.ToLog(syspathDirectoryNewParent.FullPath, "syspathDirectoryNewParent.FullPath") +
                        " they are not in the same root",
                    Test.ToLog(Directory_M, "Directory_M"), Test.ToLog(syspathDirectory, "syspathDirectory"),
                    Test.ToLog(DirectoryNewParent_M, "DirectoryNewParent_M"),
                    Test.ToLog(syspathDirectoryNewParent, "syspathDirectoryNewParent"));

            //                                              //Forma el syspath del directorio ya movido.
            PathX syspathDirectoryMoved = syspathDirectoryNewParent.AddName(syspathDirectory.Name);

            //                                              //Aborta si existe otro directorio o archivo con el mismo
            //                                              //      nombre en el el disco.
            if (
                //                                          //El nuevo syspath, ya existe.
                syspathDirectoryMoved.Exists
                )
                Test.Abort(
                    Test.ToLog(syspathDirectory.Name, "syspathToMove.Name") + " can not move to " +
                        Test.ToLog(syspathDirectoryNewParent.FullPath, "syspathDirectoryNewParent.FullPath") + ", " +
                        Test.ToLog(syspathDirectoryMoved.FullPath, "syspathDirectoryMoved.FullPath") +
                        " already exists (it is a directory or file with same name)",
                    Test.ToLog(syspathDirectoryMoved, "syspathDirectoryMoved"),
                    Test.ToLog(Directory_M, "Directory_M"),
                    Test.ToLog(syspathDirectory, "syspathDirectory"),
                    Test.ToLog(DirectoryNewParent_M, "DirectoryNewParent_M"),
                    Test.ToLog(syspathDirectoryNewParent, "syspathDirectoryNewParent"));

            //                                              //Mueve el directorio al nuevo directorio ("YaMOvido").
            try
            {
                //                                          //Nótese que se mueve abajo del NewParent y que conserva el
                //                                          //      nombre (última parte) que tenía.
                Directory_M.MoveTo(syspathDirectoryMoved.FullPath);
            }
            catch (Exception sysexcepError)
            {
                Test.Abort(
                    Test.ToLog(sysexcepError, "sysexcepError") +
                        " error in \"Directory_M.MoveTo(syspathDirectoryMoved.FullPath);\"",
                    Test.ToLog(Directory_M, "Directory_M"), Test.ToLog(syspathDirectoryMoved, "syspathDirectoryMoved"));
            }

            //                                              //Actualiza a la nueva ubicación.
            Directory_M = DirectoryX.New(syspathDirectoryMoved);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void CopyOnDisk(
            //                                              //Copia el directorio (con todo su contenido) a otro
            //                                              //      directorio, se copia con el mismo nombre.
            //                                              //No debe existir el nombre en el nuevo padre.

            //                                              //DirectoryInfo del directorio que se quiere copiar.
            this DirectoryInfo Directory_I,
            //                                              //DirectoryInfo del directorio al cual se desea copiar, este
            //                                              //      será el padre que recibe el directorio
            DirectoryInfo DirectoryReceiving_M,
            //                                              //Regresa el DirectoryInfo del directorio nuevo (lo que 
            //                                              //      quedo ya copiado).
            out DirectoryInfo DirectoryCopied_O
            )
        {
            //                                              //Verifica que ambos directorios existan.
            PathX syspathDirectory = Directory_I.GetPath();
            if (
                //                                          //No es un directorio.
                !syspathDirectory.IsDirectory
                )
                Test.Abort(Test.ToLog(syspathDirectory, "syspathDirectory") +
                    " do not exist as directory");
            PathX syspathDirectoryReceiving = DirectoryReceiving_M.GetPath();
            if (
                //                                          //No es un directorio.
                !syspathDirectoryReceiving.IsDirectory
                )
                Test.Abort(Test.ToLog(syspathDirectoryReceiving, "syspathDirectoryReceiving") +
                    " do not exist as directory");

            //                                              //Verifica que el nuevo directorio no exista.
            PathX syspathDirectoryCopied = syspathDirectoryReceiving.AddName(syspathDirectory.Name);
            if (
                //                                          //El nuevo syspath, ya existe.
                syspathDirectoryCopied.Exists
                )
                Test.Abort(
                    Test.ToLog(syspathDirectoryCopied, "syspathDirectoryCopied") +
                        " can not copy, already exists a directory or file with same name",
                    Test.ToLog(syspathDirectoryCopied.Exists, "syspathDirectoryCopied.Exists"),
                    Test.ToLog(Directory_I, "Directory_I"), Test.ToLog(DirectoryReceiving_M, "DirectoryReceiving_M"));

            //                                              //Crea el nuevo sysdir y el directorio en el disco.
            DirectoryCopied_O = DirectoryX.New(syspathDirectoryCopied);
            DirectoryCopied_O.CreateOnDisk();

            //                                              //Copia todos los archivos que se encuentran en el nivel
            //                                              //      inmediato.
            FileInfo[] arrsysfileToCopy = Directory_I.GetFiles();
            foreach (FileInfo sysfileF in arrsysfileToCopy)
            {
                //                                          //Copia cada uno, obviamente no habra remplazos.
                FileInfo sysfile;
                sysfileF.CopyOnDisk(DirectoryCopied_O, out sysfile);
            }

            //                                              //Copia todos los subdirectorios que se encuentran en el
            //                                              //      nivel inmediato.
            DirectoryInfo[] arrsysdirToCopy = Directory_I.GetDirectories();
            foreach (DirectoryInfo sysdirD in arrsysdirToCopy)
            {
                //                                          //Copia cada uno de los subdirectorios (llamada recursiva).
                DirectoryInfo sysdir;
                sysdirD.CopyOnDisk(DirectoryCopied_O, out sysdir);
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void DeleteContentFromDisk(
            //                                              //De un directorio elimina los directories or files que
            //                                              //      inician con un prefijo.

            this DirectoryInfo Directory_I,
            String NamePrefix_I
            )
        {
            if (
                Directory_I == null
                )
                Test.Abort(Test.ToLog(Directory_I, "Directory_I") + " can not be null");
            if (
                !Directory_I.Exists
                )
                Test.Abort(Test.ToLog(Directory_I.FullPath(), "Directory_I.FullPath") + " do not exists",
                    Test.ToLog(Directory_I, "Directory_I"));

            if (
                NamePrefix_I == null
                )
                Test.Abort(Test.ToLog(NamePrefix_I, "NamePrefix_I") + " can not be null");

            FileInfo[] arrsysfile = Directory_I.GetFiles();
            foreach (FileInfo sysfile in arrsysfile)
            {
                if (
                    sysfile.Name.StartsWithOrdinal(NamePrefix_I)
                    )
                {
                    sysfile.DeleteFromDisk();
                }
            }

            DirectoryInfo[] arrsysdir = Directory_I.GetDirectories();
            foreach (DirectoryInfo sysdir in arrsysdir)
            {
                if (
                    sysdir.Name.StartsWithOrdinal(NamePrefix_I)
                    )
                {
                    sysdir.DeleteFromDisk(true);
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
