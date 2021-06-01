/*TASK Bmutable Base Mutable Class*/
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
    public abstract class BmutableAbstract : BobjAbstract
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
        /*STATIC VARIABLES*/

        //--------------------------------------------------------------------------------------------------------------
        /*INITIALIZER*/

        //--------------------------------------------------------------------------------------------------------------
        /*STATIC METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        /*INSTANCE VARIABLES*/

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
        private /*MUTABLE*/ List<BmutableAbstract> darrbclassThisIsUsedIn = /*REVISAR*/null;

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

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private BmutableAbstract[] arrbclassThisIsUsedInDarrTo(
            //                                              //The name of this method should be:
            //                                              //darrobjXxxxx ==> arrobjXxxxxDarrTo().
            //                                              //queueobjXxxxx ==> arrobjXxxxxQueueTo().
            //                                              //stackobjXxxxx ==> arrobjXxxxxStackTo().
            //                                              //For dic convertion, the name of the methods should be:
            //                                              //dicobjXxxxx ==> arrobjXxxxxDicTo() &
            //                                              //      arrstrKeyXxxxxDicTo().
            //                                              //
            //                                              //darr, queue and stack of bclass, btuple, enum or Exception
            //                                              //      need to be converted to arrobj before calling LogTo
            //                                              //      method with 3 paramenters.
            //                                              //This method is an example and should be coded after LogTo
            //                                              //      methods.
            //                                              //
            //                                              //To call this method:
            //                                              //(see examples above).
            //                                              //If darr, ... is static, paramenter or local variable, you
            //                                              //      need an static method and pass darr,... as
            //                                              //      paramenter.
            //                                              //arrbclass, darrbclass converted

            //                                              //this[I], darrbclassThisIsUsedIn
            )
        {
            BmutableAbstract[] arrbclassThisIsUsedInDarrTo;
            if (
                this.darrbclassThisIsUsedIn == null
                )
            {
                arrbclassThisIsUsedInDarrTo = null;
            }
            else
            {
                arrbclassThisIsUsedInDarrTo = this.darrbclassThisIsUsedIn.ToArray();
            }

            return arrbclassThisIsUsedInDarrTo;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private String[] arrstrPrefix(                      //Sometimes content is convertes to str values.
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
                    String strObjId = null/*Test.GetObjId(this.darrbclassThisIsUsedIn[intI])*/;

                    //                                      //ObjId has the form Prefix:HashCode
                    arrstrPrefix[intI] = strObjId.Substring(0, strObjId.LastIndexOf(':'));
                }
            }

            return arrstrPrefix;
        }

        //--------------------------------------------------------------------------------------------------------------
        /*OBJECT CONSTRUCTORS*/

        //--------------------------------------------------------------------------------------------------------------
        protected BmutableAbstract(                  //Inicializa la parte más abstracta de cada objeto, y.
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

            if (
                //                                          //Estamos en un objeto DUMMY
                boolIsDummy_I
                )
            {
                //                                          //STATIC PART (ONE SET OF INFORMATION FOR THE APPLICATION).

                //                                          //El objeto DUMMY no se contabiliza en el diccionario.
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        /*TRANSFORMATION METHODS*/

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
                    foreach (BmutableAbstract bclassThisObjectIsUsedIn in this.darrbclassThisIsUsedIn)
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
            BmutableAbstract bclassToAdd_T
            )
        {
            this.darrbclassThisIsUsedIn.Add(bclassToAdd_T);
                //BmutableClassAbstract.intUsedInAddTotalCount = BmutableClassAbstract.intUsedInAddTotalCount + 1;
        }

        //--------------------------------------------------------------------------------------------------------------
        public void RemoveUsedIn(                        //Remueve una referencia UsedIn.
            //                                              //this[M], Remueve referencia UsedIn.

            //                                              //Objeto que usaba this y que será removido.
            //                                              //Ejemplo, Journal Entry que usa Currency (se pasa un
            //                                              //      Journal Entryy el this que recibe este método es un
            //                                              //      Currency.
            BmutableAbstract bclassToRemove_T
            )
        {
            int intPos = -1;

            this.darrbclassThisIsUsedIn.RemoveAt(intPos);
            //BmutableClassAbstract.intUsedInRemoveTotalCount = BmutableClassAbstract.intUsedInRemoveTotalCount + 1;
        }

        //--------------------------------------------------------------------------------------------------------------
        /*ACCESS METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        protected void VerifyIsReset(
            //                                              //Un método de tranforamción NO DEBE USAR variables
            //                                              //      calculadas, si se usaron se cancelo el reset.
            //                                              //Si sucede lo anterior ES INCORRECTO y debe abortar..
            //                                              //this[I], base object info.
            )
        {
            if (
                //                                          //El objeto marca como usadas las variables calculadas
                !this.boolIsReset
                )
                Test.Abort(
                    /*Test.ToLog(this) +*/
                        " object IS NOT RESET, transformation methods should not use computed variables",
                    Test.ToLog(this.boolIsReset, "this.boolIsReset"));
        }

    }

    //==================================================================================================================
}
/*END-TASK*/