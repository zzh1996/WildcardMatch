using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WildcardMatch;
using System.Text.RegularExpressions;

namespace UnitTestProject1
{
    [TestClass]
    public class WildcardMatchTests
    {
        [TestMethod]
        public void EmptyStringTests()
        {
            Assert.IsTrue(Program.Match("", ""));
            Assert.IsTrue(Program.Match("*", ""));
            Assert.IsTrue(Program.Match("**", ""));
            Assert.IsTrue(Program.Match("***", ""));
            Assert.IsFalse(Program.Match("?", ""));
            Assert.IsFalse(Program.Match("a", ""));
            Assert.IsFalse(Program.Match("abc", ""));
            Assert.IsFalse(Program.Match("+", ""));
            Assert.IsFalse(Program.Match("++", ""));
            Assert.IsFalse(Program.Match("a*", ""));
            Assert.IsFalse(Program.Match("*a", ""));
            Assert.IsFalse(Program.Match("?*", ""));
            Assert.IsFalse(Program.Match("*?", ""));
            Assert.IsFalse(Program.Match("", "a"));
            Assert.IsFalse(Program.Match("", "ab"));
            Assert.IsFalse(Program.Match("", "*"));
            Assert.IsFalse(Program.Match("", "?"));
        }

        [TestMethod]
        public void BasicTests()
        {
            Assert.IsTrue(Program.Match("a", "a"));
            Assert.IsTrue(Program.Match("abc", "abc"));
            Assert.IsTrue(Program.Match("你好", "你好"));
            Assert.IsFalse(Program.Match("a", "b"));
            Assert.IsFalse(Program.Match("abc", "cba"));
            Assert.IsFalse(Program.Match("abc", "abc "));
            Assert.IsFalse(Program.Match("abc ", "abc"));
            Assert.IsFalse(Program.Match("你好", "你好a"));
        }

        [TestMethod]
        public void QuestionMarkTests()
        {
            Assert.IsTrue(Program.Match("?", "a"));
            Assert.IsTrue(Program.Match("a?c", "abc"));
            Assert.IsTrue(Program.Match("你?", "你好"));
            Assert.IsTrue(Program.Match("a?c?e", "abcde"));
            Assert.IsTrue(Program.Match("???", "abc"));
            Assert.IsTrue(Program.Match("a??d", "abcd"));
            Assert.IsFalse(Program.Match("?", "ab"));
            Assert.IsFalse(Program.Match("??", "a"));
            Assert.IsFalse(Program.Match("a??", "abcd"));
            Assert.IsFalse(Program.Match("??a ", "aaaa"));
            Assert.IsFalse(Program.Match("?a?", "aa"));
        }

        [TestMethod]
        public void StarTests()
        {
            Assert.IsTrue(Program.Match("*", "a"));
            Assert.IsTrue(Program.Match("*", "abc"));
            Assert.IsTrue(Program.Match("**", "a"));
            Assert.IsTrue(Program.Match("***", "a"));
            Assert.IsTrue(Program.Match("***", "abcdefg"));
            Assert.IsTrue(Program.Match("a*b", "ab"));
            Assert.IsTrue(Program.Match("a*b", "acb"));
            Assert.IsTrue(Program.Match("a**b", "ab"));
            Assert.IsTrue(Program.Match("a**b", "acb"));
            Assert.IsTrue(Program.Match("a*b*", "ab"));
            Assert.IsTrue(Program.Match("a*b*", "acb"));
            Assert.IsTrue(Program.Match("a*b*", "abc"));
            Assert.IsTrue(Program.Match("a*b*", "acbc"));
            Assert.IsTrue(Program.Match("ab*", "ab"));
            Assert.IsTrue(Program.Match("ab*", "abc"));
            Assert.IsTrue(Program.Match("*a*b", "ab"));
            Assert.IsTrue(Program.Match("*a*b", "acb"));
            Assert.IsTrue(Program.Match("*a*b", "cab"));
            Assert.IsTrue(Program.Match("*a*b", "cacb"));
            Assert.IsTrue(Program.Match("*ab", "ab"));
            Assert.IsTrue(Program.Match("*ab", "cab"));
            Assert.IsFalse(Program.Match("a*", "b"));
            Assert.IsFalse(Program.Match("*a", "b"));
            Assert.IsFalse(Program.Match("a*b*", "ba"));
            Assert.IsFalse(Program.Match("a**b", "a"));
            Assert.IsFalse(Program.Match("a**b", "b"));
        }

