/*TASK Path*/
using System;
using System.IO;

//                                                          //AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: 19-Febrero-2014.

namespace TowaStandard
{
    //==================================================================================================================
    public class PathX : BsysAbstract, IComparable
    {
        //                                                  //Clase para manipular path.
        //                                                  //Debe funcionar correctamente para archivos y directorios
        //                                                  //      locales y en la red.

        //                                                  //Nótese que estos objetos (syspath) solo son una referencia
        //                                                  //      a un archivo o directorio que están en disco (podría
        //                                                  //      ser una referencia que ni siquiera esta en el
        //                                                  //      disco).
        //                                                  //La información que recolecta solo es válida en el momento
        //                                                  //      de la construcción del objeto (Ej. en el momento de
        //                                                  //      la construcción podría ser una referencia a un
        //                                                  //      archivo que existe, sin embargo unos momento después
        //                                                  //      "alguién" borra del disco el archivo y el contenido
        //                                                  //      de este objeto syspath ya no es preciso).

        //--------------------------------------------------------------------------------------------------------------
        /*CONSTANTS*/

        //                                                  //En teoria el conjunto de caracteres válidos en los path
        //                                                  //      names es muy extenso, sin embargo, en la realidad,
        //                                                  //      cuando se desea mover archivos y directorios entre
        //                                                  //      diferentes sistemas operativos (windows, unix, ios
        //                                                  //      de mac, etc.) suelen suceder problemas.
        //                                                  //Por estándar Towa optamos por permitir un conjunto MUY
        //                                                  //      CONSERVADOR DE CARACTERES.
        //                                                  //CONFORME ENTENDAMOS MEJOR ESTA PROBLEMATICA, ESTA LISTA
        //                                                  //      DE CARACTERES SERÁ AMPLIADA O RECORTADA.
        private const String strCHAR_IN_PATH_NAME =
            //                                              //Dígitos y letras sin acentos.
            "0123456789" + "ABCDEFGHIJKLMNOPQRSTUVWXYZ" + "abcdefghijklmnopqrstuvwxyz" +
            //                                              //Espacio.
            " " +
            //                                              //GLG 27Feb2017, opte por incluir como posibilidad acentos
            //                                              //      en español y las Ñ ñ.
            "ÁÉÍÓÚÜ" + "áéíóúü" + "Ññ" +
            //                                              //Algunos caracteres especiales.
            ",._()[]{}$@+-" + "%&#*";

        //                                                  //Información anterior en un arreglo ordenado.
        private static readonly char[] arrcharIN_PATH_NAME;

        private const String strCHAR_IN_NETWORK_NAME =
            //                                              //Dígitos y letras sin acentos.
            //                                              //Estos caracteres deben ser un subconjunto de
            //                                              //      strCHAR_IN_PATH_NAME.
            "0123456789" + "ABCDEFGHIJKLMNOPQRSTUVWXYZ" + "abcdefghijklmnopqrstuvwxyz";

        private static readonly char[] arrcharIN_NETWORK_NAME;

        //                                                  //Podra ser : (windows), ¿ (IOS) o ??.
        //                                                  //(GLG.19Mar2018) EN REALIDAD AÚN NO ENTIENDO COMO ES ESTO
        //                                                  //      IOS O UNIX.
        public static readonly char VOLUME_SEPARATOR_CHAR = Path.VolumeSeparatorChar;

        //                                                  //Podra ser \\ (windows), // (IOS) o ??.
        //                                                  //(GLG.19Mar2018) EN REALIDAD AÚN NO ENTIENDO COMO ES ESTO
        //                                                  //      IOS O UNIX.
        public static readonly String NETWORK_MARK = "" + Path.DirectorySeparatorChar + Path.DirectorySeparatorChar;

        //                                                  //Podra ser \ (windows), / (IOS) o ??
        public static readonly char DIRECTORY_SEPARATOR_CHAR = Path.DirectorySeparatorChar;

        //                                                  //String to be used as "neutral" volume mark.
        public const char VOLUME_SEPARATOR_CHAR_MASKED = ':';

        //                                                  //String to be used as "neutral" network mark.
        public const String NETWORK_MARK_MASKED = "■■";

        //                                                  //Character to be used as "neutral" directory separator.
        public const char DIRECTORY_SEPARATOR_CHAR_MASKED = '▸';

        //                                                  //Requerido para varificar que los chars del full path sean
        //                                                  //      al menos válidos.
        private static readonly char[] arrcharIN_FULLPATH;

        //--------------------------------------------------------------------------------------------------------------
        /*INITIALIZER*/

