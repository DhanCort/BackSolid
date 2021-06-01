/*TASK Btuple Base for all Tuples*/
using System;

//                                                          //AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: January 2, 2016.

namespace TowaStandard
{
    //==================================================================================================================
    public abstract class BtupleAbstract : BobjAbstract, IComparable
    {
        //                                                  //Base class for all tuple defined by user.
        //                                                  //The purpose is to have a unique class to identify all
        //                                                  //      tuples.
        //                                                  //"key" puede ser simple o múltiple (varias "key" en orden
        //                                                  //      de jerarquía son la llave).
        //                                                  //En ambos casos Std.Sort(arrtnxxxx) o
        //                                                  //      Std.Sort(arrtnxxxx, arrobj).
        //                                                  //1. Cuando "key" es simple:
        //                                                  //1a. Ver 3 ejemplos en SAI Tech.cs.
        //                                                  //1b. objKey.BinarySearch(arrtnxxx).
        //                                                  //2. Cuando "key" es múltiple:
        //                                                  //2a. Ver ejemplo T6mkeMultipleKeyExampleTuple.cs en
        //                                                  //      TowaStandard.
        //                                                  //2b. tnxxxKey.BinarySearch(arrtnxxx), con las keys
        //                                                  //      se debe construir un tnxxx usando el constructor de
        //                                                  //      "key".

        //--------------------------------------------------------------------------------------------------------------
        /*INSTANCE VARIABLES*/

        //                                                  //NO INSTANCE VARIABLES
        /*
        //--------------------------------------------------------------------------------------------------------------
        public override String ToLogShort(
            //                                              //THIS METHOD SHOULD BE IMPLEMENTED IN EVERY TUPLE.
            //                                              //Produces a SHORT version of information for each of its
            //                                              //      item.
            //                                              //Example:
            //                                              //<item, item, ..., item>
            //                                              //this[I], all its instance variables.
            )
        {
            Test.Abort("SOMETHING IS WRONG!!!, LogTo() should not be call in " +
                Test.ToLog(this.GetType(), "this.Type"));

            return null;
        }

        //--------------------------------------------------------------------------------------------------------------
        public override String ToLogFull(
            //                                              //THIS METHOD SHOULD BE IMPLEMENTED IN EVERY TUPLE.
            //                                              //Produces a FULL version of information for each of its
            //                                              //      item.
            //                                              //Example:
            //                                              //prefix{???(item), ???(item), ..., ???(item)}
            //                                              //this[I], all its instance variables.
            )
        {
            Test.Abort("SOMETHING IS WRONG!!!, LogTo() should not be call in " +
                Test.ToLog(this.GetType(), "this.Type"));

            return null;
        }
        */

        //--------------------------------------------------------------------------------------------------------------
        /*OBJECT CONTRUCTORS*/

        //--------------------------------------------------------------------------------------------------------------
        protected BtupleAbstract(
            //                                              //this.*[O], nada. 
            )
            : base()
        {
        }

        //--------------------------------------------------------------------------------------------------------------
        /*ACCESS METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public virtual int CompareTo(Object objArgument_I)
        {
            //                                              //Este método se debe implementar en todas las tuplas
            //                                              //      que tienen "key" la cual se requiere para poder
            //                                              //      usar los métodos Sort y BinarySearch.
            Test.Abort(Test.ToLog(this.GetType(), "this.type") + " this tuple does not implement CompareTo()",
                Test.ToLog(objArgument_I.GetType(), "obj_I.type"));

            return 0;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/