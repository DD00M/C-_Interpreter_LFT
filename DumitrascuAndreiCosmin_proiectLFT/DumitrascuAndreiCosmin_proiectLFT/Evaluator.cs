using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumitrascuAndreiCosmin_proiectLFT
{
    class Evaluator
    {
        private readonly Expresie _radacina;

        public Evaluator(Expresie radacina)
        {
            this._radacina = radacina;
        }

        public double Evalueaza4Values()
        {
            return EvalueazaExpresie_4Values(_radacina);
        }

        public string Evalueaza4Strings()
        {
            return EvalueazaExpresie_4Strings(_radacina);
        }
        private bool check_if_Real(string checker)
        {
            int x = 0;
            x = checker.IndexOf('.');
            if (x == -1) { return false; }
            return true;
        }

        private string EvalueazaExpresie_4Strings(Expresie nod)
        {
            string result = "";

            if (nod is ExpresieNumerica n)
            {
                return n.NumarAtomLexical.Valoare.ToString();
            }

            if (nod is ExpresieBinara b)
            {
                var stanga = EvalueazaExpresie_4Strings(b.ExpresieStanga);
                var dreapta = EvalueazaExpresie_4Strings(b.ExpresieDreapta);

                if (b.OperatorAtomLexical.Tip == TipAtomLexical.PlusAtomLexical)
                {
                    return stanga + dreapta;
                }
                else throw new Exception($"Operator binar neasteptat {b.OperatorAtomLexical.Tip}.");                
            }
            if(nod is ExpresieParanteze p)
                return EvalueazaExpresie_4Strings(p.Expresie);

            throw new Exception($"Expresie neasteptata {nod.Tip}.");
        }
        private double EvalueazaExpresie_4Values(Expresie nod)
        {
            double result = 0;
            if (nod is ExpresieNumerica n)
            {
                if (check_if_Real(n.NumarAtomLexical.Valoare.ToString()) == false)
                { 
                    result = Int32.Parse(n.NumarAtomLexical.Valoare.ToString());
                }else if (check_if_Real(n.NumarAtomLexical.Valoare.ToString()))
                {
                    result = Double.Parse(n.NumarAtomLexical.Valoare.ToString());
                }
                return result;
            }

            if (nod is ExpresieBinara b)
            {
                var stanga = EvalueazaExpresie_4Values(b.ExpresieStanga);
                var dreapta = EvalueazaExpresie_4Values(b.ExpresieDreapta);

                if (b.OperatorAtomLexical.Tip == TipAtomLexical.PlusAtomLexical)
                    return stanga + dreapta;
                else if (b.OperatorAtomLexical.Tip == TipAtomLexical.MinusAtomLexical)
                    return stanga - dreapta;
                else if (b.OperatorAtomLexical.Tip == TipAtomLexical.StarAtomLexical)
                    return stanga * dreapta;
                else if (b.OperatorAtomLexical.Tip == TipAtomLexical.SlashAtomLexical)
                    return stanga / dreapta;
                else
                    throw new Exception($"Operator binar neasteptat {b.OperatorAtomLexical.Tip}.");
            }

            if (nod is ExpresieParanteze p)
                return EvalueazaExpresie_4Values(p.Expresie);

            throw new Exception($"Expresie neasteptata {nod.Tip}.");
        }
    }
}
