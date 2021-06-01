/*TASK Binmuutable Base Inmutable Class*/
using System;
using System.Collections.Generic;

//                                                          //AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: June 24, 2015.
//                                                          //PURPOSE:
//                                                          //Base for all classes.

namespace TowaStandard
{
    //==================================================================================================================
    public abstract class BimmutableAbstract : BobjAbstract
    {
        //                                                  //Clase base para todos los objetos, conforme al estandar
        //                                                  //      Towa, TODOS los objetos que diseñemos deben heredar
        //                                                  //      de esta clase.
        //                                                  //Entre otras cosas, esta clase provee facilidades para
        //                                                  //      evaluar el desempeño de una aplicación desarrollada
        //                                                  //      conforme a estos estándares.
        //                                                  //(algo ya esta aquí, sin embargo en el futuro se puede
        //                                                  //      añadir más capacidades, ojo se debe SER CAUTELOSO
        //                                                  //      dado que todo esto afectará la eficiencia).

        //--------------------------------------------------------------------------------------------------------------
        /*CONSTANTS*/

        //--------------------------------------------------------------------------------------------------------------
        public virtual String[] InitializationInfo(
            //                                              //arrstr, constants to show

            //                                              //this[I], all constants.
            )
        {
            String[] arrstrInitializationInfo = { "<NOT IMPLEMENTED> "/* + Test.ToLog(this, "this")*/ };

            return arrstrInitializationInfo;
        }

        //--------------------------------------------------------------------------------------------------------------
        /*STATIC VARIABLES*/

        //                                                  //Diccionario para registrar la cantidad de todos los
        //                                                  //      objetos que contruye la aplicación al estar
        //                                                  //      operando.
        //                                                  //Llave: Type, será el Name de la clase concreta del
        //                                                  //      objeto.
        //                                                  //Info: Cantidad de objetos que se han creado durante la 
        //                                                  //      operación de la aplicación.
        //                                                  //No se contabilizan los objetos DUMMY.
        private static Dictionary<String, int> dicintObjectCount;

        //                                                  //Cuando se esta en modo Comparable Log se guarda esta
        //                                                  //      información de TODOS los objetos.
        //                                                  //No se guardan los objetos DUMMY.
        private static List<BimmutableAbstract> darrbclassConstructed;

        //--------------------------------------------------------------------------------------------------------------
        /*INITIALIZER*/

        //--------------------------------------------------------------------------------------------------------------
        static BimmutableAbstract(
            //                                              //Inicializa información estática.
            )
        {
            BimmutableAbstract.ResetSummary();
        }

