using System;
using System.Collections.Generic;
using System.Text;

namespace OneMFS.SharedResources.CommonService
{
    public class NumericWordConversion
    {
        public string InWords(Decimal amount)
        {
            string str = "";
            string amt = amount.ToString();
            int num = amount.ToString().IndexOf(".", 0);
            string amt_paisa = amount.ToString().Substring(num + 1);
            if (amt == amt_paisa)
                amt_paisa = "";
            else
                amt = amount.ToString().Substring(0, amount.ToString().IndexOf(".", 0)).Replace(",", "").ToString();
            switch (amt.Length)
            {
                case 1:
                    str = NumericWordConversion.F_Number("0" + amt, amt_paisa);
                    break;
                case 2:
                    str = NumericWordConversion.F_Number(amt, amt_paisa);
                    break;
                case 3:
                    str = NumericWordConversion.F_Hundred(amt, amt_paisa);
                    break;
                case 4:
                    str = NumericWordConversion.F_Thousand(amt, amt_paisa);
                    break;
                case 5:
                    str = NumericWordConversion.F_Thousands(amt, amt_paisa);
                    break;
                case 6:
                    str = NumericWordConversion.F_Lakh(amt, amt_paisa);
                    break;
                case 7:
                    str = NumericWordConversion.F_Lakhs(amt, amt_paisa);
                    break;
                case 8:
                    str = NumericWordConversion.F_Crore(amt, amt_paisa);
                    break;
                case 9:
                    str = NumericWordConversion.F_Crores(amt, amt_paisa);
                    break;
            }
            return str;
        }

        public static string Tens(string s_amt)
        {
            string str;
            switch (s_amt)
            {
                case "0":
                    str = "";
                    break;
                case "1":
                    str = "One";
                    break;
                case "2":
                    str = "Two";
                    break;
                case "3":
                    str = "Three";
                    break;
                case "4":
                    str = "Four";
                    break;
                case "5":
                    str = "Five";
                    break;
                case "6":
                    str = "Six";
                    break;
                case "7":
                    str = "Seven";
                    break;
                case "8":
                    str = "Eight";
                    break;
                case "9":
                    str = "Nine";
                    break;
                case "10":
                    str = "Ten";
                    break;
                case "11":
                    str = "Eleven";
                    break;
                case "12":
                    str = "Twelve";
                    break;
                case "13":
                    str = "Thirteen";
                    break;
                case "14":
                    str = "Forteen";
                    break;
                case "15":
                    str = "Fifteen";
                    break;
                case "16":
                    str = "Sixteen";
                    break;
                case "17":
                    str = "Seventeen";
                    break;
                case "18":
                    str = "Eighteen";
                    break;
                case "19":
                    str = "Nineteen";
                    break;
                case "20":
                    str = "Twenty";
                    break;
                case "30":
                    str = "Thirty";
                    break;
                case "40":
                    str = "Forty";
                    break;
                case "50":
                    str = "Fifty";
                    break;
                case "60":
                    str = "Sixty";
                    break;
                case "70":
                    str = "Seventy";
                    break;
                case "80":
                    str = "Eighty";
                    break;
                case "90":
                    str = "Ninety";
                    break;
                default:
                    str = "Nothing";
                    break;
            }
            return str;
        }

        public static string Word_Spell_Tens(string amt)
        {
            int int32 = Convert.ToInt32(amt.Substring(0, 2));
            return int32 <= 20 ? NumericWordConversion.Tens(int32.ToString()) : NumericWordConversion.Tens(amt.Substring(0, 1) + "0") + " " + NumericWordConversion.Tens(amt.Substring(1, 1));
        }

