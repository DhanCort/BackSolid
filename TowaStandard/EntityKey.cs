/*TASK Database Database All-in-Memory*/
using System;

//                                                          //AUTHOR: Towa (RPM-Rubén de la Peña, JJFM-Juan Jose Flores,
//                                                          //      LGCR-Leoncio Chiunty).
//                                                          //CO-AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //DATE: August 22, 2019.

namespace TowaStandard
{
    //==================================================================================================================
    public struct EntityKey<TKey> : BsysInterface, IComparable
        where TKey : IComparable /*String, int, long, char, Date or Time*/
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //Concatenation of PrimaryKeyBelongsTo & one key elements.
        //                                                  //This "EntityKey" is requires to access DataBase using it's
        //                                                  //      Unique Key witch has 2 elements; PrimaryKeyBelongsTo
        //                                                  //      and Key.
        //                                                  //Example: 1(Pk of "Towa" organization) and "LGF" witch is
        //                                                  //      a unique key of a collaborator within "Towa".
        //                                                  //Example: 215(Pk of an "invoice") and 3 witch is the 3th
        //                                                  //      item in sequence of the "invoice".

        //--------------------------------------------------------------------------------------------------------------
        /*CONSTANTS*/

        //--------------------------------------------------------------------------------------------------------------
        /*INITIALIZER*/

        //--------------------------------------------------------------------------------------------------------------
        static EntityKey(
            )
        {
            String strType = "EntityKey<" + typeof(TKey).Name() + ">";
            Test.z_TowaPRIVATE_subAbortIfInvalidTKey(typeof(TKey), strType);
        }

        //--------------------------------------------------------------------------------------------------------------
        /*INSTANCE VARIABLES*/

        //                                                  //PrimaryKey of "father" of the entity (-1 if entity do not
        //                                                  //      have a "father", OrgOrganization in Encompass do not
        //                                                  //      have a "father").
        public readonly long primaryKeyBelongsTo;

        //                                                  //One element key (Example: "LGF").
        //                                                  //TKye options: String, int, long, char, Date or Time
        public readonly TKey key;

        //--------------------------------------------------------------------------------------------------------------
        public String ToLogShort()
        {
            return "<" + Test.ToLog(this.primaryKeyBelongsTo) + ", " + Test.z_TowaPRIVATE_ToLogXT<TKey>(this.key) + ">";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public String ToLogFull()
        {
            return "<" + Test.ToLog(Test.ToLog(this.primaryKeyBelongsTo, "PrimaryKeyBelongsTo") + ", " +
                Test.z_TowaPRIVATE_ToLogXT(this.key, "Key") + ">");
        }

        //--------------------------------------------------------------------------------------------------------------
        /*OBJECT CONSTRUCTORS*/

        //--------------------------------------------------------------------------------------------------------------
        public EntityKey(
            //                                              //Creates a new EntityKey object.

            //                                              //Family name of a person (Ex. Smith).
            long PrimaryKeyBelongsTo_I,
            //                                              //Given name of a person (Ex. John).
            TKey Key_I
            )
        {
            Test.AbortIfNull(Key_I, "Key_I");
            if (
                typeof(TKey) == typeof(String)
                )
            {
                //                                          //It can not be an empty String
                String strKey = Key_I.ToString();
                Test.AbortIfNullOrEmpty(strKey, "strKey");
            }

            this.primaryKeyBelongsTo = PrimaryKeyBelongsTo_I;
            this.key = Key_I;
        }

        //--------------------------------------------------------------------------------------------------------------
        /*ACCESS METHODS*/

        //-------------------------------------------------------------------------------------------------------------
        public String ToText(
            )
        {
            return "<" + this.primaryKeyBelongsTo.ToText() + ", " + Test.z_TowaPRIVATE_ToLogXT<TKey>(this.key) + ">";
        }

        //--------------------------------------------------------------------------------------------------------------
        public int CompareTo(
            //                                              //Required for Sort, BinarySearch and CompareTo.

            //                                              //this[I], object key info.

            //                                              //EntityKey
            Object obj_I
            )
        {
            if (!(
                obj_I is EntityKey<TKey>
                ))
                Test.Abort(Test.ToLog(obj_I.GetType(), "obj_L.GetType") +
                    " should be EntityKey<" + typeof(TKey).Name() + ">");

            EntityKey<TKey> ekeyToCompare = (EntityKey<TKey>)obj_I;

            int intCompareTo = this.primaryKeyBelongsTo.CompareTo(ekeyToCompare.primaryKeyBelongsTo);
            if (
                intCompareTo == 0
                )
            {
                intCompareTo = this.key.CompareTo(ekeyToCompare.key);
            }

            return intCompareTo;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
