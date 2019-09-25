using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using APS.MC.Shared.APSShared.Enums;

namespace APS.MC.Shared.APSShared.Notifications
{
	public abstract class Notifiable : INotifiable
	{
		protected Notifiable()
		{
			Notifications = new List<Notification>();
		}

		public void AddNotification(string property, ENotifications message) => AddNotification(property, Description(message));
		public void AddNotification(string property, string message) => AddNotification(new Notification(property, message));
		public void AddNotification(Notification notification) => Notifications.Add(notification);
		public void AddNotifications(IEnumerable<Notification> notifications) => Notifications.Concat(notifications);
		public void AddNotifications(Notifiable item) => AddNotifications(item.Notifications);
		public void AddNotifications(params Notifiable[] items)
		{
			foreach (Notifiable item in items)
				AddNotifications(item);
		}

		public bool Valid { get; private set; }
		public IList<Notification> Notifications { get; private set; }

		private string Description(ENotifications notification)
		{
			DescriptionAttribute[] attributes = (DescriptionAttribute[])notification.GetType().GetField(notification.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);

			return attributes != null && attributes.Length > 0 ? attributes[0].Description : notification.ToString();
		}
	}
}