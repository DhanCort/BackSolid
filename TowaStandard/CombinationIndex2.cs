/*TASK CombinationIndex2 Combination Index for 2 items*/
using System;

//                                                          //AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //CO-AUTHOR: Towa (LGF-Liliana Gutiérrez).
//                                                          //DATE: January 14, 2020.

namespace TowaStandard
{
    //==================================================================================================================
    public struct CombinationIndex2 : BsysInterface, IComparable
    {
        //                                                  //Unmutable object to handle index required to combine 2
        //                                                  //      items form a collection.
        //                                                  //(See a sequence example in CombinationIndex3)

        //--------------------------------------------------------------------------------------------------------------
        /*CONSTANTS*/

        public static CombinationIndex2 DummyValue = new CombinationIndex2();

        //--------------------------------------------------------------------------------------------------------------
        /*INSTANCE VARIABLES*/

        //                                                  //Index for each one of the 2 items of the collection.
        public readonly int A;
        public readonly int B;

        //                                                  //Size of collection to combine
        private readonly int intCollectionSize;

        //--------------------------------------------------------------------------------------------------------------
        /*COMPUTED VARIABLES*/

        public CombinationIndex2 First
        {
            get
            {
                return new CombinationIndex2(0, 1, this.intCollectionSize);
            }
        }

        public CombinationIndex2 Last
        {
            get
            {
                return new CombinationIndex2(this.intCollectionSize - 3, this.intCollectionSize - 2,
                    this.intCollectionSize);
            }
        }

        public bool IsValid
        {
            get
            {
                return (
                    this.A.IsBetween(0, this.intCollectionSize - 2)
                    );
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public String ToLogShort()
        {
            return "<" + Test.ToLog(this.A) + ", " + Test.ToLog(this.B) + ", " + Test.ToLog(this.intCollectionSize) +
                ">";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public String ToLogFull()
        {
            return this.ToLogShort();
        }

        //--------------------------------------------------------------------------------------------------------------
        /*OBJECT CONSTRUCTORS*/

        //--------------------------------------------------------------------------------------------------------------
        private CombinationIndex2(

            //                                              //Combination index
            int intA_I,
            int intB_I,
            //                                              //Size of collection to be combined
            int intCollectionSize_I
            )
        {
            //                                              //Assign first combination
            this.A = intA_I;
            this.B = intB_I;

            this.intCollectionSize = intCollectionSize_I;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static CombinationIndex2 NewFirst(
            //                                              //Collection size
            int Size
            )
        {
            return new CombinationIndex2(0, 1, Size);
        }

        //--------------------------------------------------------------------------------------------------------------
        /*ACCESS-METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public CombinationIndex2 Next(
            )
        {
            int intA = this.A;
            int intB = this.B;
            /*CASE*/
            if (
                this.B < (this.intCollectionSize - 2)
                )
            {
                intB = intB + 1;
            }
            else
            {
                intA = intA + 1;
                intB = intA + 1;
            }
            /*END-CASE*/

            return new CombinationIndex2(intA, intB, this.intCollectionSize);
        }

        //--------------------------------------------------------------------------------------------------------------
        public CombinationIndex2 Previous(
            )
        {
            int intA = this.A;
            int intB = this.B;
            /*CASE*/
            if (
                intA < (this.B - 1)
                )
            {
                intB = intB - 1;
            }
            else
            {
                intA = intA - 1;
                intB = this.intCollectionSize - 2;
            }
            /*END-CASE*/

            return new CombinationIndex2(intA, intB, this.intCollectionSize);
        }

        //--------------------------------------------------------------------------------------------------------------
        public int CompareTo(
            //                                              //Required for Sort, BinarySearch and CompareTo.

            //                                              //this[I], object key info.

            //                                              //syspath or str
            Object obj_I
            )
        {
            if (!(
                obj_I is CombinationIndex2
                ))
                Test.Abort(Test.ToLog(obj_I.GetType(), "obj_L.GetType") + " should be CombinationIndex2");

            CombinationIndex2 cidxToCompare = (CombinationIndex2)obj_I;

            int intCompareTo = this.A.CompareTo(cidxToCompare.A);
            if (
                intCompareTo == 0
                )
            {
                intCompareTo = this.B.CompareTo(cidxToCompare.B);
            }

            return intCompareTo;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //NEXT 2 METHODS ARE TO AVOID COMPILE WARNINGS.

        //--------------------------------------------------------------------------------------------------------------
        public override bool Equals(
            Object obj_L
            )
        {
            Test.Abort(Test.ToLog(obj_L.GetType(), "obj_L.GetType") + " Equals method should not be used");

            return true;
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
