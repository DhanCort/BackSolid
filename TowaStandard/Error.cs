/*TASK DaysPeriod Days Period*/
using System;

//                                                          //AUTHOR: Towa (AQG-Andrea Quiroz, LGF-Liliana Gutiérrez).
//                                                          //CO-AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //DATE: August, 29, 2018.

namespace TowaStandard
{
    //==================================================================================================================
    public struct Error : BsysInterface
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ALL DATABASE "CATALOGS" CLASS NAME SHOULD START WITH "Z".
        //                                                  //THIS IS JUST A CONVENIENT NAMING TO EASY SEPARATE THIS
        //                                                  //      "KIND" OF TABLE THAT CAN BE TOO MANY BUT ADD'S NO
        //                                                  //      COMPEXITY TO SYSTEM.

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //Desctiption of an error

        //--------------------------------------------------------------------------------------------------------------
        /*CONSTANTS*/

        //--------------------------------------------------------------------------------------------------------------
        /*INSTANCE VARIABLES*/

        //                                                  //Unique reference to this kind of error.
        //                                                  //[Glg,25Sep2019], probablemente querramos tener un catalogo
        //                                                  //      con información util (descripción, help, ..), si
        //                                                  //      hacemos esto, este string cambia a ser una entrada
        //                                                  //      de ese catalogo.
        //                                                  //Ex. "Org2004"
        public readonly String Code;

        //                                                  //Ex. "Id", "Start", "Database".
        public readonly String VariableOrConcept;

        //                                                  //Description of error.
        //                                                  //"StartDate(2019-09-25) should be within Towa 
        public readonly String Text;

        //--------------------------------------------------------------------------------------------------------------
        /*COMPUTED VARIABLES*/

        //--------------------------------------------------------------------------------------------------------------
        public String ToLogShort()
        {
            return "<" + Test.ToLog(this.Code) + ", " + Test.ToLog(this.VariableOrConcept) + ", " + 
                Test.ToLog(this.Text) + ">";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public String ToLogFull()
        {
            return "<" + Test.ToLog(this.Code, "Code") + ", " +
                Test.ToLog(this.VariableOrConcept, "VariableOrConcept") + ", " + Test.ToLog(this.Text, "Text") + ">";
        }

        //--------------------------------------------------------------------------------------------------------------
        /*OBJECT CONSTRUCTORS*/

        //--------------------------------------------------------------------------------------------------------------
        public Error(

            String Code_I,
            String VariableOrConcept_I,
            String Text_I
            )
        {
            this.Code = Code_I;
            this.VariableOrConcept = VariableOrConcept_I;
            this.Text = Text_I;
        }

        //--------------------------------------------------------------------------------------------------------------
        /*ACCESS-METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public int CompareTo(
            //                                              //Required for Sort, BinarySearch and CompareTo.

            //                                              //this[I], object key info.

            //                                              //syspath or str
            Object obj_I
            )
        {
            if (!(
                obj_I is Error
                ))
                Test.Abort(
                    Test.ToLog(obj_I.GetType(), "obj_L.GetType") +
                        " is not a compatible CompareTo argument, the only option is: Error",
                    Test.ToLog(this.GetType(), "this.type"));

            int intTotalDaysToCompare = ((DaysPeriod)obj_I).Start.TotalDays;

            Error errorX = (Error)obj_I;

            int intCompareTo = this.Code.CompareTo(errorX.Code);
            if (
                intCompareTo == 0
                )
            {
                intCompareTo = this.VariableOrConcept.CompareTo(errorX.VariableOrConcept);
            }

            return intCompareTo;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //NEXT 2 METHODS ARE TO AVOID COMPILE WARNINGS.

        //--------------------------------------------------------------------------------------------------------------
        public override bool Equals(
            Object obj_I
            )
        {
            if (!(
                obj_I is Error
                ))
                Test.Abort(Test.ToLog(obj_I.GetType(), "obj_I.GetType") + " should be DaysPeriod");

            Error errorRight = (Error)obj_I;

            return (
                (this.Code == errorRight.Code) &&
                (this.VariableOrConcept == errorRight.VariableOrConcept) &&
                (this.Text == errorRight.Text)
                );
        }

        //--------------------------------------------------------------------------------------------------------------
        public override int GetHashCode()
        {
            return this.GetHashCode();
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
