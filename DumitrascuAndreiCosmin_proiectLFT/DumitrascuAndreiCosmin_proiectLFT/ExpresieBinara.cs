using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumitrascuAndreiCosmin_proiectLFT
{
    sealed class ExpresieBinara : Expresie
    {
        public ExpresieBinara(Expresie expresieStanga, AtomLexical operatorAtomLexical, Expresie expresieDreapta)
        {
            ExpresieStanga = expresieStanga;
            OperatorAtomLexical = operatorAtomLexical;
            ExpresieDreapta = expresieDreapta;
        }

        public Expresie ExpresieStanga { get; }
        public AtomLexical OperatorAtomLexical { get; }
        public Expresie ExpresieDreapta { get; }

        public override TipAtomLexical Tip => TipAtomLexical.ExpresieBinara;

        public override IEnumerable<NodSintactic> GetCopii()
        {
            yield return ExpresieStanga;
            yield return OperatorAtomLexical;
            yield return ExpresieDreapta;
        }
    }
}