        public static string F_Crores(string amt, string amt_paisa)
        {
            string str1 = "";
            string str2 = "";
            string str3 = "";
            string str4 = "";
            string str5 = "";
            string str6 = "";
            if (amt_paisa == "")
            {
                str1 = NumericWordConversion.Word_Spell_Tens(Convert.ToInt32(amt.Substring(0, 2)).ToString()) + " Crores";
                if (amt.Substring(2, 7) != "0000000")
                {
                    if (amt.Substring(2, 2) != "00")
                    {
                        if (amt.Substring(2, 1) != "0")
                        {
                            int int32 = Convert.ToInt32(amt.Substring(2, 2));
                            str2 = !(amt.Substring(4, 5) == "00000") ? " " + NumericWordConversion.Word_Spell_Tens(int32.ToString()) + " Lakhs" : " and " + NumericWordConversion.Word_Spell_Tens(int32.ToString()) + " Lakhs";
                        }
                        else
                        {
                            int int32 = Convert.ToInt32(amt.Substring(3, 1));
                            string str7 = !(amt.Substring(4, 5) == "00000") ? " " + NumericWordConversion.Tens(int32.ToString()) : " and " + NumericWordConversion.Tens(int32.ToString());
                            str2 = int32 <= 1 ? str7 + " Lakh" : str7 + " Lakhs";
                        }
                    }
                    if (amt.Substring(4, 2) != "00")
                    {
                        if (amt.Substring(4, 1) != "0")
                        {
                            int int32 = Convert.ToInt32(amt.Substring(4, 2));
                            str3 = !(amt.Substring(6, 3) == "000") ? " " + NumericWordConversion.Word_Spell_Tens(int32.ToString()) + " Thousands" : " and " + NumericWordConversion.Word_Spell_Tens(int32.ToString()) + " Thousands";
                        }
                        else
                        {
                            int int32 = Convert.ToInt32(amt.Substring(5, 1));
                            string str7 = !(amt.Substring(4, 5) == "000") ? " " + NumericWordConversion.Tens(int32.ToString()) : " and " + NumericWordConversion.Tens(int32.ToString());
                            str3 = int32 <= 1 ? str7 + " Thousand" : str7 + " Thousands";
                        }
                    }
                    if (amt.Substring(6, 3) != "000")
                    {
                        if (amt.Substring(6, 1) != "0")
                        {
                            int int32 = Convert.ToInt32(amt.Substring(6, 1));
                            str4 = int32 <= 1 ? (!(amt.Substring(7, 2) == "00") ? " " + NumericWordConversion.Tens(int32.ToString()) + " Hundred" : " and" + NumericWordConversion.Tens(int32.ToString()) + " Hundred") : (!(amt.Substring(7, 2) == "00") ? " " + NumericWordConversion.Tens(int32.ToString()) + " Hundreds" : " and" + NumericWordConversion.Tens(int32.ToString()) + " Hundreds");
                        }
                        if (amt.Substring(7, 2) != "00")
                        {
                            int int32 = Convert.ToInt32(amt.Substring(7, 2));
                            str5 = Convert.ToInt32(amt.Substring(7, 1)) == 0 ? " and " + NumericWordConversion.Tens(int32.ToString()) : " and " + NumericWordConversion.Word_Spell_Tens(int32.ToString());
                        }
                    }
                }
            }
            else if (amt_paisa != "")
            {
                str1 = NumericWordConversion.Word_Spell_Tens(Convert.ToInt32(amt.Substring(0, 2)).ToString()) + " Crores";
                if (amt.Substring(2, 7) != "0000000")
                {
                    if (amt.Substring(2, 2) != "00")
                    {
                        if (amt.Substring(2, 1) != "0")
                        {
                            str2 = " " + NumericWordConversion.Word_Spell_Tens(Convert.ToInt32(amt.Substring(2, 2)).ToString()) + " Lakhs";
                        }
                        else
                        {
                            int int32 = Convert.ToInt32(amt.Substring(3, 1));
                            str2 = int32 <= 1 ? " " + NumericWordConversion.Tens(int32.ToString()) + " Lakh" : " " + NumericWordConversion.Tens(int32.ToString()) + " Lakhs";
                        }
                    }
                    if (amt.Substring(4, 2) != "00")
                    {
                        if (amt.Substring(4, 1) != "0")
                        {
                            str3 = " " + NumericWordConversion.Word_Spell_Tens(Convert.ToInt32(amt.Substring(4, 2)).ToString()) + " Thousands";
                        }
                        else
                        {
                            int int32 = Convert.ToInt32(amt.Substring(5, 1));
                            str3 = int32 <= 1 ? " " + NumericWordConversion.Tens(int32.ToString()) + " Thousand" : " " + NumericWordConversion.Tens(int32.ToString()) + " Thousands";
                        }
                    }
                    if (amt.Substring(6, 3) != "000")
                    {
                        if (amt.Substring(6, 1) != "0")
                        {
                            int int32 = Convert.ToInt32(amt.Substring(6, 1));
                            str4 = int32 <= 1 ? " " + NumericWordConversion.Tens(int32.ToString()) + " Hundred" : " " + NumericWordConversion.Tens(int32.ToString()) + " Hundreds";
                        }
                        if (amt.Substring(7, 2) != "00")
                        {
                            int int32 = Convert.ToInt32(amt.Substring(7, 2));
                            str5 = !(amt.Substring(7, 1) != "0") ? " " + NumericWordConversion.Tens(int32.ToString()) : " " + NumericWordConversion.Word_Spell_Tens(int32.ToString());
                        }
                    }
                }
                if (amt_paisa.Substring(0, 2) != "00")
                    str6 = !(amt_paisa.Substring(0, 1) != "0") ? " " + NumericWordConversion.Tens(amt_paisa.Substring(0, 2)) + " Paisa" : " and " + NumericWordConversion.Word_Spell_Tens(amt_paisa.Substring(0, 2)) + " Paisa";
            }
            return "Taka " + str1 + str2 + str3 + str4 + str5 + str6 + " Only";
        }

