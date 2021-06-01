/*TASK RP. Resources*/
//                                                          //CO-AUTHOR: Towa (DPG - Daniel Pena).
//                                                          //DATE: 2019/11/11.

namespace Odyssey2Backend.XJDF
{
    //=================================================================================================================
    public enum JDFStatusEnum
    {
        //                                                  //Indicates that the process executing the JDF 
        //                                                  //      has been aborted
        Aborted,
        //                                                  //The process represented by this node is 
        //                                                  //      currently being cleaned up.
        Cleanup,
        //                                                  //Indicates that the node or queue entry has been executed
        //                                                  //      correctly, and is finished.
        Completed,
        //                                                  //An error occurred during the test run.
        FailedTestRun,
        //                                                  //The node is currently executing.
        InProgress,
        //                                                  //Indicates that the node is processing partitioned 
        //                                                  //      resources and that the status varies depending upon 
        //                                                  //      the partition keys.
        //                                                  //Details are provided in the NodeInfo resource of the
        //                                                  //      node.
        Part,
        //                                                  //Indicates that the node is processing partitioned 
        //                                                  //      resources and that the status varies
        //                                                  //      depending upon the partition keys. 
        //                                                  //Details are provided in the StatusPool element of the
        //                                                  //      node.
        Pool,
        //                                                  //The node is ready to start.
        Ready,
        //                                                  //The process represented by this node is 
        //                                                  //      currently being set up.
        Setup,
        //                                                  //The node is spawned in the form of a 
        //                                                  //      separate spawned JDF
        Spawned,
        //                                                  //Execution has been stopped. If a job is "Stopped", 
        //                                                  //      running can be resumed later. 
        Stopped,
        //                                                  //Execution has been stopped. If a job is "Suspended", 
        //                                                  //      running will be resumed later. 
        Suspended,
        //                                                  //The node is currently executing a test run.
        TestRunInProgress,
        //                                                  //The node can be executed.
        Waiting
    }

    //==================================================================================================================
}
/*END-TASK*/
