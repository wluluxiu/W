using System;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Text;
using AOT;

public class IndexedDBManager : MonoBehaviour {
    
    [DllImport("__Internal")]
    private static extern void SyncFileToIndexedDB();
    
    [DllImport("__Internal")]
    private static extern void SyncFileFromIndexedDB( CallbackDelegate callback);
    
    [DllImport("__Internal")]
    private static extern IntPtr JSReadFile(string path);
    
    [DllImport("__Internal")]
    private static extern void JSWriteFile(string path, byte[] data, int length);
    
    [DllImport("__Internal")]
    private static extern void JSCreateFile(string path, string name, byte[] dataPtr, int dataLength);
    
    [DllImport("__Internal")]
    private static extern void JSCreateDirectory(string path);
    
    [DllImport("__Internal")]
    private static extern void JSRemoveFile(string path);
    
    [DllImport("__Internal")]
    private static extern void JSRemoveDirectory(string path);
    
    [DllImport("__Internal")]
    private static extern void JSMoveFile(string oldPath, string newPath);
    
    [DllImport("__Internal")]
    private static extern void JSMoveFiles(string oldPath, string newPath);
    
    [DllImport("__Internal")]
    private static extern bool JSIsFileExists(string path);
    
    [DllImport("__Internal")]
    private static extern int JSGetFileLength(string path);
    [DllImport("__Internal")]
    private static extern bool JSIsDirectoryExists(string path);
    
    [DllImport("__Internal")]
    private static extern int JSOpenFileStream(string path, string mode);
    
    [DllImport("__Internal")]
    private static extern int JSCloseFileStream(int fd);
    
    [DllImport("__Internal")]
    private static extern int JSReadFileStream(int fd, int length, int offset);
    
    [DllImport("__Internal")]
    private static extern int JSWriteFileStream(int fd, byte[] data, int length, int offset);
    
    [DllImport("__Internal")]
    private static extern void JSFreeAllocatedMemory(int buffer);


    public class FileOpenFlag
    {
        public const string Read = "r";
        public const string ReadWrite = "r+";
        public const string Write = "w";
        public const string WriteExistFail = "wx";
        public const string ReadWriteEx = "w+";
        public const string ReadWriteExistFail = "wx+";
        public const string Append = "a";
        public const string AppendExistFail = "ax";
        public const string ReadAppendEx = "a+";
        public const string ReadAppendExistFail = "ax+";
    }
    

    public delegate void CallbackDelegate(bool error);
    public static CallbackDelegate _syncIndexedDB;

    public void SyncFromIndexedDB(CallbackDelegate callback = null)
    {
        _syncIndexedDB = callback;
        SyncFileFromIndexedDB(OnFileSystemSynced);
    }
    public void SyncToIndexedDB(CallbackDelegate callback = null) {
        _syncIndexedDB = callback;
        SyncFileToIndexedDB();
    }
    
    [MonoPInvokeCallback(typeof(Action<bool>))]
    private static void OnFileSystemSynced(bool error)
    {
        if (error)
        {
            Debug.LogError("FileSystem synchronisation error.");
        }
        else
        {
            if (_syncIndexedDB != null)
            {
                _syncIndexedDB(error);
            }
            Debug.Log("FileSystem synchronised successfully.");
        }
    }
    
    public void WriteToFile(string filePath, byte[] data) {
        JSWriteFile(filePath, data, data.Length);
    }

    public bool IsExistsFile(string path)
    {
        return JSIsFileExists(path);
    }
    
    public bool IsExistsDirectory(string path)
    {
        return JSIsDirectoryExists(path);
    }
    
    public void CreateFile(string path, string name, byte[] dataPtr, int dataLength)
    {
        JSCreateFile(path, name, dataPtr, dataLength);
    }
    
    
    public void CreateDirectory(string path)
    {
        JSCreateDirectory(path);
    }
    
    public void RemoveFile(string path)
    {
        JSRemoveFile(path);
    }
    
    public void RemoveDirectory(string path)
    {
        JSRemoveDirectory(path);
    }
    
    public void PathRename(string oldPath, string newPath)
    {
        JSMoveFile(oldPath, newPath);
    }

    public void MoveDirFiles(string oldPath, string newPath)
    {
        JSMoveFiles(oldPath, newPath);
    }
    
    public byte[] ReadFromFile(string filePath) {
        IntPtr dataPtr = JSReadFile(filePath);
        int fileLength = GetLengthOfFile(filePath);

        byte[] data = new byte[fileLength];
        Marshal.Copy(dataPtr, data, 0, fileLength);
        Marshal.FreeHGlobal(dataPtr);

        return data;
    }
    
    public int OpenStream(string path, string flag)
    {
        int fd = JSOpenFileStream(path, flag);
        if (fd == 0)
        {
            Debug.LogError("Failed to open file.");
        }

        return fd;
    }
    
    public int CloseStream(int fileDescriptor)
    {
        int closeResult = JSCloseFileStream(fileDescriptor);
        if (closeResult == 0)
        {
            Debug.LogError("Failed to close file.");
        }

        return closeResult;
    }
    
    public byte[] ReadStream(int fileDescriptor, int dataLength)
    {
        // Call the JavaScript function that reads the data
        int bufferPtr = JSReadFileStream(fileDescriptor, dataLength, 0);
        
        if (bufferPtr != 0)
        {
            // Create a managed byte array to hold the data
            byte[] data = new byte[dataLength];
            // Copy the data from the unmanaged memory
            Marshal.Copy((IntPtr)bufferPtr, data, 0, dataLength);
            // Free the unmanaged memory allocated in the JS function
            JSFreeAllocatedMemory(bufferPtr);
            // Return the data
            return data;
        }
        else
        {
            // Handle the error accordingly
            Debug.LogError("Error reading data from file descriptor.");
            return null;
        }
    }
    
    public bool WriteStream(int fileDescriptor, byte[] data, int offset)
    {
        // Ensure data is not null and fileDescriptor is valid
        if (data != null && fileDescriptor >= 0)
        {
            // Call the JavaScript method to write the data
            int bytesWritten = JSWriteFileStream(fileDescriptor, data, data.Length, offset);

            // Check if the write operation was successful
            if (bytesWritten == data.Length)
            {
                Debug.Log("Data written successfully.");
                return true;
            }
            else
            {
                Debug.LogError("Failed to write all data.");
            }
        }
        else
        {
            Debug.LogError("Data is null or fileDescriptor is invalid.");
        }
        return false;
    }
    
    public int GetLengthOfFile(string filePath) {
        return JSGetFileLength(filePath);
    }
    
}