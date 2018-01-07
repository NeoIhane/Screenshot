using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class PNGSplit : MonoBehaviour 
{
    public static List<byte[]> Split(byte[] png, int size)
    {
        List<byte[]> sps = new List<byte[]>();
        int num = png.Length / size;

        int lastsize = png.Length - num * size;
        Debug.Log("num "+num);
        for (int i = 0; i < num; i++)
        {
            byte[] sp = new byte[size];
            System.Array.Copy(png, i * size, sp, 0, size);
            sps.Add(sp);
        }
        if(lastsize!=0)
        {
            byte[] sp = new byte[size];
            System.Array.Copy(png, sps.Count * size, sp, 0, lastsize);
            sps.Add(sp);
        }

        return sps;
    }
    public static byte[] Together(List<byte[]> list, int size)
    {
        int max = (list.Count - 1) * size + list[list.Count - 1].Length;
        Debug.Log("max " + max);
        byte[] b = new byte[max];
        Debug.Log(b.Length);
        for (int i = 0; i < list.Count; i++)
        {

            System.Array.Copy(list[i], 0, b, i * size, list[i].Length);

        }
        return b;
    }


}
