/*TASK Database Database All-in-Memory*/
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

//                                                          //AUTHOR: Towa (AQG-Andrea Quiroz, LGF-Liliana Gutierrez).
//                                                          //CO-AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //DATE: October 15, 2018.

namespace TowaStandard
{
    //==================================================================================================================
    public abstract class Entity : BclassAbstract
    {
        //--------------------------------------------------------------------------------------------------------------
        /*CONSTANTS*/

        public override BclassmutabilityEnum bclassmutability { get { return BclassmutabilityEnum.MUTABLE; } }

        //                                                  //<<00010101-99991231>> Life forever 
        protected static readonly Life FullLife = new Life(Date.MinValue);

        //                                                  //Ex. "Organization"
        public abstract String entityName { get; }

        //                                                  //Parameters required to verify String ID.
        //                                                  //Entities with String ID (most of them will) should
        //                                                  //      override this virtual constants.
        protected virtual char[] arrcharIN_ID { get { return null; } }
        //                                                  //uFFFF means NO union character, Options: '.', '-' & '_'.
        protected virtual char charUNION { get { return '\uFFFF'; } }
        //                                                  //Should be 2-12. Id length should be from 2 to this value
        protected virtual int intMAX_LEN { get { return 0; } }

        //                                                  //Pattern (regex), null menas no pattern verification.
        //                                                  //Pattern should include anchor characters.
        //                                                  //Ex. PATTERN "^[A-Z]{3,4}[1-9]{1}[0-9]{0,4}$".
        protected virtual Regex regexID { get { return null; } }
        protected virtual String strPATTERN_DESCRIPTION { get { return null; } }
        protected virtual String[] arrstrOK_EXAMPLES { get { return null; } }
        protected virtual String[] arrstrNOT_OK_EXAMPLES { get { return null; } }

        //--------------------------------------------------------------------------------------------------------------
        /*INITIALIZER*/

        //--------------------------------------------------------------------------------------------------------------
        /*SUPPORT METHODS FOR INITIALIZER*/

