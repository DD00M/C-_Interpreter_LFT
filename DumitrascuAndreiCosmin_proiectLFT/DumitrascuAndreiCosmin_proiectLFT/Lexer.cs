using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumitrascuAndreiCosmin_proiectLFT
{
    class Lexer
    {
        private readonly string _text;
        private int _index;
        private List<string> _erori = new List<string>();

        public Lexer(string text)
        {
            _text = text; 
        }
        public IEnumerable<string> Erori => _erori;

        private char getMyLine()
        {
            return _text[_index];
        }
        private char Curent
        {
            get
            {
                if (_index >= _text.Length)
                {
                    return '\0';
                }
                return _text[_index];
            }
        }

        private void Urmatorul()
        {
            _index++;
        }

        public AtomLexical Atom()
        {
            if (_index >= _text.Length)
                return new AtomLexical(TipAtomLexical.TerminatorAtomLexical, _index, "\0", null);

            if (char.IsLetterOrDigit(Curent))
            {
                var start = _index;

                while (char.IsLetterOrDigit(Curent))
                    Urmatorul();

                var dimensiune = _index - start;
                var text = _text.Substring(start, dimensiune);
                //if (!int.TryParse(text, out var valoare))
                //    _erori.Add($"Numarul {text} nu poate fi reprezentat ca un Int32.");
                return new AtomLexical(TipAtomLexical.CaracterAtomLexical, start, text, text);
            }

            if (char.IsWhiteSpace(Curent))
            {
                var start = _index;

                while (char.IsWhiteSpace(Curent))
                    Urmatorul();

                var dimensiune = _index - start;
                var text = _text.Substring(start, dimensiune);
                return new AtomLexical(TipAtomLexical.SpatiuAtomLexical, start, text, null);
            }

            if (Curent == '+')
                return new AtomLexical(TipAtomLexical.PlusAtomLexical, _index++, "+", null);
            else if (Curent == '-')
                return new AtomLexical(TipAtomLexical.MinusAtomLexical, _index++, "-", null);
            else if (Curent == '*')
                return new AtomLexical(TipAtomLexical.StarAtomLexical, _index++, "*", null);
            else if (Curent == '/')
                return new AtomLexical(TipAtomLexical.SlashAtomLexical, _index++, "/", null);
            else if (Curent == '(')
                return new AtomLexical(TipAtomLexical.ParantezaDeschisaAtomLexical, _index++, "(", null);
            else if (Curent == ')')
                return new AtomLexical(TipAtomLexical.ParantezaInchisaAtomLexical, _index++, ")", null);
            else if (Curent == ';')
                return new AtomLexical(TipAtomLexical.PunctSiVirgula, _index++, ";", null);


            _erori.Add($"EROARE: caracter invalid: '{Curent}'");
            return new AtomLexical(TipAtomLexical.InvalidAtomLexical, _index++, _text.Substring(_index - 1, 1), null);
        }
    }
}
