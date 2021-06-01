/*TASK RP. RESOURCES*/

//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia).
//                                                          //COAUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //DATE: 20-11-2019.

namespace Odyssey2Backend.Catalogs
{
    //==================================================================================================================
    public enum ArtHandlingEnum
    {
        //                                                  //The printer destroys the artwork.
        Destroy,
        //                                                  //The customer picks up the artwork.
        Pickup,
        //                                                  //The artwork belongs to the printer.
        PrinterOwns,
        //                                                  //The artwork is returned to the customer independently 
        //                                                  //      directly after usage.
        Return,
        //                                                  //The artwork is returned to the customer together with 
        //                                                  //      the final product.
        ReturnWithProduct,
        //                                                  //The artwork is returned to the customer together with 
        //                                                  //      the proof if there is any.
        ReturnWithProof,
        //                                                  //The printer has to store the artwork for future purposes.
        Store
    }

    //==================================================================================================================
}
/*END-TASK*/
