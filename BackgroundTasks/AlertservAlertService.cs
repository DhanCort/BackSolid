/*TASK RP.BACKGROUNDTASKS*/
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Odyssey2Backend.PrintShop;
using Microsoft.AspNetCore.SignalR;
using Odyssey2Backend.Alert;
using Odyssey2Backend.Utilities;
using TowaStandard;
using Odyssey2Backend.Job;
using Microsoft.Extensions.Configuration;

//                                                          //AUTHOR: Towa (IUGS - Ivan Guzman).
//                                                          //CO-AUTHOR: 
//                                                          //DATE: August 08, 2020.

namespace Odyssey2Backend.BackgroundTasks
{
    //==================================================================================================================
    public class AlertservAlertService : IHostedService, IDisposable
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        private readonly IHubContext<ConnectionHub> ihubcontext_Z;
        private readonly ILogger<AlertservAlertService> ilogger_Z;
        private IConfiguration configuration;
        private Timer timerTask_Z;
        private Timer timerDueDateAtRisk_Z;
        private Timer timerDueDateInThePast_Z;


        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //--------------------------------------------------------------------------------------------------------------
        public AlertservAlertService(
            ILogger<AlertservAlertService> ilogger_I,
            IHubContext<ConnectionHub> ihubcontext_I,
            IConfiguration iConfiguration_I
            )
        {
            this.ilogger_Z = ilogger_I;
            this.ihubcontext_Z = ihubcontext_I;
            this.configuration = iConfiguration_I;
            this.timerTask_Z = null;
            this.timerDueDateAtRisk_Z = null;
            this.timerDueDateInThePast_Z = null;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //TRANSFORMATION METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public Task StartAsync(
            //                                              //IHostedService.StartAsync implementation. Called before: 
            //                                              //      - The app's request processing pipeline is 
            //                                              //          configured (Startup.Configure).
            //                                              //      - The server is started and 
            //                                              //          IApplicationLifetime.ApplicationStarted is 
            //                                              //          triggered.
            //                                              //      This method contains the logic to initialize the 
            //                                              //      timer. Declaring the task that will be performed in
            //                                              //      an specific period of time.

            CancellationToken cancellationtoken_I
            )
        {
            this.ilogger_Z.LogInformation("Alert service running.");

            //                                              //Start timers.

            //                                              //The offset seconds are calculated to synchronize the timer
            //                                              //      with the hosting clock.s
            int intOffsetSeconds = 60 - Time.Now(ZonedTimeTools.timezone).Seconds;
            //                                              //Task's alert timer.
            this.timerTask_Z = new Timer(subSendTaskAlert, null, TimeSpan.FromSeconds(intOffsetSeconds), 
                TimeSpan.FromSeconds(60));
            //                                              //Due date at risk timer.
            this.timerDueDateAtRisk_Z = new Timer(subSendDueDateAlert, null, TimeSpan.FromSeconds(intOffsetSeconds),
                TimeSpan.FromSeconds(60));
            //                                              //Due date in the past timer.
            this.timerDueDateInThePast_Z = new Timer(subSendDueDateInThePastAlert, null, 
                TimeSpan.FromSeconds(intOffsetSeconds), TimeSpan.FromSeconds(60));

            return Task.CompletedTask;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private void subSendTaskAlert(
            //                                              //This is the task tha will be performed in an specific 
            //                                              //      period of time.

            object state_I
            )
        {
            EmplEmployee.subSendTasksAlertsAndDeleteOld(this.ihubcontext_Z);
            String strTimeNow = Time.Now(ZonedTimeTools.timezone).ToString();
            this.ilogger_Z.LogInformation("Task Alert service is working at: " + strTimeNow);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private void subSendDueDateAlert(
            //                                              //This is the task tha will be performed in an specific 
            //                                              //      period of time.

            object state_I
            )
        {
            JobJob.subDeletePeriodsNotStartedOnTime(this.ihubcontext_Z);
            String strTimeNow = Time.Now(ZonedTimeTools.timezone).ToString();
            this.ilogger_Z.LogInformation("Due date at risk service is working at: " + strTimeNow);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private void subSendDueDateInThePastAlert(
            //                                              //This is the task tha will be performed in an specific 
            //                                              //      period of time.

            object state_I
            )
        {
            JobJob.subSendDueDateInThePastNotification(this.ihubcontext_Z, this.configuration);
            String strTimeNow = Time.Now(ZonedTimeTools.timezone).ToString();
            this.ilogger_Z.LogInformation("Due date in the past service is working at: " + strTimeNow);
        }


        //--------------------------------------------------------------------------------------------------------------
        public Task StopAsync(
            //                                              //IHostedService.StopAsync implementation. Triggered when 
            //                                              //      the host is performing a graceful shutdown. 
            //                                              //      This method contains the logic to disabled the
            //                                              //      timer. 

            CancellationToken cancellationtoken_I
            )
        {
            //                                              //"?" means that only if the object is different to null 
            //                                              //      it is going to do the action.

            //                                              //Stop task alert timer.
            this.ilogger_Z.LogInformation("Task Alert service is stopping.");
            this.timerTask_Z?.Change(Timeout.Infinite, 0);

            //                                              //Stop due date at risk timer.
            this.ilogger_Z.LogInformation("Due date at risk service is stopping.");
            this.timerDueDateAtRisk_Z?.Change(Timeout.Infinite, 0);

            //                                              //Stop due date in the past timer.
            this.ilogger_Z.LogInformation("Due date in the past service is stopping.");
            this.timerDueDateInThePast_Z?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        //--------------------------------------------------------------------------------------------------------------
        public void Dispose(
            //                                              //IDisposable.Dispose implementation. This method is
            //                                              //      executed when the service container is disposed and
            //                                              //      contains the logic to dipose the timer.
            )
        {
            this.timerTask_Z?.Dispose();
            this.timerDueDateAtRisk_Z?.Dispose();
            this.timerDueDateInThePast_Z?.Dispose();
        }
    }

    //==================================================================================================================
}
/*END-TASK*/