        public static string F_Crore(string amt, string amt_paisa)
        {
            string str1 = "";
            string str2 = "";
            string str3 = "";
            string str4 = "";
            string str5 = "";
            string str6 = "";
            if (amt_paisa == "")
            {
                int int32_1 = Convert.ToInt32(amt.Substring(0, 1));
                str1 = int32_1 <= 1 ? NumericWordConversion.Tens(int32_1.ToString()) + " Crore" : NumericWordConversion.Tens(int32_1.ToString()) + " Crores";
                if (amt.Substring(1, 7) != "0000000")
                {
                    if (amt.Substring(1, 2) != "00")
                    {
                        if (amt.Substring(1, 1) != "0")
                        {
                            int int32_2 = Convert.ToInt32(amt.Substring(1, 2));
                            str2 = !(amt.Substring(3, 5) == "00000") ? " " + NumericWordConversion.Word_Spell_Tens(int32_2.ToString()) + " Lakhs" : " and " + NumericWordConversion.Word_Spell_Tens(int32_2.ToString()) + " Lakhs";
                        }
                        else
                        {
                            int int32_2 = Convert.ToInt32(amt.Substring(2, 1));
                            string str7 = !(amt.Substring(3, 5) == "00000") ? " " + NumericWordConversion.Tens(int32_2.ToString()) : " and " + NumericWordConversion.Tens(int32_2.ToString());
                            str2 = int32_2 <= 1 ? str7 + " Lakh" : str7 + " Lakhs";
                        }
                    }
                    if (amt.Substring(3, 2) != "00")
                    {
                        if (amt.Substring(3, 1) != "0")
                        {
                            int int32_2 = Convert.ToInt32(amt.Substring(3, 2));
                            str3 = !(amt.Substring(5, 3) == "000") ? " " + NumericWordConversion.Word_Spell_Tens(int32_2.ToString()) + " Thousands" : " and " + NumericWordConversion.Word_Spell_Tens(int32_2.ToString()) + " Thousands";
                        }
                        else
                        {
                            int int32_2 = Convert.ToInt32(amt.Substring(4, 1));
                            string str7 = !(amt.Substring(5, 3) == "000") ? " " + NumericWordConversion.Tens(int32_2.ToString()) : " and " + NumericWordConversion.Tens(int32_2.ToString());
                            str3 = int32_2 <= 1 ? str7 + " Thousand" : str7 + " Thousands";
                        }
                    }
                    if (amt.Substring(5, 3) != "000")
                    {
                        if (amt.Substring(5, 1) != "0")
                        {
                            int int32_2 = Convert.ToInt32(amt.Substring(5, 1));
                            str4 = int32_2 <= 1 ? (!(amt.Substring(6, 2) == "00") ? " " + NumericWordConversion.Tens(int32_2.ToString()) + " Hundred" : " and" + NumericWordConversion.Tens(int32_2.ToString()) + " Hundred") : (!(amt.Substring(6, 2) == "00") ? " " + NumericWordConversion.Tens(int32_2.ToString()) + " Hundreds" : " and" + NumericWordConversion.Tens(int32_2.ToString()) + " Hundreds");
                        }
                        if (amt.Substring(6, 2) != "00")
                        {
                            int int32_2 = Convert.ToInt32(amt.Substring(6, 2));
                            str5 = Convert.ToInt32(amt.Substring(6, 1)) == 0 ? " and " + NumericWordConversion.Tens(int32_2.ToString()) : " and " + NumericWordConversion.Word_Spell_Tens(int32_2.ToString());
                        }
                    }
                }
            }
            else if (amt_paisa != "")
            {
                int int32_1 = Convert.ToInt32(amt.Substring(0, 1));
                str1 = int32_1 <= 1 ? NumericWordConversion.Tens(int32_1.ToString()) + " Crore" : NumericWordConversion.Tens(int32_1.ToString()) + " Crores";
                if (amt.Substring(1, 7) != "0000000")
                {
                    if (amt.Substring(1, 2) != "00")
                    {
                        if (amt.Substring(1, 1) != "0")
                        {
                            str2 = " " + NumericWordConversion.Word_Spell_Tens(Convert.ToInt32(amt.Substring(1, 2)).ToString()) + " Lakhs";
                        }
                        else
                        {
                            int int32_2 = Convert.ToInt32(amt.Substring(2, 1));
                            str2 = int32_2 <= 1 ? " " + NumericWordConversion.Tens(int32_2.ToString()) + " Lakh" : " " + NumericWordConversion.Tens(int32_2.ToString()) + " Lakhs";
                        }
                    }
                    if (amt.Substring(3, 2) != "00")
                    {
                        if (amt.Substring(3, 1) != "0")
                        {
                            str3 = " " + NumericWordConversion.Word_Spell_Tens(Convert.ToInt32(amt.Substring(3, 2)).ToString()) + " Thousands";
                        }
                        else
                        {
                            int int32_2 = Convert.ToInt32(amt.Substring(4, 1));
                            str3 = int32_2 <= 1 ? " " + NumericWordConversion.Tens(int32_2.ToString()) + " Thousand" : " " + NumericWordConversion.Tens(int32_2.ToString()) + " Thousands";
                        }
                    }
                    if (amt.Substring(5, 3) != "000")
                    {
                        if (amt.Substring(5, 1) != "0")
                        {
                            int int32_2 = Convert.ToInt32(amt.Substring(5, 1));
                            str4 = int32_2 <= 1 ? " " + NumericWordConversion.Tens(int32_2.ToString()) + " Hundred" : " " + NumericWordConversion.Tens(int32_2.ToString()) + " Hundreds";
                        }
                        if (amt.Substring(6, 2) != "00")
                        {
                            int int32_2 = Convert.ToInt32(amt.Substring(6, 2));
                            str5 = !(amt.Substring(6, 1) != "0") ? " " + NumericWordConversion.Tens(int32_2.ToString()) : " " + NumericWordConversion.Word_Spell_Tens(int32_2.ToString());
                        }
                    }
                }
                if (amt_paisa.Substring(0, 2) != "00")
                    str6 = !(amt_paisa.Substring(0, 1) != "0") ? " " + NumericWordConversion.Tens(amt_paisa.Substring(0, 2)) + " Paisa" : " and " + NumericWordConversion.Word_Spell_Tens(amt_paisa.Substring(0, 2)) + " Paisa";
            }
            return "Taka " + str1 + str2 + str3 + str4 + str5 + str6 + " Only";
        }

