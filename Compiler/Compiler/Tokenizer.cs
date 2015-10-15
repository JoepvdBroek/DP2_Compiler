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
        SEMICOLUM,
        WHILE,
        ELLIPSISOPEN,
        ELLIPSISCLOSE,
        BRACKETOPEN,
        BRACKETCLOSE,
        MINUS,
        PLUS,
        IF,
        NUMBER,
        IDENTIFIER,
        ELSE,
        GREATERTHAN,
        SMALLERTHAN,
        SCHRIJF,
        ANY,
        STRINGOPENCLOSE,
        DOUBLE_EQUALS
    }

    public class Tokenizer
    {
        private Dictionary<string, TokenType> map;
        
        private Stack<TokenType> ifStack;
        private Stack<TokenType> bracketStack;
        private Stack<TokenType> ellipseStack;
        private int level;
        private int lineNumber;
        public LinkedList<Token> tokenList { get; }
        private List<Token> stringList;

        public Tokenizer(string[] lines)
        {
            this.level = 0;
            this.lineNumber = 0;
            this.ifStack = new Stack<TokenType>();
            this.bracketStack = new Stack<TokenType>();
            this.ellipseStack = new Stack<TokenType>();
            this.stringList = new List<Token>();
            this.tokenList = new LinkedList<Token>();

            this.initMap();

            foreach (string line in lines)
            {
                string[] words = line.Split(' ', '\r', '\n', '\t');

                if(words.Length != 0 && !(words.Length == 1 && words[0] == ""))
                {
                    this.run(words);
                }
                
                this.lineNumber++;
            }

            var currentToken = tokenList.First;

            while(currentToken != null)
            {
                if (currentToken.Value.TokenType == TokenType.IDENTIFIER && currentToken.Value.Text == "")
                {
                    currentToken.Value.TokenType = TokenType.ANY;
                    
                }
                currentToken = currentToken.Next;
            }
        }

        private void initMap()
        {
            this.map = new Dictionary<string, TokenType>();

            this.map["="] = TokenType.EQUALS;
            this.map["ander"] = TokenType.ELSE;
            this.map["zolang"] = TokenType.WHILE;
            this.map["als"] = TokenType.IF;
            this.map[";"] = TokenType.SEMICOLUM;
            this.map[">"] = TokenType.GREATERTHAN;
            this.map["<"] = TokenType.SMALLERTHAN;
            this.map["+"] = TokenType.PLUS;
            this.map["-"] = TokenType.MINUS;
            this.map["{"] = TokenType.BRACKETOPEN;
            this.map["}"] = TokenType.BRACKETCLOSE;
            this.map["("] = TokenType.ELLIPSISOPEN;
            this.map[")"] = TokenType.ELLIPSISCLOSE;
            this.map["\""] = TokenType.STRINGOPENCLOSE;
            this.map["schrijf"] = TokenType.SCHRIJF;
            this.map["=="] = TokenType.DOUBLE_EQUALS;
        }

        private void run(string[] line)
        {
            foreach(string word in line)
            {
                Token token = new Token();
                token.Position = tokenList.Count + 1;
                token.Text = word;
                token.Level = this.level;
                token.LineNumber = this.lineNumber;

                if (!this.map.ContainsKey(word))
                {
                    int wordNumeric;
                    bool result = int.TryParse(word, out wordNumeric);
                    if (result)
                    {
                        token.TokenType = TokenType.NUMBER;
                    }
                    else
                    {
                        token.TokenType = TokenType.IDENTIFIER;
                    }
                }
                else
                {
                    this.handleStack(this.map[word]);
                    
                    token.TokenType = this.map[word];
                    
                    this.handlePartner(token);
                }
                this.tokenList.AddLast(token);
            }
        }

        private void handlePartner(Token token)
        {
            switch (token.TokenType) {
                case TokenType.STRINGOPENCLOSE:
                    if(this.stringList.Count > 0)
                    {
                        token.Partner = this.stringList[0];
                        this.tokenList.Find(this.stringList[0]).Value.Partner = token;
                        this.stringList.RemoveAt(0);
                    }
                    else
                    {
                        this.stringList.Add(token);
                    }
                    break;
            }
        }

        private void handleStack(TokenType tokenType)
        {
            switch (tokenType)
            {
                case TokenType.BRACKETOPEN:
                    this.bracketStack.Push(tokenType);
                    this.level++;
                    break;
                case TokenType.BRACKETCLOSE:
                    if(this.bracketStack.Peek() == TokenType.BRACKETOPEN)
                    {
                        this.bracketStack.Pop();
                        this.level--;
                    }
                    break;
                case TokenType.ELLIPSISOPEN:
                    this.ellipseStack.Push(tokenType);
                    this.level++;
                    break;
                case TokenType.ELLIPSISCLOSE:
                    if (this.ellipseStack.Peek() == TokenType.ELLIPSISOPEN)
                    {
                        this.ellipseStack.Pop();
                        this.level--;
                    }
                    break;
                case TokenType.IF:
                    this.ifStack.Push(tokenType);
                    break;
                case TokenType.ELSE:
                    if (this.ifStack.Peek() == TokenType.ELSE)
                    {
                        this.ifStack.Pop();
                    }
                    break;
            }
        }
    }
}
