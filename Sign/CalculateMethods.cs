using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sign
{
    class CalculateMethods
    {
        //проверка на то является ли число простым
        public bool IsPrime(long n)
        {
            if (n < 2)
                return false;
            if (n == 2)
                return true;
            for (long i = 2; i <= Math.Sqrt(n); i++)
                if (n % i == 0)
                    return false;
            return true;
        }
        //первый компонент d из закрытого ключа{d,n}
        public long Calculate_d(long fe, long e)
        {
            long d = 0;
            while (true)
            {
                if ((e * d) % fe == 1)
                {
                    if (e == d)
                        d += fe;
                    break;
                }
                else
                    d++;
            }
            return d;
        }
        //первый компонент e из открытого ключа{e,n}
        public long Calculate_e(long fe)
        {
            long e = 2;
            while (!(e < fe && (NOD(e, fe) == 1) && IsPrime(e)))
                e++;
            return e;
        }
        //вычисление НОД
        public long NOD(long a, long b)
        {
            if (b > 0)
                return NOD(b, a % b);
            else
                return a;
        }
    }
}
