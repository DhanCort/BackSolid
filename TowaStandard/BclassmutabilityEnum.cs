/*TASK Bclass Base Class*/
//                                                          //AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: June 24, 2015.
//                                                          //PURPOSE:

namespace TowaStandard
{
    //==================================================================================================================
    public enum BclassmutabilityEnum : byte
    {
        //                                                  //Posibles tipos de objeto.
        //                                                  //Objetos en los cuales (en ninguno de sus niveles de
        //                                                  //      abstracción) tiene variables de instancia mutables
        //                                                  //      (una ves construidos estos objetos, NUNCA pueden ser
        //                                                  //      alterados).
        //                                                  //Nótese que su contenido puede incluir objetos mutables
        //                                                  //      (arreglos, objetos MUTABLE, u objetos OPEN) sin
        //                                                  //      embargo, si están usados en un objeto INMUTABLE
        //                                                  //      QEnabler se asegura que núnca cambian.
        INMUTABLE,
        //                                                  //Objetos que tiene variables de instancia que pueden
        //                                                  //      cambiar su valor durante la vida del objeto.
        MUTABLE,
        //                                                  //Objetos similares a arreglos, dinamic arrays, linked list,
        //                                                  //      Queue, Stack, etc., donde su contenido no tiene
        //                                                  //      ningún tipo de restricción para su acceso y/o
        //                                                  //      modificación.
        OPEN,
    }

    //==================================================================================================================
}
/*END-TASK*/