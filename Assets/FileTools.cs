using System.Collections;
using System.IO;
using System.Text;

/// <summary>
/// IO读写类
/// </summary>
public class FileTools
{
    /// <summary>
    /// 创建目录文件夹 有就不创建
    /// </summary>
    public static void CreateDirectoryOrGet(string filePath)
    {
        if (!string.IsNullOrEmpty(filePath))
        {
            if (!File.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
        }
    }
    /// <summary>
    /// 创建文件
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="bytes"></param>
    public static void CreateFileBytes(string filePath, byte[] bytes)
    {
        using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            fs.Write(bytes, 0, bytes.Length);
        }
    }

    public static void CreateFileString(string filePath, string str)
    {
        using (StreamWriter sw = new StreamWriter(filePath, true))
        {
            sw.Write(str);
        }
    }
    /// <summary>
    /// 读取文件
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static byte[] ReadFileByte(string filePath)
    {
        using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            byte[] infbytes = new byte[fs.Length];
            fs.Read(infbytes, 0, infbytes.Length);
            return infbytes;
        }
    }

    public static string ReadFileString(string filePath)
    {
        //即使中间Return也会调用Dispose方法
        using (StreamReader sr = new StreamReader(filePath))
        {
            return sr.ReadToEnd();
        }
    }

    /// <summary>
    /// string转bytes
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static byte[] StrToBytes(string str)
    {
        return Encoding.Default.GetBytes(str);
    }

    /// <summary>
    /// bytes转string
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string BytesToStr(byte[] bytes)
    {
        return Encoding.Default.GetString(bytes, 0, bytes.Length);
    }
}