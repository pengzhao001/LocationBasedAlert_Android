
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace LocationAlert
{
	[BroadcastReceiver]
	public class ProximityIntentReceiver : BroadcastReceiver
	{
		public override void OnReceive (Context context, Intent intent)
		{

			// get the notification manager
			NotificationManager notificationManager = (NotificationManager)context.GetSystemService (Context.NotificationService); // as NotificationManager;

			//PendingIntent pendingIntent = PendingIntent.GetActivity (context, 0, null, 0);


			Notification.Builder builder = new Notification.Builder (context)
				.SetAutoCancel (true)
				.SetContentTitle ("Alert")
				.SetContentText ("You are near your point of interest")
				.SetDefaults (NotificationDefaults.Sound)
				.SetSmallIcon (Resource.Drawable.logo);

			// build the notification
			Notification notification = builder.Build(); 


			//Notification notification = new Notification ();

			//notification.Icon = Resource.Drawable.logo;
			//notification.SetLatestEventInfo (context, "Alert", "You are near your point of interest", pendingIntent);




			// publish the notification
			const int notificationId = 0;
			notificationManager.Notify(notificationId, notification);

			Console.WriteLine ("Notification Fired");


			/*
			Notification notification = createNotification ();
			notification.SetLatestEventInfo (context, "Alert", "You are near your point of interest", pendingIntent);
			notificationManager.Notify (1000, notification);

			//Toast.MakeText (context, "Received intent!", ToastLength.Short).Show ();

            */
		}
			
	}
}





