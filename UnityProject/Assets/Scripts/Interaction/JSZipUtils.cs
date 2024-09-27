using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using System.IO;

public class JSZipUtils : MonoBehaviour
{

    [DllImport("__Internal")]
    private static extern void Compress(int data, int length);
    
    [DllImport("__Internal")]
    private static extern void Decompress(int data, int length);

    private byte[] _dataToCompress;
    private byte[] _dataToDecompress;

    public void CompressData(byte[] data)
    {
        _dataToCompress = data;
        var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
        try
        {
            Compress(handle.AddrOfPinnedObject().ToInt32(), data.Length);
        }
        finally
        {
            handle.Free();
        }
    }


    public void DecompressData(byte[] data)
    {
        _dataToDecompress = data;
        var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
        try
        {
            Decompress(handle.AddrOfPinnedObject().ToInt32(), data.Length);
        }
        finally
        {
            handle.Free();
        }
    }


    public void GetCompressedData(byte[] compressedData)
    {

    }
    

    public void GetDecompressedData(byte[] decompressedData)
    {

    }
}