        //--------------------------------------------------------------------------------------------------------------
        static PathX(
            //                                              //Prepara las constantes.
            //                                              //1. Prepara, verifica y ordena CHAR_IN_PATH_NAME.
            //                                              //2. Prepara, verifica y ordena CHAR_IN_SERVER_NAME.
            //                                              //3. Verifica charVOLUME_MARK, strNETWORK_MARK y
            //                                              //      charDIRECTORY_SEPARATOR.
            //                                              //4. Ordenar posibles caracteres en fullpath
            )
        {
            //                                              //1. Prepara, verifica y ordena CHAR_IN_PATH_NAME.
            PathX.arrcharIN_PATH_NAME = PathX.strCHAR_IN_PATH_NAME.ToCharArray();
            Std.Sort(PathX.arrcharIN_PATH_NAME);

            Test.AbortIfDuplicate(PathX.arrcharIN_PATH_NAME, "PathX.arrcharIN_PATH_NAME");
            Test.AbortIfOneOrMoreItemsAreNotInSortedSet(PathX.arrcharIN_PATH_NAME,
                "Path.arrcharIN_PATH_NAME", Std.CHARS_USEFUL_IN_TEXT, "Std.CHARS_USEFUL_IN_TEXT");

            //                                              //2. Prepara, verifica y ordena CHAR_IN_SERVER_NAME.
            PathX.arrcharIN_NETWORK_NAME = PathX.strCHAR_IN_NETWORK_NAME.ToCharArray();
            Std.Sort(PathX.arrcharIN_NETWORK_NAME);

            Test.AbortIfDuplicate(PathX.arrcharIN_NETWORK_NAME, "Path.arrcharIN_SERVER_NAME");

            //                                              //3. Verifica charVOLUME_MARK, strNETWORK_MARK y
            //                                              //      charDIRECTORY_SEPARATOR
            Test.AbortIfItemIsInSortedSet(PathX.VOLUME_SEPARATOR_CHAR, "Path.VOLUME_SEPARATOR_CHAR",
                PathX.arrcharIN_PATH_NAME, "arrcharIN_PATH_NAME");
            Test.AbortIfOneOrMoreCharactersAreInSortedSet(PathX.NETWORK_MARK, "Path.NETWORK_MARK",
                PathX.arrcharIN_PATH_NAME, "arrcharIN_PATH_NAME");
            Test.AbortIfItemIsInSortedSet(PathX.DIRECTORY_SEPARATOR_CHAR, "Path.DIRECTORY_SEPARATOR_CHAR",
                PathX.arrcharIN_PATH_NAME, "arrcharIN_PATH_NAME");
            Test.AbortIfItemIsInSortedSet(PathX.VOLUME_SEPARATOR_CHAR_MASKED, "Path.VOLUME_SEPARATOR_CHAR_MASKED",
                PathX.arrcharIN_PATH_NAME, "arrcharIN_PATH_NAME");
            Test.AbortIfOneOrMoreCharactersAreInSortedSet(PathX.NETWORK_MARK_MASKED, "Path.NETWORK_MARK_MASKED",
                PathX.arrcharIN_PATH_NAME, "arrcharIN_PATH_NAME");
            Test.AbortIfItemIsInSortedSet(PathX.DIRECTORY_SEPARATOR_CHAR_MASKED, "Path.DIRECTORY_SEPARATOR_CHAR_MASKED",
                PathX.arrcharIN_PATH_NAME, "arrcharIN_PATH_NAME");

            //                                              //4. Ordenar posibles caracteres en fullpath
            String strCHAR_IN_FULLPATH = PathX.strCHAR_IN_PATH_NAME + PathX.NETWORK_MARK +
                PathX.VOLUME_SEPARATOR_CHAR + PathX.DIRECTORY_SEPARATOR_CHAR;
            PathX.arrcharIN_FULLPATH = strCHAR_IN_FULLPATH.ToCharArray();
            Std.Sort(arrcharIN_FULLPATH);
        }

        //--------------------------------------------------------------------------------------------------------------
        /*STATIC METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public static String MaskFullPath(
            //                                              //Transform an OS specific full path to masked full path.
            //                                              //From: \\Abc\Xyz To: ■■Abc▸Xyz.
            //                                              //str, full path mask

            String FullPath_I
            )
        {
            if (
                FullPath_I == null
                )
                Test.Abort(Test.ToLog(FullPath_I, "FullPath_I") + " can not be null");

            String strFullPathOsToStd;

            String strRestOfOsFullPath;
            /*CASE*/
            if (
                //                                          //Form c:____
                (FullPath_I.Length >= 2) && (FullPath_I[1] == PathX.VOLUME_SEPARATOR_CHAR)
                )
            {
                strFullPathOsToStd = "" + FullPath_I[0] + PathX.VOLUME_SEPARATOR_CHAR_MASKED;

                strRestOfOsFullPath = FullPath_I.Substring(2);
            }
            else if (
                FullPath_I.StartsWith(PathX.NETWORK_MARK, StringComparison.Ordinal)
                )
            {
                strFullPathOsToStd = PathX.NETWORK_MARK_MASKED;

                strRestOfOsFullPath = FullPath_I.Substring(PathX.NETWORK_MARK.Length);
            }
            else
            {
                //                                          //No se tiene volumen y esta en red.
                //                                          //Esto sucede al menos en Mac IOS.
                strFullPathOsToStd = "";

                strRestOfOsFullPath = FullPath_I;
            }
            /*END-CASE*/

