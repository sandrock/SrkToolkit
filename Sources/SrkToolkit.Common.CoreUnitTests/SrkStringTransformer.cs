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
    using Xunit;

    public class SrkStringTransformerTests
    {
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

            [Fact]
            public void text1_0()
            {
                var html = Text1formatted;

                string url = @"http://stackoverflow.com/questions/1188129/pregreplace-to-detect-html-php";
                Test(html, url);
            }

            [Fact]
            public void text1_1()
            {
                var html = Text1formatted;

                string url = @"http://www.google.com/search?rls=en&q=42&ie=utf-8&oe=utf-8&hl=en";
                Test(html, url);
            }

            [Fact]
            public void text1_2()
            {
                var html = Text1formatted;

                string url = @"http://en.wikipedia.org/wiki/URI_scheme#Generic_syntax";
                Test(html, url);
            }

            [Fact]
            public void text1_3()
            {
                var html = Text1formatted;

                string url = @"http://127.0.0.1";
                Test(html, url);
            }

            [Fact]
            public void text1_4()
            {
                var html = Text1formatted;

                string url = @"http://news.bbc.co.uk/1/hi/england/surrey/8168892.stm";
                Test(html, url);
            }

            [Fact]
            public void text1_5()
            {
                var html = Text1formatted;

                string url = @"http://192.168.0.1:8080";
                Test(html, url);
            }

            [Fact]
            public void text1_6()
            {
                var html = Text1formatted;

                string url = @"https://example.net:1234/";
                Test(html, url);
            }

            [Fact]
            public void text1_7()
            {
                var html = Text1formatted;

                string url = @"http://xn--hxajbheg2az3al.xn--jxalpdlp";
                Test(html, url);
            }

            [Fact]
            public void text1_8()
            {
                var html = Text1formatted;

                string url = @"http://remember.Nobody";
                Test(html, url);
            }

            [Fact]
            public void text1_9()
            {
                var html = Text1formatted;

                string url = @"http://sub.domain.rules.com";
                Test(html, url);
            }

            [Fact]
            public void UrlAtBeginning()
            {
                string input = "http://gmail.com shows your emails";
                string result = input.LinksAsHtml();

                string expected = "http://gmail.com";
                Test(result, expected);
            }

            [Fact]
            public void emailAddress()
            {
                string input = "contact me at antoine.sottiau@gmail.com for more info";
                string result = input.LinksAsHtml();

                string expected = "mailto:antoine.sottiau@gmail.com";
                Test(result, expected);
                Assert.True(result.Contains(">antoine.sottiau@gmail.com<"));
            }

            private void Test(string content, string url)
            {
                string expected1 = "\"" + url.ProperHtmlAttributeEscape() + "\"";
                bool valid = content.Contains(expected1);
                Assert.True(valid, "should contain '" + expected1 + "'");
            }


            /// <summary>
            /// Bug where the same link appear multiple time in a string.
            /// At the end there is only one &lt;a&gt; with a href attribute containing all urls.
            /// </summary>
            [Fact]
            public void Bug1()
            {
                // prepare
                string input = @"http://test.local/File/b214cb9e-8f67-43c9-9ee1-84f7a1e19b20

http://test.local/File/b214cb9e-8f67-43c9-9ee1-84f7a1e19b20
http://test.local/File/b214cb9e-8f67-43c9-9ee1-84f7a1e19b20

http://test.local/File/b214cb9e-8f67-43c9-9ee1-84f7a1e19b20http://test.local/File/b214cb9e-8f67-43c9-9ee1-84f7a1e19b20http://test.local/File/b214cb9e-8f67-43c9-9ee1-84f7a1e19b20http://test.local/File/b214cb9e-8f67-43c9-9ee1-84f7a1e19b20http://test.local/File/b214cb9e-8f67-43c9-9ee1-84f7a1e19b20";

                // execute
                string result = input.LinksAsHtml();

                // verify
                Assert.True(result.Count(c => c == '<') > 2);
            }

            /// <summary>
            /// URLs containing the symbol &amp; should not be double escaped.
            /// </summary>
            [Fact]
            public void Bug2()
            {
                string input = "http://site.com/Complex/path.php?key1=val1&key2=value2&openlink=1";
                string result = input.LinksAsHtml();

                this.Test(result, input);
            }

            [Fact]
            public void LongUrlSpanTruncate()
            {
                var input = @"hello, http://verylongsub.domain.rules.com/this/is/a/very/long/url/from/hell/and/other/dark/places/and-finaly/a-page.html?with=arguments&and=more&arguments=all&over=the&workd :D";
                var expected = ">http://verylongsub.domain<span class=\"link-trim\">.rules.com/this/is/a/very/long/url/from/hell/and/other/dark/places/and-finaly/a-page.html?with=arguments&amp;and=more&amp;ar</span>guments=all&amp;over=the&amp;workd<";
                var result = input.LinksAsHtml(avoidDoubleEscape: false);

                SrkToolkit.Testing.Assert.Contains(expected, result);
            }

            [Fact]
            public void LongUrlSpanTruncateNoEsc()
            {
                var input = @"hello, http://verylongsub.domain.rules.com/this/is/a/very/long/url/from/hell/and/other/dark/places/and-finaly/a-page.html?with=arguments&and=more&arguments=all&over=the&workd :D";
                var expected = ">http://verylongsub.domain<span class=\"link-trim\">.rules.com/this/is/a/very/long/url/from/hell/and/other/dark/places/and-finaly/a-page.html?with=arguments&and=more&ar</span>guments=all&over=the&workd<";
                var result = input.LinksAsHtml(avoidDoubleEscape: true);

                Assert.True(result.Contains(expected));
            }
        }

        public class AddHtmlLineBreaksMethod
        {
            [Fact]
            public void WorksWithWindows()
            {
                string text = "aaa\r\nbbb\r\nccc";
                string expected = "aaa<br />\r\nbbb<br />\r\nccc";
                string actual = text.AddHtmlLineBreaks();
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void WorksWithLinux()
            {
                string text = "aaa\nbbb\nccc";
                string expected = "aaa<br />\nbbb<br />\nccc";
                string actual = text.AddHtmlLineBreaks();
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void WorksWithStupidProducts()
            {
                string text = "aaa\rbbb\rccc";
                string expected = "aaa<br />\rbbb<br />\rccc";
                string actual = text.AddHtmlLineBreaks();
                Assert.Equal(expected, actual);
            }
        }

        public class HtmlParagraphizifyMethod
        {
            readonly static string subject = @"p1
p1

p2
p2


p3
p3
";

            [Fact]
            public void Single()
            {
                string subject = "bla";
                string expected = @"<p>bla</p>";
                string result = subject.HtmlParagraphizify();
                Assert.Equal(expected, result);
            }

            [Fact]
            public void Single1()
            {
                string subject = "bla\n";
                string expected = "<p>bla</p>";
                string result = subject.HtmlParagraphizify();
                Assert.Equal(expected, result);
            }

            [Fact]
            public void Many()
            {
                string expected = @"<p>p1
p1</p>
<p>p2
p2</p>
<p>p3
p3</p>";
                string result = subject.HtmlParagraphizify();
                Assert.Equal(expected, result);
            }

            [Fact]
            public void CombinedWithLineBreaks()
            {
                string expected = @"<p>p1<br />
p1</p>
<p>p2<br />
p2</p>
<p>p3<br />
p3</p>";
                string result = subject.HtmlParagraphizify(makeLineBreaks: true);
                Assert.Equal(expected, result);
            }
        }

        public class TwitterLinksAsHtmlMethod
        {
            [Fact]
            public void SimpleHash()
            {
                var text = "c'est cool de regarder #tf1 à 20h";
                var result = text.TwitterLinksAsHtml();
                Test(result, "https://twitter.com/search/realtime?q=%23tf1&src=hash");
                Assert.True(result.Contains("c'est"));
                Assert.True(result.Contains("regarder <a"));
                Assert.True(result.Contains(">#tf1</a> à"));
            }

            [Fact]
            public void SimpleUser()
            {
                var text = "suivez tous @kalexandre !";
                var result = text.TwitterLinksAsHtml();
                Test(result, "https://twitter.com/kalexandre");
                Assert.True(result.Contains("tous <a"));
                Assert.True(result.Contains(">@kalexandre</a> !"));
            }

            [Fact]
            public void HashAtBeginning()
            {
                var text = "#tf1 à 20h";
                var result = text.TwitterLinksAsHtml();
                Test(result, "https://twitter.com/search/realtime?q=%23tf1&src=hash");
                Assert.True(result.Contains("<a"));
                Assert.True(result.Contains(">#tf1</a> à"));
            }

            [Fact]
            public void UserAtBeginning()
            {
                var text = "@kalexandre !";
                var result = text.TwitterLinksAsHtml();
                Test(result, "https://twitter.com/kalexandre");
                Assert.True(result.Contains("<a"));
                Assert.True(result.Contains(">@kalexandre</a> !"));
            }

            private void Test(string content, string url)
            {
                string expected1 = "\"" + url.ProperHtmlEscape() + "\"";
                bool valid = content.Contains(expected1);
                Assert.True(valid, "should contain '" + expected1 + "'");
            }
        }

        public class ToUpperFirstLettersMethod
        {
            [Fact]
            public void OneWord()
            {
                string input = "hello";
                string expected = "Hello";

                string result = input.ToUpperFirstLetters();

                Assert.Equal(expected, result);
            }

            [Fact]
            public void ManyWords()
            {
                string input = "hello world";
                string expected = "Hello World";

                string result = input.ToUpperFirstLetters();

                Assert.Equal(expected, result);
            }

            [Fact]
            public void ManyWords_LeavesOtherCharsAsIs()
            {
                string input = "heLLo wORLd";
                string expected = "HeLLo WORLd";

                string result = input.ToUpperFirstLetters();

                Assert.Equal(expected, result);
            }

            [Fact]
            public void BadPunctuation()
            {
                string input = ".hello.world";
                string expected = ".Hello.World";

                string result = input.ToUpperFirstLetters();

                Assert.Equal(expected, result);
            }
        }

        public class CapitalizeWordsMethod
        {
            [Fact]
            public void OneWord()
            {
                string input = "hello";
                string expected = "Hello";

                string result = input.CapitalizeWords();

                Assert.Equal(expected, result);
            }

            [Fact]
            public void ManyWords()
            {
                string input = "hello world";
                string expected = "Hello World";

                string result = input.CapitalizeWords();

                Assert.Equal(expected, result);
            }

            [Fact]
            public void ManyWords_LeavesOtherCharsAsIs()
            {
                string input = "heLLo wORLd";
                string expected = "Hello World";

                string result = input.CapitalizeWords();

                Assert.Equal(expected, result);
            }

            [Fact]
            public void BadPunctuation()
            {
                string input = ".hello.world";
                string expected = ".Hello.World";

                string result = input.CapitalizeWords();

                Assert.Equal(expected, result);
            }
        }

        public class TrimTextRightMethod
        {
            [Fact]
            public void ShorterText()
            {
                string input = "hello world";
                string expected = input;

                string result = input.TrimTextRight(12);

                Assert.Equal(expected, result);
            }

            [Fact]
            public void ExactLengthText()
            {
                string input = "hello world";
                string expected = input;

                string result = input.TrimTextRight(11);

                Assert.Equal(expected, result);
            }

            [Fact]
            public void LongerText()
            {//                    123456789012 456789
                string input =    "hello greater world";
                string expected = "hello gre...";

                string result = input.TrimTextRight(12);

                Assert.Equal(expected, result);
            }

            [Fact]
            public void LongerTextOnSpace()
            {//                    123456789012345 789
                string input =    "hello great world";
                string expected = "hello great...";

                string result = input.TrimTextRight(15);

                Assert.Equal(expected, result);
            }
        }

        public class TrimTextLeftMethod
        {
            [Fact]
            public void ShorterText()
            {
                string input = "hello world";
                string expected = input;

                string result = input.TrimTextLeft(12);

                Assert.Equal(expected, result);
            }

            [Fact]
            public void ExactLengthText()
            {
                string input = "hello world";
                string expected = input;

                string result = input.TrimTextLeft(11);

                Assert.Equal(expected, result);
            }

            [Fact]
            public void LongerText()
            {//                    123456789012 456789
                string input =    "hello greater world";
                string expected = "...ter world";
                int length = 12;

                string result = input.TrimTextLeft(length);

                Assert.Equal(expected, result);
                Assert.True(result.Length <= length);
            }

            [Fact]
            public void LongerTextOnSpace()
            {//                    123456789012345 789
                string input =    "hello great world";
                string expected = "...great world";
                int length = 15;

                string result = input.TrimTextLeft(length);

                Assert.Equal(expected, result);
                Assert.True(result.Length <= length);
            }
        }

        public class RemoveDiacriticsMethod
        {
            [Fact]
            public void French()
            {
                string input =    "Là bas se trouvent une çédille et un œuf. L'été fût dur. Par la fenêtre. En grève ex æquo.";
                string expected = "La bas se trouvent une cedille et un oeuf. L'ete fut dur. Par la fenetre. En greve ex aequo.";

                string result = input.RemoveDiacritics();

                Assert.Equal(expected, result);
            }

            [Fact]
            public void German()
            {
                string input =    "German uses the umlauts ä, ö and ü. The eszett (ß) represents the unvoiced s sound.";
                string expected = "German uses the umlauts a, o and u. The eszett (ss) represents the unvoiced s sound.";

                string result = input.RemoveDiacritics();

                Assert.Equal(expected, result);
            }
        }

        public class RemoveSpacesMethod
        {
            [Fact]
            public void SomeSpaces()
            {
                string input = " \t\n\r\u00A0\u2002\u2003\u2004\u2005\u205F";
                string expected = "";

                string result = input.RemoveSpaces();

                Assert.Equal(expected, result);
            }
        }

        public class RemoveDuplicateSpacesMethod
        {
            [Fact]
            public void AllSpaces()
            {
                string input = " \t\n\r\u00A0\u2002\u2003\u2004\u2005\u205F";
                string expected = " ";

                string result = input.RemoveDuplicateSpaces();

                Assert.Equal(expected, result);
            }

            [Fact]
            public void SpacesAndWordAndSpacesAndWordAndSpaces()
            {
                string spaces = " \t\n\r\u00A0\u2002\u2003\u2004 \u2005\u205F";
                string input = spaces + "some" + spaces + "spaces" + spaces;
                string expected = " some spaces ";

                string result = input.RemoveDuplicateSpaces();

                Assert.Equal(expected, result);
            }

            [Fact]
            public void SpacesAndWordAndSpacesAndWord()
            {
                string spaces = " \t\n\r\u00A0\u2002\u2003\u2004 \u2005\u205F";
                string input = spaces + "some" + spaces + "spaces";
                string expected = " some spaces";

                string result = input.RemoveDuplicateSpaces();

                Assert.Equal(expected, result);
            }

            [Fact]
            public void WordAndSpacesAndWordAndSpaces()
            {
                string spaces = " \t\n\r\u00A0\u2002\u2003\u2004 \u2005\u205F";
                string input = "some" + spaces + "spaces" + spaces;
                string expected = "some spaces ";

                string result = input.RemoveDuplicateSpaces();

                Assert.Equal(expected, result);
            }
        }

        public class MakeUrlFriendlyMethod
        {
            [Fact]
            public void Test()
            {
                string input = "German uses  the   umlauts    ä,     ö and ü. ";
                string expected = "german-uses-the-umlauts-a-o-and-u";

                string result = input.MakeUrlFriendly(false);

                Assert.Equal(expected, result);
            }

            [Fact]
            public void PreserveCase()
            {
                string input = "German uses  the   umlauts    ä,     ö AND ü. ";
                string expected = "German-uses-the-umlauts-a-o-AND-u";

                string result = input.MakeUrlFriendly(true);

                Assert.Equal(expected, result);
            }

            [Fact]
            public void NoPreserveCase()
            {
                string input = "Hello guys,   this is SandRock from http://sparklenetworks.com.    ";
                string expected = "hello-guys-this-is-sandrock-from-http-sparklenetworks-com";

                string result = input.MakeUrlFriendly(false);

                Assert.Equal(expected, result);
            }

            [Fact]
            public void PreserveCase1()
            {
                string input = "Hello guys,   this is SandRock from http://sparklenetworks.com.    ";
                string expected = "Hello-guys-this-is-SandRock-from-http-sparklenetworks-com";

                string result = input.MakeUrlFriendly(true);

                Assert.Equal(expected, result);
            }

            [Fact]
            public void PreserveCaseAndChars()
            {
                string input = "Hello guys,   this is SandRock from http://sparklenetworks.com.    ";
                string expected = "Hello-guys,-this-is-SandRock-from-http-sparklenetworks.com.";

                string result = input.MakeUrlFriendly(true, preserveChars: new char[]{ '.', ',', });

                Assert.Equal(expected, result);
            }
        }

        public class GetIncrementedStringMethod
        {
            [Fact]
            public void Nothing()
            {
                string input = "test";
                string expected = "test-1";

                string result = input.GetIncrementedString();

                Assert.Equal(expected, result);
            }

            [Fact]
            public void After1Goes2()
            {
                string input = "test-1";
                string expected = "test-2";

                string result = input.GetIncrementedString();

                Assert.Equal(expected, result);
            }

            [Fact]
            public void After2Goes3()
            {
                string input = "test-2";
                string expected = "test-3";

                string result = input.GetIncrementedString();

                Assert.Equal(expected, result);
            }

            [Fact]
            public void After9Goes10()
            {
                string input = "test-9";
                string expected = "test-10";

                string result = input.GetIncrementedString();

                Assert.Equal(expected, result);
            }

            [Fact]
            public void After10Goes11()
            {
                string input = "test-10";
                string expected = "test-11";

                string result = input.GetIncrementedString();

                Assert.Equal(expected, result);
            }

            [Fact]
            public void AfterNothingGoes7()
            {
                string input = "test";
                string expected = "test-7";

                string result = input.GetIncrementedString(startIndex: 7);

                Assert.Equal(expected, result);
            }

            [Fact]
            public void After1Goes7()
            {
                string input = "test-1";
                string expected = "test-7";

                string result = input.GetIncrementedString(startIndex: 7);

                Assert.Equal(expected, result);
            }

            [Fact]
            public void After1Goes3()
            {
                string input = "test-1";
                string expected = "test-3";

                string result = input.GetIncrementedString(startIndex: 3);

                Assert.Equal(expected, result);
            }

            [Fact]
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

                Assert.Equal(expected, result);
            }

            [Fact]
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

                Assert.Equal(expected, result);
            }
        }

        public class ProperHtmlEscapeMethod
        {
            [Fact]
            public void Test()
            {
                string input = "helo <b class=\"test\" style='o'>w & or &amp; ld</b>";
                string expected = "helo &lt;b class=&quot;test&quot; style=&#x27;o&#x27;&gt;w &amp; or &amp;amp; ld&lt;/b&gt;";

                string result = input.ProperHtmlEscape();

                Assert.Equal(expected, result);
            }

            [Fact]
            public void NullArg()
            {
                string input = null;
                string expected = null;

                string result = input.ProperHtmlEscape();

                Assert.Equal(expected, result);
            }

            [Fact]
            public void AllCharsEscaped()
            {
                string input = "&<>\"'/";
                string expected = "&amp;&lt;&gt;&quot;&#x27;/";

                string result = input.ProperHtmlEscape();

                Assert.Equal(expected, result);
            }
        }

        public class ProperHtmlAttributeEscapeMethod
        {
            [Fact]
            public void Test()
            {
                string input = "helo <b class=\"test\" style='o'>w & or &amp; ld</b>";
                string expected = "helo&nbsp;&lt;b&nbsp;class&#x3D;&quot;test&quot;&nbsp;style&#x3D;&#x27;o&#x27;&gt;w&nbsp;&amp;&nbsp;or&nbsp;&amp;amp;&nbsp;ld&lt;/b&gt;";

                string result = input.ProperHtmlAttributeEscape();

                Assert.Equal(expected, result);
            }

            [Fact]
            public void NullArg()
            {
                string input = null;
                string expected = null;

                string result = input.ProperHtmlEscape();

                Assert.Equal(expected, result);
            }
        }

        public class UnescapeUnicodeSequencesMethod
        {
            [Fact]
            public void WorksOn0x3D()
            {
                string input = @"\x3D";
                string expected = "=";
                string result = SrkStringTransformer.UnescapeUnicodeSequences(input);
                Assert.Equal(expected, result);
            }

            [Fact]
            public void WorksOnStringWith0x3D()
            {
                string input = @"aaaa\x3Daaaa";
                string expected = "aaaa=aaaa";
                string result = SrkStringTransformer.UnescapeUnicodeSequences(input);
                Assert.Equal(expected, result);
            }

            [Fact]
            public void WorksOnStringWith0x3DDouble()
            {
                string input = @"aaaa\x3Daaaa\x3Daaaa";
                string expected = "aaaa=aaaa=aaaa";
                string result = SrkStringTransformer.UnescapeUnicodeSequences(input);
                Assert.Equal(expected, result);
            }

            [Fact]
            public void WorksOnStringWithHeart()
            {
                string input = @"aaaa\xE9aaaa";
                string expected = "aaaaéaaaa";
                string result = SrkStringTransformer.UnescapeUnicodeSequences(input);
                Assert.Equal(expected, result);
            }

            [Fact]
            public void AtStart()
            {
                string input = @"\x3Daaaa";
                string expected = "=aaaa";
                string result = SrkStringTransformer.UnescapeUnicodeSequences(input);
                Assert.Equal(expected, result);
            }

            [Fact]
            public void AtEnd()
            {
                string input = @"aaaa\x3D";
                string expected = "aaaa=";
                string result = SrkStringTransformer.UnescapeUnicodeSequences(input);
                Assert.Equal(expected, result);
            }
        }
        /*
        public class UnescapeUTF8SequencesMethod
        {
            [Fact]
            public void WorksOn0x3D()
            {
                string input = @"\x3D";
                string expected = "=";

                string result = SrkStringTransformer.UnescapeUTF8Sequences(input);

                Assert.Equal(expected, result);
            }

            [Fact]
            public void WorksOnStringWith0x3D()
            {
                string input = @"aaaa\x3Daaaa";
                string expected = "aaaa=aaaa";

                string result = SrkStringTransformer.UnescapeUTF8Sequences(input);

                Assert.Equal(expected, result);
            }

            [Fact]
            public void WorksOnStringWith0x3DDouble()
            {
                string input = @"aaaa\x3Daaaa\x3Daaaa";
                string expected = "aaaa=aaaa=aaaa";

                string result = SrkStringTransformer.UnescapeUTF8Sequences(input);

                Assert.Equal(expected, result);
            }

            [Fact]
            public void WorksOnStringWithHeart()
            {
                string input = @"aaaa\xE9aaaa";
                string expected = "aaaaéaaaa";

                string result = SrkStringTransformer.UnescapeUTF8Sequences(input);

                Assert.Equal(expected, result);
            }

            [Fact]
            public void WorksOnStringWith0021()
            {
                string input = @"aaaa\x0021\x21aaaa";
                string expected = "aaaa!!aaaa";

                string result = SrkStringTransformer.UnescapeUTF8Sequences(input);

                Assert.Equal(expected, result);
            }
            
            [Fact]
            public void WorksOnStringWith0100()
            {
                string input = @"aaaa\x0100aaaa";
                string expected = "aaaaĀaaaa";

                string result = SrkStringTransformer.UnescapeUTF8Sequences(input);

                Assert.Equal(expected, result);
            }

            [Fact]
            public void WorksOnStringWithAD9D()
            {
                string input = @"aaaa\xAD9Daaaa";
                string expected = "aaaa궝aaaa";

                string result = SrkStringTransformer.UnescapeUTF8Sequences(input);

                Assert.Equal(expected, result);
            }
        }
        */

        public class SimplifySpacesMethod
        {
            [Fact]
            public void ModeInline_MultiSpaces_FirstCharBecomesOnlyChar()
            {
                string input = "  a   b  \t\r\nc\t\t  d\t \t";
                string expected = "a b \r\nc\td\t";
                string result = SrkStringTransformer.SimplifySpaces(input, true);
                SrkToolkit.Testing.Assert.AreEqual(expected, result);
            }

            [Fact]
            public void ModeFull_MultiSpaces_FirstCharBecomesOnlyChar()
            {
                string input = "   a   b  \t\r\nc\t\t  d\t \t";
                string expected = "a b c\td\t";
                string result = SrkStringTransformer.SimplifySpaces(input, false);
                SrkToolkit.Testing.Assert.AreEqual(expected, result);
            }
        }
    }
}
