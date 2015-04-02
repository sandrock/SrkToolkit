SrkToolit.Common
================

A lot of stuff is described as unit tests. It makes it obvious to see what the code does.

- [SrkToolkit.Common.StringReplacer](SrkToolkit.Common.StringReplacer.md)
- [SrkToolkit.Common.DataAnnotations](SrkToolkit.Common.DataAnnotations.md)

String extensions
-----------------

### AddHtmlLineBreaks

    [TestMethod]
    public void WorksWithWindows()
    {
        string text = "aaa\r\nbbb\r\nccc";
        string expected = "aaa<br />\r\nbbb<br />\r\nccc";
        string actual = text.AddHtmlLineBreaks();
        Assert.AreEqual(expected, actual);
    }

### ToUpperFirstLetters

    [TestMethod]
    public void ManyWords()
    {
        string input = "hello wORLD";
        string expected = "Hello WORLD";

        string result = input.ToUpperFirstLetters();

        Assert.AreEqual(expected, result);
    }

### CapitalizeWords

    [TestMethod]
    public void ManyWords_LeavesOtherCharsAsIs()
    {
        string input = "heLLo wORLd";
        string expected = "Hello World";

        string result = input.CapitalizeWords();

        Assert.AreEqual(expected, result);
    }

### RemoveDiacritics

    [TestMethod]
    public void French()
    {
        string input =    "Là bas se trouvent une çédille et un œuf. L'été fût dur. Par la fenêtre. En grève ex æquo.";
        string expected = "La bas se trouvent une cedille et un oeuf. L'ete fut dur. Par la fenetre. En greve ex aequo.";

        string result = input.RemoveDiacritics();

        Assert.AreEqual(expected, result);
    }

### RemoveSpaces

    [TestMethod]
    public void SomeSpaces()
    {
        string input = " \t\n\r\u00A0\u2002\u2003\u2004\u2005\u205F";
        string expected = "";

        string result = input.RemoveSpaces();

        Assert.AreEqual(expected, result);
    }

### MakeUrlFriendly

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

### GetIncrementedString


    [TestMethod]
    public void Nothing()
    {
        string input = "test";
        string expected = "test-1";

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

You can pass a lambda as `uniquenessCheck` argument to check against a DB or something else.

### many more...


NameValueCollection extensions
------------------------------

This old collection needs a few methods...

.ToDictionary()
.AsEnumerable()

DateTime extensions
-------------------

TODO

Array extensions
----------------

TODO

ObservableCollection extensions
-------------------------------

.AddRange()
.RemoveAll(x => x.???)

Enum tools
----------

EnumTool.GetDescription //  gets a name from a resource dictionary

    var desc = EnumTools.GetDescription(value, EnumStrings.ResourceManager);
    var desc = EnumTools.GetDescription(value, EnumStrings.ResourceManager, null, "_Desc");





TimeZoneInfo extensions
-----------------------

### ConvertToUtc & ConvertFromUtc

Converting DateTime to UTC is a little tricky with DateTime.Kind to handle. Calling a static method is not very friendly.

As I often convert to/from UTC, those 2 methods make it nicer. 

    // prepare
    var date = new DateTime(1234, 5, 6, 7, 8, 9, 123, DateTimeKind.Utc);
    var tz = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");

    // before
    TimeZoneInfo.ConvertTimeFromUtc(dateTime.AsUnspecified(), tz)
    // after
    var result = tz.ConvertFromUtc(tz, date);

    // before
    TimeZoneInfo.ConvertTimeToUtc(dateTime.AsUnspecified(), tz)
    // after
    result = tz.ConvertToUtc(tz, result);


DisposableOnce
--------------

Sometimes it is nice to use a using(){} block as an intuitive way to finnaly{do-something}.

DisposableOnce allows you to create a disposable object that will call a delegate on dispose.

    [TestMethod]
    public void DelegateIsCalledOnceDispose()
    {
        // prepare
        int disposed = 0;
        var target = new DisposableOnce(() => disposed++);

        // execute
        target.Dispose();

        // verify
        Assert.AreEqual(1, disposed);
    }

CultureInfoHelper.GetCountries()
-----------------

Lists countries from the .NET cultures.



SrkToolkit.IO.RecursiveDelete
-----------------------------

Multi-threaded recursive file delete.


SrkToolkit.Common.Validation.EmailAddress
-----------------------------------------

Decomposes and composes an email address (account + tag + domain).


SrkToolkit.Common.Validation.PhoneNumber
-----------------------------------------

Makes nicer phone numbers

    "00123456", "0012 (0) 3456", "+12 34-56", "0012-34-56" => "+123456"