            return strFullPathOsToStd +
                strRestOfOsFullPath.Replace(PathX.DIRECTORY_SEPARATOR_CHAR, PathX.DIRECTORY_SEPARATOR_CHAR_MASKED);
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void subAbortIfRootAndFullPathIsInvalid(
            //                                              //Aborta si Standard Root and FullPath es invalido.
            //                                              //Se podría usar el método para verificar que TODO sea
            //                                              //      válido, sin embargo el diagnóstico sería muy pobre.
            //                                              //Se opto por diagnosticar en cada una de las partes que
            //                                              //      componen el Standard Root and FullPath.

            String strRootAndFullPathToVerify_I,
            String strRoot_I
            )
        {
            if (
                strRootAndFullPathToVerify_I == null
                )
                Test.Abort(Test.ToLog(strRootAndFullPathToVerify_I, "strRootAndFullPathToVerify_I") +
                    " can not be null");
            if (
                strRootAndFullPathToVerify_I.Length == 0
                )
                Test.Abort(Test.ToLog(strRootAndFullPathToVerify_I, "strRootAndFullPathToVerify_I") +
                    " has no characters");
            if (
                strRoot_I == null
                )
                Test.Abort(Test.ToLog(strRoot_I, "strRoot_I") + " can not be null");
            if (
                strRoot_I.Length == 0
                )
                Test.Abort(Test.ToLog(strRoot_I, "strRoot_I") + " has no characters");


            /*CASE*/
            if (
                //                                          //Tiene volumen (Ej c:____)
                (strRootAndFullPathToVerify_I.Length >= 2) &&
                (strRootAndFullPathToVerify_I[1] == PathX.VOLUME_SEPARATOR_CHAR)
                )
            {
                PathX.subAbortIfVolumeAndFullPathIsInvalid(strRootAndFullPathToVerify_I);
            }
            else if (
                //                                          //Es server
                strRootAndFullPathToVerify_I.StartsWith(PathX.NETWORK_MARK, StringComparison.Ordinal)
                )
            {
                PathX.subAbortIfNetworkAndFullPathIsInvalid(strRootAndFullPathToVerify_I, strRoot_I);
            }
            else
            {
                //                                          //No se tiene volumen ni esta en red.
                //                                          //Esto sucede al menos en Mac IOS.
                PathX.subAbortIfFullPathIsInvalid(strRootAndFullPathToVerify_I);
            }
            /*END-CASE*/
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subAbortIfVolumeAndFullPathIsInvalid(
            //                                              //Aborta si Volume and FullPath es invalido.

            String strVolumeAndFullPathToVerify_I
            )
        {
            String strVolume = strVolumeAndFullPathToVerify_I.Substring(0, 2);

            if (!(
                PathX.boolIsVolumeValid(strVolume)
                ))
                Test.Abort(Test.ToLog(strVolume, "strVolume") + " Volume is invalid",
                    Test.ToLog(strVolumeAndFullPathToVerify_I, "strVolumeAndFullPathToVerify_I"));

            if (
                //                                      //Es el caso especial (c:\)
                strVolumeAndFullPathToVerify_I.Length == 3
                )
            {
                if (!(
                    //                                  //Tiene la forma c:\
                    strVolumeAndFullPathToVerify_I[2] == PathX.DIRECTORY_SEPARATOR_CHAR
                    ))
                    Test.Abort(Test.ToLog(strVolumeAndFullPathToVerify_I, "strVolumeAndFullPathToVerify_I") +
                        " should have the form c:" + PathX.DIRECTORY_SEPARATOR_CHAR);
            }
            else
            {
                String strFullPath = strVolumeAndFullPathToVerify_I.Substring(2);

                PathX.subAbortIfFullPathIsInvalid(strFullPath);
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subAbortIfNetworkAndFullPathIsInvalid(
            //                                              //Aborta si Network and FullPath es invalido.

            String strNetworkAndFullPathToVerify_I,
            String strNetwork_I
            )
        {
            PathX.subAbortIfNetworkIsInvalid(strNetwork_I);

            String strFullPath = strNetworkAndFullPathToVerify_I.Substring(strNetwork_I.Length);

            //                                              //Si se tiene solo la raíz es valido
            if (
                strFullPath.Length > 0
                )
            {
                PathX.subAbortIfFullPathIsInvalid(strFullPath);
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subAbortIfNetworkIsInvalid(
            //                                              //Aborta si Network es invalido.

            //                                              //Ej. \\psf\home
            String strNetwork_I
            )
        {

            if (!(
                strNetwork_I.StartsWith(PathX.NETWORK_MARK, StringComparison.Ordinal)
                ))
                Test.Abort(Test.ToLog(strNetwork_I, "strNetwork_I") + " do not start with network mark " +
                        Test.ToLog(PathX.NETWORK_MARK, "Path.strNETWORK_MARK"));


            //                                          //Obtiene los nombres (ignora el primer separador)
            String str2Paths = strNetwork_I.Substring(PathX.NETWORK_MARK.Length);
            String[] arrstrPath = str2Paths.Split(PathX.DIRECTORY_SEPARATOR_CHAR);

            if (
                arrstrPath.Length != 2
                )
                Test.Abort(Test.ToLog(strNetwork_I, "strNetwork_I") + " should have 2 parts");

            Test.AbortIfOneOrMoreCharactersAreNotInSortedSet(arrstrPath[0], "arrstrPath[0]", PathX.arrcharIN_NETWORK_NAME,
                "arrcharIN_NETWORK_NAME");
            Test.AbortIfOneOrMoreCharactersAreNotInSortedSet(arrstrPath[1], "arrstrPath[1]", PathX.arrcharIN_NETWORK_NAME,
                "arrcharIN_NETWORK_NAME");
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subAbortIfNoVolumeNoNetworkAndFullPathIsInvalid(
            //                                              //Aborta si (no volumen, no network) and FullPath es
            //                                              //      invalido.

            String strNoVolumeNoNetworkAndFullPathToVerify_I
            )
        {
            if (
                //                                          //Es el caso especial (/)
                strNoVolumeNoNetworkAndFullPathToVerify_I.Length == 1
                )
            {
                if (!(
                    //                                      //Tiene la forma /
                    strNoVolumeNoNetworkAndFullPathToVerify_I[0] == PathX.DIRECTORY_SEPARATOR_CHAR
                    ))
                    Test.Abort(
                        Test.ToLog(strNoVolumeNoNetworkAndFullPathToVerify_I,
                            "strNoVolumeNoNetworkAndFullPathToVerify_I") +
                        " should have the form " + PathX.DIRECTORY_SEPARATOR_CHAR);
            }
            else
            {
                PathX.subAbortIfFullPathIsInvalid(strNoVolumeNoNetworkAndFullPathToVerify_I);
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subAbortIfFullPathIsInvalid(
            //                                              //Aborta si FullPath es invalido.

            String strFullPathToVerify_I
            )
        {
            //                                              //Verify first separator
            if (!(
                //                                          //Debe iniciar con un \
                (strFullPathToVerify_I.Length >= 1) &&
                (strFullPathToVerify_I[0] == PathX.DIRECTORY_SEPARATOR_CHAR)
                ))
                Test.Abort(Test.ToLog(strFullPathToVerify_I, "strFullPathToVerify_I") + " do not start with " +
                     Test.ToLog(PathX.DIRECTORY_SEPARATOR_CHAR, "Path.charDIRECTORY_SEPARATOR"));

            //                                              //Obtiene los nombres (ignora el primer separador)
            String strAllPathNames = strFullPathToVerify_I.Substring(1);

            //                                              //Puede no tener nombres, en cuyo caso no hay nada que
            //                                              //      verificar
            if ( //
                    strAllPathNames.Length > 0 //
                    )
            {
                String[] arrstrPathName = strAllPathNames.Split(PathX.DIRECTORY_SEPARATOR_CHAR);

                for (int intI = 0; intI < arrstrPathName.Length; intI = intI + 1)
                {
                    if (!(
                        PathX.IsPathNameValid(arrstrPathName[intI])
                        ))
                        Test.Abort(Test.ToLog(strFullPathToVerify_I, "strFullPathToVerify_I") + " is a not valid Name",
                            Test.ToLog(arrstrPathName[intI], "arrstrPathName[" + intI + "]"),
                            Test.ToLog(strAllPathNames, "strAllPathNames"));
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static bool boolIsVolumeValid(
            //                                              //Verifica que el volumen (c:) sea válido.
            //                                              //bool, true es válido.

            //                                              //c: (should be only 2 chars)
            String strVolume_I
            )
        {
            return (
                //                                          //Tiene la forma c:
                (strVolume_I.Length == 2) && Std.IsLetter(strVolume_I[0]) &&
                (strVolume_I[1] == PathX.VOLUME_SEPARATOR_CHAR)
                );
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool IsPathNameValid(
            //                                              //Verifica que el path name sea válido.
            //                                              //bool, true es válido.

            String PathNameToVerify_I
            )
        {
            bool boolIsPathNameValid = (
                PathNameToVerify_I.Length >= 1
                );

            if (
                boolIsPathNameValid
                )
            {
                int intI = 0;
                /*WHILE-UNTIL*/
                while (!(
                    (intI >= PathNameToVerify_I.Length) ||
                    !PathNameToVerify_I[intI].IsInSortedSet(PathX.arrcharIN_PATH_NAME)
                    ))
                {
                    intI = intI + 1;
                }

                boolIsPathNameValid = (
                    //                                      //All characters are valid
                    intI >= PathNameToVerify_I.Length
                    );
            }

            return boolIsPathNameValid;
        }

        //--------------------------------------------------------------------------------------------------------------
        /*INSTANCE VARIABLES*/

        //                                                  //Si es válido y se tiene acceso, contiene el Full Path.
        //                                                  //En su defecto, contiene el Path tal se proporciono y no
        //                                                  //      se deben tomar sus propiedades (aborta).
        //                                                  //Ejemplos:
        //                                                  //[
        //                                                  //c:\ (caso especial que es la raiz)
        //                                                  //c:\Abc
        //                                                  //c:\Abc\Xyz
        //                                                  //\\network\home
        //                                                  //\\network\home\Abc
        //                                                  //\\network\home\Abc\Xyz
        //                                                  //(ios, ¿unix, linus, etc?)
        //                                                  /// (caso especial que es la raiz)
        //                                                  ///Abc
        //                                                  ///Abc/Xyz
        //                                                  //]
        private readonly String strFullPath_Z;
        public String FullPath { get { return this.strFullPath_Z; } }

        //                                                  //dt del momento en que se construye este objeto.
        //                                                  //RECUERDESE que este es un syspath que puede o no
        //                                                  //      corresponder a un FILE o DIRECTORY que  existe.
        private readonly DateTime dtCreationTime_Z;
        public DateTime CreationTime { get { return this.dtCreationTime_Z; } }

        //                                                  //syspath de la raíz.
        //                                                  //Ejemplos:
        //                                                  //[
        //                                                  //c:\
        //                                                  //\\network\home
        //                                                  /// (ios, ¿unix, linus, etc?)
        //                                                  //]
        private readonly String strRoot_Z;
        public String Root { get { return this.strRoot_Z; } }

        //                                                  //DO_NOT_EXIST_ON_DISK, DIRECTORY, FILE or ROOT
        private readonly PathTypeEnum syspathtype_Z;
        public PathTypeEnum PathType { get { return this.syspathtype_Z; } }

        /*(20Mar2018, GLG) SE ELIMINA, POSIBLEMENTE LO REGRESE CUANDO ENTIENDA COMO FUNCIONA ESTO
        //                                                  //Determina si se tiene acceso a un Nombre, Path o Full Path
        //                                                  //      (File or Directory).
        //                                                  //Se utiliza Path.GetFullPath(strFileName_I) para
        //                                                  //      verificarlo.
        //                                                  //Si recibe un SecurityException indica que no tiene acceso.
        //                                                  //El path debe ser válido.
        private readonly bool boolHaveAccessTo_Z;
        public bool boolHaveAccessTo { get { return this.boolHaveAccessTo_Z; } } 
        */

        //                                                  //LOCAL o NETWORK.
        //                                                  //\\psf\Home que es como parallels accesa la información
        //                                                  //      LOCAL que esta en Mac IOS será registrada como
        //                                                  //      LOCAL.
        //                                                  //Nótese que esto causa que se tenga path \\psf (que parece
        //                                                  //      ser NETWORK) que será LOCAL.
        private readonly PathWhereEnum syspathwhere_Z;
        public PathWhereEnum PathWhere { get { return this.syspathwhere_Z; } }

        //--------------------------------------------------------------------------------------------------------------
        /*COMPUTED VARIABLES*/

        //                                                  //Es un Full Path local (esta en este equipo).
        private bool? boolnIsLocal_Z = null;
        public bool IsLocal
        {
            get
            {
                if (
                    this.boolnIsLocal_Z == null
                    )
                {
                    this.boolnIsLocal_Z = (
                        this.PathWhere == PathWhereEnum.LOCAL
                        );
                }

                return (bool)this.boolnIsLocal_Z;
            }
        }

        //                                                  //Es un Full Path que esta en red.
        private bool? boolnIsNetwork_Z = null;
        public bool IsNetwork
        {
            get
            {
                if (
                    this.boolnIsNetwork_Z == null
                    )
                {
                    this.boolnIsNetwork_Z = (
                        this.PathWhere == PathWhereEnum.NETWORK
                        );
                }

                return (bool)this.boolnIsNetwork_Z;
            }
        }

        //                                                  //Corresponde a un File o Directory que existe.
        private bool? boolnExists_Z = null;
        public bool Exists
        {
            get
            {
                if (
                    this.boolnExists_Z == null
                    )
                {
                    this.boolnExists_Z = (
                        //                                 //Si existe, es un File o Directory
                        this.PathType != PathTypeEnum.DO_NOT_EXIST_ON_DISK
                        );
                }

                return (bool)this.boolnExists_Z;
            }
        }

        //                                                  //Es un Directory.
        private bool? boolnIsDirectory_Z = null;
        public bool IsDirectory
        {
            get
            {
                if (
                    this.boolnIsDirectory_Z == null
                    )
                {
                    this.boolnIsDirectory_Z = (
                        this.PathType == PathTypeEnum.DIRECTORY
                        );
                }

                return (bool)this.boolnIsDirectory_Z;
            }
        }

        //                                                  //Es un File.
        private bool? boolnIsFile_Z = null;
        public bool IsFile
        {
            get
            {
                if (
                    this.boolnIsFile_Z == null
                    )
                {
                    this.boolnIsFile_Z = (
                        this.PathType == PathTypeEnum.FILE
                        );
                }

                return (bool)this.boolnIsFile_Z;
            }
        }

        //                                                  //Es un Root Directory.
        private bool? boolnIsRoot_Z = null;
        public bool IsRoot
        {
            get
            {
                if (
                    this.boolnIsRoot_Z == null
                    )
                {
                    this.boolnIsRoot_Z = (
                        this.PathType == PathTypeEnum.ROOT
                        );
                }

                return (bool)this.boolnIsRoot_Z;
            }
        }

        //                                                  //Nombre del archivo o directorio (sin el directorio que lo
        //                                                  //      contiene).
        //                                                  //Can not be root.
        private String strName_Z = null;
        public String Name
        {
            get
            {
                if (
                    this.strName_Z == null
                    )
                {
                    if (
                        this.PathType == PathTypeEnum.ROOT
                        )
                        Test.Abort(Test.ToLog(this.PathType, "PathType") + " can not have Name",
                            Test.ToLog(this, "this"));

                    this.strName_Z = Path.GetFileName(this.FullPath);
                }

                return this.strName_Z;
            }
        }

        //                                                  //SOLO SI ES UN FILE O AUN NO EXISTE.
        //                                                  //Si se solicitan cuando es un directorio va a abortar.

        //                                                  //File extension.
        private String strFileExtension_Z = null;
        public String FileExtension
        {
            get
            {
                if (
                    this.strFileExtension_Z == null
                    )
                {
                    if (
                        this.IsDirectory
                        )
                        Test.Abort(Test.ToLog(this.PathType, "PathType") + " should be a FILE",
                            Test.ToLog(this, "this"));

                    this.strFileExtension_Z = Path.GetExtension(this.FullPath);
                }

                return this.strFileExtension_Z;
            }
        }
        //                                                  //Nombre sin el file extensión.
        private String strFileNameWithoutExtension_Z = null;
        public String FileNameWithoutExtension
        {
            get
            {
                if (
                    this.strFileNameWithoutExtension_Z == null
                    )
                {
                    if (
                        this.IsDirectory
                        )
                        Test.Abort(Test.ToLog(this.PathType, "PathType") + " should be a FILE",
                            Test.ToLog(this, "this"));

                    this.strFileNameWithoutExtension_Z = Path.GetFileNameWithoutExtension(this.FullPath);
                }

                return this.strFileNameWithoutExtension_Z;
            }
        }

        private String strFullPathMasked_Z = null;
        public String FullPathMasked
        {
            get
            {
                if (
                    this.strFullPathMasked_Z == null
                    )
                {
                    this.strFullPathMasked_Z = Test.z_TowaPRIVATE_FullPathAsIsOrMasked(this);
                }

                return this.strFullPathMasked_Z;
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        /*SUPPORT METHODS FOR COMPUTED VARIABLES*/

        //--------------------------------------------------------------------------------------------------------------
        public override String ToLogShort()
        {
            return "<" + Test.ToLog(this.Name) + ", " + Test.ToLog(this.CreationTime) + ", " +
                Test.ToLog(Test.z_TowaPRIVATE_FullPathAsIsOrMasked(new PathX(this.Root))) + ", " +
                Test.ToLog(this.PathType) + ", " + Test.ToLog(this.PathWhere) + ">";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public override String ToLogFull()
        {
            return "<" + Test.ToLog(Test.z_TowaPRIVATE_FullPathAsIsOrMasked(this), "FullPath") + ", " +
                Test.ToLog(this.CreationTime, "CreationTime") + ", " +
                Test.ToLog(Test.z_TowaPRIVATE_FullPathAsIsOrMasked(new PathX(this.Root)), "Root") + ", " +
                Test.ToLog(this.PathType, "PathType") + ", " + Test.ToLog(this.PathWhere, "PathWhere") + ">";
        }

        //--------------------------------------------------------------------------------------------------------------
        /*OBJECT CONSTRUCTORS*/

        //--------------------------------------------------------------------------------------------------------------
        public PathX(
            //                                              //Crea un objeto Path.
            //                                              //this.*[O], asigna valores. 

            //                                              //OS full path (con :, /, \, ?)
            String FullPath_I
            )
            : base()
        {
            Test.AbortIfNull(FullPath_I, "FullPath_I");

            Test.AbortIfOneOrMoreCharactersAreNotInSortedSet(FullPath_I, "FullPath_I", PathX.arrcharIN_FULLPATH,
                "Path.arrcharIN_FULLPATH");

            this.strFullPath_Z = FullPath_I;
            this.dtCreationTime_Z = Std.Now();
            this.strRoot_Z = this.strRootGet();
            this.syspathtype_Z = this.syspathtypeGet();
            this.syspathwhere_Z = this.syspathwhereGet();

            //                                              //Nótese que se construyo con la información que tenía.
            //                                              //Aborta si no es válido.
            PathX.subAbortIfRootAndFullPathIsInvalid(this.FullPath, this.Root);
        }

        //--------------------------------------------------------------------------------------------------------------
        /*SUPPORT METHODS FOR CONSTRUCTORS*/

        //--------------------------------------------------------------------------------------------------------------
        private String strRootGet(
            //                                              //Calcula el strRoot.

            //                                              //this[I], acces info.
            )
        {
            return Path.GetPathRoot(this.FullPath);
        }

        //--------------------------------------------------------------------------------------------------------------
        private PathTypeEnum syspathtypeGet(
            //                                              //Calcula el syspathtype (DIRECTORY, FILE or
            //                                              //      DO_NOT_EXIST_ON_DISK).

            //                                              //this[I], acces info.
            )
        {
            PathTypeEnum syspathtypeGet;
            /*CASE*/
            if (
                //                                          //Reconoce como directorio.
                Directory.Exists(this.FullPath)
                )
            {
                syspathtypeGet = (this.FullPath.ToLower() == this.Root.ToLower()) ? PathTypeEnum.ROOT :
                    PathTypeEnum.DIRECTORY;
            }
            else if (
                //                                          //Reconoce como archivo.
                File.Exists(this.FullPath)
                )
            {
                syspathtypeGet = PathTypeEnum.FILE;
            }
            else
            {
                syspathtypeGet = PathTypeEnum.DO_NOT_EXIST_ON_DISK;
            }
            /*END-CASE*/

            return syspathtypeGet;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private PathWhereEnum syspathwhereGet(
            //                                              //Calcula el syspathwhere (LOCAL or NETWORK).

            //                                              //this[I], acces info.
            )
        {
            //                                              //\\psf\home
            const String strPARALLELS_PSF = @"\\psf\Home";

            PathWhereEnum syspathwhereGet;
            /*CASE*/
            if (
                //                                          //Tiene volumen (Ej c:....)
                (this.FullPath.Length >= 2) &&
                (this.FullPath[1] == PathX.VOLUME_SEPARATOR_CHAR)
                )
            {
                syspathwhereGet = PathWhereEnum.LOCAL;
            }
            else if (
                //                                          //Tiene network (//network/....).
                this.FullPath.StartsWith(PathX.NETWORK_MARK, StringComparison.Ordinal)
                )
            {
                //                                          //Es el caso especial de Parallels donde utiliza esta forma
                //                                          //      para hacer referencia al disco de Mac IOS que es
                //                                          //      LOCAL.
                //                                          //CONFORME SE ENTIENDA ESTO ES OTRAS MÁQUINAS VIRTUALES SE
                //                                          //      DISEÑARÁ UN MECANISMO PARAMETRIZADO
                syspathwhereGet =
                    (this.Root.ToLower() == strPARALLELS_PSF.ToLower()) ? PathWhereEnum.LOCAL : PathWhereEnum.NETWORK;
            }
            else
            {
                //                                          //No tiene volumen ni network (Ej /....)
                syspathwhereGet = PathWhereEnum.LOCAL;
            }
            /*END-CASE*/

            return syspathwhereGet;
        }

        //--------------------------------------------------------------------------------------------------------------
        /*TRANSFORMATION METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        /*ACCESS METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public PathX AddName(
            //                                              //Genera OTRO syspath añadiendo un nombre a un syspath de un
            //                                              //      directorio.
            //                                              //this[I], debe ser un directorio, toma información.

            //                                              //syspath, nuevo syspath ya combinado.

            //                                              //Nombre a combinar con el path del this.
            String NameToAdd_I
            )
        {
            if (!(
                this.IsDirectory || this.IsRoot
                ))
                Test.Abort(Test.ToLog(this.PathType, "PathType") + " should be DIRECTORY or ROOT",
                    Test.ToLog(this, "this"));
            if (!(
                PathX.IsPathNameValid(NameToAdd_I)
                ))
                Test.Abort(Test.ToLog(NameToAdd_I, "strNameToAdd_I") + " is not valid");

            //                                              //Si tiene la forma c:\ ó / ya añade otro separador
            String strFullPathPlusName = Path.Combine(this.FullPath, NameToAdd_I);

            //                                              //Regresa la combinación.
            return new PathX(strFullPathPlusName);
        }

        //--------------------------------------------------------------------------------------------------------------
        public PathX GetDirectoryPath(
            //                                              //Genera OTRO syspath con el directorio que contiene el
            //                                              //      syspath.
            //                                              //this[I], can not be root.
            )
        {
            if (
                this.IsRoot
                )
                Test.Abort(Test.ToLog(this, "this") + " is a Root Directory, can not get its directory");

            return new PathX(Path.GetDirectoryName(this.FullPath));
        }

        //--------------------------------------------------------------------------------------------------------------
        public bool IsContainedIn(
            //                                              //Determina si un directorio esta incluido en otro path.
            //                                              //Esta incluido, si:
            //                                              //1. Ambos son el mismo directorio.
            //                                              //2. En el padre que contiene el path (ya sea un file u otro
            //                                              //      directorio.
            //                                              //3. El path esta contenido en el directorio a cualquier 
            //                                              //      nivel.
            //                                              //true, si esta incluido.

            //                                              //this, info del objeto

            //                                              //Para verificar si esta contenido.
            PathX DirectoryPath_L
            )
        {
            if (!(
                DirectoryPath_L.IsDirectory || DirectoryPath_L.IsRoot
                ))
                Test.Abort(
                    Test.ToLog(DirectoryPath_L.PathType, "DirectoryPath_L.PathType") + " should be DIRECTORY or ROOT",
                    Test.ToLog(this, "this"), Test.ToLog(DirectoryPath_L, "DirectoryPath_L"));

            String strThisFullPathLower = this.FullPath.ToLower();
            String strDirectoryFullPathLower = DirectoryPath_L.FullPath.ToLower();

            return (
                (strThisFullPathLower == strDirectoryFullPathLower) ||
                //                                          //Tiene la forms: Directory\...
                strThisFullPathLower.StartsWith(strDirectoryFullPathLower, StringComparison.Ordinal) &&
                    (strThisFullPathLower[strDirectoryFullPathLower.Length] == PathX.DIRECTORY_SEPARATOR_CHAR)
                );
        }

        //--------------------------------------------------------------------------------------------------------------
        public String RestOfPath(
            //                                              //Extrae lo que queda del path al quitarle del principio el
            //                                              //      path de un directorio que lo contiene.
            //                                              //Ej, tengo path que contiene  "C:\DirA\DirB\DirC\DirOFile",
            //                                              //      al cual que quiero quitar "C:\DirA\DirB", el
            //                                              //      resultado será el String "\DirC\DirOFile".
            //                                              //str, (Ej. "\DirC\DirOFile").

            //                                              //this, info del objeto

            PathX syspathDirectoryContainingThisPath_I
            )
        {
            if (!(
                this.IsContainedIn(syspathDirectoryContainingThisPath_I)
                ))
                Test.Abort(
                    Test.ToLog(syspathDirectoryContainingThisPath_I, "syspathDirectoryContainingThisPath_I") +
                        " IS NOT contained in " + Test.ToLog(this, "this"));

            return this.FullPath.Substring(syspathDirectoryContainingThisPath_I.FullPath.Length);
        }

        //--------------------------------------------------------------------------------------------------------------
        public String MaskRestOfPath(
            //                                              //Igual a strRestOfPath pero masked si esta con
            //                                              //      comparable log.
            //                                              //str, (Ej. "▸DirC▸DirOFile").

            //                                              //this, info del objeto

            PathX DirectoryContainingThisPath_I
            )
        {
            String strMaskRestOfPath = this.RestOfPath(DirectoryContainingThisPath_I);

            if (
                Test.z_TowaPRIVATE_boolIsComparableLog()
                )
            {
                strMaskRestOfPath = strMaskRestOfPath.Replace(PathX.DIRECTORY_SEPARATOR_CHAR,
                    PathX.DIRECTORY_SEPARATOR_CHAR_MASKED);
            }

            return strMaskRestOfPath;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static PathX GetUserPath(
            )
        {
            String strPathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            return new PathX(strPathUser);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static PathX GetDesktop(
            )
        {
            String strPathDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            return new PathX(strPathDesktop); ;
        }

        //--------------------------------------------------------------------------------------------------------------
        public int CompareTo(
            //                                              //Required for Sort, BinarySearch and CompareTo.

            //                                              //this[I], object key info.

            //                                              //syspath or str
            Object obj_I
            )
        {
            String strFullPathToCompare;
            /*CASE*/
            if (
                obj_I is PathX
                )
            {
                strFullPathToCompare = ((PathX)obj_I).FullPath; ;
            }
            else if (
                obj_I is String
                )
            {
                strFullPathToCompare = (String)obj_I;
            }
            else
            {
                Test.Abort(
                    Test.ToLog(obj_I.GetType(), "obj_I.type") +
                        " is not a compatible CompareTo argument, the options are: PathX & String",
                    Test.ToLog(this.GetType(), "this.type"));

                strFullPathToCompare = null;
            }
            /*END-CASE*/

            return String.CompareOrdinal(this.FullPath.ToLower(), strFullPathToCompare.ToLower());
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================

}
/*END-TASK*/
