using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

namespace BusinessLayer
{
    public class LucroPorAnoCollection : Collection<LucroPorAno>
    {
        public LucroPorAnoCollection()
        {
        }

        internal void Adicionar(int ano, float lucro)
        {
            LucroPorAno lucroPorAno = this.FirstOrDefault(k => k.Ano == ano);
            if (lucroPorAno == null)
            {
                this.Add(new LucroPorAno(ano, lucro));
            }
            else
            {
                lucroPorAno.Lucro += lucro;
            }
        }
    }
}
