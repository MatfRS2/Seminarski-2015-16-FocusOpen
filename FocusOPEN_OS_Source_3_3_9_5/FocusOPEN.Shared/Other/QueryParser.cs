/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FocusOPEN.Shared
{
	/// <summary>
	/// This class is used to parse a user entered string into a query
	/// for use in a SQL statement with the CONTAINS clause for full-text searching
	/// </summary>
	/// <remarks>
	/// This query parser is based heavily on code originally written by Robert Dominy (http://dev.angusog.com/).
	/// </remarks>
	public class UserQueryParser
	{
		#region Constructor

		static UserQueryParser()
		{
			m_BuiltInTokens["and"] = new Token(TokenTypes.AndOperator, "AND");
			m_BuiltInTokens["or"] = new Token(TokenTypes.OrOperator, "OR");
			m_BuiltInTokens["near"] = new Token(TokenTypes.NearOperator, "NEAR");
			m_BuiltInTokens["not"] = new Token(TokenTypes.NotOperator, "NOT");
			m_BuiltInTokens["("] = new Token(TokenTypes.LeftParenthis, "(");
			m_BuiltInTokens[")"] = new Token(TokenTypes.RightParenthis, ")");
		}

		#endregion

		#region Private variables

		private string m_Error = string.Empty;
		private readonly List<Token> m_Tokens = new List<Token>();
		private static readonly List<String> m_NoiseWords = new List<string>();
		private static readonly Dictionary<String, Token> m_BuiltInTokens = new Dictionary<string, Token>();

		#endregion

		#region Accessors

		public string Error
		{
			get
			{
				return m_Error;
			}
		}

		public static List<string> NoiseWords
		{
			get
			{
				return m_NoiseWords;
			}
		}

		#endregion

		/// <summary>
		/// Gets the tokens in a string ready for use in a SQL query with the CONTAINS clause
		/// </summary>
		public string GetSqlQuery()
		{
			if (m_Tokens.Count == 0)
			{
				m_Error = "No search query";
				return string.Empty;
			}

			StringBuilder sb = new StringBuilder();

			foreach (Token token in m_Tokens)
			{
				// Get the token value
				string tokenValue = token.Value;

				// Make it SQL safe
				tokenValue = tokenValue.Replace("'", "''");

				// Wrap the token value in quotes, if it's not in quotes already (ie. might be a phrase search)
				if (token.TokenType == TokenTypes.UserItem && !tokenValue.StartsWith("\"") && !tokenValue.EndsWith("\""))
					tokenValue = string.Format("\"{0}\"", tokenValue);

				// Append the token value to the list
				sb.Append(tokenValue);
				sb.Append(" ");
			}

			return sb.ToString().Trim();
		}

		/// <summary>
		/// Parses the query and initialises the tokens.
		/// </summary>
		/// <param name="userQuery">The user query</param>
		/// <returns>[True] if query is valid, otherwise [False]</returns>
		public bool ParseTokens(string userQuery)
		{
			// First make sure that we've got an even number of quotes
			if (CountQuotes(userQuery) % 2 != 0)
			{
				m_Error = "Invalid number of quote marks";
				return false;
			}

			// Clean up query
			userQuery = userQuery.ToLower();

			// Query cannot start with a not operator, so remove it if applicable
			if (userQuery.StartsWith("-"))
				userQuery = userQuery.Substring(1);

			// Remove unrequired brackets
			userQuery = userQuery.Replace("(\"", "\"");
			userQuery = userQuery.Replace("\")", "\"");

			// Parse the query into tokens
			const string pattern = @"(/\s*([A-Za-z0-9'(-^"")_\u00C0-\u00FF]+\*)|([A-Za-z0-9'(-^"")_\u00C0-\u00FF]+(\*{0,1}))|(-{0,1}[""][^""]*[""])|([\(\)])\s*/)";
			Regex re = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
			MatchCollection matches = re.Matches(userQuery);

			// Nothing parsed yet so no token
			m_Tokens.Clear();
			Token lastParsedToken = null;

			// Parse the regex matches into a list of words
			// which we'll then turn into a list of tokens.
			IEnumerable<string> words = ParseMatchesIntoWordList(matches);

			// Iterate through all the matches in the query
			foreach (string word in words)
			{
				// Get the token from the word
				Token token = GetToken(word);

				if (lastParsedToken != null && (lastParsedToken.TokenType == TokenTypes.NoiseWord) && (token.TokenType & TokenTypes.BinaryOperator) == token.TokenType)
				{
					// Skip this token since it joins a noise word
				}
				else if (lastParsedToken == null && (token.TokenType & TokenTypes.Operator) == token.TokenType)
				{
					// Skip this as query cannot start with an operator
				}
				else if (token.TokenType == TokenTypes.NoiseWord)
				{
					UnrollExpression(TokenTypes.Operator);
					lastParsedToken = token;
				}
				else
				{
					// Get the last (previous) token
					Token lastToken = GetLastToken();

					if (token.TokenType == TokenTypes.UserItem)
					{
						// Check if there is a previous token and if it is an expression, then add an 'AND' operator
						if (lastToken != null && (lastToken.TokenType & TokenTypes.Expression) == lastToken.TokenType)
							m_Tokens.Add(m_BuiltInTokens["and"]);
					}
					else if (((token.TokenType & TokenTypes.NotOperator) == token.TokenType) && lastToken != null && (lastToken.TokenType & TokenTypes.Expression) == lastToken.TokenType)
					{
						// Same goes for not.  If this token is a 'NOT' operator and the last token is an
						// expression, then add an 'and' token to keep the syntax correct.
						m_Tokens.Add(m_BuiltInTokens["and"]);
					}

					// Add the token to the list
					m_Tokens.Add(token);

					// Update the last parsed token to this one
					lastParsedToken = token;
				}
			}

			return IsValid();
		}

		#region Private Helper Stuff

		/// <summary>
		/// Validates the tokens and checks if they correctly form a query
		/// </summary>
		private bool IsValid()
		{
			if (m_Tokens.Count == 0)
			{
				m_Error = "Search string is empty";
				return false;
			}

			bool valid = true;
			bool lastItemOK = false;
			TokenTypes nextItem = TokenTypes.UserItem | TokenTypes.LeftParenthis | TokenTypes.NotOperator;
			int balance = 0;

			foreach (Token token in m_Tokens)
			{
				if ((token.TokenType & nextItem) != 0)
				{
					switch (token.TokenType)
					{
						case (TokenTypes.UserItem):
							nextItem = TokenTypes.BinaryOperator | TokenTypes.RightParenthis;
							lastItemOK = true;
							break;

						case (TokenTypes.AndOperator):
							nextItem = TokenTypes.UserItem | TokenTypes.NotOperator | TokenTypes.LeftParenthis;
							lastItemOK = false;
							break;

						case (TokenTypes.NearOperator):
							nextItem = TokenTypes.UserItem;
							lastItemOK = false;
							break;

						case (TokenTypes.OrOperator):
							nextItem = TokenTypes.UserItem | TokenTypes.LeftParenthis;
							lastItemOK = false;
							break;

						case (TokenTypes.NotOperator):
							nextItem = TokenTypes.UserItem | TokenTypes.LeftParenthis;
							lastItemOK = false;
							break;

						case (TokenTypes.LeftParenthis):
							balance++;
							nextItem = TokenTypes.UserItem;
							lastItemOK = false;
							break;

						case (TokenTypes.RightParenthis):
							balance--;
							nextItem = TokenTypes.OrOperator | TokenTypes.AndOperator;
							lastItemOK = (balance <= 0);
							break;
					}

					if (balance < 0)
					{
						valid = false;
						m_Error = "Mismatched parenthesis";
						break;
					}
				}
				else
				{
					valid = false;
					m_Error = "Unexpected word or character found: " + token.Value;
					break;
				}
			}

			if (balance != 0)
			{
				valid = false;
				m_Error = "Mismatched parenthesis";
			}
			else if (valid && !lastItemOK)
			{
				valid = false;
				m_Error = "Unexpected end of search string after: " + m_Tokens[m_Tokens.Count - 1].Value;
			}

			return valid;
		}

		[Flags]
		private enum TokenTypes
		{
			UserItem = 1,
			AndOperator = 2,
			OrOperator = 4,
			NotOperator = 8,
			LeftParenthis = 16,
			RightParenthis = 32,
			NearOperator = 64,
			NoiseWord = 128,

			Operator = AndOperator | OrOperator | NotOperator | NearOperator,
			BinaryOperator = AndOperator | OrOperator | NearOperator,
			Expression = RightParenthis | UserItem
		}

		/// <summary>
		/// Gets a token from the specified text
		/// </summary>
		private static Token GetToken(string text)
		{
			if (m_BuiltInTokens.ContainsKey(text))
				return m_BuiltInTokens[text];

			Token token = new Token {Value = text, TokenType = m_NoiseWords.Contains(text) ? TokenTypes.NoiseWord : TokenTypes.UserItem};

			return token;
		}

		/// <summary>
		/// Gets the last token in the list.  If there is no last token, null is returned
		/// </summary>
		private Token GetLastToken()
		{
			if (m_Tokens.Count > 0)
				return m_Tokens[m_Tokens.Count - 1];

			return null;
		}

		/// <summary>
		/// Rolls back to the last token of the specified type.
		/// All tokens after it are removed from the list.
		/// </summary>
		private void UnrollExpression(TokenTypes type)
		{
			for (int i = m_Tokens.Count; i > 0; i--)
			{
				Token tok = m_Tokens[i - 1];

				if ((tok.TokenType & type) != 0)
				{
					m_Tokens.Remove(tok);
				}
				else
				{
					break;
				}
			}
		}

		/// <summary>
		/// Counts how many times the quote (") character appears in the specified string
		/// </summary>
		private static int CountQuotes(IEnumerable<char> s)
		{
			return s.Count(c => c == '"');
		}

		/// <summary>
		/// Parses the match collection into a list of words or phrases that need to be tokenized
		/// </summary>
		private static IEnumerable<string> ParseMatchesIntoWordList(MatchCollection matches)
		{
			// This will contain our list of raw words
			List<String> wordList1 = new List<string>();

			// The current word we've got.  We store this to check if it's a valid
			// word before we add it.  This is because the regex doesn't parse
			// all types of phrases correctly, so we need to build up the phrase
			// before it can be added to the list.

			// For example, "bunch of grapes" will be parsed by the regex as: "bunch, of, grapes"
			// The problem with this is that it then breaks the SQL clause generator.
			// Instead, we work around this by converting this back into a single phrase as we
			// build up the word list.

			string currentWord = string.Empty;

			foreach (Match match in matches)
			{
				// Get the match value
				string matchValue = match.Value.Trim();

				if (currentWord == string.Empty)
				{
					// This is a new word, so set our current word
					currentWord = matchValue;

					// If the word starts with a quote or -quote, and doesn't end with one then
					// skip to the next word.  We need to do this until we find the word with
					// the end quote, and add the whole word as single word to our word list.
					// This is because the regex has trouble parsing all permutations of phrases
					// so we're working around some of the problems by using this method.

					if ((currentWord.StartsWith("-\"") || currentWord.StartsWith("\"")) && !currentWord.EndsWith("\""))
						continue;
				}
				else
				{
					// Otherwise, we've got a word already which begins with a quote.
					// First we need to append it to our list.  Then we check if it doesn't
					// end with a quote, and if so, move to the next word.  This is so we can
					// build up the phrase that are properly delimited.

					currentWord += " " + matchValue;

					if (!currentWord.EndsWith("\""))
						continue;
				}

				// Remove spaces
				currentWord = currentWord.Trim();

				// Add word or phrase to the list, if it's not empty and not a dash
				// (Dashes are reserved characters used to decide if we need to prefix a NOT clause)
				if (!String.IsNullOrEmpty(currentWord) && currentWord != "-")
					wordList1.Add(currentWord);

				// Clear the word, so that we can work on the next one in the list.
				currentWord = string.Empty;
			}

			// Raw list of words
			List<String> wordList2 = new List<string>();

			// Add each match to the word list
			foreach (string w in wordList1)
			{
				// Get the word
				string word = w;

				// For words (or phrases) starting with a hypen
				// remove it and insert 'not' before it in the tokenlist
				if (word.StartsWith("-"))
				{
					word = word.Substring(1);
					wordList2.Add("not");
				}

				wordList2.Add(word);
			}

			return wordList2;
		}

		private class Token
		{
			public TokenTypes TokenType;
			public string Value;

			public Token()
			{
			}

			public Token(TokenTypes tokenType, string value)
			{
				TokenType = tokenType;
				Value = value;
			}

			public override string ToString()
			{
				return Value;
			}
		}

		#endregion
	}
}