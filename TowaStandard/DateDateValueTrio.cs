/*TASK History Set of type to handle histories*/
using System;

//                                                          //AUTHOR: Towa (AQG-Andrea Quiroz, LGF-Liliana Gutierrez).
//                                                          //CO-AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //DATE: August 29, 2018.

namespace TowaStandard
{
    //==================================================================================================================
    public struct DateDateValueTrio<TValue> : BsysInterface, IComparable, 
        SerializableInterface<DateDateValueTrio<TValue>>
        where TValue : SerializableInterface<TValue>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        public String ToText(
            )
        {
            return null;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //See History, HistoryCrossed & HistoryWith.

        //--------------------------------------------------------------------------------------------------------------
        /*CONSTANTS*/

        //--------------------------------------------------------------------------------------------------------------
        /*INSTANCE VARIABLES*/

        public static readonly DateDateValueTrio<TValue> DummyValue = new DateDateValueTrio<TValue>();

        //                                                  //This 2 dates IS a period.
        public readonly Date start;
        public readonly Date end;

        //                                                  //Value assigned to this entry, primitive or object
        public readonly TValue value;

        //--------------------------------------------------------------------------------------------------------------
        public String ToLogShort()
        {
            return "<" + Test.ToLog(this.start) + ", " + Test.ToLog(this.end) + ", " +
                Test.z_TowaPRIVATE_ToLogXT(this.value) + ">";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public String ToLogFull() { return this.ToLogShort(); }

        //--------------------------------------------------------------------------------------------------------------
        /*OBJECT CONSTRUCTORS*/

        //--------------------------------------------------------------------------------------------------------------
        public DateDateValueTrio(
            Date start_I,
            Date end_I,
            TValue Value_T
            ) 
        {
            this.start = start_I;
            this.end = end_I;
            this.value = Value_T;

            if (!(
                this.start <= this.end
                ))
                Test.Abort(Test.z_TowaPRIVATE_ToLogXT(this, "this") + " do not include a valid period");
        }


        //--------------------------------------------------------------------------------------------------------------
        /*ACCESS-METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public int CompareTo(Object objArgument_I)
        {
            if (
                !(objArgument_I is DateDateValueTrio<TValue>)
                )
                Test.Abort(
                    Test.ToLog(objArgument_I.GetType(), "objArgument_I.type") +
                        " is not a compatible CompareTo argument, the only option is: " +
                        "DateDateValueTrio<" + typeof(TValue).Name() + ">",
                    Test.ToLog(this.GetType(), "this.type"));

            DateDateValueTrio<TValue> ddvtvalue = (DateDateValueTrio<TValue>)objArgument_I;

            int intCompareTo = this.start.CompareTo(ddvtvalue.start);
            if (
                intCompareTo == 0
                )
            {
                intCompareTo = this.end.CompareTo(ddvtvalue.end);
            }

            return intCompareTo;
        }

        //--------------------------------------------------------------------------------------------------------------
        public byte[] Serialize(
            //                                              //Get a serialized version of the object.

            )
        {
            byte[] arrbyteStart = this.start.Serialize();
            byte[] arrbyteEnd = this.end.Serialize();
            byte[] arrbyteValue = this.value.Serialize();

            return Std.ConcatenateArrays(arrbyteStart, arrbyteEnd, arrbyteValue);
        }

        //--------------------------------------------------------------------------------------------------------------
        public void Deserialize(
            //                                              //Returns a deserialized object.

            //                                              //The object to deserialize.
            out DateDateValueTrio<TValue> DateDateValueTrio_O,
            //                                              //The serialized bytes.
            ref byte[] Bytes_IO
            )
        {
            Date dateStart;
            Date.DummyValue.Deserialize(out dateStart, ref Bytes_IO);

            Date dateEnd;
            Date.DummyValue.Deserialize(out dateEnd, ref Bytes_IO);

            TValue value;
            (new TValue()).Deserialize(out value, ref Bytes_IO);

            DateDateValueTrio_O = new DateDateValueTrio<TValue>(dateStart, dateEnd, value);
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
