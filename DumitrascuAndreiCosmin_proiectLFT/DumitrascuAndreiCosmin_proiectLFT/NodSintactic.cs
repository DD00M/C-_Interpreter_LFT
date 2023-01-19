using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumitrascuAndreiCosmin_proiectLFT
{
    abstract class NodSintactic
    {
        public abstract TipAtomLexical Tip { get; }

        public abstract IEnumerable<NodSintactic> GetCopii();
    }
}