        public static string F_Lakhs(string amt, string amt_paisa)
        {
            string str1 = "";
            string str2 = "";
            string str3 = "";
            string str4 = "";
            string str5 = "";
            if (amt_paisa == "")
            {
                if (amt.Substring(0, 2) != "00" && amt.Substring(0, 1) != "0")
                    str1 = NumericWordConversion.Word_Spell_Tens(Convert.ToInt32(amt.Substring(0, 2)).ToString()) + " Lakhs";
                if (amt.Substring(2, 2) != "00")
                {
                    if (amt.Substring(2, 1) != "0")
                    {
                        int int32 = Convert.ToInt32(amt.Substring(2, 2));
                        str2 = !(amt.Substring(4, 3) == "000") ? " " + NumericWordConversion.Word_Spell_Tens(int32.ToString()) + " Thousands" : " and " + NumericWordConversion.Word_Spell_Tens(int32.ToString()) + " Thousands";
                    }
                    else
                    {
                        int int32 = Convert.ToInt32(amt.Substring(3, 1));
                        string str6 = !(amt.Substring(4, 3) == "000") ? " " + NumericWordConversion.Tens(int32.ToString()) : " and " + NumericWordConversion.Tens(int32.ToString());
                        str2 = int32 <= 1 ? str6 + " Thousand" : str6 + " Thousands";
                    }
                }
                if (amt.Substring(4, 3) != "000")
                {
                    if (amt.Substring(4, 1) != "0")
                    {
                        int int32 = Convert.ToInt32(amt.Substring(4, 1));
                        str3 = int32 <= 1 ? (!(amt.Substring(5, 2) == "00") ? " " + NumericWordConversion.Tens(int32.ToString()) + " Hundred" : " and" + NumericWordConversion.Tens(int32.ToString()) + " Hundred") : (!(amt.Substring(5, 2) == "00") ? " " + NumericWordConversion.Tens(int32.ToString()) + " Hundreds" : " and" + NumericWordConversion.Tens(int32.ToString()) + " Hundreds");
                    }
                    if (amt.Substring(5, 2) != "00")
                    {
                        int int32 = Convert.ToInt32(amt.Substring(5, 2));
                        str4 = Convert.ToInt32(amt.Substring(5, 1)) == 0 ? " and " + NumericWordConversion.Tens(int32.ToString()) : " and " + NumericWordConversion.Word_Spell_Tens(int32.ToString());
                    }
                }
            }
            else if (amt_paisa != "")
            {
                if (amt.Substring(0, 2) != "00" && amt.Substring(0, 1) != "0")
                    str1 = " " + NumericWordConversion.Word_Spell_Tens(Convert.ToInt32(amt.Substring(0, 2)).ToString()) + " Lakhs";
                if (amt.Substring(2, 2) != "00")
                {
                    if (amt.Substring(2, 1) != "0")
                    {
                        str2 = " " + NumericWordConversion.Word_Spell_Tens(Convert.ToInt32(amt.Substring(2, 2)).ToString()) + " Thousands";
                    }
                    else
                    {
                        int int32 = Convert.ToInt32(amt.Substring(3, 1));
                        str2 = int32 <= 1 ? " " + NumericWordConversion.Tens(int32.ToString()) + " Thousand" : " " + NumericWordConversion.Tens(int32.ToString()) + " Thousands";
                    }
                }
                if (amt.Substring(4, 3) != "000")
                {
                    if (amt.Substring(4, 1) != "0")
                    {
                        int int32 = Convert.ToInt32(amt.Substring(4, 1));
                        str3 = int32 <= 1 ? " " + NumericWordConversion.Tens(int32.ToString()) + " Hundred" : " " + NumericWordConversion.Tens(int32.ToString()) + " Hundreds";
                    }
                    if (amt.Substring(5, 2) != "00")
                    {
                        int int32 = Convert.ToInt32(amt.Substring(5, 2));
                        str4 = !(amt.Substring(5, 1) != "0") ? " " + NumericWordConversion.Tens(int32.ToString()) : " " + NumericWordConversion.Word_Spell_Tens(int32.ToString());
                    }
                }
                if (amt_paisa.Substring(0, 2) != "00")
                    str5 = !(amt_paisa.Substring(0, 1) != "0") ? " " + NumericWordConversion.Tens(amt_paisa.Substring(0, 2)) + " Paisa" : " and " + NumericWordConversion.Word_Spell_Tens(amt_paisa.Substring(0, 2)) + " Paisa";
            }
            return "Taka " + str1 + str2 + str3 + str4 + str5 + " Only";
        }

