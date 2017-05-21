using DataAccess.Models;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;

namespace ChatBot.Services
{
	[Serializable]
	public class NotifyService
	{
		public void RunScheduler(Notification notification)
		{
			if (notification == null || notification.User == null)
			{
				return;
			}

			var scheduler = StdSchedulerFactory.GetDefaultScheduler();
			scheduler.Start();

			IDictionary<string, object> dictionary = new Dictionary<string, object>
			{
				{ "description", notification.Description },
				{ "user", notification.User }
			};

			JobDataMap map = new JobDataMap(dictionary);

			IJobDetail job = JobBuilder.Create<NotificationJob>()
				.WithIdentity("notificationJob", "notificationGroup")
				.UsingJobData(map)
				.Build();

			ITrigger trigger = TriggerBuilder.Create()
				.WithIdentity("notificationTrigger", "notificationGroup")
				.StartAt(notification.Date)
				.Build();

			scheduler.ScheduleJob(job, trigger);
		}
	}
}