        //--------------------------------------------------------------------------------------------------------------
        /*STATIC METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public static void ResetSummary(
            //                                              //Inicia los valores que son sumarizado en subWriteSummary.
            )
        {
            //                                              //Inicializa el diccionario para la cantidad de objetos que
            //                                              //      construye la aplicación.
            BimmutableAbstract.dicintObjectCount = new Dictionary<String, int>();
            BimmutableAbstract.darrbclassConstructed = new List<BimmutableAbstract>();
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void ResetSummary(
            //                                              //Igual a sin parametros, pero añade una etiqueta de 
            //                                              //      identificación.
            //                                              //También reinicia la cuenta

            String strLabel_I
            )
        {
            Test.Log("");
            Test.Log(strLabel_I + " RESET");

            BimmutableAbstract.ResetSummary();
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void WriteSummary(
            //                                              //Escribe en el log de pruebas la información de la
            //                                              //      aplicación que se encuentra en la parte estática de
            //                                              //      esta clase.
            )
        {
            Test.Log("");
            Test.Log(" Bclass SUMMARY ".Center(60, '#', '#'));

            //                                              //Compute necesary values
            int intObjectsCount = 0;
            int intLengthKey = 0;
            foreach (KeyValuePair<String, int> kvpint in BimmutableAbstract.dicintObjectCount)
            {
                intObjectsCount = intObjectsCount + kvpint.Value;

                if (
                    kvpint.Key.Length > intLengthKey
                    )
                {
                    intLengthKey = kvpint.Key.Length;
                }
            }

            String strClassMutability = "class|mutability";
            intLengthKey = Std.MaxOf(strClassMutability.Length, intLengthKey);

            //                                              //Order info.
            String[] arrstrKeyObjectCount = new String[BimmutableAbstract.dicintObjectCount.Count];
            BimmutableAbstract.dicintObjectCount.Keys.CopyTo(arrstrKeyObjectCount, 0);
            int[] arrintValueObjectCount = new int[BimmutableAbstract.dicintObjectCount.Count];
            BimmutableAbstract.dicintObjectCount.Values.CopyTo(arrintValueObjectCount, 0);
            Std.Sort(arrstrKeyObjectCount, arrintValueObjectCount);

            Test.Log(strClassMutability.PadRight(intLengthKey, '-') + " Object Count");

            //                                              //Report object count
            for (int intI = 0; intI < arrstrKeyObjectCount.Length; intI = intI + 1)
            {
                String strEntry = String.Format("{0} {1:#,##0}", arrstrKeyObjectCount[intI].PadRight(intLengthKey),
                    arrintValueObjectCount[intI]);
                Test.Log(strEntry);
            }

            Test.Log("");
            String strSums = String.Format(
                "Total Count: Objects {0:#,##0}", intObjectsCount);
            Test.Log(strSums);

            if (
                Test.z_TowaPRIVATE_boolIsComparableLog()
                )
            {
                Test.Log("");
                String strObjs = String.Format("Objects constructed: {0:#,##0}",
                    BimmutableAbstract.darrbclassConstructed.Count);
                Test.Log(strObjs);
                for (int intI = 0; intI < BimmutableAbstract.darrbclassConstructed.Count; intI = intI + 1)
                {
                    BimmutableAbstract bclass = BimmutableAbstract.darrbclassConstructed[intI];
                    Test.Log("[" + intI + "] "/* + Test.ToLog(bclass)*/);
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void WriteSummary(
            //                                              //Igual a sin parametros, pero añade una etiqueta de 
            //                                              //      identificación.
            //                                              //También reinicia la cuenta

            String strLabel_I
            )
        {
            BimmutableAbstract.WriteSummary();

            BimmutableAbstract.ResetSummary();

            Test.Log("");
            Test.Log(strLabel_I + " SUMMARY");
        }

        //--------------------------------------------------------------------------------------------------------------
        /*INSTANCE VARIABLES*/

        //                                                  //Indica si este objeto es DUMMY
        private readonly bool boolIsDummy_Z;
        public bool IsDummy { get { return this.boolIsDummy_Z; } }


