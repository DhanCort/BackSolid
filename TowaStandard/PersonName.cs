/*TASK PersonName*/
using System;

//                                                          //AUTHOR: Towa (RPM-Rubén de la Peña, JJFM-Juan Jose Flores,
//                                                          //      LGCR-Leoncio Chiunty).
//                                                          //CO-AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //DATE: August 22, 2019.

namespace TowaStandard
{
    //==================================================================================================================
    public struct PersonName : BsysInterface, IComparable
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //Components of the person name.

        //--------------------------------------------------------------------------------------------------------------
        /*CONSTANTS*/

        //                                                  //Valid chars in name
        private const String strCHARS_IN_NAME =
            //                                              //(GLG, AUGUST 30, 2019) In the future as we incorporate other
            //                                              //      countries, cultures and lenguages, we should add other
            //                                              //      characters.
            " " + "0123456789" + "ABCDEFGHIJKLMNOPQRSTUVWXYZ" + "abcdefghijklmnopqrstuvwxyz" +
            //                                              //Accents and Ñ ñ.
            "ÁÉÍÓÚ" + "áéíóú" + "ÀÈÌÒÙ" + "àèìòù" + "ÄËÏÖÜ" + "äëïöü" + "ÂÊÎÔÛ" + "âêîôû" + "Ññ";
        private static readonly char[] arrcharIN_NAME;

        //                                                  //To produce: GivenName + 'separator' + FamilyName.
        private const char charSEPARATOR = '\uFFFF';

        private const char charTO_REPLACE_INVALID = '?';

        //--------------------------------------------------------------------------------------------------------------
        /*INITIALIZER*/

        //--------------------------------------------------------------------------------------------------------------
        static PersonName(
            //                                              //Prepare and verify constants
            )
        {
            //                                              //Prepare & verify CHAR_IN_NAME.

            Test.AbortIfNullOrEmpty(strCHARS_IN_NAME, "strCHARS_IN_NAME");
            Test.AbortIfOneOrMoreCharactersAreNotInSortedSet(strCHARS_IN_NAME, "strCHARS_IN_NAME",
                Std.CHARS_USEFUL_IN_TEXT, "Std.CHARS_USEFUL_IN_TEXT");

            arrcharIN_NAME = strCHARS_IN_NAME.ToCharArray();
            Std.Sort(arrcharIN_NAME);

            Test.AbortIfDuplicate(arrcharIN_NAME, "arrcharIN_NAME");
        }

        //--------------------------------------------------------------------------------------------------------------
        /*INSTANCE VARIABLES*/

        //                                                  //Given name of a person (Ex. John).
        public readonly String given;

        //                                                  //Family name of a person (Ex. Smith).
        public readonly String family;

        //--------------------------------------------------------------------------------------------------------------
        /*COMPUTED VARIABLES*/

        //                                                  //The full name of a person (Ex. John Smith).
        public String fullName
        {
            get
            {
                return this.given + " " + this.family;
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public String ToLogShort()
        {
            return "<" + Test.ToLog(this.given) + ", " + Test.ToLog(this.family) + ">";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public String ToLogFull()
        {
            return "<" + Test.ToLog(Test.ToLog(this.given, "given") + ", " + Test.ToLog(this.family, "family") + ">");
        }

        //--------------------------------------------------------------------------------------------------------------
        /*OBJECT CONSTRUCTORS*/

        //--------------------------------------------------------------------------------------------------------------
        public PersonName(
            //                                              //Creates a new PersonName object.

            //                                              //This has been verify
            JsonPersonName json_I
            )
        {
            this.given = json_I.given;
            this.family = json_I.family;
        }

        //--------------------------------------------------------------------------------------------------------------
        public PersonName(
            //                                              //Creates a new PersonName object.

            //                                              //Given name of a person (Ex. John).
            String given_I,
            //                                              //Family name of a person (Ex. Smith).
            String family_I
            )
        {
            this.given = given_I.TrimExcel();
            this.family = family_I.TrimExcel();

            if (
                PersonName.z_TowaPRIVATE_boolIsValidGivenOrFamiliName(this.given) ||
                (this.given.Length == 0)
                )
                Test.Abort(Test.ToLog(this.given, "this.Given") + " includes invalid chars or missing");
            if (
                PersonName.z_TowaPRIVATE_boolIsValidGivenOrFamiliName(this.family) ||
                (this.family.Length == 0)
                )
                Test.Abort(Test.ToLog(this.family, "this.Family") + " includes invalid chars or missing");
        }

        //--------------------------------------------------------------------------------------------------------------
        public PersonName(
            //                                              //Creates a new PersonName object.

            //                                              //Complex version (from data base entity).
            //                                              //WE ASSUME IT IS CLEAN, no verification is needed.
            //                                              //Family Name/Given Name
            String complexPersonName_I
            )
        {
            String[] arrstrNamePart = complexPersonName_I.Split(PersonName.charSEPARATOR);

            if (
                arrstrNamePart.Length != 2
                )
                Test.Abort(Test.ToLog(complexPersonName_I) + " should be exactly 2 parts",
                    Test.ToLog(arrstrNamePart, "arrstrNamePart"));

            this.given = arrstrNamePart[0];
            this.family = arrstrNamePart[1];
        }

        //--------------------------------------------------------------------------------------------------------------
        /*TRANSFORMATION METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        /*ACCESS METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public String ToText(
            //                                              //str, "<given, family>".
            )
        {
            return this.ToLogShort();
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool z_TowaPRIVATE_boolIsValidGivenOrFamiliName(

            String strName_I
            )
        {
            return (
                strName_I.IsTextValid(PersonName.arrcharIN_NAME, '-') &&
                (strName_I.Length > 0)
                );
        }

        //--------------------------------------------------------------------------------------------------------------
        public String ComplexPersonName(
            //                                              //Creates a complex version of the object.

            //                                              //str, Complex version.
            )
        {
            return this.given + PersonName.charSEPARATOR + this.family;
        }

        //--------------------------------------------------------------------------------------------------------------
        public int CompareTo(
            //                                              //Required for Sort, BinarySearch and CompareTo.
            //                                              //Order is: Family, Given

            //                                              //this[I], object key info.

            //                                              //PersonName
            Object obj_I
            )
        {
            if (!(
                obj_I is PersonName
                ))
                Test.Abort(Test.ToLog(obj_I.GetType(), "obj_L.GetType") + " should be PersonName");

            PersonName pnToCompare = (PersonName)obj_I;

            int intCompareTo = this.family.CompareTo(pnToCompare.family);
            if (
                intCompareTo == 0
                )
            {
                intCompareTo = this.given.CompareTo(pnToCompare.given);
            }

            return intCompareTo;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