        public static string F_Lakh(string amt, string amt_paisa)
        {
            string str1 = "";
            string str2 = "";
            string str3 = "";
            string str4 = "";
            string str5 = "";
            if (amt_paisa == "")
            {
                if (amt.Substring(0, 1) != "0")
                {
                    int int32 = Convert.ToInt32(amt.Substring(0, 1));
                    str1 = int32 <= 1 ? NumericWordConversion.Tens(int32.ToString()) + " Lakh" : NumericWordConversion.Tens(int32.ToString()) + " Lakhs";
                }
                if (amt.Substring(1, 2) != "00")
                {
                    if (amt.Substring(1, 1) != "0")
                    {
                        int int32 = Convert.ToInt32(amt.Substring(1, 2));
                        str2 = !(amt.Substring(3, 3) == "000") ? " " + NumericWordConversion.Word_Spell_Tens(int32.ToString()) + " Thousands" : " and " + NumericWordConversion.Word_Spell_Tens(int32.ToString()) + " Thousands";
                    }
                    else
                    {
                        int int32 = Convert.ToInt32(amt.Substring(2, 1));
                        string str6 = !(amt.Substring(3, 3) == "000") ? " " + NumericWordConversion.Tens(int32.ToString()) : " and " + NumericWordConversion.Tens(int32.ToString());
                        str2 = int32 <= 1 ? str6 + " Thousand" : str6 + " Thousands";
                    }
                }
                if (amt.Substring(3, 3) != "000")
                {
                    if (amt.Substring(3, 1) != "0")
                    {
                        int int32 = Convert.ToInt32(amt.Substring(3, 1));
                        str3 = int32 <= 1 ? (!(amt.Substring(4, 2) == "00") ? " " + NumericWordConversion.Tens(int32.ToString()) + " Hundred" : " and" + NumericWordConversion.Tens(int32.ToString()) + " Hundred") : (!(amt.Substring(4, 2) == "00") ? " " + NumericWordConversion.Tens(int32.ToString()) + " Hundreds" : " and" + NumericWordConversion.Tens(int32.ToString()) + " Hundreds");
                    }
                    if (amt.Substring(4, 2) != "00")
                    {
                        int int32 = Convert.ToInt32(amt.Substring(4, 2));
                        str4 = Convert.ToInt32(amt.Substring(4, 1)) == 0 ? " and " + NumericWordConversion.Tens(int32.ToString()) : " and " + NumericWordConversion.Word_Spell_Tens(int32.ToString());
                    }
                }
            }
            else if (amt_paisa != "")
            {
                if (amt.Substring(0, 1) != "0")
                {
                    int int32 = Convert.ToInt32(amt.Substring(0, 1));
                    str1 = int32 <= 1 ? NumericWordConversion.Tens(int32.ToString()) + " Lakh" : NumericWordConversion.Tens(int32.ToString()) + " Lakhs";
                }
                if (amt.Substring(1, 2) != "00")
                {
                    if (amt.Substring(1, 1) != "0")
                    {
                        str2 = " " + NumericWordConversion.Word_Spell_Tens(Convert.ToInt32(amt.Substring(1, 2)).ToString()) + " Thousands";
                    }
                    else
                    {
                        int int32 = Convert.ToInt32(amt.Substring(2, 1));
                        str2 = int32 <= 1 ? " " + NumericWordConversion.Tens(int32.ToString()) + " Thousand" : " " + NumericWordConversion.Tens(int32.ToString()) + " Thousands";
                    }
                }
                if (amt.Substring(3, 3) != "000")
                {
                    if (amt.Substring(3, 1) != "0")
                    {
                        int int32 = Convert.ToInt32(amt.Substring(3, 1));
                        str3 = int32 <= 1 ? " " + NumericWordConversion.Tens(int32.ToString()) + " Hundred" : " " + NumericWordConversion.Tens(int32.ToString()) + " Hundreds";
                    }
                    if (amt.Substring(4, 2) != "00")
                    {
                        int int32 = Convert.ToInt32(amt.Substring(4, 2));
                        str4 = !(amt.Substring(4, 1) != "0") ? " " + NumericWordConversion.Tens(int32.ToString()) : " " + NumericWordConversion.Word_Spell_Tens(int32.ToString());
                    }
                }
                if (amt_paisa.Substring(0, 2) != "00")
                    str5 = !(amt_paisa.Substring(0, 1) != "0") ? " " + NumericWordConversion.Tens(amt_paisa.Substring(0, 2)) + " Paisa" : " and " + NumericWordConversion.Word_Spell_Tens(amt_paisa.Substring(0, 2)) + " Paisa";
            }
            return "Taka " + str1 + str2 + str3 + str4 + str5 + " Only";
        }

