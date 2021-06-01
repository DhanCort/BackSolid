/*TASK CombinationIndex4 Combination Index for 4 items*/
using System;

//                                                          //AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //CO-AUTHOR: Towa (LGF-Liliana Gutiérrez).
//                                                          //DATE: January 14, 2020.

namespace TowaStandard
{
    //==================================================================================================================
    public struct CombinationIndex4 : BsysInterface, IComparable
    {
        //                                                  //Unmutable object to handle index required to combine 4
        //                                                  //      items form a collection.
        //                                                  //(See a sequence example in CombinationIndex3)

        //--------------------------------------------------------------------------------------------------------------
        /*CONSTANTS*/

        public static CombinationIndex4 DummyValue = new CombinationIndex4();

        //--------------------------------------------------------------------------------------------------------------
        /*INSTANCE VARIABLES*/

        //                                                  //Index for each one of the 4 items of the collection.
        public readonly int A;
        public readonly int B;
        public readonly int C;
        public readonly int D;

        //                                                  //Size of collection to combine
        private readonly int intCollectionSize;

        //--------------------------------------------------------------------------------------------------------------
        /*COMPUTED VARIABLES*/

        public CombinationIndex4 First
        {
            get
            {
                return new CombinationIndex4(0, 1, 2, 3, this.intCollectionSize);
            }
        }

        public CombinationIndex4 Last
        {
            get
            {
                return new CombinationIndex4(this.intCollectionSize - 4, this.intCollectionSize - 3,
                    this.intCollectionSize - 2, this.intCollectionSize - 1, this.intCollectionSize);
            }
        }

        public bool IsValid
        {
            get
            {
                return (
                    this.A.IsBetween(0, this.intCollectionSize - 4)
                    );
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public String ToLogShort()
        {
            return "<" + Test.ToLog(this.A) + ", " + Test.ToLog(this.B) + ", " + Test.ToLog(this.C) + ", " +
                Test.ToLog(this.D) + ", " + Test.ToLog(this.intCollectionSize) + ">";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public String ToLogFull()
        {
            return this.ToLogShort();
        }

        //--------------------------------------------------------------------------------------------------------------
        /*OBJECT CONSTRUCTORS*/

        //--------------------------------------------------------------------------------------------------------------
        private CombinationIndex4(

            //                                              //Combination index
            int intA_I,
            int intB_I,
            int intC_I,
            int intD_I,
            //                                              //Size of collection to be combined
            int intCollectionSize_I
            )
        {
            //                                              //Assign first combination
            this.A = intA_I;
            this.B = intB_I;
            this.C = intC_I;
            this.D = intD_I;

            this.intCollectionSize = intCollectionSize_I;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static CombinationIndex4 NewFirst(
            //                                              //Collection size
            int Size
            )
        {
            return new CombinationIndex4(0, 1, 2,4,  Size);
        }

        //--------------------------------------------------------------------------------------------------------------
        /*ACCESS-METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public CombinationIndex4 Next(
            )
        {
            int intA = this.A;
            int intB = this.B;
            int intC = this.C;
            int intD = this.D;
            /*CASE*/
            if (
                this.D < (this.intCollectionSize - 1)
                )
            {
                intD = intD + 1;
            }
            else if (
                this.C < (this.intCollectionSize - 2)
                )
            {
                intC = intC + 1;
                intD = intC + 1;
            }
            else if (
                this.B < (this.intCollectionSize - 3)
                )
            {
                intB = intB + 1;
                intC = intB + 1;
                intD = intC + 1;
            }
            else
            {
                intA = intA + 1;
                intB = intA + 1;
                intC = intB + 1;
                intD = intC + 1;
            }
            /*END-CASE*/

            return new CombinationIndex4(intA, intB, intC, intD, this.intCollectionSize);
        }

        //--------------------------------------------------------------------------------------------------------------
        public CombinationIndex4 Previous(
            )
        {
            int intA = this.A;
            int intB = this.B;
            int intC = this.C;
            int intD = this.D;
            /*CASE*/
            if (
                intC < (this.D - 1)
                )
            {
                intD = intD - 1;
            }
            else if (
                intB < (this.C - 1)
                )
            {
                intC = intC - 1;
                intD = this.intCollectionSize - 1;
            }
            else if (
                intA < (this.B - 1)
                )
            {
                intB = intB - 1;
                intC = this.intCollectionSize - 2;
                intD = this.intCollectionSize - 1;
            }
            else
            {
                intA = intA - 1;
                intB = this.intCollectionSize - 3;
                intC = this.intCollectionSize - 2;
                intD = this.intCollectionSize - 1;
            }
            /*END-CASE*/

            return new CombinationIndex4(intA, intB, intC, intD, this.intCollectionSize);
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
                obj_I is CombinationIndex4
                ))
                Test.Abort(Test.ToLog(obj_I.GetType(), "obj_L.GetType") + " should be CombinationIndex4");

            CombinationIndex4 cidxToCompare = (CombinationIndex4)obj_I;

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

                    if (
                       intCompareTo == 0
                        )
                    {
                        intCompareTo = this.D.CompareTo(cidxToCompare.D);
                    }
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
