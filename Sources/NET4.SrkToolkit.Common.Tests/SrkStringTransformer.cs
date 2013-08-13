// -----------------------------------------------------------------------
// <copyright file="SrkStringTransformer.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SrkToolkit.Common.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class SrkStringTransformer
    {
        [TestClass]
        public class LinksAsHtmlMethod
        {
            ////static readonly string linkFormat = "<a href=\"{0}\" title=\"{0}\" target=\"{2}\" class=\"{3}\">{1}</a>";
            static readonly string text1 = @"Here are some URLs:
stackoverflow.com/questions/1188129/pregreplace-to-detect-html-php
Here's the answer: http://www.google.com/search?rls=en&q=42&ie=utf-8&oe=utf-8&hl=en. What was the question?
A quick look at http://en.wikipedia.org/wiki/URI_scheme#Generic_syntax is helpful.
There is no place like 127.0.0.1! Except maybe http://news.bbc.co.uk/1/hi/england/surrey/8168892.stm?
Ports: 192.168.0.1:8080, https://example.net:1234/
Beware of Greeks bringing internationalized top-level domains: xn--hxajbheg2az3al.xn--jxalpdlp.
And remember.Nobody is perfect. Don't forget sub.domain.rules.com. Can't you search on google.com?
EOD";
            static string text1formatted;
            private string Text1formatted
            {
                get { return text1formatted ?? (text1formatted = text1.LinksAsHtml()); }
            }

            [TestMethod]
            public void text1_0()
            {
                var html = Text1formatted;

                string url = @"http://stackoverflow.com/questions/1188129/pregreplace-to-detect-html-php";
                Test(html, url);
            }

            [TestMethod]
            public void text1_1()
            {
                var html = Text1formatted;

                string url = @"http://www.google.com/search?rls=en&q=42&ie=utf-8&oe=utf-8&hl=en";
                Test(html, url);
            }

            [TestMethod]
            public void text1_2()
            {
                var html = Text1formatted;

                string url = @"http://en.wikipedia.org/wiki/URI_scheme#Generic_syntax";
                Test(html, url);
            }

            [TestMethod]
            public void text1_3()
            {
                var html = Text1formatted;

                string url = @"http://127.0.0.1";
                Test(html, url);
            }

            [TestMethod]
            public void text1_4()
            {
                var html = Text1formatted;

                string url = @"http://news.bbc.co.uk/1/hi/england/surrey/8168892.stm";
                Test(html, url);
            }

            [TestMethod]
            public void text1_5()
            {
                var html = Text1formatted;

                string url = @"http://192.168.0.1:8080";
                Test(html, url);
            }

            [TestMethod]
            public void text1_6()
            {
                var html = Text1formatted;

                string url = @"https://example.net:1234/";
                Test(html, url);
            }

            [TestMethod]
            public void text1_7()
            {
                var html = Text1formatted;

                string url = @"http://xn--hxajbheg2az3al.xn--jxalpdlp";
                Test(html, url);
            }

            [TestMethod]
            public void text1_8()
            {
                var html = Text1formatted;

                string url = @"http://remember.Nobody";
                Test(html, url);
            }

            [TestMethod]
            public void text1_9()
            {
                var html = Text1formatted;

                string url = @"http://sub.domain.rules.com";
                Test(html, url);
            }

            [TestMethod]
            public void UrlAtBeginning()
            {
                string input = "http://gmail.com shows your emails";
                string result = input.LinksAsHtml();

                string expected = "http://gmail.com";
                Test(result, expected);
            }

            [TestMethod]
            public void emailAddress()
            {
                string input = "contact me at antoine.sottiau@gmail.com for more info";
                string result = input.LinksAsHtml();

                string expected = "mailto:antoine.sottiau@gmail.com";
                Test(result, expected);
                Assert.IsTrue(result.Contains(">antoine.sottiau@gmail.com<"));
            }

            private void Test(string content, string url)
            {
                string expected1 = "\"" + url.ProperHtmlAttributeEscape() + "\"";
                bool valid = content.Contains(expected1);
                Assert.IsTrue(valid, "should contain '" + expected1 + "'");
            }


            /// <summary>
            /// Bug where the same link appear multiple time in a string.
            /// At the end there is only one &lt;a&gt; with a href attribute containing all urls.
            /// </summary>
            [TestMethod]
            public void MethodName()
            {
                // prepare
                string input = @"http://test.local/File/b214cb9e-8f67-43c9-9ee1-84f7a1e19b20

http://test.local/File/b214cb9e-8f67-43c9-9ee1-84f7a1e19b20
http://test.local/File/b214cb9e-8f67-43c9-9ee1-84f7a1e19b20

http://test.local/File/b214cb9e-8f67-43c9-9ee1-84f7a1e19b20http://test.local/File/b214cb9e-8f67-43c9-9ee1-84f7a1e19b20http://test.local/File/b214cb9e-8f67-43c9-9ee1-84f7a1e19b20http://test.local/File/b214cb9e-8f67-43c9-9ee1-84f7a1e19b20http://test.local/File/b214cb9e-8f67-43c9-9ee1-84f7a1e19b20";

                // execute
                string result = input.LinksAsHtml();

                // verify
                Assert.IsTrue(result.Count(c => c == '<') > 2);
            }
        
        }

        [TestClass]
        public class AddHtmlLineBreaksMethod
        {
            [TestMethod]
            public void WorksWithWindows()
            {
                string text = "aaa\r\nbbb\r\nccc";
                string expected = "aaa<br />\r\nbbb<br />\r\nccc";
                string actual = text.AddHtmlLineBreaks();
                Assert.AreEqual(expected, actual);
            }

            [TestMethod]
            public void WorksWithLinux()
            {
                string text = "aaa\nbbb\nccc";
                string expected = "aaa<br />\nbbb<br />\nccc";
                string actual = text.AddHtmlLineBreaks();
                Assert.AreEqual(expected, actual);
            }

            [TestMethod]
            public void WorksWithStupidProducts()
            {
                string text = "aaa\rbbb\rccc";
                string expected = "aaa<br />\rbbb<br />\rccc";
                string actual = text.AddHtmlLineBreaks();
                Assert.AreEqual(expected, actual);
            }
        }

        [TestClass]
        public class HtmlParagraphizifyMethod
        {
            readonly static string subject = @"p1
p1

p2
p2


p3
p3
";

            [TestMethod]
            public void Single()
            {
                string subject = "bla";
                string expected = @"<p>bla</p>";
                string result = subject.HtmlParagraphizify();
                Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void Single1()
            {
                string subject = "bla\n";
                string expected = "<p>bla</p>";
                string result = subject.HtmlParagraphizify();
                Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void Many()
            {
                string expected = @"<p>p1
p1</p>
<p>p2
p2</p>
<p>p3
p3</p>";
                string result = subject.HtmlParagraphizify();
                Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void CombinedWithLineBreaks()
            {
                string expected = @"<p>p1<br />
p1</p>
<p>p2<br />
p2</p>
<p>p3<br />
p3</p>";
                string result = subject.HtmlParagraphizify(makeLineBreaks: true);
                Assert.AreEqual(expected, result);
            }
        }

        [TestClass]
        public class TwitterLinksAsHtmlMethod
        {
            [TestMethod]
            public void SimpleHash()
            {
                var text = "c'est cool de regarder #tf1 à 20h";
                var result = text.TwitterLinksAsHtml();
                Test(result, "https://twitter.com/search/realtime?q=%23tf1&src=hash");
                Assert.IsTrue(result.Contains("regarder <a"));
                Assert.IsTrue(result.Contains(">#tf1</a> à"));
            }

            [TestMethod]
            public void SimpleUser()
            {
                var text = "suivez tous @kalexandre !";
                var result = text.TwitterLinksAsHtml();
                Test(result, "https://twitter.com/kalexandre");
                Assert.IsTrue(result.Contains("tous <a"));
                Assert.IsTrue(result.Contains(">@kalexandre</a> !"));
            }

            [TestMethod]
            public void HashAtBeginning()
            {
                var text = "#tf1 à 20h";
                var result = text.TwitterLinksAsHtml();
                Test(result, "https://twitter.com/search/realtime?q=%23tf1&src=hash");
                Assert.IsTrue(result.Contains("<a"));
                Assert.IsTrue(result.Contains(">#tf1</a> à"));
            }

            [TestMethod]
            public void UserAtBeginning()
            {
                var text = "@kalexandre !";
                var result = text.TwitterLinksAsHtml();
                Test(result, "https://twitter.com/kalexandre");
                Assert.IsTrue(result.Contains("<a"));
                Assert.IsTrue(result.Contains(">@kalexandre</a> !"));
            }

            private void Test(string content, string url)
            {
                string expected1 = "\"" + url.ProperHtmlEscape() + "\"";
                bool valid = content.Contains(expected1);
                Assert.IsTrue(valid, "should contain '" + expected1 + "'");
            }
        }

        [TestClass]
        public class ToUpperFirstLettersMethod
        {
            [TestMethod]
            public void OneWord()
            {
                string input = "hello";
                string expected = "Hello";

                string result = input.ToUpperFirstLetters();

                Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void ManyWords()
            {
                string input = "hello world";
                string expected = "Hello World";

                string result = input.ToUpperFirstLetters();

                Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void BadPunctuation()
            {
                string input = ".hello.world";
                string expected = ".Hello.World";

                string result = input.ToUpperFirstLetters();

                Assert.AreEqual(expected, result);
            }
        }

        [TestClass]
        public class TrimTextRightMethod
        {
            [TestMethod]
            public void ShorterText()
            {
                string input = "hello world";
                string expected = input;

                string result = input.TrimTextRight(12);

                Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void ExactLengthText()
            {
                string input = "hello world";
                string expected = input;

                string result = input.TrimTextRight(11);

                Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void LongerText()
            {//                    123456789012 456789
                string input =    "hello greater world";
                string expected = "hello gre...";

                string result = input.TrimTextRight(12);

                Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void LongerTextOnSpace()
            {//                    123456789012345 789
                string input =    "hello great world";
                string expected = "hello great...";

                string result = input.TrimTextRight(15);

                Assert.AreEqual(expected, result);
            }
        }

        [TestClass]
        public class RemoveDiacriticsMethod
        {
            [TestMethod]
            public void French()
            {
                string input =    "Là bas se trouvent une çédille et un œuf. L'été fût dur. Par la fenêtre. En grève ex æquo.";
                string expected = "La bas se trouvent une cedille et un oeuf. L'ete fut dur. Par la fenetre. En greve ex aequo.";

                string result = input.RemoveDiacritics();

                Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void German()
            {
                string input =    "German uses the umlauts ä, ö and ü. The eszett (ß) represents the unvoiced s sound.";
                string expected = "German uses the umlauts a, o and u. The eszett (ss) represents the unvoiced s sound.";

                string result = input.RemoveDiacritics();

                Assert.AreEqual(expected, result);
            }
        }

        [TestClass]
        public class RemoveSpacesMethod
        {
            [TestMethod]
            public void SomeSpaces()
            {
                string input = " \t\n\r\u00A0\u2002\u2003\u2004\u2005\u205F";
                string expected = "";

                string result = input.RemoveSpaces();

                Assert.AreEqual(expected, result);
            }
        }

        [TestClass]
        public class MakeUrlFriendlyMethod
        {
            [TestMethod]
            public void Test()
            {
                string input = "German uses  the   umlauts    ä,     ö and ü. ";
                string expected = "german-uses-the-umlauts-a-o-and-u";

                string result = input.MakeUrlFriendly(false);

                Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void PreserveCase()
            {
                string input = "German uses  the   umlauts    ä,     ö AND ü. ";
                string expected = "German-uses-the-umlauts-a-o-AND-u";

                string result = input.MakeUrlFriendly(true);

                Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void NoPreserveCase()
            {
                string input = "Hello guys,   this is SandRock from http://sparklenetworks.com.    ";
                string expected = "hello-guys-this-is-sandrock-from-http-sparklenetworks-com";

                string result = input.MakeUrlFriendly(false);

                Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void PreserveCase1()
            {
                string input = "Hello guys,   this is SandRock from http://sparklenetworks.com.    ";
                string expected = "Hello-guys-this-is-SandRock-from-http-sparklenetworks-com";

                string result = input.MakeUrlFriendly(true);

                Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void PreserveCaseAndChars()
            {
                string input = "Hello guys,   this is SandRock from http://sparklenetworks.com.    ";
                string expected = "Hello-guys,-this-is-SandRock-from-http-sparklenetworks.com.";

                string result = input.MakeUrlFriendly(true, preserveChars: new char[]{ '.', ',', });

                Assert.AreEqual(expected, result);
            }
        }

        [TestClass]
        public class GetIncrementedStringMethod
        {
            [TestMethod]
            public void Nothing()
            {
                string input = "test";
                string expected = "test-1";

                string result = input.GetIncrementedString();

                Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void After1Goes2()
            {
                string input = "test-1";
                string expected = "test-2";

                string result = input.GetIncrementedString();

                Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void After2Goes3()
            {
                string input = "test-2";
                string expected = "test-3";

                string result = input.GetIncrementedString();

                Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void After9Goes10()
            {
                string input = "test-9";
                string expected = "test-10";

                string result = input.GetIncrementedString();

                Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void After10Goes11()
            {
                string input = "test-10";
                string expected = "test-11";

                string result = input.GetIncrementedString();

                Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void AfterNothingGoes7()
            {
                string input = "test";
                string expected = "test-7";

                string result = input.GetIncrementedString(startIndex: 7);

                Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void After1Goes7()
            {
                string input = "test-1";
                string expected = "test-7";

                string result = input.GetIncrementedString(startIndex: 7);

                Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void After1Goes3()
            {
                string input = "test-1";
                string expected = "test-3";

                string result = input.GetIncrementedString(startIndex: 3);

                Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void After1GoesGreaterThan3()
            {
                string input = "test-1";
                string expected = "test-4";

                string result = input.GetIncrementedString(uniquenessCheck: s =>
                {
                    switch (s)
                    {
                        case "test":
                        case "test-1":
                        case "test-2":
                        case "test-3":
                            return false;

                        default:
                            return true;
                    }
                });

                Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void AfterNothingGoesGreaterThan3()
            {
                string input = "test";
                string expected = "test-4";

                string result = input.GetIncrementedString(uniquenessCheck: s =>
                {
                    switch (s)
                    {
                        case "test":
                        case "test-1":
                        case "test-2":
                        case "test-3":
                            return false;

                        default:
                            return true;
                    }
                });

                Assert.AreEqual(expected, result);
            }
        }

        [TestClass]
        public class ProperHtmlEscapeMethod
        {
            [TestMethod]
            public void Test()
            {
                string input = "helo <b class=\"test\" style='o'>w & or &amp; ld</b>";
                string expected = "helo &lt;b class=\"test\" style='o'&gt;w &amp; or &amp;amp; ld&lt;/b&gt;";

                string result = input.ProperHtmlEscape();

                Assert.AreEqual(expected, result);
            }
        }

        [TestClass]
        public class ProperHtmlAttributeEscapeMethod
        {
            [TestMethod]
            public void Test()
            {
                string input = "helo <b class=\"test\" style='o'>w & or &amp; ld</b>";
                string expected = "helo &lt;b class=&quot;test&quot; style=&apos;o&apos;&gt;w &amp; or &amp;amp; ld&lt;/b&gt;";

                string result = input.ProperHtmlAttributeEscape();

                Assert.AreEqual(expected, result);
            }
        }
    }
}
