/*TASK Database Database All-in-Memory*/
using System;
using System.Collections.Generic;

//                                                          //AUTHOR: Towa (RPM-Rubén de la Peña, JJFM-Juan Jose Flores,
//                                                          //      LGCR-Leoncio Chiunty).
//                                                          //CO-AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //DATE: September 27, 2019.

namespace TowaStandard
{
    //==================================================================================================================
    public class ToLock : BsysAbstract, IComparable
    {
        //--------------------------------------------------------------------------------------------------------------
        /*STATIC VARIABLES*/

        //                                                  //Locks.
        //                                                  //Intencionally (it is not necessary, this lock is local)
        private static readonly Object objLOCK = new Object();

        //                                                  //Keeps count of the ID last assigned.
        private static int intUniqueIdLast = 0;

        //--------------------------------------------------------------------------------------------------------------
        /*STATIC METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public static void MultipleLocks(
            //                                          //Method with  multiple (object) locks to executes an actions
            //                                          //      while those locks are in place.

            //                                          //Some could be null.
            Entity[] ArrayOfEntities_I,
            //                                          //Action (set of statements) to be executed once the locks are
            //                                          //      placed.
            Action ActionHandler_I
            )
        {
            List<ToLock> darrlock = new List<ToLock>();
            foreach (Entity entityX in ArrayOfEntities_I)
            {
                if (
                    entityX != null
                    )
                {
                    darrlock.Add(entityX.tolockEntity);
                }
            }

            //                                          //To avoid DEADLOCKS, multiple lock should be lock in the same
            //                                          //      sequence (every lock object have a unique Id to sort)
            darrlock.Sort();

            //                                          //Set locks in place an execute action, could be 0 or many locks
            ToLock.subMultipleLocks(darrlock, ActionHandler_I);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subMultipleLocks(
            //                                          //Method that locks multiple objects and executes actions while
            //                                          //      those locks are in place.

            //                                          //The locks that should be placed by the method, 0 or many.
            List<ToLock> darrlocks_M,
            //                                          //The actions to be executed once the locks are placed.
            Action actionHandler_I
            )
        {
            if (
                darrlocks_M.Count == 0
                )
            {
                actionHandler_I();
            }
            else
            {
                //                                      //Use lock backward to avoid darr re-arrangement
                lock (darrlocks_M[darrlocks_M.Count - 1])
                {
                    darrlocks_M.RemoveAt(darrlocks_M.Count - 1);
                    ToLock.subMultipleLocks(darrlocks_M, actionHandler_I);
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        /*INSTANCE VARIABLES*/

        //                                                  //The unique ID of the lock instance.
        public readonly int UniqueId;

        //--------------------------------------------------------------------------------------------------------------
        /*COMPUTED VARIABLES*/

        //--------------------------------------------------------------------------------------------------------------
        public override String ToLogShort()
        {
            const String strCLASS = "Lock";

            return strCLASS + "<" + Test.ToLog(this.UniqueId) + ">";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public override String ToLogFull()
        {
            const String strCLASS = "Lock";

            return strCLASS + "<" + Test.ToLog(this.UniqueId, "UniqueId") + ">";
        }

        //--------------------------------------------------------------------------------------------------------------
        /*OBJECT CONSTRUCTORS*/

        //--------------------------------------------------------------------------------------------------------------
        public ToLock()
        {
            lock (ToLock.objLOCK)
            {
                ToLock.intUniqueIdLast = ToLock.intUniqueIdLast + 1;
                this.UniqueId = ToLock.intUniqueIdLast;
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        /*INSTANCE METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public int CompareTo(
            //                                              //Required for Sort, BinarySearch and CompareTo.

            //                                              //this[I], object key info.

            //                                              //lock or int
            Object obj_I
            )
        {
            int intUniqueIdToCompare;
            /*CASE*/
            if (
                obj_I is ToLock
                )
            {
                intUniqueIdToCompare = ((ToLock)obj_I).UniqueId; ;
            }
            else if (
                obj_I is int
                )
            {
                intUniqueIdToCompare = (int)obj_I;
            }
            else
            {
                Test.Abort(
                    Test.ToLog(obj_I.GetType(), "obj_I.type") +
                        " is not a compatible CompareTo argument, the options are: Lock & int",
                    Test.ToLog(this.GetType(), "this.type"));

                intUniqueIdToCompare = 0;
            }
            /*END-CASE*/

            return intUniqueIdToCompare.CompareTo(this.UniqueId);
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
