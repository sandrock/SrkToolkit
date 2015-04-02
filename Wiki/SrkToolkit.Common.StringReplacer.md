SrkToolkit.Common.StringReplacer
================================

Helps you replace variables in a text.

Variables are declared using a lambda and a model type.

Once the variables declared in an instance of `StringReplacer`, you can replace the variables for all models you pass in.

I'm using this to template markdown files that I send in plain/text emails.


    [TestMethod]
    public void Model()
    {
        var model = new UserModel
        {
            Name = "Johny",
        };
        var target = new StringReplacer<UserModel>();
        target.Setup("User.Name", r => r.Model.Name);
        var text = "Hello {User.Name}";
        var expected = "Hello Johny";
        var result = target.Replace(text, model);
        Assert.AreEqual(expected, result);
    }

Exemple markdown email templating
---------------------------------


    private SendEmailResult SendConfirmEmail(EmailAddress emailAddress, string displayName, ApiKeyModel item, ApiKeyEvent sendConfirmEvent)
    {
        // BasicEmailModel helps me store basic stuff to send an email
        // BasicEmailModel<T> allows a typed sub-model to template more variables
        // BasicEmailModel.Data allows random more variables
        var model = new BasicEmailModel<ApiKeyModel>(item);
        model.Subject = ApiStrings.SendConfirmEmail_Subject + " - " + this.Services.Configuration.AppName;
        model.Recipient = new System.Net.Mail.MailAddress(emailAddress.Value, displayName);
        model.Sender = this.ServiceGroup.Configuration.GetSenderEmailAddress();
        model.Data["ConfirmUrl"] = "http://...../..../....";

        // my markdown files are stored in assembly as embedded resources
        // this code fetches them as text
        var template = EmbeddedResourceUtil.GetResourceAsString("Api.Internals.ConfirmApiKeyEmail.md", typeof(ApiKeysService).Assembly);
        var master = EmbeddedResourceUtil.GetResourceAsString("Api.Internals.EmailTemplate.md", typeof(ApiKeysService).Assembly);

        // create a new replacer with preset variables (see below)
        var email = this.Services.EmailService.PrepareBasicMarkdownEmail<ApiKeyModel>(template, master);

        // set more variables to replace
        email.Replacer
            .Setup("Api.UniqueId", m => m.Model.Model.UniqueId)
            .Setup("Api.Key", m => m.Model.Model.Key)
            .Setup("Api.DateCreated", m => m.Model.Model.DateCreatedUtc.ToString(m.Parameter ?? "d"))
            .Setup("Api.DisplayName", m => m.Model.Model.Contact.DisplayName)
            .Setup("Api.CompanyName", m => m.Model.Model.Contact.CompanyName)
            .Setup("Api.Url", m => this.GetUrl(m.Model.Model).AbsoluteUri)
            .Setup("ConfirmUrl", m => m.Model.Data["ConfirmUrl"].ToString())
            .Setup("ApiSite", m => this.ServiceGroup.Configuration.ApiSiteUrl);

        // todo: store the email variable as static so it is not built each time 

        // replace variables from model in master md
        var masterContent = email.Replacer.Replace(email.MasterTempalte, model);
        // replace variables from model in content md
        var content = email.Replacer.Replace(email.Template, model);
        // replace master placeholder with content
        content = masterContent.Replace("@CONTENT", content);

        // my message is now templated and ready to send
        return this.Services.EmailService.Send(model, content);
    }
    
    internal BasicEmail<TModel> PrepareBasicMarkdownEmail<TModel>(string template, string master)
    {
        var email = new BasicEmail<TModel>();
        email.MasterTempalte = master;
        email.Template = template;
        email.Replacer
            .Setup("Recipient.Address", m => m.Model.Recipient.Address)
            .Setup("Recipient.DisplayName", m => m.Model.Recipient.DisplayName)
            .Setup("Recipient.FullName", m => m.Model.Recipient.ToString())
            .Setup("Sender.Address", m => m.Model.Sender.Address)
            .Setup("Sender.DisplayName", m => m.Model.Sender.DisplayName)
            .Setup("Sender.FullName", m => m.Model.Sender.ToString())
            .Setup("Footer.AppName", m => this.Services.Configuration.AppName)
            .Setup("Footer.ApiSiteUrl", m => this.Services.Configuration.ApiSiteUrl)
            .Setup("Footer.AppCompanyName", m => this.Services.Configuration.AppCompanyName)
            .Setup("Footer.AppCompanyLegalName", m => this.Services.Configuration.AppCompanyLegalName)
            .Setup("Footer.SupportEmail", m => this.Services.Configuration.SupportEmailAddress)
            .Setup("Header.Subject", m => m.Model.Subject)
            .Setup("Header.SendDate", m => DateTime.UtcNow.ToString(m.Parameter ?? "d"));
        return email;
    }

    // Api.Internals.ConfirmApiKeyEmail.md file
    
    API Key (AK)     : {Api.Key}
    Date de création : {Api.DateCreated}
    Au nom de        : {Api.DisplayName}
    Société          : {Api.CompanyName}

































