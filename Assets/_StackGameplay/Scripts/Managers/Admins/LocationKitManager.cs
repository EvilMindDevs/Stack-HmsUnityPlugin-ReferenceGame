using System;
using System.Text;
using GameDev.Library;
using GameDev.MiddleWare;
using HuaweiMobileServices.Ads;
using HuaweiMobileServices.Location;
using HuaweiMobileServices.Location.Location;
using UnityEngine;

namespace StackGamePlay
{
    public class LocationKitManager : MonoBehaviour
    {
        #region Variables

        private readonly string TAG = "LocationKitManager";
        private FusedLocationProviderClient fusedLocationProviderClient;
        private LocationRequest locationRequest;
        private LocationCallback locationCallback;

        #endregion

        #region Singleton

        public static LocationKitManager Instance;

        private void Singleton()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }

            DontDestroyOnLoad(gameObject);
        }

        #endregion

        private void Awake()
        {
            Singleton();
        }

        private void Start()
        {
            HMSLocationManager.Instance.RequestFineLocationPermission();
            HMSLocationManager.Instance.RequestCoarseLocationPermission();

            HMSLocationManager.Instance.onLocationResult += OnLocationResult;

            RequestLocationUpdatesWithCallback();
            GetLastKnownLocation();
        }

        private void OnEnable()
        {
            GLog.Log($"OnEnable", GLogName.LocationKitManager);
            this.AddListener<object>(GEventName.SESSION_PREPARE, OnSessionPrepare);
            this.AddListener<object>(GEventName.SESSION_STARTED, OnSessionStart);
            this.AddListener<object>(GEventName.SESSION_END, OnSessionEnd);
        }

        private void OnDisable()
        {
            GLog.Log($"OnDisable", GLogName.LocationKitManager);
            this.RemoveListener<object>(GEventName.SESSION_PREPARE, OnSessionPrepare);
            this.RemoveListener<object>(GEventName.SESSION_STARTED, OnSessionStart);
            this.RemoveListener<object>(GEventName.SESSION_END, OnSessionEnd);
        }

        private void OnLocationResult(LocationResult locationResult)
        {
            Debug.Log($"{TAG} Location Result success");
            var latitude = locationResult.GetLastLocation().GetLatitude();
            var longitude = locationResult.GetLastLocation().GetLongitude();

            DelegateStore.ShowLocation?.Invoke(latitude, longitude);
        }

        private void RequestLocationUpdatesWithCallback()
        {
            try
            {
                fusedLocationProviderClient = LocationServices.GetFusedLocationProviderClient();
                locationRequest = new LocationRequest();
                locationRequest.SetInterval(1000);

                if (locationCallback == null) locationCallback = HMSLocationManager.Instance.DefineLocationCallback();
                Debug.Log($"{TAG} RequestLocationUpdatesWithCallback 5 ");

                fusedLocationProviderClient
                    .RequestLocationUpdates(locationRequest, locationCallback, Looper.GetMainLooper())
                    .AddOnSuccessListener(
                        (update) => { Debug.Log($"{TAG} RequestLocationUpdatesWithCallback success"); })
                    .AddOnFailureListener((exception) =>
                    {
                        Debug.LogError($"{TAG} LocationCallBackListener Fail" + exception.WrappedCauseMessage + " " +
                                       exception.WrappedExceptionMessage +
                                       $"{TAG} RequestLocationUpdates Error code: " +
                                       exception.ErrorCode);
                    });
            }
            catch (Exception exception)
            {
                Debug.Log(
                    $"{TAG} Exception: {exception.Message} {exception.InnerException.Message}");
            }
        }

        public void GetLastKnownLocation()
        {
            locationRequest = new LocationRequest();
            Debug.Log($"{TAG} RequestLocationUpdatesWithCallback 3 ");
            locationRequest.SetInterval(1000);
            fusedLocationProviderClient = LocationServices.GetFusedLocationProviderClient();
            fusedLocationProviderClient.GetLastLocationWithAddress(locationRequest)
                .AddOnSuccessListener(location =>
                {
                    Debug.Log($"{TAG} GetLastKnownLocation success");
                    if (location != null)
                    {
                        Debug.Log(
                            $"{TAG} GetLastKnownLocation success222 {location.GetLatitude().ToString() + "," + location.GetLatitude().ToString()}");
                    }
                    else
                    {
                        Debug.LogError($"{TAG} GetLastKnownLocation not available");
                    }
                }).AddOnFailureListener(exception =>
                    {
                        Debug.LogError($"{TAG} GetLastKnownLocation exception: {exception}");
                    }
                );
        }

        #region Events: OnSessionPrepare

        private void OnSessionPrepare(object sender, GEvent<object> eventData)
        {
            GLog.Log($"OnSessionPrepare", GLogName.LocationKitManager);
        }

        #endregion

        #region Events: OnSessionStart

        private void OnSessionStart(object sender, GEvent<object> eventData)
        {
            GLog.Log($"OnSessionStart", GLogName.LocationKitManager);
        }

        #endregion

        #region Events: OnSessionEnd

        private void OnSessionEnd(object sender, GEvent<object> eventData)
        {
            GLog.Log($"OnSessionEnd", GLogName.LocationKitManager);
        }

        #endregion
    }
}