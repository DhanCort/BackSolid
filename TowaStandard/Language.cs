/*TASK Language*/
using System;

//                                                          //To allow Towa's definition of some types (TimeZoneX, etc.)
//                                                          //      Any reference to System.Globalization type should be
//                                                          //      codes (Ex. System.Globalization.CultureInfo).

//                                                          //AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //CO-AUTHOR: Towa (RGL-Rodrigo Garcia).
//                                                          //DATE: October 19, 2018.


namespace TowaStandard
{
    //==================================================================================================================
    public class Language : BsysAbstract, IComparable
    {
        //                                                  //Notice that constructor is PRIVATE and all posible object
        //                                                  //      are constructed as PUBLIC constants during
        //                                                  //      initialization.

        //                                                  //This class will include any info required to process data
        //                                                  //      (like date & time) that depends on language/culture.
        //                                                  //To implement this class in other Technology Instance (Ex.
        //                                                  //      Java) may require other non-standard content.
        //                                                  //Name is an standard content.
        //                                                  //Culture is non-standard but it is needed in C#.

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        /*CONSTANTS*/

        //                                                  //This PICTURES will be included in all languages
        private static readonly T2languagePictureTuple[] arrt2languageALL_LANGUAGES_PICTURES =
        {
            new T2languagePictureTuple(DateTextEnum.MONTH_SHORT, "MMM"),
            new T2languagePictureTuple(DateTextEnum.MONTH_FULL, "MMMM"),
            new T2languagePictureTuple(DateTextEnum.DAY_OF_WEEK_SHORT, "ddd"),
            new T2languagePictureTuple(DateTextEnum.DAY_OF_WEEK_FULL, "dddd"),
        };

        //                                                  //Follows PICTURES definition for each standard language

        private static readonly T2languagePictureTuple[] arrt2languageENGLISH_PICTURE =
        {
            new T2languagePictureTuple(DateTextEnum.SHORT, "MMM d, yyyy"),
            new T2languagePictureTuple(DateTextEnum.SHORT_YEAR_AND_MONTH, "MMM, yyyy"),
            new T2languagePictureTuple(DateTextEnum.SHORT_MONTH_DAY, "MMM d"),
            new T2languagePictureTuple(DateTextEnum.LONG, "MMMM d, yyyy"),
            new T2languagePictureTuple(DateTextEnum.LONG_YEAR_AND_MONTH, "MMMM, yyyy"),
            new T2languagePictureTuple(DateTextEnum.LONG_MONTH_DAY, "MMMM d"),
            new T2languagePictureTuple(DateTextEnum.FULL, "dddd, MMMM d, yyyy"),
            new T2languagePictureTuple(DateTextEnum.FULL_MONTH_DAY, "dddd, MMMM d"),
        };
        public static readonly Language ENGLISH = new Language("English", arrt2languageENGLISH_PICTURE,
            new System.Globalization.CultureInfo("en-US"));

        private static readonly T2languagePictureTuple[] arrt2languageSPANISH_PICTURE =
        {
            new T2languagePictureTuple(DateTextEnum.SHORT, "d MMM yyyy"),
            new T2languagePictureTuple(DateTextEnum.SHORT_YEAR_AND_MONTH, "MMM yyyy"),
            new T2languagePictureTuple(DateTextEnum.SHORT_MONTH_DAY, "d MMM"),
            new T2languagePictureTuple(DateTextEnum.LONG, "d 'de' MMMM 'de' yyyy"),
            new T2languagePictureTuple(DateTextEnum.LONG_YEAR_AND_MONTH, "MMMM 'de' yyyy"),
            new T2languagePictureTuple(DateTextEnum.LONG_MONTH_DAY, "d 'de' MMMM"),
            new T2languagePictureTuple(DateTextEnum.FULL, "dddd, d 'de' MMMM 'de' yyyy"),
            new T2languagePictureTuple(DateTextEnum.FULL_MONTH_DAY, "dddd, d 'de' MMMM"),
        };
        public static readonly Language SPANISH = new Language("Spanish", arrt2languageSPANISH_PICTURE,
            new System.Globalization.CultureInfo("es-MX"));

        private static readonly T2languagePictureTuple[] arrt2languageFRENCH_PICTURE =
        {
            new T2languagePictureTuple(DateTextEnum.SHORT, "d MMM yyyy"),
            new T2languagePictureTuple(DateTextEnum.SHORT_YEAR_AND_MONTH, "MMM yyyy"),
            new T2languagePictureTuple(DateTextEnum.SHORT_MONTH_DAY, "d MMM"),
            new T2languagePictureTuple(DateTextEnum.LONG, "d MMMM yyyy"),
            new T2languagePictureTuple(DateTextEnum.LONG_YEAR_AND_MONTH, "MMMM yyyy"),
            new T2languagePictureTuple(DateTextEnum.LONG_MONTH_DAY, "d MMMM"),
            new T2languagePictureTuple(DateTextEnum.FULL, "dddd, 'le' d MMMM yyyy"),
            new T2languagePictureTuple(DateTextEnum.FULL_MONTH_DAY, "dddd d MMMM"),
        };
        public static readonly Language FRENCH = new Language("French", arrt2languageFRENCH_PICTURE,
            new System.Globalization.CultureInfo("fr-FR"));

