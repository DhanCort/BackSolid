/*TASK CombinationIndex3 Combination Index for 3 items*/
using System;

//                                                          //AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //CO-AUTHOR: Towa (LGF-Liliana Gutiérrez).
//                                                          //DATE: January 14, 2020.

namespace TowaStandard
{
    //==================================================================================================================
    public struct CombinationIndex3 : BsysInterface, IComparable
    {
        //                                                  //Unmutable object to handle index required to combine 3
        //                                                  //      items form a collection.
        //                                                  //For example, the sequence of index to process a
        //                                                  //      combination of 3 items in a collection of size 5
        //                                                  //      will be: <0,1,2>, <0,1,3>, <0,1,4>, <0,2,3>,
        //                                                  //      <0,2,4>, <0,3,4>, <1,2,3>, <1,2,4>, <1,3,4> and
        //                                                  //      <2,3,4>.

        //--------------------------------------------------------------------------------------------------------------
        /*CONSTANTS*/

        public static CombinationIndex3 DummyValue = new CombinationIndex3();

        //--------------------------------------------------------------------------------------------------------------
        /*INSTANCE VARIABLES*/

        //                                                  //Index for each one of the 3 items of the collection.
        public readonly int A;
        public readonly int B;
        public readonly int C;

        //                                                  //Size of collection to combine
        private readonly int intCollectionSize;

        //--------------------------------------------------------------------------------------------------------------
        /*COMPUTED VARIABLES*/

        public CombinationIndex3 First
        {
            get
            {
                return new CombinationIndex3(0, 1, 2, this.intCollectionSize);
            }
        }

        public CombinationIndex3 Last
        {
            get
            {
                return new CombinationIndex3(this.intCollectionSize - 3, this.intCollectionSize - 2,
                    this.intCollectionSize - 1, this.intCollectionSize);
            }
        }

        public bool IsValid
        {
            get
            {
                return (
                    this.A.IsBetween(0, this.intCollectionSize - 3)
                    );
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public String ToLogShort()
        {
            return "<" + Test.ToLog(this.A) + ", " + Test.ToLog(this.B) + ", " + Test.ToLog(this.C) + ", " +
                Test.ToLog(this.intCollectionSize) + ">";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public String ToLogFull()
        {
            return this.ToLogShort();
        }

        //--------------------------------------------------------------------------------------------------------------
        /*OBJECT CONSTRUCTORS*/

        //--------------------------------------------------------------------------------------------------------------
        private CombinationIndex3(

            //                                              //Combination index
            int intA_I,
            int intB_I,
            int intC_I,
            //                                              //Size of collection to be combined
            int intCollectionSize_I
            )
        {
            //                                              //Assign first combination
            this.A = intA_I;
            this.B = intB_I;
            this.C = intC_I;

            this.intCollectionSize = intCollectionSize_I;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static CombinationIndex3 NewFirst(
            //                                              //Collection size
            int Size
            )
        {
            return new CombinationIndex3(0, 1, 2, Size);
        }

        //--------------------------------------------------------------------------------------------------------------
        /*ACCESS-METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public CombinationIndex3 Next(
            )
        {
            int intA = this.A;
            int intB = this.B;
            int intC = this.C;
            /*CASE*/
            if (
                this.C < (this.intCollectionSize - 1)
                )
            {
                intC = intC + 1;
            }
            else if (
                this.B < (this.intCollectionSize - 2)
                )
            {
                intB = intB + 1;
                intC = intB + 1;
            }
            else
            {
                intA = intA + 1;
                intB = intA + 1;
                intC = intB + 1;
            }
            /*END-CASE*/

            return new CombinationIndex3(intA, intB, intC, this.intCollectionSize);
        }

        //--------------------------------------------------------------------------------------------------------------
        public CombinationIndex3 Previous(
            )
        {
            int intA = this.A;
            int intB = this.B;
            int intC = this.C;
            /*CASE*/
            if (
                intB < (this.C - 1)
                )
            {
                intC = intC - 1;
            }
            else if (
                intA < (this.B - 1)
                )
            {
                intB = intB - 1;
                intC = this.intCollectionSize - 1;
            }
            else
            {
                intA = intA - 1;
                intB = this.intCollectionSize - 2;
                intC = this.intCollectionSize - 1;
            }
            /*END-CASE*/

            return new CombinationIndex3(intA, intB, intC, this.intCollectionSize);
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
                obj_I is CombinationIndex3
                ))
                Test.Abort(Test.ToLog(obj_I.GetType(), "obj_L.GetType") + " should be CombinationIndex3");

            CombinationIndex3 cidxToCompare = (CombinationIndex3)obj_I;

            int intCompareTo = this.A.CompareTo(cidxToCompare.A);
            if (
                intCompareTo == 0
                )
            {
                intCompareTo = this.B.CompareTo(cidxToCompare.B);

                if (
                   intCompareTo == 0
                    )
                {
                    intCompareTo = this.C.CompareTo(cidxToCompare.C);
                }
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