        //                                                  //Indica si la construcción del objecto ya fue concluída.
        //                                                  //Saber esto es conveniente para poder proteger el código
        //                                                  //      de manera que se diagnostique (ABORTE) si antes de
        //                                                  //      que este construído completamente el objeto se
        //                                                  //      pretende hacer referencia a:
        //                                                  //1. Alguna variable calculada.
        //                                                  //2. Algún método de transformación.
        //                                                  //3. Algún método de consulta.
        //                                                  //Esto funciona de la siguiente forma:
        //                                                  //a. Al iniciar la contrucción del objeto (en el constructor
        //                                                  //      de esta clase que es la más abstracta de todos los
        //                                                  //      objetos) se establece este valor en false.
        //                                                  //b. Al concluír la construcción del objeto, al hacer el
        //                                                  //      subResetObject() se establece este valor en true.
        //                                                  //c. Nótese que cada vez que se haga el subResetObject() se
        //                                                  //      vuelve a establecer en true, para efectos de lo que
        //                                                  //      se busca esto no es necesario, sin embargo no
        //                                                  //      afecta.
        //                                                  //d. Al iniciar el proceso de: Variable Calculada, Método de
        //                                                  //      Transformación y Método Acceso se ejecuta el método
        //                                                  //      subVerifyObjectConstructionFinished() el cuál
        //                                                  //      abortará si aún no esta concluída la construcción
        //                                                  //      del objeto.
        private /*MUTABLE*/ bool boolIsObjectConstructionFinished;

         
        //--------------------------------------------------------------------------------------------------------------
        public override String ToLogShort(
            //                                              //SHORT display.
            //                                              //THIS METHOD SHOULD BE IMPLEMENTED IN EVERY CLASS (ABSTRACT
            //                                              //      OR CONCRETE).
            //                                              //The final format of the String will be:
            //                                              //ObjId[BclassVariables, AbstractVariables, ...,
            //                                              //      AbstractVariables, ConcreteVariables].
            //                                              //To produce this String:
            //                                              //1. Concrete class produces:
            //                                              //ObjId[base.strto(S) + Variable + ... + Variable].
            //                                              //2. All abstract classes (except Bclass) produce:
            //                                              //base.strto(S) + Variable + ... + Variable.
            //                                              //3. Bclass produces:
            //                                              //Variable + ... + Variable, (see below).
            //                                              //4. Variable is:.
            //                                              //4a. Test.LogTo(Variable, TestoptionEnum.SHORT).
            //                                              //4b. When variable is darrobj, queueobj or stackobj you
            //                                              //      need to call LogTo with 3 parameters, this method is
            //                                              //      an example (see support methods below).
            //                                              //4c. When variable is dirobj you need to call LogTo with 4
            //                                              //      parameters (see example in class 
            //                                              //      SemsolooObjectOriented).
            //                                              //4d. When variable is vkpobj you need to call LogTo with 4
            //                                              //      parameters (no example included, should be similar
            //                                              //      to 4c but simpler).
            //                                              //4e. obj is class, tuple, enum or Exception (other object
            //                                              //      should use 2 paramenter methods).
            //                                              //(see examples).
            //                                              //this[I], all its instance variables.

            //                                              //str, display information
            )
        {
            //                                              //En la versión corta se decidió no agragar nada.
            return "*";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public override String ToLogFull(
            //                                              //FULL display.
            //                                              //THIS METHOD SHOULD BE IMPLEMENTED IN EVERY CLASS (ABSTRACT
            //                                              //      OR CONCRETE).
            //                                              //The final format of the String will be:
            //                                              //ObjId{Variables}==>Class{Variables}==>...==>
            //                                              //      Class{Variables}==>Bclass{Variables}. 
            //                                              //To produce this String:
            //                                              //1. Concrete class produces:
            //                                              //ObjId{Variable + ... + Variable}==>base.strto().
            //                                              //2. All abstract classes (except Bclass) produce:
            //                                              //ClassPrefix{Variable + ... + Variable}==>base.LogTo().
            //                                              //3. Bclass produces:
            //                                              //Bclass{Variable + ... + Variable}.
            //                                              //4. Variable is:.
            //                                              //4a. Test.LogTo(Variable, "Variable").
            //                                              //4b-e (see method description above).
            //                                              //this[I], all its instance variables.

            //                                              //str, display information
            )
        {            
            const String strCLASS = "Bclass";

            //                                              //Will report only prefix of the objects in 
            //                                              //      darrbclassThisIsUsedIn (can be null)

            return strCLASS + "{}";
        }

        //--------------------------------------------------------------------------------------------------------------
        /*OBJECT CONSTRUCTORS*/

        //--------------------------------------------------------------------------------------------------------------
        protected BimmutableAbstract(                  //Inicializa la parte más abstracta de cada objeto, y.
            //                                              //Acumula a la parte estática la creación de un objeto de
            //                                              //      cierto type.
            //                                              //this.*[O], Asigna darrbclass vacía. 

            //                                              //true es DUMMY, false tiene info.
            bool boolIsDummy_I
            )
            : base()
        {
            //                                              //INSTANCE PART.

            //                                              //This is THE ONLY value asigned to a DUMMY object
            this.boolIsDummy_Z = boolIsDummy_I;

            if (
                //                                          //Estamos en un objeto DUMMY
                boolIsDummy_I
                )
            {
                //                                          //STATIC PART (ONE SET OF INFORMATION FOR THE APPLICATION).
                /*
                if (
                    //                                      //Ya se tiene un DUMMY de este tipo.
                    BimmutableClassAbstract.darrtypeDummyUnique.Contains(this.GetType())
                    )
                    Test.Abort(Test.ToLog(this.GetType(), "this.type") + " a DUMMY object already exists",
                        BimmutableClassAbstract.darrtypeDummyUnique.ToLog(
                            "BmutableClassAbstract.darrtypeDummyUnique"));

                //                                          //Registra objeto DUMMY en el arreglo de DUMMYs
                BimmutableClassAbstract.darrtypeDummyUnique.Add(this.GetType());
                */

                //                                          //El objeto DUMMY no se contabiliza en el diccionario.
            }

            //                                              //Indica que AÚN NO ESTA CONCLUÍDA la construcción del
            //                                              //      objeto.
            //                                              //Al concluir la construcción, en la clase concreta se
            //                                              //      ejecuta SetObjectConstructionFinished() que
            //                                              //      cambiara esto a true.
            //                                              //La asignación de false al principio ES NECESARIA para
            //                                              //      evitar que la funcionalidad del método se utilizada
            //                                              //      ANTES de concluir la construcción.
            this.boolIsObjectConstructionFinished = false;

            //                                              //STATIC PART (ONE SET OF INFORMATION FOR THE APPLICATION).

            //                                              //Solo contabiliza los objeto NO DUMMY's.
            //                                              //Tampoco syspath (en el futuro otros sys... dado que son
            //                                              //      similares a sysfile, sysdir, etc.).
            if (
                this.IsDummy
                )
            {
                //                                          //Do not add this objetc
            }
            else
            {
                String strTypeThisFullNameAndMutability = this.GetType().Name + "|"/* + this.bclassmutability*/;

                //                                          //Create dictionary entry if needed.
                if (
                     BimmutableAbstract.dicintObjectCount.ContainsKey(strTypeThisFullNameAndMutability)
                    )
                {
                    //                                     //Do nothing
                }
                else
                {
                    BimmutableAbstract.dicintObjectCount.Add(strTypeThisFullNameAndMutability, 0);
                }

                //                                          //Add count
                BimmutableAbstract.dicintObjectCount[strTypeThisFullNameAndMutability] =
                    BimmutableAbstract.dicintObjectCount[strTypeThisFullNameAndMutability] + 1;

                if (
                    Test.z_TowaPRIVATE_boolIsComparableLog()
                    )
                {
                    BimmutableAbstract.darrbclassConstructed.Add(this);
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        /*TRANSFORMATION METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public void SetObjectConstructionFinish(
            //                                              //Este método indica que el objeto ya esta completamente
            //                                              //      construído.
            //                                              //Este método deberá ser ejecutado al concluir la
            //                                              //      construcción de un objeto en su clase concreta.
            )
        {

            //                                              //Indica que YA ESTA CONCLUÍDA la construcción del objeto.
            //                                              //A partir de esto ya será posible accesar la funcionalidad
            //                                              //      de este objeto.
            this.boolIsObjectConstructionFinished = true;

            //                                              //LOG TEMPORAL PARA ENTENDER LA SECUENCIA DE RESETEO.
            //Test.Log("    <<<CONSTRUCCIÓN>>> " + this.LogTo(TestoptionEnum.SHORT));
        }

        //--------------------------------------------------------------------------------------------------------------
        /*ACCESS METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        protected void AbortIfConstructionNotFinished(
            //                                              //Object functionality, this is:
            //                                              //1. Computed Variables.
            //                                              //2. Access Methods.
            //                                              //3. Transformation Methods.
            //                                              //can be used only on real object (non DUMMY) after its
            //                                              //      construction is finished.
            //                                              //this[I], base object info.
            )
        {
            if (
                this.IsDummy
                )
                Test.Abort(/*Test.ToLog(this) + */" can not be DUMMY for this method",
                    Test.ToLog(this.IsDummy, "this.boolIsDummy"));
            if (
                //                                          //El objeto aún no esta completo
                !this.boolIsObjectConstructionFinished
                )
                Test.Abort(
                    /*Test.ToLog(this) +*/
                        " object construction IS NOT FINISHED, its functionality can not be used yet",
                    Test.ToLog(this.boolIsObjectConstructionFinished, "this.boolIsObjectConstructionFinished"));
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/