/*TASK T3fakecharFakeCharacterTuple*/
using System;
using System.Text;
using System.Collections.Generic;

//                                                          //AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: 16-Febrero-2016.
//                                                          //PURPOSE:
//                                                          //Implementación de funciones o subrutinas de uso compartido
//                                                          //      en todos los sistemas.

namespace TowaStandard
{
    //==================================================================================================================
    public class T0maskTuple : BtupleAbstract
    {
        //                                                  //Conjunto de parámetors para "mask" la información de
        //                                                  //      pruebas que requiren Comparación Automática.
        //                                                  //(EN EL FUTURO SE PUEDEN AÑADIR MÁS VALORES, SE PUEDEN
        //                                                  //      VARIOS CONSTURCTORES).
        //

        //--------------------------------------------------------------------------------------------------------------

        //                                                  //Conjunto de directorios a enmascarar, esto es, todos los
        //                                                  //      path que pertenecen a alguno de estos directorios,
        //                                                  //      el directorio es enmascarado (el inicio de full path
        //                                                  //      se substituye con strMASK).
        public PathX[] arrsyspathDirectory;
 
        //                                                  //Cuando se requiere un dtNow, en ves de tomar el .Now del
        //                                                  //      del sistema se toma este dt, a los subsecuentes
        //                                                  //      se les agrega numDeltaSeconds del arreglo en el
        //                                                  //      siguiente orden (Ej. si se tienen 4 deltas en el
        //                                                  //      arreglo, se toma 0, 1, 2, 3, 0, 1, ....).
        public DateTime dtNowBase;
        public double[] arrnumDeltaSeconds;

        //--------------------------------------------------------------------------------------------------------------
        public override String ToLogShort()
        {
            return "<" + Test.ToLog(this.arrsyspathDirectory) + ", " +
                Test.ToLog(this.dtNowBase) + ", " + 
                Test.ToLog(this.arrnumDeltaSeconds) + ">";
        }

        //--------------------------------------------------------------------------------------------------------------
        public override String ToLogFull()
        {
            return "<" + Test.ToLog(this.arrsyspathDirectory, "arrsyspathDirectory") + ", " +
                Test.ToLog(this.dtNowBase, "dtNowBase") + ", " + 
                Test.ToLog(this.arrnumDeltaSeconds, "arrnumDeltaSeconds") + ">";
        }

        //--------------------------------------------------------------------------------------------------------------
        public T0maskTuple(PathX[] arrsyspathDirectory_I, DateTime dtNowBase_I, double[] arrnumDeltaSeconds_I)
            : base()
        {
            this.arrsyspathDirectory = arrsyspathDirectory_I;
            this.dtNowBase = dtNowBase_I;
            this.arrnumDeltaSeconds = arrnumDeltaSeconds_I;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