        public static string F_Thousands(string amt, string amt_paisa)
        {
            string str1 = "";
            string str2 = "";
            string str3 = "";
            string str4 = "";
            if (amt_paisa == "")
            {
                if (amt.Substring(0, 1) != "0")
                    str1 = NumericWordConversion.Word_Spell_Tens(Convert.ToInt32(amt.Substring(0, 2)).ToString()) + " Thousands";
                if (amt.Substring(2, 3) != "000")
                {
                    if (amt.Substring(2, 1) != "0")
                    {
                        int int32 = Convert.ToInt32(amt.Substring(2, 1));
                        str2 = int32 <= 1 ? (!(amt.Substring(3, 2) == "00") ? " " + NumericWordConversion.Tens(int32.ToString()) + " Hundred" : " and" + NumericWordConversion.Tens(int32.ToString()) + " Hundred") : (!(amt.Substring(3, 2) == "00") ? " " + NumericWordConversion.Tens(int32.ToString()) + " Hundreds" : " and" + NumericWordConversion.Tens(int32.ToString()) + " Hundreds");
                    }
                    if (amt.Substring(3, 2) != "00")
                    {
                        int int32 = Convert.ToInt32(amt.Substring(3, 2));
                        str3 = Convert.ToInt32(amt.Substring(3, 1)) == 0 ? " and " + NumericWordConversion.Tens(int32.ToString()) : " and " + NumericWordConversion.Word_Spell_Tens(int32.ToString());
                    }
                }
            }
            else if (amt_paisa != "")
            {
                if (amt.Substring(0, 1) != "0")
                {
                    int int32 = Convert.ToInt32(amt.Substring(0, 2));
                    str1 = int32 <= 1 ? NumericWordConversion.Word_Spell_Tens(int32.ToString()) + " Thousand" : NumericWordConversion.Word_Spell_Tens(int32.ToString()) + " Thousands";
                }
                if (amt.Substring(2, 3) != "000")
                {
                    if (amt.Substring(2, 1) != "0")
                    {
                        int int32 = Convert.ToInt32(amt.Substring(2, 1));
                        str2 = int32 <= 1 ? " " + NumericWordConversion.Tens(int32.ToString()) + " Hundred" : " " + NumericWordConversion.Tens(int32.ToString()) + " Hundreds";
                    }
                    if (amt.Substring(3, 2) != "00")
                    {
                        int int32 = Convert.ToInt32(amt.Substring(3, 2));
                        str3 = !(amt.Substring(3, 1) != "0") ? " " + NumericWordConversion.Tens(int32.ToString()) : " " + NumericWordConversion.Word_Spell_Tens(int32.ToString());
                    }
                }
                if (amt_paisa.Substring(0, 2) != "00")
                    str4 = !(amt_paisa.Substring(0, 1) != "0") ? " " + NumericWordConversion.Tens(amt_paisa.Substring(0, 2)) + " Paisa" : " and " + NumericWordConversion.Word_Spell_Tens(amt_paisa.Substring(0, 2)) + " Paisa";
            }
            return "Taka " + str1 + str2 + str3 + str4 + " Only";
        }

