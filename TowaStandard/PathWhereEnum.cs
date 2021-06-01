/*TASK PathWhereEnum*/
//                                                          //AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: 19-Febrero-2014.
//                                                          //PURPOSE:
//                                                          //Indica dónde se encuenta el directorio o file referido en
//                                                          //      path.

namespace TowaStandard
{
    //==================================================================================================================
    public enum PathWhereEnum : byte
    {
        //                                                  //Esta en el mismo equipo (c:...).
        LOCAL,
        //                                                  //Esta en la red (\\...).
        NETWORK,
    }

    //==================================================================================================================

}
/*END-TASK*/