        //                                                  //Temporal languages used to test time formats
        //                                                  //The specification for each of the following
        //                                                  //      T2languagaPictureTuple is invented

        //                                                  //AM designator: ҮӨ; PM designator: ҮХ
        private static readonly T2languagePictureTuple[] arrt2languageMONGOLIAN_PICTURE =
        {
            new T2languagePictureTuple(DateTextEnum.SHORT, "MMM d, yyyy"),
            new T2languagePictureTuple(DateTextEnum.SHORT_YEAR_AND_MONTH, "MMM, yyyy"),
            new T2languagePictureTuple(DateTextEnum.SHORT_MONTH_DAY, "MMM d"),
            new T2languagePictureTuple(DateTextEnum.LONG, "MMMM d, yyyy"),
            new T2languagePictureTuple(DateTextEnum.LONG_YEAR_AND_MONTH, "MMMM, yyyy"),
            new T2languagePictureTuple(DateTextEnum.LONG_MONTH_DAY, "MMMM d"),
            new T2languagePictureTuple(DateTextEnum.FULL, "dddd, MMMM d, yyyy"),
            new T2languagePictureTuple(DateTextEnum.FULL_MONTH_DAY, "dddd, MMMM d"),
        };
        public static readonly Language MONGOLIAN = new Language("Mongolian", arrt2languageMONGOLIAN_PICTURE,
            new System.Globalization.CultureInfo("mn"));

        //                                                  //AM designator: r.n.; PM designator: i.n.
        private static readonly T2languagePictureTuple[] arrt2languageIRISH_PICTURE =
        {
            new T2languagePictureTuple(DateTextEnum.SHORT, "MMM d, yyyy"),
            new T2languagePictureTuple(DateTextEnum.SHORT_YEAR_AND_MONTH, "MMM, yyyy"),
            new T2languagePictureTuple(DateTextEnum.SHORT_MONTH_DAY, "MMM d"),
            new T2languagePictureTuple(DateTextEnum.LONG, "MMMM d, yyyy"),
            new T2languagePictureTuple(DateTextEnum.LONG_YEAR_AND_MONTH, "MMMM, yyyy"),
            new T2languagePictureTuple(DateTextEnum.LONG_MONTH_DAY, "MMMM d"),
            new T2languagePictureTuple(DateTextEnum.FULL, "dddd, MMMM d, yyyy"),
            new T2languagePictureTuple(DateTextEnum.FULL_MONTH_DAY, "dddd, MMMM d"),
        };
        public static readonly Language IRISH = new Language("Irish", arrt2languageIRISH_PICTURE,
            new System.Globalization.CultureInfo("ga"));

        //                                                  //AM designator: prijepodne; PM designator: popodne
        private static readonly T2languagePictureTuple[] arrt2languageBOSNIAN_PICTURE =
        {
            new T2languagePictureTuple(DateTextEnum.SHORT, "MMM d, yyyy"),
            new T2languagePictureTuple(DateTextEnum.SHORT_YEAR_AND_MONTH, "MMM, yyyy"),
            new T2languagePictureTuple(DateTextEnum.SHORT_MONTH_DAY, "MMM d"),
            new T2languagePictureTuple(DateTextEnum.LONG, "MMMM d, yyyy"),
            new T2languagePictureTuple(DateTextEnum.LONG_YEAR_AND_MONTH, "MMMM, yyyy"),
            new T2languagePictureTuple(DateTextEnum.LONG_MONTH_DAY, "MMMM d"),
            new T2languagePictureTuple(DateTextEnum.FULL, "dddd, MMMM d, yyyy"),
            new T2languagePictureTuple(DateTextEnum.FULL_MONTH_DAY, "dddd, MMMM d"),
        };
        public static readonly Language BOSNIAN = new Language("Bosnian", arrt2languageBOSNIAN_PICTURE,
            new System.Globalization.CultureInfo("bs-Latn-BA"));

        //--------------------------------------------------------------------------------------------------------------
        /*INITIALIZER*/

        //--------------------------------------------------------------------------------------------------------------
        static Language(
            )
        {
        }

        //--------------------------------------------------------------------------------------------------------------
        /*INSTANCE VARIABLES*/

        public /*KEY*/ String Name;
        public T2languagePictureTuple[] PICTURES;

        //                                                  //Required in C# to access language(culture) information.
        //                                                  //To implement this class in other Technology Instance (Ex.
        //                                                  //      Java) may require other non-standard content.
        public System.Globalization.CultureInfo culture;