        //--------------------------------------------------------------------------------------------------------------
        protected static char[] PrepareCharsId(
            //                                              //INITIALIZER FOR CLASS'S entity, if Id is String, should
            //                                              //      call this method.
            //                                              //arrchar, CharsId sorted

            String strCHARS_ID_I
            )
        {
            Test.AbortIfNullOrEmpty(strCHARS_ID_I, "strCHARS_ID_I");
            Test.AbortIfOneOrMoreCharactersAreNotInSortedSet(strCHARS_ID_I, "strCHARS_ID_I", Std.CHARS_USEFUL_IN_TEXT,
                "Std.CHARS_USEFUL_IN_TEXT");

            char[] arrcharPrepareCharsIdX = strCHARS_ID_I.ToCharArray();
            Std.Sort(arrcharPrepareCharsIdX);
            Test.AbortIfDuplicate(arrcharPrepareCharsIdX, "arrcharPrepareCharsIdX");

            return arrcharPrepareCharsIdX;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        protected static void VerifyCharsInId(
            //                                              //INITIALIZER FOR CLASS'S entity, if Id is String, should
            //                                              //      call this method.
            //                                              //Verify and prepare all constants.

            //                                              //To access all constants.
            Entity entityDUMMY_I
            )
        {
            Std.Sort(entityDUMMY_I.arrcharIN_ID);
            Test.AbortIfDuplicate(entityDUMMY_I.arrcharIN_ID, "entityDUMMY_I.arrcharIN_ID");

            if (
                entityDUMMY_I.charUNION != '\uFFFF'
                )
            {
                if (
                    !entityDUMMY_I.charUNION.IsInSet('.', '-', '_')
                    )
                    Test.Abort(Test.ToLog(entityDUMMY_I.charUNION, "entityDUMMY_I.charUNION") +
                        " is not valid, optiona are: '.', '-', '_'");
            }

            if (
                !entityDUMMY_I.intMAX_LEN.IsBetween(2, 12)
                )
                Test.Abort(Test.ToLog(entityDUMMY_I.intMAX_LEN, "entityDUMMY_I.longMAX_LEN") +
                    " should be in the range 2-12");
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        protected static void VerifyRegexId(
            //                                              //INITIALIZER FOR CLASS'S entity, if Id verification
            //                                              //      requires string id, should call this method.
            //                                              //Verify regex.

            //                                              //To access all constants.
            Entity entityDUMMY_I
            )
        {
            if (
                entityDUMMY_I.regexID == null
                )
            {
                //                                          //3 more constant should be null
                if (
                    entityDUMMY_I.strPATTERN_DESCRIPTION != null
                    )
                    Test.Abort(
                        Test.ToLog(entityDUMMY_I.strPATTERN_DESCRIPTION, "strPATTERN_DESCRIPTION") + " should be null");
                if (
                    entityDUMMY_I.arrstrOK_EXAMPLES != null
                    )
                    Test.Abort(
                        Test.ToLog(entityDUMMY_I.arrstrOK_EXAMPLES, "arrstrOK_EXAMPLES") + " should be null");
                if (
                    entityDUMMY_I.arrstrNOT_OK_EXAMPLES != null
                    )
                    Test.Abort(
                        Test.ToLog(entityDUMMY_I.arrstrNOT_OK_EXAMPLES, "arrstrNOT_OK_EXAMPLES") + " should be null");
            }
            else
            {
                Test.AbortIfNullOrEmpty(entityDUMMY_I.strPATTERN_DESCRIPTION, "strPATTERN_DESCRIPTION");
                Test.AbortIfNullOrEmpty(entityDUMMY_I.arrstrOK_EXAMPLES, "arrstrOK_EXAMPLES");
                Test.AbortIfNullOrEmpty(entityDUMMY_I.arrstrNOT_OK_EXAMPLES, "arrstrNOT_OK_EXAMPLES");

                Test.AbortIfOneOrMoreCharactersAreNotInSortedSet(entityDUMMY_I.strPATTERN_DESCRIPTION,
                    "strPATTERN_DESCRIPTION", Std.CHARS_USEFUL_IN_TEXT, "Std.CHARS_USEFUL_IN_TEXT");

                foreach (String str in entityDUMMY_I.arrstrOK_EXAMPLES)
                {
                    if (
                        !entityDUMMY_I.regexID.IsMatch(str)
                        )
                        Test.Abort(Test.ToLog(str, "str") + " should be accepted in Regex Test",
                            Test.ToLog(entityDUMMY_I.arrstrOK_EXAMPLES, "arrstrOK_EXAMPLES"));
                }

                foreach (String str in entityDUMMY_I.arrstrNOT_OK_EXAMPLES)
                {
                    if (
                        entityDUMMY_I.regexID.IsMatch(str)
                        )
                        Test.Abort(Test.ToLog(str, "str") + " should be rejected in Regex Test",
                            Test.ToLog(entityDUMMY_I.arrstrOK_EXAMPLES, "arrstrOK_EXAMPLES"));
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        protected static void VerifyCharsInText(
            //                                              //INITIALIZER FOR CLASS'S entity, for any set of chars
            //                                              //      used in a text variable.

            //                                              //Verify chars in name or any other set of chars used in
            //                                              //      text

            Char[] arrcharIN_TEXT_I,
            String strTEXT_DESCRIPTION_I
            )
        {
            Test.AbortIfNullOrEmpty(strTEXT_DESCRIPTION_I, "strTEXT_DESCRIPTION_I");
            Test.AbortIfNullOrEmpty(arrcharIN_TEXT_I, strTEXT_DESCRIPTION_I);

            Test.AbortIfOneOrMoreItemsAreNotInSortedSet(arrcharIN_TEXT_I, strTEXT_DESCRIPTION_I,
                Std.CHARS_USEFUL_IN_TEXT, "Std.CHARS_USEFUL_IN_TEXT");
            Test.AbortIfDuplicate(arrcharIN_TEXT_I, strTEXT_DESCRIPTION_I);
        }

        //--------------------------------------------------------------------------------------------------------------
        /*INSTANCE VARIABLES*/

        //                                                  //To lock THIS entity
        public readonly ToLock tolockEntity = new ToLock();

        //                                                  //Primary Key.
        private long longPk_Z;
        public long primaryKey { get { return this.longPk_Z; } }

        //--------------------------------------------------------------------------------------------------------------
        /*COMPUTED VARIABLES*/

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //Following 3 compute varibles are required to provide info
        //                                                  //      to objects Entity

        //                                                  //-1 if BelongsTo do not exist
        public abstract long primaryKeyBelongsTo { get; }

        public abstract Object key { get; }

        //                                                  //In no life, return <<00010101-99991231>> forever
        public abstract Life lifeX { get; }

        //--------------------------------------------------------------------------------------------------------------
        /*COMPUTED VARIABLES*/

        //--------------------------------------------------------------------------------------------------------------
        protected override void ResetOneClass()
        {
        }

        //--------------------------------------------------------------------------------------------------------------
        public override String ToLogShort()
        {
            return base.ToLogShort() + ", " + Test.ToLog(this.tolockEntity) + ", " + Test.ToLog(this.primaryKey);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public override String ToLogFull()
        {
            const String strCLASS = "Entity";

            return strCLASS + "{" + Test.ToLog(this.tolockEntity) + ", " + Test.ToLog(this.primaryKey, "PrimaryKey") +
                "}==>" + base.ToLogFull();
        }

        //--------------------------------------------------------------------------------------------------------------
        /*OBJECT CONSTRUCTORS*/

        //--------------------------------------------------------------------------------------------------------------
        protected Entity() : base(true) { }

        //--------------------------------------------------------------------------------------------------------------
        protected Entity(
            long PrimaryKey_I
            )
            : base(false)
        {
            this.longPk_Z = PrimaryKey_I;
        }

        //--------------------------------------------------------------------------------------------------------------
        /*OBJECT FACTORY*/

        //--------------------------------------------------------------------------------------------------------------
        public abstract Entity EntityNew(JsonAbstract jsonadd_I);

        //--------------------------------------------------------------------------------------------------------------
        /*TRANSFORMATION METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public abstract void RollBackAdd();

        //--------------------------------------------------------------------------------------------------------------
        public void z_TowaPRIVATE_subAssignPk(
            //                                              //After inserting a entity in DB, should be retrived again
            //                                              //      to get authomatic assigned PK.
            //                                              //Then use this method to update entity.

            long longPrimaryKeyDb_I
            )
        {
            if (
                this.primaryKey >= 0
                )
                Test.Abort(Test.ToLog(this.primaryKey, "this.PrimaryKey") + " should be NON VALID",
                    Test.ToLog(longPrimaryKeyDb_I, "longPrimaryKeyDb_I"), Test.ToLog(this));

            //                                              //Correct entity
            this.longPk_Z = longPrimaryKeyDb_I;

        }

        //--------------------------------------------------------------------------------------------------------------
        /*ACCESS METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public abstract List<Error> VerifyIdJsonOkToGet(JsonAbstract json_I);


        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public List<Error> VerifyStringId(
            //                                              //Verify string id has valid form:
            //                                              //1. Visible characters.
            //                                              //2. Valid characters, and 0 or more separators.
            //                                              //3. 2 to n chars.
            //                                              //4. Pattern (Regex).
            //                                              //5. More specific verification should be coded in concrete
            //                                              //      entity (override VerifyIdEntitySpecific).

            //                                              //this(I), Access to constants 
            //                                              //Id with required case
            String id_I
            )
        {
            List<Error> darrerror = new List<Error>();

            if (
                //                                          //At least one non visible character
                !id_I.IsTextValid()
                )
            {
                Error errorX = new Error("Entity-10", "Entity Id",
                    Test.ToLog(id_I, "Id") + " contains non visible character");
                darrerror.Add(errorX);
            }
            else
            {
                if (
                    //                                      //At least one invalid character
                    !id_I.IsTextValid(this.arrcharIN_ID, charUNION)
                    )
                {
                    Error errorX = new Error("Entity-11", "Entity Id",
                        "StringId(" + id_I + ") one or more invalid characters");
                    darrerror.Add(errorX);
                }
                else
                {
                    if (
                        !id_I.Length.IsBetween(2, this.intMAX_LEN)
                        )
                    {
                        Error errorX = new Error("Entity-12", "Entity Id",
                            "StringId(" + id_I + ") should contant 2-" + this.intMAX_LEN + " characters");
                        darrerror.Add(errorX);
                    }
                    else
                    {
                        if (!(
                            //                              //Requires regex verification and it is ok
                            (this.regexID == null) || regexID.IsMatch(id_I)
                            ))
                        {
                            Error errorX = new Error("Entity-13", "Entity Id",
                                "StringId(" + id_I + ") pattern should have the form:  " + this.strPATTERN_DESCRIPTION);
                            darrerror.Add(errorX);
                        }
                    }
                }
            }

            this.VerifyStringIdEntitySpecific(darrerror, id_I);

            return darrerror;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        protected virtual void VerifyStringIdEntitySpecific(
            //                                              //THIS "virtual" METHOD SHOULD BE OVERRIDE IF REQUIRED

            //                                              //this(I), required to be a virtual method 

            List<Error> darrerror_M,
            //                                              //Most verification was done in previous method.
            String id_I
            )
        {
            //                                              //NOTHING TO DO IN THIS "virtual" METHOD.
        }

        //--------------------------------------------------------------------------------------------------------------
        public abstract List<Error> VerifyIsJsonOkToAdd(JsonAbstract jsonadd_I);

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        protected void VerifyDate(
            //                                              //Verify belongs to.

            List<Error> darrerror_M,
            String date_I,
            String dateDescription_I,
            //                                              //Date parsed or "0001-01-01" if could not parse
            out Date date_O
            )
        {
            Test.AbortIfNullOrEmpty(dateDescription_I, "dateDescription_I");

            date_O = Date.MinValue;

            if (
                date_I.IsTextValid()
                )
            {
                Error errorX = new Error("Entity-13", dateDescription_I,
                    Test.ToLog(date_I, date_I) + " contains non visible character");
                darrerror_M.Add(errorX);
            }
            else
            {
                if (
                    date_I.IsParsableToDate()
                    )
                {
                    date_O = date_I.ParseToDate();
                }
                else
                {
                    /*Ex.ZError
                    ZError zerrorX = TablezerrorError.tablezerro[-1, "Entity-14"];
                    Error errorX = new Error("Entity-14", dateDescription_I,
                        String.Format(zerrorX.TextFormat, dateDescription_I, date_I));
                    */
                    Error errorX = new Error("Entity-14", dateDescription_I,
                        String.Format("{0}({1}) is not valid (could not parse to date)", dateDescription_I, date_I));
                    darrerror_M.Add(errorX);
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        protected void VerifyBelongsToAndDateStart(
            //                                              //Verify belongs to.

            List<Error> darrerror_M,
            Entity entityDUMMY_BELONGS_TO_I,
            long primaryKeyBelongsTo_I,
            Date startLife_I
            )
        {
            //                                              //To easy code
            String strENTITY_BELONGS_TO = entityDUMMY_BELONGS_TO_I.GetType().Name();

            Entity entityBelongsTo = entityDUMMY_BELONGS_TO_I.GetEntryFromDatabase(primaryKeyBelongsTo_I);
            if (
                entityBelongsTo == null
                )
            {
                Error errorX = new Error("Entity-1", "PrimaryKeyBelongsTo",
                    "primaryKey" + strENTITY_BELONGS_TO + "BelongsTo(" + primaryKeyBelongsTo_I + ") was not found");
                darrerror_M.Add(errorX);
            }
            else
            {
                if (
                    entityBelongsTo.lifeX.IsValidDateToStartPeriod(startLife_I)
                    )
                {
                    Error errorX = new Error("Entity-21", "StartLife",
                        "StartLife(" + startLife_I.ToText() + ") is not valid in " + strENTITY_BELONGS_TO + "(" +
                        entityBelongsTo.key + ") with life(" + entityBelongsTo.lifeX.ToText() + ")");
                    darrerror_M.Add(errorX);
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        protected void VerifyText(
            //                                              //Verify text (Ex. Name)

            List<Error> darrerror_M,
            String text_I,
            String textDescription_I,
            char[] CHARS_VALID_IN_TEXT_I
            )
        {
            if (
                text_I.Length == 0
                )
            {
                Error errorX = new Error("Entity-30", textDescription_I,
                    Test.ToLog(text_I, textDescription_I) + " should have some info");
                darrerror_M.Add(errorX);
            }
            else
            {
                if (
                    //                                      //At least one non visible character
                    !text_I.IsTextValid(Std.CHARS_USEFUL_IN_TEXT)
                    )
                {
                    Error errorX = new Error("Entity-31", textDescription_I,
                        Test.ToLog(text_I, textDescription_I) + " non visible character");
                    darrerror_M.Add(errorX);
                }
                else
                {
                    if (
                        //                                  //At least one character is not valid
                        !text_I.IsTextValid(CHARS_VALID_IN_TEXT_I)
                        )
                    {
                        Error errorX = new Error("Entity-32", textDescription_I,
                            Test.ToLog(text_I, textDescription_I) + " invalid character or is missing");
                        darrerror_M.Add(errorX);
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        protected void subVerifyPersonName(
            //                                              //Verify text (Ex. Name)

            List<Error> darrerror_M,
            JsonPersonName jsonPersonName_I,
            String personNameDescription_I
            )
        {
            //                                              //Verify Given
            if (
                //                                          //At least one non visible character
                !jsonPersonName_I.given.IsTextValid(Std.CHARS_USEFUL_IN_TEXT)
                )
            {
                Error errorX = new Error("Entity-35", "[Given]" + personNameDescription_I,
                    Test.ToLog(jsonPersonName_I.given, "[Given]" + personNameDescription_I) + " non visible character");
                darrerror_M.Add(errorX);
            }
            else
            {
                if (
                    PersonName.z_TowaPRIVATE_boolIsValidGivenOrFamiliName(jsonPersonName_I.given)
                    )
                {
                    Error errorX = new Error("Entity-36", "[Given]" + personNameDescription_I,
                        personNameDescription_I + "(" + jsonPersonName_I.given + ") is not valid or is missing");
                    darrerror_M.Add(errorX);
                }
            }

            //                                              //Verify Family
            if (
                //                                          //At least one non visible character
                !jsonPersonName_I.family.IsTextValid(Std.CHARS_USEFUL_IN_TEXT)
                )
            {
                Error errorX = new Error("Entity-35", "[Family]" + personNameDescription_I,
                    Test.ToLog(jsonPersonName_I.family, "[Family]" + personNameDescription_I) + " non visible character");
                darrerror_M.Add(errorX);
            }
            else
            {
                if (
                    PersonName.z_TowaPRIVATE_boolIsValidGivenOrFamiliName(jsonPersonName_I.family)
                    )
                {
                    Error errorX = new Error("Entity-36", "[Family]" + personNameDescription_I,
                        personNameDescription_I + "(" + jsonPersonName_I.family + ") is not valid or is missing");
                    darrerror_M.Add(errorX);
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        protected void subVerifyCurrency(
            //                                              //Verify text (Ex. Name)

            List<Error> darrerror_M,
            String strCurrency_I, 
            String strCurrencyDescription_I,
            Currency currMIN_I,
            Currency currMAX_I
            )
        {
            if (
                //                                          //At least one non visible character
                !strCurrency_I.IsTextValid(Std.CHARS_USEFUL_IN_TEXT)
                )
            {
                Error errorX = new Error("Entity-40", strCurrencyDescription_I,
                    Test.ToLog(strCurrency_I, strCurrencyDescription_I) + " non visible character");
                darrerror_M.Add(errorX);
            }
            else
            {
                if (
                    strCurrency_I.IsParsableToCurrency()
                    )
                {
                    Error errorX = new Error("Entity-41", strCurrencyDescription_I,
                        strCurrencyDescription_I + "(" + strCurrency_I + ") should be a currency value");
                    darrerror_M.Add(errorX);
                }
                else
                {
                    Currency curr = strCurrency_I.ParseToCurrency();
                    if (
                        curr.IsBetween(currMIN_I, currMAX_I)
                        )
                    {
                        Error errorX = new Error("Entity-42", strCurrencyDescription_I,
                            strCurrencyDescription_I + "(" + curr.ToText() + ") should be in the range " +
                                currMIN_I.ToText() + " to " + currMAX_I.ToText());
                        darrerror_M.Add(errorX);
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        protected void subVerifyEntityIncluded(
            //                                              //Verify entity included (Ex. collManager in bo, bo
            //                                              //      assigned to a coll

            List<Error> darrerror_M,
            Date dateStartLife_I,
            //                                              //Ex. "boBusinessObject"
            String strEntityIncluded_I,
            //                                              //Ex. boBusinessObject
            Entity entityIncluded_I,
            //                                              //Ex. BoBusinessObject.boDUMMY_UNIQUE
            Entity entityDUMMY_ENTITY_INCLUDED_I,
            //                                              //Ex. "Corp"
            String strIdEntityIncluded_I
            )
        {
            //                                              //Verify entity
            int intSizeBeforeVerifyId = darrerror_M.Count;
            darrerror_M = entityDUMMY_ENTITY_INCLUDED_I.VerifyStringId(strIdEntityIncluded_I);
            if (
                //                                          //Id is ok (no error added
                darrerror_M.Count == intSizeBeforeVerifyId
                )
            {
                if (
                    entityIncluded_I == null
                    )
                {
                    Error errorX = new Error("Entity-50", strEntityIncluded_I, strEntityIncluded_I + "(" +
                        strIdEntityIncluded_I + ") was not found");
                    darrerror_M.Add(errorX);
                }
                else
                {
                    if (
                        entityIncluded_I.lifeX.IsValidDateToAddHistory(dateStartLife_I)
                        )
                    {
                        Error errorX = new Error("Entity-51", strEntityIncluded_I,
                            "StartLife(" + dateStartLife_I.ToText() + ") is not valid in " + strEntityIncluded_I + "(" +
                            strIdEntityIncluded_I + ") with life(" + entityIncluded_I.lifeX.ToText() + ")");
                        darrerror_M.Add(errorX);
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //Following method is used in ReferenceTo<Entity> when "v"
        //                                                  //      is access an it is null (notice that entity is known
        //                                                  //      but not the table)

        //--------------------------------------------------------------------------------------------------------------
        public abstract Entity GetEntryFromDatabase(long longPrimaryKey_I);

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //Following method is used in generic type that "know" the
        //                                                  //      entity but not the table

        //--------------------------------------------------------------------------------------------------------------
        public abstract Object GetReferenceToEntity(long longPrimaryKey_I);

        /*[Glg, Sep 20, 2019], we had to alternatives, we decided to try to implement using a Func<>, but kept this code
         * just in case we need to change in the future
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //Following method is used in HistoryCrosses to get the
        //                                                  //      instance variable that match. Not all entities
        //                                                  //      requires to implement this method.
        //                                                  //Ex: in bo "hiwcollMembers", in coll "hiwcollMentees"

        //--------------------------------------------------------------------------------------------------------------
        public virtual Object GetHiw(String strHiwxxxInstanceVariable_I) { return null; }
        */

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
