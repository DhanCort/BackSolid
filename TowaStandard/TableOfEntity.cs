/*TASK Database Database All-in-Memory*/
using System;
using System.Collections.Generic;

//                                                          //AUTHOR: Towa (RPM-Rubén de la Peña, JJFM-Juan Jose Flores,
//                                                          //      LGCR-Leoncio Chiunty).
//                                                          //CO-AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //DATE: August 22, 2019.

namespace TowaStandard
{
    //==================================================================================================================
    public abstract class TableOfEntity<TKey, TEntity> : BsysAbstract
        where TKey : IComparable /*String, int, long, char, Date or Time*/
        where TEntity : Entity, new()
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //This is a singleton class that provides access to data
        //                                                  //      from database by caching in-memory all of the
        //                                                  //      database entities on-demand.
        //                                                  //Concrete classes should:
        //                                                  //1. Implement some abstract methods declares in
        //                                                  //      TableOfEntity.
        //                                                  //2. Additional specific funcionality.
        //                                                  //MORE FUNCTIONALITY USEFUL FOR ALL ENTITIES, SHOULD BE
        //                                                  //      ADDED IN THIS ABSTRACT CLASS

        //--------------------------------------------------------------------------------------------------------------
        /*CONSTANTS*/

        //--------------------------------------------------------------------------------------------------------------
        /*INITIALIZER*/

        //--------------------------------------------------------------------------------------------------------------
        static TableOfEntity(
            )
        {
            String strType = "TableOfEntity<" + typeof(TKey).Name() + ", " + typeof(Entity).Name() + ">";
            Test.z_TowaPRIVATE_subAbortIfInvalidTKey(typeof(TKey), strType);
        }

        //--------------------------------------------------------------------------------------------------------------
        /*INSTANCE VARIABLES*/

        //                                                  //Instance Locks
        private readonly ToLock tolockIkeyrentity = new ToLock();
        private readonly ToLock tolockIdxrentityByPk = new ToLock();

        //                                                  //Collections by PkBelongsTo+Key & Pk
        private readonly Dictionary<EntityKey<TKey>, ReferenceTo<TEntity>> ikeyrentity;
        private readonly Dictionary<long, ReferenceTo<TEntity>> idxrentityByPk;