        [TestMethod]
        public void PlusTests()
        {
            Assert.IsTrue(Program.Match("+", "a"));
            Assert.IsTrue(Program.Match("+", "abc"));
            Assert.IsTrue(Program.Match("++", "ab"));
            Assert.IsTrue(Program.Match("+++", "abc"));
            Assert.IsTrue(Program.Match("+++", "abcdefg"));
            Assert.IsTrue(Program.Match("a+b", "acb"));
            Assert.IsTrue(Program.Match("a+b+", "acbc"));
            Assert.IsTrue(Program.Match("ab+", "abc"));
            Assert.IsTrue(Program.Match("+a+b", "cacb"));
            Assert.IsTrue(Program.Match("+ab", "cab"));
            Assert.IsFalse(Program.Match("++", "b"));
            Assert.IsFalse(Program.Match("+++", "bb"));
            Assert.IsFalse(Program.Match("a+b", "ab"));
            Assert.IsFalse(Program.Match("a++b", "acb"));
            Assert.IsFalse(Program.Match("+", ""));
            Assert.IsFalse(Program.Match("a+b+", "acb"));
            Assert.IsFalse(Program.Match("a+b+", "abd"));
        }

        [TestMethod]
        public void EscapeTests()
        {
            Assert.IsTrue(Program.Match("\\\\", "\\"));
            Assert.IsTrue(Program.Match("\\+", "+"));
            Assert.IsTrue(Program.Match("\\*", "*"));
            Assert.IsTrue(Program.Match("\\?", "?"));
            Assert.IsTrue(Program.Match("\\\\\\+\\*\\?", "\\+*?"));
            Assert.IsTrue(Program.Match("?\\?", "a?"));
            Assert.IsTrue(Program.Match("?\\?", "??"));
            Assert.IsTrue(Program.Match("\\++", "+a"));
            Assert.IsTrue(Program.Match("\\**", "*"));
            Assert.IsTrue(Program.Match("*\\**", "a*a"));
            Assert.IsTrue(Program.Match("\\\\\\\\", "\\\\"));
            Assert.IsFalse(Program.Match("\\+", "\\+"));
            Assert.IsFalse(Program.Match("\\*", "\\*"));
            Assert.IsFalse(Program.Match("\\?", "\\?"));
            Assert.IsFalse(Program.Match("\\\\", "\\\\"));
            Assert.IsFalse(Program.Match("\\+", "a"));
            Assert.IsFalse(Program.Match("\\*", "a"));
            Assert.IsFalse(Program.Match("\\?", "a"));
            Assert.IsFalse(Program.Match("?\\?", "aa"));
            Assert.IsFalse(Program.Match("*\\**", "aaa"));
        }

        [TestMethod]
        public void MixedTests()
        {
            Assert.IsTrue(Program.Match("a+*", "aa"));
            Assert.IsTrue(Program.Match("a*+", "aa"));
            Assert.IsTrue(Program.Match("*+a", "aa"));
            Assert.IsTrue(Program.Match("+*a", "aa"));
            Assert.IsTrue(Program.Match("*?*", "aaaaa"));
            Assert.IsTrue(Program.Match("*?*", "a"));
            Assert.IsTrue(Program.Match("?+", "aa"));
            Assert.IsTrue(Program.Match("+?", "aa"));
            Assert.IsTrue(Program.Match("a+b*c??d+e\\\\", "affbcffdfe\\"));
            Assert.IsFalse(Program.Match("a+*", "a"));
            Assert.IsFalse(Program.Match("a*+", "a"));
            Assert.IsFalse(Program.Match("*+a", "a"));
            Assert.IsFalse(Program.Match("+*a", "a"));
            Assert.IsFalse(Program.Match("*?*", ""));
            Assert.IsFalse(Program.Match("?+", "a"));
            Assert.IsFalse(Program.Match("+?", "a"));
            Assert.IsFalse(Program.Match("a+b*c??d+e\\\\", "abcffdfe\\"));
            Assert.IsFalse(Program.Match("a+b*c??d+e\\\\", "affbffdfe\\"));
            Assert.IsFalse(Program.Match("a+b*c??d+e\\\\", "affbcfffdfe\\"));
            Assert.IsFalse(Program.Match("a+b*c??d+e\\\\", "affbcfdfe\\"));
            Assert.IsFalse(Program.Match("a+b*c??d+e\\\\", "affbcffde\\"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ErrorTest1()
        {
            // '\' is the last char of pattern
            Program.Match("\\", "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ErrorTest2()
        {
            Program.Match("abc\\", "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ErrorTest3()
        {
            // '\' followed by an invalid char
            Program.Match("abc\\a", "");
        }

        string RandomString(string available, int length)
        {
            Random random = new Random();
            Char[] chars = new char[length];
            for (int i = 0; i < chars.Length; i++)
            {
                chars[i] = available[random.Next(available.Length)];
            }
            return new String(chars);
        }

        [TestMethod]
        public void RandomTests()
        {
            // test random cases and compare with regex result
            for (int loop = 0; loop < 100000; loop++)
            {
                string pattern = RandomString("abc?*+", 5);
                string value = RandomString("abc", 5);
                Regex rgx = new Regex("^" + pattern.Replace("?", ".").Replace("*", ".*").Replace("+", ".+") + "$");
                Assert.IsTrue(Program.Match(pattern, value) == rgx.IsMatch(value));
            }
        }
    }
}
