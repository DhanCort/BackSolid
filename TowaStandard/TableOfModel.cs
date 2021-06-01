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
    public abstract class TableOfModel<TKey, TModel> : BsysAbstract
        where TKey : IComparable /*String, int, long, char, Date or Time*/
        where TModel : EntityModel
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //This is a singleton class to simulate a table of a
        //                                                  //      relational data base.
        //                                                  //It is intended to allow regression test that can be
        //                                                  //      verify authomatically.

        //--------------------------------------------------------------------------------------------------------------
        /*CONSTANTS*/

        //--------------------------------------------------------------------------------------------------------------
        /*INITIALIZER*/

        //--------------------------------------------------------------------------------------------------------------
        static TableOfModel(
            )
        {
            String strType = "TableOfModel<" + typeof(TKey).Name() + ", " + typeof(TModel).Name() + ">";
            Test.z_TowaPRIVATE_subAbortIfInvalidTKey(typeof(TKey), strType);
        }

        //--------------------------------------------------------------------------------------------------------------
        /*INSTANCE VARIABLES*/

        //                                                  //Instance Lock
        private readonly ToLock tolockInstance = new ToLock();

        //                                                  //Authomatic NEXT Primary Key.
        //                                                  //intPk if incremented by 1 before every insert to table
        private int intPkLast;

        //                                                  //Collections by PkBelongsTo+Key & Pk
        private readonly Dictionary<EntityKey<TKey>, TModel> ikeymodel;
        private readonly Dictionary<long, TModel> idxmodelByPk;

        //--------------------------------------------------------------------------------------------------------------
        public override String ToLogShort()
        {
            String strObjId = typeof(TableOfModel<TKey, TModel>).Name + ":" + this.GetHashCode();

            return strObjId + "{" + Test.ToLog(this.intPkLast) + ", " + Test.ToLog(this.ikeymodel) + ", " +
                Test.ToLog(this.idxmodelByPk) + "]";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public override String ToLogFull()
        {
            String strObjId = typeof(TableOfModel<TKey, TModel>).Name + ":" + this.GetHashCode();

            return strObjId + "{" + Test.ToLog(this.intPkLast, "intPkLast") + ", " +
                Test.ToLog(this.ikeymodel, "ikeymodel") + ", " + Test.ToLog(this.idxmodelByPk, "idxmodelByPk") + "}";
        }

        //--------------------------------------------------------------------------------------------------------------
        /*OBJECT CONSTRUCTORS*/

        //--------------------------------------------------------------------------------------------------------------
        protected TableOfModel()
        {
            this.intPkLast = 0;
            this.ikeymodel = new Dictionary<EntityKey<TKey>, TModel>();
            this.idxmodelByPk = new Dictionary<long, TModel>();
        }

        //--------------------------------------------------------------------------------------------------------------
        /*TRANSFORMATION METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public void SetInitialStateForTest(
            )
        {
            //                                              //Reset both index and pk next
            this.intPkLast = 0;
            this.ikeymodel.Clear();
            this.idxmodelByPk.Clear();

            //                                              //Load test data
            this.LoadTestData();
        }

        //--------------------------------------------------------------------------------------------------------------
        protected abstract void LoadTestData();

        //--------------------------------------------------------------------------------------------------------------
        public void Insert(
            //                                              //Insert a new model in table.

            //                                              //-1 if no BelongTo (Ex. Organization has no BelongsTo)
            int intPkBelongToToInsert_I,
            TKey keyToInsert_I,

            //                                              //Return null to make sure model that do not include PK is
            //                                              //      not return used any more
            TModel model_M
            )
        {
            if (
                //                                          //Entry in database already exist
                this[intPkBelongToToInsert_I, keyToInsert_I] != null
                )
                Test.Abort(
                    "this[" + intPkBelongToToInsert_I + ", " + keyToInsert_I + "] should not exist, " +
                        "DATABASE IS NOT SINCRONIZED WITH TABLES ALL-IN-MEMORY",
                    Test.ToLog(model_M, "model_IO"), Test.ToLog(this));

            EntityKey<TKey> ekey = new EntityKey<TKey>(intPkBelongToToInsert_I, keyToInsert_I);

            lock (this.tolockInstance)
            {
                //                                              //Add PK to model an add (insert) to tables
                this.intPkLast = this.intPkLast + 1;
                model_M.intPk = this.intPkLast;
                this.ikeymodel.Add(ekey, model_M);
                this.idxmodelByPk.Add(model_M.intPk, model_M);
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        /*ACCESS METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public TModel this[long longPkToSearch_I]
        {
            get
            {
                return (this.idxmodelByPk.ContainsKey(longPkToSearch_I)) ? this.idxmodelByPk[longPkToSearch_I] : null;
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public TModel this[EntityKey<TKey> ekeyToSearch_I]
        {
            get
            {
                return (this.ikeymodel.ContainsKey(ekeyToSearch_I)) ? this.ikeymodel[ekeyToSearch_I] : null;
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public TModel this[long longPkBelongToToSearch_I, TKey keyToSearch_I]
        { get { return this[new EntityKey<TKey>(longPkBelongToToSearch_I, keyToSearch_I)]; } }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
