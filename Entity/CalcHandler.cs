using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AsyncClientServer.Entity
{
    public class CalcHandler : IHandler
    {

        /// <summary>
        /// calc the average
        /// </summary>
        /// <param name="vals"></param>
        /// <returns></returns>
        private decimal Average(decimal[] vals)
        {
            decimal sum = 0;
            for (int i = 0; i < vals.Length; ++i)
                sum += vals[i];
            return sum / vals.Length;
        }
        private static double Minimum(double[] vals)
        {
            double min = vals[0]; ;
            for (int i = 0; i < vals.Length; ++i)
                if (vals[i] < min) min = vals[i];
            return min;
        }

        public string Process(string jsonData)
        {
            string response = null;
            DataSet ds = Newtonsoft.Json.JsonConvert.DeserializeObject<DataSet>(jsonData);
            string method = ds.Tables["methods"].Rows[0].Field<string>("method");
            if (this.GetType().GetMethod(method, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase) != null)
            {
                System.Reflection.MethodInfo methodInfo = this.GetType().GetMethod(method, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase);
                string[] dataResult = ds.Tables["data"].Rows[0].Field<string>("data").Split(' ');
                decimal[] dec = new decimal[dataResult.Length];
                for (int i = 0; i < dataResult.Length; i++)
                {
                    dec[i] = decimal.Parse(dataResult[i]);
                }
                decimal result = (decimal)methodInfo.Invoke(this, new object[] { dec });
                response = result.ToString();
            }
            //returns response
            return response;
        }
    }
}
