using System;
using System.Data;

namespace BusinessLayer
{
    public class LucroPorAno
    {
        public LucroPorAno()
        {
           
        }

        public LucroPorAno(int ano, float lucro)
            : this()
        {
            Ano = ano;
            Lucro = lucro;
        }

        public int Ano { get; set; }
        public float Lucro { get; set; }

    }
}
