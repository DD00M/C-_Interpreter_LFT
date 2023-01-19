using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumitrascuAndreiCosmin_proiectLFT
{
    class AtomLexical : NodSintactic
    {
        public AtomLexical(TipAtomLexical tip, int index, string text, object valoare)
        {
            Tip = tip;
            Index = index;
            Text = text;
            Valoare = valoare;
        }

        public override TipAtomLexical Tip { get; }
        public int Index { get; set; }
        public string Text { get; set; }
        public object Valoare { get; set; }

        public override IEnumerable<NodSintactic> GetCopii()
        {
            return Enumerable.Empty<NodSintactic>();
        }
        
        public void change_valoare(object dstValoare, object srcValoare)
        {
            if (dstValoare == Valoare)
            {
                Valoare = srcValoare;
            }
        }
    }
}
