using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    public enum TokenType
    {
        EQUALS,
        SEMICOLON,
        WHILE,
        ELLIPSISOPEN,
        ELLIPSISCLOSE,
        BRACKETOPEN,
        BRACKETCLOSE,
        MINUS,
        PLUS,
        IF,
        GREATERTHAN,
        SMALLERTHAN,
        SCHRIJF
    }

    class Tokenizer
    {
        private Dictionary<string, TokenType> map;

        private string[] words;
        private Stack<string> characterStack;
        private int level;

        public Tokenizer(string[] lines)
        {
            this.level = 0;
            this.characterStack = new Stack<string>();

            initMap();

            foreach (string line in lines)
            {
                this.run(line.Split(' ', '\r', '\n', '\t'));
            }

        }

        private void initMap()
        {
            map = new Dictionary<string, TokenType>();

            map["="] = TokenType.EQUALS;
            map[";"] = TokenType.SEMICOLON;
            map["zolang"] = TokenType.WHILE;
            map["als"] = TokenType.IF;
            map[">"] = TokenType.GREATERTHAN;
            map["<"] = TokenType.SMALLERTHAN;
            map["+"] = TokenType.PLUS;
            map["-"] = TokenType.MINUS;
            map["{"] = TokenType.BRACKETOPEN;
            map["}"] = TokenType.BRACKETCLOSE;
            map["("] = TokenType.ELLIPSISOPEN;
            map[")"] = TokenType.ELLIPSISCLOSE;
            map["schrijf"] = TokenType.SCHRIJF;
        }

        private void run(string[] line)
        {
            string compared = "";

            foreach (string word in line)
            {
                if (word == "{" || word == "(")
                {
                    this.characterStack.Push(word);
                    this.level++;
                }

                if (word == "}")
                {
                    if (this.characterStack.Peek() == "{")
                    {
                        this.characterStack.Pop();
                        this.level--;
                    }
                    else
                    {
                        Console.WriteLine("ERROR WITH CHARACTERSTACK");
                    }
                }

                if (word == ")")
                {
                    if (this.characterStack.Peek() == "(")
                    {
                        this.characterStack.Pop();
                        this.level--;
                    }
                    else
                    {
                        Console.WriteLine("ERROR WITH CHARACTERSTACK");
                    }
                }

                compared += word;
                bool match = false;

                foreach (KeyValuePair<string, TokenType> entry in this.map)
                {
                    if (entry.Key.Contains(compared))
                    {
                        match = true;
                    }
                }

                if (!match)
                {
                    //identifier
                    Console.WriteLine("Level: " + this.level + " Identifier: " + compared);
                    compared = "";
                }
                else
                {
                    //token
                    if (this.map.ContainsKey(compared))
                    {
                        Console.WriteLine("Level: " + this.level + " Token: " + compared);
                        compared = "";
                    }
                }
            }

        }
    }
}
