using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encrypter
{
    public static string EncryptDecrypt(string Data)
    {
        string keyWord = "BurneX wAs HeRe - - - The 7th Workshop B)";
        string result = "";

        for (int i = 0; i < Data.Length; i++)
            result += (char)(Data[i] ^ keyWord[i % keyWord.Length]);

        return (result);
    }
}
