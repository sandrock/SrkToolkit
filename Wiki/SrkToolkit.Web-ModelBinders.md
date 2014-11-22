SrkToolit.Web - Model Binders
=============================

Before you start.

This page is not oriented on "how to use the stuff". It explains how things work internally justifies why things are here, why those things are useful.

There will be another page that explains how to implement this stuff.

If you read what is to follow, you may end-up thinking "WTF is this guy doing?" There is good stuff here but this page does not show it the right way.



DecimalModelBinder
---------------------

Here is some context. In France (and some french-speaking countries) something is wrong. The official decimal separator is the comma but the french keyboard has a dot instead of a comma in the numerical keypad. When using the culture "fr-FR", all numbers are displayed like `1 234,56` but every french guy will type `1234.56`. And BHAM goes the form! Invalid input (the textbox bound to a decimal/double does not validate). 

What can a web developer do? The user chooses his/her culture on the website and goes for "Français (France)" so he/she accepts to use the official french way. The system expects spaces and comma as group and decimal separators but something is wrong: there is a dot. I keep telling my users to choose the "English (United Kingdom)" culture but they complain saying "je ne comprend pas, le site est passé en anglais !" (I don't understand, the website went to english).

This model binder will try to accept inputs not matching exactly the current user culture. French users will be able to type 123.456 without validation error. Typing 123.456,789 is too confusing to be handled though.

	DecimalModelBinder.Register(ModelBinders.Binders);

