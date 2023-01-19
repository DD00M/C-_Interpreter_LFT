using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumitrascuAndreiCosmin_proiectLFT
{
    sealed class ExpresieParanteze : Expresie
    {
        public ExpresieParanteze(AtomLexical parantezaDeschisaAtomLexical, Expresie expresie, AtomLexical parantezaInchisaAtomLexical)
        {
            ParantezaDeschisaAtomLexical = parantezaDeschisaAtomLexical;
            Expresie = expresie;
            ParantezaInchisaAtomLexical = parantezaInchisaAtomLexical;
        }

        public override TipAtomLexical Tip => TipAtomLexical.ExpresieParanteze;

        public AtomLexical ParantezaDeschisaAtomLexical { get; }
        public Expresie Expresie { get; }
        public AtomLexical ParantezaInchisaAtomLexical { get; }

        public override IEnumerable<NodSintactic> GetCopii()
        {
            yield return ParantezaDeschisaAtomLexical;
            yield return Expresie;
            yield return ParantezaInchisaAtomLexical;
        }
    }
}
