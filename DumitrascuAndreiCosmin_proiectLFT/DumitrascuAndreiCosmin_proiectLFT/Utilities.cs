using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DumitrascuAndreiCosmin_proiectLFT
{
    internal class Utilities
    {
        private string[] m_strings;
        private string m_currentString;
        private int m_strings_index;
        private int m_string_index;
        private List<string> m_datatype;
        private List<string> m_dataname;
        private List<string> m_datavalue;
        private List<int> m_indexInPage;
        public bool m_can_i_continue = true;
        public Utilities(string[] firstarr) {
            this.m_strings = firstarr;
            this.m_dataname = new List<string>();
            this.m_datavalue = new List<string>();
            this.m_datatype = new List<string>();
            this.m_indexInPage = new List<int>();
        }

        public Utilities()
        {
            this.m_dataname = new List<string>();
            this.m_datavalue = new List<string>();
            this.m_datatype = new List<string>();
            this.m_indexInPage = new List<int>();
        }

        public bool check_if_chrExist(string smth, char c)
        {
            if (smth.IndexOf(c) == -1)
            {
                return false;
            }
            return true;
        }
        public void checkVars()
        {
            for (int i = 0; i < this.m_strings.Count(); i++)
            {
                if (check_smth_init_smth_value(this.m_strings[i].ToString(), i + 1) == false)
                {
                    return;
                }
                Console.ForegroundColor = ConsoleColor.White;
                this.m_currentString = this.m_strings[i].ToString();
                this.m_strings_index = i;
                List<string> list = new List<string>();
                if (check_correct(this.m_strings[this.m_strings_index].ToString()) == false)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Forgot a ';' -- line: " + (i + 1).ToString() + "\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                }
                if (check_if_unitialised(this.m_strings[this.m_strings_index].ToString()) == false && checker_ceva(this.m_strings[this.m_strings_index].ToString()) == false)
                {
                    list = parse_string_for_variables(this.m_strings[this.m_strings_index].ToString());
                    if (list[0] == "int" || list[0] == "float" || list[0] == "string" || list[0] == "double" || list[0] == "unsigned" || list[0] == "char")
                    {
                        bool is_val_existent = false;
                        for (int j = 1; j < list.Count; j++)
                        {
                            for (int z = 0; z < this.m_dataname.Count; z++)
                            {
                                if (list[j].ToString() == this.m_dataname[z].ToString())
                                {
                                    is_val_existent = true;
                                }
                            }
                            if (is_val_existent == false)
                            {
                                this.m_datatype.Add(list[0]);
                                this.m_dataname.Add(list[j]);
                                this.m_datavalue.Add("-");
                                this.m_indexInPage.Add(i + 1);
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Value " + list[j].ToString() + " already existent -- line: " + (i + 1).ToString() + "\n");
                                Console.ForegroundColor = ConsoleColor.White;
                                
                                return;
                            }
                        }
                    }
                }
                else if (check_if_unitialised(this.m_strings[this.m_strings_index].ToString()) == true && checker_ceva(this.m_strings[this.m_strings_index].ToString()) == true)
                {
                    List<string> my_list = parse_by_equal_sign(this.m_strings[this.m_strings_index].ToString());
                    List<string> finalList = parse_by_space_sign(my_list[0]);
                    var my_parser = new Parser(String.Concat(my_list[1].Where(c => !Char.IsWhiteSpace(c))).ToString());

                    bool is_inList = false;
                    int index_of_found = -1;

                    for (int p = 0; p < this.m_dataname.Count(); p++)
                    {
                        if (finalList[0] == this.m_dataname[p])
                        {
                            is_inList = true;
                            index_of_found = p;
                        }
                    }
                    if (ContainsAnyCalculusForInitialisation(this.m_strings[this.m_strings_index].ToString(), "+", "-", "*", "/") == true)
                    {
                        if (is_inList == false)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Value not initialised " + finalList[0] + " line nr -> " + (i + 1).ToString() + '\n');
                            Console.ForegroundColor = ConsoleColor.White;
                            return;
                        }
                        else
                        {
                            var auxArbore = my_parser.Parseaza();

                            Console.WriteLine("ARBORELE SINTACTIC CU VALORILE NEINLOCUITE: " + this.m_strings[this.m_strings_index].ToString() + "\n");

                            AfiseazaArbore(auxArbore.Radacina);
                            
                            
                            list = parse_string_for_variables(String.Concat(my_list[1].Where(c => !Char.IsWhiteSpace(c))).ToString());
                            for (int j = 0; j < list.Count(); j++)
                            {
                                for (int z = 0; z < this.m_dataname.Count; z++)
                                {
                                    if (this.m_dataname[z] == list[j])
                                    {
                                        my_parser.swap_values(list[j], this.m_datavalue[z]);
                                    }
                                }
                            }
                            var e = new Evaluator(auxArbore.Radacina);

                            Console.WriteLine("\n\nARBORELE SINTACTIC CU VALORILE NEINLOCUITE: " + this.m_strings[this.m_strings_index].ToString() + "\n");

                            AfiseazaArbore(auxArbore.Radacina);

                            int my_counter = 0;
                            int parse_result = 0;
                            for (int y = 0; y < list.Count(); y++)
                            {
                                for (int u = 0; u < this.m_dataname.Count; u++)
                                {
                                    if (this.m_dataname[u] == list[y])
                                    {
                                        if (Int32.TryParse(this.m_datavalue[u], out parse_result) == false)
                                        {
                                            my_counter++;
                                        }
                                    }
                                }
                            }
                            if (my_counter == list.Count())
                            {
                                int my_personal_checker = 0;
                                List<string> uninit = new List<string>();
                                string rezultat = "";
                                for (int y = 0; y < list.Count; y++)
                                {
                                    if (check_if_exist(list[y].ToString()) == true && clear_to_engage(list[y].ToString()) == true)
                                    {
                                        my_personal_checker++;
                                    }
                                    else
                                    {
                                        uninit.Add(list[y].ToString());
                                    }
                                }
                                if (my_personal_checker == list.Count)
                                {
                                    rezultat = e.Evalueaza4Strings();
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    for (int o = 0; o < uninit.Count; o++)
                                    {
                                        Console.WriteLine("Unitialised or inexistent value: " + uninit[o].ToString() + " line nr -> " + (i + 1).ToString() + '\n');
                                        return;
                                    }
                                    Console.ForegroundColor = ConsoleColor.White;
                                }
                                if (index_of_found != -1)
                                {
                                    this.m_datavalue[index_of_found] = rezultat.ToString();
                                    Console.Out.WriteLine("Rezultat : " + this.m_datatype[index_of_found] + "( " + my_list[0] + " )" + " = " + rezultat.ToString());
                                }else Console.WriteLine("Unitialised or inexistent value: " + this.m_datatype[index_of_found] + my_list[0] + " line nr -> " + (i + 1).ToString() + '\n');
                            }
                            else
                            {
                                int my_personal_checker = 0;
                                List<string> uninit = new List<string>();
                                double rezultat = 0;
                                for (int y = 0; y < list.Count; y++)
                                {
                                    if (check_if_exist(list[y].ToString()) == true && clear_to_engage(list[y].ToString()) == true)
                                    {
                                        my_personal_checker++;
                                    }
                                    else
                                    {
                                        uninit.Add(list[y].ToString());
                                    }
                                }
                                if (my_personal_checker == list.Count)
                                {
                                    rezultat = e.Evalueaza4Values();
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    for (int o = 0; o < uninit.Count; o++)
                                    {
                                        Console.WriteLine("Unitialised or inexistent value: " + uninit[o].ToString() + " line nr -> " + (i + 1).ToString() + '\n');
                                        return;
                                    }
                                    Console.ForegroundColor = ConsoleColor.White;
                                }
                                this.m_datatype.Add(this.m_datatype[index_of_found]);
                                this.m_dataname.Add(my_list[0]);
                                if (this.m_datatype[index_of_found] == "int")
                                {
                                    this.m_datavalue.Add(((int)rezultat).ToString());
                                    Console.Out.WriteLine("Rezultat : " + this.m_datatype[index_of_found] + "( " + my_list[0] + " )" + " = " + ((int)rezultat).ToString());
                                }
                                else
                                {
                                    this.m_datavalue.Add(rezultat.ToString());
                                    Console.Out.WriteLine("Rezultat : " + this.m_datatype[index_of_found] + "( " + my_list[0] + " )" + " = " + rezultat.ToString());
                                }
                            }

                        }
                    }
                    else
                    {
                        list = parse_string_for_variables(this.m_strings[this.m_strings_index].ToString());

                        if (is_inList == true && checker_ceva(this.m_strings[i].ToString()) == true)
                        {
                            List<string> list2 = new List<string>();
                            list2 = parse_string_for_variables(my_list[1]);
                            this.m_datavalue[index_of_found] = list2[0];
                            this.m_indexInPage.Add(i + 1);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Value " + list[0].ToString() + " already existent -- line: " + (i + 1).ToString() + "\n");
                            Console.ForegroundColor = ConsoleColor.White;

                            return;
                        }
                    }
                }
                else if (check_if_unitialised(this.m_strings[this.m_strings_index].ToString()) == false && checker_ceva(this.m_strings[this.m_strings_index].ToString()) == true)
                {
                    list = parse_string_for_variables(this.m_strings[this.m_strings_index].ToString());
                    var parser = new Parser(this.m_strings[this.m_strings_index].ToString());
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("ARBORELE SINTACTIC CU VALORILE NEINLOCUITE: " + this.m_strings[this.m_strings_index].ToString() + "\n");
                    var arboreSintactic1 = parser.Parseaza();
                    AfiseazaArbore(arboreSintactic1.Radacina);
                    for (int j = 0; j < list.Count; j++)
                    {
                        for (int z = 0; z < this.m_dataname.Count; z++)
                        {
                            if (this.m_dataname[z] == list[j])
                            {
                                parser.swap_values(list[j], this.m_datavalue[z]);
                            }
                        }
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("ARBORELE SINTACTIC CU VALORILE INLOCUITE: " + this.m_strings[this.m_strings_index].ToString() + "\n");
                    AfiseazaArbore(arboreSintactic1.Radacina);

                    int my_counter = 0;
                    int parse_result = 0;
                    for (int y = 0; y < list.Count; y++)
                    {
                        for (int u = 0; u < this.m_dataname.Count; u++)
                        {
                            if (this.m_dataname[u] == list[y])
                            {
                                if (Int32.TryParse(this.m_datavalue[u], out parse_result) == false)
                                {
                                    my_counter++;
                                }
                            }
                        }
                    }

                    int my_personal_checker = 0;
                    if (my_counter == list.Count)
                    {
                        var e = new Evaluator(arboreSintactic1.Radacina);
                        List<string> uninit = new List<string>();
                        for (int y = 0; y < list.Count; y++)
                        {
                            if (check_if_exist(list[y].ToString()) == true && clear_to_engage(list[y].ToString()) == true)
                            {
                                my_personal_checker++;
                            }
                            else
                            {
                                uninit.Add(list[y].ToString());
                            }
                        }
                        if (my_personal_checker == list.Count)
                        {
                            var rezultat = e.Evalueaza4Strings();
                            Console.WriteLine(rezultat);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            for (int o = 0; o < uninit.Count; o++)
                            {
                                Console.WriteLine("Unitialised or inexistent value: " + uninit[o].ToString() + " line nr -> " + (i + 1).ToString() + '\n');
                                return;
                            }
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        my_counter = 0;
                    }
                    else
                    {
                        var e = new Evaluator(arboreSintactic1.Radacina);
                        List<string> uninit = new List<string>();
                        for (int y = 0; y < list.Count; y++)
                        {
                            if (check_if_exist(list[y].ToString()) == true && clear_to_engage(list[y].ToString()) == true)
                            {
                                my_personal_checker++;
                            }
                            else
                            {
                                uninit.Add(list[y].ToString());
                            }
                        }
                        if (my_personal_checker == list.Count)
                        {
                            var rezultat = e.Evalueaza4Values();
                            Console.WriteLine(rezultat);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            for (int o = 0; o < uninit.Count; o++)
                            {
                                Console.WriteLine("Unitialised or inexistent value: " + uninit[o].ToString() + " line nr -> " + (i + 1).ToString() + '\n');
                                return;
                            }
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        my_counter = 0;
                    }
                    var culoare = Console.ForegroundColor;
                    if (parser.Erori.Any())
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        foreach (var eroare in arboreSintactic1.Erori)
                            Console.WriteLine(eroare);

                        Console.ForegroundColor = culoare;
                    }
                }
                else if (check_if_unitialised(this.m_strings[this.m_strings_index].ToString()) == true && checker_ceva(this.m_strings[this.m_strings_index].ToString()) == false)
                {
                    if (ContainsAnyCalculusForInitialisation(this.m_strings[this.m_strings_index].ToString(), "+", "-", "*", "/") == true)
                    {
                        List<string> my_list = parse_by_equal_sign(this.m_strings[this.m_strings_index].ToString());
                        var my_parser = new Parser(String.Concat(my_list[1].Where(c => !Char.IsWhiteSpace(c))).ToString());
                        List<string> init_name = parse_by_space_sign(my_list[0]);

                        bool is_inList = false;

                        for (int z = 0; z < init_name.Count(); z++)
                        {
                            for (int p = 0; p < this.m_dataname.Count; p++)
                            {
                                if (init_name[z].ToString() == this.m_dataname[p].ToString())
                                {
                                    is_inList = true;
                                }
                            }
                        }

                        if (is_inList == true)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Value already initialised " + init_name[0] + " - " + init_name[1] + " line nr -> " + (i + 1).ToString() + '\n');  
                            Console.ForegroundColor = ConsoleColor.White;
                            return;
                        }
                        else {
                            Console.ForegroundColor = ConsoleColor.White;
                            var auxArbore = my_parser.Parseaza();
                            Console.WriteLine("ARBORELE SINTACTIC CU VALORILE NEINLOCUITE: " + this.m_strings[this.m_strings_index].ToString() + "\n");
                            AfiseazaArbore(auxArbore.Radacina);
                            list = parse_string_for_variables(String.Concat(my_list[1].Where(c => !Char.IsWhiteSpace(c))).ToString());
                            for (int j = 0; j < list.Count(); j++)
                            {
                                for (int z = 0; z < this.m_dataname.Count; z++)
                                {
                                    if (this.m_dataname[z] == list[j])
                                    {
                                        my_parser.swap_values(list[j], this.m_datavalue[z]);
                                    }
                                }
                            }
                            var e = new Evaluator(auxArbore.Radacina);

                            Console.WriteLine("ARBORELE SINTACTIC CU VALORILE INLOCUITE: " + this.m_strings[this.m_strings_index].ToString() + "\n");
                            AfiseazaArbore(auxArbore.Radacina);

                            int my_counter = 0;
                            int parse_result = 0;
                            for (int y = 0; y < list.Count(); y++)
                            {
                                for (int u = 0; u < this.m_dataname.Count; u++)
                                {
                                    if (this.m_dataname[u] == list[y])
                                    {
                                        if (Int32.TryParse(this.m_datavalue[u], out parse_result) == false)
                                        {
                                            my_counter++;
                                        }
                                    }
                                }
                            }
                            if (my_counter == list.Count())
                            {
                                int my_personal_checker = 0;
                                List<string> uninit = new List<string>();
                                string rezultat = "";
                                for (int y = 0; y < list.Count; y++)
                                {
                                    if (check_if_exist(list[y].ToString()) == true && clear_to_engage(list[y].ToString()) == true)
                                    {
                                        my_personal_checker++;
                                    }
                                    else
                                    {
                                        uninit.Add(list[y].ToString());
                                    }
                                }
                                if (my_personal_checker == list.Count)
                                {
                                    rezultat = e.Evalueaza4Strings();
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    for (int o = 0; o < uninit.Count; o++)
                                    {
                                        Console.WriteLine("Unitialised or inexistent value: " + uninit[o].ToString() + " line nr -> " + (i + 1).ToString() + '\n');
                                        return;
                                    }
                                    Console.ForegroundColor = ConsoleColor.White;
                                }
                                this.m_datatype.Add(init_name[0]);
                                this.m_dataname.Add(init_name[1]);
                                this.m_datavalue.Add(rezultat.ToString());
                                Console.Out.WriteLine("Rezultat : " + init_name[0] + "( " + init_name[1] + " )" + " = " + rezultat.ToString());
                            }
                            else
                            {
                                int my_personal_checker = 0;
                                List<string> uninit = new List<string>();
                                double rezultat = 0;
                                for (int y = 0; y < list.Count; y++)
                                {
                                    if (check_if_exist(list[y].ToString()) == true && clear_to_engage(list[y].ToString()) == true)
                                    {
                                        my_personal_checker++;
                                    }
                                    else
                                    {
                                        uninit.Add(list[y].ToString());
                                    }
                                }
                                if (my_personal_checker == list.Count)
                                {
                                    rezultat = e.Evalueaza4Values();
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    for (int o = 0; o < uninit.Count; o++)
                                    {
                                        Console.WriteLine("Unitialised or inexistent value: " + uninit[o].ToString() + " line nr -> " + (i + 1).ToString() + '\n');
                                        return;
                                    }
                                    Console.ForegroundColor = ConsoleColor.White;
                                }
                                this.m_datatype.Add(init_name[0]);
                                this.m_dataname.Add(init_name[1]);
                                if (init_name[0] == "int")
                                {
                                    this.m_datavalue.Add(((int)rezultat).ToString());
                                    Console.Out.WriteLine("Rezultat : " + init_name[0] + "( " + init_name[1] + " )" + " = " + ((int)rezultat).ToString());
                                }
                                else
                                    {
                                    this.m_datavalue.Add(rezultat.ToString());
                                    Console.Out.WriteLine("Rezultat : " + init_name[0] + "( " + init_name[1] + " )" + " = " + rezultat.ToString());
                                }
                            }

                        }
                    }
                    else
                    {
                        list = parse_string_for_variables(this.m_strings[this.m_strings_index].ToString());
                        if (list[0] == "int" || list[0] == "float" || list[0] == "string" || list[0] == "double" || list[0] == "unsigned" || list[0] == "char")
                        {
                            bool is_val_existent = false;

                            for (int z = 0; z < this.m_dataname.Count; z++)
                            {
                                if (list[1].ToString() == this.m_dataname[z].ToString())
                                {
                                    is_val_existent = true;
                                }
                            }

                            if (is_val_existent == false)
                            {
                                this.m_datatype.Add(list[0]);
                                this.m_dataname.Add(list[1]);
                                this.m_datavalue.Add(list[2]);
                                this.m_indexInPage.Add(i + 1);
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Value " + list[1].ToString() + " already existent -- line: " + (i + 1).ToString() + "\n");
                                Console.ForegroundColor = ConsoleColor.White;

                                return;
                            }

                        }
                        else
                        {
                            Console.WriteLine("Wrong op2\n");
                        }
                    }
                }
            }
            ;
        }

        public void checkVarsNewForTyping()
        {
            int lo = 0;
            while (true)
            {
                lo++;
                Console.WriteLine("Type input line: ");
                string input_for_arry = Console.ReadLine();
                Console.WriteLine("\n");

                if (ContainsAnyCalculusForInitialisation(input_for_arry, "return") == true)
                {
                    return;
                }

                if (check_smth_init_smth_value(input_for_arry, lo) == false)
                {
                    return;
                }

                List<string> aux_list = new List<string>();
                aux_list.Add(input_for_arry);

                this.m_strings = aux_list.ToArray();

                for (int i = 0; i < this.m_strings.Count(); i++)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    this.m_currentString = this.m_strings[i].ToString();
                    this.m_strings_index = i;
                    List<string> list = new List<string>();
                    if (check_correct(this.m_strings[this.m_strings_index].ToString()) == false)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Forgot a ';' -- line: " + (i + 1).ToString() + "\n");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    }
                    if (check_if_unitialised(this.m_strings[this.m_strings_index].ToString()) == false && checker_ceva(this.m_strings[this.m_strings_index].ToString()) == false)
                    {
                        list = parse_string_for_variables(this.m_strings[this.m_strings_index].ToString());
                        if (list[0] == "int" || list[0] == "float" || list[0] == "string" || list[0] == "double" || list[0] == "unsigned" || list[0] == "char")
                        {
                            bool is_val_existent = false;
                            for (int j = 1; j < list.Count; j++)
                            {
                                for (int z = 0; z < this.m_dataname.Count; z++)
                                {
                                    if (list[j].ToString() == this.m_dataname[z].ToString())
                                    {
                                        is_val_existent = true;
                                    }
                                }
                                if (is_val_existent == false)
                                {
                                    this.m_datatype.Add(list[0]);
                                    this.m_dataname.Add(list[j]);
                                    this.m_datavalue.Add("-");
                                    this.m_indexInPage.Add(i + 1);
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Value " + list[j].ToString() + " already existent -- line: " + (i + 1).ToString() + "\n");
                                    Console.ForegroundColor = ConsoleColor.White;

                                    return;
                                }
                            }
                        }
                    }
                    else if (check_if_unitialised(this.m_strings[this.m_strings_index].ToString()) == true && checker_ceva(this.m_strings[this.m_strings_index].ToString()) == true)
                    {
                        List<string> my_list = parse_by_equal_sign(this.m_strings[this.m_strings_index].ToString());
                        List<string> finalList = parse_by_space_sign(my_list[0]);
                        var my_parser = new Parser(String.Concat(my_list[1].Where(c => !Char.IsWhiteSpace(c))).ToString());

                        bool is_inList = false;
                        int index_of_found = -1;

                        for (int p = 0; p < this.m_dataname.Count(); p++)
                        {
                            if (finalList[0] == this.m_dataname[p])
                            {
                                is_inList = true;
                                index_of_found = p;
                            }
                        }
                        if (ContainsAnyCalculusForInitialisation(this.m_strings[this.m_strings_index].ToString(), "+", "-", "*", "/") == true)
                        {
                            if (is_inList == false)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Value not initialised " + finalList[0] + " line nr -> " + (i + 1).ToString() + '\n');
                                Console.ForegroundColor = ConsoleColor.White;
                                return;
                            }
                            else
                            {
                                var auxArbore = my_parser.Parseaza();

                                Console.WriteLine("ARBORELE SINTACTIC CU VALORILE NEINLOCUITE: " + this.m_strings[this.m_strings_index].ToString() + "\n");

                                AfiseazaArbore(auxArbore.Radacina);


                                list = parse_string_for_variables(String.Concat(my_list[1].Where(c => !Char.IsWhiteSpace(c))).ToString());
                                for (int j = 0; j < list.Count(); j++)
                                {
                                    for (int z = 0; z < this.m_dataname.Count; z++)
                                    {
                                        if (this.m_dataname[z] == list[j])
                                        {
                                            my_parser.swap_values(list[j], this.m_datavalue[z]);
                                        }
                                    }
                                }
                                var e = new Evaluator(auxArbore.Radacina);

                                Console.WriteLine("\n\nARBORELE SINTACTIC CU VALORILE NEINLOCUITE: " + this.m_strings[this.m_strings_index].ToString() + "\n");

                                AfiseazaArbore(auxArbore.Radacina);

                                int my_counter = 0;
                                int parse_result = 0;
                                for (int y = 0; y < list.Count(); y++)
                                {
                                    for (int u = 0; u < this.m_dataname.Count; u++)
                                    {
                                        if (this.m_dataname[u] == list[y])
                                        {
                                            if (Int32.TryParse(this.m_datavalue[u], out parse_result) == false)
                                            {
                                                my_counter++;
                                            }
                                        }
                                    }
                                }
                                if (my_counter == list.Count())
                                {
                                    int my_personal_checker = 0;
                                    List<string> uninit = new List<string>();
                                    string rezultat = "";
                                    for (int y = 0; y < list.Count; y++)
                                    {
                                        if (check_if_exist(list[y].ToString()) == true && clear_to_engage(list[y].ToString()) == true)
                                        {
                                            my_personal_checker++;
                                        }
                                        else
                                        {
                                            uninit.Add(list[y].ToString());
                                        }
                                    }
                                    if (my_personal_checker == list.Count)
                                    {
                                        rezultat = e.Evalueaza4Strings();
                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        for (int o = 0; o < uninit.Count; o++)
                                        {
                                            Console.WriteLine("Unitialised or inexistent value: " + uninit[o].ToString() + " line nr -> " + (i + 1).ToString() + '\n');
                                            return;
                                        }
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }
                                    if (index_of_found != -1)
                                    {
                                        this.m_datavalue[index_of_found] = rezultat.ToString();
                                        Console.Out.WriteLine("Rezultat : " + this.m_datatype[index_of_found] + "( " + my_list[0] + " )" + " = " + rezultat.ToString());
                                    }
                                    else Console.WriteLine("Unitialised or inexistent value: " + this.m_datatype[index_of_found] + my_list[0] + " line nr -> " + (i + 1).ToString() + '\n');
                                }
                                else
                                {
                                    int my_personal_checker = 0;
                                    List<string> uninit = new List<string>();
                                    double rezultat = 0;
                                    for (int y = 0; y < list.Count; y++)
                                    {
                                        if (check_if_exist(list[y].ToString()) == true && clear_to_engage(list[y].ToString()) == true)
                                        {
                                            my_personal_checker++;
                                        }
                                        else
                                        {
                                            uninit.Add(list[y].ToString());
                                        }
                                    }
                                    if (my_personal_checker == list.Count)
                                    {
                                        rezultat = e.Evalueaza4Values();
                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        for (int o = 0; o < uninit.Count; o++)
                                        {
                                            Console.WriteLine("Unitialised or inexistent value: " + uninit[o].ToString() + " line nr -> " + (i + 1).ToString() + '\n');
                                            return;
                                        }
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }
                                    this.m_datatype.Add(this.m_datatype[index_of_found]);
                                    this.m_dataname.Add(my_list[0]);
                                    if (this.m_datatype[index_of_found] == "int")
                                    {
                                        this.m_datavalue.Add(((int)rezultat).ToString());
                                        Console.Out.WriteLine("Rezultat : " + this.m_datatype[index_of_found] + "( " + my_list[0] + " )" + " = " + ((int)rezultat).ToString());
                                    }
                                    else
                                    {
                                        this.m_datavalue.Add(rezultat.ToString());
                                        Console.Out.WriteLine("Rezultat : " + this.m_datatype[index_of_found] + "( " + my_list[0] + " )" + " = " + rezultat.ToString());
                                    }
                                }

                            }
                        }
                        else
                        {
                            list = parse_string_for_variables(this.m_strings[this.m_strings_index].ToString());

                            if (is_inList == true && checker_ceva(this.m_strings[i].ToString()) == true)
                            {
                                List<string> list2 = new List<string>();
                                list2 = parse_string_for_variables(my_list[1]);
                                this.m_datavalue[index_of_found] = list2[0];
                                this.m_indexInPage.Add(i + 1);
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Value " + list[0].ToString() + " already existent -- line: " + (i + 1).ToString() + "\n");
                                Console.ForegroundColor = ConsoleColor.White;

                                return;
                            }
                        }
                    }
                    else if (check_if_unitialised(this.m_strings[this.m_strings_index].ToString()) == false && checker_ceva(this.m_strings[this.m_strings_index].ToString()) == true)
                    {
                        list = parse_string_for_variables(this.m_strings[this.m_strings_index].ToString());
                        var parser = new Parser(this.m_strings[this.m_strings_index].ToString());
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("ARBORELE SINTACTIC CU VALORILE NEINLOCUITE: " + this.m_strings[this.m_strings_index].ToString() + "\n");
                        var arboreSintactic1 = parser.Parseaza();
                        AfiseazaArbore(arboreSintactic1.Radacina);
                        for (int j = 0; j < list.Count; j++)
                        {
                            for (int z = 0; z < this.m_dataname.Count; z++)
                            {
                                if (this.m_dataname[z] == list[j])
                                {
                                    parser.swap_values(list[j], this.m_datavalue[z]);
                                }
                            }
                        }
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("ARBORELE SINTACTIC CU VALORILE INLOCUITE: " + this.m_strings[this.m_strings_index].ToString() + "\n");
                        AfiseazaArbore(arboreSintactic1.Radacina);

                        int my_counter = 0;
                        int parse_result = 0;
                        for (int y = 0; y < list.Count; y++)
                        {
                            for (int u = 0; u < this.m_dataname.Count; u++)
                            {
                                if (this.m_dataname[u] == list[y])
                                {
                                    if (Int32.TryParse(this.m_datavalue[u], out parse_result) == false)
                                    {
                                        my_counter++;
                                    }
                                }
                            }
                        }

                        int my_personal_checker = 0;
                        if (my_counter == list.Count)
                        {
                            var e = new Evaluator(arboreSintactic1.Radacina);
                            List<string> uninit = new List<string>();
                            for (int y = 0; y < list.Count; y++)
                            {
                                if (check_if_exist(list[y].ToString()) == true && clear_to_engage(list[y].ToString()) == true)
                                {
                                    my_personal_checker++;
                                }
                                else
                                {
                                    uninit.Add(list[y].ToString());
                                }
                            }
                            if (my_personal_checker == list.Count)
                            {
                                var rezultat = e.Evalueaza4Strings();
                                Console.WriteLine(rezultat);
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                for (int o = 0; o < uninit.Count; o++)
                                {
                                    Console.WriteLine("Unitialised or inexistent value: " + uninit[o].ToString() + " line nr -> " + (i + 1).ToString() + '\n');
                                    return;
                                }
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            my_counter = 0;
                        }
                        else
                        {
                            var e = new Evaluator(arboreSintactic1.Radacina);
                            List<string> uninit = new List<string>();
                            for (int y = 0; y < list.Count; y++)
                            {
                                if (check_if_exist(list[y].ToString()) == true && clear_to_engage(list[y].ToString()) == true)
                                {
                                    my_personal_checker++;
                                }
                                else
                                {
                                    uninit.Add(list[y].ToString());
                                }
                            }
                            if (my_personal_checker == list.Count)
                            {
                                var rezultat = e.Evalueaza4Values();
                                Console.WriteLine(rezultat);
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                for (int o = 0; o < uninit.Count; o++)
                                {
                                    Console.WriteLine("Unitialised or inexistent value: " + uninit[o].ToString() + " line nr -> " + (i + 1).ToString() + '\n');
                                    return;
                                }
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            my_counter = 0;
                        }
                        var culoare = Console.ForegroundColor;
                        if (parser.Erori.Any())
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            foreach (var eroare in arboreSintactic1.Erori)
                                Console.WriteLine(eroare);

                            Console.ForegroundColor = culoare;
                        }
                    }
                    else if (check_if_unitialised(this.m_strings[this.m_strings_index].ToString()) == true && checker_ceva(this.m_strings[this.m_strings_index].ToString()) == false)
                    {
                        if (ContainsAnyCalculusForInitialisation(this.m_strings[this.m_strings_index].ToString(), "+", "-", "*", "/") == true)
                        {
                            List<string> my_list = parse_by_equal_sign(this.m_strings[this.m_strings_index].ToString());
                            var my_parser = new Parser(String.Concat(my_list[1].Where(c => !Char.IsWhiteSpace(c))).ToString());
                            List<string> init_name = parse_by_space_sign(my_list[0]);

                            bool is_inList = false;

                            for (int z = 0; z < init_name.Count(); z++)
                            {
                                for (int p = 0; p < this.m_dataname.Count; p++)
                                {
                                    if (init_name[z].ToString() == this.m_dataname[p].ToString())
                                    {
                                        is_inList = true;
                                    }
                                }
                            }

                            if (is_inList == true)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Value already initialised " + init_name[0] + " - " + init_name[1] + " line nr -> " + (i + 1).ToString() + '\n');
                                Console.ForegroundColor = ConsoleColor.White;
                                return;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.White;
                                var auxArbore = my_parser.Parseaza();
                                Console.WriteLine("ARBORELE SINTACTIC CU VALORILE NEINLOCUITE: " + this.m_strings[this.m_strings_index].ToString() + "\n");
                                AfiseazaArbore(auxArbore.Radacina);
                                list = parse_string_for_variables(String.Concat(my_list[1].Where(c => !Char.IsWhiteSpace(c))).ToString());
                                for (int j = 0; j < list.Count(); j++)
                                {
                                    for (int z = 0; z < this.m_dataname.Count; z++)
                                    {
                                        if (this.m_dataname[z] == list[j])
                                        {
                                            my_parser.swap_values(list[j], this.m_datavalue[z]);
                                        }
                                    }
                                }
                                var e = new Evaluator(auxArbore.Radacina);

                                Console.WriteLine("ARBORELE SINTACTIC CU VALORILE INLOCUITE: " + this.m_strings[this.m_strings_index].ToString() + "\n");
                                AfiseazaArbore(auxArbore.Radacina);

                                int my_counter = 0;
                                int parse_result = 0;
                                for (int y = 0; y < list.Count(); y++)
                                {
                                    for (int u = 0; u < this.m_dataname.Count; u++)
                                    {
                                        if (this.m_dataname[u] == list[y])
                                        {
                                            if (Int32.TryParse(this.m_datavalue[u], out parse_result) == false)
                                            {
                                                my_counter++;
                                            }
                                        }
                                    }
                                }
                                if (my_counter == list.Count())
                                {
                                    int my_personal_checker = 0;
                                    List<string> uninit = new List<string>();
                                    string rezultat = "";
                                    for (int y = 0; y < list.Count; y++)
                                    {
                                        if (check_if_exist(list[y].ToString()) == true && clear_to_engage(list[y].ToString()) == true)
                                        {
                                            my_personal_checker++;
                                        }
                                        else
                                        {
                                            uninit.Add(list[y].ToString());
                                        }
                                    }
                                    if (my_personal_checker == list.Count)
                                    {
                                        rezultat = e.Evalueaza4Strings();
                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        for (int o = 0; o < uninit.Count; o++)
                                        {
                                            Console.WriteLine("Unitialised or inexistent value: " + uninit[o].ToString() + " line nr -> " + (i + 1).ToString() + '\n');
                                            return;
                                        }
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }
                                    this.m_datatype.Add(init_name[0]);
                                    this.m_dataname.Add(init_name[1]);
                                    this.m_datavalue.Add(rezultat.ToString());
                                    Console.Out.WriteLine("Rezultat : " + init_name[0] + "( " + init_name[1] + " )" + " = " + rezultat.ToString());
                                }
                                else
                                {
                                    int my_personal_checker = 0;
                                    List<string> uninit = new List<string>();
                                    double rezultat = 0;
                                    for (int y = 0; y < list.Count; y++)
                                    {
                                        if (check_if_exist(list[y].ToString()) == true && clear_to_engage(list[y].ToString()) == true)
                                        {
                                            my_personal_checker++;
                                        }
                                        else
                                        {
                                            uninit.Add(list[y].ToString());
                                        }
                                    }
                                    if (my_personal_checker == list.Count)
                                    {
                                        rezultat = e.Evalueaza4Values();
                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        for (int o = 0; o < uninit.Count; o++)
                                        {
                                            Console.WriteLine("Unitialised or inexistent value: " + uninit[o].ToString() + " line nr -> " + (i + 1).ToString() + '\n');
                                            return;
                                        }
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }
                                    this.m_datatype.Add(init_name[0]);
                                    this.m_dataname.Add(init_name[1]);
                                    if (init_name[0] == "int")
                                    {
                                        this.m_datavalue.Add(((int)rezultat).ToString());
                                        Console.Out.WriteLine("Rezultat : " + init_name[0] + "( " + init_name[1] + " )" + " = " + ((int)rezultat).ToString());
                                    }
                                    else
                                    {
                                        this.m_datavalue.Add(rezultat.ToString());
                                        Console.Out.WriteLine("Rezultat : " + init_name[0] + "( " + init_name[1] + " )" + " = " + rezultat.ToString());
                                    }
                                }

                            }
                        }
                        else
                        {
                            list = parse_string_for_variables(this.m_strings[this.m_strings_index].ToString());
                            if (list[0] == "int" || list[0] == "float" || list[0] == "string" || list[0] == "double" || list[0] == "unsigned" || list[0] == "char")
                            {
                                bool is_val_existent = false;

                                for (int z = 0; z < this.m_dataname.Count; z++)
                                {
                                    if (list[1].ToString() == this.m_dataname[z].ToString())
                                    {
                                        is_val_existent = true;
                                    }
                                }

                                if (is_val_existent == false)
                                {
                                    this.m_datatype.Add(list[0]);
                                    this.m_dataname.Add(list[1]);
                                    this.m_datavalue.Add(list[2]);
                                    this.m_indexInPage.Add(i + 1);
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Value " + list[1].ToString() + " already existent -- line: " + (i + 1).ToString() + "\n");
                                    Console.ForegroundColor = ConsoleColor.White;

                                    return;
                                }

                            }
                            else
                            {
                                Console.WriteLine("Wrong op2\n");
                            }
                        }
                    }
                    //else if ()
                   // {
                   
                    //}
                }
            ;

                Array.Clear(this.m_strings, 0, this.m_strings.Length);
            }
        }
        public bool clear_to_engage(string value)
        {
            for (int i = 0; i < this.m_dataname.Count; i++)
            {
                if (value == this.m_dataname[i])
                {
                    if (this.m_datavalue[i] == "-")
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool check_if_exist(string value)
        {
            for (int i = 0; i < this.m_dataname.Count; i++)
            {
                if (value == this.m_dataname[i])
                {
                    return true;
                }
            }
            return false;
        }
        public List<string> parse_by_space_sign(string input)
        {
            string[] words = input.Split(' ');
            words = words.Where(val => val != "").ToArray();
            return words.ToList();
        }
        public List<string> parse_by_equal_sign(string input)
        {
            string[] words = input.Split('=', ';');
            words = words.Where(val => val != "").ToArray();
            return words.ToList();
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
        public bool checker_ceva(string currentString)
        {
            if (currentString.Contains("int"))
            {

                return false;
            }
            else if (currentString.Contains("char"))
            {

                return false;
            }
            else if (currentString.Contains("float"))
            {

                return false;
            }
            else if (currentString.Contains("string"))
            {

                return false;
            }
            else if (currentString.Contains("double"))
            {

                return false;
            }
            else if (currentString.Contains("unsigned"))
            {
                return false;
            }
            return true;
        }
        public bool check_correct(string currentString)
        {
            int x = 0;
            x = currentString.IndexOf(';');
            if (x == -1) { return false; }
            return true;
        }
        public List<string> parse_string_for_variables(string currentString)
        {
            string[] words = currentString.Split(' ', ',', ';', '"', '=', '+', '-', '/', '*');
            words = words.Where(val => val != "").ToArray();
            return words.ToList();
        }

        public bool check_if_unitialised(string smth)
        {
            int x = 0;
            x = smth.IndexOf('=');
            if (x == -1) { return false; }
            return true;
        }

        public static bool ContainsAnyCalculusForInitialisation(string haystack, params string[] needles)
        {
            foreach (string needle in needles)
            {
                if (haystack.Contains(needle))
                    return true;
            }

            return false;
        }
        public void print_result()
        {
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < this.m_dataname.Count; i++)
            {
                Console.WriteLine(this.m_datatype[i] + " -> " + this.m_dataname[i] + " -> " + this.m_datavalue[i] + "\n");
            }
        }
        public int CountCharsUsingIndex(string source, char toFind)
        {
            int count = 0;
            int n = 0;
            while ((n = source.IndexOf(toFind, n) + 1) != 0)
            {
                n++;
                count++;
            }
            return count;
        }
        public bool check_smth_init_smth_value(string input, int index)
        {
            if ((ContainsAnyCalculusForInitialisation(input, "string") == true) && (ContainsAnyCalculusForInitialisation(input, "=") == true) && (ContainsAnyCalculusForInitialisation(input, "+") == false))
            {
                if (ContainsAnyCalculusForInitialisation(input, "\"") == true)
                {
                    if (CountCharsUsingIndex(input, '"') == 2)
                    {
                        return true;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Incorect number of \" characters or incorrect intialisation (string must be initalised with a string \"...\"): " + (index).ToString() + "\n");
                        Console.ForegroundColor = ConsoleColor.White;
                        return false;
                    };
                }
                else return false;
            } else if ((ContainsAnyCalculusForInitialisation(input, "string") == true) && (ContainsAnyCalculusForInitialisation(input, "=") == true) && (ContainsAnyCalculusForInitialisation(input, "+") == true))
            {
                string[] m_variablesAUX1 = input.Split(new Char[] { '+', '-', '/', '*', '=', ' '},
                                 StringSplitOptions.RemoveEmptyEntries);
                string[] m_variablesAUX2 = m_variablesAUX1.Skip(2).ToArray();
                string[] m_variables = m_variablesAUX2.Take(m_variablesAUX2.Length - 1).ToArray();
                int counter = 0;
                for (int i = 0; i < this.m_dataname.Count(); i++)
                {
                    for (int j = 0; j < m_variables.Count(); j++)
                    {
                        if (this.m_dataname[i] == m_variables[j])
                        {
                            if (this.m_datatype[i] == "string")
                            {
                                counter++;
                            }
                        }
                    }
                }
                if (counter == m_variables.Count())
                {
                    return true;
                }
                else {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid operation " + "\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    return false; }
            }
            else if ((ContainsAnyCalculusForInitialisation(input, "int", "double", "float", "long") == true) && (ContainsAnyCalculusForInitialisation(input, "=") == true) && (ContainsAnyCalculusForInitialisation(input, "+", "-", "/", "*") == false))
            {
                if (ContainsAnyCalculusForInitialisation(input, "\"") == true)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Incorrect intialisation (int/double/float/long must not include \"): " + (index).ToString() + "\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    return false;
                }
                else return true;
            } else if ((ContainsAnyCalculusForInitialisation(input, "int", "double", "float", "long") == true) && (ContainsAnyCalculusForInitialisation(input, "=") == true) && (ContainsAnyCalculusForInitialisation(input, "+", "-", "/", "*") == true)) 
            {
                string[] m_variablesAUX1 = input.Split(new Char[] { '+', '-', '/', '*', '=', ' ' },
                 StringSplitOptions.RemoveEmptyEntries);
                string[] m_variablesAUX2 = m_variablesAUX1.Skip(2).ToArray();
                string[] m_variables = m_variablesAUX2.Take(m_variablesAUX2.Length - 1).ToArray();
                int counter = 0;
                for (int i = 0; i < this.m_dataname.Count(); i++)
                {
                    for (int j = 0; j < m_variables.Count(); j++)
                    {
                        if (this.m_dataname[i] == m_variables[j])
                        {
                            if (this.m_datatype[i] == "int" || this.m_datatype[i] == "float" || this.m_datatype[i] == "double" || this.m_datatype[i] == "long")
                            {
                                counter++;
                            }
                        }
                    }
                }
                if (counter == m_variables.Count())
                {
                    return true;
                }
                else {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid operation " + "\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    return false;
                     };
            }
            else return true;
        }
    }
}
