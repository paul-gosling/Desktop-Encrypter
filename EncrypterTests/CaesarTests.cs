using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Encrypter;
using Encrypter.Ciphers;


namespace EncrypterTests
{
    [TestFixture]
    public class CaesarTests
    {
        [Test]
        public void Encrypt_WithEmptyAlphabet()
        {
            var caesar = new Caesar("");
            var encrypted = caesar.Encrypt("hello", 'h');
            Assert.That(encrypted, Is.EqualTo("<== The alphabet is empty ==>"));
        }

        [Test]
        public void Encrypt_RepeatingLettersInAlphabet()
        {
            var caesar = new Caesar("aabcdefg");
            var encrypted = caesar.Encrypt("def", 'f');
            Assert.That(encrypted, Is.EqualTo("<== The alphabet has repeating letters ==>"));
        }

        [Test]
        public void Encrypt_WithWrongKey()
        {
            var caesar = new Caesar("abcdefg");
            var encrypted = caesar.Encrypt("def", '!');
            Assert.That(encrypted, Is.EqualTo("<== The alphabet does not contain the key ==>"));
        }

        [Test]
        public void Encypt_WithSymbolNotFromAlphabet()
        {
            var caesar = new Caesar("abcdefghijklmnopqrstuvwxyz");
            var encrypted = caesar.Encrypt("hello, how are you?", 'b');
            Assert.That(encrypted, Is.EqualTo("ifmmp, ipx bsf zpv?"));
        }

        [Test]
        public void Encrypt_BigOffset_WrapsAround()
        {
            var caesar = new Caesar("abcdefghijklmnopqrstuvwxyz");
            var encrypted = caesar.Encrypt("hello", 'z');
            Assert.That(encrypted, Is.EqualTo("gdkkn"));
        }

        public void Encrypt_NoOffset_TheSameText()
        {
            var caesar = new Caesar("abcdefghijklmnopqrstuvwxyz");
            var encrypted = caesar.Encrypt("hello", 'a');
            Assert.That(encrypted, Is.EqualTo("hello"));
        }

        [TestCase("hello", "abcdefghijklmnopqrstuvwxyz", 'b', "ifmmp")]
        [TestCase("ןנטגוע", "אבגדהו¸זחטיךכלםמןנסעףפץצקרשתûü‎‏ÿ", 'נ', "אבשעץד")]
        public void EncryptText(string text, string alphabet, char key, string expectedText)
        {
            var caesar = new Caesar(alphabet);
            var encrypted = caesar.Encrypt(text, key);
            Assert.That(encrypted, Is.EqualTo(expectedText));
        }

        [Test]
        public void Decrypt_WithEmptyAlphabet()
        {
            var caesar = new Caesar("");
            var decrypted = caesar.Decrypt("hello", 'h');
            Assert.That(decrypted, Is.EqualTo("<== The alphabet is empty ==>"));
        }

        [Test]
        public void Decrypt_RepeatingLettersInAlphabet()
        {
            var caesar = new Caesar("aabcdefg");
            var decrypted = caesar.Decrypt("def", 'f');
            Assert.That(decrypted, Is.EqualTo("<== The alphabet has repeating letters ==>"));
        }

        [Test]
        public void Decrypt_WithWrongKey()
        {
            var caesar = new Caesar("abcdefg");
            var decrypted = caesar.Decrypt("def", '!');
            Assert.That(decrypted, Is.EqualTo("<== The alphabet does not contain the key ==>"));
        }

        [Test]
        public void Decrypt_WithSymbolNotFromAlphabet()
        {
            var caesar = new Caesar("abcdefghijklmnopqrstuvwxyz");
            var decrypted = caesar.Decrypt("ifmmp, ipx bsf zpv?", 'b');
            Assert.That(decrypted, Is.EqualTo("hello, how are you?"));
        }

        [Test]
        public void Decrypt_BigOffset_WrapsAround()
        {
            var caesar = new Caesar("abcdefghijklmnopqrstuvwxyz");
            var decrypted = caesar.Decrypt("gdkkn", 'z');
            Assert.That(decrypted, Is.EqualTo("hello"));
        }

        public void Decrypt_NoOffset_TheSameText()
        {
            var caesar = new Caesar("abcdefghijklmnopqrstuvwxyz");
            var decrypted = caesar.Decrypt("hello", 'a');
            Assert.That(decrypted, Is.EqualTo("hello"));
        }

        [TestCase("ifmmp", "abcdefghijklmnopqrstuvwxyz", 'b', "hello")]
        [TestCase("אבשעץד", "אבגדהו¸זחטיךכלםמןנסעףפץצקרשתûü‎‏ÿ", 'נ', "ןנטגוע")]
        public void DecryptText(string text, string alphabet, char key, string expectedText)
        {
            var caesar = new Caesar(alphabet);
            var decrypted = caesar.Decrypt(text, key);
            Assert.That(decrypted, Is.EqualTo(expectedText));
        }
    }
}