        //--------------------------------------------------------------------------------------------------------------
        public override String ToLogShort()
        {
            String strObjId = typeof(TableOfEntity<TKey, TEntity>).Name + ":" + this.GetHashCode();

            return strObjId + "{" + Test.ToLog(this.tolockIkeyrentity) + ", " + Test.ToLog(this.tolockIdxrentityByPk) +
                ", " + Test.ToLog(this.ikeyrentity) + ", " + Test.ToLog(this.idxrentityByPk) + "]";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public override String ToLogFull()
        {
            String strObjId = typeof(TableOfEntity<TKey, TEntity>).Name + ":" + this.GetHashCode();

            return strObjId + "{" + Test.ToLog(this.tolockIkeyrentity, "tolockIkeyrentity") + ", " +
                Test.ToLog(this.tolockIdxrentityByPk, "tolockIdxrentityByPk") + ", " +
                Test.ToLog(this.ikeyrentity, "ikeyrentity") + ", " + Test.ToLog(this.idxrentityByPk, "idxrentityByPk") +
                "}";
        }

        //--------------------------------------------------------------------------------------------------------------
        /*OBJECT CONSTRUCTORS*/

        //--------------------------------------------------------------------------------------------------------------
        protected TableOfEntity()
        {
            this.ikeyrentity = new Dictionary<EntityKey<TKey>, ReferenceTo<TEntity>>();
            this.idxrentityByPk = new Dictionary<long, ReferenceTo<TEntity>>();
        }

        //--------------------------------------------------------------------------------------------------------------
        /*TRANSFORMATION METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public void ClearTable(
            )
        {
            //                                              //Reset both index
            this.ikeyrentity.Clear();
            this.idxrentityByPk.Clear();
        }

        //--------------------------------------------------------------------------------------------------------------
        public ReferenceTo<TEntity> GetReferenceToEntity(
            //                                              //Retrive reference to from idx.
            //                                              //If do not exist, add ReferenceTo to idx (entity = null).
            //                                              //Notice that access to entity (content of rentity.v) is
            //                                              //      deferred until first use.
            //                                              //rentity, from idx.
            long primaryKeyToSearch_I
            )
        {
            if (
                //                                          //IS NOT in index
                !this.idxrentityByPk.ContainsKey(primaryKeyToSearch_I)
                )
            {
                lock (this.tolockIdxrentityByPk)
                {
                    if (
                        //                                  //IS NOT in index
                        !this.idxrentityByPk.ContainsKey(primaryKeyToSearch_I)
                        )
                    {
                        //                                  //Add reference to table
                        ReferenceTo<TEntity> rentity =
                            ReferenceTo<TEntity>.z_TowaPRIVATE_subConstructReferenceTo(primaryKeyToSearch_I);
                        this.idxrentityByPk.Add(primaryKeyToSearch_I, rentity);
                    }
                }
            }

            //                                              //Access reference to (always exist).
            return this.idxrentityByPk[primaryKeyToSearch_I];
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS (ADD TO "In-Memory"), ANY ERROR PRODUCES null.

        //--------------------------------------------------------------------------------------------------------------
        public TEntity GetByUniqueKey(long primaryKeyBelongsTo_I, TKey key_I)
        {
            //                                              //To easy code
            EntityKey<TKey> ekey = new EntityKey<TKey>(primaryKeyBelongsTo_I, key_I);

            //                                              //Any error will produce null
            List<Error> darrerror = new List<Error>();
            TEntity entity;
            this.TryGetWithValidData(darrerror, out entity, ekey);

            return entity;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public TEntity GetByPrimaryKey(long primaryKey_I)
        {
            //                                              //Make sure it exist in idx
            ReferenceTo<TEntity> rentity = this.GetReferenceToEntity(primaryKey_I);

            //                                              //"v" could be null
            return rentity.v;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS TO DATABASE TABLE,

        //--------------------------------------------------------------------------------------------------------------
        public void TryGet(
            //                                              //Try to get an entity from index (or to database).
            //                                              //If retrived from database should add to both index.

            //                                              //List of errors. Empty if no errors occurred.
            out List<Error> errors_O,
            //                                              //Entity retrieved, null if errors
            out TEntity entity_O,
            //                                              //(pk, key) May not exist
            long primaryKeyBelongsTo_I,
            //                                              //key in Json format.
            //                                              //String, int, long & char should conserv the same type.
            //                                              //Date should be in string format (Ex. "2019-10-26").
            //                                              //Time should be in string format (Ex. "13:41:25").
            JsonAbstract jsonKey_I
            )
        {
            //                                              //To easy code
            TEntity entityDUMMY = new TEntity();

            errors_O = entityDUMMY.VerifyIdJsonOkToGet(jsonKey_I);

            //                                              //If no errors, will contain an entity
            entity_O = null;

            if (
                errors_O.Count == 0
                )
            {
                //                                          //To easy code
                TKey key = this.GetKey(jsonKey_I);
                EntityKey<TKey> ekey = new EntityKey<TKey>(primaryKeyBelongsTo_I, key);

                this.TryGetWithValidData(errors_O, out entity_O, ekey);
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private TKey GetKey(
            //                                              //Convert key in json format to corresponding key.
            //                                              //TKey has 6 posibilities:
            //                                              //String, return (String)jsonKey_I.
            //                                              //long, return (long)jsonKey_I.
            //                                              //int, return (int)jsonKey_I.
            //                                              //char, return (char)jsonKey_I.
            //                                              //Date, return ((String)jsonKey_I).ParseToDate().
            //                                              //Time, return ((String)jsonKey_I).ParseToTime().
            Object jsonKey_I
            )
        {
            return (typeof(TKey) == typeof(Date)) ? (TKey)(Object)((String)jsonKey_I).ParseToDate() :
                (typeof(TKey) == typeof(Time)) ? (TKey)(Object)((String)jsonKey_I).ParseToTime() : (TKey)jsonKey_I;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private void TryGetWithValidData(
            //                                              //Try to add an entity to database and to dictionaries.
            //                                              //Data was verify, try to get an entity from index (or to
            //                                              //      database).
            //                                              //If retrived from database should add to both index.

            //                                              //Add errors.
            List<Error> errors_M,
            //                                              //Entity retrieved, null if errors
            out TEntity entity_O,
            //                                              //(pk, key) May not exist
            EntityKey<TKey> ekey_I
            )
        {
            if (
                !this.ikeyrentity.ContainsKey(ekey_I)
                )
            {
                lock (this.tolockIkeyrentity)
                {
                    if (
                        !this.ikeyrentity.ContainsKey(ekey_I)
                        )
                    {
                        //                                  //Need to access from DB (may not exist)
                        this.TryGetFromDatabase(errors_M, out entity_O, ekey_I.primaryKeyBelongsTo, ekey_I.key);
                        if (
                            //                              //Ok, it was retrive from DB
                            entity_O != null
                            )
                        {
                            this.MakeSureBothIndexExist(entity_O);
                        }
                    }
                }
            }

            if (
                !this.ikeyrentity.ContainsKey(ekey_I)
                )
            {
                //                                              //To easy code
                TEntity entityDUMMY = new TEntity();

                Error errorX = new Error("Table-14", "id", entityDUMMY.entityName + "(" + ekey_I + ") do not exist");
                errors_M.Add(errorX);
            }

            entity_O = (errors_M.Count == 0) ? this.ikeyrentity[ekey_I].v : null;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        protected abstract void TryGetFromDatabase(

            //                                              //List of errors. Empty if no errors occurred.
            List<Error> darrerror_M,
            //                                              //Entity retrieved
            out TEntity entity_O,
            //                                              //May not exist
            long PrimaryKeyBelongsTo_I,
            TKey key_I
            );

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        protected void VerifyNotFound(

            List<Error> darrerror_M,
            //                                              //null if not found
            EntityModel emodel_I,
            long PrimaryKeyBelongsTo_I,
            TKey key_I
            )
        {
            //                                              //To easy code
            TEntity entityDUMMY = new TEntity();

            if (
                emodel_I == null
                )
            {
                Error errorNotFound = new Error("Table-50", "Table",
                    "Select " + entityDUMMY.entityName + "(" + PrimaryKeyBelongsTo_I + ", " + key_I.ToString() +
                        ") not found");
                darrerror_M.Add(errorNotFound);
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        protected void DatabaseErrorTryGet(

            List<Error> darrerror_M,
            long PrimaryKeyBelongsTo_I,
            TKey key_I,
            Exception sysexcepDatabase_I
            )
        {
            //                                              //To easy code
            TEntity entityDUMMY = new TEntity();

            Error errorDatabase = new Error("DB-11", "Table",
                "Select " + entityDUMMY.entityName + "(UniqueKey<" + PrimaryKeyBelongsTo_I + ", " + key_I.ToString() +
                    ">) failed, " + Test.ToLog(sysexcepDatabase_I, "Exception"));
            darrerror_M.Add(errorDatabase);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private void MakeSureBothIndexExist(
            //                                              //Options are:
            //                                              //a. Exist in "ikey" (==> exist in both index).
            //                                              //====>Assign "object" inside "ref".
            //                                              //b. Exist in "idx" (did not exist in "ikey").
            //                                              //====>Assign "object" inside "ref" (from "idx") & add
            //                                              //      entry to "ikey".
            //                                              //c. Do not exist at all.
            //                                              //====>Create "ref", add both index entries.

            TEntity entityToAdd_I
            )
        {
            //                                              //Access (or create) idx entry
            ReferenceTo<TEntity> rentity = this.GetReferenceToEntity(entityToAdd_I.primaryKey);

            EntityKey<TKey> ekey = new EntityKey<TKey>(entityToAdd_I.primaryKeyBelongsTo, (TKey)entityToAdd_I.key);

            //                                              //Access reference to entity and  make sure it exist in both
            //                                              //      collections.
            //                                              //Assignment of entity to reference is deferred until the
            //                                              //      end of this method

            if (
                //                                          //Do not exist in "ikey"
                !this.ikeyrentity.ContainsKey(ekey)
                )
            {
                lock (this.tolockIkeyrentity)
                {
                    if (
                        //                                   //Do not exist in "ikey"
                        !this.ikeyrentity.ContainsKey(ekey)
                        )
                    {
                        this.ikeyrentity.Add(ekey, rentity);
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public abstract TEntity GetFromDatabase(
            //                                              //Should exist.
            //                                              //entity: null (if not exist, reference integrity error)

            //                                              //Should exist if PK was taken form an entity
            long longPkToLoad_I
            );

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        protected void WarningIfNotFound(

            //                                              //null if not found
            EntityModel model_I,
            long longPkToLoad_I,
            //                                              //null if database was a simulation test
            Exception sysexcepDatabase_I
            )
        {
            if (
                model_I == null
                )
            {
                //                                          //To easy code
                TEntity entityDUMMY = new TEntity();

                Error errorDatabase = new Error("DB-10", "Table",
                    "Select " + entityDUMMY.entityName + "(PrimaryKey<" + longPkToLoad_I + ">) failed, " + 
                    Test.ToLog(sysexcepDatabase_I, "Exception"));
                Test.Warning(Test.ToLog(errorDatabase));
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ADD TO DATABASE TABLE

        //--------------------------------------------------------------------------------------------------------------
        public void TryAdd(
            //                                              //Try to add an entity to both index and database.

            //                                              //List of errors. Empty if no errors occurred.
            out List<Error> errors_O,
            //                                              //null if errors.
            out TEntity entityAdded_O,
            //                                              //Include Unique Key information
            JsonAbstract jsonadd_I
            )
        {
            //                                              //An object will be assigned before successfully adding to
            //                                              //      database, and nullify again if database fail
            entityAdded_O = null;

            //                                              //To easy code
            TEntity entityDUMMY = new TEntity();

            errors_O = entityDUMMY.VerifyIsJsonOkToAdd(jsonadd_I);
            if (
                errors_O.Count == 0
                )
            {
                //                                          //out parameters can not be used inside process of lambda
                //                                          //      expressions
                List<Error> darrerrorTemporal = errors_O;
                TEntity entityToAdd = entityAdded_O;
                Entity[] arrentity = this.GetEntitiesToLockInAdd(jsonadd_I);
                ToLock.MultipleLocks(this.GetEntitiesToLockInAdd(jsonadd_I), () =>
                {
                    //                                      //ACTION WITHIN MultipleLocks

                    darrerrorTemporal = entityDUMMY.VerifyIsJsonOkToAdd(jsonadd_I);
                    if (
                        darrerrorTemporal.Count == 0
                        )
                    {
                        this.TryAddWithValidData(darrerrorTemporal, out entityToAdd, jsonadd_I);
                    }
                });

                //                                          //Restore info to out parameters
                errors_O = darrerrorTemporal;
                entityAdded_O = entityToAdd;
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public void TryAddWithValidData(
            //                                              //Data was verify, try to add an entity to index and
            //                                              //      database.

            //                                              //Add errors. (Start with 0 errors)
            List<Error> errors_M,
            //                                              //null if errors.
            out TEntity entityAdded_O,
            //                                              //Include Unique Key information
            JsonAbstract jsonadd_I
            )
        {
            //                                              //To easy code
            TEntity entityDUMMY = new TEntity();

            //                                              //WILL NEED TO ROLLBACK if can not Insert (and Update the
            //                                              //      modified entities) in databae

            //                                              //This new entity has PK -1 (should be corrected later)
            entityAdded_O = (TEntity)entityDUMMY.EntityNew(jsonadd_I);

            //                                              //Add to database & correct PK
            long longPrimaryKeyDb;
            this.TryAddToDatabase(errors_M, entityAdded_O, out longPrimaryKeyDb);
            //                                              //Intentionally this method is called here:
            //                                              //1. Is NOT called in concrete class to keep it PRIVATE in
            //                                              //      abstract code.
            //                                              //2. out paramenter in previous method was included to force
            //                                              //      assigment of PK.
            //                                              //3. out paramenter could be removed if this method was
            //                                              //      called in concrete class, but it could be easy to
            //                                              //      forgot.
            entityAdded_O.z_TowaPRIVATE_subAssignPk(longPrimaryKeyDb);

            if (
                errors_M.Count == 0
                )
            {
                //                                          //Should lock new entity before adding to table
                lock (entityAdded_O.tolockEntity)
                {
                    //                                      //Add (and/or Update) entity to both index.
                    this.MakeSureBothIndexExist(entityAdded_O);

                    this.TryAddComplement(errors_M, entityAdded_O, jsonadd_I);
                }
            }
            else
            {
                entityAdded_O.RollBackAdd();

                entityAdded_O = null;
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        protected abstract void TryAddToDatabase(
            //                                              //Add entry to DB table and update Pk on entity. 

            //                                              //Add database error(s) if found (should start with no 
            //                                              //      errors)
            List<Error> darrerror_M,
            //                                              //Update pk (authomatic generation)
            TEntity entity_M,
            out long longPrimaryKeyDb_O
            );

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        protected void DatabaseErrorTryAdd(

            List<Error> darrerror_M,
            long PrimaryKeyBelongsTo_I,
            TKey key_I,
            Exception sysexcepDatabase_I
            )
        {
            //                                              //To easy code
            TEntity entityDUMMY = new TEntity();

            Error errorDatabase = new Error("DB-15", "Table",
                "Insert " + entityDUMMY.entityName + "(UniqueKey<" + PrimaryKeyBelongsTo_I + ", " + key_I.ToString() +
                    ">) failed, " + Test.ToLog(sysexcepDatabase_I, "Exception"));
            darrerror_M.Add(errorDatabase);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        protected virtual void TryAddComplement(
            //                                              //Some entities additional process.
            //                                              //Ex. to add an organization, will require to add his
            //                                              //      manager (first collaborator). Also,
            //                                              //      this collaborator will require his bo (first 
            //                                              //      business object).

            //                                              //Add database error(s) if found (should start with no 
            //                                              //      errors)
            List<Error> darrerror_M,
            //                                              //Update pk (authomatic generation)
            TEntity entity_M,
            //                                              //json object to add.
            JsonAbstract jsonadd_I
            )
        {
            //                                              //NOTHING TO DO IN THIS "virtual" METHOD.
        }

        //--------------------------------------------------------------------------------------------------------------
        /*ACCESS METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        protected abstract Entity[] GetEntitiesToLockInAdd(
            //                                              //Each concrete table "knows" what entities should lock

            JsonAbstract jsonadd_I
            );

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
