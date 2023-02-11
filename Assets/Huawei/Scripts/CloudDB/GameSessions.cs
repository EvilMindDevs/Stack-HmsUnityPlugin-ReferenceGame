using HuaweiMobileServices.CloudDB;
using HuaweiMobileServices.Utils;
using UnityEngine;

using System;

namespace HmsPlugin
{
	public class GameSessions : JavaObjectWrapper, ICloudDBZoneObject
	{
		public GameSessions() : base("com.refapp.stackpro.huawei.GameSessions") { }
		public GameSessions(AndroidJavaObject javaObject) : base(javaObject) { }
		private string id;
		private string huaweiIdMail;
		private int sessionNumber;
		private int score;

		public string Id
		{
			get { return Call<string>("getId"); }
			set { Call("setId", value); }
		}

		public string HuaweiIdMail
		{
			get { return Call<string>("getHuaweiIdMail"); }
			set { Call("setHuaweiIdMail", value); }
		}

		public int SessionNumber
		{
			get { return Call<AndroidJavaObject>("getSessionNumber").Call<int>("intValue"); }
			set { Call("setSessionNumber", new AndroidJavaObject("java.lang.Integer", value)); }
		}

		public int Score
		{
			get { return Call<AndroidJavaObject>("getScore").Call<int>("intValue"); }
			set { Call("setScore", new AndroidJavaObject("java.lang.Integer", value)); }
		}

		public AndroidJavaObject GetObj() => base.JavaObject;
		public void SetObj(AndroidJavaObject arg0) => base.JavaObject = arg0;
	}
}