        //--------------------------------------------------------------------------------------------------------------
        public override String ToLogShort()
        {
            return "<" + Test.ToLog(this.Name) + ", " + Test.ToLog(this.PICTURES) + ">";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public override String ToLogFull()
        {
            return "<" + Test.ToLog(this.Name, "Name") + ", " + Test.ToLog(this.PICTURES, "PICTURES") + ">";
        }

        //--------------------------------------------------------------------------------------------------------------
        /*OBJECT CONSTRUCTORS*/

        //--------------------------------------------------------------------------------------------------------------
        private Language(
            //                                              //this.*[O], assing values. 

            String strName_I,
            //                                              //arrt2languagePicture_I and
            //                                              //      Language.arrt2languageALL_LANGUAGES_PICTURES should
            //                                              //      incluce all posible option as defined in
            //                                              //      DateTextEnum.
            //                                              //Should be order in the same sequence.
            T2languagePictureTuple[] arrt2languagePicture_I,
            System.Globalization.CultureInfo culture_I
            )
        {
            Test.AbortIfNullOrEmpty(strName_I, "strName_I");
            Test.AbortIfNullOrEmpty(arrt2languagePicture_I, "arrt2languagePicture_I");
            Test.AbortIfNull(culture_I, "culture");

            //                                              //Verify language, examples: English, Spanish
            String strNameValid = strName_I.Substring(0, 1).ToUpper() + strName_I.Substring(1).ToLower();
            if (
                strName_I != strNameValid
                )
                Test.Abort(Test.ToLog(strName_I, "") + " should be \"" + strNameValid + "\"");

            this.Name = strName_I;
            this.culture = culture_I;

            T2languagePictureTuple[] arrt2languagePicture =
                new T2languagePictureTuple[
                arrt2languagePicture_I.Length + TowaStandard.Language.arrt2languageALL_LANGUAGES_PICTURES.Length];
            Array.Copy(arrt2languagePicture_I, 0, arrt2languagePicture, 0, arrt2languagePicture_I.Length);
            Array.Copy(arrt2languageALL_LANGUAGES_PICTURES, 0, arrt2languagePicture, arrt2languagePicture_I.Length,
                arrt2languageALL_LANGUAGES_PICTURES.Length);

            //                                              //FULL_MONTH_DAY is last in enum set  
            const int intArrt2Length = (int)DateTextEnum.FULL_MONTH_DAY + 1;
            if (
                arrt2languagePicture_I.Length != intArrt2Length
                )
                Test.Abort(
                    "arrt2languagePicture_I.Length(" + arrt2languagePicture_I.Length + ") should be EXACTLY " +
                        intArrt2Length + " tuples",
                    Test.ToLog(this.Name, "this.Name"), "this.culture(" + this.culture.ToString() + ")");

            this.PICTURES = arrt2languagePicture;

            Test.AbortIfDuplicate(this.PICTURES, "this.PICTURES");
            this.subVerifyPictures();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private void subVerifyPictures(
            //                                              //Verify pictures are operable
            )
        {
            DateTime dtTEST = new DateTime(2018, 11, 10);

            for (int intI = 0; intI < this.PICTURES.Length; intI = intI + 1)
            {
                //                                          //Verify it works
                try
                {
                    String str = dtTEST.ToString(this.PICTURES[intI].strPicture, this.culture);
                }
                catch (Exception sysexceptError)
                {
                    String strText = "this.PICTURES[" + intI + "].strPicture";
                    Test.Abort(
                        Test.ToLog(this.PICTURES[intI].strPicture, strText) +
                        "can not be standard picture, it does not work",
                        Test.ToLog(this.Name, "this.Name"), "this.culture(" + this.culture.ToString() + ")",
                        Test.ToLog(this.PICTURES, "this.PICTURES"), Test.ToLog(sysexceptError, "sysexceptError"));
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        /*ACCESS METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public override String ToString()
        {
            return "<" + this.Name + ", " + this.culture.ToString() + ">";
        }

        //--------------------------------------------------------------------------------------------------------------
        public int CompareTo(Object objArgument_I)
        {
            int intCompareTo;
            /*CASE*/
            if (
                objArgument_I is Language
                )
            {
                intCompareTo = String.CompareOrdinal(this.Name, ((Language)objArgument_I).Name);
            }
            else if (
                objArgument_I is String
                )
            {
                intCompareTo = String.CompareOrdinal(this.Name, (String)objArgument_I);
            }
            else
            {
                Test.Abort(
                    Test.ToLog(objArgument_I.GetType(), "objArgument_I.type") +
                        " is not a compatible CompareTo argument, the options are: Language & String",
                    Test.ToLog(this.GetType(), "this.type"));

                intCompareTo = -1;
            }
            /*CASE*/

            return intCompareTo;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
