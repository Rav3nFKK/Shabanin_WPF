using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace drSr
{
    public class DLLTry
    {
        public static double drsr(DateTime[] mas)
        {
            int i = 0;
            double sum = 0;


            foreach (DateTime c in mas)
            {
                sum += ((DateTime.Now - c).TotalDays) / 365;
            }

            sum = Math.Round(sum / mas.Length, 2);
            return sum;
        }

        public static List<string> serchname (List<string> name, string cname)
        {
            name = name.Where(x => x.Contains(cname)).ToList();
            return name;
        }
    }
}
