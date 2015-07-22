using UnityEngine;
using System.Collections;

public class Utility {

    public  static string ToCurrency(int price) {
        return ToCurrency((float)price);
    }

    public  static string ToCurrency(double price) {
        return ToCurrency((float)price);
    }

    public  static string ToCurrency(float price) {
        return "$" + ToNumber(price);
    }

    public  static string ToNumber(int num) {
        return ToNumber((float)num);
    }

    public  static string ToNumber(double num) {
        return ToNumber((float)num);
    }

    public  static string ToNumber(float num) {
        string n = num.ToString();
        int offset = 0;
        int index = 0;
        int indexEnd = n.IndexOf(".");
        if (indexEnd < 1) {
            indexEnd = n.Length;
        }
        int len = indexEnd % 3;
        string result = "";
        while (offset + len <= indexEnd) {
            result += (result.Length > 0) ? ",":"";
            result += n.Substring(offset, len);
            offset += len;
            len = 3;
            index++;
        }
        if (indexEnd != n.Length) {
            result += n.Substring(indexEnd);
        }
        return result;
    }
}
