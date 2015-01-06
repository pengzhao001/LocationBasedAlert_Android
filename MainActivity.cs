using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Locations;


// Need to simulate GPS in terminal: telnet localhost 5554
// Set Geocode to MU hospital (Longitude, Latitude): geo fix -92.3321071206523 38.9361985

// GPX file source: http://www.openstreetmap.org/user/jerjozwik/traces/970816 
// Visulize GPX route: http://www.gpsvisualizer.com
// Starting point: 33.940715, -117.258725 
// Set point of interest: Sunnymead Park 12655 Perris Boulevard, Moreno Valley, CA 92553

namespace LocationAlert
{
	[Activity (Label = "Locationbased Alert", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity , ILocationListener
	{
		TextView locationText;
		TextView radiusText;
		TextView distanceText;
		//Location currentLocation;
		Location destination;
		LocationManager locationManager;
		String locationProvider;
		private static string PROX_ALERT_INTENT = "LocationAlert.LocationAlert.LocationBasedAlert";


		// Get lattitude and longitude of current location
		// http://developer.xamarin.com/recipes/android/os_device_resources/gps/get_current_device_location/



		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Main);

			locationText = FindViewById<TextView> (Resource.Id.locationText);
			radiusText = FindViewById<TextView> (Resource.Id.radiusText);
			distanceText = FindViewById<TextView> (Resource.Id.distanceText);
			var button = FindViewById<Button> (Resource.Id.button);

			// Initialize location manager

			InitializeLocationManager();

			locationManager = (LocationManager)GetSystemService (LocationService);



			// Pass the specified destination and radius to program by clicking button

			button.Click += (sender, e) => {


			
				// Use Geocoder to get longitude and lattitude for destination
				// http://developer.xamarin.com/recipes/android/os_device_resources/geocoder/geocode_an_address/

				var geo = new Geocoder (this);

				var addresses = geo.GetFromLocationName (locationText.Text, 1);

				Address endLocation = addresses.ToList().FirstOrDefault();

				double endLat = endLocation.Latitude;
				double endLon = endLocation.Longitude;
				double radius = Convert.ToDouble(radiusText.Text)*1609.34; // in meters
				float radius_meters = (float)radius;

				destination = new Location(locationProvider);
				destination.Latitude = endLat;
				destination.Longitude = endLon;

				locationManager.RequestLocationUpdates (locationProvider, 0, 0, this);

				Intent intent = new Intent(PROX_ALERT_INTENT);
				PendingIntent proximityIntent = PendingIntent.GetBroadcast(this, 0, intent, 0);

				locationManager.AddProximityAlert(endLat, endLon, radius_meters, -1, proximityIntent);

				IntentFilter filter = new IntentFilter(PROX_ALERT_INTENT);
				RegisterReceiver(new ProximityIntentReceiver(), filter);






				/*

				// Calculate the distance between current address and the input address
				// http://androidapi.xamarin.com/index.aspx?link=M%3AAndroid.Locations.Location.DistanceBetween(System.Double%2CSystem.Double%2CSystem.Double%2CSystem.Double%2CSystem.Single%5B%5D)

				float[] list = new float[1];

				Location.DistanceBetween(currentLat, currentLon, endLat, endLon, list);

				// Show distance between current address and destination in the text field

				distance = list[0]/1000;
				double distance_round = Math.Round(Convert.ToDouble(distance), 3);
				distanceText.Text = distance_round.ToString();

				// Console.WriteLine ("distance: " + distance);

				// Alert if the distance is smaller than the specified radius

				if (distance_round <= Convert.ToDouble(radiusText.Text))
				{ 
					// if choose screen alert
					// alert(sender, e);

					// use notification
					// http://developer.xamarin.com/guides/cross-platform/application_fundamentals/notifications/android/local_notifications_in_android/

					// instantiate the builder and set notification elements
					Notification.Builder builder = new Notification.Builder(this)
						.SetAutoCancel(true)
						.SetContentTitle("Alert")
						.SetContentText("You are within " + radiusText.Text + " km to your destination")
						.SetDefaults(NotificationDefaults.Sound)
						.SetSmallIcon(Resource.Drawable.logo);

					// build the notification
					Notification notification = builder.Build();

					// get the notification manager
					NotificationManager notificationManager = GetSystemService (Context.NotificationService) as NotificationManager;

					// publish the notification
					const int notificationId = 0;
					notificationManager.Notify(notificationId, notification);

				} */
			};
		}

		// Set alert dialog in android
		// https://diptimayapatra.wordpress.com/2013/07/08/xamarin-alert-dialogs-in-android/

		void alert (object sender, EventArgs e)
		{   
			Android.App.AlertDialog.Builder builder = new AlertDialog.Builder (this);
			AlertDialog alertDialog = builder.Create ();
			alertDialog.SetTitle ("Alert");
			alertDialog.SetMessage ("You are within " + radiusText.Text + " km to your destination");
			alertDialog.SetButton ("OK", (s,EventArgs) => {});
			alertDialog.Show ();
		}

		protected override void OnResume()
		{
			base.OnResume();
			//locationManager.RequestLocationUpdates(locationProvider, 0, 0, this);
		}

		protected override void OnPause()
		{
			base.OnPause();
			//locationManager.RemoveUpdates(this);
		}

		// Initialize location manager
		// Choose the best location provider
		// http://developer.xamarin.com/recipes/android/os_device_resources/gps/get_current_device_location/

		void InitializeLocationManager()
		{
			locationManager = (LocationManager)GetSystemService(LocationService);
			Criteria criteriaForLocationService = new Criteria
			{
				Accuracy = Accuracy.Fine
			};
			IList<string> acceptableLocationProviders = locationManager.GetProviders(criteriaForLocationService, true);

			if (acceptableLocationProviders.Any())
			{
				locationProvider = acceptableLocationProviders.First();
			}
			else
			{
				locationProvider = String.Empty;
			}
		}


			public void OnLocationChanged(Location currentLocation)
			{
				double currentLat = currentLocation.Latitude;
				double currentLon = currentLocation.Longitude;
				Console.WriteLine("Current Location: " + currentLat.ToString() + ", " + currentLon.ToString());

				float distance = currentLocation.DistanceTo (destination); // in meters
			    double distance_round = Math.Round(Convert.ToDouble(distance)/1609.34, 3); // in miles
				distanceText.Text = distance_round.ToString();

			}

			public void OnProviderDisabled(string provider) {}

			public void OnProviderEnabled(string provider) {}

			public void OnStatusChanged(string provider, Availability status, Bundle extras) {}



	
	
	
	
	
	
	
	
	}


}


