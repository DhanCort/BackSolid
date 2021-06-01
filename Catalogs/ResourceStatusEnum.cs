/*TASK RP. RESOURCES*/

//                                                          //AUTHOR: Towa (VSTD - Victor Torres).
//                                                          //COAUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //DATE: November 11, 2019.

//======================================================================================================================
public enum ResourceStatusEnum
{
    //                                                      //Indicates thet the resource does not exist, and the 
    //                                                      //      metadata is not yet valid. Incomplete resources NEED
    //                                                      //      NOT specify all attributes or elements defined in
    //                                                      //      Resources.
    Incomplete,
    //                                                      //Indicates that the resource has been rejected by an
    //                                                      //      Approval process. The metadata is valid.
    Rejected,
    //                                                      //Indicates that the resource is not ready to be used or
    //                                                      //      that the resource in the real world represented
    //                                                      //      by the PhysicalResource in JDF is not available
    //                                                      //      for processing. The metadata is valid.
    Unavailable,
    //                                                      //Indicates that the resource exist, but is in use by
    //                                                      //      another process. Also used for active pipes.
    InUse,
    //                                                      //Indicates that the resource exist in a state that is
    //                                                      //      sufficient for setting up the next process but not
    //                                                      //      for production.
    Draft,
    //                                                      //Indicates that resource is completely specified and the
    //                                                      //      parameters are valid for usage. A PhysicalResource.
    Complete,
    //                                                      //Indicates that the whole resource is available for usage.
    Available
}

//======================================================================================================================

/*END-TASK*/
