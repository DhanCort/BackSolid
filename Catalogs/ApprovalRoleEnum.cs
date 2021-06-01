/*TASK RP. RESOURCES*/

//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia).
//                                                          //COAUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //DATE: 20-11-2019.

namespace Odyssey2Backend.Catalogs
{
    //==================================================================================================================
    public enum ApprovalRoleEnum
    {
        //                                                  //The decision of this approver immediately overrides 
        //                                                  //      the decisions of the other approvers and ends the 
        //                                                  //      approval cycle
        Approvinator,
        //                                                  //The approver belongs to a group of which @MinApprovals 
        //                                                  //      members SHALL sign.
        Group,
        //                                                  //The approver is informed of the Approval process, but 
        //                                                  //      the approval is still valid, even without his 7
        //                                                  //      approval.
        Informative,
        //                                                  //The approver SHALL sign the approval.
        Obligated
    }

    //==================================================================================================================
}
/*END-TASK*/
