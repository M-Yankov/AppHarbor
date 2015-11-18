namespace CloudServices.Common.ResponseTemplates
{
    public class GetMessagesTemplate
    {
        public const string HtmlTemplate =
@"
<!DOCTYPE html>
<html>
    <head lang=""en"" xmlns=""http://www.w3.org/1999/xhtml"">
        <title>Messages</title>
    </head>

    <body>
        <div>Your Messages: </div>
        <div>
            <ul>{0}</ul>
        </div>
        <p>Refresh page for a new message. <i>It's a little buggy.</i></p>
        <a href=""/AppHarborService.svc/messages/send"">Send Message</a>
    </body>
</html>";

        public const string HtmlSendTemplate = @"
<!DOCTYPE html>

<html lang=""en"" xmlns=""http://www.w3.org/1999/xhtml"">
<head>
    <meta charset = ""utf-8"" />
    <title> Send Message</title>
    <script src=""/Scripts/jquery-2.1.4.js""></script>

</head>
<body>
    <div> Send Message:</div>
    <textarea id=""message"" placeholder=""Type here.."" cols = ""50"" rows=""20"" style=""border: 1px dashed red""></textarea>
    <button id =""send"">Send</button>
     <script>
         $('body').on('click', '#send', function () {
            var message = { Text: $('#message').val() };

             $.ajax({
                 type: 'POST',
                 contentType: 'application/json',
                 url: '/AppHarborService.svc/messages/sendMessage',
                 data: JSON.stringify(message),
                 success: function(res) { alert('Message sent.'); document.location.reload(true);},
                 error: function(res) {conosle.log(res)}
             });
           });
     </script>
</body>
</html>";
    }
}
