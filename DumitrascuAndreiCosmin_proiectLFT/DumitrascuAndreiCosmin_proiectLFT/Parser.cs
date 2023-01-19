using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumitrascuAndreiCosmin_proiectLFT
{
    class Parser
    {
        private readonly AtomLexical[] _atomiLexicali;
        private int _index;
        private int contorPunctSiVirgula { get; set; }
        private List<string> _erori = new List<string>();
        
        public void swap_values(object dstValoare, object srcValoare)
        {
            if ((string)srcValoare == "-")
            {
                _erori.Add($"Empty Value\n");
            }
            else
            {
                for (int i = 0; i < this._atomiLexicali.Length; i++)
                {
                    if (this._atomiLexicali[i].Valoare == null)
                    {
                        continue;
                    }
                    if (this._atomiLexicali[i].Valoare.ToString() == dstValoare.ToString())
                    {
                        this._atomiLexicali[i].change_valoare(this._atomiLexicali[i].Valoare, srcValoare);
                    }
                }
            }
        }
        public Parser(string text)
        {
            var atomiLexicali = new List<AtomLexical>();
            var lexer = new Lexer(text);
            contorPunctSiVirgula = 0;
            AtomLexical atomLexical;
            do
            {
                atomLexical = lexer.Atom();

                if (atomLexical.Tip != TipAtomLexical.SpatiuAtomLexical &&
                    atomLexical.Tip != TipAtomLexical.InvalidAtomLexical)
                {
                    if (atomLexical.Tip == TipAtomLexical.PunctSiVirgula)
                    {
                        contorPunctSiVirgula += 1;
                    }else atomiLexicali.Add(atomLexical);
                }

            }
            while (atomLexical.Tip != TipAtomLexical.TerminatorAtomLexical);

            _atomiLexicali = atomiLexicali.ToArray();
            _erori.AddRange(lexer.Erori);
        }

        public IEnumerable<string> Erori => _erori;
        private AtomLexical Varf(int avans)
        {
            var index = _index + avans;
            if (index >= _atomiLexicali.Length)
                return _atomiLexicali[_atomiLexicali.Length - 1];

            return _atomiLexicali[index];
        }

        private AtomLexical Curent => Varf(0);

        private AtomLexical UrmatorulAtomLexical()
        {
            var curent = Curent;
            _index++;
            return curent;
        }

        private AtomLexical Verifica(TipAtomLexical tip)
        {
            if (Curent.Tip == tip)
                return UrmatorulAtomLexical();

            _erori.Add($"EROARE: Atom lexical neasteptat <{Curent.Tip}>; se asteapta <{tip}>");
            return new AtomLexical(tip, Curent.Index, null, null);
        }


        private Expresie ParseazaExpresie()
        {
            return ParseazaTermen();
        }

        public ArboreSintactic Parseaza()
        {
            var expresie = ParseazaTermen();
            var terminator = Verifica(TipAtomLexical.TerminatorAtomLexical);

            return new ArboreSintactic(_erori, expresie, terminator);
        }

        private Expresie ParseazaTermen()
        {
            var stanga = ParseazaFactor();

            while (Curent.Tip == TipAtomLexical.PlusAtomLexical ||
                Curent.Tip == TipAtomLexical.MinusAtomLexical)
            {
                var operatorAtomLexical = UrmatorulAtomLexical();
                var dreapta = ParseazaFactor();
                stanga = new ExpresieBinara(stanga, operatorAtomLexical, dreapta);
            }

            return stanga;
        }

        private Expresie ParseazaFactor()
        {
            var stanga = ParseazaPrimaExpresie();

            while (Curent.Tip == TipAtomLexical.StarAtomLexical ||
                Curent.Tip == TipAtomLexical.SlashAtomLexical)
            {
                var operatorAtomLexical = UrmatorulAtomLexical();
                var dreapta = ParseazaPrimaExpresie();
                stanga = new ExpresieBinara(stanga, operatorAtomLexical, dreapta);
            }

            return stanga;
        }

        private Expresie ParseazaPrimaExpresie()
        {
            if (Curent.Tip == TipAtomLexical.ParantezaDeschisaAtomLexical)
            {
                var stanga = UrmatorulAtomLexical();
                var expresie = ParseazaExpresie();
                var dreapta = Verifica(TipAtomLexical.ParantezaInchisaAtomLexical);
                return new ExpresieParanteze(stanga, expresie, dreapta);
            }

            var numberToken = Verifica(TipAtomLexical.CaracterAtomLexical);
            return new ExpresieNumerica(numberToken);
        }
    }
}
