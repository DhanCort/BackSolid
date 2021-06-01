/*TASK Test support for testing*/
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace TowaStandard
{
    //==================================================================================================================
    public static class Test
    {
        /*TASK Test.Integer(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog(
            //                                              //str, info. prepared for display.

            int integer_I,
            String text_I
            )
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            return text_I + "(" + Test.ToLog(integer_I) + ")";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog(
            //                                              //str, info. prepared for display.

            int integer_I
            )
        {
            String strInfo = (integer_I == Int32.MinValue) ? "<MIN_INTEGER>" :
                (integer_I == Int32.MaxValue) ? "<MAX_INTEGER>" : "";

            return Std.ToText(integer_I) + strInfo;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog(
            //                                              //str, info. prepared for display.

            long longInteger_I,
            String text_I
            )
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            return text_I + "(" + Test.ToLog(longInteger_I) + ")";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog(
            //                                              //str, info. prepared for display.

            long longInteger_I
            )
        {
            String strInfo = (longInteger_I == Int64.MinValue) ? "<MIN_LONG_INTEGER>" :
                (longInteger_I == Int64.MaxValue) ? "<MAX_LONG_INTEGER>" : "";

            return Std.ToText(longInteger_I) + strInfo;
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.Number(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog(
            //                                              //str, info. prepared for display.

            double number_I,

            String text_I
            )
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            return text_I + "(" + Test.ToLog(number_I) + ")";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog(
            //                                              //str, info. prepared for display.

            double number_I
            )
        {
            String strInfo =
                (number_I == Double.NegativeInfinity) ? "<NEGATIVE_INFINITY>" :
                (number_I == Double.MinValue) ? "<MIN_NUMBER>" :
                (number_I == Double.MaxValue) ? "<MAX_NUMBER>" :
                (number_I == Double.PositiveInfinity) ? "<POSITIVE_INFINITY>" :
                (number_I == Double.Epsilon) ? "<EPSILON>" :
                (number_I == -Double.Epsilon) ? "<-EPSILON>" :
                (number_I == Math.PI) ? "<PI>" :
                (Std.IsNaN(number_I)) ? "<NOT_A_NUMBER>" : "";

            return Std.ToText(number_I) + strInfo;
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.Character(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        /*CONSTANTS*/

        //                                                  //Caracter que será usado como substituto cuando un caracter
        //                                                  //      no sea "visible".
        private const char charSUBSTITUTE_NO_USEFUL_IN_TEXT = '●';

        //                                                  //En font consolas, estos caracteres se muestran IGUAL (o
        //                                                  //      muy parecidos) a otros.
        //                                                  //Esta información servira para que en LogTo se proporcione
        //                                                  //      una buena descripción.
        private static readonly T3fakecharTuple[] arrt3fakecharFAKE = {
            //                                              //Uppercase Letter
            new T3fakecharTuple('Α', '\u0391', "Fake A(u0041)"),
            new T3fakecharTuple('А', '\u0410', "Fake A(u0041)"),
            new T3fakecharTuple('Ӑ', '\u04D0', "Fake Ă(u0102)"),
            new T3fakecharTuple('Ӓ', '\u04D2', "Fake Ä(u00C4)"),
            new T3fakecharTuple('Ᾰ', '\u1FB8', "Fake Ă(u0102)"),
            new T3fakecharTuple('Ᾱ', '\u1FB9', "Fake Ā(u0100)"),

            new T3fakecharTuple('Ε', '\u0395', "Fake E(u0045)"),
            new T3fakecharTuple('Ѐ', '\u0400', "Fake È(u00C8)"),
            new T3fakecharTuple('Ё', '\u0401', "Fake Ë(u00CB)"),
            new T3fakecharTuple('Е', '\u0415', "Fake E(u0045)"),
            new T3fakecharTuple('Ӗ', '\u04D6', "Fake Ĕ(u0114)"),

            new T3fakecharTuple('Ι', '\u0399', "Fake I(u0049)"),
            new T3fakecharTuple('Ϊ', '\u03AA', "Fake Ï(u00CF)"),
            new T3fakecharTuple('І', '\u0406', "Fake I(u0049)"),
            new T3fakecharTuple('Ї', '\u0407', "Fake Ï(u00CF)"),
            new T3fakecharTuple('Ӏ', '\u04C0', "Fake I(u0049)"),
            new T3fakecharTuple('Ῐ', '\u1FD8', "Fake Ĭ(u012C)"),
            new T3fakecharTuple('Ῑ', '\u1FD9', "Fake Ī(u012A)"),

            new T3fakecharTuple('Ο', '\u039F', "Fake O(u004F)"),
            new T3fakecharTuple('О', '\u041E', "Fake O(u004F)"),
            new T3fakecharTuple('Ӧ', '\u04E6', "Fake Ö(u00D6)"),

            new T3fakecharTuple('Β', '\u0392', "Fake B(u0042)"),
            new T3fakecharTuple('В', '\u0412', "Fake B(u0042)"),

            new T3fakecharTuple('Ϲ', '\u03F9', "Fake C(u0043)"),
            new T3fakecharTuple('С', '\u0421', "Fake C(u0043)"),

            new T3fakecharTuple('Đ', '\u0110', "Fake Ð(u00D0)"),
            new T3fakecharTuple('Ɖ', '\u0189', "Fake Ð(u00D0)"),

            new T3fakecharTuple('Η', '\u0397', "Fake H(u0048)"),
            new T3fakecharTuple('Н', '\u041D', "Fake H(u0048)"),

            new T3fakecharTuple('Ј', '\u0408', "Fake J(u004A)"),

            new T3fakecharTuple('Κ', '\u039A', "Fake K(u004B)"),
            new T3fakecharTuple('К', '\u041A', "Fake K(u004B)"),
            new T3fakecharTuple('Ḱ', '\u1E30', "Fake Ќ(u040C)"),

            new T3fakecharTuple('Μ', '\u039C', "Fake M(u004D)"),
            new T3fakecharTuple('М', '\u041C', "Fake M(u004D)"),

            new T3fakecharTuple('Ν', '\u039D', "Fake N(u004E)"),

            new T3fakecharTuple('Ρ', '\u03A1', "Fake P(u0050)"),
            new T3fakecharTuple('Р', '\u0420', "Fake P(u0050)"),

            new T3fakecharTuple('Ѕ', '\u0405', "Fake S(u0053)"),

            new T3fakecharTuple('Τ', '\u03A4', "Fake T(u0054)"),
            new T3fakecharTuple('Т', '\u0422', "Fake T(u0054)"),

            new T3fakecharTuple('Χ', '\u03A7', "Fake X(u0058)"),
            new T3fakecharTuple('Х', '\u0425', "Fake X(u0058)"),

            new T3fakecharTuple('Υ', '\u03A5', "Fake Y(u0059)"),
            new T3fakecharTuple('Ϋ', '\u03AB', "Fake Ÿ(u0178)"),
            new T3fakecharTuple('Ү', '\u04AE', "Fake Y(u0059)"),

            new T3fakecharTuple('Ζ', '\u0396', "Fake Z(u005A)"),

            new T3fakecharTuple('Ȝ', '\u021C', "Fake Ʒ(u01B7)"),
            new T3fakecharTuple('Λ', '\u039B', "Fake Ʌ(u0245)"),
            new T3fakecharTuple('Σ', '\u03A3', "Fake Ʃ(u01A9)"),
            new T3fakecharTuple('ϴ', '\u03F4', "Fake Ɵ(u019F)"),
            new T3fakecharTuple('Ͻ', '\u03FD', "Fake Ɔ(u0186)"),
            new T3fakecharTuple('П', '\u041F', "Fake Π(u03A0)"),
            new T3fakecharTuple('Ѱ', '\u0470', "Fake Ψ(u03A8)"),
            new T3fakecharTuple('Ѳ', '\u0472', "Fake Ɵ(u019F)"),
            new T3fakecharTuple('Ә', '\u04D8', "Fake Ə(u018F)"),
            new T3fakecharTuple('Ӡ', '\u04E0', "Fake Ʒ(u01B7)"),
            new T3fakecharTuple('Ө', '\u04E8', "Fake Ɵ(u019F)"),
            new T3fakecharTuple('Ԑ', '\u0510', "Fake Ɛ(u0190)"),
            new T3fakecharTuple('Ω', '\u2126', "Fake Ω(u03A9)"),
            new T3fakecharTuple('Ↄ', '\u2183', "Fake Ɔ(u0186)"),
            //                                              //Lowercase Letter
            new T3fakecharTuple('а', '\u0430', "Fake a(u0061)"),
            new T3fakecharTuple('ӓ', '\u04D3', "Fake ä(u00E4)"),
            new T3fakecharTuple('ѐ', '\u0450', "Fake è(u00E8)"),
            new T3fakecharTuple('ё', '\u0451', "Fake ë(u00EB)"),
            new T3fakecharTuple('і', '\u0456', "Fake i(u0069)"),
            new T3fakecharTuple('ϲ', '\u03F2', "Fake c(u0063)"),
            new T3fakecharTuple('ϳ', '\u03F3', "Fake j(u006A)"),
            //                                              //Space Separator
            new T3fakecharTuple(' ', '\u2000', "Fake blank(u0020)"),
            new T3fakecharTuple(' ', '\u2001', "Fake blank(u0020)"),
            new T3fakecharTuple(' ', '\u2002', "Fake blank(u0020)"),
            new T3fakecharTuple(' ', '\u2003', "Fake blank(u0020)"),
            new T3fakecharTuple(' ', '\u2004', "Fake blank(u0020)"),
            new T3fakecharTuple(' ', '\u2005', "Fake blank(u0020)"),
            new T3fakecharTuple(' ', '\u2006', "Fake blank(u0020)"),
            new T3fakecharTuple(' ', '\u2007', "Fake blank(u0020)"),
            new T3fakecharTuple(' ', '\u2008', "Fake blank(u0020)"),
            new T3fakecharTuple(' ', '\u2009', "Fake blank(u0020)"),
            new T3fakecharTuple(' ', '\u200A', "Fake blank(u0020)"),
            new T3fakecharTuple(' ', '\u202F', "Fake blank(u0020)"),
            new T3fakecharTuple(' ', '\u205F', "Fake blank(u0020)"),
            //                                              //Dash Punctuation
            new T3fakecharTuple('‐', '\u2010', "Fake -(u002D)"),
            new T3fakecharTuple('–', '\u2013', "Fake ‒(u2012)"),
            new T3fakecharTuple('―', '\u2015', "Fake —(u2014)"),
            };

        //                                                  //The following set of characters do not print rigth (print
        //                                                  //      something in a box).
        //                                                  //Some printer print ok, some other no.
        private static readonly T2charDescriptionTuple[] arrt2charNONPRINTABLE =
        {
            //                                              //Modifier Letter
            new T2charDescriptionTuple('ˆ', "Nonprintable, accent â"),
            new T2charDescriptionTuple('ˇ', "Nonprintable, accent ň"),
            new T2charDescriptionTuple('ˉ', "Nonprintable, accent ā"),
            //                                              //Initial Quote Punctuation
            new T2charDescriptionTuple('‘', "Nonprintable, open curved (')quote"),
            //                                              //Final Quote Punctuation
            new T2charDescriptionTuple('’', "Nonprintable, close curved (')quote"),
            new T2charDescriptionTuple('”', "Nonprintable, close curved (\")double quote"),
            //                                              //Modifier Symbol
            new T2charDescriptionTuple('¯', "Nonprintable, accent ā"),
            new T2charDescriptionTuple('¸', "Nonprintable, lower accent ņ"),
            new T2charDescriptionTuple('˘', "Nonprintable, accent ă"),
            new T2charDescriptionTuple('˙', "Nonprintable, accent ċ"),
            new T2charDescriptionTuple('˚', "Nonprintable, accent å"),
            new T2charDescriptionTuple('˛', "Nonprintable, lower accent ę"),
            new T2charDescriptionTuple('˜', "Nonprintable, accent ã"),
            new T2charDescriptionTuple('˝', "Nonprintable, accent ő"),
            new T2charDescriptionTuple('῁', "Nonprintable, accent ῧ"),
            new T2charDescriptionTuple('῭', "Nonprintable, accent ῢ"),
            new T2charDescriptionTuple('΅', "Nonprintable, accent ΰ"),
            new T2charDescriptionTuple('´', "Nonprintable, accent ά"),
            //                                              //Other Punctuation
            new T2charDescriptionTuple('·', "Nonprintable, middle dot"),
        };

        //                                                  //This are all character that can be keyed on keboard
        private static readonly String strCHAR_DO_NOT_SHOW_HEX = "0123456789" +
            //                                              //Upeer case letter
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ" + "ÁÉÍÓÚ" + "ÀÈÌÒÙ" + "ÄËÏÖÜ" + "ÂÊÎÔÛ" + "Ñ" +
            //                                              //Lower case letter
            "abcdefghijklmnopqrstuvwxyz" + "áéíóú" + "àèìòù" + "äëïöü" + "âêîôû" + "ñ" +
            //                                              //Keyboard digits row (lower, upper, alt, upper alt)
            "º'¡" + ("ª!" + '"' + "·$%&/()=?¿") + ("\\" + "|@#¢∞¬÷“”≠‚") +
            //                                              //Keyboard QW... row (lower, upper, alt, upper alt)
            "`+" + "^*" + "œæ€®†¥øπ[]" + "ŒÆ‡Øˆ±" +
            //                                              //Keyboard AS... row (lower, upper, alt, upper alt).
            //                                              //'?' (2 parallel slashs) can be enter alt+G but is not in
            //                                              //      USEFUL (takes more space tha a regular character.
            //                                              //"´ç" + "¨Ç" + "å∫∂ƒ™¶§~{}" + "Å∆ﬁﬂ¯ˇ˘˜«»" ﬁﬂ are not in
            //                                              //      USEFUL
            "´ç" + "¨Ç" + "å∫∂ƒ™¶§~{}" + "Å∆¯ˇ˘˜«»" +
            //                                              //Keyboard <ZX... row (lower, upper, alt, upper alt)
            "<,.-" + ">;:_" + "≤Ω∑©√ßµ„…–" + "≥‹›◊˙˚" +
            //                                              //'▸' used as "neutral" directory separator.
            PathX.DIRECTORY_SEPARATOR_CHAR_MASKED +
            //                                              //Space
            " ";
        private static readonly char[] arrcharDO_NOT_SHOW_HEX;

        //                                                  //En la siguiente estructura se incluyen:
        //                                                  //1. Nonprintable (only on some printers).
        //                                                  //2. Fake, description "'\u????', Fake ?(u????)".
        //                                                  //3. Escape.
        //                                                  //AT CODING, SOME VALUE IS REQUIRES BECAUSE IT CAN BE USED
        //                                                  //      BEFORE INITIALIZATION IS COMPLETE.
        private static readonly T2charDescriptionTuple[] arrt2charDESCRIPTION = new T2charDescriptionTuple[0];


        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog(
            //                                              //str, info. prepared for display.

            char character_I,
            String text_I
            )
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            return text_I + "(" + Test.ToLog(character_I) + ")";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog(
            //                                              //Ejemplos:
            //                                              //1 'c'.
            //                                              //2 '©'<u00A9>.
            //                                              //3 '●'<u0009, \t, Horizontal Tab>.
            //                                              //1) No tiene nada extraño, solo se añaden las comillas.
            //                                              //2) El caracter © no esta en DO_NOT_SHOW_HEX, incluyo su
            //                                              //      su hexadecimal.
            //                                              //3) El caracter esta en arrt2charDESCRITPTION, incluyo su
            //                                              //      hexadecimal y su descripción.
            //                                              //Adicionalmente, si el caracter no esta en USEFUL_IN_TEXT
            //                                              //      se substituye por '●'.
            //                                              //str, info. prepared for display.

            //                                              //Caracter a analizar.
            char character_I
            )
        {
            //                                              //Form char ('c').
            String strChar = "'" + ((
                character_I.IsInSortedSet(Std.CHARS_USEFUL_IN_TEXT)
                )
                ? character_I : charSUBSTITUTE_NO_USEFUL_IN_TEXT) + "'";

            //                                              //Get diagnostic info.
            int intT2 = character_I.BinarySearch(Test.arrt2charDESCRIPTION);

            String strInfo;
            if (
                //                                          //Se solicita SI mostrar su hex
                !character_I.IsInSortedSet(Test.arrcharDO_NOT_SHOW_HEX) ||
                //                                          //Se solicita SI mostar su descripción
                (intT2 >= 0)
                )
            {
                //                                          //Form tuple with diagnostic info (2 options):
                //                                          //<u89FE>
                //                                          //<u89FE, description>
                strInfo = "<u" + Std.ToHexText(character_I) +
                    ((
                    intT2 >= 0
                    )
                    ? ", " + Test.arrt2charDESCRIPTION[intT2].strDESCRIPTION : "") + ">";
            }
            else
            {
                //                                          //No aditional info
                strInfo = "";
            }

            return strChar + strInfo;
        }

        //==============================================================================================================
        /*END-TASK*/

        //--------------------------------------------------------------------------------------------------------------
        /*SUPPORT METHODS FOR INITIALIZER*/

        //--------------------------------------------------------------------------------------------------------------
        private static void subPrepareConstantsCharacter(
            )
        {

        }

        //--------------------------------------------------------------------------------------------------------------
        private static void subVerifyConstantsCharacter(
            //                                              //Método de apoyo llamado en constructor estático.
            //                                              //Prepara las constantes para poder utilizarlas.
            //                                              //1. Inicializa proceso para evitar desplegar 2 veces el
            //                                              //      mismo objeto
            //                                              //2. arrt2uccCHAR_USEFUL_IN_TEXT:
            //                                              //2a. Este ordenada por ucc, sin duplicados.
            //                                              //2b. Dentro de cada ucc, este ordenada por la secuencia del
            //                                              //      caracter, sin duplicados
            //                                              //2c. Todos sean <= al caracter xD7FF.
            //                                              //2d. En forma global, no haya caracteres duplicados.
            //                                              //3. arrt3fakecharFAKE:
            //                                              //3a. ordenar.
            //                                              //3b. no duplicados
            //                                              //3c. charFAKE debe estar en USEFUL.
            //                                              //3d. charHEX y charFAKE debe ser el mismo.
            //                                              //3e. strDESCRIPTION "..... ?(u????)", el x???? debe ser la
            //                                              //      correspondiente al caracter ?.
            //                                              //4. arrt2charNONPRINTABLE.
            //                                              //4a. ordenar.
            //                                              //4b. no duplicados
            //                                              //4c. debe estar en USEFUL.
            //                                              //4d. tener descripción.
            //                                              //5. arrt2charESCAPE.
            //                                              //5a. ordenar.
            //                                              //5b. no duplicados
            //                                              //5c. NO debe estar en USEFUL.
            //                                              //5d. tener descripción.
            )
        {
            Test.subVerifyArrcharDoNotShowHex();
            Test.subVerifyArrt3fakecharFake();
            Test.subVerifyArrt2charNonprintable();
            Test.subVerifyArrt2charDescription();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static void subVerifyArrcharDoNotShowHex(
            //                                              //a. Chars are USEFUL?
            //                                              //b. Sort.
            //                                              //c. No duplicates.
            )
        {
            Test.AbortIfOneOrMoreCharactersAreNotInSortedSet(Test.strCHAR_DO_NOT_SHOW_HEX, "Test.strCHAR_DO_NOT_SHOW_HEX",
                Std.CHARS_USEFUL_IN_TEXT, "Std.CHARS_USEFUL_IN_TEXT");

            Test.AbortIfDuplicate(Test.arrcharDO_NOT_SHOW_HEX, "Test.arrcharDO_NOT_SHOW_HEX");
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static void subVerifyArrt3fakecharFake(
            //                                              //a. ordenar.
            //                                              //b. no duplicados
            //                                              //c. charFAKE debe estar en USEFUL.
            //                                              //d. charHEX y charFAKE debe ser el mismo.
            //                                              //e. strDESCRIPTION debe ser "Fake blank(u0020" o
            //                                              //      "Fake ¿(u????)" donde ¿ y ???? representan el
            //                                              //      mismo caracter.
            )
        {

            //                                              //Verifica no duplicados
            Test.AbortIfDuplicate(Test.arrt3fakecharFAKE, "Test.arrt3fakecharFAKE");

            //                                              //Verifica chars en tupla
            for (int intT3 = 0; intT3 < arrt3fakecharFAKE.Length; intT3 = intT3 + 1)
            {
                if (
                    !Test.arrt3fakecharFAKE[intT3].charFAKE.IsInSortedSet(Std.CHARS_USEFUL_IN_TEXT)
                    )
                    Test.Abort(
                        Test.ToLog(Test.arrt3fakecharFAKE[intT3].charFAKE,
                                "Test.arrt3fakecharFAKE[" + intT3 + "].charFAKE") +
                            " do not exist in USEFUL_IN_TEXT",
                        Test.ToLog(Test.arrt3fakecharFAKE[intT3], "Test.arrt3fakecharFAKE[" + intT3 + "]"),
                        Test.ToLog(Test.arrt3fakecharFAKE, "Test.arrt3fakecharFAKE"));

                if (
                    //                                      //Character IMAGE != Character in \u???? format
                    Test.arrt3fakecharFAKE[intT3].charFAKE != Test.arrt3fakecharFAKE[intT3].charHEX
                    )
                    Test.Abort(
                        Test.ToLog(Test.arrt3fakecharFAKE[intT3].charFAKE,
                                "Test.arrt3fakecharFAKE[" + intT3 + "].charFAKE") +
                            " should be equal to" +
                            Test.ToLog(Test.arrt3fakecharFAKE[intT3].charHEX,
                                "Test.arrt3fakecharFAKE[" + intT3 + "].charHEX"),
                        Test.ToLog(Test.arrt3fakecharFAKE[intT3], "Test.arrt3fakecharFAKE[" + intT3 + "]"),
                        Test.ToLog(Test.arrt3fakecharFAKE, "Test.arrt3fakecharFAKE"));

                //                                          //Verifica descripción

                //                                          //To easy code
                String strDESCRIPTION = Test.arrt3fakecharFAKE[intT3].strDESCRIPTION;

                if (
                    strDESCRIPTION == null
                    )
                    Test.Abort(
                        Test.ToLog(strDESCRIPTION,
                                "arrt3fakecharFAKE[" + intT3 + "].strDESCRIPTION") +
                            " can not be null",
                        Test.ToLog(Test.arrt3fakecharFAKE[intT3], "Test.arrt3fakecharFAKE[" + intT3 + "]"),
                        Test.ToLog(Test.arrt3fakecharFAKE, "Test.arrt3fakecharFAKE"));

                /*CASE*/
                if (
                    strDESCRIPTION == "Fake blank(u0020)"
                    )
                {
                    //                                      //Es una opción correcta, NO HACE NADA
                }
                else if (
                    //                                      //Tiene la forma correcta
                    (strDESCRIPTION.Length == "Fake ?(u????)".Length) &&
                    strDESCRIPTION.StartsWith("Fake ", StringComparison.Ordinal) &&
                    (strDESCRIPTION.Substring("Fake ?".Length, "(u".Length) == "(u") &&
                    (strDESCRIPTION[strDESCRIPTION.Length - 1] == ')')
                    )
                {
                    //                                      //Verifica descripción
                    char charFaked = strDESCRIPTION["Fake ".Length];
                    String strCharFaked = Std.ToHexText(charFaked);
                    String strFakedHex = strDESCRIPTION.Substring("Fake ?(u".Length, 4);
                    if (
                        //                                  //? y ???? no representan el mismo caracter
                        strCharFaked != strFakedHex
                        )
                        Test.Abort(
                            Test.ToLog(strDESCRIPTION, "Test.arrt3fakecharFAKE[" + intT3 + "].strDESCRIPTION") +
                                " should include hexadecimal char " +
                                "code \"????\" corresponding to \"?\" in \"Fake ?(u????)\"",
                            Test.ToLog(Test.arrt3fakecharFAKE[intT3], "Test.arrt3fakecharFAKE[" + intT3 + "]"),
                            Test.ToLog(Test.arrt3fakecharFAKE, "Test.arrt3fakecharFAKE"));

                    //                                      //Es una opción correcta, NO HACE NADA
                }
                else
                {
                    Test.Abort(
                        Test.ToLog(strDESCRIPTION, "arrt3fakecharFAKE[" + intT3 + "].strDESCRIPTION") +
                            " invalid description",
                        Test.ToLog(Test.arrt3fakecharFAKE[intT3], "Test.arrt3fakecharFAKE[" + intT3 + "]"),
                        Test.ToLog(Test.arrt3fakecharFAKE, "Test.arrt3fakecharFAKE"));
                }
                /*END-CASE*/
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static void subVerifyArrt2charNonprintable(
            //                                              //4. arrt2charNONPRINTABLE.
            //                                              //4a. ordenar.
            //                                              //4b. no duplicados
            //                                              //4c. debe estar en USEFUL.
            //                                              //4d. tener descripción.
            )
        {

            //                                              //Verifica no duplicados
            Test.AbortIfDuplicate(Test.arrt2charNONPRINTABLE, "Test.arrt2charNONPRINTABLE");

            //                                              //Verifica chars en tupla
            for (int intT2 = 0; intT2 < arrt2charNONPRINTABLE.Length; intT2 = intT2 + 1)
            {

                if (
                    !Test.arrt2charNONPRINTABLE[intT2].charX.IsInSortedSet(Std.CHARS_USEFUL_IN_TEXT)
                    )
                    Test.Abort(
                        Test.ToLog(Test.arrt2charNONPRINTABLE[intT2].charX,
                                "Test.arrt2charNONPRINTABLE[" + intT2 + "].charX") +
                            " does not exist in USEFUL_IN_TEXT",
                        Test.ToLog(Test.arrt2charNONPRINTABLE[intT2], "Test.arrt2charNONPRINTABLE[" + intT2 + "]"),
                        Test.ToLog(Test.arrt2charNONPRINTABLE, "Test.arrt2charNONPRINTABLE"));

                if (
                    Test.arrt2charNONPRINTABLE[intT2].strDESCRIPTION == null
                    )
                    Test.Abort(
                        Test.ToLog(Test.arrt2charNONPRINTABLE[intT2].strDESCRIPTION,
                                "Test.arrt2charNONPRINTABLE[" + intT2 + "].strDESCRIPTION") +
                            " can not be null",
                        Test.ToLog(Test.arrt2charNONPRINTABLE[intT2], "Test.arrt2charNONPRINTABLE[" + intT2 + "]"),
                        Test.ToLog(Test.arrt2charNONPRINTABLE, "Test.arrt2charNONPRINTABLE"));

                if (
                    Test.arrt2charNONPRINTABLE[intT2].strDESCRIPTION == ""
                    )
                    Test.Abort(
                        Test.ToLog(Test.arrt2charNONPRINTABLE[intT2].strDESCRIPTION,
                                "Test.arrt2charNONPRINTABLE[" + intT2 + "].strDESCRIPTION") +
                            " should have a description",
                        Test.ToLog(Test.arrt2charNONPRINTABLE[intT2], "Test.arrt2charNONPRINTABLE[" + intT2 + "]"),
                        Test.ToLog(Test.arrt2charNONPRINTABLE, "Test.arrt2charNONPRINTABLE"));
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static void subVerifyArrt2charDescription(
            //                                              //Se incluyen:
            //                                              //1. Nonprintable.
            //                                              //2. Fake, description "'\u????', Fake ?(u????)".
            //                                              //3. Escape.
            //                                              //Al juntarse los 3 conjuntos de descripciones no deben
            //                                              //      resultar duplicados.
            )
        {
            Test.AbortIfDuplicate(Test.arrt2charDESCRIPTION, "Test.arrt2charDESCRIPTION");
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.Boolean(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog(
            //                                              //str, info. prepared for display.

            bool boolean_I,
            String text_I
            )
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            return text_I + "(" + Test.ToLog(boolean_I) + ")";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog(
            //                                              //str, info. prepared for display.

            bool boolean_I
            )
        {
            return Std.ToText(boolean_I);
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.Currency(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog(
            //                                              //str, info. prepared for display.

            this Currency curr_I,
            String text_I
            )
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            return text_I + "(" + Test.ToLog(curr_I) + ")";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog(
            //                                              //str, info. prepared for display.

            this Currency curr_I
            )
        {
            String strInfo = (curr_I.TotalCents == Int64.MinValue) ? "<MIN_CURRENCY>" :
                (curr_I.TotalCents == Int64.MaxValue) ? "<MAX_CURRENCY>" : "";

            return curr_I.ToText() + strInfo;
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.String(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        /*CONSTANTS*/

        //                                                  //Si un String excede esta longitud, se muestra la longitud
        //                                                  //      ejemplo "abd def.... xyz"<88>.
        private const int intLONG_STRING = 50;

        //                                                  //Cantidad máxima de caracteres diagnosticados que se
        //                                                  //      mostrarán al final de un String
        private const int intMAX_DIAGNOSTIC = 100;

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog(
            //                                              //str, info. prepared for display.

            String String_I,
            String text_I
            )
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            return text_I + "(" + Test.ToLog(String_I) + ")";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog(
            //                                              //Prepare to display:
            //                                              //1. "this is an String"<17>.
            //                                              //2. "©XYX"<4>{ <0, '©', u00A9> }.
            //                                              //3. "●XYX"<4>{ <0, '●', u0001> }.
            //                                              //4. "●XYX"<4>{ <0, '●', u0009, \t, Horizontal Tab> }.
            //                                              //str, info. prepared for display.

            String String_I
            )
        {
            String ToLog;
            if (
                String_I == null
                )
            {
                ToLog = "null";
            }
            else
            {
                //                                          //Diagnostic each char.
                //                                          //Will produce arrcharStr and darrstrCharInfo

                //                                          //Will convert str to arrchar only if required.
                char[] arrcharStr = null;

                List<String> darrstrCharInfo = new List<String>();

                for (int intI = 0; intI < String_I.Length; intI = intI + 1)
                {
                    String strChar = Test.ToLog(String_I[intI]);

                    //                                      //Change String if needed.
                    if (
                        //                                  //'x' != (x in the String)
                        strChar[1] != String_I[intI]
                        )
                    {
                        //                                  //Create arrchar if needed
                        arrcharStr = (arrcharStr == null) ? String_I.ToCharArray() : arrcharStr;

                        arrcharStr[intI] = strChar[1];
                    }

                    //                                      //Add diagnotic if needed
                    if (
                        //                                  //Has the form: 'x'<info>
                        strChar.Length > 3
                        )
                    {
                        //                                  //Transform info:
                        //                                  //'x'<info> ==> <position, 'x', info>
                        strChar = "<" + intI + ", " + strChar.Substring(0, 3) + ", " + strChar.Substring(4);

                        darrstrCharInfo.Add(strChar);
                    }
                }

                //                                          //Reduce diagnostic if needed
                if (
                    darrstrCharInfo.Count > Test.intMAX_DIAGNOSTIC
                    )
                {
                    //                                      //Remove exceded info and add "... +N more chars"
                    int intRemove = darrstrCharInfo.Count - Test.intMAX_DIAGNOSTIC;
                    darrstrCharInfo.RemoveRange(Test.intMAX_DIAGNOSTIC, intRemove);
                    darrstrCharInfo.Add("... +" + intRemove + " more chars");
                }

                String strLength = (String_I.Length <= Test.intLONG_STRING) ? "" : "<" + String_I.Length + '>';
                String strStr = '"' + ((arrcharStr == null) ? String_I : new String(arrcharStr)) + '"' + strLength;

                String strInfo = (darrstrCharInfo.Count == 0) ? "" :
                    ("{ " + String.Join(", ", darrstrCharInfo.ToArray()) + " }");

                ToLog = strStr + strInfo;
            }

            return ToLog;
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.Language(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog(
            //                                              //str, info. prepared for display.

            Language language_I,
            String text_I
            )
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            return text_I + "(" + Test.ToLog(language_I) + ")";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog(
            //                                              //str, info. prepared for display.

            Language language_I
            )
        {
            return language_I.ToString();
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.TimeZoneX(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog(
            //                                              //str, info. prepared for display.

            TimeZoneX timezone_I,
            String text_I
            )
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            return text_I + "(" + Test.ToLog(timezone_I) + ")";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog(
            //                                              //str, info. prepared for display.

            TimeZoneX timeZoneXI
            )
        {
            return timeZoneXI.ToString();
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.DaylightSavingTimeInfo(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog(
            //                                              //str, info. prepared for display.

            DaylightSavingTimeInfo daylightSavingTimeInfo_I,
            String text_I
            )
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            return text_I + "(" + Test.ToLog(daylightSavingTimeInfo_I) + ")";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog(
            //                                              //str, info. prepared for display.

            DaylightSavingTimeInfo daylightSavingTimeInfo_I
            )
        {
            return daylightSavingTimeInfo_I.ToText();
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.Date(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog(
            //                                              //str, info. prepared for display.

            Date date_I,
            String text_I
            )
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            return text_I + "(" + Test.ToLog(date_I) + ")";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog(
            //                                              //str, info. prepared for display.

            Date date_I
            )
        {
            String strInfo = (date_I == Date.MinValue) ? "<MIN_DATE>" :
                (date_I == Date.MaxValue) ? "<MAX_DATE>" : "";

            return date_I.ToText() + strInfo;
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.Time(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog(
            //                                              //str, info. prepared for display.

            Time time_I,
            String text_I
            )
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            return text_I + "(" + Test.ToLog(time_I) + ")";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog(
            //                                              //str, info. prepared for display.

            Time time_I
            )
        {
            String strInfo = (time_I == Time.MinValue) ? "<MIN_TIME>" :
                (time_I == Time.MaxValue) ? "<MAX_TIME>" : "";

            return time_I.ToText() + strInfo;
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.ZonedTime(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog(
            //                                              //str, info. prepared for display.

            ZonedTime zonedTime_I,
            String text_I
            )
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            return text_I + "(" + Test.ToLog(zonedTime_I) + ")";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog(
            //                                              //str, info. prepared for display.

            ZonedTime zonedTime_I
            )
        {
            String strInfo = (zonedTime_I == ZonedTime.MinValue) ? "<MIN_TIME>" :
                (zonedTime_I == ZonedTime.MaxValue) ? "<MAX_TIME>" : "";

            return zonedTime_I.ToText() + strInfo;
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.DateTime(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog(
            //                                              //str, info. prepared for display.

            DateTime dt_I,
            String text_I
            )
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            return text_I + "(" + Test.ToLog(dt_I) + ")";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog(
            //                                              //Prepares a date o dt.
            //                                              //Example: 2013-12-28T21:31:25.703-06:00.
            //                                              //str, info. prepared for display.

            DateTime dt_I
            )
        {
            return dt_I.ToString("yyyy-MM-dd HH:mm:ss");
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.Type(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog(
            //                                              //str, info. prepared for display.

            Type type_I,
            String text_I
            )
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            return text_I + "(" + ((type_I == null) ? "null" : "<Name(" + Std.Name(type_I) + ")>") + ")";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog(
            //                                              //str, info. prepared for display.

            Type type_I
            )
        {
            return (type_I == null) ? "null" : "<" + Std.Name(type_I) + ">";
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.PersonName(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog(
            //                                              //str, info. prepared for display.

            PersonName pn_I,
            String text_I
            )
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            return text_I + "(" + Test.ToLog(pn_I) + ")";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog(
            //                                              //str, info. prepared for display.

            PersonName pn_I
            )
        {
            return pn_I.ToText();
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.Exception(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog(
            //                                              //str, info. prepared for display.

            Exception exception_I,
            String text_I
            )
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            return text_I + "(" +
                ((exception_I == null) ? "null" : "<Message(" + exception_I.ToString() + ")>") + ")";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog(
            //                                              //str, info. prepared for display.

            Exception exception_I
            )
        {
            return (exception_I == null) ? "null" : "<" + exception_I.ToString() + ">";
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.Path(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        public static String z_TowaPRIVATE_FullPathAsIsOrMasked(
            //                                              //If it is in the Test Comparable Log.
            //                                              //1. Extract FullPath and make it neutral.
            //                                              //2. Masks if it is in arrstrDirectoryToMask.
            //                                              //Example:
            //                                              //"/Users/glg0818/Desktop/Test SoftwareCompare/Data/Abc.txt"
            //                                              //      and "/Users/glg0818/Desktop" is in
            //                                              //      Test.arrsyspathDirectoryToMask then:
            //                                              //"§§§●Test SoftwareCompare●Data●Abc.txt.
            //                                              //str, full path masked.

            PathX path_I
            )
        {
            String strFullPathAsIsOrMasked = path_I.FullPath;

            if (
                Test.boolIsComparableLog_Z
                )
            {
                //                                          //Mask is required.

                //                                          //Determines if directory has to be masked
                int intX = 0;
                /*REPEAT-UNTIL*/
                while (!(
                    (intX >= Test.arrsyspathDirectoryToMask.Length) ||
                    //                                      //Is included in the array to mask
                    path_I.IsContainedIn(Test.arrsyspathDirectoryToMask[intX])
                    ))
                {
                    intX = intX + 1;
                }

                if (
                    //                                      //Found a directory to mask
                    intX < Test.arrsyspathDirectoryToMask.Length
                    )
                {
                    //                                      //To easy code
                    int intLength = Test.arrsyspathDirectoryToMask[intX].FullPath.Length;

                    strFullPathAsIsOrMasked = Test.strMASK + path_I.FullPath.Substring(intLength);
                }

                strFullPathAsIsOrMasked = strFullPathAsIsOrMasked.Replace(PathX.NETWORK_MARK,
                    PathX.NETWORK_MARK_MASKED);
                strFullPathAsIsOrMasked = strFullPathAsIsOrMasked.Replace(PathX.VOLUME_SEPARATOR_CHAR,
                    PathX.VOLUME_SEPARATOR_CHAR_MASKED);
                strFullPathAsIsOrMasked = strFullPathAsIsOrMasked.Replace(PathX.DIRECTORY_SEPARATOR_CHAR,
                    PathX.DIRECTORY_SEPARATOR_CHAR_MASKED);
            }

            return strFullPathAsIsOrMasked;
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.Directory(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog(
            //                                              //str, info. prepared for display.

            DirectoryInfo directory_I,
            String text_I
            )
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            String strInfo;
            if (
                directory_I == null
                )
            {
                strInfo = "null";
            }
            else
            {
                directory_I.Refresh();

                strInfo = "<FullName(" + Test.strFullPathAsIsOrMasked(directory_I.FullPath()) + "), " +
                    Test.ToLog(directory_I.Exists, "Exists");

                if (
                    directory_I.Exists
                    )
                {
                    strInfo = strInfo + ", " + Test.ToLog(directory_I.CreationTime, "CreationTime") + ", " +
                        Test.ToLog(directory_I.LastAccessTime, "LastAccessTime") + ", " +
                        Test.ToLog(directory_I.LastWriteTime, "LastWriteTime") + ", " +
                        Test.ToLog(directory_I.GetDirectories().Length, "Directories") + ", " +
                        Test.ToLog(directory_I.GetFiles().Length, "Files");
                }

                strInfo = strInfo + ">";

            }

            return text_I + "(" + strInfo + ")";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog(
            //                                              //str, info. prepared for display.

            DirectoryInfo directory_I
            )
        {
            String ToLog;
            if (
                directory_I == null
                )
            {
                ToLog = "null";
            }
            else
            {
                directory_I.Refresh();

                ToLog = "<Name(" + directory_I.Name + "), " + Test.ToLog(directory_I.Exists);

                if (
                    directory_I.Exists
                    )
                {
                    ToLog = ToLog + ", " + Test.ToLog(directory_I.CreationTime) + ", " +
                        Test.ToLog(directory_I.LastAccessTime) + ", " + Test.ToLog(directory_I.LastWriteTime) + ", " +
                        Test.ToLog(directory_I.GetDirectories().Length) + ", " + Test.ToLog(directory_I.GetFiles().Length);
                }

                ToLog = ToLog + ">";
            }

            return ToLog;
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.File(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog(
            //                                              //str, info. prepared for display.

            FileInfo file_I,
            String text_I
            )
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            String strInfo;
            if (
                file_I == null
                )
            {
                strInfo = "null";
            }
            else
            {
                file_I.Refresh();

                strInfo = "<FullName(" + Test.strFullPathAsIsOrMasked(file_I.FullName) + "), " +
                    Test.ToLog(file_I.Exists, "Exists");

                if (
                    file_I.Exists
                    )
                {
                    strInfo = strInfo + ", " + Test.ToLog(file_I.Length, "Length") + ", " +
                        Test.ToLog(file_I.CreationTime, "CreationTime") + ", " +
                        Test.ToLog(file_I.LastAccessTime, "LastAccessTime") + ", " +
                        Test.ToLog(file_I.LastWriteTime, "LastWriteTime");
                }

                strInfo = strInfo + ">";
            }

            return text_I + "(" + strInfo + ")";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog(
            //                                              //str, info. prepared for display.

            FileInfo file_I
            )
        {
            String ToLog;
            if (
                file_I == null
                )
            {
                ToLog = "null";
            }
            else
            {
                file_I.Refresh();

                ToLog = "<Name(" + file_I.Name + "), " + Test.ToLog(file_I.Exists);

                if (
                    file_I.Exists
                    )
                {
                    ToLog = ToLog + ", " + Test.ToLog(file_I.Length) + ", " + Test.ToLog(file_I.CreationTime) +
                        ", " + Test.ToLog(file_I.LastAccessTime) + ", " + Test.ToLog(file_I.LastWriteTime);
                }

                ToLog = ToLog + ">";
            }

            return ToLog;
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.TextReader(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog(
            //                                              //str, info. prepared for display.

            StreamReader textReader_I,
            String text_I
            )
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            return text_I + "(" +
                    ((textReader_I == null) ? "null" :
                        "<CurrentEncoding(" + textReader_I.CurrentEncoding + "), " +
                        Test.ToLog(textReader_I.EndOfStream, "EndOfStream") + ">") +
                    ")";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog(
            //                                              //str, info. prepared for display.

            StreamReader textReader_I
            )
        {
            return
                (textReader_I == null) ? "null" :
                "<" + textReader_I.CurrentEncoding + ", " + Test.ToLog(textReader_I.EndOfStream) + ">";
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.TextWriter(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog(
            //                                              //str, info. prepared for display.

            StreamWriter textWriter_I,
            String text_I
            )
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            return text_I + "(" + ((textWriter_I == null) ? "null" : "<Encoding(" + textWriter_I.Encoding + ")>") + ")";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog(
            //                                              //str, info. prepared for display.

            StreamWriter textWriter_I
            )
        {
            return (textWriter_I == null) ? "null" : "<" + textWriter_I.Encoding + ">";
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.Enum(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog(
            //                                              //str, info. prepared for display.

            Enum enum_I,
            String text_I
            )
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            String strEnum = "" + enum_I;
            strEnum = (strEnum == "Z_NULL") ? "null" : strEnum;

            return text_I +
                "(" + strEnum + ")";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog(
            //                                              //str, info. prepared for display.

            Enum enum_I
            )
        {
            String strEnum = "" + enum_I;

            return (strEnum == "Z_NULL") ? "null" : strEnum;
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.Bsys(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog(
            //                                              //str, info. prepared for display.

            BsysAbstract bsys_I,
            String text_I
            )
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            String strBsys = (bsys_I == null) ? "null" : bsys_I.ToLogFull();

            return text_I + "(" + strBsys + ")";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog(
            //                                              //str, info. prepared for display.

            BsysAbstract bsys_I
            )
        {
            return (bsys_I == null) ? "null" : bsys_I.ToLogShort();
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog(
            //                                              //str, info. prepared for display.

            BsysInterface bsys_I,
            String text_I
            )
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            String strBsys = (bsys_I == null) ? "null" : bsys_I.ToLogFull();

            return text_I + "(" + strBsys + ")";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog(
            //                                              //str, info. prepared for display.

            BsysInterface bsys_I
            )
        {
            return (bsys_I == null) ? "null" : bsys_I.ToLogShort();
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.Btuple(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog(
            //                                              //str, info. prepared for display.

            BtupleAbstract btuple_I,
            String text_I
            )
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            return text_I + "(" + ((btuple_I == null) ? "null" : btuple_I.ToLogFull()) + ")";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog(
            //                                              //str, info. prepared for display.

            BtupleAbstract btuple_I
            )
        {
            return (btuple_I == null) ? "null" : btuple_I.ToLogShort();
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.Bopen(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog(
            //                                              //str, info. prepared for display.

            BopenAbstract bopen_I,
            String text_I
            )
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            return text_I + "(" + ((bopen_I == null) ? "null" : bopen_I.ToLogFull()) + ")";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog(
            //                                              //str, info. prepared for display.

            BopenAbstract bopen_I
            )
        {
            return (bopen_I == null) ? "null" : bopen_I.ToLogShort();
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.Bclass(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog(
            //                                              //str, info. prepared for display.

            BclassAbstract bclass_I,
            String text_I
            )
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            String strToLog;
            /*CASE*/
            if (
                bclass_I == null
                )
            {
                strToLog = text_I + "(null)";
            }
            else if (
                bclass_I.IsDummy
                )
            {
                //                                          //Include only objId + DUMMY
                strToLog = text_I + "(" + Test.GetObjId(bclass_I) + "[DUMMY])";
            }
            else if (
                //                                          //Was processed before
                Test.darrobjPreviouslyProcessed.Contains(bclass_I)
                )
            {
                //                                          //Include only objId
                strToLog = Test.GetObjId(bclass_I) + Test.strMessageLogBefore;
            }
            else
            {
                //                                          //Register as processed
                Test.darrobjPreviouslyProcessed.Add(bclass_I);

                String strNL;
                String strLabel;
                Test.subOpenBlock(out strNL, out strLabel, out strToLog, text_I, "");

                strToLog = strToLog + bclass_I.ToLogFull();

                Test.subCloseBlock(ref strNL, ref strToLog, strLabel);
            }
            /*END-CASE*/

            return strToLog;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog(
            //                                              //str, info. prepared for display.

            BclassAbstract bclass_I
            )
        {
            String strToLog;
            /*CASE*/
            if (
                bclass_I == null
                )
            {
                strToLog = "null";
            }
            else if (
                bclass_I.IsDummy
                )
            {
                //                                          //Include only objId + DUMMY
                strToLog = Test.GetObjId(bclass_I) + "[DUMMY]";
            }
            else if (
                //                                          //Was processed before
                Test.darrobjPreviouslyProcessed.Contains(bclass_I)
                )
            {
                //                                          //Include only objId
                strToLog = Test.GetObjId(bclass_I) + Test.strMessageLogBefore;
            }
            else
            {
                //                                          //Register as processed
                Test.darrobjPreviouslyProcessed.Add(bclass_I);

                strToLog = bclass_I.ToLogShort();
            }
            /*END-CASE*/

            return strToLog;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String GetObjId(
            //                                              //str, Aaaaa:HashCode.

            BclassAbstract bclass_I
            )
        {
            Test.AbortIfNull(bclass_I, "bclass_I");

            //                                              //Subtract class prefix (aaaaa).
            String strPrefix = Test.strPrefixEnumOrBclass(bclass_I.GetType());

            String strHashCode = (Test.boolIsComparableLog_Z) ? Test.strMASK : "" + bclass_I.GetHashCode();

            return strPrefix + ":" + strHashCode;
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.Array(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //bytes is an array of bytes (byte IS NOT a standard
        //                                                  //      primitive) is USED ONLY for serialization.

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog(

            byte[] bytes_I,
            String text_I,
            LogArrOptionEnum logOption_I
            )
        {
            Test.Abort(Test.ToLog(logOption_I, "") + " no log option is allow in bytes");

            return null;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog(

            byte[] bytes_I,
            String text_I
            )
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            return text_I + "(" + Test.ToLog(bytes_I) + ")";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog(

            byte[] bytes_I
            )
        {
            return (bytes_I == null) ? "null" : Test.strFormatBytes(bytes_I);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static String strFormatBytes(
            //                                              //Form grupos of 8, last one 1 to 8.
            //                                              //str, {a,b,c,d,e,f,g,h}, ...., (x,y,z}

            byte[] bytes_I
            )
        {
            int intSetsOf8 = (bytes_I.Length + 7) / 8;

            //                                              //Format each segment of 8 bytes (or less in last segment)
            String[] arrstrPart = new String[intSetsOf8];
            for (int intX = 0; intX < arrstrPart.Length; intX = intX + 1)
            {
                arrstrPart[intX] = Test.strFormatPart(bytes_I, intX);
            }

            return String.Join(", ", arrstrPart);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static String strFormatPart(
            //                                              //Form one segment of 8 bytes  (or less in last segment)
            //                                              //str, {a,b,c,d,e,f,g,h}, ...., (x,y,z}

            byte[] bytes_I,
            //                                              //Part (index) to process
            int intPart_I
            )
        {
            int intLengthOnePart = Std.MinOf(bytes_I.Length - intPart_I * 8, 8);

            //                                              //To easy code
            byte[] bytesOnePart = new byte[intLengthOnePart];
            Array.Copy(bytes_I, intPart_I * 8, bytesOnePart, 0, intLengthOnePart);

            //                                              //Format one segment
            String[] arrstrOnePart = new String[intLengthOnePart];
            for (int intI = 0; intI < intLengthOnePart; intI = intI + 1)
            {
                arrstrOnePart[intI] = bytesOnePart[intI].ToString();
            }

            return "{" + String.Join(",", arrstrOnePart) + "}";
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog<XT>(

            XT[] array_I,
            String text_I
            )
        {
            LogArrOptionEnum logOption = (
                (array_I != null) &&
                //                                          //These type of array need to be VERTICAL
                Test.boolShouldDispleyVertical(array_I.GetType().GetElementType())
                ) ?
                LogArrOptionEnum.VERTICAL : LogArrOptionEnum.HORIZONTAL;

            return Test.ToLog<XT>(array_I, text_I, logOption);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog<XT>(

            XT[] array_I,
            String text_I,
            LogArrOptionEnum logOption_I
            )
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            String strToLog;
            if (
                array_I == null
                )
            {
                strToLog = text_I + "(null)";
            }
            else
            {
                Type typeItem = array_I.GetType().GetElementType();

                Test.subAbortIfNonStandardSingle(typeItem);

                LogArrOptionEnum logOption = logOption_I;
                if (
                    (logOption_I == LogArrOptionEnum.HORIZONTAL) &&
                    //                                          //These type of collection need to display VERTICAL
                    Test.boolShouldDispleyVertical(typeItem)
                    )
                {
                    Test.Warning(Test.ToLog(array_I.GetType(), "array_I.Type") +
                        " will be displayed VERTICAL, to avoid this warning, " +
                        "please change HORIZONTAL log option in the ToLog method call",
                        Test.ToLog(logOption_I, "logOption_I"));

                    logOption = LogArrOptionEnum.VERTICAL;
                }

                strToLog = Test.strFormatArrayOrCollection(array_I, array_I, text_I, Test.strObjIdGet(array_I),
                    logOption);
            }

            return strToLog;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog<XT>(

            XT[] array_I
            )
        {
            return (array_I == null) ? "null" : Test.strObjIdGet(array_I);
        }

        //--------------------------------------------------------------------------------------------------------------
        private static String strObjIdGet<XT>(
            //                                              //str, arrxxx«Size»:HashCode.

            XT[] arrxt_I
            )
        {
            Test.AbortIfNull(arrxt_I, "arrxt_I");

            String strHashCode = (Test.boolIsComparableLog_Z) ? Test.strMASK : "" + arrxt_I.GetHashCode();

            return "arr" + Test.GetPrefixFromType(typeof(XT)) + "«" + arrxt_I.Length + "»:" + strHashCode;
        }

        //--------------------------------------------------------------------------------------------------------------
        /*SUPPORT METHODS*/

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static String strFormatArrayOrCollection<XT>(
            //                                              //str, collection info.

            XT[] arrxt_I,
            Object objCollection_I,
            String text_I,
            String strObjId_I,
            LogArrOptionEnum logOption_I
            )
        {
            String strFormatCollection;
            if (
                Test.darrobjPreviouslyProcessed.Contains(objCollection_I)
                )
            {
                strFormatCollection = text_I + "(" + strObjId_I + strMessageLogBefore + ")";
            }
            else
            {
                //                                          //Register collection as processed
                Test.darrobjPreviouslyProcessed.Add(objCollection_I);

                if (
                    logOption_I == LogArrOptionEnum.VERTICAL
                    )
                {
                    String strNL;
                    String strLabel;
                    Test.subOpenBlock(out strNL, out strLabel, out strFormatCollection, text_I, strObjId_I);

                    strFormatCollection = strFormatCollection + Test.strArrayOfItems(arrxt_I, strNL);

                    Test.subCloseBlock(ref strNL, ref strFormatCollection, strLabel);
                }
                else
                {
                    strFormatCollection = text_I + "(" + strObjId_I + Test.strLineRow(arrxt_I) + ")";
                }
            }

            return strFormatCollection;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static String strArrayOfItems<XT>(
            //                                              //Format an array to a Set of Lines(Items) inside a block.
            //                                              //Example:
            //                                              //[
            //                                              //{
            //                                              //[0] item
            //                                              //...
            //                                              //[x] item
            //                                              //}
            //                                              //]

            //                                              //str, set in block format

            XT[] arrxt_I,
            String strNL_I
            )
        {
            //                                              //Chars required for longest index: "[x]"
            int intCharsInLongestIndex = ("[" + (arrxt_I.Length - 1) + "]").Length;

            //                                              //Produces a Set of Lines(Items) ready to display.
            String[] arrstrIndexAndItem = new String[arrxt_I.Length];
            for (int intI = 0; intI < arrxt_I.Length; intI = intI + 1)
            {
                //                                          //Format: NL [i]_ item
                arrstrIndexAndItem[intI] = strNL_I + ("[" + intI + "]").PadRight(intCharsInLongestIndex) + " " +
                    Test.z_TowaPRIVATE_ToLogXT(arrxt_I[intI]);
            }

            return strNL_I + "{" + String.Join("", arrstrIndexAndItem) + strNL_I + "}";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static String strLineRow<XT>(
            //                                              //Produces:
            //                                              //{ item, ..., item }.
            //                                              //str, arr in one line format.

            XT[] arrxt_I
            )
        {
            //                                              //Convert arrobj to arrstr
            String[] arrstrItem = new String[arrxt_I.Length];
            for (int intI = 0; intI < arrxt_I.Length; intI = intI + 1)
            {
                arrstrItem[intI] = Test.z_TowaPRIVATE_ToLogXT(arrxt_I[intI]);
            }

            //                                              //Format: { item, item, ..., item }
            return Test.strVectorFromSet(arrstrItem);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static String strVectorFromSet(
            //                                              //Produces:
            //                                              //{ stuff, ..., stuff }.
            //                                              //Posibilities:
            //                                              //Put a set of strItem in a vector (strRow).
            //                                              //Put a set of strRow in a vector (strMatrix).
            //                                              //Put a set of strMatrix in a vector (strCube).

            //                                              //str, vector format.

            //                                              //Stuff to be included in strVector.
            String[] arrstrStuff_I
            )
        {
            String strRowFormatBeforeAddingBrackets = (arrstrStuff_I.Length == 0) ? " " :
                " " + String.Join(", ", arrstrStuff_I) + " ";

            return "{" + strRowFormatBeforeAddingBrackets + "}";
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.DynamicArray(ToLog)*/
        //==============================================================================================================
        //                                                  //Generic collections (Dynamic Array, Linked List, etc.)
        //                                                  //      could not be processes as ICollection.
        //                                                  //The problem was, (Dynamic Array) List<int> can be
        //                                                  //      processes with generic method declaring List<XT>,
        //                                                  //      but ICollection<XT>.
        //                                                  //This problem do not exist with collection of non
        //                                                  //      primitives.

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog<XT>(

            List<XT> dynamicArray_I,
            String text_I
            )
        {
            LogArrOptionEnum logOption = (
                (dynamicArray_I != null) &&
                //                                          //These type of dynamic array need to be VERTICAL
                Test.boolShouldDispleyVertical(dynamicArray_I.GetType().GetGenericArguments()[0])
                ) ?
                LogArrOptionEnum.VERTICAL : LogArrOptionEnum.HORIZONTAL;

            return Test.ToLog<XT>(dynamicArray_I, text_I, logOption);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog<XT>(

            List<XT> dynamicArray_I,
            String text_I,
            LogArrOptionEnum logOption_I
            )
        {
            if (
                dynamicArray_I != null
                )
            {
                Test.subAbortIfNonStandardSingle(dynamicArray_I.GetType().GetGenericArguments()[0]);
            }
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            return Test.strToLogCollection(dynamicArray_I, text_I, logOption_I);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog<XT>(

            List<XT> dynamicArray_I
            )
        {
            if (
                dynamicArray_I != null
                )
            {
                Test.subAbortIfNonStandardSingle(dynamicArray_I.GetType().GetGenericArguments()[0]);
            }

            return Test.strToLogCollection(dynamicArray_I);
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.LinkedList(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog<XT>(

            LinkedList<XT> linkedList_I,
            String text_I
            )
        {
            LogArrOptionEnum logOption = (
                (linkedList_I != null) &&
                //                                          //These type of dynamic array need to be VERTICAL
                Test.boolShouldDispleyVertical(linkedList_I.GetType().GetGenericArguments()[0])
                ) ?
                LogArrOptionEnum.VERTICAL : LogArrOptionEnum.HORIZONTAL;

            return Test.ToLog<XT>(linkedList_I, text_I, logOption);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog<XT>(

            LinkedList<XT> linkedList_I,
            String text_I,
            LogArrOptionEnum logOption_I
            )
        {
            if (
                linkedList_I != null
                )
            {
                Test.subAbortIfNonStandardSingle(linkedList_I.GetType().GetGenericArguments()[0]);
            }
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            return Test.strToLogCollection(linkedList_I, text_I, logOption_I);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog<XT>(

            LinkedList<XT> linkedList_I
            )
        {
            if (
                linkedList_I != null
                )
            {
                Test.subAbortIfNonStandardSingle(linkedList_I.GetType().GetGenericArguments()[0]);
            }

            return Test.strToLogCollection(linkedList_I);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog<XT>(

            LinkedListNode<XT> node_I,
            String text_I
            )
        {
            String strNode;
            if (
                node_I == null
                )
            {
                strNode = "null";
            }
            else
            {
                String strPrevious = (node_I.Previous == null)
                    ? Test.ToLog(node_I.Previous, "Previous") :
                    Test.z_TowaPRIVATE_ToLogXT(node_I.Previous.Value, "Previous.Value");
                String strNext = (node_I.Next == null)
                    ? Test.ToLog(node_I.Next, "Next") : Test.z_TowaPRIVATE_ToLogXT(node_I.Next.Value, "Next.Value");

                strNode = "<" + Test.z_TowaPRIVATE_ToLogXT(node_I.Value, "value") + ", " + strPrevious + ", " +
                    strNext + ">";
            }

            return text_I + "(" + strNode + ")";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog<XT>(

            LinkedListNode<XT> node_I
            )
        {
            String strToLog;
            if (
                node_I == null
                )
            {
                strToLog = "null";
            }
            else
            {
                String strPrevious = (node_I.Previous == null)
                    ? Test.ToLog(node_I.Previous) : Test.z_TowaPRIVATE_ToLogXT(node_I.Previous.Value);
                String strNext = (node_I.Next == null)
                    ? Test.ToLog(node_I.Next) : Test.z_TowaPRIVATE_ToLogXT(node_I.Next.Value);

                strToLog = "<" + Test.z_TowaPRIVATE_ToLogXT(node_I.Value) + ", " + strPrevious + ", " + strNext + ">";
            }

            return strToLog;
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.Stack(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog<XT>(

            Stack<XT> stack_I,
            String text_I
            )
        {
            LogArrOptionEnum logOption = (
                (stack_I != null) &&
                //                                          //These type of dynamic array need to be VERTICAL
                Test.boolShouldDispleyVertical(stack_I.GetType().GetGenericArguments()[0])
                ) ?
                LogArrOptionEnum.VERTICAL : LogArrOptionEnum.HORIZONTAL;

            return Test.ToLog<XT>(stack_I, text_I, logOption);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog<XT>(

            Stack<XT> stack_I,
            String text_I,
            LogArrOptionEnum logOption_I
            )
        {
            if (
                stack_I != null
                )
            {
                Test.subAbortIfNonStandardSingle(stack_I.GetType().GetGenericArguments()[0]);
            }
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            return Test.strToLogCollection(stack_I, text_I, logOption_I);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog<XT>(

            Stack<XT> stack_I
            )
        {
            if (
                stack_I != null
                )
            {
                Test.subAbortIfNonStandardSingle(stack_I.GetType().GetGenericArguments()[0]);
            }

            return Test.strToLogCollection(stack_I);
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.Queue(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog<XT>(

            Queue<XT> queue_I,
            String text_I
            )
        {
            LogArrOptionEnum logOption = (
                (queue_I != null) &&
                //                                          //These type of dynamic array need to be VERTICAL
                Test.boolShouldDispleyVertical(queue_I.GetType().GetGenericArguments()[0])
                ) ?
                LogArrOptionEnum.VERTICAL : LogArrOptionEnum.HORIZONTAL;

            return Test.ToLog<XT>(queue_I, text_I, logOption);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog<XT>(

            Queue<XT> queue_I,
            String text_I,
            LogArrOptionEnum logOption_I
            )
        {
            if (
                queue_I != null
                )
            {
                Test.subAbortIfNonStandardSingle(queue_I.GetType().GetGenericArguments()[0]);
            }
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            return Test.strToLogCollection(queue_I, text_I, logOption_I);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog<XT>(

            Queue<XT> queue_I
            )
        {
            if (
                queue_I != null
                )
            {
                Test.subAbortIfNonStandardSingle(queue_I.GetType().GetGenericArguments()[0]);
            }

            return Test.strToLogCollection(queue_I);
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.IEnumerable(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        private static String strToLogCollection<XT>(

            IEnumerable<XT> ienumxt_I,
            String text_I,
            LogArrOptionEnum logOption_I
            )
        {
            String strToLogCollection;
            if (
                ienumxt_I == null
                )
            {
                strToLogCollection = text_I + "(null)";
            }
            else
            {
                LogArrOptionEnum logOption = logOption_I;
                if (
                    (logOption_I == LogArrOptionEnum.HORIZONTAL) &&
                    //                                      //This type of ienum need to display VERTICAL
                    Test.boolShouldDispleyVertical(ienumxt_I.GetType().GetGenericArguments()[0])
                    )
                {
                    Test.Warning(Test.ToLog(ienumxt_I.GetType(), "ienumxt_I.Type") +
                        " will be displayed VERTICAL, to avoid this warning, " +
                        "please change HORIZONTAL log option in the ToLog method call",
                        Test.ToLog(logOption_I, "logOption_I"));

                    logOption = LogArrOptionEnum.VERTICAL;
                }

                XT[] arrxt = ienumxt_I.ToArray();
                String strObjId = Test.strGetObjIdCollection(ienumxt_I);
                strToLogCollection = Test.strFormatArrayOrCollection(arrxt, ienumxt_I, text_I, strObjId, logOption);
            }

            return strToLogCollection;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static String strToLogCollection<XT>(

            IEnumerable<XT> ienumxt_I
            )
        {
            return (ienumxt_I == null) ? "null" : Test.strGetObjIdCollection(ienumxt_I);
        }

        //--------------------------------------------------------------------------------------------------------------
        private static String strGetObjIdCollection<XT>(
            //                                              //str, ienumxxx«Size»:HashCode.

            IEnumerable<XT> ienumxt_I
            )
        {
            Test.AbortIfNull(ienumxt_I, "ienumxt_I");

            String strHashCode = (Test.boolIsComparableLog_Z) ? Test.strMASK : "" + ienumxt_I.GetHashCode();

            /*
            String strTypeNameIenum = ienumxt_I.GetType().Name;
            String strPrefixIenum = (strTypeNameIenum == "List`1") ? "darr" :
                (strTypeNameIenum == "LinkedList`1") ? "lnkl" :
                (strTypeNameIenum == "Stack`1") ? "stack" :
                (strTypeNameIenum == "Queue`1") ? "queue" : null;
            if (
                    strPrefixIenum == null
                )
                Test.Abort("SOMETHING IS WRONG!!!, IEnumerable(" + strTypeNameIenum + ") not implemented");

            return strPrefixIenum + Test.strPrefixSingleType(typeof(XT)) + "«" + ienumxt_I.Count() + "»:" + strHashCode;
            */
            return Test.GetPrefixFromType(ienumxt_I.GetType()) + "«" + ienumxt_I.Count() + "»:" + strHashCode;
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void subAbortIfNonStandardSingle(
            Type type_L
            )
        {
            if (!(
                typeof(BobjAbstract).IsAssignableFrom(type_L) ||
                typeof(BsysInterface).IsAssignableFrom(type_L) ||
                typeof(Enum).IsAssignableFrom(type_L) ||
                (type_L == typeof(int)) || (type_L == typeof(long)) || (type_L == typeof(double)) ||
                (type_L == typeof(char)) || (type_L == typeof(bool)) || (type_L == typeof(String)) ||
                (type_L == typeof(Type)) || (type_L == typeof(DateTime)) ||
                //                                          //PathX is included in bobj
                (type_L == typeof(DirectoryInfo)) || (type_L == typeof(FileInfo)) ||
                (type_L == typeof(StreamReader)) || (type_L == typeof(StreamWriter))
                ))
                Test.Abort("SOMETHING IS WRONG!!!, type_L.Name(" + type_L.Name + ") not implemented");
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.Dictionary(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog<TValue>(

            Dictionary<String, TValue> Dictionary_I,
            String text_I
            )
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            String ToLog;
            /*CASE*/
            if (
                Dictionary_I == null
                )
            {
                ToLog = text_I + "(null)";
            }
            else if (
                Test.darrobjPreviouslyProcessed.Contains(Dictionary_I)
                )
            {
                ToLog = text_I + "(" + Test.strObjIdGet(Dictionary_I) + strMessageLogBefore + ")";
            }
            else
            {
                //                                          //Register dic as processed
                Test.darrobjPreviouslyProcessed.Add(Dictionary_I);

                //                                          //Convert to Directory NEUTRAL
                Dictionary<String, TValue> dicvalueNeutral = new Dictionary<string, TValue>();
                foreach (KeyValuePair<String, TValue> kvp in Dictionary_I)
                {
                    String strKey = Test.strFormatKey(kvp.Key);
                    dicvalueNeutral.Add(strKey, kvp.Value);
                }

                ToLog = Test.strFormatDictionary(Dictionary_I, text_I, Test.strObjIdGet(Dictionary_I));
            }
            /*END-CASE*/

            return ToLog;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog<TValue>(

            Dictionary<String, TValue> Dictionary_I
            )
        {
            return (Dictionary_I == null) ? "null" : Test.strObjIdGet(Dictionary_I);
        }

        //--------------------------------------------------------------------------------------------------------------
        private static String strObjIdGet<TValue>(
            //                                              //str, dicxxx«Size»:HashCode.

            Dictionary<String, TValue> dicvalue_I
            )
        {
            Test.AbortIfNull(dicvalue_I, "dicvalue_I");

            String strHashCode = (Test.boolIsComparableLog_Z) ? Test.strMASK : "" + dicvalue_I.GetHashCode();

            return Test.GetPrefixFromType(dicvalue_I.GetType()) + "«" + dicvalue_I.Count + "»:" + strHashCode;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog<TValue>(

            KeyValuePair<String, TValue> keyValuePair_I,
            String text_I
            )
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            return text_I + "(<Key(" + Test.strFormatKey(keyValuePair_I.Key) + "), Value(" +
                Test.z_TowaPRIVATE_ToLogXT(keyValuePair_I.Value) + ")>)";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog<TValue>(

            KeyValuePair<String, TValue> keyValuePair_I
            )
        {
            return "<" + Test.strFormatKey(keyValuePair_I.Key) + ", " +
                Test.z_TowaPRIVATE_ToLogXT(keyValuePair_I.Value) + ">";
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.KeyIndex(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog<TKey, TValue>(

            Dictionary<EntityKey<TKey>, TValue> KeyIndex_I,
            String text_I
            )
            where TKey : IComparable /*String, int, long, char, Date or Time*/
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            String ToLog;
            /*CASE*/
            if (
                KeyIndex_I == null
                )
            {
                ToLog = text_I + "(null)";
            }
            else if (
                Test.darrobjPreviouslyProcessed.Contains(KeyIndex_I)
                )
            {
                ToLog = text_I + "(" + Test.strObjIdGet(KeyIndex_I) + strMessageLogBefore + ")";
            }
            else
            {
                //                                          //Register dic as processed
                Test.darrobjPreviouslyProcessed.Add(KeyIndex_I);

                //                                          //Convert to Directory NEUTRAL
                Dictionary<String, TValue> dicvalueNeutral = new Dictionary<string, TValue>();
                foreach (KeyValuePair<EntityKey<TKey>, TValue> kvp in KeyIndex_I)
                {
                    String strKey = kvp.Key.ToText();
                    dicvalueNeutral.Add(strKey, kvp.Value);
                }

                ToLog = Test.strFormatDictionary(dicvalueNeutral, text_I, Test.strObjIdGet(KeyIndex_I));
            }
            /*END-CASE*/

            return ToLog;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog<TKey, TValue>(

            Dictionary<EntityKey<TKey>, TValue> KeyIndex_I
            )
            where TKey : IComparable /*String, int, long, char, Date or Time*/
        {
            return (KeyIndex_I == null) ? "null" : Test.strObjIdGet(KeyIndex_I);
        }

        //--------------------------------------------------------------------------------------------------------------
        private static String strObjIdGet<TKey, TValue>(
            //                                              //str, dicxxx«Size»:HashCode.

            Dictionary<EntityKey<TKey>, TValue> ikeyvalue_I
            )
            where TKey : IComparable /*String, int, long, char, Date or Time*/
        {
            Test.AbortIfNull(ikeyvalue_I, "ikeyvalue_I");

            String strHashCode = (Test.boolIsComparableLog_Z) ? Test.strMASK : "" + ikeyvalue_I.GetHashCode();

            return Test.GetPrefixFromType(ikeyvalue_I.GetType()) + "«" + ikeyvalue_I.Count + "»:" + strHashCode;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog<TKey, TValue>(

            KeyValuePair<EntityKey<TKey>, TValue> keyValuePair_I,
            String text_I
            )
            where TKey : IComparable /*String, int, long, char, Date or Time*/
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            return text_I + "(<Key(" + keyValuePair_I.Key.ToText() + "), Value(" +
                Test.z_TowaPRIVATE_ToLogXT(keyValuePair_I.Value) + ")>)";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog<TKey, TValue>(

            KeyValuePair<EntityKey<TKey>, TValue> keyValuePair_I
            )
            where TKey : IComparable /*String, int, long, char, Date or Time*/
        {
            return "<" + keyValuePair_I.Key.ToText() + ", " + Test.z_TowaPRIVATE_ToLogXT(keyValuePair_I.Value) + ">";
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.Index(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog<TValue>(

            Dictionary<long, TValue> index_I,
            String text_I
            )
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            String ToLog;
            /*CASE*/
            if (
                index_I == null
                )
            {
                ToLog = text_I + "(null)";
            }
            else if (
                Test.darrobjPreviouslyProcessed.Contains(index_I)
                )
            {
                ToLog = text_I + "(" + Test.strObjIdGet(index_I) + strMessageLogBefore + ")";
            }
            else
            {
                //                                          //Register dic as processed
                Test.darrobjPreviouslyProcessed.Add(index_I);

                //                                          //Convert to Directory NEUTRAL
                Dictionary<String, TValue> dicxtNeutral = new Dictionary<string, TValue>();
                foreach (KeyValuePair<long, TValue> kvp in index_I)
                {
                    String strKey = kvp.Key.ToText();
                    dicxtNeutral.Add(strKey, kvp.Value);
                }

                ToLog = Test.strFormatDictionary(dicxtNeutral, text_I, Test.strObjIdGet(index_I));
            }
            /*END-CASE*/

            return ToLog;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog<TValue>(

            Dictionary<long, TValue> index_I
            )
        {
            return (index_I == null) ? "null" : Test.strObjIdGet(index_I);
        }

        //--------------------------------------------------------------------------------------------------------------
        private static String strObjIdGet<TValue>(
            //                                              //str, dicxxx«Size»:HashCode.

            Dictionary<long, TValue> idxvalue_I
            )
        {
            Test.AbortIfNull(idxvalue_I, "idxvalue_I");

            String strHashCode = (Test.boolIsComparableLog_Z) ? Test.strMASK : "" + idxvalue_I.GetHashCode();

            return Test.GetPrefixFromType(idxvalue_I.GetType()) + "«" + idxvalue_I.Count + "»:" + strHashCode;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog<TValue>(

            KeyValuePair<long, TValue> keyValuePair_I,
            String text_I
            )
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            return text_I + "(<Key(" + keyValuePair_I.Key.ToText() + "), Value(" +
                Test.z_TowaPRIVATE_ToLogXT(keyValuePair_I.Value) + ")>)";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog<TValue>(

            KeyValuePair<long, TValue> keyValuePair_I
            )
        {
            return "<" + keyValuePair_I.Key.ToText() + ", " + Test.z_TowaPRIVATE_ToLogXT(keyValuePair_I.Value) + ">";
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.Dictionary,etc.(shared)(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        /*CONSTANTS*/

        //                                                  //This will be the maximun space reseved for key when LogTo
        //                                                  //      display a dictionary, if we have longhest key the
        //                                                  //      content will not be aligned.
        private const int intKEY_LEN_MAX = 50;

        //--------------------------------------------------------------------------------------------------------------
        private static String strFormatDictionary<TValue>(
            //                                              //str, dictionary info.

            Dictionary<String, TValue> dicxtNeutral_I,
            String text_I,
            String strObjId_I
            )
        {
            //                                              //Separate keys and value and sort by key
            String[] arrstrKey = new String[dicxtNeutral_I.Count];
            dicxtNeutral_I.Keys.CopyTo(arrstrKey, 0);
            TValue[] arrvalue = new TValue[dicxtNeutral_I.Count];
            dicxtNeutral_I.Values.CopyTo(arrvalue, 0);
            Std.Sort(arrstrKey, arrvalue);

            //                                              //Compute [key] size.
            int intLonghestKey = 0;
            foreach (String str in arrstrKey)
            {
                intLonghestKey = Std.MaxOf(intLonghestKey, str.Length);
            }
            intLonghestKey = intLonghestKey + "[]".Length;
            intLonghestKey = Std.MinOf(Test.intKEY_LEN_MAX, intLonghestKey);

            String strFormatDictionary;
            String strNL;
            String strLabel;
            Test.subOpenBlock(out strNL, out strLabel, out strFormatDictionary, text_I, strObjId_I);

            //                                              //Produces lines to include in block.
            String[] arrstrEntry = new String[arrvalue.Length];
            for (int intI = 0; intI < arrvalue.Length; intI = intI + 1)
            {
                arrstrEntry[intI] = ("[" + Test.strFormatKey(arrstrKey[intI]) + "]").PadRight(intLonghestKey) + " " +
                    Test.z_TowaPRIVATE_ToLogXT(arrvalue[intI]);
            }

            strFormatDictionary = strFormatDictionary + "{" + strNL + String.Join(strNL, arrstrEntry) + strNL + "}";

            Test.subCloseBlock(ref strNL, ref strFormatDictionary, strLabel);

            return strFormatDictionary;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static String strFormatKey(
            //                                              //str, key to display. 
            String strKey_I
            )
        {
            String strKeyAnalized = Test.ToLog(strKey_I);

            String strFormatKeyX;
            /*CASE*/
            if (
                //                                          //Has just String info ("String information")
                strKeyAnalized.EndsWith("\"", StringComparison.Ordinal)
                )
            {
                //                                          //Take key from "key"
                strFormatKeyX = strKeyAnalized.Substring(1, strKeyAnalized.Length - 2);
            }
            else if (
                //                                          //Has lenght info and no diagnotic info ("String info"<nn>)
                strKeyAnalized.EndsWith(">", StringComparison.Ordinal)
                )
            {
                int intMinorThanMark = strKeyAnalized.LastIndexOf('<');

                //                                          //Take key from "key"<nn>
                strFormatKeyX = strKeyAnalized.Substring(1, intMinorThanMark - 2);
            }
            else
            {
                //                                          //Include disgnostic info (or is null).
                strFormatKeyX = strKeyAnalized;
            }
            /*END-CASE*/

            return strFormatKeyX;
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.CombinationIndexN(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog(
            //                                              //str, info. prepared for display.

            CombinationIndex2 cidx_I,
            String text_I
            )
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            return text_I + "(" + Test.ToLog(cidx_I) + ")";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog(
            //                                              //str, info. prepared for display.

            CombinationIndex2 cidx_I
            )
        {
            return (cidx_I.CompareTo(default(CombinationIndex2)) == 0) ? "null" : cidx_I.ToLogShort();
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog(
            //                                              //str, info. prepared for display.

            CombinationIndex3 cidx_I,
            String text_I
            )
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            return text_I + "(" + Test.ToLog(cidx_I) + ")";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog(
            //                                              //str, info. prepared for display.

            CombinationIndex3 cidx_I
            )
        {
            return (cidx_I.CompareTo(default(CombinationIndex3)) == 0) ? "null" : cidx_I.ToLogShort();
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog(
            //                                              //str, info. prepared for display.

            CombinationIndex4 cidx_I,
            String text_I
            )
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            return text_I + "(" + Test.ToLog(cidx_I) + ")";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog(
            //                                              //str, info. prepared for display.

            CombinationIndex4 cidx_I
            )
        {
            return (cidx_I.CompareTo(default(CombinationIndex4)) == 0) ? "null" : cidx_I.ToLogShort();
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.Matrix(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog<XT>(

            XT[,] matrix_I,
            String text_I
            )
        {
            LogArr2OptionEnum logOption = (
                //                                          //These type of array need to be VERTICAL
                Test.boolShouldDispleyVertical(matrix_I.GetType().GetElementType())
                ) ?
                LogArr2OptionEnum.VERTICAL :
                LogArr2OptionEnum.MATRIX;

            return Test.ToLog(matrix_I, text_I, logOption);

        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog<XT>(

            XT[,] matrix_I,
            String text_I,
            LogArr2OptionEnum logOption_I
            )
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            LogArr2OptionEnum logOption = logOption_I;
            if (
                (logOption_I == LogArr2OptionEnum.HORIZONTAL) &&
                //                                          //These types of collection need to be displayed VERTICALY
                Test.boolShouldDispleyVertical(matrix_I.GetType().GetElementType())
                )
            {
                Test.Warning(Test.ToLog(matrix_I.GetType(), "arr2xt_I.Type") +
                    " will be displayed VERTICAL, to avoid this warning, please change option in the code",
                    Test.ToLog(logOption_I, "logOption_I"));

                logOption = LogArr2OptionEnum.VERTICAL;
            }

            String strArr2t = (matrix_I == null) ? "null" : Test.strToLogMatrix(matrix_I, text_I, logOption);

            return strArr2t;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog<XT>(

            XT[,] matrix_I
            )
        {
            return (matrix_I == null) ? "null" : Test.strObjIdGet(matrix_I);
        }

        //--------------------------------------------------------------------------------------------------------------
        private static String strObjIdGet<XT>(
            //                                              //str, arr2xxx«Size0, Size1»:HashCode.

            XT[,] arr2xt_I
            )
        {
            Test.AbortIfNull(arr2xt_I, "arr2xt_I");

            String strSize0 = "" + arr2xt_I.GetLength(0);
            String strSize1 = (strSize0 == "0") ? "0" : "" + arr2xt_I.GetLength(1);

            String strHashCode = (Test.boolIsComparableLog_Z) ? Test.strMASK : "" + arr2xt_I.GetHashCode();

            /*
            return "arr2" + Test.strPrefixSingleType(typeof(XT)) + "«" + strSize0 + "," + strSize1 + "»:" + strHashCode;
            */
            return "arr2" + Test.GetPrefixFromType(typeof(XT)) + "«" + strSize0 + "," + strSize1 + "»:" + strHashCode;
        }

        //--------------------------------------------------------------------------------------------------------------
        private static String strToLogMatrix<XT>(
            //                                              //str, arr2xxx«Size0, Size1»:HashCode.

            XT[,] arr2xt_I,
            String text_I,
            LogArr2OptionEnum logOption_I
            )
        {
            String strObjId_I = Test.strObjIdGet(arr2xt_I);

            String strFormatMatrix;
            if (
                Test.darrobjPreviouslyProcessed.Contains(arr2xt_I)
                )
            {
                strFormatMatrix = text_I + "(" + strObjId_I + strMessageLogBefore + ")";
            }
            else
            {
                //                                          //Register matrix as processed
                Test.darrobjPreviouslyProcessed.Add(arr2xt_I);

                /*CASE*/
                if (
                    logOption_I == LogArr2OptionEnum.VERTICAL
                    )
                {
                    strFormatMatrix = Test.strRowMatrix(arr2xt_I, text_I, strObjId_I);
                }
                else if (
                    logOption_I == LogArr2OptionEnum.MATRIX
                    )
                {
                    String strNL;
                    String strLabel;
                    Test.subOpenBlock(out strNL, out strLabel, out strFormatMatrix, text_I, strObjId_I);

                    strFormatMatrix = strFormatMatrix + Test.strRowLineMatrix(arr2xt_I, strNL);

                    Test.subCloseBlock(ref strNL, ref strFormatMatrix, strLabel);
                }
                else
                {
                    strFormatMatrix = text_I + "(" + strObjId_I + Test.strLineMatrix(arr2xt_I) + ")";
                }
                /*END-CASE*/
            }

            return strFormatMatrix;
        }

        //--------------------------------------------------------------------------------------------------------------
        private static String strRowMatrix<XT>(
            //                                              //Format an array to a Set of Lines(Items) inside a block.
            //                                              //Example:
            //                                              //[
            //                                              //{
            //                                              //[0] item
            //                                              //...
            //                                              //[x] item
            //                                              //}
            //                                              //]

            //                                              //str, set in block format

            XT[,] arr2xt_I,
            String text_I,
            String strObjId_I
            )
        {

            String strFormatMatrix;

            String strNL;
            String strLabel;
            Test.subOpenBlock(out strNL, out strLabel, out strFormatMatrix, text_I, strObjId_I);

            int intCharsInLongestIndex = ("[" + (arr2xt_I.GetLength(0) - 1) + "," + (arr2xt_I.GetLength(1) - 1) + "]").Length;

            strFormatMatrix = strFormatMatrix + "{";

            for (int intI = 0; intI < arr2xt_I.GetLength(0); intI = intI + 1)
            {
                String[] arrstrIndexAndItem = new String[arr2xt_I.GetLength(1)];

                for (int intJ = 0; intJ < arr2xt_I.GetLength(1); intJ = intJ + 1)
                {
                    //                                          //Format: NL [i,j]_ item
                    arrstrIndexAndItem[intJ] = strNL +
                        ("[" + intI + "," + intJ + "]").PadRight(intCharsInLongestIndex) + " " +
                        Test.z_TowaPRIVATE_ToLogXT(arr2xt_I[intI, intJ]);
                }

                strFormatMatrix = strFormatMatrix + strNL + "{" + String.Join("", arrstrIndexAndItem) + strNL + "}";
            }

            strFormatMatrix = strFormatMatrix + "}";

            Test.subCloseBlock(ref strNL, ref strFormatMatrix, strLabel);

            return strFormatMatrix;
        }

        //--------------------------------------------------------------------------------------------------------------
        private static String strRowLineMatrix<XT>(
            //                                              //Format a matrix to a Set of Arrays(Lines) inside a block.
            //                                              //Example:
            //                                              //[
            //                                              //{
            //                                              //[0] { item, ..., item }
            //                                              //...
            //                                              //[x] { item, ..., item }
            //                                              //}
            //                                              //]

            XT[,] arr2xt_I,
            String strNL_I
            )
        {
            //                                              //Chars required for longest index: "[x]"
            int intCharsInLongestIndex = ("Row<" + (arr2xt_I.GetLength(0) - 1) + ">").Length;

            String[] arrstrIndexAndItem = new String[arr2xt_I.GetLength(0)];
            for (int intI = 0; intI < arr2xt_I.GetLength(0); intI = intI + 1)
            {
                //                                          //Format: NL [i]_ array
                arrstrIndexAndItem[intI] = strNL_I + ("Row<" + intI + ">").PadRight(intCharsInLongestIndex) + " " +
                    Test.strLineRow<XT>(arrxtGetRow<XT>(arr2xt_I, intI));
            }

            return strNL_I + "{" + String.Join("", arrstrIndexAndItem) + strNL_I + "}";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static String strLineMatrix<XT>(
            //                                              //Produces:
            //                                              //{ { item, ..., item }, ..., { item, ..., item } }.
            //                                              //str, matrix in one line format.

            XT[,] arr2xt_I
            )
        {
            //                                              //Convert arrobj to arrstr
            String strMatrix = "";
            for (int intI = 0; intI < arr2xt_I.GetLength(0); intI = intI + 1)
            {
                strMatrix = strMatrix + Test.strLineRow<XT>(arrxtGetRow<XT>(arr2xt_I, intI)) + ", ";
            }

            //                                              //Format: { item, item, ..., item }
            return "{ " + strMatrix.Substring(0, strMatrix.Length - 2) + " }";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static XT[] arrxtGetRow<XT>(
            XT[,] arr2xt_I,
            int intRow_I
            )
        {

            XT[] arrxtRow = new XT[arr2xt_I.GetLength(1)];
            for (int intI = 0; intI < arr2xt_I.GetLength(1); intI = intI + 1)
            {
                arrxtRow[intI] = arr2xt_I[intRow_I, intI];
            }

            return arrxtRow;
        }
        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.3DArray(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog<XT>(

            XT[,,] _3Array_I,
            String text_I
            )
        {
            LogArr3OptionEnum logOption = (
                //                                          //These type of array need to be VERTICAL
                Test.boolShouldDispleyVertical(_3Array_I.GetType().GetElementType())
                ) ?
                LogArr3OptionEnum.VERTICAL :
                LogArr3OptionEnum.ARRAY;

            return Test.ToLog(_3Array_I, text_I, logOption);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog<XT>(

            XT[,,] _3Array_I,
            String text_I,
            LogArr3OptionEnum logOption_I
            )
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            LogArr3OptionEnum logOption = logOption_I;
            if (
                !(logOption_I == LogArr3OptionEnum.VERTICAL) &&
                //                                          //These types of collection need to be displayed VERTICAL
                Test.boolShouldDispleyVertical(_3Array_I.GetType().GetElementType())
                )
            {
                Test.Warning(Test.ToLog(_3Array_I.GetType(), "arr3xt_I.Type") +
                    " will be displayed VERTICAL, to avoid this warning, please change option in the code",
                    Test.ToLog(logOption_I, "logOption_I"));

                logOption = LogArr3OptionEnum.VERTICAL;
            }

            String strArr3t = (_3Array_I == null) ? "null" : Test.strToLog3DArray(_3Array_I, text_I, logOption);

            return strArr3t;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog<XT>(

            XT[,,] _3Array_I
            )
        {
            return (_3Array_I == null) ? "null" : Test.strObjIdGet(_3Array_I);
        }

        //--------------------------------------------------------------------------------------------------------------
        private static String strObjIdGet<XT>(
            //                                              //str, arr3xxx«Size0, Size1, Size2»:HashCode.

            XT[,,] arr3xt_I
            )
        {
            Test.AbortIfNull(arr3xt_I, "arr3xt_I");

            String strSize0 = "" + arr3xt_I.GetLength(0);
            String strSize1 = (strSize0 == "0") ? "0" : "" + arr3xt_I.GetLength(1);
            String strSize2 = (strSize1 == "0") ? "0" : "" + arr3xt_I.GetLength(2);

            String strHashCode = (Test.boolIsComparableLog_Z) ? Test.strMASK : "" + arr3xt_I.GetHashCode();

            return "arr3" + Test.GetPrefixFromType(typeof(XT)) + "«" + strSize0 + ", " + strSize1 + ", " + strSize2 +
                "»:" + strHashCode;
        }

        //--------------------------------------------------------------------------------------------------------------
        private static String strToLog3DArray<XT>(
            //                                              //str, arr2xxx«Size0, Size1»:HashCode.

            XT[,,] arr3xt_I,
            String text_I,
            LogArr3OptionEnum logOption_I
            )
        {
            String strObjId = Test.strObjIdGet(arr3xt_I);

            String strFormat3DArray;
            if (
                Test.darrobjPreviouslyProcessed.Contains(arr3xt_I)
                )
            {
                strFormat3DArray = text_I + "(" + strObjId + strMessageLogBefore + ")";
            }
            else
            {
                //                                          //Register matrix as processed
                Test.darrobjPreviouslyProcessed.Add(arr3xt_I);

                /*CASE*/
                if (
                    logOption_I == LogArr3OptionEnum.VERTICAL
                    )
                {
                    strFormat3DArray = Test.strVertical3DArray(arr3xt_I, text_I, strObjId);
                }
                else if (
                    logOption_I == LogArr3OptionEnum.MATRIX
                    )
                {
                    String strNL;
                    String strLabel;
                    Test.subOpenBlock(out strNL, out strLabel, out strFormat3DArray, text_I, strObjId);

                    strFormat3DArray = strFormat3DArray + Test.strMatrix3DArray(arr3xt_I, strNL);

                    Test.subCloseBlock(ref strNL, ref strFormat3DArray, strLabel);
                }
                else
                {
                    String strNL;
                    String strLabel;
                    Test.subOpenBlock(out strNL, out strLabel, out strFormat3DArray, text_I, strObjId);

                    strFormat3DArray = strFormat3DArray + Test.strRow3DArray(arr3xt_I, strNL);

                    Test.subCloseBlock(ref strNL, ref strFormat3DArray, strLabel);
                }
                /*END-CASE*/
            }

            return strFormat3DArray;
        }

        //--------------------------------------------------------------------------------------------------------------
        private static String strVertical3DArray<XT>(
            //                                              //Format a 3Darray to a Set of Lines(Items) inside a block.
            //                                              //Example:
            //                                              //[
            //                                              //{
            //                                              //[0,0,0] item
            //                                              //...
            //                                              //[n,m,o] item
            //                                              //}
            //                                              //]

            //                                              //str, set in block format

            XT[,,] arr3xt_I,
            String text_I,
            String strObjId_I
            )
        {

            String strFormat3DArray;

            String strNL;
            String strLabel;
            Test.subOpenBlock(out strNL, out strLabel, out strFormat3DArray, text_I, strObjId_I);

            int intCharsInLongestIndex = ("[" + (arr3xt_I.GetLength(0) - 1) + "," + (arr3xt_I.GetLength(1) - 1)
                + "," + (arr3xt_I.GetLength(2) - 1) + "]").Length;

            for (int intI = 0; intI < arr3xt_I.GetLength(0); intI = intI + 1)
            {
                for (int intJ = 0; intJ < arr3xt_I.GetLength(1); intJ = intJ + 1)
                {
                    String[] arrstrIndexAndItem = new String[arr3xt_I.GetLength(2)];

                    for (int intK = 0; intK < arr3xt_I.GetLength(2); intK = intK + 1)
                    {
                        //                                          //Format: NL [i,j,k]_ item
                        arrstrIndexAndItem[intK] = strNL
                            + ("[" + intI + "," + intJ + "," + intK + "]").PadRight(intCharsInLongestIndex) + " " +
                            Test.z_TowaPRIVATE_ToLogXT(arr3xt_I[intI, intJ, intK]);
                    }

                    strFormat3DArray = strFormat3DArray + String.Join("", arrstrIndexAndItem)
                        + (!(intJ == arr3xt_I.GetLength(1) - 1) ? strNL : "");
                }
                strFormat3DArray = strFormat3DArray + (!(intI == arr3xt_I.GetLength(0) - 1) ? strNL + strNL : "");
            }

            Test.subCloseBlock(ref strNL, ref strFormat3DArray, strLabel);

            return strFormat3DArray;
        }

        //--------------------------------------------------------------------------------------------------------------
        private static String strMatrix3DArray<XT>(
            //                                              //Format a 3DArray to a Set of Matrix(Lines) inside a block.
            //                                              //Example:
            //                                              //[
            //                                              //{
            //                                              //Matrix<0> { { item, item },  ..., { item, item } }
            //                                              //...
            //                                              //Matrix<n> { { item, item },  ..., { item, item } }
            //                                              //}
            //                                              //]

            XT[,,] arr3xt_I,
            String strNL_I
            )
        {
            //                                              //Chars required for longest index: "Matrix<i>"
            int intCharsInLongestIndex = ("Matrix<" + (arr3xt_I.GetLength(0) - 1) + ">").Length;

            String[] arrstrIndexAndItem = new String[arr3xt_I.GetLength(0)];
            for (int intI = 0; intI < arr3xt_I.GetLength(0); intI = intI + 1)
            {
                //                                          //Format: NL Matrix<i>_ matrix
                arrstrIndexAndItem[intI] = strNL_I + ("Matrix<" + intI + ">").PadRight(intCharsInLongestIndex) + " " +
                    Test.strLineMatrix<XT>(arr2xtGetMatrix<XT>(arr3xt_I, intI));
            }

            return strNL_I + "{" + String.Join("", arrstrIndexAndItem) + strNL_I + "}";
        }

        //--------------------------------------------------------------------------------------------------------------
        private static String strRow3DArray<XT>(
            //                                              //Format a 3D array to a Set of Arrays(Lines) inside a
            //                                              //      block.
            //                                              //Example:
            //                                              //[
            //                                              //{
            //                                              //Row<0,0> { item, ..., item }
            //                                              //...
            //                                              //Row<n,m> { item, ..., item }
            //                                              //}
            //                                              //]

            XT[,,] arr3xt_I,
            String strNL_I
            )
        {
            //                                              //Chars required for longest index: "Row<n,m>"
            int intCharsInLongestIndex =
                ("Row<" + (arr3xt_I.GetLength(0) - 1) + "," + (arr3xt_I.GetLength(1) - 1) + ">").Length;

            String strOpenSeparator = "{";
            String strCloseSeparator = "}";
            String strFormat3DArray = "";
            for (int intI = 0; intI < arr3xt_I.GetLength(0); intI = intI + 1)
            {
                String[] arrstrIndexAndItem = new String[arr3xt_I.GetLength(1)];

                for (int intJ = 0; intJ < arr3xt_I.GetLength(1); intJ = intJ + 1)
                {
                    //                                          //Format: NL Row<i,j>_ array
                    arrstrIndexAndItem[intJ] = strNL_I +
                        ("Row<" + intI + "," + intJ + ">").PadRight(intCharsInLongestIndex) + " " +
                        Test.strLineRow<XT>(arrxtGetRow<XT>(arr3xt_I, intI, intJ));
                }

                strFormat3DArray = strFormat3DArray + strNL_I + strOpenSeparator + String.Join("", arrstrIndexAndItem)
                    + strNL_I + strCloseSeparator;
            }

            return strNL_I + strOpenSeparator + strFormat3DArray + strCloseSeparator;
        }


        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static XT[,] arr2xtGetMatrix<XT>(
            XT[,,] arr3xt_I,
            int intMatrix_I
            )
        {

            XT[,] arr2xt = new XT[arr3xt_I.GetLength(1), arr3xt_I.GetLength(2)];
            for (int intI = 0; intI < arr3xt_I.GetLength(1); intI = intI + 1)
            {
                for (int intJ = 0; intJ < arr3xt_I.GetLength(2); intJ = intJ + 1)
                {
                    arr2xt[intI, intJ] = arr3xt_I[intMatrix_I, intI, intJ];
                }
            }

            return arr2xt;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static XT[] arrxtGetRow<XT>(
            XT[,,] arr3xt_I,
            int intI_I,
            int intJ_I
            )
        {

            XT[] arrxt = new XT[arr3xt_I.GetLength(2)];
            for (int intI = 0; intI < arr3xt_I.GetLength(2); intI = intI + 1)
            {
                arrxt[intI] = arr3xt_I[intI_I, intJ_I, intI];
            }

            return arrxt;
        }
        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.ReferenceToEntity(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog<TEntity>(
            //                                              //str, info. prepared for display.

            ReferenceTo<TEntity> rentity_I,
            String text_I
            )
            where TEntity : Entity, new()
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            String strToInfo = (rentity_I == null) ? "null" : rentity_I.ToLogFull();

            return text_I + "(" + strToInfo + ")";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog<TEntity>(
            //                                              //str, info. prepared for display.

            ReferenceTo<TEntity> rentity_I
            )
            where TEntity : Entity, new()
        {
            return (rentity_I == null) ? "null" : rentity_I.ToLogShort();
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.DaysPeriod(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog(
            //                                              //str, info. prepared for display.

            DaysPeriod dperiod_I,
            String text_I
            )
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            return text_I + "(" + Test.ToLog(dperiod_I) + ")";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog(
            //                                              //str, info. prepared for display.

            DaysPeriod dperiod_I
            )
        {
            return dperiod_I.ToText();
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.TimePeriod(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog(
            //                                              //str, info. prepared for display.

            TimePeriod tperiod_I,
            String text_I
            )
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            return text_I + "(" + Test.ToLog(tperiod_I) + ")";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog(
            //                                              //str, info. prepared for display.

            TimePeriod tperiod_I
            )
        {
            return tperiod_I.ToText();
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.Life(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog(
            //                                              //str, info. prepared for display.
            //                                              //It is formatted like a simple data (no as complex object)

            Life life_I,
            String text_I
            )
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            return text_I + "(" + Test.ToLog(life_I) + ")";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog(
            //                                              //str, info. prepared for display.

            Life life_I
            )
        {
            return (life_I == null) ? "null" : life_I.ToText();
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.ReferenceTo(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        public static String GetObjId<TKey, TEntity, TTable>(
            //                                              //str, rxxxx:HashCode.

            TableOfEntity<TKey, TEntity> tableOfEntity_I
            )
            where TKey : IComparable /*String, int, long, char, Date or Time*/
            where TEntity : Entity, new()
            where TTable : TableOfEntity<TKey, TEntity>, new()
        {
            Test.AbortIfNull(tableOfEntity_I, "tableOfEntity_I");

            String strHashCode = (Test.boolIsComparableLog_Z) ? Test.strMASK : "" + tableOfEntity_I.GetHashCode();

            return Test.GetPrefixFromType(tableOfEntity_I.GetType()) + ":" + strHashCode;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String GetObjId<TKey, TModel, TTmodel>(
            //                                              //str, rxxxx:HashCode.

            TableOfModel<TKey, TModel> tableOfModel_I
            )
            where TKey : IComparable /*String, int, long, char, Date or Time*/
            where TModel : EntityModel
            where TTmodel : TableOfModel<TKey, TModel>, new()
        {
            Test.AbortIfNull(tableOfModel_I, "tableOfEntity_I");

            String strHashCode = (Test.boolIsComparableLog_Z) ? Test.strMASK : "" + tableOfModel_I.GetHashCode();

            return Test.GetPrefixFromType(tableOfModel_I.GetType()) + ":" + strHashCode;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String GetObjId<TEntity>(
            //                                              //str, rxxxx:HashCode.

            ReferenceTo<TEntity> referenceTo_I
            )
            where TEntity : Entity, new()
        {
            Test.AbortIfNull(referenceTo_I, "referenceTo_I");

            String strHashCode = (Test.boolIsComparableLog_Z) ? Test.strMASK : "" + referenceTo_I.GetHashCode();

            return Test.GetPrefixFromType(referenceTo_I.GetType()) + ":" + strHashCode;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String z_TowaPRIVATE_strObjIdGet<TEntity>(
            //                                              //str, xxxx:HashCode.

            TEntity entity_I
            )
            where TEntity : Entity, new()
        {
            Test.AbortIfNull(entity_I, "entity_I");

            String strHashCode = (Test.boolIsComparableLog_Z) ? Test.strMASK : "" + entity_I.GetHashCode();

            return Test.GetPrefixFromType(entity_I.GetType()) + ":" + strHashCode;
        }

        //==============================================================================================================
        /*END-TASK*/

        //--------------------------------------------------------------------------------------------------------------
        /*TASK Test.History(ToLog)*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog<TValue>(
            //                                              //str, info. prepared for display.

            DateDateValueTrio<TValue> ddvtvalue_I,
            String text_I
            )
            where TValue : SerializableInterface<TValue>, new()
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            return text_I + "(" + ddvtvalue_I.ToLogFull() + ")";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog<TValue>(
            //                                              //str, info. prepared for display.

            DateDateValueTrio<TValue> ddvtvalue_I
            )
            where TValue : SerializableInterface<TValue>, new()
        {
            return ddvtvalue_I.ToLogShort();
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog<TOwner, TValue>(
            //                                              //str, info. prepared for display.

            HistoryS<TOwner, TValue> history_I,
            String text_I
            )
            where TOwner : Entity, new()
            where TValue : SerializableInterface<TValue>, new()
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            String ToLog;
            /*CASE*/
            if (
                history_I == null
                )
            {
                ToLog = text_I + "(null)";
            }
            else if (
                Test.darrobjPreviouslyProcessed.Contains(history_I)
                )
            {
                ToLog = text_I + "(" + history_I.ToLogShort() + ")";
            }
            else
            {
                //                                      //Register hix as processed
                Test.darrobjPreviouslyProcessed.Add(history_I);

                ToLog = text_I + "(" + history_I.ToLogFull() + ")";
            }
            /*END-CASE*/

            return ToLog;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog<TOwner, TValue>(
            //                                              //str, info. prepared for display.

            HistoryS<TOwner, TValue> history_I
            )
            where TOwner : Entity, new()
            where TValue : SerializableInterface<TValue>, new()
        {
            return (history_I == null) ? "null" : Test.GetObjId(history_I);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String GetObjId<TOwner, TValue>(
            //                                              //str, hixxxx«Size»:HashCode.

            HistoryS<TOwner, TValue> history_I
            )
            where TOwner : Entity, new()
            where TValue : SerializableInterface<TValue>, new()
        {
            Test.AbortIfNull(history_I, "history_I");

            String strHashCode = (Test.boolIsComparableLog_Z) ? Test.strMASK : "" + history_I.GetHashCode();

            return Test.GetPrefixFromType(history_I.GetType()) + ":" + strHashCode;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog<TOwner, TTarget>(
            //                                              //str, info. prepared for display.

            HistoryE<TOwner, TTarget> historyEntity_I,
            String text_I
            )
            where TOwner : Entity, new()
            where TTarget : Entity, new()
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            String ToLog;
            /*CASE*/
            if (
                historyEntity_I == null
                )
            {
                ToLog = text_I + "(null)";
            }
            else if (
                Test.darrobjPreviouslyProcessed.Contains(historyEntity_I)
                )
            {
                ToLog = text_I + "(" + historyEntity_I.ToLogShort() + ")";
            }
            else
            {
                //                                      //Register hix as processed
                Test.darrobjPreviouslyProcessed.Add(historyEntity_I);

                ToLog = text_I + "(" + historyEntity_I.ToLogFull() + ")";
            }
            /*END-CASE*/

            return ToLog;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog<TOwner, TTarget>(
            //                                              //str, info. prepared for display.

            HistoryE<TOwner, TTarget> historyEntity_I
            )
            where TOwner : Entity, new()
            where TTarget : Entity, new()
        {
            return (historyEntity_I == null) ? "null" : Test.GetObjId(historyEntity_I);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String GetObjId<TOwner, TTarget>(
            //                                              //str, hixxxx«Size»:HashCode.

            HistoryE<TOwner, TTarget> historyEntity_I
            )
            where TOwner : Entity, new()
            where TTarget : Entity, new()
        {
            Test.AbortIfNull(historyEntity_I, "historyEntity_I");

            String strHashCode = (Test.boolIsComparableLog_Z) ? Test.strMASK : "" + historyEntity_I.GetHashCode();

            return Test.GetPrefixFromType(historyEntity_I.GetType()) + ":" + strHashCode;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog<TOwner, TTarget>(
            //                                              //str, info. prepared for display.

            HistoryCrossed<TOwner, TTarget> historyCrossed_I,
            String text_I
            )
            where TOwner : Entity, new()
            where TTarget : Entity, new()
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            String ToLog;
            /*CASE*/
            if (
                historyCrossed_I == null
                )
            {
                ToLog = text_I + "(null)";
            }
            else if (
                Test.darrobjPreviouslyProcessed.Contains(historyCrossed_I)
                )
            {
                ToLog = text_I + "(" + historyCrossed_I.ToLogShort() + ")";
            }
            else
            {
                //                                      //Register hix as processed
                Test.darrobjPreviouslyProcessed.Add(historyCrossed_I);

                ToLog = text_I + "(" + historyCrossed_I.ToLogFull() + ")";
            }
            /*END-CASE*/

            return ToLog;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog<TOwner, TTarget>(
            //                                              //str, info. prepared for display.

            HistoryCrossed<TOwner, TTarget> historyCrossed_I
            )
            where TOwner : Entity, new()
            where TTarget : Entity, new()
        {
            return (historyCrossed_I == null) ? "null" : Test.GetObjId(historyCrossed_I);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String GetObjId<TOwner, TTarget>(
            //                                              //str, hixxxx«Size»:HashCode.

            HistoryCrossed<TOwner, TTarget> historyCrossed_I
            )
            where TOwner : Entity, new()
            where TTarget : Entity, new()
        {
            Test.AbortIfNull(historyCrossed_I, "historyCrossed_I");

            String strHashCode = (Test.boolIsComparableLog_Z) ? Test.strMASK : "" + historyCrossed_I.GetHashCode();

            return Test.GetPrefixFromType(historyCrossed_I.GetType()) + ":" + strHashCode;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String ToLog<TOwner, TTarget>(
            //                                              //str, info. prepared for display.

            HistoryWith<TOwner, TTarget> historyWith_I,
            String text_I
            )
            where TOwner : Entity, new()
            where TTarget : Entity, new()
        {
            Test.AbortIfNullOrEmpty(text_I, "text_I");

            String ToLog;
            /*CASE*/
            if (
                historyWith_I == null
                )
            {
                ToLog = text_I + "(null)";
            }
            else if (
                Test.darrobjPreviouslyProcessed.Contains(historyWith_I)
                )
            {
                ToLog = text_I + "(" + Test.GetObjId(historyWith_I) + strMessageLogBefore + ")";
            }
            else
            {
                //                                      //Register dic as processed
                Test.darrobjPreviouslyProcessed.Add(historyWith_I);

                ToLog = text_I + "(" + historyWith_I.ToLogFull() + ")";
            }
            /*END-CASE*/

            return ToLog;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String ToLog<TOwner, TTarget>(
            //                                              //str, info. prepared for display.

            HistoryWith<TOwner, TTarget> historyWith_I
            )
            where TOwner : Entity, new()
            where TTarget : Entity, new()
        {
            return (historyWith_I == null) ? "null" : Test.GetObjId(historyWith_I);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String GetObjId<TOwner, TTarget>(
            //                                              //str, hiwxxx«Size»:HashCode.

            HistoryWith<TOwner, TTarget> historyWith_I
            )
            where TOwner : Entity, new()
            where TTarget : Entity, new()
        {
            Test.AbortIfNull(historyWith_I, "historyWith_I");

            String strHashCode = (Test.boolIsComparableLog_Z) ? Test.strMASK : "" + historyWith_I.GetHashCode();

            return Test.GetPrefixFromType(historyWith_I.GetType()) + ":" + strHashCode;
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.Shared Method common to several types*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        /*CONSTANTS*/

        private const String strMessageLogBefore = "|logged before|";

        //                                                  //Towa's standard system types
        private static readonly String[,] arr2strSINGLE_TYPE_AND_PREFIX = {
            //                                              //Primitives
            { "int", "int" }, { "long", "long" }, { "number", "num" },{ "char", "char" }, { "bool", "bool" },
            { "String", "str" },
            //                                              //This is used only in Serialization
            { "byte", "byte" },
            //                                              //Other single types (non system types)
            { "RuntimeType", "type" }, { "Type", "type" },
            //                                              //System types
            { "Path", "syspath" }, { "Directory", "sysdir" }, { "File", "sysfile" },
            { "TextReader", "systextreader" }, { "TextWriter", "systextwriter" },
            //                                              //Array log option Enums
            { "LogArrOptionEnum", "logoption" }, { "LogArr2OptionEnum", "logoption" },
            { "LogArr3OptionEnum", "logoption" },
            //                                              //Path Enums
            { "PathTypeEnum", "pathtype" }, { "PathWhereEnum", "pathwhere" },
            //                                              //Other 
            { "Currency", "curr" },
            //                                              //Date & Times
            { "Language", "language" }, { "TimeZoneX", "timezone" },
            { "Date", "date" }, { "DayOfWeek", "doweek" }, { "DateTextEnum", "dtext" },
            { "Time", "time" },
            { "ZonedTime", "ztime" }, { "ZonedTimeTextEnum", "zttext" }, { "ZonedTimeDstTypeOfDayEnum", "dsttype" },
            { "DaysPeriod", "dperiod" }, { "TimePeriod", "tperiod" }, { "Life", "life" },
            };

        //                                                  //Both arrays order by first.
        private static readonly String[] arrstrSINGLE_TYPE;
        private static readonly String[] arrstrSINGLE_PREFIX;

        //                                                  //Towa's standard GENERIC types (type without `n)
        private static readonly String[,] arr2strGENERIC_TYPE_AND_PREFIX = {
            //                                              //System generic types
            { "DynamicArray<>", "darr" }, { "Stack<>", "stack" }, { "Queue<>", "queue" },
            { "Dictionary<>", "dic?" }, { "KeyValuePair<>", "kvp?" },
            //                                              //TowaStandard added generic types
            { "LinkedList<>", "lnkl" }, { "LinkedListNode<>", "lln" },
            { "EntityKey<>", "ekey" }, { "ReferenceTo<>", "r" },
            { "DateDateValueTrio<>", "ddvt" }, { "HistoryS<>", "his" }, { "HistoryE<>", "his" },
            { "HistoryCrossed<>", "hix" }, { "HistoryWith<>", "hiw" },
            };

        //                                                  //Both arrays order by first.
        private static readonly String[] arrstrGENERIC_TYPE;
        private static readonly String[] arrstrGENERIC_PREFIX;

        //--------------------------------------------------------------------------------------------------------------
        /*SUPPORT METHODS FOR INITIALIZER*/

        //--------------------------------------------------------------------------------------------------------------
        private static void subVerifySingleTypeAndPrefix(
            )
        {
            Test.AbortIfDuplicate(Test.arrstrSINGLE_TYPE, "Test.arrstrSINGLE_TYPE");
        }

        //--------------------------------------------------------------------------------------------------------------
        /*SHARED METHODS*/

        //-------------------------------------------------------------------------------------------------------------
        public static String z_TowaPRIVATE_ToLogXT<XT>(
            //                                              //THIS METHOD (AND NEXT) COULD BE CALLED ToLog<XT> and work
            //                                              //      exactly the same:
            //                                              //1. Al standard type will call its specific method (that
            //                                              //      will be a better math than this generic method).
            //                                              //2. Within a collection, where everything is generic (T),
            //                                              //      this methot (ToLog<XT>) will be the only match
            //                                              //      because is the only capable to catch all possible
            //                                              //      type included in generic (T).
            //                                              //WE DID NOT DECIDED TO NAME (ToLog<XT>) to avoid the risk
            //                                              //      of making this "public" in wish casa it will work
            //                                              //      ALMOST exactly the same:
            //                                              //1. The same.
            //                                              //2. The same.
            //                                              //3. Any other NON-STANDARD type (for which an specific
            //                                              //      ToLog method do not exist) will match this method
            //                                              //      ToLog<XT> and ABORT at running time.
            //                                              //4. As is today, this method "private", the error of
            //                                              //      trying to call ToLog with a NON-STANDARD type will
            //                                              //      detected at compile time.

            XT xt_I
            )
        {
            //                                              //Cast t to Object, this is required because T can be any
            //                                              //      type (Object accept any type)
            Object obj = (Object)xt_I;

            String strToLogXT = (obj == null) ? "null" :
                (obj is int) ? Test.ToLog((int)obj) :
                (obj is long) ? Test.ToLog((long)obj) :
                (obj is double) ? Test.ToLog((double)obj) :
                (obj is char) ? Test.ToLog((char)obj) :
                (obj is bool) ? Test.ToLog((bool)obj) :
                (obj is String) ? Test.ToLog((String)obj) :
                (obj is Type) ? Test.ToLog((Type)obj) :
                (obj is Enum) ? Test.ToLog((Enum)obj) :
                (obj is BsysAbstract) ? ((BsysAbstract)obj).ToLogShort() :
                (obj is BsysInterface) ? ((BsysInterface)obj).ToLogShort() :
                (obj is BclassAbstract) ? ((BclassAbstract)obj).ToLogFull() :
                (obj is BopenAbstract) ? ((BopenAbstract)obj).ToLogFull() :
                (obj is BtupleAbstract) ? ((BtupleAbstract)obj).ToLogFull() :
                (obj is Exception) ? Test.ToLog((Exception)obj) :
                (obj is DirectoryInfo) ? Test.ToLog((DirectoryInfo)obj) :
                (obj is FileInfo) ? Test.ToLog((FileInfo)obj) :
                (obj is StreamReader) ? Test.ToLog((StreamReader)obj) :
                (obj is StreamWriter) ? Test.ToLog((StreamWriter)obj) : null;

            //                                              //T accept any type, but not any type is implemented in
            //                                              //      ToLog()
            if (
                strToLogXT == null
                )
                Test.Abort("SOMETHING IS WRONG!!!, method Test.LogTo(?) for " +
                    Test.ToLog(obj.GetType(), "obj_I.Type") + " not implemented");

            return strToLogXT;
        }

        //-------------------------------------------------------------------------------------------------------------
        public static String z_TowaPRIVATE_ToLogXT<XT>(

            XT xt_I,
            String text_I
            )
        {
            //                                              //Cast t to Object, this is required because T can be any
            //                                              //      type (Object accept any type)
            Object obj = (Object)xt_I;

            String strToLogXT = (obj == null) ? "null" :
                (obj is int) ? Test.ToLog((int)obj, text_I) :
                (obj is long) ? Test.ToLog((long)obj, text_I) :
                (obj is double) ? Test.ToLog((double)obj, text_I) :
                (obj is char) ? Test.ToLog((char)obj, text_I) :
                (obj is bool) ? Test.ToLog((bool)obj, text_I) :
                (obj is String) ? Test.ToLog((String)obj, text_I) :
                (obj is Type) ? Test.ToLog((Type)obj, text_I) :
                (obj is Language) ? Test.ToLog((DateTime)obj, text_I) :
                (obj is TimeZoneX) ? Test.ToLog((DateTime)obj, text_I) :
                (obj is Date) ? Test.ToLog((DateTime)obj, text_I) :
                (obj is Time) ? Test.ToLog((DateTime)obj, text_I) :
                (obj is ZonedTime) ? Test.ToLog((DateTime)obj, text_I) :
                (obj is Enum) ? Test.ToLog((Enum)obj, text_I) :
                (obj is BsysAbstract) ? Test.ToLog((BsysAbstract)obj, text_I) :
                (obj is BclassAbstract) ? Test.ToLog((BclassAbstract)obj, text_I) :
                (obj is BopenAbstract) ? Test.ToLog((BopenAbstract)obj, text_I) :
                (obj is BtupleAbstract) ? Test.ToLog((BtupleAbstract)obj, text_I) :
                (obj is Exception) ? Test.ToLog((Exception)obj, text_I) :
                (obj is DirectoryInfo) ? Test.ToLog((DirectoryInfo)obj, text_I) :
                (obj is FileInfo) ? Test.ToLog((FileInfo)obj, text_I) :
                (obj is StreamReader) ? Test.ToLog((StreamReader)obj, text_I) :
                (obj is StreamWriter) ? Test.ToLog((StreamWriter)obj, text_I) : null;

            //                                              //T accept any type, but not any type is implemented in
            //                                              //      ToLog()
            if (
                strToLogXT == null
                )
                Test.Abort("SOMETHING IS WRONG!!!, method Test.LogTo(?, \"text\") for " +
                    Test.ToLog(obj.GetType(), "obj_I.Type") + " not implemented");

            return strToLogXT;
        }

        //--------------------------------------------------------------------------------------------------------------
        private static String strFullPathAsIsOrMasked(
            //                                              //str, full path masked.

            String FullPath_I
            )
        {
            PathX syspath_I = new PathX(FullPath_I);

            return Test.z_TowaPRIVATE_FullPathAsIsOrMasked(syspath_I);
        }

        //--------------------------------------------------------------------------------------------------------------
        private static bool boolShouldDispleyVertical(

            Type type_I
            )
        {
            return (
                typeof(BobjAbstract).IsAssignableFrom(type_I) ||
                //                                          //PathX is included in bobj
                (type_I == typeof(DirectoryInfo)) || (type_I == typeof(FileInfo))
                );
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String GetPrefixFromType(
            //                                              //Generate the variable name prefix correponding to a type.
            Type type_L
            )
        {
            String GetPrefixFromType = (type_L.IsGenericType) ? Test.strPrefixGenericType(type_L) :
                Test.strPrefixSingleType(type_L);
            if (
                type_L.IsArray
                )
            {
            }

            return GetPrefixFromType;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static String strPrefixSingleType(
            //                                              //Generate the variable name prefix correponding to a type.
            //                                              //Only single type, no collections.
            Type type_L
            )
        {
            int intX = Std.Name(type_L).BinarySearch(Test.arrstrSINGLE_TYPE);

            String strPrefixSingleType = (intX >= 0) ? Test.arrstrSINGLE_PREFIX[intX] :
                (typeof(Enum).IsAssignableFrom(type_L)) ? Test.strPrefixEnumOrBclass(type_L) :
                (typeof(BopenAbstract).IsAssignableFrom(type_L)) ? Test.strPrefixBopenOrBtuple(type_L) :
                (typeof(BtupleAbstract).IsAssignableFrom(type_L)) ? Test.strPrefixBopenOrBtuple(type_L) :
                (typeof(BsysAbstract).IsAssignableFrom(type_L)) ? Test.strPrefixEnumOrBclass(type_L) :
                (typeof(BclassAbstract).IsAssignableFrom(type_L)) ? Test.strPrefixEnumOrBclass(type_L) : null;

            if (
                strPrefixSingleType == null
                )
                Test.Abort("Prefix for " + Test.ToLog(Std.Name(type_L), "type_L.Name") + " could not be found",
                    Test.ToLog(Test.arr2strSINGLE_TYPE_AND_PREFIX, "Test.arr2strSINGLE_TYPE_AND_PREFIX"));

            return strPrefixSingleType;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static String strPrefixEnumOrBclass(
            //                                              //Prefix for enum and bclass (AaaaaBbbbbCcccc).
            //                                              //str, prefix ("aaaaa").

            Type type_I
            )
        {
            String strTypeName = Std.Name(type_I);

            if (
                !Std.IsLetterUpper(strTypeName[0])
                )
                Test.Abort(strTypeName + " is not an standard enum or bclass, type name should start with A-Z",
                    Test.ToLog(type_I, "type_I"), Test.ToLog(strTypeName, "strTypeName"));

            int intI = 1;
            /*WHILE-DO*/
            while (
                (intI < strTypeName.Length) &&
                Std.IsDigitOrLetterLower(strTypeName[intI])
                )
            {
                intI = intI + 1;
            }

            //                                              //Subtract type prefix ("aaaaa").
            return ("" + strTypeName[0]).ToLower() + strTypeName.Substring(1, intI - 1);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static String strPrefixBopenOrBtuple(
            //                                              //Prefix for bopen ("On.." or "X...") or btuple ("Tn").
            //                                              //Search for end of prefix

            Type type_I
            )
        {
            String strTypeName = Std.Name(type_I);

            int intI = (strTypeName[0].IsInSet('O', 'T') && Std.IsDigit(strTypeName[1])) ? 2 : 1;
            /*WHILE-DO*/
            while (
                (intI < strTypeName.Length) &&
                //                                          //Between a-z
                Std.IsLetterLower(strTypeName[intI])
                )
            {
                intI = intI + 1;
            }

            //                                              //Subtract type prefix ("aaaaa").
            return ("" + strTypeName[0]).ToLower() + strTypeName.Substring(1, intI - 1);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static String strPrefixGenericType(
            //                                              //Generate the variable name prefix correponding to a 
            //                                              //      GENERIC type.
            Type type_L
            )
        {
            int intX = Std.Name(type_L).BinarySearch(Test.arrstrGENERIC_TYPE);

            String strPrefixGenericType = (intX >= 0) ? Test.arrstrGENERIC_PREFIX[intX] : null;

            if (
                strPrefixGenericType == null
                )
                Test.Abort("Prefix for " + Test.ToLog(Std.Name(type_L), "type_L.Name") + " could not be found",
                    Test.ToLog(Test.arr2strGENERIC_TYPE_AND_PREFIX, "Test.arr2strGENERIC_TYPE_AND_PREFIX"));

            //                                              //To easy code
            Type[] arrtype = type_L.GenericTypeArguments;

            //                                              //Correct Dictionary and KeyValuePair generic types
            String strType0 = Std.Name(arrtype[0]);
            /*CASE*/
            if (
                strPrefixGenericType == "dic?"
                )
            {
                strPrefixGenericType = (strType0 == "String") ? "dic" :
                    (strType0 == "long") ? "idx" :
                    (strType0 == "EntityKey<>") ? "ikey" :
                    null;

                if (
                    strPrefixGenericType == null
                    )
                    Test.Abort("Dictionary<" + strType0 + ", ?>" + " is not a valid generic type");
            }
            else if (
                strPrefixGenericType == "kvp?"
                )
            {
                strPrefixGenericType = (strType0 == "String") ? "svpair" :
                    (strType0 == "long") ? "ivpair" : (strType0 == "EntityKey<>") ? "kvpair" :
                    null;

                if (
                    strPrefixGenericType == null
                    )
                    Test.Abort("KeyValuePair<" + strType0 + ", ?>" + " is not a valid generic type");
            }
            else
            {
                //                                          //No correction required
            }
            /*END-CASE*/

            return strPrefixGenericType + Test.GetPrefixFromType(arrtype[arrtype.Length - 1]);
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.Blocks*/
        //==============================================================================================================

        //--------------------------------------------------------------------------------------------------------------
        /*CONSTANTS*/

        //                                                  //If there are more than 25 levels the last value is used.
        private static int[] arrintLevelSpaces = {
            0, 4, 8, 12, 16, 20, 24, 27, 30, 33, 36, 39, 42, 44, 46, 48, 50, 52, 54, 55, 56, 57, 58, 59, 60
            };

        //                                                  //If there are more than 28 levels the last value is used.
        private const String strLETTERS_FOR_LEVEL = "?ABCDEFGHIJKLMNOPQRSTUVWXYZ*";

        //--------------------------------------------------------------------------------------------------------------
        /*STATIC VARIABLES*/

        //                                                  //Each START-END block must be at a higher lever than its
        //                                                  //      respective base, and should be increased when 
        //                                                  //opening a block and 
        //                                                  //decreased after closing it.
        private static int intLevel = 0;

        //                                                  //This variable is used for every START-END block
        //                                                  //      and assigning a unique identification number
        //                                                  //      (Every time the value is read, 
        //                                                  //      it should be increased by 1).
        private static int intStartEnd = 0;

        //--------------------------------------------------------------------------------------------------------------
        /*STATIC METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        private static void subOpenBlock(
            //                                              //Generates the required parameters for subToBlockFormat.
            //                                              //When used, block must be paired whith its respective END.

            //                                              //NL + indentation characters.
            out String strNL_O,
            //                                              //Label for block START_??? y END_???. (this is ???).
            out String strLabel_O,
            //                                              //information String to start block 
            out String strStartInfo_O,
            //                                              //Text to describe the object
            String text_I,
            //                                              //Object Id, if this block is meant for a bclass, should be
            //                                              //      ""
            String strObjId_I
            )
        {
            strNL_O = GetNewLine();

            //                                              //Assigns next level (it will return to its original value 
            //                                              //  after ending the block).
            Test.intLevel = Test.intLevel + 1;

            //                                              //Assigns a unique ID.
            Test.intStartEnd = Test.intStartEnd + 1;

            //                                              //Determines the label that corresponds to the block.
            //                                              //After 'Z', '*' is used.
            char charLettersStartEnd = (Test.intLevel >= Test.strLETTERS_FOR_LEVEL.Length) ? '*' :
                Test.strLETTERS_FOR_LEVEL[Test.intLevel];

            //                                              //Assigns the STARTEND label
            strLabel_O = charLettersStartEnd.ToString() + Test.intStartEnd;

            //                                              //Append Start of block.
            //                                              //START of block should not include NewLine on "A?" block
            String strNlForStart =
                (charLettersStartEnd == 'A') ? strNL_O.Substring(Environment.NewLine.Length) : strNL_O;

            strStartInfo_O = strNlForStart + "##########>>>>>START_" + strLabel_O;
            strStartInfo_O = strStartInfo_O + strNL_O + text_I + "(" + strObjId_I;
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void subCloseBlock(
            //                                              //Closes the block (and restores the level).
            //                                              //This method should only be used once a block is open.

            //                                              //NL + indentation characters.
            ref String strNL_IO,
            //                                              //String to append information.
            ref String strStartInfo_IO,
            String strLabel_I
            )
        {
            //                                              //End of Block.
            strStartInfo_IO = strStartInfo_IO + ")" + strNL_IO + "##########<<<<<END_" + strLabel_I;

            //                                              //Restores the level.
            Test.intLevel = Test.intLevel - 1;

            strNL_IO = Test.GetNewLine();
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String GetNewLine()
        {
            //                                              //Determines NL+indentation that corresponds to the block.
            if (
                intLevel < 0
                )
                Test.Abort(Test.ToLog(intLevel, "intNivel") + " should be 0 or positive");

            //                                              //Determines the needed spaces for indentation.
            int intSpaces = (Test.intLevel >= Test.arrintLevelSpaces.Length) ?
                Test.arrintLevelSpaces[arrintLevelSpaces.Length - 1] : Test.arrintLevelSpaces[intLevel];

            //                                              //Return NL with required spacing.
            return Environment.NewLine + "".PadLeft(intSpaces);
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Test.LogAborts*/
        //==============================================================================================================
        //                                                  //Implementación de apoyos para poder efectuar pruebas de
        //                                                  //      Abort y regitrar su información en un log.
        //                                                  //¿Cómo?.
        //                                                  //En el código "driver" para ejecutar la prueba (ej. en
        //                                                  //      Test Sys01.cs), llamar al método:
        //                                                  //Test.subSetTestAbort(); o.
        //                                                  //Test.subResetTestAbort(); o.

        //--------------------------------------------------------------------------------------------------------------
        /*CONSTANTS*/

        //--------------------------------------------------------------------------------------------------------------
        /*STATIC VARIABLES*/

        //                                                  //Indicador de se desea test.
        //                                                  //This is the initial value, 
        private static bool IsTestAbortOn = false;

        //--------------------------------------------------------------------------------------------------------------
        /*STATIC CONSTRUCTOR SUPPORT METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        /*SHARED METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public static void SetTestAbort(
            //                                              //Marca que desea test.
            )
        {
            Test.IsTestAbortOn = true;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void ResetTestAbort(
            //                                              //Marca que desea concluir test.
            )
        {
            Test.IsTestAbortOn = false;
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Abort&Warning*/
        //==============================================================================================================
        //                                                  //Methods to abort excecution or just to send a warning.
        //                                                  //To avoid "weird" behavior of code running, methods
        //                                                  //      that are to be executed outside the control of
        //                                                  //      developer should verify its parameter (implement
        //                                                  //      fuse's) and abort if something is wrong ("my
        //                                                  //      method is not designed to manage this input).
        //                                                  //Sometimes the intention is just to warn the user.

        //--------------------------------------------------------------------------------------------------------------
        public static void Abort(

            //                                              //Just the message, no additional info.
            String message_I
            )
        {
            Test.Abort(message_I, null);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void Abort(

            String message_I,
            params String[] complementaryInfo_I
            )
        {
            String strFullMessage = Test.strFullMessageGet("Abort", message_I, complementaryInfo_I);
            Test.subShowFullMessage(strFullMessage);

            //                                              //Existen 2 posibilidades para continuar o terminar
            if (
                Test.IsTestAbortOn
                )
            {
                throw new SysexcepuserUserAbort(strFullMessage);
            }
            else
            {
                Test.FinalizeLog();

                Environment.Exit(0);
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void Warning(
            //                                              //Ejecucion al detectar situación anormal.
            //                                              //Puede ser WinForms app o Console app.

            //                                              //Mensaje descriptivo.
            String message_I
            )
        {
            Test.Warning(message_I, null);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void Warning(
            //                                              //Ejecucion al detectar situación anormal.
            //                                              //Puede ser WinForms app o Console app.

            //                                              //Mensaje descriptivo.
            String message_I,
            //                                              //Para facilitar el diagnóstico.
            //                                              //No se incluye información complementaria
            params String[] complementaryInfo_I
            )
        {
            String strFullMessage = Test.strFullMessageGet("Warning", message_I, complementaryInfo_I);
            Test.subShowFullMessage(strFullMessage);
        }

        //--------------------------------------------------------------------------------------------------------------
        private static String strFullMessageGet(
            //                                              //Formatea el mensaje.
            //                                              //str, full message.

            //                                              //"Abort" or "Warning"
            String strAbortOrWarning_I,
            String strMessage_I,
            String[] arrstrComplementaryInfo_I
            )
        {
            String[] arrstrMethodCall = Test.arrstrMethodCallGet(strAbortOrWarning_I);

            //                                              //Extrae ubicación del aborto/warning (sin →)
            String strMethodCallAbortOrWarning = arrstrMethodCall[0].Substring(1);

            String[] arrstrPart = strMethodCallAbortOrWarning.Split('●');
            String strApplication = arrstrPart[0];
            String strRelevantPart = arrstrPart[1];
            String strClass = arrstrPart[2];
            String strMethodAndParamenters = arrstrPart[3];

            //                                              //To easy code
            String strNL = Environment.NewLine;
            String strAbnormalEndOrWarning = (strAbortOrWarning_I == "Abort") ? "ABNORMAL END" : "WARNING";
            String strAllComplementaryInfo =
                (arrstrComplementaryInfo_I == null) ? "" :
                    ("COMPLEMENTARY INFO:" + strNL + '→' + String.Join(strNL + '→', arrstrComplementaryInfo_I)) +
                strNL;

            String strFullMessageGet =
                "<<<" + strAbnormalEndOrWarning + ">>>" + strNL +
                "APPLICATION: " + strApplication + strNL +
                "RELEVANT PART: " + strRelevantPart + strNL +
                "CLASS: " + strClass + strNL +
                "METHOD: " + strMethodAndParamenters + strNL +
                strAbnormalEndOrWarning + " MESSAGE: " + strNL +
                strMessage_I + strNL + strNL +
                "METHOD CALL SEQUENCE (from last to first):" + strNL +
                String.Join(strNL, arrstrMethodCall) + strNL +
                strAllComplementaryInfo +
                "<<<END OF " + strAbnormalEndOrWarning + ">>>";

            return strFullMessageGet;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static String[] arrstrMethodCallGet(
            //                                              //THIS METHOD NEED TO BE REWRITTEN FOR EACH TECHNOLOGY
            //                                              //      INSTANCE (C#, Java, Swift, etc.).
            //                                              //In C# (the same for any other instance), stack will
            //                                              //      contain a lot of information.
            //                                              //We extract only what we want to display.
            //                                              //arrstr, info stack, sequence of method call from 
            //                                              //      execution (or test start) until BEFORE Abort/Warning
            //                                              //      method was called.

            //                                              //"Abort" o "Warning"
            String strAbortOrWarning_I
            //                                              //, Environment.StackTrace
            )
        {
            //                                              //La estrategia será:
            //                                              //1. Se localiza "TowaStandard.Test.Abort" o
            //                                              //      "TowaStandard.Test.Warning" (se toma a partir del
            //                                              //      siguiente método).
            //                                              //2. Localiza "Z_Testing.", esto aún no es claro como será
            //                                              //      cuando la aplicación este en operación (se toma
            //                                              //      hasta el primer método fuera de "Z_Testing").
            //                                              //3. Convierte a arrstr de líneas para poder analizarlas.
            //                                              //4. De cada línea elimina la información no útil.

            //                                              //To easy code.
            String strStack = Environment.StackTrace;
            String strNL = Environment.NewLine;

            //                                              //Localiza a partir de donde interesa.
            int intTowaStandardDotTestDotAbortOrWarning =
                strStack.LastIndexOfOrdinal("TowaStandard.Test." + strAbortOrWarning_I + "(");
            int intStart = strStack.IndexOfOrdinal(strNL, intTowaStandardDotTestDotAbortOrWarning) +
                strNL.Length;

            //                                              //Localiza a partir de donde ya no interesa.
            int intEndPlusOne = strStack.IndexOfOrdinal("Z_Testing.", intStart) - (strNL + "___at_").Length;

            //                                              //Take info we are looking for (not clean).
            int intX = strStack.Length;
            String strMethodCallsUnclean = strStack.Substring(intStart, intEndPlusOne - intStart);
            String[] arrstrMethodCallUnclean = strMethodCallsUnclean.Split(strNL);

            //                                              //Clean info.
            String[] arrstrMethodCallGetX = new String[arrstrMethodCallUnclean.Length];
            for (int intI = 0; intI < arrstrMethodCallUnclean.Length; intI = intI + 1)
            {
                arrstrMethodCallGetX[intI] = Test.strMethodCallGet(arrstrMethodCallUnclean[intI]);
            }

            return arrstrMethodCallGetX;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static String strMethodCallGet(
            //                                              //THIS METHOD NEED TO BE REWRITTEN FOR EACH TECHNOLOGY
            //                                              //      INSTANCE (C#, Java, Swift, etc.).
            //                                              //Info extracted will be in standarize format (the same for
            //                                              //      all technology instance).
            //                                              //The idea is to report:
            //                                              //1. System or Aplicación.
            //                                              //2. Subsystem, Module or Relevant Part.
            //                                              //3. Component, Program, or Class.
            //                                              //4. Method, rutine or function.
            //                                              //(Glg, August 27, 2018) This is well defined for Object
            //                                              //      Oriented, this will be:
            //                                              //1. Application (Ex. SoftwareAutomation,
            //                                              //      SoftwareAutomationCobol, SoftwareCompare, etc.). 
            //                                              //2. Relevant Part (Ex. Cod, Nvsbcod, Nvsbsol, etc.).
            //                                              //3. Class (Ex. CodCodeAbstract, CodcbCobol,
            //                                              //      NvsbcodNewVsBaseCodeComparison, etc.).
            //                                              //4. Method or 'initializer' for static constructor {Ex,
            //                                              //      subPrepareUseful(strCHAR_USEFUL_I, arrcharUSEFUL_O),
            //                                              //      subPrepareConstants(codDUMMY_I, strCHAR_USEFUL_I,
            //                                              //      ___ arrcharUSEFUL_O,
            //                                              //      ___ strCHAR_TO_CONVERT_AND_CONVERSION_I, 
            //                                              //      ___ arrcharTO_CONVERT_O, arrcharCONVERSION_O),
            //                                              //      'initializar', subTestCod1(), etc.}.
            //                                              //In C#, stack contains:
            //                                              //%%%ns.___.ns.class.method(parameters)%%%.
            //                                              //%%% is non useful info.
            //                                              //Get its parts:
            //                                              //1. Application, first namespace.
            //                                              //2. Relevant Part, last namespace (if was the first, then
            //                                              //      Relevant Part will be messing, this should happen if
            //                                              //      Abort/Warning is in TowaStandard.
            //                                              //3. Class.
            //                                              //4. Method (only method name).
            //                                              //5. Parameters.
            //                                              //Example:
            //                                              //→SoftwareAutomation●Cod●CodCodeAbstract●subPrepareUse... .
            //                                              //(Glg, July 25, 2018) paramenters (var1, ___, varN) where
            //                                              //      changed to (?) to make similar to Java (the code
            //                                              //      was comented).

            //                                              //Non standarized info
            String strMethodCallUnclean_I
            )
        {
            //                                              //We have:


            //                                              //Extract useful information.
            int intStart = "___at_".Length;
            int intEndPlus1 = strMethodCallUnclean_I.IndexOf(')') + 1;
            String strMethodCallClean = strMethodCallUnclean_I.Substring(intStart, intEndPlus1 - intStart);

            int intParameters = strMethodCallClean.IndexOf('(') + 1;
            int intCloseParenthesis = strMethodCallClean.Length - 1;
            int intFirstDot = strMethodCallClean.IndexOf('.');
            int intRelevantPart = intFirstDot + 1;
            int intSecondDot = strMethodCallClean.IndexOf('.', intRelevantPart);

            //                                              //Ojo, cuanto se trata del constructor estático o
            //                                              //      initializer, este tiene como nombre ".cctor".
            //                                              //Se encuentra en la secuencia:
            //                                              //ns.ns.___.ns.class..cctor(parameters).
            int intMethod = strMethodCallClean.LastIndexOf('.');
            if (
                //                                          //NO estamos en el caso (ya esta en el inicio de ".cctor").
                //                                          //ns.ns.___.ns.class..cctor(parameters).
                strMethodCallClean[intMethod - 1] != '.'
                )
            {
                //                                          //Avanza para posicionarse al inicio de método
                intMethod = intMethod + 1;
            }

            int intClass = strMethodCallClean.LastIndexOf('.', intMethod - 2) + 1;

            String strApplication = strMethodCallClean.Substring(0, intFirstDot);

            //                                              /Puede no haber Relevant Part, se deja ""
            String strRelevantPart = (intRelevantPart == intClass) ? "" :
                strMethodCallClean.Substring(intRelevantPart, intSecondDot - intRelevantPart);

            String strClass = strMethodCallClean.Substring(intClass, intMethod - 1 - intClass);
            String strMethod = strMethodCallClean.Substring(intMethod, intParameters - 1 - intMethod);
            String strParametersUnclean = strMethodCallClean.Substring(intParameters,
                intCloseParenthesis - intParameters);

            //                                              //To split each paramenter using its commas, we need to get
            //                                              //      rid of [?] and  <?>, they can contein some ,.
            //                                              //Review and clean backward.
            int intX = strParametersUnclean.Length - 1;
            /*WHILE-DO*/
            while (
                intX >= 0
                )
            {
                if (
                    (strParametersUnclean[intX] == ']') || (strParametersUnclean[intX] == '>')
                    )
                {
                    int intOpenMark = strParametersUnclean.LastIndexOf((strParametersUnclean[intX] == ']') ? '[' : '<',
                        intX);

                    //                                      //Remove [____] or <____>
                    strParametersUnclean = strParametersUnclean.Substring(0, intOpenMark) +
                        strParametersUnclean.Substring(intX + 1);

                    intX = intOpenMark - 1;
                }
                else
                {
                    intX = intX - 1;
                }
            }

            //                                              //Clean parameters (only parameter name is needed).
            String[] arrstrParameterUnclean = strParametersUnclean.Split(',');
            String[] arrstrParameterName = new String[arrstrParameterUnclean.Length];
            for (int intI = 0; intI < arrstrParameterUnclean.Length; intI = intI + 1)
            {
                int intParameterName = arrstrParameterUnclean[intI].LastIndexOf(' ') + 1;
                arrstrParameterName[intI] = arrstrParameterUnclean[intI].Substring(intParameterName);
            }

            //                                              //(Glg 25Jul2018) los paramentros (var1, ___, varN) se
            //                                              //      cambiaron a (?) esto para facilitar la similitud
            //                                              //      con Java (se conserva el código comentarizado).
            String strParameters = '(' + "?"/*String.Join(", ", arrstrParameterName)*/ + ')';
            String strMethodAndParameters;
            if (
                //                                          //Initializer
                strMethod == ".cctor"
                )
            {
                strMethodAndParameters = "'initializer'";
            }
            else if (
                //                                          //Object Constructor
                strMethod == ".ctor"
                )
            {
                strMethodAndParameters = strParameters;
            }
            else
            {
                //                                          //Other king of method
                strMethodAndParameters = strMethod + strParameters;
            }

            return '→' + strApplication + '●' + strRelevantPart + '●' + strClass + '●' + strMethodAndParameters;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subShowFullMessage(
            //                                              //Can be Windows or Console app.

            String strFullMessage_I
            )
        {
            //                                              //In "ComparableLog", test should run without human
            //                                              //      intervention
            if (
                Test.boolIsComparableLog_Z
                )
            {
                //                                          //Do nothing. (no message on window or console)
            }
            else
            {
                /*(Glg, 31Ago2019) Elimine el uso de Windows Forms, se modificaron 4 lugares
                    a. frmStart.cs, frmStart.Designer.cs y Program.cs se cancelo todo
                    b. Test, se comentarizo el mensaje por Windows
                if (
                    //                                      //Is Windows app
                    Application.MessageLoop
                    )
                {
                    //                                      //Show message on Window
                    MessageBox.Show(strFullMessage_I);
                }
                else
                {
                */
                //                                      //Show message on Console
                Console.WriteLine(strFullMessage_I);

                Console.WriteLine("");
                Console.WriteLine("ENTER KEY TO CONTINUE");
                String strReadLine = Console.ReadLine();
                /*
                }
                */
            }

            //                                              //Send message to log.
            Test.Log(strFullMessage_I);
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK TestLog*/
        //==============================================================================================================
        //                                                  //Support for test logs.

        //--------------------------------------------------------------------------------------------------------------
        /*STATIC VARIABLES*/

        //                                                  //Log para mostrar información en un archivo.
        //                                                  //Este log se asigna al iniciar una prueba con el método
        //                                                  //      subInitializeLog().
        //                                                  //Se mostrará información en los siguientes casos:
        //                                                  //1. Al concluir una prueba, usando el método subLog().
        //                                                  //2. Al abortar (Abort), si systextwriterLog != null.
        //                                                  //3. Al enviar un Warning, similar a abortar.
        //                                                  //4. Al usar Trace.
        //                                                  //AT CODING, SOME VALUE IS REQUIRES BECAUSE IT CAN BE USED
        //                                                  //      BEFORE INITIALIZATION IS COMPLETE.
        private static StreamWriter systextwriterLog = null;

        //                                                  //Object previously processed in other LogTo execution.
        //                                                  //Al inicializar la clase se establecerá { }, después podrá
        //                                                  //      ser cambiado a contener algo.
        //                                                  //AT CODING, SOME VALUE IS REQUIRES BECAUSE IT CAN BE USED
        //                                                  //      BEFORE INITIALIZATION IS COMPLETE.
        //                                                  //***** AL LIMPIAR Xest, CAMBIAR A private
        private static List<Object> darrobjPreviouslyProcessed = new List<Object>();

        //--------------------------------------------------------------------------------------------------------------
        /*SHARED METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public static void InitializeLog(
            //                                              //NO SE DESEA ComparableLog.
            //                                              //Inicializa el log. (Al inicio de cada Test se debe
            //                                              //      ejecutar uno de estos métodos sobrecargados).

            PathX directoryForLogs_I,
            String test_I
            )
        {
            Test.InitializeLog(directoryForLogs_I, test_I, null);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void InitializeLog(
            //                                              //SE DESEA ComparableLog.
            //                                              //Inicializa el log. (Al inicio de cada Test se debe
            //                                              //      ejecutar uno de estos métodos sobrecargados).

            //                                              //Path para los logs, debe ser un directorio
            PathX directoryForLogs_I,
            //                                              //Clave de la prueba (Ej. Cod1, Cod4Com3Com4Com5Cod3Com2).
            //                                              //Con esta clave se formará en nombre de archivo log de la
            //                                              //      prueba añadiendole .test (Ej. Cod1.test,
            //                                              //      Cod4Com3Com4Com5Cod3Com2.test).
            //                                              //Si el path DirectoryForLogs\Test.test existe se reescribe.
            String test_I,
            //                                              //Conjunto de directorios que desea ser enmascarado, esta
            //                                              //      información es utilizada en LogTo de Path.
            //                                              //Ej.: { "\\psf\Home\Desktop", "/user/glg0818/Desk" } o
            //                                              //      { }.
            //                                              //Puede ser null, en este caso sería similar al método que
            //                                              //      no incluye este parámetro.
            T0maskTuple t0mask_L
            )
        {
            Test.AbortIfNull(directoryForLogs_I, "directoryForLogs_I");
            if (
                !directoryForLogs_I.IsDirectory
                )
                Test.Abort(Test.ToLog(directoryForLogs_I, "syspathDirectoryForLogs_I") +
                    " should be a directory");
            Test.AbortIfNullOrEmpty(test_I, "test_I");

            PathX syspathFileForTestLog = directoryForLogs_I.AddName(test_I + ".test");
            if (
                //                                          //No esta disponible para ser FILE
                syspathFileForTestLog.IsDirectory
                )
                Test.Abort(Test.ToLog(syspathFileForTestLog, "syspathFileForTestLog") + " can not be a directory");

            //                                              //Genera log
            Test.systextwriterLog = TextWriterX.New(FileX.New(syspathFileForTestLog));

            Test.subSetComparableTestMaskingParameters(t0mask_L);

            //                                              //Cada Test inicia la secuencia de los blocks An, Bn, ...
            Test.darrobjPreviouslyProcessed = new List<Object>();
            Test.intStartEnd = 0;

            BclassAbstract.ResetSummary();

            //                                              //Write first line in log
            String strNameUser = PathX.GetUserPath().Name;
            String strNameTester = (Test.boolIsComparableLog_Z) ? "<Test for Automatic Verification>" : strNameUser;
            String str = String.Format("{0}, Now({1:yyyy-MM-dd HH:mm}), {2}", strNameTester, Std.Now(),
                syspathFileForTestLog.Name);
            Test.Log(str);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void ResetLog(
            //                                              //Reset log
            )
        {
            Test.darrobjPreviouslyProcessed = new List<Object>();
            Test.intStartEnd = 0;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void FinalizeLog(
            //                                              //Cierra el log para UNA prueba. (Al concluir cada Test
            //                                              //      se debe ejecutar este método).
            //                                              //1. Dispose log.
            //                                              //2. Lo asinga a null.
            )
        {
            //                                              //Solo si esta en una prueba
            if (
                //                                          //Hay un log, estamos en prueba
                Test.systextwriterLog != null
                )
            {
                Test.systextwriterLog.Dispose();
                Test.systextwriterLog = null;
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void Log(
            //                                              //Genera información en el log.

            params String[] informationToLog_I

            )
        {
            //                                              //Solo si esta en una prueba
            if (
                //                                          //Hay un log, estamos en prueba
                Test.systextwriterLog != null
                )
            {
                //                                          //To easy code
                String strTextToLog = (informationToLog_I.Length == 0) ? "<NO INFO TO LOG>" :
                    String.Join(Environment.NewLine, informationToLog_I);

                Test.systextwriterLog.WriteLine(strTextToLog);
            }
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK ComparableTest*/
        //==============================================================================================================
        //                                                  //Suppor for comparable test.

        //--------------------------------------------------------------------------------------------------------------
        /*CONSTANTS*/

        //                                                  //To mask hashcode and other info.
        private const String strMASK = "§§§";

        //--------------------------------------------------------------------------------------------------------------
        /*STATIC VARIABLES*/

        //                                                  //FOR IMPLEMENTING COMPARABLE TEST

        //                                                  //Indicador para enmastarar el HashCode en los ObjId y otra
        //                                                  //      información que es útil pero que sin embargo impide
        //                                                  //      que los logs sean comparables en forma automática.
        //                                                  //AT CODING, SOME VALUE IS REQUIRES BECAUSE IT CAN BE USED
        //                                                  //      BEFORE INITIALIZATION IS COMPLETE.
        private static bool boolIsComparableLog_Z = false;
        public static bool z_TowaPRIVATE_boolIsComparableLog() { return Test.boolIsComparableLog_Z; }

        //                                                  //Conjunto de directorios que desea sean enmascarados, esta
        //                                                  //      información es utilizada en LogTo de Path.
        //                                                  //Este arreglo se debe ordenar en base a la longitud del
        //                                                  //      full path (de mayor a menor).
        //                                                  //Nótese que 2 ó más syspath pueden estár incluidos en el
        //                                                  //      path que se desea enmascarar, sin embargo debe
        //                                                  //      utilizar el de mayor longitud.
        private static PathX[] arrsyspathDirectoryToMask;

        //                                                  //dtNowBase y Deltas para subtituir Now (see t0maskTupla).
        private static DateTime dtNowBase;
        private static double[] arrnumDeltaSeconds;

        //                                                  //Al inicializar:
        //                                                  //1. A dtNowNext se asigna dtNowBase.
        //                                                  //2. A intDeltas se asigna 0.
        //                                                  //Al el método dtGetNowComparableTest():
        //                                                  //1. Se toma dtNowNext.
        //                                                  //2. dtNowNext+arrnumDeltaSeconds[intDeltas % arr..Length].
        //                                                  //3. intDeltas + 1.
        private static DateTime dtNowNext;
        private static int intDeltas;

        //--------------------------------------------------------------------------------------------------------------
        /*SHARED METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        private static void subSetComparableTestMaskingParameters(
            //                                              //Método iniciar un Test for Automatic Verification, en el
            //                                              //      cual:
            //                                              //Tester name se muestra con "<Test for Automatic
            //                                              //      Verification>".
            //                                              //Los hashcode se muestran como §§§.
            //                                              //Los full path (de syspath) que estan incluidos en
            //                                              //      arrstrDirectoryToMask_L se muestran enmascarando el
            //                                              //      directorio con §§§.

            //                                              //Conjunto de directorios que desea ser enmascarado, esta
            //                                              //      información es utilizada en LogTo de Path.
            //                                              //Ej.: { "\\psf\Home\Desktop", "/user/glg0818/Desk" } o
            //                                              //      { }.
            //                                              //null to indicate it is not a Comparable Test.
            T0maskTuple t0mask_I
            )
        {
            if (
                t0mask_I == null
                )
            {
                //                                          //NO SE DESEA ComparableLog.
                Test.boolIsComparableLog_Z = false;

                //                                          //It is not required to initialize other values, they should
                //                                          //      not be used
            }
            else
            {
                //                                          //SE DESEA ComparableLog.
                Test.boolIsComparableLog_Z = true;

                //                                          //Order in descending length sequence
                Test.arrsyspathDirectoryToMask = t0mask_I.arrsyspathDirectory;
                int[] arrintLengthFullPath = new int[Test.arrsyspathDirectoryToMask.Length];
                for (int intI = 0; intI < Test.arrsyspathDirectoryToMask.Length; intI = intI + 1)
                {
                    arrintLengthFullPath[intI] = -Test.arrsyspathDirectoryToMask[intI].FullPath.Length;
                }
                Std.Sort(arrintLengthFullPath, Test.arrsyspathDirectoryToMask);

                Test.dtNowBase = t0mask_I.dtNowBase;
                Test.arrnumDeltaSeconds = t0mask_I.arrnumDeltaSeconds;

                //                                          //Secuencia de dtNow
                Test.dtNowNext = Test.dtNowBase;
                Test.intDeltas = 0;
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static DateTime z_TowaPRIVATE_GetNowForComparableTest(
            //                                              //Genera un dtNow para comparable Test.
            //                                              //dt, Now
            )
        {
            DateTime dtGetNowComparableTest = Test.dtNowNext;

            //                                              //Prepara siguiente:
            //                                              //intX será: 0, 1, 2, ..., 0, 1, 2, ..., 0, ...
            int intX = Test.intDeltas % Test.arrnumDeltaSeconds.Length;
            long longMilliseconds = (int)Math.Round(Test.arrnumDeltaSeconds[intX] * 1000);
            Test.dtNowNext = Test.dtNowNext.AddMilliseconds(longMilliseconds);
            intDeltas = intDeltas + 1;

            return dtGetNowComparableTest;
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK Trace*/
        //==============================================================================================================
        //                                                  //Support for trace

        //--------------------------------------------------------------------------------------------------------------
        /*STATIC VARIABLES*/

        //                                                  //¿Cómo?.
        //                                                  //En los puntos que se crea conveniente, añadir:
        //                                                  //Test.subTrace(?).
        //                                                  //Imprimir el log que contendrá el trace y otra información
        //                                                  //      de la prueba

        //                                                  //Cada Trace que se genere tendra un número único 1, 2, 3,
        //                                                  //      etc. (esto es, su secuencia).
        //                                                  //Antes de generar un nuevo trace se debe incrementar.
        private static int intTraceSequence = 0;

        //--------------------------------------------------------------------------------------------------------------
        /*SHARED METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public static void Trace(
            //                                              //Genera un trace a writeline.

            //                                              //true, se desea generar el trace.
            //                                              //false, No se genera el trace.
            //                                              //Se incluye este parámetro para sin tener que eliminar la
            //                                              //      la ejecución del trace poder activarlo/desactivarlo.
            bool isTraceOn_I,
            //                                              //Etiqueta para identificar el registro del trace en la
            //                                              //      impresión. Cada instrucción trace que se agregue al
            //                                              //      código debe tener una etiqueta distinta.
            String label_I,
            //                                              //Información a incluir en el trace, esta información se le
            //                                              //      da forma similar a los LogTo.
            String informationToTrace_I
            )
        {
            /*
            if (
                Test.systextwriterLog == null
                )
                Test.Abort(Test.ToLog(Test.systextwriterLog, "Test.systextwriterLog") +
                    " should be created and assigned");
            */

            //                                              //Solo se procesa el trace si esta en ON.
            if (
                isTraceOn_I &&
                (Test.systextwriterLog != null)
                )
            {
                //                                          //Avanza una secuencia (esta es la secuencia única de este
                //                                          //      trace).
                intTraceSequence = intTraceSequence + 1;

                //                                          //Produce trace.
                String str = String.Format("►trace►►►►►{0}.{1}", label_I, intTraceSequence);
                Test.Log(str);
                Test.Log(informationToTrace_I);
            }
        }

        //==============================================================================================================
        /*END-TASK*/

        /*TASK TestingTools*/
        //==============================================================================================================
        //                                                  //Support testing.

        //--------------------------------------------------------------------------------------------------------------
        /*STATIC METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public static void AbortIfNull<TEnum>(

            TEnum enum_I,
            String identifier_I
            )
            where TEnum : Enum
        {
            if (
                identifier_I == null
                )
                Test.Abort("identifier_I(null) can not be null");
            if (
                identifier_I == ""
                )
                Test.Abort("identifier_I(" + identifier_I + ") can not be empty String");
            if (
                ("" + enum_I) == "Z_NULL"
                )
                Test.Abort(identifier_I + "(null) can not be null");
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void AbortIfNull(

            Object object_I,
            String identifier_I
            )
        {
            if (
                identifier_I == null
                )
                Test.Abort("identifier_I(null) can not be null");
            if (
                identifier_I == ""
                )
                Test.Abort("identifier_I(" + identifier_I + ") can not be empty String");
            if (
                object_I == null
                )
                Test.Abort(identifier_I + "(null) can not be null");
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void AbortIfNullOrNotDummy(

            BclassAbstract bclass_I,
            String identifier_I
            )
        {
            Test.AbortIfNull(identifier_I, "identifier_I");
            Test.AbortIfNull(bclass_I, identifier_I);
            if (
                !bclass_I.IsDummy
                )
                Test.Abort(Test.ToLog(bclass_I.IsDummy, identifier_I + ".IsDummy") + " should be DUMMY",
                    Test.ToLog(bclass_I));
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void AbortIfNullOrEmpty(

            String String_I,
            String identifier_I
            )
        {
            Test.AbortIfNull(identifier_I, "identifier_I");
            if (
                identifier_I == ""
                )
                Test.Abort("identifier_I(" + identifier_I + ") can not be empty String");
            Test.AbortIfNull(String_I, identifier_I);
            if (
                String_I == ""
                )
                Test.Abort(identifier_I + "(" + String_I + ") can not be empty String");
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void AbortIfNullOrEmpty<XT>(

            XT[] array_L,
            String identifier_I
            )
        {
            Test.AbortIfNullOrEmpty(identifier_I, "identifier_I");
            Test.AbortIfNull(array_L, identifier_I);
            if (
                array_L.Length == 0
                )
                Test.Abort(Test.ToLog(array_L, identifier_I) + " should contains at least 1 item");
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void AbortIfOneOrMoreItemsAreNotComparable(

            Enum[] arrayEnum_I,
            //                                              //Ej: arrt2sep
            String identifier_I
            )
        {
            Test.AbortIfNullOrEmpty(identifier_I, "identifier_I");
            Test.AbortIfNull(arrayEnum_I, identifier_I);

            for (int intI = 0; intI < arrayEnum_I.Length; intI = intI + 1)
            {
                if (
                    ("" + arrayEnum_I[intI]) == "Z_NULL"
                    )
                    Test.Abort(
                        Test.z_TowaPRIVATE_ToLogXT(arrayEnum_I[intI], identifier_I + "[" + intI + "]") +
                            " can not be null",
                        Test.ToLog(arrayEnum_I, identifier_I));
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void AbortIfOneOrMoreItemsAreNotComparable<XC>(

            //                                              //item could be String or any object (or struct) that
            //                                              //      implement Comparable).
            XC[] array_I,
            //                                              //Ej: arrxxxx¿Name? (xxxx = type prefix)
            String identifier_I
            )
            where XC : IComparable
        {
            Test.AbortIfNullOrEmpty(identifier_I, "identifier_I");
            Test.AbortIfNull(array_I, identifier_I);

            for (int intI = 0; intI < array_I.Length; intI = intI + 1)
            {
                if (
                    array_I[intI] is Enum
                    )
                {
                    if (
                        //                                      //Z_NULL
                        ("" + array_I[intI]) == "Z_NULL"
                        )
                        Test.Abort(identifier_I + "[" + intI + "](null)" + " can not be null",
                            Test.ToLog(array_I, identifier_I));
                }
                else
                {
                    if (
                        array_I[intI] == null
                        )
                        Test.Abort(identifier_I + "[" + intI + "](null)" + " can not be null",
                            Test.ToLog(array_I, identifier_I));
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void AbortIfDuplicate(

            //                                              //comp could be String, bclass (that implement Comparable),
            //                                              //      btuple or bsys (that implement Comparable).
            String[] arraySorted_I,
            //                                              //Ej: arrxxxx¿Name? (xxxx = type prefix)
            String identifier_I
            )
        {
            Test.AbortIfNullOrEmpty(identifier_I, "identifier_I");
            Test.AbortIfNull(arraySorted_I, identifier_I);

            for (int intI = 1; intI < arraySorted_I.Length; intI = intI + 1)
            {
                if (!(
                    String.CompareOrdinal((arraySorted_I[intI - 1]), (arraySorted_I[intI])) < 0
                    ))
                    Test.Abort(
                        Test.ToLog(arraySorted_I[intI], identifier_I + "[" + intI + "]") +
                            " is not in ascending order",
                        Test.ToLog(arraySorted_I, identifier_I));
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void AbortIfDuplicate<XC>(

            //                                              //comp could be String, bclass (that implement Comparable),
            //                                              //      btuple or bsys (that implement Comparable).
            XC[] arraySorted_I,
            //                                              //Ej: arrxxxx¿Name? (xxxx = type prefix)
            String identifier_I
            )
            where XC : IComparable
        {
            Test.AbortIfNullOrEmpty(identifier_I, "identifier_I");
            Test.AbortIfNull(arraySorted_I, identifier_I);

            for (int intI = 1; intI < arraySorted_I.Length; intI = intI + 1)
            {
                if (!(
                    arraySorted_I[intI - 1].CompareTo(arraySorted_I[intI]) < 0
                    ))
                    Test.Abort(
                        Test.z_TowaPRIVATE_ToLogXT(arraySorted_I[intI], identifier_I + "[" + intI + "]") +
                            " is not in ascending order",
                        Test.ToLog(arraySorted_I, identifier_I));
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ABORT IF IS (OR IS NOT) IN SET

        //--------------------------------------------------------------------------------------------------------------
        public static void AbortIfItemIsInSortedSet<XCA, XC>(

            XCA item_I,
            String identifier_I,
            XC[] arraySortedSet_I,
            String identifierSortedSet_I
            )
            where XCA : IComparable where XC : IComparable
        {
            Test.AbortIfNullOrEmpty(identifier_I, "identifier_I");
            Test.AbortIfNullOrEmpty(identifierSortedSet_I, "identifierSortedSet_I");
            Test.AbortIfNull(item_I, identifier_I);
            Test.AbortIfNull(arraySortedSet_I, identifierSortedSet_I);

            if (
                item_I.IsInSortedSet(arraySortedSet_I)
                )
                Test.Abort(Test.z_TowaPRIVATE_ToLogXT(item_I, identifier_I) + " IS in " + identifierSortedSet_I,
                        Test.ToLog(arraySortedSet_I, identifierSortedSet_I));
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void AbortIfItemIsNotInSortedSet<XCA, XC>(

            XCA item_I,
            String identifier_I,
            XC[] arraySortedSet_I,
            String identifierSortedSet_I
            )
            where XCA : IComparable where XC : IComparable
        {
            Test.AbortIfNullOrEmpty(identifier_I, "identifier_I");
            Test.AbortIfNullOrEmpty(identifierSortedSet_I, "identifierSortedSet_I");
            Test.AbortIfNull(item_I, identifier_I);
            Test.AbortIfNull(arraySortedSet_I, identifierSortedSet_I);

            if (
                !item_I.IsInSortedSet(arraySortedSet_I)
                )
                Test.Abort(Test.z_TowaPRIVATE_ToLogXT(item_I, identifier_I) + " IS NOT in " + identifierSortedSet_I,
                        Test.ToLog(arraySortedSet_I, identifierSortedSet_I));
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ABORT IF ONE OR MORE ARE (OR ARE NOT) IN SET

        //--------------------------------------------------------------------------------------------------------------
        public static void AbortIfOneOrMoreCharactersAreInSortedSet(

            String String_I,
            String identifier_I,
            char[] characterArraySorted_I,
            String characterArrayIdentifier_I
            )
        {
            Test.AbortIfNullOrEmpty(identifier_I, "identifier_I");
            Test.AbortIfNullOrEmpty(characterArrayIdentifier_I, "characterArrayIdentifier_I");
            Test.AbortIfNull(String_I, identifier_I);
            Test.AbortIfNull(characterArraySorted_I, characterArrayIdentifier_I);

            for (int intI = 0; intI < String_I.Length; intI = intI + 1)
            {
                if (
                    String_I[intI].IsInSortedSet(characterArraySorted_I)
                    )
                    Test.Abort(
                        Test.z_TowaPRIVATE_ToLogXT(String_I[intI], identifier_I + "[" + intI + "]") + " is in " +
                            characterArrayIdentifier_I,
                        Test.ToLog(characterArraySorted_I, characterArrayIdentifier_I));
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void AbortIfOneOrMoreCharactersAreNotInSortedSet(

            String String_I,
            String identifier_I,
            char[] characterArraySorted_I,
            String characterArrayIdentifier_I
            )
        {
            Test.AbortIfNullOrEmpty(identifier_I, "identifier_I");
            Test.AbortIfNullOrEmpty(characterArrayIdentifier_I, "characterArrayIdentifier_I");
            Test.AbortIfNull(String_I, identifier_I);
            Test.AbortIfNull(characterArraySorted_I, characterArrayIdentifier_I);

            for (int intI = 0; intI < String_I.Length; intI = intI + 1)
            {
                if (
                    !String_I[intI].IsInSortedSet(characterArraySorted_I)
                    )
                    Test.Abort(
                        Test.z_TowaPRIVATE_ToLogXT(String_I[intI], identifier_I + "[" + intI + "]") + " is in " +
                            characterArrayIdentifier_I,
                        Test.ToLog(characterArraySorted_I, characterArrayIdentifier_I));
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void AbortIfOneOrMoreItemsAreInSortedSet<XCA, XC>(

            XCA[] arrxcaToAnalize_I,
            String strIdentifier_I,
            XC[] arrxcSortedSet_I,
            String strIdentifierSortedSet_I
            )
            where XCA : IComparable where XC : IComparable
        {
            Test.AbortIfNullOrEmpty(strIdentifier_I, "strIdentifier_I");
            Test.AbortIfNullOrEmpty(strIdentifierSortedSet_I, "strIdentifierSortedSet_I");
            Test.AbortIfNull(arrxcaToAnalize_I, strIdentifier_I);
            Test.AbortIfNull(arrxcSortedSet_I, strIdentifierSortedSet_I);

            for (int intI = 0; intI < arrxcaToAnalize_I.Length; intI = intI + 1)
            {
                if (
                    arrxcaToAnalize_I[intI].IsInSortedSet(arrxcSortedSet_I)
                    )
                    Test.Abort(
                        Test.z_TowaPRIVATE_ToLogXT(arrxcaToAnalize_I[intI], strIdentifier_I + "[" + intI + "]") +
                        " is in " + strIdentifierSortedSet_I,
                        Test.ToLog(arrxcSortedSet_I, strIdentifierSortedSet_I));
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void AbortIfOneOrMoreItemsAreNotInSortedSet<XCA, XC>(

            XCA[] arrxcaToAnalize_I,
            String strIdentifier_I,
            XC[] arrxcSortedSet_I,
            String strIdentifierSortedSet_I
            )
            where XCA : IComparable where XC : IComparable
        {
            Test.AbortIfNullOrEmpty(strIdentifier_I, "strIdentifier_I");
            Test.AbortIfNullOrEmpty(strIdentifierSortedSet_I, "strIdentifierSortedSet_I");
            Test.AbortIfNull(arrxcaToAnalize_I, strIdentifier_I);
            Test.AbortIfNull(arrxcSortedSet_I, strIdentifierSortedSet_I);

            for (int intI = 0; intI < arrxcaToAnalize_I.Length; intI = intI + 1)
            {
                if (
                    !arrxcaToAnalize_I[intI].IsInSortedSet(arrxcSortedSet_I)
                    )
                    Test.Abort(
                        Test.z_TowaPRIVATE_ToLogXT(arrxcaToAnalize_I[intI], strIdentifier_I + "[" + intI + "]") +
                        " is in " + strIdentifierSortedSet_I,
                        Test.ToLog(arrxcSortedSet_I, strIdentifierSortedSet_I));
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ABORT IF, IN GENERIC TYPES

        //--------------------------------------------------------------------------------------------------------------
        public static void z_TowaPRIVATE_subAbortIfInvalidTKey(
            //                                              //THIS METHOD IS CALLED IN SOME GENERIC TYPE THAT USE TKey.
            //                                              //Verity TKey type is valid

            Type typeKey_I,
            String strTypeContainingKey_I
            )
        {
            Type[] arrtypeVALID = new Type[] { typeof(String), typeof(int), typeof(long), typeof(char), typeof(Date),
                typeof(Time) };
            if (
                !typeKey_I.IsInSet(arrtypeVALID)
                )
            {
                //                                          //Last Type name will be added after Join ( & typeName)
                String[] arrstrVALID_TYPE_NAME_EXCEPT_LAST = new String[arrtypeVALID.Length - 1];
                for (int intI = 0; intI < arrstrVALID_TYPE_NAME_EXCEPT_LAST.Length; intI = intI + 1)
                {
                    arrstrVALID_TYPE_NAME_EXCEPT_LAST[intI] = arrtypeVALID[intI].Name();
                }
                String strVALID_TYPE_NAMES = String.Join(", ", arrstrVALID_TYPE_NAME_EXCEPT_LAST) + " & " +
                    arrtypeVALID[arrtypeVALID.Length - 1].Name();

                Test.Abort(Test.ToLog(typeKey_I, "typeKey_I") + " in not a valid type in " + strTypeContainingKey_I +
                    ", options: " + strVALID_TYPE_NAMES);
            }
        }

        //==============================================================================================================
        /*END-TASK*/

        //--------------------------------------------------------------------------------------------------------------
        /*INITIALIZER*/

        //--------------------------------------------------------------------------------------------------------------
        static Test(
            )
        {
            //                                              //PREPARE CONSTANTS (Verify later).
            //                                              //WARNING: intentionaly I use Array.Sort intead of Std.Sort
            //                                              //      to avoid the use of Test before completing Std
            //                                              //      initialization process.

            //                                              //Prepare constant character
            Test.arrcharDO_NOT_SHOW_HEX = Test.strCHAR_DO_NOT_SHOW_HEX.ToCharArray();
            Array.Sort(Test.arrcharDO_NOT_SHOW_HEX);
            Array.Sort(Test.arrt3fakecharFAKE);
            Array.Sort(Test.arrt2charNONPRINTABLE);
            Test.arrt2charDESCRIPTION =
                new T2charDescriptionTuple[
                    Test.arrt2charNONPRINTABLE.Length + Test.arrt3fakecharFAKE.Length + Std.arrt2charESCAPE.Length];
            Array.Copy(Test.arrt2charNONPRINTABLE, 0, Test.arrt2charDESCRIPTION, 0, Test.arrt2charNONPRINTABLE.Length);
            int intDesp = Test.arrt2charNONPRINTABLE.Length;
            for (int intT3 = 0; intT3 < Test.arrt3fakecharFAKE.Length; intT3 = intT3 + 1)
            {
                Test.arrt2charDESCRIPTION[intDesp + intT3] = new T2charDescriptionTuple(
                    Test.arrt3fakecharFAKE[intT3].charFAKE, Test.arrt3fakecharFAKE[intT3].strDESCRIPTION);
            }
            Array.Copy(Std.arrt2charESCAPE, 0, Test.arrt2charDESCRIPTION,
                Test.arrt2charNONPRINTABLE.Length + Test.arrt3fakecharFAKE.Length, Std.arrt2charESCAPE.Length);
            Array.Sort(Test.arrt2charDESCRIPTION);

            //                                              //Prepare Single Type and Prefix character
            Test.arrstrSINGLE_TYPE = new String[Test.arr2strSINGLE_TYPE_AND_PREFIX.GetLength(0)];
            Test.arrstrSINGLE_PREFIX = new String[Test.arrstrSINGLE_TYPE.Length];
            for (int intI = 0; intI < Test.arrstrSINGLE_TYPE.Length; intI = intI + 1)
            {
                Test.arrstrSINGLE_TYPE[intI] = Test.arr2strSINGLE_TYPE_AND_PREFIX[intI, 0];
                Test.arrstrSINGLE_PREFIX[intI] = Test.arr2strSINGLE_TYPE_AND_PREFIX[intI, 1];
            }
            Array.Sort(Test.arrstrSINGLE_TYPE, Test.arrstrSINGLE_PREFIX, StringComparer.Ordinal);

            //                                              //Prepare GENERIC Type and Prefix character
            Test.arrstrGENERIC_TYPE = new String[Test.arr2strGENERIC_TYPE_AND_PREFIX.GetLength(0)];
            Test.arrstrGENERIC_PREFIX = new String[Test.arrstrGENERIC_TYPE.Length];
            for (int intI = 0; intI < Test.arrstrGENERIC_TYPE.Length; intI = intI + 1)
            {
                Test.arrstrGENERIC_TYPE[intI] = Test.arr2strGENERIC_TYPE_AND_PREFIX[intI, 0];
                Test.arrstrGENERIC_PREFIX[intI] = Test.arr2strGENERIC_TYPE_AND_PREFIX[intI, 1];
            }
            Array.Sort(Test.arrstrGENERIC_TYPE, Test.arrstrGENERIC_PREFIX, StringComparer.Ordinal);

            //                                              //VERIFY CONSTANTS.
            Test.subVerifyConstantsCharacter();
            Test.subVerifySingleTypeAndPrefix();
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
