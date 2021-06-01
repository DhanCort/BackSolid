/*TASK Database Database All-in-Memory*/
using System;

//                                                          //AUTHOR: Towa (AQG-Andrea Quiroz, LGF-Liliana Gutierrez).
//                                                          //CO-AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //DATE: August 29, 2018.

namespace TowaStandard
{
    //==================================================================================================================
    public class ReferenceTo<TEntity> : BsysAbstract, SerializableInterface<ReferenceTo<TEntity>>
        where TEntity : Entity, new()
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //Reference to a DataBase Entity (this is to implement
        //                                                  //      all in memory cache).
        //                                                  //FOR EACH "entity" (record in database) CAN EXIST ONE OR
        //                                                  //      NONE ReferenceTo OBJECT.

        //--------------------------------------------------------------------------------------------------------------
        /*CONSTANTS*/

        //--------------------------------------------------------------------------------------------------------------
        /*INSTANCE VARIABLES*/

        //                                                  //Locks.
        private readonly ToLock tolockAssignV = new ToLock();

        //                                                  //This is the information really required to identify an
        //                                                  //      entity, it is 2 items of information.
        //                                                  //1. Type of object (Ex. ReferenceTo<CollCollaborator> and.
        //                                                  //2. PK of the collaborator. (Ex. 825, witch is PK of
        //                                                  //      collaborator entity for "LGF")
        private long longPrimaryKey_Z;
        public long primaryKey { get { return this.longPrimaryKey_Z;  } }

        //--------------------------------------------------------------------------------------------------------------
        /*COMPUTED VARIABLES*/

        //                                                  //To access entity content
        private TEntity v_Z = null;
        public TEntity v
        {
            get
            {
                if (
                    this.v_Z == null
                    )
                {
                    //                                          //This code will update the entity in this.v_Z.
                    //                                          //If not exist, returns null;
                    //                                          //This lock will be applied inside 
                    //                                          //      this.z_TowaPRIVATE_subAssignEntryToV(entity) but
                    //                                          //      here it is require to avoid double access of
                    //                                          //      entity
                    lock (this.tolockAssignV)
                    {
                        if (
                            //                                  //Collaborator's object is not present
                            this.v_Z == null
                            )
                        {
                            //                                  //Any database error will produce null
                            TEntity entity = (TEntity)(new TEntity()).GetEntryFromDatabase(this.longPrimaryKey_Z);
                            this.v_Z = entity;
                        }
                    }
                }
                return this.v_Z;
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public override String ToLogShort()
        {
            String strObjId = Test.GetObjId(this);

            String strV = (this.v_Z == null) ? "null" : Test.GetObjId(this.v);

            return strObjId + "<" + Test.ToLog(this.tolockAssignV) + ", " + Test.ToLog(this.primaryKey) + ", " +
                strV + ">";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public override String ToLogFull()
        {
            return this.ToLogShort();
        }

        //--------------------------------------------------------------------------------------------------------------
        /*OBJECT CONTRUCTORS*/

        //--------------------------------------------------------------------------------------------------------------
        public ReferenceTo() { }

        //--------------------------------------------------------------------------------------------------------------
        private ReferenceTo(
            //                                              //SEE NEXT STATIC METHOD.

            long longPrimaryKey_I
            )
        {
            this.longPrimaryKey_Z = longPrimaryKey_I;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static ReferenceTo<TEntity> z_TowaPRIVATE_subConstructReferenceTo(
            //                                              //THIS METHOS WAS INCLUDED JUST TO MAKE CLEAR THIS IS A
            //                                              //      TOOL TO IMPLEMENTE TowaStandard (NOT INTENDED FOR
            //                                              //      DEVELOPER USE).
            //                                              //This method is called when "TableOfEntity" needs to add an
            //                                              //      entry in idxrentityByPk.

            long longPrimaryKey_I
            )
        {
            return new ReferenceTo<TEntity>(longPrimaryKey_I);
        }

        //--------------------------------------------------------------------------------------------------------------
        /*TRANSFORMATION METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public void z_TowaPRIVATE_subCorrectPkAfterInsertInDB(

            //                                              //this[M], correct PK

            long longPk_T
            )
        {
            this.longPrimaryKey_Z = longPk_T;
        }

        //--------------------------------------------------------------------------------------------------------------
        public void z_TowaPRIVATE_subAssignEntryToV(
            //                                              //Assign object V (may be null or have a value)

            TEntity entity_T
            )
        {
            if (
                  //                                        //entity's object is not present
                  this.v_Z == null
                  )
            {
                //                                          //This code will update the entity in this.v_Z.
                //                                          //If not exist, returns null;
                lock (this.tolockAssignV)
                {
                    if (
                        //                                  //Collaborator's object is not present
                        this.v_Z == null
                        )
                    {
                        this.v_Z = entity_T;
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        /*ACCESS METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public byte[] Serialize(
            //                                              //Get a serialized version of the object.

            )
        {
            return BitConverter.GetBytes(this.longPrimaryKey_Z);
        }

        //--------------------------------------------------------------------------------------------------------------
        public void Deserialize(
            //                                              //Returns a deserialized object.
            //                                              //ONLY ONE ReferenceTo OBJECT SHOULD EXIST IN TABLE SET FOR
            //                                              //      EACH OBJECT.
            //                                              //It is contructed and added to table first time it is 
            //                                              //      deserialized

            //                                              //ReferenceTo to deserialize, or the one in table.
            out ReferenceTo<TEntity> ReferenceToEntry_O,
            //                                              //The serialized bytes.
            ref byte[] Bytes_IO
            )
        {
            byte[] arrbyteLongPk;
            Std.SeparateToDeserializeFixSize(out arrbyteLongPk, ref Bytes_IO, 8);
            long longPk = BitConverter.ToInt64(arrbyteLongPk, 0);

            //                                              //Get rentity that should exist in table (add if required)
            ReferenceToEntry_O = (ReferenceTo<TEntity>)(new TEntity()).GetReferenceToEntity(longPk);
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/