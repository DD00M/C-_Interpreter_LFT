using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumitrascuAndreiCosmin_proiectLFT
{
    sealed class ArboreSintactic
    {
        public ArboreSintactic(IEnumerable<string> erori, Expresie radacina, AtomLexical terminatorAtomLexical)
        {
            Erori = erori.ToArray();
            Radacina = radacina;
            TerminatorAtomLexical = terminatorAtomLexical;
        }

        public IReadOnlyCollection<string> Erori { get; }
        public Expresie Radacina { get; }
        public AtomLexical TerminatorAtomLexical { get; }
    }
}
