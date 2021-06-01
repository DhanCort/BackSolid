/*TASK RP. RESOURCES*/

//                                                          //AUTHOR: Towa (VSTD - Victor Torres).
//                                                          //CO-AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //DATE: November 19, 2019.

//======================================================================================================================
enum ComponentTypeEnum
{
    //                                                      //Folded or stacked product (e.g., book block).
    Block,
    //                                                      //The Component describes a sample that has not been 
    //                                                      //      produced in this job. Examples are perfume samples, 
    //                                                      //      CDs or toys that are inserted into a printed 
    //                                                      //      product. 
    Other,
    //                                                      //The Component is a ribbon on a web press.
    Ribbon,
    //                                                      //Single layer (sheet) of paper.
    Sheet,
    //                                                      //The Component is a web on a web press. 
    Web,
    //                                                      //The Component is the final product that was ordered by the
    //                                                      //      customer.
    FinalProduct,
    //                                                      // The Component is an intermediate product that will be 
    //                                                      //      input to a following process.
    PartialProduct,
    //                                                      //The Component is a proof (e.g., a press proof or output 
    //                                                      //      from a digital press). 
    Proof
}

//======================================================================================================================

/*END-TASK*/
