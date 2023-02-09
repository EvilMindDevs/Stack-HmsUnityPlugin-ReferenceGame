using HuaweiMobileServices.CloudDB;
using HuaweiMobileServices.Utils;
using UnityEngine;

using System;

namespace HmsPlugin
{
	public class testObjectTypeName : JavaObjectWrapper, ICloudDBZoneObject
	{
		public testObjectTypeName() : base("com.refapp.stackpro.huawei") { }
		public testObjectTypeName(AndroidJavaObject javaObject) : base(javaObject) { }
		private string field1;
		private long field2;
		private int field3;
		private string field4;
		private float field5;
		private byte field6;

		public string Field1
		{
			get { return Call<string>("getField1"); }
			set { Call("setField1", value); }
		}

		public long Field2
		{
			get { return Call<AndroidJavaObject>("getField2").Call<long>("longValue"); }
			set { Call("setField2", new AndroidJavaObject("java.lang.Long", value)); }
		}

		public int Field3
		{
			get { return Call<AndroidJavaObject>("getField3").Call<int>("intValue"); }
			set { Call("setField3", new AndroidJavaObject("java.lang.Integer", value)); }
		}

		public string Field4
		{
			get { return Call<AndroidJavaObject>("getField4").Call<string>("get"); }
			set { Call("setField4", new AndroidJavaObject("com.huawei.agconnect.cloud.database.Text", value)); }
		}

		public float Field5
		{
			get { return Call<AndroidJavaObject>("getField5").Call<float>("floatValue"); }
			set { Call("setField5", new AndroidJavaObject("java.lang.Float", value)); }
		}

		public byte Field6
		{
			get { return Call<AndroidJavaObject>("getField6").Call<byte>("byteValue"); }
			set { Call("setField6", new AndroidJavaObject("java.lang.Byte", value)); }
		}

		public AndroidJavaObject GetObj() => base.JavaObject;
		public void SetObj(AndroidJavaObject arg0) => base.JavaObject = arg0;
	}
}
