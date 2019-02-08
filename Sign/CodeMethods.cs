using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Sign
{
    class CodeMethods
    {
        char[] characters = new char[] { '#', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '-' };//массив для шифрования
        //кодирование хэша исзодного кода при помощи RSA(Возводите наше сообщение в степень e по модулю n)
        public List<string> RSA_Encode(string s, long e, long n)
        {
            List<string> result = new List<string>();
            BigInteger bi;//имеет большой диапазон 
            for (int i = 0; i < s.Length; i++)
            {
                int index = Array.IndexOf(characters, s[i]);//берем по символу из хэш кода и кодируем его
                bi = new BigInteger(index);//приводим тип
                bi = BigInteger.Pow(bi, (int)e);
                BigInteger n_ = new BigInteger((int)n);
                bi = bi % n_;//по модулю n
                result.Add(bi.ToString());//результат добавляем в список и приводим тип
            }
            return result;
        }
        //расшифровка
        public string RSA_Decode(List<string> input, long d, long n)
        {
            string result = "";
            BigInteger bi;
            foreach (string item in input)
            {
                bi = new BigInteger(Convert.ToInt64(item));
                bi = BigInteger.Pow(bi, (int)d);
                BigInteger n_ = new BigInteger((int)n);
                bi = bi % n_;
                int index = Convert.ToInt32(bi.ToString());
                result += characters[index].ToString();
            }
            return result;
        }
    }
}
