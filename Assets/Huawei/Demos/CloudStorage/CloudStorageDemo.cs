﻿using HmsPlugin;

using HuaweiMobileServices.CloudStorage;
using HuaweiMobileServices.Utils;

using System;

using UnityEngine;
using UnityEngine.UI;

public class CloudStorageDemo : MonoBehaviour
{
    private readonly string TAG = "[HMS] HMSCloudStorageDemo";

    void Start()
    {
        //Requires modifying manifest file https://evilminddevs.gitbook.io/hms-unity-plugin/kits-and-services/cloud-storage/guides-and-references#modifying-the-androidmanifest-file
        HMSCloudStorageManager.CheckRequestUserPermissionForCloudStorage();

        HMSCloudStorageManager.Instance.OnUploadFileSuccess = OnUploadFileSuccess;
        HMSCloudStorageManager.Instance.OnDownloadFileSuccess = OnDownloadFileSuccess;
        HMSCloudStorageManager.Instance.OnDeleteFileSuccess = OnDeleteFileSuccess;

        HMSCloudStorageManager.Instance.OnAllFailureListeners = FailureListener;

        HMSCloudStorageManager.Instance.OnGetImageByteArray = OnGetImageByteArray;
        HMSCloudStorageManager.Instance.OnGetImageByteArrayFailure = OnGetImageByteArrayFailure;
    }

    public void UploadFile()
    {
        string filePath =
            System.IO.Path.Combine(Application.persistentDataPath, "testFile.jpg");//example: \HUAWEI P40 lite\Internal storage\Android\data\com.samet.reffapp.huawei\files\testFile.jpg
        HMSCloudStorageManager.Instance.UploadFile(filePathInDevice: filePath);
    }

    public void DownloadFile()
    {
        HMSCloudStorageManager.Instance.DownloadFile("testFile.jpg");
    }

    public void DeleteFile()
    {
        HMSCloudStorageManager.Instance.DeleteFile();
    }

    public void GetByteArray()
    {
        HMSCloudStorageManager.Instance.GetImageByteArray();
    }

    private void OnDeleteFileSuccess()
    {
        Debug.Log($"{TAG} Delete File Success");
        PrintDescription("Delete File Success");
    }

    private void OnDownloadFileSuccess(DownloadTask.DownloadResult result)
    {
        Debug.Log($"{TAG} Download File Success:" + result.BytesTransferred);
        PrintDescription("Download File Success");
    }

    private void OnUploadFileSuccess(UploadTask.UploadResult result)
    {
        Debug.Log($"{TAG} Upload File Success:" + result.BytesTransferred);
        PrintDescription("Upload File Success");
    }

    private void FailureListener(HMSException exception)
    {
        Debug.LogError($"{TAG} FailureListener:" + exception);
        PrintDescription("Failed:" + exception);
    }

    private void PrintDescription(string text)
    {
        Text desc = GameObject.Find("Description").GetComponent<Text>();
        desc.text = text;
    }

    private void OnGetImageByteArrayFailure(HMSException exception)
    {
        Debug.LogError($"{TAG} OnGetImageByteArrayFailure:" + exception);
    }

    private void OnGetImageByteArray(byte[] imageByteArray)
    {
        Debug.Log($"{TAG} OnGetImageByteArray Success , image byte array length " + imageByteArray.Length);
    }

}


