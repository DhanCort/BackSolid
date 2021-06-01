/*TASK Bclass Base Class*/
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
    public abstract class BclassAbstract : BobjAbstract
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

        //                                                  //Define si el objeto es INMUTABLE, MUTABLE o OPEN.
        //                                                  //Solo los MUTABLE recolectan información en UsedIn.
        //                                                  //Nótese que un objeto es MUTABLE si al menos UNA de sus
        //                                                  //      varibles es MUTABLE, esta variable puede estár en
        //                                                  //      la clase concreta o en cualquiera de las clases
        //                                                  //      abstractas que le dan forma (de esto se excluye
        //                                                  //      Bclass que es un caso especial).
        //                                                      
        public abstract BclassmutabilityEnum bclassmutability { get; }

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
        private static List<BclassAbstract> darrbclassConstructed;

        //                                                  //Total de UsedIn en TODOS los objetos MUTABLE de la
        //                                                  //      aplicación.
        private static int intUsedInAddTotalCount;
        private static int intUsedInRemoveTotalCount;

        //                                                  //Conjunto de Type de objectos DUMMY.
        //                                                  //Por ESTÁNDARD solo se permite construír un objeto DUMMY
        //                                                  //      (objDUMMY_UNIQUE) para cada clase concreta.
        //                                                  //Este arreglo permite asegurar que se cumpla esté estándar,
        //                                                  //      se abortara si no se cumple.
        private static List<Type> darrtypeDummyUnique;

        //--------------------------------------------------------------------------------------------------------------
        /*INITIALIZER*/

        //--------------------------------------------------------------------------------------------------------------
        static BclassAbstract(
            //                                              //Inicializa información estática.
            )
        {
            BclassAbstract.ResetSummary();

            //                                              //Inicializa el arreglo de DUMMY_UNIQUE.
            BclassAbstract.darrtypeDummyUnique = new List<Type>();
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
            BclassAbstract.dicintObjectCount = new Dictionary<String, int>();
            BclassAbstract.darrbclassConstructed = new List<BclassAbstract>();

            BclassAbstract.intUsedInAddTotalCount = 0;
            BclassAbstract.intUsedInRemoveTotalCount = 0;
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

            BclassAbstract.ResetSummary();
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
            foreach (KeyValuePair<String, int> kvpint in BclassAbstract.dicintObjectCount)
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
            String[] arrstrKeyObjectCount = new String[BclassAbstract.dicintObjectCount.Count];
            BclassAbstract.dicintObjectCount.Keys.CopyTo(arrstrKeyObjectCount, 0);
            int[] arrintValueObjectCount = new int[BclassAbstract.dicintObjectCount.Count];
            BclassAbstract.dicintObjectCount.Values.CopyTo(arrintValueObjectCount, 0);
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
                "Total Count: Objects {0:#,##0}, UsedIn Added {1:#,##0}, UsedIn Removed {2:#,##0}", intObjectsCount,
                BclassAbstract.intUsedInAddTotalCount, BclassAbstract.intUsedInRemoveTotalCount);
            Test.Log(strSums);

            if (
                Test.z_TowaPRIVATE_boolIsComparableLog()
                )
            {
                Test.Log("");
                String strObjs = String.Format("Objects constructed: {0:#,##0}",
                    BclassAbstract.darrbclassConstructed.Count);
                Test.Log(strObjs);
                for (int intI = 0; intI < BclassAbstract.darrbclassConstructed.Count; intI = intI + 1)
                {
                    BclassAbstract bclass = BclassAbstract.darrbclassConstructed[intI];
                    Test.Log("[" + intI + "] " + Test.ToLog(bclass));
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
            BclassAbstract.WriteSummary();

            BclassAbstract.ResetSummary();

            Test.Log("");
            Test.Log(strLabel_I + " SUMMARY");
        }

        //--------------------------------------------------------------------------------------------------------------
        /*INSTANCE VARIABLES*/

        //                                                  //Indica si este objeto es DUMMY
        private readonly bool boolIsDummy_Z;
        public bool IsDummy { get { return this.boolIsDummy_Z; } }

        //                                                  //Indica si el objeto ha sido reseteado.
        //                                                  //Saber esto es necesario para no propagar el reset con el
        //                                                  //      arreglo UsedIn lo cual podría causar un ciclo.
        //                                                  //Esto funciona de la siguiente forma:
        //                                                  //a. Al iniciar la contrucción del objeto se establece este
        //                                                  //      valor en true. El objeto nace reseteado.
        //                                                  //b. Cada vez que se ejecuta subResetObject(), esto se hará:
        //                                                  //b1. Al concluir la construcción del objeto concreto.
        //                                                  //b2. Al iniciar un método de transformación.
        //                                                  //b3. Cuando se modifique un objeto.
        //                                                  //c. Al calcular cualquier variable calculada se debe
        //                                                  //      ejecutar el método subSetIsResetOff() el cual pondrá
        //                                                  //      este valor en false, lo cual indica que al menos una
        //                                                  //      variable calculada ya tiene valor.
        private /*MUTABLE*/ bool boolIsReset;

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

        //                                                  //Registra objetos (concretos) que "usan" la información de
        //                                                  //      "este" objeto.
        //                                                  //Esto es necesario, dado que si este objeto es modificado,
        //                                                  //      por lo cual requiere ser reseteado, el reseteo debe
        //                                                  //      propagarse a todos los objetos que "usan" "este
        //                                                  //      objeto.
        //                                                  //Ejemplo, un objeto Journal Entry esta en USD y hace
        //                                                  //      referencia a un objeto Currency para tomar de ahí
        //                                                  //      los tipos de cambio, este objeto Journal Entry debe
        //                                                  //      añadirse al arreglo de "used in" del objeto currency
        //                                                  //      para que al cambiar algo en currency le pueda avisar
        //                                                  //      a Journal Entry que cambio.
        //                                                  //Nótese que el añadir una referencia de "uso" NO SIGNIFICA
        //                                                  //      que este objeto fue modificado (no se resetea).
        //                                                  //Solo los objetos MUTABLE recolectan esta información, en
        //                                                  //      los otros objetos, este valor debe ser null.
        //                                                  //Solo los objetos MUTABLE pueden ser contenidos en este
        //                                                  //      arreglo.
        private /*MUTABLE*/ List<BclassAbstract> darrbclassThisIsUsedIn;

        //--------------------------------------------------------------------------------------------------------------
        protected abstract void ResetOneClass(
            //                                              //ESTE MÉTODO SE DEBE INCLUIR EN TODAS LAS CLASES.
            //                                              //Este método resetea solo las variables calculadas para una
            //                                              //      clase, esto es:
            //                                              //1. Inicia con la clase concreta, el método
            //                                              //      subResetObject(), al final ejecuta el método
            //                                              //      this.subResetOneClass().
            //                                              //2. Al concluir la ejecución de cada uno de lo métodos
            //                                              //      subResetOneClass() ejecuta el método
            //                                              //      base.subResetOneClass() para resetear las variables
            //                                              //      calculadas que están en la clase inmediata
            //                                              //      abstracta, hasta llegar a la clase que hereda de
            //                                              //      bclassBaseClass en dónde el método
            //                                              //      subResetOneClass() ya no vuelve a resetear.
            );
         
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

            return strCLASS + "{" + Test.ToLog(this.arrstrPrefixThisIsUsedIn(), "this.arrstrPrefixThisIsUsedIn") + "}";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private String[] arrstrPrefixThisIsUsedIn(
            //                                              //Sometimes content is convertes to str values.
            //                                              //
            //                                              //To call this method:
            //                                              //(see examples above).
            //                                              //arrbclass, darrbclass converted

            //                                              //this[I], darrbclassThisIsUsedIn
            )
        {
            String[] arrstrPrefix;
            if (
                this.darrbclassThisIsUsedIn == null
                )
            {
                arrstrPrefix = null;
            }
            else
            {
                arrstrPrefix = new String[this.darrbclassThisIsUsedIn.Count];
                for (int intI = 0; intI < this.darrbclassThisIsUsedIn.Count; intI = intI + 1)
                {
                    String strObjId = Test.GetObjId(this.darrbclassThisIsUsedIn[intI]);

                    //                                      //ObjId has the form Prefix:HashCode
                    arrstrPrefix[intI] = strObjId.Substring(0, strObjId.LastIndexOf(':'));
                }
            }

            return arrstrPrefix;
        }

        //--------------------------------------------------------------------------------------------------------------
        /*OBJECT CONSTRUCTORS*/

        //--------------------------------------------------------------------------------------------------------------
        protected BclassAbstract(                  //Inicializa la parte más abstracta de cada objeto, y.
            //                                              //Acumula a la parte estática la creación de un objeto de
            //                                              //      cierto type.
            //                                              //this.*[O], Asigna darrbclass vacía. 

            //                                              //true es DUMMY, false tiene info.
            bool boolIsDummy_I
            )
            : base()
        {
            //                                              //INSTANCE PART.

            //                                              //Un objeto "nace" reseteado.
            this.boolIsReset = true;

            //                                              //This is THE ONLY value asigned to a DUMMY object
            this.boolIsDummy_Z = boolIsDummy_I;

            if (
                //                                          //Estamos en un objeto DUMMY
                boolIsDummy_I
                )
            {
                //                                          //STATIC PART (ONE SET OF INFORMATION FOR THE APPLICATION).

                if (
                    //                                      //Ya se tiene un DUMMY de este tipo.
                    false/*PARCHE*/ && BclassAbstract.darrtypeDummyUnique.Contains(this.GetType())
                    )
                    Test.Abort(Test.ToLog(this.GetType(), "this.type") + " a DUMMY object already exists",
                        Test.ToLog(BclassAbstract.darrtypeDummyUnique, 
                            "BclassBaseClassAbstract.darrtypeDummyUnique"));

                //                                          //Registra objeto DUMMY en el arreglo de DUMMYs
                BclassAbstract.darrtypeDummyUnique.Add(this.GetType());

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

            //                                              //Inicializa arrglo de UsedIn
            if (
                this.bclassmutability == BclassmutabilityEnum.MUTABLE
                )
            {
                this.darrbclassThisIsUsedIn = new List<BclassAbstract>();
            }
            else
            {
                //                                          //Solo los objetos MUTABLE recolectan esta información.
                this.darrbclassThisIsUsedIn = null;
            }

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
                String strTypeThisFullNameAndMutability = this.GetType().Name + "|" + this.bclassmutability;

                //                                          //Create dictionary entry if needed.
                if (
                     BclassAbstract.dicintObjectCount.ContainsKey(strTypeThisFullNameAndMutability)
                    )
                {
                    //                                     //Do nothing
                }
                else
                {
                    BclassAbstract.dicintObjectCount.Add(strTypeThisFullNameAndMutability, 0);
                }

                //                                          //Add count
                BclassAbstract.dicintObjectCount[strTypeThisFullNameAndMutability] =
                    BclassAbstract.dicintObjectCount[strTypeThisFullNameAndMutability] + 1;

                if (
                    Test.z_TowaPRIVATE_boolIsComparableLog()
                    )
                {
                    BclassAbstract.darrbclassConstructed.Add(this);
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
        public void ResetObject(
            //                                              //Este método inicia el proceso de resetaer todas las
            //                                              //      variables calculadas de un objeto:
            //                                              //1. Indica que el objeto esta reseteado, en realidad apenas
            //                                              //      esta iniciando pero lo hará ejecutando al final de
            //                                              //      este método this.subResetOneClass().
            //                                              //2. Se debe ejecutar al principio de cada método de
            //                                              //      transformación, esto es necesario para:
            //                                              //2a. Si en el proceso algo detona algún reset que se
            //                                              //      a este objeto, no se vuelve a resetear (lo cual
            //                                              //      causaría un ciclo.
            //                                              //2b. El método de transformación NO USA variables
            //                                              //      calculadas.
            //                                              //2c. Al concluir el método de transformación se debe
            //                                              //      ejecutar subVerifyIsReset(), aborta si algo sucedió.
            //                                              //3. Propaga el reseteo a todos los objetos que usan este
            //                                              //      objeto.
            //                                              //Este método deberá ser ejecutado al inicio de un método de
            //                                              //      transformación.
            )
        {
            //                                              //LOG TEMPORAL PARA ENTENDER LA SECUENCIA DE RESETEO.
            /*Test.Log("    <<<RESET UP>>> " + Test.LogTo(this.boolIsReset, "Reset") + ", " +
                this.LogTo(TestoptionEnum.SHORT));*/

            if (
                this.boolIsReset
                )
            {
                //                                          //YA ESTA RESETEANO, NO HACE NADA
            }
            else
            {
                //                                          //Indica que el objeto esta reseteado reseteado. En realidad
                //                                          //      apenas se esta iniciando el reseteo pero se hará al
                //                                          //      final de este método.
                this.boolIsReset = true;

                if (
                    //                                      //Tiene "used in", es MUTABLE.
                    this.darrbclassThisIsUsedIn != null
                    )
                {
                    //                                      //LOG TEMPORAL PARA ENTENDER LA SECUENCIA DE RESETEO.
                    //Test.Log("    <<<USES_IN>>>");

                    //                                      //Propaga el reseteo a los objetos que usan este objeto.
                    foreach (BclassAbstract bclassThisObjectIsUsedIn in this.darrbclassThisIsUsedIn)
                    {
                        //                                  //En su ejecución, si ya está reseteado no se hace nada.
                        bclassThisObjectIsUsedIn.ResetObject();
                    }
                }

                //                                          //Ahora si, INICIA el reseteo de este objeto.
                //                                          //INICIA por la clase concreta.
                this.ResetOneClass();
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        protected void ResetOff(
            //                                              //Indica que no esta reseteado.
            //                                              //Este métodos se debe ejecutar cada vez que se calcula una
            //                                              //      Variable Calculada.
            //                                              //this[M], modifica reset.
            )
        {
            //                                              //Indica que no esta reseteado.
            this.boolIsReset = false;
        }

        //--------------------------------------------------------------------------------------------------------------
        public void AddUsedIn(                           //Añade una referencia UsedIn.
            //                                              //this[M], Añade referencia UsedIn.

            //                                              //Objeto MUTALBE que usa this.
            //                                              //Ejemplo, Journal Entry que usa Currency (se pasa un
            //                                              //      Journal Entry el this que recibe este método es un
            //                                              //      Currency.
            BclassAbstract bclassToAdd_T
            )
        {
            //                                              //Solo los objetos MUTABLE pueden tener UsedIn
            if (
                //                                          //Este objeto (this) no es MUTABLE
                this.bclassmutability != BclassmutabilityEnum.MUTABLE
                )
                Test.Abort(
                    Test.ToLog(this) + 
                        " should be mutable to call subAddUsedIn(...)",
                    Test.ToLog(this.bclassmutability, "this.bclassmutability"));

            //                                              //Solo los objetos MUTABLE pueden ser incluídos en este
            //                                              //      arreglo.
            if (
                //                                          //El objeto (bclassToAdd_T) no es MUTABLE
                bclassToAdd_T.bclassmutability != BclassmutabilityEnum.MUTABLE
                )
                Test.Abort(
                    Test.ToLog(bclassToAdd_T) +
                        " should be mutable to add with subAddUsedIn(bclassToAdd_T)",
                    Test.ToLog(bclassToAdd_T.bclassmutability, "bclassToAdd_T.bclassmutability"));

            this.darrbclassThisIsUsedIn.Add(bclassToAdd_T);
                BclassAbstract.intUsedInAddTotalCount = BclassAbstract.intUsedInAddTotalCount + 1;
        }

        //--------------------------------------------------------------------------------------------------------------
        public void RemoveUsedIn(                        //Remueve una referencia UsedIn.
            //                                              //this[M], Remueve referencia UsedIn.

            //                                              //Objeto que usaba this y que será removido.
            //                                              //Ejemplo, Journal Entry que usa Currency (se pasa un
            //                                              //      Journal Entryy el this que recibe este método es un
            //                                              //      Currency.
            BclassAbstract bclassToRemove_T
            )
        {
            //                                              //Solo los objetos MUTABLE pueden tener UsedIn
            if (
                //                                          //Este objeto (this) no es MUTABLE
                this.bclassmutability != BclassmutabilityEnum.MUTABLE
                )
                Test.Abort(
                    Test.ToLog(this) +
                        " should be mutable to call subRemoveUsedIn(...)",
                    Test.ToLog(this.bclassmutability, "this.bclassmutability"));

            //                                              //Solo los objetos MUTABLE pueden ser parte de este arreglo.
            if (
                //                                          //El objeto (bclassToAdd_T) no es MUTABLE
                bclassToRemove_T.bclassmutability != BclassmutabilityEnum.MUTABLE
                )
                Test.Abort(
                    Test.ToLog(bclassToRemove_T) +
                        " should be mutable to remove with subRemoveUsedIn(bclassToRemove_T)",
                    Test.ToLog(bclassToRemove_T.bclassmutability, "bclassToRemove_T.bclassmutability"));

            //                                              //Localiza el objeto y lo elimina.
            int intPos = this.darrbclassThisIsUsedIn.IndexOf(bclassToRemove_T);
            if (
                intPos < 0
                )
                Test.Abort(Test.ToLog(bclassToRemove_T) + " IS NOT in darrbclassThisIsUsedIn");

            this.darrbclassThisIsUsedIn.RemoveAt(intPos);
            BclassAbstract.intUsedInRemoveTotalCount = BclassAbstract.intUsedInRemoveTotalCount +
                1;
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
                Test.Abort(Test.ToLog(this) + " can not be DUMMY for this method",
                    Test.ToLog(this.IsDummy, "this.boolIsDummy"));
            if (
                //                                          //El objeto aún no esta completo
                !this.boolIsObjectConstructionFinished
                )
                Test.Abort(
                    Test.ToLog(this) +
                        " object construction IS NOT FINISHED, its functionality can not be used yet",
                    Test.ToLog(this.boolIsObjectConstructionFinished, "this.boolIsObjectConstructionFinished"));
        }

        //--------------------------------------------------------------------------------------------------------------
        protected void VerifyIsReset(
            //                                              //Un método de tranforamción NO DEBE USAR variables
            //                                              //      calculadas, si se usaron se cancelo el reset.
            //                                              //Si sucede lo anterior ES INCORRECTO y debe abortar..
            //                                              //this[I], base object info.
            )
        {
            if (
                this.IsDummy
                )
                Test.Abort(Test.ToLog(this) + " can not be DUMMY for this method",
                    Test.ToLog(this.IsDummy, "this.boolIsDummy"));
            if (
                //                                          //El objeto marca como usadas las variables calculadas
                !this.boolIsReset
                )
                Test.Abort(
                    Test.ToLog(this) +
                        " object IS NOT RESET, transformation methods should not use computed variables",
                    Test.ToLog(this.boolIsReset, "this.boolIsReset"));
        }

    }

    //==================================================================================================================
}
/*END-TASK*/