        public static string F_Thousand(string amt, string amt_paisa)
        {
            string str1 = "";
            string str2 = "";
            string str3 = "";
            string str4 = "";
            if (amt_paisa == "")
            {
                if (amt.Substring(0, 1) != "0")
                {
                    int int32 = Convert.ToInt32(amt.Substring(0, 1));
                    str1 = int32 <= 1 ? NumericWordConversion.Tens(int32.ToString()) + " Thousand" : NumericWordConversion.Tens(int32.ToString()) + " Thousands";
                }
                if (amt.Substring(1, 3) != "000")
                {
                    if (amt.Substring(1, 1) != "0")
                    {
                        int int32 = Convert.ToInt32(amt.Substring(1, 1));
                        str2 = int32 <= 1 ? (!(amt.Substring(2, 2) == "00") ? " " + NumericWordConversion.Tens(int32.ToString()) + " Hundred" : " and" + NumericWordConversion.Tens(int32.ToString()) + " Hundred") : (!(amt.Substring(2, 2) == "00") ? " " + NumericWordConversion.Tens(int32.ToString()) + " Hundreds" : " and" + NumericWordConversion.Tens(int32.ToString()) + " Hundreds");
                    }
                    if (amt.Substring(2, 2) != "00")
                    {
                        int int32 = Convert.ToInt32(amt.Substring(2, 2));
                        str3 = Convert.ToInt32(amt.Substring(2, 1)) == 0 ? " and " + NumericWordConversion.Tens(int32.ToString()) : " and " + NumericWordConversion.Word_Spell_Tens(int32.ToString());
                    }
                }
            }
            else if (amt_paisa != "")
            {
                if (amt.Substring(0, 1) != "0")
                {
                    int int32 = Convert.ToInt32(amt.Substring(0, 1));
                    str1 = int32 <= 1 ? NumericWordConversion.Tens(int32.ToString()) + " Thousand" : NumericWordConversion.Tens(int32.ToString()) + " Thousands";
                }
                if (amt.Substring(1, 3) != "000")
                {
                    if (amt.Substring(1, 1) != "0")
                    {
                        int int32 = Convert.ToInt32(amt.Substring(1, 1));
                        str2 = int32 <= 1 ? " " + NumericWordConversion.Tens(int32.ToString()) + " Hundred" : " " + NumericWordConversion.Tens(int32.ToString()) + " Hundreds";
                    }
                    if (amt.Substring(2, 2) != "00")
                    {
                        int int32 = Convert.ToInt32(amt.Substring(2, 2));
                        str3 = !(amt.Substring(2, 1) != "0") ? " " + NumericWordConversion.Tens(int32.ToString()) : " " + NumericWordConversion.Word_Spell_Tens(int32.ToString());
                    }
                }
                if (amt_paisa.Substring(0, 2) != "00")
                    str4 = !(amt_paisa.Substring(0, 1) != "0") ? " " + NumericWordConversion.Tens(amt_paisa.Substring(0, 2)) + " Paisa" : " and " + NumericWordConversion.Word_Spell_Tens(amt_paisa.Substring(0, 2)) + " Paisa";
            }
            return "Taka " + str1 + str2 + str3 + str4 + " Only";
        }

        public static string F_Hundred(string amt, string amt_paisa)
        {
            string str1 = "";
            string str2 = "";
            string str3 = "";
            if (amt_paisa == "")
            {
                if (amt.Substring(0, 3) != "000")
                {
                    if (amt.Substring(0, 1) != "0")
                    {
                        int int32 = Convert.ToInt32(amt.Substring(0, 1));
                        str1 = int32 <= 1 ? NumericWordConversion.Tens(int32.ToString()) + " Hundred" : NumericWordConversion.Tens(int32.ToString()) + " Hundreds";
                    }
                    if (amt.Substring(1, 2) != "00")
                    {
                        int int32 = Convert.ToInt32(amt.Substring(1, 2));
                        str2 = Convert.ToInt32(amt.Substring(1, 1)) == 0 ? " and " + NumericWordConversion.Tens(int32.ToString()) : " and " + NumericWordConversion.Word_Spell_Tens(int32.ToString());
                    }
                }
            }
            else if (amt_paisa != "")
            {
                if (amt.Substring(0, 3) != "000")
                {
                    if (amt.Substring(0, 1) != "0")
                    {
                        int int32 = Convert.ToInt32(amt.Substring(0, 1));
                        str1 = int32 <= 1 ? NumericWordConversion.Tens(int32.ToString()) + " Hundred" : NumericWordConversion.Tens(int32.ToString()) + " Hundreds";
                    }
                    if (amt.Substring(1, 2) != "00")
                    {
                        int int32 = Convert.ToInt32(amt.Substring(1, 2));
                        str2 = !(amt.Substring(1, 1) != "0") ? " " + NumericWordConversion.Tens(int32.ToString()) : " " + NumericWordConversion.Word_Spell_Tens(int32.ToString());
                    }
                }
                if (amt_paisa.Substring(0, 2) != "00")
                    str3 = !(amt_paisa.Substring(0, 1) != "0") ? " " + NumericWordConversion.Tens(amt_paisa.Substring(0, 2)) + " Paisa" : " and " + NumericWordConversion.Word_Spell_Tens(amt_paisa.Substring(0, 2)) + " Paisa";
            }
            return "Taka " + str1 + str2 + str3 + " Only";
        }

