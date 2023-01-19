using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace DumitrascuAndreiCosmin_proiectLFT
{
    internal class Program
    {
        static string[] get_list_of_string_from_file(string fs)
        {
            string[] list = File.ReadAllLines(fs);

            return list;
        }
        static void Main(string[] args)
        {
            hello:
            Console.ForegroundColor = ConsoleColor.White;

            Console.Out.WriteLine("read=readFromFile, type=Type: \n");
            string checker = "";

            checker = Console.ReadLine();

            if (checker == "read")
            {
                string fileName = "D:\\visual studio fisiere\\DumitrascuAndreiCosmin_proiectLFT\\DumitrascuAndreiCosmin_proiectLFT\\in.txt";
                string[] arry = get_list_of_string_from_file(fileName);

                Utilities utilities = new Utilities(arry);
                utilities.checkVars();
                utilities.print_result();
            }
            else if (checker == "type")
            {
                Utilities utilities = new Utilities();
                utilities.checkVarsNewForTyping();
                utilities.print_result();
            }
            else Console.WriteLine("wrong\n");

            goto hello;
        }

        static void AfiseazaArbore(NodSintactic nod, string indentare = "", bool ultimulNod = true)
        {
            var prefix = ultimulNod ? "└──" : "├──";
            Console.Write(indentare);
            Console.Write(prefix);
            Console.Write(nod.Tip);

            if (nod is AtomLexical t && t.Valoare != null)
            {
                Console.Write(" ");
                Console.Write(t.Valoare);
            }

            Console.WriteLine();

            indentare += ultimulNod ? "    " : "|   ";

            var ultimulCopil = nod.GetCopii().LastOrDefault();

            foreach (var c in nod.GetCopii())
            {
                AfiseazaArbore(c, indentare, c == ultimulCopil);
            }
        }

    }


    enum TipAtomLexical 
    {
        CaracterAtomLexical,
        SpatiuAtomLexical,
        PlusAtomLexical,
        MinusAtomLexical,
        StarAtomLexical,
        SlashAtomLexical,
        ParantezaDeschisaAtomLexical,
        ParantezaInchisaAtomLexical,
        InvalidAtomLexical,
        ExpresieNumerica,
        ExpresieBinara,
        TerminatorAtomLexical,
        ExpresieParanteze,
        PunctSiVirgula
    }
}
