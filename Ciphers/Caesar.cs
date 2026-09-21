using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encrypter.Ciphers
{
    public class Caesar
    {
        public string Alphabet { get; }

        private Dictionary<int, char> offsetToLetter;
        private Dictionary<char, int> letterToOffset;

        private bool hasError = false;
        private string errorMessage;

        public Caesar(string alphabet)
        {
            Alphabet = alphabet;

            if (LettersRepeat(alphabet))
            {
                hasError = true;
                errorMessage = "<== The alphabet has repeating letters ==>";
                return;
            }

            if (string.IsNullOrEmpty(alphabet))
            {
                hasError = true;
                errorMessage = "<== The alphabet is empty ==>";
                return;
            }

            SetDictionaries();
        }

        private bool LettersRepeat(string alphabet)
        {
            var seen = new HashSet<char>();
            foreach (var c in alphabet)
            {
                if (!seen.Add(c))
                    return true;
            }

            return false;
        }

        private void SetDictionaries()
        {
            offsetToLetter = new Dictionary<int, char>();
            letterToOffset = new Dictionary<char, int>();

            for (var i = 0; i < Alphabet.Length; i++)
            {
                offsetToLetter.Add(i, Alphabet[i]);
                letterToOffset.Add(Alphabet[i], i);
            }
        }

        public string Encrypt(string inputText, char key)
        {
            var conclusion = HandleNegativeCases(inputText, key);
            if (conclusion != "ok") return conclusion;

            var encrypted = new StringBuilder(inputText.Length);
            var offset = letterToOffset[key];

            foreach (var c in inputText)
            {
                encrypted.Append(EncryptOneLetter(c, offset));
            }

            return encrypted.ToString();
        }

        public string Decrypt(string inputText, char key)
        {
            var conclusion = HandleNegativeCases(inputText, key);
            if (conclusion != "ok") return conclusion;

            var decrypted = new StringBuilder(inputText.Length);
            var offset = letterToOffset[key];

            foreach (var c in inputText)
            {
                decrypted.Append(DecryptOneLetter(c, offset));
            }

            return decrypted.ToString();
        }

        private string HandleNegativeCases(string inputText, char key)
        {
            if (string.IsNullOrEmpty(inputText))
                return "";

            if (hasError)
                return errorMessage;

            if (!letterToOffset.ContainsKey(key))
                return "<== The alphabet does not contain the key ==>";

            return "ok";
        }

        private char EncryptOneLetter(char letter, int offset)
        {
            if (!letterToOffset.ContainsKey(letter))
                return letter;

            var letterIndex = letterToOffset[letter];
            var encryptedIndex = (letterIndex + offset) % Alphabet.Length;

            return offsetToLetter[encryptedIndex];
        }

        private char DecryptOneLetter(char letter, int offset)
        {
            if (!letterToOffset.ContainsKey(letter))
                return letter;

            var letterIndex = letterToOffset[letter];
            var decryptedIndex = (letterIndex - offset + Alphabet.Length) % Alphabet.Length;

            return offsetToLetter[decryptedIndex];
        }
    }
}