        public static string F_Number(string amt, string amt_paisa)
        {
            string str1 = "";
            string str2 = "";
            if (amt_paisa == "")
            {
                if (amt.Substring(0, 2) != "00")
                {
                    int int32 = Convert.ToInt32(amt.Substring(0, 2));
                    str1 = Convert.ToInt32(amt.Substring(0, 1)) == 0 ? NumericWordConversion.Tens(int32.ToString()) : NumericWordConversion.Word_Spell_Tens(int32.ToString());
                }
                else
                    str1 = " Zero ";
            }
            else if (amt_paisa != "")
            {
                if (amt.Substring(0, 2) != "00")
                {
                    int int32 = Convert.ToInt32(amt.Substring(0, 2));
                    str1 = !(amt.Substring(0, 1) != "0") ? NumericWordConversion.Tens(int32.ToString()) : NumericWordConversion.Word_Spell_Tens(int32.ToString());
                }
                else
                    str1 = " Zero ";
                if (amt_paisa.Substring(0, 2) != "00")
                    str2 = !(amt_paisa.Substring(0, 1) != "0") ? " " + NumericWordConversion.Tens(amt_paisa.Substring(0, 2)) + " Paisa" : " and " + NumericWordConversion.Word_Spell_Tens(amt_paisa.Substring(0, 2)) + " Paisa";
            }
            return "Taka " + str1 + str2 + " Only";
        }

        public static string comma(Decimal amount)
        {
            string str1 = amount.ToString();
            int num = amount.ToString().IndexOf(".", 0);
            string str2 = amount.ToString().Substring(num + 1);
            if (str1 == str2)
                str2 = "";
            else
                str1 = amount.ToString().Substring(0, amount.ToString().IndexOf(".", 0)).Replace(",", "").ToString();
            string str3;
            switch (str1.Length)
            {
                case 4:
                    if (str2 == "")
                    {
                        str3 = str1.Substring(0, 1) + "," + str1.Substring(1, 3);
                        break;
                    }
                    str3 = str1.Substring(0, 1) + "," + str1.Substring(1, 3) + "." + str2;
                    break;
                case 5:
                    if (str2 == "")
                    {
                        str3 = str1.Substring(0, 2) + "," + str1.Substring(2, 3);
                        break;
                    }
                    str3 = str1.Substring(0, 2) + "," + str1.Substring(2, 3) + "." + str2;
                    break;
                case 6:
                    if (str2 == "")
                    {
                        str3 = str1.Substring(0, 1) + "," + str1.Substring(1, 2) + "," + str1.Substring(3, 3);
                        break;
                    }
                    str3 = str1.Substring(0, 1) + "," + str1.Substring(1, 2) + "," + str1.Substring(3, 3) + "." + str2;
                    break;
                case 7:
                    if (str2 == "")
                    {
                        str3 = str1.Substring(0, 2) + "," + str1.Substring(2, 2) + "," + str1.Substring(4, 3);
                        break;
                    }
                    str3 = str1.Substring(0, 2) + "," + str1.Substring(2, 2) + "," + str1.Substring(4, 3) + "." + str2;
                    break;
                case 8:
                    if (str2 == "")
                    {
                        str3 = str1.Substring(0, 1) + "," + str1.Substring(1, 2) + "," + str1.Substring(3, 2) + "," + str1.Substring(5, 3);
                        break;
                    }
                    str3 = str1.Substring(0, 1) + "," + str1.Substring(1, 2) + "," + str1.Substring(3, 2) + "," + str1.Substring(5, 3) + "." + str2;
                    break;
                case 9:
                    if (str2 == "")
                    {
                        str3 = str1.Substring(0, 2) + "," + str1.Substring(2, 2) + "," + str1.Substring(4, 2) + "," + str1.Substring(6, 3);
                        break;
                    }
                    str3 = str1.Substring(0, 2) + "," + str1.Substring(2, 2) + "," + str1.Substring(4, 2) + "," + str1.Substring(6, 3) + "." + str2;
                    break;
                default:
                    str3 = !(str2 == "") ? str1 + "." + str2 : str1;
                    break;
            }
            return str3;
        }
    }    
}
